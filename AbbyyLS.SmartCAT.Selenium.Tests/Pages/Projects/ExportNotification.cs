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
		/// Кликнуть кнопку загрузки в сообщении об экспотре
		/// </summary>
		public T ClickDownloadNotifier<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Кликнуть кнопку загрузки в сообщении об экспорте");
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
            var countNotifiers = GetCountExportNotifiers();

            for (int i = 0; i < countNotifiers; i++)
            {
                //sleep стоит,чтобы закрываемые уведомления успевали пропадать
                Thread.Sleep(1000);
                CancelNotifierButton.Click();
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
	    public bool IsExportNotificationDisplayed()
	    {
	        return Driver.WaitUntilElementIsDisplay(By.XPath(NOTIFIER_DOWNLOAD_BTN), timeout: 50);
	    }

		/// <summary>
		/// Проверить, что показывается нужное кол-во уведомлений
		/// </summary>
		/// <param name="expectedCount">ожидаемое кол-во</param>
        public bool IsExportNotifiersCountMatchExpected(int expectedCount)
		{
			CustomTestContext.WriteLine("Проверить, что показывается {0} уведомлений", expectedCount);

			return Driver.WaitUntilElementIsDisplay(By.XPath(NOTIFIER_ITEM.Replace("*#*", expectedCount.ToString())));
		}

        /// <summary>
        /// Проверить, что сообщение содержит текст, который передан в качестве аргумента
        /// </summary>
        /// <param name="text">текст</param>
        public bool IsNotificationContainsText(string text)
        {
            CustomTestContext.WriteLine("Проверить, что сообщение содержит текст: '{0}'", text);

	        return Driver.WaitUntilElementIsDisplay(By.XPath(NOTIFIER_MESSAGE_BY_TEXT.Replace("*#*", text)));
        }

        /// <summary>
        /// Проверить, что в сообщении указана дата, которая не расходится с текущей более,чем на один час
        /// </summary>
        public bool IsNotificationContainsCurrentDate()
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

            CustomTestContext.WriteLine("Результат парсинга = {0}/{1}/{2} {3}:{4}", month, day, year, hour, min);

            var notifierDate = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(min), 0);
            var timeSubtract = DateTime.Now.Subtract(notifierDate).Hours;

            return timeSubtract == 0;
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

		[FindsBy(How = How.XPath, Using = NOTIFIER_MESSAGE)]
		protected IWebElement NotifierMessage { get; set; }

		protected IWebElement NotificationByNumber { get; set; }

        #endregion

        #region Описания XPath элементов

		protected const string NOTIFIER_CANCEL_BTN = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//div[not(contains(@style,'none'))]//a[contains(text(),'Close')]";
		protected const string NOTIFIER_DOWNLOAD_BTN = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//div[not(contains(@style,'none'))]//a[contains(text(),'Download')]";
		protected const string NOTIFIER_MESSAGE = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//span[@data-bind='html: message']";
		protected const string NOTIFIER_MESSAGE_BY_TEXT = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]//span[@data-bind='html: message'][contains(text(),'*#*')]";
		protected const string NOTIFIER_ITEM = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')][*#*]";

        #endregion
	}
}
