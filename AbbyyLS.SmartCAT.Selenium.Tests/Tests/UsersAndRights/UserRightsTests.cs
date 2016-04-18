using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class UserRightsTests<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void Initialize()
		{
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_groupName = _groupsAndAccessRightsTab.GetGroupUniqueName();

			_workspacePage.GoToUsersPage();
		}

		[Test, Description("S-7124")]
		public void CreateGroupTest()
		{
			_groupsAndAccessRightsTab.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(_groupName);

			Assert.IsTrue(_groupsAndAccessRightsTab.IsGroupExists(_groupName),
				"Произошла ошибка:\n не удалось создать группу");
		}

		[Test]
		public void AddProjectResourceManagementRightToGroupTest()
		{
			_groupsAndAccessRightsTab
				.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(_groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroupAnyProject(RightsType.ProjectResourceManagement);

			Assert.IsTrue(_groupsAndAccessRightsTab.IsManageProjectsRightAdded(_groupName),
				"Произошла ошибка:\n не удалось добавить право на управление проектами ");
		}

		[Test]
		public void AddProjectCreationRightToGroupTest()
		{
			_groupsAndAccessRightsTab.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(_groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroupAnyProject(RightsType.ProjectCreation);

			Assert.IsTrue(_groupsAndAccessRightsTab.IsCreateProjectsRightAdded(_groupName),
				"Произошла ошибка:\n не удалось добавить право на управление проектами ");
		}

		[Test]
		public void AddProjectViewRightToGroupTest()
		{
			_groupsAndAccessRightsTab.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(_groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroupAnyProject(RightsType.ProjectView);

			Assert.IsTrue(_groupsAndAccessRightsTab.IsViewProjectsRightAdded(_groupName),
				"Произошла ошибка:\n не удалось добавить право на просмотр проектов ");
		}

		[Test]
		public void AddUserToGroup()
		{
			_groupsAndAccessRightsTab.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(_groupName);

			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(_groupName, ThreadUser.NickName);

			Assert.IsTrue(_groupsAndAccessRightsTab.IsUserExistInGroup(_groupName, ThreadUser.NickName),
				"Произошла ошибка:\n не удалось добавить пользователя в группу");
		}

		private WorkspacePage _workspacePage;
		private GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		private AddAccessRightDialog _addAccessRightDialog;
		protected NewGroupDialog _newGroupDialog;
		private string _groupName;
	}
}
