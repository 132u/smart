using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.AssignmentPages
{
	public class ReassignDialog : ProjectsPage, IAbstractPage<ReassignDialog>
	{
		public ReassignDialog(WebDriver driver) : base(driver)
		{
		}

		public new ReassignDialog LoadPage()
		{
			if (!IsReassigneDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылась страница назначения пользователя");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на кнопку Cancel в окне переназначения сегментов.
		/// </summary>
		public DistributeSegmentsBetweenAssigneesPage ClickCancelReassignePopUpButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку Cancel в окне переназначения сегментов.");
			CancelReassignePopUpButton.Click();

			return new DistributeSegmentsBetweenAssigneesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку Continue в окне переназначения сегментов.
		/// </summary>
		public DistributeSegmentsBetweenAssigneesPage ClickContinueReassignePopUpButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку Continue в окне переназначения сегментов.");
			ContinueReassignePopUpButton.Click();

			return new DistributeSegmentsBetweenAssigneesPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылось окно переназначения сегментов.
		/// </summary>
		public bool IsReassigneDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CONTINUE_REASSIGNE_POP_UP_BUTTON));
		}

		/// <summary>
		/// Проверить, что закрылось окно переназначения сегментов.
		/// </summary>
		public bool IsReassignePopUpDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что закрылось окно переназначения сегментов.");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(CONTINUE_REASSIGNE_POP_UP_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CONTINUE_REASSIGNE_POP_UP_BUTTON)]
		protected IWebElement ContinueReassignePopUpButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_REASSIGNE_POP_UP_BUTTON)]
		protected IWebElement CancelReassignePopUpButton { get; set; }
		#endregion

		#region Описание XPath элементов

		protected const string CONTINUE_REASSIGNE_POP_UP_BUTTON = "//form[contains(@class, 'ajax-form-submit')]//a[contains(@class, 'js-submit-btn')]";
		protected const string CANCEL_REASSIGNE_POP_UP_BUTTON = "//form[contains(@class, 'ajax-form-submit')]//a[contains(@class, 'js-popup-close')]";

		#endregion
	}
}
