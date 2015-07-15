using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class SelectAccountForm : BaseObject, IAbstractPage<SelectAccountForm>
	{
		public SelectAccountForm GetPage()
		{
			var selectAccountForm = new SelectAccountForm();
			InitPage(selectAccountForm);

			return selectAccountForm;
		}

		public void LoadPage()
		{
			Driver.WaitUntilElementIsDisappeared(By.XPath(WAITING_SERVER_RESPONSE_MESSAGE));

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ACCOUNT_SELECTION_FORM), timeout: 30))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась форма выбора аккаунта.");
			}
		}

		/// <summary>
		/// Выбрать аккаунт
		/// </summary>
		/// <param name="accountName">название аккаунта</param>
		/// <param name="dataServer">расположение сервера</param>
		public WorkspacePage SelectAccount(string accountName, string dataServer)
		{
			Logger.Trace("Проверить кол-во ссылок на аккаунты на всех серверах.");
			var europeAccountsCount = Driver.GetElementsCount(By.XPath(EUROPE_ACCOUNT_LIST));
			var usaAccountsCount = Driver.GetElementsCount(By.XPath(USA_ACCOUNT_LIST));
			var totalAccountCount = europeAccountsCount + usaAccountsCount;

			Logger.Trace("Ссылок на аккаунты на всех серверах '{0}'", totalAccountCount);

			if (totalAccountCount > 1)
			{
				Logger.Debug("Выбрать аккаунт {0} на сервере {1}.", accountName, dataServer);
				AccountRef = Driver.SetDynamicValue
					(How.XPath, dataServer.ToLower() == "europe" ? RU_ACCOUNT_REF_XPATH : US_ACCOUNT_REF_XPATH, accountName);

				AccountRef.JavaScriptClick();
			}

			return new WorkspacePage().GetPage();
		}

		public SelectAccountForm AssertEuropeServerRespond()
		{
			Logger.Trace("Проверить, что сервер Europe отвечает.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(EUROPE_HEADER)),
				"Произошла ошибка:\n сервер Europe не отвечает.");

			return GetPage();
		}

		protected IWebElement AccountRef { get; set; }
		
		protected const string ACCOUNT_SELECTION_FORM = "//form[contains(@name, 'selectAccount')]";
		protected const string US_ACCOUNT_REF_XPATH = "//li[@translate = 'region-us']/following-sibling::li[@class='ng-scope']//span[contains(string(), '*#*')]";
		protected const string RU_ACCOUNT_REF_XPATH = "//li[@translate = 'region-ru']/following-sibling::li[@class='ng-scope']//span[string() = '*#*']";
		protected const string EUROPE_ACCOUNT_LIST = "//li[@translate='region-ru']//following-sibling::li//a[contains(@ng-click,'signInAccount')]";
		protected const string USA_ACCOUNT_LIST = "//li[@translate='region-us']//following-sibling::li//a[contains(@ng-click,'signInAccount')]";
		protected const string WAITING_SERVER_RESPONSE_MESSAGE = "//div[@ng-show='accountWatitngServerResponse']/span";

		protected const string EUROPE_HEADER = "//li[@translate='region-ru']";
	}
}
