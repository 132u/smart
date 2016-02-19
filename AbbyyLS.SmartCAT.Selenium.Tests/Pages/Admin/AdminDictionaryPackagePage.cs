using System;
using System.Collections.Generic;

using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminDictionaryPackagePage : AdminLingvoProPage, IAbstractPage<AdminDictionaryPackagePage>
	{
		public AdminDictionaryPackagePage(WebDriver driver) : base(driver)
		{
		}

		public new AdminDictionaryPackagePage GetPage()
		{
			var adminDictionaryPackagePage = new AdminDictionaryPackagePage(Driver);
			InitPage(adminDictionaryPackagePage, Driver);

			return adminDictionaryPackagePage;
		}

		public new void LoadPage()
		{
			if (!IsAdminDictionaryPackagePageOpened())
			{
				throw new Exception("Произошла ошибка:\n не загрузилась страница пакета словарей.");
			}
		}

		#region Простые методы

		/// <summary>
		/// Получить список словарей, включенных в пакет
		/// </summary>
		/// <param name="packageName">имя пакета</param>
		public List<String> GetIncludedDictionariesList()
		{
			CustomTestContext.WriteLine("Получить список словарей, включенных в пакет");

			return Driver.GetTextListElement(By.XPath(INCLUDED_DICTIONARIES_LIST));
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница пакета словарей
		/// </summary>
		public bool IsAdminDictionaryPackagePageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(INCLUDED_DICTIONARIES_TABLE));
		}

		#endregion

		#region Описания XPath элементов

		protected const string INCLUDED_DICTIONARIES_LIST = "//table[contains(@name, 'dictionaries')]//select[contains(@id, 'right')]//option";
		protected const string INCLUDED_DICTIONARIES_TABLE = "//table[contains(@name, 'dictionaries')]//select[contains(@id, 'right')]";

		#endregion
	}
}
