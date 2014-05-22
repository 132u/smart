using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class GlossaryMainTest : GlossaryTest
    {
        public GlossaryMainTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

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
            Driver.FindElement(By.XPath(
               ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[contains(@class,'js-save')]")).Click();

            // Проверить, что поле Имя отмечено ошибкой
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//input[contains(@class,'js-glossary-name error')]")).Displayed,
                "Ошибка: поле имя не отмечено ошибкой");
            // Проверить, что появилось сообщение о пустом имени
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//p[contains(@class,'js-error-glossary-name-required')]")).Displayed,
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
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//p[contains(@class,'js-error-glossary-exists')]")).Displayed,
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
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[contains(@class,'js-add-language-button')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-glossary-language')][3]//span[contains(@class,'js-dropdown')]"))).Click();
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list glossary')]//span[@data-id = '7']"))).Click();

            // Получить список выбранных языков
            List<string> languages = new List<string>();
            IList<IWebElement> selectedLanguages = Driver.FindElements(By.XPath(
                ".//div[contains(@class, 'js-glossary-languages')]//span[contains(@class, 'js-glossary-language')]//span[contains(@class, 'js-dropdown__text')]"));
            foreach (IWebElement el in selectedLanguages)
            {
                languages.Add(el.GetAttribute("data-id"));
            }

            // Нажать кнопку добавления языка
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[contains(@class,'js-add-language-button')]")).Click();
            // Нажать для открытия списка доступных языков (нажать на 4й только что добавленный язык)
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-glossary-language')][4]//span[contains(@class,'js-dropdown')]"))).Click();

            // Получить список доступных языков
            List<string> availableLanguages = new List<string>();
            IList<IWebElement> availableLangElements = Driver.FindElements(By.XPath(
                ".//span[contains(@class, 'js-dropdown__list')]//span[contains(@class, 'js-dropdown__item glossary')]"));
            foreach (IWebElement el in availableLangElements)
            {
                availableLanguages.Add(el.GetAttribute("data-id"));
            }

            // Проверить, есть ли выбранные языки в списке доступных языков
            bool isAvailableSelectedLang = false;
            foreach (string el in languages)
            {
                if (availableLanguages.Contains(el))
                {
                    isAvailableSelectedLang = true;
                    break;
                }
            }
            Assert.IsTrue(!isAvailableSelectedLang, "Ошибка: уже выбранный язык остался в списке для добавления");
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
            int selectedLangCountBefore = Driver.FindElements(By.XPath(
                ".//div[contains(@class, 'js-glossary-languages')]//span[contains(@class, 'js-glossary-language')]")).Count;
            // Удалить язык
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-glossary-languages')]//em[contains(@class, 'js-delete-language')]")).Click();
            // Получить количество выбранных языков
            int selectedLangCountAfter = Driver.FindElements(By.XPath(
                ".//div[contains(@class, 'js-glossary-languages')]//span[contains(@class, 'js-glossary-language')]")).Count;
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

            string xPath = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text() = '" + glossaryName + "']/../../td[8]";
            // Сравнить дату создания с текущей датой
            Assert.IsTrue(GetIsDateEqualCurrentDayOrToday(Driver.FindElement(By.XPath(xPath)).Text, todayDate),
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
            string xPath = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text() = '" + glossaryName + "']/../../td[8]";
            string dateModifiedBefore = Driver.FindElement(By.XPath(xPath)).Text;

            // Перейти в глоссарий
            SwitchCurrentGlossary(glossaryName);
            // Поставить задержку минуту, чтобы дата изменения изменилась (точность даты и времени до минуты)
            Thread.Sleep(60000);
            // Создать термин (изменение глоссария)
            CreateItemAndSave();
            Assert.IsTrue(WaitUntilDisappearElement(".//img[contains(@class,'save concept')]"), "Ошибка: термин не сохранился");
            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Получить дату и время изменения глоссария
            string dateModifiedAfter = Driver.FindElement(By.XPath(xPath)).Text;

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
            string xPath = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text() = '" + glossaryName + "']/../../td[9]/p";
            string authorName = Driver.FindElement(By.XPath(xPath)).Text;
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
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[contains(@class, 'js-delete')]"))).Click();

            // Проверить, что появилось предупреждение об удалении глоссария
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//p[contains(@class, 'js-error-confirm-delete-glossary')]")).Displayed,
                "Ошибка: не появилось предупреждение об удалении глоссария");

            // Нажать Да (удалить)
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//a[contains(@class, 'js-confirm-delete-glossary')]")).Click();

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
            int availLanguageCountBefore = Driver.FindElements(By.XPath(
                ".//div[contains(@class, 'js-glossary-languages')]//span[contains(@class, 'js-glossary-language')]")).Count;
            // Удалить язык
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-glossary-languages')]//em[contains(@class, 'js-delete-language')]")).Click();

            // Проверить, появилось ли предупреждение об удалении языка на котором уже есть термин
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-popup-edit-glossary')][2]//a[contains(@class, 'js-undo-delete-language')]")).Displayed,
                "Ошибка: не появилось предупреждение, что есть термин на удаленном языке");
            // Отменить удаление
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-edit-glossary')][2]//a[contains(@class, 'js-undo-delete-language')]")).Click();

            // Получить количество языков
            int availLanguageCountAfter = Driver.FindElements(By.XPath(
                ".//div[contains(@class, 'js-glossary-languages')]//span[contains(@class, 'js-glossary-language')]")).Count;
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
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-add-concept')]"))).Click();
            // Проверить, что появилось поле добавления нового термина в расширенном виде
            Assert.IsTrue(Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-panel')]")).Displayed,
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
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-import')][2]//span[contains(@class,'js-import-button')]")).Click();
            // Нажать Закрыть в сообщении об успешном добавлении
            WaitAndClickElement(".//div[contains(@class,'js-info-popup b-popup-info')]//span[contains(@class,'js-popup-close')]");
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
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-import')][2]//input[contains(@id,'needToClear')][@value='True']")).Click();
            // Нажать Импорт
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-import')][2]//span[contains(@class,'js-import-button')]")).Click();
            // Нажать Закрыть в сообщении об успешном добавлении
            WaitAndClickElement(".//a[contains(@class,'js-close-link')]");
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
            Driver.FindElement(By.XPath(".//a[contains(@href,'/Glossaries/Export')]")).Click();
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
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//p[contains(@class,'js-error-glossary-exists')]")).Displayed,
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
            // Очистить поле с именем глоссария
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-popup-edit-glossary')][2]//input[contains(@class,'js-glossary-name')]")).Clear();
            // Сохранить
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-popup-edit-glossary')][2]//span[contains(@class,'js-save')]")).Click();

            // Проверить, что поле Имя отмечено ошибкой
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//input[contains(@class,'js-glossary-name error')]")).Displayed,
                "Ошибка: не появилось сообщение о пустом имени");

            // Проверить, что появилось сообщение о пустом имени
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//p[contains(@class,'js-error-glossary-name-required')]")).Displayed,
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
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//input[contains(@class,'js-glossary-name error')]")).Displayed,
                "Ошибка: не появилось сообщение о пустом имени");

            // Проверить, что появилось сообщение о пустом имени
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//p[contains(@class,'js-error-glossary-name-required')]")).Displayed,
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
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//a[contains(@class,'js-save-and-edit-structure')]")).Click();
            WaitUntilDisappearElement(".//div[contains(@class,'js-popup-edit-glossary')][2]");

            // Проверить, что открылся редактор структуры
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-edit-structure')]")),
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
            List<int> langList = new List<int>();
            langList.Add(7);
            langList.Add(12);
            langList.Add(1041);
            langList.Add(1063);

            // Создать глоссарий
            CreateGlossaryMultiLanguage(glossaryName, langList);
            SwitchGlossaryTab();

            Thread.Sleep(3000);
            // Проверить, что глоссарий добавился в список
            Assert.IsTrue(GetIsExistGlossary(glossaryName), "Ошибка: глоссарий не добавился" + glossaryName);

            // Зайти в глоссарий
            SwitchCurrentGlossary(glossaryName);
            // Удалить глоссарий
            DeleteGlossary();
        }

        protected void ChangeGlossaryNameToCurrent(string glossaryName)
        {
            // Очистить поле с именем глоссария
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-popup-edit-glossary')][2]//input[contains(@class,'js-glossary-name')]")).Clear();
            // Ввести уникальное имя
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-popup-edit-glossary')][2]//input[contains(@class,'js-glossary-name')]")).SendKeys(glossaryName);
            // Сохранить
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-popup-edit-glossary')][2]//span[contains(@class,'js-save')]")).Click();
            Thread.Sleep(1000);
        }


        protected void FillImportGlossaryForm()
        {
            // Нажать Импорт
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-import-concepts')]")).Click();
            // Дождаться открытия формы
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-import')][2]")));
            // Нажать на Add для появления диалога загрузки документа
            Driver.FindElement(By.XPath(".//a[contains(@class,'js-upload-btn')]")).Click();
            // Заполнить форму загрузки документа
            FillAddDocumentForm(ImportGlossaryFile);
        }


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
    }
}
