using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageTranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageTranslationMemoriesBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
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
			_exportNotification = new ExportNotification(Driver);
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
			_usersTab = new UsersTab(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_newProjectSetUpTMDialog = new NewProjectSetUpTMDialog(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			_advancedSettingsSection = new AdvancedSettingsSection(Driver);
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}
		}

		protected string _groupName;
		protected string _projectGroupName;

		protected const string _russianLanguage = "ru";
		protected const string _englishLanguage = "en";
		protected const string _lithuanianLanguage = "lt";

		protected string _clientName;
		protected string _clientName2;
		protected string _clientName3;

		protected string _translationMemory;
		protected string _commonTranslationMemoryWithClient;
		protected string _commonTranslationMemoryWithProjectGroup;
		protected string _commonTranslationMemory;
		protected string _commonTranslationMemory2;
		protected string _commonTranslationMemory3;
		protected string _translationMemoryToDeleteTest;
		protected string _existingTranslationMemory;

		protected ProjectsPage _projectsPage;
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
		protected UsersTab _usersTab;
		protected NewGroupDialog _newGroupDialog;
		protected GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		protected AddAccessRightDialog _addAccessRightDialog;
		protected CreateProjectHelper _createProjectHelper;
		protected NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		protected NewProjectSettingsPage _newProjectSettingsPage;
		protected NewProjectSetUpTMDialog _newProjectSetUpTMDialog;
		protected NewProjectWorkflowPage _newProjectWorkflowPage;
		protected AdvancedSettingsSection _advancedSettingsSection;
	}
}
