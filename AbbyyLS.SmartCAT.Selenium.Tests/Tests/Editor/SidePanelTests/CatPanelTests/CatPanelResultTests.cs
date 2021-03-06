﻿using NUnit.Framework;

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
	[Editor]
	public class CatPanelResultsTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupCatPanelResultsTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
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
			var document = PathProvider.EditorTxtFile;
			var tmx = PathProvider.EditorTmxFile;

			_createProjectHelper
				.CreateNewProject(
					_projectUniqueName,
					filesPaths: new[] { document },
					createNewTm: true,
					tmxFilesPaths: new[] { tmx });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(document, ThreadUser.NickName, _projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(document);
			
			_selectTaskDialog.SelectTask();

			Assert.IsTrue(_editorPage.IsCatTableExist(), "Произошла ошибка:\nCAT-панель пустая.");

			Assert.IsTrue(_editorPage.IsCatTypeExist(CatType.TM),
				"Произошла ошибка:\nВ CAT-панели отсутствует подстановка {0}.", CatType.TM);

			Assert.AreEqual(_editorPage.CatTranslationMatchPercent(rowNumber: 1), 94,
				"Произошла ошибка:\nНеверный процент совпадения в CAT-панели.");
		}

		[TestCase(MachineTranslationType.SmartCATTranslator)]
		[TestCase(MachineTranslationType.Google)]
		[TestCase(MachineTranslationType.MicrosoftTranslator)]
		public void MTTest(MachineTranslationType machineTranslation)
		{
			var document = PathProvider.DocumentFile;

			_createProjectHelper
				.CreateNewProject(
					projectName: _projectUniqueName,
					filesPaths: new[] { document },
					createNewTm: true);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, document)
				.ClickDocumentSettings(_projectUniqueName, document)
				.SelectMachineTranslation(machineTranslation);

			_documentSettingsDialog.CloseDocumentSettings(_projectUniqueName);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(document, ThreadUser.NickName, _projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(document);
			
			_selectTaskDialog.SelectTask();

			Assert.IsTrue(_editorPage.IsCatTableExist(), "Произошла ошибка:\nCAT-панель пустая.");

			Assert.IsTrue(_editorPage.IsCatTypeExist(CatType.MT),
				"Произошла ошибка:\nВ CAT-панели отсутствует подстановка {0}.", CatType.MT);

			Assert.IsTrue(_editorPage.IsMTSourceTextMatchSourceText(segmentNumber: 1),
				"Произошла ошибка:\nТекст из CAT - подстановки типа {0} не соответствуют тексту в сорсе.", CatType.MT);
		}

		[Test]
		public void DefaultMTDuringProjectCreationTest()
		{
			var document = PathProvider.DocumentFile;

			_createProjectHelper
				.CreateNewProject(
					projectName: _projectUniqueName,
					filesPaths: new[] { document },
					createNewTm: true,
					useMachineTranslation: true);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, document)
				.ClickDocumentSettings(_projectUniqueName, document);

			Assert.IsTrue(_documentSettingsDialog.IsMachineTranslationSelected(),
				"Произошла ошибка:\nМашинный перевод не выбран ");

			_documentSettingsDialog.ClickSaveButtonExpectingProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(document, ThreadUser.NickName, _projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(document);

			_selectTaskDialog.SelectTask();

			Assert.IsTrue(_editorPage.IsCatTableExist(), "Произошла ошибка:\nCAT-панель пустая.");

			Assert.IsTrue(_editorPage.IsCatTypeExist(CatType.MT),
				"Произошла ошибка:\nВ CAT-панели отсутствует подстановка {0}.", CatType.MT);
		}

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private ProjectsPage _projectsPage;
		private DocumentSettingsDialog _documentSettingsDialog;
		private ProjectSettingsPage _projectSettingsPage;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
	}
}
