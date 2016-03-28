using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageProjects
{
	class ManageProjectBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
				_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
				_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
				_workspacePage.GoToProjectsPage();

				_createProjectHelper.CreateNewProject(
					_projectUniqueName, filesPaths: new[] { PathProvider.DocumentFile });

				_projectsPage
					.OpenProjectInfo(_projectUniqueName)
					.ClickDocumentUploadButton();

				_documentUploadGeneralInformationDialog
					.UploadDocument(new[] { PathProvider.DocumentFile2 })
					.ClickFihishUploadOnProjectsPage();
				_workspacePage.SignOut();

				_loginHelper = new LoginHelper(Driver);
				_loginHelper.Authorize(StartPage, AdditionalUser);
				_exportNotification.CancelAllNotifiers<Pages.Projects.ProjectsPage>();

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
			_settingsDialog = new ProjectSettingsDialog(Driver);
			_projectSettingsPage = new Pages.Projects.ProjectSettings.ProjectSettingsPage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_projectsPage = new Pages.Projects.ProjectsPage(Driver);
			_exportNotification = new ExportNotification(Driver);
			_usersTab = new UsersTab(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_qualityAssuranceDialog = new QualityAssuranceDialog(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_workflowSetUpTab = new WorkflowSetUpTab(Driver);
			_generalTab = new GeneralTab(Driver);
			_qualityAssuranceSettings = new QualityAssuranceSettings(Driver);
			_cancelConfirmationDialog = new CancelConfirmationDialog(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_documentSettingsDialog = new DocumentSettingsDialog(Driver);
			_confirmDeclineTaskDialog = new ConfirmDeclineTaskDialog(Driver);
			_statisticsPage = new BuildStatisticsPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_editorPage = new EditorPage(Driver);
			_pretranslationDialog = new PretranslationDialog(Driver);
			_userRightsHelper = new UserRightsHelper(Driver);
			_documentUploadGeneralInformationDialog = new AddFilesStep(Driver);

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			var groupName = Guid.NewGuid().ToString();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_userRightsHelper.CreateGroupWithSpecificRights(
				AdditionalUser.NickName,
				groupName,
				RightsType.ProjectResourceManagement);
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}
		}
		protected AddFilesStep _documentUploadGeneralInformationDialog;
		protected ProjectSettingsDialog _settingsDialog;
		protected EditorPage _editorPage;
		protected BuildStatisticsPage _statisticsPage;
		protected QualityAssuranceDialog _qualityAssuranceDialog;
		protected CreateProjectHelper _createProjectHelper;
		protected WorkspacePage _workspacePage;
		protected LoginHelper _loginHelper;
		protected string _projectUniqueName;
		protected UsersTab _usersTab;
		protected AddAccessRightDialog _addAccessRightDialog;
		protected Pages.Projects.ProjectsPage _projectsPage;
		protected ExportNotification _exportNotification;
		protected NewGroupDialog _newGroupDialog;
		protected GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		protected Pages.Projects.ProjectSettings.ProjectSettingsPage _projectSettingsPage;
		protected ProjectSettingsHelper _projectSettingsHelper;
		protected PretranslationDialog _pretranslationDialog;
		protected WorkflowSetUpTab _workflowSetUpTab;
		protected GeneralTab _generalTab;
		protected QualityAssuranceSettings _qualityAssuranceSettings;
		protected CancelConfirmationDialog _cancelConfirmationDialog;
		protected TaskAssignmentPage _taskAssignmentPage;
		protected ConfirmDeclineTaskDialog _confirmDeclineTaskDialog;
		protected DocumentSettingsDialog _documentSettingsDialog;
		protected SelectTaskDialog _selectTaskDialog;
		protected UserRightsHelper _userRightsHelper;
	}
}
