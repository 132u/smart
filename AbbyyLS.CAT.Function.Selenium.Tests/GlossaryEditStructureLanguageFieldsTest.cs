using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class GlossaryEditStructureLanguageFieldsTest : GlossaryTest
    {
        public GlossaryEditStructureLanguageFieldsTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Languages - поле Comment
        /// </summary>
        [Test]
        public void AddCommentFieldTest()
        {
            CheckLanguageLevelField("Comment");
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Languages - поле Interpretation
        /// </summary>
        [Test]
        public void AddInterpretationFieldTest()
        {
            CheckLanguageLevelField("Interpretation");
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Languages - поле InterpretationSource
        /// </summary>
        [Test]
        public void AddInterpretationSourceFieldTest()
        {
            CheckLanguageLevelField("InterpretationSource");
        }

        protected void CheckLanguageLevelField(string fieldName)
        {
            // Имя глоссария для тестирования структуры уровня Language, чтобы не создавать лишний раз
            string glossaryName = "TestGlossaryEditStructureLanguageLevelUniqueName";
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
            AddAllSystemLanguageFieldStructure();

            // Нажать New item
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-add-concept')]"))).Click();
            // Заполнить термин
            FillNewItemExtended();

            // Нажать на язык, чтобы появились поля для Language
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-node js-lang-node')]")).Click();
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
            // Нажать на язык, чтобы появились поля для Language
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-node js-lang-node')]")).Click();
            string fieldText = Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-lang-attrs')]//textarea[@name='" + fieldName + "']/../..//div[contains(@class,'js-value')]")).Text;

            Assert.AreEqual(fieldExample, fieldText, "Ошибка: текст не сохранился\n");
        }

        protected void AddAllSystemLanguageFieldStructure()
        {
            // Открыть редактирование структуры
            OpenEditGlossaryStructure();

            // Выбрать уровень "Language"
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-dropdown__text level')]")).Click();
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list level g-drpdwn__list')]//span[@data-id='language']")).Click();

            List<string> fieldsList = new List<string>();
            fieldsList.Add("Interpretation");
            fieldsList.Add("InterpretationSource");
            fieldsList.Add("Comment");

            foreach (string field in fieldsList)
            {
                // Получить xPath строки с нужным полем
                string rowXPath = ".//table[contains(@class, 'js-predefined-attrs-table language')]//tr[contains(@class, 'js-attr-row')][contains(@data-attr-key,'" + field + "')]";
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
