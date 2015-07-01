using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration
{
	internal class CompanyRegistrationSignInPage : WorkspacePage, IAbstractPage<CompanyRegistrationSignInPage>
	{
		public new CompanyRegistrationSignInPage GetPage()
		{
			var companyExistingAccountRegistrationFirstPage = new CompanyRegistrationSignInPage();
			InitPage(companyExistingAccountRegistrationFirstPage);

			return companyExistingAccountRegistrationFirstPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(PASSWORD))
				|| Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_PASSWORD)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница входа для регистрации компаний с существующим аккаунтом.");
			}
		}

		/// <summary>
		/// Ввести email
		/// </summary>
		/// <param name="email">email</param>
		public CompanyRegistrationSignInPage FillEmail(string email)
		{
			Logger.Debug("Ввести {0} в поле Email.", email);
			Email.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Ввести пароль
		/// </summary>
		///  <param name="password">пароль</param>
		public CompanyRegistrationSignInPage FillPassword(string password)
		{
			Logger.Debug("Ввести {0} в поле пароля.", password);
			Password.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Sign In'
		/// </summary>
		public CompanyRegistrationSecondPage ClickSignInButton()
		{
			Logger.Debug("Нажать кнопку 'Sign In'.");
			SignInButton.Click();

			return new CompanyRegistrationSecondPage().GetPage();
		}

		[FindsBy(How = How.XPath, Using = EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.Id, Using = SIGN_IN_BUTTON)]
		protected IWebElement SignInButton { get; set; }

		protected const string EMAIL = "//form[@name='signinForm']//input[@id='email']";
		protected const string PASSWORD = "//form[@name='signinForm']//input[@id='password']";
		protected const string SIGN_IN_BUTTON = "btn-sign-in";
		protected const string CONFIRM_PASSWORD = "//form[@name='signupForm']//input[@id='confirm']";
	}
}
