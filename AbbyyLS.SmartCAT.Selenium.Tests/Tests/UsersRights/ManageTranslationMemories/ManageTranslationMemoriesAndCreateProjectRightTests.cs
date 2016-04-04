using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageTranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageTranslationMemoriesAndCreateProjectRightTests<TWebDriverProvider> : ManageTranslationMemoriesBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
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

			_workspacePage.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(AdditionalUser.NickName);

			_userRightsHelper.CreateGroupWithSpecificRights(
				AdditionalUser.NickName,
				_groupName,
				RightsType.TMManagement);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);
			_addAccessRightDialog.AddRightToGroupAnyProject(RightsType.ProjectResourceManagement);
			_groupsAndAccessRightsTab.ClickSaveButton(_groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);
			_addAccessRightDialog.AddRightToGroupAnyProject(RightsType.ProjectCreation);
			_groupsAndAccessRightsTab.ClickSaveButton(_groupName);

			_workspacePage.GoToTranslationMemoriesPage();
			_translationMemoriesHelper.CreateTranslationMemory(_existingTranslationMemory);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName);

			_workspacePage.GoToProjectGroupsPage();
			_projectGroupsPage.CreateProjectGroup(_projectGroupName);

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
		public void CreateTranslationMemoryInCreateProjectWizardTest()
		{
			var projectName = _createProjectHelper.GetProjectUniqueName();
			_projectsPage.ClickCreateProjectButton();
			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(projectName)
				.ExpandAdvancedSettings();

			_advancedSettingsSection.ClickSelectTranslationMemoryButton();

			_newProjectSetUpTMDialog.SelectExisitingTranslationMemory(_existingTranslationMemory);

			_newProjectSettingsPage
				.ClickWriteTMRadioButton(_existingTranslationMemory)
				.RemoveTranslationMemory(projectName);

			Assert.IsFalse(_newProjectSettingsPage.IsTranslationMemoryDisplayed(projectName),
				"Произошла ошибка: Не удалось удалить память перевод {0}.", projectName);

			_newProjectSettingsPage.ClickCreateTranslationMemoryButton();

			Assert.IsTrue(_newProjectSettingsPage.IsTranslationMemoryDisplayed(projectName),
				"Произошла ошибка: Отсутсвует память перевод {0}.", projectName);

			_newProjectSettingsPage.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			_workspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesPage.SearchForTranslationMemory(projectName);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(projectName),
				"Произошла ошибка: Отсутсвует память перевод {0}.", projectName);
		}

		[Test]
		public void CreateTranslationMemoryWithClientInCreateProjectWizardTest()
		{
			var projectName = _createProjectHelper.GetProjectUniqueName();
			_projectsPage.ClickCreateProjectButton();
			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(projectName)
				.SelectClient(_clientName)
				.ExpandAdvancedSettings();

			_advancedSettingsSection.ClickSelectTranslationMemoryButton();

			_newProjectSetUpTMDialog.SelectExisitingTranslationMemory(_existingTranslationMemory);

			_newProjectSettingsPage
				.ClickWriteTMRadioButton(_existingTranslationMemory)
				.RemoveTranslationMemory(projectName);

			Assert.IsFalse(_newProjectSettingsPage.IsTranslationMemoryDisplayed(projectName),
				"Произошла ошибка: Не удалось удалить память перевод {0}.", projectName);

			_newProjectSettingsPage.ClickCreateTranslationMemoryButton();

			Assert.IsTrue(_newProjectSettingsPage.IsTranslationMemoryDisplayed(projectName),
				"Произошла ошибка: Отсутсвует память перевод {0}.", projectName);

			_newProjectSettingsPage.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			_workspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesPage.SearchForTranslationMemory(projectName);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(projectName),
				"Произошла ошибка: Отсутсвует память перевод {0}.", projectName);
		}

		[Test]
		public void CreateTranslationMemoryWithProjectGroupInCreateProjectWizardTest()
		{
			var projectName = _createProjectHelper.GetProjectUniqueName();
			_projectsPage.ClickCreateProjectButton();
			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(projectName)
				.SelectProjectGroup(_projectGroupName)
				.ExpandAdvancedSettings();

			_advancedSettingsSection.ClickSelectTranslationMemoryButton();

			_newProjectSetUpTMDialog.SelectExisitingTranslationMemory(_existingTranslationMemory);

			_newProjectSettingsPage
				.ClickWriteTMRadioButton(_existingTranslationMemory)
				.RemoveTranslationMemory(projectName);

			Assert.IsFalse(_newProjectSettingsPage.IsTranslationMemoryDisplayed(projectName),
				"Произошла ошибка: Не удалось удалить память перевод {0}.", projectName);

			_newProjectSettingsPage.ClickCreateTranslationMemoryButton();

			Assert.IsTrue(_newProjectSettingsPage.IsTranslationMemoryDisplayed(projectName),
				"Произошла ошибка: Отсутсвует память перевод {0}.", projectName);

			_newProjectSettingsPage.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			_workspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesPage.SearchForTranslationMemory(projectName);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(projectName),
				"Произошла ошибка: Отсутсвует память перевод {0}.", projectName);
		}
	}
}
