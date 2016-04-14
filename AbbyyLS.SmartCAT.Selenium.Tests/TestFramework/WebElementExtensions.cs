using System;
using System.Reflection;
using System.Threading;
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
		/// <summary>
		/// Вводим текст, предварительно отчистив поле web-элемента
		/// </summary>
		/// <param name="webElement">элемент</param>
		/// <param name="text">текст</param>
		/// <param name="expectedText">текст,который ожидается увидеть в поле после окончания ввода</param>
		public static void SetText(this IWebElement webElement, string text, string expectedText = null)
		{
			var i = 0;
			do
			{
				//CustomTestContext.WriteLine("Попытка ввода текста №{0}", i);
				i++;
				webElement.Clear();

				try
				{
					webElement.SendKeys(text);
				}
				catch (ElementNotVisibleException exception)
				{
					CustomTestContext.WriteLine("ElementNotVisibleException: SendKeys {0}", exception.Message);
				}

				expectedText = expectedText ?? text;
			} // TrimEnd(' ') используется в связи с тем, что иногда редактор добавляет в конце слова пробел
			// webElement.Text нужен, потому что value есть не у всех элементов
			while (i < 3 && webElement.GetAttribute("value") != expectedText && webElement.Text.TrimEnd(' ') != expectedText);

			if (webElement.GetAttribute("value") != expectedText && webElement.Text.TrimEnd(' ') != expectedText)
			{
				CustomTestContext.WriteLine("ebElement.GetAttribute('value')= {0},\nexpectedText= {1}, \nwebElement.Text.TrimEnd(' ')= {2}", webElement.GetAttribute("value"), expectedText, webElement.Text.TrimEnd(' '));
				throw new Exception("Произошла ошибка:\nНеверный текст в элементе");
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
				throw new UnexpectedTagNameException("Произошла ошибка:\n метод SelectOptionByText применен не к тэгу Select.");
			}
			catch (Exception)
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось выбрать \"{0}\" из списка.", text));
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
				((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", webElement);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format(
					"Произошла ошибка при попытке клика с помощью JavaScript на web-элемент: {0}",
					ex.Message));
			}
		}

		/// <summary>
		/// Навести курсор мыши на элемент
		/// </summary>
		public static void HoverElement(this IWebElement webElement)
		{
			try
			{
				var actionBuilder = new Actions(getDriverFromWebElement(webElement));
				actionBuilder.MoveToElement(webElement).Build().Perform();
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				CustomTestContext.WriteLine(
					"StaleElementReferenceException: HoverElement.", staleElementReferenceException);
				webElement.HoverElement();
			}
		}

		/// <summary>
		/// Произвести двойной клик по определенной точке элемента
		/// </summary>
		public static void DoubleClickElementAtPoint(this IWebElement webElement, int x, int y)
		{
			var actionBuilder = new Actions(getDriverFromWebElement(webElement));
			actionBuilder
				.MoveToElement(webElement, x, y)
				.DoubleClick()
				.Build()
				.Perform();
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
				CustomTestContext.WriteLine("StaleElementReferenceException: GetElementAttribute: " + webElement.TagName, staleElementReferenceException);

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
			catch (StaleElementReferenceException)
			{
				CustomTestContext.WriteLine("StaleElementReferenceException: Не удалось кликнуть по элементу после скроллинга. Предпринять повторную попытку.");
				webElement.Click();
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format(
					"Произошла ошибка при попытки клика на web-элемент: {0}", ex.Message));
			}
		}

		/// <summary>
		/// Прокрутить до элемента
		/// </summary>
		public static void Scroll(this IWebElement webElement)
		{
			try
			{
				webElement.scrollToWebElement();
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				CustomTestContext.WriteLine(
					"StaleElementReferenceException: Scroll: " + webElement.TagName, staleElementReferenceException);
				webElement.Scroll();
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
				CustomTestContext.WriteLine(
					"StaleElementReferenceException: DoubleClickElement: "
					+ webElement.TagName, staleElementReferenceException);
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
				CustomTestContext.WriteLine(
					"StaleElementReferenceException: GetIsInputChecked: "
					+ webElement.TagName, staleElementReferenceException);
				return GetIsInputChecked(webElement);
			}
		}

		/// <summary>
		/// Метод скроллит до того момента, пока web-элемент не станет видимым
		/// </summary>
		private static void scrollToWebElement(this IWebElement webElement)
		{
			var driver = getDriverFromWebElement(webElement);

			try
			{
				((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true); window.scrollBy(0,-200);", webElement);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format(
					"При попытке скроллинга страницы произошла ошибка: " + ex.Message));
			}
		}

		/// <summary>
		/// Метод скроллит до того момента, пока web-элемент не станет видимым
		/// </summary>
		public static void ScrollDown(this IWebElement webElement)
		{
			var driver = getDriverFromWebElement(webElement);

			try
			{
				((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true); window.scrollBy(0, 100);", webElement);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format(
					"При попытке скроллинга страницы произошла ошибка: " + ex.Message));
			}
		}


		public static void WaitTargetAndClick(this IWebElement webElement)
		{
			var i = 0;

			do
			{
				i++;

				try
				{
					webElement.Click();
					break;
				}
				catch (TargetInvocationException)
				{
					Thread.Sleep(500);
				}
			} while (i < 4);
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
