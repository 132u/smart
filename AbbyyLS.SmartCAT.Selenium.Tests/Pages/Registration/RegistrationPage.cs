using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration
{
	public class RegistrationPage : IAbstractPage<RegistrationPage>
	{
		public WebDriver Driver;

		public RegistrationPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public RegistrationPage GetPage(string email = null)
		{
			var newRegistrationUrl = 
				(email == null)
				? RelativeUrlProvider.Registratioin + Guid.NewGuid() + "@mailforspam.com"
				: RelativeUrlProvider.Registratioin + email;

			CustomTestContext.WriteLine("Переход на страницу регистрации компаний: {0}.", ConfigurationManager.Url + newRegistrationUrl);
			Driver.Navigate().GoToUrl(ConfigurationManager.Url + newRegistrationUrl);

			return new RegistrationPage(Driver).LoadPage();
		}

		public SignInPage GetPageExpectingRedirectToSignInPage(string email)
		{
			var newRegistrationUrl = RelativeUrlProvider.Registratioin + email;

			CustomTestContext.WriteLine("Переход на страницу регистрации: {0}.", ConfigurationManager.Url + newRegistrationUrl);
			Driver.Navigate().GoToUrl(ConfigurationManager.Url + newRegistrationUrl);

			return new SignInPage(Driver).LoadPage();
		}

		public RegistrationPage LoadPage()
		{
			if (!IsRegistrationPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка: не открылась страница регистрации");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Кликнуть по форме регистрации фрилансера, для активации
		/// </summary>
		public RegistrationPage ClickFreelancerForm()
		{
			CustomTestContext.WriteLine("Кликнуть по форме регистрации фрилансера, для активации");
			FreelancerForm.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по форме регистрации корпоративного пользователя, для активации
		/// </summary>
		public RegistrationPage ClickCorporateAccountForm()
		{
			CustomTestContext.WriteLine("Кликнуть по форме регистрации корпоративного пользователя, для активации");
			CorpForm.Click();

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле 'Password' в форме регистрации фрилансера
		/// </summary>
		/// <param name="password">пароль</param>
		public RegistrationPage FillFreelancerPassword(string password)
		{
			CustomTestContext.WriteLine("Заполнить поле 'Password' в форме регистрации фрилансера");
			FreelancerPasswordField.SetText(password);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле 'First and last name' в форме регистрации фрилансера
		/// </summary>
		/// <param name="firstAndLastName">имя и фамилия</param>
		public RegistrationPage FillFreelancerFirstAndLastName(string firstAndLastName)
		{
			CustomTestContext.WriteLine("Заполнить поле 'First and last name' в форме регистрации фрилансера");
			FreelancerFirstAndLastNameField.SetText(firstAndLastName);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле 'Password' в форме регистрации корпоративного пользователя
		/// </summary>
		/// <param name="password">пароль</param>
		public RegistrationPage FillCorporateAccountPassword(string password)
		{
			CustomTestContext.WriteLine("Заполнить поле 'Password' в форме регистрации корпоративного пользователя");
			CorpPasswordField.SetText(password);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле 'First and last name' в форме регистрации корпоративного пользователя
		/// </summary>
		/// <param name="firstAndLastName">имя и фамилия</param>
		public RegistrationPage FillCorporateAccountFirstAndLastName(string firstAndLastName)
		{
			CustomTestContext.WriteLine("Заполнить поле 'First and last name' в форме регистрации корпоративного пользователя");
			CorpFirstAndLastNameField.SetText(firstAndLastName);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле 'Phone' в форме регистрации корпоративного пользователя
		/// </summary>
		/// <param name="phoneNumber">номер телефона</param>
		public RegistrationPage FillCorporateAccountPhone(string phoneNumber)
		{
			CustomTestContext.WriteLine("Заполнить поле 'Phone' в форме регистрации корпоративного пользователя");
			CompanyPhoneField.SetText(phoneNumber);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле 'Company Name' в форме регистрации корпоративного пользователя
		/// </summary>
		/// <param name="companyName">название компании</param>
		public RegistrationPage FillCorporateAccountCompanyName(string companyName)
		{
			CustomTestContext.WriteLine("Заполнить поле 'Company Name' в форме регистрации корпоративного пользователя");
			CompanyNameField.SetText(companyName);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Confirm'
		/// </summary>
		public WorkspacePage ClickConfirmButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Confirm'");
			ConfirmButton.Click();

			return new WorkspacePage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Confirm', ожидая сообщение об ошибке
		/// </summary>
		public RegistrationPage ClickConfirmButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Confirm', ожидая сообщение об ошибке");
			ConfirmButton.Click();

			return LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Заполнить форму регистрации корпоративного пользователя
		/// </summary>
		/// <param name="password">пароль</param>
		/// <param name="firstAndLastName">имя и фамилия</param>
		/// <param name="phone">номер телефона</param>
		/// <param name="companyName">название компании</param>
		public RegistrationPage FillCorporateAccountRegistrationForm(
			string password,
			string firstAndLastName,
			string phone,
			string companyName)
		{
			ClickCorporateAccountForm();
			FillCorporateAccountPassword(password);
			FillCorporateAccountFirstAndLastName(firstAndLastName);
			FillCorporateAccountPhone(phone);
			FillCorporateAccountCompanyName(companyName);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить форму регистрации фрилансера
		/// </summary>
		/// <param name="password">пароль</param>
		/// <param name="firstAndLastName">имя и фамилия</param>
		public RegistrationPage FillFreelancerRegistrationForm(string password, string firstAndLastName)
		{
			ClickFreelancerForm();
			FillFreelancerPassword(password);
			FillFreelancerFirstAndLastName(firstAndLastName);

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница регистрации
		/// </summary>
		public bool IsRegistrationPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(FORM_HEADER));
		}

		/// <summary>
		/// Проверить, появилось ли сообщение об ошибке в поле 'Password' для фрилансера
		/// </summary>
		public bool IsEnterPasswordErrorMessageForFreelancerDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение об ошибке в поле 'Password' для фрилансера");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ENTER_PASSWORD_ERROR_MESSAGE_FREELANCER));
		}

		/// <summary>
		/// Проверить, появилось ли сообщение об ошибке в поле 'First and last name' для фрилансера
		/// </summary>
		public bool IsEnterFirstAndLastNameErrorMessageForFreelancerDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение об ошибке в поле 'First and last name' для фрилансера");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ENTER_FIRST_AND_LAST_NAME_ERROR_MESSAGE_FREELANCER));
		}

		/// <summary>
		/// Проверить, появилось ли сообщение об ошибке в поле 'Password' для корпоративного пользователя
		/// </summary>
		public bool IsEnterPasswordErrorMessageForCorporateAccountDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение об ошибке в поле 'Password' корпоративного пользователя");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ENTER_PASSWORD_ERROR_MESSAGE_CORP));
		}

		/// <summary>
		/// Проверить, появилось ли сообщение об ошибке в поле 'First and last name' корпоративного пользователя
		/// </summary>
		public bool IsEnterFirstAndLastNameErrorMessageForCorporateAccountDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение об ошибке в поле 'First and last name' корпоративного пользователя");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ENTER_FIRST_AND_LAST_NAME_ERROR_MESSAGE_CORP));
		}

		/// <summary>
		/// Проверить, появилось ли сообщение об ошибке в поле 'Phone' корпоративного пользователя
		/// </summary>
		public bool IsEnterPhoneErrorMessageForCorporateAccountDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение об ошибке в поле 'Phone' корпоративного пользователя");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ENTER_PHONE_ERROR_MESSAGE_CORP));
		}

		/// <summary>
		/// Проверить, появилось ли сообщение об ошибке в поле 'Company Name' корпоративного пользователя
		/// </summary>
		public bool IsEnterCompanyNameErrorMessageForCorporateAccountDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение об ошибке в поле 'Company Name' корпоративного пользователя");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ENTER_COMPANY_NAME_ERROR_MESSAGE_CORP));
		}

		/// <summary>
		/// Проверить, появилось ли сообщение о слишком коротком пароле для фрилансера
		/// </summary>
		public bool IsShortPasswordErrorMessageForFreelancerDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение о слишком коротком пароле для фрилансера");

			return Driver.WaitUntilElementIsDisplay(By.XPath(SHORT_PASSWORD_ERROR_MESSAGE_FREELANCER));
		}

		/// <summary>
		/// Проверить, появилось ли сообщение о слишком коротком пароле для корпоративного пользователя
		/// </summary>
		public bool IsShortPasswordErrorMessageForCorporateAccountDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение о слишком коротком пароле для корпоративного пользователя");

			return Driver.WaitUntilElementIsDisplay(By.XPath(SHORT_PASSWORD_ERROR_MESSAGE_CORP));
		}

		/// <summary>
		/// Проверить, появилось ли сообщение о слишком коротком названии компании для корпоративного пользователя
		/// </summary>
		public bool IsShortCompanyNameErrorMessageForCorporateAccountDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение о слишком коротком названии компании для корпоративного пользователя");

			return Driver.WaitUntilElementIsDisplay(By.XPath(SHORT_COMPANY_NAME_ERROR_MESSAGE_CORP));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = FREELANCER_FORM)]
		private IWebElement FreelancerForm { get; set; }

		[FindsBy(How = How.XPath, Using = FREELANCER_PASSWORD_FIELD)]
		private IWebElement FreelancerPasswordField { get; set; }

		[FindsBy(How = How.XPath, Using = FREELANCER_FIRST_AND_LAST_NAME_FIELD)]
		private IWebElement FreelancerFirstAndLastNameField { get; set; }

		[FindsBy(How = How.XPath, Using = CORP_FORM)]
		private IWebElement CorpForm { get; set; }

		[FindsBy(How = How.XPath, Using = CORP_PASSWORD_FIELD)]
		private IWebElement CorpPasswordField { get; set; }

		[FindsBy(How = How.XPath, Using = CORP_FIRST_AND_LAST_NAME_FIELD)]
		private IWebElement CorpFirstAndLastNameField { get; set; }

		[FindsBy(How = How.XPath, Using = CORP_PHONE_FIELD)]
		private IWebElement CompanyPhoneField { get; set; }

		[FindsBy(How = How.XPath, Using = CORP_NAME_FIELD)]
		private IWebElement CompanyNameField { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_BUTTON)]
		private IWebElement ConfirmButton { get; set; }

		[FindsBy(How = How.XPath, Using = ENTER_PASSWORD_ERROR_MESSAGE_FREELANCER)]
		private IWebElement EnterPasswordErrorMessageFreelancer { get; set; }

		[FindsBy(How = How.XPath, Using = ENTER_FIRST_AND_LAST_NAME_ERROR_MESSAGE_FREELANCER)]
		private IWebElement EnterFirstAndLastNameErrorMessageFrelancer { get; set; }

		[FindsBy(How = How.XPath, Using = ENTER_PASSWORD_ERROR_MESSAGE_CORP)]
		private IWebElement EnterPasswordErrorMessageCorp { get; set; }

		[FindsBy(How = How.XPath, Using = ENTER_FIRST_AND_LAST_NAME_ERROR_MESSAGE_CORP)]
		private IWebElement EnterFirstAndLastNameErrorMessageCorp { get; set; }

		[FindsBy(How = How.XPath, Using = ENTER_PHONE_ERROR_MESSAGE_CORP)]
		private IWebElement EnterPhoneErrorMessageCorp { get; set; }

		[FindsBy(How = How.XPath, Using = ENTER_COMPANY_NAME_ERROR_MESSAGE_CORP)]
		private IWebElement EnterCompanyNameErrorMessageCorp { get; set; }

		[FindsBy(How = How.XPath, Using = SHORT_PASSWORD_ERROR_MESSAGE_FREELANCER)]
		private IWebElement ShortPasswordErrorMessageFreelancer { get; set; }

		[FindsBy(How = How.XPath, Using = SHORT_PASSWORD_ERROR_MESSAGE_CORP)]
		private IWebElement ShortPasswordErrorMessageCorp { get; set; }

		[FindsBy(How = How.XPath, Using = SHORT_COMPANY_NAME_ERROR_MESSAGE_CORP)]
		private IWebElement ShortCompanyNameErrorMessageCorp { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string FORM_HEADER = "//h1[text()=\"What's your role\"]";

		protected const string CORP_FORM = "//div[@id='js-registration-corp']";
		protected const string CORP_PASSWORD_FIELD = "//div[@id='js-registration-corp']//input[@data-bind='value: password']";
		protected const string CORP_FIRST_AND_LAST_NAME_FIELD = "//div[@id='js-registration-corp']//input[@data-bind='value: userName']";
		protected const string CORP_PHONE_FIELD = "//div[@id='js-registration-corp']//input[@data-bind='value: phone']";
		protected const string CORP_NAME_FIELD = "//div[@id='js-registration-corp']//input[@data-bind='value: companyName']";

		protected const string FREELANCER_FORM = "//div[@id='js-registration-freel']";
		protected const string FREELANCER_PASSWORD_FIELD = "//div[@id='js-registration-freel']//input[@data-bind='value: password']";
		protected const string FREELANCER_FIRST_AND_LAST_NAME_FIELD = "//div[@id='js-registration-freel']//input[@data-bind='value: userName']";

		protected const string CONFIRM_BUTTON = "//input[@value='Confirm']";

		protected const string ENTER_PASSWORD_ERROR_MESSAGE_CORP = "//div[@id='js-registration-corp']//div[@class='error' and text()='Enter a password']";
		protected const string ENTER_FIRST_AND_LAST_NAME_ERROR_MESSAGE_CORP = "//div[@id='js-registration-corp']//div[@class='error' and text()='Please specify your first and last name']";
		protected const string ENTER_PHONE_ERROR_MESSAGE_CORP = "//div[@id='js-registration-corp']//div[@class='error' and text()='Enter a phone number in the international format']";
		protected const string ENTER_COMPANY_NAME_ERROR_MESSAGE_CORP = "//div[@id='js-registration-corp']//div[@class='error' and text()='Specify the company name']";
		protected const string SHORT_PASSWORD_ERROR_MESSAGE_CORP = "//div[@id='js-registration-corp']//div[@class='error' and text()='A password cannot be shorter than 6 symbols']";
		protected const string SHORT_COMPANY_NAME_ERROR_MESSAGE_CORP = "//div[@id='js-registration-corp']//div[@class='error' and text()='The company name cannot be shorter than 2 symbols']";

		protected const string ENTER_PASSWORD_ERROR_MESSAGE_FREELANCER = "//div[@id='js-registration-freel']//div[@class='error' and text()='Enter a password']";
		protected const string ENTER_FIRST_AND_LAST_NAME_ERROR_MESSAGE_FREELANCER = "//div[@id='js-registration-freel']//div[@class='error' and text()='Please specify your first and last name']";
		protected const string SHORT_PASSWORD_ERROR_MESSAGE_FREELANCER = "//div[@id='js-registration-freel']//div[@class='error' and text()='A password cannot be shorter than 6 symbols']";

		#endregion
	}
}
