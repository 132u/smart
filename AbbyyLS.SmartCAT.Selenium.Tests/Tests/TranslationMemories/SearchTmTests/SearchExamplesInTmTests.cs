using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search.SearchPageTabs;

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
		}

		[Test(Description = "S-7296")]
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

		[Test, Description("S-14645")]
		public void AdvancedSearchTranslationExampleInTmTest()
		{
			var _tmName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();
			var source = "pretranslation line.";
			var rightTargetPartial = "для претранслейта";
			var leftTargetPartial = "предложение";

			TranslationMemoriesHelper.CreateTranslationMemory(
				_tmName, importFilePath: PathProvider.OneLineTmxFile);

			WorkspacePage.GoToSearchPage();

			_searchPage.SwitchToExamplesTab();

			_examplesTab.InitAdvancedSearch(source, rightTargetPartial);

			Assert.IsTrue(_examplesTab.IsSourceInSearchResultCorrect(1, source),
				"Выведенный сорс не совпадает с сорсом в TM.");

			Assert.IsTrue(_examplesTab.IsTargetInSearchResultCorrect(1, leftTargetPartial, rightTargetPartial),
				"Выведенный пример перевода не совпадает с переводом в ТМ.");
			//заведён баг, PRX-16144, как починят надо будет дописать один Assert.
			//должно отображаться название ТМ.
		}

		private SearchPage _searchPage;
		private ExamplesTab _examplesTab;
	}
}