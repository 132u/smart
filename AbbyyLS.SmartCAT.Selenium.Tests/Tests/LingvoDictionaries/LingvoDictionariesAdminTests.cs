﻿using System.Collections.Generic;

﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
﻿using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
﻿using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
﻿using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.LingvoDictionaries
{
	[LingvoDictionaries]
	[PriorityMajor]
	class LingvoDictionariesAdminTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public LingvoDictionariesAdminTests() 
		{
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void LingvoDictionariesSetUp()
		{
			_accountUniqueName = AdminHelper.GetAccountUniqueName();
		}

		[Test]
		public void EnableLingvoDictionariesFeatureWithPackagesTest()
		{
			_adminHelper
				.CreateAccountIfNotExist(
					accountName : _accountUniqueName,
					features: new List<string> { Feature.LingvoDictionaries.ToString() },
					packagesNeed: true)
				.AddUserToSpecificAccount(ConfigurationManager.Login, _accountUniqueName);

			_commonHelper.GoToSignInPage();
			_loginHelper.LogInSmartCat(
				ConfigurationManager.Login,
				ConfigurationManager.NickName,
				ConfigurationManager.Password,
				_accountUniqueName);

			_workspaceHelper
				.AssertLingvoDictionariesDisplayed()
				.GoToLingvoDictionariesPage()
				.AssertLingvoDictionariesListIsNotEmpty();
		}

		[Test]
		public void EnableLingvoDictionariesFeatureWithoutPackagesTest()
		{
			_adminHelper
				.CreateAccountIfNotExist(
					accountName: _accountUniqueName,
					features: new List<string> { Feature.LingvoDictionaries.ToString() })
				.AddUserToSpecificAccount(ConfigurationManager.Login, _accountUniqueName)
				.GoToDictionaryPackagePage(AdminHelper.PublicDictionaryPackageName);

			List<string> includedDictionaryList = _adminHelper.GetIncludedDictionariesList();

			_commonHelper.GoToSignInPage();
			_loginHelper.LogInSmartCat(
				ConfigurationManager.Login,
				ConfigurationManager.NickName,
				ConfigurationManager.Password,
				_accountUniqueName);

			_workspaceHelper
				.GoToLingvoDictionariesPage()
				.AssertLingvoDictionariesListCorrect(includedDictionaryList);
		}

		[Test]
		public void DisableLingvoDictionariesFeatureTest()
		{
			_adminHelper.CreateAccountIfNotExist(
				accountName: _accountUniqueName,
				workflow: true,
				features: new List<string>
				{
					Feature.Clients.ToString(),
					Feature.Domains.ToString(),
					Feature.TranslateConnector.ToString(),
				})
				.AddUserToSpecificAccount(ConfigurationManager.Login, _accountUniqueName);

			_commonHelper.GoToSignInPage();
			_loginHelper.LogInSmartCat(
				ConfigurationManager.Login,
				ConfigurationManager.NickName,
				ConfigurationManager.Password,
				_accountUniqueName);

			_workspaceHelper.AssertLingvoDictionariesIsNotDisplayed();
		}

		[Test]
		public void AddLingvoDictionariesPackagesViaAccountEditModeTest()
		{
			_adminHelper
				.CreateAccountIfNotExist(accountName: _accountUniqueName, features: new List<string> { Feature.LingvoDictionaries.ToString()})
				.AddUserToSpecificAccount(ConfigurationManager.Login, _accountUniqueName)
				.OpenEditModeForEnterpriceAccount(_accountUniqueName)
				.AddAllDictionariesPackages();

			_commonHelper.GoToSignInPage();
			_loginHelper.LogInSmartCat(
				ConfigurationManager.Login,
				ConfigurationManager.NickName,
				ConfigurationManager.Password,
				_accountUniqueName);

			_workspaceHelper
				.AssertLingvoDictionariesDisplayed()
				.GoToLingvoDictionariesPage()
				.AssertLingvoDictionariesListIsNotEmpty();
		}

		private string _accountUniqueName;
		private readonly AdminHelper _adminHelper = new AdminHelper();
		private readonly WorkspaceHelper _workspaceHelper = new WorkspaceHelper();
		private readonly LoginHelper _loginHelper = new LoginHelper();
		private readonly CommonHelper _commonHelper = new CommonHelper();
	}
}