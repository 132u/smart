﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

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
			bool isDisplay = WaitUntilDisplayElement(By.CssSelector(EMAIL_CSS), maxwait);

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
		public void ClickSubmitCredentials()
		{
			ClickElement(By.XPath(SUBMIT_BTN_CSS));

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
		/// Кликнуть по имени пользователя
		/// </summary>
		/// <param name="accountName">Имя пользователя</param>
		public void ClickAccountName(string accountName)
		{
			ClickElement(By.XPath(GetAccountItemXPath(accountName)));
		}

		/// <summary>
		/// Дождаться загрузки страницы на lpro
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitPageLoadLpro()
		{
			return WaitUntilDisplayElement(By.XPath(LOGIN_BTN_LPRO_XPATH));
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
			return "//select/option[contains(concat(' ', normalize-space(text()), ' '), ' " + accountName + " ')]";
		}
		
		protected const string EMAIL_CSS = "input[name=\"email\"]";
		protected const string PASSWORD_CSS = "input[name=\"password\"]";
		protected const string SUBMIT_BTN_CSS = "//button[contains(@class, 'btn-danger')]";
		protected const string SUBMIT_BTN_CSS2 = "//button[contains(@class, 'btn btn-danger ng-binding') and @ng-class='{ disabled: selectAccount.$invalid }']";
		protected const string ERROR_XPATH = "//div[contains(@class,'js-dynamic-errors')]";

		protected const string LOGIN_BTN_LPRO_XPATH = "//input[@id='email']";
		protected const string EMAIL_LPRO_XPATH = "email";
		protected const string PASSWORD_LPRO_XPATH = "password";
		protected const string SUBMIT_LPRO_XPATH = "//button[@id='btn-sign-in']";

		protected const string PRO_ELEMENT = "//div[@class='logo-description']";
	}
}

