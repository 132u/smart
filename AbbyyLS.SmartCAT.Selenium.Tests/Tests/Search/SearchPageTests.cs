using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search.SearchPageTabs;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Search
{
	[Parallelizable(ParallelScope.Fixtures)]
	class SearchPageTests<TWebDriverSettings>
		: BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_searchPage = new SearchPage(Driver);
			_phrasesTab = new PhrasesTab(Driver);
			_workspacePage = new WorkspacePage(Driver);
		}

		[Test, Description("S-14646"), ShortCheckList]
		public void SearchPhrasesTest()
		{
			var russianLanguage = "ru";
			var englishLanguage = "en";
			var phrase = "System";

			_workspacePage.GoToSearchPage();

			_searchPage.SwitchToPhrasesTab();

			_phrasesTab
				.SelectSourceLanguage(russianLanguage)
				.SelectTargetLanguage(englishLanguage)
				.InitSearch(phrase);

			Assert.IsTrue(_phrasesTab.IsTableWithSearchResultDisplayed(),
				"Неотобразились результаты поиска.");

			Assert.IsTrue(_phrasesTab.IsReverseLanguageMessageDisplayed(),
				"Не сработало автоперключение языков перевода.");

			Assert.IsTrue(_phrasesTab.IsSourceLanguageCorrect(englishLanguage),
				"После автопереключения языков, отобразился неверный исходный язык.");

			Assert.IsTrue(_phrasesTab.IsTargetLanguageCorrect(russianLanguage),
				"После автопереключения языков, отобразился неверный язык перевода.");
		}

		private SearchPage _searchPage;
		private PhrasesTab _phrasesTab;
		private WorkspacePage _workspacePage;
	}
}