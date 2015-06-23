﻿using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class SearchHelper
	{
		public SearchHelper SetSourceLanguage(string sourceLanguage)
		{
			BaseObject.InitPage(_searchPage);
			_searchPage.SelectSourceLanguage(sourceLanguage);

			return this;
		}

		public SearchHelper SetTargetLanguage(string targetLanguage)
		{
			BaseObject.InitPage(_searchPage);
			_searchPage.SelectTargetLanguage(targetLanguage);

			return this;
		}

		public SearchHelper AssertDefinitionTabIsActive()
		{
			BaseObject.InitPage(_searchPage);
			_searchPage.AssertDefinitionTabIsActive();

			return this;
		}

		public SearchHelper InitSearch(string searchText)
		{
			BaseObject.InitPage(_searchPage);
			_searchPage
				.AddTextSearch(searchText)
				.ClickTranslateButton()
				.AssertSearchResultDisplay();

			return this;
		}

		public SearchHelper ClickTranslationWord(string text)
		{
			BaseObject.InitPage(_searchPage);
			_searchPage.ClickTranslationWord(text);

			return this;
		}

		public SearchHelper AssertTranslationReferenceExist(string text)
		{
			BaseObject.InitPage(_searchPage);
			_searchPage.AssertTranslationReferenceExist(text);

			return this;
		}

		public SearchHelper OpenWordByWordTranslation()
		{
			BaseObject.InitPage(_searchPage);
			_searchPage
				.ClickTranslationFormReference()
				.AssertWordByWordTranslationAppear();

			return this;
		}

		public SearchHelper AssertReverseTranslationListExist()
		{
			BaseObject.InitPage(_searchPage);
			_searchPage.AssertReverseTranslationListExist();

			return this;
		}

		public SearchHelper AssertAutoreversedMessageExist()
		{
			BaseObject.InitPage(_searchPage);
			_searchPage.AssertAutoreversedMessageExist();

			return this;
		}

		public SearchHelper AsserttAutoreversedReferenceExist()
		{
			BaseObject.InitPage(_searchPage);
			_searchPage.AssertAutoreversedReferenceExist();

			return this;
		}

		private readonly SearchPage _searchPage = new SearchPage();
	}
}
