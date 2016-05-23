using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageUsersAndGroupsBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_addAccessRightDialog = new AddAccessRightDialog(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_userRightsHelper = new UserRightsHelper(Driver);
			_removeUserDialog = new RemoveUserDialog(Driver);
			_changeUserDataDialog = new ChangeUserDataDialog(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_usersTab = new UsersTab(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_addUserDialog = new AddUserDialog(Driver);
			_removeGroupDialog = new RemoveGroupDialog(Driver);

			var groupName = Guid.NewGuid().ToString();

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_userRightsHelper.CreateGroupWithoutSpecificRight(AdditionalUser.NickName, groupName, RightsType.UsersManagement);

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
		
		protected ProjectGroupsPage _projectGroupsPage;
		protected ClientsPage _clientsPage;
		protected LoginHelper _loginHelper;
		protected UserRightsHelper _userRightsHelper;
		protected WorkspacePage _workspacePage;
		protected AddAccessRightDialog _addAccessRightDialog;
		protected RemoveUserDialog _removeUserDialog;
		protected ChangeUserDataDialog _changeUserDataDialog;
		protected AddUserDialog _addUserDialog;
		protected GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		protected UsersTab _usersTab;
		protected NewGroupDialog _newGroupDialog;
		protected RemoveGroupDialog _removeGroupDialog;
	}
}
