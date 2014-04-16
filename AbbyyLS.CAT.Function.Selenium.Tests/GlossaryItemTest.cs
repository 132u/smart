using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class GlossaryItemTest : GlossaryTest
    {
        public GlossaryItemTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Метод тестирования создания Item с обычном режиме
        /// </summary>
        [Test]
        public void CreateItemGeneralTest()
        {
            // Создать глоссарий
            CreateGlossaryByName(GetUniqueGlossaryName());
            // Создать термин
            CreateItemAndSave();

            // Проверить количество терминов
            Assert.IsTrue(GetCountOfItems() > 0, "Ошибка: количество терминов должно быть больше 0 (термин не сохранился)");
        }

        /// <summary>
        /// Метод тестирования создания Item с расширенном режиме
        /// </summary>
        [Test]
        public void CreateItemExtendedTest()
        {
            // Создать новый глоссарий
            CreateGlossaryByName(GetUniqueGlossaryName());

            // Изменить структуру для перехода в расширенный режим
            EditGlossaryStructureAddField();

            // Нажать New item
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-add-concept')]"))).Click();
            // Заполнить поля с терминами
            FillNewItemExtended();
            // Сохранить
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-text')]")).Click();
            // Свернуть
            Driver.FindElement(By.XPath(".//a[contains(@class,'iconup')]")).Click();

            // Проверить количество терминов
            Assert.IsTrue(GetCountOfItems() > 0, "Ошибка: количество терминов должно быть больше 0 (термин не сохранился)");
        }

        /// <summary>
        /// Метод тестирования создания существующего термина
        /// </summary>
        [Test]
        public void CreateExistingItemTest()
        {
            // Создать глоссарий
            CreateGlossaryByName(GetUniqueGlossaryName());
            string uniqueTerm = "TestTermText" + DateTime.Now.ToString();
            // Создать термин
            CreateItemAndSave(uniqueTerm, uniqueTerm);
            // Создать такой же термин
            CreateItemAndSave(uniqueTerm, uniqueTerm);
            // Проверить, что появилось предупреждение
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-confirm')]//form[contains(@action,'/Concepts/Save')]")).Displayed,
                "Ошибка: должно появиться предупреждение о добавлении существующего термина");
        }

        /// <summary>
        /// Метод тестирования создания пустого термина
        /// </summary>
        [Test]
        public void CreateEmptyItemTest()
        {
            // Создать глоссарий
            CreateGlossaryByName(GetUniqueGlossaryName());
            // Нажать New item
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'js-add-concept')]"))).Click();
            // Дождаться появления строки для ввода
            Wait.Until((d) => d.FindElement(By.XPath(".//table[contains(@class,'js-concepts')]")));
            // Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
            Driver.Manage().Window.Maximize();
            // Нажать Сохранить
            Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing')]//a[contains(@class, 'js-save-btn')]")).Click();
            // Проверить, что появилось предупреждение
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//tr//td[contains(@class,'glossaryError')]")).Displayed,
                "Ошибка: должно появиться предупреждение о добавлении пустого термина");
        }

        /// <summary>
        /// Метод тестирования создания термина с синонимами
        /// </summary>
        [Test]
        public void CreateItemSynonymsTest()
        {
            // Создать глоссарий
            CreateGlossaryByName(GetUniqueGlossaryName());
            string term1 = "Term1";
            string term2 = "Term2";
            // Открыть форму добавления термина и заполнить поля
            FillCreateItem(term1, term2);

            // Нажать добавить синоним для первого слова
            Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing')]/td[1]//span[contains(@class,'js-add-term')]")).Click();
            term1 += DateTime.Now;
            // Ввести синоним
            Driver.FindElement(By.XPath(
                ".//tr[contains(@class, 'js-concept-row js-editing')]/td[1]//div[contains(@class,'js-term-box')]//input[contains(@class,'js-term')]")).SendKeys(term1);

            // Нажать добавить синоним для второго слова
            Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing')]/td[2]//span[contains(@class,'js-add-term')]")).Click();
            term2 += DateTime.Now;
            // Ввести синоним
            Driver.FindElement(By.XPath(
                ".//tr[contains(@class, 'js-concept-row js-editing')]/td[2]//div[contains(@class,'js-term-box')]//input[contains(@class,'js-term')]")).SendKeys(term2);

            // Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
            Driver.Manage().Window.Maximize();
            // Нажать Сохранить
            Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing opened')]//a[contains(@class, 'js-save-btn')]")).Click();
            Thread.Sleep(2000);

            // Проверить количество терминов
            Assert.IsTrue(GetCountOfItems() > 0, "Ошибка: количество терминов должно быть больше 0 (термин не сохранился)");
        }

        /// <summary>
        /// Метод тестирования создания термина с одинаковыми синонимами
        /// </summary>
        [Test]
        public void CreateItemEqualSynonymsTest()
        {
            // Создать глоссарий
            CreateGlossaryByName(GetUniqueGlossaryName());
            string term1 = "Term1";
            string term2 = "Term2";
            // Открыть форму добавления термина и заполнить поля
            FillCreateItem(term1, term2);

            // Нажать добавить синоним для первого слова
            Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing')]/td[1]//span[contains(@class,'js-add-term')]")).Click();
            term1 += DateTime.Now;
            // Ввести синоним
            Driver.FindElement(By.XPath(
                ".//tr[contains(@class, 'js-concept-row js-editing')]/td[1]//div[contains(@class,'js-term-box')]//input[contains(@class,'js-term')]")).SendKeys(term1);

            // Нажать добавить синоним для второго слова
            Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing')]/td[2]//span[contains(@class,'js-add-term')]")).Click();
            // Ввести синоним
            Driver.FindElement(By.XPath(
                ".//tr[contains(@class, 'js-concept-row js-editing')]/td[2]//div[contains(@class,'js-term-box')]//input[contains(@class,'js-term')]")).SendKeys(term2);

            // Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
            Driver.Manage().Window.Maximize();
            // Нажать Сохранить
            Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing opened')]//a[contains(@class, 'js-save-btn')]")).Click();
            Thread.Sleep(2000);

            // Проверить, что поля отмечены красным
            Assert.IsTrue(IsElementPresent(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing')]/td[2]//p[contains(@class,'l-error')]")),
                "Ошибка: поле с совпадающим термином не отмечено ошибкой");
            Assert.IsTrue(IsElementPresent(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing')]/td[2]//div[contains(@class,'js-term-box')][contains(@class,'l-error')]")),
                "Ошибка: поле с совпадающим термином не отмечено ошибкой");
        }

        /// <summary>
        /// Метод тестирования удаления Термина
        /// </summary>
        [Test]
        public void DeleteItemTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Создать термин
            CreateItemAndSave();
            int itemsCount = GetCountOfItems();

            // Расширить окно, чтобы "корзинка" была видна, иначе Selenium ее "не видит" и выдает ошибку
            Driver.Manage().Window.Maximize();
            // Выделить ячейку, чтобы "корзинка" появилась
            string rowXPath = ".//tr[contains(@class, 'js-concept-row')][1]/td[5]";
            Driver.FindElement(By.XPath(rowXPath)).Click();
            // Нажать на "корзинку"
            rowXPath += "//a[contains(@class, 'js-delete-btn')]";
            Driver.FindElement(By.XPath(rowXPath)).Click();
            Thread.Sleep(2000);
            // Сравнить количество терминов
            int itemsCountAfter = GetCountOfItems();
            Assert.IsTrue(itemsCountAfter < itemsCount, "Ошибка: количество терминов не уменьшилось");
        }


        /// <summary>
        /// Метод тестирования кнопки отмены создания термина
        /// </summary>
        [Test]
        public void CancelCreateItemTest()
        {
            // Создать глоссарий
            CreateGlossaryByName(GetUniqueGlossaryName());
            // Открыть форму Создание термина и заполнить ее
            FillCreateItem();
            // Отменить
            Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing')]//a[contains(@class, 'js-cancel-btn')]")).Click();

            // Проверить, что количество терминов равно нулю
            Assert.IsTrue(GetCountOfItems() == 0, "Ошибка: количество терминов должно быть равно 0");
        }

        /// <summary>
        /// Метод тестирования поиска термина в глоссарии по слову из первого языка
        /// </summary>
        [Test]
        public void SearchItemGlossaryFirstLangTest()
        {
            // Создать глоссарий
            CreateGlossaryByName(GetUniqueGlossaryName());
            string uniqueData = DateTime.UtcNow.Ticks.ToString() + "1Term";
            string firstTerm = "Test First Term " + uniqueData;
            string secondTerm = "Test Second Term " + DateTime.UtcNow.ToString();
            // Создать термин
            CreateItemAndSave(firstTerm, secondTerm);
            // Создать другой термин
            CreateItemAndSave();

            // Получить количество терминов
            int itemCountBefore = GetCountOfItems();

            // Инициировать поиск по уникальному слову в первом термине
            Driver.FindElement(By.XPath(".//input[contains(@class,'js-search-term')]")).Clear();
            Driver.FindElement(By.XPath(".//input[contains(@class,'js-search-term')]")).SendKeys(uniqueData);
            Driver.FindElement(By.XPath(".//a[contains(@class,'js-search-by-term')]")).Click();
            // Дождаться окончания поиска
            Thread.Sleep(2000);
            int itemCountAfter = GetCountOfItems();
            // Проверить, что найден только один термин
            Assert.IsTrue(itemCountAfter == 1, "Ошибка: должен быть найден только один термин");

            // Проверить, что показан нужный термин
            string itemText = Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row')]//td[contains(@class,'glossaryShort')][1]//p")).Text.Trim();
            Assert.AreEqual(firstTerm, itemText, "Ошибка: найден неправильный термин");
        }


        /// <summary>
        /// Метод тестирования поиска термина в глоссарии по слову из второго языка
        /// </summary>
        [Test]
        public void SearchItemGlossarySecondLangTest()
        {
            // Создать глоссарий
            CreateGlossaryByName(GetUniqueGlossaryName());
            string uniqueData = DateTime.UtcNow.Ticks.ToString() + "2Term";
            string firstTerm = "Test First Term " + DateTime.UtcNow.ToString();
            string secondTerm = "Test Second Term " + uniqueData;

            // Создать термин
            CreateItemAndSave(firstTerm, secondTerm);
            // Создать другой термин
            CreateItemAndSave();

            // Получить количество терминов
            int itemCountBefore = GetCountOfItems();

            // Инициировать поиск по уникальному слову в первом термине
            Driver.FindElement(By.XPath(".//input[contains(@class,'js-search-term')]")).Clear();
            Driver.FindElement(By.XPath(".//input[contains(@class,'js-search-term')]")).SendKeys(uniqueData);
            Driver.FindElement(By.XPath(".//a[contains(@class,'js-search-by-term')]")).Click();
            // Дождаться окончания поиска
            Thread.Sleep(2000);
            int itemCountAfter = GetCountOfItems();
            // Проверить, что найден только один термин
            Assert.IsTrue(itemCountAfter == 1, "Ошибка: должен быть найден только один термин");

            // Проверить, что показан нужный термин
            string itemText = Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row')]//td[contains(@class,'glossaryShort')][1]//p")).Text.Trim();
            Assert.AreEqual(firstTerm, itemText, "Ошибка: найден неправильный термин");
        }

        /// <summary>
        /// Метод тестирования поиска термина из вкладки Поиска
        /// </summary>
        [Test]
        public void SearchItemSearchTabTest()
        {
            // Создать глоссарий
            string firstGlossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(firstGlossaryName);

            string uniqueData = DateTime.UtcNow.Ticks.ToString() + "SearchTest";
            string firstTerm = "Test First Term " + uniqueData;
            string secondTerm = "Test Second Term ";
            // Создать термин
            CreateItemAndSave(firstTerm, secondTerm + DateTime.UtcNow.Ticks.ToString());

            // Перейти на вкладку Глоссарии
            SwitchGlossaryTab();

            // Создать глоссарий
            string secondGlossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(secondGlossaryName);
            // Создать термин
            CreateItemAndSave(firstTerm, secondTerm + DateTime.UtcNow.Ticks.ToString());
            // Перейти на вкладку Поиск
            SwitchSearchTab();
            // Ввести слово для поиска
            Driver.FindElement(By.XPath(".//form[contains(@class,'js-search-form')]//textarea[@id='searchText']")).Clear();
            Driver.FindElement(By.XPath(".//form[contains(@class,'js-search-form')]//textarea[@id='searchText']")).SendKeys(uniqueData);
            // Нажать Перевести
            Driver.FindElement(By.XPath(".//form[contains(@class,'js-search-form')]//span[contains(@class,'g-redbtn search')]//input")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-search-results')]")));
            // Проверить результат
            string tableXPath = ".//div[contains(@class,'js-search-results')]//div[contains(@class,'l-glossary__data')]";
            IList<IWebElement> resultList = Driver.FindElements(By.XPath(tableXPath + "//h2[contains(@class,'l-glossary__srctext')]"));
            // Проверить, что найдено два термина
            Assert.IsTrue(resultList.Count == 2, "Ошибка: поиск должен найти только два результата");

            for (int i = 0; i < resultList.Count; ++i)
            {
                // Получить название глоссария
                string foundGlossary = resultList[i].Text;
                // Проверить, что найден правильный глоссарий
                bool isRightGlossary = foundGlossary.Contains(firstGlossaryName) || foundGlossary.Contains(secondGlossaryName);

                // Получить найденный термин
                string itemXPath = tableXPath +
                    "//table[" + (i + 1) + "]//td/table[contains(@class,'l-glossary__tblsrcword')]//td[contains(@class,'js-cell-width first')]//span[contains(@class,'l-glossary__srcwordtxt')]";
                string itemText = Driver.FindElement(By.XPath(itemXPath)).Text;
                // Проверить, что найден правильный термин
                bool isRightItem = itemText == firstTerm;

                // Проверить, что найден и правильный глоссарий, и правильный термин
                Assert.IsTrue(isRightGlossary && isRightItem, "Ошибка: найден неправильный термин (" + (i + 1) + "-й найденный результат)");
            }
        }

        protected void SwitchSearchTab()
        {
            // Перейти на страницу поиска
            Driver.FindElement(By.XPath(
                ".//ul[@class='g-corprmenu__list']//a[contains(@href,'/Start')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//form[contains(@class,'js-search-form')]")).Displayed);
        }
    }
}
