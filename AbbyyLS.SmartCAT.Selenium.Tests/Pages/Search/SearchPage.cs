﻿﻿using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search
{
	public class SearchPage : WorkspacePage, IAbstractPage<SearchPage>
	{
		public new SearchPage GetPage()
		{
			var searchPage = new SearchPage();
			InitPage(searchPage);

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
			Logger.Debug("Ввести {0} в поле поиска", text);
			SearchField.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Проверить, отображается ли таблица с обратным переводом со ссылками
		/// </summary>
		public SearchPage AssertReverseTranslationListExist()
		{
			Logger.Trace("Проверить, отображается ли таблица с обратным переводом со ссылками.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(REVERSE_TRANSLATION_WORDS)),
				"Произошла ошибка:\n таблица с обратным переводом со ссылками не отображается.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Translate
		/// </summary>
		public SearchPage ClickTranslateButton()
		{
			Logger.Debug("Нажать кнопку Translate.");
			TranslateButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по слову-переводу
		/// </summary>
		public SearchPage ClickTranslationWord()
		{
			Logger.Debug("Кликнуть по слову-переводу");
			TranslationWord.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появились результаты поиска.
		/// </summary>
		/// <returns></returns>
		public SearchPage AssertSearchResultDisplay()
		{
			Logger.Trace("Проверить, что появились результаты поиска.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(SEARCH_RESULT)),
				"Произошла ошибка:\n результаты поиска не появились.");

			return GetPage();
		}

		/// <summary>
		/// Выбрать исходный язык
		/// </summary>
		public SearchPage SelectSourceLanguage(string source)
		{
			Logger.Debug("Выбрать исходный язык {0}.", source);
			SourceLanguageList.Click();
			Driver.SetDynamicValue(How.XPath, LANGUAGE_SOURCE_OPTION, source).Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать язык перевода
		/// </summary>
		public SearchPage SelectTargetLanguage(string target)
		{
			Logger.Debug("Выбрать язык перевода{0}.", target);
			TargetLanguageList.Click();
			Driver.SetDynamicValue(How.XPath, LANGUAGE_TARGET_OPTION, target).Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, активна ли вкладка Definitions
		/// </summary>
		public SearchPage AssertDefinitionTabIsActive()
		{
			Logger.Trace("Проверить, активна ли вкладка Definitions.");

			Assert.IsTrue(DefinitionTab.GetElementAttribute("class").Contains("active"),
				"Произошла ошибка:\n вкладка Definitions неактивна.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, есть ли ссылка на перевод
		/// </summary>
		public SearchPage AssertTranslationReferenceExist()
		{
			Logger.Trace("Проверить, есть ли ссылка на перевод.");

			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(TRANSLATION_WORD)),
				"Произошла ошибка:\n ссылка на перевод отсутствует.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, появилось ли сообщение об автоматическом изменении языка
		/// </summary>
		public SearchPage AssertAutoreversedMessageExist()
		{
			Logger.Trace("Проверить, появилось ли сообщение об автоматическом изменении языка.");

			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(AUTOREVERSED_MESSAGE)),
				"Произошла ошибка:\n сообщение об автоматическом изменении языка не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, есть ли ссылка автоматического изменения языка
		/// </summary>
		public SearchPage AssertAutoreversedReferenceExist()
		{
			Logger.Trace("Проверить, есть ли ссылка автоматического изменения языка.");

			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(AUTOREVERSED_REFERENCE)),
				"Произошла ошибка:\n ссылка автоматического изменения языка отсутствует.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, открылась ли форма с переводом
		/// </summary>
		public SearchPage AsssertTranslationFormAppear()
		{
			Logger.Trace("Проверить, открылась ли форма с переводом.");

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
			Logger.Trace("Проверить, появился ли перевод по словам.");

			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(WORD_BY_WORD_TRANSLATION)),
				"Произошла ошибка:\n перевод по словам не появился.");

			return GetPage();
		}

		/// <summary>
		/// Нажать на перевод в окне перевода
		/// </summary>
		public SearchPage ClickTranslationFormReference()
		{
			Logger.Debug("Нажать на перевод в окне перевода.");
			TranslationFormReference.Click();

			return GetPage();
		}

		[FindsBy(How = How.Id, Using = SEARCH_FIELD)]
		protected IWebElement SearchField { get; set; }

		[FindsBy(How = How.Id, Using = SOURCE_LANGUAGE_LIST)]
		protected IWebElement SourceLanguageList { get; set; }

		[FindsBy(How = How.Id, Using = TARGET_LANGUAGE_LIST)]
		protected IWebElement TargetLanguageList { get; set; }

		[FindsBy(How = How.XPath, Using = TRANSLATION_FORM_REFERENCE)]
		protected IWebElement TranslationFormReference { get; set; }

		[FindsBy(How = How.XPath, Using = TRANSLATION_WORD)]
		protected IWebElement TranslationWord { get; set; }

		[FindsBy(How = How.XPath, Using = DEFINITION_TAB)]
		protected IWebElement DefinitionTab { get; set; }

		[FindsBy(How = How.XPath, Using = TRANSLATE_BUTTON)]
		protected IWebElement TranslateButton { get; set; }

		protected const string TRANSLATION_WORD = "//a[contains(@class,'js-show-examples')]//span[contains(@class,'translation')]";
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
	}
}