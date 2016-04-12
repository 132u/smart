using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search
{
	class SearchHistoryPage : SearchPage, IAbstractPage<SearchHistoryPage>
	{
		public SearchHistoryPage(WebDriver driver): base(driver)
		{
		}

		public new SearchHistoryPage LoadPage()
		{
			if (!IsSearchHistoryPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась вкладка 'Translations' страница поиска.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на кнопку вкл / выкл записи поисковых запросов.
		/// </summary>
		public SearchPage ClickOnSearchHistoryButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку вкл / выкл записи поисковых запросов.");
			HistoryOnOffButton.Click();

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, включена ли запись истории поисковых запросов.
		/// </summary>
		public bool IsSearchHistoryButtonOn()
		{
			CustomTestContext.WriteLine("Проверить, включена ли запись истории поисковых запросов.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(HISTORY_BUTTON_ON_POSITION));
		}

		/// <summary>
		/// Проверить, произошло ли переключение на страницу истории поиска.
		/// </summary>
		public bool IsSearchHistoryPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(HISTORY_ON_OFF_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = HISTORY_ON_OFF_BUTTON)]
		protected IWebElement HistoryOnOffButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string HISTORY_BUTTON_ON_POSITION = "//div[contains(@class, 'l-searchHistory__settings')]//img[contains(@alt, 'On')]";
		protected const string HISTORY_ON_OFF_BUTTON = "//div[contains(@class, 'l-searchHistory__settings')]//span[contains(@class, 'l-searchHistory__onOff')]//a";

		#endregion
	}
}