using System.Collections.Generic;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.Glossary.EditStructure.Custom
{
	/// <summary>
	/// Группа тестов для проверки редактирования структуры глоссариев
	/// </summary>
	[Category("Standalone")]
	public class GlossaryEditStructureCustomFieldsTest<TWebDriverSettings> : GlossaryTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского текстового поля
		/// </summary>
		[Test]
		public void AddTextFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			var fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Text, true);

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsExistCustomField(fieldName);

			// Ввести текст в поле
			var interpretationExample = fieldName + " Example";
			GlossaryPage.FillCustomFieldText(fieldName, interpretationExample);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что в термине выбранные элементы
			var text = GlossaryPage.GetCustomFieldValue(fieldName);
			Assert.AreEqual(
				interpretationExample, 
				text.Trim(), 
				"Ошибка: неправильное значение поля");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского текстового поля
		/// </summary>
		[Test]
		public void AddTextRequiredFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			var fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Text, true);

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsExistCustomField(fieldName);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым			
			GlossaryPage.AssertionIsExistCustomFieldError(fieldName);

			// Ввести текст в поле
			var interpretationExample = fieldName + " Example";
			GlossaryPage.FillCustomFieldText(fieldName, interpretationExample);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что в термине выбранные элементы
			var text = GlossaryPage.GetCustomFieldValue(fieldName);
			Assert.AreEqual(interpretationExample, text.Trim(), "Ошибка: неправильное значение поля");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Дата
		/// </summary>
		[Test]
		public void AddDateFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			var fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Date);

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsExistCustomField(fieldName);

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldDate(fieldName);

			// Проверить, что календарь открылся
			GlossaryPage.AssertionIsCalendarExist();
			// Выбрать текущую дату
			GlossaryPage.SelectCalendarToday();

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что в термине выбранные элементы
			var text = GlossaryPage.GetCustomFieldValue(fieldName);
			Assert.IsTrue(text.Trim().Length > 0, "Ошибка: поле пустое");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Дата
		/// </summary>
		[Test]
		public void AddDateRequiredFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			var fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Date, true);

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsExistCustomField(fieldName);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым			
			GlossaryPage.AssertionIsExistCustomFieldError(fieldName);

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldDate(fieldName);

			// Проверить, что календарь открылся
			GlossaryPage.AssertionIsCalendarExist();

			// Выбрать текущую дату
			GlossaryPage.SelectCalendarToday();

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что в термине выбранные элементы
			var text = GlossaryPage.GetCustomFieldValue(fieldName);

			Assert.IsTrue(text.Trim().Length > 0, "Ошибка: поле пустое");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Аудио/Видео
		/// </summary>
		[Test]
		[Category("ForLocalRun")]
		public void AddMediaFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			var fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Media);

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsCustomFieldImageExist(fieldName);

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldMedia(fieldName);
			// Загрузить документ
			GlossaryPage.UploadFileInGlossary(PathProvider.AudioFile);
			
			// Дождаться загрузки документа
			GlossaryPage.AssertionDocumentUploaded(fieldName);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что изображение загрузилось
			Assert.IsTrue(
				GlossaryPage.GetIsCustomFieldMediaFilled(fieldName), 
				"Ошибка: аудио не загрузилось");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Аудио/Видео
		/// </summary>
		[Test]
		[Category("ForLocalRun")]
		public void AddMediaRequiredFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			var fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Media, true);

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsCustomFieldImageExist(fieldName);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым			
			GlossaryPage.AssertionIsExistCustomFieldImageError(fieldName);

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldMedia(fieldName);

			// Загрузить документ
			GlossaryPage.UploadFileInGlossary(PathProvider.AudioFile);

			// Дождаться загрузки документа
			GlossaryPage.AssertionDocumentUploaded(fieldName);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что изображение загрузилось
			Assert.IsTrue(
				GlossaryPage.GetIsCustomFieldMediaFilled(fieldName), 
				"Ошибка: аудио не загрузилось");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Image
		/// </summary>
		[Test]
		[Category("ForLocalRun")]
		public void AddImageFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			var fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Image);

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsCustomFieldImageExist(fieldName);

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldImage(fieldName);
			// Загрузить документ
			GlossaryPage.UploadFileInGlossary(PathProvider.ImageFile);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что изображение загрузилось
			Assert.IsTrue(
				GlossaryPage.GetCustomFieldImageFilled(fieldName), 
				"Ошибка: изображение не загрузилось");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Изображение
		/// </summary>
		[Test]
		[Category("ForLocalRun")]
		public void AddImageRequiredFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			var fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Image, true);

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsCustomFieldImageExist(fieldName);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым			
			GlossaryPage.AssertionIsExistCustomFieldImageError(fieldName);

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldImage(fieldName);
			// Загрузить документ
			GlossaryPage.UploadFileInGlossary(PathProvider.ImageFile);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что изображение загрузилось
			Assert.IsTrue(
				GlossaryPage.GetCustomFieldImageFilled(fieldName), 
				"Ошибка: изображение не загрузилось");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Список
		/// </summary>
		[Test]
		public void AddListFieldTest()
		{
			// Создать глоссарий, изменить структуру с добавлением списка, открыть добавление нового термина
			var choiceList = new List<string>();
			choiceList.Add("select1");
			choiceList.Add("select2");
			var fieldName = SetCustomGlossaryStructureAddList(GlossaryEditStructureFormHelper.FIELD_TYPE.Choice, choiceList);

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsExistCustomField(fieldName);

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldChoice(fieldName);
			// Выбрать элемент из списка
			GlossaryPage.SelectChoiceItem(choiceList[0]);
			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что в термине сохранился выбранный элемент
			Assert.AreEqual(
				GlossaryPage.GetCustomFieldValue(fieldName), 
				choiceList[0], 
				"Ошибка: в термине не сохранился выбранный элемент");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Список
		/// </summary>
		[Test]
		public void AddListRequiredFieldTest()
		{
			// here
			// Создать глоссарий, изменить структуру с добавлением списка, открыть добавление нового термина
			var choiceList = new List<string>
			{
				"select1", 
				"select2"
			};

			var fieldName = SetCustomGlossaryStructureAddList(GlossaryEditStructureFormHelper.FIELD_TYPE.Choice, choiceList, true);

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsExistCustomField(fieldName);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым			
			GlossaryPage.AssertionIsExistCustomFieldError(fieldName);

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldChoice(fieldName);
			// Выбрать элемент из списка
			GlossaryPage.SelectChoiceItem(choiceList[0]);
			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что в термине сохранился выбранный элемент
			Assert.AreEqual(
				GlossaryPage.GetCustomFieldValue(fieldName), 
				choiceList[0], 
				"Ошибка: в термине не сохранился выбранный элемент");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Число
		/// </summary>
		[Test]
		public void AddNumberFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			var fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Number, false, true, "0");

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsExistCustomField(fieldName);

			// Ввести в поле текст
			GlossaryPage.FillCustomFieldNumber(fieldName, "Text 123 another text 0123");

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что в поле осталось число
			var text = GlossaryPage.GetCustomFieldNumberValue(fieldName);
			Assert.IsTrue(text == "1230123", "Ошибка: в поле сохранилось неправильное число");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Число, проверка необходимости значения по умолчанию
		/// </summary>
		[Test]
		public void AddNumberDefaultValueFieldTest()
		{
			var fieldType = GlossaryEditStructureFormHelper.FIELD_TYPE.Number;
			// Создать глоссарий и начать создание пользовательского поля
			var fieldName = CreateGlossaryAddCustom(fieldType);
			// Нажать "Добавить"
			GlossaryEditStructureForm.ClickAddCustomAttribute();

			// Дождаться появления ошибки о необходимости ввести значение по умолчанию
			GlossaryEditStructureForm.AssertionIsExistCustomAttrErrorEmptyDefault();
			
			// Заполняем поле "Значение по умолчанию"
			GlossaryEditStructureForm.AddDefaultValue("0");

			// Нажать "Добавить"
			GlossaryEditStructureForm.ClickAddCustomAttribute();
			// Сохранить
			GlossaryEditStructureForm.ClickSaveStructureBtn();
			// Дождаться закрытия формы
			GlossaryEditStructureForm.WaitFormClose();
			// Нажать New item
			GlossaryPage.ClickNewItemBtn();
			// Дождаться открытия формы добавления нового термина
			GlossaryPage.WaitNewItemOpen();
			// Заполнить термин
			FillNewItemExtended();

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsExistCustomField(fieldName);

			// Ввести в поле текст
			GlossaryPage.FillCustomFieldNumber(
				fieldName, 
				"Text 123 another text 0123");
			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что в поле осталось число
			var text = GlossaryPage.GetCustomFieldNumberValue(fieldName);
			Assert.IsTrue(text == "1230123", "Ошибка: в поле сохранилось неправильное число");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Множественный выбор
		/// </summary>
		[Test]
		public void AddMultipleChoiceFieldTest()
		{
			// Создать глоссарий, изменить структуру с добавлением списка, открыть добавление нового термина
			var choiceList = new List<string>
			{
				"select1", 
				"select2", 
				"select3"
			};

			var fieldName = SetCustomGlossaryStructureAddList(
				GlossaryEditStructureFormHelper.FIELD_TYPE.MultipleChoice, 
				choiceList);

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsExistCustomField(fieldName);

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldMultiSelect(fieldName);
			// Выбрать два элемента
			GlossaryPage.SelectItemMultiSelect(choiceList[0]);
			GlossaryPage.SelectItemMultiSelect(choiceList[1]);
			var resultString = choiceList[0] + ", " + choiceList[1];
			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что в термине выбранные элементы
			Assert.AreEqual(
				resultString,
				GlossaryPage.GetCustomFieldValue(fieldName), 
				"Ошибка: в поле сохранился неправильный выбор");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Множественный выбор
		/// </summary>
		[Test]
		public void AddMultipleChoiceRequiredFieldTest()
		{
			// Создать глоссарий, изменить структуру с добавлением списка, открыть добавление нового термина
			var choiceList = new List<string>
			{
				"select1", 
				"select2", 
				"select3"
			};

			var fieldName = SetCustomGlossaryStructureAddList(
				GlossaryEditStructureFormHelper.FIELD_TYPE.MultipleChoice, 
				choiceList,
				isRequired: true);

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsExistCustomField(fieldName);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым			
			GlossaryPage.AssertionIsExistCustomFieldError(fieldName);

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldMultiSelect(fieldName);
			// Добавить два элемента
			GlossaryPage.SelectItemMultiSelect(choiceList[0]);
			GlossaryPage.SelectItemMultiSelect(choiceList[1]);

			var resultString = choiceList[0] + ", " + choiceList[1];

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Дождаться появления поля с сохраненным термином
			GlossaryPage.AssertionConceptSave();

			// Проверить, что в термине выбранные элементы
			Assert.AreEqual(
				resultString, 
				GlossaryPage.GetCustomFieldValue(fieldName), 
				"Ошибка: в поле сохранился неправильный выбор");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Да/Нет
		/// </summary>
		[Test]
		public void AddBooleanFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			var fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Boolean);

			// Проверить, что поле появилось
			GlossaryPage.AssertionIsExistCustomFieldBool(fieldName);

			// Отметить галочку
			GlossaryPage.ClickCustomFieldBool(fieldName);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Проверить значение в поле
			GlossaryPage.AssertionIsCustomBooleanChecked(fieldName);
		}

		/// <summary>
		/// Добавить пользовательское поле с добавлением списка
		/// </summary>
		/// <param name="fieldType">тип поля</param>
		/// <param name="choiceList">список значений</param>
		/// <param name="isRequired">нужно указать, что поле обязательное</param>
		/// <returns>название поля</returns>
		protected string SetCustomGlossaryStructureAddList(
			GlossaryEditStructureFormHelper.FIELD_TYPE fieldType, 
			List<string> choiceList, 
			bool isRequired = false)
		{
			// Создать глоссарий и начать создание пользовательского поля
			var fieldName = CreateGlossaryAddCustom(fieldType, isRequired);

			// Нажать "Добавить"
			GlossaryEditStructureForm.ClickAddCustomAttribute();

			// Проверить, что появилась ошибка пустого списка
			GlossaryEditStructureForm.AssertionIsDisplayedErrorEmptyChoice();
			
			// Элементы списка, регистр важен - маленькие!
			var choiceListText = "";

			foreach (var it in choiceList)
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

			GlossaryEditStructureForm.FillChoiceValues(choiceListText);
			// Нажать "Добавить"
			GlossaryEditStructureForm.ClickAddCustomAttribute();

			// Сохранить
			GlossaryEditStructureForm.ClickSaveStructureBtn();
			// Дождаться закрытия формы
			GlossaryEditStructureForm.WaitFormClose();

			// Нажать New item
			GlossaryPage.ClickNewItemBtn();

			// Дождаться открытия формы добавления нового термина
			GlossaryPage.WaitNewItemOpen();
			
			// Заполнить термин
			FillNewItemExtended();

			return fieldName;
		}

		/// <summary>
		/// Добавить пользовательское поле
		/// </summary>
		/// <param name="fieldType">тип поля</param>
		/// <param name="isRequired">обязательное ли</param>
		/// <param name="isNeedDefaultValue">нужно ли заполнить поле по умолчанию</param>
		/// <param name="defaultValue">значение поля по умочанию</param>
		/// <returns>название поля</returns>
		protected string SetCustomFieldGlossaryStructure(
			GlossaryEditStructureFormHelper.FIELD_TYPE fieldType, 
			bool isRequired = false, 
			bool isNeedDefaultValue = false, 
			string defaultValue = "")
		{
			// Создать глоссарий и начать создание пользовательского поля
			var fieldName = CreateGlossaryAddCustom(fieldType, isRequired);

			if (isNeedDefaultValue)
			{
				// Ввести значение по умолчанию
				GlossaryEditStructureForm.AddDefaultValue(defaultValue);
			}

			// Нажать "Добавить"
			GlossaryEditStructureForm.ClickAddCustomAttribute();
			Thread.Sleep(1000);
			// Сохранить
			GlossaryEditStructureForm.ClickSaveStructureBtn();
			// Дождаться закрытия формы
			GlossaryEditStructureForm.WaitFormClose();
			MainHelperClass.WaitUntilCloseDialogBackground();
			// Нажать New item
			GlossaryPage.ClickNewItemBtn();
			// Дождаться появления полей языков нового термина
			GlossaryPage.WaitUntilTermsDisplay();
			// Заполнить термин
			FillNewItemExtended();

			return fieldName;
		}

		/// <summary>
		/// Создать глоссарий, начать создание пользовательского поля
		/// </summary>
		/// <param name="fieldType">тип поля</param>
		/// <param name="isRequired">обязательное ли</param>
		/// <returns>название поля</returns>
		protected string CreateGlossaryAddCustom(
			GlossaryEditStructureFormHelper.FIELD_TYPE fieldType, 
			bool isRequired = false)
		{
			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();

			CreateGlossaryByName(glossaryName);
			// Открыть редактирование структуры
			OpenEditGlossaryStructure();
			// Перейти на пользовательские поля
			GlossaryEditStructureForm.SwitchCustomTab();

			// Ввести названиe
			var fieldName = "CustomField: " + fieldType;

			GlossaryEditStructureForm.FillNameCustomField(fieldName);
			// Выбрать тип
			GlossaryEditStructureForm.SelectCustomFieldType(fieldType);

			// Если обязательное - поставить галочку
			if (isRequired)
			{
				GlossaryEditStructureForm.SelectRequiredCheckbox();
			}

			return fieldName;
		}
	}
}
