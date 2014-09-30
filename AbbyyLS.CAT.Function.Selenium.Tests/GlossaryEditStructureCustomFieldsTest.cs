﻿using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки редактирования структуры глоссариев
	/// </summary>
	public class GlossaryEditStructureCustomFieldsTest : GlossaryTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		 
		 
		/// <param name="browserName">Название браузера</param>
		public GlossaryEditStructureCustomFieldsTest(string browserName)
			: base(browserName)
		{

		}



		/// <summary>
		/// Предварительная подготовка группы тестов
		/// </summary>
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
			string fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Text, true);

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistCustomField(fieldName), "Ошибка: поле не появилось");

			// Ввести текст в поле
			string interpretationExample = fieldName + " Example";
			GlossaryPage.FillCustomFieldText(fieldName, interpretationExample);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что в термине выбранные элементы
			string text = GlossaryPage.GetCustomFieldValue(fieldName);
			Assert.AreEqual(interpretationExample, text.Trim(), "Ошибка: неправильное значение поля");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского текстового поля
		/// </summary>
		[Test]
		public void AddTextRequiredFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			string fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Text, true);

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistCustomField(fieldName), "Ошибка: поле не появилось");

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым			
			Assert.IsTrue(GlossaryPage.GetIsExistCustomFieldError(fieldName),
				"Ошибка: обязательное поле не отмечено ошибкой");

			// Ввести текст в поле
			string interpretationExample = fieldName + " Example";
			GlossaryPage.FillCustomFieldText(fieldName, interpretationExample);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что в термине выбранные элементы
			string text = GlossaryPage.GetCustomFieldValue(fieldName);
			Assert.AreEqual(interpretationExample, text.Trim(), "Ошибка: неправильное значение поля");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Дата
		/// </summary>
		[Test]
		public void AddDateFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			string fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Date);

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistCustomField(fieldName), "Ошибка: поле не появилось");

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldDate(fieldName);

			// Проверить, что календарь открылся
			Assert.IsTrue(GlossaryPage.GetIsExistCalendar(),
				"Ошибка: календарь не появился");
			// Выбрать текущую дату
			GlossaryPage.SelectCalendarToday();

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что в термине выбранные элементы
			string text = GlossaryPage.GetCustomFieldValue(fieldName);
			Assert.IsTrue(text.Trim().Length > 0, "Ошибка: поле пустое");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Дата
		/// </summary>
		[Test]
		public void AddDateRequiredFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			string fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Date, true);

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistCustomField(fieldName), "Ошибка: поле не появилось");

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым			
			Assert.IsTrue(GlossaryPage.GetIsExistCustomFieldError(fieldName),
				"Ошибка: обязательное поле не отмечено ошибкой");

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldDate(fieldName);

			// Проверить, что календарь открылся
			Assert.IsTrue(GlossaryPage.GetIsExistCalendar(),
				"Ошибка: календарь не появился");
			// Выбрать текущую дату
			GlossaryPage.SelectCalendarToday();

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что в термине выбранные элементы
			string text = GlossaryPage.GetCustomFieldValue(fieldName);
			Assert.IsTrue(text.Trim().Length > 0, "Ошибка: поле пустое");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Аудио/Видео
		/// </summary>
		[Test]
		public void AddMediaFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			string fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Media);

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsCustomFieldImageExist(fieldName), "Ошибка: поле не появилось");

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldMedia(fieldName);
			// Загрузить документ
			FillAddDocumentForm(AudioFile);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что изображение загрузилось
			Assert.IsTrue(GlossaryPage.GetIsCustomFieldMediaFilled(fieldName), "Ошибка: аудио не загрузилось");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Аудио/Видео
		/// </summary>
		[Test]
		public void AddMediaRequiredFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			string fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Media, true);

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsCustomFieldImageExist(fieldName), "Ошибка: поле не появилось");

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым			
			Assert.IsTrue(GlossaryPage.GetIsExistCustomFieldImageError(fieldName),
				"Ошибка: обязательное поле не отмечено ошибкой");

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldMedia(fieldName);
			// Загрузить документ
			FillAddDocumentForm(AudioFile);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что изображение загрузилось
			Assert.IsTrue(GlossaryPage.GetIsCustomFieldMediaFilled(fieldName), "Ошибка: аудио не загрузилось");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Image
		/// </summary>
		[Test]
		public void AddImageFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			string fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Image);

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsCustomFieldImageExist(fieldName), "Ошибка: поле не появилось");

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldImage(fieldName);
			// Загрузить документ
			FillAddDocumentForm(ImageFile);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что изображение загрузилось
			Assert.IsTrue(GlossaryPage.GetCustomFieldImageFilled(fieldName), "Ошибка: изображение не загрузилось");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Изображение
		/// </summary>
		[Test]
		public void AddImageRequiredFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			string fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Image, true);

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsCustomFieldImageExist(fieldName), "Ошибка: поле не появилось");

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым			
			Assert.IsTrue(GlossaryPage.GetIsExistCustomFieldImageError(fieldName),
				"Ошибка: обязательное поле не отмечено ошибкой");

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldImage(fieldName);
			// Загрузить документ
			FillAddDocumentForm(ImageFile);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что изображение загрузилось
			Assert.IsTrue(GlossaryPage.GetCustomFieldImageFilled(fieldName), "Ошибка: изображение не загрузилось");
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
			string fieldName = SetCustomGlossaryStructureAddList(GlossaryEditStructureFormHelper.FIELD_TYPE.Choice, choiceList);

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistCustomField(fieldName), "Ошибка: поле не появилось");

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldChoice(fieldName);
			// Выбрать элемент из списка
			GlossaryPage.SelectChoiceItem(choiceList[0]);
			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что в термине сохранился выбранный элемент
			Assert.AreEqual(GlossaryPage.GetCustomFieldValue(fieldName), choiceList[0], "Ошибка: в термине не сохранился выбранный элемент");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление ОБЯЗАТЕЛЬНОГО пользовательского поля Список
		/// </summary>
		[Test]
		public void AddListRequiredFieldTest()
		{
			// here
			// Создать глоссарий, изменить структуру с добавлением списка, открыть добавление нового термина
			List<string> choiceList = new List<string>();
			choiceList.Add("select1");
			choiceList.Add("select2");
			string fieldName = SetCustomGlossaryStructureAddList(GlossaryEditStructureFormHelper.FIELD_TYPE.Choice, choiceList, true);

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistCustomField(fieldName), "Ошибка: поле не появилось");

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым			
			Assert.IsTrue(GlossaryPage.GetIsExistCustomFieldError(fieldName),
				"Ошибка: обязательное поле не отмечено ошибкой");

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldChoice(fieldName);
			// Выбрать элемент из списка
			GlossaryPage.SelectChoiceItem(choiceList[0]);
			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что в термине сохранился выбранный элемент
			Assert.AreEqual(GlossaryPage.GetCustomFieldValue(fieldName), choiceList[0], "Ошибка: в термине не сохранился выбранный элемент");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Число
		/// </summary>
		[Test]
		public void AddNumberFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			string fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Number, false, true, "0");

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistCustomField(fieldName), "Ошибка: поле не появилось");

			// Ввести в поле текст
			GlossaryPage.FillCustomFieldNumber(fieldName, "Text 123 another text 0123");

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что в поле осталось число
			string text = GlossaryPage.GetCustomFieldNumberValue(fieldName);
			Assert.IsTrue(text == "1230123", "Ошибка: в поле сохранилось неправильное число");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Число, проверка необходимости значения по умолчанию
		/// </summary>
		[Test]
		public void AddNumberDefaultValueFieldTest()
		{
			GlossaryEditStructureFormHelper.FIELD_TYPE fieldType = GlossaryEditStructureFormHelper.FIELD_TYPE.Number;
			// Создать глоссарий и начать создание пользовательского поля
			string fieldName = CreateGlossaryAddCustom(fieldType);

			// Нажать "Добавить"
			GlossaryEditStructureForm.ClickAddCustomAttribute();

			// Дождаться появления ошибки о необходимости ввести значение по умолчанию
			Assert.IsTrue(GlossaryEditStructureForm.GetIsExistCustomAttrErrorEmptyDefault(),
				"Ошибка: не появилось оповещение о пустом значении по умолчанию для пользовательского поля");
			SetDefaultValueCustomField("0");
			// Нажать "Добавить"
			GlossaryEditStructureForm.ClickAddCustomAttribute();
			// Сохранить
			GlossaryEditStructureForm.ClickSaveStructureBtn();
			// Дождаться закрытия формы

			// Нажать New item
			GlossaryPage.ClickNewItemBtn();
			// Заполнить термин
			FillNewItemExtended();

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistCustomField(fieldName), "Ошибка: поле не появилось");

			// Ввести в поле текст
			GlossaryPage.FillCustomFieldNumber(fieldName, "Text 123 another text 0123");

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что в поле осталось число
			string text = GlossaryPage.GetCustomFieldNumberValue(fieldName);
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
			string fieldName = SetCustomGlossaryStructureAddList(GlossaryEditStructureFormHelper.FIELD_TYPE.MultipleChoice, choiceList);

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistCustomField(fieldName), "Ошибка: поле не появилось");

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldMultiSelect(fieldName);
			// Выбрать два элемента
			GlossaryPage.SelectItemMultiSelect(choiceList[0]);
			GlossaryPage.SelectItemMultiSelect(choiceList[1]);
			string resultString = choiceList[0] + ", " + choiceList[1];

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что в термине выбранные элементы
			string text = GlossaryPage.GetCustomFieldValue(fieldName);
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
			string fieldName = SetCustomGlossaryStructureAddList(GlossaryEditStructureFormHelper.FIELD_TYPE.MultipleChoice, choiceList, true);

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistCustomField(fieldName), "Ошибка: поле не появилось");

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Проверить, что поле отмечено ошибкой - поле обязательное, поэтому не может сохраняться пустым			
			Assert.IsTrue(GlossaryPage.GetIsExistCustomFieldError(fieldName),
				"Ошибка: обязательное поле не отмечено ошибкой");

			// Кликнуть по полю
			GlossaryPage.ClickCustomFieldMultiSelect(fieldName);
			// Добавить два элемента
			GlossaryPage.SelectItemMultiSelect(choiceList[0]);
			GlossaryPage.SelectItemMultiSelect(choiceList[1]);

			string resultString = choiceList[0] + ", " + choiceList[1];

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что в термине выбранные элементы
			string text = GlossaryPage.GetCustomFieldValue(fieldName);
			Assert.AreEqual(resultString, text, "Ошибка: в поле сохранился неправильный выбор");
		}

		/// <summary>
		/// Метод тестирования изменения структуры: добавление пользовательского поля Да/Нет
		/// </summary>
		[Test]
		public void AddBooleanFieldTest()
		{
			// Создать глоссарий, изменить структуру, открыть добавление нового термина
			string fieldName = SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE.Boolean);

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistCustomFieldBool(fieldName), "Ошибка: поле не появилось");

			// Отметить галочку
			GlossaryPage.ClickCustomFieldBool(fieldName);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Проверить значение в поле
			Assert.IsTrue(GlossaryPage.GetIsCustomBooleanChecked(fieldName), "Ошибка: в поле неверное значение");
		}



		/// <summary>
		/// Добавить пользовательское поле с добавлением списка
		/// </summary>
		/// <param name="fieldType">тип поля</param>
		/// <param name="choiceList">список значений</param>
		/// <param name="isRequired">нужно указать, что поле обязательное</param>
		/// <returns>название поля</returns>
		protected string SetCustomGlossaryStructureAddList(GlossaryEditStructureFormHelper.FIELD_TYPE fieldType, List<string> choiceList, bool isRequired = false)
		{
			// Создать глоссарий и начать создание пользовательского поля
			string fieldName = CreateGlossaryAddCustom(fieldType, isRequired);

			// Нажать "Добавить"
			GlossaryEditStructureForm.ClickAddCustomAttribute();

			// Проверить, что появилась ошибка пустого списка
			Assert.IsTrue(GlossaryEditStructureForm.GetIsDisplayedErrorEmptyChoice(), "Ошибка: не появилось сообщение, что нужно добавить элементы списка");
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

			GlossaryEditStructureForm.FillChoiceValues(choiceListText);

			// Нажать "Добавить"
			GlossaryEditStructureForm.ClickAddCustomAttribute();

			// Сохранить
			GlossaryEditStructureForm.ClickSaveStructureBtn();
			// Дождаться закрытия формы
			GlossaryEditStructureForm.WaitFormClose();

			// Нажать New item
			GlossaryPage.ClickNewItemBtn();
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
		protected string SetCustomFieldGlossaryStructure(GlossaryEditStructureFormHelper.FIELD_TYPE fieldType, bool isRequired = false, bool isNeedDefaultValue = false, string defaultValue = "")
		{
			// Создать глоссарий и начать создание пользовательского поля
			string fieldName = CreateGlossaryAddCustom(fieldType, isRequired);

			if (isNeedDefaultValue)
			{
				// Ввести значение по умолчанию
				SetDefaultValueCustomField(defaultValue);
			}

			// Нажать "Добавить"
			GlossaryEditStructureForm.ClickAddCustomAttribute();
			Thread.Sleep(1000);
			// Сохранить
			GlossaryEditStructureForm.ClickSaveStructureBtn();
			// Дождаться закрытия формы
			GlossaryEditStructureForm.WaitFormClose();

			// Нажать New item
			GlossaryPage.ClickNewItemBtn();
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
		protected string CreateGlossaryAddCustom(GlossaryEditStructureFormHelper.FIELD_TYPE fieldType, bool isRequired = false)
		{
			// Создать глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);

			// Открыть редактирование структуры
			OpenEditGlossaryStructure();
			// Перейти на пользовательские поля
			GlossaryEditStructureForm.SwitchCustomTab();
			// Ввести названиe
			string fieldName = "CustomField: " + fieldType;
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

		/// <summary>
		/// Ввести значение по умолчанию
		/// </summary>
		/// <param name="defaultValue">значение</param>
		protected void SetDefaultValueCustomField(string defaultValue)
		{
			GlossaryEditStructureForm.AddDefaultValue(defaultValue);
		}
	}
}
