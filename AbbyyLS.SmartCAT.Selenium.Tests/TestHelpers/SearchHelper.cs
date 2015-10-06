﻿using System.Collections.Generic;
﻿using System.Linq;

﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
﻿using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class SearchHelper
	{
		public WebDriver Driver { get; private set; }

		public SearchHelper(WebDriver driver)
		{
			Driver = driver;
			_searchPage = new SearchPage(Driver);
		}

		public SearchHelper SetSourceLanguage(string sourceLanguage)
		{
			BaseObject.InitPage(_searchPage, Driver);
			_searchPage.SelectSourceLanguage(sourceLanguage);

			return this;
		}

		public SearchHelper SetTargetLanguage(string targetLanguage)
		{
			BaseObject.InitPage(_searchPage, Driver);
			_searchPage.SelectTargetLanguage(targetLanguage);

			return this;
		}

		public SearchHelper AssertDefinitionTabIsActive()
		{
			BaseObject.InitPage(_searchPage, Driver);
			_searchPage.AssertDefinitionTabIsActive();

			return this;
		}

		public SearchHelper AssertGlossariesNamesMatch(List<string> glossaryNames)
		{
			BaseObject.InitPage(_searchPage, Driver);

			Assert.IsTrue(glossaryNames.OrderBy(m => m).SequenceEqual(_searchPage.GlossaryNamesList().OrderBy(m => m)),
				"Произошла ошибка:\n списки имен глоссариев не совпадают.");

			return this;
		}

		public SearchHelper InitSearch(string searchText)
		{
			BaseObject.InitPage(_searchPage, Driver);
			_searchPage
				.AddTextSearch(searchText)
				.ClickTranslateButton()
				.AssertSearchResultDisplay();

			return this;
		}

		public SearchHelper ClickTranslationWord(string text)
		{
			BaseObject.InitPage(_searchPage, Driver);
			_searchPage.ClickTranslationWord(text);

			return this;
		}

		public SearchHelper AssertTranslationReferenceExist(string text)
		{
			BaseObject.InitPage(_searchPage, Driver);
			_searchPage.AssertTranslationReferenceExist(text);

			return this;
		}

		public SearchHelper AssertTermNamesMatch(string term)
		{
			CustomTestContext.WriteLine("Проверить, что термин {0} сопадает с найденными терминами.", term);
			BaseObject.InitPage(_searchPage, Driver);

			for (var i = 1; i <= _searchPage.GlossaryNamesList().Count; i++)
			{
				var searchTerm = _searchPage.TermName(i);

				Assert.AreEqual(term, searchTerm,
					"Произошла ошибка:\n найденный термин №{0} '{1}' не совпадает с заданным термином '{2}'.",
					i, searchTerm, term);
			}

			return this;
		}

		public SearchHelper OpenWordByWordTranslation()
		{
			BaseObject.InitPage(_searchPage, Driver);
			_searchPage
				.ClickTranslationFormReference()
				.AssertWordByWordTranslationAppear();

			return this;
		}

		public SearchHelper AssertReverseTranslationListExist()
		{
			BaseObject.InitPage(_searchPage, Driver);
			_searchPage.AssertReverseTranslationListExist();

			return this;
		}

		public SearchHelper AssertAutoreversedMessageExist()
		{
			BaseObject.InitPage(_searchPage, Driver);
			_searchPage.AssertAutoreversedMessageExist();

			return this;
		}

		public SearchHelper AsserttAutoreversedReferenceExist()
		{
			BaseObject.InitPage(_searchPage, Driver);
			_searchPage.AssertAutoreversedReferenceExist();

			return this;
		}

		private readonly SearchPage _searchPage;
	}
}
