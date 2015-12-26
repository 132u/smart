using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorMatchColumnTMSubstitutionTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_translationMemoriesPage = new TranslationMemoriesPage(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);

			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(
					projectName: projectUniqueName,
					filePath: PathProvider.TxtFileForMatchTest,
					createNewTm: true,
					tmxFilePath: PathProvider.TmxFileForMatchTest)
				.GoToTranslationMemoriesPage();

			_translationMemoriesPage.CloseAllNotifications<TranslationMemoriesPage>();

			_workspaceHelper.GoToProjectsPage();

			_workspaceHelper
				.GoToProjectSettingsPage(projectUniqueName)
				.AssignTasksOnDocument(PathProvider.TxtFileForMatchTest, ThreadUser.NickName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(PathProvider.TxtFileForMatchTest);

			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();
		}

		[Test]
		public void CheckMatchAfterTmSubstitutionSegmentNumber()
		{
			_editorPage.PasteTranslationFromCAT(catType: CatType.TM);

			var catRowNumber = _editorPage.CatTypeRowNumber(CatType.TM);

            Assert.IsTrue(_editorPage.IsCATPercentMatchTargetPercent(1, catRowNumber),
				"Произошла ошибка:\n Процент совпадения в CAT-панели и в таргете не совпадает.");

            Assert.IsTrue(_editorPage.IsTargetMatchPercentCollorCorrect(),
				"Произошла ошибка:\n неправильный цвет процента совпадения в сегменте");
		}

		[Test]
		public void CheckMatchAfterEditCell()
		{
			_editorPage
				.PasteTranslationFromCAT(catType: CatType.TM)
				.FillSegmentTargetField("Translation", rowNumber: 1, clearField: false);

			Assert.IsTrue(_editorPage.IsMatchColumnCatTypeMatch(catType: CatType.TM),
				"Произошла ошибка:\n тип подстановки в колонке Match Column не совпал с типом перевода {0}.", CatType.TM);

			var catRowNumber = _editorPage.CatTypeRowNumber(CatType.TM);

            Assert.IsTrue(_editorPage.IsCATPercentMatchTargetPercent(1, catRowNumber),
				"Произошла ошибка:\n Процент совпадения в CAT-панели и в таргете не совпадает.");
		}

		[Test]
		public void CheckMatchAfterDelete()
		{
			_editorPage
				.PasteTranslationFromCAT(catType: CatType.TM)
				.FillSegmentTargetField();

			Assert.IsTrue(_editorPage.IsMatchColumnCatTypeMatch(catType: CatType.TM),
				"Произошла ошибка:\n тип подстановки в колонке Match Column не совпал с типом перевода {0}.", CatType.TM);

			var catRowNumber = _editorPage.CatTypeRowNumber(CatType.TM);

            Assert.IsTrue(_editorPage.IsCATPercentMatchTargetPercent(1, catRowNumber),
				"Произошла ошибка:\n Процент совпадения в CAT-панели и в таргете не совпадает.");
		}

		[Test]
		public void CheckTmMatchAfterAdd()
		{
			_editorPage
				.FillSegmentTargetField()
				.PasteTranslationFromCAT(CatType.TM);

			Assert.IsTrue(_editorPage.IsMatchColumnCatTypeMatch(catType: CatType.TM),
				"Произошла ошибка:\n тип подстановки в колонке Match Column не совпал с типом перевода {0}.", CatType.TM);

			var catRowNumber = _editorPage.CatTypeRowNumber(CatType.TM);

			Assert.IsTrue(_editorPage.IsCATPercentMatchTargetPercent(1, catRowNumber),
				"Произошла ошибка:\n Процент совпадения в CAT-панели и в таргете не совпадает.");
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsPage _projectSettingsPage;
		private WorkspaceHelper _workspaceHelper;
		private TranslationMemoriesPage _translationMemoriesPage;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
	}
}
