using System;
using System.Windows.Forms.VisualStyles;
using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки формы входа
	/// </summary>
	class AuthorizationTest<TWebDriverSettings> : AdminTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[SetUp]
		public void SetUp()
		{
			if (Standalone)
			{
				Assert.Ignore("Тест игнорируется, так как это отделяемое решение");
			}
			
			_email = "TestEmail@" + RandomString.Generate(10) + ".com";
			_password = "TestPassword" + RandomString.Generate(10);
			_nickName = "TestNickName" + RandomString.Generate(10);
			_persAccount = "PersAccount" + RandomString.Generate(10);
			_corpAccount = "CorpAccount" + RandomString.Generate(10);
		}

		private string _email;

		private string _password;

		private string _nickName;

		private string _persAccount;

		private string _corpAccount;

		/// <summary>
		/// Авторизация пользователя
		/// </summary>
		[Test]
		public void AuthorizationMethodTest()
		{
			Authorization(Login, Password);
		}

		/// <summary>
		/// Авторизация пользователя через соц сети
		/// </summary>
		[Category("ForLocalRun")]
		[TestCase("Facebook", "margarita.kolly@yandex.ru", "0onWolkap")]
		[TestCase("Google", "smaartcat@gmail.com", "smaartcattest")]
		[TestCase("LinkedIn", "margarita.kolly@yandex.ru", "0onWolkap")]
		public void LoginViaSocialNetwork(string site, string email, string password)
		{
			GoToSignInPage();
			
			switch (site)
			{
				case "Facebook":
					LoginPage.ClickFacebookIcon();
					Assert.IsTrue(SocialNetworkPage.WaitFacebookIsLoad(), "Ошибка: " + site + " не загрузился");
					LoginInFacebook(email, password);
					break;

				case "Google":
					LoginPage.ClickGoogleIcon();
					Assert.IsTrue(SocialNetworkPage.WaitGoogleIsLoad(), "Ошибка: " + site + " не загрузился");
					LoginInGoogle(email, password);
					break;

				case "LinkedIn":
					LoginPage.ClickLinkedInIcon();
					Assert.IsTrue(SocialNetworkPage.WaitLinkedInIsLoad(), "Ошибка: " + site + " не загрузился");
					LoginInLinkedIn(email, password);
					break;

				default: 
					throw new Exception("Передано неправильное название сайта");
			}

			if (LoginPage.GetErrorNoServerDisplay()) Logger.Trace("На странице ошибка, сервер(ы) недоступен(ны): " + LoginPage.GetErrorNoServerMessage());

			Assert.IsTrue(
				(GetLoginSuccess() || GetStartSignUpPageDisplay()),
				"Ошибка: ни страница WS ни страница выбора аккаунта ни страница SignUp не открылись после авторизации через " + site);
		}

		/// <summary>
		/// Авторизация пользователя и выход из системы
		/// </summary>
		[Test]
		public void SignInAndSignOut()
		{
			// Авторизация
			Authorization(Login, Password);
			Assert.IsTrue(
				WorkspacePage.WaitPageLoad(),
				"Ошибка: страница WS не открылась после авторизации (не нашли кнопку 'Create Project')");
			// Выход
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();
			Assert.IsTrue(GetLoginPageDisplay(), "Ошибка: страница авторизации не открылась после выхода из системы");
		}

		/// <summary>
		/// Авторизация с неверными данными пользователя
		/// </summary>
		[TestCase("bobby@mailforspam.com", "wrongPassword", "wrongPassword")] // неправильный пароль
		[TestCase("bobby@mailforspam.com", "", "emptyPassword")] // пустой пароль
		[TestCase("", "YrdyNpnnu", "emptyEmail")] // пустой email
		[TestCase("noFoundEmail@mailforspam.com", "YrdyNpnnu", "notFoundEmail")] // несуществующий email
		[TestCase("invalidEmail", "YrdyNpnnu", "invalidEmail")] // неправильный email, без символа @
		public void LoginIncorrectCredentials(string email, string password, string error)
		{
			LogIn(email, password);
			switch (error)
			{
				case "wrongPassword":
					Assert.IsTrue(
						LoginPage.GetErrorWrongPasswordDisplay(), "Ошибка: сообщение о неправильном пароле не появилось");
					break;

				case "emptyPassword":
					Assert.IsTrue(
						LoginPage.GetErrorNoPasswordDisplay(), "Ошибка: сообщение о пустом пароле не появилось");
					break;

				case "emptyEmail":
					Assert.IsTrue(
						LoginPage.GetErrorNoEmailDisplay(), "Ошибка: сообщение о пустом email не появилось");
					break;

				case "notFoundEmail":
					Assert.IsTrue(
						LoginPage.GetErrorNoFoundEmailDisplay(), "Ошибка: сообщение о том, что email не найден, не появилось");
					break;

				case "invalidEmail":
					Assert.IsTrue(
						LoginPage.GetErrorInvalidEmailDisplay(), "Ошибка: сообщение о невалидном email не появилось");
					break;
			}
		}

		/// <summary>
		/// Авторизация пользователя без аккаунта
		/// </summary>
		[Test]
		public void LoginWithoutAccount()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(_email, _nickName, _password);
			LogIn(_email, _password);
			Assert.IsTrue(
				GetStartSignUpPageDisplay(),
				"Ошибка: страница предложения создать аккаунт не открылась после авторизации (для пользователя без аккаунта)");
		}

		/// <summary>
		/// Авторизация пользователя с персональным аккаунтом
		/// </summary>
		[Test]
		public void LoginPersonalAccount()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(_email, _nickName, _password);
			CreateNewPersonalAccount(_persAccount, true);
			LogIn(_email, _password);
			Assert.IsTrue(
				LoginPage.WaitAccountExist("Personal"),
				"Ошибка: название аккаунта Personal не отображается на странице выбора аккаунта");
		}

		/// <summary>
		/// Авторизация пользователя с персональным и корпоративным аккаунтом 
		/// </summary>
		[Test]
		public void LoginPersonalAndCorporateAccount()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(_email, _nickName, _password);
			CreateNewPersonalAccount(_persAccount, true);
			CreateCorporateAccount(_corpAccount, true);
			AddUserToSpecifyAccount(_email, _corpAccount);
			LogIn(_email, _password);
			Assert.IsTrue(
				LoginPage.WaitAccountExist("Personal"),
				"Ошибка: название аккаунта Personal не отображается на странице выбора аккаунта");

			Assert.IsTrue(
				LoginPage.WaitAccountExist(_corpAccount),
				"Ошибка: название аккаунта не отображается на странице выбора аккаунта");
		}

		/// <summary>
		/// Авторизация пользователя с корпоративным аккаунтом 
		/// </summary>
		[Test]
		public void LoginCorporateAccount()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(_email, _nickName, _password);
			CreateCorporateAccount(_corpAccount, true);
			AddUserToSpecifyAccount(_email, _corpAccount);
			LogIn(_email, _password);

			Assert.IsTrue(
				LoginPage.WaitAccountExist(_corpAccount),
				"Ошибка: название аккаунта не отображается на странице выбора аккаунта");
		}

		/// <summary>
		/// Проверка возможности выбирать аккаунт
		/// </summary>
		[Test]
		public void SelectAccount()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(_email, _nickName, _password);
			CreateCorporateAccount(_corpAccount, true);
			AddUserToSpecifyAccount(_email, _corpAccount);
			LogIn(_email, _password);
			Assert.IsTrue(
				LoginPage.WaitAccountExist(_corpAccount),
				"Ошибка: название аккаунта не отображается на странице выбора аккаунта");
			LoginPage.ClickAccountName(_corpAccount);
			Assert.IsTrue(WorkspacePage.WaitPageLoad(), "Ошибка: страница WS не загрузилась");
			Assert.IsTrue(
				WorkspacePage.GetCompanyName() == _corpAccount,
				"Ошибка: название корпоративного аккаунта не отображается в черной плашке WS");
		}

		/// <summary>
		/// Проверка возможности выбирать аккаунт Coursera
		/// </summary>
		[TestCase("Coursera")]
		[TestCase("Perevedem")]
		public void LoginVenture(string venture)
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(_email, _nickName, _password);
			AddUserToSpecifyAccount(_email, venture);
			LogIn(_email, _password);

			switch (venture)
			{
				case "Coursera":
					Assert.IsTrue(GetStartSignUpPageDisplay(),
						"Ошибка: не открылось окно выбора аккаунта для регистрации (фрилансер или корпоративный).");
					break;

				case "Perevedem":
					if (!WorkspacePage.WaitPageLoad() && LoginPage.GetAccountsCount() > 1)
						LoginPage.ClickAccountName(venture);

					Assert.IsTrue(WorkspacePage.WaitPageLoad(),
						"Ошибка: Не загрузилась страница workspace (т.к. аккаунт один, должна произойти автоматическая переадресация).");

					Assert.IsTrue(WorkspacePage.GetCompanyName() == venture,
						"Ошибка: Имя компании не соответствует ожидаемому.");
					break;

				default:
					Assert.Fail("В тест передан не верный аргумент, который не предусмотрен!");
					break;
			}
		}

		/// <summary>
		/// Авторизация пользователя с неактивными аккаунтом
		/// </summary>
		[Test]
		public void LoginInactiveAccount()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(_email, _nickName, _password);
			CreateNewPersonalAccount(_persAccount, false);
			LogIn(_email, _password);
			Assert.IsTrue(
				GetStartSignUpPageDisplay(), "Ошибка: страница предложения создать аккаунт не открылась после авторизации"
				+ " (для пользователя с неактивным персональным аккаунтом)");
		}

		/// <summary>
		/// Авторизация пользователя с неактивированным аккаунтом в aol
		/// </summary>
		[Category("ForLocalRun")]
		[TestCase(0, "active")]
		[TestCase(1, "inactive")]
		public void LoginActiveAolAccount(int userNumber, string active)
		{
			if (!AolUserFileExist() || (AolUserList.Count == 0))
			{
				Assert.Ignore("Файл AolUsers.xml с тестовыми пользователями отсутствует или в файле нет данных о юзере");
			}

			LogIn(AolUserList[userNumber].Login, AolUserList[userNumber].Password);
			
			if (GetStartSignUpPageDisplay()) 
				Assert.Pass();

			RefreshPage();
			Assert.IsTrue(GetStartSignUpPageDisplay(),
				"Ошибка: страница предложения создать аккаунт не открылась после авторизации" + 
				" (для пользователя с " + active + " AOL аккаунтом)");
		}

		/// <summary>
		/// Авторизация в Facebook
		/// </summary>
		/// <param name="email"> email </param>
		/// <param name="password"> пароль </param>
		protected void LoginInFacebook(string email, string password)
		{
			Logger.Trace("Авторизация в Facebook");
			SocialNetworkPage.FillEmailFieldFacebook(email);
			SocialNetworkPage.FillPasswordFieldFacebook(password);
			SocialNetworkPage.ClickSubmitBtnFacebook();
		}

		/// <summary>
		/// Авторизация в LinkedIn
		/// </summary>
		/// <param name="email"> email </param>
		/// <param name="password"> пароль </param>
		protected void LoginInLinkedIn(string email, string password)
		{
			Logger.Trace("Авторизация в LinkedIn");
			SocialNetworkPage.FillEmailFieldLinkedIn(email);
			SocialNetworkPage.FillPasswordFieldLinkedIn(password);
			SocialNetworkPage.ClickSubmitBtnLinkedIn();
		}

		/// <summary>
		/// Авторизация в Google+
		/// </summary>
		/// <param name="email"> email </param>
		/// <param name="password"> пароль </param>
		protected void LoginInGoogle(string email, string password)
		{
			Logger.Trace("Авторизация в Google+");
			SocialNetworkPage.FillEmailFieldGoogle(email);
			SocialNetworkPage.FillPasswordFieldGoogle(password);
			SocialNetworkPage.ClickSubmitBtnGoogle();
		}

		/// <summary>
		/// Проверить открылась ли страница выбора аккаунта или страница WS после авторизации
		/// </summary>
		protected bool GetLoginSuccess()
		{
			return (LoginPage.GetSignInHeaderDisplay()
				&& (LoginPage.CheckEuropeServerIsDisplayed() || LoginPage.CheckUsaServerIsDisplayed()))
				|| WorkspacePage.WaitPageLoad();
		}

		/// <summary>
		/// Проверить открылась ли страница авторизации
		/// </summary>
		protected bool GetLoginPageDisplay()
		{
			return LoginPage.GetEmailFieldIsDisplay() && LoginPage.GetPasswordFieldIsDisplay()
				&& LoginPage.GetSignInHeaderDisplay();
		}

		/// <summary>
		/// Проверить, что страница предложения создать аккаунт открылась после авотризации (для пользователя без аккаунта)
		/// </summary>
		/// <returns></returns>
		protected bool GetStartSignUpPageDisplay()
		{
			return LoginPage.GetMessageAccountNotFoundDisplay() && LoginPage.GetSignUpCompanyBtnDisplay()
				&& LoginPage.GetSignUpFreelanceBtnDisplay();
		}

		/// <summary>
		/// Ввод данных на странице авторизации
		/// </summary>
		protected void LogIn(string email, string password)
		{
			GoToSignInPage();
			LoginPage.EnterLogin(email);
			LoginPage.EnterPassword(password);
			LoginPage.ClickSubmitAuthEmail();
		}

		protected void GoToSignInPage()
		{
			Logger.Trace("Переход на страницу Sign In");
			Driver.Navigate().GoToUrl(Url + RelativeUrlProvider.SingIn);
			LoginPage.SelectLocale(LOCALE_LANGUAGE_SELECT.English);
			Assert.IsTrue(GetLoginPageDisplay(), "Ошибка: страница авторизации не открылась");
		}
	}
}
