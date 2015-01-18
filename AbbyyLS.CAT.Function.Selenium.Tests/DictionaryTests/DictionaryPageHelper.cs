using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

		/// <summary>
		/// Вернуть список словарей на странице
		/// </summary>
		public List<string> GetDictionaryList()
		{
			var dictionaryLinks = GetElementList(
				By.XPath(DICTIONARY_LIST_LINKS_XPATH))
				.Select(item => item.GetAttribute("href"))
				.ToList();

			return convertLinksToDictionaryNames(dictionaryLinks);
		}

		private static List<string> convertLinksToDictionaryNames(IEnumerable<string> links)
		{
			if (links.Any(item => item == null))
			{
				throw new ArgumentException("Для одного из словарей не найдена ссылка. Невозможно получить ID словаря.");
			}

			var regex = new Regex(@"\S*dictionary=");
			var dictionaryNames = new List<string>();
			
			foreach (var link in links)
			{
				var dictionaryNameWithHexValue = link.Replace(regex.Match(link).ToString(), string.Empty);
				dictionaryNames.Add(hexValueReplacer(dictionaryNameWithHexValue));
			}

			return dictionaryNames;
		}

		private static string hexValueReplacer(string stringToReplace)
		{
			return stringToReplace
				.Replace("%20", " ")
				.Replace("%28", "(")
				.Replace("%29", ")");
		}

		protected const string DICTIONARY_LIST_XPATH = ".//div[contains(@class,'js-dictionaries-search-result')]//div[contains(@class,'l-dctnrs__dict')]";
		protected const string DICTIONARY_LIST_LINKS_XPATH = "//div[contains(@class,'js-dictionaries-search-result')]//div[contains(@class,'l-dctnrs__dict')]//div[contains(@class, 'l-dctnrs__icondict')]//a";
	}
}