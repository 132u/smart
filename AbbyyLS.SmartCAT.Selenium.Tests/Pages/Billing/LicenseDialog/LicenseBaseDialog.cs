using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog
{
	public class LicenseBaseDialog : BillingPage, IAbstractPage<LicenseBaseDialog>
	{
		public new LicenseBaseDialog GetPage()
		{
			var licenseBaseDialog = new LicenseBaseDialog();
			InitPage(licenseBaseDialog);

			return licenseBaseDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CANCEL_BUTTON)))
			{
				Assert.Fail("Произошла ошибка:\n не открылся основной диалог покупки/обновления/продления пакета лицензий.");
			}
		}

		/// <summary>
		/// Нажать кнопку Buy
		/// </summary>
		public T ClickBuyButton<T>() where T : class, IAbstractPage<T>, new()
		{
			Logger.Debug("Нажать кнопку Buy.");
			BuyButton.Click();

			return new T().GetPage();
		}

		/// <summary>
		/// Нажать кнопку Close
		/// </summary>
		public LicenseBaseDialog ClickCloseButton()
		{
			Logger.Debug("Нажать кнопку Close.");
			CloseButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Дождаться исчезновения кнопки Close
		/// </summary>
		/// <returns></returns>
		public BillingPage WaitUntilCloseButtonDissappear()
		{
			Logger.Trace("Дождаться исчезновения кнопки Close.");
			Driver.WaitUntilElementIsDissapeared(By.XPath(CLOSE_BUTTON));

			return new BillingPage().GetPage();
		}

		/// <summary>
		/// Перейти в IFrame платежной системы
		/// </summary>
		public LicensePaymentDialog SwitchToPaymentIFrame()
		{
			Logger.Trace("Перейти в IFrame платежной системы.");
			Driver.WaitUntilElementIsDisplay(By.XPath(PAYMENT_IFRAME));
			Driver.SwitchToIFrame(By.XPath(PAYMENT_IFRAME));

			return new LicensePaymentDialog().GetPage();
		}

		/// <summary>
		/// Получить сумму доплаты
		/// </summary>
		public string GetAdditionalPayment()
		{
			Logger.Trace("Получить сумму доплаты для продления лицензии.");
			return AdditionalPayment.Text;
		}

		/// <summary>
		/// Проверить, что дополнительная сумма оплаты изменилась после выбора нового количества лицензий или продления пакета срока действия.
		/// </summary>
		/// <param name="amountBefore"></param>
		/// <param name="amountaAfter"></param>
		public LicenseBaseDialog AssertAdditionalAmountChanged(string amountBefore)
		{
			Logger.Trace("Проверить, что дополнительная сумма оплаты до ({0}) и после ({1}) обновления/продления изменилась.",
				amountBefore, GetAdditionalPayment());

			Assert.AreNotEqual(amountBefore, GetAdditionalPayment(),
				"Произошла ошибка:\n дополнительная сумма оплаты не изменилась.");

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = CLOSE_BUTTON)]
		protected IWebElement CloseButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = BUY_BUTTON_IN_DIALOG)]
		protected IWebElement BuyButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADDITIONAL_PAYMENT)]
		protected IWebElement AdditionalPayment { get; set; }

		public const string ADDITIONAL_PAYMENT = "//tr[@ng-if='ctrl.isIncrease() || ctrl.isProlongation()']//td[2]";
		public const string CLOSE_BUTTON = "//a[contains(@abb-link-click, 'close')]";
		public const string CANCEL_BUTTON = "//footer[contains(@class, 'clearfix')]//a[contains(@abb-link-click, 'close')]";
		public const string BUY_BUTTON_IN_DIALOG = "//div[@class='lic-popup ng-scope']//a[contains(@class, 'danger')]";
		public const string PAYMENT_IFRAME = "//form[@id='checkoutForm']//iframe";
	}
}
