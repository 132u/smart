using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы регистрации 
	/// </summary>
	public class RegistrationPageHelper : CommonHelper
	{
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

		public string FirstName;
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
			Logger.Trace("Заполняем данные на первом шаге регистрации");
			TypeTextInEmailField(email);
			TypeTextInPasswordField(password);
			TypeTextInConfirmPasswordField(confirmPassword);
		}

		/// <summary>
		/// Заполнить все поля на втором шаге регистрации фрилансера
		/// </summary>
		/// <param name="firstName">имя фрилансера</param>
		/// <param name="lastName">фамилия фридансера</param>
		public void FillRegistrationDataInSecondStep(string firstName, string lastName)
		{
			Logger.Trace("Заполняем данные на втором шаге регистрации");
			FirstName = firstName;
			LastName = lastName;
			TypeTextInFirstNameField(firstName);
			TypeTextInLastNameField(lastName);
			Logger.Trace("Выбираем Country");
			ClickElement(By.XPath(COUNTRY2));
			Logger.Trace("Выбираем Native language");
			ClickElement(By.XPath(NATIVE_LANGUAGE));
			Logger.Trace("Выбираем Language 1");
			ClickElement(By.XPath(SERVICE_PROVIDE));
			Logger.Trace("Выбираем Language 2");
			ClickElement(By.XPath(SERVICE_PROVIDE2));
			Logger.Trace("Выбираем Time zone");
			ClickElement(By.XPath(TIMEZONE));
			Logger.Trace("Выбираем Service");
			ClickElement(By.XPath(LANG_LEVEL));
		}

		/// <summary>
		/// Выбрать вторую пару языков на втором шаге регистрации
		/// </summary>
		public void SelectSecondService()
		{
			ClickAddServiceBtn();
			Logger.Trace("Выбираем Language 1 во второй паре языков");
			ClickElement(By.XPath(SERVICE_PROVID3));
			Logger.Trace("Выбираем Language 2 во второй паре языков");
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
			return WaitUntilDisplayElement(By.XPath(SECOND_STEP_LABEL));
		}

		/// <summary>
		/// Кликнуть по кнопке Sign up - на странице регистрации в форме Sign up
		/// </summary>
		public void ClickSignUpButton()
		{
			Logger.Trace("Нажать кнопку Sign up");
			ClickElement(By.XPath(SIGN_UP_BUTTON));
		}

		/// <summary>
		/// Кликнуть по кнопке Sign In в форме Sign in
		/// </summary>
		public void ClickSignInButton()
		{
			Logger.Trace("Нажать кнопку Sign in");
			ClickElement(By.XPath(SIGN_IN_BUTTON));
		}

		/// <summary>
		/// Ввод текста в поле email - на странице регистрации в форме Sign up
		/// </summary>
		public void TypeTextInEmailField(string email)
		{
			Logger.Trace("Ввод email " + email + " на странице регистрации фрилансеров");
			ClickClearAndAddText(By.XPath(EMAIL_FIELD_IN_SIGN_UP), email);
		}

		/// <summary>
		/// Ввод текста в поле пароля в форме Sign In
		/// </summary>
		/// <param name="password">пароль</param>
		public void TypeTextInPasswordFieldSignIn(string password)
		{
			Logger.Trace("Ввод пароля " + password + " на странице Sign In");
			ClearAndAddText(By.XPath(PASSWORD_FIELD_IN_SIGN_IN), password);
		}

		/// <summary>
		/// Ввод текста в поле email в форме Sign In
		/// </summary>
		/// <param name="email">email</param>
		public void TypeTextInEmailFieldSignIn(string email)
		{
			Logger.Trace("Ввод email " + email + " на странице Sign In");
			ClearAndAddText(By.XPath(EMAIL_FIELD_IN_SIGN_IN), email);
		}

		/// <summary>
		/// Ввод текста в поле пароля в форме Sign Up
		/// </summary>
		///  <param name="password">пароль</param>
		public void TypeTextInPasswordField(string password)
		{
			Logger.Trace("Ввод пароля " + password + " на странице Sign Up");
			ClickClearAndAddText(By.XPath(PASSWORD_FIELD_IN_SIGN_UP), password);
		}

		/// <summary>
		/// Ввод текста в поле подтверждения пароля  в форме Sign Up
		/// </summary>
		///  <param name="password">пароль</param>
		public void TypeTextInConfirmPasswordField(string password)
		{
			Logger.Trace("Ввод пароля " + password + " в поле подтверждения пароля на странице Sign Up");
			ClickClearAndAddText(By.XPath(CONFIRM_PASSWORD_FIELD), password);
		}

		/// <summary>
		/// Ввод текста в поле имя - второй шаг регистрации
		/// </summary>
		/// <param name="firstName">имя</param>
		public void TypeTextInFirstNameField(string firstName)
		{
			Logger.Trace("Ввод имени " + firstName + " на втором шаге регистрации");
			ClearAndAddText(By.XPath(FIRST_NAME_FIELD), firstName);
		}

		/// <summary>
		/// Ввод текста в поле фамилия - второй шаг регистрации
		/// </summary>
		/// <param name="lastName">фамилия</param>
		public void TypeTextInLastNameField(string lastName)
		{
			Logger.Trace("Ввод фамилии " + lastName + " на втором шаге регистрации");
			ClearAndAddText(By.XPath(LAST_NAME_FIELD), lastName);
		}

		/// <summary>
		/// Нажать кнопку создания аккаунта  - второй шаг регистрации
		/// </summary>
		public void ClickCreateAccountButton()
		{
			Logger.Trace("Нажать кнопку Create account на втором шаге регистрации");
			ClickElement(By.XPath(CREATE_ACCOUNT_BUTTON));
		}

		/// <summary>
		/// Проверить , что в панели WS отображается имя и фамилия юзера
		/// </summary>
		public bool CheckNameSurnameInWSPanel()
		{
			Logger.Trace(string.Format("Проверить, что в панели WS отображается имя {0} и фамилия {1} пользователя ", FirstName, LastName));
			return GetTextElement(By.XPath(USER_NAME_XPATH)).Contains(string.Format("{0} {1}", FirstName, LastName));
		}

		/// <summary>
		/// Проверить , что в панели WS отображается только имя юзера
		/// </summary>
		public bool CheckNameInWSPanel()
		{
			Logger.Trace("Проверить, что в панели WS отображается имя пользователя " + FirstName );
			var accountName = Environment.NewLine + GetTextElement(By.XPath(USER_NAME_XPATH +"/small"));
			return GetTextElement(By.XPath(USER_NAME_XPATH)).Replace(accountName, "") == FirstName;
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
			return RandomString.Generate(10) + "@mailforspam.com";
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
			return GetElementAttribute(By.XPath(SIGN_UP_BUTTON), "disabled") == "true";
		}

		/// <summary>
		/// Проверить активна ли кнопка Create new account - второй шаг регистрации
		/// </summary>
		public bool CheckThatCreateAccountBtnIsDisable()
		{
			return GetElementAttribute(By.XPath(CREATE_ACCOUNT_BUTTON), "disabled") == "true";
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
			Logger.Trace("Переход по ссылке 'or sign in with an existing ABBYY account'");
			WaitUntilDisplayElement(By.XPath(EXIST_ACCOUNT_LINK_ABBY_ONLINE));
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
			Logger.Trace("Нажать кнопку загрузки фото");
			ClickElement(By.XPath(LOAD_PHOTO_BTN));
		}


		protected const string EMAIL_FIELD_IN_SIGN_UP = "//form[@name='signupForm']//input[@id='email']";
		protected const string PASSWORD_FIELD_IN_SIGN_UP = "//form[@name='signupForm']//input[@id='password']";
		protected const string EMAIL_FIELD_IN_SIGN_IN = "//form[@name='signinForm']//input[@id='email']";
		protected const string PASSWORD_FIELD_IN_SIGN_IN = "//form[@name='signinForm']//input[@id='password']";
		protected const string CONFIRM_PASSWORD_FIELD = "//input[@id='confirm']";
		protected const string SIGN_UP_BUTTON = "//button[@id='btn-sign-up']";
		protected const string SIGN_IN_BUTTON = "//button[@id='btn-sign-in']";
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
		protected const string ACCOUNT_XPATH = ".//div[contains(@class,'js-usermenu')]";
		protected const string USER_NAME_XPATH = ACCOUNT_XPATH + "//span[contains(@class,'nameuser')]//b";
		protected const string ERROR_MESSAGE_USER_IS_ALREADY_EXIST = "//div[@ng-message='already-exists']"; //сообщение, что юзер уже существует
		protected const string CREATE_ACCOUNT_IN_ABBYY_ONLINE_LINK = "//a[text()='account in ABBYY-Online']";
		protected const string USER_NOT_EXIST_MESSAGE = "//span[contains(@translate, 'USER-NOT-FOUND-ERROR')]";//сообщение ,что юзера не существует 
		protected const string EXIST_ACCOUNT_LINK_ABBY_ONLINE = "//a[@id='show-sign-in']";
		protected const string WRONG_PASSWORD_MESSAGE = "//div[@ng-message='wrong-password']";
		protected const string LOAD_PHOTO_BTN = "//input[@type='file']";
		protected const string LABEL_WRONG_FORMAT = "//i[text()='Wrong format' and @class='ng-binding']";
		protected const string LABEL_LOAD_PHOTO = "//i[text()='Load photo' and @class='ng-binding']";

		/// <summary>
		/// Заполнить поле имя на 2м шаге стр регистрации компании 
		/// </summary>
		public void FillFirstNameCompany(string firstNameCompany)
		{
			Logger.Trace("Ввод имени " + firstNameCompany + " на втором шаге регистрации компаний");
			ClearAndAddText(By.XPath(FIRST_NAME_COMPANY), firstNameCompany);
		}

		/// <summary>
		/// Заполнить поле фамилия на 2м шаге стр регистрации компании 
		/// </summary>
		public void FillLastNameCompany(string lastNameCompany)
		{
			Logger.Trace("Ввод фамилии " + lastNameCompany + " на втором шаге регистрации компаний");
			ClearAndAddText(By.XPath(LAST_NAME_COMPANY), lastNameCompany);
		}

		/// <summary>
		/// Заполнить поле название компании на 2м шаге стр регистрации компании 
		/// </summary>
		public void FillNameCompany(string nameCompany)
		{
			Logger.Trace("Ввод названия компании " + nameCompany + " на втором шаге регистрации компаний");
			ClearAndAddText(By.XPath(COMPANY_NAME_COMPANY), nameCompany);
		}

		public string GetCompanyName()
		{
			Logger.Trace("Получаем название компании из поля ввода на странице регистрации");
			return GetElementAttribute(By.XPath(COMPANY_NAME_COMPANY), "value");
		}

		/// <summary>
		/// Заполнить поле домен на 2м шаге стр регистрации компании 
		/// </summary>
		public void FillDomainNameCompany(string domainNameCompany)
		{
			Logger.Trace("Ввод домена " + domainNameCompany + " на втором шаге регистрации компаний");
			ClearAndAddText(By.XPath(DOMAIN_NAME_COMPANY), domainNameCompany);
		}

		/// <summary>
		/// Заполнить поле телефон на 2м шаге стр регистрации компании 
		/// </summary>
		public void FillPhoneNumberCompany(string phoneNumber)
		{
			Logger.Trace("Ввод телефона " + phoneNumber + " на втором шаге регистрации компаний");
			ClearAndAddText(By.XPath(PHONE_NUMBER), phoneNumber);
		}

		/// <summary>
		/// Нажать кнопку Создать аккаунт на 2м шаге стр регистрации компании 
		/// </summary>
		public void ClickCreateAccountCompanyBtn()
		{
			Logger.Trace("Нажать кнопку Create account на втором шаге регистрации компаний");
			DoubleClickElement(By.XPath(CREATE_ACCOUNT_COMPANY_BTN));
		}

		/// <summary>
		/// Выбрать тип компании на 2м шаге стр регистрации компании 
		/// </summary>
		/// <param name="option"> Номер опции в комбобоксе тип компании </param>
		public void SelectCompanyType(int option)
		{
			Logger.Trace("Выбираем тип компании №" + option);
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
			Logger.Trace("Нажать Log In в сообщение о том, что пользователь уже зарегистрирован");
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
			Logger.Trace("Нажать Sign out на втором шаге регистрации компаний");
			ClickElement(By.XPath(SIGN_OUT_BTN));
		}

		/// <summary>
		/// Кликнуть кнопку Add для добавления второй пары языков
		/// </summary>
		public void ClickAddServiceBtn()
		{
			Logger.Trace("Нажать кнопку Add для добавления второй пары языков");
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

		/// <summary>
		/// Загрузка фото
		/// </summary>
		/// <param name="photo"> название файла </param>
		public void UploadPhoto(string photo)
		{
			Logger.Trace("Загрузка фото");
			UploadDocNativeAction(photo);
		}

		/// <summary>
		/// Ждём появления поля ввода пароля.
		/// </summary>
		public void WaitShowPasswordInput()
		{
			WaitUntilDisplayElement(By.XPath(PASSWORD_FIELD_IN_SIGN_IN));
		}

		/// <summary>
		/// Возвращает отображается ли сообщение о неверном пароле (пустой пароль)
		/// </summary>
		/// <returns>Сообщение отображается</returns>
		public bool GetIsMsgWrongPasswordRequiredDisplay()
		{
			return WaitUntilDisplayElement(By.XPath(PASSWORD_FIELD + WRONG_PASSWORD_MSG_REQUIRED));
		}

		/// <summary>
		/// Возвращает отображается ли сообщение о неверном пароле (минимальная длина)
		/// </summary>
		/// <returns>Сообщение отображается</returns>
		public bool GetIsMsgWrongPasswordMinLenghtDisplay()
		{
			return WaitUntilDisplayElement(By.XPath(PASSWORD_FIELD + WRONG_PASSWORD_MSG_MINLENGHT));
		}

		/// <summary>
		/// Возвращает отображается ли сообщение о неверном пароле (одни пробелы)
		/// </summary>
		/// <returns>Сообщение отображается</returns>
		public bool GetIsMsgWrongPasswordSpacesDisplay()
		{
			return WaitUntilDisplayElement(By.XPath(PASSWORD_FIELD + WRONG_PASSWORD_MSG_SPACES));
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
		protected const string WRONG_PASSWORD = "//div[@ng-message='wrong-password']";
		protected const string CAPTCHA_LABEL = "//label[@for='captcha']";
		protected const string CAPTCHA_INPUT = "//input[@id='captcha']";
		protected const string SIGN_OUT_BTN = "//a[@id='btn-signout']"; // кнопка Sign out на странице corp-reg (2й шаг)
		protected const string ADD_SERVICE = "//div[@class='add-item']//a";
		protected const string PASSWORD_ERROR_MATCH = "//span[contains(@ng-show, 'error.match ')]";
		protected const string EMAIL_INVALID_MSG = "//span[contains(@ng-show,'signupForm.email')]";
		protected const string PASSWORD_FIELD = "//fieldset[contains(@valid,'password')]";
		protected const string WRONG_PASSWORD_MSG_REQUIRED = "//div[contains(@ng-message,'required')]//span[not(contains(@class,'ng-hide'))]";
		protected const string WRONG_PASSWORD_MSG_MINLENGHT = "//div[contains(@ng-message,'minlength')]//span[not(contains(@class,'ng-hide'))]";
		protected const string WRONG_PASSWORD_MSG_SPACES = "//div[contains(@ng-message,'pattern')]//span[not(contains(@class,'ng-hide'))]";
	}
}
