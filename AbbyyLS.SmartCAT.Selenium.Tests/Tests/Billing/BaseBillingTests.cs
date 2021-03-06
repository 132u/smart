﻿using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing.LicenseDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
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
			SignInPage = new SignInPage(Driver);
			WorkspacePage = new WorkspacePage(Driver);
			BillingPage = new BillingPage(Driver);
			LicenseBaseDialog = new LicenseBaseDialog(Driver);
			LicenseExtendDialog = new LicenseExtendDialog(Driver);
			LicensePurchaseDialog = new LicensePurchaseDialog(Driver);
			LicensePaymentDialog = new LicensePaymentDialog(Driver);
			LicenseTrialDialog = new LicenseTrialDialog(Driver);
			LicenseUpgradeDialog = new LicenseUpgradeDialog(Driver);
			LicensePurchaseCompleteDialog = new LicensePurchaseCompleteDialog(Driver);
			LicenseUpgradeCompleteDialog = new LicenseUpgradeCompleteDialog(Driver);
			LicenseExtendCompleteDialog = new LicenseExtendCompleteDialog(Driver);

			var accountUniqueName = AdminHelper.GetAccountUniqueName();

			new AdminHelper(Driver).CreateAccountIfNotExist(
				accountName: accountUniqueName,
				workflow: true,
				features:
					new List<string>
					{
						Feature.Clients.ToString(),
						Feature.Domains.ToString(),
						Feature.LingvoDictionaries.ToString(),
					})
				.AddUserToAdminGroupInAccountIfNotAdded(
					ThreadUser.Login, ThreadUser.Name, ThreadUser.Surname, accountUniqueName);

			SignInPage.GetPage();

			LoginHelper.LogInSmartCat(
				ThreadUser.Login,
				ThreadUser.NickName,
				ThreadUser.Password,
				accountUniqueName);

			WorkspacePage.GoToBillingPage();
		}

		protected LoginHelper LoginHelper;
		protected WorkspacePage WorkspacePage;
		protected SignInPage SignInPage;
		public BillingPage BillingPage;
		public LicenseBaseDialog LicenseBaseDialog;
		public LicenseExtendDialog LicenseExtendDialog;
		public LicensePurchaseDialog LicensePurchaseDialog;
		public LicensePaymentDialog LicensePaymentDialog;
		public LicenseTrialDialog LicenseTrialDialog;
		public LicenseUpgradeDialog LicenseUpgradeDialog;
		public LicensePurchaseCompleteDialog LicensePurchaseCompleteDialog;
		public LicenseUpgradeCompleteDialog LicenseUpgradeCompleteDialog;
		public LicenseExtendCompleteDialog LicenseExtendCompleteDialog;
	}
}
