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
	/// Хелпер страницы глоссариев
	/// </summary>
	public class GlossaryPageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public GlossaryPageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
		}

		/// <summary>
		/// Дождаться загрузки страницы
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(ADD_CONCEPT_XPATH));
		}

		/// <summary>
		/// Открыть редактирование глоссария
		/// </summary>
		public void OpenEditGlossaryList()
		{
			ClickElement(By.XPath(OPEN_EDIT_GLOSSARY_LIST_XPATH));
		}

		/// <summary>
		/// Открыть форму редактирования структуры
		/// </summary>
		public void OpenEditStructureForm()
		{
			ClickElement(By.XPath(OPEN_EDIT_STRUCTURE_FORM_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть New Item
		/// </summary>
		public void ClickNewItemBtn()
		{
			ClickElement(By.XPath(ADD_CONCEPT_XPATH));
		}

		/// <summary>
		/// Кликнуть по полю Domain в новом термине
		/// </summary>
		public void NewItemClickDomainField()
		{
			ClickElement(By.XPath(NEW_ITEM_DOMAIN_FIELD_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли domain в списке в термине
		/// </summary>
		/// <param name="domainName">название</param>
		/// <returns>есть</returns>
		public bool GetIsDomainExistInItemDomainList(string domainName)
		{
			// Получить список проектов в списке
			IList<IWebElement> DomainList = GetElementList(By.XPath(CHOICE_LIST_XPATH));
			bool isDomainExist = false;
			foreach (IWebElement el in DomainList)
			{
				if (el.Text == domainName)
				{
					// Если проект в списке
					isDomainExist = true;
					break;
				}
			}
			return isDomainExist;
		}

		/// <summary>
		/// Кликнуть открыть настройки
		/// </summary>
		public void ClickOpenProperties()
		{
			ClickElement(By.XPath(OPEN_EDIT_PROPERTIES_FORM_BTN_XPATH));
		}

		/// <summary>
		/// Заполнить термины в расширенной версии
		/// </summary>
		/// <param name="text">текст</param>
		public void FillItemTermsExtended(string text)
		{
			// Поля языков
			IList<IWebElement> termList = GetElementList(By.XPath(ITEM_ADD_EXTENDED_XPATH));
			for (int i = 0; i < termList.Count; ++i)
			{
				// Нажать Add
				termList[i].Click();
				// Ввести термин
				ClearAndAddText(By.XPath(ITEM_TERMS_EXTENDED_XPATH + "[" + (i + 1) + "]" + ITEM_EDITOR_INPUT_XPATH), text);
			}
		}

		/// <summary>
		/// Изменить термины в расширенной версии
		/// </summary>
		/// <param name="text">текст</param>
		public void EditTermsExtended(string text)
		{
			// Поля языков
			IList<IWebElement> termList = GetElementList(By.XPath(ITEM_TERMS_EXTENDED_XPATH + TERM_ROW_XPATH));
			for (int i = 0; i < termList.Count; ++i)
			{
				// Нажать Add
				termList[i].Click();
				// Ввести термин
				ClearAndAddText(By.XPath(ITEM_TERMS_EXTENDED_XPATH + "[" + (i + 1) + "]" + ITEM_EDITOR_INPUT_XPATH), text);
			}
		}

		/// <summary>
		/// Дождаться появления таблицы для ввода нового термина в нерасширенном режиме
		/// </summary>
		/// <returns>появилась</returns>
		public bool WaitConceptTableAppear()
		{
			return WaitUntilDisplayElement(By.XPath(CONCEPT_TABLE_XPATH));
		}

		/// <summary>
		/// Заполнить термин
		/// </summary>
		/// <param name="termNum">номер (первый язык или второй)</param>
		/// <param name="text">текст</param>
		public void FillTerm(int termNum, string text)
		{
			ClearAndAddText(By.XPath(CONCEPT_EDITING_TD_XPATH + "[" + termNum + "]" + INPUT_TERM_XPATH), text);
		}

		/// <summary>
		/// Кликнуть сохранить термин
		/// </summary>
		public void ClickSaveTermin()
		{
			ClickElement(By.XPath(EDIT_CONCEPT_SAVE_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться сохранения термина в обычном режиме
		/// </summary>
		/// <returns></returns>
		public bool WaitConceptGeneralSave()
		{
			return WaitUntilDisappearElement(By.XPath(EDIT_CONCEPT_SAVE_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться удаления термина в обычном режиме
		/// </summary>
		/// <returns></returns>
		public bool WaitConceptGeneralDelete()
		{
			return WaitUntilDisappearElement(By.XPath(DELETE_CONCEPT_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть количество терминов
		/// </summary>
		/// <returns>количество</returns>
		public int GetConceptCount()
		{
			SetDriverTimeoutMinimum();
			int result = GetElementsCount(By.XPath(CONCEPT_ROW_XPATH));
			SetDriverTimeoutDefault();
			return result;
		}

		/// <summary>
		/// Кликнить Сохранить в расширенной форме термина
		/// </summary>
		public void ClickSaveExtendedConcept()
		{
			ClickElement(By.XPath(SAVE_EXTENDED_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться появление открытого сохраненного термина
		/// </summary>
		/// <returns>открылся</returns>
		public bool WaitConceptSave()
		{
			return WaitUntilDisplayElement(By.XPath(OPENED_CONCEPT_ROW_XPATH));
		}

		///////////////////// Пользовательское поле Boolean
		/// <summary>
		/// Вернуть, есть ли пользовательское поле Boolean при создании термина
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>есть</returns>
		public bool GetIsExistCustomFieldBool(string fieldName)
		{
			return GetIsElementDisplay(By.XPath(GetCustomFieldBoolXPath(fieldName)));
		}

		/// <summary>
		/// Нажать checkbox пользовательского поля Boolean
		/// </summary>
		/// <param name="fieldName">название поля</param>
		public void ClickCustomFieldBool(string fieldName)
		{
			ClickElement(By.XPath(GetCustomFieldBoolXPath(fieldName) + CUSTOM_FIELD_BOOL_CHECKBOX_XPATH));
		}

		/// <summary>
		/// Вернуть, отмечена ли галочка пользовательского поля Boolean
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>отмечена</returns>
		public bool GetIsCustomBooleanChecked(string fieldName)
		{
			return GetElementAttribute(By.XPath(GetCustomFieldBoolXPath(fieldName) + INPUT_FIELD_VALUE_XPATH), "value") == "true";
		}

		///////////////////// Пользовательское поле Boolean
		/// <summary>
		/// Вернуть, есть ли пользовательское поле
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>есть</returns>
		public bool GetIsExistCustomField(string fieldName)
		{
			return GetIsElementDisplay(By.XPath(GetCustomFieldXPath(fieldName)));
		}

		/// <summary>
		/// Вернуть: отмечено ли поле ошибкой
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>отмечено</returns>
		public bool GetIsExistCustomFieldError(string fieldName)
		{
			return GetIsElementDisplay(By.XPath(GetCustomFieldErrorXPath(fieldName)));
		}

		//////////////////////////////////// Пользовательское поле MultiSelect
		/// <summary>
		/// Кликнуть на поле множественного выбора
		/// </summary>
		/// <param name="fieldName">название поля</param>
		public void ClickCustomFieldMultiSelect(string fieldName)
		{
			ClickElement(By.XPath(GetCustomFieldXPath(fieldName) + MULTISELECT_FIELD_VALUE_XPATH));
		}

		/// <summary>
		/// Выбрать элемент множественного выбора
		/// </summary>
		/// <param name="item">элемент</param>
		public void SelectItemMultiSelect(string item)
		{
			ClickElement(By.XPath(MULTISELECT_ITEM_XPATH + "[text()='" + item + "']"));
		}
		
		//////////////////////////////////// Пользовательское поле MultiSelect
		/// <summary>
		/// Получить значение пользовательского поля
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>значение</returns>
		public string GetCustomFieldValue(string fieldName)
		{
			return GetTextElement(By.XPath(GetCustomFieldXPath(fieldName) + FIELD_DIV_VALUE_XPATH));
		}

		//////////////////////////////////// Пользовательское поле Number
		/// <summary>
		/// Ввести значение в пользовательское поле нового термина - Number
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <param name="value">значение</param>
		public void FillCustomFieldNumber(string fieldName, string value)
		{
			SendTextElement(By.XPath(GetCustomFieldXPath(fieldName) + INPUT_FIELD_VALUE_XPATH), value);
		}

		/// <summary>
		/// Вернуть значение из пользовательского поля Number
		/// </summary>
		/// <param name="fieldName">поле</param>
		/// <returns>значение</returns>
		public string GetCustomFieldNumberValue(string fieldName)
		{
			return GetTextElement(By.XPath(GetCustomFieldViewXPath(fieldName) + NUMBER_FIELD_VALUE_XPATH));
		}
		
		//////////////////////////////////// Пользовательское поле Number

		//////////////////////////////////// Пользовательское поле List/Choice
		/// <summary>
		/// Кликнуть по пользовательскому полю выбора
		/// </summary>
		/// <param name="fieldName">название поля</param>
		public void ClickCustomFieldChoice(string fieldName)
		{
			ClickElement(By.XPath(GetCustomFieldXPath(fieldName) + CHOICE_FIELD_DROPDOWN_XPATH));
		}

		/// <summary>
		/// Выбрать элемент списка
		/// </summary>
		/// <param name="item">элемент</param>
		public void SelectChoiceItem(string item)
		{
			ClickElement(By.XPath(CHOICE_LIST_XPATH + "[@title='" + item + "']"));
		}
		
		//////////////////////////////////// Пользовательское поле List/Choice

		//////////////////////////////////// Пользовательское поле Image
		/// <summary>
		/// Вернуть, есть ли пользовательское поле Изображение
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>есть</returns>
		public bool GetIsCustomFieldImageExist(string fieldName)
		{
			return GetIsElementDisplay(By.XPath(GetCustomFieldImageXPath(fieldName)));
		}

		/// <summary>
		/// Вернуть: отмечено ли пользовательское поле Изображение ошибкой
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>отмечено</returns>
		public bool GetIsExistCustomFieldImageError(string fieldName)
		{
			return GetIsElementDisplay(By.XPath(GetCustomFieldImageErrorXPath(fieldName)));
		}

		/// <summary>
		/// Кликнуть по пользовательскому полю Изображение
		/// </summary>
		/// <param name="fieldName">название поля</param>
		public void ClickCustomFieldImage(string fieldName)
		{
			ClickElement(By.XPath(GetCustomFieldImageXPath(fieldName) + FIELD_IMAGE_FIELD_XPATH));
		}

		/// <summary>
		/// Вернуть, заполнено ли поле Изображение
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>есть изображение</returns>
		public bool GetCustomFieldImageFilled(string fieldName)
		{
			string xPath = GetCustomFieldImageXPath(fieldName) + CUSTOM_IMAGE_EXIT_XPATH;
			return GetElementAttribute(By.XPath(xPath), "src").Trim().Length > 0;
		}
		//////////////////////////////////// Пользовательское поле Image

		//////////////////////////////////// Пользовательское поле Media

		/// <summary>
		/// Кликнуть по пользовательскому полю Media
		/// </summary>
		/// <param name="fieldName"></param>
		public void ClickCustomFieldMedia(string fieldName)
		{
			ClickElement(By.XPath(GetCustomFieldImageXPath(fieldName) + CUSTOM_FIELD_MEDIA_REF_XPATH));
		}

		/// <summary>
		/// Получить, заполнено ли пользовательское поле Media
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>заполнено</returns>
		public bool GetIsCustomFieldMediaFilled(string fieldName)
		{
			string xPath = GetCustomFieldImageXPath(fieldName) + FIELD_MEDIA_CONTAINS_XPATH;

			return GetElementAttribute(By.XPath(xPath), "href").Trim().Length > 0;
		}
		//////////////////////////////////// Пользовательское поле Media

		/////////////////////////////////// Пользовательское поле Дата

		/// <summary>
		/// Кликнуть по пользовательское поле Дата
		/// </summary>
		/// <param name="fieldName">название поля</param>
		public void ClickCustomFieldDate(string fieldName)
		{
			ClickElement(By.XPath(GetCustomFieldXPath(fieldName) + CUSTOM_FIELD_DATE_XPATH));
		}

		/// <summary>
		/// Вернуть - показывается ли календарь
		/// </summary>
		/// <returns>показывается</returns>
		public bool GetIsExistCalendar()
		{
			return GetIsElementDisplay(By.XPath(CALEDNAR_XPATH));
		}
		
		/// <summary>
		/// Выбрать текущую дату
		/// </summary>
		public void SelectCalendarToday()
		{
			ClickElement(By.XPath(CALENDAR_TODAY_XPATH));
		}
		/////////////////////////////////// Пользовательское поле Дата
		
		/////////////////////////////////// Пользовательское поле Текст

		/// <summary>
		/// Заполнить пользовательское поле Текст
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <param name="text">текст</param>
		public void FillCustomFieldText(string fieldName, string text)
		{
			SendTextElement(By.XPath(GetCustomFieldXPath(fieldName) + CUSTOM_FIELD_TEXT_XPATH), text);
		}
		/////////////////////////////////// Пользовательское поле Текст

		/// <summary>
		/// Вернуть: есть ли textarea
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>есть</returns>
		public bool GetIsExistTextarea(string fieldName)
		{
			return GetIsElementDisplay(By.XPath(GetTextareaXPath(fieldName)));
		}

		/// <summary>
		/// Заполнить textarea
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <param name="text">текст</param>
		public void FillTextarea(string fieldName, string text)
		{
			SendTextElement(By.XPath(GetTextareaXPath(fieldName)), text);
		}

		/// <summary>
		/// Вернуть значение textarea
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>текст</returns>
		public string GetTextareaValue(string fieldName)
		{
			return GetTextElement(By.XPath(GetTextareaValueXPath(fieldName)));
		}

		/// <summary>
		/// Вернуть: есть ли input
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>есть</returns>
		public bool GetIsExistInput(string fieldName)
		{
			return GetIsElementExist(By.XPath(GetInputXPath(fieldName)));
		}

		/// <summary>
		/// Кликнуть по полю Media для загрузки
		/// </summary>
		public void ClickMediaToImport(string fieldName)
		{
			ClickElement(By.XPath(GetInputXPath(fieldName) + INPUT_IMPORT_LINK_XPATH));
		}

		/// <summary>
		/// Получить, заполнено ли поле Media
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>заполнено</returns>
		public bool GetIsFieldMediaFilled(string fieldName)
		{
			string xPath = GetInputXPath(fieldName) + FIELD_MEDIA_CONTAINS_XPATH;

			return GetElementAttribute(By.XPath(xPath), "href").Trim().Length > 0;
		}

		/// <summary>
		/// Кликнуть по полю Image для загрузки
		/// </summary>
		public void ClickImageToImport(string fieldName)
		{
			ClickElement(By.XPath(GetInputXPath(fieldName) + FIELD_IMAGE_FIELD_XPATH));
		}

		/// <summary>
		/// Вернуть, заполнено ли поле Изображение
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>есть изображение</returns>
		public bool GetFieldImageFilled(string fieldName)
		{
			string xPath = GetInputXPath(fieldName) + CUSTOM_IMAGE_EXIT_XPATH;
			return GetElementAttribute(By.XPath(xPath), "src").Trim().Length > 0;
		}

		/// <summary>
		/// Вернуть, есть ли поле Select
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>есть</returns>
		public bool GetIsExistSelect(string fieldName)
		{
			return GetIsElementExist(By.XPath(GetSelectXPath(fieldName)));
		}

		/// <summary>
		/// Кликнуть Select для выпадения списка
		/// </summary>
		/// <param name="fieldName">название поля</param>
		public void ClickSelectDropdown(string fieldName)
		{
			ClickElement(By.XPath(GetSelectXPath(fieldName) + CHOICE_FIELD_DROPDOWN_XPATH));
		}

		/// <summary>
		/// Вернуть, раскрылся ли список
		/// </summary>
		/// <returns>открылся</returns>
		public bool GetIsSelectListVisible()
		{
			return GetIsElementDisplay(By.XPath(CHOICE_LIST_XPATH));
		}

		/// <summary>
		/// Получить значение поля Select
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>значение</returns>
		public string GetSelectValue(string fieldName)
		{
			return GetTextElement(By.XPath(GetSelectXPath(fieldName) + FIELD_DIV_VALUE_XPATH));
		}

		/// <summary>
		/// Кликнуть по полю Topic для выпадения списка
		/// </summary>
		/// <param name="fieldName">название поля</param>
		public void ClickTopicDropdown(string fieldName)
		{
			ClickElement(By.XPath(GetInputXPath(fieldName) + TOPIC_FIELD_DROPDOWN_XPATH));
		}

		/// <summary>
		/// Вернуть, виден ли список для Topic
		/// </summary>
		/// <returns>виден</returns>
		public bool GetIsTopicListVisible()
		{
			return GetIsElementDisplay(By.XPath(TOPIC_CHOICE_XPATH));
		}

		/// <summary>
		/// Выбрать элемент Topic
		/// </summary>
		public void SelectTopicItem()
		{
			ClickElement(By.XPath(TOPIC_ITEM_XPATH));
		}

		/// <summary>
		/// Вернуть значение Topic
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>значение</returns>
		public string GetTopicValue(string fieldName)
		{
			return GetTextElement(By.XPath(GetInputXPath(fieldName) + FIELD_DIV_VALUE_XPATH));
		}

		/// <summary>
		/// Открыть атрибуты языка
		/// </summary>
		public void OpenLanguageAttributes()
		{
			ClickElement(By.XPath(LANGUAGE_ROW_XPATH));
		}

		/// <summary>
		/// Вернуть, отображается ли поле в детальных атрибутах
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>отображается</returns>
		public bool GetIsExistDetailsTextarea(string fieldName)
		{
			return GetIsElementDisplay(By.XPath(GetDetailsTextareaXPath(fieldName)));
		}

		/// <summary>
		/// Заполнить textarea в детальных атрибутах
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <param name="text">текст</param>
		public void FillDetailTextarea(string fieldName, string text)
		{
			SendTextElement(By.XPath(GetDetailsTextareaXPath(fieldName)), text);
		}

		/// <summary>
		/// Получить значение textarea детальных атрибутов
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>значение</returns>
		public string GetDetailTextareaValue(string fieldName)
		{
			return GetTextElement(By.XPath(GetDetailsTextareaXPath(fieldName) + DETAILS_TEXTAREA_VALUE));
		}

		/// <summary>
		/// Открыть атрибуты уровня Term
		/// </summary>
		public void OpenTermLevel()
		{
			ClickElement(By.XPath(TERM_ROW_XPATH));
		}

		/// <summary>
		/// Вернуть, отображается ли поле Select в детальных атрибутах
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>отображается</returns>
		public bool GetIsExistDetailsSelect(string fieldName)
		{
			return GetIsElementExist(By.XPath(GetDetailsSelectXPath(fieldName)));
		}

		/// <summary>
		/// Вернуть id из списка select детальных атрибутов
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <param name="optionNumber">номер элемента списка</param>
		/// <returns>id</returns>
		public string GetDetailsSelectOptionID(string fieldName, int optionNumber)
		{
			string xPath = GetDetailsSelectXPath(fieldName) + DETAILS_SELECT_OPTION_XPATH + "[" + optionNumber + "]";
			return GetElementAttribute(By.XPath(xPath), "value");
		}

		/// <summary>
		/// Кликнуть по Select в детальных атрибутах для открытия списка
		/// </summary>
		/// <param name="fieldName">название поля</param>
		public void ClickDetailsSelectDropdown(string fieldName)
		{
			ClickElement(By.XPath(GetDetailsSelectXPath(fieldName) + CHOICE_FIELD_DROPDOWN_XPATH));
		}

		/// <summary>
		/// Кликнуть по элементу списка
		/// </summary>
		/// <param name="optionID">id</param>
		public void ClickListItemByID(string optionID)
		{
			ClickElement(By.XPath(GetSelectOptionText(optionID)));
		}

		/// <summary>
		/// Получить текст элемента списка
		/// </summary>
		/// <param name="optionID">id</param>
		/// <returns>текст</returns>
		public string GetListItemText(string optionID)
		{
			return GetTextElement(By.XPath(GetSelectOptionText(optionID)));
		}

		/// <summary>
		/// Вернуть значение поля Select детальных атрибутов
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>значение</returns>
		public string GetDetailsSelectValue(string fieldName)
		{
			return GetTextElement(By.XPath(GetDetailsSelectXPath(fieldName) + DETAILS_TEXTAREA_VALUE));
		}

		/// <summary>
		/// Вернуть, есть ли термин с таким названием
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public bool GetIsTermExistByText(string text)
		{
			return GetIsElementDisplay(By.XPath(CONCEPT_ROW_XPATH + "//p[contains(text(),'" + text + "')]"));
		}

		/// <summary>
		/// Вернуть, есть ли данный одиночный сорс термин
		/// </summary>
		/// <param name="text">текст</param>
		/// <returns>есть</returns>
		public bool GetIsSingleSourceTermExists(string text)
		{
			if (GetIsExistTerm(text))
			{
				return !GetIsElementDisplay(By.XPath(SINGLE_SOURCE_TERM_XPATH.Replace("#", text)));
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Вернуть, есть ли данный одиночный таргет термин
		/// </summary>
		/// <param name="text">текст</param>
		/// <returns>есть</returns>
		public bool GetIsSingleTargetTermExists(string text)
		{
			if (GetIsExistTerm(text))
			{
				return !GetIsElementDisplay(By.XPath(SINGLE_TARGET_TERM_XPATH.Replace("#", text)));
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Вернуть, есть ли термин с заданными сорсом и таргетом
		/// </summary>
		/// <param name="sourceText">текст сорса</param>
		/// <param name="targetText">текст таргета</param>
		/// <returns>есть</returns>
		public bool GetIsSourceTargetTermExists(string sourceText, string targetText)
		{
			return GetIsElementDisplay(By.XPath(SOURCE_TARGET_TERM_XPATH.Replace("#", sourceText).Replace("**", targetText)));
		}

		/// <summary>
		/// Вернуть, есть ли термин с заданным комментарием
		/// </summary>
		/// <param name="text">текст комментария</param>	   
		/// <returns>есть</returns>
		public bool GetIsCommentExists(string text)
		{
			return GetIsElementDisplay(By.XPath(COMMENT_XPATH.Replace("#", text)));
		}

		/// <summary>
		/// Вернуть, есть ли два одинаковых заданных термина
		/// </summary>
		/// <param name="sourceText">текст сорса</param>
		/// <param name="targetText">текст таргета</param>
		/// <returns>есть</returns>
		public bool GetAreTwoEqualTermsExist(string sourceText, string targetText)
		{
		   List<IWebElement> terms = Driver.FindElements(By.XPath(SOURCE_TARGET_TERM_XPATH.Replace("#", sourceText).
			   Replace("**", targetText))).ToList();
			if (terms.Count > 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Кликнуть Edit
		/// </summary>
		public void ClickEditBtn()
		{
			ClickElement(By.XPath(EDIT_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть на строку термина
		/// </summary>
		public void ClickTermRow()
		{
			ClickElement(By.XPath(CONCEPT_ROW_XPATH + TERM_COMMENT_TD));
		}

		/// <summary>
		/// Кликнуть на заданную строку термина
		/// </summary>
		public void ClickTermRowByNameOfTerm(string source, string target)
		{
			ClickElement(By.XPath(SOURCE_TARGET_TERM_COMMENT_RAW_XPATH.Replace("#", source).Replace("**", target)));
		}

		/// <summary>
		/// Кликнуть на Edit в строке термина
		/// </summary>
		public void ClickEditTermBtn()
		{
			ClickElement(By.XPath(CONCEPT_ROW_XPATH + EDIT_TERM_BTN_XPATH));
		}

		/// <summary>
		/// Заполнить все термины в обычном режиме
		/// </summary>
		/// <param name="text">текст</param>
		public void FillTermGeneralMode(string text)
		{
			ClearAndFillElementsList(By.XPath(INPUT_TERM_XPATH), text);
		}

		/// <summary>
		/// Кликнуть кнопку Свернуть
		/// </summary>
		public void ClickTurnOffBtn()
		{
			ClickElement(By.XPath(TURN_OFF_BTN_XPATH));
		}

		/// <summary>
		/// Заполнить поле Поиск
		/// </summary>
		/// <param name="text">текст</param>
		public void FillSearchField(string text)
		{
			ClearAndAddText(By.XPath(SEARCH_INPUT_XPATH), text);
		}

		/// <summary>
		/// Кликнуть кнопку Search
		/// </summary>
		public void ClickSearchBtn()
		{
			ClickElement(By.XPath(SEARCH_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть текст первого термина
		/// </summary>
		/// <returns>текст термина</returns>
		public string GetFirstTermText()
		{
			return GetTextElement(By.XPath(TERM_TEXT_XPATH)).Trim();
		}

		/// <summary>
		/// Вернуть, есть ли такой термин
		/// </summary>
		/// <param name="termText">текст термина</param>
		/// <returns>есть</returns>
		public bool GetIsExistTerm(string termText)
		{
			return GetIsElementExist(By.XPath(TERM_TEXT_XPATH + "[contains(text(),'" + termText + "')]"));
		}

		/// <summary>
		/// Кликнуть Cancel
		/// </summary>
		public void ClickCancelEditBtn()
		{
			ClickElement(By.XPath(CANCEL_EDIT_CONCEPT_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Delete
		/// </summary>
		public void ClickDeleteBtn()
		{
			ClickElement(By.XPath(DELETE_CONCEPT_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Add 
		/// </summary>
		/// <param name="langNumber">номер языка</param>
		public void ClickAddSynonym(int langNumber)
		{
			ClickElement(By.XPath(CONCEPT_EDITING_TD_XPATH + "[" + langNumber + "]" + ADD_SYNONYM_BTN_XPATH));
		}

		/// <summary>
		/// Заполнить язык термина
		/// </summary>
		/// <param name="langNumber">номер языка</param>
		/// <param name="text">текст</param>
		public void FillSynonymTermLanguage(int langNumber, string text)
		{
			ClearAndAddText(By.XPath(CONCEPT_EDITING_TD_XPATH + "[" + langNumber + "]" + SYNONYM_INPUT_XPATH), text);
		}

		/// <summary>
		/// Вернуть, отмечен ли термин ошибкой
		/// </summary>
		/// <param name="langNumber">номер языка</param>
		/// <returns>отмечен</returns>
		public bool GetIsTermErrorExist(int langNumber)
		{
			return GetIsElementExist(By.XPath(CONCEPT_EDITING_TD_XPATH + "[" + langNumber + "]" + TERM_ERROR_XPATH));
		}

		/// <summary>
		/// Вернуть, появилось ли сообщение об ошибке в термине
		/// </summary>
		/// <param name="langNumber">номер языка</param>
		/// <returns>есть</returns>
		public bool GetIsTermErrorMessageExist(int langNumber)
		{
			return GetIsElementExist(By.XPath(CONCEPT_EDITING_TD_XPATH + "[" + langNumber + "]" + TERM_ERROR_MESSAGE_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли ошибка глоссария
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsGlossaryErrorExist()
		{
			return GetIsElementExist(By.XPath(GLOSSARY_ERROR_XPATH));
		}

		/// <summary>
		/// Дождаться ошибки дубликата
		/// </summary>
		/// <returns>появилась</returns>
		public bool WaitDuplicateErrorAppear()
		{
			return WaitUntilDisplayElement(By.XPath(DUPLICATE_ERROR_XPATH));
		}

		/// <summary>
		/// Кликнуть Import
		/// </summary>
		public void ClickImportBtn()
		{
			ClickElement(By.XPath(IMPORT_BTN_XPATH));
		}

		/// <summary>
		///  Дождаться появления формы импорта
		/// </summary>
		/// <returns>появилась</returns>
		public bool WaitImportForm()
		{
			return WaitUntilDisplayElement(By.XPath(IMPORT_FORM_XPATH));
		}

		/// <summary>
		///  Дождаться закрытия формы импорта
		/// </summary>
		/// <returns>закрылась</returns>
		public bool WaitUntilImportFormDisappear()
		{
			return WaitUntilDisappearElement(By.XPath(IMPORT_FORM_XPATH));
		}

		/// <summary>
		/// Кликнуть кнопку Upload
		/// </summary>
		public void ClickUploadBtn()
		{
			ClickElement(By.XPath(UPLOAD_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть кнопку Export
		/// </summary>
		public void ClickExportBtn()
		{
			ClickElement(By.XPath(EXPORT_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Replace All
		/// </summary>
		public void ClickReplaceAll()
		{
			ClickElement(By.XPath(REPLACE_ALL_XPATH));
		}

		/// <summary>
		/// Кликнуть импорт в форме импорта
		/// </summary>
		public void ClickImportFormImportBtn()
		{
			ClickElement(By.XPath(IMPORT_FORM_IMPORT_BTN_XPATH));
		}

		/// <summary>
		/// Закрыть сообщение об успешном добавлении
		/// </summary>
		public void ClickCloseSuccessResult()
		{
			ClickElement(By.XPath(SUCCESS_RESULT_CLOSE_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли добавление термина в расширенном режиме
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsExistNewItemExtendedMode()
		{
			return GetIsElementDisplay(By.XPath(CONCEPT_EDITING_TD_XPATH));
		}

		/// <summary>
		/// Получить xPath пользовательского поля (для поля типа Bool)
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetCustomFieldBoolXPath(string fieldName)
		{
			return CUSTOM_FIELD_BOOL_EDIT_CONCEPT_XPATH + "[contains(text(),'" + fieldName + "')]";
		}

		/// <summary>
		/// Получить xPath пользовательского поля
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetCustomFieldXPath(string fieldName)
		{
			return CUSTOM_FIELD_EDIT_CONCEPT_XPATH + "[contains(text(),'" + fieldName + "')]";
		}

		/// <summary>
		/// Получить xPath пользовательского поля для чтения
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetCustomFieldViewXPath(string fieldName)
		{
			return CUSTOM_FIELD_VIEW_CONCEPT_XPATH + "[contains(text(),'" + fieldName + "')]";
		}

		/// <summary>
		/// Получить xPath поля, отмеченного ошибкой
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetCustomFieldErrorXPath(string fieldName)
		{
			return CUSTOM_ERROR_FIELD_XPATH + "[contains(text(),'" + fieldName + "')]";
		}

		/// <summary>
		/// Получить XPath пользовательского поля для изоб;ражения
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetCustomFieldImageXPath(string fieldName)
		{
			return CUSTOM_FIELD_IMAGE_XPATH + "[contains(text(),'" + fieldName + "')]";
		}

		/// <summary>
		/// Получить xPath поля, отмеченного ошибкой
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetCustomFieldImageErrorXPath(string fieldName)
		{
			return CUSTOM_ERROR_FIELD_IMAGE_XPATH + "[contains(text(),'" + fieldName + "')]";
		}

		/// <summary>
		/// Вернуть xPath textarea
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetTextareaXPath(string fieldName)
		{
			return TEXTAREA_FIELD_XPATH + "[@name='" + fieldName + "']";
		}

		/// <summary>
		/// Вернуть xPath значения textarea
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetTextareaValueXPath(string fieldName)
		{
			return TEXTAREA_FIELD_XPATH + "[@name='" + fieldName + "']" + FIELD_DIV_VALUE_XPATH;
		}

		/// <summary>
		/// Вернуть xPath input
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetInputXPath(string fieldName)
		{
			return INPUT_FIELD_XPATH + "[@name='" + fieldName + "']";
		}

		/// <summary>
		/// Вернуть xPath Select
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetSelectXPath(string fieldName)
		{
			return SELECT_FIELD_XPATH + "[@name='" + fieldName + "']";
		}

		/// <summary>
		/// Вернуть xPath для поля textarea в детальных атрибутах
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetDetailsTextareaXPath(string fieldName)
		{
			return DETAILS_TEXTAREA_XPATH + "[@name='" + fieldName + "']";
		}

		/// <summary>
		/// Вернуть xPath для поля select в детальных атрибутах
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		protected string GetDetailsSelectXPath(string fieldName)
		{
			return DETAILS_SELECT_XPATH + "[@name='" + fieldName + "']";
		}

		/// <summary>
		/// Вернуть текст списка выбора по id
		/// </summary>
		/// <param name="optionID">id</param>
		/// <returns>текст</returns>
		protected string GetSelectOptionText(string optionID)
		{
			return CHOICE_DROPDOWN_LIST + "[@data-id='" + optionID + "']";
		}



		protected const string ADD_CONCEPT_XPATH = "//span[contains(@class,'js-add-concept')]";
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

		protected const string CONCEPT_TABLE_XPATH = "//table[contains(@class,'js-concepts')]";
		protected const string CONCEPT_EDITING_TD_XPATH = "//tr[contains(@class, 'js-concept')]//td";
		protected const string CONCEPT_EDITING_OPENED = "//tr[contains(@class, 'js-concept-row js-editing opened')]";
		protected const string EDIT_CONCEPT_SAVE_BTN_XPATH = CONCEPT_EDITING_OPENED + "//a[contains(@class, 'js-save-btn')]";
		protected const string CANCEL_EDIT_CONCEPT_BTN_XPATH = CONCEPT_EDITING_OPENED + "//a[contains(@class, 'js-cancel-btn')]";
		protected const string DELETE_CONCEPT_BTN_XPATH = CONCEPT_ROW_XPATH + "//a[contains(@class, 'js-delete-btn')]";
		protected const string CONCEPT_ROW_XPATH = "//tr[contains(@class, 'js-concept-row')]";
		protected const string OPENED_CONCEPT_ROW_XPATH = "//tr[contains(@class,'js-concept-row opened')]";
		protected const string SAVE_EXTENDED_BTN_XPATH = CONCEPT_TABLE_XPATH + "//span[contains(@class,'js-save-btn')]";
		
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
	}
}