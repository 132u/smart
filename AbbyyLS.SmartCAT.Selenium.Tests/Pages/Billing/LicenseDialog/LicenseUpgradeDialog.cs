using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog
{
	public class LicenseUpgradeDialog : LicenseBaseDialog, IAbstractPage<LicenseUpgradeDialog>
	{
		public new LicenseUpgradeDialog GetPage()
		{
			var upgradeDialog = new LicenseUpgradeDialog();
			InitPage(upgradeDialog);

			return upgradeDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(NEW_LICENSE_NUMBER)))
			{
				Assert.Fail("Произошла ошибка:\n не открылся диалог обновления пакета лицензий.");
			}
		}

		/// <summary>
		/// Выбрать количество лицензий
		/// </summary>
		public LicenseUpgradeDialog SelectLiceneQuantityToUpgrade(int newLicenseNumber)
		{
			Logger.Debug("Выбрать {0} лицензий для обновления.", newLicenseNumber);
			NewLicenseNumber.SelectOptionByText(newLicenseNumber.ToString());

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected new IWebElement CancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = LICENSE_NUMBER)]
		protected new IWebElement LicenseNumber { get; set; }

		[FindsBy(How = How.XPath, Using = NEW_LICENSE_NUMBER)]
		protected IWebElement NewLicenseNumber { get; set; }

		[FindsBy(How = How.XPath, Using = PACKAGE_PRICE)]
		protected IWebElement PackagePrice { get; set; }

		[FindsBy(How = How.XPath, Using = LICENSE_NUMBER_IN_PACKAGE)]
		protected IWebElement LicenseNumberInPackage { get; set; }

		public const string LICENSE_NUMBER_IN_PACKAGE = "//table[@class='t-licenses']//td[contains(text(),'Количество лицензий') or contains(text(),'Number of Licenses')]/following-sibling::td";
		public const string PACKAGE_PRICE = "//table[@class='t-licenses']//tr[3]/td[2]";
		public const string NEW_LICENSE_NUMBER = "//tr[contains(@ng-if, 'ctrl.isIncrease')]//select[contains(@ng-options, 'option.amount')]";
		public new const string CANCEL_BUTTON = "//div[@class='lic-popup ng-scope']//a[contains(@abb-link-click, 'close') and contains(@class, 'btn')]";
		public new const string LICENSE_NUMBER = "//select[contains(@class, 'ng-pristine ng-untouched ng-valid')]";
		public const string UPGRADE_LICENSE_NUMBER = "//select[contains(@class, 'ng-pristine ng-untouched ng-valid')]";
	}
}
