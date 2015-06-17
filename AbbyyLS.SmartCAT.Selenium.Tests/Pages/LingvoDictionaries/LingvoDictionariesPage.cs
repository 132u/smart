﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.LingvoDictionaries
{
	public class LingvoDictionariesPage : WorkspacePage, IAbstractPage<LingvoDictionariesPage>
	{
		public new LingvoDictionariesPage GetPage()
		{
			var dictionariesPage = new LingvoDictionariesPage();
			InitPage(dictionariesPage);

			return dictionariesPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(DICTIONARY_SEARCH_FIELD)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница 'Словари Lingvo'.");
			}
		}

		/// <summary>
		/// Проверить, что cловари из стандартного пакета и словари из SmartCat совпадают
		/// </summary>
		/// <param name="dictionariesList">ожидаемый список словарей</param>
		public LingvoDictionariesPage AssertDictionariesListsMatch(IList<string> dictionariesList)
		{
			Logger.Trace("Проверить, что cловари из стандартного пакета и словари из SmartCat совпадают.");
			var actualDictionariesList = getDictionariesList();

			Assert.IsTrue(
				dictionariesList.Count() == actualDictionariesList.Count()
				&& !dictionariesList.Except(actualDictionariesList).Any(),
				"Произошла ошибка:\n Словари из стандартного пакета и словари из SmartCat не совпадают.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что список словарей не пуст
		/// </summary>
		public LingvoDictionariesPage AssertLingvoDictionariesListIsNotEmpty()
		{
			Logger.Trace("Проверить, что список словарей не пуст.");

			Assert.IsTrue(getDictionaryListCount() > 0,
				"Произошла ошибка:\n список словарей пуст.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что список словарей пуст
		/// </summary>
		public LingvoDictionariesPage AssertLingvoDictionariesListIsEmpty()
		{
			Logger.Trace("Проверить, что список словарей пуст");

			Assert.IsTrue(getDictionaryListCount() == 0,
				"Произошла ошибка:\n список словарей не пуст.");

			return GetPage();
		}

		/// <summary>
		/// Сконвертировать список ссылок в имена словарей
		/// </summary>
		/// <param name="links">список ссылок</param>
		/// <returns>спсиок имен словарей</returns>
		private static List<string> convertLinksToDictionaryNames(IList<string> links)
		{
			Logger.Trace("Сконвертировать список ссылок в имена словарей");
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

		/// <summary>
		/// Вернуть список словарей на странице
		/// </summary>
		private List<string> getDictionariesList()
		{
			Logger.Trace("Получить список словарей на странице");
			var dictionaryLinks = Driver.GetElementList(By.XPath(DICTIONARY_LIST_LINKS))
				.Select(item => item.GetElementAttribute("href"))
				.ToList();

			return convertLinksToDictionaryNames(dictionaryLinks);
		}

		/// <summary>
		/// Вернуть количество словарей на странице
		/// </summary>
		private int getDictionaryListCount()
		{
			Logger.Trace("Получить количество словарей на странице.");

			return Driver.GetElementList(By.XPath(DICTIONARY_LIST)).Count;
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