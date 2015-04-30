using System;
using NLog;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	/// <summary>
	///Дополнительные методы для элементов
	/// </summary>
	public static class WebElementExtensions
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Вводим текст, предварительно отчистив поле web-элемента
		/// </summary>
		/// <param name="webElement">элемент</param>
		/// <param name="text">текст</param>
		public static void SetText(this IWebElement webElement, string text)
		{

			webElement.Clear();

			try
			{
				webElement.SendKeys(text);
			}
			catch (ElementNotVisibleException exception)
			{
				Logger.Warn("ElementNotVisibleException: SendKeys {0}", exception.Message);
			}

			if (webElement.GetAttribute("value") != text)
			{
				webElement.SetText(text);
			}
		}

		/// <summary>
		/// Выбрать вариант из списка (использовать только для тэга Select!)
		/// </summary>
		/// <param name="webElement">элемент Select</param>
		/// <param name="text">текст</param>
		public static void SelectOptionByText(this IWebElement webElement, string text)
		{
			try
			{
				var select = new SelectElement(webElement);
				select.SelectByText(text);
			}
			catch (UnexpectedTagNameException)
			{

				Assert.Fail("Произошла ошибка:\n метод SelectOptionByText применен не к тэгу Select.");
			}
			catch (Exception)
			{
				Assert.Fail("Произошла ошибка:\n не удалось выбрать \"{0}\" из списка.", text);
			}
		}

		/// <summary>
		/// Навести курсор мыши на элемент
		/// </summary>
		public static void HoverElement(this IWebElement webElement)
		{
			var actionBuilder = new Actions(getDriverFromWebElement(webElement));
			actionBuilder.MoveToElement(webElement).Build().Perform();
		}

		/// <summary>
		/// Прокрутить и кликнуть на элемент
		/// </summary>
		public static void ScrollAndClick(this IWebElement webElement)
		{
			webElement.scrollToWebElement();

			try
			{
				webElement.Click();
			}
			catch (Exception ex)
			{
				Assert.Fail("Произошла ошибка при попытки клика на web-элемент: {0}", ex.Message);
			}
		}

		/// <summary>
		/// Метод скроллит до того момента, пока web-элемент не станет видимым
		/// </summary>
		/// <param name="webElement"></param>
		private static void scrollToWebElement(this IWebElement webElement)
		{
			Logger.Trace("Скроллинг страницы до того момента, пока web-элемент не станет видимым");
			var driver = getDriverFromWebElement(webElement);

			try
			{
				driver.Scripts().ExecuteScript("arguments[0].scrollIntoView(true); window.scrollBy(0,-200);", webElement);
			}
			catch (Exception ex)
			{
				Assert.Fail("При попытке скроллинга страницы произошла ошибка: " + ex.Message);
			}
		}

		/// <summary>
		/// Получить драйвер из web-элемента
		/// </summary>
		/// <param name="webElement"></param>
		private static IWebDriver getDriverFromWebElement(this IWebElement webElement)
		{
			IWebDriver wrappedDriver;

			try
			{
				wrappedDriver = ((IWrapsDriver)webElement).WrappedDriver;
			}
			catch (InvalidCastException)
			{
				var wrappedElement = ((IWrapsElement)webElement).WrappedElement;
				wrappedDriver = ((IWrapsDriver)wrappedElement).WrappedDriver;
			}
			
			return wrappedDriver;
		}
	}
}
