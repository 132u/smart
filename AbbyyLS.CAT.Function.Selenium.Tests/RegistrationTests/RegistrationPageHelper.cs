using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы регистрации 
	/// </summary>
	public class RegistrationPageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера страницы регистрации 
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public RegistrationPageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
			FirstName = GetUniqueString();
			LastName = GetUniqueString();
			Email = GetUniqueEmail();
			Password = GetUniquePassword();
			NickName = GetUniqueString();
			NameCompany = "CN" + RandomString.Generate(8);
			DomainName = "D" + RandomString.GenerateRandomString();
		}

		public  string FirstName;
		public string LastName;
		public string Email;
		public string Password;
		public string NickName;
		public string NameCompany;
		public string DomainName;

		/// <summary>
		/// Заполнить все поля на певом шаге регистрации фрилансера
		/// </summary>
		/// <param name="email">email фрилансера</param>
		/// <param name="password">пароль</param>
		/// <param name="confirmPassword">подтверждение пароля</param>
		public void FillRegistrationDataInFirstStep(string email, string password, string confirmPassword)
		{
			//заполнить поле email
			TypeTextInEmailField(email);
			//заполнить поле пароля
			TypeTextInPasswordField(password);
			//заполнить поле подтверждения пароля
			TypeTextInConfirmPasswordField(confirmPassword);
		}

		/// <summary>
		/// Заполнить все поля на втором шаге регистрации фрилансера
		/// </summary>
		/// <param name="firstName">имя фрилансера</param>
		/// <param name="lastName">фамилия фридансера</param>
		public void FillRegistrationDataInSecondStep(string firstName, string lastName)
		{
			//запомнить имя фрилансера
			this.FirstName = firstName;
			//запомнить фамилию фрилансера
			this.LastName = lastName;
			//заполнить поле имя
			TypeTextInFirstNameField(firstName);
			//заполнить поле фамилии
			TypeTextInLastNameField(lastName);
			// выбрать страну
			ClickElement(By.XPath(COUNTRY2));
			// выбрать язык
			ClickElement(By.XPath(NATIVE_LANGUAGE));
			//this.SelectItemInDropDownByText(By.XPath(NATIVE_LANGUAGE), "Akan");
			// выбрать сервис(язык)
			ClickElement(By.XPath(SERVICE_PROVIDE));
			// выбрать сервис2(язык)
			ClickElement(By.XPath(SERVICE_PROVIDE2));
			// выбрать таймзону
			ClickElement(By.XPath(TIMEZONE));
			// выбрать уровень языка(этап)
			ClickElement(By.XPath(LANG_LEVEL));
		}

		/// <summary>
		/// Выбрать вторую пару языков на втором шаге регистрации
		/// </summary>
		public void SelectSecondService()
		{
			// Кликнуть кнопку Add
			ClickAddServiceBtn();
			ClickElement(By.XPath(SERVICE_PROVID3));
			ClickElement(By.XPath(SERVICE_PROVIDE4));
		}

		/// <summary>
		/// Дождаться загрузки страницы - первый шаг регистрации
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitFirstStepPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(EMAIL_FIELD_IN_SIGN_IN));
		}

		/// <summary>
		/// Дождаться загрузки страницы - втрой шаг регистрации
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitSecondStepPageLoad()
		{
			return WaitUntilDisplayElement(By.Id(SECOND_STEP_LABEL));
		}

		/// <summary>
		/// Кликнуть по кнопке Sign up - на странице регистрации в форме Sign up
		/// </summary>
		public void ClickSignUpButton()
		{
			ClickElement(By.XPath(SIGN_UP_BUTTON));
		}
		/// <summary>
		/// Кликнуть по кнопке Sign In в форме Sign in
		/// </summary>
		public void ClickSignInButton()
		{
			ClickElement(By.XPath(SIGN_IN_BUTTON));
		}
		/// <summary>
		/// Ввод текста в поле email - на странице регистрации в форме Sign up
		/// </summary>
		public void TypeTextInEmailField(string email)
		{
			ClearAndAddText(By.XPath(EMAIL_FIELD_IN_SIGN_UP), email);
		}

		/// <summary>
		/// Ввод текста в поле пароля в форме Sign In
		/// </summary>
		/// <param name="password">пароль</param>
		public void TypeTextInPasswordFieldSignIn(string password)
		{
			ClearAndAddText(By.XPath(PASSWORD_FIELD_IN_SIGN_IN), password);
		}
		/// <summary>
		/// Ввод текста в поле email в форме Sign In
		/// </summary>
		/// <param name="email">email</param>
		public void TypeTextInEmailFieldSignIn(string email)
		{

			ClearAndAddText(By.XPath(EMAIL_FIELD_IN_SIGN_IN), email);
		}

		/// <summary>
		/// Ввод текста в поле пароля в форме Sign Up
		/// </summary>
		///  <param name="password">пароль</param>
		public void TypeTextInPasswordField(string password)
		{
			ClearAndAddText(By.XPath(PASSWORD_FIELD_IN_SIGN_UP), password);
		}
		/// <summary>
		/// Ввод текста в поле подтверждения пароля  в форме Sign Up
		/// </summary>
		///  <param name="password">пароль</param>
		public void TypeTextInConfirmPasswordField(string password)
		{
			ClearAndAddText(By.XPath(CONFIRM_PASSWORD_FIELD), password);
		}

		/// <summary>
		/// Ввод текста в поле имя - второй шаг регистрации
		/// </summary>
		/// <param name="firstName">имя</param>
		public void TypeTextInFirstNameField(string firstName)
		{
			ClearAndAddText(By.XPath(FIRST_NAME_FIELD), firstName);
		}

		/// <summary>
		/// Ввод текста в поле фамилия - второй шаг регистрации
		/// </summary>
		/// <param name="lastName">фамилия</param>
		public void TypeTextInLastNameField(string lastName)
		{
			ClearAndAddText(By.XPath(LAST_NAME_FIELD), lastName);
		}

		/// <summary>
		/// Нажать кнопку создания аккаунта  - второй шаг регистрации
		/// </summary>
		public void ClickCreateAccountButton()
		{
			ClickElement(By.XPath(CREATE_ACCOUNT_BUTTON));
		}

		/// <summary>
		/// Проверить , что в панели WS отображается имя и фамилия юзера
		/// </summary>
		public bool CheckNameSurnameInWSPanel()
		{
			if (GetTextElement(By.XPath(USER_NAME_XPATH)) == this.FirstName + ' ' + this.LastName) return true;
			else return false;
		}

		/// <summary>
		/// Проверить , что в панели WS отображается только имя юзера
		/// </summary>
		public bool CheckNameInWSPanel()
		{
			if (GetTextElement(By.XPath(USER_NAME_XPATH)) == this.FirstName) return true;
			else return false;
		}
		/// <summary>
		/// Создать рандомную строку
		/// </summary>
		public string GetUniqueString()
		{
			return "Test_" + RandomString.Generate(10);
		}

		/// <summary>
		/// Создать рандомную строку email
		/// </summary>
		private string GetUniqueEmail()
		{
			return "TestEmail@" + RandomString.Generate(10) + ".com";
		}

		/// <summary>
		/// Создать рандомную строку password
		/// </summary>
		private string GetUniquePassword()
		{
			return "TestPassword" + RandomString.Generate(10);
		}

		/// <summary>
		/// Проверить активна ли кнопка Sign up
		/// </summary>
		public bool CheckThatSignUpButtonIsDisable()
		{
			if (GetElementAttribute(By.XPath(SIGN_UP_BUTTON), "disabled") == "true") return true;
			else return false;
		}

		/// <summary>
		/// Проверить активна ли кнопка Create new account - второй шаг регистрации
		/// </summary>
		public bool CheckThatCreateAccountBtnIsDisable()
		{
			if (GetElementAttribute(By.XPath(CREATE_ACCOUNT_BUTTON), "disabled") == "true") return true;
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Обновить страницу Login
		/// </summary>
		public void RefreshLoginPage()
		{
			RefreshPage();
		}

		/// <summary>
		/// Проверить что в поле фото отображается Label photo
		/// </summary>
		public bool CheckLabelForPhoto()
		{
			return GetIsElementDisplay(By.XPath(LABEL_LOAD_PHOTO));
		}

		/// <summary>
		/// Проверить сообщение, что юзер уже существует
		/// </summary>
		public bool CheckErrorMessageThatUserIsAlreadyExist()
		{
			WaitUntilDisplayElement(By.XPath(ERROR_MESSAGE_USER_IS_ALREADY_EXIST));
			return GetIsElementDisplay(By.XPath(ERROR_MESSAGE_USER_IS_ALREADY_EXIST));
		}
		/// <summary>
		/// Открыть страницу авторизация для существующих юзеров (форма Sign In)
		/// </summary>
		public void GoToLoginPageWithExistAccount()
		{
			ClickElement(By.XPath(EXIST_ACCOUNT_LINK_ABBY_ONLINE));
		}

		/// <summary>
		/// Проверить, что сообщение, о том что юзер не существует , отображается корректно
		/// </summary>
		public bool CheckThatErrorMeassageThatUserIsNotExistIsDisplay()
		{
			return GetIsElementDisplay(By.XPath(USER_NOT_EXIST_MESSAGE));
		}

		/// <summary>
		/// Проверить, что сообщение, о том пароль неверный, отображается корректно
		/// </summary>
		public bool CheckThatErrorMeassageThatPasswordIsWrongDisplay()
		{
			return GetIsElementDisplay(By.XPath(WRONG_PASSWORD_MESSAGE));
		}

		public void PressLoadPhoto()
		{
			ClickElement(By.XPath(LABEL_LOAD_PHOTO));
		}


		/// <summary>
		/// Проверить,что сообщение Wrong format появляется , когда загружен неверный формат фото
		/// </summary>
		public bool WrongFormatLabelISDisplay()
		{
			return GetIsElementDisplay(By.XPath(LABEL_WRONG_FORMAT));
		}

		/// <summary>
		/// Нажать кнопку загрузки фото
		/// </summary>
		public void ClickLoadPhotoBtn()
		{
			ClickElement(By.XPath(LOAD_PHOTO_BTN));
		}
		protected const string EMAIL_FIELD_IN_SIGN_UP = "//form[@name='signupForm']//input[@id='email']";
		protected const string PASSWORD_FIELD_IN_SIGN_UP = "//form[@name='signupForm']//input[@id='password']";
		protected const string EMAIL_FIELD_IN_SIGN_IN = "//form[@name='signinForm']//input[@id='email']";
		protected const string PASSWORD_FIELD_IN_SIGN_IN = "//form[@name='signinForm']//input[@id='password']";
		protected const string CONFIRM_PASSWORD_FIELD = "//input[@id='confirm']";
		protected const string SIGN_UP_BUTTON = "//button[@id='btn-sign-up']";
		protected const string SIGN_IN_BUTTON = "//button[contains(text(),'Sign In')]";
		protected const string FIRST_NAME_FIELD = "//input[@id='firstname']";
		protected const string LAST_NAME_FIELD = "//input[@id='lastname']";
		protected const string COUNTRY = "//select[@id='country']";
		protected const string COUNTRY2 = "//select[@id='country']/option[@value='2']";
		protected const string TIMEZONE = "//select[@id='timezone']/option[@value='3']";
		protected const string NATIVE_LANGUAGE =
			"//select[contains(@class, 'multi-select-item ')]//option[@value='4']";
		protected const string SERVICE_PROVIDE = "//table[@class='t-servSelect']//td[1]/select/option[@value='2']";
		protected const string SERVICE_PROVIDE2 = "//table[@class='t-servSelect']//td[3]/select/option[@value='6']";
		protected const string SERVICE_PROVID3 = "//table[@class='t-servSelect']//tbody[3]//select//option[@value='6']";
		protected const string SERVICE_PROVIDE4 = "//table[@class='t-servSelect']//tbody[3]//td[3]/select/option[@value='9']";
		protected const string CREATE_ACCOUNT_BUTTON = "//button[@id='btn-create-account']";
		protected const string LANG_LEVEL = "//tr[contains(@name,'serviceSelectForm')]/td[4]//select/option[@value='0']";
		protected const string LANG_LEVEL2 = "//tr[@class='ng-scope']//td[@class='service_select']";
		protected const string FIRST_STEP_LABEL = "//strong[@class='ng-binding']";
		protected const string SECOND_STEP_LABEL = "//div[@class='panel-heading']/h4";
		protected const string ACCOUNT_XPATH = ".//div[contains(@class,'js-corp-account')]";
		protected const string USER_NAME_XPATH = ACCOUNT_XPATH + "//span[contains(@class,'nameuser')]";
		protected const string ERROR_MESSAGE_USER_IS_ALREADY_EXIST = "//div[@ng-message='already-exists']"; //сообщение, что юзер уже существует
		protected const string CREATE_ACCOUNT_IN_ABBYY_ONLINE_LINK = "//a[text()='account in ABBYY-Online']";
		protected const string USER_NOT_EXIST_MESSAGE = "//p[@class='help-block messagebox ng-binding ng-scope' and contains(@ng-show,'[errors.userNotFound]')]";//сообщение ,что юзера не существует 
		protected const string EXIST_ACCOUNT_LINK_ABBY_ONLINE = "//a[@id='show-sign-in']";
		protected const string WRONG_PASSWORD_MESSAGE = "//p[@class='help-block ng-binding' and contains(@ng-show,'[errors.wrongPassword]')]";
		protected const string LOAD_PHOTO_BTN = "//input[@type='file']";
		protected const string LABEL_WRONG_FORMAT = "//i[text()='Wrong format' and @class='ng-binding']";
		protected const string LABEL_LOAD_PHOTO = "//i[text()='Load photo' and @class='ng-binding']";

		/// <summary>
		/// Заполнить поле имя на 2м шаге стр регистрации компании 
		/// </summary>
		public void FillFirstNameCompany(string firstNameCompany)
		{
			ClearAndAddText(By.XPath(FIRST_NAME_COMPANY), firstNameCompany);
		}

		/// <summary>
		/// Заполнить поле фамилия на 2м шаге стр регистрации компании 
		/// </summary>
		public void FillLastNameCompany(string lastNameCompany)
		{
			ClearAndAddText(By.XPath(LAST_NAME_COMPANY), lastNameCompany);
		}

		/// <summary>
		/// Заполнить поле название компании на 2м шаге стр регистрации компании 
		/// </summary>
		public void FillNameCompany(string nameCompany)
		{
			ClearAndAddText(By.XPath(COMPANY_NAME_COMPANY), nameCompany);
		}

		/// <summary>
		/// Заполнить поле домен на 2м шаге стр регистрации компании 
		/// </summary>
		public void FillDomainNameCompany(string domainNameCompany)
		{
			ClearAndAddText(By.XPath(DOMAIN_NAME_COMPANY), domainNameCompany);
		}

		/// <summary>
		/// Заполнить поле телефон на 2м шаге стр регистрации компании 
		/// </summary>
		public void FillPhoneNumberCompany(string phoneNumber)
		{
			ClearAndAddText(By.XPath(PHONE_NUMBER), phoneNumber);
		}

		/// <summary>
		/// Нажать кнопку Создать аккаунт на 2м шаге стр регистрации компании 
		/// </summary>
		public void ClickCreateAccountCompanyBtn()
		{
			DoubleClickElement(By.XPath(CREATE_ACCOUNT_COMPANY_BTN));
		}


		/// <summary>
		/// Выбрать тип компании на 2м шаге стр регистрации компании 
		/// </summary>
		/// <param name="option"> Номер опции в комбобоксе тип компании </param>
		public void SelectCompanyType(int option)
		{
			ClickElement(By.XPath(COMPANY_TYPE_DD + OPTION_IN_COMPANY_TYPE_DD + option + "']"));
		}

		/// <summary>
		/// Получить Xpath опции в дропдауне тип компании
		/// </summary>
		public string GetOptionXpathFromCompanyDD(string companyTypeOptpion)
		{
			return companyTypeOptpion + "']";
		}

		public bool CheckCreateAccBtnActive()
		{
			return ((GetElementAttribute(By.XPath(CREATE_ACCOUNT_COMPANY_BTN), "disabled") == "true"));
		}

		public void ClickLogInFromMsg()
		{
			ClickElement(By.XPath(LOGIN_LINK_FROM_MSG));
		}

		public bool GetWrongPasswordMsgIsDisplay()
		{
			WaitUntilDisplayElement(By.XPath(WRONG_PASSWORD));
			return GetIsElementDisplay(By.XPath(WRONG_PASSWORD));
		}

		/// <summary>
		/// Проверить, что поле для ввода капчи и сообщение "Введите символы на картинке" появились
		/// </summary>
		public bool GetCaptchaIsDisplayed()
		{
			return (GetIsElementDisplay(By.XPath(CAPTCHA_LABEL)) && GetIsElementDisplay(By.XPath(CAPTCHA_INPUT)));

		}


		/// <summary>
		/// Кликнуть Sign out на странице corp-reg (2й шаг)
		/// </summary>
		public void ClickSignOutBtn()
		{
			ClickElement(By.XPath(SIGN_OUT_BTN));
		}

		/// <summary>
		/// Кликнуть кнопку Add для добавления второй пары языков
		/// </summary>
		public void ClickAddServiceBtn()
		{
			ClickElement(By.XPath(ADD_SERVICE));
		}

		/// <summary>
		/// Вернуть появилось ли сообщение о том, что пароли не совпадают
		/// </summary>
		public bool GetPasswordMatchMsgIsDisplayed()
		{
			return WaitUntilDisplayElement(By.XPath(PASSWORD_ERROR_MATCH));
		}

		/// <summary>
		/// Вернуть появилось ли сообщение о том, email невалидный
		/// </summary>
		public bool GetInvalidEmailMsgIsDisplayed()
		{
			return WaitUntilDisplayElement(By.XPath(EMAIL_INVALID_MSG));
		}
		protected const string FIRST_NAME_COMPANY = ".//input[@id='firstname']";
		protected const string LAST_NAME_COMPANY = ".//input[@id='lastname']";
		protected const string COMPANY_NAME_COMPANY = ".//input[@id='company']";
		protected const string DOMAIN_NAME_COMPANY = ".//input[@id='subdomain']";
		protected const string CREATE_ACCOUNT_COMPANY_BTN = "//button[@id='btn-create-account']";
		protected const string PHONE_NUMBER = ".//input[@id='phone-number']";
		protected const string COMPANY_TYPE_DD = ".//select[@id='company-type']";
		protected const string OPTION_IN_COMPANY_TYPE_DD = "//option[@value='";
		protected const string LOGIN_LINK_FROM_MSG = "//a[@ng-click='showSignIn()' and text()='log in']";
		protected const string WRONG_PASSWORD = "//div[@class='password-messages ng-active']//span[contains(text(),'Wrong')]";
		protected const string CAPTCHA_LABEL = "//label[@for='captcha']";
		protected const string CAPTCHA_INPUT = "//input[@id='captcha']";
		protected const string SIGN_OUT_BTN = "//a[@id='btn-signout']"; // кнопка Sign out на странице corp-reg (2й шаг)
		protected const string ADD_SERVICE = "//div[@class='add-item']//a";
		protected const string PASSWORD_ERROR_MATCH = "//span[contains(@ng-show, 'error.match ')]";
		protected const string EMAIL_INVALID_MSG = "//span[contains(@ng-show,'signupForm.email')]";
	}
}
