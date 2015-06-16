using System;
using System.Collections.ObjectModel;
using System.IO;
using NLog;
using OpenQA.Selenium;

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

			Logger.Info("Браузер создан");
		}


		#region Проксирование действий над драйвером

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

		private readonly IWebDriver _driver;
		private readonly string _tempFolder;
	}
}
