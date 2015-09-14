using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Billing
{
	public class BaseBillingTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>where TWebDriverProvider : IWebDriverProvider, new()
	{
		public BaseBillingTests()
		{
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void SetUpBaseBillingTests()
		{
			var accountUniqueName = AdminHelper.GetAccountUniqueName();

			new AdminHelper().CreateAccountIfNotExist(
				accountName: accountUniqueName,
				workflow: true,
				features:
					new List<string>
					{
						Feature.Clients.ToString(),
						Feature.Domains.ToString(),
						Feature.TranslateConnector.ToString(),
						Feature.LingvoDictionaries.ToString(),
					})
				.AddUserToSpecificAccount(ConfigurationManager.Login, accountUniqueName);

			CommonHelper.GoToSignInPage();
			LoginHelper.LogInSmartCat(
				ConfigurationManager.Login,
				ConfigurationManager.NickName,
				ConfigurationManager.Password,
				accountUniqueName);

			WorkspaceHelper.GoToBillingPage();
		}

		protected readonly LoginHelper LoginHelper = new LoginHelper();
		protected readonly CommonHelper CommonHelper = new CommonHelper();
		protected readonly BillingHelper BillingHelper = new BillingHelper();
		protected readonly LicenseDialogHelper LicenseDialogHelper = new LicenseDialogHelper();
		protected readonly WorkspaceHelper WorkspaceHelper = new WorkspaceHelper();
	}
}
