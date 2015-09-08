﻿using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[TestFixture]
	[Standalone]
	class GlossariesEditorTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossariesSetUp()
		{
			_projectName = _createProjectHelper.GetProjectUniqueName();
			_glossary1Name = GlossariesHelper.UniqueGlossaryName();
			_glossary2Name = GlossariesHelper.UniqueGlossaryName();
			_glossary3Name = GlossariesHelper.UniqueGlossaryName();

			WorkspaceHelper
				.GoToUsersRightsPage()
				.ClickGroupsButton()
				.CheckOrAddUserToGroup("Administrators", NickName)
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
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), NickName)
				//TODO: Убрать шаг AddGlossaryToDocument, когда пофиксят PRX-11398
				.AddGlossaryToDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), _glossary1Name)
				.AssertDialogBackgroundDisappeared()
				.ClickDocumentProgress(PathProvider.DocumentFile)
				.AddGlossaryToDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), _glossary2Name)
				.AssertDialogBackgroundDisappeared()
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

		private readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
		private readonly EditorHelper _editorHelper = new EditorHelper();
	}
}