using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.IO;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы глоссариев
	/// </summary>
	public class GlossaryPageHelper : CommonHelper
	{
		public GlossaryPageHelper(IWebDriver driver, WebDriverWait wait)
			: base(driver, wait)
		{
		}

		public void WaitPageLoad()
		{
			Logger.Trace("Проверка успешной загрузки страницы глоссария");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(ADD_CONCEPT_XPATH)), 
				"Ошибка: не зашли в глоссарий");
		}

		public void OpenEditGlossaryList()
		{
			Logger.Debug("Открыть редактирование глоссария");
			ClickElement(By.XPath(OPEN_EDIT_GLOSSARY_LIST_XPATH));
		}

		public void ClickNewItemBtn()
		{
			Logger.Debug("Нажать кнопку New Item");
			ClickElement(By.XPath(ADD_CONCEPT_XPATH));
		}

		public void ClickOpenProperties()
		{
			Logger.Debug("Кликнуть кнопку открытия настроек");
			ClickElement(By.XPath(OPEN_EDIT_PROPERTIES_FORM_BTN_XPATH));
		}

		public void WaitOpenGlossaryProperties()
		{
			Logger.Debug("Дождаться открытия настроек");
			WaitUntilDisplayElement(By.XPath(EDIT_PROPERTIES_FORM));
		}
		
		public void AssertionConceptTableAppear()
		{
			Logger.Debug("Проверка появления таблицы для ввода нового термина в нерасширенном режиме");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(CONCEPT_TABLE_XPATH)),
				"Ошибка: таблица для ввода нового термина в нерасширенном режиме не появилась");
		}

		public void FillTerm(int termNum, string text)
		{
			Logger.Debug(string.Format("Заполнить термин. Номер (первый язык или второй): {0}; текст: {1}", termNum, text));
			ClearAndAddText(By.XPath(CONCEPT_EDITING_TD_XPATH + "[" + termNum + "]" + INPUT_TERM_XPATH), text);
		}

		public void ClickSaveTermin()
		{
			Logger.Debug("Кликнуть кнопку сохранения термина");
			ClickElement(By.XPath(EDIT_CONCEPT_SAVE_BTN_XPATH));
		}

		public void AssertionConceptGeneralSave()
		{
			Logger.Trace("Ожидание сохранения термина в обычном режиме");

			Assert.IsTrue(WaitUntilDisappearElement(By.XPath(EDIT_CONCEPT_SAVE_BTN_XPATH)), 
				"Ошибка: термин не сохранился");
		}


		protected const string IMPORT_TERMS = "//form[contains(@action,'Enterprise/Glossaries/Import')]//input[contains(@class,'js-submit-input')]";

		protected const string ADD_CONCEPT_XPATH = "//div[contains(@class,'js-add-concept')]";
		protected const string OPEN_EDIT_GLOSSARY_LIST_XPATH = "//span[contains(@class,'js-edit-submenu')]";
		protected const string OPEN_EDIT_STRUCTURE_FORM_BTN_XPATH = "//div[contains(@class,'js-edit-structure-btn')]";
		protected const string OPEN_EDIT_PROPERTIES_FORM_BTN_XPATH = "//div[contains(@class,'js-edit-glossary-btn')]";

		protected const string NEW_ITEM_CONCEPT_PART_XPATH = "//div[contains(@class,'js-concept-attrs')]";
		protected const string NEW_ITEM_EDIT_DIV_XPATH = "//div[contains(@class,'js-edit')]";
		protected const string NEW_ITEM_VIEW_DIV_XPATH = "//div[contains(@class,'l-corpr__viewmode__view')]";
		protected const string NEW_ITEM_CONTROL_XPATH = "//div[contains(@class,'js-control')]";

		protected const string NEW_ITEM_DOMAIN_FIELD_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_EDIT_DIV_XPATH + "//select[@name='Domain']/..//span[contains(@class,'js-dropdown')]";

		protected const string ITEM_TERMS_EXTENDED_XPATH = "//div[contains(@class, 'js-terms-tree')]//div[contains(@class, 'l-corprtree__langbox')]";
		protected const string ITEM_ADD_EXTENDED_XPATH = ITEM_TERMS_EXTENDED_XPATH + "//span[contains(@class,'js-add-term')]";
		protected const string ITEM_EDITOR_INPUT_XPATH = "//span[contains(@class,'js-term-editor')]//input";

		protected const string CONCEPT_TABLE_XPATH = "//td[1]//span[contains(@class,'js-add-term')]";
		protected const string CONCEPT_EDITING_TD_XPATH = "//tr[contains(@class, 'js-concept')]//td";
		protected const string CONCEPT_EDITING_OPENED = "//tr[contains(@class, 'js-concept-row js-editing opened')]";
		protected const string EDIT_CONCEPT_SAVE_BTN_XPATH = CONCEPT_EDITING_OPENED + "//a[contains(@class, 'js-save-btn')]";
		protected const string CANCEL_EDIT_CONCEPT_BTN_XPATH = CONCEPT_EDITING_OPENED + "//a[contains(@class, 'js-cancel-btn')]";
		protected const string DELETE_CONCEPT_BTN_XPATH = CONCEPT_ROW_XPATH + "//a[contains(@class, 'js-delete-btn')]";
		protected const string DELETE_CONCEPT_BTN_BY_SOURCE_AND_TARGET = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '**')]//a[contains(@class, 'js-delete-btn')]";
		protected const string CONCEPT_ROW_XPATH = "//tr[contains(@class, 'js-concept-row')]";
		protected const string OPENED_CONCEPT_ROW_XPATH = "//tr[@class='js-concept-panel']/preceding-sibling::tr[1]";
		protected const string SAVE_EXTENDED_BTN_XPATH = "//table[contains(@class,'js-concepts')]//span[contains(@class,'js-save-btn')]";
		
		protected const string CUSTOM_FIELD_BOOL_EDIT_CONCEPT_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_EDIT_DIV_XPATH + "//span[contains(@class,'l-editgloss__name')]";
		protected const string CUSTOM_FIELD_BOOL_CHECKBOX_XPATH = "/..//span[contains(@class,'js-chckbx')]//input[contains(@class,'js-chckbx__orig')]";
		protected const string INPUT_FIELD_VALUE_XPATH = "/..//input[contains(@class,'js-submit-input')]";

		protected const string CUSTOM_FIELD_EDIT_CONCEPT_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_EDIT_DIV_XPATH + "//p";
		protected const string CUSTOM_FIELD_VIEW_CONCEPT_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_VIEW_DIV_XPATH + "//p";
		protected const string CUSTOM_ERROR_FIELD_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + "//div[contains(@class,'js-edit l-error')]//p";

		protected const string FIELD_DIV_VALUE_XPATH = "/../../div[contains(@class,'js-view')]//div";
		protected const string MULTISELECT_FIELD_VALUE_XPATH = "/..//div[contains(@class,'ui-multiselect')]";
		protected const string MULTISELECT_ITEM_XPATH = "//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')]";
		protected const string NUMBER_FIELD_VALUE_XPATH = "/../div[contains(@class,'js-value')]";
		protected const string CHOICE_FIELD_DROPDOWN_XPATH = "/..//span[contains(@class,'js-dropdown')]";
		protected const string FIELD_IMAGE_FIELD_XPATH = "/..//div[contains(@class,'l-editgloss__imagebox')]//a";
		protected const string CUSTOM_IMAGE_EXIT_XPATH = "/../../div[contains(@class,'l-editgloss__image')]//img[contains(@class,'l-editgloss__imageview')]";
		protected const string CUSTOM_FIELD_IMAGE_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_CONTROL_XPATH + "//p";
		protected const string CUSTOM_ERROR_FIELD_IMAGE_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_CONTROL_XPATH + "[contains(@class,'l-error')]//p";
		protected const string CUSTOM_FIELD_MEDIA_REF_XPATH = "/..//span[contains(@class,'l-editgloss__linkbox')]" + UPLOAD_BTN_XPATH;
		protected const string UPLOAD_BTN_XPATH = "//a[contains(@class,'js-upload-btn')]";
		protected const string FIELD_MEDIA_CONTAINS_XPATH = "/../div[contains(@class,'l-editgloss__filemedia')]//a[contains(@class,'l-editgloss__filelink')]";
		protected const string CUSTOM_FIELD_DATE_XPATH = "/../input[contains(@class,'hasDatepicker')]";
		protected const string CALEDNAR_XPATH = "//table[contains(@class,'ui-datepicker-calendar')]";
		protected const string CALENDAR_TODAY_XPATH = CALEDNAR_XPATH + "//td[contains(@class,'ui-datepicker-today')]";
		protected const string CUSTOM_FIELD_TEXT_XPATH = "/..//textarea";

		protected const string TEXTAREA_FIELD_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_CONTROL_XPATH + "//textarea";
		protected const string INPUT_FIELD_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_CONTROL_XPATH + "//input";
		protected const string INPUT_IMPORT_LINK_XPATH = "/..//span[contains(@class,'l-editgloss__linkbox')]//a";
		protected const string SELECT_FIELD_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_EDIT_DIV_XPATH + "//select";
		protected const string TOPIC_FIELD_DROPDOWN_XPATH = "/..//div[contains(@class,'ui-dropdown-treeview-wrapper')]/div[1]//span";

		protected const string CHOICE_DROPDOWN_LIST = "//span[contains(@class,'js-dropdown__list')]//span";
		protected const string CHOICE_LIST_XPATH = CHOICE_DROPDOWN_LIST + "[contains(@class,'js-dropdown__item')]";
		protected const string TOPIC_CHOICE_XPATH = "//div[contains(@class,'ui-dropdown-treeview_dropDown')]";
		protected const string TOPIC_ITEM_XPATH = "//div[contains(@class,'ui-treeview_node')]//div/span";

		protected const string LANGUAGE_ROW_XPATH = "//div[contains(@class,'js-node js-lang-node')]";
		protected const string DETAILS_XPATH = "//td[contains(@class,'js-details-panel')]";
		protected const string DETAILS_TEXTAREA_XPATH = DETAILS_XPATH + "//textarea";
		protected const string DETAILS_TEXTAREA_VALUE = "/../..//div[contains(@class,'js-value')]";

		protected const string DETAILS_SELECT_XPATH = DETAILS_XPATH + "//select";
		protected const string DETAILS_SELECT_OPTION_XPATH = "//option";

		protected const string TERM_ROW_XPATH = "//div[contains(@class,'js-term-node')]";

		protected const string EDIT_BTN_XPATH = "//span[contains(@class,'js-edit-btn')]";
		protected const string EDIT_TERM_BTN_XPATH = "//a[contains(@class,'js-edit-btn')]";
		protected const string TERM_COMMENT_TD = "//td[contains(@class,'glossaryComment')]";
		protected const string INPUT_TERM_XPATH = "//input[contains(@class,'js-term')]";
		protected const string TURN_OFF_BTN_XPATH = "//a[contains(@class,'iconup')]";

		protected const string SEARCH_INPUT_XPATH = "//input[contains(@class,'js-search-term')]";
		protected const string SEARCH_BTN_XPATH = "//a[contains(@class,'js-search-by-term')]";
		protected const string TERM_TEXT_XPATH = CONCEPT_ROW_XPATH + "//td[contains(@class,'glossaryShort')]//p";
		protected const string TERM_ERROR_XPATH = "//p[contains(@class,'l-error')]";
		protected const string TERM_ERROR_MESSAGE_XPATH = "//div[contains(@class,'js-term-box')][contains(@class,'l-error')]";
		protected const string SINGLE_SOURCE_TERM_XPATH = CONCEPT_ROW_XPATH + "//td[1][contains(string(), '#')]/following-sibling::td[1]//p";
		protected const string SINGLE_TARGET_TERM_XPATH = CONCEPT_ROW_XPATH + "//td[1]/p../following-sibling::td[1][contains(string(), '#')]";
		protected const string COMMENT_XPATH = CONCEPT_ROW_XPATH + "//td[contains(@class, 'glossaryComment') and contains(string(), '#')]";
		protected const string SOURCE_TARGET_TERM_XPATH = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '**')]";
		protected const string SOURCE_TARGET_TERM_COMMENT_RAW_XPATH = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '**')]//td[4]";

		protected const string ADD_SYNONYM_BTN_XPATH = "//span[contains(@class,'js-add-term')]";
		protected const string SYNONYM_INPUT_XPATH = "//div" + INPUT_TERM_XPATH;

		protected const string GLOSSARY_ERROR_XPATH = "//tr//td[contains(@class,'glossaryError')]";
		protected const string DUPLICATE_ERROR_XPATH = "//div[contains(@class,'js-popup-confirm')]//form//span[contains(@class,'duplicate-term')]";

		protected const string IMPORT_BTN_XPATH = "//span[contains(@class,'js-import-concepts')]";
		protected const string IMPORT_FORM_XPATH = "//div[contains(@class,'js-popup-import')][2]";

		protected const string EXPORT_BTN_XPATH = "//a[contains(@href,'/Glossaries/Export')]";
		protected const string REPLACE_ALL_XPATH = IMPORT_FORM_XPATH + "//input[contains(@id,'needToClear')][@value='True']";
		protected const string IMPORT_FORM_IMPORT_BTN_XPATH = IMPORT_FORM_XPATH + "//span[contains(@class,'js-import-button')]";
		protected const string SUCCESS_RESULT_CLOSE_BTN_XPATH = "//a[contains(@class,'js-close-link')]";

		protected const string HREF_EXPORT = "//a[contains(@class,'js-export-concepts')]";
		protected const string NEW_ITEM_OPEN = "//div[@class='l-corprtree']";
		protected const string EDIT_PROPERTIES_FORM = ".//div[contains(@class,'js-popup-edit-glossary')][2]";
		protected const string GLOSSARY_TERM = "//div[@class='l-corprtree__langbox'][2]";
		protected const string EDIT_GLOSSARY_LIST_XPATH = "//div[contains(@class,'js-edit-submenu-list')]";
	}
}