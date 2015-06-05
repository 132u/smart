using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class LicenseDialogHelper : BillingHelper
	{
		public LicenseDialogHelper SelectDuration(int duration)
		{
			BaseObject.InitPage(_licenseExtendDialog);
			_licenseExtendDialog.SelectExtendDuration(duration);

			return this;
		}

		public LicenseDialogHelper ClickBuyButtonInDialog(bool trial = false)
		{
			BaseObject.InitPage(_licenseBaseDialog);
			if (trial)
			{
				_licenseBaseDialog.ClickBuyButton<LicenseTrialDialog>();
			}
			else
			{
				_licenseBaseDialog.ClickBuyButton<LicenseBaseDialog>();
			}

			return this;
		}

		public LicenseDialogHelper FillCreditCardData(
			string cardNumber = "4111111111111111",
			string cvv = "123",
			string expirationDate = "11/16")
		{
			switchToPaymentIFrame();

			BaseObject.InitPage(_licensePaymentDialog);
			_licensePaymentDialog
				.FillCardNumber(cardNumber)
				.FillCvv(cvv)
				.FillExpirationDate(expirationDate);

			switchToDefaultContentFromPaymentIFrame();

			return this;
		}

		public BillingHelper CloseCompleteDialog()
		{
			BaseObject.InitPage(_licenseBaseDialog);
			_licenseBaseDialog
				.ClickCloseButton()
				.WaitUntilCloseButtonDissappear();

			return this;
		}

		public LicenseDialogHelper SelectLicenseNumber(int newLicenseNumber)
		{
			BaseObject.InitPage(_licenseUpgradeDialog);
			_licenseUpgradeDialog.SelectLicenseNumber(newLicenseNumber);

			return this;
		}

		public LicenseDialogHelper ClickPayButton(LicenseOperation licenseOperation = LicenseOperation.Buy)
		{
			BaseObject.InitPage(_licenseBaseDialog);
			switch (licenseOperation)
			{
				case LicenseOperation.Upgrade:
					_licensePaymentDialog.ClickPayButton<LicenseUpgradeCompleteDialog>();
					break;

				case LicenseOperation.Extend:
					_licensePaymentDialog.ClickPayButton<LicenseExtendCompleteDialog>();
					break;

				default:
					_licensePaymentDialog.ClickPayButton<LicensePurchaseCompleteDialog>();
					break;
			}

			return this;
		}

		public LicenseDialogHelper CloseTrialDialog()
		{
			BaseObject.InitPage(_licenseTrialDialog);
			_licenseTrialDialog.ClickContinueInTrialDialog();

			return this;
		}

		public LicenseDialogHelper SelectNewLicenseNumber(int newLicenseNumber)
		{
 			BaseObject.InitPage(_licenseUpgradeDialog);
			_licenseUpgradeDialog.SelectLiceneQuantityToUpgrade(newLicenseNumber);

			return this;
		}

		public LicenseDialogHelper AssertAdditionalAmountChanged(string amountBefore)
		{
			BaseObject.InitPage(_licenseBaseDialog);
			_licenseBaseDialog.AssertAdditionalAmountChanged(amountBefore);

			return this;
		}

		public LicenseDialogHelper AdditionalAmountPayment(out string additionalPayment)
		{
			BaseObject.InitPage(_licenseBaseDialog);
			additionalPayment = _licenseBaseDialog.GetAdditionalPayment();

			return this;
		}

		private LicenseDialogHelper switchToPaymentIFrame()
		{
			BaseObject.InitPage(_licenseBaseDialog);
			_licenseBaseDialog.SwitchToPaymentIFrame();

			return this;
		}

		private LicenseDialogHelper switchToDefaultContentFromPaymentIFrame()
		{
			BaseObject.InitPage(_licensePaymentDialog);
			_licensePaymentDialog.SwitchToDefaultContentFromPaymentIFrame();

			return this;
		}

		private readonly LicenseExtendDialog _licenseExtendDialog = new LicenseExtendDialog();
		private readonly LicenseBaseDialog _licenseBaseDialog = new LicenseBaseDialog();
		private readonly LicensePaymentDialog _licensePaymentDialog = new LicensePaymentDialog();
		private readonly LicenseUpgradeDialog _licenseUpgradeDialog = new LicenseUpgradeDialog();
		private readonly LicenseTrialDialog _licenseTrialDialog = new LicenseTrialDialog();
	}
}
