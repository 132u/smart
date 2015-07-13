using System;
using System.Globalization;

using NLog;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class LicenseDialogHelper : BillingHelper
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();

		public LicenseDialogHelper SelectDuration(Period duration)
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
			_licenseBaseDialog.ClickCloseButton();

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

		public LicenseDialogHelper SelectNewLicenseNumber(int licenseNumber)
		{
 			BaseObject.InitPage(_licenseUpgradeDialog);
			_licenseUpgradeDialog.SelectLiceneQuantityToUpgrade(licenseNumber);

			return this;
		}

		public LicenseDialogHelper AssertCurrentLicenseNumberOptionNotExistInDropdown(int licenseNumber)
		{
			BaseObject.InitPage(_licenseUpgradeDialog);
			_licenseUpgradeDialog
				.OpenLicenseNumberDropdown()
				.AssertLicenseNumberNotExistInDropdown(licenseNumber);

			return this;
		}

		public LicenseDialogHelper AssertAdditionalPaymentIsCorrect(int newPackagePrice, Period monthsPeriod)
		{
			var expectedAdditionalPayment = calculateAdditionalPayment(CurrentPackagePrice(), newPackagePrice, monthsPeriod);

			Logger.Trace("Проверить, что дополнительная сумма оплаты при апгрейде пакета равна {0}.", expectedAdditionalPayment);

			Assert.AreEqual(expectedAdditionalPayment, AdditionalPayment(),
				"Произошла ошибка:\n дополнительная сумма оплаты при апгрейде пакета вычислена неверно.");

			return this;
		}

		public LicenseDialogHelper AdditionalPaymentForPackageExtend(int expectedAdditionalPayment)
		{
			Logger.Trace("Проверить, что дополнительная сумма оплаты при продлении пакета равна {0}.", expectedAdditionalPayment);

			Assert.AreEqual(expectedAdditionalPayment, AdditionalPayment(),
				"Произошла ошибка:\n дополнительная сумма оплаты при продлении пакета вычислена неверно.");

			return this;
		}

		public int AdditionalPayment()
		{
			BaseObject.InitPage(_licenseBaseDialog);

			return _licenseBaseDialog.GetAdditionalPayment();
		}

		public int CurrentPackagePrice()
		{
			BaseObject.InitPage(_licenseBaseDialog);

			return _licenseBaseDialog.CurrentPackagePrice();
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

		/// <summary>
		/// Посчитать количество оставшихся дней действия текущего пакета лицензий
		/// </summary>
		private int calculateLeftDays()
		{
			Logger.Trace("Посчитать количество оставшихся дней действия текущего пакета лицензий.");
			BaseObject.InitPage(_licenseUpgradeDialog);

			var period = _licenseUpgradeDialog.PackageValidityPeriod();
			var periodArray = period.Split('—');

			DateTime startDate = DateTime.ParseExact(periodArray[0], "M/d/yyyy", CultureInfo.InvariantCulture);
			DateTime endDate = DateTime.ParseExact(periodArray[1], "M/d/yyyy", CultureInfo.InvariantCulture);

			return (endDate - startDate).Days;
		}

		/// <summary>
		/// Посчитать дополнительную плату за продление/обновление пакета лицензий
		/// </summary>
		/// <param name="currentPackagePrice">стоимость нового пакета лицензий</param>
		/// <param name="newPackagePrice"> стоимость старого пакета лицензий</param>
		/// <param name="totalPeriod">общий срок действия текущего пакета лицензий</param>
		private int calculateAdditionalPayment(int currentPackagePrice, int newPackagePrice, Period totalPeriod)
		{
			Logger.Trace("Посчитать дополнительную плату за продление/обновление пакета лицензий по формуле:"
						 + "\n Стоимость = n*(y-x)/k, Где n – количество оставшихся дней действия текущего пакета лицензий,"
						 + " k – общий срок действия текущего пакета лицензий, y – стоимость нового пакета лицензий, x – стоимость старого пакета лицензий.");

			var totalCountDays = (DateTime.Now.AddMonths((int)totalPeriod) - DateTime.Now).Days;

			return calculateLeftDays() * (newPackagePrice - currentPackagePrice) / totalCountDays;


		}

		private readonly LicenseExtendDialog _licenseExtendDialog = new LicenseExtendDialog();
		private readonly LicenseBaseDialog _licenseBaseDialog = new LicenseBaseDialog();
		private readonly LicensePaymentDialog _licensePaymentDialog = new LicensePaymentDialog();
		private readonly LicenseUpgradeDialog _licenseUpgradeDialog = new LicenseUpgradeDialog();
		private readonly LicenseTrialDialog _licenseTrialDialog = new LicenseTrialDialog();
	}
}
