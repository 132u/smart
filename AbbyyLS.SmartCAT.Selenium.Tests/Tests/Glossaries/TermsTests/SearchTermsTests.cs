using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries.SuggestedTerms;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search.SearchPageTabs;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Glossaries]
	class SearchTermsTests<TWebDriverSettings>
		: BaseGlossaryTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_filePaths = new List<string>();
			_glossaryNamesList = new List<string>();
			_termsFromHistory = new List<string>();

			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_searchPage = new SearchPage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_editorPage = new EditorPage(Driver);
			_addTermDialog = new AddTermDialog(Driver);
			_examplesFromTextsDialog = new ExamplesFromTextsDialog(Driver);
			_termAlreadyExistsDialog = new TermAlreadyExistsDialog(Driver);
			_suggestTermDialog = new SuggestTermDialog(Driver);
			_suggestedTermsPage = new SuggestedTermsPage(Driver);
			_searchHistoryPage = new SearchHistoryPage(Driver);
			_translationsTab = new TranslationsTab(Driver);
			_glossaryStructureDialog = new GlossaryStructureDialog(Driver);
			_newGlossaryDialog = new NewGlossaryDialog(Driver);
			_definitionsTab = new DefinitionsTab(Driver);

			_glossariesHelper.CreateGlossary(_glossaryUniqueName);
		}

		[Test]
		public void SearchTermByFirstLanguageTest()
		{
			var uniqueText = DateTime.UtcNow.Ticks + "1Term";
			var term1 = "Term1";
			var term2 = "Term2";
			var term3 = "Term3 " + uniqueText;
			var term4 = "Term4 " + DateTime.UtcNow;

			_glossaryPage
				.CreateTerm(term1, term2)
				.CreateTerm(term3, term4)
				.SearchTerm(uniqueText);

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 1),
				"Произошла ошибка:\n неверное количество терминов");

			Assert.AreEqual(term3, _glossaryPage.GetTermText(),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}

		[Test]
		public void SearchTermBySecondLanguageTest()
		{
			var uniqueText = DateTime.UtcNow.Ticks + "1Term";
			var term1 = "Term1";
			var term2 = "Term2";
			var term3 = "Term3 " + DateTime.UtcNow;
			var term4 = "Term4 " + uniqueText;

			_glossaryPage
				.CreateTerm(term1, term2)
				.CreateTerm(term3, term4)
				.SearchTerm(uniqueText);

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 1),
				"Произошла ошибка:\n неверное количество терминов");

			Assert.AreEqual(term3, _glossaryPage.GetTermText(),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}

		[Test, Description("S-7295"), ShortCheckList]
		public void SearchTermTest()
		{
			var uniqueData = DateTime.UtcNow.Ticks + "SearchTest";
			var firstTerm = "Test First Term " + uniqueData;
			var secondTerm = "Test Second Term ";
			var glossaryUniqueName2 = GlossariesHelper.UniqueGlossaryName();

			var glossaryList = new List<string>
			{
				_glossaryUniqueName, 
				glossaryUniqueName2
			};

			_glossaryPage.CreateTerm(firstTerm, secondTerm + DateTime.UtcNow.Ticks);

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(glossaryUniqueName2);

			_glossaryPage.CreateTerm(firstTerm, secondTerm + DateTime.UtcNow.Ticks);

			_workspacePage.GoToSearchPage();

			_searchPage.InitSearch(uniqueData);

			Assert.IsTrue(_searchPage.IsGlossariesNamesMatch(glossaryList),
				"Произошла ошибка:\n списки имен глоссариев не совпадают.");

			Assert.IsTrue(_searchPage.IsTermNamesMatch(firstTerm),
				"Произошла ошибка:\n найденные термины не совпадает с заданным термином '{0}'.", firstTerm);
		}

		[Test, Description("S-14637"), ShortCheckList]
		public void SearchExistingTermsTest()
		{
			var term1 = "Term1" + Guid.NewGuid();
			var term2 = "Term2" + Guid.NewGuid();

			_glossaryNamesList.Add(_glossaryUniqueName);

			_glossaryPage.CreateTerm(term1, term2);

			_workspacePage.GoToSearchPage();

			_searchPage.InitSearch(term1);

			Assert.IsTrue(_searchPage.IsTermNamesMatch(term1),
				"Произошла ошибка:\n заданный термин не совпадает с найденным.");

			Assert.IsTrue(_searchPage.IsGlossariesNamesMatch(_glossaryNamesList),
				"Произошла ошибка:\n списки имён глоссариев не совпадают.");
		}

		[Test, Description("S-14638"), ShortCheckList]
		public void ExampleFromGlossaryTermTest()
		{
			var firstWordInSource = "first";
			var firstWordInTarget = "term1" + Guid.NewGuid();
			var sourceSentence = "first sentence.";
			var targetSentense = firstWordInTarget + " " +"предложение.";
			var uniqueProjectName = _createProjectHelper.GetProjectUniqueName();
			var comment = "Comment" + Guid.NewGuid();
			var glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			var document = PathProvider.LongTxtFile;

			_filePaths.Add(document);

			_workspacePage.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(
				uniqueProjectName,
				glossaryName: glossaryUniqueName,
				createNewTm:true,
				filesPaths: _filePaths
				);

			_projectsPage.OpenProjectSettingsPage(uniqueProjectName);

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(_filePaths[0]);

			_editorPage
				.FillTarget(targetSentense)
				.ConfirmSegmentTranslation()
				.ClickAddTermButton();

			_addTermDialog.AddNewTerm(firstWordInSource, firstWordInTarget, comment, glossaryUniqueName);

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToSearchPage();

			_searchPage.InitSearch(firstWordInTarget);

			_translationsTab.ClickTranslationWordFromGlossary(firstWordInSource);

			Assert.IsTrue(_examplesFromTextsDialog.IsExamplesFromTextsDialogOpened(),
				"Не открылся диалог 'Примеры из текста'");

			_examplesFromTextsDialog.ClickOnArrowButton();

			Assert.IsTrue(_examplesFromTextsDialog.IsSourceTextInEditorAndFindedTextSame(sourceSentence),
				"Фраза из source сегмента редактора не совпадает с найденной");

			Assert.IsTrue(_examplesFromTextsDialog.IsTagetTextInEditorAndFindedTextSame(targetSentense),
				"Фраза из target сегмента редактора не совпадает с найденной");

			Assert.IsTrue(_examplesFromTextsDialog.IsTmNameCorrect(uniqueProjectName),
				"Имя созданного проекта не совпадает с найденным");
		}

		[Test, Description("S-14642"), ShortCheckList]
		public void ReverseSearchForGlossaryTermTest()
		{
			var term1 = "term1" + Guid.NewGuid();
			var term2 = "term2" + Guid.NewGuid();

			_glossaryPage.CreateTerm(term1, term2);

			_workspacePage.GoToSearchPage();

			_searchPage.InitSearch(term1);

			_translationsTab.ClickTranslationWordFromGlossary(term2);

			_examplesFromTextsDialog.ClickOnFoundedTranslationWord();

			Assert.IsTrue(_searchPage.IsWordInSearchFieldCorrect(term2),
				"Не сработал обратный поиск термина по переводу."); 
		}

		[Test, Description("S-14639"), ShortCheckList]
		public void AddNewTermInGlossaryFromSearchTabTest()
		{
			var term1 = "term1" + Guid.NewGuid();
			var term2 = "term2" + Guid.NewGuid();
			var term3 = "term3" + Guid.NewGuid();

			_glossaryPage.CreateTerm(term1, term2);

			_workspacePage.GoToSearchPage();

			_searchPage.InitSearch(term1);

			_translationsTab.ClickAddEntryButton(_glossaryUniqueName);

			_glossaryPage
				.FillTerm(columnNumber: 2,text: term3)
				.ClickSaveTermButtonExpectingTermAlreadyExistsDialog();

			_termAlreadyExistsDialog.ClickSaveChangesButton();

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(term1, term2),
				"Указанная пара терми-перевод, не отобразилась."); 

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(term1, term3),
				"Указанная пара терми-перевод, не отобразилась.");
		}

		[Test, Description("S-14640"), ShortCheckList]
		public void SuggestTermsTest()
		{
			var term1 = "term1" + Guid.NewGuid();
			var term2 = "term2" + Guid.NewGuid();
			var term3 = "term3" + Guid.NewGuid();
			var comment = "Comment" + Guid.NewGuid();

			_glossaryPage.CreateTerm(term1, term2);

			_workspacePage.GoToSearchPage();

			_searchPage.InitSearch(term1);

			_translationsTab.ClickOnSuggestTermButton(_glossaryUniqueName);

			_suggestTermDialog
				.FillSuggestTermDialog(term1, term3, comment: comment)
				.ClickSaveButtonExpectingError()
				.ClickSaveTermAnywayButtonExpectingSearchPage();

			_translationsTab.ClickOnGoToTheGlossaryLink(_glossaryUniqueName);

			_glossaryPage. ClickSuggestedTermsButton();
			
			Assert.IsTrue(_suggestedTermsPage.IsSuggestedTermDisplayed(term1, term3),
				"Предложенные термины не совпадают с введёнными ранее или не отображаются.");
		}

		[Test, Description("S-14641"), ShortCheckList]
		public void SwitchToGlossaryFromSearchPageTest()
		{
			var term1 = "term1" + Guid.NewGuid();
			var term2 = "term2" + Guid.NewGuid();

			_glossaryPage.CreateTerm(term1, term2);

			_workspacePage.GoToSearchPage();

			_searchPage.InitSearch(term1);

			_translationsTab.ClickOnGoToTheGlossaryLink(_glossaryUniqueName);

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(term1, term2),
				"Предложенные термины не совпадают с введёнными ранее или не отображаются.");

			Assert.IsTrue(_glossaryPage.IsSearchFieldContainSearchedTerm(term1),
				"Поле поиска после перехода со страницы поиска не заполнилось значение термина.");
		}

		[Test, Description("S-14643"), ShortCheckList]
		public void AutoReverseLanguageTest()
		{
			var term1 = "term1" + Guid.NewGuid();
			var term2 = "term2" + Guid.NewGuid();

			_glossaryPage.CreateTerm(term1, term2);

			_workspacePage.GoToSearchPage();

			_searchPage.InitSearch(term2);

			Assert.IsTrue(_searchPage.IsAutoreversedMessageExist(),
				"Сообщение об автоматической смене языков не появилось.");

			Assert.IsTrue(_searchPage.IsSearchResultDisplayed(),
				"Результаты поиска не появились.");
		}

		[Test, Description("S-14644"), ShortCheckList]
		public void SearchQueryHistoryTest()
		{
			var term1 = "term1" + Guid.NewGuid();
			var term2 = "term2" + Guid.NewGuid();
			var firstNotAddedTerm = "First not added term" + DateTime.Now;
			var secondNotAddedTerm = "Second not added term" + DateTime.Now;

			_glossaryPage.CreateTerm(term1, term2);

			_workspacePage.GoToSearchPage();

			_searchPage.OpenSearchHistory();

			if (!_searchHistoryPage.IsSearchHistoryButtonOn())
			{
				_searchHistoryPage.ClickOnSearchHistoryButton();
			}

			_searchPage.InitSearch(term1);
			Assert.IsFalse(_searchPage.IsNothingFoundInGlossariesDisplayed(), 
				"Не отобразился результат поиска для добавленного термина.");

			_searchPage.InitSearch(firstNotAddedTerm);
			Assert.IsTrue(_searchPage.IsNothingFoundInGlossariesDisplayed(),
				"Отобразился результат поиска для недобавленного в глоссарий термина.");

			_searchPage.OpenSearchHistory();

			_searchHistoryPage.ClickOnSearchHistoryButton();

			_searchPage.InitSearch(secondNotAddedTerm);
			_termsFromHistory = _searchPage.GetSearchinqQueryFromHistory();

			Assert.IsFalse(_termsFromHistory.Contains(secondNotAddedTerm),
				"После выключения записи истории поиска, в ней отобразился термин.");

			_searchPage.OpenSearchHistory();

			_searchHistoryPage.ClickOnSearchHistoryButton();
		}

		[Test, Description("S-14647"), ShortCheckList]
		public void SearchDefinitionsFromGlossary()
		{
			var forTerm1 = new Random();
			var term1 = "term1" + forTerm1.Next(100000); //Guid не подходит, сложная вёрстка.
			var term2 = "term2" + Guid.NewGuid();
			var comment = "comment" + Guid.NewGuid();
			var definition = "difinition" + Guid.NewGuid();

			_glossaryPage.CreateTerm(term1, term2);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog
				.SelectLevelGlossaryStructure(GlossaryStructureLevel.GeneralInfo)
				.SelectSystemField(GlossarySystemField.Interpretation)
				.ClickAddToListButton()
				.ClickSaveButton();

			_glossaryPage
				.UnrollEditMenuForTerms(term1)
				.ClickEditEntryButton()
				.FillDefinition(definition)
				.FillCommentInDropDownEditMenu(comment)
				.ClickSaveEntryButton();

			_workspacePage.GoToSearchPage();

			_searchPage
				.SwitchToDefinitionsTab()
				.InitSearch(term1);

			Assert.IsTrue(_definitionsTab.IsTermAndDefinitionInSearchResultCorrect(1, term1, definition),
				"Не отобразились заданные толкование или термин.");

			Assert.IsTrue(_definitionsTab.IsGlossaryNameCorrect(_glossaryUniqueName),
				"Имя глоссария не совпадает с заданным.");
		}

		private List<string> _filePaths;
		private List<string> _glossaryNamesList;
		private List<string> _termsFromHistory;

		private SearchPage _searchPage;
		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private ProjectSettingsPage _projectSettingsPage;
		private EditorPage _editorPage;
		private AddTermDialog _addTermDialog;
		private ExamplesFromTextsDialog _examplesFromTextsDialog;
		private TermAlreadyExistsDialog _termAlreadyExistsDialog;
		private SuggestTermDialog _suggestTermDialog;
		private SuggestedTermsPage _suggestedTermsPage;
		private SearchHistoryPage _searchHistoryPage;
		private TranslationsTab _translationsTab;
		private DefinitionsTab _definitionsTab;
	}
}