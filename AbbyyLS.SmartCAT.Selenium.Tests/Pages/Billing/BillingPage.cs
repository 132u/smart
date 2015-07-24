using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
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
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(LICENSE_NUMBER), timeout: 20))
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
		/// Получить количество лицензий в пакете из первой колонки верхней таблицы
		/// </summary>
		/// <returns>количество лицензий в пакете</returns>
		public int LicenseCountInPackage()
		{
			Logger.Trace("Получить количество лицензий в пакете из первой колонки верхней таблицы.");
			var licenseNumber = LicenseQuantityColumn.Text.Split(' ');
			int licenseNumberInteger;

			if (!int.TryParse(licenseNumber[0], out licenseNumberInteger))
			{
				Assert.Fail("Произошла ошибка:\n не удалось преобразование количества лицензий в пакете из первой колонки верхней таблицы {0} в число.", licenseNumber[0]);
			}

			return licenseNumberInteger;
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
		public LicensePurchaseDialog ClickBuyButton(Period monthPeriod)
		{
			Logger.Debug("Нажать кнопку Buy для периода {0} месяцев.", monthPeriod);
			Driver.FindElement(By.XPath(BUY_BUTTON.Replace("'*#*'", monthPeriod.Description()))).Click();
			
			return new LicensePurchaseDialog().GetPage();
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
		public DateTime GetEndDate()
		{
			Logger.Trace("Получить конечную дату пакета лицензий.");

			return DateTime.ParseExact(EndDateColumn.Text, "M/d/yyyy", CultureInfo.InvariantCulture);
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
		/// Получить стоимость пакета лицензий из нижней таблицы
		/// </summary>
		/// <param name="period">период 1/3/6/12 месяцев</param>
		public int PackagePrice(Period period)
		{
			Logger.Trace("Получить стоимость пакета лицензий за {0} месяцев.", period.Description());
			var priceWithDot = Driver.FindElement(By.XPath(PACKAGE_PRICE.Replace("'*#*'", period.Description()))).Text;
			var price = priceWithDot.Substring(0, priceWithDot.IndexOf(".")).Replace(",","");
			int resultPrice;
			int.TryParse(price, out resultPrice);

			return resultPrice;
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

		[FindsBy(How = How.XPath, Using = LICENSE_NUMBER)]
		protected IWebElement LicenseNumber { get; set; }

		[FindsBy(How = How.XPath, Using = END_DATE_COLUMN)]
		protected IWebElement EndDateColumn { get; set; }

		[FindsBy(How = How.XPath, Using = LICENSE_QUANTITY_COLUMN)]
		protected IWebElement LicenseQuantityColumn { get; set; }

		[FindsBy(How = How.XPath, Using = LOGO)]
		protected IWebElement Logo { get; set; }

		public const string UPGRADE_BUTTONS = "//tr[@class='ng-scope']//td[contains(@ng-if, 'ManuallyCreated')]//a[contains(@ng-show, 'ctrl.canIncrease')]";
		public const string LICENSES_LIST = "//table[@class='t-licenses ng-scope']/tbody/tr[not(@ng-if='ctrl.demoPackage')]//td[contains(@ng-bind, 'LicensesAmountText')]";
		public const string LICENSE_NUMBER = "//table[contains(@class, 'add-lic')]//select[contains(@ng-model, 'selectedOption')]";
		public const string BUY_BUTTON = "//td['*#*']//a[contains(@class, 'danger')]";
		public const string EXTEND_BUTTON = "//tbody//tr['*#*']//a[contains(@abb-link-click, 'editLicensePackage(package, false)')]";
		public const string TABLE_HEADER = "//table[@class='t-licenses add-lic']//th[contains(@ng-repeat, 'period')]";
		public const string END_DATE_COLUMN = "//td[contains(@ng-class,'willExpireSoon')]";
		public const string LICENSE_QUANTITY_COLUMN = "//td[(contains(text(),'license') or contains(text(),' licenses')) and not(contains(text(), 'You have been'))]"; 
		public const string LOGO = "//a[@id='logo']";
		public const string PACKAGE_PRICE = "//table[contains(@class, ' add-lic')]//tbody//tr[1]//td['*#*']";
	}
}
