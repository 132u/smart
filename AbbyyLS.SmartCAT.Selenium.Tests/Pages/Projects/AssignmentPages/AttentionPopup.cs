using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.AssignmentPages
{
	public class AttentionPopup : ProjectsPage, IAbstractPage<AttentionPopup>
	{
		public AttentionPopup(WebDriver driver) : base(driver)
		{
		}

		public new AttentionPopup LoadPage()
		{
			if (!IsAttentionPopupOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылась страница назначения пользователя");
			}

			return this;
		}
		
		#region Простые методы страницы

		/// <summary>
		/// Нажать на кнопку Cancel.
		/// </summary>
		public TaskAssignmentPage ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку Cancel.");
			CancelButton.Click();

			return new TaskAssignmentPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку Save.
		/// </summary>
		public TaskAssignmentPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку Save.");
			SaveButton.Click();

			return new TaskAssignmentPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылся ли поп-ап сообщения о точ, что дедлайн позже дедлайна проекта.
		/// </summary>
		public bool IsAttentionPopupOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CANCEL_BUTTON));
		}

		#endregion

		#region Описание XPath элементов

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string CANCEL_BUTTON = "//form[contains(@class, 'ajax-form-submit')]//a[contains(@class, 'js-popup-close')]";
		protected const string SAVE_BUTTON = "//form[contains(@class, 'ajax-form-submit')]//input[contains(@class, 'js-submit-btn')]";

		#endregion
	}
}
