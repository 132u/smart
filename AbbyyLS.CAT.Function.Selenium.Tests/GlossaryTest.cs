using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class GlossaryTest : BaseTest
    {
        public GlossaryTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
            // Авторизация
            Authorization();

            // Перейти на вкладку Glossary
            SwitchGlossaryTab();
        }

        protected void SwitchCurrentGlossary(string glossaryName)
        {
            // Перейти на страницу глоссария
            string xPath = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text()='" + glossaryName + "']";
            Driver.FindElement(By.XPath(xPath)).Click();
        }

        protected bool GetIsExistGlossary(string glossaryName)
        {
            // Получить: существует ли глоссарий с таким именем
            bool isExist = false;
            setDriverTimeoutMinimum();
            IList<IWebElement> glossaryList = Driver.FindElements(By.XPath("//tr[contains(@class, 'js-glossary-row')]/td[1]/p"));
            foreach (IWebElement el in glossaryList)
            {
                if (el.Text == glossaryName)
                {
                    isExist = true;
                    break;
                }
            }
            setDriverTimeoutDefault();
            return isExist;
        }

        protected string GetUniqueGlossaryName()
        {
            // Получить уникальное имя глоссария (т.к. добавляется точная дата и время, то не надо проверять, есть ли такой глоссарий в списке)
            return GlossaryName + DateTime.Now.ToString();
        }

        protected string CreateGlossaryAndReturnToGlossaryList()
        {
            // Получить уникальное имя для глоссария
            string glossaryName = GetUniqueGlossaryName();
            // Создать глоссарий
            CreateGlossaryByName(glossaryName);
            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            return glossaryName;
        }

        protected int GetCountOfItems()
        {
            setDriverTimeoutMinimum();
            int result = Driver.FindElements(By.XPath(".//tr[contains(@class, 'js-concept-row')]")).Count;
            setDriverTimeoutDefault();
            return result;
        }

        protected void OpenEditGlossaryStructure()
        {
            // Открыть Редактирование глоссария
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-edit-submenu')]"))).Click();
            // Выбрать Редактирование структуры
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-edit-structure-btn')]"))).Click();
            // Дождаться появления формы
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-structure')]")));
        }

        protected void EditGlossaryStructureAddField()
        {
            OpenEditGlossaryStructure();

            // Найти невыбранное поле и добавить его
            string attributeValue = "";
            // Получить список полей
            IList<IWebElement> allFields = Driver.FindElements(By.XPath(".//table[contains(@class, 'js-predefined-attrs-table concept')]//tr[contains(@class, 'l-editgloss__tr js-attr-row')]"));
            for (int i = 0; i < allFields.Count; ++i)
            {
                attributeValue = allFields[i].GetAttribute("class");
                // Если поле не скрыто, нажать на него
                if (!attributeValue.Contains("g-hidden"))
                {
                    // Получить xPath текущей строки
                    string xPath = ".//table[contains(@class, 'js-predefined-attrs-table concept')]//tr[contains(@class, 'l-editgloss__tr js-attr-row')][" + (i + 1).ToString() + "]/td[1]";
                    // Выбрать строку
                    Driver.FindElement(By.XPath(xPath)).Click();
                    // Добавить
                    Wait.Until((d) => d.FindElement(By.XPath(
                        ".//span[contains(@class,'js-add-tbx-attribute')]"))).Click();
                    Thread.Sleep(500);
                    break;
                }
            }

            // Сохранить
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]")).Click();
            // Дождаться закрытия формы
            Thread.Sleep(1000);
        }

        protected void CreateItemAndSave(string firstTerm = "", string secondTerm = "", bool shouldSaveOk = true)
        {
            // Открыть форму добавления термина и заполнить поля
            FillCreateItem(firstTerm, secondTerm);
            // Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
            Driver.Manage().Window.Maximize();
            // Нажать Сохранить
            Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing opened')]//a[contains(@class, 'js-save-btn')]")).Click();
            if (shouldSaveOk)
            {
                Assert.IsTrue(WaitUntilDisappearElement(".//tr[contains(@class, 'js-concept-row')]//a[contains(@class,'js-save-btn')]"), "Ошибка: термин не сохранился");
            }
        }

        protected void FillCreateItem(string firstTerm = "", string secondTerm = "")
        {
            // Нажать New item
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'js-add-concept')]"))).Click();
            // Дождаться появления строки для ввода
            Wait.Until((d) => d.FindElement(By.XPath(".//table[contains(@class,'js-concepts')]")));
            // Заполнить термин
            if (firstTerm.Length == 0)
            {
                firstTerm = "Term First Language" + DateTime.Now.ToString();
            }
            Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing')]/td[1]//input")).SendKeys(firstTerm);
            if (secondTerm.Length == 0)
            {
                secondTerm = "Term Second Language" + DateTime.Now.ToString();
            }
            Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing')]/td[2]//input")).SendKeys(secondTerm);
        }

        protected void SuggestTermAndSave(string termFirst, string termSecond, bool isNeedSelectGlossary = false, string glossaryName = "")
        {
            // Открыть форму предложения термина и заполнить полня
            SuggestTermFillTerms(termFirst, termSecond);
            if (isNeedSelectGlossary)
            {
                // Открыть список с глоссариями
                Driver.FindElement(By.XPath(".//span[contains(@class, 'js-dropdown addsuggglos')]")).Click();
                Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class, 'js-dropdown__list addsuggglos')]")).Enabled);
                // Выбрать наш первый созданный глоссарий в выпавшем списке
                string xPathFirstGlossary = ".//span[contains(@class, 'js-dropdown__item addsuggglos')][@title='" + glossaryName + "']";
                Driver.FindElement(By.XPath(xPathFirstGlossary)).Click();
            }

            // Сохранить
            Driver.FindElement(By.XPath(".//input[contains(@class, 'js-save-btn')]")).Click();
            Thread.Sleep(2000);
        }

        protected void SuggestTermFillTerms(string suggestTermFirst, string suggestTermSecond)
        {
            // Нажать Предложить термин
            Driver.FindElement(By.XPath(".//span[contains(@class, 'g-redbtn js-add-suggest')]//a")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-bd js-add-suggest-popup')]")).Displayed);
            // Заполнить термин
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'l-addsugg__contr lang js-language')][1]//input[contains(@class, 'js-addsugg-term')]"))
                .SendKeys(suggestTermFirst);
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'l-addsugg__contr lang js-language')][2]//input[contains(@class, 'js-addsugg-term')]"))
                .SendKeys(suggestTermSecond);
        }

        protected string GetSuggestTermRowsXPath()
        {
            // Вернуть xPath строк с предложенными терминами
            return ".//tr[contains(@class, 'js-suggest-row')]/td[contains(@class, 'js-glossary-cell')]//p";
        }

        protected void FillNewItemExtended()
        {
            // Текст терминов
            string termText = "Example Term Text " + DateTime.Now.ToString();
            string xPathLang = ".//div[contains(@class, 'js-terms-tree')]//div[contains(@class, 'l-corprtree__langbox')]";
            // Поля языков
            IList<IWebElement> termList = Driver.FindElements(By.XPath(xPathLang + "//span[contains(@class,'js-add-term')]"));
            for (int i = 0; i < termList.Count; ++i )
            {
                // Нажать Add
                termList[i].Click();
                // Ввести термин
                Driver.FindElement(By.XPath(xPathLang + "[" + (i + 1) + "]//span[contains(@class,'js-term-editor')]//input")).SendKeys(termText);
            }
        }


        /// <summary>
        /// Добавить язык при создании глоссария
        /// </summary>
        /// <param name="langNumber">код языка</param>
        protected void AddLanguageCreateGlossary(int langNumber)
        {
            // Кликнуть по Плюсу
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-edit-glossary')][2]//span[contains(@class,'js-add-language-button')]")).Click();
            // Открыть выпадающий список у добавленного языка
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-glossary-language')][last()]//span[contains(@class,'js-dropdown')]")).Click();
            // Выбрать язык
            Assert.IsTrue(IsElementPresent(By.XPath(".//span[contains(@class,'js-dropdown__list')]//span[@data-id='" + langNumber + "']")),
                "Ошибка: указанного языка (data-id=" + langNumber + ") нет в списке");

            Driver.FindElement(By.XPath(".//span[contains(@class,'js-dropdown__list')]//span[@data-id='" + langNumber + "']")).Click();
            WaitUntilDisappearElement(".//span[contains(@class,'js-dropdown__list')]");
        }

        /// <summary>
        /// Создать глоссарий с несколькими языками
        /// </summary>
        /// <param name="glossaryName">имя глоссария</param>
        /// <param name="langList">список ID языков</param>
        protected void CreateGlossaryMultiLanguage(string glossaryName, List<int> langList)
        {
            // Открыть форму создания глоссария
            OpenCreateGlossary();

            // Ввести имя
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//input[contains(@class,'js-glossary-name')]")).
                SendKeys(glossaryName);

            // Добавить комментарий
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//textarea[contains(@name,'Comment')]")).
                SendKeys("Test Glossary Generated by Selenium");

            // Добавить языки
            foreach (int langID in langList)
            {
                AddLanguageCreateGlossary(langID);
            }

            // Нажать сохранить
            Driver.FindElement(By.XPath(
               ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[contains(@class,'js-save')]")).Click();

            // Дождаться закрытия формы
            Assert.IsTrue(WaitUntilDisappearElement(".//div[contains(@class,'js-popup-edit-glossary')]"),
                "Ошибка: форма добавления глоссария не пропала - глоссарий не добавился");
        }


        /// <summary>
        /// Удаление глоссария
        /// </summary>
        protected void DeleteGlossary()
        {
            // Открыть редактирование свойств глоссария
            OpenGlossaryProperties();
            // Нажать Удалить глоссарий 
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[contains(@class, 'js-delete')]"))).Click();

            // Нажать Да (удалить)
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//a[contains(@class, 'js-confirm-delete-glossary')]")).Click();

            Wait.Until((d) => d.FindElement(By.XPath(
               ".//span[contains(@class,'js-create-glossary-button')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Displayed);
        }

        /// <summary>
        /// Открыть свойства глоссария
        /// </summary>
        protected void OpenGlossaryProperties()
        {
            // Нажать Редактирование
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-edit-submenu')]"))).Click();
            // Нажать на Properties
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-edit-glossary-btn')]"))).Click();
        }

        protected void AddUserRights()
        {
            // хардкор!
            // Проверить, что есть кнопка Предложить термин на странице со списком глоссариев
            if (IsElementPresent(By.XPath(".//span[contains(@class,'js-add-suggest')]")))
            {
                Assert.Pass("Все хорошо - у пользователя права есть!");
            }
            // Иначе: добавляем права

            // Перейти в пользователи и права
            Driver.FindElement(By.XPath(".//a[contains(@href,'/Enterprise/Users')]")).Click();
            // Перейти в группы
            Driver.FindElement(By.XPath(".//a[contains(@href,'/Enterprise/Groups')]")).Click();
            // Выбрать Administrators
            Driver.FindElement(By.XPath(".//td[contains(@class,'js-group-name')][text()='Administrators']")).Click();
            // Edit
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-editgroup-btn')]")).Click();
            // Add right
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-add-right-btn')]")).Click();
            // Suggest concepts without glossary
            Driver.FindElement(By.XPath(".//li[@data-type='AddSuggestsWithoutGlossary']//input")).Click();
            // Next
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-next')]")).Click();
            // Add
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-add')]//a[contains(text(),'Add')]")).Click();
            Thread.Sleep(1000);
            // Add right
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-add-right-btn')]")).Click();
            // Suggest concepts without glossary
            Driver.FindElement(By.XPath(".//li[@data-type='GlossarySearch']//input")).Click();
            // Next
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-next')]")).Click();
            // All Glossaries
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-scope-section')][2]//input[contains(@name,'accessRightScopeType')]")).Click();
            // Next
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-next')]")).Click();
            // Add
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-add')]//a[contains(text(),'Add')]")).Click();
            Thread.Sleep(1000);
            // Save
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn')]")).Click();
            Thread.Sleep(1000);
            SwitchGlossaryTab();
            Assert.IsTrue(IsElementPresent(By.XPath(".//span[contains(@class,'js-add-suggest')]")), "права не добавились");
        }
    }
}
