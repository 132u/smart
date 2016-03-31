using System;

using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageGlossaries;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageGlossariesCompoundRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	public class ManageGlossariesSpecificClientBaseTests<TWebDriverProvider> : ManageGlossariesCommonBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_groupName = Guid.NewGuid().ToString();
			_commonClientName = _clientsPage.GetClientUniqueName();
			_commonClientName2 = _clientsPage.GetClientUniqueName();
			_commonGlossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_commonClientName);
			_clientsPage.CreateNewClient(_commonClientName2);

			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(_commonGlossaryUniqueName, client: _commonClientName2);

			_workspacePage.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(AdditionalUser.NickName)
				.OpenNewGroupDialog();
			
			_newGroupDialog.CreateNewGroup(_groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroupSpecificClient(RightsType.GlossaryManagement, _commonClientName);
			_groupsAndAccessRightsTab
				.ClickSaveButton(_groupName)
				.AddUserToGroupIfNotAlredyAdded(_groupName, AdditionalUser.NickName);
		}
	}
}
