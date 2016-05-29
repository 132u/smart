using System.Collections.Generic;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class UserRightsHelper
	{
		public WebDriver Driver { get; private set; }

		public UserRightsHelper(WebDriver driver)
		{
			Driver = driver;

			_workspacePage = new WorkspacePage(Driver);
			_usersTab = new UsersTab(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);
		}

		public UserRightsHelper CreateGroupWithoutSpecificRight(string fullName, string groupName, RightsType right)
		{
			_workspacePage.GoToUsersPage();
			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(fullName)
				.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(groupName);

			_addAccessRightDialog
				.ClickRightRadio(right)
				.ClickAddRightButton();

			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(groupName, fullName);

			return this;
		}

		public UserRightsHelper CreateGroupWithSpecificRights(string fullName, string groupName, List<RightsType> rights)
		{
			_workspacePage.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(fullName)
				.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(groupName);
			
			for (int i = 0; i < rights.Count; i++)
			{
				_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(groupName);

				_addAccessRightDialog
					.AddRightToGroupAnyProject(rights[i])
					.ClickSaveButton(groupName);
			}

			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(groupName, fullName);

			return this;
		}

		public UserRightsHelper CreateGroupWithSpecificRightsAndSpecificClient(string fullName, string groupName, RightsType right, string client)
		{
			_workspacePage.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(fullName)
				.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(groupName);

			_addAccessRightDialog.AddRightToGroupSpecificClient(right, client);

			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(groupName, fullName);

			return this;
		}

		private WorkspacePage _workspacePage;
		private UsersTab _usersTab;
		private NewGroupDialog _newGroupDialog;
		private GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		private AddAccessRightDialog _addAccessRightDialog;
	}
}
