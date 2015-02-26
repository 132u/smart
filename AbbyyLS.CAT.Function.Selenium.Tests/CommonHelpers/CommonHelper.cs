using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using System.Text.RegularExpressions;
using NLog;
using System.Windows.Forms;
using System.IO;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{

	/// <summary>
	/// Общий хелпер
	/// </summary>
	public class CommonHelper
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Драйвер
		/// </summary>
		private IWebDriver _driver;
		/// <summary>
		/// Драйвер
		/// </summary>
		protected IWebDriver Driver
		{
			get
			{
				return _driver;
			}
		}

		/// <summary>
		/// Таймаут
		/// </summary>
		private WebDriverWait _wait;
		/// <summary>
		/// Таймаут
		/// </summary>
		protected WebDriverWait Wait
		{
			get
			{
				return _wait;
			}
		}

		// TODO очень аккуратно проверить StaleElementReferenceException: ввела рекурсию!

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="driver"></param>
		/// <param name="wait"></param>
		public CommonHelper(IWebDriver driver, WebDriverWait wait)
		{
			_driver = driver;
			_wait = wait;

			languageID = new Dictionary<LANGUAGE, string>
			{
				{LANGUAGE.English, STANDARD_LANG_ID_EN},
				{LANGUAGE.Russian, STANDARD_LANG_ID_RU},
				{LANGUAGE.German, STANDARD_LANG_ID_DE},
				{LANGUAGE.French, STANDARD_LANG_ID_FR},
				{LANGUAGE.Japanese, STANDARD_LANG_ID_JP},
				{LANGUAGE.Lithuanian, STANDARD_LANG_ID_LT}
			};
		}

		/// <summary>
		/// Дождаться, пока появится элемент
		/// </summary>
		/// <param name="by">by</param>
		/// <param name="maxWait">таймаут</param>
		/// <returns>появился элемент</returns>
		protected bool WaitUntilDisplayElement(By by, int maxWait = 15)
		{
			if (by == null)
			{
				throw new ArgumentNullException("by");
			}
			// Записываем таймаут, который был. чтобы вернуть его обратно
			var timeout = _wait.Timeout.Seconds;
			try
			{
				_wait.Timeout = TimeSpan.FromSeconds(maxWait);
				_wait.Until((d) => d.FindElement(by).Displayed);
				return true;
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: WaitUntilDisplayElement: " + by.ToString(), staleElementReferenceException);
				return WaitUntilDisplayElement(by, maxWait);
			}
			catch (WebDriverTimeoutException)
			{
				Logger.Trace("WebDriverTimeoutException: WaitUntilDisplayElement: " + by.ToString());
				return false;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				// Возвращаем таймаут обратнно
				_wait.Timeout = TimeSpan.FromSeconds(timeout);
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
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Перейти в iframe
		/// </summary>
		protected void SwitchToFrame(By by)
		{
			var frame = Driver.FindElement(by);
			Driver.SwitchTo().Frame(frame);
		}

		/// <summary>
		/// Выйти из iframe 
		/// </summary>
		protected void SwitchToDefaultContent()
		{
			Driver.SwitchTo().DefaultContent();
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
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetIsElementDisplay: " + by.ToString(), staleElementReferenceException);
				return GetIsElementDisplay(by);
			}
			catch (NoSuchElementException)
			{
				Logger.Trace("NoSuchElementException: GetIsElementDisplay: " + by.ToString());
				return false;
			}
			catch (Exception)
			{
				throw;
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
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: ClickElement: " + by.ToString(), staleElementReferenceException);
				ClickElement(by);
			}
			catch (NoSuchElementException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
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

		/// <summary>
		/// Очистить элемент и ввести текст
		/// </summary>
		/// <param name="by">by</param>
		/// <param name="text">текст</param>
		protected void ClearAndAddText(By by, string text)
		{
			var txt = Regex.Replace(text, "[+^%~()]", "{$0}");

			try
			{
				var el = Driver.FindElement(by);
				el.Clear();
				el.SendKeys(txt);
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: ClearAndAddText: " + by.ToString(), staleElementReferenceException);
				ClearAndAddText(by, text);
			}
			catch (NoSuchElementException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Навести курсор мыши на элемент
		/// </summary>
		protected void HoverElement(By by)
		{
			IWebElement el = Driver.FindElement(by);
			Actions actionBuilder = new Actions(Driver);
			actionBuilder.MoveToElement(el).Build().Perform();
		}

		/// <summary>
		/// Очистить элемент
		/// </summary>
		/// <param name="by">by</param>
		protected void ClearElement(By by)
		{
			try
			{
				var el = Driver.FindElement(by);
				el.Clear();
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: ClearElement: " + by.ToString(), staleElementReferenceException);
				ClearElement(by);
			}
			catch (NoSuchElementException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Кликнуть по элементу и ввести текст
		/// </summary>
		/// <param name="by"></param>
		/// <param name="text"></param>
		protected void ClickAndSendTextElement(By by, string text)
		{
			var txt = Regex.Replace(text, "[+^%~()]", "{$0}");

			try
			{
				var el = Driver.FindElement(by);
				el.Click();
				el.SendKeys(txt);
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: ClickAndSendTextElement: " + by.ToString(), staleElementReferenceException);
				ClickAndSendTextElement(by, text);
			}
			catch (NoSuchElementException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Кликнуть по элементу, очистить и ввести текст
		/// </summary>
		/// <param name="by">by</param>
		/// <param name="text">текст</param>
		protected void ClickClearAndAddText(By by, string text)
		{
			var txt = Regex.Replace(text, "[+^%~()]", "{$0}");

			try
			{
				var el = Driver.FindElement(by);
				el.Click();
				el.SendKeys(OpenQA.Selenium.Keys.Home);
				el.SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.End);
				el.SendKeys(text);
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: ClickAndSendTextElement: " + by.ToString(), staleElementReferenceException);
				ClickAndSendTextElement(by, text);
			}
			catch (NoSuchElementException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Отправить текст элементу
		/// </summary>
		/// <param name="by">by</param>
		/// <param name="text">текст</param>
		protected void SendTextElement(By by, string text)
		{
			var txt = Regex.Replace(text, "[+^%~()]", "{$0}");

			try
			{
				Driver.FindElement(by).SendKeys(txt);
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: SendTextElement: " + by.ToString(), staleElementReferenceException);
				SendTextElement(by, text);
			}
			catch (NoSuchElementException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Дождаться, пока элемент пропадет
		/// </summary>
		/// <param name="by">by</param>
		/// <param name="maxWaitSeconds">максимальное время ожидания</param>
		/// <returns>пропал</returns>
		protected bool WaitUntilDisappearElement(By by, int maxWaitSeconds = 10)
		{
			var isDisplayed = false;
			var timeBegin = DateTime.Now.TimeOfDay;

			SetDriverTimeoutMinimum();

			do
			{
				isDisplayed = GetIsElementDisplay(by);

				if (!isDisplayed || DateTime.Now.TimeOfDay.Subtract(timeBegin).Seconds > maxWaitSeconds)
				{
					break;
				}

				Thread.Sleep(1000);
			} while (isDisplayed);

			SetDriverTimeoutDefault();

			return !isDisplayed;
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
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetTextElement: " + by.ToString(), staleElementReferenceException);
				return GetTextElement(by);
			}
			catch (NoSuchElementException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Вернуть содержимое класса элемента
		/// </summary>
		/// <param name="by">by</param>
		/// <returns>class</returns>
		protected string GetElementClass(By by)
		{
			try
			{
				return Driver.FindElement(by).GetAttribute("class");
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetElementClass: " + by.ToString(), staleElementReferenceException);
				return GetElementClass(by);
			}
			catch (NoSuchElementException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Вернуть список эелементов
		/// </summary>
		/// <param name="by">by</param>
		/// <returns>список</returns>
		protected IList<IWebElement> GetElementList(By by)
		{
			try
			{
				return Driver.FindElements(by);
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetElementList: " + by.ToString(), staleElementReferenceException);
				return GetElementList(by);
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Вернуть элемент
		/// </summary>
		/// <param name="by">by</param>
		/// <returns>элемент</returns>
		protected IWebElement GetElement(By by)
		{
			try
			{
				return Driver.FindElement(by);
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetElementList: " + by.ToString(), staleElementReferenceException);
				return GetElement(by);
			}
			catch (Exception)
			{
				throw;
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
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetElementCount: " + by.ToString(), staleElementReferenceException);
				return GetElementsCount(by);
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Вернуть список текстов элементов
		/// </summary>
		/// <param name="by">by</param>
		/// <returns>список текстов</returns>
		protected List<string> GetTextListElement(By by)
		{
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
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Двойной клик по элементу
		/// </summary>
		/// <param name="by">by</param>
		protected void DoubleClickElement(By by)
		{
			try
			{
				Thread.Sleep(2000);
				// Двойной клик
				var action = new Actions(Driver);
				action.DoubleClick(Driver.FindElement(by));
				action.Perform();
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: DoubleClickElement: " + by.ToString(), staleElementReferenceException);
				DoubleClickElement(by);
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Вернуть, активен ли элемент
		/// </summary>
		/// <param name="by">by</param>
		/// <returns>активен</returns>
		protected bool GetIsElementActive(By by)
		{
			try
			{
				// Проверить где находится курсор, и если в поле source, то все ок
				var a = Driver.FindElement(by);
				var b = Driver.SwitchTo().ActiveElement();

				var a_loc = a.Location;
				var b_loc = b.Location;

				var a_size = a.Size;
				var b_size = b.Size;

				return (a_loc == b_loc) && (a_size == b_size);
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetIsElementActive: " + by.ToString(), staleElementReferenceException);
				return GetIsElementActive(by);
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Проверить, отмечен ли checkbox или radio
		/// </summary>
		/// <param name="by">by</param>
		/// <returns>отмечен (стоит галочка или точка)</returns>
		protected bool GetIsInputChecked(By by)
		{
			try
			{
				return Driver.FindElement(by).GetAttribute("checked") == "true";
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetIsInputChecked: " + by.ToString(), staleElementReferenceException);
				return GetIsInputChecked(by);
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Вернуть значение атрибута элемента
		/// </summary>
		/// <param name="by">by</param>
		/// <param name="attr">атрибут</param>
		/// <returns>значение атрибута</returns>
		protected string GetElementAttribute(By by, string attr)
		{
			try
			{
				var result = Driver.FindElement(by).GetAttribute(attr);
				if (result == null)
				{
					result = "";
				}
				return result;
			}
			catch (StaleElementReferenceException staleElementReferenceException)
			{
				Logger.Warn("StaleElementReferenceException: GetElementAttribute: " + by.ToString(), staleElementReferenceException);
				return GetElementAttribute(by, attr);
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Преобразовать строку в число
		/// </summary>
		/// <param name="text">строка</param>
		/// <returns>число</returns>
		protected int ParseStrToInt(string text)
		{
			var result = 0;
			// Пытаемся преобразовать из строки в число
			Assert.IsTrue(int.TryParse(text, out result), "Ошибка: невозможно преобразовать в число: " + text);
			return result;
		}

		/// <summary>
		/// Очистить и заполнить все элементы списка
		/// </summary>
		/// <param name="by">by</param>
		/// <param name="text">текст</param>
		protected void ClearAndFillElementsList(By by, string text)
		{
			var txt = Regex.Replace(text, "[+^%~()]", "{$0}");

			var termList = GetElementList(by);
			foreach (var el in termList)
			{
				el.Clear();
				el.SendKeys(txt);
			}
		}

		/// <summary>
		/// Определить чекнут чекбокс или нет
		/// </summary>
		public bool IsSelected(By by)
		{
			return Driver.FindElement(by).Selected;
		}

		/// <summary>
		/// Работа с диалогом браузера: загрузка документа с помощью javascript
		/// </summary>
		/// <param name="DocumentName">полный путь к документу</param>
		protected void UploadDocument(string DocumentName, string file)
		{
			//Проверка, что элемент найден
			Assert.IsTrue(GetIsElementExist(By.XPath(file)),"Ошибка: элемент input для загрузки документа не найден, возможно xpath поменялся");
			((IJavaScriptExecutor)Driver).ExecuteScript("document.evaluate('" + file + "', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.style.display = 'block';");
			Driver.FindElement(By.XPath(file)).SendKeys(DocumentName);
			Thread.Sleep(1000); // Sleep Не удалять! необходим для предотвращения появления окна загрузки
		}

		/// <summary>
		/// Работа с диалогом браузера: загрузка документа
		/// </summary>
		/// <param name="DocumentName">полный путь к документу</param>
		protected void UploadDocNativeAction(string DocumentName)
		{
			Thread.Sleep(3000); // слип необходим, так как не всегда успевает открыться окно загрузки

			var txt = Regex.Replace(DocumentName, "[+^%~()]", "{$0}");

			SendKeys.SendWait(txt);
			Thread.Sleep(2000);
			SendKeys.SendWait(@"{Enter}");
			Thread.Sleep(2000);
		}

		/// <summary>
		/// Загрузка TMX во время создания проекта
		/// Загрузка TMX при создании TM
		/// </summary>
		protected void UploadTM(string DocumentName, string file)
		{
			//Проверка, что элемент найден
			Assert.IsTrue(GetIsElementExist(By.XPath(file)), "Ошибка: элемент input для загрузки TMX (во время создания ТМ или во вреям создания проекта) не найден, возможно xpath поменялся");
			((IJavaScriptExecutor)Driver).ExecuteScript("$(\"input:file\").removeClass(\"g-hidden\").css(\"opacity\", 100)");
			Driver.FindElement(By.XPath(file)).SendKeys(DocumentName);
			((IJavaScriptExecutor)Driver).ExecuteScript("$(\".js-import-file-form .js-control\").data(\"controller\").filenameLink.text($(\".js-import-file-form .js-control\").data(\"controller\").fileInput.val());");
			((IJavaScriptExecutor)Driver).ExecuteScript("$(\".js-import-file-form .js-control\").data(\"controller\").trigger(\"valueChanged\");");
		}

		public enum LANGUAGE { English, Russian, German, French, Japanese, Lithuanian };

		protected Dictionary<LANGUAGE, string> languageID;

		protected const string STANDARD_LANG_ID_EN = "9";
		protected const string STANDARD_LANG_ID_RU = "25";
		protected const string STANDARD_LANG_ID_DE = "7";
		protected const string STANDARD_LANG_ID_FR = "12";
		protected const string STANDARD_LANG_ID_JP = "1041";
		protected const string STANDARD_LANG_ID_LT = "1063";

	}
}
