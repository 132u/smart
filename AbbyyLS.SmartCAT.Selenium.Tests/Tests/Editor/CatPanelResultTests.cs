﻿using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	public class CatPanelResultsTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupCatPanelResultsTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_projectsPage = new ProjectsPage(Driver);
			_documentSettingsDialog = new DocumentSettingsDialog(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
		}

		[Test]
		[Standalone]
		public void TMTest()
		{
			_createProjectHelper
				.CreateNewProject(
					_projectUniqueName,
					filePath: PathProvider.EditorTxtFile,
					createNewTm: true,
					tmxFilePath: PathProvider.EditorTmxFile)
				.GoToProjectSettingsPage(_projectUniqueName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile), ThreadUser.NickName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));
			
			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();

			Assert.IsTrue(_editorPage.IsCatTableExist(), "Произошла ошибка:\nCAT-панель пустая.");

			Assert.AreNotEqual(_editorPage.CatTypeRowNumber(CatType.TM), 0,
				"Произошла ошибка:\nВ CAT-панели отсутствует подстановка {0}.", CatType.TM);

			Assert.AreEqual(_editorPage.CatTranslationMatchPercent(rowNumber: 1), 94,
				"Произошла ошибка:\nНеверный процент совпадения в CAT-панели.");
		}

		[TestCase(MachineTranslationType.DefaultMT)]
		[TestCase(MachineTranslationType.Google)]
		[TestCase(MachineTranslationType.MicrosoftBing)]
		public void MTTest(MachineTranslationType machineTranslation)
		{
			_createProjectHelper
				.CreateNewProject(
					projectName: _projectUniqueName,
					filePath: PathProvider.DocumentFile,
					createNewTm: true,
					useMachineTranslation: true);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.ClickDocumentSettings(_projectUniqueName, documentNumber: 1)
				.UnselectMachineTranslation(machineTranslation)
				.SelectMachineTranslation(machineTranslation);

			_documentSettingsDialog.CloseDocumentSettings(_projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), ThreadUser.NickName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
			
			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();

			Assert.IsTrue(_editorPage.IsCatTableExist(), "Произошла ошибка:\nCAT-панель пустая.");

			Assert.AreNotEqual(_editorPage.CatTypeRowNumber(CatType.MT), 0,
				"Произошла ошибка:\nВ CAT-панели отсутствует подстановка {0}.", CatType.MT);

			Assert.IsTrue(_editorPage.IsCatTermsMatchSourceTerms(segmentNumber: 1),
				"Произошла ошибка:\nТермины из CAT-панели не соответствуют терминам в сорсе");
		}

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private DocumentSettingsDialog _documentSettingsDialog;
		private ProjectSettingsPage _projectSettingsPage;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
	}
}
