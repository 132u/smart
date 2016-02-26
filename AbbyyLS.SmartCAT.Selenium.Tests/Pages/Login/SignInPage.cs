﻿using System;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class SignInPage : BaseObject, IAbstractPage<SignInPage>
	{
		public WebDriver Driver { get; protected set; }

		public SignInPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public SignInPage GetPage()
		{
			var signInPage = new SignInPage(Driver);
			InitPage(signInPage, Driver);

			return signInPage;
		}

		public void LoadPage()
		{
			if (!IsSignInPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница SignInPage (вход в смарткат).");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести логин
		/// </summary>
		/// <param name="email">емаил пользователя</param>
		public SignInPage SetLogin(string email)
		{
			CustomTestContext.WriteLine("Ввести логин пользователя {0}.", email);

			Login.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Ввести пароль
		/// </summary>
		/// <param name="password">пароль пользователя</param>
		public SignInPage SetPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести пароль пользователя {0}.", password);

			Password.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Sign In"
		/// </summary>
		public SelectAccountForm ClickSignInButton()
		{
			CustomTestContext.WriteLine("Нажать 'Sign In'.");
			SignInButton.Click();

			return new SelectAccountForm(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Sign In"
		/// </summary>
		public WorkspacePage ClickSubmitButtonExpectingWorkspacePage()
		{
			CustomTestContext.WriteLine("Нажать 'Sign In'.");
			SignInButton.Click();

			return new WorkspacePage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать 'Sign In', ожидая открытие страницы выбора профиля.
		/// </summary>
		public SelectProfileForm ClickSubmitButtonExpectingSelectProfileForm()
		{
			CustomTestContext.WriteLine("Нажать 'Sign In', ожидая открытие страницы выбора профиля.");
			SignInButton.Click();

			return new SelectProfileForm(Driver).GetPage();
		}

		/// <summary>
		/// Нажать 'Sign In', ожидая открытие страницы создания аккаунта.
		/// </summary>
		public CreateAccountPage ClickSubmitButtonExpectingCreateAccountPage()
		{
			CustomTestContext.WriteLine("Нажать 'Sign In', ожидая открытие страницы создания аккаунта.");
			SignInButton.Click();

			return new CreateAccountPage(Driver).GetPage();
		}


		/// <summary>
		/// Нажать кнопку "Sign In" ожидая сообщение об ошибке
		/// </summary>
		public SignInPage ClickSubmitButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать 'Sign In'.");
			SignInButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать иконку Facebook
		/// </summary>
		public FacebookPage ClickFacebookIcon()
		{
			CustomTestContext.WriteLine("Нажать иконку Faceebook.");
			FacebookIcon.JavaScriptClick();

			return new FacebookPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать иконку Google+
		/// </summary>
		public GooglePage ClickGooglePlusIcon()
		{
			CustomTestContext.WriteLine("Нажать иконку Google+.");

			GoogleIcon.JavaScriptClick();

			return new GooglePage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать иконку LinkedIn
		/// </summary>
		public LinkedInPage ClickLinkedInIcon()
		{
			CustomTestContext.WriteLine("Нажать иконку LinkedIn.");

			LinkedInIcon.JavaScriptClick();

			return new LinkedInPage(Driver).GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Авторизация
		/// </summary>
		/// <param name="login">логин (email)</param>
		/// <param name="password">пароль</param>
		public SelectAccountForm SubmitForm(string login, string password)
		{
			SetLogin(login);
			SetPassword(password);
			ClickSignInButton();

			return new SelectAccountForm(Driver).GetPage();
		}

		/// <summary>
		/// Авторизация
		/// </summary>
		/// <param name="login">логин (email)</param>
		/// <param name="password">пароль</param>
		public WorkspacePage SubmitFormExpectingWorkspacePage(string login, string password)
		{
			SetLogin(login);
			SetPassword(password);
			ClickSubmitButtonExpectingWorkspacePage();

			return new WorkspacePage(Driver).GetPage();
		}

		/// <summary>
		/// Авторизация
		/// </summary>
		/// <param name="login">логин (email)</param>
		/// <param name="password">пароль</param>
		public SelectProfileForm SubmitFormExpectingSelectProfileForm(string login, string password)
		{
			SetLogin(login);
			SetPassword(password);
			ClickSubmitButtonExpectingSelectProfileForm();

			return new SelectProfileForm(Driver).GetPage();
		}

		/// <summary>
		/// Авторизация
		/// </summary>
		/// <param name="login">логин (email)</param>
		/// <param name="password">пароль</param>
		public CreateAccountPage SubmitFormExpectingCreateAccountForm(string login, string password)
		{
			SetLogin(login);
			SetPassword(password);
			ClickSubmitButtonExpectingCreateAccountPage();

			return new CreateAccountPage(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что на странице появилось сообщение о неправильном пароле
		/// </summary>
		public bool IsWrongPasswordMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что на странице появилось сообщение о неправильном пароле.");

            return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_WRONG_PASSWORD));
		}

		/// <summary>
		/// Проверить, что на странице появилось сообщение о ненайденном пользователе
		/// </summary>
		public bool IsUserNotFoundMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что на странице появилось сообщение о ненайденном пользователе.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_USER_NOT_FOUND));
		}

		/// <summary>
		/// Проверить, что на странице появилось сообщение о незаполненном пароле
		/// </summary>
		public bool IsEmptyPasswordMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что на странице появилось сообщение о незаполненном пароле.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_EMPTY_PASSWORD));
		}

		/// <summary>
		/// Проверить, что  сообщение о незаполненном пароле исчезло.
		/// </summary>
		public SignInPage WaitEmptyPasswordMessageDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что  сообщение о незаполненном пароле исчезло.");

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(ERROR_EMPTY_PASSWORD)))
			{
				throw new Exception("Произошла ошибка:\n Сообщение 'Enter password' не исчезло.");
			}

			return new SignInPage(Driver).GetPage();
		}

		/// <summary>
		/// Проверить, что на странице появилось сообщение о невалидном email
		/// </summary>
		public bool IsInvalidEmailMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что на странице появилось сообщение о невалидном email.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_EMAIL_INVALID));
		}

		/// <summary>
		/// Проверить, открыта ли станица авторизации
		/// </summary>
		public bool IsSignInPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(LOGIN_FORM_XPATH));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(Using = EMAIL_INPUT_ID)]
		protected IWebElement Login { get; set; }

		[FindsBy(Using = PASSWORD_INPUT_ID)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = SING_IN_BUTTON)]
		protected IWebElement SignInButton { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_WRONG_PASSWORD)]
		protected IWebElement WrongPasswordMessage { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_USER_NOT_FOUND)]
		protected IWebElement UserNotFoundMessage { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_EMPTY_PASSWORD)]
		protected IWebElement EmptyPasswordMessage { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_EMAIL_INVALID)]
		protected IWebElement InvalidEmailMessage { get; set; }

		[FindsBy(How = How.XPath, Using = GOOGLE_ICON)]
		protected IWebElement GoogleIcon { get; set; }

		[FindsBy(How = How.XPath, Using = FACEBOOK_ICON)]
		protected IWebElement FacebookIcon { get; set; }

		[FindsBy(How = How.XPath, Using = LINKED_IN_ICON)]
		protected IWebElement LinkedInIcon { get; set; }

		[FindsBy(How = How.XPath, Using = SIGN_UP_FREELANCE_BTN)]
		protected IWebElement SignUpAsFreelancerButton { get; set; }

		[FindsBy(How = How.XPath, Using = SIGN_UP_COMPANY_BTN)]
		protected IWebElement SignUpAsCompanyButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string LOGIN_FORM_XPATH = "//form[contains(@class, 'form_auth')]";

		protected const string EMAIL_INPUT_ID = "email";
		protected const string PASSWORD_INPUT_ID = "password";
		protected const string SING_IN_BUTTON= "//button[@click='signin']";

		protected const string ERROR_WRONG_PASSWORD = "//div[contains(text(),'Wrong password')]";
		protected const string ERROR_USER_NOT_FOUND = "//div[contains(text(), 'user with this email address does not exist')]";
		protected const string ERROR_EMPTY_PASSWORD = "//div[contains(text(),'Enter a password')]";
		protected const string ERROR_EMAIL_INVALID = "//div[contains(text(), 'Enter a valid email address')]";

		protected const string FACEBOOK_ICON = "//a[contains(@class, 'fb')]";
		protected const string GOOGLE_ICON = "//a[contains(@class, 'gplus')]";
		protected const string LINKED_IN_ICON = "//a[contains(@class, 'link_in')]";

		protected const string SIGN_UP_FREELANCE_BTN = "//a[@translate='FREELANCE']";
		protected const string SIGN_UP_COMPANY_BTN = "//a[@translate='CORPORATE']";

		#endregion
	}
}
