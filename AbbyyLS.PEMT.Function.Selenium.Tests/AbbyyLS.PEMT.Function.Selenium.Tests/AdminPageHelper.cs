using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.PEMT.Function.Selenium.Tests
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
		/// Проверить - зашли или нет в админку
		/// (видно ссылку на корп.аккаунты)
		/// </summary>
		/// <returns>зашли</returns>
	    public bool GetLoginSuccess()
	    {
		    return GetIsElementDisplay(By.XPath(ENTERPRISE_ACCOUNTS_REF_XPATH));
	    }

        /// <summary>
        /// Кликнуть для перехода в корпоративные аккаунты
        /// </summary>
        public void ClickOpenEntepriseAccounts()
        {
            ClickElement(By.XPath(ENTERPRISE_ACCOUNTS_REF_XPATH));
        }
		
		/// <summary>
		/// Проверить, есть ли аккаунт с таким названием
		/// </summary>
		/// <param name="accountName"></param>
		/// <returns></returns>
	    public bool GetIsAccountExist(string accountName)
	    {
		    return GetIsElementExist(By.XPath(ACCOUNT_NAME_ACCOUNTS_LIST_XPATH + "[contains(text(),'" + accountName + "')]"));
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
        /// Ввести имя аккаунта
        /// </summary>
        /// <param name="name">имя</param>
        public void FillAccountName(string name)
        {
            SendTextElement(By.XPath(ACCOUNT_NAME_XPATH), name);
        }

        /// <summary>
        /// Ввести deadline дату
        /// </summary>
        /// <param name="date">дата</param>
        public void FillDeadLineDate(DateTime date)
        {
            SendTextElement(By.Id(DEADLINE_DATE_ID), GetDateString(date));
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

		protected const string LOGIN_FORM_XPATH = "//form[contains(@action,'/Home/Login')]";
		protected const string LOGIN_FORM_LOGIN_XPATH = "//input[@name='email']";
		protected const string LOGIN_FORM_PASSWORD_XPATH = "//input[@name='password']";
		protected const string SUBMIT_BTN_XPATH = "//input[@type='submit']";
		protected const string ENTERPRISE_ACCOUNTS_REF_XPATH = ".//a[@href='/EnterpriseAccounts']";
		protected const string ADD_ACCOUNT_REF_XPATH = "//a[contains(@href,'/EnterpriseAccounts/Edit')]";
		protected const string SUCCESS_MESSAGE_XPATH = "//p[contains(@class,'b-success-message')]";
		protected const string MANAGEMENT_USERS_REF_XPATH = "//a[contains(@href,'/EnterpriseAccounts/ManageUsers')]";
		protected const string USER_SEARCH_ID = "searchText";
		protected const string USER_SEARCH_BTN_ID = "findUser";
		protected const string ADD_USER_BTN_ID = "addUsersBtn";
		protected const string DEADLINE_DATE_ID = "ExpirationDate";
		
		protected const string ACCOUNT_NAME_XPATH = "//input[@name='Name']";
		protected const string CREATE_ACCOUNT_FORM_XPATH = "//form[contains(@action,'Edit')]";
		protected const string VENTURE_XPATH = "//select[@id='VentureId']";

	    protected const string ACCOUNT_NAME_ACCOUNTS_LIST_XPATH =
		    "//tbody[contains(@class,'js-alternate-rows')]//tr[contains(@class,'b-alternate')]";
    }
}