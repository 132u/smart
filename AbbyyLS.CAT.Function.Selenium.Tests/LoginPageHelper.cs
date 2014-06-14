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

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class LoginPageHelper : CommonHelper
    {
        IWebDriver _driver;
        public LoginPageHelper(IWebDriver driver, WebDriverWait wait):
            base (driver, wait)
        {
        }

        /// <summary>
        /// Дождаться загрузки страницы
        /// </summary>
        /// <returns>загрузилась</returns>
        public bool WaitPageLoad()
        {
            return WaitUntilDisplayElement(By.CssSelector(EMAIL_CSS));
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

        public void ClickSubmit()
        {
            ClickElement(By.CssSelector(SUBMIT_BTN_CSS));
        }

        public bool WaitAccountExist(string accountName)
        {
            return WaitUntilDisplayElement(By.XPath(GetAccountItemXPath(accountName)));
        }

        public void ClickAccountName(string accountName)
        {
            ClickElement(By.XPath(GetAccountItemXPath(accountName)));
        }

        protected string GetAccountItemXPath(string accountName)
        {
            return "//select/option[contains(text(), '" + accountName + "')]";
        }

        protected const string EMAIL_CSS = "input[name=\"email\"]";
        protected const string PASSWORD_CSS = "input[name=\"password\"]";
        protected const string SUBMIT_BTN_CSS = "input[type =\"submit\"]";
    }
}