﻿using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Editor]
	public class EditorErrorTests<TWebDriverProvider>
		: BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void Initialization()
		{
			_errorsDialog = new ErrorsDialog(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_projectsPage = new ProjectsPage(Driver);

			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			var document = PathProvider.EditorTxtFile;
			var documentName = Path.GetFileNameWithoutExtension(document);
			var glossaryName = GlossariesHelper.UniqueGlossaryName();

			_createProjectHelper.CreateNewProject(
				projectUniqueName,
				filesPaths: new[] { document },
				glossaryName: glossaryName);

			_projectsPage.OpenProjectSettingsPage(projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(document, ThreadUser.NickName, projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(documentName);

			_selectTaskDialog.SelectTask();
		}

		[Test(Description = "Проверяет открытие диалога поиска ошибок с помощью кнопки"), Ignore("PRX-17394")]
		public void FindErrorButtonTest()
		{
			_editorPage.ClickFindErrorButton();

			Assert.IsTrue(_errorsDialog.IsErrorsDialogOpened(),
				"Произошла ошибка:\n не появился диалог поиска ошибок");
		}

		[Test(Description = "Проверяет открытие диалога поиска ошибок нажатием F7"), Ignore("PRX-17394")]
		public void FindErrorHotkeyTest()
		{
			_editorPage.OpenFindErrorsDialogByHotkey();

			Assert.IsTrue(_errorsDialog.IsErrorsDialogOpened(),
				"Произошла ошибка:\n не появился диалог поиска ошибок");
		}

		private ErrorsDialog _errorsDialog;
		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private ProjectSettingsPage _projectSettingsPage;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
		private ProjectsPage _projectsPage;
	}
}
