using System;
using System.Globalization;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing
{
	public class BillingPage : WorkspacePage, IAbstractPage<BillingPage>
	{
		public BillingPage(WebDriver driver) : base(driver)
		{
		}

		public new BillingPage GetPage()
		{
			InitPage(this, Driver);

			return this;
		}

		public new void LoadPage()
		{
			if (!IsBillingPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница управления лицензиями.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Buy для определенного периода
		/// </summary>
		/// <param name="monthPeriod">период, на который покупается лицензия</param>
		public LicensePurchaseDialog ClickBuyButton(Period monthPeriod)
		{
			CustomTestContext.WriteLine("Нажать кнопку Buy для периода {0} месяцев.", monthPeriod.ToString());
			Driver.FindElement(By.XPath(BUY_BUTTON.Replace("'*#*'", monthPeriod.Description()))).Click();

			return new LicensePurchaseDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Upgrade
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public LicenseUpgradeDialog ClickUpgradeButton(int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать кнопку Upgrade в {0} строке.", rowNumber.ToString());
			// Получить список видимых кнопок Upgrade
			var upgradeButtonsList = Driver.GetElementList(By.XPath(UPGRADE_BUTTONS));
			var buttons = upgradeButtonsList.Where(btn => btn.Displayed).ToList();

			buttons[rowNumber - 1].Click();

			return new LicenseUpgradeDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Extend
		/// </summary>
		/// <param name="rowNumebr">номер строки</param>
		public LicenseExtendDialog ClickExtendButton(int rowNumebr = 1)
		{
			CustomTestContext.WriteLine("Нажать кнопку Extend в {0} строке.", rowNumebr.ToString());
			Driver.SetDynamicValue(How.XPath, EXTEND_BUTTON, rowNumebr.ToString()).Click();

			return new LicenseExtendDialog(Driver).GetPage();
		}

		/// <summary>
		/// Выбрать количество лицензий для покупки
		/// </summary>
		/// <param name="licenseNumber">количество лицензий</param>
		public BillingPage SelectLicenseNumber(int licenseNumber)
		{
			CustomTestContext.WriteLine("Выбрать {0} лицензий для покупки.", licenseNumber.ToString());
			LicenseNumber.SelectOptionByText(licenseNumber.ToString());

			return GetPage();
		}

		/// <summary>
		/// Нажать по логотипу
		/// </summary>
		public WorkspacePage ClickLogo()
		{
			CustomTestContext.WriteLine("Нажать по логотипу.");
			Logo.Click();

			return new WorkspacePage(Driver);
		}

		/// <summary>
		/// Получить количество пакетов лицензий
		/// </summary>
		/// <returns>количество лицензий</returns>
		public int PackagesCount()
		{
			CustomTestContext.WriteLine("Получить количество пакетов лицензий.");

			return Driver.GetElementsCount(By.XPath(LICENSES_LIST));
		}

		/// <summary>
		/// Получить конечную дату пакета лицензий.
		/// </summary>
		/// <returns>конечная дата</returns>
		public DateTime GetEndDate()
		{
			CustomTestContext.WriteLine("Получить конечную дату пакета лицензий.");

			return DateTime.ParseExact(EndDateColumn.Text, "M/d/yyyy", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Получить стоимость пакета лицензий из нижней таблицы
		/// </summary>
		/// <param name="period">период 1/3/6/12 месяцев</param>
		public int PackagePrice(Period period, int numberLicenses)
		{
			CustomTestContext.WriteLine("Получить стоимость пакета лицензий за {0} месяцев.", period.Description());

			SelectLicenseNumber(numberLicenses);

			var priceWithDot = Driver.FindElement(By.XPath(PACKAGE_PRICE.Replace("'*#*'", period.Description()))).Text;
			var price = priceWithDot.Substring(0, priceWithDot.IndexOf(".")).Replace(",", "");
			int resultPrice;

			if (!int.TryParse(price, out resultPrice))
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось преобразование стоимости пакета лицензий в число."));
			}
			
			return resultPrice;
		}

		/// <summary>
		/// Посчитать дополнительную плату за продление/обновление пакета лицензий
		/// </summary>
		/// <param name="currentPackagePrice">стоимость нового пакета лицензий</param>
		/// <param name="newPackagePrice"> стоимость старого пакета лицензий</param>
		/// <param name="totalPeriod">общий срок действия текущего пакета лицензий</param>
		/// <param name="packageValidityPeriod">оставшийся срок действия текущего пакета лицензий</param>
		public int CalculateAdditionalPayment(int currentPackagePrice, int newPackagePrice, Period totalPeriod, string packageValidityPeriod)
		{
			CustomTestContext.WriteLine("Посчитать дополнительную плату за продление/обновление пакета лицензий по формуле:"
						 + "\n Стоимость = n*(y-x)/k, Где n – количество оставшихся дней действия текущего пакета лицензий,"
						 + " k – общий срок действия текущего пакета лицензий, y – стоимость нового пакета лицензий, x – стоимость старого пакета лицензий.");

			var totalCountDays = (DateTime.Now.AddMonths((int)totalPeriod) - DateTime.Now).Days;

			return calculateLeftDays(packageValidityPeriod) * (newPackagePrice - currentPackagePrice) / totalCountDays;
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Открыть диалог покупки лицензий
		/// </summary>
		/// <param name="licenseNumber">кол-во лицензий</param>
		/// <param name="period">период</param>
		public LicensePurchaseDialog OpenLicensePurchaseDialog(int licenseNumber = 5, Period period = Period.ThreeMonth)
		{
			SelectLicenseNumber(licenseNumber);
			var licensePurchaseDialog = ClickBuyButton(period);

			return licensePurchaseDialog;
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница
		/// </summary>
		public bool IsBillingPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(LICENSE_NUMBER), timeout: 20);
		}

		/// <summary>
		/// Проверить, что количество пакетов лицензий соответствует ожидаемому
		/// </summary>
		/// <param name="expectedCount">ожидаемое кол-во пакетов лицензий</param>
		public bool IsPackagesCountEquals(int expectedCount)
		{
			CustomTestContext.WriteLine("Проверить, что количество пакетов лицензий равно {0}.", expectedCount);

			var actualCount = PackagesCount();

			return expectedCount == actualCount;
		}

		/// <summary>
		/// Проверить, что количество пакетов лицензий изменилось на ожидаемое
		/// </summary>
		/// <param name="expectedCount">ожидаемое кол-во пакетов лицензий</param>
		public bool IsLicensesCountChanged(int expectedCount)
		{
			CustomTestContext.WriteLine("Проверить, что количество пакетов лицензий изменилось на {0}.", expectedCount);

			var licenseNumber = LicenseQuantityColumn.Text.Split(' ');
			int licenseNumberInteger;

			if (!int.TryParse(licenseNumber[0], out licenseNumberInteger))
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось преобразование количества лицензий в пакете из первой колонки верхней таблицы {0} в число.", licenseNumber[0]));
			}

			return expectedCount == licenseNumberInteger;
		}

		/// <summary>
		/// Проверить знак валюты
		/// </summary>
		/// <param name="currency">валюта</param>
		public bool IsCurrencyInPurchaseTable(string currency)
		{
			CustomTestContext.WriteLine("Проверить, знак валюты {0}.", currency);
			var periodList = Driver.GetTextListElement(By.XPath(TABLE_HEADER));

			return periodList.All(p => p.Contains(currency));
		}

		#endregion

		#region Вспомогательные методы 

		/// <summary>
		/// Посчитать количество оставшихся дней действия текущего пакета лицензий
		/// </summary>
		private int calculateLeftDays(string packageValidityPeriod)
		{
			CustomTestContext.WriteLine("Посчитать количество оставшихся дней действия текущего пакета лицензий.");

			var periodArray = packageValidityPeriod.Split('—');

			DateTime startDate = DateTime.ParseExact(periodArray[0], "M/d/yyyy", CultureInfo.InvariantCulture);
			DateTime endDate = DateTime.ParseExact(periodArray[1], "M/d/yyyy", CultureInfo.InvariantCulture);

			return (endDate - startDate).Days;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = LICENSE_NUMBER)]
		protected IWebElement LicenseNumber { get; set; }

		[FindsBy(How = How.XPath, Using = END_DATE_COLUMN)]
		protected IWebElement EndDateColumn { get; set; }

		[FindsBy(How = How.XPath, Using = LICENSE_QUANTITY_COLUMN)]
		protected IWebElement LicenseQuantityColumn { get; set; }

		[FindsBy(How = How.XPath, Using = LOGO)]
		protected IWebElement Logo { get; set; }

		#endregion

		#region Описание XPath элементов

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

		#endregion
	}
}
