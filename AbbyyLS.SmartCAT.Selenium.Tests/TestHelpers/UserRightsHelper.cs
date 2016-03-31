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

		public UserRightsHelper CreateGroupWithSpecificRights(string nickName, string groupName, RightsType right)
		{
			_workspacePage.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(nickName)
				.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(groupName);

			_addAccessRightDialog
				.AddRightToGroupAnyProject(right)
				.ClickSaveButton(groupName);

			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(groupName, nickName);

			return this;
		}

		private WorkspacePage _workspacePage;
		private UsersTab _usersTab;
		private NewGroupDialog _newGroupDialog;
		private GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		private AddAccessRightDialog _addAccessRightDialog;
	}
}
