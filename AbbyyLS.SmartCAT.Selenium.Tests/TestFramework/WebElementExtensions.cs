using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	/// <summary>
	///Дополнительные методы для элементов
	/// </summary>
	public static class WebElementExtensions
	{
		/// <summary>
		/// Удаляем старый текст и вводим новый
		/// </summary>
		/// <param name="e">элемент</param>
		/// <param name="text">текст</param>
		/// <param name="clearFirst">нужно ли удалять предыдущий текст</param>
		public static void SetText(this IWebElement e, string text, bool clearFirst = true)
		{
			if (clearFirst)
			{
				e.Clear();
			}
			e.SendKeys(text);
		}

		/// <summary>
		/// Выбираем вариант из списка (использовать только для тэга Select!)
		/// </summary>
		/// <param name="e">элемент Select</param>
		/// <param name="text">текст</param>
		public static void SelectOptionByText(this IWebElement e, string text)
		{
			try
			{
				var select = new SelectElement(e);
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
		/// Присваиваем значение элементу с динамическим локатором
		/// </summary>
		/// <param name="driver">драйвер</param>
		/// <param name="how">как ищем (Xpath и т.д.)</param>
		/// <param name="locator">динамический локатор</param>
		/// <param name="value">значение, на которое меняем часть локатора</param>
		/// <param name="value2">второе значение, на которое меняем часть локатора</param>
		public static IWebElement SetDynamicValue(this IWebDriver driver, How how, string locator, string value, string value2 = "")
		{
			IWebElement e = null;
			try
			{
				switch (how)
				{
					case How.XPath:

						e = driver.FindElement(By.XPath(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.CssSelector:
						e = driver.FindElement(By.CssSelector(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.Id:
						e = driver.FindElement(By.Id(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.ClassName:
						e = driver.FindElement(By.ClassName(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.LinkText:
						e = driver.FindElement(By.LinkText(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.PartialLinkText:
						e = driver.FindElement(By.PartialLinkText(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.Name:
						e = driver.FindElement(By.Name(locator.Replace("*#*", value).Replace("*##*", value2)));
						break;
					case How.TagName:
						e = driver.FindElement(By.TagName(locator.Replace("*#*", value).Replace("*##*", value2)));
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
				Console.WriteLine("StaleElementReferenceException: GetIsElementDisplay: " + "How " +
					how + " Using " + locator.Replace("*#*", value).Replace("*##*", value2));
				return SetDynamicValue(driver, how, locator, value);
			}
			catch (Exception ex)
			{
				Assert.Fail("Ошибка: " + ex.Message + "How " +
					how + " Using " + locator.Replace("*#*", value).Replace("*##*", value2));
			}

			return e;
		}
	}
}
