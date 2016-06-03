using System;
using System.IO;
using NLog;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Drivers
{
	public class ChromeDriverProvider : IWebDriverProvider
	{
		public static Logger Log = LogManager.GetCurrentClassLogger();

		public RemoteWebDriver GetWebDriver(string tempFolder, string downloadDirectory, string userDataDirectory)
		{
			var defaultDriverPath = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chromedriver.exe")).LocalPath;
			var driverCopyPath = new Uri(Path.Combine(tempFolder, "chromedriver.exe")).LocalPath;

			if (!File.Exists(defaultDriverPath))
			{
				throw new FileNotFoundException("Вебдрайвер хрома не найден", defaultDriverPath);
			}

			Directory.CreateDirectory(tempFolder);
			Directory.CreateDirectory(downloadDirectory);
			Directory.CreateDirectory(userDataDirectory);

			if (!File.Exists(driverCopyPath))
			{
				File.Copy(defaultDriverPath, driverCopyPath);
			}

			var options = new ChromeOptions();
			options.AddArgument("--enable-file-cookies");
			options.AddArguments("--lang=en");
			options.AddArguments(String.Format("--user-data-dir={0}", userDataDirectory));
			options.AddUserProfilePreference("download.default_directory", downloadDirectory);
			options.AddUserProfilePreference("safebrowsing.enabled", "true");
			Log.Info("Настройки браузера {0}", String.Join(" ", options.Arguments));
			Log.Info("Путь к папке для скаченных файлов {0}", downloadDirectory);
			
			return new ChromeDriver(tempFolder, options);
		}
	}
}
