using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using NLog;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Windows.Forms;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	/// <summary>
	/// Дополнительные методы для драйвера
	/// </summary>
	public static class WebDriverExtensions
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();
		public static readonly TimeSpan ImplicitWait = new TimeSpan(0, 0, 0, 5);
		public static readonly TimeSpan NoWait = new TimeSpan(0, 0, 0, 0);

		/// <summary>
		/// Проверить, присуствует ли элемент на странице
		/// </summary>
		/// <param name="driver">драйвер</param>
		/// <param name="by">локатор</param>
		public static bool ElementIsPresent(this IWebDriver driver, By by)
		{
			var present = false;
			driver.Manage().Timeouts().ImplicitlyWait(NoWait);

			try
			{
				present = driver.FindElement(by).Displayed;
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
				driver.Navigate().Refresh();
			}

			driver.Manage().Timeouts().ImplicitlyWait(ImplicitWait);

			return present;
		}

		/// <summary>
		/// Ждем, отобразиться ли элемент
		/// </summary>
		/// <param name="driver">драйвер</param>
		/// <param name="by">локатор</param>
		/// <param name="timeout">время ожидания</param>
		public static bool WaitUntilElementIsPresent(this IWebDriver driver, By by, int timeout = 10)
		{
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));

			try
			{
				return wait.Until(d => d.ElementIsPresent(by));
			}
			catch (WebDriverTimeoutException)
			{
				return false;
			}
			catch (StaleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: WaitUntilElementIsPresent: " + by);

				return WaitUntilElementIsPresent(driver, by, timeout);
			}
		}

		/// <summary>
		/// Ждем, пропадет ли элемент
		/// </summary>
		/// <param name="driver">драйвер</param>
		/// <param name="by">локатор</param>
		/// <param name="timeout">время ожидания</param>
		public static bool WaitUntilElementIsDissapeared(this IWebDriver driver, By by, int timeout = 10)
		{
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));

			try
			{
				return wait.Until(d => !d.ElementIsPresent(by));
			}
			catch (WebDriverTimeoutException)
			{
				return false;
			}
			catch (StaleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: WaitUntilElementIsDissapeared: " + by);

				return WaitUntilElementIsDissapeared(driver, by, timeout);
			}
		}

		/// <summary>
		/// Проверить, доступен ли элемент на странице
		/// </summary>
		/// <param name="driver">драйвер</param>
		/// <param name="by">локатор</param>
		public static bool ElementIsEnabled(this IWebDriver driver, By by)
		{
			var enabled = false;
			driver.Manage().Timeouts().ImplicitlyWait(NoWait);

			try
			{
				enabled = driver.FindElement(by).Enabled;
			}
			catch (NoSuchElementException)
			{
			}
			
			driver.Manage().Timeouts().ImplicitlyWait(ImplicitWait);
			
			return enabled;
		}

		/// <summary>
		/// Ждем, станет ли элемент доступен
		/// </summary>
		/// <param name="driver">драйвер</param>
		/// <param name="by">локатор</param>
		/// <param name="timeout">время ожидания</param>
		public static bool WaitUntilElementIsEnabled(this IWebDriver driver, By by, int timeout = 10)
		{
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));

			try
			{
				return wait.Until(d => d.ElementIsEnabled(by));
			}
			catch (WebDriverTimeoutException)
			{
				
				return false;
			}
			catch (StaleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: WaitUntilElementIsEnabled: " + by);
				
				return WaitUntilElementIsEnabled(driver, by, timeout);
			}
		}

		/// <summary>
		/// Вернуть список эелементов
		/// </summary>
		/// <param name="driver">драйвер</param>
		/// <param name="by">локатор</param>
		public static IList<IWebElement> GetElementList(this IWebDriver driver, By by)
		{
			try
			{
				return driver.FindElements(by);
			}
			catch (StaleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetElementList: " + @by);

				return GetElementList(driver, by);
			}
			catch (Exception ex)
			{
				Assert.Fail("Произошла ошибка:\n" + ex.Message);
			}
			
			return null;
		}

		/// <summary>
		/// Исполнить JavaScript
		/// </summary>
		/// <param name="driver">драйвер</param>
		public static IJavaScriptExecutor Scripts(this IWebDriver driver)
		{
			return (IJavaScriptExecutor)driver;
		}

		/// <summary>
		/// Делаем скриншот
		/// </summary>
		/// <param name="driver">драйвер</param>
		/// <param name="prefix">префикс файла, к нему добавляем дату и расширение</param>
		public static string TakeScreenshot(this IWebDriver driver, string prefix)
		{
			var fileName = String.Format("{0}{1}{2}", prefix, DateTime.Now.ToString("HHmmss"), ".png");
			var screenShot = ((ITakesScreenshot)driver).GetScreenshot();
			screenShot.SaveAsFile(fileName, ImageFormat.Png);
			
			return fileName;
		}

		/// <summary>
		/// Присваиваем значение элементу с динамическим локатором
		/// </summary>
		/// <param name="driver">драйвер</param>
		/// <param name="how">как ищем (Xpath и т.д.)</param>
		/// <param name="locator">динамический локатор</param>
		/// <param name="value">значение, на которое меняем часть локатора</param>
		/// <param name="value2">второе значение, на которое меняем часть локатора</param>
		public static IWebElement SetDynamicValue(this IWebDriver driver, How how, string locator, string value, string value2 = "")
		{
			IWebElement webElement = null;

			try
			{
				switch (how)
				{
					case How.XPath:
						webElement = driver.FindElement(By.XPath(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.CssSelector:
						webElement = driver.FindElement(By.CssSelector(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.Id:
						webElement = driver.FindElement(By.Id(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.ClassName:
						webElement = driver.FindElement(By.ClassName(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.LinkText:
						webElement = driver.FindElement(By.LinkText(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.PartialLinkText:
						webElement = driver.FindElement(By.PartialLinkText(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.Name:
						webElement = driver.FindElement(By.Name(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.TagName:
						webElement = driver.FindElement(By.TagName(locator.Replace("*#*", value).Replace("*##*", value2)));
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
				return SetDynamicValue(driver, how, locator, value);
			}
			catch (Exception ex)
			{
				Assert.Fail("Ошибка: " + ex.Message + "How " +
					how + " Using " + locator.Replace("*#*", value).Replace("*##*", value2));
			}

			return webElement;
		}
	}
}
