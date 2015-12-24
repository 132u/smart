﻿using System.Collections.Generic;

﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
﻿using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
﻿using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
﻿using AbbyyLS.SmartCAT.Selenium.Tests.Pages.LingvoDictionaries;
﻿using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.LingvoDictionaries
{
	[Parallelizable(ParallelScope.Fixtures)]
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
			_adminHelper = new AdminHelper(Driver);
			_commonHelper = new CommonHelper(Driver);
			_loginHelper = new LoginHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_lingvoDictionariesPage = new LingvoDictionariesPage(Driver);
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
				.AddUserToSpecificAccount(ThreadUser.Login, _accountUniqueName);

			_commonHelper.GoToSignInPage();
			_loginHelper.LogInSmartCat(
				ThreadUser.Login,
				ThreadUser.NickName,
				ThreadUser.Password,
				_accountUniqueName);

			_workspaceHelper
				.AssertLingvoDictionariesDisplayed()
				.GoToLingvoDictionariesPage();

			Assert.IsTrue(_lingvoDictionariesPage.IsLingvoDictionariesListNotEmpty(),
				"Произошла ошибка:\n список словарей пуст.");
		}

		[Test]
		public void EnableLingvoDictionariesFeatureWithoutPackagesTest()
		{
			_adminHelper
				.CreateAccountIfNotExist(
					accountName: _accountUniqueName,
					features: new List<string> { Feature.LingvoDictionaries.ToString() })
				.AddUserToSpecificAccount(ThreadUser.Login, _accountUniqueName)
				.GoToDictionaryPackagePage(AdminHelper.PublicDictionaryPackageName);

			List<string> includedDictionaryList = _adminHelper.GetIncludedDictionariesList();

			_commonHelper.GoToSignInPage();

			_loginHelper.LogInSmartCat(
				ThreadUser.Login,
				ThreadUser.NickName,
				ThreadUser.Password,
				_accountUniqueName);

			_workspaceHelper.GoToLingvoDictionariesPage();

			Assert.IsTrue(_lingvoDictionariesPage.IsLingvoDictionariesListMatchExpected(includedDictionaryList),
				"Произошла ошибка:\n список словарей на странице не совпадает с ожидаемым");
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
					Feature.Domains.ToString()
				})
				.AddUserToSpecificAccount(ThreadUser.Login, _accountUniqueName);

			_commonHelper.GoToSignInPage();

			_loginHelper.LogInSmartCat(
				ThreadUser.Login,
				ThreadUser.NickName,
				ThreadUser.Password,
				_accountUniqueName);

			_workspaceHelper.AssertLingvoDictionariesIsNotDisplayed();
		}

		[Test]
		public void AddLingvoDictionariesPackagesViaAccountEditModeTest()
		{
			_adminHelper
				.CreateAccountIfNotExist(accountName: _accountUniqueName, features: new List<string> { Feature.LingvoDictionaries.ToString()})
				.AddUserToSpecificAccount(ThreadUser.Login, _accountUniqueName)
				.OpenEditModeForEnterpriceAccount(_accountUniqueName)
				.AddAllDictionariesPackages();

			_commonHelper.GoToSignInPage();

			_loginHelper.LogInSmartCat(
				ThreadUser.Login,
				ThreadUser.NickName,
				ThreadUser.Password,
				_accountUniqueName);

			_workspaceHelper
				.AssertLingvoDictionariesDisplayed()
				.GoToLingvoDictionariesPage();

			Assert.IsTrue(_lingvoDictionariesPage.IsLingvoDictionariesListNotEmpty(),
				"Произошла ошибка:\n список словарей пуст.");
		}

		private string _accountUniqueName;
		private AdminHelper _adminHelper;
		private WorkspaceHelper _workspaceHelper;
		private LoginHelper _loginHelper;
		private CommonHelper _commonHelper;
		private LingvoDictionariesPage _lingvoDictionariesPage;
	}
}