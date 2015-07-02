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

		private string _driverCopyPath;

		public RemoteWebDriver GetWebDriver(string tempFolder, string downloadDirectory)
		{
			var defaultDriverPath = Path.Combine(Directory.GetCurrentDirectory(), "chromedriver.exe");

			if (!File.Exists(defaultDriverPath))
			{
				throw new FileNotFoundException("Вебдрайвер хрома не найден", defaultDriverPath);
			}

			Directory.CreateDirectory(tempFolder);

			_driverCopyPath = Path.Combine(tempFolder, Path.GetFileName(defaultDriverPath));
			File.Copy(defaultDriverPath, _driverCopyPath);

			var options = new ChromeOptions();
			options.AddArguments("--lang=en");
			options.AddUserProfilePreference("download.default_directory", downloadDirectory);
			Log.Info("Настройки браузера {0}", String.Join(" ", options.Arguments));

			return new ChromeDriver(tempFolder, options);
		}
	}
}
