using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search.SearchPageTabs
{
	public class PhrasesTab : SearchPage, IAbstractPage<PhrasesTab>
	{
		public PhrasesTab(WebDriver driver): base(driver)
		{
		}

		public new PhrasesTab LoadPage()
		{
			if (!IsSearchPageSwitchedToPhrasesTab())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась вкладка 'Phrases' страница поиска.");
			}

			return this;
		}

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, произошло ли переключение во вкладку 'Phrases'.
		/// </summary>
		public bool IsSearchPageSwitchedToPhrasesTab()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(TAB_LOCATION));
		}

		/// <summary>
		/// Проверить, что появилось сообщение о реверсе языков перевода.
		/// </summary>
		public bool IsReverseLanguageMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение о реверсе языков перевода.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(MESSAGE_ABOOUT_REVERSE_LANGUAGES));
		}

		/// <summary>
		/// Проверить, что отобразились результаты поиска.
		/// </summary>
		public bool IsTableWithSearchResultDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отобразились результаты поиска.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(TABLE_WITH_SEARCH_RESULT));
		}

		#endregion

		#region Объявление элементов страницы

		#endregion

		#region Описание XPath элементов

		protected const string TAB_LOCATION = "//li[contains(@class, 'active')]//a[contains(text(), 'Phrases')]//parent::li";
		protected const string MESSAGE_ABOOUT_REVERSE_LANGUAGES = "//div[contains(@class, 'js-language-autoreversed')]//span[contains(@class, 'js-warning-text')]//a/following-sibling::text()[contains(.,'has been reversed.')]//parent::span";
		protected const string TABLE_WITH_SEARCH_RESULT = "//table[contains(@class, 'g-tblresult')]";

		#endregion
	}
}
