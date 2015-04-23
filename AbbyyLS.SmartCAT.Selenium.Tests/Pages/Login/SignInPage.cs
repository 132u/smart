using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class SignInPage : BaseObject, IAbstractPage<SignInPage>
	{
		public SignInPage GetPage()
		{
			var signInPage = new SignInPage();
			InitPage(signInPage);
			LoadPage();
			return signInPage;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(LOGIN_FORM_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница SignInPage (вход в смарткат).");
			}
		}

		/// <summary>
		/// Ввести логин
		/// </summary>
		/// <param name="email">емаил пользователя</param>
		public SignInPage SetLogin(string email)
		{
			Logger.Debug("Залогиниться в кат, ввести логин пользователя {0}.", email);
			Login.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Ввести пароль
		/// </summary>
		/// <param name="password">пароль пользователя</param>
		public SignInPage SetPassword(string password)
		{
			Logger.Debug("Ввести пароль пользователя {0}.", password);
			Password.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Нажать сабмит
		/// </summary>
		public SelectAccountForm ClickSubmitBtn()
		{
			Logger.Debug("Нажать 'Sign In'.");
			SubmitBtn.Click();
			var selectAccountForm = new SelectAccountForm();

			return selectAccountForm.GetPage();
		}

		[FindsBy(Using = EMAIL_INPUT_ID)]
		protected IWebElement Login { get; set; }

		[FindsBy(Using = PASSWORD_INPUT_ID)]
		protected IWebElement Password { get; set; }

		[FindsBy(Using = SUBMIT_BTN_ID)]
		protected IWebElement SubmitBtn { get; set; }

		protected const string LOGIN_FORM_XPATH = "//form[contains(@class, 'corp-login-form')]";
		protected const string EMAIL_INPUT_ID = "email";
		protected const string PASSWORD_INPUT_ID = "password";
		protected const string SUBMIT_BTN_ID = "btn-sign-in";
	}
}
