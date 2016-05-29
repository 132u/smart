using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
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

			_userRightsHelper.CreateGroupWithSpecificRights(
				AdditionalUser.FullName,
				_groupName,
				new List<RightsType>
				{
					RightsType.TMManagement,
					RightsType.ProjectResourceManagement, 
					RightsType.ProjectCreation
				});
			
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
