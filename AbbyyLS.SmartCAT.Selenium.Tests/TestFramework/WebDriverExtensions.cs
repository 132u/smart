using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	/// <summary>
	/// Дополнительные методы для драйвера
	/// </summary>
	public static class WebDriverExtensions
	{
		public static readonly TimeSpan ImplicitWait = new TimeSpan(0, 0, 0, 5);
		public static readonly TimeSpan NoWait = new TimeSpan(0, 0, 0, 0);

		/// <summary>
		/// Проверяем, присуствует ли элемент на странице
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
				Console.WriteLine("StaleElementReferenceException: WaitUntilElementIsPresent: " + by);

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
				Console.WriteLine("StaleElementReferenceException: WaitUntilElementIsDissapeared: " + by);

				return WaitUntilElementIsDissapeared(driver, by, timeout);
			}
		}

		/// <summary>
		/// Проверяем, доступен ли элемент на странице
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
				Console.WriteLine("StaleElementReferenceException: WaitUntilElementIsEnabled: " + by);
				
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
				Console.WriteLine("StaleElementReferenceException: GetElementList: " + @by);
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
	}
}
