using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using NLog;
using System.Threading;
using NUnit.Framework;
using System.Linq;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	public class GlossaryTermFilterHelper : GlossarySuggestPageHelper
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();

		public GlossaryTermFilterHelper(IWebDriver driver, WebDriverWait wait)
			: base(driver, wait)
		{
		}

		/// <summary>
		/// Кликнуть Filter кнопку на стр глоссари
		/// </summary>
		/// <returns> Открыта Filter форма </returns>
		public bool OpenFilterForm()
		{
			Thread.Sleep(10000);
			ClickElement(By.XPath(FILTER_BTN));
			WaitUntilDisplayElement(By.XPath(FILTER_FORM));
			return GetIsElementDisplay(By.XPath(FILTER_FORM));
		}

		/// <summary>
		/// Раскрыть дропдаун языков в Filter форме
		/// </summary>
		public void ClickLangDropDownInFilterForm()
		{
			ClickElement(By.XPath(FILTER_LANG));
		}

		/// <summary>
		/// Определить все ли языки выбраны в дропдайне языков в Filter форме
		/// </summary>
		public bool LangCheckboxesInFilterForm()
		{
			var checkboxList = GetElementList(By.XPath(FILTER_LANG_CHECKBOXES));
			for (int i = 0; i < checkboxList.Count; i++)
			{
				if (GetElementAttribute(By.XPath("//li[" + (i + 1) + "]" + FILTER_LANG_CHECKBOXES), "checked") != "true")
					return false;
			}
			return true;
		}

		/// <summary>
		/// Определить, что все чекбоксы нечекнуты в дропдауне
		/// </summary>
		public bool GetCheckboxesAreNotChecked(By by)
		{
			var checkboxList = GetElementList(by);
			for (int i = 0; i < checkboxList.Count; i++)
			{
				string checkedAttr = GetElementAttribute(by, "checked");
				if (checkedAttr != "false")
					return false;
			}
			return true;
		}

		/// <summary>
		/// Чекнуты ли чекбоксы в Modified дропдауне
		/// </summary>
		public bool GetCheckboxesModifier()
		{
			ExpandModifierDropDown();
			return GetCheckboxesAreNotChecked(By.XPath(MODIFIER_CHECKBOXES));
		}

		/// <summary>
		/// Чекнуты ли чекбоксы в дропдауне автора
		/// </summary>
		public bool GetCheckboxesAuthor()
		{
			ClickElement(By.XPath(AUTHOR_DROPDOWN));
			return GetCheckboxesAreNotChecked(By.XPath(AUTHOR_CHECKBOXES));
		}

		public string GetDateSetValue(By by)
		{
			return Driver.FindElement(by).GetAttribute("title");
		}

		public string GetCreatedSetValue()
		{
			return GetDateSetValue(By.XPath(SET_CREATED_DATA));
		}

		public string GetModifiedSetValue()
		{
			return GetDateSetValue(By.XPath(SET_MODIFIED_DATA));
		}

		/// <summary>
		/// Выбрать только один язык в фильтре
		/// </summary>
		/// <param name="language"> Язык </param>
		public void CheckOnlyOneLanguage(string language)
		{
			ClickLangDropDownInFilterForm();
			IList<IWebElement> checkboxList = GetElementList(By.XPath(FILTER_LANG_CHECKBOXES));
			foreach (IWebElement checkbox in checkboxList)
			{
				if((checkbox.GetAttribute("title") != language) && (checkbox.GetAttribute("checked") == "true"))
					checkbox.Click();
			}
		}

		/// <summary>
		/// Анчекнуть все язкив  фильтре
		/// </summary>
		public void UnCheckOnlyAllLanguagesInFilter()
		{
			IList<IWebElement> checkboxList = GetElementList(By.XPath(FILTER_LANG_CHECKBOXES));
			foreach (IWebElement checkbox in checkboxList)
			{
				if (checkbox.GetAttribute("checked") == "true")
					checkbox.Click();
			}
		}

		/// <summary>
		/// Открыть Suggested Terms страницу на стр словаря
		/// </summary>
		public void OpenSuggestedTermsForGlossary()
		{
			ClickElement(By.XPath(SUGGESTED_TERMS_LINK));
		}

		/// <summary>
		/// Кликнуть Apply кнопку в Filter форме
		/// </summary>
		public void ClickApplyBtnInFilterForm()
		{
			ClickElement(By.XPath(APPLY_BTN_IN_FILTER_FORM));
			WaitUntilDisappearElement(By.XPath(FILTER_FORM));
		}

		/// <summary>
		/// Получить названия колонок на стр одного глоссари
		/// </summary>
		/// <returns> Список названий колонок </returns>
		public List<string> GetLangFromTable()
		{
			List<string> langNames = new List<string>();;
			IList<IWebElement> langColumns = GetElementList(By.XPath(LANG_TERM_IN_TABLE));
			for (int i = 0; i < langColumns.Count; i++)
			{
				langNames.Add(langColumns[i].Text.Replace("Term",""));
			}
			return langNames;
		}

		/// <summary>
		/// Получить список выбранных языков в фильтре
		/// </summary>
		/// <returns> Список выбранных языков </returns>
		public List<string> GetSelectedLangInFilterForm()
		{
			List<string> langNames = new List<string>();
			IList<IWebElement> selectedLang = GetElementList(By.XPath(SELECTED_LANGIN_FILTER_FORM));
			for (int i = 0; i < selectedLang.Count; i++)
			{
				langNames.Add(selectedLang[i].Text.Trim());
			}
			return langNames;
		}

		/// <summary>
		/// Сравнить выбранные языки в фильтре и названия колонок на стр одного глоссари
		/// </summary>
		public bool CompareSelctedLangAndColumnNames(List<string> selectedLang, List<string> columnNames)
		{
			bool returnValue = false;

			List<string> selectedLanguages = GetSelectedLangInFilterForm();
			List<string> langFromTable = GetLangFromTable();
			List<string> result = new List<string>();
			if (selectedLanguages.Count != langFromTable.Count)
				returnValue = false;

			foreach (var lang in selectedLanguages)
			{
				if (!langFromTable.Contains(lang))
					result.Add(lang);
			}
			if(result.Count > 0)
			{
				returnValue = false;
			}
			else
			{
				returnValue = true;
			}

			foreach (string item in result)
			{
				Logger.Trace("в результате: " + item.ToString());
			}
			return returnValue;
		}

		/// <summary>
		/// Вернуть появилось ли сообщение "The Language field cannot be empty."
		/// </summary>
		public bool GetBlockMsgIsDisplay()
		{
			return GetIsElementDisplay(By.XPath(EMPTY_LANG_BLOCK_MSG));
		}

		/// <summary>
		/// Удалить язык в окне св-ва глоссари
		/// </summary>
		/// <param name="number"> Номер языка </param>
		public void DeleteLanguageInProperties(int number)
		{
			ClickElement(By.XPath("//span["+ number + "]"+DELETELANG_BTN));
		}

		/// <summary>
		/// Заполнить поля для новой позиции
		/// </summary>
		public void AddNewEntry()
		{
			ClickElement(By.XPath(NEW_ENTRY_BTN));
			Assert.IsTrue(GetIsElementDisplay(By.XPath(NEW_RNTRY_ROW)), "Ошибка: строка для заполнения термина не появилась");
			IList<IWebElement> editableFields = GetElementList(By.XPath(TERM_EDITABLE_FIELDS));
			foreach(var term in editableFields)
			{
				term.SendKeys("Term" + DateTime.Now);
			}
			ClickElement(By.XPath(SAVE_BTN_IN_ROW));
			Thread.Sleep(10000);
		}

		public string[] GetSortedTerms()
		{
			IList<IWebElement> terminsList = GetElementList(By.XPath(ALL_TERMS));
			string[] array = terminsList.Select(t => t.Text).ToArray();
			Array.Sort(array);
			return array;
		}

		/// <summary>
		/// Выбрать автора в фильтре
		/// </summary>
		/// <param name="userName"> Ник </param>
		public void SelectAuthor(string userName)
		{
			ClickElement(By.XPath(AUTHOR_DROPDOWN));
			var author = GetElementList(By.XPath(AUTHOR_CHECKBOXES)).FirstOrDefault();
			if (author == null) 
			{
				throw new ArgumentNullException("author");
			}
			else
			{
				author.Click();
			}
		}

		/// <summary>
		/// Вернуть появился ли лэйбл фильтра в хидере таблицы
		/// </summary>
		public bool GetIsCreatedFilterDisplay()
		{
			return GetIsElementDisplay(By.XPath(CREATED_FILTER_LABEL));
		}

		/// <summary>
		/// Кликнуть Edit кнопку
		/// </summary>
		public void ClickEditBtn()
		{
			ClickElement(By.XPath(EDIT_BTN));
		}

		/// <summary>
		/// Кликнуть по строке термина
		/// </summary>
		public void ClickTermRow()
		{
			ClickElement(By.XPath(TERMIN_ROW));
		}

		/// <summary>
		/// Изменить термин
		/// </summary>
		/// <param name="terminRow"> Номер строки</param>
		public void EditEditableFields(string text, int terminRow)
		{
			SendTextElement(By.XPath("//tr[" + (terminRow + 1) + "]" + TERM_EDITABLE_FIELDS), text);
		}

		/// <summary>
		/// Раскрыть дропдаун Modified by
		/// </summary>
		public void ExpandModifierDropDown()
		{
			ClickElement(By.XPath(MODIFIER_DROPDOWN));
		}


		/// <summary>
		/// Выбрать автора в фильтре
		/// </summary>
		/// <param name="userName"> Ник </param>
		public void SelectModifier(string userName = "All")
		{
			ExpandModifierDropDown();
			if (userName == "All")
			{
				foreach (var modifier in GetElementList(By.XPath(MODIFIER_CHECKBOXES)))
				{
					modifier.Click();
				}
			}
			else
			{
				var modifier = GetElementList(By.XPath(MODIFIER_CHECKBOXES)).FirstOrDefault();
				if (modifier == null)
				{
					throw new ArgumentNullException("modifier");
				}
				else
				{
					modifier.Click();
				}

			}
		}

		/// <summary>
		/// Получить диапазон дат из желтой панели 
		/// </summary>
		public string GetRangeFilterPanel()
		{
			return GetElementAttribute(By.XPath(DIAPASON_PANEL), "title");
		}

		/// <summary>
		/// Проверить, что в комбобоксе дата создания содержится 4 опции
		/// </summary>
		/// <returns></returns>
		public bool GetIsAlldateAreDispalyed(int optionsCount = 4)
		{
			return GetElementList(By.XPath(DATE_CREATED)).Count == optionsCount;
		}

		/// <summary>
		/// Кликнуть по комбобоксу дата создания
		/// </summary>
		public void ClickCreatedDateDropDown()
		{
			ClickElement(By.XPath(DATE_CREATED_DROPDOWN));
		}

		/// <summary>
		/// Выбрать дату создания 
		/// </summary>
		/// <param name="date"> "7 days"/"last month"/"anytime"</param>
		public void SelectCreatedDate(string date)
		{
			ClickCreatedDateDropDown();
			ClickElement(By.XPath(CREATED_DATE_OPTION + date + "')]"));
		}

		/// <summary>
		/// Кликнуть Modified дропдаун
		/// </summary>
		public void ClickModifiedDateDropDown()
		{
			ClickElement(By.XPath(DATE_MODIFIED_DROPDOWN));
		}

		/// <summary>
		/// Выбрать дату изменения
		/// </summary>
		/// <param name="date"> Дата изменения </param>
		public void SelectModifiedDate(string date)
		{
			ClickModifiedDateDropDown();
			ClickElement(By.XPath(CREATED_DATE_OPTION + date + "')]"));
		}

		/// <summary>
		/// Получить сегодняшнюю дату создания 
		/// </summary>
		/// <returns> Сегодняшняя дата </returns>
		public string GetTodayCreatedDate()
		{
			return GetCreateDateElement().GetAttribute("innerHTML");
		}

		/// <summary>
		/// Получить элемент даты
		/// </summary>
		/// <returns> IWebElement </returns>
		public IWebElement GetCreateDateElement()
		{
			return Driver.FindElement(By.XPath(TODAY_CREATED_DATA));
		}

		/// <summary>
		/// Получить дату создания, которая была неделю назад
		/// </summary>
		/// <returns>  Дата неделя назад </returns>
		public string GetWeekCreatedDate()
		{
			return Driver.FindElement(By.XPath(WEEK_CREATED_DATA)).GetAttribute("innerHTML");
		}

		/// <summary>
		/// Получить дату создания, которая была месяц назад
		/// </summary>
		/// <returns>  Дата месяц назад </returns>
		public string GetMonthCreatedDate()
		{
			return Driver.FindElement(By.XPath(MONTH_CREATED_DATA)).GetAttribute("innerHTML");
		}

		/// <summary>
		/// Получить дату изменения, которая была неделю назад
		/// </summary>
		/// <returns>  Дата неделя назад </returns>
		public string GetWeekModifiedDate()
		{
			return Driver.FindElement(By.XPath(WEEK_MODIFIED_DATA)).GetAttribute("innerHTML");
		}

		/// <summary>
		/// Получить дату изменения, которая была месяц назад
		/// </summary>
		/// <returns>  Дата месяц назад </returns>
		public string GetMonthModifiedDate()
		{
			return Driver.FindElement(By.XPath(MONTH_MODIFIED_DATA)).GetAttribute("innerHTML");
		}

		/// <summary>
		/// Кликнуть Clear filter кнопку
		/// </summary>
		public void ClickClearFilter()
		{
			ClickElement(By.XPath(CLEAR_FILTER_BTN));
		}

		/// <summary>
		/// Кликнуть Cancel кнопку
		/// </summary>
		public void ClickCancelFilter()
		{
			ClickElement(By.XPath(CANCEL_FILTER_BTN));
		}

		/// <summary>
		/// Удалить фильтр дата/автор изменения из желтой панели в таблице терминов
		/// </summary>
		/// <param name="filter"> Значение фильтра </param>
		public void DeleteModifiedPanel(string filter)
		{
			ClickElement(By.XPath(MODIFIED_CLEAR_FILTER_PANEL + filter + "')]" + X_BTN));
			Thread.Sleep(10000); // ждем, так как фильтр не сразу удаляется из панели
		}

		/// <summary>
		/// Удалить фильтр автор/дата создания из желтой панели в таблице терминов
		/// </summary>
		/// <param name="filter"> Значение фильтра </param>
		public void DeleteCreatedPanel(string filter)
		{
			ClickElement(By.XPath(CREATED_CLEAR_FILTER_PANEL + filter + "')]" + X_BTN));
			Thread.Sleep(10000); // ждем, так как фильтр не сразу удаляется из панели
		}

		/// <summary>
		/// Удалить все фильтры из желтой панели в таблице терминов
		/// </summary>
		public void DeleteAllPanel()
		{
			ClickElement(By.XPath(COMMON_CLEAR_FILTER_BTN));
		}

		/// <summary>
		/// Проверить появился ли фильтр дата/автор создания в желтой панели
		/// </summary>
		///  <param name="filter"> Значение фильтра </param>
		public bool GetCreatedPanelIsDislay(string filter)
		{
			return GetIsElementDisplay(By.XPath(CREATED_CLEAR_FILTER_PANEL + filter + "')]"));
		}

		/// <summary>
		/// Проверить появился ли фильтр дата/автор изменения в желтой панели
		/// </summary>
		/// <param name="filter"> Значение фильтра </param>
		public bool GetModifiedPanelIsDislay(string filter)
		{
			return GetIsElementDisplay(By.XPath(MODIFIED_CLEAR_FILTER_PANEL + filter + "')]"));
		}

		/// <summary>
		/// Получить отображается ли общая панель фильтров
		/// </summary>
		public bool GetCommonPahelIsDisplay()
		{
			return GetIsElementDisplay(By.XPath(COMMON_CLEAR_FILTER_SECTION));
		}

		/// <summary>
		/// Ввод в начальную дату создания
		/// </summary>
		/// <param name="date"> Дата </param>
		public void SendCreatedStartDate(string date)
		{
			SendTextElement(By.XPath(CREATED_START_DATE), date);
		}

		/// <summary>
		/// Ввод в конечную дату создания
		/// </summary>
		/// <param name="date"> Дата </param>
		public void SendCreatedEndDate(string date)
		{
			SendTextElement(By.XPath(CREATED_END_DATE), date);
		}

		/// <summary>
		/// Ввод в начальную дату изменения
		/// </summary>
		/// <param name="date"> Дата </param>
		public void SendModifiedStartDate(string date)
		{
			SendTextElement(By.XPath(MODIFIED_START_DATE), date);
		}

		/// <summary>
		/// Ввод в конечную дату изменения
		/// </summary>
		/// <param name="date"> Дата </param>
		public void SendModifiedEndDate(string date)
		{
			SendTextElement(By.XPath(MODIFIED_END_DATE), date);
		}

		/// <summary>
		/// Получить значение в начальной дате создания
		/// </summary>
		public string GetStartCreatedDate()
		{
			return GetElementAttribute(By.XPath(CREATED_START_DATE), "value");
		}

		/// <summary>
		/// Получить значение в начальной дате изменения
		/// </summary>
		public string GetStartModifiedDate()
		{
			return GetElementAttribute(By.XPath(MODIFIED_START_DATE), "value");
		}

		/// <summary>
		/// Установить начальную дату создания с помощью календаря
		/// </summary>
		public void SetStartDayInLeftCalendar()
		{
			ClickElement(By.XPath(START_CREATED_CALENDAR));
			ClickElement(By.XPath(DAY_IN_CALENDAR));
		}

		/// <summary>
		/// Установить конечную дату создания с помощью календаря
		/// </summary>
		public void SetEndDayInLeftCalendar()
		{
			ClickElement(By.XPath(END_CREATED_CALENDAR));
			ClickElement(By.XPath(DAY_IN_CALENDAR));
		}

		/// <summary>
		/// Получить значение в  комбоксе Created
		/// </summary>
		public string GetTitleInDateCreatedDD()
		{
			return GetElementAttribute(By.XPath(DATE_DD_CREATED), "title");
		}

		/// <summary>
		/// Установить начальную дату изменения с помощью календаря
		/// </summary>
		public void SetStartDayInRightCalendar()
		{
			ClickElement(By.XPath(START_MODIFIED_CALENDAR));
			ClickElement(By.XPath(DAY_IN_CALENDAR));
		}

		/// <summary>
		/// Установить конечную дату изменения с помощью календаря
		/// </summary>
		public void SetEndDayInRightCalendar()
		{
			ClickElement(By.XPath(END_MODIFIED_CALENDAR));
			ClickElement(By.XPath(DAY_IN_CALENDAR));
		}

		/// <summary>
		/// Получить значение в  комбоксе Modified
		/// </summary>
		public string GetTitleInDateModifiedDD()
		{
			return GetElementAttribute(By.XPath(DATE_DD_MODIFIED), "title");
		}

		protected const string FILTER_BTN = "//span[contains(@class, 'js-set-filter')]";
		protected const string FILTER_FORM = "//div[@class='g-popupbox l-filtersrc']//h2[contains(text(),'Filter')]";
		protected const string FILTER_LANG = "//div[contains(@class, 'js-languages-multiselect')]//div[@class='ui-multiselect-text']";
		protected const string FILTER_LANG_CHECKBOXES = "//input[contains(@id,'ui-multiselect-languages')]";
		protected const string SUGGESTED_TERMS_LINK = "//a[contains(@href, 'Suggests')]";
		protected const string APPLY_BTN_IN_FILTER_FORM = "//input[@value='Apply']";
		protected const string LANG_TERM_IN_TABLE = "//th[@class='l-corpr__th']//a[@class='g-block l-corpr__thsort' and contains(@href,'orderBy=Language')]"; // колонки языков в таблице на стр одного глоссари
		protected const string SELECTED_LANGIN_FILTER_FORM = "//span[@class='g-iblock g-bold ui-multiselect-value']"; // выбранные язки в фильтре
		protected const string EMPTY_LANG_BLOCK_MSG = "//div[@style='display: block;']//p[text()='The \"Language\" field cannot be empty.']";
		protected const string DELETELANG_BTN = "//em[contains(@class, 'js-delete-language')]";
		protected const string NEW_ENTRY_BTN = "//span[contains(@title,'new entry')]";
		protected const string NEW_RNTRY_ROW = "//tr[contains(@class, 'js-editing opened')]";
		protected const string TERM_EDITABLE_FIELDS = "//p[contains(@class, 'js-term-box')]//input";
		protected const string SAVE_BTN_IN_ROW ="//a[@title='Save']"; // галка в строке при добавлении Entry
		protected const string ALL_TERMS = "//tr[@class='l-corpr__trhover js-concept-row']//td[contains(@class, 'g-bold')]//p";
		protected const string AUTHOR_DROPDOWN = "//div[@class='l-filtersrc__control creator']//div[@class='ui-multiselect-text']";
		protected const string AUTHOR_CHECKBOXES = "//input[contains(@id,'ui-multiselect-creator-option')]";
		protected const string CREATED_FILTER_LABEL = "//div[contains(@title, 'Created:')]";
		protected const string TERMIN_ROW = "//tr[contains(@class, 'concept-row')][1]";
		protected const string EDIT_BTN = "//tr[contains(@class, 'concept-row')][1]//a[contains(@class, 'js-edit-btn')]";
		protected const string MODIFIER_DROPDOWN = "//div[contains(@class,'modifier')]//div[contains(@class, 'ui-multiselect ui-widget')]";
		protected const string MODIFIER_CHECKBOXES = "//input[contains(@id,'ui-multiselect-modifier-option')]";
		protected const string DATE_CREATED = "//span[@class='js-dropdown__list filterDate g-drpdwn__list']//span";
		protected const string DATE_CREATED_DROPDOWN = "//div[@class='l-filtersrc__lside']//img[@class='g-drpdwn__img g-bg']";
		protected const string DATE_MODIFIED_DROPDOWN = "//div[@class='l-filtersrc__rside'][1]//span[contains(@class, 'filterDate')]";
		protected const string LEFT_CALENDAR = "//div[@class='l-filtersrc__lside']";
		protected const string RIGHT_CALENDAR = "//div[@class='l-filtersrc__rside']";
		protected const string DIAPASON_PANEL = "//div[@class='l-corpr__filterSection inline']";
		protected const string CREATED_DATE_OPTION = "//span[contains(@title, '";
		protected const string TODAY_CREATED_DATA = LEFT_CALENDAR + "//span[@class='js-data today']";
		protected const string WEEK_CREATED_DATA = LEFT_CALENDAR + "//span[@class='js-data week']";
		protected const string TODAY_MODIFIED_DATA = RIGHT_CALENDAR + "//span[@class='js-data today']";
		protected const string WEEK_MODIFIED_DATA = RIGHT_CALENDAR + "//span[@class='js-data week']";
		protected const string CLEAR_FILTER_BTN = "//a[contains(text(),'Clear fields')]";
		protected const string MONTH_CREATED_DATA = LEFT_CALENDAR + "//span[@class='js-data month']";
		protected const string MONTH_MODIFIED_DATA = RIGHT_CALENDAR + "//span[@class='js-data month']";
		protected const string SET_CREATED_DATA = LEFT_CALENDAR + "//span[@class='js-dropdown__text filterDate g-drpdwn__text g-block']";
		protected const string SET_MODIFIED_DATA = RIGHT_CALENDAR + "//span[@class='js-dropdown__text filterDate g-drpdwn__text g-block']";
		protected const string CANCEL_FILTER_BTN = "//div[@class='g-popupbox__bd']//a[contains(@class, 'js-popup-close')]";
		protected const string CREATED_START_DATE = LEFT_CALENDAR + "//span[5]/input";
		protected const string CREATED_END_DATE = LEFT_CALENDAR  + "//span[6]/input";
		protected const string MODIFIED_START_DATE = RIGHT_CALENDAR + "//span[5]/input";
		protected const string MODIFIED_END_DATE = RIGHT_CALENDAR + "//span[6]/input";

		// желтая панель в таблице
		protected const string COMMON_CLEAR_FILTER_SECTION = "//div[@class='l-corpr__filter g-bold']";
		protected const string COMMON_CLEAR_FILTER_BTN = COMMON_CLEAR_FILTER_SECTION + "/img";
		protected const string INSIDE_CLEAR_FILTER_SECTION = COMMON_CLEAR_FILTER_SECTION + "/table";
		protected const string X_BTN = "//img";
		protected const string CREATED_CLEAR_FILTER_PANEL = "//div[contains(@title, 'Created: " ;
		protected const string MODIFIED_CLEAR_FILTER_PANEL = "//div[contains(@title, 'Modified: ";

		protected const string START_CREATED_CALENDAR = LEFT_CALENDAR + "//span[5]//img[@class='ui-datepicker-trigger']";
		protected const string END_CREATED_CALENDAR = LEFT_CALENDAR + "//span[@class='g-iblock l-filtersrc__datebox ']//img[@class='ui-datepicker-trigger']";
		protected const string START_MODIFIED_CALENDAR = RIGHT_CALENDAR + "//span[5]//img[@class='ui-datepicker-trigger']";
		protected const string END_MODIFIED_CALENDAR = RIGHT_CALENDAR + "//span[@class='g-iblock l-filtersrc__datebox ']//img[@class='ui-datepicker-trigger']";
		protected const string DAY_IN_CALENDAR = "//table[@class='ui-datepicker-calendar']//tr[3]/td[1]/a";
		protected const string DATE_DD_CREATED = LEFT_CALENDAR + "//span[@class='js-dropdown filterDate g-drpdwn g-iblock ']//span";
		protected const string DATE_DD_MODIFIED = RIGHT_CALENDAR + "//span[@class='js-dropdown filterDate g-drpdwn g-iblock ']//span";
	}

}
