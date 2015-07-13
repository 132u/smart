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
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(NEW_ENTRY_BUTTON)))
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

		[FindsBy(How = How.XPath, Using = TERM_FIELD)]
		protected IWebElement TermField { get; set; }

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

		protected const string GLOSSARY_SAVE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[@class='g-btn g-redbtn ']";
		protected const string GLOSSARY_PROPERTIES = "//div[contains(@class,'js-edit-glossary-btn')]";
		protected const string EDIT_GLOSSARY_MENU = "//span[contains(@class,'js-edit-submenu')]";
		protected const string TERM_FIELD = "//tr[contains(@class, 'js-concept')]//td[*#*]//input[contains(@class,'js-term')]";
		protected const string NEW_ENTRY_BUTTON = "//span[contains(@class,'js-add-concept')]";
		protected const string PLUS_BUTTON = "//td[1]//span[contains(@class,'js-add-term')]";
		protected const string TERM_SAVE_BUTTON = "//tr[contains(@class, 'js-concept-row js-editing opened')]//a[contains(@class, 'js-save-btn')]";
		protected const string GLOSSARY_STRUCTURE = "//div[contains(@class,'js-edit-structure-btn')]";
		protected const string EXTEND_MODE = "//tr[contains(@class, 'js-concept')]//td";
		protected const string IMPORT_BUTTON = "//span[contains(@class,'js-import-concepts')]";
		protected const string EXPORT_BUTTON = "//a[contains(@class,'js-export-concepts')]";
		protected const string TERM_ROW = "//tr[contains(@class, 'js-concept-row')]";

		protected const string SORT_BY_ENGLISH_TERM = "//th[contains(@data-sort-by,'Language1')]//a";
		protected const string SORT_BY_RUSSIAN_TERM = "//th[contains(@data-sort-by,'Language2')]//a";
		protected const string SORT_BY_DATE_MODIFIED = "//th[contains(@data-sort-by,'LastModifiedDate')]//a";
	}
}
