using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class SuggestTermDialog : WorkspacePage, IAbstractPage<SuggestTermDialog>
	{
		public SuggestTermDialog(WebDriver driver) : base(driver)
		{
		}

		public new SuggestTermDialog LoadPage()
		{
			if (!IsSuggestTermDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\nНе открылся диалог создания терминов");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Заполнить название термина
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		/// <param name="term">термин</param>
		public SuggestTermDialog FillTerm(int termNumber, string term)
		{
			CustomTestContext.WriteLine("Ввести {0} в термин №{1}.", term, termNumber);
			Driver.SetDynamicValue(How.XPath, TERM_INPUT, termNumber.ToString()).SetText(term);

			return LoadPage();
		}

		/// <summary>
		/// Нажать на выпадающий список глоссариев
		/// </summary>
		public SuggestTermDialog ClickGlossariesDropdown()
		{
			CustomTestContext.WriteLine("Нажать на выпадающий список глоссариев.");
			GlossaryDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать глоссарий в дропдауне
		/// </summary>
		/// <param name="glossary">название глоссария</param>
		public SuggestTermDialog SelectGlossariesInDropdown(string glossary)
		{
			CustomTestContext.WriteLine("Выбрать глоссарий {0} в дропдауне.", glossary);
			Driver.SetDynamicValue(How.XPath, GLOSSARU_IN_LIST, glossary).Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Save, ожидая открытия страницы глоссария
		/// </summary>
		public GlossaryPage ClickSaveButtonExpectingGlossaryPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save, ожидая открытия страницы глоссария");
			SaveButon.Click();

			return new GlossaryPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Save, ожидая открытия страницы со списком глоссариев
		/// </summary>
		public GlossariesPage ClickSaveButtonExpectingGlossariesPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save, ожидая открытия страницы со списком глоссариев");
			SaveButon.Click();

			return new GlossariesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Save, ожидая сообщение об ошибке
		/// </summary>
		public SuggestTermDialog ClickSaveButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save, ожидая сообщение об ошибке");
			SaveButon.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на выпадающий список языков
		/// </summary>
		/// <param name="languageNumber">номер языка</param>
		public SuggestTermDialog ClickLanguageList(int languageNumber)
		{
			CustomTestContext.WriteLine("Нажать на выпадающий список языков №{0}.", languageNumber);
			Driver.SetDynamicValue(How.XPath, LANGUAGE_DROPDOWN_ARROW, languageNumber.ToString()).Click();

			return LoadPage();
		}

		/// <summary>
		/// Вернуть название языка, установленного для термина
		/// </summary>
		public string GetLanguageText(int languageNumber)
		{
			CustomTestContext.WriteLine("Вернуть название языка, установленного для термина №{0}.", languageNumber);

			return Driver.SetDynamicValue(How.XPath, LANGUAGE, languageNumber.ToString()).Text.Trim();
		}

		/// <summary>
		/// Выбрать язык
		/// </summary>
		public SuggestTermDialog SelectLanguageInList(Language language)
		{
			CustomTestContext.WriteLine("Выбрать язык {0}.", language);
			Driver.SetDynamicValue(How.XPath, LANGUAGE_OPTION, language.ToString()).Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel, ожидая открытия страницы со списком глоссариев
		/// </summary>
		public GlossariesPage ClickCancelButtonExpectingGlossariesPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel, ожидая открытия страницы со списком глоссариев");
			CancelButon.Click();

			return new GlossariesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel, ожидая открытия страницы глоссария
		/// </summary>
		public GlossaryPage ClickCancelButtonExpectingGlossaryPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel, ожидая открытия страницы глоссария");
			CancelButon.Click();

			return new GlossaryPage(Driver).LoadPage();
		}
		
		/// <summary>
		/// Нажать кнопку 'Save term anyway', ожидая открытия страницы со списком глоссариев
		/// </summary>
		public GlossariesPage ClickSaveTermAnywayButtonExpectingGlossariesPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Save term anyway', ожидая открытия страницы со списком глоссариев");
			SaveTermAnywayButon.Click();

			return new GlossariesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Save term anyway', ожидая открытия страницы глоссария
		/// </summary>
		public GlossaryPage ClickSaveTermAnywayButtonExpectingGlossaryPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Save term anyway', ожидая открытия страницы глоссария");
			SaveTermAnywayButon.Click();

			return new GlossaryPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы

		public SuggestTermDialog FillSuggestTermDialog(
			string term1 = "term1",
			string term2 = "term2",
			Language language1 = Language.English,
			Language language2 = Language.Russian,
			bool defaultLanguages = false,
			string glossary = null)
		{
			FillTerm(termNumber: 1, term: term1);
			FillTerm(termNumber: 2, term: term2);

			if (!defaultLanguages)
			{
				ClickLanguageList(languageNumber: 1);
				SelectLanguageInList(language1);
				ClickLanguageList(languageNumber: 2);
				SelectLanguageInList(language2);
			}

			if (glossary != null)
			{
				ClickGlossariesDropdown();
				SelectGlossariesInDropdown(glossary);
			}

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылся ли диалог создания терминов
		/// </summary>
		public bool IsSuggestTermDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SUGGEST_TERM_DIALOG));
		}

		/// <summary>
		/// Проверить, что сообщение о том, что такой термин уже существует, появилось
		/// </summary>
		public bool IsDublicateErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение о том, что такой термин уже существует, появилось.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(DUPLICATE_ERROR));
		}

		/// <summary>
		/// Проверить, что сообщение 'Enter at least one term.' появилось
		/// </summary>
		public bool IsEmptyTermErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'Enter at least one term.' появилось.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(EMPTY_TERM_ERROR_MESSAGE));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = GLOSSARY_DROPDOWN)]
		protected IWebElement GlossaryDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTON)]
		protected IWebElement SaveButon { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButon { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_TERM_ANYWAY_BUTON)]
		protected IWebElement SaveTermAnywayButon { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string SUGGEST_TERM_DIALOG = "//div[contains(@class,'js-add-suggest-popup')]";
		protected const string TERM_INPUT = "//div[contains(@class, 'l-addsugg__contr lang js-language')][*#*]//input[contains(@class, 'js-addsugg-term')]";
		protected const string GLOSSARY_DROPDOWN = "//span[contains(@class, 'js-dropdown addsuggglos')]";
		protected const string GLOSSARU_IN_LIST = "//span[contains(@class,'js-dropdown__item')][@title='*#*']";
		protected const string SAVE_BUTON = "//input[contains(@class, 'js-save-btn')]";
		protected const string LANGUAGE_DROPDOWN_ARROW = "//div[contains(@class, 'l-addsugg__contr lang js-language')][*#*]//span[contains(@class,'js-dropdown__text addsugglang')]/../..//i[contains(@class, 'down-arrow')]";
		protected const string LANGUAGE_LIST = "//div[contains(@class, 'l-addsugg__contr lang js-language')][*#*]//span[contains(@class,'js-dropdown__text addsugglang')]/../..//i[contains(@class, 'down-arrow')]";
		protected const string LANGUAGE = "//div[contains(@class, 'l-addsugg__contr lang js-language')][*#*]//span[contains(@class,'js-dropdown__text addsugglang')]";
		protected const string LANGUAGE_OPTION = "//span[contains(@class,'js-dropdown__item')][@title='*#*']";
		protected const string CANCEL_BUTTON = "//div[contains(@class,'js-add-suggest-popup')]//a[contains(@class,'g-popupbox__cancel js-popup-close')]";
		protected const string DUPLICATE_ERROR = "//div[contains(@class,'js-duplicate-warning')]";
		protected const string SAVE_TERM_ANYWAY_BUTON = "//input[contains(@value, 'anyway')]";
		protected const string EMPTY_TERM_ERROR_MESSAGE = "//div[contains(@class,'js-add-suggest-popup')]//div[contains(@class,'js-error-message')]";

		#endregion
	}
}
