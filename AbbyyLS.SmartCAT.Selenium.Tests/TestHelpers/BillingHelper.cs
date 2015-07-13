using System;

using NLog;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class BillingHelper
	{
		public static Logger Log = LogManager.GetCurrentClassLogger();

		public LicenseDialogHelper OpenLicensePurchaseDialog(int licenseNumber = 5, Period period = Period.ThreeMonth)
		{
			BaseObject.InitPage(_billingPage);
			_billingPage
				.SelectLicenseNumber(licenseNumber)
				.ClickBuyButton(period);

			return new LicenseDialogHelper();
		}

		public LicenseDialogHelper OpenLicenseExtendDialog(int rowNumber = 1)
		{
			BaseObject.InitPage(_billingPage);
			_billingPage.ClickExtendButton(rowNumber);

			return new LicenseDialogHelper();
		}

		public int PackagesCount()
		{
			BaseObject.InitPage(_billingPage);

			return _billingPage.PackagesCount();
		}

		public BillingHelper AssertPackagesCountEquals(int expectedCount)
		{
			BaseObject.InitPage(_billingPage);
			var actualCount = _billingPage.PackagesCount();

			Log.Trace("Проверить, что количество пакетов лицензий равно {0}.", expectedCount);

			Assert.AreEqual(expectedCount, actualCount,
				"Произошла ошибка:\n количество пакетов лицензий не изменилось.");

			return this;
		}

		public BillingHelper AssertLicensesCountChanged(int expectedCount)
		{
			BaseObject.InitPage(_billingPage);
			var actualCount = _billingPage.LicenseCountInPackage();

			Log.Trace("Проверить, что количество лицензий в пакете изменилось и равно {0}.", expectedCount);

			Assert.AreEqual(expectedCount, actualCount,
				"Произошла ошибка:\n количество лицензий в пакете не соответствует ожидаемому.");

			return this;
		}

		public BillingHelper AssertEndDateChanged(DateTime expectedEndDate)
		{
			BaseObject.InitPage(_billingPage);
			var actualEndDate = _billingPage.GetEndDate();

			Log.Trace("Проверить, что срок действия пакета равен {0}.", expectedEndDate);

			Assert.AreEqual(expectedEndDate, actualEndDate,
				"Произошла ошибка:\n срок действия пакета лицензий не соответствует ожидаемому.");

			return this;
		}

		public DateTime GetEndDate()
		{
			BaseObject.InitPage(_billingPage);

			return _billingPage.GetEndDate();
		}

		public LicenseDialogHelper OpenLicenseUpgradeDialog(int rowNumber = 1)
		{
			BaseObject.InitPage(_billingPage);
			_billingPage.ClickUpgradeButton(rowNumber);

			return new LicenseDialogHelper();
		}

		public WorkspaceHelper GoToWorkspace()
		{
			BaseObject.InitPage(_billingPage);
			_billingPage.ClickLogo();

			return new WorkspaceHelper();
		}

		public BillingHelper AssertCurrencyMatchInPurchaseTable(string currency)
		{
			BaseObject.InitPage(_billingPage);
			_billingPage.AssertCurrencyInPurchaseTable(currency);

			return this;
		}

		public int PackagePrice(Period period, int numberLicenses)
		{
			BaseObject.InitPage(_billingPage);

			return _billingPage
				.SelectLicenseNumber(numberLicenses)
				.PackagePrice(period);
		}

		private readonly BillingPage _billingPage = new BillingPage();
	}
}
