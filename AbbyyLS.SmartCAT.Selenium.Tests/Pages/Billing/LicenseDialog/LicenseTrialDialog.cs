using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog
{
	public class LicenseTrialDialog : LicenseBaseDialog, IAbstractPage<LicenseTrialDialog>
	{
		public new LicenseTrialDialog GetPage()
		{

			var trialDialog = new LicenseTrialDialog();
			InitPage(trialDialog);

			return trialDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(TRIAL_MESSAGE)))
			{
				Assert.Fail("Произошла ошибка:\n не открылось сообщение о триальном периоде.");
			}
		}

		/// <summary>
		/// Нажать кнопку Continue
		/// </summary>
		public LicenseBaseDialog ClickContinueInTrialDialog()
		{
			Logger.Debug("Нажать кнопку Continue.");
			ContiniueButton.Click();

			return new LicenseBaseDialog().GetPage();
		}

		[FindsBy(How = How.XPath, Using = CONTINIUE_BUTTON)]
		protected IWebElement ContiniueButton { get; set; }

		public const string CONTINIUE_BUTTON = "//div[contains(@class,'lic-popup message')]//footer//a[contains(@class,'btn btn-danger')]";
		public const string TRIAL_MESSAGE = "//div[contains(text(),'trial') or contains(text(),'пробные лицензии будут аннулированы')]";
	}
}
