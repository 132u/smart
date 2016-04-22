using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Editor]
	public class EditorMatchColumnTMSubstitutionTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_translationMemoriesPage = new TranslationMemoriesPage(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_projectsPage = new ProjectsPage(Driver);

			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(
					projectName: projectUniqueName,
					filesPaths: new[] { PathProvider.TxtFileForMatchTest },
					createNewTm: true,
					tmxFilesPaths: new[] { PathProvider.TmxFileForMatchTest });

			_workspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesPage.CloseAllNotifications<TranslationMemoriesPage>();

			_workspacePage.GoToProjectsPage();

			_projectsPage.ClickProject(projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(PathProvider.TxtFileForMatchTest, ThreadUser.NickName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(PathProvider.TxtFileForMatchTest);

			_selectTaskDialog.SelectTask();
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
		private ProjectSettingsHelper _projectSettingsHelper;
		private ProjectsPage _projectsPage;
		private WorkspacePage _workspacePage;
		private TranslationMemoriesPage _translationMemoriesPage;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
	}
}
