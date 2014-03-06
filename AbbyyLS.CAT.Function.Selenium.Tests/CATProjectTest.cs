﻿using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class CATProject : BaseTest
    {
        public CATProject(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
            // Авторизация
            Authorization();

            // Перейти на вкладку Проект
            SwitchProjectTab();
        }

        /// <summary>
        /// Метод тестирования создания Проекта
        /// </summary>
        [Test]
        public void CreateProjectTest()
        {
            // Создать проект с уникальным именем
            string projectName = GetProjectUniqueName();
            CreateCATProject(projectName);

            // Проверить, что проект сохранился
            Assert.IsTrue(GetIsProjectExist(projectName), "Ошибка: проект не создался");
        }

        /// <summary>
        /// Метод тестирования создания Проекта с существующим именем
        /// </summary>
        [Test]
        public void CreateProjectExistingNameTest()
        {
            // Создать проект с уникальным именем
            string projectName = GetProjectUniqueName();
            CreateCATProject(projectName);
            // Создать проект с таким же именем
            CreateCATProject(projectName);
            Thread.Sleep(2000);

            // Проверить, что появилась ошибка существующего имени - Assert внутри
            AssertExistingProjectNameError();
        }

        /// <summary>
        /// Метод тестирования создания Проекта с пустым именем
        /// </summary>
        [Test]
        public void CreateProjectEmptyNameTest()
        {
            // Создать проект с пустым именем
            CreateCATProject("");

            // Проверить, остался ли проект в режиме редактирования - Assert внутри
            AssertEditingModeProject();
        }

        /// <summary>
        /// Метод тестирования создания Проекта с пробельным именем
        /// </summary>
        [Test]
        public void CreateProjectSpaceNameTest()
        {
            // Создать проект с пробельным именем
            CreateCATProject("  ");

            // Проверить, остался ли проект в режиме редактирования - Assert внутри
            AssertEditingModeProject();
        }

        /// <summary>
        /// Метод тестирования создания Проекта, проверка, что проект появился в списке при создании ТМ
        /// </summary>
        [Test]
        public void CreateProjectCheckCreateTMTest()
        {
            // Создать проект
            string projectName = GetProjectUniqueName();
            CreateCATProject(projectName);

            // Проверить, что проект есть в списке при создании ТМ
            Assert.IsTrue(GetIsProjectExistCreateTM(projectName),
                "Ошибка: проекта нет в списке при создании ТМ");
        }

        /// <summary>
        /// Метод тестирования создания Проекта, проверка, что проект появился в списке при создании глоссария
        /// </summary>
        [Test]
        public void CreateProjectCheckCreateGlossaryTest()
        {
            // Создать проект
            string projectName = GetProjectUniqueName();
            CreateCATProject(projectName);

            // Проверить проект в списке при создании глоссария
            Assert.IsTrue(GetIsProjectExistCreateGlossaryTest(projectName),
                "Ошибка: проекта нет в списке");
        }

        /// <summary>
        /// Метод тестирования создания Проекта, проверка, что проект появился в списке при создании термина глоссария
        /// </summary>
        [Test]
        public void CreateProjectCheckCreateGlossaryItemTest()
        {
            // Создать проект
            string projectName = GetProjectUniqueName();
            CreateCATProject(projectName);

            // Проверить, что проект есть в списке при создании термина глоссария
            Assert.IsTrue(GetIsProjectExistCreateGlossaryItemTest(projectName),
                "Ошибка: проекта нет в списке");
        }

        /// <summary>
        /// Метод тестирования изменения имени Проекта
        /// </summary>
        [Test]
        public void ChangeProjectNameTest()
        {
            // Создать проект с уникальным именем
            string projectName = GetProjectUniqueName();
            CreateCATProject(projectName);
            Thread.Sleep(1000);

            // Новое имя проекта
            string newProjectName = GetProjectUniqueName();
            // Изменить имя проекта
            SetProjectNewName(projectName, newProjectName);

            // Проверить, что проекта со старым названием нет
            Assert.IsTrue(!GetIsProjectExist(projectName), "Ошибка: старый проект не удалился");
            // Проверить, что есть проект с новым названием
            Assert.IsTrue(GetIsProjectExist(newProjectName), "Ошибка: новый проект не сохранился");
        }

        /// <summary>
        /// Метод тестирования изменения имени Проекта на пустое
        /// </summary>
        [Test]
        public void ChangeProjectEmptyNameTest()
        {
            // Создать проект с уникальным именем
            string projectName = GetProjectUniqueName();
            CreateCATProject(projectName);
            Thread.Sleep(1000);

            // Изменить имя проекта
            SetProjectNewName(projectName, "");

            // Проверить, остался ли проект в режиме редактирования - Assert внутри
            AssertEditingModeProject();
        }

        /// <summary>
        /// Метод тестирования изменения имени Проекта на пробельное
        /// </summary>
        [Test]
        public void ChangeProjectSpaceNameTest()
        {
            // Создать проект с уникальным именем
            string projectName = GetProjectUniqueName();
            CreateCATProject(projectName);
            Thread.Sleep(1000);

            // Изменить имя проекта
            SetProjectNewName(projectName, "  ");

            // Проверить, остался ли проект в режиме редактирования - Assert внутри
            AssertEditingModeProject();
        }

        /// <summary>
        /// Метод тестирования изменения имени Проекта на существующее
        /// </summary>
        [Test]
        public void ChangeProjectExistingNameTest()
        {
            // Создать проект с уникальным именем
            string projectName = GetProjectUniqueName();
            CreateCATProject(projectName);
            Thread.Sleep(1000);
            // Создать другой проект с уникальным именем
            string secondProjectName = GetProjectUniqueName();
            CreateCATProject(secondProjectName);
            Thread.Sleep(1000);

            // Изменить имя проекта
            SetProjectNewName(secondProjectName, projectName);

            // Проверить, появилась ли ошибка существующего имени - Assert внутри
            AssertExistingProjectNameError();
        }

        /// <summary>
        /// Метод тестирования удаления Проекта
        /// </summary>
        [Test]
        public void DeleteProjectTest()
        {
            // Создать проект с уникальным именем
            string projectName = GetProjectUniqueName();
            CreateCATProject(projectName);
            Thread.Sleep(1000);

            // Удалить проект
            ClickDeleteProject(projectName);

            Thread.Sleep(1000);
            // Проверить, что проект удалился
            Assert.IsTrue(!GetIsProjectExist(projectName), "Ошибка: проект не удалился");
        }

        /// <summary>
        /// Метод тестирования удаления Проекта, проверка списка при создании ТМ
        /// </summary>
        [Test]
        public void DeleteProjectCheckCreateTM()
        {
            // Создать проект с уникальным именем
            string projectName = GetProjectUniqueName();
            CreateCATProject(projectName);
            Thread.Sleep(1000);

            // Удалить проект
            ClickDeleteProject(projectName);

            Thread.Sleep(1000);
            // Проверить, что проекта нет в списке при создании TM
            Assert.IsTrue(!GetIsProjectExistCreateTM(projectName),
                "Ошибка: проект остался в списке");
        }

        /// <summary>
        /// Метод тестирования удаления Проекта, проверка списка при создании глоссария
        /// </summary>
        [Test]
        public void DeleteProjectCheckCreateGlossaryTest()
        {
            // Создать проект с уникальным именем
            string projectName = GetProjectUniqueName();
            CreateCATProject(projectName);
            Thread.Sleep(1000);

            // Удалить проект
            ClickDeleteProject(projectName);

            Thread.Sleep(1000);
            // Проверить, что проекта нет в списке при создании глоссария
            Assert.IsTrue(!GetIsProjectExistCreateGlossaryTest(projectName),
                "Ошибка: проект остался в списке");
        }

        /// <summary>
        /// Метод тестирования удаления Проекта, проверка списка при создании термина глоссария
        /// </summary>
        [Test]
        public void DeleteProjectCheckCreateGlossaryItemTest()
        {
            // Создать проект с уникальным именем
            string projectName = GetProjectUniqueName();
            CreateCATProject(projectName);
            Thread.Sleep(1000);

            // Удалить проект
            ClickDeleteProject(projectName);

            Thread.Sleep(1000);
            // Проверить, что проекта нет в списке при создании термина глоссария
            Assert.IsTrue(!GetIsProjectExistCreateGlossaryItemTest(projectName),
                "Ошибка: проект остался в списке");
        }
        
        private void SetProjectNewName(string projectName, string newProjectName)
        {
            // Нажать Изменить
            string projectXPath = GetProjectRowXPath(projectName);
            string editBtnXPath = projectXPath + "//a[contains(@class,'domain js-edit-domain')]";
            Driver.FindElement(By.XPath(editBtnXPath)).Click();

            // Ввести новое имя проекта
            string projectNameXPath = projectXPath + "//div[contains(@class,'js-edit-mode')]//input[contains(@class,'js-domain-name-input')]";
            Driver.FindElement(By.XPath(projectNameXPath)).Clear();
            Driver.FindElement(By.XPath(projectNameXPath)).SendKeys(newProjectName);
            // Сохранить
            Driver.FindElement(By.XPath(projectXPath +
                "//div[contains(@class,'l-corpr__domainbox js-edit-mode')]//a[contains(@class,'save js-save-domain')]")).Click();
            Thread.Sleep(1000);
        }

        private string GetProjectRowXPath(string projectName)
        {
            string xPath = "";
            // Получить список всех проектов
            IList<IWebElement> projectsList = Driver.FindElements(By.XPath(
                ".//table[contains(@class,'js-domains js-sortable-table')]//tr[contains(@class,'l-corpr__trhover js-row')]"));
            for (int i = 0; i < projectsList.Count; ++i)
            {
                // Проверить имя проекта
                if (projectsList[i].Text == projectName)
                {
                    xPath = ".//tr[contains(@class,'l-corpr__trhover js-row')][" + (i + 1) + "]";
                    break;
                }
            }
            return xPath;
        }

        private string GetProjectUniqueName()
        {
            return "TestProject" + DateTime.UtcNow.Ticks.ToString();
        }

        private void AssertExistingProjectNameError()
        {
            // Проверить, появилась ли ошибка существующего имени
            string rowXPath = ".//table[contains(@class,'js-domains js-sortable-table')]//tr[contains(@class,'js-row js-error-row')]//div[contains(@class,'js-error-text g-hidden')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(rowXPath)).Displayed,
                "Ошибка: не появилась ошибка существующего имени");
        }

        private void ClickDeleteProject(string projectName)
        {
            // Получить xPath кнопки Удалить для проекта
            string deleteBtnXPath = GetProjectRowXPath(projectName) + "//a[contains(@class,'domain js-delete-domain')]";
            // Нажать Удалить
            Driver.FindElement(By.XPath(deleteBtnXPath)).Click();
        }

        private bool GetIsProjectExistCreateTM(string projectName)
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
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-domains-multiselect')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'ui-multiselect-menu')][2]")).Displayed);

            // Получить список проектов
            IList<IWebElement> projectList = Driver.FindElements(By.XPath(
                ".//div[contains(@class,'ui-multiselect-menu')][2]//ul[contains(@class,'ui-multiselect-checkboxes')]//li//label//span[contains(@class,'ui-multiselect-item-text')]"));
            bool isProjectExist = false;
            foreach (IWebElement el in projectList)
            {
                if (el.Text == projectName)
                {
                    // Если проект в списке
                    isProjectExist = true;
                    break;
                }
            }

            return isProjectExist;
        }

        private bool GetIsProjectExistCreateGlossaryTest(string projectName)
        {
            // Перейти на вкладку Глоссарии
            SwitchGlossaryTab();

            // Нажать кнопку Create a glossary
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-create-glossary-button')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Click();
            // ждем загрузку формы
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]")));

            string xPathProjectField = ".//div[contains(@class,'js-popup-edit-glossary')][2]//input[contains(@name,'Domain')]";
            // Нажать, чтобы появился список проектов
            Driver.FindElement(By.XPath(
                xPathProjectField + "/..//div[contains(@class,'ui-multiselect')]")).Click();
            // Получить список проектов в списке
            IList<IWebElement> projectList = Driver.FindElements(By.XPath(
                ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')]"));
            bool isProjectExist = false;
            foreach (IWebElement el in projectList)
            {
                if (el.Text == projectName)
                {
                    // Если проект в списке
                    isProjectExist = true;
                    break;
                }
            }

            return isProjectExist;
        }

        private bool GetIsProjectExistCreateGlossaryItemTest(string projectName)
        {
            // Перейти на вкладку Глоссарии
            SwitchGlossaryTab();

            // Нажать кнопку Create a glossary
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-create-glossary-button')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Click();
            // ждем загрузку формы
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]")));

            // Ввести имя
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//input[contains(@class,'js-glossary-name')]")).
                SendKeys("TestGlossary:" + DateTime.UtcNow.Ticks.ToString());

            // Добавить комментарий
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//textarea[contains(@name,'Comment')]")).
                SendKeys("Test Glossary Generated by Selenium");

            // Нажать сохранить
            Driver.FindElement(By.XPath(
               ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[contains(@class,'js-save')]")).Click();
            // Ответ формы
            Thread.Sleep(2000);

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
            Driver.FindElement(By.XPath(rowXPath)).Click();
            // Добавить
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'js-add-tbx-attribute')]"))).Click();
            Thread.Sleep(1000);

            // Сохранить
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]")).Click();
            // Дождаться закрытия формы
            Thread.Sleep(2000);

            // Нажать New item
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-add-concept')]"))).Click();

            // Нажать на поле появилось
            string fieldXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__edit js-edit')]//select[@name='Domain']/..//span[contains(@class,'js-dropdown')]";
            Driver.FindElement(By.XPath(fieldXPath)).Click();

            // Получить список проектов в списке
            IList<IWebElement> projectList = Driver.FindElements(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item')]"));
            bool isProjectExist = false;
            foreach (IWebElement el in projectList)
            {
                if (el.Text == projectName)
                {
                    // Если проект в списке
                    isProjectExist = true;
                    break;
                }
            }

            return isProjectExist;
        }

        private void AssertEditingModeProject()
        {
            // Проверить, что проект не сохранился, а остался в режиме редактирования
            string saveBtnXPath =
                ".//tr[@class='l-corpr__trhover js-row']//td[contains(@class,'js-cell')]//div[contains(@class,'js-edit-mode')]//a[contains(@class,'domain save js-save-domain')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(saveBtnXPath)).Displayed,
                "Ошибка: не остался в режиме редактирования");
        }
    }
}