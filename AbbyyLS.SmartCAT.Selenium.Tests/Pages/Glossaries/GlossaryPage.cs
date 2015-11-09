using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossaryPage : GlossariesPage, IAbstractPage<GlossaryPage>
	{
		public GlossaryPage(WebDriver driver) : base(driver)
		{
		}

		public new GlossaryPage GetPage()
		{
			var glossaryPage = new GlossaryPage(Driver);
			InitPage(glossaryPage, Driver);

			return glossaryPage;
		}

		public new void LoadPage()
		{
			Driver.WaitPageTotalLoad();
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(NEW_ENTRY_BUTTON), timeout: 30))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница глоссария.");
			}
		}

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, верен ли диапазон дат
		/// </summary>
		/// <param name="dateTimeList">диапазон дат</param>
		public bool IsDateRangeInFilterMatch(List<DateTime> dateTimeList)
		{
			CustomTestContext.WriteLine("Проверить, верен ли диапазон дат {0}.", dateTimeList);
			var rangeList = new List<DateTime>();
			var panelText = Driver.FindElement(By.XPath(DIAPASON_PANEL)).GetAttribute("title").Split(' ');
			var startDate = DateTime.ParseExact(panelText[2], "M/d/yyyy", CultureInfo.InvariantCulture);
			var endDate = DateTime.ParseExact(panelText[4], "M/d/yyyy", CultureInfo.InvariantCulture);

			rangeList.Add(startDate);
			rangeList.Add(endDate);

			return dateTimeList.SequenceEqual(rangeList);
		}

		/// <summary>
		/// Проверить, отображается ли сообщение 'The term already exists'
		/// </summary>
		public bool IsExistTermErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, отображается ли сообщение 'The term already exists'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ALREADY_EXIST_TERM_ERROR));
		}

		/// <summary>
		/// Проверить, отображается ли сообщение 'Please add at least one term'.
		/// </summary>
		public bool IsEmptyTermErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, отображается ли сообщение 'Please add at least one term'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(EMPTY_TERM_ERROR));
		}

		/// <summary>
		/// Проверить, что термин присутствует в секции 'Languages and terms '
		/// </summary>
		public bool IsTermDisplayedInLanguagesAndTermsSection(string term)
		{
			CustomTestContext.WriteLine("Проверить, что термин {0} присутствует в секции 'Languages and terms '.", term);

			return Driver.SetDynamicValue(How.XPath, TERMS_IN_LANGUAGE_AND_TERMS_SECTION, term).Displayed;
		}

		/// <summary>
		/// Проверить, отображается ли термин в режиме редактирования
		/// </summary>
		public bool IsTermFieldEditModeDisplayed(GlossarySystemField termField)
		{
			CustomTestContext.WriteLine("Проверить, отображается ли термин {0} в режиме редактирования.", termField);

			return Driver.GetIsElementExist(By.XPath(TERM_FIELD_EDIT_MODE.Replace("*#*", termField.ToString())));
		}

		/// <summary>
		/// Проверить, отображается ли термин типа дропдаун в режиме редактирования
		/// </summary>
		public bool IsDropdownTermFieldEditModeDisplayed(GlossarySystemField termField)
		{
			CustomTestContext.WriteLine("Проверить, отображается ли термин {0} типа дропдаун в режиме редактирования.", termField);

			return Driver.GetIsElementExist(By.XPath(DROPDOWN_TERM_FIELD_EDIT_MODE.Replace("*#*", termField.ToString())));
		}

		/// <summary>
		/// Получить текст из фильтра 'Modified'
		/// </summary>
		public string IsModifiedByFilterText()
		{
			CustomTestContext.WriteLine("Получить текст из фильтра 'Modified'.");

			return ModifiedByFilterLabel.Text;
		}

		/// <summary>
		/// Проверить отображается ли фильтр 'Created' в таблице терминов
		/// </summary>
		public bool IsCreatedByFilterLabelDisplayed()
		{
			CustomTestContext.WriteLine("Проверить отображается ли фильтр 'Created' в таблице терминов.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(CREATED_BY_FILTER_LABEL));
		}

		/// <summary>
		/// Проверить отображается ли фильтр 'Modified' в таблице терминов
		/// </summary>
		public bool IsModifiedByFilterLabelDisplayed()
		{
			CustomTestContext.WriteLine("Проверить отображается ли фильтр 'Modified' в таблице терминов.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(MODIFIED_BY_FILTER_LABEL));
		}

		/// <summary>
		/// Проверить, что желтая панель с фильтрами пуста
		/// </summary>
		public bool IsClearFilterSectionDisplayed()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(COMMON_CLEAR_FILTER_SECTION));
		}

		/// <summary>
		/// Проверить, что фильтр Modified отображается в шапке таблицы
		/// </summary>
		public bool IsModifiedFilterDisplayedInTableHeader(string filterValue)
		{
			CustomTestContext.WriteLine("Проверить, что фильтр Modified отображается в шапке таблицы.");

			return Driver.GetIsElementExist(By.XPath(MODIFIED_CLEAR_FILTER_PANEL.Replace("*#*", filterValue)));
		}

		/// <summary>
		/// Проверить, что фильтр Created отображается в шапке таблицы
		/// </summary>
		public bool IsCreatedFilterDisplayedInTableHeader(string filterValue)
		{
			CustomTestContext.WriteLine("Проверить, что фильтр Created отображается в шапке таблицы.");

			return Driver.GetIsElementExist(By.XPath(CREATED_CLEAR_FILTER_PANEL.Replace("*#*", filterValue)));
		}

		#endregion

		#region Простые методы страницы

		/// <summary>
		/// Получить текст из термина типа дропдаун в режиме просмотра
		/// </summary>
		public string GetDropdownTermFieldViewModelText(GlossarySystemField termField)
		{
			CustomTestContext.WriteLine("Получить текст из термина типа дропдаун в режиме просмотра.", termField);

			return Driver.SetDynamicValue(How.XPath, DROPDOWN_TERM_FIELD_VIEW_MODE, termField.ToString()).Text.ToLower();
		}

		/// <summary>
		/// Нажать на кнопку Export
		/// </summary>
		public GlossaryPage ClickExportGlossary()
		{
			CustomTestContext.WriteLine("Нажать кнопку Export.");
			ExportButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'New Entry'
		/// </summary>
		public GlossaryPage ClickNewEntryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'New Entry'.");
			NewEntryButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Заполнить термин
		/// </summary>
		/// <param name="columnNumber">номер колонки</param>
		/// <param name="text">текст</param>
		public GlossaryPage FillTerm(int columnNumber, string text)
		{
			CustomTestContext.WriteLine("Ввести {0} в {1} колонке термина.", columnNumber, text);
			var term = Driver.SetDynamicValue(How.XPath, TERM_FIELD, columnNumber.ToString());

			term.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Нажать на галочку для сохраенния термина
		/// </summary>
		public GlossaryPage ClickSaveTermButton()
		{
			CustomTestContext.WriteLine("Нажать на галочку для сохраенния термина.");
			TermSaveButton.Click();

			Driver.WaitUntilElementIsDisappeared(By.XPath(TERM_SAVE_BUTTON));

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку Save Entry для сохранения термина
		/// </summary>
		public GlossaryPage ClickSaveEntryButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку Save Entry для сохранения термина.");

			var i = 0;

			while (i < 5)
			{
				if (!Driver.WaitUntilElementIsDisappeared(By.XPath(SAVE_ENTRY_BUTTON), 1))
				{
					SaveEntryButton.Click();
				}

				i++;
			}

			return GetPage();
		}


		/// <summary>
		/// Раскрыть меню редактирования глоссария
		/// </summary>
		public GlossaryPage ExpandEditGlossaryMenu()
		{
			CustomTestContext.WriteLine("Раскрыть меню редактирования глоссария.");
			EditGlossaryMenu.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать 'Glossary properties' в меню редактирования глоссария
		/// </summary>
		public GlossaryPropertiesDialog ClickGlossaryProperties()
		{
			CustomTestContext.WriteLine("Выбрать 'Glossary properties' в меню редактирования глоссария");
			GlossaryProperties.Click();

			return new GlossaryPropertiesDialog(Driver).GetPage();
		}

		/// <summary>
		/// Выбрать 'Glossary structure' в меню редактирования глоссария
		/// </summary>
		public GlossaryStructureDialog ClickGlossaryStructure()
		{
			CustomTestContext.WriteLine("Выбрать 'GlossaryStructure' в меню редактирования глоссария.");
			GlossaryStructure.Click();

			return new GlossaryStructureDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Import
		/// </summary>
		public GlossaryImportDialog ClickImportButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Import.");
			ImportButton.Click();

			return new GlossaryImportDialog(Driver).GetPage();
		}

		/// <summary>
		/// Получить количество терминов.
		/// </summary>
		public int TermsCount()
		{
			CustomTestContext.WriteLine("Получить количество терминов.");

			return Driver.GetElementsCount(By.XPath(TERM_ROW));
		}

		/// <summary>
		/// Нажать кнопку сортировки по английским терминам
		/// </summary>
		public GlossaryPage ClickSortByEnglishTerm()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по английским терминам.");
			SortByEnglishTerm.Click();

			return GetPage();
		}

		/// <summary>
		/// Открыть подробную информацию по языку или термину в режиме редактирования
		/// </summary>
		public GlossaryPage OpenLanguageAndTermDetailsEditMode()
		{
			CustomTestContext.WriteLine("Открыть подробную информацию по языку или термину в режиме редактирования.");
			LanguageHeaderEditMode.Click();

			return GetPage();
		}

		/// <summary>
		/// Открыть подробную информацию по языку или термину в режиме просмотра
		/// </summary>
		public GlossaryPage OpenLanguageAndTermDetailsViewMode()
		{
			CustomTestContext.WriteLine("Открыть подробную информацию по языку или термину в режиме просмотра.");
			LanguageHeaderViewMode.Click();

			return GetPage();
		}


		/// <summary>
		/// Ввести текст в поле комментария в секции 'Language and term details'
		/// </summary>
		public GlossaryPage FillLanguageComment(string comment)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле комментария в секции 'Language and term details'.", comment);
			LanguageCommentEditMode.SetText(comment);

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в поле Definition в секции 'Language and term details'
		/// </summary>
		public GlossaryPage FillDefinition(string text)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле Definition в секции 'Language and term details'.", text);
			DefinitionEditMode.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в поле 'Definition source' в секции 'Language and term details'
		/// </summary>
		public GlossaryPage FillDefinitionSource(string text)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле 'Definition source' в секции 'Language and term details'.", text);
			DefinitionSourceEditMode.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Получить значение опции в дропдауне поля
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <param name="optionNumber">номер опции</param>
		public string OptionValue(GlossarySystemField fieldName, int optionNumber)
		{
			CustomTestContext.WriteLine("Получить значение опции №{0} в дропдауне поля {1}.", optionNumber, fieldName);

			return Driver.SetDynamicValue(How.XPath, OPTION_TEXT_IN_TERM_FIELD, fieldName.ToString(), optionNumber.ToString()).GetAttribute("value");
		}

		/// <summary>
		/// Раскрыть поле типа дропдаун
		/// </summary>
		public GlossaryPage ExpandDropdownTermField(GlossarySystemField fieldName)
		{
			CustomTestContext.WriteLine("Раскрыть поле {0} типа дропдаун.", fieldName);
			Driver.SetDynamicValue(How.XPath, DROPDOWN_TERM_FIELD, fieldName.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать опцию
		/// </summary>
		public GlossaryPage SelectOptionDropdownTermField(string option)
		{
			CustomTestContext.WriteLine("Выбрать опцию {0}.", option);
			Driver.SetDynamicValue(How.XPath, OPTION_IN_TERM_FIELD, option).Click();

			return GetPage();
		}

		/// <summary>
		/// Посчитать количество терминов в секции 'Languages and terms'
		/// </summary>
		public int TermsCountInLanguagesAndTermsSection()
		{
			CustomTestContext.WriteLine("Посчитать количесвто терминов в секции 'Languages and terms'.");

			return Driver.GetElementsCount(By.XPath(TERMS_IN_LANGUAGE_AND_TERMS_SECTION));
		}

		/// <summary>
		/// Нажать на термин в секции 'Languages and terms'
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		public GlossaryPage ClickTermInLanguagesAndTermsSection(int termNumber)
		{
			CustomTestContext.WriteLine("Нажать на термин №{0} в секции 'Languages and terms'.", termNumber);
			Driver.SetDynamicValue(How.XPath, TERM_INPUT_VIEW_MODE, termNumber.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в термин
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		public GlossaryPage EditTermInLanguagesAndTermsSection(string text, int termNumber)
		{
			CustomTestContext.WriteLine("Ввести текст {0} в термин №{1}.", text, termNumber);
			Driver.SetDynamicValue(How.XPath, TERM_INPUT, termNumber.ToString()).SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по русским терминам
		/// </summary>
		public GlossaryPage ClickSortByRussianTerm()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по русским терминам.");
			SortByRussianTerm.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате изменения
		/// </summary>
		public GlossaryPage ClickSortByDateModified()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по дате изменения");
			SortByDateModified.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить количество обычных терминов
		/// </summary>
		public int DefaultTermsCount()
		{
			CustomTestContext.WriteLine("Получить количество обычных терминов.");

			return Driver.GetElementsCount(By.XPath(DEFAULT_TERM_ROWS));
		}

		/// <summary>
		/// Получить количество расширенных терминов
		/// </summary>
		public int CustomTermsCount()
		{
			CustomTestContext.WriteLine("Получить количество расширенных терминов.");

			return Driver.GetElementsCount(By.XPath(CUSTOM_TERM_ROWS));
		}

		/// <summary>
		/// Нажать на строку термина
		/// </summary>
		public GlossaryPage ClickCustomTerm()
		{
			CustomTestContext.WriteLine("Нажать на строку термина.");
			CustomTermRow.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку закрытия раскрытых терминов
		/// </summary>
		public GlossaryPage CloseExpandedTerms()
		{
			CustomTestContext.WriteLine("Нажать кнопку закрытия раскрытых терминов.");
			CloseExpandTermsButton.Scroll();
			CloseExpandTermsButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку добавления синонима
		/// </summary>
		/// <param name="columnNumber">номер колонки</param>
		public GlossaryPage ClickSynonymPlusButton(int columnNumber)
		{
			CustomTestContext.WriteLine("Нажать кнопку добавления синонима.");
			var plusButton = Driver.SetDynamicValue(How.XPath, SYNONYM_PLUS_BUTTON, columnNumber.ToString());
			plusButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в поле синонима
		/// </summary>
		public GlossaryPage FillSynonym(string text, int columnNumber)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле синонима в столбце №{1}.", text, columnNumber);
			var synonymInput = Driver.SetDynamicValue(How.XPath, SYNONYM_INPUT, columnNumber.ToString());
			synonymInput.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Получить количесвто синонимов в поле
		/// </summary>
		/// <param name="termRow">номер строки термина</param>
		/// <param name="columnNumber">номер столбца</param>
		/// <returns>количество синонимов</returns>
		public int SynonymFieldsCount(int termRow, int columnNumber)
		{
			CustomTestContext.WriteLine("Получить количеcтво синонимов в термине №{0}, стоблец №{1}.", termRow, columnNumber);

			return Driver.GetElementsCount(By.XPath(SYNONYM_FIELDS_IN_COLUMN.Replace("term", termRow.ToString()).Replace("column", columnNumber.ToString())));
		}

		/// <summary>
		/// Нажать кнопку удаления термина
		/// </summary>
		/// <param name="source">источник</param>
		/// <param name="target">таргет</param>
		public GlossaryPage ClickDeleteButton(string source, string target)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления термина с sourse:{0}, target:{1}.", source, target);
			Driver.FindElement(By.XPath(DELETE_BUTTON.Replace("#", source).Replace("**", target))).Click();

			return GetPage();
		}

		/// <summary>
		/// Получить текст из термина в режиме просмотра
		/// </summary>
		public string TermFieldViewModelText(GlossarySystemField termField)
		{
			CustomTestContext.WriteLine("Получить текст из термина {0} в режиме просмотра.", termField);

			return Driver.SetDynamicValue(How.XPath, TERM_FIELD_VIEW_MODE, termField.Description()).Text;
		}

		/// <summary>
		/// Нажать на термин в колонке 'Langiages and terms'
		/// </summary>
		public GlossaryPage ClickTermInLanguagesAndTermsColumn(int termNumber = 2)
		{
			CustomTestContext.WriteLine("Нажать на термин №{0} в колонке 'Langiages and terms'.", termNumber);
			Driver.SetDynamicValue(How.XPath, TERM_IN_LANGUAGES_AND_TERMS_COLUMN, termNumber.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Заполнить поле
		/// </summary>
		public GlossaryPage FillTermField(GlossarySystemField termField, string text)
		{
			CustomTestContext.WriteLine("Заполнить поле {0}.", termField);

			Driver.SetDynamicValue(How.XPath, TERM_FIELD_EDIT_MODE, termField.ToString()).SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Выбрать значение в поле типа List
		/// </summary>
		public GlossaryPage SelectItemInListDropdown(string item)
		{
			CustomTestContext.WriteLine("Выбрать значение {0} в комбобоксе типа List.", item);
			Driver.SetDynamicValue(How.XPath, ITEMS_LIST, item).Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать значение в поле типа 'Multi-selection list'
		/// </summary>
		public GlossaryPage SelectItemInMultiselectListDropdown(string item)
		{
			CustomTestContext.WriteLine("Выбрать значение {0} в комбобоксе 'Multi-selection list'.", item);
			Driver.SetDynamicValue(How.XPath, MULTISELECT_LIST, item).Click();

			return GetPage();
		}

		/// <summary>
		/// Раскрыть дропаун Topic 
		/// </summary>
		public GlossaryPage ExpandTopicDropdown()
		{
			CustomTestContext.WriteLine("Раскрыть дропдаун Topic.");
			TopicField.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать значение в дропдауне Topic
		/// </summary>
		public GlossaryPage ClickOptionInTopicDropdown(string option)
		{
			CustomTestContext.WriteLine("Выбрать значение {0} в дропдауне Topic.", option);
			Driver.SetDynamicValue(How.XPath, TOPIC_OPTION, option.Trim()).Click();

			return GetPage();
		}

		/// <summary>
		/// Заполнить системное поле в новом термине
		/// </summary>
		/// <param name="fieldName">название системного поля</param>
		/// <param name="value">значение</param>
		public GlossaryPage FillSystemField(GlossarySystemField fieldName, string value)
		{
			CustomTestContext.WriteLine("Ввести {0} в системное поле {1}.", value, fieldName);
			Driver.SetDynamicValue(How.XPath, SYSTEM_FIELD_TEXTAREA_TYPE, fieldName.ToString()).SetText(value);

			return GetPage();
		}

		/// <summary>
		/// Раскрыть комбобокс типа List
		/// </summary>
		public GlossaryPage ExpandItemsListDropdown(string fieldName)
		{
			CustomTestContext.WriteLine("Раскрыть комбобокс {0} типа List.", fieldName);
			Driver.SetDynamicValue(How.XPath, ITEMS_LIST_DROPDOWN, fieldName).Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на комбобокс типа 'Multi-selection list'
		/// </summary>
		public GlossaryPage ClickMultiselectListDropdown(string fieldName)
		{
			CustomTestContext.WriteLine("Нажать на комбобокс {0} типа 'Multi-selection list'.", fieldName);
			Driver.SetDynamicValue(How.XPath, MULTISELECT_DROPDOWN, fieldName).Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по чекбоксу Yes/No
		/// </summary>
		public GlossaryPage ClickYesNoCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть по чекбоксу Yes/No.");
			YesNoCheckbox.Click();

			return GetPage();
		}

		/// <summary>
		/// Заполнить поле
		/// </summary>
		public GlossaryPage FillField(string fieldName, string text)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле {1}.", text, fieldName);
			var customField = Driver.SetDynamicValue(How.XPath, CUSTOM_FIELD_INPUT, fieldName);
			customField.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Ввести в поле типа Number.
		/// </summary>
		public GlossaryPage FillNumberCustomField(string fieldName, string text)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле {1} типа Number.", text, fieldName);
			var customNumberField = Driver.SetDynamicValue(How.XPath, CUSTOM_NUMBER_FIELD, fieldName);
			customNumberField.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Открыть календарь
		/// </summary>
		public GlossaryPage OpenCalendar(string fieldName)
		{
			CustomTestContext.WriteLine("Открыть календарь в поле {0}.", fieldName);
			Driver.SetDynamicValue(How.XPath, CUSTOM_DATE_FIELD, fieldName).Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать текущую дату в календаре
		/// </summary>
		public GlossaryPage ClickTodayInCalendar()
		{
			CustomTestContext.WriteLine("Выбрать текущую дату в календаре.");
			TodayDate.Click();

			return GetPage();
		}


		/// <summary>
		/// Получить текст из первого термина.
		/// </summary>
		public string FirstTermText()
		{
			CustomTestContext.WriteLine("Получить текст из первого термина.");

			return FirstTerm.Text.Trim();
		}

		/// <summary>
		/// Посчитать количество колонок с языками
		/// </summary>
		public int LanguageColumnCount()
		{
			CustomTestContext.WriteLine("Посчитать количество колонок с языками.");

			return Driver.GetElementsCount(By.XPath(LANGUAGE_COLUMNS));
		}

		/// <summary>
		/// Получить список текстов из терминов
		/// </summary>
		public List<string> TermsList()
		{
			CustomTestContext.WriteLine("Получить список текстов из терминов.");
			var terms = Driver.GetTextListElement(By.XPath(TERMS_TEXT));

			return terms.Select(el => el.Trim()).ToList();
		}


		/// <summary>
		/// Нажать кнопку редактирования термина
		/// </summary>
		public GlossaryPage ClickEditButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку редактирования термина.");
			EditTermButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Навести курсор мыши на заданную строку термина
		/// </summary>
		/// <param name="source">источник</param>
		/// <param name="target">таргет</param>
		public GlossaryPage HoverTermRow(string source, string target)
		{
			CustomTestContext.WriteLine("Навести курсор мыши на заданную строку термина с sourse:{0}, target:{1}", source, target);
			var term = Driver.FindElement(By.XPath(TERM_ROW_BY_SOURCE_AND_TARGET.Replace("#", source).Replace("**", target)));
			term.HoverElement();

			return GetPage();
		}

		/// <summary>
		/// Заполнить поле поиск
		/// </summary>
		public GlossaryPage FillSearchField(string text)
		{
			CustomTestContext.WriteLine(string.Format("Ввести {0} в поле поиск.", text));
			SearchInput.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку поиска 
		/// </summary>
		public GlossaryPage ClickSearchButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку поиска.");
			SearchButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Edit Entry'
		/// </summary>
		public GlossaryPage ClickEditEntryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Edit Entry'.");
			EditEntryButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel
		/// </summary>
		public GlossaryPage ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel.");
			CancelButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на поле Image
		/// </summary>
		public GlossaryPage ClickImageField(string fieldName)
		{
			CustomTestContext.WriteLine("Нажать на поле Image.");
			Driver.SetDynamicValue(How.XPath, IMAGE_FIELD, fieldName).Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку Add в поле типа Media
		/// </summary>
		public GlossaryPage ClickAddMediaButton(string fieldName)
		{
			CustomTestContext.WriteLine("Нажать на кнопку Add в поле {0} типа Media.", fieldName);
			Driver.SetDynamicValue(How.XPath, ADD_MEDIA_BUTTON, fieldName).Click();

			return GetPage();
		}

		/// <summary>
		/// Получить названия колонок с языками
		/// </summary>
		public List<string> GetLanguagesColumnList()
		{
			CustomTestContext.WriteLine("Получить названия колонок с языками.");

			return Driver.GetElementList(By.XPath(LANGUAGE_COLUMNS)).Select(l => l.Text.Replace("Term", "").Trim()).ToList();
		}

		/// <summary>
		/// Получить текст из фильтра 'Created'
		/// </summary>
		public string GetCreatedByFilterText()
		{
			CustomTestContext.WriteLine("Получить текст из фильтра 'Created'.");

			return CreatedByFilterLabel.Text;
		}

		/// <summary>
		/// Нажать кнопку Filter
		/// </summary>
		public FilterDialog ClickFilterButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Filter.");
			FilterButton.Click();

			return new FilterDialog(Driver).GetPage();
		}

		/// <summary>
		/// Получить список терминов
		/// </summary>
		public List<string> GetTermList()
		{
			CustomTestContext.WriteLine("Получить список терминов.");

			return Driver.GetElementList(By.XPath(ALL_TERMS)).Select(t => t.Text).ToList();
		}

		/// <summary>
		/// Удалить фильтр Created
		/// </summary>
		public GlossaryPage ClickDeleteCreatedFilter(string filterValue)
		{
			CustomTestContext.WriteLine("Удалить фильтр Created со значением {0}.", filterValue);
			Driver.SetDynamicValue(How.XPath, DELETE_CREATED_FILTER_BUTTON, filterValue).Click();

			return GetPage();
		}

		/// <summary>
		/// Удалить фильтр Modified
		/// </summary>
		public GlossaryPage ClickDeleteModifiedFilter(string filterValue)
		{
			CustomTestContext.WriteLine("Удалить фильтр Modified со значением {0}.", filterValue);
			Driver.SetDynamicValue(How.XPath, DELETE_MODIFIED_FILTER_BUTTON, filterValue).Click();

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Удалить термин из глоссария
		/// </summary>
		/// <param name="source">термин</param>
		public GlossaryPage DeleteTerm(string source)
		{
			CustomTestContext.WriteLine("Удалить термин {0} из глоссария", source);
			var deleteTermButton = Driver.SetDynamicValue(How.XPath, DELETE_TERM_BUTTON, source);
			var termRow = Driver.SetDynamicValue(How.XPath, SOURCE_TERM, source);
			termRow.HoverElement();
			deleteTermButton.Click();

			return this;
		}

		/// <summary>
		/// Добавить термин и ввести текст в поле термина в секции 'Languages and terms'
		/// </summary>
		/// <param name="text">текст</param>
		public GlossaryPage FillTermInLanguagesAndTermsSection(string text)
		{
			CustomTestContext.WriteLine("Добавить термин и ввести текст в поле термина в секции 'Languages and terms'.");
			Driver.WaitUntilElementIsDisplay(By.XPath(ADD_BUTTON_LIST));
			var addButtonLists = Driver.GetElementList(By.XPath(ADD_BUTTON_LIST));

			for (var i = 1; i <= addButtonLists.Count; i++)
			{
				CustomTestContext.WriteLine("Нажать кнопку Add №{0} в 'Languages and terms'.", i);
				addButtonLists[i - 1].Click();

				CustomTestContext.WriteLine("Ввести {0} в поле термина №{1} в секции 'Languages and terms'.", text, i);
				Driver.SetDynamicValue(How.XPath, TERM_INPUT, i.ToString()).SetText(text);
			}

			return GetPage();
		}

		/// <summary>
		/// Загрузка файла картинка.
		/// </summary>
		/// <param name="filepath">путь к файлу</param>
		public GlossariesPage UploadImageFile(string filepath)
		{
			CustomTestContext.WriteLine("Загрузка файла {0}.\nВвести путь к файлу в системное окно.", filepath);
			AddImageLink.HoverElement();
			Driver.ExecuteScript("$(\"input:file[name = x1]\").removeClass(\"g-hidden\").css(\"opacity\", 100).css(\"width\", 500)");
			AddImageInput.SendKeys(filepath);
			Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_IMAGE_BUTTON));

			return GetPage();
		}

		/// <summary>
		/// Загрузка файла картинка и мультимедиа.
		/// </summary>
		/// <param name="filepath">путь к файлу</param>
		public GlossariesPage UploadImageFileWithMultimedia(string filepath)
		{
			CustomTestContext.WriteLine("Загрузка файла {0}.\nВвести путь к файлу в системное окно.", filepath);
			AddImageLink.HoverElement();
			Driver.ExecuteScript("$(\"input:file[name = Image]\").removeClass(\"g-hidden\").css(\"opacity\", 100).css(\"width\", 500)");
			AddImageInputWithMultimedia.SendKeys(filepath);
			Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_IMAGE_BUTTON));

			return GetPage();
		}

		/// <summary>
		/// Загрузка файла мультимедиа.
		/// </summary>
		/// <param name="filepath">путь к файлу</param>
		public GlossariesPage UploadMultimediaFile(string filepath)
		{
			CustomTestContext.WriteLine("Загрузка файла {0}.\nВвести путь к файлу в системное окно.", filepath);
			AddMultimediaLink.HoverElement();
			try
			{
				Driver.ExecuteScript("$(\"input:file[name = Multimedia]\").removeClass(\"g-hidden\").css(\"opacity\", 100).css(\"width\", 500)");
				Driver.FindElement(By.XPath("//input[@type='file' and @name='Multimedia']")).SendKeys(filepath);
			}
			catch
			{
				CustomTestContext.WriteLine("Элемент отсутствует");
			}

			try
			{
				Driver.ExecuteScript("$(\"input:file[name = x1]\").removeClass(\"g-hidden\").css(\"opacity\", 100).css(\"width\", 500)");
				Driver.FindElement(By.XPath("//input[@type='file' and @name='x1']")).SendKeys(filepath);
			}
			catch
			{
				CustomTestContext.WriteLine("Элемент отсутствует");
			}

			Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_IMAGE_BUTTON));

			return GetPage();
		}

		#endregion

		/// <summary>
		/// Проверить, что новый термин открыт в расширенном режиме
		/// </summary>
		public GlossaryPage AssertExtendModeOpen()
		{
			CustomTestContext.WriteLine("Проверить, что новый термин открыт в расширенном режиме.");

			Assert.IsTrue(ExtendMode.Displayed, "Произошла ошибка:\n новый термин не открыт в расширенном режиме.");

			return GetPage();
		}


		/// <summary>
		/// Проверить, что глоссарий содержит необходимое количество терминов
		/// </summary>
		public GlossaryPage AssertGlossaryContainsCorrectTermsCount(int expectedTermsCount)
		{
			CustomTestContext.WriteLine("Проверить, что глоссарий содержит {0} терминов.", expectedTermsCount);
			var actualTermsCount = Driver.GetElementsCount(By.XPath(TERM_ROW));

			Assert.AreEqual(actualTermsCount, expectedTermsCount,
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что комментарий содержит текст.
		/// </summary>
		public GlossaryPage AssertCommentIsFilled(string comment)
		{
			CustomTestContext.WriteLine("Проверить, что комментарий содержит текст {0}.", comment);

			Assert.AreEqual(comment, LanguageCommentViewMode.Text, "Произошла ошибка:\n неверный текст в поле комментария.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что поле Definition содержит текст.
		/// </summary>
		public GlossaryPage AssertDefinitionIsFilled(string text)
		{
			CustomTestContext.WriteLine("Проверить, что поле Definition содержит текст {0}.", text);

			Assert.AreEqual(text, DefinitionViewMode.Text, "Произошла ошибка:\n неверный текст в поле Definition.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что поле 'Definition source' содержит текст.
		/// </summary>
		public GlossaryPage AssertDefinitionSourceIsFilled(string text)
		{
			CustomTestContext.WriteLine("Проверить, что поле 'Definition source' содержит текст {0}.", text);

			Assert.AreEqual(text, DefinitionSourceViewMode.Text,
				"Произошла ошибка:\n неверный текст в поле 'Definition source'.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что термины подсвечены красным цветом, так как термины должны быть уникальны
		/// </summary>
		public GlossaryPage AssertSynonumUniqueErrorDisplayed(int columnNumber)
		{
			CustomTestContext.WriteLine("Проверить, что термины подсвечены красным цветом в стоблце №{0}, так как термины должны быть уникальны.", columnNumber);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(SYNONYM_UNIQUE_ERROR.Replace("*#*", columnNumber.ToString()))),
				"Произошла ошибка:\n Термины не подсвечены красным цветом в стоблце №{0}.", columnNumber);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка удаления термина исчезла
		/// </summary>
		public GlossaryPage AssertDeleteButtonDisappeared(string source, string target)
		{
			CustomTestContext.WriteLine("Проверить, что кнопка удаления термина исчезла.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_BUTTON.Replace("#", source).Replace("**", target))),
				"Произошла ошибка:\n Кнопка удаления термина не исчезла.");

			return GetPage();
		}


		public GlossaryPage AssertProgressUploadDissapeared(string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что прогресс загрузки медиа файла исчез.", fieldName);

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(PROGRESS_MEDIA_FILE.Replace("*#*", fieldName)), timeout: 45),
				"Произошла ошибка:\n прогресс загрузки медиа файла не исчез.");

			return GetPage();
		}

		/// <summary>
		/// Проверить наличие термина
		/// </summary>
		/// <param name="text">текст</param>
		public GlossaryPage AssertIsSingleTermExists(string text)
		{
			CustomTestContext.WriteLine("Проверить наличие термина {0}", text);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(SOURCE_TERM.Replace("*#*", text))),
				"Произошла ошибка:\n термин не обнаружен.");

			return GetPage();
		}

		/// <summary>
		/// Получить, есть ли термин с сорсом и таргетом
		/// </summary>
		/// <param name="sourceText">сорс</param>
		/// <param name="targetText">таргет</param>
		public GlossaryPage AssertIsSingleTermWithTranslationExists(string sourceText, string targetText)
		{
			CustomTestContext.WriteLine("Получить, есть ли термин с сорсом {0} и таргетом {1}", sourceText, targetText);

			Assert.IsTrue(
				Driver.WaitUntilElementIsDisplay(
					By.XPath(SOURCE_TARGET_TERM.Replace("#", sourceText).Replace("**", targetText))),
				"Произошла ошибка:\n термин не обнаружен.");

			return GetPage();
		}

		/// <summary>
		/// Получить, есть ли термин с сорсом, таргетом и комментом
		/// </summary>
		/// <param name="sourceText">сорс</param>
		/// <param name="targetText">таргет</param>
		/// <param name="comment">комментарий</param>
		public GlossaryPage AssertIsTermWithTranslationAndCommentExists(string sourceText, string targetText, string comment)
		{
			CustomTestContext.WriteLine("Получить, есть ли термин с сорсом '{0}', таргетом '{1}' и комментом: '{2}'", sourceText, targetText,
				comment);

			Assert.IsTrue(
				Driver.WaitUntilElementIsDisplay(
					By.XPath(SOURCE_TARGET_TERM_WITH_COMMENT.Replace("#", sourceText).Replace("**", targetText).Replace("$$", comment))),
				"Произошла ошибка:\n термин не обнаружен.");

			return GetPage();
		}

		/// <summary>
		/// Получить, есть ли термин с сорсом и комментом
		/// </summary>
		/// <param name="sourceText">сорс</param>
		/// <param name="comment">комментарий</param>
		public GlossaryPage AssertIsTermWithCommentExists(string sourceText, string comment)
		{
			CustomTestContext.WriteLine("Получить, есть ли термин с сорсом '{0}' и комментом: '{1}'", sourceText, comment);

			Assert.IsTrue(
				Driver.WaitUntilElementIsDisplay(
					By.XPath(SOURCE_TERM_WITH_COMMENT.Replace("#", sourceText).Replace("$$", comment))),
				"Произошла ошибка:\n термин не обнаружен.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что значение в поле совпадает с ожидаемым значением
		/// </summary>
		public GlossaryPage AssertFieldValueMatch(GlossarySystemField fieldName, string text)
		{
			CustomTestContext.WriteLine("Проверить, что значение в поле {0} совпадает с ожидаемым значением {1}.", fieldName, text);
			var customField = Driver.SetDynamicValue(How.XPath, CUSTOM_FIELD_VIEW_MODE, fieldName.Description());

			Assert.AreEqual(text, customField.Text,
				"Произошла ошибка:\n значение в поле не совпадает с ожидаемым значением.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что значение в поле совпадает с ожидаемым значением
		/// </summary>
		public GlossaryPage AssertCustomFieldValueMatch(string fieldName, string text)
		{
			CustomTestContext.WriteLine("Проверить, что значение в кастомном поле {0} совпадает с ожидаемым значением {1}.", fieldName, text);
			var customField = Driver.SetDynamicValue(How.XPath, CUSTOM_FIELD_VIEW_MODE, fieldName);

			Assert.AreEqual(text, customField.Text,
				"Произошла ошибка:\n значение в кастомном поле не совпадает с ожидаемым значением.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что поле присутствует в новом термине
		/// </summary>
		public GlossaryPage AssertFieldExistInNewEntry(string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что поле {0} присутствует в новом термине.", fieldName);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CUSTOM_FIELD_NAME.Replace("*#*", fieldName))),
				"Произошла ошибка:\n поле {0} отсутствует в новом термине.", fieldName);


			return GetPage();
		}

		/// <summary>
		/// Проверить, что термин содержит правильные синонимы
		/// </summary>
		/// <param name="columnNumber">номер колонки</param>
		/// <param name="synonyms">синонимы</param>
		public GlossaryPage AssertSynonymsMatch(int columnNumber, List<string> synonyms)
		{
			CustomTestContext.WriteLine("Проверить, что термин содержит правильные синонимы.");
			var synonumsList = Driver.GetTextListElement(By.XPath(TERM_TEXT.Replace("*#*", columnNumber.ToString())));

			Assert.IsTrue(synonyms.SequenceEqual(synonumsList), "Произошла ошибка:\nНеверный список синонимов.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что поле типа Image присутствует в новом термине
		/// </summary>
		public GlossaryPage AssertImageFieldExistInNewEntry(string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что поле {0} типа Image присутствует в новом термине.", fieldName);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(IMAGE_FIELD.Replace("*#*", fieldName))),
				"Произошла ошибка:\n поле {0} типа Image отсутствует в новом термине.", fieldName);

			return GetPage();
		}
		
		/// <summary>
		/// Проверить, что поле типа Media присутствует в новом термине
		/// </summary>
		public GlossaryPage AssertMediaFieldExistInNewEntry(string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что поле {0} типа Media присутствует в новом термине.", fieldName);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(MEDIA_FIELD.Replace("*#*", fieldName))),
				"Произошла ошибка:\n поле {0} типа Media отсутствует в новом термине.", fieldName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что текст в поле {0} совпадает с дефолтным значением
		/// </summary>
		public GlossaryPage AssertCustomDefaultValueMatch(string fieldName, string defaultValue)
		{
			CustomTestContext.WriteLine("Проверить, что текст в поле {0} совпадает с дефолтным значением {1}.", fieldName, defaultValue);

			Assert.AreEqual(
				defaultValue,
				Driver.SetDynamicValue(How.XPath, CUSTOM_FIELD_NAME, fieldName).Text,
				"Произошла ошибка:\n неверное значение в поле.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что поле подсвечено красным цветом, так как обязательно для заполнения
		/// </summary>
		public GlossaryPage AssertFieldErrorDisplayed(string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что поле {0} подсвечено красным цветом, так как обязательно для заполнения.", fieldName);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CUSTOM_FIELD_ERROR.Replace("*#*", fieldName))),
				"Произошла ошибка:\n поле {0} не подсвечено красным цветом.", fieldName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что поле типа Image подсвечено красным цветом, так как обязательно для заполнения
		/// </summary>
		public GlossaryPage AssertImageFieldErrorDisplayed(string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что поле {0} типа Image подсвечено красным цветом, так как обязательно для заполнения.", fieldName);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CUSTOM_IMAGE_FIELD_ERROR.Replace("*#*", fieldName))),
				"Произошла ошибка:\n поле {0} не подсвечено красным цветом.", fieldName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что поле Image заполнено
		/// </summary>
		public GlossaryPage AssertImageFieldFilled(string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что поле {0} типа Image заполнено.", fieldName);

			Assert.IsTrue(
				Driver.SetDynamicValue(How.XPath, FILLED_IMAGE_FIELD, fieldName)
					.GetAttribute("src")
					.Trim()
					.Length > 0,
				"Произошла ошибка:\n  поле {0} типа Image не заполнено.", fieldName);

			return GetPage();
		}


		/// <summary>
		/// Проверить, что стоит или не стоит галочка в Yes/No чекбоксе
		/// </summary>
		public GlossaryPage AssertYesNoCheckboxChecked(string yesNo, string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что в поле {0} типа Yes/No стоит или не стоит галочка {1}.", fieldName, yesNo);

			Assert.AreEqual(yesNo, Driver.SetDynamicValue(How.XPath, YES_NO_CHECKBOX_VIEW_MODE, fieldName).Text,
				"Произошла ошибка:\n неверное значение в поле Yes/No.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что в поле типа Media правильное название файла
		/// </summary>
		public GlossaryPage AssertMediaFileMatch(string mediaFile, string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что в поле {0} типа Media правльное название файла {1}.", fieldName, mediaFile);

			Assert.AreEqual(mediaFile, Driver.SetDynamicValue(How.XPath, MEDIA_FIELD_TEXT,fieldName).Text,
				"Произошла ошибка:\n неверное значение в поле {0} типа Media.", fieldName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что текстовое системное поле отображается  в новом термине
		/// </summary>
		/// <param name="fieldName">название системного поля</param>
		public GlossaryPage AssertSystemTextAreaFieldDisplayed(GlossarySystemField fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что текстовое системное поле {0} отображается в новом термине.", fieldName);

			Assert.IsTrue(Driver.SetDynamicValue(How.XPath, SYSTEM_FIELD_TEXTAREA_TYPE, fieldName.ToString()).Displayed,
				"Произошла ошибка:\nТекстовое cистемное поле {0} не отображается  в новом термине.", fieldName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что системное поле типа дропдаун отображается в новом термине
		/// </summary>
		/// <param name="fieldName">название системного поля</param>
		public GlossaryPage AssertSystemDropdownFieldDisplayed(GlossarySystemField fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что в системное поле {0} типа дропдаун отображается в новом термине.", fieldName);

			Assert.IsTrue(Driver.SetDynamicValue(How.XPath, SYSTEM_FIELD_DROPDOWN_TYPE, fieldName.Description()).Displayed,
				"Произошла ошибка:\nСистемное поле {0} вида дропдаун не отображается в новом термине.", fieldName);

			return GetPage();
		}

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = FIRST_TERM)]
		protected IWebElement FirstTerm { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_INPUT)]
		protected IWebElement SearchInput { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_BUTTON)]
		protected IWebElement SearchButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = NEW_ENTRY_BUTTON)]
		protected IWebElement NewEntryButton { get; set; }

		[FindsBy(How = How.XPath, Using = PLUS_BUTTON)]
		protected IWebElement PlusButton { get; set; }

		[FindsBy(How = How.XPath, Using = TERM_SAVE_BUTTON)]
		protected IWebElement TermSaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_ENTRY_BUTTON)]
		protected IWebElement SaveEntryButton { get; set; }

		[FindsBy(How = How.XPath, Using = EDIT_GLOSSARY_MENU)]
		protected IWebElement EditGlossaryMenu { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY_PROPERTIES)]
		protected IWebElement GlossaryProperties { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY_STRUCTURE)]
		protected IWebElement GlossaryStructure { get; set; }

		[FindsBy(How = How.XPath, Using = EXTEND_MODE)]
		protected IWebElement ExtendMode { get; set; }

		[FindsBy(How = How.XPath, Using = EXPORT_BUTTON)]
		protected IWebElement ExportButton { get; set; }

		[FindsBy(How = How.XPath, Using = IMPORT_BUTTON)]
		protected IWebElement ImportButton { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_ENGLISH_TERM)]
		protected IWebElement SortByEnglishTerm { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_RUSSIAN_TERM)]
		protected IWebElement SortByRussianTerm { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_DATE_MODIFIED)]
		protected IWebElement SortByDateModified { get; set; }

		[FindsBy(How = How.XPath, Using = LANGUAGE_HEADER_EDIT_MODE)]
		protected IWebElement LanguageHeaderEditMode { get; set; }

		[FindsBy(How = How.XPath, Using = LANGUAGE_HEADER_VIEW_MODE)]
		protected IWebElement LanguageHeaderViewMode { get; set; }

		[FindsBy(How = How.XPath, Using = LANGUAGE_COMMENT_EDIT_MODE)]
		protected IWebElement LanguageCommentEditMode { get; set; }

		[FindsBy(How = How.XPath, Using = LANGUAGE_COMMENT_VIEW_MODE)]
		protected IWebElement LanguageCommentViewMode { get; set; }

		[FindsBy(How = How.XPath, Using = CUSTOM_TERM_ROWS)]
		protected IWebElement CustomTermRow { get; set; }

		[FindsBy(How = How.XPath, Using = DEFINITION_EDIT_MODE)]
		protected IWebElement DefinitionEditMode { get; set; }

		[FindsBy(How = How.XPath, Using = DEFINITION_VIEW_MODE)]
		protected IWebElement DefinitionViewMode { get; set; }

		[FindsBy(How = How.XPath, Using = DEFINITION_SOURCE_EDIT_MODE)]
		protected IWebElement DefinitionSourceEditMode { get; set; }

		[FindsBy(How = How.XPath, Using = DEFINITION_SOURCE_VIEW_MODE)]
		protected IWebElement DefinitionSourceViewMode { get; set; }

		[FindsBy(How = How.XPath, Using = CLOSE_EXPAND_TERMS_BUTTON)]
		protected IWebElement CloseExpandTermsButton { get; set; }

		[FindsBy(How = How.XPath, Using = EDIT_TERM_BUTTON)]
		protected IWebElement EditTermButton { get; set; }

		[FindsBy(How = How.XPath, Using = SYNONYM_INPUT)]
		protected IWebElement SynonymInput { get; set; }

		[FindsBy(How = How.XPath, Using = EDIT_ENTRY_BUTTON)]
		protected IWebElement EditEntryButton { get; set; }

		[FindsBy(How = How.XPath, Using = CUSTOM_DATE_FIELD)]
		protected IWebElement CustomDateField { get; set; }

		[FindsBy(How = How.XPath, Using = TODAY_DATE)]
		protected IWebElement TodayDate { get; set; }

		[FindsBy(How = How.XPath, Using = YES_NO_CHECKBOX)]
		protected IWebElement YesNoCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = TOPIC_FIELD)]
		protected IWebElement TopicField { get; set; }

		[FindsBy(How = How.LinkText, Using = ADD_IMAGE_LINK)]
		protected IWebElement AddImageLink { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_IMAGE_INPUT_WITH_MULTIMEDIA)]
		protected IWebElement AddImageInputWithMultimedia { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_IMAGE_INPUT)]
		protected IWebElement AddImageInput { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_MULTIMEDIA_LINK)]
		protected IWebElement AddMultimediaLink { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_IMAGE_BUTTON)]
		protected IWebElement DeleteImageButton { get; set; }

		[FindsBy(How = How.XPath, Using = FILTER_BUTTON)]
		protected IWebElement FilterButton { get; set; }

		[FindsBy(How = How.XPath, Using = CREATED_BY_FILTER_LABEL)]
		protected IWebElement CreatedByFilterLabel { get; set; }

		[FindsBy(How = How.XPath, Using = MODIFIED_BY_FILTER_LABEL)]
		protected IWebElement ModifiedByFilterLabel { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string GLOSSARY_SAVE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[@class='g-btn g-redbtn ']";
		protected const string GLOSSARY_PROPERTIES = "//div[contains(@class,'js-edit-glossary-btn')]";
		protected const string EDIT_GLOSSARY_MENU = "//span[contains(@class,'js-edit-submenu')]";
		protected const string TERM_FIELD = "//tr[contains(@class, 'js-concept')]//td[*#*]//input[contains(@class,'js-term')]";
		protected const string NEW_ENTRY_BUTTON = "//span[contains(@class,'js-add-concept')]";
		protected const string PLUS_BUTTON = "//td[1]//span[contains(@class,'js-add-term')]";
		protected const string TERM_SAVE_BUTTON = "//span//a[contains(@class,'js-save-btn')]";
		protected const string SAVE_ENTRY_BUTTON = "//span[contains(@class,'g-btn g-bluebtn js-save-btn js-edit l-corpr__btnmargin')]";
		protected const string GLOSSARY_STRUCTURE = "//div[contains(@class,'js-edit-structure-btn')]";
		protected const string EXTEND_MODE = "//tr[contains(@class, 'js-concept')]//td";
		protected const string IMPORT_BUTTON = "//span[contains(@class,'js-import-concepts')]";
		protected const string EXPORT_BUTTON = "//a[contains(@class,'js-export-concepts')]";
		protected const string TERM_ROW = "//tr[contains(@class, 'js-concept-row')]";
		protected const string TERMS_TEXT = "//tr[contains(@class, 'js-concept-row')]//td[contains(@class, 'glossaryLang')]//p";

		protected const string SORT_BY_ENGLISH_TERM = "//th[contains(@data-sort-by,'Language1')]//a";
		protected const string SORT_BY_RUSSIAN_TERM = "//th[contains(@data-sort-by,'Language2')]//a";
		protected const string SORT_BY_DATE_MODIFIED = "//th[contains(@data-sort-by,'LastModifiedDate')]//a";

		protected const string LANGUAGE_HEADER_EDIT_MODE = "//div[contains(@class, 'corprtree')]//div[contains(@class, 'corprtree__langbox')][1]//div[contains(@class, 'l-inactive-lang')]";
		protected const string LANGUAGE_HEADER_VIEW_MODE = "//div[contains(@class, 'js-lang-node')]//span[contains(text(), 'English')]";
		protected const string LANGUAGE_COMMENT_EDIT_MODE = "//div[contains(@class, 'lang-attrs')][1]//textarea[@name='Comment']";
		protected const string LANGUAGE_COMMENT_VIEW_MODE = "//td[@class='l-corpr__tbledit__td js-details-panel']//div[@class='l-corpr__viewmode js-lang-attrs']//div[@class='js-control'][1]//div[contains(@class, 'viewmode__val js-value')]";
		protected const string ADD_BUTTON = "//div[@class='l-corprtree__langbox']['*#*']//div[contains(@class, 'l-inactive-lang')]//span[contains(@class, 'addbtn js-add-term')]";
		protected const string TERM_LANGUAGE = "//span[@class='js-term-editor g-block l-corprtree__edittrasl']//input[contains(@class, 'txttrasl')]";
		protected const string ADD_BUTTON_LIST = "//div[contains(@class, 'js-terms-tree')]//div[contains(@class, 'l-corprtree__langbox')]//span[contains(@class,'js-add-term')]";
		protected const string TERM_INPUT = "//div[contains(@class, 'js-terms-tree')]//div[contains(@class, 'l-corprtree__langbox')][*#*]//span[contains(@class,'js-term-editor')]//input";
		protected const string TERM_INPUT_VIEW_MODE = "//div[contains(@class, 'js-terms-tree')]//div[contains(@class, 'l-corprtree__langbox')][*#*]//span[@class='js-term-viewer']";
		protected const string DEFINITION_EDIT_MODE = "//div[contains(@class, 'viewmode js-lang-attrs')]//textarea[@name='Interpretation']";
		protected const string DEFINITION_VIEW_MODE = "//div[@class='l-corpr__viewmode js-lang-attrs']//div[@class='js-control'][2]//div[@class='g-bold l-corpr__viewmode__val js-value']";
		protected const string DEFINITION_SOURCE_VIEW_MODE = "//div[@class='l-corpr__viewmode js-lang-attrs']//div[@class='js-control'][3]//div[@class='g-bold l-corpr__viewmode__val js-value']";
		protected const string DEFINITION_SOURCE_EDIT_MODE = "//div[contains(@class, 'viewmode js-lang-attrs')]//textarea[@name='InterpretationSource']";

		protected const string CUSTOM_TERM_ROWS = "//tr[@class='l-corpr__trhover clickable js-concept-row']";
		protected const string CLOSE_EXPAND_TERMS_BUTTON = "//span[contains(@class, 'close-all')]";
		protected const string DEFAULT_TERM_ROWS = "//tr[@class='l-corpr__trhover js-concept-row']";
		protected const string ALREADY_EXIST_TERM_ERROR = "//span[contains(text(),'The term already exists')]";
		protected const string EMPTY_TERM_ERROR = "//div[contains(text(),'Please add at least one term.')]";
		protected const string SYNONYM_PLUS_BUTTON = "//tr[contains(@class, 'js-concept')]//td[*#*]//span[contains(@class,'js-add-term')]";
		protected const string SYNONYM_INPUT = "//tr[contains(@class, 'js-concept')]//td[*#*]//div//input[contains(@class,'js-term')]";
		protected const string SYNONYM_FIELDS_IN_COLUMN = "//tr[@class='l-corpr__trhover js-concept-row'][term]//td[column]//p";
		protected const string SYNONYM_UNIQUE_ERROR = "//td[*#*]//p[@title='A term must be unique within a language.']";
		protected const string DELETE_BUTTON = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '**')]//a[contains(@class, 'js-delete-btn')]";
		protected const string TERM_ROW_BY_SOURCE_AND_TARGET = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '**')]//td[4]";
		protected const string CANCEL_BUTTON = "//tr[contains(@class, 'js-concept-row js-editing opened')]//a[contains(@class, 'js-cancel-btn')]";
		protected const string SEARCH_INPUT = "//input[contains(@class,'js-search-term')]";
		protected const string SEARCH_BUTTON = "//a[contains(@class,'js-search-by-term')]";
		protected const string FIRST_TERM = "//tr[contains(@class, 'js-concept-row')]//td[contains(@class,'glossaryShort')]//p";
		protected const string LANGUAGE_COLUMNS = "//tr[@class='js-table-header']//th[contains(@data-sort-by, 'Language')]";
		protected const string EDIT_TERM_BUTTON = "//tr[contains(@class, 'js-concept-row')]//a[contains(@class,'js-edit-btn')]";
		protected const string EDIT_ENTRY_BUTTON = "//span[contains(@class,'js-edit-btn')]";
		protected const string TERMS_IN_LANGUAGE_AND_TERMS_SECTION = "//div[@class='l-corprtree__langbox']";

		protected const string SINGLE_TERM = "//tr[contains(@class, 'js-concept-row')]//td[1]/p../following-sibling::td[1][contains(string(), '*#*')]";
		protected const string SOURCE_TERM = "//tr[contains(@class, 'js-concept-row') and contains(string(), '*#*')]";
		protected const string SOURCE_TARGET_TERM = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '**')]";
		protected const string SOURCE_TERM_WITH_COMMENT = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '$$')]";
		protected const string SOURCE_TARGET_TERM_WITH_COMMENT = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '**') and contains(string(), '$$')]";
		protected const string DELETE_TERM_BUTTON = "//tr[contains(@class, 'js-concept-row') and contains(string(), '*#*')]//a[@title='Delete']";
		protected const string TERM_TEXT = "//tr[contains(@class, 'js-concept-row')]//td[contains(@class,'glossaryShort')][*#*]//p";
		protected const string CUSTOM_FIELD_NAME = "//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]";
		protected const string CUSTOM_FIELD_INPUT = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]/..//textarea";
		protected const string CUSTOM_FIELD_VIEW_MODE = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class, 'js-view')]//p[text() ='*#*']//following-sibling::*";
		protected const string CUSTOM_FIELD = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]/..//textarea";
		protected const string CUSTOM_FIELD_ERROR = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit l-error')]//p[contains(text(),'*#*')]";
		protected const string CUSTOM_DATE_FIELD = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]/../input[contains(@class,'hasDatepicker')]";
		protected const string TODAY_DATE = "//table[contains(@class,'ui-datepicker-calendar')]//td[contains(@class,'ui-datepicker-today')]";
		protected const string IMAGE_FIELD = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(text(),'*#*')]/..//div[contains(@class,'l-editgloss__imagebox')]//a";
		protected const string FILLED_IMAGE_FIELD = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(text(),'*#*')]/../../div[contains(@class,'l-editgloss__image')]//img[contains(@class,'l-editgloss__imageview')]";
		protected const string CUSTOM_IMAGE_FIELD_ERROR = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')][contains(@class,'l-error')]//p[contains(text(), '*#*')]";
		protected const string ADD_MEDIA_BUTTON = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(text(),'*#*')]/..//span[contains(@class,'l-editgloss__linkbox')]//a[contains(@class,'js-upload-btn')]";
		protected const string CUSTOM_NUMBER_FIELD = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]/..//input[contains(@class,'js-submit-input')]";
		protected const string YES_NO_CHECKBOX = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//span[contains(@class,'l-editgloss__name')]/..//span[contains(@class,'js-chckbx')]//input[contains(@class,'js-chckbx__orig')]";
		protected const string YES_NO_CHECKBOX_VIEW_MODE = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class, 'js-view')]//p[contains(text(),'*#*')]/..//div";
		protected const string ITEMS_LIST_DROPDOWN = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]/..//span[contains(@class,'js-dropdown')]";
		protected const string ITEMS_LIST = "//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item')][@title='*#*']";
		protected const string MULTISELECT_DROPDOWN = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]/..//div[contains(@class,'ui-multiselect')]";
		protected const string MULTISELECT_LIST = "//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')][text()='*#*']";
		protected const string MEDIA_FIELD = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(text(),'*#*')]";
		protected const string PROGRESS_MEDIA_FILE = "//p[contains(text(), '*#*')]/..//img[contains(@class, 'prgrssbar ')]";
		protected const string MEDIA_FIELD_TEXT = "//p[contains(text(), '*#*')]/..//a[contains(@class, 'js-filename-link')]";
		protected const string ADD_IMAGE_LINK = "Add";
		protected const string ADD_IMAGE_INPUT = "//input[@type='file' and @name='x1']";
		protected const string ADD_IMAGE_INPUT_WITH_MULTIMEDIA = "//input[@type='file' and @name='Image']";
		protected const string ADD_MULTIMEDIA_LINK = "//div[@class='l-editgloss__fileName js-filename-click-area js-tour-file-area']";
		protected const string DELETE_IMAGE_BUTTON = "//div[@class='l-editgloss__rmvimgbtn js-clear-btn']";

		protected const string SYSTEM_FIELD_TEXTAREA_TYPE = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//textarea[@name='*#*']";
		protected const string SYSTEM_FIELD_DROPDOWN_TYPE = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//div[contains(@class, 'js-edit')]//p[text()='*#*']";
		protected const string TOPIC_FIELD = "//div[contains(@class,'ui-dropdown-treeview-wrapper')]";
		protected const string TOPIC_OPTION = "//div[contains(@class,'ui-treeview_node')]//div/span[text()='Life']";
		protected const string TERM_FIELD_EDIT_MODE = "//div[@class='l-corpr__viewmode js-term-attrs']//textarea[@name='*#*']";
		protected const string TERM_FIELD_VIEW_MODE = "//div[@class='l-corpr__viewmode js-term-attrs']//p[@title='*#*']//following-sibling::div";
		protected const string DROPDOWN_TERM_FIELD_EDIT_MODE = "//td[contains(@class,'js-details-panel')]//select[@name='*#*']";
		protected const string TERM_IN_LANGUAGES_AND_TERMS_COLUMN = "//div[@class='l-corprtree__langbox'][*#*]//span[contains(@class, 'transtxt js-text')]";
		protected const string DROPDOWN_TERM_FIELD_VIEW_MODE = "//td[contains(@class,'js-details-panel')]//div[@class='l-corpr__viewmode js-term-attrs']//select[@name='*#*']/../..//div[contains(@class,'js-value')]";
		protected const string OPTION_TEXT_IN_TERM_FIELD = "//td[contains(@class,'js-details-panel')]//div[@class='l-corpr__viewmode js-term-attrs']//select[@name='*#*']//option[*##*]";
		protected const string DROPDOWN_TERM_FIELD = "//td[contains(@class,'js-details-panel')]//div[@class= 'l-corpr__viewmode js-term-attrs']//select[@name='*#*']/..//span[contains(@class,'js-dropdown')]";
		protected const string OPTION_IN_TERM_FIELD = "//span[contains(@class,'js-dropdown__list')]//span[@data-id='*#*']";

		protected const string FILTER_BUTTON = "//span[contains(@class, 'js-set-filter')]";
		//protected const string LANGUAGE_COLUMNS = "//th[@class='l-corpr__th']//a[@class='g-block l-corpr__thsort' and contains(@href,'orderBy=Language')]"; // колонки языков в таблице на стр одного глоссари
		protected const string CREATED_BY_FILTER_LABEL = "//div[contains(@title, 'Created:')]";
		protected const string MODIFIED_BY_FILTER_LABEL = "//div[contains(@title, 'Modified:')]";
		protected const string ALL_TERMS = "//tr[@class='l-corpr__trhover js-concept-row']//td[contains(@class, 'g-bold')]//p";
		protected const string CREATED_CLEAR_FILTER_PANEL = "//div[contains(@title, 'Created: *#*')]";
		protected const string MODIFIED_CLEAR_FILTER_PANEL = "//div[contains(@title, 'Modified: *#*')]";
		protected const string DELETE_CREATED_FILTER_BUTTON = "//div[contains(@title, 'Created: *#*')]//img";
		protected const string DELETE_MODIFIED_FILTER_BUTTON = "//div[contains(@title, 'Modified: *#*')]//img";
		protected const string COMMON_CLEAR_FILTER_SECTION = "//div[@class='l-corpr__filter g-bold']";
		protected const string DIAPASON_PANEL = "//div[@class='l-corpr__filterSection inline']";

		#endregion
	}
}
