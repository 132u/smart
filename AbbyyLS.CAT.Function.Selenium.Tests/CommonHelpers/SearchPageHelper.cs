using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Windows.Forms;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы поиска
	/// </summary>
	public class SearchPageHelper : CommonHelper
	{
		public SearchPageHelper(IWebDriver driver, WebDriverWait wait)
			: base(driver, wait)
		{
		}

		public void WaitPageLoad()
		{
			Logger.Debug("Ожидание загрузки страницы поиска");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(SEARCH_FORM_XPATH)),
				"Ошибка: страница поиска не загрузилась");
		}

		public void AddTextSearch(string text)
		{
			Logger.Debug(string.Format("Ввести текст {0} для поиска", text));
			ClearAndAddText(By.Id(SEARCH_TEXT_ID), text);
		}

		public void ClickTranslateBtn()
		{
			Logger.Debug("Нажать кнопку Translate/Search");
			ClickElement(By.XPath(SEARCH_BTN_XPATH));
		}

		public void WaitUntilShowResults()
		{
			Logger.Debug("Проверка успешного появления результаов");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(SEARCH_RESULT_XPATH)),
				"Ошибка: результаты не появились");
		}

		/// <summary>
		/// Вернуть названия найденных глоссариев
		/// </summary>
		/// <returns>названия глоссариев</returns>
		public List<string> GetGlossaryResultNames()
		{
			return GetTextListElement(By.XPath(GLOSSARY_RESULT_GLOSSARY_ITEM_XPATH));
		}

		public string GetGlossaryResultSrcText(int resultNumber)
		{
			Logger.Debug(string.Format("Вернуть текст src найденного перевода глоссария. Номер результата: {0}", resultNumber));
			return GetTextElement(By.XPath("//table[" + resultNumber + "]" + GLOSSARY_RESULT_TEXT_TD));
		}

		protected const string SEARCH_FORM_XPATH = "//form[contains(@class,'js-search-form')]";
		protected const string SEARCH_TEXT_ID = "searchText";
		protected const string SEARCH_BTN_XPATH = SEARCH_FORM_XPATH + "//span[contains(@class,'g-redbtn search')]//input";
		protected const string SEARCH_RESULT_XPATH = "//div[contains(@class,'js-search-results')]";

		protected const string SOURCE_LANGUAGE_LIST_ID = "SearchSrcLang";
		protected const string DESTINATION_LANGUAGE_LIST_ID = "SearchDestLang";
		protected const string SOURCE_LANGUAGE_LIST_XPATH = ".//select[@id='" + SOURCE_LANGUAGE_LIST_ID + "']";
		protected const string DESTINATION_LANGUAGE_LIST_XPATH = ".//select[@id='" + DESTINATION_LANGUAGE_LIST_ID + "']";
		protected const string LANGUAGE_EN_OPTION = "//option[@value='en']";
		protected const string LANGUAGE_RU_OPTION = "//option[@value='ru']";
		protected const string SELECTED_CLASS = "[@selected='selected']";

		protected const string DEFINITION_TAB_XPATH = "//li[contains(@data-search-mode,'Interpret')]";
		protected const string TAB_ACTIVE_CLASS = "active";

		protected const string TRANSLATION_REF_XPATH = "//a[contains(@class,'js-show-examples')]//span[contains(@class,'translation')]";
		protected const string AUTOREVERSED_MES_XPATH = "//div[contains(@class,'js-language-autoreversed')]";
		protected const string AUTOREVERSED_REF = "//a[contains(@href,'/Translate')]";

		protected const string TRANSLATION_FORM_XPATH = "//div[contains(@class,'js-window-examples-data')]";
		protected const string TRANSLATION_FORM_REF_XPATH = TRANSLATION_FORM_XPATH + "//a[contains(@class,'g-winexamp__reverse')]";
		protected const string WORD_BY_WORD_TRANSLATION_XPATH = ".//div[contains(@class,'l-wordbyword')]";
		protected const string TRANSLATION_TABLE_REF_XPATH = "//table//td//a[contains(@href,'Translate/ru/en')]";

		protected const string DICTIONARY_SEARCH_RESULT_XPATH = SEARCH_RESULT_XPATH + "//div[contains(@class,'l-articles')]/div/span[2]";

		protected const string GLOSSARY_SEARCH_XPATH = SEARCH_RESULT_XPATH + "//div[contains(@class,'l-glossary__data')]";
		protected const string GLOSSARY_RESULT_GLOSSARY_ITEM_XPATH = GLOSSARY_SEARCH_XPATH + "//h2";
		protected const string GLOSSARY_RESULT_TEXT_TD = "//td/table[contains(@class,'l-glossary__tblsrcword')]//td//span[contains(@class,'l-glossary__srcwordtxt')]";
	}
}