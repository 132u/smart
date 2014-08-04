using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    /// <summary>
    /// Группа тестов для общей проверки глоссриев
    /// </summary>
	public class GlossaryMainTest : GlossaryTest
    {
        /// <summary>
        /// Конструктор теста
        /// </summary>
        /// <param name="url">Адрес</param>
        /// <param name="workspaceUrl">Адрес workspace</param>
        /// <param name="browserName">Название браузера</param>
		public GlossaryMainTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

		/// <summary>
		/// Начальная подготовка для каждого теста
		/// </summary>
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Метод тестирования создания Глоссария
        /// </summary>
        [Test]
        public void CreateGlossaryTest()
        {
            // Создать новый глоссарий
            string glossaryName = CreateGlossaryAndReturnToGlossaryList();

            // Проверить, что глоссарий сохранился
            Assert.IsTrue(GetIsExistGlossary(glossaryName), "Ошибка: глоссарий не создался");
        }

        /// <summary>
        /// Метод тестирования создания Глоссария без имени
        /// </summary>
        [Test]
        public void CreateGlossaryWithoutNameTest()
        {
            // Открыть форму создания глоссария
            OpenCreateGlossary();
            // Нажать сохранить
            GlossaryEditForm.ClickSaveGlossary();

            // Проверить, что поле Имя отмечено ошибкой
            Assert.IsTrue(GlossaryEditForm.GetIsExistGlossaryNameError(),
                "Ошибка: поле имя не отмечено ошибкой");
            // Проверить, что появилось сообщение о пустом имени
            Assert.IsTrue(GlossaryEditForm.GetIsExistErrorMessageEmptyGlossaryName(),
                "Ошибка: не появилось сообщение о пустом имени");
        }

        /// <summary>
        /// Метод тестирования создания Глоссария с существующим именем
        /// </summary>
        [Test]
        public void CreateGlossaryWithExistingNameTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Перейти в список глоссариев
            SwitchGlossaryTab();

            // Создать глоссарий с этим же именем
            CreateGlossaryByName(glossaryName, false);
            // Проверить, что появилось сообщение о существующем имени
            Assert.IsTrue(GlossaryEditForm.GetIsExistErrorMessageExistGlossaryName(),
                "Ошибка: не появилось сообщение о существующем имени");
        }

        /// <summary>
        /// Метод: проверка отсутствия уже выбранного языка в выпадающем списке при выборе нового языка (при создании глоссария)
        /// </summary>
        [Test]
        public void CheckAbsenceSelectedLanguageAmongAvailableTest()
        {
            // Открыть форму создания глоссария
            OpenCreateGlossary();

            // Добавить язык (de)
            GlossaryEditForm.ClickAddLanguage();
            GlossaryEditForm.ClickLastLangOpenCloseList();
            GlossaryEditForm.SelectLanguage(CommonHelper.LANGUAGE.German);

            // Нажать кнопку добавления языка
            GlossaryEditForm.ClickAddLanguage();
            GlossaryEditForm.ClickLastLangOpenCloseList();

            // Проверить, что языка нет в списке для добавления языка
            Assert.IsTrue(!GlossaryEditForm.GetIsExistLanguageInLangList(CommonHelper.LANGUAGE.German), "Ошибка: уже выбранный язык остался в списке для добавления");
        }

        /// <summary>
        /// Метод тестирования удаления языка при добавлении глоссария
        /// </summary>
        [Test]
        public void DeleteLanguageCreateGlossaryTest()
        {
            // Открыть форму создания глоссария
            OpenCreateGlossary();

            // Получить количество выбранных языков
            int selectedLangCountBefore = GlossaryEditForm.GetGlossaryLanguageCount();
            // Удалить язык
            GlossaryEditForm.ClickDeleteLanguage();
            // Получить количество выбранных языков
            int selectedLangCountAfter = GlossaryEditForm.GetGlossaryLanguageCount();
            // Проверить, что количество уменьшилось
            Assert.IsTrue(selectedLangCountAfter < selectedLangCountBefore, "Ошибка: количество языков не уменьшилось!");
        }

        /// <summary>
        /// Метод проверки даты создания при добавлении глоссария
        /// </summary>
        [Test]
        public void CheckDateCreatedGlossaryTest()
        {
            // Получить текущую дату
            DateTime todayDate = DateTime.Now;
            // Создать новый глоссарий
            string glossaryName = CreateGlossaryAndReturnToGlossaryList();

            // Сравнить дату создания с текущей датой
            Assert.IsTrue(GetIsDateEqualCurrentDayOrToday(GlossaryListPage.GetGlossaryDateModified(glossaryName), todayDate),
                "Ошибка: дата не совпадает");
        }

        /// <summary>
        /// Метод проверки даты и времени изменения глоссария
        /// </summary>
        [Test]
        public void CheckDateModifyGlossaryTest()
        {
            // Создать новый глоссарий
            string glossaryName = CreateGlossaryAndReturnToGlossaryList();
            // Получить дату и время создания глоссария
            string dateModifiedBefore = GlossaryListPage.GetGlossaryDateModified(glossaryName);

            // Перейти в глоссарий
            SwitchCurrentGlossary(glossaryName);
            // Поставить задержку минуту, чтобы дата изменения изменилась (точность даты и времени до минуты)
            Thread.Sleep(60000);
            // Создать термин (изменение глоссария)
            CreateItemAndSave();
            Assert.IsTrue(GlossaryPage.WaitConceptGeneralSave(), "Ошибка: термин не сохранился");
            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Получить дату и время изменения глоссария
            string dateModifiedAfter = GlossaryListPage.GetGlossaryDateModified(glossaryName);

            // Сравнить дату с предыдущей датой
            Assert.IsTrue(dateModifiedBefore != dateModifiedAfter, "Ошибка: дата изменения глоссария не изменилась");
        }

        /// <summary>
        /// Метод проверки автора при добавлении глоссария
        /// </summary>
        [Test]
        public void CheckAuthorCreatedGlossaryTest()
        {
            // Получить имя пользователя из профиля
            string userName = GetUserNameProfile();

            // Создать глоссарий
            string glossaryName = CreateGlossaryAndReturnToGlossaryList();

            // Получить имя автора глоссария
            string authorName = GlossaryListPage.GetGlossaryAuthor(glossaryName);
            // Проверить, что текущий пользователь и есть автор
            Assert.AreEqual(userName, authorName, "Ошибка: автор нового глоссария - не текущий пользователь");
        }

        /// <summary>
        /// Метод тестирования удаления Глоссария
        /// </summary>
        [Test]
        public void DeleteGlossaryTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Открыть редактирование свойств глоссария
            OpenGlossaryProperties();
            // Нажать Удалить глоссарий 
            GlossaryEditForm.ClickDeleteGlossary();

            // Проверить, что появилось предупреждение об удалении глоссария
            Assert.IsTrue(GlossaryEditForm.GetIsExistWarningDeleteGlossary(),
                "Ошибка: не появилось предупреждение об удалении глоссария");

            // Нажать Да (удалить)
            GlossaryEditForm.ClickConfirmDeleteGlossary();

            // Проверить, что глоссария нет
            Assert.IsTrue(!GetIsExistGlossary(glossaryName), "Ошибка: глоссарий не удалился");
        }

        /// <summary>
        /// Метод тестирования удаления языка, когда уже есть термин на этом языке
        /// </summary>
        [Test]
        public void DeleteLanguageExistTermTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Создать термин
            CreateItemAndSave();

            // Открыть редактирование свойств глоссария
            OpenGlossaryProperties();
            // Получить количество языков
            int availLanguageCountBefore = GlossaryEditForm.GetGlossaryLanguageCount();
            // Удалить язык
            GlossaryEditForm.ClickDeleteLanguage();

            // Проверить, появилось ли предупреждение об удалении языка на котором уже есть термин
            Assert.IsTrue(GlossaryEditForm.GetIsExistWarningDeleteLanguage(),
                "Ошибка: не появилось предупреждение, что есть термин на удаленном языке");
            // Отменить удаление
            GlossaryEditForm.CancelDeleteLanguage();

            // Получить количество языков
            int availLanguageCountAfter = GlossaryEditForm.GetGlossaryLanguageCount();
            // Сравнить количество языков
            Assert.AreEqual(availLanguageCountBefore, availLanguageCountAfter, "Ошибка: количество языков разное (должно быть одинаковое, т.к. удаление отменили)");
        }

        /// <summary>
        /// Метод тестирования изменения структуры
        /// </summary>
        [Test]
        public void EditGlossaryStructureTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Добавить элемент в структуре глоссария
            EditGlossaryStructureAddField();

            // Нажать New item
            GlossaryPage.ClickNewItemBtn();

            // Проверить, что появилось поле добавления нового термина в расширенном виде
            Assert.IsTrue(GlossaryPage.GetIsExistNewItemExtendedMode(),
                "Ошибка: не появилось расширенного режима добавления термина");
        }

        /// <summary>
        /// Метод тестирования импорта глоссария с добавлением терминов
        /// </summary>
        [Test]
        public void ImportGlossaryAddConceptsTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Открыть Импорт, указать документ для импорта
            FillImportGlossaryForm();
            // Нажать Импорт
            GlossaryPage.ClickImportFormImportBtn();
            // Дождаться закрытия формы импорта
            GlossaryPage.WaitUntilImportFormDisappear();
            // Нажать Закрыть в сообщении об успешном добавлении
            GlossaryPage.ClickCloseSuccessResult();
            // TODO убрать sleep
            Thread.Sleep(1000);
            // Проверить, что количество терминов больше нуля
            Assert.IsTrue(GetCountOfItems() > 0, "Ошибка: количество терминов должно быть больше нуля");
        }

        /// <summary>
        /// Метод тестирования импорта глоссария с заменой всех терминов
        /// </summary>
        [Test]
        public void ImportGlossaryReplaceAllConceptsTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Создать термины
            CreateItemAndSave();
            CreateItemAndSave();
            Assert.IsTrue(GetCountOfItems() == 2, "Ошибка: должно сохраниться 2 термина");

            // Открыть Импорт, указать документ для импорта
            FillImportGlossaryForm();
            // Указать "Заменить все термины"
            GlossaryPage.ClickReplaceAll();
            // Нажать Импорт
            GlossaryPage.ClickImportFormImportBtn();
            // Дождаться закрытия формы импорта
            GlossaryPage.WaitUntilImportFormDisappear();
            // Нажать Закрыть в сообщении об успешном добавлении
            GlossaryPage.ClickCloseSuccessResult();
            // TODO убрать sleep
            Thread.Sleep(1000);
            // Проверить, что количество терминов изменилось (количество терминов должно быть равно 1 для этого импортируемого файла)
            Assert.IsTrue(GetCountOfItems() == 1, "Ошибка: количество терминов должно быть равно 1");
        }

        /// <summary>
        /// Метод тестирования Экспорта глоссария
        /// </summary>
        [Test]
        public void ExportGlossaryTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Создать термины
            CreateItemAndSave();
            CreateItemAndSave();

            // Создать уникальное название для экспортируемого файла
            string uniqueGlossaryName = GlossaryName + DateTime.UtcNow.Ticks.ToString();
            string resultPath = System.IO.Path.Combine(PathTestResults, "GlossaryExportTest");
            // Создать папку для экспорта
            System.IO.Directory.CreateDirectory(resultPath);
            uniqueGlossaryName = System.IO.Path.Combine(resultPath, uniqueGlossaryName);

            // Нажать Экспорт
            GlossaryPage.ClickExportBtn();
            // Сохранить документ
            ExternalDialogSelectSaveDocument(uniqueGlossaryName);

            // Проверить, экспортировался ли файл
            Assert.IsTrue(System.IO.File.Exists(uniqueGlossaryName + ".xlsx"), "Ошибка: файл не экспортировался");
        }

        /// <summary>
        /// Метод тестирования изменения названия глоссария
        /// </summary>
        [Test]
        public void ChangeGlossaryNameTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Получить уникальное имя для глоссария
            string uniqueGlossaryName = GetUniqueGlossaryName();

            // Открыть редактирование свойств глоссария
            OpenGlossaryProperties();
            // Изменить имя глоссария и сохранить
            ChangeGlossaryNameToCurrent(uniqueGlossaryName);
            // Перейти к списку глоссариев
            SwitchGlossaryTab();

            // Проверить, что нет глоссария со старым именем
            Assert.IsTrue(!GetIsExistGlossary(glossaryName), "Ошибка: старое имя глоссария не удалилось");
            // Проверить, что появился глоссарий с новым именем
            Assert.IsTrue(GetIsExistGlossary(uniqueGlossaryName), "Ошибка: новое имя глоссария не появилось");
        }

        /// <summary>
        /// Метод тестирования изменения названия глоссария на существующее
        /// </summary>
        [Test]
        public void ChangeGlossaryExistingNameTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Создать другой глоссарий
            string secondGlossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(secondGlossaryName);

            // Открыть редактирование свойств глоссария
            OpenGlossaryProperties();
            // Изменить имя глоссария и сохранить
            ChangeGlossaryNameToCurrent(glossaryName);

            // Проверить, что появилось сообщение о существующем имени
            Assert.IsTrue(GlossaryEditForm.GetIsExistErrorMessageExistGlossaryName(),
                "Ошибка: не появилось сообщение о существующем имени");
        }

        /// <summary>
        /// Метод тестирования изменения названия глоссария на пустое
        /// </summary>
        [Test]
        public void ChangeGlossaryEmptyNameTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Открыть редактирование свойств глоссария
            OpenGlossaryProperties();
            // Ввести имя
            ChangeGlossaryNameToCurrent("");

            // Проверить, что поле Имя отмечено ошибкой
            Assert.IsTrue(GlossaryEditForm.GetIsExistGlossaryNameError(),
                "Ошибка: не появилось сообщение о пустом имени");

            // Проверить, что появилось сообщение о пустом имени
            Assert.IsTrue(GlossaryEditForm.GetIsExistErrorMessageEmptyGlossaryName(),
                "Ошибка: не появилось сообщение о пустом имени");
        }

        /// <summary>
        /// Метод тестирования изменения названия глоссария на пробельное
        /// </summary>
        [Test]
        public void ChangeGlossarySpaceNameTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Открыть редактирование свойств глоссария
            OpenGlossaryProperties();
            // Изменить имя глоссария и сохранить
            ChangeGlossaryNameToCurrent(" ");

            // Проверить, что поле Имя отмечено ошибкой
            Assert.IsTrue(GlossaryEditForm.GetIsExistGlossaryNameError(),
                "Ошибка: не появилось сообщение о пустом имени");

            // Проверить, что появилось сообщение о пустом имени
            Assert.IsTrue(GlossaryEditForm.GetIsExistErrorMessageEmptyGlossaryName(),
                "Ошибка: не появилось сообщение о пустом имени");
        }

        /// <summary>
        /// Тест: Открыть редактор структуры глоссария из редактора свойств
        /// </summary>
        [Test]
        public void OpenStructureFromPropertiesTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Открыть редактирование свойств глоссария
            OpenGlossaryProperties();

            // Нажать Изменить структуру
            GlossaryEditForm.ClickSaveAndEditStructureBtn();
            GlossaryEditForm.WaitPageClose();

            // Проверить, что открылся редактор структуры
            Assert.IsTrue(GlossaryPage.WaitPageLoad(),
                "Ошибка: редактор структуры не открылся");
        }

        /// <summary>
        /// Тест: создать глоссарий с несколькими языками
        /// </summary>
        [Test]
        public void CreateMultiLanguageGlossary()
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
            SwitchGlossaryTab();

            // TODO убрать sleep
            Thread.Sleep(3000);
            // Проверить, что глоссарий добавился в список
            Assert.IsTrue(GetIsExistGlossary(glossaryName), "Ошибка: глоссарий не добавился" + glossaryName);

            // Зайти в глоссарий
            SwitchCurrentGlossary(glossaryName);
            // Удалить глоссарий
            DeleteGlossary();
        }



        /// <summary>
        /// Изменить имя глоссария
        /// </summary>
        /// <param name="glossaryName">новое название</param>
        protected void ChangeGlossaryNameToCurrent(string glossaryName)
        {
            // Ввести имя глоссария
            GlossaryEditForm.EnterGlossaryName(glossaryName);
            // Сохранить
            GlossaryEditForm.ClickSaveGlossary();
            // TODO убрать sleep
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Заполнить форму импорт глоссария
        /// </summary>
        protected void FillImportGlossaryForm()
        {
            // Нажать Импорт
            GlossaryPage.ClickImportBtn();
            // Дождаться открытия формы
            GlossaryPage.WaitImportForm();
            // Нажать на Add для появления диалога загрузки документа
            GlossaryPage.ClickUploadBtn();
            // Заполнить форму загрузки документа
            FillAddDocumentForm(ImportGlossaryFile);
        }

        /// <summary>
        /// Вернуть, равна ли дата переданной дате или равна дате-сегодня (переданная строка содержит дату, которая равна переданной дате или сегодня)
        /// </summary>
        /// <param name="dateTimeString">строка с датой</param>
        /// <param name="curDay">дата для сравнения</param>
        /// <returns>Равна</returns>
        protected bool GetIsDateEqualCurrentDayOrToday(string dateTimeString, DateTime curDay)
        {
            // Формат dateTimeString: M(M)/D(D)/YYYY H(H):M(M) AM
            int beginIndex = 0;
            int splitIndex = dateTimeString.IndexOf("/");
            // Месяц
            string month = dateTimeString.Substring(beginIndex, splitIndex - beginIndex);

            beginIndex = splitIndex + 1;
            splitIndex = dateTimeString.IndexOf("/", beginIndex);
            // День
            string day = dateTimeString.Substring(beginIndex, splitIndex - beginIndex);

            beginIndex = splitIndex + 1;
            splitIndex = dateTimeString.IndexOf(" ", beginIndex);
            // Год
            string year = dateTimeString.Substring(beginIndex, splitIndex - beginIndex);

            // Создать дату в стандартном формате
            DateTime resDate = DateTime.Parse(day + "." + month + "." + year);

            // Сравнить с текущей датой или с сегодня (если тест проходит в полночь)
            return curDay.Date == resDate || DateTime.Today.Date == resDate;
        }

        /// <summary>
        /// Получить имя пользователя из профиля
        /// </summary>
        /// <returns>имя</returns>
        protected string GetUserNameProfile()
        {
            string userName = "";
            // Нажать на Профиль
            MainHelperClass.OpenProfile();
            // Дождаться открытия окна с профилем
            MainHelperClass.WaitProfileOpen();
            // Получить имя пользователя
            userName = MainHelperClass.GetUserNameProfile();
            // Закрыть профиль
            MainHelperClass.CloseProfile();

            return userName;
        }
    }
}
