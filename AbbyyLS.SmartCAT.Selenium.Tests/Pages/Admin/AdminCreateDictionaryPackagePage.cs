﻿using System;
using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminCreateDictionaryPackagePage : AdminLingvoProPage, IAbstractPage<AdminCreateDictionaryPackagePage>
	{
		public AdminCreateDictionaryPackagePage(WebDriver driver) : base(driver)
		{
		}

		public new AdminCreateDictionaryPackagePage LoadPage()
		{
			if (!IsAdminCreateDictionaryPackagePageOpened())
			{
				throw new Exception("Произошла ошибка:\n не загрузилась страница создания пакета словарей.");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Ввести имя пакета словарей
		/// </summary>
		/// <param name="packageName">имя пакета</param>
		public AdminCreateDictionaryPackagePage FillDictionaryPackageName(string packageName)
		{
			CustomTestContext.WriteLine("Ввести {0} в названии пакета словарей.", packageName);
			DictionaryPackageName.SetText(packageName);

			return LoadPage();
		}

		/// <summary>
		/// Поставить галочку в чекбоксе 'Общедоступный пакет'
		/// </summary>
		public AdminCreateDictionaryPackagePage ClickPublicDictionaryCheckbox()
		{
			CustomTestContext.WriteLine("Поставить галочку в чекбоксе 'Общедоступный пакет'");
			PublicDictionaryCheckbox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Создать пакет'
		/// </summary>
		public AdminCreateDictionaryPackagePage ClickCreateDictionaryPack()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Создать пакет'.");
			CreateDictionaryPackButton.Click();

			return LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Выбрать словари для пакета
		/// </summary>
		public AdminCreateDictionaryPackagePage AddDictionariesToPackage(List<string> dictionariesList)
		{
			CustomTestContext.WriteLine("Выбрать словари для пакета.");

			dictionariesList.ForEach(item =>
			{
				DictionariesList.SelectOptionByText(item);
				AddDictionaryToPack.Click();
			});

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить открылась ли страница создания пакета словарей
		/// </summary>
		/// <returns></returns>
		public bool IsAdminCreateDictionaryPackagePageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DICTIONARY_PACKAGE_NAME));
		}

		#endregion

		#region Объявление элементов страницы

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

		#endregion

		#region Описания XPath элементов

		protected const string ADD_DICTIONARY_TO_PACK_BUTTON = "//input[@name='toRight']";
		protected const string PUBLIC_DICTIONARY_CHECKBOX = "//input[@id='isPublic']";
		protected const string DICTIONARY_PACKAGE_NAME = "//input[@id='packageName']";
		protected const string DICTIONARIES_LIST = "//select[@id='left']";
		protected const string CREATE_DICTIONARY_PACK_BUTTON = "//input[@data-ref='frmCreatePackage']";

		#endregion
	}
}
