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
		/// Проверить, что строка для нового термина появилась
		/// </summary>
		public GlossaryPage AssertEmptyRowForNewTermDisplay()
		{
			Logger.Trace("Проверить, что строка для нового термина появилась.");

			Assert.IsTrue(PlusButton.Displayed, "Произошла ошибка:\n строка для нового термина не появилась.");

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

			Assert.AreEqual(actualTermsCount , expectedTermsCount,
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

			Assert.AreEqual(comment, LanguageCommentViewMode.Text,
				"Произошла ошибка:\n неверный текст в поле комментария.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что поле Definition содержит текст.
		/// </summary>
		public GlossaryPage AssertDefinitionIsFilled(string text)
		{
			Logger.Trace("Проверить, что поле Definition содержит текст {0}.", text);

			Assert.AreEqual(text, DefinitionViewMode.Text,
				"Произошла ошибка:\n неверный текст в поле Definition.");

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

		[FindsBy(How = How.XPath, Using = NEW_ENTRY_BUTTON)]
		protected IWebElement NewEntryButton { get; set; }

		[FindsBy(How = How.XPath, Using = PLUS_BUTTON)]
		protected IWebElement PlusButton { get; set; }
		
		[FindsBy(How = How.XPath, Using = TERM_SAVE_BUTTON)]
		protected IWebElement TermSaveButton { get; set; }

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

		[FindsBy(How = How.XPath, Using = DEFINITION_EDIT_MODE)]
		protected IWebElement DefinitionEditMode { get; set; }

		[FindsBy(How = How.XPath, Using = DEFINITION_VIEW_MODE)]
		protected IWebElement DefinitionViewMode { get; set; }

		[FindsBy(How = How.XPath, Using = DEFINITION_SOURCE_EDIT_MODE)]
		protected IWebElement DefinitionSourceEditMode { get; set; }

		[FindsBy(How = How.XPath, Using = DEFINITION_SOURCE_VIEW_MODE)]
		protected IWebElement DefinitionSourceViewMode { get; set; }

		protected const string GLOSSARY_SAVE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[@class='g-btn g-redbtn ']";
		protected const string GLOSSARY_PROPERTIES = "//div[contains(@class,'js-edit-glossary-btn')]";
		protected const string EDIT_GLOSSARY_MENU = "//span[contains(@class,'js-edit-submenu')]";
		protected const string TERM_FIELD = "//tr[contains(@class, 'js-concept')]//td[*#*]//input[contains(@class,'js-term')]";
		protected const string NEW_ENTRY_BUTTON = "//span[contains(@class,'js-add-concept')]";
		protected const string PLUS_BUTTON = "//td[1]//span[contains(@class,'js-add-term')]";
		protected const string TERM_SAVE_BUTTON = "//span//a[contains(@class,'js-save-btn')]";
		protected const string GLOSSARY_STRUCTURE = "//div[contains(@class,'js-edit-structure-btn')]";
		protected const string EXTEND_MODE = "//tr[contains(@class, 'js-concept')]//td";
		protected const string IMPORT_BUTTON = "//span[contains(@class,'js-import-concepts')]";
		protected const string EXPORT_BUTTON = "//a[contains(@class,'js-export-concepts')]";
		protected const string TERM_ROW = "//tr[contains(@class, 'js-concept-row')]";

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
		protected const string DEFINITION_EDIT_MODE = "//div[contains(@class, 'viewmode js-lang-attrs')]//textarea[@name='Interpretation']";
		protected const string DEFINITION_VIEW_MODE = "//div[@class='l-corpr__viewmode js-lang-attrs']//div[@class='js-control'][2]//div[@class='g-bold l-corpr__viewmode__val js-value']";
		protected const string DEFINITION_SOURCE_VIEW_MODE = "//div[@class='l-corpr__viewmode js-lang-attrs']//div[@class='js-control'][3]//div[@class='g-bold l-corpr__viewmode__val js-value']";
		protected const string DEFINITION_SOURCE_EDIT_MODE = "//div[contains(@class, 'viewmode js-lang-attrs')]//textarea[@name='InterpretationSource']";

	}
}
