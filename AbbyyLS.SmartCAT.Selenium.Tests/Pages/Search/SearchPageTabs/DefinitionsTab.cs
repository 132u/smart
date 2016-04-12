using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search.SearchPageTabs
{
	public class DefinitionsTab: SearchPage, IAbstractPage<DefinitionsTab>
	{
		public DefinitionsTab(WebDriver driver) : base(driver)
		{
		}

		public new DefinitionsTab LoadPage()
		{
			if (!IsSearchPageSwitchedToDefinitionsTab())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась вкладка 'Examples' страница поиска.");
			}

			return this;
		}

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, произошло ли переключение во вкладку 'Examples'.
		/// </summary>
		public bool IsSearchPageSwitchedToDefinitionsTab()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(TAB_LOCATION));
		}

		/// <summary>
		/// Проверить, что отобразился термин и соответствующее ему толкование.
		/// </summary>
		/// <param name="rowNumber">номер строки в таблице</param>
		/// <param name="term">термин</param>
		/// <param name="definition">толкование</param>
		public bool IsTermAndDefinitionInSearchResultCorrect(int rowNumber, string term, string definition)
		{
			CustomTestContext.WriteLine("Проверить, что отобразился термин и соответствующее ему толкование.");
			var termInTable = Driver.SetDynamicValue(How.XPath, TERM_IN_SEARCH_RESULT, rowNumber.ToString(), term);
			var definitionInTable = Driver.SetDynamicValue(How.XPath, DEFINITION_IN_SEARCH_RESULT, rowNumber.ToString(), definition);

			return termInTable.Displayed && definitionInTable.Displayed;
		}

		/// <summary>
		/// Проверить, корректность имени глоссария.
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public bool IsGlossaryNameCorrect(string glossaryName)
		{
			CustomTestContext.WriteLine("Проверить, корректность имени глоссария.");
			GlossaryNane = Driver.SetDynamicValue(How.XPath, GLOSSARY_NAME, glossaryName);

			return GlossaryNane.Displayed;
		}

		#endregion

		#region Объявление элементов страницы

		protected IWebElement GlossaryNane { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string TAB_LOCATION = "//li[contains(@class, 'active')]//a[contains(text(), 'Definitions')]//parent::li";
		protected const string TERM_IN_SEARCH_RESULT = "(//table[contains(@class, 'l-glossary__tblsrcword')])[*#*]//td//span[contains(text(), '*##*')]";
		protected const string DEFINITION_IN_SEARCH_RESULT = "(//table[contains(@class, 'l-glossary__tblsrcword')])[*#*]//td//span[contains(text(), '*##*')]";
		protected const string GLOSSARY_NAME = "//div[contains(@class, 'l-glossary__data')]//h2//span/following-sibling::text()[contains(.,'*#*')]//parent::h2";

		#endregion
	}
}
