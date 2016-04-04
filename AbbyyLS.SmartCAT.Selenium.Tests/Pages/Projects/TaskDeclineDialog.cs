using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class TaskDeclineDialog : ProjectsPage, IAbstractPage<TaskDeclineDialog>
	{
		public TaskDeclineDialog(WebDriver driver)
			: base(driver)
		{
		}

		public new TaskDeclineDialog LoadPage()
		{
			if (!IsTaskDeclineDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог отмены задачи.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Decline.
		/// </summary>
		public ProjectsPage ClickDeclineButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Decline.");
			DeclineButton.Click();

			return new ProjectsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel.
		/// </summary>
		public ProjectsPage ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel.");
			CancelButton.Click();

			return new ProjectsPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы



		#endregion

		#region Методы, проверяющие состояние страницы

		public bool IsTaskDeclineDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DECLINE_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = DECLINE_BUTTON)]
		protected IWebElement DeclineButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string DECLINE_BUTTON = "//div[contains(@class, 'popup-confirm')]//div[contains(@class, 'greenbtn')]";
		protected const string CANCEL_BUTTON = "//a[contains(@class, 'js-popup-close')]";

		#endregion
	}
}
