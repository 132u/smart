using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class UsersAndAccessRightGroupsTest<TWebDriverProvider> : UsersAndAccessRightGroupsBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void BeforeTest()
		{
			_workspacePage.GoToUsersPage();
		}

		[Test, Description("S-7123"), ShortCheckList]
		public void CreateAndActivateNewUserTest()
		{
			_usersTab.ClickAddUserButton();

			_addUserDialog.AddUser(_name, _surname, _email);

			Assert.AreEqual("Invitation sent", _usersTab.GetStatus(_name),
				"Произошла ошибка: неверный статус пользователя {0}.", _name);

			_workspacePage.SignOut();

			_loginHelper.Authorize(StartPage.Admin, ThreadUser);

			_adminHelper.ActivateUser(_email);

			Assert.IsTrue(_accountInvitationPage.IsAccountInvitationPageOpened(),
				"Произошла ошибка: не открылась страница завершения активации пользователя.");

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToUsersPage();

			Assert.AreEqual("Active", _usersTab.GetStatus(_name),
				"Произошла ошибка: неверный статус пользователя {0} после активации.", _name);
		}

		[Test, Description("S-7053"), ShortCheckList]
		public void CreateNewUserAndLogInTest()
		{
			_usersTab.ClickAddUserButton();

			_addUserDialog.AddUser(_name, _surname, _email);

			Assert.AreEqual("Invitation sent", _usersTab.GetStatus(_name),
				"Произошла ошибка: неверный статус пользователя {0}.", _name);

			_workspacePage.SignOut();

			_loginHelper.Authorize(StartPage.Admin, ThreadUser);

			_adminHelper.ActivateUser(_email);

			Assert.AreEqual(_email, _accountInvitationPage.GetEmail(), 
				"Произошла ошибка: неверный email.");

			var password = _accountInvitationPage.GetPassword();
			var email = _accountInvitationPage.GetEmail();

			_signInPage
				.GetPage()
				.SubmitFormExpectingWorkspacePage(email, password);

			Assert.IsTrue(_workspacePage.IsWorkspacePageOpened(),
				"Произошла ошибка: не открылась страница WS.");
		}

		[Test, Description("S-13741"), ShortCheckList]
		public void CreateNewUserCheckRightsTest()
		{
			_usersTab.ClickAddUserButton();

			_addUserDialog.AddUser(_name, _surname, _email);

			_workspacePage.SignOut();
			_loginHelper.Authorize(StartPage.Admin, ThreadUser);

			_adminHelper.ActivateUser(_email);

			var password = _accountInvitationPage.GetPassword();

			_signInPage
				.GetPage()
				.SubmitFormExpectingWorkspacePage(_email, password);

			Assert.IsFalse(_projectsPage.IsCreateProjectButtonDisplayed(),
				"Произошла ошибка: отображается кнопка создания проекта.");

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			_workspacePage.GoToUsersPage();

			Assert.AreEqual(string.Empty, _usersTab.GetGroup(_name),
				"Произошла ошибка: неверная группа у пользователя {0}.", _name);

			_usersTab.ClickGroupsButton();

			Assert.IsFalse(_groupsAndAccessRightsTab.CheckGroupsContainsUser(_fullName),
				"Произошла ошибка: пользователь добавился в группу.");
		}

		[Test, Description("S-7125"), ShortCheckList]
		public void AddUserToGroupTest()
		{
			_usersTab.ClickAddUserButton();

			_addUserDialog.AddUser(_name, _surname, _email);

			_workspacePage.SignOut();

			_loginHelper.Authorize(StartPage.Admin, ThreadUser);

			_adminHelper.ActivateUser(_email);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			_workspacePage.GoToUsersPage();

			var userList = _usersTab.GetUserNameList();
			userList.Sort();

			_usersTab.ClickGroupsButton();

			_newGroupDialog
				.OpenNewGroupDialog()
				.CreateNewGroup(_groupName);

			_groupsAndAccessRightsTab
				.ClickEditGroupButton(_groupName)
				.ClickAddUsersSearchbox(_groupName);

			Assert.AreEqual(userList, _groupsAndAccessRightsTab.GetUserListInSearchDropdown(),
				"Произошла ошибка: неверный список пользователей.");

			_groupsAndAccessRightsTab
				.ClickAddGroupUserButton(_groupName, _fullName)
				.ClickSaveButton(_groupName);

			Assert.IsTrue(_groupsAndAccessRightsTab.IsUserExistInGroup(_groupName, _fullName),
				"Произошла ошибка: пользователь {0} не добавился в группу {1}.", _fullName, _groupName);

			_groupsAndAccessRightsTab
				.ClickEditGroupButton(_groupName)
				.ClickAddUsersSearchbox(_groupName);

			Assert.AreEqual(userList.Except(new List<string> { _fullName }), _groupsAndAccessRightsTab.GetUserListInSearchDropdown(),
				"Произошла ошибка: неверный список пользователей.");

			_workspacePage.GoToUsersPage();
			
			Assert.AreEqual(_groupName, _usersTab.GetGroup(_name),
				"Произошла ошибка: неверная группа у пользователя {0}.", _name);
		}

		[Test, Description("S-15120"), ShortCheckList]
		public void DeleteUserFromGroupTest()
		{
			_usersTab.ClickAddUserButton();

			_addUserDialog.AddUser(_name, _surname, _email);

			_workspacePage.SignOut();

			_loginHelper.Authorize(StartPage.Admin, ThreadUser);

			_adminHelper.ActivateUser(_email);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			_workspacePage.GoToUsersPage();
			
			_newGroupDialog
				.OpenNewGroupDialog()
				.CreateNewGroup(_groupName);
			
			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(_groupName, _fullName);

			_groupsAndAccessRightsTab
				.ClickEditGroupButton(_groupName)
				.ClickAddUsersSearchbox(_groupName)
				.ClickGroupRow(_groupName);

			Assert.IsFalse(_groupsAndAccessRightsTab.GetUserListInSearchDropdown().Contains(_fullName),
				"Произошла ошибка: пользователь {0} состоит в списке кандидатов на добавление в группу.", _fullName);

			_groupsAndAccessRightsTab
				.ClickDeleteUserButton(_groupName, _fullName)
				.ClickSaveButton(_groupName);

			_workspacePage.GoToUsersPage();

			Assert.AreEqual(String.Empty, _usersTab.GetGroup(_name),
				"Произошла ошибка: неверная группа у пользователя {0}.", _name);
		}
	}
}
