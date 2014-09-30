using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.CAT.Function.Selenium.Tests
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