using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов дял проверки редактирования структуры глоссария (термины)
	/// </summary>
	public class GlossaryEditStructureTermFieldsTest : GlossaryTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		 
		 
		/// <param name="browserName">Название браузера</param>
		public GlossaryEditStructureTermFieldsTest(string browserName)
			: base(browserName)
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
		/// Метод тестирования изменения структуры на уровне Term - поле Source
		/// </summary>
		[Test]
		public void AddSourceFieldTest()
		{
			string fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Source];
			CheckTermLevelField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле Interpretation
		/// </summary>
		[Test]
		public void AddInterpretationFieldTest()
		{
			string fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Interpretation];
			CheckTermLevelField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле InterpretationSource
		/// </summary>
		[Test]
		public void AddInterpretationSourceFieldTest()
		{
			string fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.InterpretationSource];
			CheckTermLevelField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле Context
		/// </summary>
		[Test]
		public void AddContextFieldTest()
		{
			string fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Context];
			CheckTermLevelField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле ContextSource
		/// </summary>
		[Test]
		public void AddContextSourceFieldTest()
		{
			string fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.ContextSource];
			CheckTermLevelField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле Status
		/// </summary>
		[Test]
		public void AddStatusFieldTest()
		{
			string fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Status];
			CheckTermLevelSelectField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле Label
		/// </summary>
		[Test]
		public void AddLabelFieldTest()
		{
			string fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Label];
			CheckTermLevelSelectField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле Gender
		/// </summary>
		[Test]
		public void AddGenderFieldTest()
		{
			string fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Gender];
			CheckTermLevelSelectField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле Number
		/// </summary>
		[Test]
		public void AddNumberFieldTest()
		{
			string fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Number];
			CheckTermLevelSelectField(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры на уровне Term - поле PartOfSpeech
		/// </summary>
		[Test]
		public void AddPartOfSpeechFieldTest()
		{
			string fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.PartOfSpeech];
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
			Assert.IsTrue(GlossaryPage.GetIsExistDetailsSelect(fieldName), "Ошибка: поле не появилось!");

			string optionId = GlossaryPage.GetDetailsSelectOptionID(fieldName, 2);
			// Нажать, чтобы список открылся
			GlossaryPage.ClickDetailsSelectDropdown(fieldName);
			// Выбрать значение
			string optionText = GlossaryPage.GetListItemText(optionId);
			GlossaryPage.ClickListItemByID(optionId);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");
			// Нажать на термин, чтобы появились поля для Term
			GlossaryPage.OpenTermLevel();
			string fieldText = GlossaryPage.GetDetailsSelectValue(fieldName);
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
			Assert.IsTrue(GlossaryPage.GetIsExistDetailsTextarea(fieldName), "Ошибка: поле не появилось!");
			// Ввести текст в поле
			string fieldExample = fieldName + " Example";
			GlossaryPage.FillDetailTextarea(fieldName, fieldExample);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");
			// Нажать на термин, чтобы появились поля для Term
			GlossaryPage.OpenTermLevel();
			// Проверить, что значение сохранилось
			string fieldText = GlossaryPage.GetDetailTextareaValue(fieldName);
			Assert.AreEqual(fieldExample, fieldText, "Ошибка: текст не сохранился\n");
		}

		/// <summary>
		/// Добавить глоссарий, изменить структуру, открыть уровень Term
		/// </summary>
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
			GlossaryPage.ClickNewItemBtn();
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
