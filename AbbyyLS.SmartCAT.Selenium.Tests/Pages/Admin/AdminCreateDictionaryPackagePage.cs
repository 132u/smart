using System.Collections.Generic;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminCreateDictionaryPackagePage : AdminLingvoProPage, IAbstractPage<AdminCreateDictionaryPackagePage>
	{
		public new AdminCreateDictionaryPackagePage GetPage()
		{
			var adminCreateDictionaryPackagePage = new AdminCreateDictionaryPackagePage();
			InitPage(adminCreateDictionaryPackagePage);

			return adminCreateDictionaryPackagePage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(DICTIONARY_PACKAGE_NAME)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница создания пакета словарей.");
			}
		}

		/// <summary>
		/// Ввести имя пакета словарей
		/// </summary>
		/// <param name="packageName">имя пакета</param>
		public AdminCreateDictionaryPackagePage FillDictionaryPackageName(string packageName)
		{
			Logger.Debug("Ввести {0} в названии пакета словарей.", packageName);
			DictionaryPackageName.SetText(packageName);

			return GetPage();
		}

		/// <summary>
		/// Поставить галочку в чекбоксе 'Общедоступный пакет'
		/// </summary>
		public AdminCreateDictionaryPackagePage ClickPublicDictionaryCheckbox()
		{
			Logger.Debug("Поставить галочку в чекбоксе 'Общедоступный пакет'");
			PublicDictionaryCheckbox.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать словари для пакета
		/// </summary>
		public AdminCreateDictionaryPackagePage AddDictionariesToPackage(List<string> dictionariesList)
		{
			Logger.Debug("Выбрать словари для пакета.");
			dictionariesList.ForEach(item =>
			{
				DictionariesList.SelectOptionByText(item);
				AddDictionaryToPack.Click();
			});

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Создать пакет'
		/// </summary>
		public AdminCreateDictionaryPackagePage ClickCreateDictionaryPack()
		{
			Logger.Debug("Нажать кнопку 'Создать пакет'.");
			CreateDictionaryPackButton.Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = DICTIONARY_PACKAGE_NAME)]
		protected IWebElement DictionaryPackageName { get; set; }

		[FindsBy(How = How.XPath, Using = PUBLIC_DICTIONARY_CHECKBOX)]
		protected IWebElement PublicDictionaryCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_DICTIONARY_TO_PACK_BUTTON)]
		protected IWebElement AddDictionaryToPack { get; set; }

		[FindsBy(How = How.XPath, Using = DICTIONARIES_LIST)]
		protected IWebElement DictionariesList { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_DICTIONARY_PACK_BUTTON)]
		protected IWebElement CreateDictionaryPackButton { get; set; }

		protected const string ADD_DICTIONARY_TO_PACK_BUTTON = "//input[@name='toRight']";
		protected const string PUBLIC_DICTIONARY_CHECKBOX = "//input[@id='isPublic']";
		protected const string DICTIONARY_PACKAGE_NAME = "//input[@id='packageSystemName']";
		protected const string DICTIONARIES_LIST = "//select[@id='left']";
		protected const string CREATE_DICTIONARY_PACK_BUTTON = "//input[@data-ref='frmCreatePackage']";
	}
}
