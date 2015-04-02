using System;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Driver
{
	class IEWebDriverSettings : IWebDriverSettings
	{
		public IWebDriver Driver { get; set; }

		public string[] ProcessNames { get; set; }
		
		public IEWebDriverSettings()
		{
			Driver = new InternetExplorerDriver();
			ProcessNames = new[] { "IEDriverServer", "conhost", "iexplore"};
		}
	}
}
