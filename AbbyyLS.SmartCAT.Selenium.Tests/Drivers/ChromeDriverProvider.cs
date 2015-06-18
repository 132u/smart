using System;
using System.IO;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Drivers
{
	public class ChromeDriverProvider : IWebDriverProvider
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();

		private string _driverCopyPath;

		public IWebDriver GetWebDriver(string tempFolder)
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

			var driver = new ChromeDriver(tempFolder, options);

			Logger.Info("Браузер {0}", driver.Capabilities.BrowserName);
			Logger.Info("Ключи {0}", String.Join(" ", options.Arguments));
			Logger.Info("Версия {0}", driver.Capabilities.Version);
			Logger.Info("Платформа {0}", driver.Capabilities.Platform);
			Logger.Info("JavaScript enabled {0}", driver.Capabilities.IsJavaScriptEnabled);

			return driver;
		}
	}
}
