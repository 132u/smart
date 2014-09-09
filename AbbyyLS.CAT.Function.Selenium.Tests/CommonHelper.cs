using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    /// <summary>
    /// Общий хелпер
    /// </summary>
	public class CommonHelper
    {
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
        public CommonHelper (IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;

            languageID = new Dictionary<LANGUAGE, string>();
            languageID.Add(LANGUAGE.English, STANDARD_LANG_ID_EN);
            languageID.Add(LANGUAGE.Russian, STANDARD_LANG_ID_RU);
            languageID.Add(LANGUAGE.German, STANDARD_LANG_ID_DE);
            languageID.Add(LANGUAGE.French, STANDARD_LANG_ID_FR);
            languageID.Add(LANGUAGE.Japanese, STANDARD_LANG_ID_JP);
            languageID.Add(LANGUAGE.Lithuanian, STANDARD_LANG_ID_LT);
        }

        /// <summary>
        /// Дождаться, пока появится элемент
        /// </summary>
        /// <param name="by">by</param>
		/// <param name="maxWait">таймаут</param>
        /// <returns>появился элемент</returns>
        protected bool WaitUntilDisplayElement(By by, int maxWait = 15)
        {
			// Записываем таймаут, который был. чтобы вернуть его обратно
			int timeout = _wait.Timeout.Seconds;
			try
            {
				_wait.Timeout = TimeSpan.FromSeconds(maxWait);
				_wait.Until((d) => d.FindElement(by).Displayed);
                return true;
            }
            catch (StaleElementReferenceException )
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
            catch(StaleElementReferenceException)
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
        /// Кликнуть по элементу и ввести текст
        /// </summary>
        /// <param name="by"></param>
        /// <param name="text"></param>
        protected void ClickAndSendTextElement(By by, string text)
        {
			string txt = Regex.Replace(text, "[+^%~()]", "{$0}");
			
			try
            {
                IWebElement el = Driver.FindElement(by);
                el.Click();
                el.SendKeys(txt);
            }
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("StaleElementReferenceException: ClickAndSendTextElement: " + by.ToString());
                ClickAndSendTextElement(by, text);
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
        /// Кликнуть по элементу, очистить и ввести текст
        /// </summary>
        /// <param name="by">by</param>
        /// <param name="text">тест</param>
        protected void ClickClearAndAddText(By by, string text)
        {
			string txt = Regex.Replace(text, "[+^%~()]", "{$0}");

			try
            {
                IWebElement el = Driver.FindElement(by);
                el.Click();
                el.Clear();
                el.SendKeys(txt);
            }
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("StaleElementReferenceException: ClickAndSendTextElement: " + by.ToString());
                ClickAndSendTextElement(by, text);
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
        /// Дождаться, пока элемент пропадет
        /// </summary>
        /// <param name="by">by</param>
        /// <param name="maxWaitSeconds">максимальное время ожидания</param>
        /// <returns>пропал</returns>
        protected bool WaitUntilDisappearElement(By by, int maxWaitSeconds = 10)
        {
            bool isDisplayed = false;
            TimeSpan timeBegin = DateTime.Now.TimeOfDay;
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
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("StaleElementReferenceException: GetElementClass: " + by.ToString());
                return GetElementClass(by);
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
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("StaleElementReferenceException: GetElementList: " + by.ToString());
                return GetElementList(by);
            }
            catch (Exception exType)
            {
                Assert.Fail("Произошла ошибка:\n" + exType.ToString());
                return null;
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
			catch (StaleElementReferenceException)
			{
				Console.WriteLine("StaleElementReferenceException: GetElementList: " + by.ToString());
				return GetElement(by);
			}
			catch (Exception exType)
			{
				Assert.Fail("Произошла ошибка:\n" + exType.ToString());
				return null;
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
        /// Вернуть список текстов элементов
        /// </summary>
        /// <param name="by">by</param>
        /// <returns>список текстов</returns>
        protected List<string> GetTextListElement(By by)
        {
            try
            {
                List<string> textList = new List<string>();
                IList<IWebElement> elList = GetElementList(by);
                foreach (IWebElement el in elList)
                {
                    textList.Add(el.Text);
                }
                return textList;
            }
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("StaleElementReferenceException: GetTextListElement: " + by.ToString());
                return GetTextListElement(by);
            }
            catch (Exception exType)
            {
                Assert.Fail("Произошла ошибка:\n" + exType.ToString());
                return null;
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
                Actions action = new Actions(Driver);
                action.DoubleClick(Driver.FindElement(by));
                action.Perform();
            }
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("StaleElementReferenceException: DoubleClickElement: " + by.ToString());
                DoubleClickElement(by);
            }
            catch (Exception exType)
            {
                Assert.Fail("Произошла ошибка:\n" + exType.ToString());
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
                IWebElement a = Driver.FindElement(by);
                IWebElement b = Driver.SwitchTo().ActiveElement();

                Point a_loc = a.Location;
                Point b_loc = b.Location;

                Size a_size = a.Size;
                Size b_size = b.Size;

                return (a_loc == b_loc) && (a_size == b_size);
            }
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("StaleElementReferenceException: GetIsElementActive: " + by.ToString());
                return GetIsElementActive(by);
            }
            catch (Exception exType)
            {
                Assert.Fail("Произошла ошибка:\n" + exType.ToString());
                return false;
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
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("StaleElementReferenceException: GetIsInputChecked: " + by.ToString());
                return GetIsInputChecked(by);
            }
            catch (Exception exType)
            {
                Assert.Fail("Произошла ошибка:\n" + exType.ToString() + "\n\nBy:\n" + by.ToString());
                return false;
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
                string result = Driver.FindElement(by).GetAttribute(attr);
                if (result == null)
                {
                    result = "";
                }
                return result;
            }
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("StaleElementReferenceException: GetElementAttribute: " + by.ToString());
                return GetElementAttribute(by, attr);
            }
            catch (Exception exType)
            {
                Assert.Fail("Произошла ошибка:\n" + exType.ToString() + "\n\nBy:\n" + by.ToString());
                return "";
            }
        }

        /// <summary>
        /// Преобразовать строку в число
        /// </summary>
        /// <param name="text">строка</param>
        /// <returns>число</returns>
        protected int ParseStrToInt(string text)
        {
            int result = 0;
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
			string txt = Regex.Replace(text, "[+^%~()]", "{$0}");
			
			IList<IWebElement> termList = GetElementList(by);
            foreach (IWebElement el in termList)
            {
                el.Clear();
                el.SendKeys(txt);
            }
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
