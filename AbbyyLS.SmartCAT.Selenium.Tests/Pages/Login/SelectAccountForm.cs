using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class SelectAccountForm : BaseObject, IAbstractPage<SelectAccountForm>
	{
		public WebDriver Driver { get; protected set; }

		public SelectAccountForm(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public SelectAccountForm GetPage()
		{
			var selectAccountForm = new SelectAccountForm(Driver);
			InitPage(selectAccountForm, Driver);

			return selectAccountForm;
		}

		public void LoadPage()
		{
			Driver.WaitUntilElementIsDisappeared(By.XPath(WAITING_SERVER_RESPONSE_MESSAGE));

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ACCOUNT_SELECTION_FORM), timeout: 30))
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась форма выбора аккаунта.");
			}
		}

		#region Простые методы страницы

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать аккаунт
		/// </summary>
		/// <param name="accountName">название аккаунта</param>
		/// <param name="dataServer">расположение сервера</param>
		public WorkspacePage SelectAccount(
			string accountName = LoginHelper.TestAccountName,
			string dataServer = LoginHelper.EuropeTestServerName)
		{
			CustomTestContext.WriteLine("Проверить кол-во ссылок на аккаунты на всех серверах.");
			var europeAccountsCount = Driver.GetElementsCount(By.XPath(EUROPE_ACCOUNT_LIST));
			var usaAccountsCount = Driver.GetElementsCount(By.XPath(USA_ACCOUNT_LIST));
			var totalAccountCount = europeAccountsCount + usaAccountsCount;

			CustomTestContext.WriteLine("Ссылок на аккаунты на всех серверах '{0}'", totalAccountCount);

			if (totalAccountCount > 1)
			{
				CustomTestContext.WriteLine("Выбрать аккаунт {0} на сервере {1}.", accountName, dataServer);
				AccountRef = Driver.SetDynamicValue
					(How.XPath, dataServer.ToLower() == "europe" ? RU_ACCOUNT_REF_XPATH : US_ACCOUNT_REF_XPATH, accountName);

				AccountRef.JavaScriptClick();
			}

			return new WorkspacePage(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что сервер Europe отвечает
		/// </summary>
		public bool IsEuropeServerRespond()
		{
			CustomTestContext.WriteLine("Проверить, что сервер Europe отвечает.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(EUROPE_HEADER));
		}

		/// <summary>
		/// Проверить наличие сообщения о ненайденном аккаунте
		/// </summary>
		public bool IsChooseProfileFormDisplayed()
		{
			CustomTestContext.WriteLine("Проверить наличие сообщения о ненайденном аккаунте.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(FREELANCE_PROFILE)) && Driver.WaitUntilElementIsDisplay(By.XPath(CORPORATE_PROFILE));
		}

		#endregion

		#region Объявление элементов страницы

		protected IWebElement AccountRef { get; set; }

		#endregion

		#region Описание Xpath элементов

		protected const string ACCOUNT_SELECTION_FORM = "//form[contains(@name, 'selectAccount')]";
		protected const string US_ACCOUNT_REF_XPATH = "//li[@translate = 'region-us']/following-sibling::li[@class='ng-scope']//span[contains(string(), '*#*')]";
		protected const string RU_ACCOUNT_REF_XPATH = "//li[@translate = 'region-ru']/following-sibling::li[@class='ng-scope']//span[string() = '*#*']";
		protected const string EUROPE_ACCOUNT_LIST = "//li[@translate='region-ru']//following-sibling::li//a[contains(@ng-click,'signInAccount')]";
		protected const string USA_ACCOUNT_LIST = "//li[@translate='region-us']//following-sibling::li//a[contains(@ng-click,'signInAccount')]";
		protected const string WAITING_SERVER_RESPONSE_MESSAGE = "//div[@ng-show='accountWatitngServerResponse']/span";
		protected const string FREELANCE_PROFILE = "//div[contains(@data-bind, 'chooseFreelancerProfile')]";
		protected const string CORPORATE_PROFILE = "//div[contains(@data-bind, 'chooseCorporateProfile')]";

		protected const string EUROPE_HEADER = "//li[@translate='region-ru']";

		#endregion
	}
}
