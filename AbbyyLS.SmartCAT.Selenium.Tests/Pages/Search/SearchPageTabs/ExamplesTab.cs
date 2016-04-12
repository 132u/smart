using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search.SearchPageTabs
{
	public class ExamplesTab : SearchPage, IAbstractPage<ExamplesTab>
	{
		public ExamplesTab(WebDriver driver) : base(driver)
		{
		}

		public new ExamplesTab LoadPage()
		{
			if (!IsSearchPageSwitchedToExamplesTab())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась вкладка 'Examples' страница поиска.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Открыть поле для расширенного поиска.
		/// </summary>
		public ExamplesTab ClickOnAdvancedSearchButton()
		{
			CustomTestContext.WriteLine("Открыть поле для расширенного поиска.");
			AdvancedSearch.Click();

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле для расширенного поиска.
		/// </summary>
		/// <param name="text">текст</param>
		public ExamplesTab FillAdvancedSearchField(string text)
		{
			CustomTestContext.WriteLine("Заполнить поле для расширенного поиска.");
			AdvancedSearchField.SetText(text);

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Инициализировать расширенный поиск.
		/// </summary>
		/// <param name="source">исходная фраза</param>
		/// <param name="targetPartial">часть перевода</param>
		/// <param name="sourceLanguage">исходный язык</param>
		/// <param name="targetLanguage">язык для перевода</param>
		public ExamplesTab InitAdvancedSearch(
			string source,
			string targetPartial,
			string sourceLanguage = "en",
			string targetLanguage = "ru")
		{
			CustomTestContext.WriteLine("Инициализировать расширенный поиск.");
			AddTextSearch(source);
			ClickOnAdvancedSearchButton();
			FillAdvancedSearchField(targetPartial);
			SelectSourceLanguage(sourceLanguage);
			SelectTargetLanguage(targetLanguage);
			ClickTranslateButton();

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, произошло ли переключение во вкладку 'Examples'.
		/// </summary>
		public bool IsSearchPageSwitchedToExamplesTab()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(TAB_LOCATION));
		}

		/// <summary>
		/// Проверить, отобразился ли заданный термин в результатах поиска.
		/// </summary>
		/// <param name="term">термин</param>
		public bool IsSerchedTermDisplayed(string term)
		{
			CustomTestContext.WriteLine("Проверить, отобразился ли заданный термин в результатах поиска.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(SEARCH_TERM.Replace("*#*", term)));
		}

		/// <summary>
		/// Проверить, что текст в сорс совпадает с заданным в поиске.
		/// </summary>
		/// <param name="rowNumber">номер строки в таблице</param>
		/// <param name="source">исходное предложение</param>
		public bool IsSourceInSearchResultCorrect(int rowNumber, string source)
		{
			CustomTestContext.WriteLine("Проверить, что текст в сорс совпадает с заданным в поиске.");
			SourcElement = Driver.SetDynamicValue(How.XPath, SOURCE_IN_RESULT, rowNumber.ToString(), source);

			return SourcElement.Displayed;
		}

		/// <summary>
		/// Проверить, что текст в таргете совпадает с заданным в поиске.
		/// </summary>
		/// <param name="rowNumber">номер строки в таблице</param>
		/// <param name="leftTargetPart">левая часть таргета</param>
		/// <param name="rightTargetPart">правая часть таргета</param>
		public bool IsTargetInSearchResultCorrect(int rowNumber, string leftTargetPart, string rightTargetPart)
		{
			CustomTestContext.WriteLine("Проверить, что текст в таргете совпадает с заданным в поиске.");
			TargetElement = Driver.SetDynamicValue(How.XPath, TARGET_IN_RESULT, rowNumber.ToString(), leftTargetPart, rightTargetPart);

			return TargetElement.Displayed;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = ADVANCED_SEARCH)]
		protected IWebElement AdvancedSearch { get; set; }

		[FindsBy(How = How.Id, Using = ADVANCED_SEARCH_FIELD)]
		protected IWebElement AdvancedSearchField { get; set; }

		protected IWebElement TargetElement { get; set; }

		protected IWebElement SourcElement { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string ADVANCED_SEARCH_FIELD = "translation";
		protected const string SEARCH_TERM = "//div[contains(@class, 'l-examples__text js-first-source-text')]//em[contains(text(), '*#*')]";
		protected const string ADVANCED_SEARCH = "//div[contains(@class, 'g-srcpanel__long js-srcpanel-long')]//a[contains(@class, 'g-srcpanel__typesrc advanced')]";
		protected const string TAB_LOCATION = "//li[contains(@class, 'active')]//a[contains(text(), 'Examples')]//parent::li";
		protected const string SOURCE_IN_RESULT = "//table[contains(@class, 'js-tmtable')]//tr[*#*]//td[1]//em[contains(text(), '*##*')]";
		protected const string TARGET_IN_RESULT = "//table[contains(@class, 'js-tmtable')]//tr[*#*]//td[2]//div[contains(text(), '*##*')]//em[contains(text(), '*###*')]";

		#endregion
	}
}
