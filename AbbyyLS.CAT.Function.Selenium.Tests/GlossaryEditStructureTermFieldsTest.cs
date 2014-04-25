using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class GlossaryEditStructureTermFieldsTest : GlossaryTest
    {
        public GlossaryEditStructureTermFieldsTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Term - поле Source
        /// </summary>
        [Test]
        public void AddSourceFieldTest()
        {
            CheckTermLevelField("Source");
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Term - поле Interpretation
        /// </summary>
        [Test]
        public void AddInterpretationFieldTest()
        {
            CheckTermLevelField("Interpretation");
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Term - поле InterpretationSource
        /// </summary>
        [Test]
        public void AddInterpretationSourceFieldTest()
        {
            CheckTermLevelField("InterpretationSource");
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Term - поле Context
        /// </summary>
        [Test]
        public void AddContextFieldTest()
        {
            CheckTermLevelField("Context");
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Term - поле ContextSource
        /// </summary>
        [Test]
        public void AddContextSourceFieldTest()
        {
            CheckTermLevelField("ContextSource");
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Term - поле Status
        /// </summary>
        [Test]
        public void AddStatusFieldTest()
        {
            CheckTermLevelSelectField("Status");
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Term - поле Label
        /// </summary>
        [Test]
        public void AddLabelFieldTest()
        {
            CheckTermLevelSelectField("Label");
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Term - поле Gender
        /// </summary>
        [Test]
        public void AddGenderFieldTest()
        {
            CheckTermLevelSelectField("Gender");
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Term - поле Number
        /// </summary>
        [Test]
        public void AddNumberFieldTest()
        {
            CheckTermLevelSelectField("Number");
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Term - поле PartOfSpeech
        /// </summary>
        [Test]
        public void AddPartOfSpeechFieldTest()
        {
            CheckTermLevelSelectField("PartOfSpeech");
        }

        protected void CheckTermLevelSelectField(string fieldName)
        {
            // Создать глоссарий, изменить структуру, открыть добавление термина
            EditGlossaryTermStructure();

            // Проверить, что поле есть            
            string selectXPath = ".//td[contains(@class,'js-details-panel')]//select[@name='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElements(By.XPath(selectXPath)).Count > 0, "Ошибка: поле не появилось!");
            string optionId = Driver.FindElement(By.XPath(selectXPath + "//option[2]")).GetAttribute("value");
            // Нажать, чтобы список открылся
            string fieldXPath = selectXPath + "/..//span[contains(@class,'js-dropdown')]";
            Driver.FindElement(By.XPath(fieldXPath)).Click();
            // Выбрать значение
            string optionText =
                Driver.FindElement(By.XPath(".//span[contains(@class,'js-dropdown__list')]//span[@data-id='" + optionId + "']")).Text;
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-dropdown__list')]//span[@data-id='" + optionId + "']")).Click();

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);
            // Нажать на термин, чтобы появились поля для Term
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-term-node')]")).Click();
            string fieldText = Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-term-attrs')]//select[@name='" + fieldName + "']/../..//div[contains(@class,'js-value')]")).Text;

            Assert.AreEqual(optionText, fieldText, "Ошибка: значение не сохранилось\n");
        }

        protected void CheckTermLevelField(string fieldName)
        {
            // Создать глоссарий, изменить структуру, открыть добавление термина
            EditGlossaryTermStructure();

            // Проверить, что поле есть            
            string textareaXPath = ".//td[contains(@class,'js-details-panel')]//textarea[@name='" + fieldName + "']";
            Assert.IsTrue(Driver.FindElements(By.XPath(textareaXPath)).Count > 0, "Ошибка: поле не появилось!");
            // Ввести текст в поле
            string fieldExample = fieldName + " Example";
            Driver.FindElement(By.XPath(textareaXPath)).SendKeys(fieldExample);

            // Сохранить термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            // Дождаться появления поля с сохраненным термином
            Wait.Until((d) => d.FindElement(By.XPath(".//tr[contains(@class,'js-concept-row opened')]")).Displayed);
            // Нажать на термин, чтобы появились поля для Term
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-term-node')]")).Click();
            string fieldText = Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-term-attrs')]//textarea[@name='" + fieldName + "']/../..//div[contains(@class,'js-value')]")).Text;

            Assert.AreEqual(fieldExample, fieldText, "Ошибка: текст не сохранился\n");
        }

        protected void EditGlossaryTermStructure()
        {
            // Имя глоссария для тестирования структуры уровня Language, чтобы не создавать лишний раз
            string glossaryName = "TestGlossaryEditStructureTermLevelUniqueName";
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
            AddAllSystemTermFieldStructure();

            // Нажать New item
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-add-concept')]"))).Click();
            // Заполнить термин
            FillNewItemExtended();

            // Нажать на термин, чтобы появились поля для Term
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-term-node')]")).Click();
        }

        protected void AddAllSystemTermFieldStructure()
        {
            // Открыть редактирование структуры
            OpenEditGlossaryStructure();

            // Выбрать уровень "Term"
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-dropdown__text level')]")).Click();
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list level g-drpdwn__list')]//span[@data-id='term']")).Click();

            List<string> fieldsList = new List<string>();
            fieldsList.Add("Source");
            fieldsList.Add("Gender");
            fieldsList.Add("Number");
            fieldsList.Add("PartOfSpeech");
            fieldsList.Add("Interpretation");
            fieldsList.Add("InterpretationSource");
            fieldsList.Add("Context");
            fieldsList.Add("ContextSource");
            fieldsList.Add("Status");
            fieldsList.Add("Label");

            foreach (string field in fieldsList)
            {
                // Получить xPath строки с нужным полем
                string rowXPath = ".//table[contains(@class, 'js-predefined-attrs-table term')]//tr[contains(@class, 'js-attr-row')][contains(@data-attr-key,'" + field + "')]";
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
            WaitUntilDisappearElement(".//div[contains(@class,'js-popup-edit-structure')]");
        }
    }
}
