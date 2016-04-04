using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog
{
	public class LicenseUpgradeDialog : LicenseBaseDialog, IAbstractPage<LicenseUpgradeDialog>
	{
		public LicenseUpgradeDialog(WebDriver driver) : base(driver)
		{
		}

		public new LicenseUpgradeDialog LoadPage()
		{
			if (!IsLicenseUpgradeDialogOpened())
			{
				throw new Exception("Произошла ошибка:\n не открылся диалог обновления пакета лицензий.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Выбрать количество лицензий
		/// </summary>
		public LicenseUpgradeDialog SelectLiceneQuantityToUpgrade(int newLicenseNumber)
		{
			CustomTestContext.WriteLine("Выбрать {0} лицензий для обновления.", newLicenseNumber.ToString());
			NewLicenseNumber.SelectOptionByText(newLicenseNumber.ToString());

			return LoadPage();
		}

		/// <summary>
		/// Открыть дропдаун выбора количества лицензий при апгрейде
		/// </summary>
		public LicenseUpgradeDialog OpenLicenseNumberDropdown()
		{
			CustomTestContext.WriteLine("Открыть дропдаун выбора количества лицензий при апгрейде.");
			NewLicenseNumber.Click();

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, существует ли опция с указанным кол-вом лицензий в дропдауне
		/// </summary>
		/// <param name="licenseNumber">кол-во лицензий</param>
		public bool IsLicenseNumberOptionExistInDropdown(int licenseNumber)
		{
			OpenLicenseNumberDropdown();

			return Driver.GetIsElementExist(By.XPath(NEW_LICENSE_NUMBER_OPTION.Replace("*#*", licenseNumber.ToString())));
		}

		/// <summary>
		/// Проверить, открылся ли диалог обновления пакета лицензий
		/// </summary>
		public bool IsLicenseUpgradeDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(NEW_LICENSE_NUMBER), timeout: 20);
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = LICENSE_NUMBER)]
		protected new IWebElement LicenseNumber { get; set; }

		[FindsBy(How = How.XPath, Using = NEW_LICENSE_NUMBER)]
		protected IWebElement NewLicenseNumber { get; set; }

		[FindsBy(How = How.XPath, Using = LICENSE_NUMBER_IN_PACKAGE)]
		protected IWebElement LicenseNumberInPackage { get; set; }

		#endregion

		#region Описание XPath елементов

		public const string LICENSE_NUMBER_IN_PACKAGE = "//table[@class='t-licenses']//td[contains(text(),'Количество лицензий') or contains(text(),'Number of Licenses')]/following-sibling::td";
		public const string NEW_LICENSE_NUMBER = "//tr[contains(@ng-if, 'ctrl.isIncrease')]//select[contains(@ng-options, 'option.amount')]";
		public new const string CANCEL_BUTTON = "//div[@class='lic-popup ng-scope']//a[contains(@abb-link-click, 'close') and contains(@class, 'btn')]";
		public new const string LICENSE_NUMBER = "//select[contains(@class, 'ng-pristine ng-untouched ng-valid')]";
		public const string UPGRADE_LICENSE_NUMBER = "//select[contains(@class, 'ng-pristine ng-untouched ng-valid')]";
		public const string NEW_LICENSE_NUMBER_OPTION = "//tr[contains(@ng-if, 'ctrl.isIncrease')]//select[contains(@ng-options, 'option.amount')]//option[@label='*#*']";

		#endregion
	}
}
