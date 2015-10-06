﻿using NUnit.Framework;
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
			var extendCompleteLicenseDialog = new LicenseExtendCompleteDialog(Driver);
			InitPage(extendCompleteLicenseDialog, Driver);

			return extendCompleteLicenseDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(EXTEND_COMPLETE_DIALOG), timeout: 20))
			{
				Assert.Fail("Произошла ошибка:\n сообщение о завершении продления лицензии не открылось.");
			}
		}

		public const string EXTEND_COMPLETE_DIALOG = "//div[contains(@class, 'message-popup ng-scope')]//div[contains(@class, 'content') and (contains(text(), 'Thank you for extending') or contains(text(), 'Спасибо, что продлили'))]";
	}
}
