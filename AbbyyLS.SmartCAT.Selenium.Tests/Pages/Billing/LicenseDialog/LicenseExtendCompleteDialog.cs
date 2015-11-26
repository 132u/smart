using System;

using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog
{
	public class LicenseExtendCompleteDialog : LicenseBaseDialog, IAbstractPage<LicenseExtendCompleteDialog>
	{
		public LicenseExtendCompleteDialog(WebDriver driver) : base(driver)
		{
		}

		public new LicenseExtendCompleteDialog GetPage()
		{
			InitPage(this, Driver);

			return this;
		}

		public new void LoadPage()
		{
			if (!IsLicenseExtendCompleteDialogOpened())
			{
				throw new Exception("Произошла ошибка:\n сообщение о завершении продления лицензии не открылось.");
			}
		}

		/// <summary>
		/// Проверить, открыт ли диалог завершения продления лицензии
		/// </summary>
		public bool IsLicenseExtendCompleteDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(EXTEND_COMPLETE_DIALOG), timeout: 20);
		}

		public const string EXTEND_COMPLETE_DIALOG = "//div[contains(@class, 'message-popup ng-scope')]//div[contains(@class, 'content') and (contains(text(), 'Thank you for extending') or contains(text(), 'Спасибо, что продлили'))]";
	}
}
