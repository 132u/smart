using System;
using System.IO;
using System.Linq;
using System.Threading;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
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

		public ExportNotification GetPage()
		{
			var exportNotification = new ExportNotification(Driver);
			InitPage(exportNotification, Driver);

			return exportNotification;
		}

		public void LoadPage()
		{
			if (!IsExportNotificationDisplayed())
			{
				throw new XPathLookupException("Произошла ошибка:\n уведомление не появилось");
			}
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
			return instance.GetPage();
		}

		/// <summary>
		/// Закрыть уведомление
		/// </summary>
		public T ClickCancelNotifier<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Закрыть уведомление.");
			CancelNotifierButton.Click();

			var instance = Activator.CreateInstance(typeof(T), Driver) as T;
			return instance.GetPage();
		}

		/// <summary>
		/// Закрыть все уведомления
		/// </summary>
		public T CancelAllNotifiers<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Закрыть все уведомления");

			var countNotifiers = GetCountExportNotifiers();

			for (int i = 0; i < countNotifiers; i++)
			{
				//sleep стоит,чтобы закрываемые уведомления успевали пропадать
				Thread.Sleep(1000);
				var cancelNotifierButton = Driver.WaitUntilElementIsClickable(By.XPath(NOTIFIER_CANCEL_BTN));
				if (cancelNotifierButton != null)
				{
					CustomTestContext.WriteLine("Закрыть уведомление №{0}", i);
					CancelNotifierButton.Click();
				}
				else
				{
					CustomTestContext.WriteLine(
						"Уведомление №{0} не стало кликабильным. Вызвать метод снова, чтобы работать с актуальным числом уведомлений.", i);
					CancelAllNotifiers<T>();
				}
			}

			var instance = Activator.CreateInstance(typeof(T), Driver) as T;
			return instance.GetPage();
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

			return GetPage();
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
			return instance.GetPage();
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

		#region Вспомогательные методы

		/// <summary>
		/// Возвращает маску имени экспортиремого файла для поиска на жёстком диске
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		/// <param name="filePath">путь до файла</param>
		public string GetExportFileNameMask(ExportType exportType, string filePath)
		{
			return exportType == ExportType.Tmx
				? Path.GetFileNameWithoutExtension(filePath) + "*.tmx"
				: Path.GetFileNameWithoutExtension(filePath) + "*" + Path.GetExtension(filePath);
		}

		/// <summary>
		/// Проверяет, что файл загрузился на жёсткий диск
		/// </summary>
		/// <param name="fileMask">макса имени файла</param>
		public bool IsFileDownloaded(string fileMask)
		{
			var files = getDownloadedFiles(fileMask, 15, PathProvider.ExportFiles);

			if (files.Length == 0)
			{
				CustomTestContext.WriteLine("Ошибка: файл не загрузился за отведённое время (15 секунд)");
				return false;
			}

			var directoryInfo = Directory.CreateDirectory(Path.Combine(
				new []{ 
					PathProvider.ResultsFolderPath,
					TestContext.CurrentContext.Test.Name,
					DateTime.Now.Ticks.ToString()
				})).FullName;

			var pathToMove = Path.Combine(
				new []{
					directoryInfo, 
					Path.GetFileNameWithoutExtension(files[0]) + DateTime.Now.Ticks + Path.GetExtension(files[0])
				});

			File.Move(files[0], pathToMove);

			return true;
		}

		/// <summary>
		/// Получает файлы на диске с заданной маской имени
		/// </summary>
		/// <param name="mask">маска имени файла</param>
		/// <param name="waitTime">максимальное время ожидания</param>
		/// <param name="dirName">каталог, в котором осуществляется поиск файлов</param>
		private string[] getDownloadedFiles(string mask, int waitTime, string dirName)
		{
			var files = new string[0];

			for (int i = 0; i < waitTime; i++)
			{
				files = Directory.GetFiles(dirName, mask, SearchOption.TopDirectoryOnly);
				if (files.Length > 0)
				{
					break;
				}
				Thread.Sleep(1000);//Ждём секунду
			}

			return files;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = NOTIFIER_DOWNLOAD_BTN)]
		protected IWebElement DownloadNotifierButton { get; set; }

		[FindsBy(How = How.XPath, Using = NOTIFIER_CANCEL_BTN)]
		protected IWebElement CancelNotifierButton { get; set; }

		protected IWebElement NotificationByNumber { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string NOTIFIER_CANCEL_BTN = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//div[not(contains(@style,'none'))]//button//span[contains(text(), 'Close')]/..";
		protected const string NOTIFIER_DOWNLOAD_BTN = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//div[not(contains(@style,'none'))]//button//span[contains(text(),'Download')]/..";
		protected const string NOTIFIER_MESSAGE = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//span[@data-bind='html: message']";
		protected const string NOTIFIER_MESSAGE_BY_TEXT = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//div[not(@style)]//span[@data-bind='html: message'][contains(text(),'*#*')]";
		protected const string NOTIFIER_ITEM = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')][*#*]";

		#endregion
	}
}
