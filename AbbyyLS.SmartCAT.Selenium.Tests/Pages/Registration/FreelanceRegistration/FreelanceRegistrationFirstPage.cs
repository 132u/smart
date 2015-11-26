using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration.FreelanceRegistration
{
	class FreelanceRegistrationFirstPage : IAbstractPage<FreelanceRegistrationFirstPage>
	{
		public WebDriver Driver { get; protected set; }

		public FreelanceRegistrationFirstPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public FreelanceRegistrationFirstPage GetPage()
		{
			var freelanceRegistrationFirstPage = new FreelanceRegistrationFirstPage(Driver);
			LoadPage();

			return freelanceRegistrationFirstPage;
		}

		public void LoadPage()
		{
			if (!IsFreelanceRegistrationFirstPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась первая страница регистрации фрилансеров.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести email
		/// </summary>
		/// <param name="email">email</param>
		public FreelanceRegistrationFirstPage FillEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести email {0}.", email);
			Email.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Ввести пароль
		/// </summary>
		/// <param name="password">пароль</param>
		public FreelanceRegistrationFirstPage FillPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести пароль {0}.", password);
			Password.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Ввести подтверждение пароля
		/// </summary>
		/// <param name="confirmPassword">подтверждение пароля</param>
		public FreelanceRegistrationFirstPage FillConfirmPassword(string confirmPassword)
		{
			CustomTestContext.WriteLine("Ввести подтверждение пароля {0}.", confirmPassword);
			ConfirmPassword.SetText(confirmPassword);

			return GetPage();
		}
		
		/// <summary>
		/// Нажать кнопку Continue.
		/// </summary>
		public FreelanceRegistrationSecondPage ClickContinueButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Continue.");
			ContinueButton.Click();

			return new FreelanceRegistrationSecondPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Sign Up', ожидая ошибку.
		/// </summary>
		public FreelanceRegistrationFirstPage ClickSignUpButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Sign Up', ожидая ошибку.");
			ContinueButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на ссылку ' or sign in with an existing ABBYY account'
		/// </summary>
		public FreelanceRegistrationSignInPage ClickExistAccountAbbyyOnlineLink()
		{
			CustomTestContext.WriteLine("Нажать на ссылку ' or sign in with an existing ABBYY account'.");
			ExistAccountAbbyOnlineLink.Click();

			return new FreelanceRegistrationSignInPage(Driver).GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Заполнить данные на первом шаге регистрации фрилансера.
		/// </summary>
		/// <param name="email">email</param>
		/// <param name="password">пароль</param>
		/// <param name="confirmPassword">подтверждение пароля</param>
		public FreelanceRegistrationFirstPage FillFreelanceRegistrationFirstStep(
			string email,
			string password,
			string confirmPassword)
		{
			CustomTestContext.WriteLine("Заполнить данные на первом шаге регистрации фрилансера.");
			FillEmail(email);
			FillPassword(password);
			FillConfirmPassword(confirmPassword);

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась первая страница регистрации фрилансеров
		/// </summary>
		/// <returns></returns>
		public bool IsFreelanceRegistrationFirstPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(EMAIL));
		}

		/// <summary>
		/// Проверить, отображается ли сообщение о неверном подтверждении пароля
		/// </summary>
		public bool IsPasswordMatchErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, отображается ли сообщение о неверном подтверждении пароля.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(MISMATCH_PASSWORD_ERROR));
		}

		/// <summary>
		/// Проверить, что отображается сообщение о том, что пользователь уже зарегистрирован в системе
		/// </summary>
		public bool IsUserAlreadyExistErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, отображается ли сообщение о том, что пользователь уже зарегистрирован в системе.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(USER_IS_ALREADY_EXIST_ERROR));
		}

		/// <summary>
		/// Проверить, что отображается сообщение 'The password must have at least 6 characters'.
		/// </summary>
		public bool IsInvalidPasswordErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отображается сообщение 'The password must have at least 6 characters'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(INVALID_PASSWORD_ERROR));
		}

		/// <summary>
		/// Проверить, что кнопка Continue неактивна
		/// </summary>
		public bool IsContinueButtonDisabled()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка Continue неактивна.");

			return ContinueButton.GetAttribute("disabled") == "true";
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_PASSWORD)]
		protected IWebElement ConfirmPassword { get; set; }

		[FindsBy(How = How.XPath, Using = CONTINUE_BUTTON)]
		protected IWebElement ContinueButton { get; set; }

		[FindsBy(How = How.XPath, Using = EXIST_ACCOUNT_LINK_ABBY_ONLINE)]
		protected IWebElement ExistAccountAbbyOnlineLink { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string EMAIL = "//form[@name='signupForm']//input[@id='email']";
		protected const string PASSWORD = "//form[@name='signupForm']//input[@id='password']";
		protected const string CONFIRM_PASSWORD = "//input[@id='confirm']";
		protected const string CONTINUE_BUTTON = "//button[@id='btn-sign-up']";
		protected const string MISMATCH_PASSWORD_ERROR = "//span[contains(@ng-show, 'error.match ')]";
		protected const string USER_IS_ALREADY_EXIST_ERROR = "//div[@ng-message='already-exists']";
		protected const string EXIST_ACCOUNT_LINK_ABBY_ONLINE = "//a[@id='show-sign-in']";
		protected const string INVALID_PASSWORD_ERROR = "//div[contains(@ng-messages, 'signupForm.password.$error')]//span[text()='The password must have at least 6 characters']";

		#endregion
	}
}
