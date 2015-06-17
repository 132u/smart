using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminDictionaryPackagePage : AdminLingvoProPage, IAbstractPage<AdminDictionaryPackagePage>
	{
		public new AdminDictionaryPackagePage GetPage()
		{
			var adminDictionaryPackagePage = new AdminDictionaryPackagePage();
			InitPage(adminDictionaryPackagePage);

			return adminDictionaryPackagePage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(INCLUDED_DICTIONARIES_TABLE)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница пакета словарей.");
			}
		}

		/// <summary>
		/// Получить список словарей, включенных в пакет
		/// </summary>
		/// <param name="packageName">имя пакета</param>
		public List<String> IncludedDictionariesList()
		{
			Logger.Trace("Получить список словарей, включенных в пакет");

			return Driver.GetTextListElement(By.XPath(INCLUDED_DICTIONARIES_LIST));
		}

		protected const string INCLUDED_DICTIONARIES_LIST = "//table[contains(@name, 'dictionaries')]//select[contains(@id, 'right')]//option";
		protected const string INCLUDED_DICTIONARIES_TABLE = "//table[contains(@name, 'dictionaries')]//select[contains(@id, 'right')]";
	}
}
