using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

using OpenQA.Selenium.Interactions;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class DictionaryTest : BaseTest
    {
        public DictionaryTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        /// <summary>
        /// Старт тестов, переменные
        /// </summary>
        [SetUp]
        public void SetupTest()
        {}

        /// <summary>
        /// Тест: проверка перевода со словарями
        /// </summary>
        [Test]
        public void CheckResultTranslation()
        {
            AddAccountWithDictionaries();

            // Найти перевод слова
            InitSearch("tester");
            // Проверить результат: должен быть показан результат из словаря
            Assert.IsTrue(IsElementPresent(By.XPath(".//div[contains(@class,'js-search-results')]//div[contains(@class,'l-articles')]/div/span[2]")),
                "Ошибка: не показаны результаты поиска по словарю");
            // Проверить словарь
            string resultText = Driver.FindElement(By.XPath(".//div[contains(@class,'js-search-results')]//div[contains(@class,'l-articles')]/div/span[2]")).Text;
            Assert.AreEqual("ABBYY Lingvo dictionaries (En-Ru)", resultText,
                "Ошибка: не тот словарь: " + resultText);
        }

        /// <summary>
        /// Тест: проверка, что перевод является ссылкой
        /// </summary>
        [Test]
        public void CheckTranslationReference()
        {
            AddAccountWithDictionaries();

            // Найти перевод слова
            InitSearch("tester");
            
            // Проверить, что перевод - ссылка
            Assert.IsTrue(IsElementPresent(By.XPath(".//a[contains(@class,'js-show-examples')]//span[contains(@class,'translation')]")),
                "Ошибка: перевод - не ссылка");
        }

        /// <summary>
        /// Тест: проверка обратного перевода
        /// </summary>
        [Test]
        public void CheckReverseTranslation()
        {
            AddAccountWithDictionaries();

            // Найти перевод слова
            InitSearch("tester");

            // Нажать на перевод
            Driver.FindElement(By.XPath(".//a[contains(@class,'js-show-examples')]//span[contains(@class,'translation')]")).Click();

            // Дождаться открытия форму с переводом
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-window-examples-data')]")));
            // Кликнуть по переводу
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-window-examples-data')]//a[contains(@class,'g-winexamp__reverse')]")).Click();

            // Дождаться появления результата (перевод по каждому слову)
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'l-wordbyword')]")));

            // Проверить, что Source теперь Русский
            Assert.IsTrue(IsElementPresent(By.XPath(".//select[@id='SearchSrcLang']//option[@value='ru'][@selected='selected']")),
                "Ошибка: язык Source не изменился на русский");

            // Проверить, что появились переводы слов
            Assert.IsTrue(IsElementPresent(By.XPath(".//div[contains(@class,'l-wordbyword')]//table//td//a[contains(@href,'Translate/ru-en')]")),
                "Ошибка: нет обратных переводов");
        }

        /// <summary>
        /// Тест: проверка, что автоматически изменяется пара Source-Target языков
        /// </summary>
        [Test]
        public void CheckAutoReverse()
        {
            AddAccountWithDictionaries();

            // Найти перевод слова
            InitSearch("tester");

            // Нажать на перевод
            Driver.FindElement(By.XPath(".//a[contains(@class,'js-show-examples')]//span[contains(@class,'translation')]")).Click();

            // Проверить, что появилось сообщение об автоматическом изменении языков
            Assert.IsTrue(IsElementPresent(By.XPath(".//div[contains(@class,'js-language-autoreversed')]")),
                "Ошибка: не появилось сообщение об автоматическом изменении языков");
            // Проверить ссылку дял возврата языков
            Assert.IsTrue(IsElementPresent(By.XPath(".//div[contains(@class,'js-language-autoreversed')]//a[contains(@href,'/Translate/ru-en')]")),
                "Ошибка: не появилось ссылки для возврата языков");
        }

        /// <summary>
        /// Тест: проверка создания аккаунта со списком словарей
        /// </summary>
        [Test]
        public void AccountExistDictionaryList()
        {
            // Открыть форму создания аккаунта
            OpenCreateAccountForm();

            // Заполнить форму аккаунта
            string accountName = FillGeneralAccountFields();
            // Добавить словари
            AddDictionaryAccount();

            // Сохранить
            Driver.FindElement(By.XPath(".//input[@type='submit']")).Click();

            // Дождаться успеха
            Wait.Until((d) => d.FindElement(By.XPath(".//p[contains(@class,'b-success-message')]")).Displayed);

            // Добавить пользователя в аккаунт
            AddUserToAccount();

            // Перейти в CAT
            Driver.Navigate().GoToUrl(Url);
            // Зайти пользователем
            Authorization(accountName);

            // Проверка, что вкладка LingvoDictionaries видна
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//a[contains(@href,'/LingvoDictionaries')]")),
                "Ошибка: не показывается вкладка Lingvo Dictionaries");

            // Перейти на вкладку со словарями
            Driver.FindElement(By.XPath(".//a[contains(@href,'/LingvoDictionaries')]")).Click();

            IList<IWebElement> dictList = Driver.FindElements(By.XPath(
                ".//div[contains(@class,'js-dictionaries-search-result')]//div[contains(@class,'l-dctnrs__dict')]"));
            Assert.IsTrue(dictList.Count > 0, "Ошибка: список словарей пуст");
        }

        /// <summary>
        /// Тест: создание аккаунта без функции Словари
        /// </summary>
        [Test]
        public void AccountWithoutDictionaryTab()
        {
            // Открыть форму создания аккаунта
            OpenCreateAccountForm();

            // Заполнить форму аккаунта
            string accountName = FillGeneralAccountFields();

            // Сохранить
            Driver.FindElement(By.XPath(".//input[@type='submit']")).Click();

            // Дождаться успеха
            Wait.Until((d) => d.FindElement(By.XPath(".//p[contains(@class,'b-success-message')]")).Displayed);

            // Добавить пользователя в аккаунт
            AddUserToAccount();

            // Перейти в CAT
            Driver.Navigate().GoToUrl(Url);
            // Зайти пользователем
            Authorization(accountName);

            // Проверка, что вкладка LingvoDictionaries не видна
            Assert.IsFalse(IsElementDisplayed(By.XPath(".//a[contains(@href,'/LingvoDictionaries')]")),
                "Ошибка: вкладка Lingvo Dictionaries видна (не должно быть видно)");
        }

        /// <summary>
        /// Тест: создание аккаунта с Фукнцией Словари, но без списка словарей
        /// </summary>
        [Test]
        public void AccountDictionaryEmptyList()
        {
            // Открыть форму создания аккаунта
            OpenCreateAccountForm();

            // Заполнить форму аккаунта
            string accountName = FillGeneralAccountFields();
            // Добавить функцию Словари, но не добавлять список словарей
            AddDictionaryAccount(false);

            // Сохранить
            Driver.FindElement(By.XPath(".//input[@type='submit']")).Click();

            // Дождаться успеха
            Wait.Until((d) => d.FindElement(By.XPath(".//p[contains(@class,'b-success-message')]")).Displayed);

            // Добавить пользователя в аккаунт
            AddUserToAccount();

            // Перейти в CAT
            Driver.Navigate().GoToUrl(Url);
            // Зайти пользователем
            Authorization(accountName);

            // Проверка, что вкладка LingvoDictionaries видна
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//a[contains(@href,'/LingvoDictionaries')]")),
                "Ошибка: не показывается вкладка Lingvo Dictionaries");

            // Перейти на вкладку со словарями
            Driver.FindElement(By.XPath(".//a[contains(@href,'/LingvoDictionaries')]")).Click();

            IList<IWebElement> dictList = Driver.FindElements(By.XPath(
                ".//div[contains(@class,'js-dictionaries-search-result')]//div[contains(@class,'l-dctnrs__dict')]"));
            Assert.IsTrue(dictList.Count == 0, "Ошибка: список словарей НЕ пуст");
        }

        /// <summary>
        /// Тест: создание аккаунта, добавление словарей при редактировании
        /// </summary>
        [Test]
        public void AccountAddDictionaries()
        {
            // Открыть форму создания аккаунта
            OpenCreateAccountForm();

            // Заполнить форму аккаунта
            string accountName = FillGeneralAccountFields();
            // Добавить функцию Словари, но не добавлять список словарей
            AddDictionaryAccount(false);

            // Сохранить
            Driver.FindElement(By.XPath(".//input[@type='submit']")).Click();

            // Дождаться успеха
            Wait.Until((d) => d.FindElement(By.XPath(".//p[contains(@class,'b-success-message')]")).Displayed);

            // Добавить пользователя в аккаунт
            AddUserToAccount();

            // Перейти в корпоративные аккаунты
            SwitchEnterpriseAccountList();
            // Нажать редактирование созданного аккаунта
            Driver.FindElement(By.XPath(".//td[text()='" + accountName + "']/../td[1]//a[contains(@href,'/EnterpriseAccounts/Edit')]")).Click();

            // Добавить словари
            AddDictionaryAccount();
            // Сохранить
            Driver.FindElement(By.XPath(".//input[@type='submit']")).Click();

            // Перейти в CAT
            Driver.Navigate().GoToUrl(Url);
            // Зайти пользователем
            Authorization(accountName);

            // Проверка, что вкладка LingvoDictionaries видна
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//a[contains(@href,'/LingvoDictionaries')]")),
                "Ошибка: не показывается вкладка Lingvo Dictionaries");

            // Перейти на вкладку со словарями
            Driver.FindElement(By.XPath(".//a[contains(@href,'/LingvoDictionaries')]")).Click();

            IList<IWebElement> dictList = Driver.FindElements(By.XPath(
                ".//div[contains(@class,'js-dictionaries-search-result')]//div[contains(@class,'l-dctnrs__dict')]"));
            Assert.IsTrue(dictList.Count > 0, "Ошибка: список словарей пуст");
        }

        /// <summary>
        /// Тест: проверка перехода на Definitions, если Source и Target языки совпадают
        /// </summary>
        [Test]
        public void CheckDefinitions()
        {
            // Зайти
            Authorization();
            SwitchSearchTab();
            Driver.FindElement(By.Id("SearchSrcLang")).Click();
            Driver.FindElement(By.XPath(".//select[@id='SearchSrcLang']//option[@value='en']")).Click();
            SendKeys.SendWait(@"{Enter}");
            Driver.FindElement(By.Id("SearchDestLang")).Click();
            Driver.FindElement(By.XPath(".//select[@id='SearchDestLang']//option[@value='en']")).Click();
            SendKeys.SendWait(@"{Enter}");
            
            // Найти перевод слова
            InitSearch("tester");
            Wait.Until((d) => d.FindElement(By.XPath(".//p[contains(@class,'l-glossary__nothingFound')]")));
            string definitionClass = Driver.FindElement(By.XPath(".//li[contains(@data-search-mode,'Interpret')]")).GetAttribute("class");
            // Проверить, что вкладка Definitions активна
            Assert.IsTrue(definitionClass.Contains("active"), "Ошибка: не перешли на вкладку Definitions");
        }

        /// <summary>
        /// Открыть форму создания аккаунта
        /// </summary>
        protected void OpenCreateAccountForm()
        {
            // Перейти в админку
            Driver.Navigate().GoToUrl(AdminUrl);
            // Дождаться открытия формы входа
            Wait.Until((d) => d.FindElement(By.XPath(".//form[contains(@action,'/Home/Login')]")));
            // Логин
            Driver.FindElement(By.XPath(".//input[@name='email']")).SendKeys(Login);
            Driver.FindElement(By.XPath(".//input[@name='password']")).SendKeys(Password);
            Driver.FindElement(By.XPath(".//input[@type='submit']")).Click();

            // Зайти в аккаунты
            SwitchEnterpriseAccountList();

            // Нажать Создать
            Driver.FindElement(By.XPath(".//a[contains(@href,'/EnterpriseAccounts/Edit')]")).Click();

            bool isWinWithForm = false;
            Driver.SwitchTo().Window(Driver.WindowHandles[1]);
            isWinWithForm = IsElementPresent(By.XPath(".//form[contains(@action,'Edit')]"));

            // Если вдруг неправильное окно - перебирать все окна в цикле
            /*foreach (string winH in Driver.WindowHandles)
            {
                Driver.SwitchTo().Window(winH);
                isWinWithForm = IsElementPresent(By.XPath(".//form[contains(@action,'Edit')]"));
                if (isWinWithForm)
                {
                    break;
                }
            }*/

            Assert.IsTrue(isWinWithForm, "Ошибка: не нашли окно с формой создания аккаунта");
        }

        /// <summary>
        /// Заполнить основные поля создания аккаунта
        /// </summary>
        /// <returns>имя аккаунта</returns>
        protected string FillGeneralAccountFields()
        {
            // Заполнить форму аккаунта
            string uniqPref = DateTime.Now.Ticks.ToString();
            string accountName = "TestAccount" + uniqPref;
            // Название
            Driver.FindElement(By.XPath(".//input[@name='Name']")).SendKeys(accountName);
            // Поддомен
            Driver.FindElement(By.XPath(".//input[@name='SubDomain']")).SendKeys("testaccount" + uniqPref);

            // Дата
            string deadLineDate = DateTime.Now.AddDays(10).Date.ToString();
            deadLineDate = deadLineDate.Substring(0, deadLineDate.IndexOf(" "));
            Driver.FindElement(By.Id("ExpirationDate")).SendKeys(deadLineDate);

            // Вернуть имя аккаунта
            return accountName;
        }

        /// <summary>
        /// Добавить словари в аккаунт
        /// </summary>
        /// <param name="needAddAllDictionaries">добавить все словари из списка (false - не добавлять)</param>
        protected void AddDictionaryAccount(bool needAddAllDictionaries = true)
        {
            // Выбрать Функцию Словари
            if (IsElementPresent(By.XPath(".//table[@name='Features']//select[@id='left']//option[@value='LingvoDictionaries']")))
            {
                Driver.FindElement(By.XPath(".//table[@name='Features']//select[@id='left']//option[@value='LingvoDictionaries']")).Click();
                Driver.FindElement(By.XPath(".//table[@name='Features']//input[@name='toRight']")).Click();
            }

            if (needAddAllDictionaries)
            {
                // Добавить словари
                Driver.FindElement(By.XPath(".//table[contains(@name,'dictionariesPackages')]//input[@name='allToRight']")).Click();
            }

            // Добавить дату
            string deadLineDate = DateTime.Now.AddDays(10).Date.ToString();
            deadLineDate = deadLineDate.Substring(0, deadLineDate.IndexOf(" "));
            Driver.FindElement(By.Id("DictionariesExpirationDate")).Clear();
            Driver.FindElement(By.Id("DictionariesExpirationDate")).SendKeys(deadLineDate);
        }

        /// <summary>
        /// Добавить пользователя в аккаунт
        /// </summary>
        protected void AddUserToAccount()
        {
            // Перейти в управление пользователями
            Driver.FindElement(By.XPath(".//a[contains(@href,'/EnterpriseAccounts/ManageUsers')]")).Click();
            // Дождаться открытия формы
            Wait.Until((d) => d.FindElement(By.Id("searchText")));
            // Ввести логин пользователя
            Driver.FindElement(By.Id("searchText")).SendKeys(Login);
            // Найти
            Driver.FindElement(By.Id("findUser")).Click();
            // Дождаться появления пользователя
            Wait.Until((d) => d.FindElement(By.Id("addUsersBtn")));
            // Добавить
            Driver.FindElement(By.Id("addUsersBtn")).Click();
        }

        /// <summary>
        /// Перейти на страницу корпоративных аккаунтов
        /// </summary>
        protected void SwitchEnterpriseAccountList()
        {
            // Зайти в аккаунты
            WaitAndClickElement(".//a[@href='/EnterpriseAccounts']");
        }

        /// <summary>
        /// Создать аккаунт со словарями
        /// </summary>
        protected void AddAccountWithDictionaries()
        {
            // Открыть форму создания аккаунта
            OpenCreateAccountForm();

            // Заполнить форму аккаунта
            string accountName = FillGeneralAccountFields();
            // Добавить словари
            AddDictionaryAccount();

            // Сохранить
            Driver.FindElement(By.XPath(".//input[@type='submit']")).Click();

            // Дождаться успеха
            Wait.Until((d) => d.FindElement(By.XPath(".//p[contains(@class,'b-success-message')]")).Displayed);

            // Добавить пользователя в аккаунт
            AddUserToAccount();

            // Перейти в CAT
            Driver.Navigate().GoToUrl(Url);
            // Зайти пользователем
            Authorization(accountName);

            // Проверка, что вкладка LingvoDictionaries видна
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//a[contains(@href,'/LingvoDictionaries')]")),
                "Ошибка: не показывается вкладка Lingvo Dictionaries");

            // Перейти на вкладку со словарями
            Driver.FindElement(By.XPath(".//a[contains(@href,'/LingvoDictionaries')]")).Click();

            IList<IWebElement> dictList = Driver.FindElements(By.XPath(
                ".//div[contains(@class,'js-dictionaries-search-result')]//div[contains(@class,'l-dctnrs__dict')]"));
            Assert.IsTrue(dictList.Count > 0, "Ошибка: список словарей пуст");

            // Перейти на вкладку Search
            SwitchSearchTab();
        }
    }
}
