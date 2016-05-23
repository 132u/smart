using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Editor]
	class SearchInDictionariesTests<TWebDriverProvider> :
		EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_searchPage = new SearchPage(Driver);
		}

		[Test, Description("S-7242")]
		public void SearchSourceInDictionariesTest()
		{
			var searchQuery = "proton";

			_editorPage
				.ClickDictionariesTab()
				.FillSearchQueryInDictionariesTab(searchQuery)
				.ClickSearchButtonInDictionariesTab();

			Assert.IsTrue(_editorPage.IsDictionariesSearchResultsAppeared(searchQuery),
				"Произошла ошибка: не появились результаты поиска");
		}

		[Test, Description("S-29216")]
		public void SearchTargetInDictionariesTest()
		{
			var searchQuery = "электрон";

			_editorPage
				.ClickDictionariesTab()
				.ClickTranslationDirectionSwitchOnDictionariesTab()
				.FillSearchQueryInDictionariesTab(searchQuery)
				.ClickSearchButtonInDictionariesTab();

			Assert.IsTrue(_editorPage.IsDictionariesSearchResultsAppeared(searchQuery),
				"Произошла ошибка: не появились результаты поиска");
		}

		[Test, Description("S-7243")]
		public void AutoPasteWordToSearchQueryTest()
		{
			var translation = "первый";

			_editorPage
				.FillTarget(translation, rowNumber: 1)
				.SelectFirstWordInSegment(rowNumber: 1, segmentType: SegmentType.Target)
				.ClickSearchInLingvoDictionariesButton();

			Assert.AreEqual(translation, _editorPage.GetTextFromSearchFieldInDictionariesTab(),
				"Произошла ошибка: не всработала автоподстановка");
		}

		[Test, Description("S-7244")]
		public void LinkToSearchPageTest()
		{
			var searchQuery = "звезда";

			_editorPage
				.ClickDictionariesTab()
				.ClickTranslationDirectionSwitchOnDictionariesTab()
				.FillSearchQueryInDictionariesTab(searchQuery)
				.ClickSearchButtonInDictionariesTab()
				.ClickOpenTranslationInNewTabLink();

			Assert.AreEqual(searchQuery, _searchPage.GetSearchQueryFromSearchField(),
				"Произошла ошибка: не сработала автоподстановка");
		}

		private SearchPage _searchPage;
	}
}
