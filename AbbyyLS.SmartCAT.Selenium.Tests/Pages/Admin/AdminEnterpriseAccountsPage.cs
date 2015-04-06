﻿using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	/// <summary>
	/// Страничка с корпоративными аккаунтами
	/// </summary>
	public class AdminEnterpriseAccountsPage : AdminLingvoProPage, IAbstractPage<AdminEnterpriseAccountsPage>
	{
		public new AdminEnterpriseAccountsPage GetPage()
		{
			var adminEnterpriseAccountsPage = new AdminEnterpriseAccountsPage();
			InitPage(adminEnterpriseAccountsPage);
			LoadPage();
			return adminEnterpriseAccountsPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.XPath(SELECT_VENTURE_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не загружена страничка AdminEnterpriseAccountsPage (Корпоративные аккаунты).");
			}
		}

		/// <summary>
		/// Выбираем затею
		/// </summary>
		/// <param name="venture">затея</param>
		public AdminEnterpriseAccountsPage ChooseVenture(string venture)
		{
			Logger.Trace("Выбираем затею {0}.", venture);
			SelectVenture.SelectOptionByText(venture);
			return GetPage();
		}

		/// <summary>
		/// Нажимаем ссылку редактирование пользователей аккаунта
		/// </summary>
		/// <param name="accountName">имя аккаунта</param>
		public AdminEnterpriseAccountUsersPage ClickManageUsersRef(string accountName)
		{
			Logger.Trace("Нажимаем ссылку для редактирования пользователей аккаунта {0}.", accountName);
			ManageUsersRef = Driver.SetDynamicValue(How.XPath, MANAGE_USERS_REF_XPATH, accountName);
			ManageUsersRef.Click();
			var adminEnterpriseAccountUsersPage = new AdminEnterpriseAccountUsersPage();
			return adminEnterpriseAccountUsersPage.GetPage();
		}

		/// <summary>
		/// Проверяем, есть ли в таблице аккаунтов аккаунт с заданным именем
		/// </summary>
		/// <param name="accountName">имя аккаунта</param>
		public bool IsAccountExists(string accountName)
		{
			Logger.Trace("Проверяем, есть ли в таблице аккаунтов аккаунт с именем {0}.", accountName);
			if (Driver.ElementIsPresent(By.XPath(MANAGE_USERS_REF_XPATH.Replace("*#*", accountName))))
			{
				return true;
			}
			return false;
		}

		[FindsBy(How = How.XPath, Using = SELECT_VENTURE_XPATH)]
		protected IWebElement SelectVenture { get; set; }

		protected IWebElement ManageUsersRef { get; set; }

		protected const string SELECT_VENTURE_XPATH = "//select[@id='VentureId']";
		protected const string ADD_ACCOUNT_REF_XPATH = "//a[contains(@href,'/EnterpriseAccounts/Edit')]";
		protected const string MANAGE_USERS_REF_XPATH = "//table//tr[contains(string(), '*#*')]//a[contains(@href,'/EnterpriseAccountUsers')]";
	}
}