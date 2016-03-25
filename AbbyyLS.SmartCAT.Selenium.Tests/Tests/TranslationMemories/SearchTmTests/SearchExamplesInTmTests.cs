using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories.SearchTmTests
{
	class SearchExamplesInTmTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSearchTranslationExample()
		{
			_searchPage = new SearchPage(Driver);
		}

		[Test(Description = "S-7296")]
		public void SearchTranslationExampleInTmTest()
		{
			var _uniquePhraseForSearch = "Hello world! (8a4fcecf-b9ec-419c-a947-89f44ac2ea63)";
			var _tmName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();

			WorkspacePage.GoToSearchPage();

			_searchPage
				.SwitchToExamplesTab()
				.InitSearch(_uniquePhraseForSearch);

			if (!_searchPage.IsNoExamplesFoundMessageDisplayed())
				throw new Exception("Найден пример перевода, для недобавленной ТМ");

			WorkspacePage.GoToTranslationMemoriesPage();

			TranslationMemoriesHelper.CreateTranslationMemory(
				_tmName, importFilePath: PathProvider.TmForSearchTest);

			WorkspacePage.GoToSearchPage();

			_searchPage
				.SwitchToExamplesTab()
				.InitSearch(_uniquePhraseForSearch);

			Assert.IsTrue(_searchPage.IsSearchResultDisplayed(),
				"Произошла ошибка: \n не нашелся пример перевода в созданной ТМ");
		}

		private SearchPage _searchPage;
	}
}