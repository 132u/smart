using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class ConfirmDeclineTaskDialog: ProjectSettingsPage, IAbstractPage<ConfirmDeclineTaskDialog>
	{
		public ConfirmDeclineTaskDialog(WebDriver driver) : base(driver)
		{
		}

		public new ConfirmDeclineTaskDialog LoadPage()
		{
			if (!IsConfirmDeclineTaskDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Диалог подтверждения отмены задачи не открылся.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Decline.
		/// </summary>
		public ProjectSettingsPage ClickDeclineButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Decline.");
			DeclineButton.Click();

			return new ProjectSettingsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Decline.
		/// </summary>
		public ProjectsPage ClickDeclineButtonExpectingProjectsPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку Decline.");
			DeclineButton.Click();

			return new ProjectsPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся диалог отмены задачи
		/// </summary>
		public bool IsConfirmDeclineTaskDialogOpened()
		{
			return !IsDialogBackgroundDisappeared() && Driver.WaitUntilElementIsAppear(By.XPath(DECLINE_BUTTON));
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
