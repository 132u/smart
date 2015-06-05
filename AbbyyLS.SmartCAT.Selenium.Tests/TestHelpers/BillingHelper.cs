using System;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class BillingHelper
	{
		public LicenseDialogHelper OpenLicensePurchaseDialog(int licenseNumber = 5, int period = 3)
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

		public int PackagesQuantity()
		{
			BaseObject.InitPage(_billingPage);

			return _billingPage.PackagesCount();
		}

		public BillingHelper AssertPackagesQuantityMatch(int expectedLicenseQuantity)
		{
			BaseObject.InitPage(_billingPage);
			_billingPage.ComparePackageQuantity(expectedLicenseQuantity);

			return this;
		}

		public BillingHelper AssertLicensesQuantityMatch(int expectedNumber)
		{
			BaseObject.InitPage(_billingPage);
			_billingPage.AssertLicenseQuantityMatch(expectedNumber);

			return this;
		}

		public BillingHelper AssertEndDateIncremented(DateTime beforeExtendDate, DateTime afterExtendDate, int months)
		{
			BaseObject.InitPage(_billingPage);

			_billingPage.AssertEndDateChangedAfterExtend(beforeExtendDate, afterExtendDate, months);

			return this;
		}

		public DateTime EndDate()
		{
			BaseObject.InitPage(_billingPage);

			return _billingPage.EndDate();
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

		private readonly BillingPage _billingPage = new BillingPage();
	}
}
