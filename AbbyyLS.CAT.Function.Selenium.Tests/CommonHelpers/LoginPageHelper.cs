using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы логиина
	/// </summary>
	public class LoginPageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public LoginPageHelper(IWebDriver driver, WebDriverWait wait):
			base (driver, wait)
		{
		}

		/// <summary>
		/// Дождаться загрузки страницы
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitPageLoad(int maxwait = 15)
		{
			var isDisplay = WaitUntilDisplayElement(By.CssSelector(EMAIL_CSS), maxwait);

			return isDisplay;
		}

		/// <summary>
		/// Дождаться загрузки страницы ОР авторизации по email
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitPageEmailAuthLoad(int maxwait = 15)
		{
			var isDisplay = WaitUntilDisplayElement(By.XPath(EMAIL_AUTH_XPATH), maxwait);

			return isDisplay;
		}

		/// <summary>
		/// Дождаться загрузки страницы http://www.smartcat.pro/?backUrl=%2fworkspace
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitPromoPageLoad(int maxwait = 15)
		{
			return  WaitUntilDisplayElement(By.XPath(PRO_ELEMENT), maxwait);
		}

		/// <summary>
		/// Ввести логин
		/// </summary>
		/// <param name="login">логин</param>
		public void EnterLogin(string login)
		{
			Logger.Trace("Ввод логина " + login + " на странице авторизации");
			ClearAndAddText(By.CssSelector(EMAIL_CSS), login);
		}

		/// <summary>
		/// Ввести пароль
		/// </summary>
		/// <param name="password">пароль</param>
		public void EnterPassword(string password)
		{
			Logger.Trace("Ввод пароля " + password + " на странице авторизации");
			ClearAndAddText(By.CssSelector(PASSWORD_CSS), password);
		}

		/// <summary>
		/// Кликнуть Submit
		/// </summary>
		public void ClickSubmitCredentials()
		{
			Logger.Trace("Клик по кнопке 'Submit'");
			ClickElement(By.XPath(SUBMIT_BTN_CSS));
		}

		/// <summary>
		/// Ввести логин авторизации по email
		/// </summary>
		/// <param name="login">логин</param>
		public void EnterLoginAuthEmail(string login)
		{
			ClearAndAddText(By.XPath(EMAIL_AUTH_XPATH), login);
		}

		/// <summary>
		/// Ввести пароль авторизации по email
		/// </summary>
		/// <param name="password">пароль</param>
		public void EnterPasswordAuthEmail(string password)
		{
			ClearAndAddText(By.XPath(PASSWORD_AUTH_XPATH), password);
		}

		/// <summary>
		/// Кликнуть Submit на странице авторизации по email
		/// </summary>
		public void ClickSubmitAuthEmail()
		{
			Logger.Trace("Клик по кнопке 'Submit' на странице авторизации");
			ClickElement(By.XPath(SUBMIT_BTN_AUTH_XPATH));
		}

		/// <summary>
		/// Дождаться отображения названия аккаунта
		/// </summary>
		/// <param name="accountName">Название аккаунта</param>
		/// <param name="waitmax">Максимальный таймаут</param>
		/// <param name="dataServer">Расположение сервера</param>
		/// <returns>Имя отображается</returns>
		public bool WaitAccountExist(
			string accountName, 
			int waitmax = 15, 
			string dataServer = "Europe")
		{
			Logger.Trace("Ждем появления названия аккаунта " + accountName + " на странице выбора аккаунта");
			return WaitUntilDisplayElement(By.XPath(GetAccountItemXPath(accountName, dataServer)), waitmax);
		}

		/// <summary>
		/// Кликнуть по названию аккаунта
		/// </summary>
		/// <param name="accountName">Название аккаунта</param>
		/// <param name="dataServer">Местоположение сервера</param>
		public void ClickAccountName(string accountName, string dataServer = "Europe")
		{
			ClickElement(By.XPath(GetAccountItemXPath(accountName, dataServer)));
		}

		public void WaitPageLoadLpro()
		{
			Logger.Trace("Дождаться загрузки страницы на lpro");
			Assert.IsTrue(
				WaitUntilDisplayElement(By.XPath(LOGIN_BTN_LPRO_XPATH)),
				"Не прогрузилась страница Login - возможно, сайт недоступен");
		}

		/// <summary>
		/// Дождаться появления заголовка SIGN IN на странице выбора аккаунта
		///  </summary>
		public bool GetSignInHeaderDisplay()
		{
			Logger.Trace("Ожидаем появления заголовка SIGN IN на странице выбора аккаунта");
			return WaitUntilDisplayElement(By.XPath(SIGN_IN_HEADER));
		}

		/// <summary>
		/// Ввести логин на lpro
		/// </summary>
		/// <param name="login">логин</param>
		public void EnterLoginLpro(string login)
		{
			ClearAndAddText(By.Id(EMAIL_LPRO_XPATH), login);
		}

		/// <summary>
		/// Ввести пароль на lpro
		/// </summary>
		/// <param name="password">пароль</param>
		public void EnterPasswordLpro(string password)
		{
			ClearAndAddText(By.Id(PASSWORD_LPRO_XPATH), password);
		}

		/// <summary>
		/// Кликнуть кнопку Login на странице авторизации
		/// </summary>
		public void ClickSubmitLpro()
		{
			ClickElement(By.XPath(SUBMIT_LPRO_XPATH));
		}

		/// <summary>
		/// Кликнуть кнопку Login на странице выбора аккаунта
		/// </summary>
		public void ClickSubmitAccount()
		{
			ClickElement(By.XPath(SUBMIT_BTN_CSS2));
		} 

		/// <summary>
		/// Вернуть, появилась ли ошибка
		/// </summary>
		/// <returns>появилась</returns>
		public bool GetIsErrorExist()
		{
			SetDriverTimeoutMinimum();
			var isErrorExist = GetIsElementDisplay(By.XPath(ERROR_XPATH));
			SetDriverTimeoutDefault();

			return isErrorExist;
		}
		
		/// <summary>
		/// Возвращает XPath аккаунта по его названию
		/// </summary>
		/// <param name="accountName">Название аккаунта</param>
		/// <param name="dataServer">Расположение сервера</param>
		/// <returns>XPath</returns>
		protected string GetAccountItemXPath(string accountName, string dataServer)
		{
			return "//li[text()='" + dataServer + "']/following-sibling::li[@class='ng-scope']//span[text()='" + accountName + "']";
		}

		/// <summary>
		/// Проверить, что Европа отображаестя на стр выбора аккаунта
		/// </summary>
		/// <returns></returns>
		public bool CheckEuropeServerIsDisplayed()
		{
			Logger.Trace("Ожидаем появления 'Europe' на странице выбора аккаунта");
			return WaitUntilDisplayElement(By.XPath(EUROPE_SERVER));
		}

		/// <summary>
		/// Проверить, что США отображаестя на стр выбора аккаунта
		/// </summary>
		/// <returns></returns>
		public bool CheckUsaServerIsDisplayed()
		{
			Logger.Trace("Ожидаем появления 'USA' на странице выбора аккаунта");
			return WaitUntilDisplayElement(By.XPath(USA_SERVER));
		}

		/// <summary>
		/// Метод возвращает кол-во доступных для выбора аккаунтов
		/// </summary>
		public int GetAccountsCount()
		{
			// Sleep не убирать, необходим для авторизации пользователя  с одним аккаунтом
			Thread.Sleep(2000);
			return GetElementList(By.XPath(ACCOUNT_LIST)).Count;
		}
		
		public bool IsOneOfServersNotRespondingErrorExist()
		{
			return WaitUntilDisplayElement(By.XPath(ERROR_NOT_RESPONDING_MESSAGE_XPATH), maxWait: 5);
		}
		
		public void ClickFacebookIcon()
		{
			Logger.Trace("Клик по иконке Facebook"); 
			ClickElement(By.XPath(FACEBOOK_ICON));
		}
		
		public void ClickGoogleIcon()
		{
			Logger.Trace("Клик по иконке Google"); 
			ClickElement(By.XPath(GOOGLE_ICON));
		}

		public void ClickLinkedInIcon()
		{
			Logger.Trace("Клик по иконке LinkedIn"); 
			ClickElement(By.XPath(LINKED_IN_ICON));
		}

		public bool GetEmailFieldIsDisplay()
		{
			return WaitUntilDisplayElement(By.XPath(EMAIL_AUTH_XPATH));
		}

		public bool GetPasswordFieldIsDisplay()
		{
			return WaitUntilDisplayElement(By.XPath(PASSWORD_AUTH_XPATH));
		}

		public bool GetErrorNoEmailDisplay()
		{
			return GetIsElementDisplay(By.XPath(ERROR_NO_EMAIL));
		}

		public bool GetErrorNoFoundEmailDisplay()
		{
			return GetIsElementDisplay(By.XPath(ERROR_USER_NOT_FOUND));
		}

		public bool GetErrorNoPasswordDisplay()
		{
			return GetIsElementDisplay(By.XPath(ERROR_NO_PASSWORD));
		}

		public bool GetErrorWrongPasswordDisplay()
		{
			return GetIsElementDisplay(By.XPath(ERROR_WRONG_PASSWORD));
		}

		public bool GetErrorInvalidEmailDisplay()
		{
			return GetIsElementDisplay(By.XPath(ERROR_INVALID_EMAIL));
		}

		public bool GetMessageAccountNotFoundDisplay()
		{
			WaitUntilDisplayElement(By.XPath(MESSAGE_ACCOUNT_NOT_FOUND));
			return GetIsElementDisplay(By.XPath(MESSAGE_ACCOUNT_NOT_FOUND));
		}

		public bool GetSignUpFreelanceBtnDisplay()
		{
			WaitUntilDisplayElement(By.XPath(SIGN_UP_FREELANCE_BTN));
			return GetIsElementDisplay(By.XPath(SIGN_UP_FREELANCE_BTN));
		}

		public bool GetSignUpCompanyBtnDisplay()
		{
			WaitUntilDisplayElement(By.XPath(SIGN_UP_COMPANY_BTN));
			return GetIsElementDisplay(By.XPath(SIGN_UP_COMPANY_BTN));
		}

		public bool GetErrorNoServerDisplay()
		{
			return GetIsElementDisplay(By.XPath(ERROR_NO_SERVER));
		}

		public string GetErrorNoServerMessage()
		{
			return GetElementAttribute(By.XPath(ERROR_NO_SERVER), "innerHTML");
		}

		protected const string EMAIL_CSS = "input[name=\"email\"]";
		protected const string PASSWORD_CSS = "input[name=\"password\"]";
		protected const string SUBMIT_BTN_CSS = "//button[contains(@class, 'btn-danger')]";
		protected const string SUBMIT_BTN_CSS2 = "//button[contains(@class, 'btn btn-danger ng-binding') and @ng-class='{ disabled: selectAccount.$invalid }']";
		protected const string ERROR_XPATH = "//div[contains(@class,'js-dynamic-errors')]";
		protected const string ERROR_NOT_RESPONDING_MESSAGE_XPATH = "//div[@ng-show='accountServerNotResponding']//span";

		protected const string EMAIL_AUTH_XPATH = "//input[contains(@name,'email')]";
		protected const string PASSWORD_AUTH_XPATH = "//input[@id='password']";
		protected const string SUBMIT_BTN_AUTH_XPATH = "//button[@id='btn-sign-in']";

		protected const string LOGIN_BTN_LPRO_XPATH = "//input[@id='email']";
		protected const string EMAIL_LPRO_XPATH = "email";
		protected const string PASSWORD_LPRO_XPATH = "password";
		protected const string SUBMIT_LPRO_XPATH = "//button[@id='btn-sign-in']";
		protected const string EUROPE_SERVER = "//li[text()='Europe']";
		protected const string USA_SERVER = "//li[text()='USA']";
		protected const string ENGLISH_LANGUAGE_IN_ACCOUNTS_LIST = "//a[@translate='switch-to-en']";

		protected const string PRO_ELEMENT = "//div[@class='logo-description']";

		protected const string ACCOUNT_LIST = "//li[@class='ng-scope']";
		protected const string SIGN_IN_HEADER ="//h1/span[text()='Sign In']";

		protected const string FACEBOOK_ICON = "//a[@class='fb']";
		protected const string GOOGLE_ICON = "//a[@class='gplus']";
		protected const string LINKED_IN_ICON = "//a[@class='linkedin']";
		protected const string ERROR_USER_NOT_FOUND = "//span[@translate='USER-NOT-FOUND-ERROR']";
		protected const string ERROR_NO_EMAIL = "//span[@translate='ENTER-EMAIL']";
		protected const string ERROR_WRONG_PASSWORD = "//span[@translate='ERR-WRONG-PASSWORD']";
		protected const string ERROR_NO_PASSWORD = "//span[@translate='ERR-NO-PASSWORD']";
		protected const string ERROR_INVALID_EMAIL = "//span[@translate='EMAIL-INVALID']";
		protected const string MESSAGE_ACCOUNT_NOT_FOUND = "//b[@translate='ACCOUNT-NOT-FOUND-SIGNED']";
		protected const string SIGN_UP_FREELANCE_BTN = "//a[@translate='FREELANCE']";
		protected const string SIGN_UP_COMPANY_BTN = "//a[@translate='CORPORATE']";
		protected const string ERROR_NO_SERVER = "//span[@translate='ACCOUNT-SERVER-NOT-RESPONDING']";
	}
}

