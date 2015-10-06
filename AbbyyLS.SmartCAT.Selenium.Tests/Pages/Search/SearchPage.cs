﻿using System.Collections.Generic;
﻿using System.Linq;

﻿using NUnit.Framework;
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
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SEARCH_FORM_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница поиска.");
			}
		}

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
		/// Проверить, отображается ли таблица с обратным переводом со ссылками
		/// </summary>
		public SearchPage AssertReverseTranslationListExist()
		{
			CustomTestContext.WriteLine("Проверить, отображается ли таблица с обратным переводом со ссылками.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(REVERSE_TRANSLATION_WORDS)),
				"Произошла ошибка:\n таблица с обратным переводом со ссылками не отображается.");

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
		/// Проверить, что появились результаты поиска.
		/// </summary>
		/// <returns></returns>
		public SearchPage AssertSearchResultDisplay()
		{
			CustomTestContext.WriteLine("Проверить, что появились результаты поиска.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(SEARCH_RESULT)),
				"Произошла ошибка:\n результаты поиска не появились.");

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
		/// Проверить, активна ли вкладка Definitions
		/// </summary>
		public SearchPage AssertDefinitionTabIsActive()
		{
			CustomTestContext.WriteLine("Проверить, активна ли вкладка Definitions.");

			Assert.IsTrue(DefinitionTab.GetElementAttribute("class").Contains("active"),
				"Произошла ошибка:\n вкладка Definitions неактивна.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, есть ли ссылка на перевод
		/// </summary>
		/// <param name="text">слово, у которого должна быть ссылка на перевод</param>
		public SearchPage AssertTranslationReferenceExist(string text)
		{
			CustomTestContext.WriteLine("Проверить, есть ли ссылка на перевод.");

			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(TRANSLATION_WORD.Replace("*#*", text))),
				"Произошла ошибка:\n ссылка на перевод отсутствует.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, появилось ли сообщение об автоматическом изменении языка
		/// </summary>
		public SearchPage AssertAutoreversedMessageExist()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение об автоматическом изменении языка.");

			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(AUTOREVERSED_MESSAGE)),
				"Произошла ошибка:\n сообщение об автоматическом изменении языка не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, есть ли ссылка автоматического изменения языка
		/// </summary>
		public SearchPage AssertAutoreversedReferenceExist()
		{
			CustomTestContext.WriteLine("Проверить, есть ли ссылка автоматического изменения языка.");

			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(AUTOREVERSED_REFERENCE)),
				"Произошла ошибка:\n ссылка автоматического изменения языка отсутствует.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, открылась ли форма с переводом
		/// </summary>
		public SearchPage AsssertTranslationFormAppear()
		{
			CustomTestContext.WriteLine("Проверить, открылась ли форма с переводом.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(TRANSLATION_FORM)),
				"Произошла ошибка:\n форма с переводом не открылась.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, появился ли перевод по словам
		/// </summary>
		/// <returns>появился</returns>
		public SearchPage AssertWordByWordTranslationAppear()
		{
			CustomTestContext.WriteLine("Проверить, появился ли перевод по словам.");

			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(WORD_BY_WORD_TRANSLATION)),
				"Произошла ошибка:\n перевод по словам не появился.");

			return GetPage();
		}

		/// <summary>
		/// Получить список названий глоссариев
		/// </summary>
		public List<string> GlossaryNamesList()
		{
			CustomTestContext.WriteLine("Получить список названий глоссариев.");
			var glossaries = Driver.GetTextListElement(By.XPath(GLOSSARY_NAMES_LIST));

			return glossaries.Select(g => g.Substring(g.IndexOf('\n') + 1)).ToList();
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

		protected const string TRANSLATION_WORD = "//a[contains(@class,'js-show-examples')]//span[contains(@class,'translation') and contains(text(),'*#*')]";
		protected const string SEARCH_FORM_XPATH = "//form[contains(@class,'js-search-form')]";
		protected const string SEARCH_FIELD= "searchText";
		protected const string TRANSLATE_BUTTON = "//form[contains(@class,'js-search-form')]//span[contains(@class,'g-redbtn search')]//input";
		protected const string SEARCH_RESULT = "//div[contains(@class,'js-search-results')]";

		protected const string SOURCE_LANGUAGE_LIST = "SearchSrcLang";
		protected const string TARGET_LANGUAGE_LIST = "SearchDestLang";
		protected const string LANGUAGE_SOURCE_OPTION = "//select[@id='SearchSrcLang']//option[@value='*#*']";
		protected const string LANGUAGE_TARGET_OPTION = "//select[@id='SearchDestLang']//option[@value='*#*']";

		protected const string DEFINITION_TAB = "//li[contains(@data-search-mode,'Interpret')]";

		protected const string AUTOREVERSED_MESSAGE = "//div[contains(@class,'js-language-autoreversed')]";
		protected const string AUTOREVERSED_REFERENCE = "//div[contains(@class,'js-language-autoreversed')]//a[contains(@href,'/Translate')]";

		protected const string TRANSLATION_FORM= "//div[contains(@class,'js-window-examples-data')]";
		protected const string TRANSLATION_FORM_REFERENCE = "//div[contains(@class,'js-window-examples-data')]//a[contains(@class,'g-winexamp__reverse')]";
		protected const string WORD_BY_WORD_TRANSLATION = ".//div[contains(@class,'l-wordbyword')]";
		protected const string REVERSE_TRANSLATION_WORDS  = ".//div[contains(@class,'l-wordbyword')]//table//td//a[contains(@href,'Translate/ru/en')]";
		
		protected const string GLOSSARY_NAMES_LIST = "//div[contains(@class,'js-search-results')]//div[contains(@class,'l-glossary__data')]//h2";
		protected const string TERM_NAME = "//table[*#*]//td/table[contains(@class,'l-glossary__tblsrcword')]//td//span[contains(@class,'l-glossary__srcwordtxt')]";
	}
}
