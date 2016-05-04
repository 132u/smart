using System;
using System.Linq;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminEnterpriseAccountsPage : AdminLingvoProPage, IAbstractPage<AdminEnterpriseAccountsPage>
	{
		public AdminEnterpriseAccountsPage(WebDriver driver) : base(driver)
		{
		}

		public new AdminEnterpriseAccountsPage LoadPage()
		{
			if (!IsAdminEnterpriseAccountsPageOpened())
			{
				throw new Exception(
					"Произошла ошибка:\n не загружена страница корпоративных аккаунтов.");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Выбрать затею
		/// </summary>
		/// <param name="venture">затея</param>
		public AdminEnterpriseAccountsPage ChooseVenture(string venture)
		{
			CustomTestContext.WriteLine("Выбрать затею {0}.", venture);
			SelectVenture.SelectOptionByText(venture);

			return LoadPage();
		}

		/// <summary>
		/// Нажать ссылку редактирование пользователей аккаунта
		/// </summary>
		/// <param name="accountName">имя аккаунта</param>
		public AdminEnterpriseAccountUsersPage ClickManageUsersReference(string accountName)
		{
			CustomTestContext.WriteLine("Нажать ссылку для редактирования пользователей аккаунта {0}.", accountName);
			Driver.WaitUntilElementIsDisplay(By.XPath(MANAGE_USERS_REF.Replace("*#*", accountName)));
			ManageUsersRef = Driver.SetDynamicValue(How.XPath, MANAGE_USERS_REF, accountName);
			ManageUsersRef.Click();

			return new AdminEnterpriseAccountUsersPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать 'Создать аккаунт'
		/// </summary>
		public AdminEnterpriseAccountsPage ClickCreateAccount()
		{
			CustomTestContext.WriteLine("Нажать 'Создать аккаунт'");
			AddAccountRef.Click();

			return LoadPage();
		}

		/// <summary>
		/// Переключиться в окно создания нового аккаунта
		/// </summary>
		public AdminCreateAccountPage SwitchToAdminCreateAccountWindow()
		{
			CustomTestContext.WriteLine("Переключиться в окно создания нового аккаунта.");
			Thread.Sleep(1000);
			if (Driver.WindowHandles.Count > 1)
			{
				Driver.SwitchTo().Window(Driver.WindowHandles.First()).Close();
				Driver.SwitchTo().Window(Driver.WindowHandles.Last());
			}

			return new AdminCreateAccountPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку редактирования аккаунта
		/// </summary>
		/// <param name="accountName">имя аккаунта</param>
		public AdminCreateAccountPage ClickEditAccount(string accountName)
		{
			CustomTestContext.WriteLine("Нажать кнопку редактирования аккаунта {0}.", accountName);
			Driver.SetDynamicValue(How.XPath, EDIT_BUTTON, accountName).Click();

			return new AdminCreateAccountPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница корпоративных аккаунтов
		/// </summary>
		public bool IsAdminEnterpriseAccountsPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SELECT_VENTURE), timeout: 25);
		}

		/// <summary>
		/// Проверить, есть ли в таблице аккаунтов аккаунт с заданным именем
		/// </summary>
		/// <param name="accountName">имя аккаунта</param>
		public bool IsAccountExists(string accountName)
		{
			CustomTestContext.WriteLine("Проверить, есть ли в таблице аккаунтов аккаунт с именем {0}.", accountName);

			return Driver.ElementIsDisplayed(By.XPath(MANAGE_USERS_REF.Replace("*#*", accountName)));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SELECT_VENTURE)]
		protected IWebElement SelectVenture { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_ACCOUNT_REF)]
		protected IWebElement AddAccountRef { get; set; }

		protected IWebElement ManageUsersRef { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string SELECT_VENTURE = "//select[@id='VentureId']";
		protected const string ADD_ACCOUNT_REF = "//a[contains(@href,'/EnterpriseAccounts/Edit')]";
		protected const string MANAGE_USERS_REF = "//table//tr[contains(string(), '*#*')]//a[contains(@href,'/EnterpriseAccountUsers')]";
		protected const string EDIT_BUTTON = "//td[text()='*#*']/../td[1]//a[contains(@href,'/EnterpriseAccounts/Edit')]";

		#endregion
	}
}
