﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;


namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminCreateAccountPage : AdminLingvoProPage, IAbstractPage<AdminCreateAccountPage>
	{
		public AdminCreateAccountPage(WebDriver driver) : base(driver)
		{
		}

		public new AdminCreateAccountPage GetPage()
		{
			var adminCreateAccountPage = new AdminCreateAccountPage(Driver);
			InitPage(adminCreateAccountPage, Driver);

			return adminCreateAccountPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(INPUT_NAME)))
			{
				Assert.Fail("Произошла ошибка:\n не загружена страница создания аккаунта.");
			}
		}

		/// <summary>
		/// Выбрать дату окончания действия словаря
		/// </summary>
		public AdminCreateAccountPage SelectExpirationDate()
		{
			var nextYear = DateTime.Today.AddYears(1).ToString("dd.MM.yyyy");
			CustomTestContext.WriteLine("Установить дату '{0}'", nextYear);
			DictionariesExtDate.SetText(nextYear);

			return GetPage();
		}
		
		/// <summary>
		/// Нажать кнопку 'Сохранить' 
		/// </summary>
		public AdminCreateAccountPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Сохранить'.");
			SaveButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Закрыть окно с вопросом о Workflow
		/// </summary>
		public AdminCreateAccountPage AcceptWorkflowModalDialog()
		{
			try
			{
				// sleep нужен, чтоб диалог успел появиться
				Thread.Sleep(1000);
				if (Driver.SwitchTo().Alert().Text.Contains("Включение функции"
					+ " workflow для аккаунта необратимо, обратное выключение "
					+ "будет невозможно. Продолжить?"))
				{
					CustomTestContext.WriteLine("Закрыть модальное диалоговое окно");
					Driver.SwitchTo().Alert().Accept();
				}
				// sleep нужен, чтобы диалог успел закрыться.
				Thread.Sleep(500);
			}
			catch (NoAlertPresentException)
			{
				tryCloseExternalDialog();
			}

			return GetPage();
		}

		/// <summary>
		/// Отметить Workflow чекбокс при создании корп аккаунта
		/// </summary>
		public AdminCreateAccountPage CheckWorkflowCheckbox()
		{
			CustomTestContext.WriteLine("Поставить галочку в  Workflow чекбокс");
			WorkflowCheckbox.Click();

			return this;
		}

		/// <summary>
		/// Ввести имя аккаунта
		/// </summary>
		/// <param name="name">имя</param>
		public AdminCreateAccountPage FillAccountName(string name)
		{
			CustomTestContext.WriteLine("Ввести названия аккаунта " + name);
			Name.SetText(name);

			return GetPage();
		}

		/// <summary>
		/// Выбрать затею
		/// </summary>
		/// <param name="name">название</param>
		public AdminCreateAccountPage SetVenture(string name)
		{
			CustomTestContext.WriteLine("Выбрать нужную затею: {0}", name);
			Venture.SelectOptionByText(name);

			return GetPage();
		}

		/// <summary>
		/// Выбрать тип корпоративного аккаунта
		/// </summary>
		public AdminCreateAccountPage SetEnterpriseAccountType(string accountType)
		{
			CustomTestContext.WriteLine("Выбрать тип корпоративного аккаунта: {0}", accountType);
			EnterpriseAccountTypeDropdown.SelectOptionByText(accountType);

			return GetPage();
		}

		/// <summary>
		/// Ввести название поддомена
		/// </summary>
		/// <param name="name">название</param>
		public AdminCreateAccountPage FillSubdomainName(string name)
		{
			CustomTestContext.WriteLine("Ввести название поддомена {0}", name);
			Subdomain.SetText(name);

			return GetPage();
		}

		/// <summary>
		/// Выбрать функцию
		/// </summary>
		public AdminCreateAccountPage SelectFeature(string feature)
		{
			CustomTestContext.WriteLine("Выбрать функцию {0}", feature);
			FeaturesOptions = Driver.SetDynamicValue(How.XPath, FEATURES_OPTIONS, feature);
			FeaturesOptions.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть стрелку, чтоб доавбить функию
		/// </summary>
		public AdminCreateAccountPage ClickRightArrowToAddFeature()
		{
			CustomTestContext.WriteLine("Кликнуть по стрелке, чтоб добавить функцию");
			FeaturesToRightArrow.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку добавления всех пакетов словарей
		/// </summary>
		public AdminCreateAccountPage AddAllPackages()
		{
			CustomTestContext.WriteLine("Нажать на кнопку добавления всех пакетов словарей.");
			AddAllDictionariesPackagesButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить список включенных в аккаунт фич из правой таблицы.
		/// </summary>
		public List<string> FeaturesListInAccount()
		{
			CustomTestContext.WriteLine("Получить список включенных в аккаунт фич из правой таблицы.");

			return Driver.GetTextListElement(By.XPath(FEATURES_OPTIONS_IN_RIGHT_TABLE));
		}

		/// <summary>
		/// Нажать на кнопку добавления всех алгоритмов разбора документов 
		/// </summary>
		public AdminCreateAccountPage ClickAddAllDisassemblesButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку добавления всех алгоритмов разбора документов.");

			AddAllDisassemblesButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке 'Управление платными услугами'
		/// </summary>
		/// <returns></returns>
		public AdminManagementPaidServicesPage ClickManagementPaidServicesReference()
		{
			CustomTestContext.WriteLine("Кликнуть по ссылке 'Управление платными услугами'");

			ManagementPaidServicesReference.Click();

			return new AdminManagementPaidServicesPage(Driver).GetPage();
		}

		/// <summary>
		/// Закрыть открытый диалог
		/// </summary>
		private static void tryCloseExternalDialog()
		{
			CustomTestContext.WriteLine("Закрыть открытый диалог");
			SendKeys.SendWait(@"{Enter}");
			// sleep нужен, чтобы диалог успел закрыться.
			Thread.Sleep(1000);
		}

		[FindsBy(How = How.XPath, Using = FEATURES_OPTIONS)]
		protected IWebElement FeaturesOptions { get; set; }

		[FindsBy(How = How.XPath, Using = FEATURES_TO_RIGHT_ARROW)]
		protected IWebElement FeaturesToRightArrow { get; set; }

		[FindsBy(How = How.XPath, Using = DICTIONARIES_EXP_DATE)]
		protected IWebElement DictionariesExtDate {get; set;}

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = INPUT_NAME)]
		protected IWebElement Name { get; set; }

		[FindsBy(How = How.XPath, Using = INPUT_VENTURE)]
		protected IWebElement Venture { get; set; }

		[FindsBy(How = How.XPath, Using = SUBDOMAIN_NAME)]
		protected IWebElement Subdomain { get; set; }

		[FindsBy(How = How.XPath, Using = WORKFLOW_CHECKBOX)]
		protected IWebElement WorkflowCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_ALL_DICTIONARIES_PACKAGES_BUTTON)]
		protected IWebElement AddAllDictionariesPackagesButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_ALL_DISASSEMBLES_BUTTON)]
		protected IWebElement AddAllDisassemblesButton { get; set; }

		[FindsBy(How = How.XPath, Using = MANAGEMENT_PAID_SERVICES_REFERENCE)]
		protected IWebElement ManagementPaidServicesReference { get; set; }

		[FindsBy(How = How.XPath, Using = ENTERPRISE_ACCOUNT_TYPE_DROPDOWN)]
		protected IWebElement EnterpriseAccountTypeDropdown { get; set; }

		protected const string INPUT_NAME = "//input[(@id='Name')]";
		protected const string FEATURES_OPTIONS = "//table[@name='Features']//select[@id='left']//option[@value='*#*']";
		protected const string FEATURES_OPTIONS_IN_RIGHT_TABLE = "//table[@name='Features']//select[@id='right']//option";
		protected const string FEATURES_TO_RIGHT_ARROW = "//table[@name='Features']//input[@name='toRight']";
		protected const string DICTIONARIES_EXP_DATE = "//input[@id='DictionariesExpirationDate']";
		protected const string SAVE_BUTTON = "//p[@class='submit-area']/input";
		protected const string INPUT_VENTURE = "//select[@id='VentureId']";
		protected const string SUBDOMAIN_NAME = "//input[@name='SubDomain']";
		protected const string WORKFLOW_CHECKBOX = "//input[@id='WorkflowEnabled']";
		protected const string ADD_ALL_DICTIONARIES_PACKAGES_BUTTON = "//table[@name='dictionariesPackages']//input[@name='allToRight']";
		protected const string ADD_ALL_DISASSEMBLES_BUTTON = "//table[@name='DisassembleDocumentMethodNames']//input[@name='allToRight']";
		protected const string MANAGEMENT_PAID_SERVICES_REFERENCE = "//a[contains(@href,'PaidServices')]";
		protected const string ENTERPRISE_ACCOUNT_TYPE_DROPDOWN = "//select[@id='EnterpriseAccountType']";
	}
}
