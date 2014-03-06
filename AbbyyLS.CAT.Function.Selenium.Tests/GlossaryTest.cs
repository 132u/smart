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
            // Ответ формы
            Thread.Sleep(2000);

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
            CreateGlossaryByName(glossaryName);
            // Проверить, что появилось сообщение о существующем имени
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//p[contains(@class,'js-error-from-server')][contains(@data-key,'name')]")).Displayed,
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
            Thread.Sleep(1000);
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

            string xPath = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text() = '" + glossaryName + "']/../../td[7]";
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
            string xPath = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text() = '" + glossaryName + "']/../../td[7]";
            string dateModifiedBefore = Driver.FindElement(By.XPath(xPath)).Text;

            // Перейти в глоссарий
            SwitchCurrentGlossary(glossaryName);
            // Поставить задержку минуту, чтобы дата изменения изменилась (точность даты и времени до минуты)
            Thread.Sleep(60000);
            // Создать термин (изменение глоссария)
            CreateItemAndSave();

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Получить дату и время изменения глоссария
            string dateModifiedAfter = Driver.FindElement(By.XPath(xPath)).Text;

            // Сравнить дату создания с текущей датой
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
            string xPath = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text() = '" + glossaryName + "']/../../td[8]/p";
            string authorName = Driver.FindElement(By.XPath(xPath)).Text;
            // Проверить, что текущий пользователь и есть автор
            Assert.AreEqual(userName, authorName, "Ошибка: автор нового глоссария - не текущий пользователь");
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
            Thread.Sleep(2000);
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
            Thread.Sleep(2000);
            // Проверить, что появилось предупреждение
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//tr//td[contains(@class,'glossaryError')]")).Displayed,
                "Ошибка: должно появиться предупреждение о добавлении пустого термина");
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
            // Ответ формы
            Thread.Sleep(1000);

            // Проверить, что появилось предупреждение об удалении глоссария
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//p[contains(@class, 'js-error-confirm-delete-glossary')]")).Displayed,
                "Ошибка: не появилось предупреждение об удалении глоссария");

            // Нажать Да (удалить)
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//a[contains(@class, 'js-confirm-delete-glossary')]")).Click();
            // Закрытие формы
            Thread.Sleep(2000);

            // Проверить, что глоссария нет
            Assert.IsTrue(!GetIsExistGlossary(glossaryName), "Ошибка: глоссарий не удалился");
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
            Thread.Sleep(2000);

            // Проверить, появилось ли предупреждение об удалении языка на котором уже есть термин
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-popup-edit-glossary')][2]//a[contains(@class, 'js-undo-delete-language')]")).Displayed,
                "Ошибка: не появилось предупреждение, что есть термин на удаленном языке");
            // Отменить удаление
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-edit-glossary')][2]//a[contains(@class, 'js-undo-delete-language')]")).Click();
            Thread.Sleep(1000);

            // Получить количество языков
            int availLanguageCountAfter = Driver.FindElements(By.XPath(
                ".//div[contains(@class, 'js-glossary-languages')]//span[contains(@class, 'js-glossary-language')]")).Count;
            // Сравнить количество языков
            Assert.AreEqual(availLanguageCountBefore, availLanguageCountAfter, "Ошибка: количество языков разное (должно быть одинаковое, т.к. удаление отменили)");
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
            Thread.Sleep(2000);

            // Проверить, что количество терминов равно нулю
            Assert.IsTrue(GetCountOfItems() == 0, "Ошибка: количество терминов должно быть равно 0");
        }

        /// <summary>
        /// Метод тестирования предложения термина без указания глоссария со страницы со списком глоссариев
        /// </summary>
        [Test]
        public void SuggestTermWithoutGlossaryFromGlossaryListTest()
        {
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Получить количество терминов без указанного глоссария
            int unglossaryTermsCountBefore = GetCountSuggestTermsWithoutGlossary();

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Предложить термин
            CreateSuggestTerm();

            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Получить количество терминов без указанного глоссария
            int unglossaryTermsCountAfter = GetCountSuggestTermsWithoutGlossary();
            // Проверить, что таких терминов стало больше
            Assert.IsTrue(unglossaryTermsCountAfter > unglossaryTermsCountBefore, "Ошибка: предложенный термин не сохранился");
        }

        /// <summary>
        /// Метод тестирования предложения термина для глоссария со страницы со списком глоссариев
        /// </summary>
        [Test]
        public void SuggestTermWithGlossaryFromGlossaryListTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Предложить термин для этого глоссария
            SuggestTermSetGlossary(glossaryName);

            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Проверить, что терминов для этого глоссария больше нуля
            Assert.IsTrue(GetCountSuggestTermsGlossary(glossaryName) > 0, "Ошибка: нет предложенного термина для этого глоссария");
        }

        /// <summary>
        /// Метод тестирования предложения термина со страницы глоссария
        /// </summary>
        [Test]
        public void SuggestTermWithGlossaryFromGlossaryTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Предложить термин
            CreateSuggestTerm();

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти к предложенным терминам
            SwitchSuggestedTerms();
            // Проверить, что терминов для этого глоссария больше нуля
            Assert.IsTrue(GetCountSuggestTermsGlossary(glossaryName) > 0, "Ошибка: нет предложенного термина для этого глоссария");
        }

        /// <summary>
        /// Метод тестирования предложения термина со страницы другого глоссария
        /// </summary>
        [Test]
        public void SuggestTermWithGlossaryFromAnotherGlossaryTest()
        {
            // Создать один глоссарий
            string firstGlossaryName = CreateGlossaryAndReturnToGlossaryList();
            // Создать другой глоссарий
            string secondGlossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(secondGlossaryName);

            // Предложить термин для первого глоссария на странице второго глоссария
            SuggestTermSetGlossary(firstGlossaryName);

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Проверить, что терминов для первого глоссария больше нуля
            Assert.IsTrue(GetCountSuggestTermsGlossary(firstGlossaryName) > 0, "Ошибка: нет предложенного термина для первого глоссария");
        }

        /// <summary>
        /// Метод тестирования предложения термина со страницы глоссария без привязки к глоссарию
        /// </summary>
        [Test]
        public void SuggestTermWithoutGlossaryFromAnotherGlossaryTest()
        {
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Получить количество терминов без указанного глоссария
            int unglossaryTermsCountBefore = GetCountSuggestTermsWithoutGlossary();

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Создать  глоссарий
            CreateGlossaryByName(GetUniqueGlossaryName());

            // Предложить термин с отсутствием глоссария
            SuggestTermSetGlossary("");

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Получить количество терминов без указанного глоссария
            int unglossaryTermsCountAfter = GetCountSuggestTermsWithoutGlossary();
            // Проверить, что количество терминов без глоссария увеличилось
            Assert.IsTrue(unglossaryTermsCountAfter > unglossaryTermsCountBefore, "Ошибка: термин без указанного глоссария не сохранился");
        }

        /// <summary>
        /// Метод тестирования предложения существующего термина со страницы глоссария, проверка появления предупреждения
        /// </summary>
        [Test]
        public void SuggestExistingTermWarningFromGlossaryTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            string uniquePrefix = DateTime.Now.ToString();
            string SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
            string SuggestTerm2 = "Suggest Term 2" + uniquePrefix;
            // Создать термин
            CreateItemAndSave(SuggestTerm1, SuggestTerm2);
            Thread.Sleep(2000);
            // Предложить термин
            SuggestTermAndSave(SuggestTerm1, SuggestTerm2);
            Thread.Sleep(1000);
            // Проверить, что появилось предупреждение
            Assert.IsTrue(Driver.FindElement(By.XPath(".//div[contains(@class,'js-duplicate-warning')]")).Displayed,
                "Ошибка: не появилось предупреждение о существующем термине");
            // Нажать отмену
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-add-suggest-popup')]//a[contains(@class,'g-popupbox__cancel js-popup-close')]")).Click();
            // Перейти в предложенные термины
            SwitchSuggestTermCurrentGlossary();
            // Проверить, что нет предложенных терминов
            Assert.IsTrue(GetCountOfSuggestTerms() == 0, "Ошибка: предложенный термин сохранился");
        }

        /// <summary>
        /// Метод тестирования предложения существующего термина со страницы глоссария, одобрение
        /// </summary>
        [Test]
        public void SuggestExistingTermAcceptFromGlossaryTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            string uniquePrefix = DateTime.Now.ToString();
            string SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
            string SuggestTerm2 = "Suggest Term 2" + uniquePrefix;
            // Создать термин
            CreateItemAndSave(SuggestTerm1, SuggestTerm2);
            Thread.Sleep(2000);
            // Предложить термин
            SuggestTermAndSave(SuggestTerm1, SuggestTerm2);
            Thread.Sleep(2000);
            // Согласиться
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-add-suggest-popup')]//input[contains(@class,'js-save-btn')]")).Click();
            Thread.Sleep(3000);
            // Перейти в предложенные термины
            SwitchSuggestTermCurrentGlossary();
            // Проверить, что предложенный термин сохранился
            Assert.IsTrue(GetCountOfSuggestTerms() > 0, "Ошибка: предложенный термин не сохранился");
        }

        /// <summary>
        /// Метод тестирования предложения существующего термина из списка глоссариев, проверка появления предупреждения
        /// </summary>
        [Test]
        public void SuggestExistingTermWarningFromGlossaryListTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            string uniquePrefix = DateTime.Now.ToString();
            string SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
            string SuggestTerm2 = "Suggest Term 2" + uniquePrefix;
            // Создать термин
            CreateItemAndSave(SuggestTerm1, SuggestTerm2);
            Thread.Sleep(2000);

            // Перейти в список глоссариев
            SwitchGlossaryTab();

            // Предложить термин  с указанием глоссария
            SuggestTermAndSave(SuggestTerm1, SuggestTerm2, true, glossaryName);
            Thread.Sleep(1000);
            // Проверить, что появилось предупреждение
            Assert.IsTrue(Driver.FindElement(By.XPath(".//div[contains(@class,'js-duplicate-warning')]")).Displayed,
                "Ошибка: не появилось предупреждение о существующем термине");
            // Нажать отмену
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-add-suggest-popup')]//a[contains(@class,'g-popupbox__cancel js-popup-close')]")).Click();
            // Перейти в глоссарий
            SwitchCurrentGlossary(glossaryName);
            // Перейти в предложенные термины
            SwitchSuggestTermCurrentGlossary();
            // Проверить, что нет предложенных терминов
            Assert.IsTrue(GetCountOfSuggestTerms() == 0, "Ошибка: предложенный термин сохранился");
        }

        /// <summary>
        /// Метод тестирования предложения существующего термина из списка глоссариев, одобрение
        /// </summary>
        [Test]
        public void SuggestExistingTermAcceptFromGlossaryListTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            string uniquePrefix = DateTime.Now.ToString();
            string SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
            string SuggestTerm2 = "Suggest Term 2" + uniquePrefix;
            // Создать термин
            CreateItemAndSave(SuggestTerm1, SuggestTerm2);
            Thread.Sleep(2000);

            // Перейти в список глоссариев
            SwitchGlossaryTab();
            // Предложить термин  с указанием глоссария
            SuggestTermAndSave(SuggestTerm1, SuggestTerm2, true, glossaryName);
            Thread.Sleep(1000);

            // Согласиться
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-add-suggest-popup')]//input[contains(@class,'js-save-btn')]")).Click();
            Thread.Sleep(3000);
            // Перейти в глоссарий
            SwitchCurrentGlossary(glossaryName);
            // Перейти в предложенные термины
            SwitchSuggestTermCurrentGlossary();
            // Проверить, что предложенный термин сохранился
            Assert.IsTrue(GetCountOfSuggestTerms() > 0, "Ошибка: предложенный термин не сохранился");
        }

        /// <summary>
        /// Метод тестирования одобрения термина с указанным глоссарием со страницы со списком глоссариев
        /// </summary>
        [Test]
        public void AcceptSuggestedTermWithGlossaryFromGlossaryListTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Предложить термин
            CreateSuggestTerm();

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // В строке с термином для текущего глоссария нажать "Принять"
            ClickButtonSuggestTermRowByGlossary(glossaryName, "sugg js-accept-suggest");

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти в глоссарий
            SwitchCurrentGlossary(glossaryName);

            // Проверить количество терминов
            Assert.IsTrue(GetCountOfItems() > 0, "Ошибка: термин не добавился");
        }

        /// <summary>
        /// Метод тестирования одобрения термина с указанным глоссарием со страницы этого глоссария
        /// </summary>
        [Test]
        public void AcceptSuggestedTermWithGlossaryFromGlossaryTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Предложить термин
            CreateSuggestTerm();
            // Перейти в предложенные термины для этого глоссария
            SwitchSuggestTermCurrentGlossary();
            // Одобрить термин
            ClickButtonSuggestTermRow("sugg js-accept-suggest");
            // Перейти в этот глоссарий
            SwitchGlossaryFromSuggestedTerm();

            // Проверить количество терминов
            Assert.IsTrue(GetCountOfItems() > 0, "Ошибка: термин не добавился");
        }

        /// <summary>
        /// Метод тестирования одобрения термина с указанным глоссарием со страницы другого глоссария
        /// </summary>
        [Test]
        public void AcceptSuggestedTermWithGlossaryFromAnotherGlossaryTest()
        {
            // Создать глоссарий
            string glossaryNameWithSuggestTerm = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryNameWithSuggestTerm);
            // Предложить термин
            CreateSuggestTerm();

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Создать другой глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Перейти в предложенные термины для этого глоссария
            SwitchSuggestTermCurrentGlossary();

            // Открыть выпадающий список с выбором глоссария
            Driver.FindElement(By.XPath(".//span[contains(@class, 'js-dropdown sigggloss g-drpdwn g-iblock')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'js-dropdown__list sigggloss g-drpdwn__list')]")));
            // Выбрать в выпавшем списке наш первый созданный глоссарий
            string xPathGlossary = ".//span[contains(@class, 'js-dropdown__item sigggloss g-drpdwn__item')][@title='" + glossaryNameWithSuggestTerm + "']";
            Driver.FindElement(By.XPath(xPathGlossary)).Click();
            Thread.Sleep(2000);

            // Одобрить термин
            ClickButtonSuggestTermRow("sugg js-accept-suggest");

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти в наш первый глоссарий
            SwitchCurrentGlossary(glossaryNameWithSuggestTerm);

            // Проверить количество терминов
            Assert.IsTrue(GetCountOfItems() > 0, "Ошибка: термин не добавился");
        }

        /// <summary>
        /// Метод тестирования одобрения термина без глоссария со страницы со списком глоссариев
        /// </summary>
        [Test]
        public void AcceptSuggestedTermWithoutGlossaryFromGlossaryListTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Перейти к списку глоссариев
            SwitchGlossaryTab();

            // Предложить термин
            CreateSuggestTerm();
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();

            // Найти все термины
            IList<IWebElement> termList = Driver.FindElements(By.XPath(GetSuggestTermRowsXPath()));
            for (int i = 0; i < termList.Count; ++i)
            {
                // Найти термин без указанного глоссария (с пустым полем глоссария)
                if (termList[i].Text.Trim().Length == 0)
                {
                    // Одобрить этот термин
                    ClickButtonSuggestTermRow("sugg js-accept-suggest", true, (i + 1));
                    // Дождаться появления формы выбора глоссария
                    Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-select-glossary-popup')]//span[contains(@class, 'js-dropdown siggglossp')]"))).Click();
                    // Выбрать нужный глоссарий
                    string xPathGlossary = ".//span[contains(@class, 'js-dropdown__item siggglosspp')][@title='" + glossaryName + "']";
                    Wait.Until((d) => d.FindElement(By.XPath(xPathGlossary))).Click();
                    // Сохранить
                    Driver.FindElement(By.XPath(".//input[contains(@class, 'js-glossary-selected-button')]")).Click();
                    Thread.Sleep(2000);
                    break;
                }
            }

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти в глоссарий
            SwitchCurrentGlossary(glossaryName);

            // Проверить количество терминов
            Assert.IsTrue(GetCountOfItems() > 0, "Ошибка: термин не добавился");
        }

        /// <summary>
        /// Метод тестирования удаления предложенного термина без глоссария
        /// </summary>
        [Test]
        public void DeleteSuggestedTermWithoutGlossaryTest()
        {
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Получить количество терминов без указанного глоссария
            int unglossaryTermsCount = GetCountSuggestTermsWithoutGlossary();
            if (unglossaryTermsCount == 0)
            {
                // Перейти к списку глоссариев
                SwitchGlossaryTab();
                // Предложить термин
                CreateSuggestTerm();
                // Перейти к списку предложенных терминов
                SwitchSuggestedTerms();
                // Получить количество терминов без указанного глоссария
                unglossaryTermsCount = GetCountSuggestTermsWithoutGlossary();
            }

            // Найти все предложенные термины
            IList<IWebElement> termList = Driver.FindElements(By.XPath(GetSuggestTermRowsXPath()));
            for (int i = 0; i < termList.Count; ++i)
            {
                // Найти термин без указанного глоссария (с пустым полем глоссария)
                if (termList[i].Text.Trim().Length == 0)
                {
                    // Удалить термин
                    ClickButtonSuggestTermRow("js-reject-suggest", true, (i + 1));
                    break;
                }
            }
            // Проверить количество терминов без указанного глоссария
            int unglossaryTermsCountAfter = GetCountSuggestTermsWithoutGlossary();
            Assert.IsTrue(unglossaryTermsCountAfter < unglossaryTermsCount, "Ошибка: предложенный термин не удалился");
        }

        /// <summary>
        /// Метод тестирования удаления предложенного термина для глоссария
        /// </summary>
        [Test]
        public void DeleteSuggestedTermWithGlossaryTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Предложить термин
            CreateSuggestTerm();

            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Удалить термин
            ClickButtonSuggestTermRow("js-reject-suggest");

            // Проверить количество предложенных терминов глоссария
            int count = 0;
            IList<IWebElement> termList = Driver.FindElements(By.XPath("//tr[contains(@class, 'js-suggest-row')]"));
            for (int i = 0; i < termList.Count; ++i)
            {
                // Проверить, что термин не скрыт (удаленные термины становятся скрытыми до обновления страницы)
                if (!termList[i].GetAttribute("class").Contains("g-hidden"))
                {
                    ++count;
                }
            }

            // Проверить количество предложенных терминов
            Assert.IsTrue(count == 0, "Ошибка: термин не удалился");
        }

        /// <summary>
        /// Метод тестирования редактирования предложенного термина из глоссария
        /// </summary>
        [Test]
        public void EditSuggestedTermFromGlossaryTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Предложить термин
            CreateSuggestTerm();
            // Перейти к списку предложенных терминов
            SwitchSuggestTermCurrentGlossary();

            string newTermText = "New Term Text" + DateTime.UtcNow.ToString();
            // Расширить окно, чтобы кнопка была видна, иначе она недоступна для Selenium
            Driver.Manage().Window.Maximize();
            // Нажать на редактирование
            ClickButtonSuggestTermRow("js-edit-suggest");

            // Ввести в термин новое значение
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][1]//span[contains(@class,'js-term-editor')]//input")).Clear();
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][1]//span[contains(@class,'js-term-editor')]//input")).SendKeys(newTermText);
            // Перейти в другому языку
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-term-viewer')]")).Click();
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-term-editor')]//input")).Clear();
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-term-editor')]//input")).SendKeys(newTermText);
            Thread.Sleep(2000);
            // Принять термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            Thread.Sleep(5000);
            // Перейти в глоссарий
            SwitchGlossaryFromSuggestedTerm();

            // Проверить термин в глоссарии
            bool isTermAccepted = false;
            IList<IWebElement> termLangList = Driver.FindElements(By.XPath(".//tr[contains(@class, 'js-concept-row')]//td[contains(@class,'glossaryShort')][1]/p"));
            foreach (IWebElement el in termLangList)
            {
                if (el.Text.Trim() == newTermText)
                {
                    isTermAccepted = true;
                    break;
                }
            }
            Assert.IsTrue(termLangList.Count > 0, "Ошибка: термин не сохранился");
            Assert.IsTrue(isTermAccepted, "Ошибка: термин сохранился неотредактированным");
        }

        /// <summary>
        /// Метод тестирования редактирования предложенного термина без глоссария
        /// </summary>
        [Test]
        public void EditSuggestedTermWithoutGlossaryTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Перейти к списку глоссариев
            SwitchGlossaryTab();

            // Предложить термин
            CreateSuggestTerm();

            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();

            string newTermText = "New Term Text" + DateTime.UtcNow.ToString();
            int rowNumber = 0;

            IList<IWebElement> termList = Driver.FindElements(By.XPath(GetSuggestTermRowsXPath()));
            for (int i = 0; i < termList.Count; ++i)
            {
                // Если после удаления пробелов нет символов, значит, что глоссарий не указан
                if (termList[i].Text.Trim().Length == 0)
                {
                    rowNumber = i + 1;
                    break;
                }
            }

            if (rowNumber > 0)
            {
                // Расширить окно, чтобы кнопка была видна, иначе она недоступна для Selenium
                Driver.Manage().Window.Maximize();
                // Нажать на редактирование
                ClickButtonSuggestTermRow("js-edit-suggest", true, rowNumber);

                // Дождаться появления формы выбора глоссария
                Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-select-glossary-popup')]//span[contains(@class, 'js-dropdown siggglossp')]"))).Click();
                // Выбрать нужный глоссарий
                string xPathGlossary = ".//span[contains(@class, 'js-dropdown__item siggglosspp')][@title='" + glossaryName + "']";
                Wait.Until((d) => d.FindElement(By.XPath(xPathGlossary))).Click();
                // Сохранить
                Driver.FindElement(By.XPath(".//input[contains(@class, 'js-glossary-selected-button')]")).Click();
                Thread.Sleep(2000);

                // Ввести в термин новое значение
                Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][1]//span[contains(@class,'js-term-editor')]//input")).Clear();
                Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][1]//span[contains(@class,'js-term-editor')]//input")).SendKeys(newTermText);
                // Перейти в другому языку
                Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-term-viewer')]")).Click();
                Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-term-editor')]//input")).Clear();
                Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-term-editor')]//input")).SendKeys(newTermText);
                Thread.Sleep(2000);
                // Принять термин
                Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
                Thread.Sleep(5000);
                // Перейти к списку глоссариев
                SwitchGlossaryTab();
                // Перейти в глоссарий
                SwitchCurrentGlossary(glossaryName);

                // Проверить термин в глоссарии
                bool isTermAccepted = false;
                IList<IWebElement> termLangList = Driver.FindElements(By.XPath(".//tr[contains(@class, 'js-concept-row')]//td[contains(@class,'glossaryShort')][1]/p"));
                foreach (IWebElement el in termLangList)
                {
                    if (el.Text.Trim() == newTermText)
                    {
                        isTermAccepted = true;
                        break;
                    }
                }
                Assert.IsTrue(termLangList.Count > 0, "Ошибка: термин не сохранился");
                Assert.IsTrue(isTermAccepted, "Ошибка: термин сохранился неотредактированным");
            }
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
            /// Открыть Импорт, указать документ для импорта
            FillImportGlossaryForm();
            // Нажать Импорт
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-import')][2]//span[contains(@class,'js-import-button')]")).Click();
            Thread.Sleep(2000);
            // Нажать Закрыть в сообщении об успешном добавлении
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-info-popup b-popup-info')]//span[contains(@class,'js-popup-close')]")).Click();
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
            Thread.Sleep(2000);
            // Нажать Закрыть в сообщении об успешном добавлении
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-info-popup b-popup-info')]//span[contains(@class,'js-popup-close')]")).Click();
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
            Thread.Sleep(2000);
            // В открывшемся диалоге выбираем "Сохранить"
            SendKeys.SendWait(@"{DOWN}");
            Thread.Sleep(1000);
            SendKeys.SendWait(@"{Enter}");
            Thread.Sleep(2000);
            // Ввести адрес
            SendKeys.SendWait(uniqueGlossaryName);
            Thread.Sleep(1000);
            SendKeys.SendWait(@"{Enter}");
            Thread.Sleep(1000);

            // Проверить, экспортировался ли файл
            Assert.IsTrue(System.IO.File.Exists(uniqueGlossaryName + ".xlsx"), "Ошибка: файл не экспортировался");
        }

        /// <summary>
        /// Метод тестирования изменение названия глоссария
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
        /// Метод тестирования изменение названия глоссария на существующее
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
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//p[contains(@class,'js-error-from-server')]")).Displayed,
                "Ошибка: не появилось сообщение о существующем имени");
        }

        /// <summary>
        /// Метод тестирования изменение названия глоссария на пустое
        /// </summary>
        [Test]
        public void ChangeGlossaryEmptyNameTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Получить уникальное имя для глоссария
            string uniqueGlossaryName = GetUniqueGlossaryName();

            // Открыть редактирование свойств глоссария
            OpenGlossaryProperties();
            // Очистить поле с именем глоссария
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-popup-edit-glossary')][2]//input[contains(@class,'js-glossary-name')]")).Clear();
            Thread.Sleep(1000);
            // Сохранить
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-popup-edit-glossary')][2]//span[contains(@class,'js-save')]")).Click();
            Thread.Sleep(2000);

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
        /// Метод тестирования изменение названия глоссария на пробельное
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
        /// Метод тестирования изменения структуры - Definition/Interpretation
        /// </summary>
        [Test]
        public void EditGlossaryStructureInterpretationTest()
        {
            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryStructure();
            // Проверить поле Interpretation
            CheckEditGlossaryStructureTextarea("Interpretation");
        }

        /// <summary>
        /// Метод тестирования изменения структуры - Definition source/Interpretation source
        /// </summary>
        [Test]
        public void EditGlossaryStructureInterpretationSourceTest()
        {
            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryStructure();
            // Проверить поле InterpretationSource
            CheckEditGlossaryStructureTextarea("InterpretationSource");
        }

        /// <summary>
        /// Метод тестирования изменения структуры - Example
        /// </summary>
        [Test]
        public void EditGlossaryStructureExampleTest()
        {
            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryStructure();
            // Проверить поле Example
            CheckEditGlossaryStructureTextarea("Example");
        }

        /// <summary>
        /// Метод тестирования изменения структуры - Topic
        /// </summary>
        [Test]
        public void EditGlossaryStructureTopicTest()
        {
            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryStructure();

            string fieldName = "Topic";

            // Проверить, что поле появилось
            string elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//input[@name='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Enabled, "Ошибка: поле не появилось");

            // Нажать на поле
            string fieldXPath = elXPath + "/..//div[contains(@class,'ui-dropdown-treeview-wrapper')]/div[1]//span";
            Driver.FindElement(By.XPath(fieldXPath)).Click();
            // Проверить, что список открылся
            fieldXPath = ".//div[contains(@class,'l-corpr__viewmode__edit js-edit')]//div[contains(@class,'ui-dropdown-treeview_dropDown')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(fieldXPath)).Displayed, "Ошибка: список не открылся");

            // Выбрать Все
            fieldXPath += "//div[contains(@class,'ui-treeview_node  ui-treeview_rootNode ui-treeview_openedNode')]//div/span";
            Driver.FindElement(By.XPath(fieldXPath)).Click();
            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что значение в поле есть
            elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[@title='" + fieldName + "']/..//div";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.IsTrue(text.Length > 0, "Ошибка: значение не сохранилось");
        }

        /// <summary>
        /// Метод тестирования изменения структуры - Project/Domain
        /// </summary>
        [Test]
        public void EditGlossaryStructureDomainTest()
        {
            // Перейти на вкладку проектов
            SwitchProjectTab();

            // Проверить, есть ли проект с таким именем
            string projectName = "TestGlossaryEditStructureProject";
            CreateProjectIfNotExist(projectName);

            // Вернуться к глоссариям
            SwitchGlossaryTab();

            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryStructure();

            string fieldName = "Domain";

            // Проверить, что поле появилось
            string elXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__edit js-edit')]//select[@name='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Enabled, "Ошибка: поле не появилось");

            // Нажать на поле, чтобы открылся список
            string fieldXPath = elXPath + "/..//span[contains(@class,'js-dropdown')]";
            Driver.FindElement(By.XPath(fieldXPath)).Click();
            // Проверить, что список открылся
            Assert.IsTrue(Driver.FindElement(By.XPath(".//span[contains(@class,'js-dropdown__list')]")).Displayed, "Ошибка: список не открылся");

            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item')][contains(@title,'" + projectName + "')]")).Click();

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что значение в поле есть
            elXPath += "/../../div[contains(@class,'l-corpr__viewmode__view js-view')]//div[contains(@class,'js-value')]";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.AreEqual(text, projectName, "Ошибка: проект не сохранился в поле");

        }

        /// <summary>
        /// Метод тестирования изменения структуры - Image
        /// </summary>
        [Test]
        public void EditGlossaryStructureImageTest()
        {
            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryStructure();

            string fieldName = "Image";

            // Проверить, что поле появилось
            string elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//input[@name='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Enabled, "Ошибка: поле не появилось");
            // Нажать на поле, чтобы открылся диалог загрузки документа
            string fieldXPath = elXPath + "/..//div[contains(@class,'l-editgloss__imagebox')]//a";
            Driver.FindElement(By.XPath(fieldXPath)).Click();

            // Заполнить диалог загрузки изображения
            FillAddDocumentForm(ImageFile);
            Thread.Sleep(2000);
            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что изображение загрузилось
            elXPath += "/../../div[contains(@class,'l-editgloss__image')]//img[contains(@class,'l-editgloss__imageview')]";
            string srcValue = Driver.FindElement(By.XPath(elXPath)).GetAttribute("src");
            Assert.IsTrue(srcValue.Length > 0, "Ошибка: изображение не загрузилось");
        }

        /// <summary>
        /// Метод тестирования изменения структуры - Multimedia
        /// </summary>
        [Test]
        public void EditGlossaryStructureMultimediaTest()
        {
            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryStructure();

            string fieldName = "Multimedia";

            // Проверить, что поле появилось
            string elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//input[@name='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Enabled, "Ошибка: поле не появилось");
            // Нажать на поле, чтобы открылся диалог загрузки документа
            string fieldXPath = elXPath + "/..//span[contains(@class,'l-editgloss__linkbox')]//a";
            Driver.FindElement(By.XPath(fieldXPath)).Click();

            // Загружать видео или звук
            FillAddDocumentForm(AudioFile);

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что файл загрузился
            elXPath += "/../div[contains(@class,'l-editgloss__filemedia')]//a[contains(@class,'l-editgloss__filelink')]";
            string hrefValue = Driver.FindElement(By.XPath(elXPath)).GetAttribute("href");
            Assert.IsTrue(hrefValue.Length > 0, "Ошибка: файл не загрузился");
        }

        /// <summary>
        /// TODO Метод тестирования изменения структуры - уровень Language - на stage2 не работает уровень Language
        /// </summary>
        //[Test]
        public void EditGlossaryStructureLanguageLevelTest()
        {
            Assert.IsTrue(false, "На работает уровень Языков");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление пользовательского текстового поля
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomTextFieldTest()
        {
            // Создать глоссарий, изменить структуру, открыть добавление нового термина
            string fieldName = SetCustomFieldGlossaryStructure("Text");

            // Проверить, что поле появилось
            string elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(@class,'l-editgloss__name')][text()='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Ввести текст в поле
            string textareaXPath = elXPath + "/../textarea";
            string interpretationExample = fieldName + " Example";
            Driver.FindElement(By.XPath(textareaXPath)).SendKeys(interpretationExample);

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();

            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что текст в поле сохранился
            elXPath += "/../../div[contains(@class,'l-corpr__viewmode__view js-view')]//div";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.AreEqual(text, interpretationExample, "Ошибка: текст не сохранился");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского текстового поля
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomTextReaquiredFieldTest()
        {
            // Создать глоссарий, изменить структуру, открыть добавление нового термина
            string fieldName = SetCustomFieldGlossaryStructure("Text", true);

            // Проверить, что поле появилось
            string elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(@class,'l-editgloss__name')][text()='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();

            // Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым            
            Assert.IsTrue(
                Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit l-error')]//p[contains(@class,'l-editgloss__name')][text()='" + fieldName + "']")).Displayed,
                "Ошибка: обязательное поле не отмечено ошибкой");

            // Ввести текст в поле
            string textareaXPath = elXPath + "/../textarea";
            string interpretationExample = fieldName + " Example";
            Driver.FindElement(By.XPath(textareaXPath)).SendKeys(interpretationExample);
            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что текст в поле сохранился
            elXPath += "/../../div[contains(@class,'l-corpr__viewmode__view js-view')]//div";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.AreEqual(text, interpretationExample, "Ошибка: текст не сохранился");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление пользовательского поля Дата
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomDateFieldTest()
        {
            // Создать глоссарий, изменить структуру, открыть добавление нового термина
            string fieldName = SetCustomFieldGlossaryStructure("Date");

            // Проверить, что поле появилось
            string elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(@class,'l-editgloss__name')][text()='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");
            string textareaXPath = elXPath + "/../input[contains(@class,'hasDatepicker')]";

            // Кликнуть по полю
            Driver.FindElement(By.XPath(textareaXPath)).Click();
            // Проверить, что календарь открылся
            Assert.IsTrue(Driver.FindElement(By.XPath(".//table[contains(@class,'ui-datepicker-calendar')]")).Displayed, "Ошибка: календарь не появился");
            Driver.FindElement(By.XPath(".//table[contains(@class,'ui-datepicker-calendar')]//td[contains(@class,'ui-datepicker-today')]")).Click();

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что в термине поле не пустое
            elXPath += "/../../div[contains(@class,'l-corpr__viewmode__view js-view')]//div";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.IsTrue(text.Length > 0, "Ошибка: поле пустое");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Дата
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomDateReaquiredFieldTest()
        {
            // Создать глоссарий, изменить структуру, открыть добавление нового термина
            string fieldName = SetCustomFieldGlossaryStructure("Date", true);

            // Проверить, что поле появилось
            string elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(@class,'l-editgloss__name')][text()='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();

            // Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым            
            Assert.IsTrue(
                Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit l-error')]//p[contains(@class,'l-editgloss__name')][text()='" + fieldName + "']")).Displayed,
                "Ошибка: обязательное поле не отмечено ошибкой");

            // Кликнуть по полю
            string textareaXPath = elXPath + "/../input[contains(@class,'hasDatepicker')]";
            Driver.FindElement(By.XPath(textareaXPath)).Click();
            // Проверить, что календарь открылся
            Assert.IsTrue(Driver.FindElement(By.XPath(".//table[contains(@class,'ui-datepicker-calendar')]")).Displayed, "Ошибка: календарь не появился");
            // Выбрать текущую дату
            Driver.FindElement(By.XPath(".//table[contains(@class,'ui-datepicker-calendar')]//td[contains(@class,'ui-datepicker-today')]")).Click();

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что в термине поле не пустое
            elXPath += "/../../div[contains(@class,'l-corpr__viewmode__view js-view')]//div";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.IsTrue(text.Length > 0, "Ошибка: поле пустое");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление пользовательского поля Аудио/Видео
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomMediaFieldTest()
        {
            // Создать глоссарий, изменить структуру, открыть добавление нового термина
            string fieldName = SetCustomFieldGlossaryStructure("Media");

            // Проверить, что поле появилось
            string elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(@class,'l-editgloss_mediaName')][contains(text(),'" + fieldName + "')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Кликнуть по полю
            string textareaXPath = elXPath + "/..//span[contains(@class,'l-editgloss__linkbox')]//a[contains(@class,'js-upload-btn')]";
            Driver.FindElement(By.XPath(textareaXPath)).Click();
            // Загрузить документ
            FillAddDocumentForm(AudioFile);

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что файл загрузился
            elXPath += "/../div[contains(@class,'l-editgloss__filemedia')]//a[contains(@class,'l-editgloss__filelink')]";
            string hrefValue = Driver.FindElement(By.XPath(elXPath)).GetAttribute("href");
            Assert.IsTrue(hrefValue.Length > 0, "Ошибка: файл не загрузился");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Аудио/Видео
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomMediaReaquiredFieldTest()
        {
            // Создать глоссарий, изменить структуру, открыть добавление нового термина
            string fieldName = SetCustomFieldGlossaryStructure("Media", true);

            // Проверить, что поле появилось
            string elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(@class,'l-editgloss_mediaName')][contains(text(),'" + fieldName + "')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();

            // Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым            
            Assert.IsTrue(
                Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__view l-error')]//p[contains(@class,'l-editgloss_mediaName')][contains(text(),'" + fieldName + "')]")).Displayed,
                "Ошибка: обязательное поле не отмечено ошибкой");

            // Кликнуть по полю
            string textareaXPath = elXPath + "/..//span[contains(@class,'l-editgloss__linkbox')]//a[contains(@class,'js-upload-btn')]";
            Driver.FindElement(By.XPath(textareaXPath)).Click();
            FillAddDocumentForm(AudioFile);

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что файл загрузился
            elXPath += "/../div[contains(@class,'l-editgloss__filemedia')]//a[contains(@class,'l-editgloss__filelink')]";
            string hrefValue = Driver.FindElement(By.XPath(elXPath)).GetAttribute("href");
            Assert.IsTrue(hrefValue.Length > 0, "Ошибка: файл не загрузился");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление пользовательского поля Image
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomImageFieldTest()
        {
            // Создать глоссарий, изменить структуру, открыть добавление нового термина
            string fieldName = SetCustomFieldGlossaryStructure("Image");

            // Проверить, что поле появилось
            string elXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(text(),'" + fieldName + "')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Кликнуть по полю
            string textareaXPath = elXPath + "/..//div[contains(@class,'l-editgloss__imagebox')]//a";
            Driver.FindElement(By.XPath(textareaXPath)).Click();
            // Загрузить документ
            FillAddDocumentForm(ImageFile);

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что изображение загрузилось
            elXPath += "/../../div[contains(@class,'l-editgloss__image')]//img[contains(@class,'l-editgloss__imageview')]";
            string srcValue = Driver.FindElement(By.XPath(elXPath)).GetAttribute("src");
            Assert.IsTrue(srcValue.Length > 0, "Ошибка: изображение не загрузилось");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Изображение
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomImageReaquiredFieldTest()
        {
            // Создать глоссарий, изменить структуру, открыть добавление нового термина
            string fieldName = SetCustomFieldGlossaryStructure("Image", true);

            // Проверить, что поле появилось
            string elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(@class,'l-corpr__viewmode__text')][text()='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();

            // Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым            
            Assert.IsTrue(
                Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-error')]//p[contains(@class,'l-corpr__viewmode__text')][text()='" + fieldName + "']")).Displayed,
                "Ошибка: обязательное поле не отмечено ошибкой");

            // Кликнуть по полю
            string textareaXPath = elXPath + "/..//div[contains(@class,'l-editgloss__imagebox')]//a";
            Driver.FindElement(By.XPath(textareaXPath)).Click();
            // Загрузить документ
            FillAddDocumentForm(ImageFile);

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что изображение загрузилось
            elXPath += "/../../div[contains(@class,'l-editgloss__image')]//img[contains(@class,'l-editgloss__imageview')]";
            string srcValue = Driver.FindElement(By.XPath(elXPath)).GetAttribute("src");
            Assert.IsTrue(srcValue.Length > 0, "Ошибка: изображение не загрузилось");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление пользовательского поля Список
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomListFieldTest()
        {
            // Создать глоссарий, изменить структуру с добавлением списка, открыть добавление нового термина
            List<string> choiceList = new List<string>();
            choiceList.Add("select1");
            choiceList.Add("select2");
            string fieldName = SetCustomGlossaryStructureAddList("Choice", choiceList);

            // Проверить, что поле появилось
            string elXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__edit')]//p[text()='" + fieldName + "']";
            Thread.Sleep(1000);
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Кликнуть по полю
            string textareaXPath = elXPath + "/..//span[contains(@class,'js-dropdown')]";
            Driver.FindElement(By.XPath(textareaXPath)).Click();
            // Выбрать элемент в списке
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item')][@title='" + choiceList[0] + "']")).Click();
            Thread.Sleep(1000);
            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что в термине сохранился выбранный элемент
            elXPath += "/../../div[contains(@class,'l-corpr__viewmode__view js-view')]//div";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.AreEqual(text, choiceList[0], "Ошибка: в термине не сохранился выбранный элемент");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Список
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomListReaquiredFieldTest()
        {
            // Создать глоссарий, изменить структуру с добавлением списка, открыть добавление нового термина
            List<string> choiceList = new List<string>();
            choiceList.Add("select1");
            choiceList.Add("select2");
            string fieldName = SetCustomGlossaryStructureAddList("Choice", choiceList, true);

            // Проверить, что поле появилось
            string elXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__edit')]//p[text()='" + fieldName + "']";
            Thread.Sleep(1000);
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();

            // Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым            
            Assert.IsTrue(
                Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit l-error')]//p[contains(@class,'l-editgloss__name')][text()='" + fieldName + "']")).Displayed,
                "Ошибка: обязательное поле не отмечено ошибкой");

            // Кликнуть по полю
            string textareaXPath = elXPath + "/..//span[contains(@class,'js-dropdown')]";
            Driver.FindElement(By.XPath(textareaXPath)).Click();
            // Выбрать элемент из списка
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item')][@title='" + choiceList[0] + "']")).Click();
            Thread.Sleep(1000);
            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что в термине сохранился выбранный элемент
            elXPath += "/../../div[contains(@class,'l-corpr__viewmode__view js-view')]//div";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.AreEqual(text, choiceList[0], "Ошибка: в термине не сохранился выбранный элемент");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление пользовательского поля Число
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomNumberFieldTest()
        {
            // Создать глоссарий, изменить структуру, открыть добавление нового термина
            string fieldName = SetCustomFieldGlossaryStructure("Number", false, true, "0");

            // Проверить, что поле появилось
            string elXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__edit')]//p[text()='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Ввести в поле текст
            string textareaXPath = elXPath + "/..//input[contains(@class,'js-submit-input')]";
            Driver.FindElement(By.XPath(textareaXPath)).SendKeys("Text 123 another text 0123");

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что в поле осталось число
            elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__view')]//p[text()='" +
                        fieldName + "']/../div[contains(@class,'js-value')]";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.IsTrue(text == "1230123", "Ошибка: в поле сохранилось неправильное число");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление пользовательского поля Число, проверка необходимости значения по умолчанию
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomNumberDefaultValueFieldTest()
        {
            // Создать глоссарий, изменить структуру, открыть добавление нового термина
            string fieldName = "CustomField: " + "Number";

            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Открыть редактирование структуры
            OpenEditGlossaryStructure();
            // Перейти на пользовательские поля
            Driver.FindElement(By.XPath(".//a[contains(@class,'js-type-tab js-custom-tab')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//table[contains(@class,'l-editgloss__tblEditStructure')]")).Displayed);
            // Ввести названиe
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-custom-attrs')]//input[contains(@class,'l-editgloss__cusattrbox__text long js-name')]")).SendKeys(fieldName);
            Thread.Sleep(1000);
            // Выбрать тип
            Driver.FindElement(By.XPath(".//table[contains(@class,'l-editgloss__tblEditStructure')]//span[contains(@class,'js-dropdown__text type')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item type')][@data-id='Number']"))).Click();
            Thread.Sleep(1000);
            // Нажать "Добавить"
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-add-custom-attribute')]")).Click();

            // Дождаться появления ошибки о необходимости ввести значение по умолчанию
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-attribute-errors')]//p[contains(@class,'js-error-default-value')]")).Displayed);
            SetDefaultValueCustomField("0");
            Thread.Sleep(1000);
            // Нажать "Добавить"
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-add-custom-attribute')]")).Click();
            Thread.Sleep(1000);
            // Сохранить
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]")).Click();
            // Дождаться закрытия формы
            Thread.Sleep(2000);

            // Нажать New item
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-add-concept')]"))).Click();
            // Заполнить термин
            FillNewItemExtended();

            // Проверить, что поле появилось
            string elXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__edit')]//p[text()='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Ввести в поле текст
            string textareaXPath = elXPath + "/..//input[contains(@class,'js-submit-input')]";
            Driver.FindElement(By.XPath(textareaXPath)).SendKeys("Text 123 another text 0123");

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что в поле осталось число
            elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__view')]//p[text()='" +
                        fieldName + "']/../div[contains(@class,'js-value')]";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.IsTrue(text == "1230123", "Ошибка: в поле сохранилось неправильное число");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление пользовательского поля Множественный выбор
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomMultipleChoiceFieldTest()
        {
            // Создать глоссарий, изменить структуру с добавлением списка, открыть добавление нового термина
            List<string> choiceList = new List<string>();
            choiceList.Add("select1");
            choiceList.Add("select2");
            choiceList.Add("select3");
            string fieldName = SetCustomGlossaryStructureAddList("MultipleChoice", choiceList);

            // Проверить, что поле появилось
            string elXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__edit')]//p[text()='" + fieldName + "']";
            Thread.Sleep(1000);
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Кликнуть по полю
            string textareaXPath = elXPath + "/..//div[contains(@class,'ui-multiselect')]";
            Driver.FindElement(By.XPath(textareaXPath)).Click();
            // Выбрать два элемента
            Driver.FindElement(By.XPath(
                ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')][text()='" + choiceList[0] + "']")).Click();
            Driver.FindElement(By.XPath(
                ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')][text()='" + choiceList[1] + "']")).Click();
            Thread.Sleep(1000);
            string resultString = choiceList[0] + ", " + choiceList[1];

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что в термине выбранные элементы
            elXPath += "/../../div[contains(@class,'l-corpr__viewmode__view js-view')]//div";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.AreEqual(resultString, text, "Ошибка: в поле сохранился неправильный выбор");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Множественный выбор
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomMultipleChoiceReaquiredFieldTest()
        {
            // Создать глоссарий, изменить структуру с добавлением списка, открыть добавление нового термина
            List<string> choiceList = new List<string>();
            choiceList.Add("select1");
            choiceList.Add("select2");
            choiceList.Add("select3");
            string fieldName = SetCustomGlossaryStructureAddList("MultipleChoice", choiceList, true);

            // Проверить, что поле появилось
            string elXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__edit')]//p[text()='" + fieldName + "']";
            Thread.Sleep(1000);
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();

            // Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым            
            Assert.IsTrue(
                Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit l-error')]//p[contains(@class,'l-editgloss__name')][text()='" + fieldName + "']")).Displayed,
                "Ошибка: обязательное поле не отмечено ошибкой");

            // Кликнуть по полю
            string textareaXPath = elXPath + "/..//div[contains(@class,'ui-multiselect')]";
            Driver.FindElement(By.XPath(textareaXPath)).Click();
            // Добавить два элемента
            Driver.FindElement(By.XPath(
                ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')][text()='" + choiceList[0] + "']")).Click();
            Driver.FindElement(By.XPath(
                ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')][text()='" + choiceList[1] + "']")).Click();
            Thread.Sleep(1000);
            string resultString = choiceList[0] + ", " + choiceList[1];

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что в термине выбранные элементы
            elXPath += "/../../div[contains(@class,'l-corpr__viewmode__view js-view')]//div";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.AreEqual(resultString, text, "Ошибка: в поле сохранился неправильный выбор");
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление пользовательского поля Да/Нет
        /// </summary>
        [Test]
        public void EditGlossaryStructureAddCustomBooleanFieldTest()
        {
            // Создать глоссарий, изменить структуру, открыть добавление нового термина
            string fieldName = SetCustomFieldGlossaryStructure("Boolean");

            // Проверить, что поле появилось
            string elXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__edit')]//span[contains(@class,'l-editgloss__name')][text()='" + fieldName + "']";
            Thread.Sleep(1000);
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Отметить галочку
            string inputXPath = elXPath + "/..//span[contains(@class,'js-chckbx')]//input[contains(@class,'js-chckbx__orig')]";
            Driver.FindElement(By.XPath(inputXPath)).Click();

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();

            // Проверить значение в поле
            inputXPath = elXPath + "/..//input[contains(@class,'js-submit-input')]";
            string valueResult = Driver.FindElement(By.XPath(inputXPath)).GetAttribute("value");
            Assert.AreEqual("true", valueResult, "Ошибка: в поле неверное значение");
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
            Thread.Sleep(1000);
            // Создать другой термин
            CreateItemAndSave();
            Thread.Sleep(1000);

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
            Thread.Sleep(1000);
            // Создать другой термин
            CreateItemAndSave();
            Thread.Sleep(1000);

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
            Thread.Sleep(5000);
            // Ввести слово для поиска
            Driver.FindElement(By.XPath(".//form[contains(@class,'js-search-form')]//textarea[@id='searchText']")).Clear();
            Driver.FindElement(By.XPath(".//form[contains(@class,'js-search-form')]//textarea[@id='searchText']")).SendKeys(uniqueData);
            // Нажать Перевести
            Driver.FindElement(By.XPath(".//form[contains(@class,'js-search-form')]//span[contains(@class,'g-redbtn search')]//input")).Click();
            Thread.Sleep(5000);
            // Проверить результат
            string tableXPath = ".//div[contains(@class,'js-search-results')]//div[contains(@class,'l-glossary__data')]";
            IList<IWebElement> resultList = Driver.FindElements(By.XPath(tableXPath + "//h2[contains(@class,'l-glossary__srctext')]"));
            Thread.Sleep(5000);
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

        private void SwitchCurrentGlossary(string glossaryName)
        {
            // Перейти на страницу глоссария
            string xPath = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text()='" + glossaryName + "']";
            Driver.FindElement(By.XPath(xPath)).Click();
        }

        private void SwitchSuggestedTerms()
        {
            // Перейти на страницу Предложенные термины (из списка глоссариев)
            Driver.FindElement(By.XPath(".//a[contains(@href,'/Suggests')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'l-corpr__sugggloss')]")).Displayed);
        }

        private void SwitchSuggestTermCurrentGlossary()
        {
            // Перейти в предложенные термины для этого глоссария
            Driver.FindElement(By.XPath(".//div[contains(@class, 'l-corprsubmn__data')]/ul//li[3]/a")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'l-corpr__sugggloss')]")).Displayed);
        }

        private void SwitchGlossaryFromSuggestedTerm()
        {
            // Перейти в глоссарий из его предложенных терминов
            Driver.FindElement(By.XPath(".//div[contains(@class, 'l-corprsubmn__data')]/ul//li[2]/a")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'l-corpr__addbtnbox js-buttons-block')]")).Displayed);
        }

        private void SwitchSearchTab()
        {
            // Перейти на страницу поиска
            Driver.FindElement(By.XPath(
                ".//ul[@class='g-corprmenu__list']//a[contains(@href,'/Start')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//form[contains(@class,'js-search-form')]")).Displayed);
        }

        private void OpenGlossaryProperties()
        {
            // Нажать Редактирование
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-edit-submenu')]"))).Click();
            // Нажать на Properties
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-edit-glossary-btn')]"))).Click();
        }

        private void CreateGlossaryByName(string glossaryName)
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

            // Нажать сохранить
            Driver.FindElement(By.XPath(
               ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[contains(@class,'js-save')]")).Click();
            // Ответ формы
            Thread.Sleep(2000);
        }

        private bool GetIsExistGlossary(string glossaryName)
        {
            // Получить: существует ли глоссарий с таким именем
            bool isExist = false;
            IList<IWebElement> glossaryList = Driver.FindElements(By.XPath("//tr[contains(@class, 'js-glossary-row')]/td[1]/p"));
            foreach (IWebElement el in glossaryList)
            {
                if (el.Text == glossaryName)
                {
                    isExist = true;
                    break;
                }
            }
            return isExist;
        }

        private string GetUniqueGlossaryName()
        {
            // Получить уникальное имя глоссария (т.к. добавляется точная дата и время, то не надо проверять, есть ли такой глоссарий в списке)
            return GlossaryName + DateTime.Now.ToString();
        }

        private void OpenCreateGlossary()
        {
            // Нажать кнопку Create a glossary
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-create-glossary-button')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Click();
            // ждем загрузку формы
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]")));
        }

        private string CreateGlossaryAndReturnToGlossaryList()
        {
            // Получить уникальное имя для глоссария
            string glossaryName = GetUniqueGlossaryName();
            // Создать глоссарий
            CreateGlossaryByName(glossaryName);
            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            return glossaryName;
        }

        private int GetCountOfItems()
        {
            // Получить количество терминов
            return Driver.FindElements(By.XPath(".//tr[contains(@class, 'js-concept-row')]")).Count;
        }

        private int GetCountOfSuggestTerms()
        {
            // Получить количество предложенных терминов
            return Driver.FindElements(By.XPath(".//tr[contains(@class, 'js-suggest-row')]")).Count;
        }

        private void OpenEditGlossaryStructure()
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

        private void EditGlossaryStructureAddField()
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
                    Thread.Sleep(1000);
                    break;
                }
            }

            // Сохранить
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]")).Click();
            // Дождаться закрытия формы
            Thread.Sleep(2000);
        }

        private void CreateItemAndSave(string firstTerm = "", string secondTerm = "")
        {
            // Открыть форму добавления термина и заполнить поля
            FillCreateItem(firstTerm, secondTerm);
            // Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
            Driver.Manage().Window.Maximize();
            // Нажать Сохранить
            Driver.FindElement(By.XPath(".//tr[contains(@class, 'js-concept-row js-editing opened')]//a[contains(@class, 'js-save-btn')]")).Click();
            Thread.Sleep(2000);
        }

        private void FillCreateItem(string firstTerm = "", string secondTerm = "")
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

        private bool GetIsDateEqualCurrentDayOrToday(string dateTimeString, DateTime curDay)
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

        private int GetCountSuggestTermsWithoutGlossary()
        {
            int count = 0;
            // Получить все строки с предложенными терминами
            IList<IWebElement> termList = Driver.FindElements(By.XPath(GetSuggestTermRowsXPath()));
            for (int i = 0; i < termList.Count; ++i)
            {
                // Если после удаления пробелов нет символов, значит, что глоссарий не указан
                if (termList[i].Text.Trim().Length == 0)
                {
                    // Проверить, что строка не скрыта
                    string xPath = ".//tr[contains(@class, 'js-suggest-row')][" + (i + 1) + "]";
                    if (!Driver.FindElement(By.XPath(xPath)).GetAttribute("class").Contains("g-hidden"))
                    {
                        ++count;
                    }
                }
            }
            // Возвращаем количество терминов без указанного глоссария
            return count;
        }

        private void CreateSuggestTerm()
        {
            // Предложить термин
            SuggestTermAndSave("Suggest Term 1", "Suggest Term 2");
        }

        private void SuggestTermSetGlossary(string glossaryName)
        {
            // Предложить термин и выбрать другой глоссарий
            SuggestTermAndSave("Suggest Term 1", "Suggest Term 2", true, glossaryName);
        }

        private void SuggestTermAndSave(string termFirst, string termSecond, bool isNeedSelectGlossary = false, string glossaryName = "")
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

        private void SuggestTermFillTerms(string suggestTermFirst, string suggestTermSecond)
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

        private void ClickButtonSuggestTermRowByGlossary(string glossaryName, string btnName)
        {
            // Найти термины, относящиеся к глоссарию
            IList<IWebElement> termList = Driver.FindElements(By.XPath(GetSuggestTermRowsXPath()));
            for (int i = 0; i < termList.Count; ++i)
            {
                if (termList[i].Text.Trim() == glossaryName)
                {
                    // Нажать в строке с этим глоссарием кнопку
                    ClickButtonSuggestTermRow(btnName, true, (i + 1));
                    break;
                }
            }
        }

        private void ClickButtonSuggestTermRow(string btnName, bool isNeedSelectCurrentRow = false, int rowNumber = 0)
        {
            // Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
            Driver.Manage().Window.Maximize();
            // Получить xPath ячейки с комментарием в строке
            string xPathTermRow = "//tr[contains(@class, 'js-suggest-row')]";
            if (isNeedSelectCurrentRow)
            {
                xPathTermRow += "[" + rowNumber + "]";
            }
            xPathTermRow += "//td[contains(@class, 'l-corpr__td suggestComment')]";

            // Нажать на ячейку
            Driver.FindElement(By.XPath(xPathTermRow)).Click();
            // Нажать кнопку
            xPathTermRow += "//a[contains(@class, '" + btnName + "')]";
            Driver.FindElement(By.XPath(xPathTermRow)).Click();
            Thread.Sleep(2000);
        }

        private int GetCountSuggestTermsGlossary(string glossaryName)
        {
            // Найти термины, относящиеся к текущему глоссарию
            string xPathTermRow = GetSuggestTermRowsXPath() + "[contains(text(),'" + glossaryName + "')]";
            // Вернуть количество таких терминов
            return Driver.FindElements(By.XPath(GetSuggestTermRowsXPath() + "[contains(text(),'" + glossaryName + "')]")).Count;
        }

        private string GetSuggestTermRowsXPath()
        {
            // Вернуть xPath строк с предложенными терминами
            return ".//tr[contains(@class, 'js-suggest-row')]/td[contains(@class, 'js-glossary-cell')]//p";
        }

        private void FillImportGlossaryForm()
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

        private void ChangeGlossaryNameToCurrent(string glossaryName)
        {
            // Очистить поле с именем глоссария
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-popup-edit-glossary')][2]//input[contains(@class,'js-glossary-name')]")).Clear();
            Thread.Sleep(1000);
            // Ввести уникальное имя
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-popup-edit-glossary')][2]//input[contains(@class,'js-glossary-name')]")).SendKeys(glossaryName);
            Thread.Sleep(1000);
            // Сохранить
            Driver.FindElement(By.XPath(
                ".//div[contains(@class, 'js-popup-edit-glossary')][2]//span[contains(@class,'js-save')]")).Click();
            Thread.Sleep(2000);
        }

        private void FillNewItemExtended()
        {
            // Нажать Add у первого языка
            string xPath = ".//div[contains(@class, 'js-terms-tree')]//div[contains(@class, 'l-corprtree__langbox')][1]//span[contains(@class, 'js-add-term')]";
            Driver.FindElement(By.XPath(xPath)).Click();
            // Ввести текст
            string termText = "Example Term Text " + DateTime.Now.ToString();
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-term-editor')]//input")).SendKeys(termText);
            // Нажать Add у второго языка
            xPath = ".//div[contains(@class, 'js-terms-tree')]//div[contains(@class, 'l-corprtree__langbox')][2]//span[contains(@class, 'js-add-term')]";
            Driver.FindElement(By.XPath(xPath)).Click();
            // Ввести текст
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-term-editor')]//input")).SendKeys(termText);
        }

        private void EditGlossaryStructure()
        {
            // Имя глоссария для тестирования структуры, чтобы не создавать лишний раз
            string glossaryName = "TestGlossaryEditStructureUniqueName";
            if (!GetIsExistGlossary(glossaryName))
            {
                // Создать глоссарий
                CreateGlossaryByName(glossaryName);
            }
            else
            {
                // Открыть глоссарий
                SwitchCurrentGlossary(glossaryName);
            }

            // Добавить все поля в структуру
            AddAllSystemGeneralFieldStructure();

            // Нажать New item
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-add-concept')]"))).Click();
            // Заполнить термин
            FillNewItemExtended();
        }

        private void AddAllSystemGeneralFieldStructure()
        {
            // Открыть редактирование структуры
            OpenEditGlossaryStructure();

            List<string> fieldsList = new List<string>();
            fieldsList.Add("Interpretation");
            fieldsList.Add("InterpretationSource");
            fieldsList.Add("Topic");
            fieldsList.Add("Domain");
            fieldsList.Add("Multimedia");
            fieldsList.Add("Image");
            fieldsList.Add("Example");

            foreach (string field in fieldsList)
            {
                // Получить xPath строки с нужным полем
                string rowXPath = ".//table[contains(@class, 'js-predefined-attrs-table concept')]//tr[contains(@class, 'js-attr-row')][contains(@data-attr-key,'" + field + "')]";
                // Получить аттрибут class этой строки
                if (!Driver.FindElement(By.XPath(rowXPath)).GetAttribute("class").Contains("g-hidden"))
                {
                    rowXPath += "/td[1]";
                    // Нажать на поле
                    Driver.FindElement(By.XPath(rowXPath)).Click();
                    // Добавить
                    Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'js-add-tbx-attribute')]"))).Click();
                    Thread.Sleep(1000);
                }
            }

            // Сохранить
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]")).Click();
            // Дождаться закрытия формы
            Thread.Sleep(2000);
        }

        private void CheckEditGlossaryStructureTextarea(string fieldName)
        {
            // Проверить, что поле появилось
            string elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//textarea[@name='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");
            // Ввести текст в поле
            string interpretationExample = fieldName + " Example";
            Driver.FindElement(By.XPath(elXPath)).SendKeys(interpretationExample);

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что текст в поле сохранился
            elXPath += "/../../div[contains(@class,'l-corpr__viewmode__view js-view')]//div";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.AreEqual(text, interpretationExample, "Ошибка: текст не сохранился");
        }

        private void EditGlossaryStructureAddField(string fieldName)
        {
            // Открыть редактирование структуры
            OpenEditGlossaryStructure();

            // Получить xPath строки с нужным полем
            string rowXPath = ".//table[contains(@class, 'js-predefined-attrs-table Concept')]//tr[contains(@class, 'js-attr-row')][contains(@data-attr-key,'" + fieldName + "')]";
            // Получить аттрибут class этой строки
            string attributeValue = Driver.FindElement(By.XPath(rowXPath)).GetAttribute("class");
            // Проверить, что строка не скрыта
            Assert.IsTrue(!attributeValue.Contains("g-hidden"), "Ошибка: поле не должно быть скрыто в новом глоссарии");

            rowXPath += "/td[1]";
            // Нажать на поле
            Driver.FindElement(By.XPath(rowXPath)).Click();
            // Добавить
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'js-add-tbx-attribute')]"))).Click();
            Thread.Sleep(1000);
            // Сохранить
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]")).Click();
            // Дождаться закрытия формы
            Thread.Sleep(2000);
        }

        private string SetCustomFieldGlossaryStructure(string fieldType, bool isRequired = false, bool isNeedDefaultValue = false, string defaultValue = "")
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Открыть редактирование структуры
            OpenEditGlossaryStructure();
            // Перейти на пользовательские поля
            Driver.FindElement(By.XPath(".//a[contains(@class,'js-type-tab js-custom-tab')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//table[contains(@class,'l-editgloss__tblEditStructure')]")).Displayed);
            // Ввести названиe
            string fieldName = "CustomField: " + fieldType;
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-custom-attrs')]//input[contains(@class,'l-editgloss__cusattrbox__text long js-name')]")).SendKeys(fieldName);
            Thread.Sleep(1000);
            // Выбрать тип
            Driver.FindElement(By.XPath(".//table[contains(@class,'l-editgloss__tblEditStructure')]//span[contains(@class,'js-dropdown__text type')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item type')][@data-id='" + fieldType + "']"))).Click();
            // Если обязательное - поставить галочку
            if (isRequired)
            {
                Driver.FindElement(By.XPath(".//span[contains(@class,'js-chckbx g-iblock')]")).Click();
            }

            if (isNeedDefaultValue)
            {
                // Ввести значение по умолчанию
                SetDefaultValueCustomField(defaultValue);
            }

            Thread.Sleep(1000);
            // Нажать "Добавить"
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-add-custom-attribute')]")).Click();
            Thread.Sleep(1000);
            // Сохранить
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]")).Click();
            // Дождаться закрытия формы
            Thread.Sleep(2000);

            // Нажать New item
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-add-concept')]"))).Click();
            // Заполнить термин
            FillNewItemExtended();
            return fieldName;
        }

        private void SetDefaultValueCustomField(string defaultValue)
        {
            // Очистить поле значения по умолчанию
            Driver.FindElement(By.XPath(
                ".//td[contains(@class,'js-default-editor-placeholder')]//input[contains(@class,'js-submit-input')]")).Clear();
            // Ввести в него значение
            Driver.FindElement(By.XPath(
                ".//td[contains(@class,'js-default-editor-placeholder')]//input[contains(@class,'js-submit-input')]")).SendKeys(defaultValue);
        }

        private string SetCustomGlossaryStructureAddList(string fieldType, List<string> choiceList, bool isRequired = false)
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Открыть редактирование структуры
            OpenEditGlossaryStructure();
            // Перейти на пользовательские поля
            Driver.FindElement(By.XPath(".//a[contains(@class,'js-type-tab js-custom-tab')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//table[contains(@class,'l-editgloss__tblEditStructure')]")).Displayed);
            // Ввести названиe
            string fieldName = "CustomField: " + fieldType;
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-custom-attrs')]//input[contains(@class,'l-editgloss__cusattrbox__text long js-name')]")).SendKeys(fieldName);
            Thread.Sleep(1000);
            // Выбрать тип
            Driver.FindElement(By.XPath(".//table[contains(@class,'l-editgloss__tblEditStructure')]//span[contains(@class,'js-dropdown__text type')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item type')][@data-id='" + fieldType + "']"))).Click();

            // Если обязательное - поставить галочку
            if (isRequired)
            {
                Driver.FindElement(By.XPath(".//span[contains(@class,'js-chckbx g-iblock')]")).Click();
            }

            Thread.Sleep(1000);
            // Нажать "Добавить"
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-add-custom-attribute')]")).Click();

            // Проверить, что появилась ошибка пустого списка
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//table[contains(@class,'l-editgloss__tblEditStructure')]//div[contains(@class,'js-attribute-errors')]//p[contains(@class,'js-error-no-choices')]")).
                Displayed, "Ошибка: не появилось сообщение, что нужно добавить элементы списка");
            // Элементы списка, регистр важен - маленькие!

            string choiceListText = "";
            foreach (string it in choiceList)
            {
                if (choiceListText.Length == 0)
                {
                    choiceListText = it;
                }
                else
                {
                    choiceListText += "; " + it;
                }
            }

            Driver.FindElement(By.XPath(".//table[contains(@class,'l-editgloss__tblEditStructure')]//input[contains(@class,'js-choice-values')]"))
                .SendKeys(choiceListText);

            Thread.Sleep(1000);
            // Нажать "Добавить"
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-add-custom-attribute')]")).Click();

            Thread.Sleep(1000);
            // Сохранить
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]")).Click();
            // Дождаться закрытия формы
            Thread.Sleep(2000);

            // Нажать New item
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-add-concept')]"))).Click();
            // Заполнить термин
            FillNewItemExtended();

            return fieldName;
        }
    }
}
