using System;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration
{
	public class CompanyRegistrationSecondPage : WorkspacePage, IAbstractPage<CompanyRegistrationSecondPage>
	{
		public CompanyRegistrationSecondPage(WebDriver driver) : base(driver)
		{
		}

		public new CompanyRegistrationSecondPage GetPage()
		{
			var companyRegistrationSecondPage = new CompanyRegistrationSecondPage(Driver);
			InitPage(companyRegistrationSecondPage, Driver);

			return companyRegistrationSecondPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(COMPANY_TYPE_COMBOBOX), timeout: 45))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась вторая страница регистрации компаний.");
			}
		}

		/// <summary>
		/// Ввести 'First name' на втором шаге регистрации компании 
		/// </summary>
		public CompanyRegistrationSecondPage FillFirstName(string firstNameCompany)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле 'First name' на втором шаге регистрации компании.", firstNameCompany);
			FirstName.SetText(firstNameCompany);

			return GetPage();
		}

		/// <summary>
		/// Ввести 'Last name' на втором шаге регистрации компании 
		/// </summary>
		public CompanyRegistrationSecondPage FillLastName(string lastNameCompany)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле 'Last name' на втором шаге регистрации компании.", lastNameCompany);
			LastName.SetText(lastNameCompany);

			return GetPage();
		}

		/// <summary>
		/// Ввести 'Company name' на втором шаге регистрации компании 
		/// </summary>
		public CompanyRegistrationSecondPage FillCompanyName(string companyName)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле 'Company name' на втором шаге регистрации компании.", companyName);
			CompanyName.SendKeys(companyName);

			return GetPage();
		}

		/// <summary>
		/// Ввести Subdomain на втором шаге регистрации компании 
		/// </summary>
		public CompanyRegistrationSecondPage FillSubdomain(string subdomain)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле Subdomain на втором шаге регистрации компании.", subdomain);
			Subdomain.SetText(subdomain);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка 'Create Corporate Account' активна
		/// </summary>
		/// <returns></returns>
		public CompanyRegistrationSecondPage AssertCreateCorporateAccountButtonActive()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Create Corporate Account' активна.");

			Assert.IsTrue(CreateAccountCompanyButton.GetAttribute("disabled") == null,
				"Произошла ошибка:\n кнопка 'Create Corporate Account' неактивна.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка 'Create Corporate Account' неактивна
		/// </summary>
		/// <returns></returns>
		public CompanyRegistrationSecondPage AssertCreateCorporateAccountButtonInactive()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Create Corporate Account' неактивна.");

			Assert.IsTrue(CreateAccountCompanyButton.GetAttribute("disabled") == "true",
				"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

			return GetPage();
		}

		/// <summary>
		/// Ввести телефон на втором шаге регистрации компании 
		/// </summary>
		public CompanyRegistrationSecondPage FillPhoneNumber(string phoneNumber)
		{
			CustomTestContext.WriteLine("Ввести телефон {0} на втором шаге регистрации компании.", phoneNumber);
			PhoneNumber.SetText(phoneNumber);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Create Corporate Account' на втором шаге регистрации компании 
		/// </summary>
		public T ClickCreateCorporateAccountButton<T>(WebDriver driver) where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Create Corporate Account' на втором шаге регистрации компании.");
			CreateAccountCompanyButton.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { driver }) as T;
			return instance.GetPage();
		}

		/// <summary>
		/// Дождаться, когда кнопка 'Create Corporate Account' станет активной
		/// </summary>
		public CompanyRegistrationSecondPage WaitCreateCorporateAccountButtonBecomeActive()
		{
			CustomTestContext.WriteLine("Дождаться, когда кнопка 'Create Corporate Account' станет активной.");
			Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_ACCOUNT_COMPANY_ACTIVE_BUTTON));

			return GetPage();
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
		/// Проверить, что название компании обрезается до максимально допустимой длины 
		/// </summary>
		public CompanyRegistrationSecondPage AssertCompanyNameLenght(int maximumCompanyNameLenght)
		{
			CustomTestContext.WriteLine("Проверить, что название компании обрезается до максимально допустимой длины в {0} символов.", maximumCompanyNameLenght);

			Assert.AreEqual(companyNameLenght(), maximumCompanyNameLenght,
				"Произошла ошибка:\n название компании не обрезается до максимально допустимой длины в {0} символов.", maximumCompanyNameLenght);

			return GetPage();
		}

		/// <summary>
		/// Получить длину названия компании
		/// </summary>
		/// <returns>длина названия компании</returns>
		private int companyNameLenght()
		{
			CustomTestContext.WriteLine("Получить длину названия компании.");

			return CompanyName.GetElementAttribute("value").Length;
		}

		/// <summary>
		/// Проверить, что появилось сообщение 
		/// 'Use only latin letters, digits, hyphen and underscope. The domain name cannot start with "www", digit, hyphen or underscope.'
		/// </summary>
		public CompanyRegistrationSecondPage AssertSubdomainPatternMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Use only latin letters, digits, hyphen and underscope...'.");

			Assert.IsTrue(SubdomainPatternMessage.Displayed,
				"Произошла ошибка:\n сообщение 'Use only latin letters, digits, hyphen and underscope...' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'The domain name must contain at least 3 characters'
		/// </summary>
		public CompanyRegistrationSecondPage AssertSubdomainLenghtMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'The domain name must contain at least 3 characters'.");

			Assert.IsTrue(SubdomainLenghtMessage.Displayed,
				"Произошла ошибка:\n сообщение 'The domain name must contain at least 3 characters' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'A company with this name already exists'
		/// </summary>
		public CompanyRegistrationSecondPage AssertCompanyNameMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'A company with this name already exists'.");

			Assert.IsTrue(CompanyNameMessage.Displayed,
				"Произошла ошибка:\n сообщение 'A company with this name already exists' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Enter your domain name'
		/// </summary>
		public CompanyRegistrationSecondPage AssertEnterDomainMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Enter your domain name'.");

			Assert.IsTrue(EnterDomaineMessage.Displayed,
				"Произошла ошибка:\n сообщение 'Enter your domain name' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Enter your name'
		/// </summary>
		public CompanyRegistrationSecondPage AssertEnterNameMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Enter your name'.");

			Assert.IsTrue(EnterNameMessage.Displayed,
				"Произошла ошибка:\n сообщение 'Enter your name' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Enter your last name'
		/// </summary>
		public CompanyRegistrationSecondPage AssertEnterLastNameMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Enter your last name'.");

			Assert.IsTrue(EnterLastNameMessage.Displayed,
				"Произошла ошибка:\n сообщение 'Enter your last name' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Enter your company name'
		/// </summary>
		public CompanyRegistrationSecondPage AssertEnterCompanyNameMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Enter your company name'.");

			Assert.IsTrue(EnterCompanyNameMessage.Displayed,
				"Произошла ошибка:\n сообщение 'Enter your company name' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Enter your international phone number'
		/// </summary>
		public CompanyRegistrationSecondPage AssertEnterPhoneMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Enter your international phone number'.");

			Assert.IsTrue(EnterPhoneMessage.Displayed,
				"Произошла ошибка:\n сообщение 'Enter your international phone number' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Enter your company type'
		/// </summary>
		public CompanyRegistrationSecondPage AssertSelectCompanyTypeMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Enter your company type'.");

			Assert.IsTrue(SelectCompanyTypeMessage.Displayed,
				"Произошла ошибка:\n сообщение 'Enter your company type' не появилось.");

			return GetPage();
		}
		
		/// <summary>
		/// Проверить, что сообщение 'The company name must contain at least 2 characters' появилось
		/// </summary>
		public CompanyRegistrationSecondPage AssertCompanyNameLengthInvalidMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'The company name must contain at least 2 characters' появилось.");

			Assert.IsTrue(CompanyNameLengthInvalidMessage.Displayed,
				"Произошла ошибка:\n сообщение 'The company name must contain at least 2 characters.' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сообщение 'The company name must begin with a letter or a quotation mark' появилось
		/// </summary>
		public CompanyRegistrationSecondPage AssertCompanyNamePatternMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'The company name must begin with a letter or a quotation mark' появилось.");

			Assert.IsTrue(InvalidPatternCompanyNameMessage.Displayed,
				"Произошла ошибка:\n сообщение 'The company name must begin with a letter or a quotation mark' не появилось.");

			return GetPage();
		}

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
		protected IWebElement EnterDomaineMessage { get; set; }

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

	}
}
