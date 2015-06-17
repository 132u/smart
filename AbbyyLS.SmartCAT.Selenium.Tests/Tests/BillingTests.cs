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
			AdminLoginPage = true;
		}

		[SetUp]
		public void SetUpBillingTests()
		{
			var accountUniqueName = AdminHelper.GetAccountUniqueName();

			_adminHelper
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
			var licenseNumberBefore = _billingHelper.PackagesQuantity();

			_licenseDialogHelper
				.OpenLicensePurchaseDialog()
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton()
				.CloseCompleteDialog()
				.AssertPackagesQuantityMatch(licenseNumberBefore);
		}

		[Test]
		public void UpgradeLicenseTest()
		{
			var newlicenseQuantity = 20;

			_licenseDialogHelper
				.OpenLicenseUpgradeDialog()
				.SelectNewLicenseNumber(newlicenseQuantity)
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton(LicenseOperation.Upgrade)
				.CloseCompleteDialog()
				.AssertLicensesQuantityMatch(newlicenseQuantity);
		}

		[Test]
		public void DoubleUpgradeLicenseTest()
		{
			var firstNewlicenseQuantity = 20;
			var secondNewlicenseQuantity = 30;

			_licenseDialogHelper
				.OpenLicenseUpgradeDialog()
				.SelectNewLicenseNumber(firstNewlicenseQuantity)
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton(LicenseOperation.Upgrade)
				.CloseCompleteDialog()
				.AssertLicensesQuantityMatch(firstNewlicenseQuantity)
				.OpenLicenseUpgradeDialog()
				.SelectNewLicenseNumber(secondNewlicenseQuantity)
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton(LicenseOperation.Upgrade)
				.CloseCompleteDialog()
				.AssertLicensesQuantityMatch(secondNewlicenseQuantity);
		}

		[Test]
		public void ExtendLicenseTest()
		{
			var endDateBeforeExtend = _billingHelper.EndDate();
			var months = 3;

			_licenseDialogHelper
				.OpenLicenseExtendDialog()
				.SelectDuration(months)
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton(LicenseOperation.Extend)
				.CloseCompleteDialog()
				.AssertEndDateIncremented(endDateBeforeExtend, _billingHelper.EndDate(), months);
		}

		[Test]
		public void DoubleExtendLicenseTest()
		{
			var endDateBeforeFirstExtend = _billingHelper.EndDate();
			var firstDuration = 3;
			var secondDuration = 6;

			_licenseDialogHelper
				.OpenLicenseExtendDialog()
				.SelectDuration(firstDuration)
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton(LicenseOperation.Extend)
				.CloseCompleteDialog()
				.AssertEndDateIncremented(endDateBeforeFirstExtend, _billingHelper.EndDate(), firstDuration);

			var endDateBeforeSecondExtend = _billingHelper.EndDate();

			_licenseDialogHelper
				.OpenLicenseExtendDialog()
				.SelectDuration(secondDuration)
				.ClickBuyButtonInDialog()
				.FillCreditCardData()
				.ClickPayButton(LicenseOperation.Extend)
				.CloseCompleteDialog()
				.AssertEndDateIncremented(endDateBeforeSecondExtend, _billingHelper.EndDate(), secondDuration);
		}

		[Test]
		public void AdditionalPaymentUpgradeTest()
		{
			var additionalPaymentBefore = string.Empty;

			_licenseDialogHelper
				.OpenLicenseUpgradeDialog()
				.AdditionalAmountPayment(out additionalPaymentBefore)
				.SelectNewLicenseNumber(30)
				.AssertAdditionalAmountChanged(additionalPaymentBefore);
		}

		[Test]
		public void AdditionalPaymentExtendTest()
		{
			var additionalPaymentBefore = string.Empty;

			_licenseDialogHelper
				.OpenLicenseExtendDialog()
				.AdditionalAmountPayment(out additionalPaymentBefore)
				.SelectDuration(6)
				.AssertAdditionalAmountChanged(additionalPaymentBefore);
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
		private AdminHelper _adminHelper = new AdminHelper();
		private LicenseDialogHelper _licenseDialogHelper = new LicenseDialogHelper();
	}
}
