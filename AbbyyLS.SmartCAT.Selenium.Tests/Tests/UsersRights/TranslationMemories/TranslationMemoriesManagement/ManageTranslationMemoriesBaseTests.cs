using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageTranslationMemories
{
	class ManageTranslationMemoriesBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
				_loginHelper.Authorize(StartPage, AdditionalUser);
				_workspacePage.GoToTranslationMemoriesPage();

				_translationMemory = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_deleteTmDialog = new DeleteTmDialog(Driver);
			_importTmxDialog = new ImportTmxDialog(Driver);
			_translationMemoriesPage = new TranslationMemoriesPage(Driver);
			_translationMemoriesHelper = new TranslationMemoriesHelper(Driver);
			_clientsPage = new ClientsPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_userRightsHelper = new UserRightsHelper(Driver);
			_projectGroupsPage = new ProjectGroupsPage(Driver);
			_confirmReplacementDialog = new ConfirmReplacementDialog(Driver);

			var groupName = Guid.NewGuid().ToString();
			_projectGroupName = _projectGroupsPage.GetProjectGroupUniqueName();
			_clientName = _clientsPage.GetClientUniqueName();
			_commonTranslationMemory = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_translationMemoryToDeleteTest = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_commonTranslationMemoryWithClient =  _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_commonTranslationMemoryWithProjectGroup =  _translationMemoriesHelper.GetTranslationMemoryUniqueName();

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToTranslationMemoriesPage();
			_translationMemoriesHelper.CreateTranslationMemory(_commonTranslationMemory);
			_translationMemoriesHelper.CreateTranslationMemory(_commonTranslationMemoryWithClient, client: _clientName);
			_translationMemoriesHelper.CreateTranslationMemory(_commonTranslationMemoryWithProjectGroup, projectGroup: _projectGroupName);
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemoryToDeleteTest, client: _clientName, projectGroup: _commonTranslationMemoryWithProjectGroup);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName);

			_workspacePage.GoToProjectGroupsPage();
			_projectGroupsPage.CreateProjectGroup(_projectGroupName);

			_userRightsHelper.CreateGroupWithSpecificRights(
				AdditionalUser.NickName,
				groupName,
				RightsType.TMManagement);

			_workspacePage.SignOut();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}
		}

		protected string _clientName;
		protected string _translationMemory;
		protected string _projectGroupName;
		protected string _commonTranslationMemoryWithClient;
		protected string _commonTranslationMemoryWithProjectGroup;
		protected string _commonTranslationMemory;
		protected string _translationMemoryToDeleteTest;

		protected DeleteTmDialog _deleteTmDialog;
		protected ConfirmReplacementDialog _confirmReplacementDialog;
		protected ImportTmxDialog _importTmxDialog;
		protected ProjectGroupsPage _projectGroupsPage;
		protected TranslationMemoriesHelper _translationMemoriesHelper;
		protected TranslationMemoriesPage _translationMemoriesPage;
		protected ClientsPage _clientsPage;
		protected UserRightsHelper _userRightsHelper;
		protected WorkspacePage _workspacePage;
		protected LoginHelper _loginHelper;
		protected ExportNotification _exportNotification;
	}
}
