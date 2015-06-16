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

			return new ChromeDriver(tempFolder, options);
		}
	}
}
