using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class GlossariesEditorTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossariesSetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_editorHelper = new EditorHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);

			_projectName = _createProjectHelper.GetProjectUniqueName();
			_glossary1Name = GlossariesHelper.UniqueGlossaryName();
			_glossary2Name = GlossariesHelper.UniqueGlossaryName();
			_glossary3Name = GlossariesHelper.UniqueGlossaryName();

			_workspaceHelper
				.GoToUsersRightsPage()
				.ClickGroupsButton()
				.CheckOrAddUserToGroup("Administrators", ThreadUser.NickName)
				.GoToGlossariesPage()
				.CreateGlossary(_glossary1Name)
				.GoToGlossariesPage()
				.CreateGlossary(_glossary2Name)
				.GoToProjectsPage();

			_createProjectHelper
				.CreateNewProject(_projectName, _glossary3Name, createGlossary: true)
				.GoToProjectSettingsPage(_projectName)
				.UploadDocument(PathProvider.DocumentFile)
				.RefreshPage<ProjectSettingsPage, ProjectSettingsHelper>()
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), ThreadUser.NickName)
				.AddGlossaryToDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), _glossary1Name)
				.AddGlossaryToDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), _glossary2Name)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile))
				.SelectTask()
				.CloseTutorialIfExist();
		}

		/// <summary>
		/// Проверка выпадающего списка с глоссариями, при создании проекта подключалось два глоссария
		/// </summary>
		[Test]
		public void CheckGlossaryListInProjectWithTwoGlossaries()
		{
			_editorHelper
				.OpenAddTermDialog()
				.AssertGlossaryExistInList(_glossary1Name)
				.ClickCancelAddTerm()
				.OpenAddTermDialog()
				.AssertGlossaryExistInList(_glossary2Name);
		}

		/// <summary>
		/// Добавление одинаковых терминов в разные глоссарии
		/// </summary>
		[Test]
		public void AddEqualTermsInTwoGlossaries()
		{
			var source = "Space";
			var target = "Космос";

			_editorHelper
				.AddNewTerm(source, target, glossaryName: _glossary1Name)
				.AddNewTerm(source, target, glossaryName: _glossary2Name);
		}

		/// <summary>
		/// Проверка выпадающего списка с глоссариями, проект с двумя глоссариями, второй глоссарий подключается в настройках проекта
		/// </summary>
		[Test, Ignore("PRX-11398")]
		public void CheckGlossaryListInProjectCreatedWithOneGlossary()
		{
			_editorHelper
				.OpenAddTermDialog()
				.AssertGlossaryExistInList(_glossary1Name)
				.ClickCancelAddTerm()
				.OpenAddTermDialog()
				.AssertGlossaryExistInList(_glossary2Name)
				.ClickCancelAddTerm()
				.OpenAddTermDialog()
				.AssertGlossaryExistInList(_glossary3Name);
		}

		private string _projectName;
		private string _glossary1Name;
		private string _glossary2Name;
		private string _glossary3Name;

		private CreateProjectHelper _createProjectHelper;
		private EditorHelper _editorHelper;
		private WorkspaceHelper _workspaceHelper;
	}
}
