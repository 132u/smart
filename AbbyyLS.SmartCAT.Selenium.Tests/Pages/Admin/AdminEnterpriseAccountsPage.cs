using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

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

			return adminEnterpriseAccountsPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SELECT_VENTURE)))
			{
				Assert.Fail("Произошла ошибка:\n не загружена страничка AdminEnterpriseAccountsPage (Корпоративные аккаунты).");
			}
		}

		/// <summary>
		/// Выбрать затею
		/// </summary>
		/// <param name="venture">затея</param>
		public AdminEnterpriseAccountsPage ChooseVenture(string venture)
		{
			Logger.Debug("Выбрать затею {0}.", venture);
			SelectVenture.SelectOptionByText(venture);

			return GetPage();
		}

		/// <summary>
		/// Нажать ссылку редактирование пользователей аккаунта
		/// </summary>
		/// <param name="accountName">имя аккаунта</param>
		public AdminEnterpriseAccountUsersPage ClickManageUsersReference(string accountName)
		{
			Logger.Debug("Нажать ссылку для редактирования пользователей аккаунта {0}.", accountName);
			ManageUsersRef = Driver.SetDynamicValue(How.XPath, MANAGE_USERS_REF, accountName);
			ManageUsersRef.Click();

			return new AdminEnterpriseAccountUsersPage().GetPage();
		}

		/// <summary>
		/// Проверить, есть ли в таблице аккаунтов аккаунт с заданным именем
		/// </summary>
		/// <param name="accountName">имя аккаунта</param>
		public bool IsAccountExists(string accountName)
		{
			Logger.Trace("Проверить, есть ли в таблице аккаунтов аккаунт с именем {0}.", accountName);
			
			return Driver.ElementIsDisplayed(By.XPath(MANAGE_USERS_REF.Replace("*#*", accountName)));
		}

		/// <summary>
		/// Нажать 'Создать аккаунт'
		/// </summary>
		public AdminEnterpriseAccountsPage ClickCreateAccount()
		{
			Logger.Trace("Нажать 'Создать аккаунт'");
			AddAccountRef.Click();
			

			return GetPage();
		}

		/// <summary>
		/// Переключиться в окно создания нового аккаунта
		/// </summary>
		public AdminCreateAccountPage SwitchToAdminCreateAccountWindow()
		{
			Logger.Trace("Переключиться в окно создания нового аккаунта.");
			Driver.SwitchTo().Window(Driver.WindowHandles[1]);

			return new AdminCreateAccountPage().GetPage();
		}

		[FindsBy(How = How.XPath, Using = SELECT_VENTURE)]
		protected IWebElement SelectVenture { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_ACCOUNT_REF)]
		protected IWebElement AddAccountRef { get; set; }

		protected IWebElement ManageUsersRef { get; set; }

		protected const string SELECT_VENTURE = "//select[@id='VentureId']";
		protected const string ADD_ACCOUNT_REF = "//a[contains(@href,'/EnterpriseAccounts/Edit')]";
		protected const string MANAGE_USERS_REF = "//table//tr[contains(string(), '*#*')]//a[contains(@href,'/EnterpriseAccountUsers')]";
	}
}
