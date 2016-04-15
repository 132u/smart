using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class AccountInvitationCompletePage : WorkspacePage, IAbstractPage<AccountInvitationCompletePage>
	{
		public AccountInvitationCompletePage(WebDriver driver) : base(driver)
		{
		}

		public new AccountInvitationCompletePage LoadPage()
		{
			if (!IsAccountInvitationCompletePageOpened())
			{
				throw new XPathLookupException("Произошла ошибка: не открылась страница завершения активации пользователя.");
			}

			return this;
		}

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница завершения активации пользователя.
		/// </summary>
		public bool IsAccountInvitationCompletePageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(COMPLETE_TEXT));
		}

		#endregion

		#region Описания XPath элементов

		protected const string COMPLETE_TEXT = "//p[contains(text(), 'new user has been created and added to the corporate account')]";

		#endregion
	}
}
