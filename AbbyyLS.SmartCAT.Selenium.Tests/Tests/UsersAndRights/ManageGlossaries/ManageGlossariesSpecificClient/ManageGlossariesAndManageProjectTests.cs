using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class ManageGlossariesAndManageProjectTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
			_exportNotification = new ExportNotification(Driver);
			_clientsPage = new ClientsPage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_glossariesAdvancedSettingsSection = new GlossariesAdvancedSettingsSection(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_usersTab = new UsersTab(Driver);
			_userRightsHelper = new UserRightsHelper(Driver);

			_clientName = _clientsPage.GetClientUniqueName();
			_clientName2 = _clientsPage.GetClientUniqueName();
			var groupName = Guid.NewGuid().ToString();

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName);
			_clientsPage.CreateNewClient(_clientName2);

			_userRightsHelper.CreateGroupWithSpecificRights(
				AdditionalUser.NickName,
				groupName,
				new List<RightsType> { RightsType.ProjectResourceManagement, RightsType.ProjectCreation });

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(groupName);
			_addAccessRightDialog.AddRightToGroupSpecificClient(RightsType.GlossaryManagement, _clientName);
			_groupsAndAccessRightsTab.ClickSaveButton(groupName);

			_workspacePage.SignOut();
		}


		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
				_projectUniqueName = Guid.NewGuid().ToString();
				_loginHelper.Authorize(StartPage.Workspace, AdditionalUser);
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}

		[Test]
		public void CreateGlossaryInWizardWithoutClientTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { PathProvider.DocumentFile})
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ExpandAdvancedSettings()
				.ClickGlossariesTab();

			_glossariesAdvancedSettingsSection.ClickCreateGlossaryButton();

			Assert.IsTrue(_glossariesAdvancedSettingsSection.IsClientErrorDisplayed(),
				"Произошла ошибка: Не отображается сообщение 'The glossary cannot be created until a client is selected.'.");
		}

		[Test]
		public void CreateGlossaryInWizardWrongClientTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { PathProvider.DocumentFile })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.SelectClient(_clientName2)
				.ExpandAdvancedSettings()
				.ClickGlossariesTab();

			_glossariesAdvancedSettingsSection.ClickCreateGlossaryButton();

			Assert.IsTrue(_glossariesAdvancedSettingsSection.IsWrongClientErrorDisplayed(),
				"Произошла ошибка: Не отображается сообщение 'You cannot create glossaries for the selected client.'.");
		}

		[Test]
		public void CreateGlossaryInWizardTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { PathProvider.DocumentFile })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.SelectClient(_clientName)
				.ExpandAdvancedSettings()
				.ClickGlossariesTab();

			_glossariesAdvancedSettingsSection.ClickCreateGlossaryButton();

			_newProjectSettingsPage.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			_projectsPage.ClickProject(_projectUniqueName);

			Assert.IsTrue(_projectSettingsPage.IsGlossaryChecked(_projectUniqueName),
				"Произошла ошибка: Не выбран глоссарий '{0}'.", _projectUniqueName);
		}

		private string _clientName;
		private string _clientName2;
		private string _groupName;
		private string _projectUniqueName;

		private UserRightsHelper _userRightsHelper;
		private ClientsPage _clientsPage;
		private ExportNotification _exportNotification;
		private WorkspacePage _workspacePage;
		private GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		private NewGroupDialog _newGroupDialog;
		private ProjectsPage _projectsPage;
		private AddAccessRightDialog _addAccessRightDialog;
		private NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		private NewProjectSettingsPage _newProjectSettingsPage;
		private GlossariesAdvancedSettingsSection _glossariesAdvancedSettingsSection;
		private ProjectSettingsPage _projectSettingsPage;
		private NewProjectWorkflowPage _newProjectWorkflowPage;
		private UsersTab _usersTab;
	}
}
