using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Billing
{
	[Parallelizable(ParallelScope.Fixtures)]
	class BillingTests<TWebDriverProvider> : BaseBillingTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBillingTests()
		{
			BillingPage.OpenLicensePurchaseDialog(_licenseNumber, _period);

			LicensePurchaseDialog.ClickBuyButton<LicenseTrialDialog>();

			LicenseTrialDialog.ClickContinueInTrialDialog();

			LicensePaymentDialog
				.FillCreditCardData()
				.ClickPayButton<LicensePurchaseCompleteDialog>();

			LicensePurchaseCompleteDialog.ClickCloseButton();
		}

		[Test, Ignore("PRX-13374")]
		public void UpgradeLicenseTest()
		{
			var newlicenseCount = 20;

			BillingPage.ClickUpgradeButton();

			Assert.IsFalse(LicenseUpgradeDialog.IsLicenseNumberOptionExistInDropdown(_licenseNumber),
				"Произошла ошибка:\n текущее количество лицензий {0} присутствует в дропдауне при апргрейде пакета.", _licenseNumber);

			LicenseUpgradeDialog
				.SelectLiceneQuantityToUpgrade(newlicenseCount)
				.ClickBuyButton<LicenseBaseDialog>();

			LicensePaymentDialog
				.FillCreditCardData()
				.ClickPayButton<LicenseUpgradeCompleteDialog>();

			LicenseUpgradeCompleteDialog.ClickCloseButton();

			Assert.IsTrue(BillingPage.IsLicensesCountChanged(newlicenseCount),
				"Произошла ошибка:\n количество лицензий в пакете не соответствует ожидаемому.");
		}

		[Test, Ignore("PRX-13374")]
		public void DoubleUpgradeLicenseTest()
		{
			var firstLicenseCount = 20;
			var secondLicenseCount = 30;

			BillingPage.ClickUpgradeButton();

			LicenseUpgradeDialog
				.SelectLiceneQuantityToUpgrade(firstLicenseCount)
				.ClickBuyButton<LicenseBaseDialog>();

			LicensePaymentDialog
				.FillCreditCardData()
				.ClickPayButton<LicenseUpgradeCompleteDialog>();

			LicenseUpgradeCompleteDialog.ClickCloseButton();

			Assert.IsTrue(BillingPage.IsLicensesCountChanged(firstLicenseCount),
				"Произошла ошибка:\n количество лицензий в пакете не соответствует ожидаемому.");

			BillingPage.ClickUpgradeButton();

			LicenseUpgradeDialog
				.SelectLiceneQuantityToUpgrade(secondLicenseCount)
				.ClickBuyButton<LicenseBaseDialog>();

			LicensePaymentDialog
				.FillCreditCardData()
				.ClickPayButton<LicenseUpgradeCompleteDialog>();

			LicenseUpgradeCompleteDialog.ClickCloseButton();

			Assert.IsTrue(BillingPage.IsLicensesCountChanged(secondLicenseCount),
				"Произошла ошибка:\n количество лицензий в пакете не соответствует ожидаемому.");
		}

		[Test, Ignore("PRX-13374")]
		public void ExtendLicenseTest()
		{
			var endDateBeforeExtend = BillingPage.GetEndDate();
			var months = Period.ThreeMonth;

			BillingPage.ClickExtendButton();

			LicenseExtendDialog
				.SelectExtendDuration(months)
				.ClickBuyButton<LicenseBaseDialog>();

			LicensePaymentDialog
				.FillCreditCardData()
				.ClickPayButton<LicenseExtendCompleteDialog>();

			LicenseExtendCompleteDialog.ClickCloseButton();

			Assert.AreEqual(BillingPage.GetEndDate(), endDateBeforeExtend.AddMonths((int)months),
				"Произошла ошибка:\n срок действия пакета лицензий не соответствует ожидаемому.");
		}

		[Test, Ignore("PRX-13374")]
		public void DoubleExtendLicenseTest()
		{
			var firstDuration = Period.ThreeMonth;
			var secondDuration = Period.SixMonth;
			var endDateBeforeFirstExtend = BillingPage.GetEndDate();
			var endDateBeforeSecondExtend = endDateBeforeFirstExtend.AddMonths((int)firstDuration);
			
			BillingPage.ClickExtendButton();

			LicenseExtendDialog
				.SelectExtendDuration(firstDuration)
				.ClickBuyButton<LicenseBaseDialog>();

			LicensePaymentDialog
				.FillCreditCardData()
				.ClickPayButton<LicenseExtendCompleteDialog>();

			LicenseExtendCompleteDialog.ClickCloseButton();

			BillingPage.ClickExtendButton();

			LicenseExtendDialog
				.SelectExtendDuration(secondDuration)
				.ClickBuyButton<LicenseBaseDialog>();

			LicensePaymentDialog
				.FillCreditCardData()
				.ClickPayButton<LicenseExtendCompleteDialog>();

			LicenseExtendCompleteDialog.ClickCloseButton();

			Assert.AreEqual(BillingPage.GetEndDate(), endDateBeforeSecondExtend.AddMonths((int)secondDuration),
				"Произошла ошибка:\n срок действия пакета лицензий не соответствует ожидаемому.");
		}

		[Test, Ignore("PRX-13374")]
		public void AdditionalPaymentUpgradeTest()
		{
			var newLisenceNumber = 30;
			var packagePriceForNewPackage = BillingPage.PackagePrice(_period, newLisenceNumber);

			BillingPage.ClickUpgradeButton();

			LicenseUpgradeDialog.SelectLiceneQuantityToUpgrade(newLisenceNumber);

			var packageValidityPeriod = LicenseUpgradeDialog.PackageValidityPeriod();
			var additionalPayment = LicenseUpgradeDialog.GetAdditionalPayment();
			var currentPackagePrice = LicenseUpgradeDialog.CurrentPackagePrice();
			var expectedAdditionalPayment = BillingPage.CalculateAdditionalPayment(
				currentPackagePrice, packagePriceForNewPackage, _period, packageValidityPeriod);

			Assert.AreEqual(expectedAdditionalPayment, additionalPayment,
				"Произошла ошибка:\n дополнительная сумма оплаты при апгрейде пакета вычислена неверно.");
		}

		[Test, Ignore("PRX-13374")]
		public void AdditionalPaymentExtendTest()
		{
			var extendPeriod = Period.TwelveMonth;
			var packagePriceForExtendPackage = BillingPage.PackagePrice(extendPeriod, _licenseNumber);

			BillingPage.ClickExtendButton();
			
			LicenseExtendDialog.SelectExtendDuration(extendPeriod);

			var additionalPayment = LicenseBaseDialog.GetAdditionalPayment();

			Assert.AreEqual(packagePriceForExtendPackage, additionalPayment,
				"Произошла ошибка:\n дополнительная сумма оплаты при продлении пакета вычислена неверно.");
		}

		[TestCase(Language.Russian, Language.English, "руб", "$"), Ignore("PRX-13374")]
		[TestCase(Language.English, Language.Russian, "$", "руб")]
		public void LocaleCurrencyInTableTest(Language firstLanguage, Language secondLanguage, string firstCurrency, string secondCurrency)
		{
			BillingPage.ClickLogo();

			WorkspacePage
				.SelectLocale(firstLanguage)
				.GoToBillingPage();

			Assert.IsTrue(BillingPage.IsCurrencyInPurchaseTable(firstCurrency),
				"Произошла ошибка:\n валюта {0} не совпадает с валютой в таблице {1}", firstCurrency);

			BillingPage.ClickLogo();

			WorkspacePage
				.SelectLocale(secondLanguage)
				.GoToBillingPage();

			Assert.IsTrue(BillingPage.IsCurrencyInPurchaseTable(secondCurrency),
				"Произошла ошибка:\n валюта {0} не совпадает с валютой в таблице {1}", firstCurrency);
		}

		private int _licenseNumber = 5;
		private Period _period = Period.ThreeMonth;
	}
}
