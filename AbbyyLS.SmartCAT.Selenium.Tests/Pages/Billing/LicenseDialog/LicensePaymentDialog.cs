using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog
{
	public class LicensePaymentDialog : LicenseBaseDialog, IAbstractPage<LicensePaymentDialog>
	{
		public new LicensePaymentDialog GetPage()
		{
			var licensePurchasePaymentDialog = new LicensePaymentDialog();
			InitPage(licensePurchasePaymentDialog);

			return licensePurchasePaymentDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CREDIT_CARD_NUMBER)))
			{
				Assert.Fail("Произошла ошибка:\n не открылась форма для ввода данных кредитной карты.");
			}
		}

		/// <summary>
		/// Нажать кнопку Pay
		/// </summary>
		public T ClickPayButton<T>() where T : class, IAbstractPage<T>, new()
		{
			Logger.Debug("Нажать кнопку Pay.");
			PayButton.Click();

			return new T().GetPage();
		}

		/// <summary>
		/// Ввести номер карты
		/// </summary>
		/// <param name="cardNumber">номер карты</param>
		public LicensePaymentDialog FillCardNumber(string cardNumber)
		{
			Logger.Debug("Ввести номер карты {0}.", cardNumber);
			CreditCardNumber.SendKeys(cardNumber);

			return GetPage();
		}

		/// <summary>
		/// Ввести CVV карты
		/// </summary>
		/// <param name="cvv">cvv</param>
		public LicensePaymentDialog FillCvv(string cvv)
		{
			Logger.Debug("Ввести CVV карты {0}.", cvv);
			Cvv.SetText(cvv);

			return GetPage();
		}

		/// <summary>
		/// Ввести дату окончания срока действия карты
		/// </summary>
		/// <param name="date">дата</param>
		public LicensePaymentDialog FillExpirationDate(string date)
		{
			Logger.Debug("Ввести {0} дату окончания срока действия карты.", date);
			ExpirationDate.SendKeys(date);

			return GetPage();
		}

		/// <summary>
		/// Выйти из IFrame платежной системы
		/// </summary>
		public LicenseBaseDialog SwitchToDefaultContentFromPaymentIFrame()
		{
			Logger.Trace("Выйти из IFrame платежной системы.");
			Driver.SwitchToDefaultContent();

			return new LicenseBaseDialog().GetPage();
		}

		[FindsBy(How = How.XPath, Using = CREDIT_CARD_NUMBER)]
		protected IWebElement CreditCardNumber { get; set; }

		[FindsBy(How = How.XPath, Using = CVV)]
		protected IWebElement Cvv { get; set; }

		[FindsBy(How = How.XPath, Using = EXPIRATION_DATE)]
		protected IWebElement ExpirationDate { get; set; }
		[FindsBy(How = How.XPath, Using = PAY_BUTTON)]
		protected IWebElement PayButton { get; set; }

		public const string PAY_BUTTON = "//footer[contains(@class, 'clearfix')]//a[contains(@abb-link-click, 'commitPayment')]";
		public const string CREDIT_CARD_NUMBER = "//input[@id='credit-card-number']";
		public const string CVV = "//input[@id='cvv']";
		public const string CALENDAR_CONTROL = "//button[contains(@class, 'calendar')]";
		public const string DATE_FIELD = "//input[contains(@class, 'date')]";
		public const string EXPIRATION_DATE = "//input[@id='expiration']";
	}
}
