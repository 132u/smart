using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing
{
	public class BillingPage : WorkspacePage, IAbstractPage<BillingPage>
	{
		public new BillingPage GetPage()
		{
			var billingPage = new BillingPage();
			InitPage(billingPage);

			return billingPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(LICENSE_NUMBER)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница управления лицензиями.");
			}
		}

		/// <summary>
		/// Получить количество пакетов лицензий
		/// </summary>
		/// <returns>количество лицензий</returns>
		public int PackagesCount()
		{
			Logger.Trace("Получить количество пакетов лицензий.");

			return Driver.GetElementsCount(By.XPath(LICENSES_LIST));
		}

		/// <summary>
		/// Выбрать количество лицензий для покупки
		/// </summary>
		/// <param name="licenseNumber">количество лицензий</param>
		public BillingPage SelectLicenseNumber(int licenseNumber)
		{
			Logger.Debug("Выбрать {0} лицензий для покупки.", licenseNumber);
			LicenseNumber.SelectOptionByText(licenseNumber.ToString());

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Buy для определенного периода
		/// </summary>
		/// <param name="monthPeriod">период, на который покупается лицензия</param>
		public LicensePurchaseDialog ClickBuyButton(int monthPeriod)
		{
			Logger.Debug("Нажать кнопку Buy для периода {0} месяцев.", monthPeriod);
			Driver.SetDynamicValue(How.XPath, BUY_BUTTON, monthPeriod.ToString()).Click();

			return new LicensePurchaseDialog().GetPage();
		}

		/// <summary>
		/// Проверить, что количество лицензий увеличилось после покупки
		/// </summary>
		/// <param name="licenseNumberBefore">количество лицензий до покупки</param>
		public BillingPage ComparePackageQuantity(int licenseNumberBefore)
		{
			Assert.AreEqual(licenseNumberBefore + 1, PackagesCount(),
				"Произошла ошибка:\n количество лицензий не увеличилось.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Upgrade
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public LicenseUpgradeDialog ClickUpgradeButton(int rowNumber)
		{
			Logger.Debug("Нажать кнопку Upgrade в {0} строке.", rowNumber);
			var buttons = visibleUpgradeButtonsList();
			buttons[rowNumber - 1].Click();

			return new LicenseUpgradeDialog().GetPage();
		}

		/// <summary>
		/// Нажать кнопку Extend
		/// </summary>
		/// <param name="rowNumebr">номер строки</param>
		public LicenseExtendDialog ClickExtendButton(int rowNumebr)
		{
			Logger.Debug("Нажать кнопку Extend в {0} строке.", rowNumebr);
			Driver.SetDynamicValue(How.XPath, EXTEND_BUTTON, rowNumebr.ToString()).Click();

			return new LicenseExtendDialog().GetPage();
		}

		/// <summary>
		/// Проверить знак валюты
		/// </summary>
		/// <param name="currency">валюта</param>
		public BillingPage AssertCurrencyInPurchaseTable(string currency)
		{
			Logger.Trace("Проверить, знак валюты {0}.", currency);
			var periodList = Driver.GetTextListElement(By.XPath(TABLE_HEADER));

			Assert.IsTrue(periodList.All(p => p.Contains(currency)),
				"Произошла ошибка:\n валюта {0} не совпадает с валютой в таблице {1}", currency, periodList[0]);

			return GetPage();
		}

		/// <summary>
		/// Получить конечную дату пакета лицензий.
		/// </summary>
		/// <returns>конечная дата</returns>
		public DateTime EndDate()
		{
			Logger.Trace("Получить конечную дату пакета лицензий.");

			return DateTime.ParseExact(EndDateColumn.Text, "M/d/yyyy", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Проверить, что дата окончания изменилась после продления пакета лицензий
		/// </summary>
		/// <param name="dateBeforeExtend">дата окончания до продления</param>
		/// <param name="dateAfterExtend">дата окончания после продления</param>
		/// <param name="months">количество месяцев на которое продлили пакет</param>
		public BillingPage AssertEndDateChangedAfterExtend(DateTime dateBeforeExtend, DateTime dateAfterExtend, int months)
		{
			Logger.Trace("Проверить, что дата окончания изменилась после продления пакета лицензий.");

			Assert.AreEqual(dateBeforeExtend.AddMonths(months), dateAfterExtend,
				"Произошла ошибка:\n после продления пакета лицензий дата окончания не изменилась.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что после обновления количество лицензий в пакете увеличилось
		/// </summary>
		/// <param name="licenseQuantityExpected">ожидаемое количество</param>
		public BillingPage AssertLicenseQuantityMatch(int licenseQuantityExpected)
		{
			Logger.Trace("Проверить, что после обновления в пакете {0} лицензий.", licenseQuantityInPackage());

			Assert.AreEqual(licenseQuantityExpected, licenseQuantityInPackage(),
				"Произошла ошибка:\n количество лицензий после обновления не изменилось");

			return GetPage();
		}

		/// <summary>
		/// Нажать по логотипу
		/// </summary>
		public WorkspacePage ClickLogo()
		{
			Logger.Debug("Нажать по логотипу.");
			Logo.Click();

			return new WorkspacePage();
		}

		/// <summary>
		/// Получить список всех кнопок Upgrade(видимых и невидимых)
		/// </summary>
		/// <returns>cписок кнопок Upgrade</returns>
		private IList<IWebElement> upgradeButtonsList()
		{
			Logger.Trace("Получить список всех кнопок Upgrade(видимых и невидимых).");

			return Driver.GetElementList(By.XPath(UPGRADE_BUTTONS));
		}

		/// <summary>
		/// Получить список видимых Upgrade кнопок
		/// </summary>
		/// <returns>список видимых Upgrade кнопок</returns>
		private IList<IWebElement> visibleUpgradeButtonsList()
		{
			Logger.Trace("Получить список видимых кнопок Upgrade.");

			return upgradeButtonsList().Where(btn => btn.Displayed).ToList();
		}

		/// <summary>
		/// Получить количество лицензий в пакете из первой колонки верхней таблицы
		/// </summary>
		/// <returns>количество лицензий в пакете</returns>
		private int licenseQuantityInPackage()
		{
			Logger.Trace("Получить количество лицензий в пакете из первой колонки верхней таблицы.");
			var licenseNumber = LicenseQuantityColumn.Text.Split(' ');

			return int.Parse(licenseNumber[0]);
		}

		[FindsBy(How = How.XPath, Using = LICENSE_NUMBER)]
		protected IWebElement LicenseNumber { get; set; }

		[FindsBy(How = How.XPath, Using = END_DATE_COLUMN)]
		protected IWebElement EndDateColumn { get; set; }

		[FindsBy(How = How.XPath, Using = LICENSE_QUANTITY_COLUMN)]
		protected IWebElement LicenseQuantityColumn { get; set; }

		[FindsBy(How = How.XPath, Using = LOGO)]
		protected IWebElement Logo { get; set; }

		public const string UPGRADE_BUTTONS = "//tr[@class='ng-scope']//td[contains(@ng-if, 'ManuallyCreated')]//a[contains(@ng-show, 'ctrl.canIncrease')]";
		public const string LICENSES_LIST = "//table[@class='t-licenses ng-scope']/tbody/tr[not(@ng-if='ctrl.demoPackage')]";
		public string LICENSES_TABLE = "//table[contains(@class, 'licenses ng-scope')]";
		public const string LICENSE_NUMBER = "//table[contains(@class, 'add-lic')]//select[contains(@ng-model, 'selectedOption')]";
		public const string BUY_BUTTON = "//td['*#*']//a[contains(@class, 'danger')]";
		public const string EXTEND_BUTTON = "//tbody//tr['*#*']//a[contains(@abb-link-click, 'editLicensePackage(package, false)')]";
		public const string EXTEND_BUTTONS = "//a[contains(@abb-link-click, 'editLicensePackage(package, false)')]";
		public const string TABLE_HEADER = "//table[@class='t-licenses add-lic']//th[contains(@ng-repeat, 'period')]";
		public const string LICENCE_TITLE = "//td[contains(text(),'license') or contains(text(),' licenses')]";
		public const string END_DATE_COLUMN = "//td[contains(@ng-class,'willExpireSoon')]";
		public const string LICENSE_QUANTITY_COLUMN = "//td[contains(text(),'license') or contains(text(),' licenses')]"; 
		public const string LOGO = "//a[@id='logo']";
	}
}
