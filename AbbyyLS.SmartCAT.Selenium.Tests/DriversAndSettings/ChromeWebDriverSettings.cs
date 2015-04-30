using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DriversAndSettings
{
	class ChromeWebDriverSettings : IWebDriverSettings
	{
		public ChromeWebDriverSettings()
		{
			var options = new ChromeOptions();
			options.AddArguments("--lang=en");
			Driver = new ChromeDriver(options);
			ProcessNames = new[] { "chrome", "chromedriver" };
		}

		public IWebDriver Driver { get; set; }

		public string[] ProcessNames { get; set; }
	}
}
