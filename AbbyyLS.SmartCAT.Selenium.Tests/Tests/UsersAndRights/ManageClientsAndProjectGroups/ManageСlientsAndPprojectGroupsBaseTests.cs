using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	class ManageСlientsAndPprojectGroupsBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_userRightsHelper = new UserRightsHelper(Driver);
			_projectGroupsPage = new ProjectGroupsPage(Driver);
			_clientsPage = new ClientsPage(Driver);

			var groupName = Guid.NewGuid().ToString();

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_userRightsHelper.CreateGroupWithoutSpecificRight(AdditionalUser.NickName, groupName, RightsType.ClientsAndDomainsManagement);

			_workspacePage.SignOut();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}
		}

		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);

				_loginHelper = new LoginHelper(Driver);
				_loginHelper.Authorize(StartPage, AdditionalUser);
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}

		protected string _projectGroup;
		protected string _clientName;

		protected ProjectGroupsPage _projectGroupsPage;
		protected ClientsPage _clientsPage;
		protected LoginHelper _loginHelper;
		protected UserRightsHelper _userRightsHelper;
		protected WorkspacePage _workspacePage;
		protected GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		protected AddAccessRightDialog _addAccessRightDialog;
	}
}
