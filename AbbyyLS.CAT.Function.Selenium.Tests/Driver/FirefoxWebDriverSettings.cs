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
			profile.SetPreference("browser.download.useDownloadDir", true);
			profile.SetPreference("browser.helperApps.alwaysAsk.force", false);
			profile.SetPreference
				("browser.helperApps.neverAsk.saveToDisk", "application/xml, text/xml, text/csv, text/plain, text/log, application/zip, application/x-gzip, application/x-compressed, application/x-gtar, multipart/x-gzip, application/tgz, application/gnutar, application/x-tar, application/x-xliff+xml,  application/msword.docx, application/pdf, application/x-pdf, application/octetstream, application/x-ttx, application/x-tmx, application/octet-stream, application/vnd.openxmlformats-officedocument.wordprocessingml.document, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
			Driver = new FirefoxDriver(profile);
			ProcessNames = new[] { "firefox", "conhost" };
		}
	}
}
