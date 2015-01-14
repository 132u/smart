using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер админки
	/// </summary>
	public class AdminPageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public AdminPageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
		}

		/// <summary>
		/// Дождаться загрузки страницы
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(LOGIN_FORM_XPATH));
		}

		/// <summary>
		/// Ввести логин
		/// </summary>
		/// <param name="login">логин</param>
		public void FillLogin(string login)
		{
			SendTextElement(By.XPath(LOGIN_FORM_LOGIN_XPATH), login);
		}

		/// <summary>
		/// Ввести пароль
		/// </summary>
		/// <param name="pass">пароль</param>
		public void FillPassword(string pass)
		{
			SendTextElement(By.XPath(LOGIN_FORM_PASSWORD_XPATH), pass);
		}

		/// <summary>
		/// Кликнуть Submit
		/// </summary>
		public void ClickSubmit()
		{
			ClickElement(By.XPath(SUBMIT_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода в корпоративные аккаунты
		/// </summary>
		public void ClickOpenEntepriseAccounts()
		{
			ClickElement(By.XPath(ENTERPRISE_ACCOUNTS_REF_XPATH));
		}

		/// <summary>
		/// Дождаться появления сообщения об успехе
		/// </summary>
		/// <returns>появилось сообщение</returns>
		public bool WaitSuccessAnswer()
		{
			return WaitUntilDisplayElement(By.XPath(SUCCESS_MESSAGE_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода на страницу управления пользователями
		/// </summary>
		/// <returns>страница открыта</returns>
		public bool OpenUserManagementPage()
		{
			ClickElement(By.XPath(MANAGEMENT_USERS_REF_XPATH));

			return WaitUntilDisplayElement(By.Id(USER_SEARCH_ID));
		}

		/// <summary>
		/// Кликнуть кнопку управления пользователями для определенного корп аккаунта в таблице аккаунтов
		/// </summary>
		public void ClickUserAddBtnInCorpList(string account)
		{
			ClickElement(By.XPath(ACCOUNT_NAME_IN_LIST + account + "']" + ALL_MANAGE_USER_BTN));
		}
		/// <summary>
		/// Кликнуть для перехода на страницу управления пользователями определенного аккаунта
		/// </summary>
		/// <param name="accountName">имя аккаунта</param>
		public bool OpenUserManagementPageForAccount(string accountName)
		{
			ClickElement(By.XPath("//td[text()='" + accountName + "']"+ALL_MANAGE_USER_BTN));

			return WaitUntilDisplayElement(By.Id(USER_SEARCH_ID));
		}

		/// <summary>
		/// Ввести имя пользователя в поиск
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public void FillUserNameSearch(string userName)
		{
			SendTextElement(By.Id(USER_SEARCH_ID), userName);
		}

		/// <summary>
		/// Кликнуть Поиск
		/// </summary>
		public void ClickSearchUserBtn()
		{
			ClickElement(By.Id(USER_SEARCH_BTN_ID));
		}

		/// <summary>
		/// Дождаться появления результатов (появление кнопки Добавить)
		/// </summary>
		/// <returns>появилась</returns>
		public bool WaitSearchUserResult()
		{
			return WaitUntilDisplayElement(By.Id(ADD_USER_BTN_ID));
		}

		/// <summary>
		/// Кликнуть Добавить пользователя
		/// </summary>
		public void ClickAddUser()
		{
			ClickElement(By.Id(ADD_USER_BTN_ID));
		}

		/// <summary>
		/// Вернуть, есть ли Словарь в таблице функций слева
		/// </summary>
		/// <returns>есть</returns>
		public bool GetAvailableAddDictionaryFeature()
		{
			return GetIsElementExist(By.XPath(DICTIONARY_OPTION_XPATH));
		}

		/// <summary>
		/// Кликнуть по словарям в функциях
		/// </summary>
		public void ClickDictioaryInFeatures()
		{
			ClickElement(By.XPath(DICTIONARY_OPTION_XPATH));
		}

		/// <summary>
		/// Кликнуть по To right в таблице Функции
		/// </summary>
		public void ClickToRightFeatureTable()
		{
			ClickElement(By.XPath(TABLE_FEATURES_XPATH + TO_RIGHT_BTN_XPATH));
		}

		/// <summary>
		/// Добавить все словари
		/// </summary>
		public void AddAllDictionaries()
		{
			ClickElement(By.XPath(TABLE_DICTIONARIES_XPATH + ALL_TO_RIGHT_BTN_XPATH));
		}

		/// <summary>
		/// Добавить Dealine дату для словарей
		/// </summary>
		/// <param name="date">дата</param>
		public void FillDictionaryDeadlineDate(DateTime date)
		{
			ClearAndAddText(By.Id(DICTIONARY_DEADLINE_DATE_ID), GetDateString(date));
		}

		/// <summary>
		/// Ввести имя аккаунта
		/// </summary>
		/// <param name="name">имя</param>
		public void FillAccountName(string name)
		{
			SendTextElement(By.XPath(ACCOUNT_NAME_XPATH), name);
		}

		/// <summary>
		/// Ввести название поддомена
		/// </summary>
		/// <param name="name">название</param>
		public void FillSubdomainName(string name)
		{
			SendTextElement(By.XPath(SUBDOMAIN_NAME_XPATH), name);
		}
		
		/// <summary>
		/// Кликнуть Создать аккаунт
		/// </summary>
		public void ClickAddAccount()
		{
			ClickElement(By.XPath(ADD_ACCOUNT_REF_XPATH));
		}

		/// <summary>
		/// Вернуть: видна ли форма создания аккаунта
		/// </summary>
		/// <returns></returns>
		public bool GetIsAddAccountFormDisplay()
		{
			return GetIsElementDisplay(By.XPath(CREATE_ACCOUNT_FORM_XPATH));
		}

		/// <summary>
		/// Вернуть: существует ли уже такой корпоративный аккаунт
		/// </summary>
		/// <returns></returns>
		public bool GetCorpAccountExists()
		{
			return GetIsElementDisplay(By.XPath(ACCOUNT_EXISTS_FROM_XPATH));
		}

		/// <summary>
		/// Кликнуть Edit около аккаунта
		/// </summary>
		/// <param name="account"></param>
		public void ClickEditAccount(string account)
		{
			ClickElement(By.XPath(GetAccountEditBtnXPath(account)));
		}

		/// <summary>
		/// Вернуть xPath кнопки Edit около акканта
		/// </summary>
		/// <param name="accountName">название аккаунта</param>
		/// <returns>XPath</returns>
		protected string GetAccountEditBtnXPath(string accountName)
		{
			return "//td[text()='" + accountName + "']/../td[1]" + ADD_ACCOUNT_REF_XPATH;
		}

		/// <summary>
		/// Получить строку с датой нужном формате
		/// </summary>
		/// <param name="date">дата</param>
		/// <returns>строка с датой в нужном формате</returns>
		protected string GetDateString(DateTime date)
		{
			return date.Day + "." + date.Month + "." + date.Year;
		}

		/// <summary>
		/// Выбрать затею
		/// </summary>
		/// <param name="name">название</param>
		public void SetVenture(string name)
		{
			if (GetIsElementDisplay(By.XPath(VENTURE_XPATH)))
			{
				ClickElement(By.XPath(VENTURE_XPATH));
				WaitUntilDisplayElement(By.XPath(VENTURE_XPATH + "//option[contains(@value,'" + name + "')]"));
				SendTextElement(By.XPath(VENTURE_XPATH), name);
				ClickElement(By.XPath(VENTURE_XPATH));
			}
		}

		/// <summary>
		/// Нажать кнопку Создать нового юзера
		/// </summary>
		public void ClickCreateNewUserBtn()
		{
			ClickElement(By.XPath(CREATE_NEW_USER_XPATH));
		}

		/// <summary>
		/// Заполнить поле email для нового юзера
		/// </summary>
		/// <param name="emailForNewUser">email</param>
		public void FillEmailForNewUser(string emailForNewUser)
		{
			ClearAndAddText(By.XPath(EMAIL_FOR_NEW_USER_XPATH), emailForNewUser);
		}

		/// <summary>
		/// Заполнить поле Nickname для нового юзера
		/// </summary>
		/// <param name="nickNameForNewUser">Nickname</param>
		public void FillNickNameForNewUser(string nickNameForNewUser)
		{
			ClearAndAddText(By.XPath(NICKNAME_FOR_NEW_USER_XPATH), nickNameForNewUser);
		}

		/// <summary>
		/// Заполнить поле пароль для нового юзера
		/// </summary>
		/// <param name="password">пароль</param>
		public void FillPasswordForNewUser(string password)
		{
			ClearAndAddText(By.XPath(PASSWORD_FOR_NEW_USER_XPATH), password);
		}

		/// <summary>
		/// Заполнить поле подтверждения пароля для нового юзера
		/// </summary>
		/// <param name="confirmPassword">пароль</param>
		public void FillConfirmPasswordForNewUser(string confirmPassword)
		{
			ClearAndAddText(By.XPath(PASSWORD2_FOR_NEW_USER_XPATH), confirmPassword);
		}

		/// <summary>
		/// Нажать кнопку Submit при создании нового юзера
		/// </summary>
		public void ClickSubmitBtnNewUser()
		{
			ClickElement(By.XPath(SAVE_BTN_XPATH));
		}

		/// <summary>
		/// Нажать кнопку Сохранить 
		/// </summary>
		public void ClickSaveBtn()
		{
			ClickElement(By.XPath(SAVE_BTN_XPATH));
		}

		/// <summary>
		/// Нажать кнопку Создать перс аккаунт для нового юзера
		/// </summary>
		public void ClickCreatePersonalAccBtnForNewUser()
		{
			ClickElement(By.XPath(CREATE_PERS_ACCOUNT_XPATH));
		}

		/// <summary>
		/// переход по сылке Редактировать для редактирования перс. аккаунта для юзера
		/// </summary>
		public void ClickEditPersonalAccountBtn()
		{
			ClickElement(By.XPath(EDIT_PERS_ACCOUNT_XPATH));
		}

		/// <summary>
		/// проверить наличие ссылки Редактировать (если ссылка есть, значит перс. аккаунт есть)
		/// </summary>
		public bool CheckEditPersonalAccountButtonExists()
		{
			return GetIsElementDisplay(By.XPath(EDIT_PERS_ACCOUNT_XPATH));
		}


		/// <summary>
		/// Запонить поле фамилия , когда создается новый перс аккаунт
		/// </summary>
		/// <param name="surname">фамилия</param>
		public void FillSurname(string surname)
		{
			ClickClearAndAddText(By.XPath(SURNAME_FIELD_IN_PERS_ACC), surname);
		}

		/// <summary>
		/// Нажать кнопку Сохранить при создании перс аккаунт
		/// </summary>
		public void ClickSaveBtnPersAcc()
		{
			ClickElement(By.XPath(SAVE_BTN_NEW_PEERS_ACC));
		}

		/// <summary>
		/// Проверить чексбокс Active 
		/// </summary>
		/// <returns>чекнут(true) или не чекнут чекбокс(false)</returns>
		public bool IsSelectedActiveCheckbox()
		{
			return IsSelected(By.XPath(ACTIVE_CHECKBOX_XPATH));
		}

		/// <summary>
		/// Чекнуть Active чебкбокс 
		/// </summary>
		public void SelectCheckbox()
		{
			ClickElement(By.XPath(ACTIVE_CHECKBOX_XPATH));
		}

		/// <summary>
		/// Чекнуть чекбокс Администратор
		/// </summary>
		public void ChechIsAdminCheckbox()
		{
			ClickElement(By.XPath(IS_ADMIN_CHECKBOX));
		}

		/// <summary>
		/// Определить чекунт ли Admin чекбокс
		/// </summary>
		public bool GetIsAdminCheckboxIsChecked()
		{
			return GetIsInputChecked(By.XPath(IS_ADMIN_CHECKBOX));
		}


		/// <summary>
		/// Кликнуть по ссылке Поиск пользователей в меню слева
		/// </summary>
		public void ClickSearchUserLink()
		{
			ClickElement(By.XPath(SERACH_USER_LINK));
			WaitUntilDisplayElement(By.XPath(SERACH_USER_LINK));
		}

		/// <summary>
		/// Кликнуть по кнопке Найти рядом с полем поиска
		/// </summary>
		public void ClickFindBtn()
		{
			ClickElement(By.XPath(FIND_BTN));
		}

		/// <summary>
		/// Кликнуть по email в таблице результатов поиска
		/// </summary>
		public void ClickEmailInSearchResultTable(string email)
		{
			ClickElement(By.XPath(EMAIL_IN_SEARCH_RES_TABLE + email + "']"));
		}

		/// <summary>
		/// Вернуть: сообщение "Пользователь с таким e-mail уже существует в AOL, теперь добавлен и в БД приложения"
		/// </summary>
		/// <returns></returns>
		public bool GetIsUserIsExistMsgDisplay()
		{
			return GetIsElementDisplay(By.XPath(USER_IS_EXIST_MSG));
		}

		/// <summary>
		/// Отметить Workflow чекбокс при создании корп аккаунта
		/// </summary>
		public void CheckWorkflowCheckbox()
		{
			ClickElement(By.XPath(WORKFLOW_CHECKBOX));
		}
		
		/// <summary>
		/// Выбрать функцию
		/// </summary>
		public void SelectFeature(string feature)
		{
			ClickElement(By.XPath(FEATURES_OPTIONS + feature+ "']"));
		}

		/// <summary>
		/// Кликнуть стрелку, чтоб доавбить функию
		/// </summary>
		public void ClickRightArrowToAddFeature()
		{
			ClickElement(By.XPath(FEATURES_TO_RIGHT_ARROW));
		}

		/// <summary>
		/// Установить дату окончания действия словаря
		/// </summary>
		/// <param name="date"> дата окончания </param>
		public void SetDictionariesExpirationDate()
		{
			DateTime today = DateTime.Today;
			string nextWeek = today.AddYears(1).ToString("dd/MM/yyyy");
			SendTextElement(By.XPath(DICTIONARIES_EXP_DATE), nextWeek);
		}
		public void ClickCalendar()
		{
			ClickElement(By.XPath(DICTIONARIES_EXP_DATE));
			//ClickElement(By.XPath(CALENDAR));
		}

		public void SelectYear(string year)
		{
			ClickElement(By.XPath(SELCTED_YEAR + year + "']"));
		}

		public void SelectDay(string day)
		{
			ClickElement(By.XPath(SELCTED_DAY + "text()='" + day + "']"));
		}
		/// <summary>
		/// Закрываем окно создания корп. аккаунта,если аккаунт с таким именем уже существует.
		/// </summary>
		public void CloseCurrentWindow()
		{
			//закрываем окно, если сообщение появилось
			Driver.Close();
			//переключаемся на старое окно
			Driver.SwitchTo().Window(Driver.WindowHandles[0]);
		}
		
		/// <summary>
		/// Проверяет, появилось ли сообщение о том,что такой пользователь уже добавлен в аккаунт
		/// </summary>
		public bool CheckAccountAlreadyAdded()
		{
			return GetIsElementDisplay(By.XPath(THIS_USER_IS_INSIDE_ACCOUNT_XPATH));
		}

		protected const string LOGIN_FORM_XPATH = "//form[contains(@action,'/Home/Login')]";
		protected const string LOGIN_FORM_LOGIN_XPATH = "//input[@name='email']";
		protected const string LOGIN_FORM_PASSWORD_XPATH = "//input[@name='password']";
		protected const string SUBMIT_BTN_XPATH = "//input[@type='submit']";
		protected const string ENTERPRISE_ACCOUNTS_REF_XPATH = ".//a[@href='/EnterpriseAccounts']";
		protected const string ADD_ACCOUNT_REF_XPATH = "//a[contains(@href,'/EnterpriseAccounts/Edit')]";
		protected const string THIS_USER_IS_INSIDE_ACCOUNT_XPATH = "//div[2]/p";
		protected const string SUCCESS_MESSAGE_XPATH = "//p[contains(@class,'b-success-message')]";
		protected const string MANAGEMENT_USERS_REF_XPATH = "//a[contains(@href,'/EnterpriseAccounts/ManageUsers')]";
		protected const string USER_SEARCH_ID = "searchText";
		protected const string USER_SEARCH_BTN_ID = "findUser";
		protected const string ADD_USER_BTN_ID = "addUsersBtn";
		protected const string DICTIONARY_DEADLINE_DATE_ID = "DictionariesExpirationDate";
		protected const string DEADLINE_DATE_ID = "ExpirationDate";

		protected const string TABLE_FEATURES_XPATH = "//table[@name='Features']";
		protected const string TABLE_DICTIONARIES_XPATH = "//table[@name='dictionariesPackages']";
		protected const string DICTIONARY_OPTION_XPATH = TABLE_FEATURES_XPATH + "//select[@id='left']//option[@value='LingvoDictionaries']";

		protected const string TO_RIGHT_BTN_XPATH = "//input[@name='toRight']";
		protected const string ALL_TO_RIGHT_BTN_XPATH = "//input[@name='allToRight']";

		protected const string ACCOUNT_NAME_XPATH = "//input[@name='Name']";
		protected const string SUBDOMAIN_NAME_XPATH = "//input[@name='SubDomain']";
		protected const string CREATE_ACCOUNT_FORM_XPATH = "//form[contains(@action,'Edit')]";
		protected const string ACCOUNT_EXISTS_FROM_XPATH = "//div[2]/span[@class='field-validation-error']";//подпись о том, что аккаунт с таким именем уже есть. 
		protected const string VENTURE_XPATH = "//select[@id='VentureId']";

		protected const string CREATE_NEW_USER_XPATH = "//a[@href='/Users/Create']";
		protected const string EMAIL_FOR_NEW_USER_XPATH = "//input[@id='EMail']";
		protected const string NICKNAME_FOR_NEW_USER_XPATH = "//input[@id='Nickname']";
		protected const string PASSWORD_FOR_NEW_USER_XPATH = "//div[6]/input[@class='inputField']";
		protected const string PASSWORD2_FOR_NEW_USER_XPATH = "//div[8]/input[@class='inputField']";
		protected const string SAVE_BTN_XPATH = "//p[@class='submit-area']/input";
		protected const string CREATE_PERS_ACCOUNT_XPATH = "//form[@action='/Users/CreatePersonalAccount']/input[2]"; //кнопка создать перс акккаунт
		protected const string EDIT_PERS_ACCOUNT_XPATH = "//div[@class='b-form']/p[1]/a[1]"; //ссылка Редактировать перс. аккаунт
		protected const string SURNAME_FIELD_IN_PERS_ACC = ".//input[@id='Surname']"; // поле фамилия на стр создания перс аккаунта
		protected const string SAVE_BTN_NEW_PEERS_ACC = "//p[@class='submit-area']/input"; //кнопка Сохранить при создании персонального аккаунта
		protected const string ACTIVE_CHECKBOX_XPATH = "//input[@type='checkbox' and @id='IsActive']"; // чекбокс Active
		//все кнопки ManageUsers в таблице корпоративных аккаунтов
		protected const string ALL_MANAGE_USER_BTN = "//preceding-sibling::td//a[contains(@href,'/EnterpriseAccounts/ManageUsers/')]";
		protected const string ACCOUNT_NAME_IN_LIST = "//td[text()='";
		//td[text()='Coursera3']/preceding-sibling::td//div//a[contains(@href,'/EnterpriseAccounts/ManageUsers')]
		protected const string IS_ADMIN_CHECKBOX = "//input[@id='isAdmin'] ";
		protected const string SERACH_USER_LINK = "//a[@href='/Users']";
		protected const string FIND_BTN = "//form[@action='/Users']/input[2]";
		protected const string EMAIL_IN_SEARCH_RES_TABLE = "//a[text()='";
		protected const string USER_IS_EXIST_MSG = "//fieldset//div[2]/span[contains(text(),'таким e-mail уже существует в AOL')]";
		protected const string WORKFLOW_CHECKBOX = "//input[@id='WorkflowEnabled']";

		protected const string FEATURES_TABLE = "//table[@name='Features']";
		protected const string FEATURES_OPTIONS = FEATURES_TABLE + "//select[@id='left']//option[@value='";
		protected const string FEATURES_TO_RIGHT_ARROW = FEATURES_TABLE + "//input[@name='toRight']";
		protected const string DICTIONARIES_EXP_DATE = "//input[@id='DictionariesExpirationDate']";
		protected const string CALENDAR = "//div[@id='ui-datepicker-div']";
		protected const string SELCTED_YEAR = "//select[@class='ui-datepicker-year']/option[@value='";
		protected const string SELCTED_DAY = "//table[@class='ui-datepicker-calendar']//tbody//td//a[";
	}
}