using System;
using NLog;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
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
		/// Удаляем старый текст и вводим новый
		/// </summary>
		/// <param name="webElement">элемент</param>
		/// <param name="text">текст</param>
		/// <param name="clearFirst">нужно ли удалять предыдущий текст</param>
		public static void SetText(this IWebElement webElement, string text, bool clearFirst = true)
		{
			if (clearFirst)
			{
				webElement.Clear();
			}
			webElement.SendKeys(text);
		}

		/// <summary>
		/// Выбираем вариант из списка (использовать только для тэга Select!)
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
			var actionBuilder = new Actions(((IWrapsDriver)webElement).WrappedDriver);
			actionBuilder.MoveToElement(webElement).Build().Perform();
		}
	}
}
