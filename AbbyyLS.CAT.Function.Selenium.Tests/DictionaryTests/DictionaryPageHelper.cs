using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы словарей
	/// </summary>
	public class DictionaryPageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public DictionaryPageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
		}

		/// <summary>
		/// Вернуть количество словарей на странице
		/// </summary>
		/// <returns>количество</returns>
		public int GetDictionaryListCount()
		{
			return GetElementList(By.XPath(DICTIONARY_LIST_XPATH)).Count;
		}

		protected const string DICTIONARY_LIST_XPATH = ".//div[contains(@class,'js-dictionaries-search-result')]//div[contains(@class,'l-dctnrs__dict')]";
	}
}