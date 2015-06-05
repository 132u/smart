using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog
{
	public class LicenseUpgradeCompleteDialog : LicenseBaseDialog, IAbstractPage<LicenseUpgradeCompleteDialog>
	{
		public new LicenseUpgradeCompleteDialog GetPage()
		{
			var upgradeCompleteLicenseDialog = new LicenseUpgradeCompleteDialog();
			InitPage(upgradeCompleteLicenseDialog);

			return upgradeCompleteLicenseDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(UPGRADE_COMPLETE_DIALOG)))
			{
				Assert.Fail("Произошла ошибка:\n сообщение о завершении обновления пакета лицензий не открылось.");
			}
		}

		[FindsBy(How = How.XPath, Using = EXTEND_DATE)]
		protected IWebElement ExtendDate { get; set; }

		public const string EXTEND_DATE = "//div[@class='b-popup-content ng-binding']";
		public const string UPGRADE_COMPLETE_DIALOG = "//div[contains(@class, 'message-popup ng-scope')]//div[contains(@class, 'content') and (contains(text(), 'Thank you for upgrading ') or contains(text(), 'Спасибо, что увеличили'))]";

	}
}
