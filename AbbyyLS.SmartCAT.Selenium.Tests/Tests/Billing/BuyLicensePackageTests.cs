using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Billing
{
	[Parallelizable(ParallelScope.Fixtures)]
	class BuyLicensePackageTests<TWebDriverProvider> : BaseBillingTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase(Period.ThreeMonth), Ignore("PRX-13374")]
		[TestCase(Period.SixMonth)]
		[TestCase(Period.TwelveMonth)]
		[TestCase(Period.OneMonth)]
		public void BuyLicenseTest(Period period)
		{
			var licenseCountBefore = BillingPage.PackagesCount();

			BillingPage
				.OpenLicensePurchaseDialog(period: period)
				.ClickBuyButton<LicenseTrialDialog>();

			LicenseTrialDialog.ClickContinueInTrialDialog();

			LicensePaymentDialog
				.FillCreditCardData()
				.ClickPayButton<LicensePurchaseCompleteDialog>();

			LicensePurchaseCompleteDialog.ClickCloseButton();

			Assert.IsTrue(BillingPage.IsPackagesCountEquals(licenseCountBefore + 1),
				"Произошла ошибка:\n количество пакетов лицензий не изменилось.");
		}
	}
}
