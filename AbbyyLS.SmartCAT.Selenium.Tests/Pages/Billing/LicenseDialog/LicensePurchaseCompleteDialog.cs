﻿using System;

using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog
{
	public class LicensePurchaseCompleteDialog : LicenseBaseDialog, IAbstractPage<LicensePurchaseCompleteDialog>
	{
		public LicensePurchaseCompleteDialog(WebDriver driver) : base(driver)
		{
		}

		public new LicensePurchaseCompleteDialog GetPage()
		{

			var licensePurchaseCompleteDialog = new LicensePurchaseCompleteDialog(Driver);
			InitPage(licensePurchaseCompleteDialog, Driver);

			return licensePurchaseCompleteDialog;
		}

		public new void LoadPage()
		{
			if (!IsLicensePurchaseCompleteDialogOpened())
			{
				throw new Exception("Произошла ошибка:\n не открылось сообщение о завершении покупки пакета лицензий.");
			}
		}

		/// <summary>
		/// Проверить, открылось ли сообщение о завершении покупки пакета лицензий
		/// </summary>
		public bool IsLicensePurchaseCompleteDialogOpened()
		{
			CustomTestContext.WriteLine("Проверить, открылось ли сообщение о завершении покупки пакета лицензий");

			return Driver.WaitUntilElementIsDisplay(By.XPath(COMPLETE_DIALOG), timeout: 50);
		}

		public const string COMPLETE_DIALOG = "//div[contains(@class, 'message-popup ng-scope')]//div[contains(@class, 'content') and (contains(text(), 'Thank you for purchasing') or contains(text(), 'Спасибо, что приобрели'))]";
	}
}
