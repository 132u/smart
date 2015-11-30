using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration
{
	class CompanyRegistrationSecondPage : IAbstractPage<CompanyRegistrationSecondPage>
	{
		public WebDriver Driver { get; protected set; }

		public CompanyRegistrationSecondPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public CompanyRegistrationSecondPage GetPage()
		{
			var companyRegistrationSecondPage = new CompanyRegistrationSecondPage(Driver);
			LoadPage();

			return companyRegistrationSecondPage;
		}

		public void LoadPage()
		{
			if (!IsCompanyRegistrationSecondPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась вторая страница регистрации компаний.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести 'First name' на втором шаге регистрации компании 
		/// </summary>
		/// <param name="firstName">имя</param>
		public CompanyRegistrationSecondPage FillFirstName(string firstName)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле 'First name' на втором шаге регистрации компании.", firstName);
			FirstName.SetText(firstName);

			return GetPage();
		}

		/// <summary>
		/// Ввести 'Last name' на втором шаге регистрации компании 
		/// </summary>
		/// <param name="lastName">фамилия</param>
		public CompanyRegistrationSecondPage FillLastName(string lastName)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле 'Last name' на втором шаге регистрации компании.", lastName);
			LastName.SetText(lastName);

			return GetPage();
		}

		/// <summary>
		/// Ввести 'Company name' на втором шаге регистрации компании 
		/// </summary>
		/// <param name="companyName">название компании</param>
		public CompanyRegistrationSecondPage FillCompanyName(string companyName)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле 'Company name' на втором шаге регистрации компании.", companyName);
			CompanyName.SendKeys(companyName);

			return GetPage();
		}

		/// <summary>
		/// Ввести Subdomain на втором шаге регистрации компании 
		/// </summary>
		/// <param name="subdomain">поддомен</param>
		public CompanyRegistrationSecondPage FillSubdomain(string subdomain)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле Subdomain на втором шаге регистрации компании.", subdomain);
			Subdomain.SetText(subdomain);

			return GetPage();
		}

		/// <summary>
		/// Ввести телефон на втором шаге регистрации компании 
		/// </summary>
		/// <param name="phoneNumber">телефонный номер</param>
		public CompanyRegistrationSecondPage FillPhoneNumber(string phoneNumber)
		{
			CustomTestContext.WriteLine("Ввести телефон {0} на втором шаге регистрации компании.", phoneNumber);
			PhoneNumber.SetText(phoneNumber);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Create Corporate Account' на втором шаге регистрации компании 
		/// </summary>
		public WorkspacePage ClickCreateCorporateAccountButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Create Corporate Account' на втором шаге регистрации компании.");
            Driver.WaitUntilElementIsEnabled(By.XPath(CREATE_ACCOUNT_COMPANY_BUTTON));
			CreateAccountCompanyButton.Click();

			return new WorkspacePage(Driver);
		}

		/// <summary>
		/// Выбрать тип компании на втором шаге регистрации компании 
		/// </summary>
		/// <param name="companyType">тип компании</param>
		public CompanyRegistrationSecondPage SelectCompanyType(string companyType)
		{
			CustomTestContext.WriteLine("Выбрать {0} тип компании на втором шаге регистрации компании", companyType);
			CompanyTypeCombobox.SelectOptionByText(companyType);

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по Company Type комбобоксу
		/// </summary>
		public CompanyRegistrationSecondPage ClickCompanyTypeDropdown()
		{
			CustomTestContext.WriteLine("Кликнуть по Company Type комбобоксу");
			CompanyTypeCombobox.Click();

			return GetPage();
		}

		/// <summary>
		/// Двойной клик по Company Type комбобоксу
		/// </summary>
		public CompanyRegistrationSecondPage DoubleClickCompanyTypeDropdown()
		{
			CompanyTypeCombobox.DoubleClick();

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Заполнить форму регистрации на второй странице
		/// </summary>
		/// <param name="firstName">имя</param>
		/// <param name="lastName">фамилия</param>
		/// <param name="companyName">название компании</param>
		/// <param name="subDomain">поддомен</param>
		/// <param name="companyType">тип компании</param>
		/// <param name="phoneNumber">номер телефона</param>
		public CompanyRegistrationSecondPage FillCompanyDataSecondStep(
			string firstName,
			string lastName,
			string companyName,
			string subDomain,
			CompanyType companyType,
			string phoneNumber = "12312312312312")
		{
			FillFirstName(firstName);
			FillLastName(lastName);
			FillCompanyName(companyName);
			FillSubdomain(subDomain);
			FillPhoneNumber(phoneNumber);
			SelectCompanyType(companyType.Description());

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что кнопка 'Create Corporate Account' неактивна
		/// </summary>
		public bool IsCreateCorporateAccountButtonInactive()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Create Corporate Account' неактивна.");

			return CreateAccountCompanyButton.GetAttribute("disabled") == "true";
		}

		/// <summary>
		/// Проверить, что название компании обрезается до максимально допустимой длины 
		/// </summary>
		/// <param name="maximumCompanyNameLenght">максимальное кол-во символов</param>
		public bool IsCompanyNameCutting(int maximumCompanyNameLenght)
		{
			CustomTestContext.WriteLine("Проверить, что название компании обрезается до максимально допустимой длины в {0} символов.", maximumCompanyNameLenght);

			var companyNameLenght = CompanyName.GetElementAttribute("value").Length;

			return companyNameLenght == maximumCompanyNameLenght;
		}

		/// <summary>
		/// Проверить, что появилось сообщение 
		/// 'Use only latin letters, digits, hyphen and underscope. The domain name cannot start with "www", digit, hyphen or underscope.'
		/// </summary>
		public bool IsSubdomainPatternMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Use only latin letters, digits, hyphen and underscope...'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(SUBDOMAIN_PATTERN_MESSAGE));
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'The domain name must contain at least 3 characters'
		/// </summary>
		public bool IsSubdomainLenghtMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'The domain name must contain at least 3 characters'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(SUBDOMAIN_LENGHT_MESSAGE));
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'A company with this name already exists'
		/// </summary>
		public bool IsCompanyNameAlredyInUseMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'A company with this name already exists'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(COMPANY_NAME_MESSAGE));
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Enter your domain name'
		/// </summary>
		public bool IsEnterDomainMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Enter your domain name'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ENTER_DOMAIN_MESSAGE));
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Enter your name'
		/// </summary>
		public bool IsEnterNameMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Enter your name'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ENTER_NAME_MESSAGE));
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Enter your last name'
		/// </summary>
		public bool IsEnterLastNameMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Enter your last name'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ENTER_LASTNAME_MESSAGE));
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Enter your company name'
		/// </summary>
		public bool IsEnterCompanyNameMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Enter your company name'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ENTER_COMPANY_NAME_MESSAGE));
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Enter your international phone number'
		/// </summary>
		public bool IsEnterPhoneMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Enter your international phone number'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ENTER_PHONE_MESSAGE));
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Enter your company type'
		/// </summary>
		public bool IsSelectCompanyTypeMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Enter your company type'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(SELECT_COMPANY_TYPE_MESSAGE));
		}
		
		/// <summary>
		/// Проверить, что сообщение 'The company name must contain at least 2 characters' появилось
		/// </summary>
		public bool IsCompanyNameLengthInvalidMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'The company name must contain at least 2 characters' появилось.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(COMPANY_NAME_LENGTH_INVALID_MESSAGE));
		}

		/// <summary>
		/// Проверить, что сообщение 'The company name must begin with a letter or a quotation mark' появилось
		/// </summary>
		public bool IsCompanyNameInvalidPatternMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'The company name must begin with a letter or a quotation mark' появилось.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(INVALID_PATTERN_COMPANY_NAME_MESSAGE));
		}

		/// <summary>
		/// Проверить, открыта ли страница
		/// </summary>
		/// <returns></returns>
		public bool IsCompanyRegistrationSecondPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(COMPANY_TYPE_COMBOBOX));
		}

		#endregion

		#region Вспомогательные методы

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.Id, Using = FIRST_NAME)]
		protected IWebElement FirstName { get; set; }

		[FindsBy(How = How.Id, Using = LAST_NAME)]
		protected IWebElement LastName { get; set; }

		[FindsBy(How = How.Id, Using = COMPANY_NAME)]
		protected IWebElement CompanyName { get; set; }

		[FindsBy(How = How.Id, Using = SUBDOMAIN)]
		protected IWebElement Subdomain { get; set; }

		[FindsBy(How = How.Id, Using = PHONE_NUMBER)]
		protected IWebElement PhoneNumber { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_ACCOUNT_COMPANY_BUTTON)]
		protected IWebElement CreateAccountCompanyButton { get; set; }

		[FindsBy(How = How.XPath, Using = COMPANY_TYPE_COMBOBOX)]
		protected IWebElement CompanyTypeCombobox { get; set; }

		[FindsBy(How = How.XPath, Using = LOGIN_LINK)]
		protected IWebElement LoginLink { get; set; }

		[FindsBy(How = How.XPath, Using = WRONG_PASSWORD_MESSAGE)]
		protected IWebElement WrongPasswordMessage { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD_MATCH_MESSAGE)]
		protected IWebElement PasswordMatchMessage { get; set; }

		[FindsBy(How = How.XPath, Using = EMAIL_INVALID_MESSAGE)]
		protected IWebElement EmailInvalidMessage { get; set; }

		[FindsBy(How = How.XPath, Using = REQUIRED_PASSWORD_MESSAGE)]
		protected IWebElement RequiredPasswordMessage { get; set; }

		[FindsBy(How = How.XPath, Using = MINIMUM_LENGHT_PASSWORD_MESSAGE)]
		protected IWebElement MinimumLenghtPasswordMessage { get; set; }

		[FindsBy(How = How.XPath, Using = EMPTY_PASSWORD_MESSAGE)]
		protected IWebElement EmptyPasswordMessage { get; set; }

		[FindsBy(How = How.XPath, Using = SUBDOMAIN_PATTERN_MESSAGE)]
		protected IWebElement SubdomainPatternMessage { get; set; }

		[FindsBy(How = How.XPath, Using = SUBDOMAIN_LENGHT_MESSAGE)]
		protected IWebElement SubdomainLenghtMessage { get; set; }

		[FindsBy(How = How.XPath, Using = COMPANY_NAME_MESSAGE)]
		protected IWebElement CompanyNameMessage { get; set; }

		[FindsBy(How = How.XPath, Using = ENTER_DOMAIN_MESSAGE)]
		protected IWebElement EnterDomainMessage { get; set; }

		[FindsBy(How = How.XPath, Using = ENTER_NAME_MESSAGE)]
		protected IWebElement EnterNameMessage { get; set; }

		[FindsBy(How = How.XPath, Using = ENTER_LASTNAME_MESSAGE)]
		protected IWebElement EnterLastNameMessage { get; set; }

		[FindsBy(How = How.XPath, Using = ENTER_COMPANY_NAME_MESSAGE)]
		protected IWebElement EnterCompanyNameMessage { get; set; }

		[FindsBy(How = How.XPath, Using = ENTER_PHONE_MESSAGE)]
		protected IWebElement EnterPhoneMessage { get; set; }

		[FindsBy(How = How.XPath, Using = SELECT_COMPANY_TYPE_MESSAGE)]
		protected IWebElement SelectCompanyTypeMessage { get; set; }

		[FindsBy(How = How.XPath, Using = INVALID_PATTERN_COMPANY_NAME_MESSAGE)]
		protected IWebElement InvalidPatternCompanyNameMessage { get; set; }

		[FindsBy(How = How.XPath, Using = COMPANY_NAME_LENGTH_INVALID_MESSAGE)]
		protected IWebElement CompanyNameLengthInvalidMessage { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string FIRST_NAME = "firstname";
		protected const string LAST_NAME = "lastname";
		protected const string COMPANY_NAME = "company";
		protected const string SUBDOMAIN = "subdomain";
		protected const string PHONE_NUMBER = "phone-number";
		protected const string CREATE_ACCOUNT_COMPANY_BUTTON = "//button[@id='btn-create-account']";
		protected const string COMPANY_TYPE_COMBOBOX = "//select[@id='company-type']";
		protected const string CREATE_ACCOUNT_COMPANY_ACTIVE_BUTTON = "//button[@id='btn-create-account'  and not(@disabled='disabled')]";

		protected const string LOGIN_LINK = "//a[@ng-click='showSignIn()' and text()='log in']";
		protected const string WRONG_PASSWORD_MESSAGE = "//div[@ng-message='wrong-password']";
		protected const string PASSWORD_MATCH_MESSAGE = "//span[contains(@ng-show, 'error.match ')]";
		protected const string EMAIL_INVALID_MESSAGE = "//span[contains(@ng-show,'signupForm.email')]";
		protected const string REQUIRED_PASSWORD_MESSAGE = "//div[contains(@ng-message,'required')]//span[not(contains(@class,'ng-hide'))]";
		protected const string MINIMUM_LENGHT_PASSWORD_MESSAGE = "//fieldset[contains(@valid,'password')]//div[contains(@ng-message,'minlength')]//span[not(contains(@class,'ng-hide'))]";
		protected const string EMPTY_PASSWORD_MESSAGE = "//fieldset[contains(@valid,'password')]//div[contains(@ng-message,'required')]//span[not(contains(@class,'ng-hide'))]";

		protected const string SUBDOMAIN_LENGHT_MESSAGE = "//span[@translate='DOMAIN-LENGTH-INVALID']";
		protected const string SUBDOMAIN_PATTERN_MESSAGE = "//span[@translate='DOMAIN-PATTERN-INVALID']";
		protected const string COMPANY_NAME_MESSAGE = "//span[@translate='COMPANY-NAME-ALREADY-IN-USE']";
		protected const string ENTER_DOMAIN_MESSAGE = "//span[@translate='ENTER-DOMAIN']";
		protected const string ENTER_NAME_MESSAGE = "//span[@translate='ENTER-NAME']";
		protected const string ENTER_LASTNAME_MESSAGE = "//span[@translate='ENTER-LASTNAME']";
		protected const string ENTER_COMPANY_NAME_MESSAGE = "//span[@translate='ENTER-COMPANY-NAME']";
		protected const string ENTER_PHONE_MESSAGE = "//span[@translate='ENTER-TELEPHONE']";
		protected const string SELECT_COMPANY_TYPE_MESSAGE = "//span[@translate='SELECT-COMPANY-TYPE']";
		protected const string INVALID_PATTERN_COMPANY_NAME_MESSAGE = "//span[@translate='COMPANY-NAME-PATTERN-INVALID']";
		protected const string COMPANY_NAME_LENGTH_INVALID_MESSAGE = "//span[@translate='COMPANY-NAME-LENGTH-INVALID']";

		#endregion
	}
}
