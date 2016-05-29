using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageUserGroupsTests<TWebDriverProvider> : ManageUsersAndGroupsBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void BeforeTest()
		{
			_groupName = Guid.NewGuid().ToString();

			_workspacePage.GoToUsersPage();
			_usersTab.ClickGroupsButton();
			
			_groupsAndAccessRightsTab.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(_groupName);
		}
		
		[Test]
		public void CreateGroupTest()
		{
			Assert.IsTrue(_groupsAndAccessRightsTab.IsGroupExists(_groupName),
				"Произошла ошибка: отсутствует группа {0}.", _groupName);
		}
		
		[Test]
		public void EditGroupTest()
		{
			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog
				.AddRightToGroupAnyProject(RightsType.GlossaryManagement)
				.ClickSaveButton(_groupName);

			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(_groupName, ThreadUser.FullName);
		}

		[Test]
		public void DeleteGroupTest()
		{
			_groupsAndAccessRightsTab.OpenRemoveGroupDialog(_groupName);
			_removeGroupDialog.ClickDeleteButton();

			Assert.IsFalse(_groupsAndAccessRightsTab.IsGroupExists(_groupName),
				"Произошла ошибка: группа {0} не удалилась.", _groupName);
		}
		
		protected string _groupName;
	}
}
