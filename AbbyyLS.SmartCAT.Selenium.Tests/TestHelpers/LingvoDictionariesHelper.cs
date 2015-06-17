using System.Collections.Generic;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.LingvoDictionaries;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class LingvoDictionariesHelper
	{

		public LingvoDictionariesHelper AssertLingvoDictionariesListIsNotEmpty()
		{
			BaseObject.InitPage(_lingvoDictionariesPage);
			_lingvoDictionariesPage.AssertLingvoDictionariesListIsNotEmpty();

			return this;
		}

		public LingvoDictionariesHelper AssertLingvoDictionariesListsMatch(IList<string> dictionariesList)
		{
			BaseObject.InitPage(_lingvoDictionariesPage);
			_lingvoDictionariesPage.AssertDictionariesListsMatch(dictionariesList);

			return this;
		}

		private readonly LingvoDictionariesPage _lingvoDictionariesPage = new LingvoDictionariesPage();
	}
}
