using System;
using System.Linq;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class ExportNotification : WorkspacePage, IAbstractPage<ExportNotification>
	{
		public ExportNotification(WebDriver driver) : base(driver)
		{
		}

		public new ExportNotification LoadPage()
		{
			if (!IsExportNotificationDisplayed())
			{
				throw new XPathLookupException("Произошла ошибка:\n уведомление не появилось");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Навести курсор на кнопку загрузки в сообщении об экспотре
		/// </summary>
		public T HoverOnDownloadNotifierButton<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Навести курсор на кнопку загрузки в сообщении об экспотре");
			DownloadNotifierButton.HoverElement();

			var instance = Activator.CreateInstance(typeof(T), Driver) as T;
			return instance.LoadPage();
		}

		/// <summary>
		/// Закрыть все уведомления
		/// </summary>
		public T CancelAllNotifiers<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Закрыть все уведомления");
			var countNotifiers = GetCountExportNotifiers();

			for (int i = countNotifiers; i > 0; i--)
			{
				var n = 0;
				//sleep стоит,чтобы закрываемые уведомления успевали пропадать
				Thread.Sleep(1000);
				Driver.WaitUntilElementIsDisplay(By.XPath(NOTIFIER_CANCEL_BTN.Replace("*#*", i.ToString())));
				CancelNotifierButton = Driver.SetDynamicValue(How.XPath, NOTIFIER_CANCEL_BTN, i.ToString());
				CustomTestContext.WriteLine("Закрыть уведомление №{0}, попытка клика №1.", i);
				CancelNotifierButton.JavaScriptClick();
				if (n < 5 && !Driver.WaitUntilElementIsDisappeared(By.XPath(NOTIFIER_CANCEL_BTN.Replace("*#*", i.ToString()))))
				{
					CustomTestContext.WriteLine("Закрыть уведомление №{0}, попытка клика №{1}.", i, n + 2);
					CancelNotifierButton = Driver.SetDynamicValue(How.XPath, NOTIFIER_CANCEL_BTN, i.ToString());
					CancelNotifierButton.JavaScriptClick();
					n++;
				}

				if (!Driver.WaitUntilElementIsDisappeared(By.XPath(NOTIFIER_CANCEL_BTN.Replace("*#*", i.ToString()))))
				{
					throw new Exception("Кнопка закрытия №" + i + " не исчезла.");
				}
			}

			var instance = Activator.CreateInstance(typeof(T), Driver) as T;
			return instance.LoadPage();
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
				throw new Exception(String.Format("Произошла ошибка:\n не удалось кликнуть на сообщение. Текст ошибки: {1}", ex.Message));
			}

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Кликнуть кнопку загрузки в сообщении об экспотре
		/// </summary>
		public T ClickDownloadNotifier<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Кликнуть кнопку загрузки в сообщении об экспорте");
			HoverOnDownloadNotifierButton<T>();
			DownloadNotifierButton.Click();

			var instance = Activator.CreateInstance(typeof(T), Driver) as T;
			return instance.LoadPage();
		}

		/// <summary>
		/// Перелючиться на уведомление по его номеру
		/// </summary>
		/// <remarks>
		/// Уведомления считаются по оси z
		/// 1-е - дальнее, скрытое
		/// Последнее - ближнее, видимое целиком
		/// </remarks>
		/// <param name="notificationNumber">Номер уведомления</param>
		/// <returns>Текст верхнего сообщения уведомления</returns>
		public string GetTextNotificationByNumber(int notificationNumber)
		{
			SwitchToNotificationByNumber(notificationNumber);

			return GetTextUpperNotification();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, присутствует ли уведомление
		/// </summary>
		public bool IsExportNotificationDisplayed(int timeout = 60)
		{
			CustomTestContext.WriteLine("Проверить, есть ли сообщение со ссылкой на скачивание документа.");

			if (!Driver.WaitUntilElementIsAppear(By.XPath(NOTIFIER_DOWNLOAD_BTN), timeout: timeout))
			{
				RefreshPage<WorkspacePage>();
			}

			return Driver.WaitUntilElementIsAppear(By.XPath(NOTIFIER_DOWNLOAD_BTN), timeout: timeout);
		}

		/// <summary>
		/// Проверить, что сообщение со ссылкой на скачивание документа исчезло.
		/// </summary>
		public bool IsExportNotificationDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение со ссылкой на скачивание документа исчезло.");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(NOTIFIER_DOWNLOAD_BTN), timeout: 20);
		}

		/// <summary>
		/// Проверить, что показывается нужное кол-во уведомлений
		/// </summary>
		/// <param name="expectedCount">ожидаемое кол-во</param>
		public bool IsExportNotifiersCountMatchExpected(int expectedCount)
		{
			CustomTestContext.WriteLine("Проверить, что показывается {0} уведомлений", expectedCount);

			return Driver.WaitUntilElementIsAppear(By.XPath(NOTIFIER_ITEM.Replace("*#*", expectedCount.ToString())));
		}

		/// <summary>
		/// Проверить, что верхнее сообщение содержит текст, который передан в качестве аргумента
		/// </summary>
		/// <param name="text">текст</param>
		public bool IsUpperNotificationContainsText(string text)
		{
			CustomTestContext.WriteLine("Проверить, что верхнее сообщение содержит текст: '{0}'", text);

			return Driver.WaitUntilElementIsAppear(By.XPath(NOTIFIER_MESSAGE_BY_TEXT.Replace("*#*", text)));
		}

		#endregion
		
		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = NOTIFIER_DOWNLOAD_BTN)]
		protected IWebElement DownloadNotifierButton { get; set; }

		protected IWebElement CancelNotifierButton { get; set; }

		protected IWebElement NotificationByNumber { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string NOTIFIER_CANCEL_BTN = "(//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//div[not(contains(@style,'none'))]//button//span[contains(text(), 'Close')]/..)[*#*]//span";
		protected const string NOTIFIER_DOWNLOAD_BTN = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//div[not(contains(@style,'none'))]//button//span[contains(text(),'Download')]/..";
		protected const string NOTIFIER_MESSAGE = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//span[@data-bind='html: message']";
		protected const string NOTIFIER_MESSAGE_BY_TEXT = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//div[not(@style)]//span[@data-bind='html: message'][contains(text(),'*#*')]";
		protected const string NOTIFIER_ITEM = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')][*#*]";

		#endregion
	}
}
