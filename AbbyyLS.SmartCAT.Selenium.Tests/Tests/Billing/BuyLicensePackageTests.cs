using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Billing
{
	[Parallelizable(ParallelScope.Fixtures)]
	class BuyLicensePackageTests<TWebDriverProvider> : BaseBillingTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase(Period.ThreeMonth)]
		[TestCase(Period.SixMonth)]
		[TestCase(Period.TwelveMonth)]
		[TestCase(Period.OneMonth)]
		public void BuyLicenseTest(Period period)
		{
			var licenseCountBefore = BillingHelper.PackagesCount();

			LicenseDialogHelper
				.OpenLicensePurchaseDialog(period: period)
				.ClickBuyButtonInDialog(trial: true)
				.CloseTrialDialog()
				.FillCreditCardData()
				.ClickPayButton()
				.CloseCompleteDialog()
				.AssertPackagesCountEquals(licenseCountBefore + 1);
		}
	}
}
