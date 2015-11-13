using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration.FreelanceRegistration
{
	class FreelanceRegistrationSecondPage : IAbstractPage<FreelanceRegistrationSecondPage>
	{
		public WebDriver Driver { get; protected set; }

		public FreelanceRegistrationSecondPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public FreelanceRegistrationSecondPage GetPage()
		{
			var freelanceRegistrationSecondPage = new FreelanceRegistrationSecondPage(Driver);
			LoadPage();

			return freelanceRegistrationSecondPage;
		}

		public void LoadPage()
		{
			if (!IsFreelanceRegistrationSecondPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась вторая страница регистрации фрилансеров.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести имя фрилансера
		/// </summary>
		/// <param name="firstName">имя фрилансера</param>
		public FreelanceRegistrationSecondPage FillFirstName(string firstName)
		{
			CustomTestContext.WriteLine("Ввести имя фрилансера {0}.", firstName);
			FirstName.SetText(firstName);

			return GetPage();
		}

		/// <summary>
		/// Ввести фамилию фрилансера
		/// </summary>
		/// <param name="lastName">фамилия фрилансера</param>
		public FreelanceRegistrationSecondPage FillLastName(string lastName)
		{
			CustomTestContext.WriteLine("Ввести фамилию фрилансера {0}.", lastName);
			LastName.SetText(lastName);

			return GetPage();
		}

		/// <summary>
		/// Выбрать страну
		/// </summary>
		/// <param name="country">страна</param>
		public FreelanceRegistrationSecondPage SelectCountry(string country)
		{
			CustomTestContext.WriteLine("Выбрать страну {0}.", country);
			Driver.SetDynamicValue(How.XPath, COUNTRY, country).Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать родной язык
		/// </summary>
		/// <param name="nativeLanguage">родной язык</param>
		public FreelanceRegistrationSecondPage SelectNativeLanguage(Language nativeLanguage)
		{
			CustomTestContext.WriteLine("Выбрать родной язык {0}.", nativeLanguage);
			Driver.SetDynamicValue(How.XPath, NATIVE_LANGUAGE, nativeLanguage.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать первую предоставляемую услугу 
		/// </summary>
		/// <param name="service">услуга</param>
		public FreelanceRegistrationSecondPage SelectFirstServiceProvide(WorkflowTask service)
		{
			CustomTestContext.WriteLine("Выбрать первую предоставляемую услугу {0}.", service);
			Service.SelectOptionByText(service.ToString());

			return GetPage();
		}

		/// <summary>
		/// Выбрать часовой пояс
		/// </summary>
		/// <param name="timezone">часовой пояс</param>
		public FreelanceRegistrationSecondPage SelectTimezone(string timezone)
		{
			CustomTestContext.WriteLine("Выбрать часовой пояс {0}.", timezone);
			Driver.SetDynamicValue(How.XPath, TIMEZONE, timezone).Click();
		
			return GetPage();
		}

		/// <summary>
		/// Выбрать исходный язык
		/// </summary>
		/// <param name="language">исходный язык</param>
		public FreelanceRegistrationSecondPage SelectSourceLanguage(Language language)
		{
			CustomTestContext.WriteLine("Выбрать исходный язык {0}.", language);
			SourceLanguage.SelectOptionByText(language.ToString());

			return GetPage();
		}

		/// <summary>
		/// Выбрать язык перевода
		/// </summary>
		/// <param name="language">язык перевода</param>
		public FreelanceRegistrationSecondPage SelectTargetLanguage(Language language)
		{
			CustomTestContext.WriteLine("Выбрать язык перевода {0}.", language);
			TargetLanguage.SelectOptionByText(language.ToString());

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Create Account', ожидая ошибку.
		/// </summary>
		public FreelanceRegistrationSecondPage ClickCreateAccountButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Create Account', ожидая ошибку.");
			CreateFreelancerAccountButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Create Account'.
		/// </summary>
		public WorkspacePage ClickCreateAccountButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Create Account'.");
			CreateFreelancerAccountButton.Click();

			return new WorkspacePage(Driver).GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Заполнить данные на втором шаге регистрации фрилансера
		/// </summary>
		/// <param name="firstName">имя</param>
		/// <param name="lastName">фамилия</param>
		/// <param name="sourceLanguage">исходный язык</param>
		/// <param name="targetLanguage">язык перевода</param>
		/// <param name="service">услуга</param>
		/// <param name="nativeLanguage">родной язык</param>
		/// <param name="country">страна</param>
		/// <param name="timezone">часовой пояс</param>
		public FreelanceRegistrationSecondPage FillFreelanceRegistrationSecondStep(
			string firstName,
			string lastName,
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian,
			WorkflowTask service = WorkflowTask.Translation,
			Language nativeLanguage = Language.German,
			string country = "Belarus",
			string timezone = "(UTC-03:00) Greenland")
		{
			CustomTestContext.WriteLine("Заполнить данные на втором шаге регистрации фрилансера.");
			FillFirstName(firstName);
			FillLastName(lastName);
			SelectCountry(country);
			SelectTimezone(timezone);
			SelectNativeLanguage(nativeLanguage);
			SelectSourceLanguage(sourceLanguage);
			SelectTargetLanguage(targetLanguage);

			if (service != WorkflowTask.Empty)
			{
				SelectFirstServiceProvide(service);
			}

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась вторая страница регистрации фрилансеров
		/// </summary>
		private bool IsFreelanceRegistrationSecondPageOpened()
		{
			CustomTestContext.WriteLine("Проверить, что открылась вторая страница регистрации фрилансеров.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(LAST_NAME));
		}

		/// <summary>
		/// Проверить, что кнопка 'Create Freelancer Account' неактивна.
		/// </summary>
		public bool IsCreateFreelancerAccountButtonDisabled()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Create Freelancer Account' неактивна.");

			return Driver.FindElement(By.XPath(CREATE_ACCOUNT_BUTTON)).GetElementAttribute("disabled") == "true";
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CREATE_ACCOUNT_BUTTON)]
		protected IWebElement CreateFreelancerAccountButton { get; set; }

		[FindsBy(How = How.XPath, Using = FIRST_NAME)]
		protected IWebElement FirstName { get; set; }

		[FindsBy(How = How.XPath, Using = LAST_NAME)]
		protected IWebElement LastName { get; set; }

		[FindsBy(How = How.XPath, Using = TIMEZONE)]
		protected IWebElement Timezone { get; set; }

		[FindsBy(How = How.XPath, Using = SOURCE_LANGUAGE)]
		protected IWebElement SourceLanguage { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_LANGUAGE)]
		protected IWebElement TargetLanguage { get; set; }

		[FindsBy(How = How.XPath, Using = SERVICE)]
		protected IWebElement Service { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string FIRST_NAME = "//input[@id='firstname']";
		protected const string LAST_NAME = "//input[@id='lastname']";
		protected const string COUNTRY = "//select[@id='country']//option[@label='*#*']";
		protected const string TIMEZONE = "//select[@id='timezone']//option[@label='*#*']";
		protected const string NATIVE_LANGUAGE = "//select[contains(@class, 'multi-select-item ')]//option[@label='*#*']";
		protected const string SERVICE = "//span[@id='service']//select";
		protected const string SERVICE_PROVIDE2 = "//table[@class='t-servSelect']//td[3]/select//option[@label='*#*']";
		protected const string SERVICE_PROVID3 = "//table[@class='t-servSelect']//tbody[3]//select//option[@label='*#*']";
		protected const string SERVICE_PROVIDE4 = "//table[@class='t-servSelect']//tbody[3]//td[3]/select//option[@label='*#*']";
		protected const string CREATE_ACCOUNT_BUTTON = "//button[@id='btn-create-account']";
		protected const string SOURCE_LANGUAGE = "//select[@id='source-service-language']";
		protected const string TARGET_LANGUAGE = "//select[@id='target-service-language']";

		#endregion
	}
}
