using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NLog;
using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Drivers
{
	public class WebDriver : IWebDriver
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();
		public static readonly TimeSpan ImplicitWait = new TimeSpan(0, 0, 0, 5);
		public static readonly TimeSpan NoWait = new TimeSpan(0, 0, 0, 0);
		private Navigation _customNavigate;

		public WebDriver(IWebDriverProvider provider, string tempFolder, string downloadDirectory)
		{
			_tempFolder = Path.Combine(tempFolder, Guid.NewGuid().ToString());
			_driver = provider.GetWebDriver(_tempFolder, downloadDirectory);

			_driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(15));
			_driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(60));
			_driver.Manage().Window.Maximize();
			
			_customNavigate = new Navigation(_driver);
			
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
			IWebElement element;

			try
			{
				element = _driver.FindElement(by);
			}
			catch (StaleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: Не удалось найти элемент {0}. Предпринять повторную попытку поиска.", by);
				element = _driver.FindElement(by);
			}

			return element;
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
			return _customNavigate;
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
		/// Проверить, доступен ли элемент на странице
		/// </summary>
		/// <param name="by">локатор</param>
		public bool ElementIsEnabled(By by)
		{
			var enabled = false;
			_driver.Manage().Timeouts().ImplicitlyWait(NoWait);

			try
			{
				enabled = _driver.FindElement(by).Enabled;
			}
			catch (NoSuchElementException)
			{
			}

			_driver.Manage().Timeouts().ImplicitlyWait(ImplicitWait);

			return enabled;
		}

		/// <summary>
		/// Ждем, станет ли элемент доступен
		/// </summary>
		/// <param name="by">локатор</param>
		/// <param name="timeout">время ожидания</param>
		public bool WaitUntilElementIsEnabled(By by, int timeout = 10)
		{
			var wait = new WebDriverWait(this, TimeSpan.FromSeconds(timeout));

			try
			{
				return wait.Until(d => ((WebDriver)d).ElementIsEnabled(by));
			}
			catch (WebDriverTimeoutException)
			{
				return false;
			}
			catch (StaleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: WaitUntilElementIsEnabled: " + by);

				return WaitUntilElementIsEnabled(by, timeout);
			}
		}

		/// <summary>
		/// Проверить, присуствует ли элемент на странице
		/// </summary>
		/// <param name="by">локатор</param>
		public bool ElementIsDisplayed(By by)
		{
			var present = false;
			_driver.Manage().Timeouts().ImplicitlyWait(NoWait);

			try
			{
				present = _driver.FindElement(by).Displayed;
			}
			catch (NoSuchElementException)
			{
			}
			catch (InvalidOperationException exception)
			{
				// Exception является багой возникающей в Selenium 2.43.1
				// https://code.google.com/p/selenium/issues/detail?id=7977

				Logger.Warn("InvalidOperationException: ElementIsPresent {0}", exception.Message);
				Logger.Debug("Обновить страницу браузера.");
				_driver.Navigate().Refresh();
			}

			_driver.Manage().Timeouts().ImplicitlyWait(ImplicitWait);

			return present;
		}

		/// <summary>
		/// Ждем, отобразится ли элемент
		/// </summary>
		/// <param name="by">локатор</param>
		/// <param name="timeout">время ожидания</param>
		public bool WaitUntilElementIsDisplay(By by, int timeout = 10)
		{
			var wait = new WebDriverWait(this, TimeSpan.FromSeconds(timeout));

			try
			{
				return wait.Until(d => ((WebDriver)d).ElementIsDisplayed(by));
			}
			catch (WebDriverTimeoutException)
			{
				return false;
			}
			catch (StaleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: WaitUntilElementIsDisplayed: " + by);

				return WaitUntilElementIsDisplay(by, timeout);
			}
		}

		/// <summary>
		/// Ждем, пропадет ли элемент
		/// </summary>
		/// <param name="by">локатор</param>
		/// <param name="timeout">время ожидания</param>
		public bool WaitUntilElementIsDisappeared(By by, int timeout = 10)
		{
			var wait = new WebDriverWait(this, TimeSpan.FromSeconds(timeout));

			try
			{
				return wait.Until(d => !((WebDriver)d).ElementIsDisplayed(by));
			}
			catch (WebDriverTimeoutException)
			{
				return false;
			}
			catch (StaleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: WaitUntilElementIsDisappeared: " + by);

				return WaitUntilElementIsDisappeared(by, timeout);
			}
		}

		/// <summary>
		/// Вернуть список эелементов
		/// </summary>
		/// <param name="by">локатор</param>
		public IList<IWebElement> GetElementList(By by)
		{
			try
			{
				return _driver.FindElements(by);
			}
			catch (StaleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetElementList: " + @by);

				return GetElementList(by);
			}
			catch (Exception ex)
			{
				Assert.Fail("Произошла ошибка:\n" + ex.Message);
			}

			return null;
		}

		/// <summary>
		/// Получить список текстов из элементов
		/// </summary>
		/// <param name="by">локатор</param>
		/// <returns>список текстов</returns>
		public List<string> GetTextListElement(By by)
		{
			Logger.Trace("Вернуть список текстов элементов");

			try
			{
				var elList = GetElementList(by);

				return elList.Select(el => el.Text).ToList();
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetTextListElement: " + by.ToString(), staleElementReferenceException);
				return GetTextListElement(by);
			}
		}

		/// <summary>
		/// Вернуть количество элементов
		/// </summary>
		/// <param name="by">локатор</param>
		public int GetElementsCount(By by)
		{
			try
			{
				return _driver.FindElements(by).Count;
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetElementsCount: " + by.ToString(), staleElementReferenceException);
				return GetElementsCount(by);
			}
		}

		/// <summary>
		/// Присваиваем значение элементу с динамическим локатором
		/// </summary>
		/// <param name="how">как ищем (Xpath и т.д.)</param>
		/// <param name="locator">динамический локатор</param>
		/// <param name="value">значение, на которое меняем часть локатора</param>
		/// <param name="value2">второе значение, на которое меняем часть локатора</param>
		public IWebElement SetDynamicValue(How how, string locator, string value, string value2 = "")
		{
			IWebElement webElement = null;

			try
			{
				switch (how)
				{
					case How.XPath:
						webElement = _driver.FindElement(By.XPath(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.CssSelector:
						webElement = _driver.FindElement(By.CssSelector(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.Id:
						webElement = _driver.FindElement(By.Id(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.ClassName:
						webElement = _driver.FindElement(By.ClassName(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.LinkText:
						webElement = _driver.FindElement(By.LinkText(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.PartialLinkText:
						webElement = _driver.FindElement(By.PartialLinkText(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.Name:
						webElement = _driver.FindElement(By.Name(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.TagName:
						webElement = _driver.FindElement(By.TagName(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					default:
						throw new Exception(
							"Нельзя определить как искать элемент " + how);
				}
			}
			catch (NoSuchElementException)
			{
				Assert.Fail("Ошибка: не удалось найти элемент How " +
					how + " Using " + locator.Replace("*#*", value).Replace("*##*", value2));
			}
			catch (StaleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetIsElementDisplay: " + "How " +
					how + " Using " + locator.Replace("*#*", value).Replace("*##*", value2));
				return SetDynamicValue(how, locator, value);
			}
			catch (Exception ex)
			{
				Assert.Fail("Ошибка: " + ex.Message + "How " +
					how + " Using " + locator.Replace("*#*", value).Replace("*##*", value2));
			}

			return webElement;
		}

		/// <summary>
		/// Перейти в IFrame
		/// </summary>
		public void SwitchToIFrame(By by)
		{
			var frame = _driver.FindElement(by);
			_driver.SwitchTo().Frame(frame);
		}

		/// <summary>
		/// Выйти из iframe 
		/// </summary>
		public void SwitchToDefaultContent()
		{
			_driver.SwitchTo().DefaultContent();
		}

		/// <summary>
		/// Вернуть, существует ли элемент
		/// </summary>
		public bool GetIsElementExist(By by)
		{
			try
			{
				_driver.FindElement(by);
				return true;
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetIsElementExist: " + by.ToString(), staleElementReferenceException);
				return GetIsElementExist(by);
			}
			catch (NoSuchElementException)
			{
				Logger.Trace("NoSuchElementException: GetIsElementExist: " + by.ToString());
				return false;
			}
		}

		/// <summary>
		/// Делаем скриншот
		/// </summary>
		/// <param name="path">путь к папке со скриншотами</param>
		public string TakeScreenshot(string path)
		{
			Directory.CreateDirectory(path);

			var nameParts = TestContext.CurrentContext.Test.FullName.Split('.');

			var className = nameParts[6].Replace('<', '(').Replace('>', ')');
			

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
				CustomTestContext.WriteLine("Ошибка: необработанный алерт. Текст алерта: {0}.", alert.Text);
				alert.Accept();
				screenShot = ((ITakesScreenshot)_driver).GetScreenshot();
			}

			screenShot.SaveAsFile(fileName, ImageFormat.Png);

			return fileName;
		}

		/// <summary>
		/// Открыть новую вкладку, удалив старую и почистив cookies
		/// </summary>
		public void SwitchToNewTab()
		{
			_driver.Manage().Cookies.DeleteAllCookies();
			// Открываем новую вкладку для следующего теста
			_driver.ExecuteScript("window.open()");

			try
			{
				// Закрываем текущую вкладку
				_driver.Close();
			}
			catch (UnhandledAlertException)
			{
				var alert = _driver.SwitchTo().Alert();
				CustomTestContext.WriteLine("Ошибка: необработанный алерт. Текст алерта: {0}.", alert.Text);
				alert.Accept();
			}

			// Переключаемся на только что открытую вкладку
			_driver.SwitchTo().Window(_driver.WindowHandles.Last());
		}

		/// <summary>
		/// Дождаться, пока страница полностью загрузится
		/// </summary>
		/// <param name="maxWait">таймаут</param>
		/// <returns>загрузится ли страница</returns>
		public bool WaitPageTotalLoad(int maxWait = 15)
		{
			var wait = new WebDriverWait(_driver, ImplicitWait);
			var timeout = wait.Timeout.Seconds;
			try
			{
				wait.Timeout = TimeSpan.FromSeconds(maxWait);
				wait.Until(d => ((IJavaScriptExecutor)_driver)
					.ExecuteScript("return document.readyState")
					.Equals("complete"));
				return true;
			}
			catch (WebDriverTimeoutException)
			{
				Logger.Trace("Не удалось дождаться загрузки страницы.");
				return false;
			}
			catch (Exception ex)
			{
				Logger.Trace("Во время ожидания загрузки страницы произошла ошибка:" + ex.Message);
				return false;
			}
			finally
			{
				wait.Timeout = TimeSpan.FromSeconds(timeout);
			}
		}

		public void SendHotKeys(string key, bool control = false, bool shift = false, bool alt = false)
		{
			var actions = new Actions(_driver);

			if (control)
			{
				actions.KeyDown(Keys.Control);
			}
			if (shift)
			{
				actions.KeyDown(Keys.Shift);
			}
			if (alt)
			{
				actions.KeyDown(Keys.Alt);
			}

			Thread.Sleep(1000);
			actions.SendKeys(key);

			if (control)
			{
				actions.KeyUp(Keys.Control);
			}
			if (shift)
			{
				actions.KeyUp(Keys.Shift);
			}
			if (alt)
			{
				actions.KeyUp(Keys.Alt);
			}

			actions.Build().Perform();
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
