using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
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
			_workspaceHelper = new WorkspaceHelper(Driver);
			_usersRightsPage = new UsersRightsPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_addTermDialog = new AddTermDialog(Driver);

			_projectName = _createProjectHelper.GetProjectUniqueName();
			_glossary1Name = GlossariesHelper.UniqueGlossaryName();
			_glossary2Name = GlossariesHelper.UniqueGlossaryName();
			_glossary3Name = GlossariesHelper.UniqueGlossaryName();

			_workspaceHelper.GoToUsersRightsPage();

			_usersRightsPage
				.ClickGroupsButton()
				.AddUserToGroupIfNotAlredyAdded("Administrators", ThreadUser.NickName);

			_workspaceHelper
				.GoToGlossariesPage()
				.CreateGlossary(_glossary1Name)
				.GoToGlossariesPage()
				.CreateGlossary(_glossary2Name)
				.GoToProjectsPage();

			_createProjectHelper
				.CreateNewProject(_projectName, glossaryName: _glossary3Name)
				.GoToProjectSettingsPage(_projectName)
				.UploadDocument(new []{PathProvider.DocumentFile})
				.RefreshPage<ProjectSettingsPage, ProjectSettingsHelper>()
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), ThreadUser.NickName)
				.AddGlossaryToDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), _glossary1Name)
				.AddGlossaryToDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), _glossary2Name);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));

			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();
		}

		[Test(Description = "Проверяет выпадающий список с глоссариями, при создании проекта подключалось 2 глоссария")]
		public void CheckGlossaryListInProjectWithTwoGlossaries()
		{
			_editorPage.ClickAddTermButton();

			_addTermDialog.OpenGlossarySelect();

			Assert.IsTrue(_addTermDialog.IsGlossaryExistInDropdown(_glossary1Name),
				"Произошла ошибка:\n глоссарий не найден в выпадающем списке");

			Assert.IsTrue(_addTermDialog.IsGlossaryExistInDropdown(_glossary2Name),
				"Произошла ошибка:\n глоссарий не найден в выпадающем списке");
		}

		[Test(Description = "Проверяет добавление одинаковых терминов в разные глоссарии")]
		public void AddEqualTermsInTwoGlossaries()
		{
			var source = "Space";
			var target = "Космос";

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target, glossaryName: _glossary1Name);

			Assert.IsTrue(_editorPage.IsTermSaved(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не появилось");

			Assert.IsTrue(_editorPage.IsTermSavedMessageDisappeared(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не исчезло");

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target, glossaryName: _glossary2Name);

			Assert.IsTrue(_editorPage.IsTermSaved(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не появилось");

			Assert.IsTrue(_editorPage.IsTermSavedMessageDisappeared(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не исчезло");
		}

		[Test(Description = "Проверяет выпадающий список с глоссариями (проект с 2 глоссариями, второй глоссарий подключается в настройках проекта")]
		public void CheckGlossaryListInProjectCreatedWithOneGlossary()
		{
			_editorPage.ClickAddTermButton();

			_addTermDialog.OpenGlossarySelect();

			Assert.IsTrue(_addTermDialog.IsGlossaryExistInDropdown(_glossary1Name),
				"Произошла ошибка:\n глоссарий не найден в выпадающем списке");

			Assert.IsTrue(_addTermDialog.IsGlossaryExistInDropdown(_glossary2Name),
				"Произошла ошибка:\n глоссарий не найден в выпадающем списке");

			Assert.IsTrue(_addTermDialog.IsGlossaryExistInDropdown(_glossary3Name),
				"Произошла ошибка:\n глоссарий не найден в выпадающем списке");
		}

		private string _projectName;
		private string _glossary1Name;
		private string _glossary2Name;
		private string _glossary3Name;

		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsPage _projectSettingsPage;
		private WorkspaceHelper _workspaceHelper;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
		private UsersRightsPage _usersRightsPage;
		private AddTermDialog _addTermDialog;
	}
}
