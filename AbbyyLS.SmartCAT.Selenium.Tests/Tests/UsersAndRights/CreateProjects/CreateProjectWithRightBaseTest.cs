using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	class CreateProjectWithRightBaseTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public override void BeforeTest()
		{
			CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
		}

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_deleteDialog = new DeleteDialog(Driver);
			_exportNotification = new ExportNotification(Driver);
			_documentUploadGeneralInformationDialog = new AddFilesStep(Driver);
			_usersTab = new UsersTab(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_settingsDialog = new ProjectSettingsDialog(Driver);
			_buildStatisticsPage = new BuildStatisticsPage(Driver);
			_deleteProjectOrFileDialog = new DeleteProjectOrFileDialog(Driver);

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			var groupName = Guid.NewGuid().ToString();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(AdditionalUser.FullName)
				.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(groupName);

			_addAccessRightDialog
				.AddRightToGroupAnyProject(RightsType.ProjectCreation)
				.ClickSaveButton(groupName);

			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(groupName, AdditionalUser.FullName);

			_workspacePage.SignOut();
		}

		[SetUp]
		public void SetUp()
		{
			_loginHelper.Authorize(StartPage.Workspace, AdditionalUser);

			_exportNotification.CancelAllNotifiers<ProjectsPage>();

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_document = PathProvider.EditorTxtFile;
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}
		}

		protected CreateProjectHelper _createProjectHelper;
		protected WorkspacePage _workspacePage;
		protected LoginHelper _loginHelper;
		protected string _projectUniqueName;

		protected AddFilesStep _documentUploadGeneralInformationDialog;
		protected UsersTab _usersTab;
		protected AddAccessRightDialog _addAccessRightDialog;
		protected ProjectsPage _projectsPage;
		protected DeleteDialog _deleteDialog;
		protected ExportNotification _exportNotification;
		protected NewGroupDialog _newGroupDialog;
		protected GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		protected ProjectSettingsDialog _settingsDialog;
		protected BuildStatisticsPage _buildStatisticsPage;
		protected DeleteProjectOrFileDialog _deleteProjectOrFileDialog;
		protected string _document;
	}
}
