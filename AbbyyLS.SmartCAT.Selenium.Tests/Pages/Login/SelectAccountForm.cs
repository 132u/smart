﻿using System;
using System.Collections.Generic;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class SelectAccountForm : IAbstractPage<SelectAccountForm>
	{
		public WebDriver Driver { get; protected set; }

		public SelectAccountForm(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public SelectAccountForm LoadPage()
		{
			Driver.WaitUntilElementIsDisappeared(By.XPath(WAITING_SERVER_RESPONSE_MESSAGE));

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ACCOUNT_SELECTION_FORM), timeout: 30))
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась форма выбора аккаунта.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку "Create Account"
		/// </summary>
		public RegistrationPage ClickCreateAccountButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Create Account'");
			CreateAccountButton.Click();

			return new RegistrationPage(Driver).LoadPage();
		}

		/// <summary>
		/// Получить список доступных аккаунтов.
		/// </summary>
		public List<string> GetAccountList()
		{
			CustomTestContext.WriteLine("Получить список доступных аккаунтов.");

			return Driver.GetTextListElement(By.XPath(AVAILIBLE_ACCOUNTS_LIST));
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать аккаунт
		/// </summary>
		/// <param name="accountName">название аккаунта</param>
		/// <param name="dataServer">расположение сервера</param>
		public WorkspacePage SelectAccount(
			string accountName = LoginHelper.TestAccountName,
			string dataServer = LoginHelper.EuropeTestServerName)
		{
			CustomTestContext.WriteLine("Проверить кол-во ссылок на аккаунты на всех серверах.");
			var europeAccountsCount = Driver.GetElementsCount(By.XPath(EUROPE_ACCOUNT_LIST));
			// На 12.01.2016 USA сервер недоступен на тестовых стендах. Проверка занимает 3 секунды на тест.
			//var usaAccountsCount = Driver.GetElementsCount(By.XPath(USA_ACCOUNT_LIST));
			var totalAccountCount = europeAccountsCount; // + usaAccountsCount;

			CustomTestContext.WriteLine("Ссылок на аккаунты на всех серверах '{0}'", totalAccountCount);

			if (totalAccountCount > 1)
			{
				CustomTestContext.WriteLine("Выбрать аккаунт {0} на сервере {1}.", accountName, dataServer);
				AccountRef = Driver.SetDynamicValue
					(How.XPath, dataServer.ToLower() == "europe" ? RU_ACCOUNT_REF_XPATH : US_ACCOUNT_REF_XPATH, accountName);

				AccountRef.JavaScriptClick();
			}

			return new WorkspacePage(Driver).LoadPage();
		}

		/// <summary>
		/// Отсортировать список аккаунтов для сравнения в нужном порядке.
		/// </summary>
		/// <param name="accountList">список аккаунтов для сортировки в нужном порядке</param>
		public List<string> GetSortedAccountsList(List<string> accountList)
		{
			CustomTestContext.WriteLine("Отсортировать список аккаунтов для сравнения в нужном порядке.");

			var personalAccount = LoginHelper.PersonalAccountName;

			accountList.Sort();

			if (accountList.Contains(personalAccount))
			{
				accountList.Remove(personalAccount);
				accountList.Insert(0, personalAccount);
			}

			return accountList;
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что сервер Europe отвечает
		/// </summary>
		public bool IsEuropeServerRespond()
		{
			CustomTestContext.WriteLine("Проверить, что сервер Europe отвечает.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(EUROPE_HEADER));
		}

		/// <summary>
		/// Проверить сортировку и количество отображаемых аккаунтов.
		/// </summary>
		/// <param name="accountsList">список аккаунтов в которые добавлен пользователь.</param>
		public bool IsSortAndContentInAccountsListCorrect(List<string> accountsList)
		{
			CustomTestContext.WriteLine("Проверить сортировку и количество отображаемых аккаунтов.");

			var accountsFromSelectForm = GetAccountList();
			var sortedAccountsList = GetSortedAccountsList(accountsList);

			if (accountsFromSelectForm.Count != sortedAccountsList.Count)
			{
				throw new Exception("Количество доступных и подключенных аккаунтов не совпадает.");
			}

			int matches = accountsFromSelectForm
				.Where((t, i) => t == sortedAccountsList[i])
				.Count();

			if (accountsFromSelectForm.Count != matches)
			{
				CustomTestContext.WriteLine("Не совпали названия аккаунтов или их порядок.");

				return false;
			}
			

			return true;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CREATE_ACCOUNT_BUTTON)]
		protected IWebElement CreateAccountButton { get; set; }

		[FindsBy(How = How.XPath, Using = AVAILIBLE_ACCOUNTS_LIST)]
		protected IWebElement AvailibleAccountsList { get; set; }

		protected IWebElement AccountRef { get; set; }

		#endregion

		#region Описание Xpath элементов

		protected const string ACCOUNT_SELECTION_FORM = "//div[contains(@data-bind, 'selectAccount')]";
		protected const string AVAILIBLE_ACCOUNTS_LIST = "//div[contains(@data-bind, 'selectAccount')]//ul[contains(@class, 'l-choice-acc__region-list')]//ul";
		protected const string US_ACCOUNT_REF_XPATH = "//li[@translate = 'region-us']/following-sibling::li[@class='ng-scope']//span[contains(string(), '*#*')]";
		protected const string RU_ACCOUNT_REF_XPATH = "//span[contains(@data-bind, 'localRegionName') and text()='Europe']/../..//li[contains(@class,'choice-acc')]//following-sibling::li//a[contains(@data-bind , 'loginToAccount')]//span[text()='*#*']";
		protected const string EUROPE_ACCOUNT_LIST = "//span[contains(@data-bind, 'localRegionName') and text()='Europe']/../..//li[contains(@class,'choice-acc')]//following-sibling::li//a[contains(@data-bind , 'loginToAccount')]";
		protected const string USA_ACCOUNT_LIST = "//li[@translate='region-us']//following-sibling::li//a[contains(@ng-click,'signInAccount')]";
		protected const string WAITING_SERVER_RESPONSE_MESSAGE = "//div[@ng-show='accountWatitngServerResponse']/span";
		protected const string CREATE_ACCOUNT_BUTTON = "//button[@data-bind='click: createAccount']";

		protected const string EUROPE_HEADER = "//li[@translate='region-ru']";

		#endregion
	}
}
