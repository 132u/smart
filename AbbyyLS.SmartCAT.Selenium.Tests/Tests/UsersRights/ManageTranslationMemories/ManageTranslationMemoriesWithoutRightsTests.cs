using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageTranslationMemories
{
	class ManageTranslationMemoriesWithoutRightsBaseTests<TWebDriverProvider> : ManageTranslationMemoriesBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_groupName = Guid.NewGuid().ToString();
			_projectGroupName = _projectGroupsPage.GetProjectGroupUniqueName();
			_clientName = _clientsPage.GetClientUniqueName();
			_clientName2 = _clientsPage.GetClientUniqueName();
			_clientName3 = _clientsPage.GetClientUniqueName();
			_commonTranslationMemory = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_commonTranslationMemory2 = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_commonTranslationMemory3 = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_translationMemoryToDeleteTest = _translationMemoriesHelper.GetTranslationMemoryUniqueName();

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName);
			_clientsPage.CreateNewClient(_clientName2);
			_clientsPage.CreateNewClient(_clientName3);

			_workspacePage.GoToTranslationMemoriesPage();
			_translationMemoriesHelper.CreateTranslationMemory(_commonTranslationMemory, client: _clientName);
			_translationMemoriesHelper.CreateTranslationMemory(_commonTranslationMemory2, client: _clientName2);
			_translationMemoriesHelper.CreateTranslationMemory(_commonTranslationMemory3, client: _clientName3);
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemoryToDeleteTest, client: _clientName);

			_workspacePage.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(AdditionalUser.NickName)
				.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(_groupName);
			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(_groupName, AdditionalUser.NickName);

			_workspacePage.SignOut();
		}

	}
}
