using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using NLog;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Drivers
{
	public class WebDriver : IWebDriver
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();

		public WebDriver(IWebDriverProvider provider, string tempFolder)
		{
			_tempFolder = Path.Combine(tempFolder, Guid.NewGuid().ToString());
			_driver = provider.GetWebDriver(_tempFolder);

			_driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(15));
			_driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(60));
			_driver.Manage().Window.Maximize();
			
			Logger.Info("Браузер {0}", _driver.Capabilities.BrowserName);
			Logger.Info("Версия {0}", _driver.Capabilities.Version);
			Logger.Info("Платформа {0}", _driver.Capabilities.Platform);
			Logger.Info("JavaScript enabled {0}", _driver.Capabilities.IsJavaScriptEnabled);

			Logger.Info("Браузер создан");
		}


		#region Реализация интерфейса IWebDriver

		public string CurrentWindowHandle
		{
			get { return _driver.CurrentWindowHandle; }
		}

		public string PageSource
		{
			get { return _driver.PageSource; }
		}

		public string Title
		{
			get { return _driver.Title; }
		}

		public string Url
		{
			get { return _driver.Url; }
			set { _driver.Url = value; }
		}

		public ReadOnlyCollection<string> WindowHandles
		{
			get { return _driver.WindowHandles; }
		}

		public IWebElement FindElement(By by)
		{
			return _driver.FindElement(by);
		}

		public ReadOnlyCollection<IWebElement> FindElements(By by)
		{
			return _driver.FindElements(by);
		}

		public void Close()
		{
			_driver.Close();
		}

		public IOptions Manage()
		{
			return _driver.Manage();
		}

		public INavigation Navigate()
		{
			return _driver.Navigate();
		}

		public void Quit()
		{
			_driver.Quit();
		}

		public ITargetLocator SwitchTo()
		{
			return _driver.SwitchTo();
		}

		#endregion


		public object ExecuteScript(string script, params object[] args)
		{
			return ((IJavaScriptExecutor)_driver).ExecuteScript(script, args);
		}

		/// <summary>
		/// Делаем скриншот
		/// </summary>
		/// <param name="path">путь к папке со скриншотами</param>
		public string TakeScreenshot(string path)
		{
			Directory.CreateDirectory(path);

			var nameParts = TestContext.CurrentContext.Test.FullName.Split('.');
			var className = nameParts[nameParts.Length - 2].Replace('<', '(').Replace('>', ')');

			var screenName = TestContext.CurrentContext.Test.Name;
			if (screenName.Contains("("))
			{
				// Убрать из названия теста аргументы (файлы)
				screenName = screenName.Substring(0, screenName.IndexOf("(", StringComparison.InvariantCulture));
			}

			var fileName = String.Format("{0} {1}{2}",
				Path.Combine(path, className + "." + screenName), DateTime.Now.ToString("yyyy.MM.dd HH.mm.ss"), ".png");
			Screenshot screenShot;

			try
			{
				screenShot = ((ITakesScreenshot)_driver).GetScreenshot();
			}
			catch (UnhandledAlertException)
			{
				var alert = _driver.SwitchTo().Alert();
				Logger.Error("Ошибка: необработанный алерт. Текст алерта: {0}.", alert.Text);
				alert.Accept();
				screenShot = ((ITakesScreenshot)_driver).GetScreenshot();
			}

			screenShot.SaveAsFile(fileName, ImageFormat.Png);

			return fileName;
		}

		public void Dispose()
		{
			_driver.Quit();
			_driver.Dispose();

			try
			{
				if (_tempFolder != null && Directory.Exists(_tempFolder))
				{
					Directory.Delete(_tempFolder, true);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Ошибка. Не удалось удалить временную папку {0}. Причина: {1}", _tempFolder, ex);
			}

			GC.SuppressFinalize(this);

			Logger.Info("Браузер остановлен");
		}

		~WebDriver()
		{
			Dispose();
		}

		private readonly RemoteWebDriver _driver;
		private readonly string _tempFolder;
	}
}
