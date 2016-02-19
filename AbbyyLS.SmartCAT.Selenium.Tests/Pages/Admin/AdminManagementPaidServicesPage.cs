using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminManagementPaidServicesPage : AdminLingvoProPage, IAbstractPage<AdminManagementPaidServicesPage>
	{
		public AdminManagementPaidServicesPage(WebDriver driver) : base(driver)
		{
		}

		public new AdminManagementPaidServicesPage GetPage()
		{
			var adminManagementPaidServicesPage = new AdminManagementPaidServicesPage(Driver);
			InitPage(adminManagementPaidServicesPage, Driver);

			return adminManagementPaidServicesPage;
		}

		public new void LoadPage()
		{
			if (!IsAdminManagementPaidServicesPageOpened())
			{
				throw new Exception(
					"Произошла ошибка:\n не загружена страница управления платными услугами");
			}
		}

		#region Простые методы

		/// <summary>
		/// Нажать кнопку включения безлимитного использования услуг
		/// </summary>
		public AdminManagementPaidServicesPage ClickEnableUnlimitedUseServicesButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку включения безлимитного использования услуг");
			EnableUnlimitedUseServicesButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку отключения безлимитного использования услуг
		/// </summary>
		public AdminManagementPaidServicesPage ClickDisableUnlimitedUseServicesButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку отключения безлимитного использования услуг");
			DisableUnlimitedUseServicesButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке редактирования аккаунта
		/// </summary>
		public AdminCreateAccountPage ClickEditAccountReference()
		{
			CustomTestContext.WriteLine("Кликнуть по ссылке редактирования аккаунта");
			EditAccountReference.Click();

			return new AdminCreateAccountPage(Driver).GetPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Включить безлимитное использование услуг, если не включено
		/// </summary>
		public AdminManagementPaidServicesPage EnableUnlimitedUseIfNotEnabled()
		{
			if (!IsUnlimitedUnitsEnabled())
			{
				ClickEnableUnlimitedUseServicesButton();

				if (!IsUnlimitedUseServicesEnabled())
				{
					throw new Exception(
						"Произошла ошибка:\n не удалось включить безлимитное использование услуг");
				}
			}

			return GetPage();
		}

		/// <summary>
		/// Выключить безлимитное использование услуг, если не выключено
		/// </summary>
		public AdminManagementPaidServicesPage DisableUnlimitedUseIfNotDisabled()
		{
			if (IsUnlimitedUnitsEnabled())
			{
				ClickDisableUnlimitedUseServicesButton();

				if (!IsUnlimitedUseServicesDisabled())
				{
					throw new Exception(
						"Произошла ошибка:\n не удалось отключить безлимитное использование услуг.");
				}
			}

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница управления платными услугами
		/// </summary>
		public bool IsAdminManagementPaidServicesPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(EDIT_ACCOUNT_REFERENCE));
		}

		/// <summary>
		/// Вернуть, включено ли неограниченное число тарифных единиц
		/// </summary>
		public bool IsUnlimitedUnitsEnabled()
		{
			CustomTestContext.WriteLine("Вернуть, включено ли неограниченное число тарифных единиц");

			return Driver.ElementIsDisplayed(By.XPath(TABLE_WHEN_UNLIMITED_UNITS_ENABLE));
		}

		/// <summary>
		/// Проверить, что безлимитное использование услуг включено
		/// </summary>
		public bool IsUnlimitedUseServicesEnabled()
		{
			CustomTestContext.WriteLine("Проверить, что безлимитное использование услуг включено");

			return Driver.WaitUntilElementIsDisplay(By.XPath(TABLE_WHEN_UNLIMITED_UNITS_ENABLE));
		}

		/// <summary>
		/// Проверить, что безлимитное использование услуг отключено
		/// </summary>
		public bool IsUnlimitedUseServicesDisabled()
		{
			CustomTestContext.WriteLine("Проверить, что безлимитное использование услуг отключено");

			return Driver.WaitUntilElementIsDisplay(By.XPath(TABLE_WHEN_UNLIMITED_UNITS_DISABLE));
		}

		#endregion

		#region Объявление элементов страниц

		[FindsBy(How = How.XPath, Using = ENABLE_UNLIMITED_USE_SERVICES_BUTTON)]
		protected IWebElement EnableUnlimitedUseServicesButton { get; set; }

		[FindsBy(How = How.XPath, Using = EDIT_ACCOUNT_REFERENCE)]
		protected IWebElement EditAccountReference { get; set; }

		[FindsBy(How = How.XPath, Using = DISABLE_UNLIMITED_USE_SERVICES_BUTTON)]
		protected IWebElement DisableUnlimitedUseServicesButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string EDIT_ACCOUNT_REFERENCE = "//a[contains(@href,'EnterpriseAccounts/Edit')]";
		protected const string TABLE_WHEN_UNLIMITED_UNITS_ENABLE = "//table[contains(@class,'show-when-unlimited')]";
		protected const string ENABLE_UNLIMITED_USE_SERVICES_BUTTON = "//span[contains(@class,'hide-when-unlimited')]/input";
		protected const string TABLE_WHEN_UNLIMITED_UNITS_DISABLE = "//table[contains(@class,'hide-when-unlimited')]";
		protected const string DISABLE_UNLIMITED_USE_SERVICES_BUTTON = "//span[contains(@class,'show-when-unlimited')]/input";

		#endregion
	}
}
