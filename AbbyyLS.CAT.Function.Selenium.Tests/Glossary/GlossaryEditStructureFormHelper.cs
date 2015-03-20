﻿using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер формы редактирования структуры глоссария
	/// </summary>
	public class GlossaryEditStructureFormHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public GlossaryEditStructureFormHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
			attributeDict = new Dictionary<ATTRIBUTE_TYPE, string>
			{
				{ATTRIBUTE_TYPE.Interpretation, ATTRIBUTE_INTERPRETATION},
				{ATTRIBUTE_TYPE.InterpretationSource, ATTRIBUTE_INTERPRETATION_SRC},
				{ATTRIBUTE_TYPE.Topic, ATTRIBUTE_TOPIC},
				{ATTRIBUTE_TYPE.Domain, ATTRIBUTE_DOMAIN},
				{ATTRIBUTE_TYPE.Comment, ATTRIBUTE_COMMENT},
				{ATTRIBUTE_TYPE.Multimedia, ATTRIBUTE_MEDIA},
				{ATTRIBUTE_TYPE.Image, ATTRIBUTE_IMAGE},
				{ATTRIBUTE_TYPE.Example, ATTRIBUTE_EXAMPLE},
				{ATTRIBUTE_TYPE.Source, ATTRIBUTE_SOURCE},
				{ATTRIBUTE_TYPE.Gender, ATTRIBUTE_GENDER},
				{ATTRIBUTE_TYPE.Number, ATTRIBUTE_NUMBER},
				{ATTRIBUTE_TYPE.PartOfSpeech, ATTRIBUTE_PART_OF_SPEECH},
				{ATTRIBUTE_TYPE.Context, ATTRIBUTE_CONTEXT},
				{ATTRIBUTE_TYPE.ContextSource, ATTRIBUTE_CONTEXT_SRC},
				{ATTRIBUTE_TYPE.Status, ATTRIBUTE_STATUS},
				{ATTRIBUTE_TYPE.Label, ATTRIBUTE_LABEL}
			};

			fieldTypeDict = new Dictionary<FIELD_TYPE, int>
			{
				{FIELD_TYPE.Media, FIELD_MEDIA},
				{FIELD_TYPE.Date, FIELD_DATE},
				{FIELD_TYPE.Image, FIELD_IMAGE},
				{FIELD_TYPE.Choice, FIELD_CHOICE},
				{FIELD_TYPE.MultipleChoice, FIELD_MULTI_CHOICE},
				{FIELD_TYPE.Number, FIELD_NUMBER},
				{FIELD_TYPE.Text, FIELD_TEXT},
				{FIELD_TYPE.Boolean, FIELD_BOOL}
			};
		}

		/// <summary>
		/// Дождаться появления формы
		/// </summary>
		/// <returns>появилась</returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(FORM_XPATH));
		}

		public bool WaitFormClose()
		{
			Logger.Debug("Дождаться закрытия формы");
			return WaitUntilDisappearElement(By.XPath(FORM_XPATH));
		}

		public void AssertionIsConceptTableDisplay()
		{
			Logger.Trace("Проверить, что таблица Concept видна");

			Assert.IsTrue(GetIsElementDisplay(By.XPath(GetTableXPath(CONCEPT_TABLE_CLASS))),
				"Ошибка: в редакторе структуры отображается не та таблица");
		}

		public bool ClickFieldToAdd(ATTRIBUTE_TYPE attr)
		{
			Logger.Debug(string.Format("Кликнуть по полю добавления {0}", attr));

			var isDisplayed = GetIsElementDisplay(By.XPath(GetAttributeXPath(CONCEPT_TABLE_CLASS, attr)));

			if (isDisplayed)
			{
				ClickElement(By.XPath(GetAttributeXPath(CONCEPT_TABLE_CLASS, attr)));
			}

			return isDisplayed && GetIsElementDisplay(By.XPath(SELECTED_ROW_XPATH));
		}

		public void ClickAddToListBtn()
		{
			Logger.Debug("Нажать кнопку добавления в список");
			ClickElement(By.XPath(ADD_TO_LIST_BTN_XPATH));
		}

		public void ClickSaveStructureBtn()
		{
			Logger.Debug("Нажать кнопку сохранения");
			ClickElement(By.XPath(SAVE_STRUCTURE_BTN_XPATH));
		}

		public void SelectAllFields()
		{
			Logger.Trace("Выбрать все поля таблицы");

			SetDriverTimeoutMinimum();

			Logger.Debug("Получить список видимых элементов таблицы");
			var attrList = GetElementList(By.XPath(GetVisibleTableXPath() + TABLE_NOT_HIDDEN_TR_XPATH));
			
			SetDriverTimeoutDefault();

			foreach(var el in attrList)
			{
				Logger.Debug("Кликнуть на элемент талицы");
				el.Click();
				Logger.Debug("Кликнуть добавить");
				ClickAddToListBtn();
			}
		}

		public void AddDefaultValue(string value)
		{
			Logger.Debug(string.Format("Ввести значение по умолчанию {0}", value));
			ClearAndAddText(By.XPath(DEFAULT_INPUT_XPATH), value);
		}

		public void SwitchCustomTab()
		{
			Logger.Debug("Перейти на вкладку пользовательские поля");
			ClickElement(By.XPath(CUSTOM_TAB_REF_XPATH));

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(CUSTOM_TABLE_XPATH)),
				"Ошибка: вкладка пользовательские поля не была открыта");
		}

		public void FillNameCustomField(string name)
		{
			Logger.Debug(string.Format("Заполнить название пользовательского поля: {0}", name));
			SendTextElement(By.XPath(CUSTOM_FIELD_NAME_XPATH), name);
		}

		public void SelectCustomFieldType(FIELD_TYPE field)
		{
			Logger.Debug(string.Format("Выбрать тип поля {0} в пользовательских настройках", field));
			ClickElement(By.XPath(CUSTOM_TYPE_DROPDOWN_XPATH));
			ClickElement(By.XPath(CUSTOM_TYPE_ITEM_XPATH + "[@data-id='" + fieldTypeDict[field] + "']"));
		}

		public void SelectRequiredCheckbox()
		{
			Logger.Debug("Кликнуть галку Required Field");
			ClickElement(By.XPath(REQUIRED_CHECKBOX_XPATH));
		}

		public void ClickAddCustomAttribute()
		{
			Logger.Debug("Кликнуть добавить пользовательское поле");
			ClickElement(By.XPath(ADD_CUSTOM_BTN_XPATH));
		}

		public void AssertionIsDisplayedErrorEmptyChoice()
		{
			Logger.Trace("Проверить, что появилась ошибка пустого списка выбора");

			Assert.IsTrue(
				GetIsElementDisplay(By.XPath(CUSTOM_ATTR_ERROR_EMPTY_CHOICE_XPATH)),
				"Ошибка: не появилось сообщение, что нужно добавить элементы списка");
		}

		public void FillChoiceValues(string values)
		{
			Logger.Debug(string.Format("Заполнить список выбора значением {0}", values));
			SendTextElement(By.XPath(CUSTOM_CHOICE_VALUE_XPATH), values);
		}

		public void AssertionIsExistCustomAttrErrorEmptyDefault()
		{
			Logger.Trace("Проверить наличие оповещения о пустом значении по умолчанию для пользовательского поля");

			Assert.IsTrue(
				GetIsElementDisplay(By.XPath(CUSTOM_ATTR_ERROR_EMPTY_DEFAULT_XPATH)),
				"Ошибка: не появилось оповещение о пустом значении по умолчанию для пользовательского поля");
		}

		/// <summary>
		/// Кликнуть для выпадения списка уровней
		/// </summary>
		public void ClickLevelDropdown()
		{
			ClickElement(By.XPath(LEVEL_DROPDOWN_XPATH));
		}

		/// <summary>
		/// Выбрать уровень Language
		/// </summary>
		public void SelectLanguageLevel()
		{
			ClickElement(By.XPath(LEVEL_LANGUAGE_XPATH));
		}

		/// <summary>
		/// Выбрать уровень Term
		/// </summary>
		public void SelectTermLevel()
		{
			ClickElement(By.XPath(LEVEL_TERM_XPATH));
		}

		/// <summary>
		/// Получить xpath нужной таблицы
		/// </summary>
		/// <param name="tableType">тип таблицы</param>
		/// <returns>xpath</returns>
		protected string GetTableXPath(string tableType)
		{
			return TABLE_CLASS + "[contains(@class,'" + tableType + "')]";
		}

		/// <summary>
		/// Получить xPath видимой таблицы
		/// </summary>
		/// <returns>xPath</returns>
		protected string GetVisibleTableXPath()
		{
			return TABLE_CLASS + "[contains(@style,'table')]";
		}

		/// <summary>
		/// Получить xPath нужного атрибута
		/// </summary>
		/// <param name="tableType">тип таблицы</param>
		/// <param name="attr">атрибут</param>
		/// <returns>xPath</returns>
		protected string GetAttributeXPath(string tableType, ATTRIBUTE_TYPE attr)
		{
			return GetTableXPath(tableType) + "//tr[contains(@data-attr-key,'" + attributeDict[attr] + "')]/td";
		}



		public enum ATTRIBUTE_TYPE
		{
			Interpretation, InterpretationSource, Topic, Domain, Comment,
			Multimedia, Image, Example, Source, Gender, Number, PartOfSpeech, Context, ContextSource, Status, Label
		};
		public enum FIELD_TYPE
		{
			Media, Date, Image, Choice, MultipleChoice, Number, Text, Boolean
		};

		protected const string FORM_XPATH = ".//div[contains(@class,'js-popup-edit-structure')]";
		protected const string TABLE_CLASS = "//table[contains(@class,'js-predefined-attrs-table')]";
		protected const string CONCEPT_TABLE_CLASS = "concept";
		protected const string TABLE_NOT_HIDDEN_TR_XPATH = "//tr[contains(@class,'js-attr-row') and not(contains(@class,'g-hidden'))]/td[1]";

		protected const string SELECTED_ROW_XPATH = "//tr[contains(@class,'selected-row')]";

		protected const string ADD_TO_LIST_BTN_XPATH = "//span[contains(@class,'js-add-tbx-attribute')]";
		protected const string SAVE_STRUCTURE_BTN_XPATH = "//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]";

		protected const string DEFAULT_INPUT_XPATH = "//td[contains(@class,'js-default-editor-placeholder')]//input[contains(@class,'js-submit-input')]";
		protected const string CUSTOM_TAB_REF_XPATH = "//a[contains(@class,'js-type-tab js-custom-tab')]";
		protected const string CUSTOM_TABLE_XPATH = "//table[contains(@class,'l-editgloss__tblEditStructure')]";
		protected const string CUSTOM_FIELD_NAME_XPATH = "//div[contains(@class,'js-custom-attrs')]//input[contains(@class,'js-name')]";
		protected const string CUSTOM_TYPE_DROPDOWN_XPATH = CUSTOM_TABLE_XPATH + "//span[contains(@class,'js-dropdown__text type')]";
		protected const string CUSTOM_TYPE_ITEM_XPATH = "//span[contains(@class,'js-dropdown__item type')]";
		protected const string REQUIRED_CHECKBOX_XPATH = "//input[contains(@class,'js-required')]";
		protected const string ADD_CUSTOM_BTN_XPATH = "//span[contains(@class,'js-add-custom-attribute')]";
		protected const string CUSTOM_ATTR_ERROR_EMPTY_CHOICE_XPATH = CUSTOM_TABLE_XPATH + "//div[contains(@class,'js-attribute-errors')]//p[contains(@class,'js-error-no-choices')]";
		protected const string CUSTOM_CHOICE_VALUE_XPATH = CUSTOM_TABLE_XPATH + "//input[contains(@class,'js-choice-values')]";
		protected const string CUSTOM_ATTR_ERROR_EMPTY_DEFAULT_XPATH = "//div[contains(@class,'js-attribute-errors')]//p[contains(@class,'js-error-default-value')]";

		protected const string LEVEL_DROPDOWN_XPATH = "//span[contains(@class,'js-dropdown__text level')]";
		protected const string LEVEL_ITEM_XPATH = "//span[contains(@class,'js-dropdown__list level')]";
		protected const string LEVEL_LANGUAGE_XPATH = LEVEL_ITEM_XPATH + "//span[@data-id='language']";
		protected const string LEVEL_TERM_XPATH = LEVEL_ITEM_XPATH + "//span[@data-id='term']";

		public Dictionary<ATTRIBUTE_TYPE, string> attributeDict;
		protected const string ATTRIBUTE_INTERPRETATION = "Interpretation";
		protected const string ATTRIBUTE_INTERPRETATION_SRC = "InterpretationSource";
		protected const string ATTRIBUTE_TOPIC = "Topic";
		protected const string ATTRIBUTE_DOMAIN = "Domain";
		protected const string ATTRIBUTE_COMMENT = "Comment";
		protected const string ATTRIBUTE_MEDIA = "Multimedia";
		protected const string ATTRIBUTE_IMAGE = "Image";
		protected const string ATTRIBUTE_EXAMPLE = "Example";
		protected const string ATTRIBUTE_SOURCE = "Source";
		protected const string ATTRIBUTE_GENDER = "Gender";
		protected const string ATTRIBUTE_NUMBER = "Number";
		protected const string ATTRIBUTE_PART_OF_SPEECH = "PartOfSpeech";
		protected const string ATTRIBUTE_CONTEXT = "Context";
		protected const string ATTRIBUTE_CONTEXT_SRC = "ContextSource";
		protected const string ATTRIBUTE_STATUS = "Status";
		protected const string ATTRIBUTE_LABEL = "Label";

		protected Dictionary<FIELD_TYPE, int> fieldTypeDict;
		protected const int FIELD_MEDIA = 5;
		protected const int FIELD_DATE = 4;
		protected const int FIELD_IMAGE = 6;
		protected const int FIELD_CHOICE = 7;
		protected const int FIELD_MULTI_CHOICE = 8;
		protected const int FIELD_NUMBER = 2;
		protected const int FIELD_TEXT = 1;
		protected const int FIELD_BOOL = 3;
	}
}