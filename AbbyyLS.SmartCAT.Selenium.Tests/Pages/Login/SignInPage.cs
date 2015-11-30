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
		public SelectAccountForm ClickSubmitButton()
		{
			CustomTestContext.WriteLine("Нажать 'Sign In'.");

			SubmitButton.JavaScriptClick();

			return new SelectAccountForm(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Sign In" ожидая сообщение об ошибке
		/// </summary>
		public SignInPage ClickSubmitButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать 'Sign In'.");

			SubmitButton.JavaScriptClick();

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
			ClickSubmitButton();

			return new SelectAccountForm(Driver).GetPage();
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

		[FindsBy(Using = SUBMIT_BTN_ID)]
		protected IWebElement SubmitButton { get; set; }

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

		protected const string LOGIN_FORM_XPATH = "//form[contains(@class, 'corp-login-form')]";

		protected const string EMAIL_INPUT_ID = "email";
		protected const string PASSWORD_INPUT_ID = "password";
		protected const string SUBMIT_BTN_ID = "btn-sign-in";

		protected const string ERROR_WRONG_PASSWORD = "//span[@translate='ERR-WRONG-PASSWORD']";
		protected const string ERROR_USER_NOT_FOUND = "//span[@translate='USER-NOT-FOUND-ERROR']";
		protected const string ERROR_EMPTY_PASSWORD = "//span[@translate='ERR-NO-PASSWORD']";
		protected const string ERROR_EMAIL_INVALID = "//span[@translate='EMAIL-INVALID']";

		protected const string FACEBOOK_ICON = "//a[@class='fb']";
		protected const string GOOGLE_ICON = "//a[@class='gplus']";
		protected const string LINKED_IN_ICON = "//a[@class='linkedin']";

		protected const string SIGN_UP_FREELANCE_BTN = "//a[@translate='FREELANCE']";
		protected const string SIGN_UP_COMPANY_BTN = "//a[@translate='CORPORATE']";

		#endregion
	}
}
