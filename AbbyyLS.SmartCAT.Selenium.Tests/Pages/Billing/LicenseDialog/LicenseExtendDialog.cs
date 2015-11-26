using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog
{
	public class LicenseExtendDialog : LicenseBaseDialog, IAbstractPage<LicenseExtendDialog>
	{
		public LicenseExtendDialog(WebDriver driver) : base(driver)
		{
		}

		public new LicenseExtendDialog GetPage()
		{
			InitPage(this, Driver);

			return this;
		}

		public new void LoadPage()
		{
			if (!IsLicenseExtendDialogOpened())
			{
				throw new Exception("Произошла ошибка:\n не открылся диалог продления пакета лицензий.");
			}
		}

		/// <summary>
		/// Выбрать период для продления лицензии
		/// </summary>
		/// <param name="duration">период</param>
		public LicenseExtendDialog SelectExtendDuration(Period duration)
		{
			CustomTestContext.WriteLine("Выбрать {0} месяц(а/ев) для продления пакета лицензий.", duration.Description());

			ExtendPeriod.SelectOptionByText((int)duration + " months");

			return GetPage();
		}

		/// <summary>
		/// Проверить, открыт ли диалог продления пакета лицензий
		/// </summary>
		public bool IsLicenseExtendDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(EXTEND_HEADER));
		}

		[FindsBy(How = How.XPath, Using = EXTEND_PERIOD)]
		protected IWebElement ExtendPeriod { get; set; }

		public const string EXTEND_PERIOD = "//select[contains(@ng-options, 'optionsPrices')]";
		public const string EXTEND_HEADER = "//h3[contains(text(), 'Extention') or contains(text(), 'Продление')]";
		public const string UPGRADE_COMPLETE_DIALOG = "//div[contains(@class, 'message-popup ng-scope')]//div[contains(@class, 'content') and (contains(text(), 'Thank you') or contains(text(), 'Спасибо, что увеличили'))]";
	}
}
