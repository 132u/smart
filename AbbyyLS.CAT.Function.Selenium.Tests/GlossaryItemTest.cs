﻿using System;
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
            GlossaryPage.ClickNewItemBtn();
            // Заполнить поля с терминами
            FillNewItemExtended();
            // Сохранить
            GlossaryPage.ClickSaveExtendedConcept();
            GlossaryPage.WaitConceptSave();
            // Свернуть
            GlossaryPage.ClickTurnOffBtn();

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
            CreateItemAndSave(uniqueTerm, uniqueTerm, false);
            
            // Проверить, что появилось предупреждение
            Assert.IsTrue(GlossaryPage.WaitDuplicateErrorAppear(),
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
            GlossaryPage.ClickNewItemBtn();
            // Дождаться появления строки для ввода
            GlossaryPage.WaitConceptTableAppear();
            // Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
            Driver.Manage().Window.Maximize();
            // Нажать Сохранить
            GlossaryPage.ClickSaveTermin();
            // Проверить, что появилось предупреждение
            Assert.IsTrue(GlossaryPage.GetIsGlossaryErrorExist(),
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
            GlossaryPage.ClickAddSynonym(1);
            term1 += DateTime.Now;
            // Ввести синоним
            GlossaryPage.FillSynonymTermLanguage(1, term1);

            // Нажать добавить синоним для второго слова
            GlossaryPage.ClickAddSynonym(2);
            term2 += DateTime.Now;
            // Ввести синоним
            GlossaryPage.FillSynonymTermLanguage(2, term2);

            // Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
            Driver.Manage().Window.Maximize();
            // Нажать Сохранить
            GlossaryPage.ClickSaveTermin();
            GlossaryPage.WaitConceptGeneralSave();

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
            GlossaryPage.ClickAddSynonym(1);
            term1 += DateTime.Now;
            // Ввести синоним
            GlossaryPage.FillSynonymTermLanguage(1, term1);

            // Нажать добавить синоним для второго слова
            GlossaryPage.ClickAddSynonym(2);
            // Ввести синоним
            GlossaryPage.FillSynonymTermLanguage(2, term2);
            
            // Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
            Driver.Manage().Window.Maximize();
            // Нажать Сохранить
            GlossaryPage.ClickSaveTermin();
            Thread.Sleep(1000);

            // Проверить, что поля отмечены красным
            Assert.IsTrue(GlossaryPage.GetIsTermErrorExist(2),
                "Ошибка: поле с совпадающим термином не отмечено ошибкой");
            Assert.IsTrue(GlossaryPage.GetIsTermErrorMessageExist(2),
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
            GlossaryPage.ClickTermRow();
            // Нажать на "корзинку"
            GlossaryPage.ClickDeleteBtn();
            GlossaryPage.WaitConceptGeneralSave();
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
            GlossaryPage.ClickCancelEditBtn();

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
            GlossaryPage.FillSearchField(uniqueData);
            GlossaryPage.ClickSearchBtn();
            // Дождаться окончания поиска
            Thread.Sleep(2000);
            int itemCountAfter = GetCountOfItems();
            // Проверить, что найден только один термин
            Assert.IsTrue(itemCountAfter == 1, "Ошибка: должен быть найден только один термин");

            // Проверить, что показан нужный термин
            string itemText = GlossaryPage.GetFirstTermText();
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
            GlossaryPage.FillSearchField(uniqueData);
            GlossaryPage.ClickSearchBtn();
            // Дождаться окончания поиска
            Thread.Sleep(2000);
            int itemCountAfter = GetCountOfItems();
            // Проверить, что найден только один термин
            Assert.IsTrue(itemCountAfter == 1, "Ошибка: должен быть найден только один термин");

            // Проверить, что показан нужный термин
            string itemText = GlossaryPage.GetFirstTermText();
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
            // Найти перевод слова
            InitSearch(uniqueData);

            List<string> glossaryList = new List<string>();
            glossaryList.Add(firstGlossaryName);
            glossaryList.Add(secondGlossaryName);


            List<string> glossaryNames = SearchPage.GetGlossaryResultNames();
            // Проверить, что найдено два термина
            Assert.IsTrue(glossaryNames.Count == glossaryList.Count, "Ошибка: поиск должен найти только два результата");
			
			Assert.IsTrue(glossaryNames[0].Contains(glossaryList[0]), "Ошибка: в списке нет глоссария:\n" + glossaryList[0]);

			Assert.IsTrue(glossaryNames[1].Contains(glossaryList[1]), "Ошибка: в списке нет глоссария:\n" + glossaryList[1]);

            for (int i = 0; i < glossaryNames.Count; ++i)
            {
                // Получить найденный термин
                string itemText = SearchPage.GetGlossaryResultSrcText(i + 1);
                // Проверить, что найден правильный термин
                bool isRightItem = itemText == firstTerm;

                // Проверить, что найден и правильный глоссарий, и правильный термин
                Assert.IsTrue(isRightItem, "Ошибка: найден неправильный термин (" + (i + 1) + "-й найденный результат)");
            }
        }

        /// <summary>
        /// Тест: создать термин с несколькими языками
        /// </summary>
        [Test]
        public void CreateItemMultiLanguageGlossary()
        {
            // Имя глоссария
            string glossaryName = "TestGlossary" + DateTime.Now.Ticks;
            // Список языков
            List<CommonHelper.LANGUAGE> langList = new List<CommonHelper.LANGUAGE>();
            langList.Add(CommonHelper.LANGUAGE.German);
            langList.Add(CommonHelper.LANGUAGE.French);
            langList.Add(CommonHelper.LANGUAGE.Japanise);
            langList.Add(CommonHelper.LANGUAGE.Lithuanian);

            // Создать глоссарий
            CreateGlossaryByName(glossaryName, true, langList);

            // Создать термин
            CreateItemExtended();

            // Свернуть
            GlossaryPage.ClickTurnOffBtn();

            // Проверить количество терминов
            Assert.IsTrue(GetCountOfItems() > 0, "Ошибка: количество терминов должно быть больше 0 (термин не сохранился)");

            // Удалить глоссарий, чтобы не было глоссария с многими языками
            DeleteGlossary();
        }

        /// <summary>
        /// Тест: изменение термина (обычный режим)
        /// </summary>
        [Test]
        public void EditItemGeneral()
        {
            // Создать глоссарий
            CreateGlossaryByName(GetUniqueGlossaryName());
            // Создать термин
            CreateItemAndSave();

            // Нажать на строку с термином
            GlossaryPage.ClickTermRow();
            // Нажать Редактировать
            GlossaryPage.ClickEditTermBtn();

            string newTermText = "New Term " + DateTime.Now;
            GlossaryPage.FillTermGeneralMode(newTermText);

            // Сохранить
            GlossaryPage.ClickSaveTermin();
            // Проверить, что термин сохранился
            Assert.IsTrue(GlossaryPage.WaitConceptGeneralSave(), "Ошибка: термин не сохранился");
            // Проверить, что термин сохранился с новым значением
            Assert.IsTrue(GetIsTermTextExist(newTermText), "Ошибка: термин не сохранил изменения");
        }

        /// <summary>
        /// Тест: изменение термина (расширенный режим)
        /// </summary>
        [Test]
        public void EditItemExtended()
        {
            // Имя глоссария
            string glossaryName = "TestGlossary" + DateTime.Now.Ticks;
            // Список языков
            // Список языков
            List<CommonHelper.LANGUAGE> langList = new List<CommonHelper.LANGUAGE>();
            langList.Add(CommonHelper.LANGUAGE.German);
            langList.Add(CommonHelper.LANGUAGE.French);
            langList.Add(CommonHelper.LANGUAGE.Japanise);
            langList.Add(CommonHelper.LANGUAGE.Lithuanian);

            // Создать глоссарий
            CreateGlossaryByName(glossaryName, true, langList);

            // Создать термин
            CreateItemExtended();

            // Нажать Редактировать
            GlossaryPage.ClickEditBtn();
            string newTermText = "New Term " + DateTime.Now;
            EditAllExtendedItems(newTermText);

            // Сохранить
            GlossaryPage.ClickSaveExtendedConcept();
			Thread.Sleep(5000);
            // Проверить, что термин сохранился
            Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");
            // Свернуть
			GlossaryPage.ClickTurnOffBtn();
            // Проверить, что термин сохранился с новым значением
            Assert.IsTrue(GetIsTermTextExist(newTermText), "Ошибка: термин не сохранил изменения");
            // Удалить глоссарий
            DeleteGlossary();
        }

        /// <summary>
        /// Не тест: перед выполнением остальных тестов нужно проверить права пользователя
        /// Если нет прав на поиск терминов нет,
        /// добавить права
        /// </summary>
        [Test]
        public void aaaaaFirstTestCheckUserRights()
        {
            AddUserRights();
        }

        /// <summary>
        /// Вернуть, есть ли термин с таким текстом
        /// </summary>
        /// <param name="termText">текст</param>
        /// <returns>есть</returns>
        protected bool GetIsTermTextExist(string termText)
        {
            return GlossaryPage.GetIsTermExistByText(termText);
        }

        /// <summary>
        /// Создать термин в расширенном режиме
        /// </summary>
        protected void CreateItemExtended()
        {
            // Изменить структуру для перехода в расширенный режим
            EditGlossaryStructureAddField();

            // Нажать New item
            GlossaryPage.ClickNewItemBtn();
            // Заполнить поля с терминами
            FillNewItemExtended();
            // Сохранить
            GlossaryPage.ClickSaveExtendedConcept();

            Assert.IsTrue(GlossaryPage.WaitConceptSave(),
                "Ошибка: термин не сохранился");
        }
    }
}
