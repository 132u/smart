using System;
using System.Linq;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	class ExportNotification : BaseObject, IAbstractPage<ExportNotification>
	{
		public WebDriver Driver { get; private set; }

		public ExportNotification(WebDriver driver)
		{
			Driver = driver;
		}

		public ExportNotification GetPage()
		{
			var exportNotification = new ExportNotification(Driver);
			InitPage(exportNotification, Driver);

			return exportNotification;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(NOTIFIER_DOWNLOAD_BTN), timeout: 30))
			{
				Assert.Fail("Произошла ошибка:\n уведомление не появилось.");
			}
		}

		/// <summary>
		/// Кликнуть кнопку загрузки в сообщении об экспотре
		/// </summary>
		public T ClickDownloadNotifier<T>(WebDriver driver) where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Кликнуть кнопку загрузки в сообщении об экспорте.");
			IWebElement element;

			try
			{
				element = Driver.FindElement(By.XPath(NOTIFIER_DOWNLOAD_BTN));
			}
			catch (StaleElementReferenceException)
			{
				CustomTestContext.WriteLine("Не удалось найти кнопку загрузки в сообщении об экспорте. Предпринять повторную попытка.");
				element = Driver.FindElement(By.XPath(NOTIFIER_DOWNLOAD_BTN));
			}

			element.JavaScriptClick();

			var instance = Activator.CreateInstance(typeof(T), new object[] { driver }) as T;
			return instance.GetPage();
		}

		/// <summary>
		/// Закрыть уведомление
		/// </summary>
		public T ClickCancelNotifier<T>(WebDriver driver) where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Закрыть уведомление.");
			CancelNotifierButton.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { driver }) as T;
			return instance.GetPage();
		}

		/// <summary>
		/// Проверить, что сообщение содержит текст, который передан в качестве аргумента
		/// </summary>
		/// <param name="text">текст</param>
		public ExportNotification AssertContainsText(string text)
		{
			CustomTestContext.WriteLine("Проверить, что сообщение содержит текст: '{0}'", text);

			Assert.IsTrue(NotifierMessage.Text.Contains(text),
				"Произошла ошибка:\n сообщение не содержит искомый текст.");
			
			return GetPage();
		}

		/// <summary>
		/// Проверить, что в сообщении указана дата, которая не расходится с текущей более,чем на один час
		/// </summary>
		public ExportNotification AssertContainsCurrentDate()
		{
			CustomTestContext.WriteLine("Проверить, что в сообщении указана дата, которая не расходится с текущей более, чем на один час");
			CustomTestContext.WriteLine("Пробуем распарсить дату и привести к формату: MM/dd/yy hh:mm");
			
			var notifierText = NotifierMessage.Text;
			var startIndex = notifierText.IndexOf("/") - 2;
			var month = notifierText.Substring(startIndex, 2);
			startIndex += 3; // "mm/" = 3
			var day = notifierText.Substring(startIndex, 2);
			startIndex += 3; // "dd/" = 3
			var year = notifierText.Substring(startIndex, 4);
			startIndex += 5; // "yyyy " = 5;
			var hour = notifierText.Substring(startIndex, 2);
			startIndex += 3; // "hh:" = 3
			var min = notifierText.Substring(startIndex, 2);

			CustomTestContext.WriteLine("Результат парсинга = {0}/{1}/{2} {3}:{4}", month , day, year, hour, min);

			var notifierDate = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(min), 0);
			var timeSubtract = DateTime.Now.Subtract(notifierDate).Hours;

			Assert.IsTrue(timeSubtract == 0,
				"Произошла ошибка:\n сообщение не содержит требуемую дату.");
			
			return GetPage();
		}

		/// <summary>
		/// Получить текст верхнего сообщения.
		/// </summary>
		public string GetTextUpperNotification()
		{
			CustomTestContext.WriteLine("Получить текст верхнего сообщения.");

			var upperNotification = Driver.GetElementList(By.XPath(NOTIFIER_MESSAGE)).LastOrDefault();

			return upperNotification != null ? upperNotification.Text : string.Empty;
		}

		/// <summary>
		/// Переключиться на сообщение с заданным номером.
		/// </summary>
		/// <param name="notificationNumber">номер сообщения</param>
		public ExportNotification SwitchToNotificationByNumber(int notificationNumber)
		{
			CustomTestContext.WriteLine("Переключиться на сообщение №{0}", notificationNumber);

			NotificationByNumber = Driver.SetDynamicValue(How.XPath, NOTIFIER_ITEM, notificationNumber.ToString());

			try
			{
				Driver.ExecuteScript("arguments[0].click()", NotificationByNumber);
			}
			catch (StaleElementReferenceException)
			{
				CustomTestContext.WriteLine("Не удалось переключиться на сообщение {0}. Предпринять повторную попытку.", notificationNumber);
				SwitchToNotificationByNumber(notificationNumber);
			}
			catch (Exception ex)
			{
				Assert.Fail("Произошла ошибка:\n не удалось кликнуть на сообщение '{0}'. Текст ошибки: {1}",
					notificationNumber,
					ex.Message);
			}

			return GetPage();
		}

		/// <summary>
		/// Проверить, что показывается нужное кол-во уведомлений
		/// </summary>
		/// <param name="expectedCount">ожидаемое кол-во</param>
		public ExportNotification AssertCountExportNotifiers(int expectedCount)
		{
			CustomTestContext.WriteLine("Проверить, что показывается {0} уведомлений.", expectedCount);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(NOTIFIER_ITEM.Replace("*#*", expectedCount.ToString()))),
				"Произошла ошибка:\n не появилось {0} уведомлений", expectedCount);

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = NOTIFIER_DOWNLOAD_BTN)]
		protected IWebElement DownloadNotifierButton { get; set; }

		[FindsBy(How = How.XPath, Using = NOTIFIER_CANCEL_BTN)]
		protected IWebElement CancelNotifierButton { get; set; }

		[FindsBy(How = How.XPath, Using = NOTIFIER_MESSAGE)]
		protected IWebElement NotifierMessage { get; set; }

		protected IWebElement NotificationByNumber { get; set; }

		protected const string NOTIFIER_CANCEL_BTN = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//div[not(contains(@style,'none'))]//a[contains(text(),'Close')]";
		protected const string NOTIFIER_DOWNLOAD_BTN = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//div[not(contains(@style,'none'))]//a[contains(text(),'Download')]";
		protected const string NOTIFIER_MESSAGE = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//span[@data-bind='html: message']";
		protected const string NOTIFIER_ITEM = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')][*#*]";

	}
}
