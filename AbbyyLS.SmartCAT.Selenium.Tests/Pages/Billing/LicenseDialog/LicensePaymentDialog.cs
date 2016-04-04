using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog
{
	public class LicensePaymentDialog : LicenseBaseDialog, IAbstractPage<LicensePaymentDialog>
	{
		public LicensePaymentDialog(WebDriver driver) : base(driver)
		{
		}

		public new LicensePaymentDialog LoadPage()
		{
			if (!IsLicensePaymentDialogOpebed())
			{
				throw new Exception("Произошла ошибка:\n не открылась форма для ввода данных кредитной карты.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Pay
		/// </summary>
		public T ClickPayButton<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Нажать кнопку Pay.");
			PayButton.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.LoadPage();
		}

		/// <summary>
		/// Ввести номер карты
		/// </summary>
		/// <param name="cardNumber">номер карты</param>
		public LicensePaymentDialog FillCardNumber(string cardNumber)
		{
			CustomTestContext.WriteLine("Ввести номер карты {0}.", cardNumber);
			CreditCardNumber.SendKeys(cardNumber);

			return LoadPage();
		}

		/// <summary>
		/// Ввести CVV карты
		/// </summary>
		/// <param name="cvv">cvv</param>
		public LicensePaymentDialog FillCvv(string cvv)
		{
			CustomTestContext.WriteLine("Ввести CVV карты {0}.", cvv);
			Cvv.SetText(cvv);

			return LoadPage();
		}

		/// <summary>
		/// Ввести дату окончания срока действия карты
		/// </summary>
		/// <param name="date">дата</param>
		public LicensePaymentDialog FillExpirationDate(string date)
		{
			CustomTestContext.WriteLine("Ввести {0} дату окончания срока действия карты.", date);
			ExpirationDate.SendKeys(date);

			return LoadPage();
		}

		/// <summary>
		/// Выйти из IFrame платежной системы
		/// </summary>
		public LicenseBaseDialog SwitchToDefaultContentFromPaymentIFrame()
		{
			CustomTestContext.WriteLine("Выйти из IFrame платежной системы.");
			Driver.SwitchToDefaultContent();

			return new LicenseBaseDialog(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Заполнить данные платежной карты
		/// </summary>
		/// <param name="cardNumber">номер карты</param>
		/// <param name="cvv">cvv</param>
		/// <param name="expirationDate">дата окончания действия</param>
		public LicensePaymentDialog FillCreditCardData(
			string cardNumber = "4111111111111111",
			string cvv = "123",
			string expirationDate = "11/16")
		{
			SwitchToPaymentIFrame();

			FillCardNumber(cardNumber);
			FillCvv(cvv);
			FillExpirationDate(expirationDate);

			SwitchToDefaultContentFromPaymentIFrame();

			return this;
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли форма ввода данных платежной карты
		/// </summary>
		public bool IsLicensePaymentDialogOpebed()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CREDIT_CARD_NUMBER));
		}

		#endregion

		#region Описание элементов страницы

		[FindsBy(How = How.XPath, Using = CREDIT_CARD_NUMBER)]
		protected IWebElement CreditCardNumber { get; set; }

		[FindsBy(How = How.XPath, Using = CVV)]
		protected IWebElement Cvv { get; set; }

		[FindsBy(How = How.XPath, Using = EXPIRATION_DATE)]
		protected IWebElement ExpirationDate { get; set; }
		[FindsBy(How = How.XPath, Using = PAY_BUTTON)]
		protected IWebElement PayButton { get; set; }

		#endregion

		#region Описание XPath элементов

		public const string PAY_BUTTON = "//footer[contains(@class, 'clearfix')]//a[contains(@abb-link-click, 'commitPayment')]";
		public const string CREDIT_CARD_NUMBER = "//input[@id='credit-card-number']";
		public const string CVV = "//input[@id='cvv']";
		public const string CALENDAR_CONTROL = "//button[contains(@class, 'calendar')]";
		public const string DATE_FIELD = "//input[contains(@class, 'date')]";
		public const string EXPIRATION_DATE = "//input[@id='expiration']";

		#endregion
	}
}
