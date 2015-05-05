using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Driver
{
	class ChromeWebDriverSettings: IWebDriverSettings
	{
		public IWebDriver Driver { get; set; }
		
		public string[] ProcessNames { get; set; }
		
		public ChromeWebDriverSettings()
		{
			var options = new ChromeOptions();
			options.AddArguments("--lang=en");
			options.AddUserProfilePreference("download.default_directory", PathProvider.ResultsFolderPath);
			Driver = new ChromeDriver(options);
			ProcessNames = new[] { "chrome", "chromedriver" };
		}
	}
}
