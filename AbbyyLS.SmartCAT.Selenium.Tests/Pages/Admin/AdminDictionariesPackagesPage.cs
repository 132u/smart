using System.Linq;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminDictionariesPackagesPage : AdminLingvoProPage, IAbstractPage<AdminDictionariesPackagesPage>
	{
		public new AdminDictionariesPackagesPage GetPage()
		{
			var adminDictionariesPackagesPage = new AdminDictionariesPackagesPage();
			InitPage(adminDictionariesPackagesPage);

			return adminDictionariesPackagesPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(DICTIONARIES_TABLE_HEADER)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница пакетов словарей.");
			}
		}

		/// <summary>
		/// Проверить, существует ли требуемый пакет словарей в списке
		/// </summary>
		/// <param name="packageName">имя пакета</param>
		public bool DictionaryPackageExist(string packageName)
		{
			Logger.Trace("Проверить, что {0} пакет существует", packageName);

			return Driver.GetTextListElement(By.XPath(DICTIONARIES_PACKAGES_LIST))
				.Any(item => item == packageName);
		}

		/// <summary>
		/// Нажать по 'Создать новый пакет' в меню
		/// </summary>
		public AdminCreateDictionaryPackagePage ClickCreateDictionaryPackageLink()
		{
			Logger.Debug("Нажать по 'Создать новый пакет' в меню");
			Driver.FindElement(By.XPath(CREATE_DICTIONARY_PACKAGE_LINK)).Click();

			return new AdminCreateDictionaryPackagePage().GetPage();
		}

		/// <summary>
		/// Кликнуть по имени пакета
		/// </summary>
		/// <param name="packageName">имя пакета</param>
		public AdminDictionaryPackagePage ClickDictionaryPackageName(string packageName)
		{
			Logger.Debug("Кликнуть по имени пакета {0}.", packageName);
			isRequiredDictionaryLinkExist(packageName);

			var requiredDictionaryNameLink = Driver.GetElementList(By.XPath(DICTIONARIES_PACKAGES_LIST))
				.First(item => item.Text == packageName);
			requiredDictionaryNameLink.Click();

			return new AdminDictionaryPackagePage().GetPage();
		}

		/// <summary>
		/// Проверить, что ссылка на пакет словарей существует.
		/// </summary>
		/// <param name="packageName">имя пакета</param>
		public AdminDictionariesPackagesPage isRequiredDictionaryLinkExist(string packageName)
		{
			Logger.Trace("Проверить, что ссылка на {0} пакет словарей существует.", packageName);
			var package = Driver.GetElementList(By.XPath(DICTIONARIES_PACKAGES_LIST))
							.FirstOrDefault(item => item.Text == packageName);

			Assert.NotNull(package, "Произошла ошибка:\n {0} пакет не найден, вовзращено значение null.", packageName);

			Assert.IsTrue(package.Displayed, "Произошла ошибка:\n ссылка на {0} пакет словарей не существует", packageName);

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = CREATE_DICTIONARY_PACKAGE_LINK)]
		protected IWebElement CreateDictionaryPackageLink { get; set; }

		protected const string CREATE_DICTIONARY_PACKAGE_LINK = "//a[contains(@href, '/DictionariesPackages/New')]";
		protected const string DICTIONARIES_TABLE = "//table[contains(@class, 'js-sortable-table__activated')]";
		protected const string DICTIONARIES_TABLE_HEADER = "//h2[contains(text(), 'Пакеты словарей')]";
		protected const string DICTIONARIES_PACKAGES_LIST = "//table[contains(@class, 'js-sortable-table__activated')]//tbody//tr//td//a";
	}
}
