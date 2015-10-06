using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Billing
{
	[Parallelizable(ParallelScope.Fixtures)]
	class BillingTests<TWebDriverProvider> : BaseBillingTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBillingTests()
		{
			LicenseDialogHelper
				.OpenLicensePurchaseDialog(_licenseNumber, _period)
				.ClickBuyButtonInDialog(trial: true)
				.CloseTrialDialog()
				.FillCreditCardData()
				.ClickPayButton()
				.CloseCompleteDialog();
		}

		[Test]
		public void UpgradeLicenseTest()
		{
			var newlicenseCount = 20;

			LicenseDialogHelper
				.OpenLicenseUpgradeDialog()
				.AssertCurrentLicenseNumberOptionNotExistInDropdown(_licenseNumber)
				.SelectNewLicenseNumber(newlicenseCount)
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton(LicenseOperation.Upgrade)
				.CloseCompleteDialog()
				.AssertLicensesCountChanged(newlicenseCount);
		}

		[Test]
		public void DoubleUpgradeLicenseTest()
		{
			var firstLicenseCount = 20;
			var secondLicenseCount = 30;

			LicenseDialogHelper
				.OpenLicenseUpgradeDialog()
				.AssertCurrentLicenseNumberOptionNotExistInDropdown(_licenseNumber)
				.SelectNewLicenseNumber(firstLicenseCount)
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton(LicenseOperation.Upgrade)
				.CloseCompleteDialog()
				.AssertLicensesCountChanged(firstLicenseCount)
				.OpenLicenseUpgradeDialog()
				.SelectNewLicenseNumber(secondLicenseCount)
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton(LicenseOperation.Upgrade)
				.CloseCompleteDialog()
				.AssertLicensesCountChanged(secondLicenseCount);
		}

		[Test]
		public void ExtendLicenseTest()
		{
			var endDateBeforeExtend = BillingHelper.GetEndDate();
			var months = Period.ThreeMonth;

			LicenseDialogHelper
				.OpenLicenseExtendDialog()
				.SelectDuration(months)
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton(LicenseOperation.Extend)
				.CloseCompleteDialog()
				.AssertEndDateChanged(endDateBeforeExtend.AddMonths((int)months));
		}

		[Test]
		public void DoubleExtendLicenseTest()
		{
			var firstDuration = Period.ThreeMonth;
			var secondDuration = Period.SixMonth;
			var endDateBeforeFirstExtend = BillingHelper.GetEndDate();
			var endDateBeforeSecondExtend = endDateBeforeFirstExtend.AddMonths((int)firstDuration);
			
			LicenseDialogHelper
				.OpenLicenseExtendDialog()
				.SelectDuration(firstDuration)
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton(LicenseOperation.Extend)
				.CloseCompleteDialog()
				.AssertEndDateChanged(endDateBeforeSecondExtend)
				.OpenLicenseExtendDialog()
				.SelectDuration(secondDuration)
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton(LicenseOperation.Extend)
				.CloseCompleteDialog()
				.AssertEndDateChanged(endDateBeforeSecondExtend.AddMonths((int)secondDuration));
		}

		[Test]
		public void AdditionalPaymentUpgradeTest()
		{
			var newLisenceNumber = 30;
			var packagePriceForNewPackage = LicenseDialogHelper.PackagePrice(_period, newLisenceNumber);

			LicenseDialogHelper
				.OpenLicenseUpgradeDialog()
				.SelectNewLicenseNumber(newLisenceNumber)
				.AssertAdditionalPaymentIsCorrect(packagePriceForNewPackage, _period);
		}

		[Test]
		public void AdditionalPaymentExtendTest()
		{
			var extendPeriod = Period.TwelveMonth;
			var packagePriceForExtendPackage = LicenseDialogHelper.PackagePrice(extendPeriod, _licenseNumber);

			LicenseDialogHelper
				.OpenLicenseExtendDialog()
				.SelectDuration(extendPeriod)
				.AdditionalPaymentForPackageExtend(packagePriceForExtendPackage);
		}

		[TestCase(Language.Russian, Language.English, "руб", "$")]
		[TestCase(Language.English, Language.Russian, "$", "руб")]
		public void LocaleCurrencyInTableTest(Language firstLanguage, Language secondLanguage, string firstCurrency, string secondCurrency)
		{
			BillingHelper
				.GoToWorkspace()
				.SelectLocale(firstLanguage)
				.GoToBillingPage()
				.AssertCurrencyMatchInPurchaseTable(firstCurrency)
				.GoToWorkspace()
				.SelectLocale(secondLanguage)
				.GoToBillingPage()
				.AssertCurrencyMatchInPurchaseTable(secondCurrency);
		}

		private int _licenseNumber = 5;
		private Period _period = Period.ThreeMonth;
	}
}
