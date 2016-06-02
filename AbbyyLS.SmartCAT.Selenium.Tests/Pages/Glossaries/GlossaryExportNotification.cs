using System;
using System.Threading;

using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossaryExportNotification : GlossariesPage, IAbstractPage<GlossaryExportNotification>
	{
		public GlossaryExportNotification(WebDriver driver) : base(driver)
		{
		}

		public new GlossaryExportNotification LoadPage()
		{
			if (!IsGlossaryExportNotificationDisplayed())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница глоссария");
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
		#endregion

		#region Составные методы страницы

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

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, присутствует ли уведомление
		/// </summary>
		public bool IsGlossaryExportNotificationDisplayed(int timeout = 60)
		{
			CustomTestContext.WriteLine("Проверить, есть ли сообщение со ссылкой на скачивание глоссария.");

			if (!Driver.WaitUntilElementIsAppear(By.XPath(NOTIFIER_DOWNLOAD_BTN), timeout: timeout))
			{
				RefreshPage<WorkspacePage>();
			}

			return Driver.WaitUntilElementIsAppear(By.XPath(NOTIFIER_DOWNLOAD_BTN), timeout: timeout);
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = NOTIFIER_DOWNLOAD_BTN)]
		protected IWebElement DownloadNotifierButton { get; set; }

		protected IWebElement CancelNotifierButton { get; set; }
		#endregion

		#region Описания XPath элементов

		protected const string NOTIFIER_CANCEL_BTN = "//button[contains(@class, 'g-btn_theme_dark')]//span[text()='Close']";
		protected const string NOTIFIER_DOWNLOAD_BTN = "//button[contains(@class, 'g-btn_theme_dark')]//span[text()='Скачать']";
	   
		#endregion
	}
}
