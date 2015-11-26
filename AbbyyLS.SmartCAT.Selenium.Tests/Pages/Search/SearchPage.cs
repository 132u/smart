﻿using System.Collections;
﻿using System.Collections.Generic;
﻿using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search
{
	public class SearchPage : WorkspacePage, IAbstractPage<SearchPage>
	{
		public SearchPage(WebDriver driver) : base(driver)
		{
		}

		public new SearchPage GetPage()
		{
			var searchPage = new SearchPage(Driver);
			InitPage(searchPage, Driver);

			return searchPage;
		}

		public new void LoadPage()
		{
			if (!IsSearchPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница поиска.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести текст в поле поиска
		/// </summary>
		/// <param name="text">текст</param>
		public SearchPage AddTextSearch(string text)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле поиска", text);
			SearchField.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Translate
		/// </summary>
		public SearchPage ClickTranslateButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Translate.");
			TranslateButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по слову-переводу
		/// </summary>
		/// <param name="text">текст перевода</param>
		public SearchPage ClickTranslationWord(string text)
		{
			CustomTestContext.WriteLine("Кликнуть по слову-переводу");
			TranslationWord = Driver.SetDynamicValue(How.XPath, TRANSLATION_WORD, text);
			TranslationWord.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать исходный язык
		/// </summary>
		public SearchPage SelectSourceLanguage(string source)
		{
			CustomTestContext.WriteLine("Выбрать исходный язык {0}.", source);
			SourceLanguageList.Click();
			Driver.SetDynamicValue(How.XPath, LANGUAGE_SOURCE_OPTION, source).Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать язык перевода
		/// </summary>
		public SearchPage SelectTargetLanguage(string target)
		{
			CustomTestContext.WriteLine("Выбрать язык перевода{0}.", target);
			TargetLanguageList.Click();
			Driver.SetDynamicValue(How.XPath, LANGUAGE_TARGET_OPTION, target).Click();

			return GetPage();
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

			return GetPage();
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

			return GetPage();
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
		public bool IsSearchResultDisplay()
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
		/// Проверить, есть ли ссылка на перевод
		/// </summary>
		/// <param name="text">слово, у которого должна быть ссылка на перевод</param>
		public bool IsTranslationReferenceExist(string text)
		{
			CustomTestContext.WriteLine("Проверить, есть ли ссылка на перевод.");

			return Driver.ElementIsDisplayed(By.XPath(TRANSLATION_WORD.Replace("*#*", text)));
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
			return Driver.WaitUntilElementIsDisplay(By.XPath(SEARCH_FORM_XPATH));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.Id, Using = SEARCH_FIELD)]
		protected IWebElement SearchField { get; set; }

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

		protected IWebElement TranslationWord { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string TRANSLATION_WORD = "//a[contains(@class,'js-show-examples')]//span[contains(@class,'translation') and contains(text(),'*#*')]";
		protected const string SEARCH_FORM_XPATH = "//form[contains(@class,'js-search-form')]";
		protected const string SEARCH_FIELD= "searchText";
		protected const string TRANSLATE_BUTTON = "//form[contains(@class,'js-search-form')]//div[contains(@class, 'js-srcpanel-long')]//input[@type='submit']";
		protected const string SEARCH_RESULT = "//div[contains(@class,'js-search-results')]";

		protected const string SOURCE_LANGUAGE_LIST = "SearchSrcLang";
		protected const string TARGET_LANGUAGE_LIST = "SearchDestLang";
		protected const string LANGUAGE_SOURCE_OPTION = "//select[@id='SearchSrcLang']//option[@value='*#*']";
		protected const string LANGUAGE_TARGET_OPTION = "//select[@id='SearchDestLang']//option[@value='*#*']";

		protected const string DEFINITION_TAB = "//li[contains(@data-search-mode,'Interpret')]";

		protected const string AUTOREVERSED_MESSAGE = "//div[contains(@class,'js-language-autoreversed')]";
		protected const string AUTOREVERSED_REFERENCE = "//div[contains(@class,'js-language-autoreversed')]//a[contains(@href,'/Translate')]";

		protected const string TRANSLATION_FORM= "//div[contains(@class,'js-window-examples-data')]";
		protected const string TRANSLATION_FORM_REFERENCE = "//div[contains(@class,'js-window-examples-data')]//a[contains(@class,'g-bold g-link')]";
		protected const string WORD_BY_WORD_TRANSLATION = ".//div[contains(@class,'l-wordbyword')]";
		protected const string REVERSE_TRANSLATION_WORDS  = ".//div[contains(@class,'l-wordbyword')]//table//td//a[contains(@href,'Translate/ru/en')]";
		
		protected const string GLOSSARY_NAMES_LIST = "//div[contains(@class,'js-search-results')]//div[contains(@class,'l-glossary__data')]//h2";
		protected const string TERM_NAME = "//table[*#*]//td/table[contains(@class,'l-glossary__tblsrcword')]//td//span[contains(@class,'l-glossary__srcwordtxt')]";

		#endregion
	}
}
