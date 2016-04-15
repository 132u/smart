using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class AccountInvitationPage : WorkspacePage, IAbstractPage<AccountInvitationPage>
	{
		public AccountInvitationPage(WebDriver driver) : base(driver)
		{
		}

		public new AccountInvitationPage LoadPage()
		{
			if (!IsAccountInvitationPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка: не открылась страница активации пользователя.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Кликнуть на ссылку актвации пользователя.
		/// </summary>
		public AccountInvitationCompletePage ClickActivationLink()
		{
			CustomTestContext.WriteLine("Кликнуть на ссылку актвации пользователя.");
			ActivationLink.Click();

			return  new AccountInvitationCompletePage(Driver).LoadPage();
		}

		/// <summary>
		/// Получить пароль.
		/// </summary>
		public string GetPassword()
		{
			CustomTestContext.WriteLine("Получить пароль.");

			return Password.Text.Split(new[] { ':' })[1].Trim();
		}

		/// <summary>
		/// Получить email.
		/// </summary>
		public string GetEmail()
		{
			CustomTestContext.WriteLine("Получить email.");

			return Email.Text.Split(new[] { ':' })[1].Trim();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Проверить, что открылась страница письма
		/// </summary>
		public bool IsAccountInvitationPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(GO_TO_MAIN_PAGE));
		}

		/// <summary>
		/// Проверить, что пароль отображается
		/// </summary>
		public bool IsPasswordDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что пароль отображается.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(PASSWORD));
		}

		/// <summary>
		/// Проверить, что сообщение 'Owner of email has been added to the corporate account'
		/// содержит верную информацию о email.
		/// </summary>
		/// <param name="email">email</param>
		public bool IsCorporateAccountMessageContainsCorrectEmail(string email)
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'Owner of email has been added to the corporate account'"
				+ " содержит верную информацию о email");

			return CorporateAccountMessage.Text.Contains(email);
		}

		#endregion
		
		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = ACTIVATION_LINK)]
		protected IWebElement ActivationLink { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = CORPORATE_ACCOUNT_MESSAGE)]
		protected IWebElement CorporateAccountMessage { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string ACTIVATION_LINK = "//a[contains(@href, 'Anonymous=true')]";
		protected const string PASSWORD = "//p[contains(text(), 'Password:')]";
		protected const string EMAIL = "//p[contains(text(), 'Email:')]";
		protected const string GO_TO_MAIN_PAGE = "//a[text()='Go to main page']";
		protected const string CORPORATE_ACCOUNT_MESSAGE = "//p[contains(string(), 'has been added to the corporate account')]";

		#endregion
	}
}
