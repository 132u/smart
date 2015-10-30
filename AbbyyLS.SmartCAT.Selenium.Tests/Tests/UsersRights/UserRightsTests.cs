using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class UserRightsTests<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void Initialize()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);
			_usersRightsPage = new UsersRightsPage(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);

			_workspaceHelper.GoToUsersRightsPage();

			_groupName = _usersRightsPage.GetGroupUniqueName();
		}

		[Test]
		public void CreateGroupTest()
		{
			_usersRightsPage.CreateGroupIfNotExist(_groupName);

			Assert.IsTrue(_usersRightsPage.IsGroupExists(_groupName),
				"Произошла ошибка:\n не удалось создать группу");
		}

		[Test]
		public void AddProjectResourceManagementRightToGroupTest()
		{
			_usersRightsPage
				.CreateGroupIfNotExist(_groupName)
				.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroup(RightsType.ProjectResourceManagement);

			Assert.IsTrue(_usersRightsPage.IsManageProjectsRightAdded(_groupName),
				"Произошла ошибка:\n не удалось добавить право на управление проектами ");
		}

		[Test]
		public void AddProjectCreationRightToGroupTest()
		{
			_usersRightsPage
				.CreateGroupIfNotExist(_groupName)
				.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroup(RightsType.ProjectCreation);

			Assert.IsTrue(_usersRightsPage.IsCreateProjectsRightAdded(_groupName),
				"Произошла ошибка:\n не удалось добавить право на управление проектами ");
		}

		[Test]
		public void AddProjectViewRightToGroupTest()
		{
			_usersRightsPage
				.CreateGroupIfNotExist(_groupName)
				.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroup(RightsType.ProjectView);

			Assert.IsTrue(_usersRightsPage.IsViewProjectsRightAdded(_groupName),
				"Произошла ошибка:\n не удалось добавить право на просмотр проектов ");
		}

		[Test]
		public void AddUserToGroup()
		{
			_usersRightsPage
				.CreateGroupIfNotExist(_groupName)
				.AddUserToGroupIfNotAlredyAdded(_groupName, ThreadUser.NickName);

			Assert.IsTrue(_usersRightsPage.IsUserExistInGroup(_groupName, ThreadUser.NickName),
				"Произошла ошибка:\n не удалось добавить пользователя в группу");
		}

		private WorkspaceHelper _workspaceHelper;
		private UsersRightsPage _usersRightsPage;
		private AddAccessRightDialog _addAccessRightDialog;

		private string _groupName;
	}
}
