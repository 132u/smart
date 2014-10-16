using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
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
			attributeDict = new Dictionary<ATTRIBUTE_TYPE, string>();
			attributeDict.Add(ATTRIBUTE_TYPE.Interpretation, ATTRIBUTE_INTERPRETATION);
			attributeDict.Add(ATTRIBUTE_TYPE.InterpretationSource, ATTRIBUTE_INTERPRETATION_SRC);
			attributeDict.Add(ATTRIBUTE_TYPE.Topic, ATTRIBUTE_TOPIC);
			attributeDict.Add(ATTRIBUTE_TYPE.Domain, ATTRIBUTE_DOMAIN);
			attributeDict.Add(ATTRIBUTE_TYPE.Comment, ATTRIBUTE_COMMENT);
			attributeDict.Add(ATTRIBUTE_TYPE.Multimedia, ATTRIBUTE_MEDIA);
			attributeDict.Add(ATTRIBUTE_TYPE.Image, ATTRIBUTE_IMAGE);
			attributeDict.Add(ATTRIBUTE_TYPE.Example, ATTRIBUTE_EXAMPLE);
			attributeDict.Add(ATTRIBUTE_TYPE.Source, ATTRIBUTE_SOURCE);
			attributeDict.Add(ATTRIBUTE_TYPE.Gender, ATTRIBUTE_GENDER);
			attributeDict.Add(ATTRIBUTE_TYPE.Number, ATTRIBUTE_NUMBER);
			attributeDict.Add(ATTRIBUTE_TYPE.PartOfSpeech, ATTRIBUTE_PART_OF_SPEECH);
			attributeDict.Add(ATTRIBUTE_TYPE.Context, ATTRIBUTE_CONTEXT);
			attributeDict.Add(ATTRIBUTE_TYPE.ContextSource, ATTRIBUTE_CONTEXT_SRC);
			attributeDict.Add(ATTRIBUTE_TYPE.Status, ATTRIBUTE_STATUS);
			attributeDict.Add(ATTRIBUTE_TYPE.Label, ATTRIBUTE_LABEL);

			fieldTypeDict = new Dictionary<FIELD_TYPE, string>();
			fieldTypeDict.Add(FIELD_TYPE.Media, FIELD_MEDIA);
			fieldTypeDict.Add(FIELD_TYPE.Date, FIELD_DATE);
			fieldTypeDict.Add(FIELD_TYPE.Image, FIELD_IMAGE);
			fieldTypeDict.Add(FIELD_TYPE.Choice, FIELD_CHOICE);
			fieldTypeDict.Add(FIELD_TYPE.MultipleChoice, FIELD_MULTI_CHOICE);
			fieldTypeDict.Add(FIELD_TYPE.Number, FIELD_NUMBER);
			fieldTypeDict.Add(FIELD_TYPE.Text, FIELD_TEXT);
			fieldTypeDict.Add(FIELD_TYPE.Boolean, FIELD_BOOL);
		}

		/// <summary>
		/// Дождаться появления формы
		/// </summary>
		/// <returns>появилась</returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(FORM_XPATH));
		}

		/// <summary>
		/// Дождаться закрытия формы
		/// </summary>
		/// <returns>закрылась</returns>
		public bool WaitFormClose()
		{
			return WaitUntilDisappearElement(By.XPath(FORM_XPATH));
		}

		/// <summary>
		/// Вернуть, видна ли таблица Concept
		/// </summary>
		/// <returns>видна</returns>
		public bool GetIsConceptTableDisplay()
		{
			return GetIsElementDisplay(By.XPath(GetTableXPath(CONCEPT_TABLE_CLASS)));
		}

		/// <summary>
		/// Кликнуть по полю для добавления
		/// </summary>
		/// <param name="attr">поле</param>
		/// <returns>поле выделено</returns>
		public bool ClickFieldToAdd(ATTRIBUTE_TYPE attr)
		{
			string xpath = GetAttributeXPath(CONCEPT_TABLE_CLASS, attr);

			bool isDisplayed = GetIsElementDisplay(By.XPath(xpath));
			if (isDisplayed)
			{
				ClickElement(By.XPath(GetAttributeXPath(CONCEPT_TABLE_CLASS, attr)));
			}
			return isDisplayed && GetIsElementDisplay(By.XPath(SELECTED_ROW_XPATH));
		}

		/// <summary>
		/// Кликнуть Добавить в список
		/// </summary>
		public void ClickAddToListBtn()
		{
			ClickElement(By.XPath(ADD_TO_LIST_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Сохранить
		/// </summary>
		public void ClickSaveStructureBtn()
		{
			ClickElement(By.XPath(SAVE_STRUCTURE_BTN_XPATH));
		}

		/// <summary>
		/// Выбрать все поля таблицы
		/// </summary>
		public void SelectAllFields()
		{
			// Список видимых элементов таблицы
			SetDriverTimeoutMinimum();
			IList<IWebElement> attrList = GetElementList(By.XPath(GetVisibleTableXPath() + TABLE_NOT_HIDDEN_TR_XPATH));
			SetDriverTimeoutDefault();
			foreach(IWebElement el in attrList)
			{
				// Кликнуть по элементу
				el.Click();
				// Кликнуть добавить
				ClickAddToListBtn();
			}
		}

		/// <summary>
		/// Ввести значение по умолчанию
		/// </summary>
		/// <param name="value">значение</param>
		public void AddDefaultValue(string value)
		{
			ClearAndAddText(By.XPath(DEFAULT_INPUT_XPATH), value);
		}

		/// <summary>
		/// Перейти на вкладку Пользовательские
		/// </summary>
		/// <returns>открылась</returns>
		public bool SwitchCustomTab()
		{
			ClickElement(By.XPath(CUSTOM_TAB_REF_XPATH));
			return WaitUntilDisplayElement(By.XPath(CUSTOM_TABLE_XPATH));
		}

		/// <summary>
		/// Заполнить название пользовательского поля
		/// </summary>
		/// <param name="name">название</param>
		public void FillNameCustomField(string name)
		{
			SendTextElement(By.XPath(CUSTOM_FIELD_NAME_XPATH), name);
		}

		/// <summary>
		/// Выбрать тип поля в пользовательских настройках
		/// </summary>
		/// <param name="field">поле</param>
		public void SelectCustomFieldType(FIELD_TYPE field)
		{
			ClickElement(By.XPath(CUSTOM_TYPE_DROPDOWN_XPATH));
			ClickElement(By.XPath(CUSTOM_TYPE_ITEM_XPATH + "[@data-id='" + fieldTypeDict[field] + "']"));
		}

		/// <summary>
		/// Кликнуть галочку Required Field
		/// </summary>
		public void SelectRequiredCheckbox()
		{
			ClickElement(By.XPath(REQUIRED_CHECKBOX_XPATH));
		}

		/// <summary>
		/// Кликнуть добавить пользовательское поле
		/// </summary>
		public void ClickAddCustomAttribute()
		{
			ClickElement(By.XPath(ADD_CUSTOM_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли ошибка пустого списка выбора
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsDisplayedErrorEmptyChoice()
		{
			return GetIsElementDisplay(By.XPath(CUSTOM_ATTR_ERROR_EMPTY_CHOICE_XPATH));
		}

		/// <summary>
		/// Заполнить список выбора
		/// </summary>
		/// <param name="values">значения списка</param>
		public void FillChoiceValues(string values)
		{
			SendTextElement(By.XPath(CUSTOM_CHOICE_VALUE_XPATH), values);
		}

		/// <summary>
		/// Вернуть, есть ли ошибка пользовательского поля: пустое значение по умолчанию
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsExistCustomAttrErrorEmptyDefault()
		{
			return GetIsElementDisplay(By.XPath(CUSTOM_ATTR_ERROR_EMPTY_DEFAULT_XPATH));
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

		protected Dictionary<FIELD_TYPE, string> fieldTypeDict;
		protected const string FIELD_MEDIA = "Media";
		protected const string FIELD_DATE = "Date";
		protected const string FIELD_IMAGE = "Image";
		protected const string FIELD_CHOICE = "Choice";
		protected const string FIELD_MULTI_CHOICE = "MultipleChoice";
		protected const string FIELD_NUMBER = "Number";
		protected const string FIELD_TEXT = "Text";
		protected const string FIELD_BOOL = "Boolean";
	}
}