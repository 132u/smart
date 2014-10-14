using System;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace AbbyyLS.PEMT.Function.Selenium.Tests
{
	public class CommonHelper
	{
		private IWebDriver _driver;
		protected IWebDriver Driver
		{
			get
			{
				return _driver;
			}
		}
		private WebDriverWait _wait;
		protected WebDriverWait Wait
		{
			get
			{
				return _wait;
			}
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="driver"></param>
		/// <param name="wait"></param>
		public CommonHelper(IWebDriver driver, WebDriverWait wait)
		{
			_driver = driver;
			_wait = wait;
		}

		/// <summary>
		/// Обновить страницу
		/// </summary>
		protected void RefreshPage()
		{
			Driver.Navigate().Refresh();
		}

		/// <summary>
		/// Дождаться, пока появится элемент
		/// </summary>
		/// <param name="by">by</param>
		/// <returns>появился элемент</returns>
		protected bool WaitUntilDisplayElement(By by, int maxWait = 10)
		{
			try
			{
				_wait.Until((d) => d.FindElement(by).Displayed);
				return true;
			}
			catch (StaleElementReferenceException)
			{
				Console.WriteLine("StaleElementReferenceException: WaitUntilDisplayElement: " + by.ToString());
				return WaitUntilDisplayElement(by, maxWait);
			}
			catch (WebDriverTimeoutException)
			{
				return false;
			}
			catch (Exception exType)
			{
				Assert.Fail("Произошла ошибка:\n" + exType.ToString());
				return false;
			}
		}

		/// <summary>
		/// Вернуть, есть ли элемент
		/// </summary>
		/// <param name="by">by</param>
		/// <returns>есть элемент</returns>
		protected bool GetIsElementExist(By by)
		{
			try
			{
				Driver.FindElement(by);
				return true;
			}
			catch (StaleElementReferenceException)
			{
				Console.WriteLine("StaleElementReferenceException: GetIsElementExist: " + by.ToString());
				return GetIsElementExist(by);
			}
			catch (NoSuchElementException)
			{
				return false;
			}
			catch (Exception exType)
			{
				Assert.Fail("Произошла ошибка:\n" + exType.ToString());
				return false;
			}
		}

		/// <summary>
		/// Вернуть, показан ли элемент
		/// </summary>
		/// <param name="by">by</param>
		/// <returns>показан</returns>
		protected bool GetIsElementDisplay(By by)
		{
			try
			{
				return Driver.FindElement(by).Displayed;
			}
			catch (StaleElementReferenceException)
			{
				Console.WriteLine("StaleElementReferenceException: GetIsElementDisplay: " + by.ToString());
				return GetIsElementDisplay(by);
			}
			catch (NoSuchElementException)
			{
				return false;
			}
			catch (Exception exType)
			{
				Assert.Fail("Произошла ошибка:\n" + exType.ToString());
				return false;
			}
		}

		/// <summary>
		/// Кликнуть по элементу
		/// </summary>
		/// <param name="by">by</param>
		protected void ClickElement(By by)
		{
			try
			{
				Driver.FindElement(by).Click();
			}
			catch (StaleElementReferenceException)
			{
				Console.WriteLine("StaleElementReferenceException: ClickElement: " + by.ToString());
				ClickElement(by);
			}
			catch (NoSuchElementException)
			{
				Assert.Fail("Элемент не найден!\n" + by.ToString());
			}
			catch (Exception exType)
			{
				Assert.Fail("Произошла ошибка:\n" + exType.ToString() + "\n\n" + by.ToString());
			}
		}

		/// <summary>
		/// Очистить элемент и ввести текст
		/// </summary>
		/// <param name="by">by</param>
		/// <param name="text">текст</param>
		protected void ClearAndAddText(By by, string text)
		{
			string txt = Regex.Replace(text, "[+^%~()]", "{$0}");
			
			try
			{
				IWebElement el = Driver.FindElement(by);
				el.Clear();
				el.SendKeys(txt);
			}
			catch (StaleElementReferenceException)
			{
				Console.WriteLine("StaleElementReferenceException: ClearAndAddText: " + by.ToString());
				ClearAndAddText(by, text);
			}
			catch (NoSuchElementException)
			{
				Assert.Fail("Элемент не найден!\n" + by.ToString());
			}
			catch (Exception exType)
			{
				Assert.Fail("Произошла ошибка:\n" + exType.ToString());
			}
		}

		/// <summary>
		/// Очистить элемент
		/// </summary>
		/// <param name="by">by</param>
		protected void ClearElement(By by)
		{
			try
			{
				IWebElement el = Driver.FindElement(by);
				el.Clear();
			}
			catch (StaleElementReferenceException)
			{
				Console.WriteLine("StaleElementReferenceException: ClearElement: " + by.ToString());
				ClearElement(by);
			}
			catch (NoSuchElementException)
			{
				Assert.Fail("Элемент не найден!\n" + by.ToString());
			}
			catch (Exception exType)
			{
				Assert.Fail("Произошла ошибка:\n" + exType.ToString());
			}
		}

		/// <summary>
		/// Отправить текст элементу
		/// </summary>
		/// <param name="by">by</param>
		/// <param name="text">текст</param>
		protected void SendTextElement(By by, string text)
		{
			string txt = Regex.Replace(text, "[+^%~()]", "{$0}");
			
			try
			{
				Driver.FindElement(by).SendKeys(txt);
			}
			catch (StaleElementReferenceException)
			{
				Console.WriteLine("StaleElementReferenceException: SendTextElement: " + by.ToString());
				SendTextElement(by, text);
			}
			catch (NoSuchElementException)
			{
				Assert.Fail("Элемент не найден!\n" + by.ToString());
			}
			catch (Exception exType)
			{
				Assert.Fail("Произошла ошибка:\n" + exType.ToString());
			}
		}

		/// <summary>
		/// Вернуть текст элемента
		/// </summary>
		/// <param name="by">by</param>
		/// <returns>текст</returns>
		protected string GetTextElement(By by)
		{
			try
			{
				return Driver.FindElement(by).Text;
			}
			catch (StaleElementReferenceException)
			{
				Console.WriteLine("StaleElementReferenceException: GetTextElement: " + by.ToString());
				return GetTextElement(by);
			}
			catch (NoSuchElementException)
			{
				Assert.Fail("Элемент не найден!\n" + by.ToString());
				return "";
			}
			catch (Exception exType)
			{
				Assert.Fail("Произошла ошибка:\n" + exType.ToString());
				return "";
			}
		}
		
		/// <summary>
		/// Вернуть количество элементов
		/// </summary>
		/// <param name="by">by</param>
		/// <returns>количество</returns>
		protected int GetElementsCount(By by)
		{
			try
			{
				return Driver.FindElements(by).Count;
			}
			catch (StaleElementReferenceException)
			{
				Console.WriteLine("StaleElementReferenceException: GetElementCount: " + by.ToString());
				return GetElementsCount(by);
			}
			catch (Exception exType)
			{
				Assert.Fail("Произошла ошибка:\n" + exType.ToString());
				return 0;
			}
		}

		/// <summary>
		/// Уменьшить таймаут драйвера
		/// </summary>
		protected void SetDriverTimeoutMinimum()
		{
			Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));
		}

		/// <summary>
		/// Изменить таймаут драйвера на значение по умолчанию
		/// </summary>
		protected void SetDriverTimeoutDefault()
		{
			Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
		}
	}
}
