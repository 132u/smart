using System;

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

		public new LicensePurchaseDialog LoadPage()
		{
			if (!IsLicensePurchaseDialogOpened())
			{
				throw new Exception("Произошла ошибка:\n не открылся диалог покупки лицензий.");
			}

			return this;
		}

		/// <summary>
		/// Проверить, открылся ли диалог покупки лицензий
		/// </summary>
		public bool IsLicensePurchaseDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CALENDAR_CONTROL));
		}

		public const string CALENDAR_CONTROL = "//button[contains(@class, 'calendar')]";
	}
}
