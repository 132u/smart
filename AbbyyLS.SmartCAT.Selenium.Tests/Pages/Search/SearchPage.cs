﻿﻿using System.Collections.Generic;
﻿using System.Linq;

﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search.SearchPageTabs;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search
{
	public class SearchPage : WorkspacePage, IAbstractPage<SearchPage>
	{
		public SearchPage(WebDriver driver) : base(driver)
		{
		}

		public new SearchPage LoadPage()
		{
			if (!IsSearchPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница поиска.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Получить поисковый запрос из поля поиска
		/// </summary>
		public string GetSearchQueryFromSearchField()
		{
			CustomTestContext.WriteLine("Получить поисковый запрос из поля поиска");

			return SearchField.GetAttribute("value");
		}

		/// <summary>
		/// Ввести текст в поле поиска
		/// </summary>
		/// <param name="text">текст</param>
		public SearchPage AddTextSearch(string text)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле поиска", text);
			SearchField.SetText(text);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Translate
		/// </summary>
		public SearchPage ClickTranslateButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Translate.");
			TranslateButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать исходный язык
		/// </summary>
		public SearchPage SelectSourceLanguage(string source)
		{
			CustomTestContext.WriteLine("Выбрать исходный язык {0}.", source);
			SourceLanguageList.Click();
			Driver.SetDynamicValue(How.XPath, LANGUAGE_SOURCE_OPTION, source).Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать язык перевода
		/// </summary>
		public SearchPage SelectTargetLanguage(string target)
		{
			CustomTestContext.WriteLine("Выбрать язык перевода{0}.", target);
			TargetLanguageList.Click();
			Driver.SetDynamicValue(How.XPath, LANGUAGE_TARGET_OPTION, target).Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить список названий глоссариев
		/// </summary>
		public IEnumerable<string> GlossaryNamesList()
		{
			CustomTestContext.WriteLine("Получить список названий глоссариев.");
			var glossaries = Driver.GetTextListElement(By.XPath(GLOSSARY_NAMES_LIST));

			return glossaries.Select(g => g.Substring(g.IndexOf('\n') + 1));
		}

		/// <summary>
		/// Нажать на перевод в окне перевода
		/// </summary>
		public SearchPage ClickTranslationFormReference()
		{
			CustomTestContext.WriteLine("Нажать на перевод в окне перевода.");
			TranslationFormReference.Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить имя термина
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		public string TermName(int termNumber)
		{
			CustomTestContext.WriteLine("Получить имя термина №{0}.", termNumber);
			var termName = Driver.SetDynamicValue(How.XPath, TERM_NAME, termNumber.ToString());

			return termName.Text;
		}

		/// <summary>
		/// Открыть историю поиска.
		/// </summary>
		public SearchPage OpenSearchHistory()
		{
			CustomTestContext.WriteLine("Открыть историю поиска.");
			SearchHistoryButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить список названий терминов из истории поиска.
		/// </summary>
		public List<string> GetSearchinqQueryFromHistory()
		{
			CustomTestContext.WriteLine("Получить список названий терминов из истории поиска.");

			return Driver.GetTextListElement(By.XPath(LIST_WITH_SEARCHING_QUERYS));
		}

		/// <summary>
		/// Переключиться на вкладку 'Translations'.
		/// </summary>
		public TranslationsTab SwitchToTranslationsTab()
		{
			CustomTestContext.WriteLine("Переключиться на вкладку 'Translations'.");
			TranslationsTab.Click();

			return new TranslationsTab(Driver).LoadPage();
		}

		/// <summary>
		/// Переключиться на вкладку 'Examples'.
		/// </summary>
		public ExamplesTab SwitchToExamplesTab()
		{
			CustomTestContext.WriteLine("Переключиться на вкладку 'Examples'.");
			ExamplesTab.Click();

			return new ExamplesTab(Driver).LoadPage();
		}

		/// <summary>
		/// Переключиться на вкладку 'Phrases'.
		/// </summary>
		public PhrasesTab SwitchToPhrasesTab()
		{
			CustomTestContext.WriteLine("Переключиться на вкладку 'Phrases'.");
			PhrasesTab.Click();

			return new PhrasesTab(Driver).LoadPage();
		}

		/// <summary>
		/// Переключиться на вкладку 'Definitions'.
		/// </summary>
		public DefinitionsTab SwitchToDefinitionsTab()
		{
			CustomTestContext.WriteLine("Переключиться на вкладку Definitions'.");
			DefenitionsTab.Click();

			return new DefinitionsTab(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выполнить поиск
		/// </summary>
		/// <param name="searchText"></param>
		public SearchPage InitSearch(string searchText)
		{
			AddTextSearch(searchText);
			ClickTranslateButton();

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что заданный термин совпадает с найденными терминами
		/// </summary>
		/// <param name="term">термин</param>
		public bool IsTermNamesMatch(string term)
		{
			CustomTestContext.WriteLine("Проверить, что термин {0} совпадает с найденными терминами.", term);

			for (var i = 1; i <= GlossaryNamesList().Count(); i++)
			{
				if (term != TermName(i))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Проверить, отображается ли таблица с обратным переводом со ссылками
		/// </summary>
		public bool IsReverseTranslationListExist()
		{
			CustomTestContext.WriteLine("Проверить, отображается ли таблица с обратным переводом со ссылками.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(REVERSE_TRANSLATION_WORDS));
		}

		/// <summary>
		/// Проверить, что появились результаты поиска.
		/// </summary>
		/// <returns></returns>
		public bool IsSearchResultDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появились результаты поиска.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(SEARCH_RESULT));
		}

		/// <summary>
		/// Проверить, что списки имен глоссариев совпадают
		/// </summary>
		/// <param name="glossaryNames">список имен глоссариев</param>
		public bool IsGlossariesNamesMatch(List<string> glossaryNames)
		{
			CustomTestContext.WriteLine("Проверить, что списки имен глоссариев совпадают.");

			return glossaryNames.OrderBy(m => m).SequenceEqual(GlossaryNamesList().OrderBy(m => m));
		}

		/// <summary>
		/// Проверить, активна ли вкладка Definitions
		/// </summary>
		public bool IsDefinitionTabActive()
		{
			CustomTestContext.WriteLine("Проверить, активна ли вкладка Definitions.");

			return DefinitionTab.GetElementAttribute("class").Contains("active");
		}

		/// <summary>
		/// Проверить, появилось ли сообщение об автоматическом изменении языка
		/// </summary>
		public bool IsAutoreversedMessageExist()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение об автоматическом изменении языка.");

			return Driver.ElementIsDisplayed(By.XPath(AUTOREVERSED_MESSAGE));
		}

		/// <summary>
		/// Проверить, есть ли ссылка автоматического изменения языка
		/// </summary>
		public bool IsAutoreversedReferenceExist()
		{
			CustomTestContext.WriteLine("Проверить, есть ли ссылка автоматического изменения языка.");

			return Driver.ElementIsDisplayed(By.XPath(AUTOREVERSED_REFERENCE));
		}

		/// <summary>
		/// Проверить, появился ли перевод по словам
		/// </summary>
		public bool IsWordByWordTranslationAppear()
		{
			CustomTestContext.WriteLine("Проверить, появился ли перевод по словам.");

			return Driver.ElementIsDisplayed(By.XPath(WORD_BY_WORD_TRANSLATION));
		}

		/// <summary>
		/// Проверить, открыта ли страница поиска
		/// </summary>
		public bool IsSearchPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SEARCH_FORM_XPATH)) &&
				IsDialogBackgroundDisappeared();
		}

		/// <summary>
		/// Проверить, появилась ли надпись 'Примеры не найдены'
		/// </summary>
		public bool IsNoExamplesFoundMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появилась ли надпись 'Примеры не найдены'");

			return Driver.WaitUntilElementIsDisplay(By.XPath(NO_EXAMPLES_FOUND_IMAGE));
		}

		/// <summary>
		/// Проверить, появилась ли надпись 'Поиск в корпоративных глоссариях не дал результатов'"
		/// </summary>
		public bool IsNothingFoundInGlossariesDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появилась ли надпись, 'Поиск в корпоративных глоссариях не дал результатов'");

			return Driver.WaitUntilElementIsDisplay(By.XPath(NOTHING_FOUND_IN_GLOSSARIES_MESSAGE));
		}

		/// <summary>
		/// Проверить, совпадает ли слово в поисковой строке с заданным.
		/// </summary>
		/// <param name="word">слово, которое ожидается в поисковой строке</param>
		public bool IsWordInSearchFieldCorrect(string word)
		{
			CustomTestContext.WriteLine("Проверить, совпадает ли слово в поисковой строке с заданным.");
			var wordFromSearchField = Driver.GetTextListElement(By.XPath(SEARCH_FIELD_PATH));

			return word == wordFromSearchField[0];
		}

		/// <summary>
		/// Проверить, совпадает ли отображенный язык в таргете с заданным.
		/// </summary>
		/// <param name="language">язык</param>
		public bool IsTargetLanguageCorrect(string language)
		{
			CustomTestContext.WriteLine("Проверить, совпадает ли отображенный язык в таргете с заданным.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(TARGET_LANGUAGE.Replace("*#*", language)));
		}

		/// <summary>
		/// Проверить, совпадает ли отображенный язык в сорсе с заданным.
		/// </summary>
		/// <param name="language">язык</param>
		public bool IsSourceLanguageCorrect(string language)
		{
			CustomTestContext.WriteLine("Проверить, совпадает ли отображенный язык в сорсе с заданным.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(SOURCE_LANGUAGE.Replace("*#*", language)));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.Id, Using = SEARCH_FIELD)]
		protected IWebElement SearchField { get; set; }

		[FindsBy(How = How.XPath, Using = NO_EXAMPLES_FOUND_IMAGE)]
		protected IWebElement NoExamplesFoundImage { get; set; }

		[FindsBy(How = How.Id, Using = SOURCE_LANGUAGE_LIST)]
		protected IWebElement SourceLanguageList { get; set; }

		[FindsBy(How = How.Id, Using = TARGET_LANGUAGE_LIST)]
		protected IWebElement TargetLanguageList { get; set; }

		[FindsBy(How = How.XPath, Using = TRANSLATION_FORM_REFERENCE)]
		protected IWebElement TranslationFormReference { get; set; }

		[FindsBy(How = How.XPath, Using = DEFINITION_TAB)]
		protected IWebElement DefinitionTab { get; set; }

		[FindsBy(How = How.XPath, Using = TRANSLATE_BUTTON)]
		protected IWebElement TranslateButton { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_HISTORY_BUTTON)]
		protected IWebElement SearchHistoryButton { get; set; }

		[FindsBy(How = How.XPath, Using = TRANSLATIONS_TAB)]
		protected IWebElement TranslationsTab { get; set; }

		[FindsBy(How = How.XPath, Using = EXAMPLES_TAB)]
		protected IWebElement ExamplesTab { get; set; }

		[FindsBy(How = How.XPath, Using = PHRASES_TAB)]
		protected IWebElement PhrasesTab { get; set; }

		[FindsBy(How = How.XPath, Using = DEFENITIONS_TAB)]
		protected IWebElement DefenitionsTab { get; set; }

		protected IWebElement TranslationWord { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string SEARCH_FORM_XPATH = "//form[contains(@class,'js-search-form')]";
		protected const string SEARCH_FIELD= "searchText";
		protected const string TRANSLATE_BUTTON = "//form[contains(@class,'js-search-form')]//div[contains(@class, 'js-srcpanel-long')]//input[@type='submit']";
		protected const string SEARCH_RESULT = "//div[contains(@class,'js-search-results')]";
		protected const string SEARCH_FIELD_PATH = "//div[contains(@class, 'g-srcpanel__srcbar')]//textarea[contains(@class, 'js-source-text')]";
		protected const string SEARCH_HISTORY_BUTTON = "//div[contains(@class, 'g-srchistr')]//a[contains(text(), 'Search history')]";
		protected const string LIST_WITH_SEARCHING_QUERYS = "//div[contains(@class, 'g-srchistr')]";

		protected const string SOURCE_LANGUAGE_LIST = "SearchSrcLang";
		protected const string TARGET_LANGUAGE_LIST = "SearchDestLang";
		protected const string LANGUAGE_SOURCE_OPTION = "//select[@id='SearchSrcLang']//option[@value='*#*']";
		protected const string LANGUAGE_TARGET_OPTION = "//select[@id='SearchDestLang']//option[@value='*#*']";

		protected const string DEFINITION_TAB = "//li[contains(@data-search-mode,'Interpret')]";

		protected const string DEFENITIONS_TAB = "//div[contains(@class, 'g-srctabs')]//a[text()='Definitions']";
		protected const string TRANSLATIONS_TAB = "//div[contains(@class, 'g-srctabs')]//a[text()='Translations']";
		protected const string PHRASES_TAB = "//div[contains(@class, 'g-srctabs')]//a[text()='Phrases']";
		protected const string EXAMPLES_TAB = "//div[contains(@class, 'g-srctabs')]//a[text()='Examples']";

		protected const string NO_EXAMPLES_FOUND_IMAGE = "//div[contains(@class, 'js-empty-icon-box')]";
		protected const string NOTHING_FOUND_IN_GLOSSARIES_MESSAGE = "//p[contains(@class, 'l-glossary__nothingFound')]";

		protected const string AUTOREVERSED_MESSAGE = "//div[contains(@class,'js-language-autoreversed')]";
		protected const string AUTOREVERSED_REFERENCE = "//div[contains(@class,'js-language-autoreversed')]//a[contains(@href,'/Translate')]";

		protected const string TRANSLATION_FORM= "//div[contains(@class,'js-window-examples-data')]";
		protected const string TRANSLATION_FORM_REFERENCE = "//div[contains(@class,'js-window-examples-data')]//a[contains(@class,'g-bold g-link')]";
		protected const string WORD_BY_WORD_TRANSLATION = ".//div[contains(@class,'l-wordbyword')]";
		protected const string REVERSE_TRANSLATION_WORDS  = ".//div[contains(@class,'l-wordbyword')]//table//td//a[contains(@href,'Translate/ru/en')]";
		
		protected const string GLOSSARY_NAMES_LIST = "//div[contains(@class,'js-search-results')]//div[contains(@class,'l-glossary__data')]//h2";
		protected const string TERM_NAME = "//table[*#*]//td/table[contains(@class,'l-glossary__tblsrcword')]//td//span[contains(@class,'l-glossary__srcwordtxt')]";

		protected const string TARGET_LANGUAGE = "//div[contains(@class, 'js-swap-container')]//select[contains(@class, 'js-swap-second')]//option[contains(@selected, 'selected') and contains(@value, '*#*')]//parent::select";
		protected const string SOURCE_LANGUAGE = "//div[contains(@class, 'js-swap-container')]//select[contains(@class, 'js-swap-first')]//option[contains(@selected, 'selected') and contains(@value, '*#*')]//parent::select";

		#endregion
	}
}
