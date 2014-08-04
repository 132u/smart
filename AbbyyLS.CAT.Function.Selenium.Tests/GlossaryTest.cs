using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    /// <summary>
    /// Тесты глоссариев
    /// </summary>
	public class GlossaryTest : BaseTest
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="url"></param>
        /// <param name="workspaceUrl"></param>
        /// <param name="browserName"></param>
		public GlossaryTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

		/// <summary>
		/// Предварительная подготовка группы тестов
		/// </summary>
		[SetUp]
		public void SetupGlossary()
        {
            // Авторизация
            Authorization();

            // Перейти на вкладку Glossary
            SwitchGlossaryTab();
        }

        /// <summary>
        /// Зайти в глоссарий
        /// </summary>
        /// <param name="glossaryName"></param>
        protected void SwitchCurrentGlossary(string glossaryName)
        {
            // Перейти на страницу глоссария
            GlossaryListPage.ClickGlossaryRow(glossaryName);
            Assert.IsTrue(GlossaryPage.WaitPageLoad(), 
                "Ошибка: не зашли в глоссарий");
        }

        /// <summary>
        /// Вернуть, есть ли глоссарий с таким именем
        /// </summary>
        /// <param name="glossaryName">навзание</param>
        /// <returns>есть</returns>
        protected bool GetIsExistGlossary(string glossaryName)
        {
            // Получить: существует ли глоссарий с таким именем
            return GlossaryListPage.GetIsExistGlossary(glossaryName);
        }

        /// <summary>
        /// Создать уникальное имя
        /// </summary>
        /// <returns>имя</returns>
        protected string GetUniqueGlossaryName()
        {
            // Получить уникальное имя глоссария (т.к. добавляется точная дата и время, то не надо проверять, есть ли такой глоссарий в списке)
            return GlossaryName + DateTime.Now.ToString();
        }

        /// <summary>
        /// Создать глоссарий и вернуться к списку глоссариев
        /// </summary>
        /// <returns>название глоссария</returns>
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

        /// <summary>
        /// Получить количество терминов
        /// </summary>
        /// <returns></returns>
        protected int GetCountOfItems()
        {
            return GlossaryPage.GetConceptCount();
        }

        /// <summary>
        /// Открыть редактирование структуры
        /// </summary>
        protected void OpenEditGlossaryStructure()
        {
            // Открыть Редактирование глоссария
            GlossaryPage.OpenEditGlossaryList();
            // Открыть форму Редактирование структуры
            GlossaryPage.OpenEditStructureForm();
            GlossaryEditStructureForm.WaitPageLoad();
        }

        /// <summary>
        /// Добавить поле в структуру глоссария
        /// </summary>
        protected void EditGlossaryStructureAddField()
        {
            OpenEditGlossaryStructure();

            // Проверить, что открыта нужнная таблица
            Assert.IsTrue(GlossaryEditStructureForm.GetIsConceptTableDisplay(), "Ошибка: в редакторе структуры отображается не та таблица");

            // Нажать на поле Domain
            if (GlossaryEditStructureForm.ClickFieldToAdd(GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Domain))
            {
                // Добавить
                GlossaryEditStructureForm.ClickAddToListBtn();
            }
            // Сохранить
            GlossaryEditStructureForm.ClickSaveStructureBtn();
            // Дождаться закрытия формы
            GlossaryEditStructureForm.WaitFormClose();

            // Дождаться закрытия формы
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Создать термин и сохранить
        /// </summary>
        /// <param name="firstTerm">текст 1 термина/языка</param>
        /// <param name="secondTerm">текст 2 термина/языка</param>
        /// <param name="shouldSaveOk">Должно сохраниться (проверить сохранение)</param>
        protected void CreateItemAndSave(string firstTerm = "", string secondTerm = "", bool shouldSaveOk = true)
        {
            // Открыть форму добавления термина и заполнить поля
            FillCreateItem(firstTerm, secondTerm);
            // Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
            Driver.Manage().Window.Maximize();
            // Нажать Сохранить
            GlossaryPage.ClickSaveTermin();
            if (shouldSaveOk)
            {
                // TODO проверить!
                Assert.IsTrue(GlossaryPage.WaitConceptGeneralSave(), "Ошибка: термин не сохранился");
            }
        }

        /// <summary>
        /// Создать новый термин и заполнить
        /// </summary>
        /// <param name="firstTerm">текст для первого термина</param>
        /// <param name="secondTerm">текст для второго термина</param>
        protected void FillCreateItem(string firstTerm = "", string secondTerm = "")
        {
            // Нажать New item
            GlossaryPage.ClickNewItemBtn();
            // Дождаться появления строки для ввода
            GlossaryPage.WaitConceptTableAppear();
            // Заполнить термин
            if (firstTerm.Length == 0)
            {
                firstTerm = "Term First Language" + DateTime.Now.ToString();
            }
            GlossaryPage.FillTerm(1, firstTerm);
            if (secondTerm.Length == 0)
            {
                secondTerm = "Term Second Language" + DateTime.Now.ToString();
            }
            GlossaryPage.FillTerm(2, secondTerm);
        }

        /// <summary>
        /// Предложить термин и сохранить
        /// </summary>
        /// <param name="termFirst">текст первого термина</param>
        /// <param name="termSecond">текст второго термина</param>
        /// <param name="isNeedSelectGlossary">нужно ли выбирать глоссарий</param>
        /// <param name="glossaryName">название глоссария</param>
        protected void SuggestTermAndSave(string termFirst, string termSecond, bool isNeedSelectGlossary = false, string glossaryName = "")
        {
            // Открыть форму предложения термина и заполнить полня
            SuggestTermFillTerms(termFirst, termSecond);

			// Проверяем языки En-Ru
			if (SuggestTermDialog.GetLanguageId(1) != "9")
			{
				SuggestTermDialog.OpenLanguageList(1);
				SuggestTermDialog.SelectLanguage("9");
			}

			if (SuggestTermDialog.GetLanguageId(2) != "25")
			{
				SuggestTermDialog.OpenLanguageList(2);
				SuggestTermDialog.SelectLanguage("25");
			}

            if (isNeedSelectGlossary)
            {
                // Открыть список с глоссариями
                SuggestTermDialog.OpenGlossaryList();
                // Выбрать наш первый созданный глоссарий в выпавшем списке
                SuggestTermDialog.SelectGlossary(glossaryName);
            }

            // Сохранить
            SuggestTermDialog.ClickSave();
            // TODO убрать sleep
            Thread.Sleep(500);
        }

        /// <summary>
        /// Заполнить термины
        /// </summary>
        /// <param name="suggestTermFirst">текст первого термина</param>
        /// <param name="suggestTermSecond">текст второго термина</param>
        protected void SuggestTermFillTerms(string suggestTermFirst, string suggestTermSecond)
        {
            // Нажать Предложить термин
            GlossaryListPage.ClickAddSuggest();
            SuggestTermDialog.WaitPageLoad();
            // Заполнить термин
            SuggestTermDialog.FillTerm(1, suggestTermFirst);
            SuggestTermDialog.FillTerm(2, suggestTermSecond);
        }

        /// <summary>
        /// Заполнить термин в расширенной форме
        /// </summary>
        protected void FillNewItemExtended()
        {
            GlossaryPage.FillItemTermsExtended("Example Term Text " + DateTime.Now.ToString());
        }

        /// <summary>
        /// Изменить все термины в расширенной версии
        /// </summary>
        /// <param name="text">текст</param>
        protected void EditAllExtendedItems(string text)
        {
            GlossaryPage.EditTermsExtended(text);
        }

        /// <summary>
        /// Удаление глоссария
        /// </summary>
        protected void DeleteGlossary()
        {
            // Открыть редактирование свойств глоссария
            OpenGlossaryProperties();
            // Нажать Удалить глоссарий 
            GlossaryEditForm.ClickDeleteGlossary();

            // Нажать Да (удалить)
            GlossaryEditForm.ClickConfirmDeleteGlossary();
            GlossaryListPage.WaitPageLoad();
        }

        /// <summary>
        /// Открыть свойства глоссария
        /// </summary>
        protected void OpenGlossaryProperties()
        {
            // Нажать Редактирование
            GlossaryPage.OpenEditGlossaryList();
            // Нажать на Properties
            GlossaryPage.ClickOpenProperties();
        }

        protected void AddUserRights()
        {
            // хардкор!
            // Проверить, что есть кнопка Предложить термин на странице со списком глоссариев
            if (GlossaryListPage.GetIsAddSuggestExist())
            {
                Assert.Pass("Все хорошо - у пользователя права есть!");
            }
            // Иначе: добавляем права

            // Перейти в пользователи и права
			WorkspacePage.ClickUsersAndRightsBtn();
			// Ожидание открытия страницы
			Assert.IsTrue(UserRightsPage.WaitUntilUsersRightsDisplay(), "Ошибка: Страница прав пользователя не открылась.");
            // Перейти в группы
            UserRightsPage.OpenGroups();
            // Выбрать Administrators
            UserRightsPage.SelectAdmins();
            // Edit
            UserRightsPage.ClickEdit();
            // Add right
            UserRightsPage.ClickAddRights();
            // Suggest concepts without glossary
            UserRightsPage.SelectSuggestWithoutGlossary();
            // Next
            UserRightsPage.ClickNext();
            // Add
            UserRightsPage.ClickAdd();
            Thread.Sleep(1000);
            // Add right
            UserRightsPage.ClickAddRights();
            // Suggest concepts without glossary
            UserRightsPage.SelectGlossarySearch();
            // Next
            UserRightsPage.ClickNext();
            // All Glossaries
            UserRightsPage.SelectAllGlossaries();
            // Next
            UserRightsPage.ClickNext();
            // Add
            UserRightsPage.ClickAdd();
            Thread.Sleep(1000);
            // Save
            UserRightsPage.ClickSave();
            Thread.Sleep(1000);
            SwitchGlossaryTab();
            Assert.IsTrue(GlossaryListPage.GetIsAddSuggestExist(), "ОШИБКА! Права не добавились.");
        }

        // TODO TODO УБРААААТЬ!!!!!!
        /*protected string GetSuggestTermRowsXPath()
        {
            return ".//tr[contains(@class, 'js-suggest-row')]/td[contains(@class, 'js-glossary-cell')]//p";
        }*/

    }
}
