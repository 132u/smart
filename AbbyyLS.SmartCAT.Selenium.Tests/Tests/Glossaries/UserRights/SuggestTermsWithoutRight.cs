﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	class SuggestTermsWithoutRight<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public SuggestTermsWithoutRight()
		{
			StartPage = StartPage.SignIn;
		}

		[SetUp]
		public void SetUp()
		{
			_glossaryName = GlossariesHelper.UniqueGlossaryName();

			_additionalUser = TakeUser(ConfigurationManager.AdditionalUsers);
			_loginHelper.LogInSmartCat(
				_additionalUser.Login,
				_additionalUser.NickName,
				_additionalUser.Password,
				"Personal");
		}

		[TearDown]
		public void TearDown()
		{
			ReturnUser(ConfigurationManager.AdditionalUsers, _additionalUser);
		}

		[Test]
		public void SuggestTermsRightPersonalAccountTest()
		{
			_workspaceHelper
				.GoToGlossariesPage()
				.AssertSuggestedTermsButtonNotExist(glossariesPage: true)
				.AssertSuggestTermButtonNotExist(glossariesPage: true)
				.CreateGlossary(_glossaryName)
				.AssertSuggestedTermsButtonNotExist(glossariesPage: false)
				.AssertSuggestTermButtonNotExist(glossariesPage: false);
		}

		private string _glossaryName;
		private readonly WorkspaceHelper _workspaceHelper = new WorkspaceHelper();
		private readonly LoginHelper _loginHelper = new LoginHelper();
		private TestUser _additionalUser;
	}
}