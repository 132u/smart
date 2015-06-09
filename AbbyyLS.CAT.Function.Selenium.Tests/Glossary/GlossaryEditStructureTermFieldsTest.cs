using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.Glossary.EditStructure.Terms
{
	/// <summary>
	/// Группа тестов дял проверки редактирования структуры глоссария (термины)
	/// </summary>
	[Category("Standalone")]
	public class GlossaryEditStructureTermFieldsTest<TWebDriverSettings> : GlossaryTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле Source
		/// </summary>
		[Test]
		public void AddSourceFieldTest()
		{
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Source];
			CheckTermLevelField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле Interpretation
		/// </summary>
		[Test]
		public void AddInterpretationFieldTest()
		{
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Interpretation];
			CheckTermLevelField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле InterpretationSource
		/// </summary>
		[Test]
		public void AddInterpretationSourceFieldTest()
		{
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.InterpretationSource];
			CheckTermLevelField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле Context
		/// </summary>
		[Test]
		public void AddContextFieldTest()
		{
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Context];
			CheckTermLevelField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле ContextSource
		/// </summary>
		[Test]
		public void AddContextSourceFieldTest()
		{
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.ContextSource];
			CheckTermLevelField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле Status
		/// </summary>
		[Test]
		public void AddStatusFieldTest()
		{
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Status];
			CheckTermLevelSelectField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле Label
		/// </summary>
		[Test]
		public void AddLabelFieldTest()
		{
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Label];
			CheckTermLevelSelectField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле Gender
		/// </summary>
		[Test]
		public void AddGenderFieldTest()
		{
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Gender];
			CheckTermLevelSelectField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле Number
		/// </summary>
		[Test]
		public void AddNumberFieldTest()
		{
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Number];
			CheckTermLevelSelectField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле PartOfSpeech
		/// </summary>
		[Test]
		public void AddPartOfSpeechFieldTest()
		{
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.PartOfSpeech];
			CheckTermLevelSelectField(fieldName);
		}



		/// <summary>
		/// Проверить поле Select уровня Term
		/// </summary>
		/// <param name="fieldName">название поля</param>
		protected void CheckTermLevelSelectField(string fieldName)
		{
			// Создать глоссарий, изменить структуру, открыть добавление термина
			EditGlossaryTermStructure();

			// Проверить, что поле есть			
			Assert.IsTrue(
				GlossaryPage.GetIsExistDetailsSelect(fieldName), 
				"Ошибка: поле не появилось!");

			var optionId = GlossaryPage.GetDetailsSelectOptionId(fieldName, 2);
			// Нажать, чтобы список открылся
			GlossaryPage.ClickDetailsSelectDropdown(fieldName);
			// Выбрать значение
			var optionText = GlossaryPage.GetListItemText(optionId);
			GlossaryPage.ClickListItemById(optionId);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();
			// Нажать на термин, чтобы появились поля для Term
			GlossaryPage.OpenTermLevel();
			var fieldText = GlossaryPage.GetDetailsSelectValue(fieldName);
			// Проверить значение поля
			Assert.AreEqual(optionText, fieldText, "Ошибка: значение не сохранилось\n");
		}

		/// <summary>
		/// Проверить поле уровня Term
		/// </summary>
		/// <param name="fieldName">название поля</param>
		protected void CheckTermLevelField(string fieldName)
		{
			// Создать глоссарий, изменить структуру, открыть добавление термина
			EditGlossaryTermStructure();

			// Проверить, что поле есть			
			GlossaryPage.AssertionIsExistDetailsTextarea(fieldName);

			// Ввести текст в поле
			var fieldExample = fieldName + " Example";
			GlossaryPage.FillDetailTextarea(fieldName, fieldExample);
			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Нажать на термин, чтобы появились поля для Term
			GlossaryPage.OpenTermLevel();

			// Проверить, что значение сохранилось
			Assert.AreEqual(
				fieldExample, 
				GlossaryPage.GetDetailTextareaValue(fieldName), 
				"Ошибка: текст не сохранился\n");
		}

		/// <summary>
		/// Добавить глоссарий, изменить структуру, открыть уровень Term
		/// </summary>
		protected void EditGlossaryTermStructure()
		{
			if (!GlossaryListPage.GetIsExistGlossary(GlossaryUniqueName))
			{
				// Создать глоссарий
				CreateGlossaryByName(GlossaryUniqueName);
			}
			else
			{
				// Открыть глоссарий
				SwitchCurrentGlossary(GlossaryUniqueName);
			}

			// Добавить все поля в структуру
			AddAllSystemTermFieldStructure();
			MainHelperClass.WaitUntilCloseDialogBackground();
			// Нажать New item
			GlossaryPage.ClickNewItemBtn();
			GlossaryPage.WaitUntilTermsDisplay();
			// Заполнить термин
			FillNewItemExtended();
			// Нажать на термин, чтобы появились поля для Term
			GlossaryPage.OpenTermLevel();
		}

		/// <summary>
		/// Изменить структуру: добавить все поля уровня Term
		/// </summary>
		protected void AddAllSystemTermFieldStructure()
		{
			// Открыть редактирование структуры
			OpenEditGlossaryStructure();

			// Выбрать уровень "Term"
			GlossaryEditStructureForm.ClickLevelDropdown();
			GlossaryEditStructureForm.SelectTermLevel();

			// Добавить все поля
			GlossaryEditStructureForm.SelectAllFields();

			// Сохранить
			GlossaryEditStructureForm.ClickSaveStructureBtn();
			// Дождаться закрытия формы
			GlossaryEditStructureForm.WaitFormClose();
		}
	}
}
