using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class ConfirmDeclineTaskDialog: ProjectSettingsPage, IAbstractPage<ConfirmDeclineTaskDialog>
	{
		public ConfirmDeclineTaskDialog(WebDriver driver)
			: base(driver)
		{
		}

		public new ConfirmDeclineTaskDialog GetPage()
		{
			var confirmDeclineTaskDialog = new ConfirmDeclineTaskDialog(Driver);
			InitPage(confirmDeclineTaskDialog, Driver);

			return confirmDeclineTaskDialog;
		}

		public new void LoadPage()
		{
			if (!IsConfirmDeclineTaskDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Диалог подтверждения отмены задачи не открылся.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Decline.
		/// </summary>
		public ProjectSettingsPage ClickDeclineButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Decline.");
			DeclineButton.Click();

			return new ProjectSettingsPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Decline.
		/// </summary>
		public ProjectsPage ClickDeclineButtonExpectingProjectsPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку Decline.");
			DeclineButton.Click();

			return new ProjectsPage(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся диалог отмены задачи
		/// </summary>
		public bool IsConfirmDeclineTaskDialogOpened()
		{
			return Driver.GetIsElementExist(By.XPath(DECLINE_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = DECLINE_BUTTON)]
		protected IWebElement DeclineButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string DECLINE_BUTTON = "//div[contains(@class, 'js-popup-confirm')]//input[@value = 'Decline']";

		#endregion
	}
}
