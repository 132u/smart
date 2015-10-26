using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog
{
	public class LicenseTrialDialog : LicenseBaseDialog, IAbstractPage<LicenseTrialDialog>
	{
		public LicenseTrialDialog(WebDriver driver) : base(driver)
		{
		}

		public new LicenseTrialDialog GetPage()
		{
			InitPage(this, Driver);

			return this;
		}

		public new void LoadPage()
		{
			if (!IsLicenseTrialDialogOpened())
			{
				throw new Exception("Произошла ошибка:\n не открылось сообщение о триальном периоде.");
			}
		}

		/// <summary>
		/// Нажать кнопку Continue
		/// </summary>
		public LicenseBaseDialog ClickContinueInTrialDialog()
		{
			CustomTestContext.WriteLine("Нажать кнопку Continue.");
			ContiniueButton.Click();

			return new LicenseBaseDialog(Driver).GetPage();
		}

		/// <summary>
		/// Проверить, открылось ли сообщение о триальном периоде
		/// </summary>
		public bool IsLicenseTrialDialogOpened()
		{
			CustomTestContext.WriteLine("Проверить, открылось ли сообщение о триальном периоде");

			return Driver.WaitUntilElementIsDisplay(By.XPath(TRIAL_MESSAGE));
		}

		[FindsBy(How = How.XPath, Using = CONTINIUE_BUTTON)]
		protected IWebElement ContiniueButton { get; set; }

		public const string CONTINIUE_BUTTON = "//div[contains(@class,'lic-popup message')]//footer//a[contains(@class,'btn btn-danger')]";
		public const string TRIAL_MESSAGE = "//div[contains(text(),'trial') or contains(text(),'пробные лицензии будут аннулированы')]";
	}
}
