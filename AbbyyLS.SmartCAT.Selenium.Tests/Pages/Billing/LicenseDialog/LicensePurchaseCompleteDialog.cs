using NUnit.Framework;

using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog
{
	public class LicensePurchaseCompleteDialog : LicenseBaseDialog, IAbstractPage<LicensePurchaseCompleteDialog>
	{
		public new LicensePurchaseCompleteDialog GetPage()
		{

			var licensePurchaseCompleteDialog = new LicensePurchaseCompleteDialog();
			InitPage(licensePurchaseCompleteDialog);

			return licensePurchaseCompleteDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(COMPLETE_DIALOG)))
			{
				Assert.Fail("Произошла ошибка:\n не открылось сообщение о завершении покупки пакета лицензий.");
			}
		}

		public const string COMPLETE_DIALOG = "//div[contains(@class, 'message-popup ng-scope')]//div[contains(@class, 'content') and (contains(text(), 'Thank you for purchasing') or contains(text(), 'Спасибо, что приобрели'))]";
	}
}
