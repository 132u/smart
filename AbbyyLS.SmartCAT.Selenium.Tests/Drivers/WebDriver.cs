using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Drivers
{
	public class WebDriver : IWebDriver
	{
		public static readonly TimeSpan ImplicitWait = new TimeSpan(0, 0, 0, 3);
		public static readonly TimeSpan NoWait = new TimeSpan(0, 0, 0, 0);
		private Navigation _customNavigate;

		public WebDriver(IWebDriverProvider provider, string tempFolder, string downloadDirectory)
		{
			_tempFolder = Path.Combine(tempFolder, Guid.NewGuid().ToString());
			_driver = provider.GetWebDriver(_tempFolder, downloadDirectory);

			_driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));
			_driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(60));
			_driver.Manage().Window.Maximize();
			
			_customNavigate = new Navigation(_driver);
			
			CustomTestContext.WriteLine("Браузер {0}", _driver.Capabilities.BrowserName);
			CustomTestContext.WriteLine("Версия {0}", _driver.Capabilities.Version);
			CustomTestContext.WriteLine("Платформа {0}", _driver.Capabilities.Platform);
			CustomTestContext.WriteLine("JavaScript enabled {0}", _driver.Capabilities.IsJavaScriptEnabled);

			CustomTestContext.WriteLine("Браузер создан");
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

		/// <summary>
		/// Выполнить JavaScript
		/// </summary>
		/// <param name="script">script</param>
		/// <param name="args">аргументы</param>
		public object ExecuteScript(string script, params object[] args)
		{
			return ((IJavaScriptExecutor)_driver).ExecuteScript(script, args);
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
				return wait.Until(d => ((WebDriver)d).FindElement(by).Enabled);
			}
			catch (WebDriverTimeoutException)
			{
				return false;
			}
		}

		/// <summary>
		/// Проверить, присуствует ли элемент на странице
		/// </summary>
		/// <param name="by">локатор</param>
		public bool ElementIsDisplayed(By by)
		{
			var wait = new WebDriverWait(this, TimeSpan.FromSeconds(0));

			try
			{
				return wait.Until(d => ((WebDriver)d).FindElement(by).Displayed);
			}
			catch (WebDriverTimeoutException)
			{
				return false;
			}
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
				return wait.Until(d => ((WebDriver)d).FindElement(by).Displayed);
			}
			catch (WebDriverTimeoutException)
			{
				return false;
			}
		}

		/// <summary>
		/// Ждем, появится ли элемент в DOM
		/// </summary>
		/// <param name="by">локатор</param>
		/// <param name="timeout">время ожидания</param>
		public bool WaitUntilElementIsAppear(By by, int timeout = 10)
		{
			var wait = new WebDriverWait(this, TimeSpan.FromSeconds(timeout));

			try
			{
				wait.Until(d => d.FindElement(by));
				_driver.Manage().Timeouts().ImplicitlyWait(ImplicitWait);
				return true;
			}
			catch (WebDriverTimeoutException)
			{
				_driver.Manage().Timeouts().ImplicitlyWait(ImplicitWait);
				return false;
			}
		}

		/// <summary>
		/// Ждем, пропадет ли элемент
		/// </summary>
		/// <param name="by">локатор</param>
		/// <param name="timeout">время ожидания</param>
		public bool WaitUntilElementIsDisappeared(By by, int timeout = 10)
		{
			_driver.Manage().Timeouts().ImplicitlyWait(NoWait);
			var wait = new WebDriverWait(this, TimeSpan.FromSeconds(timeout));

			try
			{
				var result = wait.Until(ExpectedConditions.InvisibilityOfElementLocated(by));
				_driver.Manage().Timeouts().ImplicitlyWait(ImplicitWait);
				return result;
			}
			catch (WebDriverTimeoutException)
			{
				_driver.Manage().Timeouts().ImplicitlyWait(ImplicitWait);
				return false;
			}
		}

		/// <summary>
		/// Ждем, когда элемент станет кликабельным
		/// </summary>
		/// <param name="by">локатор</param>
		/// <param name="timeout">время ожидания</param>
		public IWebElement WaitUntilElementIsClickable(By by, int timeout = 10)
		{
			var wait = new WebDriverWait(this, TimeSpan.FromSeconds(timeout));

			try
			{
				return wait.Until(ExpectedConditions.ElementToBeClickable(by));
			}
			catch (WebDriverTimeoutException)
			{
				return null;
			}
		}


		/// <summary>
		/// Вернуть список эелементов
		/// </summary>
		/// <param name="by">локатор</param>
		public IList<IWebElement> GetElementList(By by)
		{
			_driver.Manage().Timeouts().ImplicitlyWait(NoWait);
			ReadOnlyCollection<IWebElement> elementsList = null;

			try
			{
				elementsList = _driver.FindElements(by);
			}
			catch (Exception ex)
			{
				Assert.Fail("Произошла ошибка:\n" + ex.Message);
			}

			_driver.Manage().Timeouts().ImplicitlyWait(ImplicitWait);

			return elementsList;
		}

		/// <summary>
		/// Получить список текстов из элементов
		/// </summary>
		/// <param name="by">локатор</param>
		/// <returns>список текстов</returns>
		public List<string> GetTextListElement(By by)
		{
			CustomTestContext.WriteLine("Вернуть список текстов элементов");

			var elList = GetElementList(by);

			return elList.Select(el => el.Text).ToList();
		}

		/// <summary>
		/// Вернуть количество элементов
		/// </summary>
		/// <param name="by">локатор</param>
		public int GetElementsCount(By by)
		{
			return _driver.FindElements(by).Count;
		}

		/// <summary>
		/// Проверить, существует ли элемент, содержащий заданный текст
		/// </summary>
		/// <param name="by">локатор</param>
		/// <param name="text">текст</param>
		public bool GetIsTextToBePresentInElementLocated(By by, string text)
		{
			var wait = new WebDriverWait(this, ImplicitWait);
			try
			{
				return wait.Until(ExpectedConditions.TextToBePresentInElementLocated(by, text));
			}
			catch (WebDriverTimeoutException)
			{
				return false;
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
			_driver.Manage().Timeouts().ImplicitlyWait(NoWait);

			try
			{
				_driver.FindElement(by);
				_driver.Manage().Timeouts().ImplicitlyWait(ImplicitWait);
				return true;
			}
			catch (NoSuchElementException)
			{
				_driver.Manage().Timeouts().ImplicitlyWait(ImplicitWait);
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

			CloseAlertIfExist();

			screenShot = ((ITakesScreenshot)_driver).GetScreenshot();

			screenShot.SaveAsFile(fileName, ImageFormat.Png);

			CustomTestContext.WriteLine("<img src=\"data:image/jpeg;base64," + screenShot.AsBase64EncodedString + "\"/>");

			return fileName;
		}

		/// <summary>
		/// Закрыть алерт, если он найден на странице
		/// </summary>
		public void CloseAlertIfExist()
		{
			try
			{
				var alert = _driver.SwitchTo().Alert();

				CustomTestContext.WriteLine("Произошла ошибка: появился необработанный Alert");
				CustomTestContext.WriteLine("Текст Alert'а: {0}.", alert.Text);
				alert.Accept();
			}
			catch (NoAlertPresentException)
			{
			}

			// Временная проверка. Есть подозрение, что алерт не всегда
			// закрывается с  помощью метода alert.Accept()
			if (ExpectedConditions.AlertIsPresent() == null)
			{
				CustomTestContext.WriteLine("Произошла ошибка: Alert не закрылся с помощью alert.Accept()");
			}
		}

		/// <summary>
		/// Открыть новую вкладку, удалив старую и почистив cookies
		/// </summary>
		public void SwitchToNewTab()
		{
			_driver.Manage().Cookies.DeleteAllCookies();
			// Открываем новую вкладку для следующего теста
			_driver.ExecuteScript("window.open()");
			// Закрываем текущую вкладку
			_driver.Close();
			// Переключаемся на только что открытую вкладку
			_driver.SwitchTo().Window(_driver.WindowHandles.Last());
		}

		/// <summary>
		/// Ожидает открытие новой вкладки браузера в течении трёх секунд
		/// </summary>
		/// <param name="timeout">время ожидания</param>
		public void SwitchToNewBrowserTab(int timeout = 3)
		{
			var timer = 1;
			// Лог необходим для отладки теста SCAT-900 AssignUserOneTaskTest
			CustomTestContext.WriteLine("Количество вкладок = {0}.", WindowHandles.Count);

			while (timer <= timeout)
			{
				Thread.Sleep(1000);
				timer++;

				if (WindowHandles.Count > 1)
				{
					// Лог необходим для отладки теста SCAT-900 AssignUserOneTaskTest
					CustomTestContext.WriteLine("Количество вкладок = {0}.", WindowHandles.Count);
					SwitchTo().Window(WindowHandles.First()).Close();

					SwitchTo().Window(WindowHandles.Last());

					break;
				}
			}

			if (timer > timeout)
			{
				throw new Exception("Произошла ошибка:\n время ожидания открытия новой вкладки истекло");
			}
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
				CustomTestContext.WriteLine("Не удалось дождаться загрузки страницы.");
				return false;
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Во время ожидания загрузки страницы произошла ошибка:" + ex.Message);
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
				CustomTestContext.WriteLine("Ошибка. Не удалось удалить временную папку {0}. Причина: {1}", _tempFolder, ex);
			}

			GC.SuppressFinalize(this);

			CustomTestContext.WriteLine("Браузер остановлен");
		}

		~WebDriver()
		{
			Dispose();
		}

		private readonly RemoteWebDriver _driver;
		private readonly string _tempFolder;
	}
}
