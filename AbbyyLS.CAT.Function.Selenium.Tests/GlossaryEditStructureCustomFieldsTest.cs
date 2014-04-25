using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class GlossaryEditStructureCustomFieldsTest : GlossaryTest
    {
        public GlossaryEditStructureCustomFieldsTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Метод тестирования изменения структуры: добавление пользовательского текстового поля
        /// </summary>
        [Test]
        public void AddTextFieldTest()
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
        public void AddTextRequiredFieldTest()
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
        public void AddDateFieldTest()
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
        public void AddDateRequiredFieldTest()
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
        public void AddMediaFieldTest()
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
        public void AddMediaRequiredFieldTest()
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
        public void AddImageFieldTest()
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
        public void AddImageRequiredFieldTest()
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
        public void AddListFieldTest()
        {
            // Создать глоссарий, изменить структуру с добавлением списка, открыть добавление нового термина
            List<string> choiceList = new List<string>();
            choiceList.Add("select1");
            choiceList.Add("select2");
            string fieldName = SetCustomGlossaryStructureAddList("Choice", choiceList);

            // Проверить, что поле появилось
            string elXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__edit')]//p[text()='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Кликнуть по полю
            string textareaXPath = elXPath + "/..//span[contains(@class,'js-dropdown')]";
            Driver.FindElement(By.XPath(textareaXPath)).Click();
            // Выбрать элемент в списке
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item')][@title='" + choiceList[0] + "']")).Click();
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
        public void AddListRequiredFieldTest()
        {
            // Создать глоссарий, изменить структуру с добавлением списка, открыть добавление нового термина
            List<string> choiceList = new List<string>();
            choiceList.Add("select1");
            choiceList.Add("select2");
            string fieldName = SetCustomGlossaryStructureAddList("Choice", choiceList, true);

            // Проверить, что поле появилось
            string elXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__edit')]//p[text()='" + fieldName + "']";
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
        public void AddNumberFieldTest()
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
        public void AddNumberDefaultValueFieldTest()
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
            // Выбрать тип
            Driver.FindElement(By.XPath(".//table[contains(@class,'l-editgloss__tblEditStructure')]//span[contains(@class,'js-dropdown__text type')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item type')][@data-id='Number']"))).Click();
            // Нажать "Добавить"
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-add-custom-attribute')]")).Click();

            // Дождаться появления ошибки о необходимости ввести значение по умолчанию
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-attribute-errors')]//p[contains(@class,'js-error-default-value')]")).Displayed);
            SetDefaultValueCustomField("0");
            // Нажать "Добавить"
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-add-custom-attribute')]")).Click();
            // Сохранить
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]")).Click();
            // Дождаться закрытия формы

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
        public void AddMultipleChoiceFieldTest()
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
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Displayed, "Ошибка: поле не появилось");

            // Кликнуть по полю
            string textareaXPath = elXPath + "/..//div[contains(@class,'ui-multiselect')]";
            Driver.FindElement(By.XPath(textareaXPath)).Click();
            // Выбрать два элемента
            Driver.FindElement(By.XPath(
                ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')][text()='" + choiceList[0] + "']")).Click();
            Driver.FindElement(By.XPath(
                ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')][text()='" + choiceList[1] + "']")).Click();
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
        public void AddMultipleChoiceRequiredFieldTest()
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
        public void AddBooleanFieldTest()
        {
            // Создать глоссарий, изменить структуру, открыть добавление нового термина
            string fieldName = SetCustomFieldGlossaryStructure("Boolean");

            // Проверить, что поле появилось
            string elXPath =
                ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'l-corpr__viewmode__edit')]//span[contains(@class,'l-editgloss__name')][text()='" + fieldName + "']";
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

        protected string SetCustomGlossaryStructureAddList(string fieldType, List<string> choiceList, bool isRequired = false)
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
            // Выбрать тип
            Driver.FindElement(By.XPath(".//table[contains(@class,'l-editgloss__tblEditStructure')]//span[contains(@class,'js-dropdown__text type')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item type')][@data-id='" + fieldType + "']"))).Click();

            // Если обязательное - поставить галочку
            if (isRequired)
            {
                Driver.FindElement(By.XPath(".//span[contains(@class,'js-chckbx g-iblock')]")).Click();
            }

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

            // Нажать "Добавить"
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-add-custom-attribute')]")).Click();

            // Сохранить
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]")).Click();
            // Дождаться закрытия формы
            WaitUntilDisappearElement(".//div[contains(@class,'js-popup-edit-structure')]");

            // Нажать New item
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-add-concept')]"))).Click();
            // Заполнить термин
            FillNewItemExtended();

            return fieldName;
        }

        protected string SetCustomFieldGlossaryStructure(string fieldType, bool isRequired = false, bool isNeedDefaultValue = false, string defaultValue = "")
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
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-custom-attrs')]//input[contains(@class,'l-editgloss__cusattrbox__text long js-name')]")).SendKeys(fieldName);
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

            // Нажать "Добавить"
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-add-custom-attribute')]")).Click();
            Thread.Sleep(1000);
            // Сохранить
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]")).Click();
            // Дождаться закрытия формы
            WaitUntilDisappearElement(".//div[contains(@class,'js-popup-edit-structure')]");

            // Нажать New item
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-add-concept')]"))).Click();
            // Заполнить термин
            FillNewItemExtended();
            return fieldName;
        }

        protected void SetDefaultValueCustomField(string defaultValue)
        {
            // Очистить поле значения по умолчанию
            Driver.FindElement(By.XPath(
                ".//td[contains(@class,'js-default-editor-placeholder')]//input[contains(@class,'js-submit-input')]")).Clear();
            // Ввести в него значение
            Driver.FindElement(By.XPath(
                ".//td[contains(@class,'js-default-editor-placeholder')]//input[contains(@class,'js-submit-input')]")).SendKeys(defaultValue);
        }
    }
}
