using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class UsersAndAccessRightGroupsBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public virtual void OneTimeSetUp()
		{
			_addAccessRightDialog = new AddAccessRightDialog(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_userRightsHelper = new UserRightsHelper(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_usersTab = new UsersTab(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_addUserDialog = new AddUserDialog(Driver);
			_signInPage = new SignInPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_adminHelper = new AdminHelper(Driver);
			_accountInvitationPage = new AccountInvitationPage(Driver);
			_adminSignInPage = new AdminSignInPage(Driver);
		}

		[SetUp]
		public virtual void BeforeTest()
		{
			_name = Guid.NewGuid().ToString();
			_surname = Guid.NewGuid().ToString();
			_password = Guid.NewGuid().ToString();
			_email = Guid.NewGuid() + "@mailforspam.com";
			_today = DateTime.Today.Date;
			_nickName = _name + " " + _surname;
			_groupName = Guid.NewGuid().ToString();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}
		}

		protected string _password;
		protected string _name;
		protected string _surname;
		protected string _email;
		protected string _nickName;
		protected DateTime _today;
		protected string _groupName;

		protected AdminSignInPage _adminSignInPage;
		protected LoginHelper _loginHelper;
		protected UserRightsHelper _userRightsHelper;
		protected WorkspacePage _workspacePage;
		protected AddAccessRightDialog _addAccessRightDialog;
		protected AddUserDialog _addUserDialog;
		protected GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		protected UsersTab _usersTab;
		protected NewGroupDialog _newGroupDialog;
		protected SignInPage _signInPage;
		protected ProjectsPage _projectsPage;
		protected AdminHelper _adminHelper;
		protected AccountInvitationPage _accountInvitationPage;
	}
}
