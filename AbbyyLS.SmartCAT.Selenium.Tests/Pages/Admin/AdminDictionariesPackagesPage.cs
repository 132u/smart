using System;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminDictionariesPackagesPage : AdminLingvoProPage, IAbstractPage<AdminDictionariesPackagesPage>
	{
		public AdminDictionariesPackagesPage(WebDriver driver) : base(driver)
		{
		}

		public new AdminDictionariesPackagesPage LoadPage()
		{
			if (!IsAdminDictionariesPackagesPageOpened())
			{
				throw new Exception("Произошла ошибка:\n не загрузилась страница пакетов словарей.");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Нажать по 'Создать новый пакет' в меню
		/// </summary>
		public AdminCreateDictionaryPackagePage ClickCreateDictionaryPackageLink()
		{
			CustomTestContext.WriteLine("Нажать по 'Создать новый пакет' в меню");
			Driver.FindElement(By.XPath(CREATE_DICTIONARY_PACKAGE_LINK)).Click();

			return new AdminCreateDictionaryPackagePage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Кликнуть по имени пакета
		/// </summary>
		/// <param name="packageName">имя пакета</param>
		public AdminDictionaryPackagePage ClickDictionaryPackageName(string packageName)
		{
			CustomTestContext.WriteLine("Кликнуть по имени пакета {0}.", packageName);
			IsRequiredDictionaryLinkExist(packageName);

			var requiredDictionaryNameLink = Driver.GetElementList(By.XPath(DICTIONARIES_PACKAGES_LIST))
				.First(item => item.Text == packageName);
			requiredDictionaryNameLink.Click();

			return new AdminDictionaryPackagePage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница пакетов словарей
		/// </summary>
		public bool IsAdminDictionariesPackagesPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DICTIONARIES_TABLE_HEADER));
		}

		/// <summary>
		/// Проверить, существует ли требуемый пакет словарей в списке
		/// </summary>
		/// <param name="packageName">имя пакета</param>
		public bool IsDictionaryPackageExist(string packageName)
		{
			CustomTestContext.WriteLine("Проверить, что {0} пакет существует", packageName);

			return Driver.GetTextListElement(By.XPath(DICTIONARIES_PACKAGES_LIST))
				.Any(item => item == packageName);
		}

		/// <summary>
		/// Проверить, что ссылка на пакет словарей существует.
		/// </summary>
		/// <param name="packageName">имя пакета</param>
		public AdminDictionariesPackagesPage IsRequiredDictionaryLinkExist(string packageName)
		{
			CustomTestContext.WriteLine("Проверить, что ссылка на {0} пакет словарей существует.", packageName);
			var package = Driver.GetElementList(By.XPath(DICTIONARIES_PACKAGES_LIST))
							.FirstOrDefault(item => item.Text == packageName);

			if (package == null)
			{
				throw new Exception(string.Format(
					"Произошла ошибка:\n {0} пакет не найден, вовзращено значение null.", packageName));
			}

			if (!package.Displayed)
			{
				throw new Exception(string.Format(
					"Произошла ошибка:\n ссылка на {0} пакет словарей не существует", packageName));
			}
			
			return LoadPage();
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CREATE_DICTIONARY_PACKAGE_LINK)]
		protected IWebElement CreateDictionaryPackageLink { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CREATE_DICTIONARY_PACKAGE_LINK = "//a[contains(@href, '/DictionariesPackages/New')]";
		protected const string DICTIONARIES_TABLE = "//table[contains(@class, 'js-sortable-table__activated')]";
		protected const string DICTIONARIES_TABLE_HEADER = "//h2[contains(text(), 'Пакеты словарей')]";
		protected const string DICTIONARIES_PACKAGES_LIST = "//table[contains(@class, 'js-sortable-table__activated')]//tbody//tr//td//a";

		#endregion
	}
}
