using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration
{
	class CompanyRegistrationFirstPage : IAbstractPage<CompanyRegistrationFirstPage>
	{
		public WebDriver Driver { get; protected set; }

		public CompanyRegistrationFirstPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public CompanyRegistrationFirstPage GetPage()
		{
			var companyRegistrationFirstPage = new CompanyRegistrationFirstPage(Driver);
			LoadPage();

			return companyRegistrationFirstPage;
		}

		public void LoadPage()
		{
			if (!IsCompanyRegistrationFirstPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась первая страница регистрации компаний.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Continue
		/// </summary>
		public CompanyRegistrationSecondPage ClickContinueButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Continue.");
			ContinueButton.JavaScriptClick();

			return new CompanyRegistrationSecondPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Continue, ожидая сообщение об ошибке
		/// </summary>
		public CompanyRegistrationFirstPage ClickContinueButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку Continue.");
			ContinueButton.JavaScriptClick();

			return GetPage();
		}

		/// <summary>
		/// Ввести email
		/// </summary>
		/// <param name="email">email</param>
		public CompanyRegistrationFirstPage FillEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле Email.", email);
			Email.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Ввести пароль
		/// </summary>
		/// <param name="password">пароль</param>
		public CompanyRegistrationFirstPage FillPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле пароля.", password);
			Password.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Ввести подтверждение пароля
		/// </summary>
		/// <param name="password">пароль</param>
		public CompanyRegistrationFirstPage FillConfirmPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле подтверждения пароля.", password);
			ConfirmPassword.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Нажать по ссылке 'or sign in with an existing ABBYY account'
		/// </summary>
		public CompanyRegistrationSignInPage ClickExistingAbbyyAccountLink()
		{
			CustomTestContext.WriteLine("Нажать по ссылке 'or sign in with an existing ABBYY account'.");
			ExistingAbbyyAccountLink.Click();

			return new CompanyRegistrationSignInPage(Driver).GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Заполнить форму регистрации на первом шаге
		/// </summary>
		/// <param name="email">email</param>
		/// <param name="password">пароль</param>
		/// <param name="confirmPassword">подтверждение пароля</param>
		public CompanyRegistrationFirstPage FillCompanyDataFirstStep(
			string email,
			string password,
			string confirmPassword)
		{
			FillEmail(email);
			FillPassword(password);
			FillConfirmPassword(confirmPassword);

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что кнопка Continue неактивна
		/// </summary>
		public bool IsContinueButtonInactive()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка Continue неактивна.");

			return ContinueButton.GetAttribute("disabled") == "true";
		}

		/// <summary>
		/// Проверить, что сообщение 'Invalid email' появилось
		/// </summary>
		public bool IsInvalidEmailMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'Invalid email' появилось.");

            return Driver.WaitUntilElementIsDisplay(By.XPath(INVALID_EMAIL_MESSAGE));
		}

		/// <summary>
		/// Проверить, что сообщение 'The password must have at least 6 characters' появилось
		/// </summary>
		public bool IsMinimumLenghPasswordMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'The password must have at least 6 characters' появилось.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(MINIMUM_LENGHT_PASSWORD_MESSAGE));
		}

		/// <summary>
		/// Проверить, что сообщение 'The password cannot consist of spaces only' появилось
		/// </summary>
		public bool IsOnlySpacesPasswordMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'The password cannot consist of spaces only' появилось.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ONLY_SPACES_PASSWORD_MESSAGE));
		}

		/// <summary>
		/// Проверить, что сообщение 'The passwords do not match' появилось
		/// </summary>
		public bool IsPasswordMatchMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'The passwords do not match' появилось.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(PASSWORD_MATCH_MESSAGE));
		}

		/// <summary>
		/// Проверить, что сообщение 'You have already signed up for one of the ABBYY services with this email' появилось
		/// </summary>
		public bool IsAlreadySignUpMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'You have already signed up for one of the ABBYY services with this email.' появилось.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ALREADY_SIGN_UP_MESSAGE));
		}

		/// <summary>
		/// Проверить, что сообщение 'The password must have at least 6 characters.' появилось
		/// </summary>
		public bool IsInvalidPasswordMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'The password must have at least 6 characters.' появилось.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(INVALID_PASSWORD_MESSAGE));
		}

		/// <summary>
		/// Проверить, открыта ли страница
		/// </summary>
		public bool IsCompanyRegistrationFirstPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_PASSWORD));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.Id, Using = CONTINUE_BUTTON)]
		protected IWebElement ContinueButton { get; set; }

		[FindsBy(How = How.XPath, Using = EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_PASSWORD)]
		protected IWebElement ConfirmPassword { get; set; }

		[FindsBy(How = How.Id, Using = EXISTING_ABBYY_ACCOUNT_LINK)]
		protected IWebElement ExistingAbbyyAccountLink { get; set; }

		[FindsBy(How = How.XPath, Using = INVALID_EMAIL_MESSAGE)]
		protected IWebElement InvalidEmailMessage { get; set; }

		[FindsBy(How = How.XPath, Using = ONLY_SPACES_PASSWORD_MESSAGE)]
		protected IWebElement OnlySpacesPasswordMessage { get; set; }

		[FindsBy(How = How.XPath, Using = MINIMUM_LENGHT_PASSWORD_MESSAGE)]
		protected IWebElement MinimumLenghPasswordMessage { get; set; }

		[FindsBy(How = How.XPath, Using = ALREADY_SIGN_UP_MESSAGE)]
		protected IWebElement AlreadySignUpMessage { get; set; }
		
		[FindsBy(How = How.XPath, Using = PASSWORD_MATCH_MESSAGE)]
		protected IWebElement PasswordMatchMessage { get; set; }

		[FindsBy(How = How.XPath, Using = INVALID_PASSWORD_MESSAGE)]
		protected IWebElement InvalidPasswordMessage { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string CONTINUE_BUTTON = "btn-sign-up";
		protected const string EMAIL = "//form[@name='signupForm']//input[@id='email']";
		protected const string PASSWORD = "//form[@name='signupForm']//input[@id='password']";
		protected const string EXISTING_ABBYY_ACCOUNT_LINK = "show-sign-in";
		protected const string CONFIRM_PASSWORD = "//form[@name='signupForm']//input[@id='confirm']";

		protected const string INVALID_EMAIL_MESSAGE = "//span[contains(@ng-show,'signupForm.email')]";
		protected const string MINIMUM_LENGHT_PASSWORD_MESSAGE = "//fieldset[contains(@valid,'password')]//div[contains(@ng-message,'minlength')]//span[not(contains(@class,'ng-hide'))]";
		protected const string ONLY_SPACES_PASSWORD_MESSAGE = "//div[contains(@ng-message,'pattern')]//span[not(contains(@class,'ng-hide'))]";
		protected const string PASSWORD_MATCH_MESSAGE = "//span[contains(@ng-show, 'error.match ')]";
		protected const string ALREADY_SIGN_UP_MESSAGE = "//div[@ng-message='already-exists']";
		protected const string INVALID_PASSWORD_MESSAGE = "//span[@translate='PASSWORD-INVALID']";

		#endregion
	}
}
