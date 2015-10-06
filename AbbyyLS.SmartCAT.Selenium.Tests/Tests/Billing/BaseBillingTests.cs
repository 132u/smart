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
			LoginHelper = new LoginHelper(Driver);
			CommonHelper = new CommonHelper(Driver);
			BillingHelper = new BillingHelper(Driver);
			LicenseDialogHelper = new LicenseDialogHelper(Driver);
			WorkspaceHelper = new WorkspaceHelper(Driver);

			var accountUniqueName = AdminHelper.GetAccountUniqueName();

			new AdminHelper(Driver).CreateAccountIfNotExist(
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
				.AddUserToSpecificAccount(ThreadUser.Login, accountUniqueName);

			CommonHelper.GoToSignInPage();
			LoginHelper.LogInSmartCat(
				ThreadUser.Login,
				ThreadUser.NickName,
				ThreadUser.Password,
				accountUniqueName);

			WorkspaceHelper.GoToBillingPage();
		}

		protected LoginHelper LoginHelper;
		protected CommonHelper CommonHelper;
		protected BillingHelper BillingHelper;
		protected LicenseDialogHelper LicenseDialogHelper;
		protected WorkspaceHelper WorkspaceHelper;
	}
}
