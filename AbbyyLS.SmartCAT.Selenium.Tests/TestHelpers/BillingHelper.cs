using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class BillingHelper
	{
		public WebDriver Driver { get; private set; }

		public BillingHelper(WebDriver driver)
		{
			Driver = driver;
			_billingPage = new BillingPage(Driver);
		}

		public LicenseDialogHelper OpenLicensePurchaseDialog(int licenseNumber = 5, Period period = Period.ThreeMonth)
		{
			BaseObject.InitPage(_billingPage, Driver);
			_billingPage
				.SelectLicenseNumber(licenseNumber)
				.ClickBuyButton(period);

			return new LicenseDialogHelper(Driver);
		}

		public LicenseDialogHelper OpenLicenseExtendDialog(int rowNumber = 1)
		{
			BaseObject.InitPage(_billingPage, Driver);
			_billingPage.ClickExtendButton(rowNumber);

			return new LicenseDialogHelper(Driver);
		}

		public int PackagesCount()
		{
			BaseObject.InitPage(_billingPage, Driver);

			return _billingPage.PackagesCount();
		}

		public BillingHelper AssertPackagesCountEquals(int expectedCount)
		{
			BaseObject.InitPage(_billingPage, Driver);
			var actualCount = _billingPage.PackagesCount();

			CustomTestContext.WriteLine("Проверить, что количество пакетов лицензий равно {0}.", expectedCount);

			Assert.AreEqual(expectedCount, actualCount,
				"Произошла ошибка:\n количество пакетов лицензий не изменилось.");

			return this;
		}

		public BillingHelper AssertLicensesCountChanged(int expectedCount)
		{
			BaseObject.InitPage(_billingPage, Driver);
			var actualCount = _billingPage.LicenseCountInPackage();

			CustomTestContext.WriteLine("Проверить, что количество лицензий в пакете изменилось и равно {0}.", expectedCount);

			Assert.AreEqual(expectedCount, actualCount,
				"Произошла ошибка:\n количество лицензий в пакете не соответствует ожидаемому.");

			return this;
		}

		public BillingHelper AssertEndDateChanged(DateTime expectedEndDate)
		{
			BaseObject.InitPage(_billingPage, Driver);
			var actualEndDate = _billingPage.GetEndDate();

			CustomTestContext.WriteLine("Проверить, что срок действия пакета равен {0}.", expectedEndDate);

			Assert.AreEqual(expectedEndDate, actualEndDate,
				"Произошла ошибка:\n срок действия пакета лицензий не соответствует ожидаемому.");

			return this;
		}

		public DateTime GetEndDate()
		{
			BaseObject.InitPage(_billingPage, Driver);

			return _billingPage.GetEndDate();
		}

		public LicenseDialogHelper OpenLicenseUpgradeDialog(int rowNumber = 1)
		{
			BaseObject.InitPage(_billingPage, Driver);
			_billingPage.ClickUpgradeButton(rowNumber);

			return new LicenseDialogHelper(Driver);
		}

		public WorkspaceHelper GoToWorkspace()
		{
			BaseObject.InitPage(_billingPage, Driver);
			_billingPage.ClickLogo();

			return new WorkspaceHelper(Driver);
		}

		public BillingHelper AssertCurrencyMatchInPurchaseTable(string currency)
		{
			BaseObject.InitPage(_billingPage, Driver);
			_billingPage.AssertCurrencyInPurchaseTable(currency);

			return this;
		}

		public int PackagePrice(Period period, int numberLicenses)
		{
			BaseObject.InitPage(_billingPage, Driver);

			return _billingPage
				.SelectLicenseNumber(numberLicenses)
				.PackagePrice(period);
		}

		private readonly BillingPage _billingPage;
	}
}
