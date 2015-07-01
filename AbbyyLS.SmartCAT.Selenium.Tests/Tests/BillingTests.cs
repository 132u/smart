using System.Collections.Generic;
using NUnit.Framework;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Billing
{
	class BillingTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public BillingTests()
		{
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void SetUpBillingTests()
		{
			var accountUniqueName = AdminHelper.GetAccountUniqueName();
			
			new AdminHelper()
				.CreateAccountIfNotExist(
					accountName: accountUniqueName,
					workflow: true,
					features: new List<string>
					{
						Feature.Clients.ToString(),
						Feature.Domains.ToString(),
						Feature.TranslateConnector.ToString(),
						Feature.LingvoDictionaries.ToString(),
					})
				.AddUserToSpecificAccount(Login, accountUniqueName);

			LogInSmartCat(Login, Password, accountUniqueName);

			_billingHelper = WorkspaceHelper.GoToBillingPage();

			_licenseDialogHelper
				.OpenLicensePurchaseDialog()
				.ClickBuyButtonInDialog(trial: true)
				.CloseTrialDialog()
				.FillCreditCardData()
				.ClickPayButton()
				.CloseCompleteDialog();
		}

		[Test]
		public void BuyLicenseTest()
		{
			var licenseCountBefore = _billingHelper.PackagesCount();

			_licenseDialogHelper
				.OpenLicensePurchaseDialog()
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton()
				.CloseCompleteDialog()
				.AssertPackagesCountChanged(licenseCountBefore + 1);
		}

		[Test]
		public void UpgradeLicenseTest()
		{
			var newlicenseCount = 20;

			_licenseDialogHelper
				.OpenLicenseUpgradeDialog()
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

			_licenseDialogHelper
				.OpenLicenseUpgradeDialog()
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
			var endDateBeforeExtend = _billingHelper.GetEndDate();
			var months = 3;

			_licenseDialogHelper
				.OpenLicenseExtendDialog()
				.SelectDuration(months)
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton(LicenseOperation.Extend)
				.CloseCompleteDialog()
				.AssertEndDateChanged(endDateBeforeExtend.AddMonths(months));
		}

		[Test]
		public void DoubleExtendLicenseTest()
		{
			var firstDuration = 3;
			var secondDuration = 6;
			var endDateBeforeFirstExtend = _billingHelper.GetEndDate();
			var endDateBeforeSecondExtend = endDateBeforeFirstExtend.AddMonths(firstDuration);

			_licenseDialogHelper
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
				.AssertEndDateChanged(endDateBeforeSecondExtend.AddMonths(secondDuration));
		}

		[Test]
		public void AdditionalPaymentUpgradeTest()
		{
			var additionalPaymentBefore = _licenseDialogHelper
				.OpenLicenseUpgradeDialog()
				.GetAdditionalPayment();

			_licenseDialogHelper
				.SelectNewLicenseNumber(30)
				.AssertAdditionalPaymentChanged(additionalPaymentBefore);
		}

		[Test]
		public void AdditionalPaymentExtendTest()
		{
			var additionalPaymentBefore = _licenseDialogHelper
				.OpenLicenseExtendDialog()
				.GetAdditionalPayment();

			_licenseDialogHelper
				.SelectDuration(6)
				.AssertAdditionalPaymentChanged(additionalPaymentBefore);
		}

		[TestCase(Language.Russian, Language.English, "руб", "$")]
		[TestCase(Language.English, Language.Russian, "$", "руб")]
		public void LocaleCurrencyInTableTest(Language firstLanguage, Language secondLanguage, string firstCurrency, string secondCurrency)
		{
			_billingHelper
				.GoToWorkspace()
				.SelectLocale(firstLanguage)
				.GoToBillingPage()
				.AssertCurrencyMatchInPurchaseTable(firstCurrency)
				.GoToWorkspace()
				.SelectLocale(secondLanguage)
				.GoToBillingPage()
				.AssertCurrencyMatchInPurchaseTable(secondCurrency);
		}

		private BillingHelper _billingHelper;
		private readonly LicenseDialogHelper _licenseDialogHelper = new LicenseDialogHelper();
	}
}
