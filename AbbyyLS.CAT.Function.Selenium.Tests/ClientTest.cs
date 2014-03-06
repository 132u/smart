﻿using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class ClientTest : BaseTest
    {
        public ClientTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
            // Авторизация
            Authorization();

            // Перейти на вкладку Клиенты
            SwitchClientTab();
        }

        /// <summary>
        /// Метод тестирования создания Клиента
        /// </summary>
        [Test]
        public void CreateClientTest()
        {
            // Создать клиента с уникальным именем
            string clientName = GetClientUniqueName();
            CreateClient(clientName);

            // Проверить, что клиент сохранился
            Assert.IsTrue(GetIsClientExist(clientName), "Ошибка: клиент не сохранился");
        }

        /// <summary>
        /// Метод тестирования создания Клиента с существующим именем
        /// </summary>
        [Test]
        public void CreateClientExistingNameTest()
        {
            // Создать клиента с уникальным именем
            string clientName = GetClientUniqueName();
            CreateClient(clientName);
            // Создать клиента с таким же именем
            CreateClient(clientName);
            Thread.Sleep(2000);

            // Проверить, появилась ли ошибка существующего имени - Assert внутри
            AssertExistingClientNameError();
        }

        /// <summary>
        /// Метод тестирования создания Клиента с пустым именем
        /// </summary>
        [Test]
        public void CreateClientEmptyNameTest()
        {
            // Создать клиента с пустым именем
            CreateClient("");

            // Проверить, что клиент не сохранился, в остался в режиме редактирования - Asssert внутри
            AssertEditingModeClient();
        }

        /// <summary>
        /// Метод тестирования создания Клиента с пробельным именем
        /// </summary>
        [Test]
        public void CreateClientSpaceNameTest()
        {
            // Создать клиента с пробельным именем
            CreateClient("  ");

            // Проверить, что клиент не сохранился, а остался в режиме редактирования - Asssert внутри
            AssertEditingModeClient();
        }

        /// <summary>
        /// Метод тестирования создания Клиента, проверка, что клиент появился в списке при создании ТМ
        /// </summary>
        [Test]
        public void CreateClientCheckCreateTMTest()
        {
            // Создать клиента с уникальным именем
            string clientName = GetClientUniqueName();
            CreateClient(clientName);

            // Проверить, что клиент есть в списке при создании ТМ
            Assert.IsTrue(GetIsClientExistCreateTM(clientName),
                "Ошибка: клиента нет в списке при создании ТМ");
        }

        /// <summary>
        /// Метод тестирования создания Клиента, проверка, что клиент появился в списке при создании глоссария
        /// </summary>
        [Test]
        public void CreateClientCheckCreateGlossaryTest()
        {
            // Создать клиента
            string clientName = GetClientUniqueName();
            CreateClient(clientName);

            // Проверить, что клиент есть в списке при создании глоссария
            Assert.IsTrue(GetIsClientExistCreateGlossaryTest(clientName),
                "Ошибка: клиента нет в списке при создании глоссария");
        }

        /// <summary>
        /// Метод тестирования изменения имени Клиента
        /// </summary>
        [Test]
        public void ChangeClientNameTest()
        {
            // Создать клиента с уникальным именем
            string clientName = GetClientUniqueName();
            CreateClient(clientName);
            Thread.Sleep(1000);

            // Новое имя клиента
            string newClientName = GetClientUniqueName();
            // Изменить имя клиента
            SetClientNewName(clientName, newClientName);

            // Проверить, что клиента со старым названием нет
            Assert.IsTrue(!GetIsClientExist(clientName), "Ошибка: старый клиент не удалился");
            // Проверить, что есть клиент с новым названием
            Assert.IsTrue(GetIsClientExist(newClientName), "Ошибка: новый клиент не сохранился");
        }

        /// <summary>
        /// Метод тестирования изменения имени Клиента на пустое
        /// </summary>
        [Test]
        public void ChangeClientEmptyNameTest()
        {
            // Создать клиента с уникальным именем
            string clientName = GetClientUniqueName();
            CreateClient(clientName);
            Thread.Sleep(1000);
            // Получить xPath кнопки Сохранить для созданного клиента
            string saveBtnXPath = GetClientRowXPath(clientName) + "//div[contains(@class,'js-edit-mode')]//a[contains(@class,'save js-save-client')]";

            // Изменить имя клиента
            SetClientNewName(clientName, "");
            Thread.Sleep(10000);
            // Проверить, что клиент не сохранился, а остался в режиме редактирования - Asssert внутри
            AssertEditingModeClient();
        }

        /// <summary>
        /// Метод тестирования изменения имени Клиента на пробельное
        /// </summary>
        [Test]
        public void ChangeClientSpaceNameTest()
        {
            // Создать клиента с уникальным именем
            string clientName = GetClientUniqueName();
            CreateClient(clientName);
            Thread.Sleep(1000);
            // Получить xPath кнопки Сохранить для созданного клиента
            string saveBtnXPath = GetClientRowXPath(clientName) + "//div[contains(@class,'js-edit-mode')]//a[contains(@class,'save js-save-client')]";

            // Изменить имя клиента
            SetClientNewName(clientName, "  ");
            Thread.Sleep(10000);
            // Проверить, что клиент не сохранился, а остался в режиме редактирования - Asssert внутри
            AssertEditingModeClient();
        }

        /// <summary>
        /// Метод тестирования изменения имени Клиента на существующее
        /// </summary>
        [Test]
        public void ChangeClientExistingNameTest()
        {
            // Создать клиента с уникальным именем
            string clientName = GetClientUniqueName();
            CreateClient(clientName);
            Thread.Sleep(1000);
            // Создать другого клиента с уникальным именем
            string secondClientName = GetClientUniqueName();
            CreateClient(secondClientName);
            Thread.Sleep(1000);

            // Изменить имя клиента
            SetClientNewName(secondClientName, clientName);

            // Проверить, появилась ли ошибка существующего имени - Assert внутри
            AssertExistingClientNameError();
        }

        /// <summary>
        /// Метод тестирования удаления Клиента
        /// </summary>
        [Test]
        public void DeleteClientTest()
        {
            // Создать клиента с уникальным именем
            string clientName = GetClientUniqueName();
            CreateClient(clientName);
            Thread.Sleep(1000);

            // Удалить клиента
            ClickDeleteClient(clientName);
            Thread.Sleep(1000);
            // Проверить, что клиент удалился
            Assert.IsTrue(!GetIsClientExist(clientName), "Ошибка: клиент не удалился");
        }

        /// <summary>
        /// Метод тестирования удаления Клиента, проверка списка при создании ТМ
        /// </summary>
        [Test]
        public void DeleteClientCheckCreateTM()
        {
            // Создать клиента с уникальным именем
            string clientName = GetClientUniqueName();
            CreateClient(clientName);
            Thread.Sleep(1000);

            // Удалить клиента
            ClickDeleteClient(clientName);
            Thread.Sleep(1000);
            // Проверить, что клиента нет в списке при создании TM
            Assert.IsTrue(!GetIsClientExistCreateTM(clientName),
                "Ошибка: клиент остался в списке при создании ТМ");
        }

        /// <summary>
        /// Метод тестирования удаления Клиента, проверка списка при создании глоссария
        /// </summary>
        [Test]
        public void DeleteClientCheckCreateGlossaryTest()
        {
            // Создать клиента с уникальным именем
            string clientName = GetClientUniqueName();
            CreateClient(clientName);
            Thread.Sleep(1000);

            // Удалить клиента
            ClickDeleteClient(clientName);
            Thread.Sleep(1000);
            // Проверить, что клиента нет в списке при создании глоссария
            Assert.IsTrue(!GetIsClientExistCreateGlossaryTest(clientName),
                "Ошибка: клиент остался в списке");
        }
        
        private void SwitchClientTab()
        {
            // Перейти на страницу Клиенты
            Driver.FindElement(By.XPath(
                ".//ul[@class='g-corprmenu__list']//a[contains(@href,'/Clients')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'g-btn g-redbtn js-add-client')]//a[contains(@class,'g-btn__text g-redbtn__text')]"))
                .Displayed);
        }

        private string GetClientUniqueName()
        {
            return "TestClient" + DateTime.UtcNow.Ticks.ToString();
        }

        private void CreateClient(string clientName)
        {
            // Нажать "Новый клиент"
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'g-btn g-redbtn js-add-client')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Click();
            string rowXPath = ".//table[contains(@class,'js-clients')]//tr[contains(@class,'l-corpr__trhover js-row')]";
            // Получить все строки в таблице с клиентами
            IList<IWebElement> rowsList = Driver.FindElements(By.XPath(rowXPath));
            string tdXPath = "";
            for (int i = 0; i < rowsList.Count; ++i)
            {
                // Найти нескрытую строку для ввода имени нового клиента
                if (!rowsList[i].GetAttribute("class").Contains("g-hidden"))
                {
                    tdXPath = rowXPath + "[" + (i + 1) + "]//td[contains(@class,'js-cell')]";
                    if (Driver.FindElement(By.XPath(tdXPath)).GetAttribute("class").Contains("clientNew"))
                    {
                        string clientNameXPath = tdXPath +
                            "//div[contains(@class,'js-edit-mode')]//input[contains(@class,'js-client-name-input')]";
                        // Ввести имя клиента
                        Driver.FindElement(By.XPath(clientNameXPath)).SendKeys(clientName);
                        break;
                    }
                }
            }

            // Расширить окно, чтобы кнопка была видна, иначе она недоступна для Selenium
            Driver.Manage().Window.Maximize();
            // Сохранить клиента
            Driver.FindElement(By.XPath(tdXPath +
                "//div[contains(@class,'l-corpr__clientbox js-edit-mode')]//a[contains(@class,'save js-save-client')]")).Click();
            Thread.Sleep(1000);
        }

        private bool GetIsClientExist(string clientName)
        {
            // Получить список всех клиентов
            IList<IWebElement> clientsList = Driver.FindElements(By.XPath(
                ".//table[contains(@class,'js-clients js-sortable-table')]//tr[contains(@class,'l-corpr__trhover js-row')]"));
            bool bClientExist = false;
            foreach (IWebElement el in clientsList)
            {
                // Проверить имя клиента
                if (el.Text == clientName)
                {
                    bClientExist = true;
                    break;
                }
            }

            return bClientExist;
        }

        private void AssertExistingClientNameError()
        {
            // Проверить, появилась ли ошибка существующего имени
            string rowXPath = ".//table[contains(@class,'js-clients js-sortable-table')]//tr[contains(@class,'js-row js-error-row')]//div[contains(@class,'js-error-text g-hidden')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(rowXPath)).Displayed,
                "Ошибка: не появилась ошибка существующего имени");
        }

        private void AssertEditingModeClient()
        {
            // Проверить, что клиент не сохранился, а остался в режиме редактирования
            string saveBtnXPath =
                ".//tr[@class='l-corpr__trhover js-row']//td[contains(@class,'js-cell')]//div[contains(@class,'js-edit-mode')]//a[contains(@class,'client save js-save-client')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(saveBtnXPath)).Displayed,
                "Ошибка: не остался в режиме редактирования");
        }

        private bool GetIsClientExistCreateTM(string clientName)
        {
            // Перейти на вкладку ТМ
            SwitchTMTab();

            // Нажать кнопку Создать TM
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'l-corpr__addbtnbox')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Click();
            // ждем загрузку формы
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]")));

            // Нажать на открытие списка клиентов
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-client-select')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'js-client-select g-drpdwn__list')]")).Displayed);

            // Получить список клиентов
            IList<IWebElement> clientList = Driver.FindElements(By.XPath(
                ".//span[contains(@class,'js-client-select g-drpdwn__list')]//span[contains(@class,'js-dropdown__item')]"));
            bool bClientExist = false;
            foreach (IWebElement el in clientList)
            {
                if (el.GetAttribute("title") == clientName)
                {
                    // Если клиент в списке
                    bClientExist = true;
                    break;
                }
            }

            return bClientExist;
        }

        private bool GetIsClientExistCreateGlossaryTest(string clientName)
        {
            // Перейти на вкладку Глоссарии
            SwitchGlossaryTab();

            // Нажать кнопку Create a glossary
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-create-glossary-button')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Click();
            // ждем загрузку формы
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]")));

            // Нажать, чтобы появился список клиентов
            string xPathClientField = ".//div[contains(@class,'js-popup-edit-glossary')][2]//select[contains(@name,'Client')]";
            Driver.FindElement(By.XPath(
                xPathClientField + "/..//span[contains(@class,'js-dropdown')]")).Click();
            Thread.Sleep(1000);
            // Получить список клиентов
            IList<IWebElement> clientList = Driver.FindElements(By.XPath(
                ".//span[contains(@class,'js-dropdown__list  g-drpdwn__list')]//span[contains(@class,'js-dropdown__item')]"));
            bool bClientExist = false;
            foreach (IWebElement el in clientList)
            {
                if (el.GetAttribute("title") == clientName)
                {
                    // Если клиент в списке
                    bClientExist = true;
                    break;
                }
            }

            return bClientExist;
        }

        private void SetClientNewName(string clientName, string newClientName)
        {
            // Нажать Изменить
            string clientXPath = GetClientRowXPath(clientName);
            string editBtnXPath = clientXPath + "//a[contains(@class,'client js-edit-client')]";
            Driver.FindElement(By.XPath(editBtnXPath)).Click();

            // Ввести новое имя клиента
            string clientNameXPath = clientXPath + "//div[contains(@class,'js-edit-mode')]//input[contains(@class,'js-client-name-input')]";
            Driver.FindElement(By.XPath(clientNameXPath)).Clear();
            Driver.FindElement(By.XPath(clientNameXPath)).SendKeys(newClientName);
            // Сохранить
            Driver.FindElement(By.XPath(clientXPath +
                "//div[contains(@class,'l-corpr__clientbox js-edit-mode')]//a[contains(@class,'save js-save-client')]")).Click();
            Thread.Sleep(1000);
        }

        private string GetClientRowXPath(string clientName)
        {
            string xPath = "";
            // Получить список всех клиентов
            IList<IWebElement> clientsList = Driver.FindElements(By.XPath(
                ".//table[contains(@class,'js-clients js-sortable-table')]//tr[contains(@class,'l-corpr__trhover js-row')]"));
            for (int i = 0; i < clientsList.Count; ++i)
            {
                // Проверить имя клиента
                if (clientsList[i].Text == clientName)
                {
                    xPath = ".//tr[contains(@class,'l-corpr__trhover js-row')][" + (i + 1) + "]";
                    break;
                }
            }
            return xPath;
        }

        private void ClickDeleteClient(string clientName)
        {
            // Получить xPath кнопки Удалить
            string deleteBtnXPath = GetClientRowXPath(clientName) + "//a[contains(@class,'client js-delete-client')]";
            Driver.FindElement(By.XPath(deleteBtnXPath)).Click();
        }
    }
}