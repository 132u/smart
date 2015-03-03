using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов дял проверки редактирования структуры глоссария (язык)
	/// </summary>
	[Category("Standalone")]
	public class GlossaryEditStructureLanguageFieldsTest : GlossaryTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		 
		/// <param name="browserName">Название браузера</param>
		public GlossaryEditStructureLanguageFieldsTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Languages - поле Comment
		/// </summary>
		[Test]
		public void AddCommentFieldTest()
		{
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Comment];
			CheckLanguageLevelField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Languages - поле Interpretation
		/// </summary>
		[Test]
		public void AddInterpretationFieldTest()
		{
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Interpretation];
			CheckLanguageLevelField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Languages - поле InterpretationSource
		/// </summary>
		[Test]
		public void AddInterpretationSourceFieldTest()
		{
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.InterpretationSource];
			CheckLanguageLevelField(fieldName);
		}

		/// <summary>
		/// Проверить поля уровня Language
		/// </summary>
		/// <param name="fieldName">назваие поля</param>
		protected void CheckLanguageLevelField(string fieldName)
		{
			// Имя глоссария для тестирования структуры уровня Language, чтобы не создавать лишний раз
			const string glossaryName = "TestGlossaryEditStructureLanguageLevelUniqueName";

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
			Assert.IsTrue(
				GlossaryPage.GetIsExistDetailsTextarea(fieldName), 
				"Ошибка: поле не появилось!");

			// Ввести текст в поле
			var fieldExample = fieldName + " Example";
			GlossaryPage.FillDetailTextarea(fieldName, fieldExample);
			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Нажать на язык, чтобы появились поля для Language
			GlossaryPage.OpenLanguageAttributes();
			
			Assert.AreEqual(
				fieldExample,
				GlossaryPage.GetDetailTextareaValue(fieldName), 
				"Ошибка: текст не сохранился\n");
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
