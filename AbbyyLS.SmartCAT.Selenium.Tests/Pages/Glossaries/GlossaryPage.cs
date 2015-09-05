using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossaryPage : WorkspacePage, IAbstractPage<GlossaryPage>
	{
		public new GlossaryPage GetPage()
		{
			var glossaryPage = new GlossaryPage();
			InitPage(glossaryPage);

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

		/// <summary>
		/// Нажать на кнопку Export
		/// </summary>
		public GlossaryPage ClickExportGlossary()
		{
			Logger.Debug("Нажать кнопку Export.");
			ExportButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'New Entry'
		/// </summary>
		public GlossaryPage ClickNewEntryButton()
		{
			Logger.Debug("Нажать кнопку 'New Entry'.");
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
			Logger.Debug("Ввести {0} в {1} колонке термина.", columnNumber, text);
			var term = Driver.SetDynamicValue(How.XPath, TERM_FIELD, columnNumber.ToString());

			term.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Нажать на галочку для сохраенния термина
		/// </summary>
		public GlossaryPage ClickSaveTermButton()
		{
			Logger.Debug("Нажать на галочку для сохраенния термина.");
			TermSaveButton.Click();

			Driver.WaitUntilElementIsDisappeared(By.XPath(TERM_SAVE_BUTTON));

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку Save Entry для сохраенния термина
		/// </summary>
		public GlossaryPage ClickSaveEntryButton()
		{
			Logger.Debug("Нажать на кнопку Save Entry для сохраенния термина.");
			SaveEntryButton.Click();

			Driver.WaitUntilElementIsDisappeared(By.XPath(SAVE_ENTRY_BUTTON));

			return GetPage();
		}

		/// <summary>
		/// Проверить, что новый термин открыт в расширенном режиме
		/// </summary>
		public GlossaryPage AssertExtendModeOpen()
		{
			Logger.Trace("Проверить, что новый термин открыт в расширенном режиме.");

			Assert.IsTrue(ExtendMode.Displayed, "Произошла ошибка:\n новый термин не открыт в расширенном режиме.");

			return GetPage();
		}

		/// <summary>
		/// Раскрыть меню редактирования глоссария
		/// </summary>
		public GlossaryPage ExpandEditGlossaryMenu()
		{
			Logger.Debug("Раскрыть меню редактирования глоссария.");
			EditGlossaryMenu.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать 'Glossary properties' в меню редактирования глоссария
		/// </summary>
		public GlossaryPropertiesDialog ClickGlossaryProperties()
		{
			Logger.Debug("Выбрать 'Glossary properties' в меню редактирования глоссария");
			GlossaryProperties.Click();

			return new GlossaryPropertiesDialog().GetPage();
		}

		/// <summary>
		/// Выбрать 'Glossary structure' в меню редактирования глоссария
		/// </summary>
		public GlossaryStructureDialog ClickGlossaryStructure()
		{
			Logger.Debug("Выбрать 'GlossaryStructure' в меню редактирования глоссария.");
			GlossaryStructure.Click();

			return new GlossaryStructureDialog().GetPage();
		}

		/// <summary>
		/// Нажать кнопку Import
		/// </summary>
		public GlossaryImportDialog ClickImportButton()
		{
			Logger.Debug("Нажать кнопку Import.");
			ImportButton.Click();

			return new GlossaryImportDialog().GetPage();
		}

		/// <summary>
		/// Проверить, что глоссарий содержит необходимое количество терминов
		/// </summary>
		public GlossaryPage AssertGlossaryContainsCorrectTermsCount(int expectedTermsCount)
		{
			Logger.Trace("Проверить, что глоссарий содержит {0} терминов.", expectedTermsCount);
			var actualTermsCount = Driver.GetElementsCount(By.XPath(TERM_ROW));

			Assert.AreEqual(actualTermsCount, expectedTermsCount,
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по английским терминам
		/// </summary>
		public GlossaryPage ClickSortByEnglishTerm()
		{
			Logger.Debug("Нажать кнопку сортировки по английским терминам.");

			SortByEnglishTerm.Click();

			return GetPage();
		}

		/// <summary>
		/// Открыть подробную информацию по языку или термину в режиме редактирования
		/// </summary>
		public GlossaryPage OpenLanguageAndTermDetailsEditMode()
		{
			Logger.Debug("Открыть подробную информацию по языку или термину в режиме редактирования.");
			LanguageHeaderEditMode.Click();

			return GetPage();
		}

		/// <summary>
		/// Открыть подробную информацию по языку или термину в режиме просмотра
		/// </summary>
		public GlossaryPage OpenLanguageAndTermDetailsViewMode()
		{
			Logger.Debug("Открыть подробную информацию по языку или термину в режиме просмотра.");
			LanguageHeaderViewMode.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что комментарий содержит текст.
		/// </summary>
		public GlossaryPage AssertCommentIsFilled(string comment)
		{
			Logger.Trace("Проверить, что комментарий содержит текст {0}.", comment);

			Assert.AreEqual(comment, LanguageCommentViewMode.Text, "Произошла ошибка:\n неверный текст в поле комментария.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что поле Definition содержит текст.
		/// </summary>
		public GlossaryPage AssertDefinitionIsFilled(string text)
		{
			Logger.Trace("Проверить, что поле Definition содержит текст {0}.", text);

			Assert.AreEqual(text, DefinitionViewMode.Text, "Произошла ошибка:\n неверный текст в поле Definition.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что поле 'Definition source' содержит текст.
		/// </summary>
		public GlossaryPage AssertDefinitionSourceIsFilled(string text)
		{
			Logger.Trace("Проверить, что поле 'Definition source' содержит текст {0}.", text);

			Assert.AreEqual(text, DefinitionSourceViewMode.Text,
				"Произошла ошибка:\n неверный текст в поле 'Definition source'.");

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в поле комментария в секции 'Language and term details'
		/// </summary>
		public GlossaryPage FillLanguageComment(string comment)
		{
			Logger.Debug("Ввести {0} в поле комментария в секции 'Language and term details'.", comment);

			LanguageCommentEditMode.SetText(comment);

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в поле Definition в секции 'Language and term details'
		/// </summary>
		public GlossaryPage FillDefinition(string text)
		{
			Logger.Debug("Ввести {0} в поле Definition в секции 'Language and term details'.", text);

			DefinitionEditMode.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в поле 'Definition source' в секции 'Language and term details'
		/// </summary>
		public GlossaryPage FillDefinitionSource(string text)
		{
			Logger.Debug("Ввести {0} в поле 'Definition source' в секции 'Language and term details'.", text);

			DefinitionSourceEditMode.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Добавить термин и ввести текст в поле термина в секции 'Languages and terms'
		/// </summary>
		/// <param name="text">текст</param>
		public GlossaryPage FillTermInLanguagesAndTermsSection(string text)
		{
			Logger.Debug("Добавить термин и ввести текст в поле термина в секции 'Languages and terms'.");
			var addButtonLists = Driver.GetElementList(By.XPath(ADD_BUTTON_LIST));

			for (var i = 1; i <= addButtonLists.Count; i++)
			{
				Logger.Debug("Нажать кнопку Add №{0} в 'Languages and terms'.", i);
				addButtonLists[i - 1].Click();

				Logger.Debug("Ввести {0} в поле термина №{1} в секции 'Languages and terms'.", text, i);
				Driver.SetDynamicValue(How.XPath, TERM_INPUT, i.ToString()).SetText(text);
			}

			return GetPage();
		}

		/// <summary>
		/// Посчитать количество терминов в секции 'Languages and terms'
		/// </summary>
		public int TermsCountInLanguagesAndTermsSection()
		{
			Logger.Trace("Посчитать количесвто терминов в секции 'Languages and terms'.");
			return Driver.GetElementsCount(By.XPath(TERMS_IN_LANGUAGE_AND_TERMS_SECTION));
		}

		/// <summary>
		/// Нажать на термин в секции 'Languages and terms'
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		public GlossaryPage ClickTermInLanguagesAndTermsSection(int termNumber)
		{
			Logger.Debug("Нажать на термин №{0} в секции 'Languages and terms'.", termNumber);
			Driver.SetDynamicValue(How.XPath, TERM_INPUT_VIEW_MODE, termNumber.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в термин
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		public GlossaryPage EditTermInLanguagesAndTermsSection(string text, int termNumber)
		{
			Logger.Debug("Ввести текст {0} в термин №{1}.", text, termNumber);
			Driver.SetDynamicValue(How.XPath, TERM_INPUT, termNumber.ToString()).SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по русским терминам
		/// </summary>
		public GlossaryPage ClickSortByRussianTerm()
		{
			Logger.Debug("Нажать кнопку сортировки по русским терминам.");

			SortByRussianTerm.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате изменения
		/// </summary>
		public GlossaryPage ClickSortByDateModified()
		{
			Logger.Debug("Нажать кнопку сортировки по дате изменения");
			SortByDateModified.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить количество обычных терминов
		/// </summary>
		public int DefaultTermsCount()
		{
			Logger.Trace("Получить количество обычных терминов.");

			return Driver.GetElementsCount(By.XPath(DEFAULT_TERM_ROWS));
		}

		/// <summary>
		/// Получить количество расширенных терминов
		/// </summary>
		public int CustomTermsCount()
		{
			Logger.Trace("Получить количество расширенных терминов.");

			return Driver.GetElementsCount(By.XPath(CUSTOM_TERM_ROWS));
		}

		/// <summary>
		/// Нажать на строку термина
		/// </summary>
		public GlossaryPage ClickCustomTerm()
		{
			Logger.Trace("Нажать на строку термина.");
			CustomTermRow.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку закрытия раскрытых терминов
		/// </summary>
		public GlossaryPage CloseExpandedTerms()
		{
			Logger.Debug("Нажать кнопку закрытия раскрытых терминов.");
			CloseExpandTermsButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить значение свсойства Display для элемента сообщения 'The term already exists'
		/// </summary>
		public bool AlreadyExistTermErrorDisplayed()
		{
			Logger.Trace("Получить значение свойства Display для элемента сообщения 'The term already exists'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ALREADY_EXIST_TERM_ERROR));
		}

		/// <summary>
		/// Получить значение свсойства Display для элемента сообщения 'Please add at least one term'.
		/// </summary>
		public bool EmptyTermErrorDisplayed()
		{
			Logger.Trace("Получить значение свойства Display для элемента сообщения 'Please add at least one term'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(EMPTY_TERM_ERROR));
		}

		/// <summary>
		/// Нажать кнопку добавления синонима
		/// </summary>
		/// <param name="columnNumber">номер колонки</param>
		public GlossaryPage ClickSynonymPlusButton(int columnNumber)
		{
			Logger.Debug("Нажать кнопку добавления синонима.");
			var plusButton = Driver.SetDynamicValue(How.XPath, SYNONYM_PLUS_BUTTON, columnNumber.ToString());

			plusButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в поле синонима
		/// </summary>
		public GlossaryPage FillSynonym(string text, int columnNumber)
		{
			Logger.Debug("Ввести {0} в поле синонима в столбце №{1}.", text, columnNumber);
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
			Logger.Trace("Получить количеcтво синонимов в термине №{0}, стоблец №{1}.", termRow, columnNumber);

			return Driver.GetElementsCount(By.XPath(SYNONYM_FIELDS_IN_COLUMN.Replace("term", termRow.ToString()).Replace("column", columnNumber.ToString())));
		}

		/// <summary>
		/// Проверить, что термины подсвечены красным цветом, так как термины должны быть уникальны
		/// </summary>
		public GlossaryPage AssertSynonumUniqueErrorDisplayed(int columnNumber)
		{
			Logger.Trace("Проверить, что термины подсвечены красным цветом в стоблце №{0}, так как термины должны быть уникальны.", columnNumber);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(SYNONYM_UNIQUE_ERROR.Replace("*#*", columnNumber.ToString()))),
				"Произошла ошибка:\n Термины не подсвечены красным цветом в стоблце №{0}.", columnNumber);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку удаления термина
		/// </summary>
		/// <param name="source">источник</param>
		/// <param name="target">таргет</param>
		public GlossaryPage ClickDeleteButton(string source, string target)
		{
			Logger.Debug("Нажать кнопку удаления термина с sourse:{0}, target:{1}.", source, target);
			Driver.FindElement(By.XPath(DELETE_BUTTON.Replace("#", source).Replace("**", target))).Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка удаления термина исчезла
		/// </summary>
		public GlossaryPage AssertDeleteButtonDisappeared(string source, string target)
		{
			Logger.Debug("Проверить, что кнопка удаления термина исчезла.");
			
			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_BUTTON.Replace("#", source).Replace("**", target))),
				"Произошла ошибка:\n Кнопка удаления термина не исчезла.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку редактирования термина
		/// </summary>
		public GlossaryPage ClickEditButton()
		{
			Logger.Debug("Нажать кнопку редактирования термина.");
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
			Logger.Debug("Навести курсор мыши на заданную строку термина с sourse:{0}, target:{1}", source, target);
			var term = Driver.FindElement(By.XPath(TERM_ROW_BY_SOURCE_AND_TARGET.Replace("#", source).Replace("**", target)));
			term.HoverElement();

			return GetPage();
		}

		/// <summary>
		/// Заполнить поле поиск
		/// </summary>
		public GlossaryPage FillSearchField(string text)
		{
			Logger.Debug(string.Format("Ввести {0} в поле поиск.", text));
			SearchInput.SetText(text);
			
			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку поиска 
		/// </summary>
		public GlossaryPage ClickSearchButton()
		{
			Logger.Debug("Нажать кнопку поиска.");
			SearchButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Edit Entry'
		/// </summary>
		public GlossaryPage ClickEditEntryButton()
		{
			Logger.Debug("Нажать кнопку 'Edit Entry'.");
			EditEntryButton.Click();

			return GetPage();
		}
		
		/// <summary>
		/// Нажать кнопку Cancel
		/// </summary>
		public GlossaryPage ClickCancelButton()
		{
			Logger.Debug("Нажать кнопку Cancel.");
			CancelButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить текст из первого термина.
		/// </summary>
		public string FirstTermText()
		{
			Logger.Trace("Получить текст из первого термина.");

			return FirstTerm.Text.Trim();
		}

		/// <summary>
		/// Посчитать количество колонок с языками
		/// </summary>
		public int LanguageColumnCount()
		{
			Logger.Trace("Посчитать количество колонок с языками.");

			return Driver.GetElementsCount(By.XPath(LANGUAGE_COLUMNS));
		}

		/// <summary>
		/// Получить список текстов из терминов
		/// </summary>
		public List<string> TermsList()
		{
			Logger.Trace("Получить список текстов из терминов.");
			var terms = Driver.GetTextListElement(By.XPath(TERMS_TEXT));

			return terms.Select(el => el.Trim()).ToList();
		}

		/// <summary>
		/// Проверить, что термин присутствует в секции 'Languages and terms '
		/// </summary>
		public bool AssertTermDisplayedInLanguagesAndTermsSection(string term)
		{
			Logger.Trace("Проверить, что термин {0} присутствует в секции 'Languages and terms '.", term);
			var termInSectioin = Driver.SetDynamicValue(How.XPath, TERMS_IN_LANGUAGE_AND_TERMS_SECTION, term);
			
			return termInSectioin.Displayed;
		}

		/// Проверить наличие термина
		/// </summary>
		/// <param name="text">текст</param>
		public GlossaryPage AssertIsSingleTermExists(string text)
		{
			Logger.Trace("Проверить наличие термина {0}", text);

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
			Logger.Trace("Получить, есть ли термин с сорсом {0} и таргетом {1}", sourceText, targetText);

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
			Logger.Trace("Получить, есть ли термин с сорсом '{0}', таргетом '{1}' и комментом: '{2}'", sourceText, targetText,
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
			Logger.Trace("Получить, есть ли термин с сорсом '{0}' и комментом: '{1}'", sourceText, comment);

			Assert.IsTrue(
				Driver.WaitUntilElementIsDisplay(
					By.XPath(SOURCE_TERM_WITH_COMMENT.Replace("#", sourceText).Replace("$$", comment))),
				"Произошла ошибка:\n термин не обнаружен.");

			return GetPage();
		}

		/// <summary>
		/// Удалить термин из глоссария
		/// </summary>
		/// <param name="source">термин</param>
		public GlossaryPage DeleteTerm(string source)
		{
			Logger.Debug("Удалить термин {0} из глоссария", source);
			var deleteTermButton = Driver.SetDynamicValue(How.XPath, DELETE_TERM_BUTTON, source);
			var termRow = Driver.SetDynamicValue(How.XPath, SOURCE_TERM, source);
			termRow.HoverElement();
			deleteTermButton.Click();

			return this;
		}

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

		protected const string GLOSSARY_SAVE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[@class='g-btn g-redbtn ']";
		protected const string GLOSSARY_PROPERTIES = "//div[contains(@class,'js-edit-glossary-btn')]";
		protected const string EDIT_GLOSSARY_MENU = "//span[contains(@class,'js-edit-submenu')]";
		protected const string TERM_FIELD = "//tr[contains(@class, 'js-concept')]//td[*#*]//input[contains(@class,'js-term')]";
		protected const string NEW_ENTRY_BUTTON = "//span[contains(@class,'js-add-concept')]";
		protected const string PLUS_BUTTON = "//td[1]//span[contains(@class,'js-add-term')]";
		protected const string TERM_SAVE_BUTTON = "//span//a[contains(@class,'js-save-btn')]";
		protected const string SAVE_ENTRY_BUTTON = "//span[contains(@class,'js-save-btn')]";
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
	}
}
