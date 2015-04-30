using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DriversAndSettings
{
	class InternetExplorerWebDriverSettings : IWebDriverSettings
	{
		public InternetExplorerWebDriverSettings()
		{
			Driver = new InternetExplorerDriver();
			ProcessNames = new[] { "IEDriverServer", "conhost", "iexplore" };
		}

		public IWebDriver Driver { get; set; }

		public string[] ProcessNames { get; set; }
	}
}
