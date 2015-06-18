﻿﻿using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
﻿using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
﻿using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.LingvoDictionariesTests
{
	[TestFixture]
	//[PriorityMajor]
	class LingvoDictionariesAdminTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public LingvoDictionariesAdminTests() 
		{
			AdminLoginPage = true;
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
				.AddUserToSpecificAccount(Login, _accountUniqueName);

			LogInSmartCat(Login, Password, _accountUniqueName);

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
				.AddUserToSpecificAccount(Login, _accountUniqueName)
				.GoToDictionaryPackagePage(AdminHelper.PublicDictionaryPackageName);

			IList<string> includedDictionaryList = _adminHelper.GetIncludedDictionariesList();

			LogInSmartCat(Login, Password, _accountUniqueName);

			_workspaceHelper
				.GoToLingvoDictionariesPage()
				.AssertLingvoDictionariesListsMatch(includedDictionaryList);
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
				.AddUserToSpecificAccount(Login, _accountUniqueName);

			LogInSmartCat(Login, Password, _accountUniqueName);

			_workspaceHelper.AssertLingvoDictionariesIsNotDisplayed();
		}

		[Test]
		public void AddLingvoDictionariesPackagesViaAccountEditModeTest()
		{
			_adminHelper
				.CreateAccountIfNotExist(accountName: _accountUniqueName, features: new List<string> { Feature.LingvoDictionaries.ToString()})
				.AddUserToSpecificAccount(Login, _accountUniqueName)
				.OpenEditModeForEnterpriceAccount(_accountUniqueName)
				.AddAllDictionariesPackages();

			LogInSmartCat(Login, Password, _accountUniqueName);

			_workspaceHelper
				.AssertLingvoDictionariesDisplayed()
				.GoToLingvoDictionariesPage()
				.AssertLingvoDictionariesListIsNotEmpty();
		}

		private string _accountUniqueName;
		private readonly AdminHelper _adminHelper = new AdminHelper();
		private readonly WorkspaceHelper _workspaceHelper = new WorkspaceHelper();
	}
}