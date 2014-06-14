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
            string fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Comment];
            CheckLanguageLevelField(fieldName);
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Languages - поле Interpretation
        /// </summary>
        [Test]
        public void AddInterpretationFieldTest()
        {
            string fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Interpretation];
            CheckLanguageLevelField(fieldName);
        }

        /// <summary>
        /// Метод тестирования изменения структуры на уровне Languages - поле InterpretationSource
        /// </summary>
        [Test]
        public void AddInterpretationSourceFieldTest()
        {
            string fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.InterpretationSource];
            CheckLanguageLevelField(fieldName);
        }

        /// <summary>
        /// Проверить поля уровня Language
        /// </summary>
        /// <param name="fieldName">назваие поля</param>
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
            GlossaryPage.ClickNewItemBtn();
            // Заполнить термин
            FillNewItemExtended();

            // Нажать на язык, чтобы появились поля для Language
            GlossaryPage.OpenLanguageAttributes();
            // Проверить, что поле есть            
            Assert.IsTrue(GlossaryPage.GetIsExistDetailsTextarea(fieldName), "Ошибка: поле не появилось!");
            // Ввести текст в поле
            string fieldExample = fieldName + " Example";
            GlossaryPage.FillDetailTextarea(fieldName, fieldExample);

            // Сохранить термин
            GlossaryPage.ClickSaveExtendedConcept();
            // Дождаться появления поля с сохраненным термином
            Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");
            // Нажать на язык, чтобы появились поля для Language
            GlossaryPage.OpenLanguageAttributes();
            string fieldText = GlossaryPage.GetDetailTextareaValue(fieldName);

            Assert.AreEqual(fieldExample, fieldText, "Ошибка: текст не сохранился\n");
        }

        /// <summary>
        /// Изменить структуру: добавить все поля уровня Language
        /// </summary>
        protected void AddAllSystemLanguageFieldStructure()
        {
            // Открыть редактирование структуры
            OpenEditGlossaryStructure();

            // Выбрать уровень "Language"
            GlossaryEditStructureForm.ClickLevelDropdown();
            GlossaryEditStructureForm.SelectLanguageLevel();

            // Добавить все поля
            GlossaryEditStructureForm.SelectAllFields();

            // Сохранить
            GlossaryEditStructureForm.ClickSaveStructureBtn();
            // Дождаться закрытия формы
            GlossaryEditStructureForm.WaitFormClose();
        }
    }
}
