using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.LingvoDictionaries;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class LingvoDictionariesHelper
	{
		public WebDriver Driver { get; private set; }

		public LingvoDictionariesHelper(WebDriver driver)
		{
			Driver = driver;
			_lingvoDictionariesPage = new LingvoDictionariesPage(Driver);
		}

		public LingvoDictionariesHelper AssertLingvoDictionariesListIsNotEmpty()
		{
			BaseObject.InitPage(_lingvoDictionariesPage, Driver);
			_lingvoDictionariesPage.AssertLingvoDictionariesListIsNotEmpty();

			return this;
		}

		public LingvoDictionariesHelper AssertLingvoDictionariesListCorrect(List<string> firstList)
		{
			var secondList = getDictionariesList();

			Assert.IsTrue(firstList.Match(secondList),
				"Произошла ошибка:\n список словарей на странице неверный.");

			return this;
		}

		private List<string> getDictionariesList()
		{
			BaseObject.InitPage(_lingvoDictionariesPage, Driver);

			return _lingvoDictionariesPage.GetDictionariesList();
		}

		private readonly LingvoDictionariesPage _lingvoDictionariesPage;
	}
}
