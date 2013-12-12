using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.IO;
using System.Text;
using System.Configuration;
using System.Diagnostics;

namespace AbbyyLS.CAT.LingvoPro.Selenium.Tests
{
    public class LingvoProTest
    {
        private IWebDriver _driver;
        private string _login;
        private string _password;
        private string _devUrl;
        private string _stageUrl;
        private string _stableUrl;

        public void SetupTest()
        {
            _driver = new FirefoxDriver();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            _login = "bob@test.ru";
            _password = "12345";
            _stageUrl = "https://test-acc.cat-stage.perevedem.local"; 

        }

        public void FirstTest()
        {
            _driver.Navigate().GoToUrl(_stageUrl);
        }



    }
}
