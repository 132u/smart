using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace AbbyyLs.CAT.Editor.Selenium.Tests
{
    [TestFixture]
    public class SimplestExample
    {

        private IWebDriver _driver;
        private string baseURL;
        private string Login;
        private string Password;


        [SetUp]
        public void SetupTest()
        {
            _driver = new FirefoxDriver();
            baseURL = "http://project-x:10085";
            Login = "a.kurenkova@abbyy-ls.com";
            Password = "8i0fsbrs";
        }

        [TearDown] 
        public void TeardownTest()
        {
            try
            {
                _driver.Quit();
            }
            catch (Exception)
            {
               
            }
        }
        [Test]
        public void AutorizationTest()
        {
            _driver.Navigate().GoToUrl(baseURL + "/");
            _driver.FindElement(By.Id("login")).Clear();
            _driver.FindElement(By.Id("login")).SendKeys(Login);
            _driver.FindElement(By.Id("password")).Clear();
            _driver.FindElement(By.Id("password")).SendKeys(Password);
            _driver.FindElement(By.CssSelector("input.btn")).Click();
            Thread.Sleep(2000);
        }
    }
}
