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
	[Standalone]
	class EditorUndoRedoCatPasteTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void EditorUndoRedoCatPasteTestsSetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_editorPage = new EditorPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			SettingsDialog = new SettingsDialog(Driver);
			SelectTaskDialog = new SelectTaskDialog(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			
			_createProjectHelper.CreateNewProject(
				projectName: projectUniqueName,
				filePath: PathProvider.EditorTxtFile,
				tmxFilePath: PathProvider.EditorTmxFile,
				createNewTm: true);

			_projectsPage.ClickProject(projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(PathProvider.EditorTxtFile, ThreadUser.NickName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);

			SelectTaskDialog.SelectTask();
		}

		[Test]
		public void UndoRedoButtonAfterCatPasteTest()
		{
			var text = "Translation";

			_editorPage
				.FillTarget(text)
				.PasteTranslationFromCAT(CatType.TM)
				.ClickUndoButton();

			Assert.AreEqual(text, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверное значение в таргете.");

			_editorPage.ClickRedoButton();

			Assert.AreEqual(
				_editorPage.GetCatTranslationText(_segmentNumber),
				_editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверное значение в таргете.");
		}

		[Test]
		public void UndoRedoHotkeyAfterCatPasteTest()
		{
			var text = "Translation";

			_editorPage
				.FillTarget(text)
				.PasteTranslationFromCAT(CatType.TM)
				.PressUndoHotkey();

			Assert.AreEqual(text, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверное значение в таргете.");

			_editorPage.PressRedoHotkey();

			Assert.AreEqual(
				_editorPage.GetCatTranslationText(_segmentNumber),
				_editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверное значение в таргете.");
		}

		const int _segmentNumber = 1;
		private CreateProjectHelper _createProjectHelper;
		private EditorPage _editorPage;
		private ProjectSettingsPage _projectSettingsPage;
		protected SettingsDialog SettingsDialog;
		public SelectTaskDialog SelectTaskDialog;
		private ProjectsPage _projectsPage;
		private ProjectSettingsHelper _projectSettingsHelper;
	}
}
