﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

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
			Logger.Trace("Ввод логина на странице авторизации в админке");
			SendTextElement(By.XPath(LOGIN_FORM_LOGIN_XPATH), login);
		}

		/// <summary>
		/// Ввести пароль
		/// </summary>
		/// <param name="pass">пароль</param>
		public void FillPassword(string pass)
		{
			Logger.Trace("Ввод пароля на странице авторизации в админке");
			SendTextElement(By.XPath(LOGIN_FORM_PASSWORD_XPATH), pass);
		}

		/// <summary>
		/// Кликнуть Submit
		/// </summary>
		public void ClickSubmit()
		{
			Logger.Trace("Нажать кнопку 'Сохранить'");
			ClickElement(By.XPath(SUBMIT_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода в корпоративные аккаунты
		/// </summary>
		public void ClickOpenEntepriseAccounts()
		{
			Logger.Trace("Клик по пункту 'Корпоративные' в меню слева");
			ClickElement(By.XPath(ENTERPRISE_ACCOUNTS_REF_XPATH));
		}

		/// <summary>
		/// Дождаться появления сообщения об успехе
		/// </summary>
		/// <returns>появилось сообщение</returns>
		public bool WaitSuccessAnswer()
		{
			Logger.Trace("Ожидание сообщения 'Изменения сохранены'");
			return WaitUntilDisplayElement(By.XPath(SUCCESS_MESSAGE_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода на страницу управления пользователями
		/// </summary>
		/// <returns>страница открыта</returns>
		public bool OpenUserManagementPage()
		{
			Logger.Trace("Клик по ссылке 'Управление пользователями'");
			ClickElement(By.XPath(MANAGEMENT_USERS_REF_XPATH));
			return WaitUntilDisplayElement(By.Id(USER_SEARCH_ID));
		}

		/// <summary>
		/// Кликнуть кнопку управления пользователями для определенного корп аккаунта в таблице аккаунтов
		/// </summary>
		public void ClickUserAddBtnInCorpList(string account)
		{
			Logger.Trace("Клик по кнопке управления пользователями корпоративного аккаунта " + account + " в таблице аккаунтов");
			ClickElement(By.XPath(ACCOUNT_NAME_IN_LIST + account + "']" + ALL_MANAGE_USER_BTN));
		}

		/// <summary>
		/// Ввести имя пользователя в поиск
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public void FillUserNameSearch(string userName)
		{
			Logger.Trace("Ввод имени пользователя " + userName + " в поле поиска");
			SendTextElement(By.Id(USER_SEARCH_ID), userName);
		}

		/// <summary>
		/// Кликнуть Поиск
		/// </summary>
		public void ClickSearchUserBtn()
		{
			Logger.Trace("Клик по кнопке 'Найти'");
			ClickElement(By.Id(USER_SEARCH_BTN_ID));
		}

		/// <summary>
		/// Дождаться появления результатов (появление кнопки Добавить)
		/// </summary>
		/// <returns>появилась</returns>
		public bool WaitSearchUserResult()
		{
			Logger.Trace("Ожидание появления кнопки 'Добавить'");
			return WaitUntilDisplayElement(By.Id(ADD_USER_BTN_ID));
		}

		/// <summary>
		/// Кликнуть Добавить пользователя
		/// </summary>
		public void ClickAddUser()
		{
			Logger.Trace("Клик по кнопке 'Добавить пользователя'");
			ClickElement(By.Id(ADD_USER_BTN_ID));
		}

		/// <summary>
		/// Вернуть, есть ли Словарь в таблице функций слева
		/// </summary>
		/// <returns>есть</returns>
		public bool GetAvailableAddDictionaryFeature()
		{
			Logger.Trace("Проверка есть ли Словарь в таблице функций");
			return GetIsElementExist(By.XPath(DICTIONARY_OPTION_XPATH));
		}

		/// <summary>
		/// Кликнуть по словарям в функциях
		/// </summary>
		public void ClickDictioaryInFeatures()
		{
			Logger.Trace("Клик по 'Lingvo Dictionaries' в таблице функций");
			ClickElement(By.XPath(DICTIONARY_OPTION_XPATH));
		}

		/// <summary>
		/// Кликнуть по To right в таблице Функции
		/// </summary>
		public void ClickToRightFeatureTable()
		{
			Logger.Trace("Клик по стрелке вправо в таблице функций");
			ClickElement(By.XPath(TABLE_FEATURES_XPATH + TO_RIGHT_BTN_XPATH));
		}

		/// <summary>
		/// Добавить все словари
		/// </summary>
		public void AddAllDictionaries()
		{
			Logger.Trace("Клик по стрелке 'Добавить все словари' в таблице 'Пакеты словарей'");
			ClickElement(By.XPath(TABLE_DICTIONARIES_XPATH + ALL_TO_RIGHT_BTN_XPATH));
		}

		/// <summary>
		/// Добавить Dealine дату для словарей
		/// </summary>
		/// <param name="date">дата</param>
		public void FillDictionaryDeadlineDate(DateTime date)
		{
			Logger.Trace("Установка Deadline даты " + date);
			ClearAndAddText(By.Id(DICTIONARY_DEADLINE_DATE_ID), GetDateString(date));
		}

		/// <summary>
		/// Ввести имя аккаунта
		/// </summary>
		/// <param name="name">имя</param>
		public void FillAccountName(string name)
		{
			Logger.Trace("Ввод названия аккаунта " + name);
			SendTextElement(By.XPath(ACCOUNT_NAME_XPATH), name);
		}

		/// <summary>
		/// Ввести название поддомена
		/// </summary>
		/// <param name="name">название</param>
		public void FillSubdomainName(string name)
		{
			Logger.Trace("Ввод названия поддомена " + name);
			SendTextElement(By.XPath(SUBDOMAIN_NAME_XPATH), name);
		}
		
		/// <summary>
		/// Кликнуть Создать аккаунт
		/// </summary>
		public void ClickAddAccount()
		{
			Logger.Trace("Клик по кнопке 'Создать аккаунт'");
			ClickElement(By.XPath(ADD_ACCOUNT_REF_XPATH));
		}

		/// <summary>
		/// Вернуть: видна ли форма создания аккаунта
		/// </summary>
		/// <returns></returns>
		public bool GetIsAddAccountFormDisplay()
		{
			Logger.Trace("Проверка, видна ли форма создания аккаунта");
			return GetIsElementDisplay(By.XPath(CREATE_ACCOUNT_FORM_XPATH));
		}

		/// <summary>
		/// Вернуть: существует ли уже такой корпоративный аккаунт
		/// </summary>
		/// <returns></returns>
		public bool GetCorpAccountExists()
		{
			Logger.Trace("Проверяем, появилось ли сообщение о том, что аккаунт уже существует");
			return GetIsElementDisplay(By.XPath(ACCOUNT_EXISTS_FROM_XPATH));
		}

		/// <summary>
		/// Кликнуть Edit около аккаунта
		/// </summary>
		/// <param name="account"></param>
		public void ClickEditAccount(string account)
		{
			Logger.Trace("Клик по кнопке'Edit' около аккаунта " + account);
			ClickElement(By.XPath(GetAccountEditBtnXPath(account)));
		}

		/// <summary>
		/// Вернуть xPath кнопки Edit около акканта
		/// </summary>
		/// <param name="accountName">название аккаунта</param>
		/// <returns>XPath</returns>
		protected string GetAccountEditBtnXPath(string accountName)
		{
			Logger.Trace("Вернуть XPath кнопки Edit около акканта " + accountName);
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
			Logger.Trace("Проверка, что комбобокс Затея есть на странице");
			if (GetIsElementDisplay(By.XPath(VENTURE_XPATH)))
			{
				Logger.Trace("Выбор нужной затеи: " + name);
				SendTextElement(By.XPath(VENTURE_XPATH), name);
			}
		}

		/// <summary>
		/// Нажать кнопку Создать пользователя
		/// </summary>
		public void ClickCreateNewUserBtn()
		{
			Logger.Trace("Клик по кнопке 'Создать пользователя'");
			ClickElement(By.XPath(CREATE_NEW_USER_XPATH));
		}

		/// <summary>
		/// Заполнить поле email для нового пользователя
		/// </summary>
		/// <param name="emailForNewUser">email</param>
		public void FillEmailForNewUser(string emailForNewUser)
		{
			Logger.Trace("Ввод email " + emailForNewUser + " для нового пользователя");
			ClearAndAddText(By.XPath(EMAIL_FOR_NEW_USER_XPATH), emailForNewUser);
		}

		/// <summary>
		/// Заполнить поле Nickname для нового юзера
		/// </summary>
		/// <param name="nickNameForNewUser">Nickname</param>
		public void FillNickNameForNewUser(string nickNameForNewUser)
		{
			Logger.Trace("Ввод Nickname " + nickNameForNewUser + " для нового пользователя");
			ClearAndAddText(By.XPath(NICKNAME_FOR_NEW_USER_XPATH), nickNameForNewUser);
		}

		/// <summary>
		/// Заполнить поле пароль для нового юзера
		/// </summary>
		/// <param name="password">пароль</param>
		public void FillPasswordForNewUser(string password)
		{
			Logger.Trace("Ввод пароля " + password + " для нового пользователя");
			ClearAndAddText(By.XPath(PASSWORD_FOR_NEW_USER_XPATH), password);
		}

		/// <summary>
		/// Заполнить поле подтверждения пароля для нового юзера
		/// </summary>
		/// <param name="confirmPassword">пароль</param>
		public void FillConfirmPasswordForNewUser(string confirmPassword)
		{
			Logger.Trace("Ввод подтверждения пароля " + confirmPassword + " для нового пользователя");
			ClearAndAddText(By.XPath(PASSWORD2_FOR_NEW_USER_XPATH), confirmPassword);
		}

		/// <summary>
		/// Нажать кнопку Submit при создании нового юзера
		/// </summary>
		public void ClickSubmitBtnNewUser()
		{
			Logger.Trace("Клик по кнопке Submit");
			ClickElement(By.XPath(SAVE_BTN_XPATH));
		}

		/// <summary>
		/// Нажать кнопку Сохранить 
		/// </summary>
		public void ClickSaveBtn()
		{
			Logger.Trace("Клик по кнопке Save");
			ClickElement(By.XPath(SAVE_BTN_XPATH));
		}

		/// <summary>
		/// Нажать кнопку Создать перс аккаунт для нового юзера
		/// </summary>
		public void ClickCreatePersonalAccBtnForNewUser()
		{
			Logger.Trace("Клик по кнопке 'Создать персональный аккаунт'");
			ClickElement(By.XPath(CREATE_PERS_ACCOUNT_XPATH));
		}

		/// <summary>
		/// Клик по ссылке Редактировать персонального аккаунта
		/// </summary>
		public void ClickEditPersonalAccountBtn()
		{
			Logger.Trace("Клик по ссылке Редактировать персонального аккаунта");
			ClickElement(By.XPath(EDIT_PERS_ACCOUNT_XPATH));
		}

		/// <summary>
		/// проверить наличие ссылки Редактировать (если ссылка есть, значит перс. аккаунт есть)
		/// </summary>
		public bool CheckEditPersonalAccountButtonExists()
		{
			Logger.Trace("Проверка наличия ссылки Редактировать");
			return GetIsElementDisplay(By.XPath(EDIT_PERS_ACCOUNT_XPATH));
		}


		/// <summary>
		/// Запонить поле фамилия , когда создается новый перс аккаунт
		/// </summary>
		/// <param name="surname">фамилия</param>
		public void FillSurname(string surname)
		{
			Logger.Trace("Ввод фамилии " + surname + " при создании персонального аккаунта");
			ClickClearAndAddText(By.XPath(SURNAME_FIELD_IN_PERS_ACC), surname);
		}

		/// <summary>
		/// Нажать кнопку Сохранить при создании перс аккаунт
		/// </summary>
		public void ClickSaveBtnPersAcc()
		{
			Logger.Trace("Клик по кнопке 'Save' при создании персонального аккаунта");
			ClickElement(By.XPath(SAVE_BTN_NEW_PEERS_ACC));
		}

		/// <summary>
		/// Проверка стоит ли галочка в чекбоксе Active
		/// </summary>
		public bool IsSelectedActiveCheckbox()
		{
			Logger.Trace("Проверка стоит ли галочка в чекбоксе 'Active'");
			return IsSelected(By.XPath(ACTIVE_CHECKBOX_XPATH));
		}

		/// <summary>
		/// Клик по чекбоксу Active
		/// </summary>
		public void SelectCheckbox()
		{
			Logger.Trace("Клик по чекбоксу 'Active'");
			ClickElement(By.XPath(ACTIVE_CHECKBOX_XPATH));
		}

		/// <summary>
		/// Клик по чекбоксу 'Администратор'
		/// </summary>
		public void CheckIsAdminCheckbox()
		{
			Logger.Trace("Клик по чекбоксу 'Администратор'");
			ClickElement(By.XPath(IS_ADMIN_CHECKBOX));
		}

		/// <summary>
		/// Проверка стоит ли галочка в чекбоксе 'Администратор'
		/// </summary>
		public bool GetIsAdminCheckboxIsChecked()
		{
			Logger.Trace("Проверка стоит ли галочка в чекбоксе 'Администратор'");
			return GetIsInputChecked(By.XPath(IS_ADMIN_CHECKBOX));
		}

		/// <summary>
		/// Кликнуть по ссылке Поиск пользователей в меню
		/// </summary>
		public void ClickSearchUserLink()
		{
			Logger.Trace("Клик по ссылке Поиск пользователей в меню");
			ClickElement(By.XPath(SERACH_USER_LINK));
			WaitUntilDisplayElement(By.XPath(SERACH_USER_LINK));
		}

		/// <summary>
		/// Кликнуть по кнопке Найти рядом с полем поиска
		/// </summary>
		public void ClickFindBtn()
		{
			Logger.Trace("Клик по кнопке Найти рядом с полем поиска");
			ClickElement(By.XPath(FIND_BTN));
		}

		/// <summary>
		/// Кликнуть по email в таблице результатов поиска
		/// </summary>
		public void ClickEmailInSearchResultTable(string email)
		{
			Logger.Trace("Клик по email " + email + " в таблице результатов поиска");
			ClickElement(By.XPath(EMAIL_IN_SEARCH_RES_TABLE + email + "']"));
		}

		/// <summary>
		/// Вернуть: сообщение "Пользователь с таким e-mail уже существует в AOL, теперь добавлен и в БД приложения"
		/// </summary>
		/// <returns></returns>
		public bool GetIsUserIsExistMsgDisplay()
		{
			SetDriverTimeoutMinimum();
			var isDisplay = GetIsElementDisplay(By.XPath(USER_IS_EXIST_MSG));
			SetDriverTimeoutDefault();
			return isDisplay;
		}

		/// <summary>
		/// Отметить Workflow чекбокс при создании корп аккаунта
		/// </summary>
		public void CheckWorkflowCheckbox()
		{
			Logger.Trace("Поставили галочку в  Workflow чекбокс");
			ClickElement(By.XPath(WORKFLOW_CHECKBOX));
		}
		
		/// <summary>
		/// Выбрать функцию
		/// </summary>
		public void SelectFeature(string feature)
		{
			Logger.Trace("Выбрали функцию " + feature);
			ClickElement(By.XPath(FEATURES_OPTIONS + feature+ "']"));
		}

		/// <summary>
		/// Кликнуть стрелку, чтоб доавбить функию
		/// </summary>
		public void ClickRightArrowToAddFeature()
		{
			Logger.Trace("Клик по стрелке, чтоб добавить функцию");
			ClickElement(By.XPath(FEATURES_TO_RIGHT_ARROW));
		}

		/// <summary>
		/// Установить дату окончания действия словаря
		/// </summary>
		/// <param name="date"> дата окончания </param>
		public void SetDictionariesExpirationDate()
		{
			DateTime today = DateTime.Today;
			string nextWeek = today.AddYears(1).ToString("dd.MM.yyyy");
			Logger.Trace("Установка даты " + nextWeek);
			SendTextElement(By.XPath(DICTIONARIES_EXP_DATE), nextWeek);
		}

		public void ClickCalendar()
		{
			Logger.Trace("Клик, чтоб раскрыть календарь");
			ClickElement(By.XPath(DICTIONARIES_EXP_DATE));
		}

		public void SelectYear(string year)
		{
			Logger.Trace("Выбрали год " + year);
			ClickElement(By.XPath(SELCTED_YEAR + year + "']"));
		}

		public void SelectDay(string day)
		{
			Logger.Trace("Выбрали день " + day);
			ClickElement(By.XPath(SELCTED_DAY + "text()='" + day + "']"));
		}
		/// <summary>
		/// Закрываем окно создания корп. аккаунта,если аккаунт с таким именем уже существует.
		/// </summary>
		public void CloseCurrentWindow()
		{
			Logger.Trace("Закрыли окно создания корпоративного аккаунта");
			Driver.Close();
			Logger.Trace("Переключились в первое окно");
			Driver.SwitchTo().Window(Driver.WindowHandles[0]);
		}
		
		/// <summary>
		/// Проверяет, появилось ли сообщение о том,что такой пользователь уже добавлен в аккаунт
		/// </summary>
		public bool CheckAccountAlreadyAdded()
		{
			Logger.Trace("Проверка, появилось ли сообщение 'Изменения не сохранены. Этот пользователь уже есть в этом аккаунте.'");
			return GetIsElementDisplay(By.XPath(THIS_USER_IS_INSIDE_ACCOUNT_XPATH));
		}

		/// <summary>
		/// Зайти на страницу просмотра пакетов словарей 
		/// </summary>
		public void GotoDictionaryPackPage()
		{
			Logger.Trace("Клик по 'Просмотреть пакеты словарей' в меню");
			ClickElement(By.XPath(DICTIONARY_PAGE_PACK_LINK));
		}

		/// <summary>
		/// Проверить, существует ли требуемый пакет словарей в списке
		/// </summary>
		public bool IsRequiredDictionaryPackExist(string packName)
		{
			if (!GetIsElementExist(By.XPath(DICTIONARIES_TABLE_XPATH)))
			{
				return false;
			}

			return GetElementList(By.XPath(PATH_TO_LIST_OF_DICTIONARIES_NAMES))
				.Select(item => item.Text)
				.Any(item => item == packName);
		}

		/// <summary>
		/// Перейти на стриницу со списком словарей
		/// </summary>
		public void SelectDictionaryPack(string packName)
		{
			Logger.Trace("Проверка, что появился список пакетов словарей");
			var requiredDictionaryName =
				GetElementList(By.XPath(PATH_TO_LIST_OF_DICTIONARIES_NAMES))
				.First(item => item.Text == packName);
			Logger.Trace("Клик по пакету " + packName);
			requiredDictionaryName.Click();
		}

		/// <summary>
		/// Вернуть список словарей
		/// </summary>
		public List<string> GetListOfDictionaries()
		{
			Logger.Trace("Получить список словарей в пакете");
			return GetTextListElement(By.XPath(PATH_TO_LIST_OF_DICTIONARIES));
		}

		/// <summary>
		/// Перейти на страницу создания пакета словарей
		/// </summary>
		public void GoToDictionaryPackCreationPage()
		{
			Logger.Trace("Клик по 'Создать новый пакет' в меню");
			ClickElement(By.XPath(CREATE_DICTIONARY_PACK_MENU));
		}

		/// <summary>
		/// Ввети имя пакета словарей
		/// </summary>
		public void AddDictionaryPackName(string dictionaryPackName)
		{
			Logger.Trace("Ввод названия " + dictionaryPackName + " словаря");
			ClickClearAndAddText(By.XPath(DICTIONARY_PACK_NAME), dictionaryPackName);
		}

		/// <summary>
		/// Поставить галочку в чекбоксе 'Общедоступный пакет'
		/// </summary>
		public void MakePublicDictionatyPack()
		{
			Logger.Trace("Поставить галочку в чекбоксе 'Общедоступный пакет'");
			ClickElement(By.XPath(PUBLIC_DICTIONARY_CHECKBOX));
		}

		/// <summary>
		/// Выбрать словари для пакета
		/// </summary>
		public void AddDictionariesToPack(List<string> dictionariesList)
		{
			dictionariesList.ForEach(item =>
			{
				Logger.Trace("Добавляем словарь " + item + " в пакет");
				ClickElement(By.XPath("//select[@id='left']//option[text()='" + item + "']"));
				ClickElement(By.XPath(ADD_DICTIONARY_TO_PACK));
			});
		}

		/// <summary>
		/// Сохранить пакет словарей
		/// </summary>
		public void CreateDictionaryPack()
		{
			Logger.Trace("Клик по кнопке 'Создать пакет'");
			ClickElement(By.XPath(CREATE_DICTIONARY_PACK_BUTTON));
		}

		protected const string LOGIN_FORM_XPATH = "//form[contains(@action,'/Home/Login')]";
		protected const string LOGIN_FORM_LOGIN_XPATH = "//input[@name='email']";
		protected const string LOGIN_FORM_PASSWORD_XPATH = "//input[@name='password']";
		protected const string SUBMIT_BTN_XPATH = "//input[@type='submit']";
		protected const string ENTERPRISE_ACCOUNTS_REF_XPATH = ".//a[@href='/EnterpriseAccounts']";
		protected const string ADD_ACCOUNT_REF_XPATH = "//a[contains(@href,'/EnterpriseAccounts/Edit')]";
		protected const string THIS_USER_IS_INSIDE_ACCOUNT_XPATH = "//div[2]/p";
		protected const string SUCCESS_MESSAGE_XPATH = "//p[contains(@class,'b-success-message')]";
		protected const string MANAGEMENT_USERS_REF_XPATH = "//a[contains(@href,'/EnterpriseAccountUsers/Index')]";
		protected const string USER_SEARCH_ID = "searchText";
		protected const string USER_SEARCH_BTN_ID = "findUser";
		protected const string ADD_USER_BTN_ID = "addUsersBtn";
		protected const string DICTIONARY_DEADLINE_DATE_ID = "DictionariesExpirationDate";
		protected const string DEADLINE_DATE_ID = "ExpirationDate";
		protected const string DICTIONARIES_TABLE_XPATH = "//table[contains(@class, 'js-sortable-table__activated')]";
		protected const string DICTIONARY_PAGE_PACK_LINK = "//a[contains(@href,'/DictionariesPackages')]";
		protected const string PATH_TO_LIST_OF_DICTIONARIES = "//table[contains(@name, 'dictionaries')]//select[contains(@id, 'right')]//option";
		protected const string PATH_TO_LIST_OF_DICTIONARIES_NAMES =
			"//table[contains(@class, 'js-sortable-table__activated')]//tbody//tr//td//a";

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
		protected const string ALL_MANAGE_USER_BTN = "//preceding-sibling::td//a[contains(@href,'/EnterpriseAccountUsers/Index/')]";
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

		protected const string CREATE_DICTIONARY_PACK_MENU = "//a[contains(@href, '/DictionariesPackages/New')]";
		protected const string DICTIONARY_PACK_NAME = "//input[@id='packageSystemName']";
		protected const string PUBLIC_DICTIONARY_CHECKBOX = "//input[@id='isPublic']";
		protected const string ADD_DICTIONARY_TO_PACK = "//input[@name='toRight']";
		protected const string CREATE_DICTIONARY_PACK_BUTTON = "//input[@data-ref='frmCreatePackage']";
	}
}