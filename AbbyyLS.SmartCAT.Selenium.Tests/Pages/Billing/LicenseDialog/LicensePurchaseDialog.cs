using NUnit.Framework;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog
{
	public class LicensePurchaseDialog : LicenseBaseDialog, IAbstractPage<LicensePurchaseDialog>
	{
		public LicensePurchaseDialog(WebDriver driver) : base(driver)
		{
		}

		public new LicensePurchaseDialog GetPage()
		{

			var licensePurchaseDialog = new LicensePurchaseDialog(Driver);
			InitPage(licensePurchaseDialog, Driver);

			return licensePurchaseDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CALENDAR_CONTROL)))
			{
				Assert.Fail("Произошла ошибка:\n не открылся диалог покупки лицензий.");
			}
		}

		public const string CALENDAR_CONTROL = "//button[contains(@class, 'calendar')]";
	}
}
