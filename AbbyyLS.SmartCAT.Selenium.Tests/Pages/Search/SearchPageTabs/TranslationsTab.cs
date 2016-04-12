using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search.SearchPageTabs
{
	public class TranslationsTab : SearchPage, IAbstractPage<TranslationsTab>
	{
		public TranslationsTab(WebDriver driver): base(driver)
		{
		}

		public new TranslationsTab LoadPage()
		{
			if (!IsSearchPageSwitchedToTranslationsTab())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась вкладка 'Translations' страница поиска.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Перейти на страницу глоссария.
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public GlossaryPage ClickOnGoToTheGlossaryLink(string glossaryName)
		{
			CustomTestContext.WriteLine("Перейти на страницу глоссария.");
			Driver.WaitUntilElementIsDisplay(By.XPath(GO_TO_THE_GLOSSARY.Replace("*#*", glossaryName)));
			GoToTheGlossary = Driver.SetDynamicValue(How.XPath, GO_TO_THE_GLOSSARY, glossaryName);
			GoToTheGlossary.Click();

			return new GlossaryPage(Driver).LoadPage();
		}

		/// <summary>
		/// Кликнуть на кнопку 'SuggestTerm'.
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public SuggestTermDialog ClickOnSuggestTermButton(string glossaryName)
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку 'SuggestTerm'.");
			SuggestTermButton = Driver.SetDynamicValue(How.XPath, SUGGEST_TERM_BUTTON, glossaryName);
			SuggestTermButton.Click();

			return new SuggestTermDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Кликнуть на кнопку 'Add Entry'.
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public GlossaryPage ClickAddEntryButton(string glossaryName)
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку 'Add Entry'.");
			AddEntryButton = Driver.SetDynamicValue(How.XPath, ADD_ENTRY_BUTTON, glossaryName);
			AddEntryButton.Click();

			return new GlossaryPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, произошло ли переключение во вкладку 'Translations'.
		/// </summary>
		public bool IsSearchPageSwitchedToTranslationsTab()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(TRANSLATIONS_TAB));
		}

		#endregion

		#region Объявление элементов страницы

		protected IWebElement GoToTheGlossary { get; set; }

		protected IWebElement SuggestTermButton { get; set; }

		protected IWebElement AddEntryButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string GO_TO_THE_GLOSSARY = "//div[contains(@class, 'l-glossary__data js-section-data')]//h2//span/following-sibling::text()[contains(.,'*#*')]//parent::h2//span//a[contains(text(), 'Go to the glossary')]";
		protected const string SUGGEST_TERM_BUTTON = "//div[contains(@class, 'l-glossary__data js-section-data')]//h2//span/following-sibling::text()[contains(.,'*#*')]//parent::h2//span//a[contains(text(), 'Suggest Term')]";
		protected const string ADD_ENTRY_BUTTON = "//div[contains(@class, 'l-glossary__data js-section-data')]//h2//span/following-sibling::text()[contains(.,'*#*')]//parent::h2//span//a[contains(text(), 'Add Entry')]";

		#endregion
	}
}
