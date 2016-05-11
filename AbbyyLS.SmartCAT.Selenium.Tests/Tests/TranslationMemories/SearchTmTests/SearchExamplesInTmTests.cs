using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search.SearchPageTabs;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[TranslationMemories]
	class SearchExamplesInTmTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSearchTranslationExample()
		{
			_searchPage = new SearchPage(Driver);
			_examplesTab = new ExamplesTab(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_editorPage = new EditorPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
		}

		[Test(Description = "S-7296"), ShortCheckList]
		public void SearchTranslationExampleInTmTest()
		{
			var _uniquePhraseForSearch = "Hello world! (8a4fcecf-b9ec-419c-a947-89f44ac2ea63)";
			var _tmName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();

			WorkspacePage.GoToSearchPage();

			_searchPage.SwitchToExamplesTab();

			_examplesTab.InitSearch(_uniquePhraseForSearch);

			if (!_searchPage.IsNoExamplesFoundMessageDisplayed())
				throw new Exception("Найден пример перевода, для недобавленной ТМ");

			WorkspacePage.GoToTranslationMemoriesPage();

			TranslationMemoriesHelper.CreateTranslationMemory(
				_tmName, importFilePath: PathProvider.TmForSearchTest);

			WorkspacePage.GoToSearchPage();

			_searchPage.SwitchToExamplesTab();
			
			_examplesTab.InitSearch(_uniquePhraseForSearch);

			Assert.IsTrue(_examplesTab.IsSerchedTermDisplayed(_uniquePhraseForSearch),
				"Произошла ошибка: \n не нашелся пример перевода в созданной ТМ");
		}

		[Test, Description("S-14645"), ShortCheckList]
		public void AdvancedSearchTranslationExampleInTmTest()
		{
			var projectName = "Project Name" + Guid.NewGuid();
			var source = "first sentence.";
			var rightTargetPartial = Guid.NewGuid().ToString() + ".";
			var leftTargetPartial = "первое";

			WorkspacePage.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(
				projectName,
				new[] { PathProvider.EditorTxtFile},
				createNewTm: true);

			_projectsPage
				.OpenProjectInfo(projectName)
				.ClickDocumentRefExpectingEditorPage(PathProvider.EditorTxtFile);

			_editorPage
				.FillSegmentTargetField(text: leftTargetPartial + " " + rightTargetPartial)
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			WorkspacePage.GoToSearchPage();

			_searchPage.SwitchToExamplesTab();

			_examplesTab.InitAdvancedSearch(source, rightTargetPartial);

			Assert.IsTrue(_examplesTab.IsSourceInSearchResultCorrect(1, source),
				"Произошла ошибка: \n Выведенный сорс не совпадает с сорсом в TM.");

			Assert.IsTrue(_examplesTab.IsTargetInSearchResultCorrect(1, leftTargetPartial, rightTargetPartial),
				"Произошла ошибка: \n Выведенный пример перевода не совпадает с переводом в ТМ.");

			_examplesTab.ClickArrowButton();

			Assert.AreEqual(projectName, _examplesTab.GetProjectName(),
				"Произошла ошибка: \nЗаданное имя проекта не совпадает с отображенным.");
		}

		private SearchPage _searchPage;
		private ExamplesTab _examplesTab;
		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private EditorPage _editorPage;
	}
}