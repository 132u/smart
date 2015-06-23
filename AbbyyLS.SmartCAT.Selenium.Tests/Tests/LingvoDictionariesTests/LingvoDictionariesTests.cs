using System.Collections.Generic;
using NUnit.Framework;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.LingvoDictionariesTests
{
	[Standalone]
	[LingvoDictionaries]
	class LingvoDictionariesTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public LingvoDictionariesTests()
		{
			AdminLoginPage = true;
		}

		[SetUp]
		public void LingvoDictionariesSetUp()
		{
			var accountUniqueName = AdminHelper.GetAccountUniqueName();

			_adminHelper
				.CreateAccountIfNotExist(
					accountName: accountUniqueName,
					workflow: true,
					features: new List<string>
					{
						Feature.Clients.ToString(),
						Feature.Domains.ToString(),
						Feature.TranslateConnector.ToString(),
						Feature.LingvoDictionaries.ToString(),
					})
				.AddUserToSpecificAccount(Login, accountUniqueName);

			LogInSmartCat(Login, Password, accountUniqueName);

			_workspaceHelper
				.AssertLingvoDictionariesDisplayed()
				.GoToLingvoDictionariesPage()
				.AssertLingvoDictionariesListIsNotEmpty();

			_workspaceHelper.GotToSearchPage();
		}

		[Test]
		public void TranslationReferenceTest()
		{
			_searchHelper
				.InitSearch("tester")
				.AssertTranslationReferenceExist();
		}

		[Test]
		public void DefinitionsTabActiveTest()
		{
			_searchHelper
				.SetSourceLanguage("en")
				.SetTargetLanguage("en")
				.InitSearch("tester")
				.AssertDefinitionTabIsActive();
		}

		[Test]
		public void ReverseTranslationTest()
		{
			_searchHelper
				.InitSearch("tester")
				.ClickTranslationWord()
				.OpenWordByWordTranslation()
				.AssertReverseTranslationListExist();
		}

		[Test]
		public void AutoReverseTest()
		{
			_searchHelper
				.SetSourceLanguage("ru")
				.SetTargetLanguage("en")
				.InitSearch("tester")
				.AssertAutoreversedMessageExist()
				.AsserttAutoreversedReferenceExist();
		}

		private readonly AdminHelper _adminHelper = new AdminHelper() ;
		private readonly SearchHelper _searchHelper = new SearchHelper();
		private readonly WorkspaceHelper _workspaceHelper = new WorkspaceHelper();
	}
}