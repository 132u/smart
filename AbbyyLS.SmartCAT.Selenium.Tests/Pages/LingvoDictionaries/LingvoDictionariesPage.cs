﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.LingvoDictionaries
{
	public class LingvoDictionariesPage : WorkspacePage, IAbstractPage<LingvoDictionariesPage>
	{
		public LingvoDictionariesPage(WebDriver driver) : base(driver)
		{
		}

		public new LingvoDictionariesPage GetPage()
		{
			var dictionariesPage = new LingvoDictionariesPage(Driver);
			InitPage(dictionariesPage, Driver);

			return dictionariesPage;
		}

		public new void LoadPage()
		{
			if (!IsLingvoDictionariesPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница 'Словари Lingvo'");
			}
		}

		/// <summary>
		/// Вернуть список словарей на странице
		/// </summary>
		public List<string> GetDictionariesList()
		{
			CustomTestContext.WriteLine("Получить список словарей на странице");
			var dictionaryLinks = Driver.GetElementList(By.XPath(DICTIONARY_LIST_LINKS))
				.Select(item => item.GetElementAttribute("href"))
				.ToList();

			return convertLinksToDictionaryNames(dictionaryLinks);
		}

		/// <summary>
		/// Проверить, открыта ли страница 'Словари Lingvo'
		/// </summary>
		public bool IsLingvoDictionariesPageOpened()
		{
			CustomTestContext.WriteLine("Проверить, открыта ли страница 'Словари Lingvo'");

			return Driver.WaitUntilElementIsDisplay(By.XPath(DICTIONARY_SEARCH_FIELD));
		}

		/// <summary>
		/// Проверить, что список словарей не пуст
		/// </summary>
		public bool IsLingvoDictionariesListNotEmpty()
		{
			CustomTestContext.WriteLine("Проверить, что список словарей не пуст.");

			return getDictionaryListCount() > 0;
		}

		/// <summary>
		/// Сравнить списки словарей
		/// </summary>
		/// <param name="expectedList">ожидаемый список</param>
		public bool IsLingvoDictionariesListMatchExpected(List<string> expectedList)
		{
			CustomTestContext.WriteLine("Сравнить списки словарей");
			var actualList = GetDictionariesList();

			return actualList.OrderBy(t => t).SequenceEqual(expectedList.OrderBy(t => t));
		}

		/// <summary>
		/// Вернуть количество словарей на странице
		/// </summary>
		private int getDictionaryListCount()
		{
			CustomTestContext.WriteLine("Получить количество словарей на странице");
			var dictionaryListCount = Driver.GetElementList(By.XPath(DICTIONARY_LIST)).Count;
			CustomTestContext.WriteLine("Количество словарей на странице - {0}", dictionaryListCount);

			return dictionaryListCount;
		}

		/// <summary>
		/// Сконвертировать список ссылок в имена словарей
		/// </summary>
		/// <param name="links">список ссылок</param>
		/// <returns>спсиок имен словарей</returns>
		private static List<string> convertLinksToDictionaryNames(IList<string> links)
		{
			CustomTestContext.WriteLine("Сконвертировать список ссылок в имена словарей");
			if (links.Any(item => item == null))
			{
				throw new ArgumentException("Для одного из словарей не найдена ссылка. Невозможно получить ID словаря.");
			}

			var regex = new Regex(@"\S*dictionary=");
			var dictionaryNames = new List<string>();

			foreach (var link in links)
			{
				var dictionaryNameWithHexValue = link.Replace(regex.Match(link).ToString(), string.Empty);
				dictionaryNames.Add(replaceHexValue(dictionaryNameWithHexValue));
			}

			return dictionaryNames;
		}

		private static string replaceHexValue(string stringToReplace)
		{
			return stringToReplace
				.Replace("%20", " ")
				.Replace("%28", "(")
				.Replace("%29", ")");
		}

		[FindsBy(How = How.XPath, Using = DICTIONARY_LIST)]
		protected IWebElement DictionaryList { get; set; }

		[FindsBy(How = How.XPath, Using = DICTIONARY_LIST_LINKS)]
		protected IWebElement DictionaryListLink { get; set; }

		[FindsBy(How = How.XPath, Using = DICTIONARY_SEARCH_FIELD)]
		protected IWebElement DictionarySearchField { get; set; }

		protected const string DICTIONARY_LIST = ".//div[contains(@class,'js-dictionaries-search-result')]//div[contains(@class,'l-dctnrs__dict')]";
		protected const string DICTIONARY_LIST_LINKS = "//div[contains(@class,'js-dictionaries-search-result')]//div[contains(@class,'l-dctnrs__dict')]//div[contains(@class, 'l-dctnrs__icondict')]//a";
		protected const string DICTIONARY_SEARCH_FIELD = "//input[contains(@data-watermark-text, 'dictionary')]";
	}
}