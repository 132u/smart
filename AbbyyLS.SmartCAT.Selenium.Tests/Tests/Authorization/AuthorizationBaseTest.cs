﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Authorization
{
	internal class AuthorizationBaseTest<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void Initialization()
		{
			_adminHelper = new AdminHelper(Driver);
			_commonHelper = new CommonHelper(Driver);

			_signInPage = new SignInPage(Driver);
			_facebookPage = new FacebookPage(Driver);
			_googlePage = new GooglePage(Driver);
			_linkedInPage = new LinkedInPage(Driver);
			_selectAccountForm = new SelectAccountForm(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_selectProfileForm = new SelectProfileForm(Driver);
		}

		protected AdminHelper _adminHelper;
		protected CommonHelper _commonHelper;
		protected SignInPage _signInPage;
		protected FacebookPage _facebookPage;
		protected GooglePage _googlePage;
		protected LinkedInPage _linkedInPage;
		protected SelectAccountForm _selectAccountForm;
		protected SelectProfileForm _selectProfileForm;
		protected WorkspacePage _workspacePage;
	}
}