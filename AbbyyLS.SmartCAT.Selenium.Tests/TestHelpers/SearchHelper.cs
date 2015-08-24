﻿using System.Collections.Generic;
﻿using System.Linq;

﻿using NLog;

﻿using NUnit.Framework;

﻿using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class SearchHelper
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();

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

		public SearchHelper AssertGlossariesNamesMatch(List<string> glossaryNames)
		{
			BaseObject.InitPage(_searchPage);

			Assert.IsTrue(glossaryNames.SequenceEqual(_searchPage.GlossaryNamesList()),
				"Произошла ошибка:\n списки имен глоссариев не совпадают.");

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

		public SearchHelper AssertTermNamesMatch(string term)
		{
			Logger.Trace("Проверить, что термин {0} сопадает с найденными терминами.", term);
			BaseObject.InitPage(_searchPage);

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
