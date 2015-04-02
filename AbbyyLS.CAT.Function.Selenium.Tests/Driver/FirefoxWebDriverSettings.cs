using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Driver
{
	class FirefoxWebDriverSettings : IWebDriverSettings
	{
		public IWebDriver Driver { get; set; }

		public string[] ProcessNames { get; set; }

		public FirefoxWebDriverSettings()
		{
			// Создать профиль с заданными настройками
			var profile = new FirefoxProfile { AcceptUntrustedCertificates = true };
			profile.SetPreference("browser.download.dir", PathProvider.ResultsFolderPath);
			profile.SetPreference("browser.download.folderList", 2);
			profile.SetPreference("browser.download.useDownloadDir", false);
			//profile.SetPreference("browser.download.manager.showWhenStarting", false);
			profile.SetPreference("browser.helperApps.alwaysAsk.force", false);
			profile.SetPreference
				("browser.helperApps.neverAsk.saveToDisk", "text/xml, text/csv, text/plain, text/log, application/zip, application/x-gzip, application/x-compressed, application/vnd.openxmlformats-officedocument.wordprocessingml.document, application/msword, application/octet-stream");
			Driver = new FirefoxDriver(profile);
			ProcessNames = new[] { "firefox", "conhost" };
		}
	}
}
