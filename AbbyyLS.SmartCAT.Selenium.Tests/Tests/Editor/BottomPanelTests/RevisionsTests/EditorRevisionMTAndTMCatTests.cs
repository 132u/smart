using System.IO;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	class EditorRevisionMTAndTMCatTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_settingsDialog = new ProjectSettingsDialog(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_signInPage = new SignInPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_pretranslationDialog = new PretranslationDialog(Driver);
			_newTranslationMemoryDialog = new NewTranslationMemoryDialog(Driver);
			_editTranslationMemoryDialog = new EditTranslationMemoryDialog(Driver);
			_translationMemoriesHelper = new TranslationMemoriesHelper(Driver);

			var glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_newTranslationName = _translationMemoriesHelper.GetTranslationMemoryUniqueName();

			_createProjectHelper
				.CreateNewProject(
				_projectUniqueName,
				filePath: PathProvider.EditorTxtFile,
				tmxFilePath: PathProvider.EditorTmxFile,
				createNewTm: true,
				useMachineTranslation: true,
				glossaryName: glossaryUniqueName);

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsHelper.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile), ThreadUser.NickName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			_selectTaskDialog.SelectTask();
		}
		
		[Test]
		public void MTRevisionPretranslateTest()
		{
			_editorPage.PasteTranslationFromCAT(CatType.MT);

			Assert.AreEqual(RevisionType.InsertMT.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка:\nНеверный тип ревизии.");
			
			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.ClickPretranslateButton();

			_pretranslationDialog
				.ClickAddInsertionRuleButton()
				.SelectResourceOption(CatType.MT)
				.ClickSaveAndRunButton();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			_selectTaskDialog.SelectTask();

			Assert.AreEqual(RevisionType.InsertMT.Description(), _editorPage.GetRevisionType(),
						"Произошла ошибка:\nНеверный тип ревизии.");
		}

		[Test]
		public void TMRevisionPretranslateTest()
		{
			_editorPage.PasteTranslationFromCAT(CatType.TM);

			Assert.AreEqual(RevisionType.InsertTM.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка:\nНеверный тип ревизии.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.ClickEditTranslatioinMemoryButton();

			_editTranslationMemoryDialog.ClickUploadButton();

			_newTranslationMemoryDialog.CreateNewTranslationMemory(_newTranslationName, PathProvider.OneLineTmxFile);

			Assert.IsTrue(_editTranslationMemoryDialog.IsTranslationMemoryCheckboxChecked(_newTranslationName),
				"Произошла ошибка:\nПамять перевода не выбрана.");

			_editTranslationMemoryDialog.ClickSaveButton();

			_projectSettingsPage.ClickPretranslateButton();

			_pretranslationDialog
				.ClickAddInsertionRuleButton()
				.SelectTranslationMemoryResourceOption(_newTranslationName)
				.ClickSaveAndRunButton();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			_selectTaskDialog.SelectTask();

			Assert.AreEqual(RevisionType.InsertTM.Description(), _editorPage.GetRevisionType(),
						"Произошла ошибка:\nНеверный тип ревизии.");
		}

		[Test]
		public void MTRevisionHotkeyPretranslateTest()
		{
			_editorPage.PasteTranslationFromCATByHotkey(CatType.MT);

			Assert.AreEqual(RevisionType.InsertMT.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка:\nНеверный тип ревизии.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.ClickPretranslateButton();

			_pretranslationDialog
				.ClickAddInsertionRuleButton()
				.SelectResourceOption(CatType.MT)
				.ClickSaveAndRunButton();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			_selectTaskDialog.SelectTask();

			Assert.AreEqual(RevisionType.InsertMT.Description(), _editorPage.GetRevisionType(),
						"Произошла ошибка:\nНеверный тип ревизии.");
		}

		[Test]
		public void TMRevisionHotkeyPretranslateTest()
		{
			_editorPage.PasteTranslationFromCATByHotkey(CatType.TM);

			Assert.AreEqual(RevisionType.InsertTM.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка:\nНеверный тип ревизии.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.ClickEditTranslatioinMemoryButton();

			_editTranslationMemoryDialog.ClickUploadButton();

			_newTranslationMemoryDialog.CreateNewTranslationMemory(_newTranslationName, PathProvider.OneLineTmxFile);

			Assert.IsTrue(_editTranslationMemoryDialog.IsTranslationMemoryCheckboxChecked(_newTranslationName),
				"Произошла ошибка:\nПамять перевода не выбрана.");

			_editTranslationMemoryDialog.ClickSaveButton();

			_projectSettingsPage.ClickPretranslateButton();

			_pretranslationDialog
				.ClickAddInsertionRuleButton()
				.SelectTranslationMemoryResourceOption(_newTranslationName)
				.ClickSaveAndRunButton();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			_selectTaskDialog.SelectTask();

			Assert.AreEqual(RevisionType.InsertTM.Description(), _editorPage.GetRevisionType(),
						"Произошла ошибка:\nНеверный тип ревизии.");
		}

		protected CreateProjectHelper _createProjectHelper;
		protected ProjectSettingsHelper _projectSettingsHelper;
		protected ProjectSettingsPage _projectSettingsPage;
		protected NewTranslationMemoryDialog _newTranslationMemoryDialog;
		protected EditTranslationMemoryDialog _editTranslationMemoryDialog;
		protected ProjectSettingsDialog _settingsDialog;
		protected SignInPage _signInPage;
		protected ProjectsPage _projectsPage;
		protected EditorPage _editorPage;
		protected SelectTaskDialog _selectTaskDialog;
		protected string _projectUniqueName;
		protected string _newTranslationName;
		protected PretranslationDialog _pretranslationDialog;
		protected TranslationMemoriesHelper _translationMemoriesHelper;
	}
}
