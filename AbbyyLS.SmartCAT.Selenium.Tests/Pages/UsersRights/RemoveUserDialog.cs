using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class RemoveUserDialog: UsersAndRightsBasePage, IAbstractPage<RemoveUserDialog>
	{
		public RemoveUserDialog(WebDriver driver) : base(driver)
		{
		}

		public new RemoveUserDialog LoadPage()
		{
			if (!IsRemoveUserDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка: Не открылся диалог удаления пользователя.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Delete.
		/// </summary>
		public UsersTab ClickDeleteButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Delete.");
			DeleteButton.Click();

			return new UsersTab(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся диалог удаления пользователя
		/// </summary>
		public bool IsRemoveUserDialogOpened()
		{
			return Driver.WaitUntilElementIsEnabled(By.XPath(DELETE_BUTTON))
				&& Driver.WaitUntilElementIsAppear(By.XPath(DELETE_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = DELETE_BUTTON)]
		protected IWebElement DeleteButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string DELETE_BUTTON = "//div[contains(@class,'popupbox l-confirm')]//input[@value= 'Delete']";

		#endregion
	}
}
