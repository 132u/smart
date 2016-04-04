﻿using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageTranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageTranslationMemoriesAndCreateProjectRightSpecificClientTests<TWebDriverProvider> : ManageTranslationMemoriesBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_groupName = Guid.NewGuid().ToString();
			_existingTranslationMemory = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_projectGroupName = _projectGroupsPage.GetProjectGroupUniqueName();
			_clientName = _clientsPage.GetClientUniqueName();

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			
			_workspacePage.GoToTranslationMemoriesPage();
			_translationMemoriesHelper.CreateTranslationMemory(_existingTranslationMemory);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName);

			_workspacePage.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(AdditionalUser.NickName)
				.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(_groupName);
			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroupSpecificClient(RightsType.TMManagement, _clientName);
			_groupsAndAccessRightsTab
				.ClickSaveButton(_groupName)
				.AddUserToGroupIfNotAlredyAdded(_groupName, AdditionalUser.NickName);
			
			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);
			_addAccessRightDialog.AddRightToGroupAnyProject(RightsType.ProjectResourceManagement);
			_groupsAndAccessRightsTab.ClickSaveButton(_groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);
			_addAccessRightDialog.AddRightToGroupAnyProject(RightsType.ProjectCreation);
			_groupsAndAccessRightsTab.ClickSaveButton(_groupName);
			
			_workspacePage.SignOut();
		}

		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
				_loginHelper.Authorize(StartPage, AdditionalUser);
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}

		[Test]
		public void CreateTranslationMemoryInCreateProjectWizardSpecificClientTest()
		{
			var translationMemory = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			var projectName = _createProjectHelper.GetProjectUniqueName();

			_workspacePage.GoToTranslationMemoriesPage();
			_translationMemoriesHelper.CreateTranslationMemory(translationMemory, client: _clientName);

			_workspacePage.GoToProjectsPage();
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(projectName)
				.SelectClient(_clientName)
				.ExpandAdvancedSettings();

			Assert.IsTrue(_newProjectSettingsPage.IsTranslationMemoryDisplayed(translationMemory),
				"Произошла ошибка: Отсутсвует память перевод {0}.", translationMemory);

			_newProjectSettingsPage.ClickCreateTranslationMemoryButton();

			Assert.IsTrue(_newProjectSettingsPage.IsTranslationMemoryDisplayed(projectName),
				"Произошла ошибка: Отсутсвует память перевод {0}.", projectName);

			_newProjectSettingsPage.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			_workspacePage.GoToTranslationMemoriesPage();

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(projectName),
				"Произошла ошибка: Отсутсвует память перевод {0}.", projectName);
		}


	}
}
