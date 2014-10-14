using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.PEMT.Function.Selenium.Tests
{
	public class LoginPageHelper : CommonHelper
	{
		public LoginPageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
		}

		/// Дождаться загрузки страницы
        /// </summary>
        /// <returns>загрузилась</returns>
		public bool WaitPageLoad(int maxwait = 15)
        {
			bool isDisplay = WaitUntilDisplayElement(By.CssSelector(EMAIL_CSS), maxwait);

			return isDisplay;
        }

		/// <summary>
		/// Ввести логин
		/// </summary>
		/// <param name="login">логин</param>
		public void EnterLogin(string login)
		{
			ClearAndAddText(By.CssSelector(EMAIL_CSS), login);
		}

		/// <summary>
		/// Ввести пароль
		/// </summary>
		/// <param name="password">пароль</param>
		public void EnterPassword(string password)
		{
			ClearAndAddText(By.CssSelector(PASSWORD_CSS), password);
		}

		/// <summary>
		/// Кликнуть Submit
		/// </summary>
		public void ClickSubmit()
		{
			ClickElement(By.CssSelector(SUBMIT_BTN_CSS));
		}

		/// <summary>
		/// Дождаться отображения имени аккаунта
		/// </summary>
		/// <param name="accountName">Имя аакаунта</param>
		/// <param name="waitmax">Максимальный таймаут</param>
		/// <returns>Имя отображается</returns>
		public bool WaitAccountExist(string accountName, int waitmax = 15)
		{
			return WaitUntilDisplayElement(By.XPath(GetAccountItemXPath(accountName)), waitmax);
		}

		/// <summary>
		/// Кликнуть по имени аккаунта
		/// </summary>
		/// <param name="accountName">Имя аккаунта</param>
		public void ClickAccountName(string accountName)
		{
			ClickElement(By.XPath(GetAccountItemXPath(accountName)));
		}

		/// <summary>
		/// Вернуть, появилась ли ошибка
		/// </summary>
		/// <returns>появилась</returns>
		public bool GetIsErrorExist()
		{
			SetDriverTimeoutMinimum();
			bool isErrorExist = GetIsElementDisplay(By.XPath(ERROR_XPATH));
			SetDriverTimeoutDefault();
			return isErrorExist;
		}


		/// <summary>
		/// Возвращает XPath заданного имени пользователя
		/// </summary>
		/// <param name="accountName">Имя пользователя</param>
		/// <returns>XPath</returns>
		protected string GetAccountItemXPath(string accountName)
		{
			return "//select/option[contains(text(), '" + accountName + "')]";
		}

		protected const string EMAIL_CSS = "input[name=\"email\"]";
		protected const string PASSWORD_CSS = "input[name=\"password\"]";
		protected const string SUBMIT_BTN_CSS = "input[type =\"submit\"]";
		protected const string ERROR_XPATH = "//div[contains(@class,'js-dynamic-errors')]";

		/*protected const string LOGIN_BTN_LPRO_XPATH = "btnLogin";
		protected const string EMAIL_LPRO_XPATH = "lfLogin";
		protected const string PASSWORD_LPRO_XPATH = "lfPwd";
		protected const string SUBMIT_LPRO_XPATH = "//input[@value='Login']";*/

	};
}
