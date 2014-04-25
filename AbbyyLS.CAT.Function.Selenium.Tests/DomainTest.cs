﻿using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class DomainTest : BaseTest
    {
        public DomainTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
            // Авторизация
            Authorization();

            // Перейти на вкладку Проект
            SwitchDomainTab();
        }

        /// <summary>
        /// Метод тестирования создания Проекта
        /// </summary>
        [Test]
        public void CreateDomainTest()
        {
            // Создать проект с уникальным именем
            string domainName = GetDomainUniqueName();
            CreateDomain(domainName);

            // Проверить, что проект сохранился
            Assert.IsTrue(GetIsDomainExist(domainName), "Ошибка: проект не создался");
        }

        /// <summary>
        /// Метод тестирования создания Проекта с существующим именем
        /// </summary>
        [Test]
        public void CreateDomainExistingNameTest()
        {
            // Создать проект с уникальным именем
            string domainName = GetDomainUniqueName();
            CreateDomain(domainName);
            // Создать проект с таким же именем
            CreateDomain(domainName, false);

            // Проверить, что появилась ошибка существующего имени - Assert внутри
            AssertExistingDomainNameError();
        }

        /// <summary>
        /// Метод тестирования создания Проекта с пустым именем
        /// </summary>
        [Test]
        public void CreateDomainEmptyNameTest()
        {
            // Создать проект с пустым именем
            CreateDomain("", false);

            // Проверить, остался ли проект в режиме редактирования - Assert внутри
            AssertEditingModeDomain();
        }

        /// <summary>
        /// Метод тестирования создания Проекта с пробельным именем
        /// </summary>
        [Test]
        public void CreateDomainSpaceNameTest()
        {
            // Создать проект с пробельным именем
            CreateDomain("  ", false);

            // Проверить, остался ли проект в режиме редактирования - Assert внутри
            AssertEditingModeDomain();
        }

        /// <summary>
        /// Метод тестирования создания Проекта, проверка, что проект появился в списке при создании ТМ
        /// </summary>
        [Test]
        public void CreateDomainCheckCreateTMTest()
        {
            // Создать проект
            string domainName = GetDomainUniqueName();
            CreateDomain(domainName);

            // Проверить, что проект есть в списке при создании ТМ
            Assert.IsTrue(GetIsDomainExistCreateTM(domainName),
                "Ошибка: проекта нет в списке при создании ТМ");
        }

        /// <summary>
        /// Метод тестирования создания Проекта, проверка, что проект появился в списке при создании глоссария
        /// </summary>
        [Test]
        public void CreateDomainCheckCreateGlossaryTest()
        {
            // Создать проект
            string domainName = GetDomainUniqueName();
            CreateDomain(domainName);

            // Проверить проект в списке при создании глоссария
            Assert.IsTrue(GetIsDomainExistCreateGlossaryTest(domainName),
                "Ошибка: проекта нет в списке");
        }

        /// <summary>
        /// Метод тестирования создания Проекта, проверка, что проект появился в списке при создании термина глоссария
        /// </summary>
        [Test]
        public void CreateDomainCheckCreateGlossaryItemTest()
        {
            // Создать проект
            string domainName = GetDomainUniqueName();
            CreateDomain(domainName);

            // Проверить, что проект есть в списке при создании термина глоссария
            Assert.IsTrue(GetIsDomainExistCreateGlossaryItemTest(domainName),
                "Ошибка: проекта нет в списке");
        }

        /// <summary>
        /// Метод тестирования изменения имени Проекта
        /// </summary>
        [Test]
        public void ChangeDomainNameTest()
        {
            // Создать проект с уникальным именем
            string domainName = GetDomainUniqueName();
            CreateDomain(domainName);

            // Новое имя проекта
            string newDomainName = GetDomainUniqueName();
            // Изменить имя проекта
            SetDomainNewName(domainName, newDomainName);

            // Проверить, что проекта со старым названием нет
            Assert.IsTrue(!GetIsDomainExist(domainName), "Ошибка: старый проект не удалился");
            // Проверить, что есть проект с новым названием
            Assert.IsTrue(GetIsDomainExist(newDomainName), "Ошибка: новый проект не сохранился");
        }

        /// <summary>
        /// Метод тестирования изменения имени Проекта на пустое
        /// </summary>
        [Test]
        public void ChangeDomainEmptyNameTest()
        {
            // Создать проект с уникальным именем
            string domainName = GetDomainUniqueName();
            CreateDomain(domainName);

            // Изменить имя проекта
            SetDomainNewName(domainName, "", false);

            // Проверить, остался ли проект в режиме редактирования - Assert внутри
            AssertEditingModeDomain();
        }

        /// <summary>
        /// Метод тестирования изменения имени Проекта на пробельное
        /// </summary>
        [Test]
        public void ChangeDomainSpaceNameTest()
        {
            // Создать проект с уникальным именем
            string domainName = GetDomainUniqueName();
            CreateDomain(domainName);

            // Изменить имя проекта
            SetDomainNewName(domainName, "  ", false);

            // Проверить, остался ли проект в режиме редактирования - Assert внутри
            AssertEditingModeDomain();
        }

        /// <summary>
        /// Метод тестирования изменения имени Проекта на существующее
        /// </summary>
        [Test]
        public void ChangeDomainExistingNameTest()
        {
            // Создать проект с уникальным именем
            string domainName = GetDomainUniqueName();
            CreateDomain(domainName);
            // Создать другой проект с уникальным именем
            string secondDomainName = GetDomainUniqueName();
            CreateDomain(secondDomainName);

            // Изменить имя проекта
            SetDomainNewName(secondDomainName, domainName, false);

            // Проверить, появилась ли ошибка существующего имени - Assert внутри
            AssertExistingDomainNameError();
        }

        /// <summary>
        /// Метод тестирования удаления Проекта
        /// </summary>
        [Test]
        public void DeleteDomainTest()
        {
            // Создать проект с уникальным именем
            string domainName = GetDomainUniqueName();
            CreateDomain(domainName);

            // Удалить проект
            ClickDeleteDomain(domainName);
            // Проверить, что проект удалился
            Assert.IsTrue(!GetIsDomainExist(domainName), "Ошибка: проект не удалился");
        }

        /// <summary>
        /// Метод тестирования удаления Проекта, проверка списка при создании ТМ
        /// </summary>
        [Test]
        public void DeleteDomainCheckCreateTM()
        {
            // Создать проект с уникальным именем
            string domainName = GetDomainUniqueName();
            CreateDomain(domainName);

            // Удалить проект
            ClickDeleteDomain(domainName);
            // Проверить, что проекта нет в списке при создании TM
            Assert.IsTrue(!GetIsDomainExistCreateTM(domainName),
                "Ошибка: проект остался в списке");
        }

        /// <summary>
        /// Метод тестирования удаления Проекта, проверка списка при создании глоссария
        /// </summary>
        [Test]
        public void DeleteDomainCheckCreateGlossaryTest()
        {
            // Создать проект с уникальным именем
            string domainName = GetDomainUniqueName();
            CreateDomain(domainName);

            // Удалить проект
            ClickDeleteDomain(domainName);
            // Проверить, что проекта нет в списке при создании глоссария
            Assert.IsTrue(!GetIsDomainExistCreateGlossaryTest(domainName),
                "Ошибка: проект остался в списке");
        }

        /// <summary>
        /// Метод тестирования удаления Проекта, проверка списка при создании термина глоссария
        /// </summary>
        [Test]
        public void DeleteDomainCheckCreateGlossaryItemTest()
        {
            // Создать проект с уникальным именем
            string domainName = GetDomainUniqueName();
            CreateDomain(domainName);

            // Удалить проект
            ClickDeleteDomain(domainName);
            // Проверить, что проекта нет в списке при создании термина глоссария
            Assert.IsTrue(!GetIsDomainExistCreateGlossaryItemTest(domainName),
                "Ошибка: проект остался в списке");
        }
        
        private void SetDomainNewName(string domainName, string newDomainName, bool shouldSaveOk = true)
        {
            // Нажать на строку
            string domainXPath = GetDomainRowXPath(domainName);
            Driver.FindElement(By.XPath(domainXPath)).Click();
            // Нажать на Изменить
            string editBtnXPath = domainXPath + "//a[contains(@class,'domain js-edit-domain')]";
            Driver.FindElement(By.XPath(editBtnXPath)).Click();

            // Ввести новое имя проекта
            string domainNameXPath = domainXPath + "//div[contains(@class,'js-edit-mode')]//input[contains(@class,'js-domain-name-input')]";
            Driver.FindElement(By.XPath(domainNameXPath)).Clear();
            Driver.FindElement(By.XPath(domainNameXPath)).SendKeys(newDomainName);
            domainXPath += "//div[contains(@class,'l-corpr__domainbox js-edit-mode')]//a[contains(@class,'save js-save-domain')]";
            // Сохранить
            Driver.FindElement(By.XPath(domainXPath)).Click();
            if (shouldSaveOk)
            {
                WaitUntilDisappearElement(domainXPath);
            }
            else
            {
                Thread.Sleep(1000);
            }
        }

        private string GetDomainRowXPath(string domainName)
        {
            string xPath = "";
            // Получить список всех проектов
            IList<IWebElement> DomainsList = Driver.FindElements(By.XPath(
                ".//table[contains(@class,'js-domains js-sortable-table')]//tr[contains(@class,'l-corpr__trhover js-row')]"));
            for (int i = 0; i < DomainsList.Count; ++i)
            {
                // Проверить имя проекта
                if (DomainsList[i].Text == domainName)
                {
                    xPath = ".//tr[contains(@class,'l-corpr__trhover js-row')][" + (i + 1) + "]";
                    break;
                }
            }
            return xPath;
        }

        private string GetDomainUniqueName()
        {
            return "TestDomain" + DateTime.UtcNow.Ticks.ToString();
        }

        private void AssertExistingDomainNameError()
        {
            // Проверить, появилась ли ошибка существующего имени
            string rowXPath = ".//table[contains(@class,'js-domains js-sortable-table')]//tr[contains(@class,'js-row js-error-row')]//div[contains(@class,'js-error-text g-hidden')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(rowXPath)).Displayed,
                "Ошибка: не появилась ошибка существующего имени");
        }

        private void ClickDeleteDomain(string domainName)
        {
            // Нажать на строку
            string rowXPath = GetDomainRowXPath(domainName);
            Driver.FindElement(By.XPath(rowXPath)).Click();
            // Получить xPath кнопки Удалить для проекта
            string deleteBtnXPath = rowXPath + "//a[contains(@class,'domain js-delete-domain')]";
            // Нажать Удалить
            Driver.FindElement(By.XPath(deleteBtnXPath)).Click();
            WaitUntilDisappearElement(deleteBtnXPath);
        }

        private bool GetIsDomainExistCreateTM(string domainName)
        {
            // Перейти на вкладку ТМ
            SwitchTMTab();

            // Нажать кнопку Создать TM
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'l-corpr__addbtnbox')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Click();
            // ждем загрузку формы
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]")));

            // Нажать на открытие списка проектов
            WaitAndClickElement(".//div[contains(@class,'js-domains-multiselect')]");
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'ui-multiselect-menu')][2]")).Displayed);

            // Получить список проектов
            IList<IWebElement> DomainList = Driver.FindElements(By.XPath(
                ".//div[contains(@class,'ui-multiselect-menu')][2]//ul[contains(@class,'ui-multiselect-checkboxes')]//li//label//span[contains(@class,'ui-multiselect-item-text')]"));
            bool isDomainExist = false;
            foreach (IWebElement el in DomainList)
            {
                if (el.Text == domainName)
                {
                    // Если проект в списке
                    isDomainExist = true;
                    break;
                }
            }

            return isDomainExist;
        }

        private bool GetIsDomainExistCreateGlossaryTest(string domainName)
        {
            // Перейти на вкладку Глоссарии
            SwitchGlossaryTab();

            // Нажать кнопку Create a glossary
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-create-glossary-button')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Click();
            // ждем загрузку формы
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]")));

            string xPathDomainField = ".//div[contains(@class,'js-popup-edit-glossary')][2]//input[contains(@name,'Domain')]";
            // Нажать, чтобы появился список проектов
            WaitAndClickElement(xPathDomainField + "/..//div[contains(@class,'ui-multiselect')]");
            // Получить список проектов в списке
            IList<IWebElement> DomainList = Driver.FindElements(By.XPath(
                ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')]"));
            bool isDomainExist = false;
            foreach (IWebElement el in DomainList)
            {
                if (el.Text == domainName)
                {
                    // Если проект в списке
                    isDomainExist = true;
                    break;
                }
            }

            return isDomainExist;
        }

        private bool GetIsDomainExistCreateGlossaryItemTest(string domainName)
        {
            // Перейти на вкладку Глоссарии
            SwitchGlossaryTab();
            
            // Создать глоссарий
            CreateGlossaryByName("Test Glossary Check Domain" + DateTime.Now);

            // Открыть Редактирование глоссария
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-edit-submenu')]"))).Click();
            // Выбрать Редактирование структуры
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-edit-structure-btn')]"))).Click();
            // Дождаться появления формы
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-structure')]")));

            // Получить xPath строки с нужным полем
            string rowXPath = ".//table[contains(@class, 'js-predefined-attrs-table concept')]//tr[contains(@class, 'js-attr-row')][contains(@data-attr-key,'Domain')]/td[1]";
            // Нажать на поле
            WaitAndClickElement(rowXPath);
            // Добавить
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'js-add-tbx-attribute')]"))).Click();

            // Сохранить
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]")).Click();
            // Дождаться закрытия формы
            WaitUntilDisappearElement(".//div[contains(@class,'js-popup-edit-structure')]");

            // Нажать New item
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-add-concept')]"))).Click();

            // Нажать на поле появилось
            string fieldXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__edit js-edit')]//select[@name='Domain']/..//span[contains(@class,'js-dropdown')]";
            WaitAndClickElement(fieldXPath);

            // Получить список проектов в списке
            IList<IWebElement> DomainList = Driver.FindElements(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item')]"));
            bool isDomainExist = false;
            foreach (IWebElement el in DomainList)
            {
                if (el.Text == domainName)
                {
                    // Если проект в списке
                    isDomainExist = true;
                    break;
                }
            }

            return isDomainExist;
        }

        private void AssertEditingModeDomain()
        {
            // Проверить, что проект не сохранился, а остался в режиме редактирования
            string saveBtnXPath =
                ".//tr[@class='l-corpr__trhover js-row']//td[contains(@class,'js-cell')]//div[contains(@class,'js-edit-mode')]//a[contains(@class,'domain save js-save-domain')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(saveBtnXPath)).Displayed,
                "Ошибка: не остался в режиме редактирования");
        }
    }
}