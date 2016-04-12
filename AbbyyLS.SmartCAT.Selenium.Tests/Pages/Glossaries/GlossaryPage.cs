using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

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

		public new GlossaryPage LoadPage()
		{
			if (!IsGlossaryPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница глоссария");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Раскрыть меню редактирования термина.
		/// </summary>
		/// <param name="term">термин</param>
		public GlossaryPage UnrollEditMenuForTerms(string term)
		{
			CustomTestContext.WriteLine("Раскрыть меню редактирования термина.");
			TableWithTerms = Driver.SetDynamicValue(How.XPath, TABLE_WITH_TERMS, term);
			TableWithTerms.Click();

			return LoadPage();
		}

		/// <summary>
		/// Ввести комментарий в выпадающем меню редактирования терминов.
		/// </summary>
		/// <param name="comment">еомментарий</param>
		public GlossaryPage FillCommentInDropDownEditMenu(string comment)
		{
			CustomTestContext.WriteLine("Ввести комментарий в выпадающем меню редактирования терминов.");
			CommentField.SetText(comment);

			return LoadPage();
		}

		/// <summary>
		/// Получить текст из термина типа дропдаун в режиме просмотра
		/// </summary>
		public string GetDropdownTermFieldViewModelText(GlossarySystemField termField)
		{
			CustomTestContext.WriteLine("Получить текст из термина типа дропдаун в режиме просмотра.", termField);

			return Driver.SetDynamicValue(How.XPath, DROPDOWN_TERM_FIELD_VIEW_MODE, termField.ToString()).Text.ToLower();
		}

		/// <summary>
		/// Раскрыть поле для добавления термина в секции 'Language and term details'.
		/// </summary>
		/// <param name="rowNumber">номер поля для ввода</param>
		public GlossaryPage GetDropdownAddTermInLanguagesAndTerms(int rowNumber)
		{
			CustomTestContext.WriteLine("Раскрыть поле для добавления термина в секции 'Language and term details'.");
			Driver.WaitUntilElementIsDisplay(By.XPath(ADD_BUTTON.Replace("*#*", rowNumber.ToString())));
			var addTermButton = Driver.SetDynamicValue(How.XPath, ADD_BUTTON, rowNumber.ToString());
			addTermButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку Export
		/// </summary>
		public GlossaryPage ClickExportGlossary()
		{
			CustomTestContext.WriteLine("Нажать кнопку Export.");
			ExportButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'New Entry'
		/// </summary>
		public GlossaryPage ClickNewEntryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'New Entry'.");
			NewEntryButton.Click();

			return LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Нажать на галочку для сохранения термина
		/// </summary>
		public GlossaryPage ClickSaveTermButton()
		{
			CustomTestContext.WriteLine("Нажать на галочку для сохранения термина.");
			TermSaveButton.Click();

			Driver.WaitUntilElementIsDisappeared(By.XPath(TERM_SAVE_BUTTON));

			return LoadPage();
		}

		/// <summary>
		/// Нажать на галочку для сохраенния термина
		/// </summary>
		public TermAlreadyExistsDialog ClickSaveTermButtonExpectingTermAlreadyExistsDialog()
		{
			CustomTestContext.WriteLine("Нажать на галочку для сохраенния термина.");
			TermSaveButton.Click();

			Driver.WaitUntilElementIsDisappeared(By.XPath(TERM_SAVE_BUTTON));

			return new TermAlreadyExistsDialog(Driver).LoadPage();
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
				if (!Driver.WaitUntilElementIsDisappeared(By.XPath(SAVE_ENTRY_BUTTON), 3))
				{
					SaveEntryButton.HoverElement();
					SaveEntryButton.JavaScriptClick();
				}

				i++;
			}

			Driver.WaitUntilElementIsDisappeared(By.XPath(SAVE_ENTRY_BUTTON));

			return LoadPage();
		}

		/// <summary>
		/// Раскрыть меню редактирования глоссария
		/// </summary>
		public GlossaryPage ExpandEditGlossaryMenu()
		{
			CustomTestContext.WriteLine("Раскрыть меню редактирования глоссария.");
			EditGlossaryMenu.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать 'Glossary properties' в меню редактирования глоссария
		/// </summary>
		public GlossaryPropertiesDialog ClickGlossaryProperties()
		{
			CustomTestContext.WriteLine("Выбрать 'Glossary properties' в меню редактирования глоссария");
			GlossaryProperties.Click();

			return new GlossaryPropertiesDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Выбрать 'Glossary structure' в меню редактирования глоссария
		/// </summary>
		public GlossaryStructureDialog ClickGlossaryStructure()
		{
			CustomTestContext.WriteLine("Выбрать 'GlossaryStructure' в меню редактирования глоссария.");
			GlossaryStructure.Click();

			return new GlossaryStructureDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Import
		/// </summary>
		public GlossaryImportDialog ClickImportButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Import.");
			ImportButton.Click();

			return new GlossaryImportDialog(Driver).LoadPage();
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
		/// Нажать кнопку сортировки по английским терминам, ожидая алерт
		/// </summary>
		public void ClickSortByEnglishTermAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по английским терминам, ожидая алерт.");

			SortByEnglishTerm.Click();
		}

		/// <summary>
		/// Открыть подробную информацию по языку или термину в режиме редактирования
		/// </summary>
		public GlossaryPage OpenLanguageAndTermDetailsEditMode()
		{
			CustomTestContext.WriteLine("Открыть подробную информацию по языку или термину в режиме редактирования.");
			LanguageHeaderEditMode.Click();

			return LoadPage();
		}

		/// <summary>
		/// Открыть подробную информацию по языку или термину в режиме просмотра
		/// </summary>
		public GlossaryPage OpenLanguageDetailsViewMode(Language language = Language.English)
		{
			CustomTestContext.WriteLine("Открыть подробную информацию по языку или термину в режиме просмотра.");
			Driver.SetDynamicValue(How.XPath, LANGUAGE_HEADER_VIEW_MODE, language.ToString()).Click();
			
			return LoadPage();
		}

		/// <summary>
		/// Ввести текст в поле комментария в секции 'Language and term details'
		/// </summary>
		public GlossaryPage FillLanguageComment(string comment)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле комментария в секции 'Language and term details'.", comment);
			LanguageCommentEditMode.SetText(comment);

			return LoadPage();
		}

		/// <summary>
		/// Ввести текст в поле Definition в секции 'Language and term details'
		/// </summary>
		public GlossaryPage FillDefinition(string text)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле Definition в секции 'Language and term details'.", text);
			DefinitionEditMode.SetText(text);

			return LoadPage();
		}

		/// <summary>
		/// Ввести текст в поле 'Definition source' в секции 'Language and term details'
		/// </summary>
		public GlossaryPage FillDefinitionSource(string text)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле 'Definition source' в секции 'Language and term details'.", text);
			DefinitionSourceEditMode.SetText(text);

			return LoadPage();
		}

		/// <summary>
		/// Получить значение опции в дропдауне поля
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <param name="optionNumber">номер опции</param>
		public string GetOptionValue(GlossarySystemField fieldName, int optionNumber = 2)
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

			return LoadPage();
		}

		/// <summary>
		/// Выбрать опцию
		/// </summary>
		public GlossaryPage SelectOptionDropdownTermField(string option)
		{
			CustomTestContext.WriteLine("Выбрать опцию {0}.", option);
			Driver.SetDynamicValue(How.XPath, OPTION_IN_TERM_FIELD, option).Click();

			return LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Ввести текст в термин
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		public GlossaryPage EditTermInLanguagesAndTermsSection(string text, int termNumber)
		{
			CustomTestContext.WriteLine("Ввести текст {0} в термин №{1}.", text, termNumber);
			Driver.SetDynamicValue(How.XPath, TERM_INPUT, termNumber.ToString()).SetText(text);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по русским терминам, ожидая алерт.
		/// </summary>
		public void ClickSortByRussianTermAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по русским терминам, ожидая алерт.");

			SortByRussianTerm.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате изменения, ожидая алерт.
		/// </summary>
		public void ClickSortByDateModifiedAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по дате изменения, ожидая алерт.");

			SortByDateModified.Click();
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

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку закрытия раскрытых терминов
		/// </summary>
		public GlossaryPage CloseExpandedTerms()
		{
			CustomTestContext.WriteLine("Нажать кнопку закрытия раскрытых терминов.");
			CloseExpandTermsButton.Scroll();
			CloseExpandTermsButton.Click();

			return LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Ввести текст в поле синонима
		/// </summary>
		public GlossaryPage FillSynonym(string text, int columnNumber)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле синонима в столбце №{1}.", text, columnNumber);
			var synonymInput = Driver.SetDynamicValue(How.XPath, SYNONYM_INPUT, columnNumber.ToString());
			synonymInput.SetText(text);

			return LoadPage();
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
			DeleteTermButton = Driver.SetDynamicValue(How.XPath, DELETE_BUTTON, source, target);
			DeleteTermButton.Click();

			return LoadPage();
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
		/// Нажать на термин в колонке 'Languages and terms'
		/// </summary>
		public GlossaryPage ClickTermInLanguagesAndTermsColumn(int termNumber = 2)
		{
			CustomTestContext.WriteLine("Нажать на термин №{0} в колонке 'Languages and terms'.", termNumber);
			Driver.SetDynamicValue(How.XPath, TERM_IN_LANGUAGES_AND_TERMS_COLUMN, termNumber.ToString()).Click();

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле
		/// </summary>
		public GlossaryPage FillTermField(GlossarySystemField termField, string text)
		{
			CustomTestContext.WriteLine("Заполнить поле {0}.", termField);

			Driver.SetDynamicValue(How.XPath, TERM_FIELD_EDIT_MODE, termField.ToString()).SetText(text);

			return LoadPage();
		}

		/// <summary>
		/// Выбрать значение в поле типа List
		/// </summary>
		public GlossaryPage ClickItemInListDropdown(string item)
		{
			CustomTestContext.WriteLine("Выбрать значение {0} в комбобоксе типа List.", item);
			Driver.SetDynamicValue(How.XPath, ITEMS_LIST, item).Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать значение в поле типа 'Multi-selection list'
		/// </summary>
		public GlossaryPage ClickItemInMultiselectListDropdown(string item)
		{
			CustomTestContext.WriteLine("Выбрать значение {0} в комбобоксе 'Multi-selection list'.", item);
			MultiselectList = Driver.SetDynamicValue(How.XPath, MULTISELECT_LIST, item);
			MultiselectList.Click();

			return LoadPage();
		}

		/// <summary>
		/// Раскрыть дропаун Topic 
		/// </summary>
		public GlossaryPage ExpandTopicDropdown()
		{
			CustomTestContext.WriteLine("Раскрыть дропдаун Topic.");
			TopicField.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать значение в дропдауне Topic
		/// </summary>
		public GlossaryPage ClickOptionInTopicDropdown(string option)
		{
			CustomTestContext.WriteLine("Выбрать значение {0} в дропдауне Topic.", option);
			Driver.SetDynamicValue(How.XPath, TOPIC_OPTION, option.Trim()).Click();

			return LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Раскрыть комбобокс типа List
		/// </summary>
		public GlossaryPage ExpandItemsListDropdown(string fieldName)
		{
			CustomTestContext.WriteLine("Раскрыть комбобокс {0} типа List.", fieldName);
			Driver.SetDynamicValue(How.XPath, ITEMS_LIST_DROPDOWN, fieldName).Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на комбобокс типа 'Multi-selection list'
		/// </summary>
		public GlossaryPage ClickMultiselectListDropdown(string fieldName)
		{
			CustomTestContext.WriteLine("Нажать на комбобокс {0} типа 'Multi-selection list'.", fieldName);
			Driver.SetDynamicValue(How.XPath, MULTISELECT_DROPDOWN, fieldName).Click();
			Driver.WaitUntilElementIsAppear(By.XPath(OPTION_LIST));

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по чекбоксу Yes/No
		/// </summary>
		public GlossaryPage ClickYesNoCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть по чекбоксу Yes/No.");
			YesNoCheckbox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле
		/// </summary>
		public GlossaryPage FillField(string fieldName, string text)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле {1}.", text, fieldName);
			var customField = Driver.SetDynamicValue(How.XPath, CUSTOM_FIELD_INPUT, fieldName);
			customField.SetText(text);

			return LoadPage();
		}

		/// <summary>
		/// Ввести в поле типа Number.
		/// </summary>
		public GlossaryPage FillNumberCustomField(string fieldName, string text)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле {1} типа Number.", text, fieldName);
			var customNumberField = Driver.SetDynamicValue(How.XPath, CUSTOM_NUMBER_FIELD, fieldName);
			customNumberField.SetText(text);

			return LoadPage();
		}

		/// <summary>
		/// Открыть календарь
		/// </summary>
		public GlossaryPage OpenCalendar(string fieldName)
		{
			CustomTestContext.WriteLine("Открыть календарь в поле {0}.", fieldName);
			Driver.SetDynamicValue(How.XPath, CUSTOM_DATE_FIELD, fieldName).Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать текущую дату в календаре
		/// </summary>
		public GlossaryPage ClickTodayInCalendar()
		{
			CustomTestContext.WriteLine("Выбрать текущую дату в календаре.");
			TodayDate.Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить текст из термина.
		/// </summary>
		public string GetTermText(int termNumber = 1)
		{
			CustomTestContext.WriteLine("Получить текст из {0} термина.", termNumber);
			Term = Driver.SetDynamicValue(How.XPath, TERM, termNumber.ToString());

			return Term.Text.Trim();
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

			return LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле поиск
		/// </summary>
		public GlossaryPage FillSearchField(string text)
		{
			CustomTestContext.WriteLine(string.Format("Ввести {0} в поле поиск.", text));
			SearchInput.SetText(text);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку поиска 
		/// </summary>
		public GlossaryPage ClickSearchButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку поиска.");
			SearchButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Edit Entry'
		/// </summary>
		public GlossaryPage ClickEditEntryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Edit Entry'.");
			EditEntryButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel
		/// </summary>
		public GlossaryPage ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel.");
			CancelButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на поле Image
		/// </summary>
		public GlossaryPage ClickImageField(string fieldName)
		{
			CustomTestContext.WriteLine("Нажать на поле Image.");
			Driver.SetDynamicValue(How.XPath, IMAGE_FIELD, fieldName).Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку Add в поле типа Media
		/// </summary>
		public GlossaryPage ClickAddMediaButton(string fieldName)
		{
			CustomTestContext.WriteLine("Нажать на кнопку Add в поле {0} типа Media.", fieldName);
			Driver.SetDynamicValue(How.XPath, ADD_MEDIA_BUTTON, fieldName).Click();

			return LoadPage();
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

			return new FilterDialog(Driver).LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Удалить фильтр Modified
		/// </summary>
		public GlossaryPage ClickDeleteModifiedFilter(string filterValue)
		{
			CustomTestContext.WriteLine("Удалить фильтр Modified со значением {0}.", filterValue);
			Driver.SetDynamicValue(How.XPath, DELETE_MODIFIED_FILTER_BUTTON, filterValue).Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку удаления всех фильтров
		/// </summary>
		public GlossaryPage ClickClearAllFilters()
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления всех фильтров");
			ClearAllFiltersButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить текст из фильтра 'Modified'
		/// </summary>
		public string GetModifiedByFilterText()
		{
			CustomTestContext.WriteLine("Получить текст из фильтра 'Modified'.");

			return ModifiedByFilterLabel.Text;
		}

		/// <summary>
		/// Ввести путь к медиа файлу в поле импорта
		/// </summary>
		/// <param name="filepath">путь до файла</param>
		public GlossaryPage SetMediaFileName(string filepath)
		{
			CustomTestContext.WriteLine("Ввести путь к файлу {0} в поле импорта.", filepath);
			AddMediaInput.SendKeys(filepath);

			return LoadPage();
		}

		/// <summary>
		/// Ввести путь к файлу изображения в поле импорта (с мультимедиа)
		/// </summary>
		/// <param name="filepath">путь до файла</param>
		public GlossaryPage SetImageWithMultimediaFileName(string filepath)
		{
			CustomTestContext.WriteLine("Ввести путь к файлу изображения {0} в поле импорта(с мультимедиа).", filepath);
			AddImageInputWithMultimedia.SendKeys(filepath);

			return LoadPage();
		}

		/// <summary>
		/// Ввести путь к файлу изображения в поле импорта
		/// </summary>
		/// <param name="filepath">путь до файла</param>
		public GlossaryPage SetImageFileName(string filepath)
		{
			CustomTestContext.WriteLine("Ввести путь к файлу изображения {0} в поле импорта.", filepath);
			AddImageInput.SendKeys(filepath);

			return LoadPage();
		}

		/// <summary>
		/// Навести курсор мыши на ссылку добавления медиа файла
		/// </summary>
		public GlossaryPage HoverMultimediaLink()
		{
			CustomTestContext.WriteLine("Навести курсор мыши на ссылку добавления медиа файла");
			AddMultimediaLink.HoverElement();

			return LoadPage();
		}

		/// <summary>
		/// Навести курсор мыши на ссылку добавления изображения
		/// </summary>
		public GlossaryPage HoverImageWithMultimediaLink()
		{
			CustomTestContext.WriteLine("Навести курсор мыши на ссылку добавления изображения");
			AddImageLink.HoverElement();

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать значение в поле типа List
		/// </summary>
		/// <param name="fieldName">имя поля</param>
		/// <param name="item">значение</param>
		public GlossaryPage SelectItemInListDropdown(string fieldName, string item)
		{
			ExpandItemsListDropdown(fieldName);
			ClickItemInListDropdown(item);

			return LoadPage();
		}

		/// <summary>
		/// Выбрать значение в поле типа 'Multi-selection list'
		/// </summary>
		/// <param name="fieldName">имя поля</param>
		/// <param name="items">значение</param>
		public GlossaryPage SelectItemInMultiSelectListDropdown(string fieldName, List<string> items)
		{
			ClickMultiselectListDropdown(fieldName);
			for (int i = 0; i < items.Count; i++)
			{
				ClickItemInMultiselectListDropdown(items[i]);
			}
			ClickMultiselectListDropdown(fieldName);

			return LoadPage();
		}


		/// <summary>
		/// Открыть структуру глоссария
		/// </summary>
		public GlossaryStructureDialog OpenGlossaryStructure()
		{
			ExpandEditGlossaryMenu();
			var glossaryStructureDialog = ClickGlossaryStructure();

			return glossaryStructureDialog.LoadPage();
		}

		/// <summary>
		/// Открыть настройки глоссария
		/// </summary>
		public GlossaryPropertiesDialog OpenGlossaryProperties()
		{
			ExpandEditGlossaryMenu();
			var glossaryPropertiesDialog = ClickGlossaryProperties();

			return glossaryPropertiesDialog.LoadPage();
		}

		/// <summary>
		/// Создать термин
		/// </summary>
		/// <param name="firstTerm">первый термин</param>
		/// <param name="secondTerm">второй термин</param>
		public GlossaryPage CreateTerm(string firstTerm = "firstTerm", string secondTerm = "secondTerm")
		{
			ClickNewEntryButton();
			FillAllTerm(firstTerm, secondTerm);
			ClickSaveTermButton();

			return LoadPage();
		}

		/// <summary>
		/// Создать несколько терминов
		/// </summary>
		/// <param name="terms">термины</param>
		public GlossaryPage CreateTerms(Dictionary<string, string> terms)
		{
			CustomTestContext.WriteLine("Создать несколько терминов.");
			foreach (var term in terms)
			{
				ClickNewEntryButton();
				FillAllTerm(term.Key, term.Value);
				ClickSaveTermButton();
			}
			
			return LoadPage();
		}

		/// <summary>
		/// Заполнить термин для всех языков
		/// </summary>
		/// <param name="firstTerm">первый термин</param>
		/// <param name="secondTerm">второй термин</param>
		public GlossaryPage FillAllTerm(string firstTerm = "firstTerm", string secondTerm = "secondTerm")
		{
			FillTerm(1, firstTerm);
			FillTerm(2, secondTerm);

			return LoadPage();
		}

		/// <summary>
		/// Редактировать custom термины
		/// </summary>
		/// <param name="text">текст</param>
		public GlossaryPage EditCustomTerms(string text)
		{
			ClickEditEntryButton();

			var termsCount = TermsCountInLanguagesAndTermsSection();

			for (int i = 1; i <= termsCount; i++)
			{
				ClickTermInLanguagesAndTermsSection(i);
				EditTermInLanguagesAndTermsSection(text, i);
			}

			ClickSaveEntryButton();

			return LoadPage();
		}

		/// <summary>
		/// Добавить синоним
		/// </summary>
		/// <param name="columnNumber">номер колонки</param>
		/// <param name="text">текст</param>
		public GlossaryPage AddSynonym(int columnNumber, string text)
		{
			ClickSynonymPlusButton(columnNumber);
			FillSynonym(text, columnNumber);

			return LoadPage();
		}

		/// <summary>
		/// Добавить определение
		/// </summary>
		/// <param name="definition">определение</param>
		public GlossaryPage AddDefinition(string definition)
		{
			OpenLanguageAndTermDetailsEditMode();
			FillDefinition(definition);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить дату
		/// </summary>
		/// <param name="fieldName">имя поля</param>
		public GlossaryPage FillDateField(string fieldName)
		{
			OpenCalendar(fieldName);
			ClickTodayInCalendar();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать опцию в топике
		/// </summary>
		/// <param name="option">опция</param>
		public GlossaryPage SelectOptionInTopic(string option)
		{
			ExpandTopicDropdown();
			ClickOptionInTopicDropdown(option);

			return LoadPage();
		}

		/// <summary>
		/// Выбрать опцию
		/// </summary>
		/// <param name="fieldName">имя системного поля</param>
		/// <param name="option">опция</param>
		public GlossaryPage SpecifyDropdownTermField(GlossarySystemField fieldName, string option)
		{
			ExpandDropdownTermField(fieldName);
			SelectOptionDropdownTermField(option);

			return LoadPage();
		}

		/// <summary>
		/// Удалить термин
		/// </summary>
		/// <param name="source">исходное слово</param>
		/// <param name="target">перевод</param>
		public GlossaryPage DeleteTerm(string source, string target)
		{
			HoverTermRow(source, target);
			ClickDeleteButton(source, target);

			return LoadPage();
		}

		/// <summary>
		/// Ввести текст в поле 'Definition source' в секции 'Language and term details'
		/// </summary>
		/// <param name="definitionSource">текст</param>
		public GlossaryPage AddDefinitionSource(string definitionSource)
		{
			OpenLanguageAndTermDetailsEditMode();
			FillDefinitionSource(definitionSource);

			return LoadPage();
		}

		/// <summary>
		/// Ввести текст в поле комментария в секции 'Language and term details'
		/// </summary>
		/// <param name="comment">комментарий</param>
		public GlossaryPage AddLanguageComment(string comment)
		{
			OpenLanguageAndTermDetailsEditMode();
			FillLanguageComment(comment);

			return LoadPage();
		}

		/// <summary>
		/// Поиск термина
		/// </summary>
		/// <param name="text">текст</param>
		public GlossaryPage SearchTerm(string text)
		{
			FillSearchField(text);
			ClickSearchButton();

			return LoadPage();
		}

		/// <summary>
		/// Редактировать термин по умолчанию
		/// </summary>
		/// <param name="source">исходное слово</param>
		/// <param name="target">перевод</param>
		/// <param name="text">добавочное слово</param>
		public GlossaryPage EditDefaultTerm(string source, string target, string text)
		{
			HoverTermRow(source, target);
			ClickEditButton();
			FillAllTerm(source + text, target + text);
			ClickSaveTermButton();

			return LoadPage();
		}

		/// <summary>
		/// Удалить термин из глоссария
		/// </summary>
		/// <param name="source">термин</param>
		public GlossaryPage DeleteTerm(string source)
		{
			CustomTestContext.WriteLine("Удалить термин {0} из глоссария", source);
			DeleteTermButton = Driver.SetDynamicValue(How.XPath, DELETE_TERM_BUTTON, source);
			TermRow = Driver.SetDynamicValue(How.XPath, SOURCE_TERM, source);
			TermRow.HoverElement();
			DeleteTermButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Добавить термин и ввести текст в поле термина в секции 'Languages and terms'
		/// </summary>
		/// <param name="text">текст</param>
		public GlossaryPage FillTermInLanguagesAndTermsSection(string text = "Term Example")
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

			return LoadPage();
		}

		/// <summary>
		/// Загрузка файла картинка.
		/// </summary>
		/// <param name="filePath">путь к файлу</param>
		public GlossaryPage UploadImageFile(string filePath)
		{
			HoverImageWithMultimediaLink();
			makeImageInputVisible();
			SetImageFileName(filePath);
			Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_IMAGE_BUTTON));

			return LoadPage();
		}

		/// <summary>
		/// Загрузка файла картинка и мультимедиа.
		/// </summary>
		/// <param name="filePath">путь к файлу</param>
		public GlossaryPage UploadImageFileWithMultimedia(string filePath)
		{
			HoverImageWithMultimediaLink();
			makeImageWithMultimediaInputVisible();
			SetImageWithMultimediaFileName(filePath);
			Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_IMAGE_BUTTON));

			return LoadPage();
		}

		/// <summary>
		/// Загрузка файла мультимедиа.
		/// </summary>
		/// <param name="filePath">путь к файлу</param>
		public GlossaryPage UploadMediaFile(string filePath)
		{
			HoverMultimediaLink();
			makeMultimediaInputVisible();
			SetMediaFileName(filePath);
			Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_IMAGE_BUTTON));

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница глоссария
		/// </summary>
		public bool IsGlossaryPageOpened()
		{
			return IsDialogBackgroundDisappeared() &&
			       Driver.WaitUntilElementIsDisplay(By.XPath(CONCEPTS_TABLE), timeout: 30);
		}

		/// <summary>
		/// Проверить, загрузился ли файл
		/// </summary>
		/// <param name="pathToFile">путь до файла</param>
		public bool IsGlossaryExportedSuccesfully(string pathToFile)
		{
			var timeout = 0;

			while (!File.Exists(pathToFile) && (timeout < 10))
			{
				timeout++;
				Thread.Sleep(1000);
			}

			return File.Exists(pathToFile);
		}

		/// <summary>
		/// Проверить, что активирован режим создания нового термина
		/// </summary>
		public bool IsCreationModeActivated()
		{
			CustomTestContext.WriteLine("Проверить, что активирован режим создания нового термина");

			return ExtendMode.Displayed;
		}

		/// <summary>
		/// Проверить, что глоссарий содержит необходимое количество терминов
		/// </summary>
		/// <param name="expectedTermsCount">ожидаемое кол-во терминов</param>
		public bool IsGlossaryContainsCorrectTermsCount(int expectedTermsCount)
		{
			CustomTestContext.WriteLine("Проверить, что глоссарий содержит {0} терминов.", expectedTermsCount);
			var actualTermsCount = Driver.GetElementsCount(By.XPath(TERM_ROW));

			return actualTermsCount == expectedTermsCount;
		}

		/// <summary>
		/// Проверить, что комментарий содержит текст.
		/// </summary>
		/// <param name="comment">тест комментария</param>
		public bool IsCommentFilled(string comment)
		{
			CustomTestContext.WriteLine("Проверить, что комментарий содержит текст {0}.", comment);
			
			return comment == LanguageCommentViewMode.Text;
		}

		/// <summary>
		/// Проверить, что поле Definition содержит текст.
		/// </summary>
		/// <param name="text">текст поля Definition</param>
		public bool IsDefinitionFilled(string text)
		{
			CustomTestContext.WriteLine("Проверить, что поле Definition содержит текст {0}", text);

			return text == DefinitionViewMode.Text;
		}

		/// <summary>
		/// Проверить, что поле 'Definition source' содержит текст.
		/// </summary>
		/// <param name="text">текст поля Definition source</param>
		public bool IsDefinitionSourceFilled(string text)
		{
			CustomTestContext.WriteLine("Проверить, что поле 'Definition source' содержит текст {0}.", text);

			return text == DefinitionSourceViewMode.Text;
		}

		/// <summary>
		/// Проверить, что термины подсвечены красным цветом, так как термины должны быть уникальны
		/// </summary>
		/// <param name="columnNumber">номер столбца</param>
		public bool IsSynonumUniqueErrorDisplayed(int columnNumber)
		{
			CustomTestContext.WriteLine("Проверить, что термины подсвечены красным цветом в стоблце №{0}, так как термины должны быть уникальны.", columnNumber);

			return Driver.WaitUntilElementIsDisplay(By.XPath(SYNONYM_UNIQUE_ERROR.Replace("*#*", columnNumber.ToString())));
		}

		/// <summary>
		/// Проверить наличие термина
		/// </summary>
		/// <param name="text">текст</param>
		public bool IsSingleTermExists(string text)
		{
			CustomTestContext.WriteLine("Проверить наличие термина {0}", text);

			return Driver.WaitUntilElementIsDisplay(By.XPath(SOURCE_TERM.Replace("*#*", text)));
		}

		/// <summary>
		/// Получить, есть ли термин с сорсом и таргетом
		/// </summary>
		/// <param name="sourceText">сорс</param>
		/// <param name="targetText">таргет</param>
		public bool IsSingleTermWithTranslationExists(string sourceText, string targetText)
		{
			CustomTestContext.WriteLine("Получить, есть ли термин с сорсом {0} и таргетом {1}", sourceText, targetText);

			return Driver.WaitUntilElementIsDisplay(By.XPath(SOURCE_TARGET_TERM.Replace("#", sourceText).Replace("**", targetText)));
		}

		/// <summary>
		/// Получить, есть ли термин с сорсом, таргетом и комментом
		/// </summary>
		/// <param name="sourceText">сорс</param>
		/// <param name="targetText">таргет</param>
		/// <param name="comment">комментарий</param>
		public bool IsTermWithTranslationAndCommentExists(string sourceText, string targetText, string comment)
		{
			CustomTestContext.WriteLine("Получить, есть ли термин с сорсом '{0}', таргетом '{1}' и комментом: '{2}'", sourceText, targetText, comment);

			return Driver.WaitUntilElementIsDisplay(By.XPath(SOURCE_TARGET_TERM_WITH_COMMENT.Replace("#", sourceText).Replace("**", targetText).Replace("$$", comment)));
		}

		/// <summary>
		/// Проверить, что значение в поле совпадает с ожидаемым значением
		/// </summary>
		/// <param name="fieldName">имя поля</param>
		/// <param name="text">ожидаемое значение</param>
		public bool IsFieldValueMatchExpected(GlossarySystemField fieldName, string text)
		{
			CustomTestContext.WriteLine("Проверить, что значение в поле {0} совпадает с ожидаемым значением {1}.", fieldName.Description(), text);
			var customField = Driver.SetDynamicValue(How.XPath, CUSTOM_FIELD_VIEW_MODE, fieldName.Description());

			return text == customField.Text;
		}

		/// <summary>
		/// Проверить, что значение в поле совпадает с ожидаемым значением
		/// </summary>
		/// <param name="fieldName">имя поля</param>
		/// <param name="text">ожидаемое значение</param>
		public bool IsCustomFieldValueMatchExpected(string fieldName, string text)
		{
			CustomTestContext.WriteLine("Проверить, что значение в кастомном поле {0} совпадает с ожидаемым значением {1}.", fieldName, text);
			var customField = Driver.SetDynamicValue(How.XPath, CUSTOM_FIELD_VIEW_MODE, fieldName);

			return text == customField.Text;
		}

		/// <summary>
		/// Проверить, что поле присутствует в новом термине
		/// </summary>
		/// <param name="fieldName">имя поля</param>
		public bool IsFieldExistInNewEntry(string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что поле {0} присутствует в новом термине", fieldName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(CUSTOM_FIELD_NAME.Replace("*#*", fieldName)));
		}

		/// <summary>
		/// Проверить, что термин содержит правильные синонимы
		/// </summary>
		/// <param name="columnNumber">номер колонки</param>
		/// <param name="synonyms">синонимы</param>
		public bool IsTermContainsExpectedSynonyms(int columnNumber, List<string> synonyms)
		{
			CustomTestContext.WriteLine("Проверить, что термин содержит правильные синонимы");
			var synonumsList = Driver.GetTextListElement(By.XPath(TERM_TEXT.Replace("*#*", columnNumber.ToString())));

			return synonyms.SequenceEqual(synonumsList);
		}

		/// <summary>
		/// Проверить, что поле подсвечено красным цветом, так как обязательно для заполнения
		/// </summary>
		/// <param name="fieldName">имя поля</param>
		public bool IsFieldErrorDisplayed(string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что поле {0} подсвечено красным цветом, так как обязательно для заполнения.", fieldName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(CUSTOM_FIELD_ERROR.Replace("*#*", fieldName)));
		}

		/// <summary>
		/// Проверить, что поле типа Image подсвечено красным цветом, так как обязательно для заполнения
		/// </summary>
		/// <param name="fieldName">имя поля</param>
		public bool IsImageFieldErrorDisplayed(string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что поле {0} типа Image подсвечено красным цветом, так как обязательно для заполнения.", fieldName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(CUSTOM_IMAGE_FIELD_ERROR.Replace("*#*", fieldName)));
		}

		/// <summary>
		/// Проверить, что поле Image заполнено
		/// </summary>
		/// <param name="fieldName">имя поля</param>
		public bool IsImageFieldFilled(string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что поле {0} типа Image заполнено", fieldName);
			var srcAttribute = Driver.SetDynamicValue(How.XPath, FILLED_IMAGE_FIELD, fieldName).GetAttribute("src");

			if (srcAttribute != null)
			{
				return srcAttribute.Trim().Length > 0;
			}
			
			return false;
		}

		/// <summary>
		/// Проверить, что значение чекбокса совпадает с ожидаемым
		/// </summary>
		/// <param name="yesNo">значение чекбокса</param>
		/// <param name="fieldName">имя поля</param>
		public bool IsYesNoCheckboxChecked(string yesNo, string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что в поле {0} типа значение чекбокса совпадает с ожидаемым {1}.", fieldName, yesNo);

			return yesNo == Driver.SetDynamicValue(How.XPath, YES_NO_CHECKBOX_VIEW_MODE, fieldName).Text;
		}

		/// <summary>
		/// Проверить, что в поле типа Media правильное название файла
		/// </summary>
		/// <param name="mediaFile">название файла</param>
		/// <param name="fieldName">имя поля</param>
		public bool IsMediaFileMatchExpected(string mediaFile, string fieldName)
		{
			CustomTestContext.WriteLine("Проверить, что в поле {0} типа Media правльное название файла {1}.", fieldName, mediaFile);

			return mediaFile == Driver.SetDynamicValue(How.XPath, MEDIA_FIELD_TEXT,fieldName).Text;
		}
		
		/// <summary>
		/// Проверить, что все термины совпадают с ожидаемым
		/// </summary>
		/// <param name="text">ожидаемый термин</param>
		public bool IsTermsTextMatchExpected(string text)
		{
			CustomTestContext.WriteLine("Проверить, что все термины совпадают с {0}.", text);

			return TermsList().All(term => term.Trim() == text);
		}

		/// <summary>
		/// Проверить, что количество терминов совпадает с ожидаемым
		/// </summary>
		/// <param name="expectedTermCount">ожидаемое кол-во терминов</param>
		public bool IsDefaultTermsCountMatchExpected(int expectedTermCount)
		{
			CustomTestContext.WriteLine("Проверить, что количество терминов равно {0}.", expectedTermCount);
			RefreshPage<GlossaryPage>();

			return expectedTermCount == Driver.GetElementsCount(By.XPath(DEFAULT_TERM_ROWS));
		}

		/// <summary>
		/// Проверить, верен ли диапазон дат
		/// </summary>
		/// <param name="dateTimeList">диапазон дат</param>
		public bool IsDateRangeInFilterMatch(List<DateTime> dateTimeList)
		{
			CustomTestContext.WriteLine("Проверить, верен ли диапазон дат {0}.", dateTimeList);
			var panelText = DiapasonPanel.GetAttribute("title").Split(' ');
			var startDate = DateTime.ParseExact(panelText[2], "M/d/yyyy", CultureInfo.InvariantCulture);
			var endDate = DateTime.ParseExact(panelText[4], "M/d/yyyy", CultureInfo.InvariantCulture);
			var rangeList = new List<DateTime> { startDate, endDate };

			return dateTimeList.SequenceEqual(rangeList);
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
		/// <param name="term">термин</param>
		public bool IsTermDisplayedInLanguagesAndTermsSection(string term)
		{
			CustomTestContext.WriteLine("Проверить, что термин {0} присутствует в секции 'Languages and terms '.", term);

			return Driver.SetDynamicValue(How.XPath, TERMS_IN_LANGUAGE_AND_TERMS_SECTION, term).Displayed;
		}

		/// <summary>
		/// Проверить, отображается ли термин в режиме редактирования
		/// </summary>
		/// <param name="termField">поле с термином</param>
		public bool IsTermFieldEditModeDisplayed(GlossarySystemField termField)
		{
			CustomTestContext.WriteLine("Проверить, отображается ли термин {0} в режиме редактирования.", termField);

			return Driver.GetIsElementExist(By.XPath(TERM_FIELD_EDIT_MODE.Replace("*#*", termField.ToString())));
		}

		/// <summary>
		/// Проверить, отображается ли термин типа дропдаун в режиме редактирования
		/// </summary>
		/// <param name="termField">поле с термином</param>
		public bool IsDropdownTermFieldEditModeDisplayed(GlossarySystemField termField)
		{
			CustomTestContext.WriteLine("Проверить, отображается ли термин {0} типа дропдаун в режиме редактирования.", termField);

			return Driver.GetIsElementExist(By.XPath(DROPDOWN_TERM_FIELD_EDIT_MODE.Replace("*#*", termField.ToString())));
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
		/// Проверить, есть ли что-то на панели фильтров
		/// </summary>
		public bool IsAnyFilterDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, есть ли что-то на панели фильтров");

			return Driver.WaitUntilElementIsDisplay(By.XPath(CLEAR_ALL_FILTERS_BUTTON));
		}

		/// <summary>
		/// Проверить, что фильтр Modified отображается в шапке таблицы
		/// </summary>
		/// <param name="filterValue">значение фильтра</param>
		public bool IsModifiedFilterDisplayedInTableHeader(string filterValue)
		{
			CustomTestContext.WriteLine("Проверить, что фильтр Modified отображается в шапке таблицы.");

			return Driver.GetIsElementExist(By.XPath(MODIFIED_CLEAR_FILTER_PANEL.Replace("*#*", filterValue)));
		}

		/// <summary>
		/// Проверить, что фильтр Created отображается в шапке таблицы
		/// </summary>
		/// <param name="filterValue">значение фильтра</param>
		public bool IsCreatedFilterDisplayedInTableHeader(string filterValue)
		{
			CustomTestContext.WriteLine("Проверить, что фильтр Created отображается в шапке таблицы.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(CREATED_CLEAR_FILTER_PANEL.Replace("*#*", filterValue)));
		}

		/// <summary>
		/// Проверить, исчезла ли кнопка удаления
		/// </summary>
		/// <param name="source">исходное слово</param>
		/// <param name="target">перевод</param>
		public bool IsDeleteButtonDisappeared(string source, string target)
		{
			CustomTestContext.WriteLine("Проверить, исчезла ли кнопка удаления");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_BUTTON.Replace("*#*", source).Replace("*##*", target)));
		}

		/// <summary>
		/// Проверить, что поле поиска автоматически заполненно значением первого термина.
		/// </summary>
		public bool IsSearchFieldContainSearchedTerm(string term)
		{
			//появляется при переходе со страницы поиска
			CustomTestContext.WriteLine("Проверить, что поле поиска автоматически заполненно значением первого термина.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(SEARCH_FIELD_WITH_TERM.Replace("*#*", term)));
		}

		#endregion

		#region Вспомогательные методы

		/// <summary>
		/// Выполнить скрипт для того, чтобы сделать диалог импорта медиа файла видимым для теста
		/// </summary>
		private GlossaryPage makeMultimediaInputVisible()
		{
			CustomTestContext.WriteLine("Выполнить скрипт для того, чтобы сделать диалог импорта медиа файла видимым для теста");
			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";arguments[0].style[\"visibility\"] = \"visible\";", AddMediaInput);

			return LoadPage();
		}

		/// <summary>
		/// Выполнить скрипт для того, чтобы сделать диалог импорта изображения(с мультимедиа) видимым для теста
		/// </summary>
		private GlossaryPage makeImageWithMultimediaInputVisible()
		{
			CustomTestContext.WriteLine("Выполнить скрипт для того, чтобы сделать диалог импорта изображения(с мультимедиа) видимым для теста");
			Driver.ExecuteScript("$(\"input:file[name = Image]\").removeClass(\"g-hidden\").css(\"opacity\", 100).css(\"width\", 500)");

			return LoadPage();
		}

				/// <summary>
		/// Выполнить скрипт для того, чтобы сделать диалог импорта изображения видимым для теста
		/// </summary>
		private GlossaryPage makeImageInputVisible()
		{
			CustomTestContext.WriteLine("Выполнить скрипт для того, чтобы сделать диалог импорта изображения видимым для теста");
			Driver.ExecuteScript("$(\"input:file[name = x1]\").removeClass(\"g-hidden\").css(\"opacity\", 100).css(\"width\", 500)");

			return LoadPage();
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = TERM)]
		protected IWebElement Term { get; set; }

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

		[FindsBy(How = How.XPath, Using = ADD_MEDIA_INPUT)]
		protected IWebElement AddMediaInput { get; set; }

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

		[FindsBy(How = How.XPath, Using = CLEAR_ALL_FILTERS_BUTTON)]
		protected IWebElement ClearAllFiltersButton { get; set; }

		[FindsBy(How = How.XPath, Using = DIAPASON_PANEL)]
		protected IWebElement DiapasonPanel { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_BUTTON)]
		protected IWebElement AddButton { get; set; }

		[FindsBy(How = How.XPath, Using = COMMENT_FIELD)]
		protected IWebElement CommentField { get; set; }

		protected IWebElement TableWithTerms { get; set; }

		protected IWebElement MultiselectList { get; set; }

		protected IWebElement DeleteTermButton { get; set; }

		protected IWebElement TermRow { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string GLOSSARY_SAVE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[@class='g-btn g-redbtn ']";
		protected const string GLOSSARY_PROPERTIES = "//div[contains(@class,'js-edit-glossary-btn')]";
		protected const string EDIT_GLOSSARY_MENU = "//i[contains(@class,'edit-submenu')]";
		protected const string TERM_FIELD = "//tr[contains(@class, 'js-concept')]//td[*#*]//input[contains(@class,'js-term')]";
		protected const string NEW_ENTRY_BUTTON = "//div[contains(@class,'js-add-concept')]";
		protected const string PLUS_BUTTON = "//td[1]//span[contains(@class,'js-add-term')]";
		protected const string TERM_SAVE_BUTTON = "//i[contains(@class, 'js-save-btn')]";
		protected const string SAVE_ENTRY_BUTTON = "//span[contains(@class,'js-save-btn js-edit')]";
		protected const string GLOSSARY_STRUCTURE = "//div[contains(@class,'js-edit-structure-btn')]";
		protected const string EXTEND_MODE = "//tr[contains(@class, 'js-concept')]//td";
		protected const string IMPORT_BUTTON = "//button[contains(@class,'js-import-concepts')]";
		protected const string EXPORT_BUTTON = "//button[contains(@class,'js-export-concepts')]";
		protected const string TERM_ROW = "//tr[contains(@class, 'js-concept-row')]";
		protected const string TERMS_TEXT = "//tr[contains(@class, 'js-concept-row')]//td[contains(@class, 'glossaryLang')]//p";
		protected const string TABLE_WITH_TERMS = "//table[contains(@class, 'js-sortable-table')]//td//p[contains(text(), '*#*')]//parent::td";
		protected const string COMMENT_FIELD = "//div[contains(@class, 'l-corpr__viewmode__edit js-edit')]//textarea[contains(@name, 'Comment')]";

		protected const string SORT_BY_ENGLISH_TERM = "//th[contains(@data-sort-by,'Language1')]//a";
		protected const string SORT_BY_RUSSIAN_TERM = "//th[contains(@data-sort-by,'Language2')]//a";
		protected const string SORT_BY_DATE_MODIFIED = "//th[contains(@data-sort-by,'LastModifiedDate')]//a";

		protected const string LANGUAGE_HEADER_EDIT_MODE = "//div[contains(@class, 'corprtree')]//div[contains(@class, 'corprtree__langbox')][1]//div[contains(@class, 'l-inactive-lang')]";
		protected const string LANGUAGE_HEADER_VIEW_MODE = "//div[contains(@class, 'js-lang-node')]//span[contains(text(), '*#*')]";
		protected const string LANGUAGE_COMMENT_EDIT_MODE = "//div[contains(@class, 'lang-attrs')][1]//textarea[@name='Comment']";
		protected const string LANGUAGE_COMMENT_VIEW_MODE = "//td[contains(@class, 'js-details-panel')]//div[contains(@class, 'viewmode js-lang-attr')]//p[@title='Comment']/following-sibling::div";
		protected const string ADD_BUTTON = "//div[@class='l-corprtree__langbox'][*#*]//div[contains(@class, 'l-inactive-lang')]//i";
		protected const string TERM_LANGUAGE = "//span[@class='js-term-editor g-block l-corprtree__edittrasl']//input[contains(@class, 'txttrasl')]";
		protected const string ADD_BUTTON_LIST = "//div[contains(@class, 'js-terms-tree')]//div[contains(@class, 'l-corprtree__langbox')]//i[contains(@class,'js-add-term')]";
		protected const string TERM_INPUT = "//div[contains(@class, 'js-terms-tree')]//div[contains(@class, 'js-selected-node')]//span[contains(@class,'js-term-editor')]//input";
		protected const string TERM_INPUT_VIEW_MODE = "//div[contains(@class, 'js-terms-tree')]//div[contains(@class, 'l-corprtree__langbox')][*#*]//span[@class='js-term-viewer']";
		protected const string DEFINITION_EDIT_MODE = "//div[contains(@class, 'l-corpr__viewmode__edit')]//textarea[@name='Interpretation']";
		protected const string DEFINITION_VIEW_MODE = "//div[contains(@class, 'viewmode js-lang-attrs')]//p[@title='Definition']/following-sibling::div[contains(@class, '_viewmode__val js-value')]";
		protected const string DEFINITION_SOURCE_VIEW_MODE = "//div[contains(@class, 'viewmode js-lang-attrs')]//p[@title='Definition source']/following-sibling::div[contains(@class, '_viewmode__val js-value')]";
		protected const string DEFINITION_SOURCE_EDIT_MODE = "//div[contains(@class, 'viewmode js-lang-attrs')]//textarea[@name='InterpretationSource']";

		protected const string CUSTOM_TERM_ROWS = "//tr[@class='l-corpr__trhover clickable js-concept-row']";
		protected const string CLOSE_EXPAND_TERMS_BUTTON = "//i[contains(@class,'js-close-all')]";
		protected const string DEFAULT_TERM_ROWS = "//tr[@class='l-corpr__trhover js-concept-row']";
		protected const string EMPTY_TERM_ERROR = "//div[contains(text(),'Please add at least one term.')]";
		protected const string SYNONYM_PLUS_BUTTON = "//tr[contains(@class, 'js-concept')]//td[*#*]//span[contains(@class,'js-add-term')]//i";
		protected const string SYNONYM_INPUT = "//tr[contains(@class, 'js-concept')]//td[*#*]//div//input[contains(@class,'js-term')]";
		protected const string SYNONYM_FIELDS_IN_COLUMN = "//tr[@class='l-corpr__trhover js-concept-row'][term]//td[column]//p";
		protected const string SYNONYM_UNIQUE_ERROR = "//td[*#*]//p[@title='A term must be unique within a language.']";
		protected const string DELETE_BUTTON = "//tr[contains(@class, 'js-concept-row') and contains(string(), '*#*') and contains(string(), '*##*')]//i[contains(@class, 'js-delete-btn')]";
		protected const string TERM_ROW_BY_SOURCE_AND_TARGET = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '**')]//td[4]";
		protected const string CANCEL_BUTTON = "//tr[contains(@class, 'js-concept-row js-editing opened')]//i[contains(@class, 'js-cancel-btn')]";
		protected const string SEARCH_INPUT = "//input[@name='searchTerm']";
		protected const string SEARCH_FIELD_WITH_TERM = "//div[contains(@class, 'l-corpr__hd right')]//input[contains(@value, '*#*')]";
		protected const string SEARCH_BUTTON = "//a[@title='Search']";
		protected const string TERM = "//tr[contains(@class, 'js-concept-row')]//td[contains(@class,'glossaryShort')][*#*]//p";
		protected const string LANGUAGE_COLUMNS = "//tr[@class='js-table-header']//th[contains(@data-sort-by, 'Language')]";
		protected const string EDIT_TERM_BUTTON = "//tr[contains(@class, 'js-concept-row')]//i[contains(@class,'js-edit-btn')]";
		protected const string EDIT_ENTRY_BUTTON = "//div[contains(@class,'js-edit-btn')]";
		protected const string TERMS_IN_LANGUAGE_AND_TERMS_SECTION = "//div[@class='l-corprtree__langbox']";

		protected const string SINGLE_TERM = "//tr[contains(@class, 'js-concept-row')]//td[1]/p../following-sibling::td[1][contains(string(), '*#*')]";
		protected const string SOURCE_TERM = "//tr[contains(@class, 'js-concept-row') and contains(string(), '*#*')]";
		protected const string SOURCE_TARGET_TERM = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '**')]";
		protected const string SOURCE_TERM_WITH_COMMENT = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '$$')]";
		protected const string SOURCE_TARGET_TERM_WITH_COMMENT = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '**') and contains(string(), '$$')]";
		protected const string DELETE_TERM_BUTTON = "//tr[contains(@class, 'js-concept-row') and contains(string(), '*#*')]//i[@title='Delete']";
		protected const string TERM_TEXT = "//tr[contains(@class, 'js-concept-row')]//td[contains(@class,'glossaryShort')][*#*]//p";
		protected const string CUSTOM_FIELD_NAME = "//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]";
		protected const string CUSTOM_FIELD_INPUT = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]/..//textarea";
		protected const string CUSTOM_FIELD_VIEW_MODE = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class, 'js-view')]//p[text() ='*#*']//following-sibling::div";
		protected const string CUSTOM_FIELD = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]/..//textarea";
		protected const string CUSTOM_FIELD_ERROR = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit l-error')]//p[contains(text(),'*#*')]";
		protected const string CUSTOM_DATE_FIELD = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]/../input[contains(@class,'hasDatepicker')]";
		protected const string TODAY_DATE = "//table[contains(@class,'ui-datepicker-calendar')]//td[contains(@class,'ui-datepicker-today')]";
		protected const string IMAGE_FIELD = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(text(),'*#*')]/..//div[contains(@class,'l-editgloss__imagebox')]//a";
		protected const string FILLED_IMAGE_FIELD = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(text(),'*#*')]/../../div[contains(@class,'l-editgloss__image')]//img[contains(@class,'l-editgloss__imageview')]";
		protected const string CUSTOM_IMAGE_FIELD_ERROR = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')][contains(@class,'l-error')]//p[contains(text(), '*#*')]";
		protected const string ADD_MEDIA_BUTTON = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(text(),'*#*')]/..//span[contains(@class,'l-editgloss__linkbox')]//a[contains(@class,'js-upload-btn')]";
		protected const string CUSTOM_NUMBER_FIELD = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]/..//input[contains(@class,'js-submit-input')]";
		protected const string YES_NO_CHECKBOX = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//input[1]";
		protected const string YES_NO_CHECKBOX_VIEW_MODE = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class, 'js-view')]//p[contains(text(),'*#*')]/..//div";
		protected const string ITEMS_LIST_DROPDOWN = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]/..//span[contains(@class,'js-dropdown')]";
		protected const string ITEMS_LIST = "//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item')][@title='*#*']";
		protected const string MULTISELECT_DROPDOWN = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-edit')]//p[contains(text(),'*#*')]/..//div[contains(@class,'ui-multiselect')]";
		protected const string OPTION_LIST = "//ul[contains(@class, 'multiselect-checkboxes')]";
		protected const string MULTISELECT_LIST = "//span[contains(@class,'ui-multiselect-item-text')][text()='*#*']/..";
		protected const string MEDIA_FIELD = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//p[contains(text(),'*#*')]";
		protected const string PROGRESS_MEDIA_FILE = "//p[contains(text(), '*#*')]/..//img[contains(@class, 'prgrssbar ')]";
		protected const string MEDIA_FIELD_TEXT = "//p[contains(text(), '*#*')]/..//a[contains(@class, 'js-filename-link')]";
		protected const string ADD_IMAGE_LINK = "Add";
		protected const string ADD_IMAGE_INPUT = "//input[@type='file' and @name='x1']";
		protected const string ADD_IMAGE_INPUT_WITH_MULTIMEDIA = "//input[@type='file' and @name='Image']";
		protected const string ADD_MULTIMEDIA_LINK = "//div[@class='l-editgloss__fileName js-filename-click-area js-tour-file-area']";
		protected const string DELETE_IMAGE_BUTTON = "//div[@class='l-editgloss__rmvimgbtn js-clear-btn']";
		protected const string ADD_MEDIA_INPUT = "//input[@type='file' and (@name='Multimedia' or @name='x1')]";

		protected const string SYSTEM_FIELD_TEXTAREA_TYPE = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//textarea[@name='*#*']";
		protected const string SYSTEM_FIELD_DROPDOWN_TYPE = "//div[contains(@class,'js-concept-attrs')]//div[contains(@class,'js-control')]//div[contains(@class, 'js-edit')]//p[text()='*#*']";
		protected const string TOPIC_FIELD = "//div[contains(@class,'ui-dropdown-treeview-wrapper')]";
		protected const string TOPIC_OPTION = "//div[contains(@class,'ui-treeview_node')]//div/span[text()='Life']";
		protected const string TERM_FIELD_EDIT_MODE = "//div[@class='l-corpr__viewmode js-term-attrs']//textarea[@name='*#*']";
		protected const string TERM_FIELD_VIEW_MODE = "//div[@class='l-corpr__viewmode js-term-attrs']//p[@title='*#*']//following-sibling::div";
		protected const string DROPDOWN_TERM_FIELD_EDIT_MODE = "//td[contains(@class,'js-details-panel')]//select[@name='*#*']";
		protected const string TERM_IN_LANGUAGES_AND_TERMS_COLUMN = "//div[@class='l-corprtree__langbox'][*#*]//span[contains(@class, 'term-viewer')]";
		protected const string DROPDOWN_TERM_FIELD_VIEW_MODE = "//td[contains(@class,'js-details-panel')]//div[@class='l-corpr__viewmode js-term-attrs']//select[@name='*#*']/../..//div[contains(@class,'js-value')]";
		protected const string OPTION_TEXT_IN_TERM_FIELD = "//td[contains(@class,'js-details-panel')]//div[@class='l-corpr__viewmode js-term-attrs']//select[@name='*#*']//option[*##*]";
		protected const string DROPDOWN_TERM_FIELD = "//td[contains(@class,'js-details-panel')]//div[@class= 'l-corpr__viewmode js-term-attrs']//select[@name='*#*']/..//span[contains(@class,'js-dropdown')]";
		protected const string OPTION_IN_TERM_FIELD = "//span[contains(@class,'js-dropdown__list')]//span[@data-id='*#*']";

		protected const string FILTER_BUTTON = "//div[contains(@class, 'js-set-filter')]";
		//protected const string LANGUAGE_COLUMNS = "//th[@class='l-corpr__th']//a[@class='g-block l-corpr__thsort' and contains(@href,'orderBy=Language')]"; // колонки языков в таблице на стр одного глоссари
		protected const string CREATED_BY_FILTER_LABEL = "//div[contains(@title, 'Created:')]";
		protected const string MODIFIED_BY_FILTER_LABEL = "//div[contains(@title, 'Modified:')]";
		protected const string ALL_TERMS = "//tr[@class='l-corpr__trhover js-concept-row']//td[contains(@class, 'g-bold')]//p";
		protected const string CREATED_CLEAR_FILTER_PANEL = "//div[contains(@title, 'Created: *#*')]";
		protected const string MODIFIED_CLEAR_FILTER_PANEL = "//div[contains(@title, 'Modified: *#*')]";
		protected const string DELETE_CREATED_FILTER_BUTTON = "//div[contains(@title, 'Created: *#*')]//i";
		protected const string DELETE_MODIFIED_FILTER_BUTTON = "//div[contains(@title, 'Modified: *#*')]//i";
		protected const string CLEAR_ALL_FILTERS_BUTTON = "//i[@title='Clear all filters']";
		protected const string COMMON_CLEAR_FILTER_SECTION = "//div[@class='l-corpr__filter g-bold']";
		protected const string DIAPASON_PANEL = "//div[@class='l-corpr__filterSection inline']";
		protected const string CONCEPTS_TABLE = "//table[contains(@class,'js-concepts')]";
		#endregion
	}
}
