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
			LoadPage();

			return selectAccountForm;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.Id(SIGN_OUT_BTN_ID)))
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
			Logger.Trace("Выбираем аккаунт {0} на сервере {1}.", accountName, dataServer);
			AccountRef = Driver.SetDynamicValue
				(How.XPath, dataServer.ToLower() == "europe" ? RU_ACCOUNT_REF_XPATH : US_ACCOUNT_REF_XPATH, accountName);

			AccountRef.Click();
			var workspacePage = new WorkspacePage();

			return workspacePage.GetPage();
		}

		protected IWebElement AccountRef { get; set; }

		protected const string SIGN_OUT_BTN_ID = "btn-signout";
		protected const string US_ACCOUNT_REF_XPATH = "//li[@translate = 'region-us']/following-sibling::li[@class='ng-scope']//span[contains(string(), '*#*')]";
		protected const string RU_ACCOUNT_REF_XPATH = "//li[@translate = 'region-ru']/following-sibling::li[@class='ng-scope']//span[string() = '*#*']";
	}
}
