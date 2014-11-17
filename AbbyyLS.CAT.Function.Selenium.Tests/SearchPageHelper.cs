using System.Collections.Generic;
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
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public SearchPageHelper(IWebDriver driver, WebDriverWait wait)
			: base(driver, wait)
		{
		}

		/// <summary>
		/// Дождаться загрузки страницы
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(SEARCH_FORM_XPATH));
		}

		/// <summary>
		/// Ввести текст для поиска
		/// </summary>
		/// <param name="text"></param>
		public void AddTextSearch(string text)
		{
			ClearAndAddText(By.Id(SEARCH_TEXT_ID), text);
		}

		/// <summary>
		/// Кликнуть Translate/Search
		/// </summary>
		public void ClickTranslateBtn()
		{
			ClickElement(By.XPath(SEARCH_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться появления результатов
		/// </summary>
		/// <returns>появились</returns>
		public bool WaitUntilShowResults()
		{
			return WaitUntilDisplayElement(By.XPath(SEARCH_RESULT_XPATH));
		}

		/// <summary>
		/// Выбрать Source английский
		/// </summary>
		public void SelectEnSourceLanguage()
		{
			ClickElement(By.Id(SOURCE_LANGUAGE_LIST_ID));
			ClickElement(By.XPath(SOURCE_LANGUAGE_LIST_XPATH + LANGUAGE_EN_OPTION));
			SendKeys.SendWait(@"{Enter}");
		}

		/// <summary>
		/// Выбрать Source русский
		/// </summary>
		public void SelectRuSourceLanguage()
		{
			ClickElement(By.Id(SOURCE_LANGUAGE_LIST_ID));
			ClickElement(By.XPath(SOURCE_LANGUAGE_LIST_XPATH + LANGUAGE_RU_OPTION));
			SendKeys.SendWait(@"{Enter}");
		}

		/// <summary>
		/// Выбрать Target английский
		/// </summary>
		public void SelectEnTargetLanguage()
		{
			ClickElement(By.Id(DESTINATION_LANGUAGE_LIST_ID));
			ClickElement(By.XPath(DESTINATION_LANGUAGE_LIST_XPATH + LANGUAGE_EN_OPTION));
			SendKeys.SendWait(@"{Enter}");
		}

		/// <summary>
		/// Дождаться появления результатов поиска
		/// </summary>
		/// <returns>появились</returns>
		public bool WaitSearchResult()
		{
			return WaitUntilDisplayElement(By.XPath(SEARCH_RESULT_XPATH));
		}

		/// <summary>
		/// Вернуть, активна ли вкладка Definitions
		/// </summary>
		/// <returns></returns>
		public bool GetIsDefinitionTabActive()
		{
			return GetElementClass(By.XPath(DEFINITION_TAB_XPATH)).Contains(TAB_ACTIVE_CLASS);
		}

		/// <summary>
		/// Кликнуть по переводу
		/// </summary>
		public void ClickTranslation()
		{
			ClickElement(By.XPath(TRANSLATION_REF_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли ссылка на перевод
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsTranslationRefExist()
		{
			return GetIsElementExist(By.XPath(TRANSLATION_REF_XPATH));
		}

		/// <summary>
		/// Вернуть, появилось ли сообщение об автоматическом изменении языка
		/// </summary>
		/// <returns>появилось</returns>
		public bool GetIsExistAutoreversedMessage()
		{
			return GetIsElementExist(By.XPath(AUTOREVERSED_MES_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли ссылка автоматического изменения языка
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsExistAutoreversedRef()
		{
			return GetIsElementExist(By.XPath(AUTOREVERSED_MES_XPATH + AUTOREVERSED_REF));
		}

		/// <summary>
		/// Дождаться открытия формы с переводом
		/// </summary>
		/// <returns></returns>
		public bool WaitUntilTranslationFormAppear()
		{
			return WaitUntilDisplayElement(By.XPath(TRANSLATION_FORM_XPATH));
		}

		/// <summary>
		/// Кликнуть по переводу в окне перевода
		/// </summary>
		public void ClickTranslationFormRef()
		{
			ClickElement(By.XPath(TRANSLATION_FORM_REF_XPATH));
		}

		/// <summary>
		/// Дождаться появления перевода по словам
		/// </summary>
		/// <returns>появился</returns>
		public bool WaitWordByWordTranslationAppear()
		{
			return WaitUntilDisplayElement(By.XPath(WORD_BY_WORD_TRANSLATION_XPATH));
		}

		/// <summary>
		/// Вернуть: является ли source - Русский
		/// </summary>
		/// <returns>является</returns>
		public bool GetIsSourceRussian()
		{
			return GetIsElementExist(By.XPath(SOURCE_LANGUAGE_LIST_XPATH + LANGUAGE_RU_OPTION + SELECTED_CLASS));
		}

		/// <summary>
		/// Вернуть, есть ли таблица с обратным переводом со ссылками
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsReverseTranslationListExist()
		{
			return GetIsElementExist(By.XPath(WORD_BY_WORD_TRANSLATION_XPATH + TRANSLATION_TABLE_REF_XPATH));
		}

		/// <summary>
		/// Вернуть, отображаются ли результаты поиска по словарю
		/// </summary>
		/// <returns>отображаются</returns>
		public bool GetIsDictionarySearchResultExist()
		{
			return GetIsElementExist(By.XPath(DICTIONARY_SEARCH_RESULT_XPATH));
		}

		/// <summary>
		/// Вернуть название словаря
		/// </summary>
		/// <returns>название</returns>
		public string GetDictionaryName()
		{
			return GetTextElement(By.XPath(DICTIONARY_SEARCH_RESULT_XPATH));
		}

		/// <summary>
		/// Вернуть названия найденных глоссариев
		/// </summary>
		/// <returns>названия глоссариев</returns>
		public List<string> GetGlossaryResultNames()
		{
			return GetTextListElement(By.XPath(GLOSSARY_RESULT_GLOSSARY_ITEM_XPATH));
		}

		/// <summary>
		/// Вернуть текст src найденного перевода глоссария
		/// </summary>
		/// <param name="resultNumber">номер результата</param>
		/// <returns>текст Src</returns>
		public string GetGlossaryResultSrcText(int resultNumber)
		{
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
		protected const string TRANSLATION_TABLE_REF_XPATH = "//table//td//a[contains(@href,'Translate/ru-en')]";

		protected const string DICTIONARY_SEARCH_RESULT_XPATH = SEARCH_RESULT_XPATH + "//div[contains(@class,'l-articles')]/div/span[2]";

		protected const string GLOSSARY_SEARCH_XPATH = SEARCH_RESULT_XPATH + "//div[contains(@class,'l-glossary__data')]";
		protected const string GLOSSARY_RESULT_GLOSSARY_ITEM_XPATH = GLOSSARY_SEARCH_XPATH + "//h2";
		protected const string GLOSSARY_RESULT_TEXT_TD = "//td/table[contains(@class,'l-glossary__tblsrcword')]//td//span[contains(@class,'l-glossary__srcwordtxt')]";
	}
}