using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class GlossaryEditStructureGeneralFieldsTest : GlossaryTest
    {
        public GlossaryEditStructureGeneralFieldsTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Метод тестирования изменения структуры - Definition/Interpretation
        /// </summary>
        [Test]
        public void AddInterpretationFieldTest()
        {
            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryGeneralStructure();
            // Проверить поле Interpretation
            CheckEditGlossaryStructureTextarea("Interpretation");
        }

        /// <summary>
        /// Метод тестирования изменения структуры - Definition source/Interpretation source
        /// </summary>
        [Test]
        public void AddInterpretationSourceFieldTest()
        {
            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryGeneralStructure();
            // Проверить поле InterpretationSource
            CheckEditGlossaryStructureTextarea("InterpretationSource");
        }

        /// <summary>
        /// Метод тестирования изменения структуры - Example
        /// </summary>
        [Test]
        public void AddExampleFieldTest()
        {
            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryGeneralStructure();
            // Проверить поле Example
            CheckEditGlossaryStructureTextarea("Example");
        }

        /// <summary>
        /// Метод тестирования изменения структуры - Topic
        /// </summary>
        [Test]
        public void AddTopicFieldTest()
        {
            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryGeneralStructure();

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
        public void AddDomainFieldTest()
        {
            // Перейти на вкладку проектов
            SwitchDomainTab();

            // Проверить, есть ли проект с таким именем
            string domainName = "TestDomainGlossaryEditStructure";
            CreateDomainIfNotExist(domainName);

            // Вернуться к глоссариям
            SwitchGlossaryTab();

            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryGeneralStructure();

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
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item')][contains(@title,'" + domainName + "')]")).Click();

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);

            // Проверить, что значение в поле есть
            elXPath += "/../../div[contains(@class,'l-corpr__viewmode__view js-view')]//div[contains(@class,'js-value')]";
            string text = Driver.FindElement(By.XPath(elXPath)).Text;
            Assert.AreEqual(text, domainName, "Ошибка: проект не сохранился в поле");

        }

        /// <summary>
        /// Метод тестирования изменения структуры - Image
        /// </summary>
        [Test]
        public void AddImageFieldTest()
        {
            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryGeneralStructure();

            string fieldName = "Image";

            // Проверить, что поле появилось
            string elXPath = ".//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//input[@name='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElement(By.XPath(elXPath)).Enabled, "Ошибка: поле не появилось");
            // Нажать на поле, чтобы открылся диалог загрузки документа
            string fieldXPath = elXPath + "/..//div[contains(@class,'l-editgloss__imagebox')]//a";
            Driver.FindElement(By.XPath(fieldXPath)).Click();

            // Заполнить диалог загрузки изображения
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
        /// Метод тестирования изменения структуры - Multimedia
        /// </summary>
        [Test]
        public void AddMultimediaFieldTest()
        {
            // Изменить структуру глоссария, открыть создание нового термина
            EditGlossaryGeneralStructure();

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

        protected void CheckEditGlossaryStructureTextarea(string fieldName)
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


        protected void EditGlossaryGeneralStructure()
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


        protected void AddAllSystemGeneralFieldStructure()
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
                }
            }

            // Сохранить
            Driver.FindElement(By.XPath(".//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]")).Click();
            // Дождаться закрытия формы
            Thread.Sleep(2000);
        }

    }
}