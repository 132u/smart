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
		/// <param name="expectedText">текст,который ожидается увидеть в поле после окончания ввода</param>
		public static void SetText(this IWebElement webElement, string text, string expectedText = null)
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

			expectedText = expectedText ?? text;

			// TrimEnd(' ') используется в связи с тем что иногда редактор добаляет в конце слова пробел
			if (webElement.GetAttribute("value") != expectedText && webElement.Text.TrimEnd(' ') != expectedText)
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
		/// Кликнуть по веб-элементу с использованием Action - класса
		/// </summary>
		/// <remarks> 
		/// Приментяется в тех случаях, когда обычный клик отрабатывает нестабильно
		/// </remarks>
		public static void AdvancedClick(this IWebElement webElement)
		{
			var actionBuilder = new Actions(getDriverFromWebElement(webElement));
			actionBuilder.Click(webElement).Build().Perform();
		}

		/// <summary>
		/// Кликнуть по веб-элементу с использованием JavaScript
		/// </summary>
		public static void JavaScriptClick(this IWebElement webElement)
		{
			var driver = webElement.getDriverFromWebElement();
			try
			{
				driver.Scripts().ExecuteScript("arguments[0].click();", webElement);
			}
			catch (Exception ex)
			{
				Assert.Fail("Произошла ошибка при попытке клика с помощью JavaScript на web-элемент: {0}", ex.Message);
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
		/// Вернуть значение атрибута элемента
		/// </summary>
		/// <param name="webElement">элемент,у которого ищется значение атрибута</param>
		/// <param name="attr">атрибут</param>
		/// <returns>значение атрибута</returns>
		public static string GetElementAttribute(this IWebElement webElement, string attr)
		{
			try
			{
				return webElement.GetAttribute(attr) ?? string.Empty;
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetElementAttribute: " + webElement.TagName, staleElementReferenceException);

				return GetElementAttribute(webElement, attr);
			}
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
		/// Двойной клик по элементу
		/// </summary>
		/// <param name="webElement">элемент</param>
		public static void DoubleClick(this IWebElement webElement)
		{
			var driver = getDriverFromWebElement(webElement);

			try
			{
				var action = new Actions(driver);
				action.DoubleClick(webElement);
				action.Perform();
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: DoubleClickElement: " + webElement.TagName, staleElementReferenceException);
				DoubleClick(webElement);
			}
		}

		/// <summary>
		/// Проверить, отмечен ли checkbox или radio
		/// </summary>
		/// <param name="webElement">элемент</param>
		/// <returns>отмечен (стоит галочка или точка)</returns>
		public static bool GetIsInputChecked(this IWebElement webElement)
		{
			try
			{
				return webElement.GetAttribute("checked") == "true";
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetIsInputChecked: " + webElement.TagName, staleElementReferenceException);
				return GetIsInputChecked(webElement);
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
		/// <param name="webElement">элемент</param>
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
