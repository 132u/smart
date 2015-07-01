using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration
{
	public class CompanyRegistrationSecondPage : WorkspacePage, IAbstractPage<CompanyRegistrationSecondPage>
	{
		public new CompanyRegistrationSecondPage GetPage()
		{
			var companyRegistrationSecondPage = new CompanyRegistrationSecondPage();
			InitPage(companyRegistrationSecondPage);

			return companyRegistrationSecondPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(COMPANY_TYPE_COMBOBOX)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась вторая страница регистрации компаний.");
			}
		}

		/// <summary>
		/// Ввести 'First name' на втором шаге регистрации компании 
		/// </summary>
		public CompanyRegistrationSecondPage FillFirstName(string firstNameCompany)
		{
			Logger.Debug("Ввести {0} в поле 'First name' на втором шаге регистрации компании.", firstNameCompany);
			FirstName.SetText(firstNameCompany);

			return GetPage();
		}

		/// <summary>
		/// Ввести 'Last name' на втором шаге регистрации компании 
		/// </summary>
		public CompanyRegistrationSecondPage FillLastName(string lastNameCompany)
		{
			Logger.Debug("Ввести {0} в поле 'Last name' на втором шаге регистрации компании.", lastNameCompany);
			LastName.SetText(lastNameCompany);

			return GetPage();
		}

		/// <summary>
		/// Ввести 'Company name' на втором шаге регистрации компании 
		/// </summary>
		public CompanyRegistrationSecondPage FillCompanyName(string companyName)
		{
			Logger.Debug("Ввести {0} в поле 'Company name' на втором шаге регистрации компании.", companyName);
			CompanyName.SendKeys(companyName);

			return GetPage();
		}

		/// <summary>
		/// Ввести Subdomain на втором шаге регистрации компании 
		/// </summary>
		public CompanyRegistrationSecondPage FillSubdomain(string subdomain)
		{
			Logger.Debug("Ввести {0} в поле Subdomain на втором шаге регистрации компании.", subdomain);
			Subdomain.SetText(subdomain);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка 'Create Corporate Account' активна
		/// </summary>
		/// <returns></returns>
		public CompanyRegistrationSecondPage AssertCreateCorporateAccountButtonActive()
		{
			Logger.Trace("Проверить, что кнопка 'Create Corporate Account' активна.");

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
			Logger.Trace("Проверить, что кнопка 'Create Corporate Account' неактивна.");

			Assert.IsTrue(CreateAccountCompanyButton.GetAttribute("disabled") == "true",
				"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

			return GetPage();
		}

		/// <summary>
		/// Ввести телефон на втором шаге регистрации компании 
		/// </summary>
		public CompanyRegistrationSecondPage FillPhoneNumber(string phoneNumber)
		{
			Logger.Debug("Ввести телефон {0} на втором шаге регистрации компании.", phoneNumber);
			PhoneNumber.SetText(phoneNumber);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Create Corporate Account' на втором шаге регистрации компании 
		/// </summary>
		public WorkspacePage ClickCreateCorporateAccountButton()
		{
			Logger.Debug("Нажать кнопку 'Create Corporate Account' на втором шаге регистрации компании.");
			CreateAccountCompanyButton.Click();

			return new WorkspacePage().GetPage();
		}

		/// <summary>
		/// Дождаться, когда кнопка 'Create Corporate Account' станет активной
		/// </summary>
		public CompanyRegistrationSecondPage WaitCreateCorporateAccountButtonBecomeActive()
		{
			Logger.Trace("Дождаться, когда кнопка 'Create Corporate Account' станет активной.");
			Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_ACCOUNT_COMPANY_ACTIVE_BUTTON));

			return GetPage();
		}

		/// <summary>
		/// Выбрать тип компании на втором шаге регистрации компании 
		/// </summary>
		/// <param name="companyType">тип компании</param>
		public CompanyRegistrationSecondPage SelectCompanyType(string companyType)
		{
			Logger.Trace("Выбрать {0} тип компании на втором шаге регистрации компании", companyType);
			CompanyTypeCombobox.SelectOptionByText(companyType);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что название компании обрезается до максимально допустимой длины 
		/// </summary>
		public CompanyRegistrationSecondPage AssertCompanyNameLenght(int maximumCompanyNameLenght)
		{
			Logger.Trace("Проверить, что название компании обрезается до максимально допустимой длины в {0} символов.", maximumCompanyNameLenght);

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
			Logger.Trace("Получить длину названия компании.");

			return CompanyName.GetElementAttribute("value").Length;
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
	}
}
