using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class UsersAndAccessRightGroupsAdminTest<TWebDriverProvider> : UsersAndAccessRightGroupsBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public UsersAndAccessRightGroupsAdminTest()
		{
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void BeforeTest()
		{
			_adminHelper.CreateNewUser(_email, _nickName, _password, aolUser: false);
		}

		[Test, Description("S-7127")]
		public void AddUserPersonalAccountToGroupTest()
		{
			_adminHelper.CreateNewPersonalAccount(_surname, state: true);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToUsersPage();
			_usersTab.ClickAddUserButton();

			_addUserDialog.AddUser(_name, _surname, _email);

			_workspacePage.SignOut();
			_loginHelper.Authorize(StartPage.Admin, ThreadUser);

			_adminHelper.ActivateUser(_email);

			Assert.IsFalse(_accountInvitationPage.IsPasswordDisplayed(),
				"Произошла ошибка: пароль отображается.");

			Assert.IsTrue(_accountInvitationPage.IsCorporateAccountMessageContainsCorrectEmail(_email),
				"Произошла ошибка: неверный email в сообщении.");

			_signInPage.GetPage();
			_loginHelper.LogInSmartCat(_email, _nickName, _password);

			Assert.IsTrue(_workspacePage.IsWorkspacePageOpened(),
				"Произошла ошибка: не открылась страница WAddUserCorporateAccountToGroupTest()S.");
		}

		[Test, Description("S-7128")]
		public void AddUserCorporateAccountToGroupTest()
		{
			var testAccount = "AddUserCorporateAccountToGroupTest";
			_adminHelper
				.CreateAccountIfNotExist(
					LoginHelper.SmartCATVenture,
					testAccount,
					workflow: true)
				.AddUserToAdminGroupInAccountIfNotAdded(_email, _name, _surname, testAccount);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToUsersPage();
			_usersTab.ClickAddUserButton();

			_addUserDialog.AddUser(_name, _surname, _email);

			_workspacePage.GoToUsersPage();
			_loginHelper.Authorize(StartPage.Admin, ThreadUser);

			_adminHelper.ActivateUser(_email);

			Assert.IsFalse(_accountInvitationPage.IsPasswordDisplayed(),
			"Произошла ошибка: пароль отображается.");

			Assert.IsTrue(_accountInvitationPage.IsCorporateAccountMessageContainsCorrectEmail(_email),
				"Произошла ошибка: неверный email в сообщении.");

			_signInPage.GetPage();
			_loginHelper.LogInSmartCat(_email, _nickName, _password, testAccount);

			Assert.IsTrue(_workspacePage.IsWorkspacePageOpened(),
				"Произошла ошибка: не открылась страница WS.");
		}
	}
}
