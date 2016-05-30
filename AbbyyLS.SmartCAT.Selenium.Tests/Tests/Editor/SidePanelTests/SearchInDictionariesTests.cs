using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
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

		[Test, Description("S-7242"), ShortCheckList]
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

		[Test, Description("S-29216"), ShortCheckList]
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

		[Test, Description("S-29217"), ShortCheckList]
		public void AutoPasteWordToSearchQueryFromTargetByHotkeyTest()
		{
			var translation = "первый";

			_editorPage
				.FillTarget(translation, rowNumber: 1)
				.SelectFirstWordInSegment(rowNumber: 1, segmentType: SegmentType.Target)
				.ClickSearchInLingvoDictionariesButtonByHotkey();

			Assert.AreEqual(translation, _editorPage.GetTextFromSearchFieldInDictionariesTab(),
				"Произошла ошибка: не сработала автоподстановка");
		}

		[Test, Description("S-7243"), ShortCheckList]
		public void AutoPasteWordToSearchQueryFromSourceByHotkeyTest()
		{
			var firstWord = "first";

			_editorPage
				.SelectFirstWordInSegment(rowNumber: 1, segmentType: SegmentType.Source)
				.ClickSearchInLingvoDictionariesButtonByHotkey();

			Assert.AreEqual(firstWord, _editorPage.GetTextFromSearchFieldInDictionariesTab(),
				"Произошла ошибка: не сработала автоподстановка");
		}

		[Test]
		public void AutoPasteWordToSearchQueryFromTargetTest()
		{
			var translation = "первый";

			_editorPage
				.FillTarget(translation, rowNumber: 1)
				.SelectFirstWordInSegment(rowNumber: 1, segmentType: SegmentType.Target)
				.ClickSearchInLingvoDictionariesButton();

			Assert.AreEqual(translation, _editorPage.GetTextFromSearchFieldInDictionariesTab(),
				"Произошла ошибка: не сработала автоподстановка");
		}

		[Test]
		public void AutoPasteWordToSearchQueryFromSourceTest()
		{
			var firstWord = "first";

			_editorPage
				.SelectFirstWordInSegment(rowNumber: 1, segmentType: SegmentType.Source)
				.ClickSearchInLingvoDictionariesButton();

			Assert.AreEqual(firstWord, _editorPage.GetTextFromSearchFieldInDictionariesTab(),
				"Произошла ошибка: не сработала автоподстановка");
		}

		[Test, Description("S-7244"), ShortCheckList]
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
