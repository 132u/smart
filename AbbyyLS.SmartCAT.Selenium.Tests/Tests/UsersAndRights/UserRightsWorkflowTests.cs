using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.AssignmentPages;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class UserRightsWorkflowTests<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void UserRightsWorkflowTestsSetUp()
		{
			AdditionalUser = null;
			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);
			document1 = PathProvider.DocumentFile;
			document2 = PathProvider.EditorTxtFile;

			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);

			_projectsPage = new ProjectsPage(Driver);
			_settingsDialog = new ProjectSettingsDialog(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_editorPage = new EditorPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_usersTab = new UsersTab(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);
			_documentUploadGeneralInformationDialog = new AddFilesStep(Driver);
			_distributeSegmentsBetweenAssigneesPage = new DistributeSegmentsBetweenAssigneesPage(Driver);
			_distributeDocumentBetweenAssigneesPage = new DistributeDocumentBetweenAssigneesPage(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_workflowSetUptab = new WorkflowSetUpTab(Driver);

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_groupName = _groupsAndAccessRightsTab.GetGroupUniqueName();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { document1, document2 });

			_workspacePage.GoToUsersPage();

			_usersTab.ClickGroupsButton();

			_groupsAndAccessRightsTab.RemoveUserFromAllGroups(AdditionalUser.NickName);

			_newGroupDialog
				.OpenNewGroupDialog()
				.CreateNewGroup(_groupName);

			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(_groupName, AdditionalUser.NickName);
		}

		[Test, Description("ТС-71")]
		public void ViewSpecificProjectAccessOneDocumentTest()
		{
			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroupSpecificProject(RightsType.ProjectView, _projectUniqueName);

			_groupsAndAccessRightsTab.ClickSaveButton(_groupName);

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.AssignTasksOnDocument(document1, AdditionalUser.NickName, _projectUniqueName);

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, document1);

			Assert.IsTrue(_editorPage.IsEditorPageOpened(),
				"Произошла ошибка:\nРедактор не открылся.");

			_editorPage.ClickHomeButtonExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, document2);

			Assert.IsTrue(_editorPage.IsEditorPageOpened(),
				"Произошла ошибка:\nРедактор не открылся.");
		}

		[Test, Description("ТС-72")]
		public void ViewSpecificProjectAccessTwoDocumentTest()
		{
			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroupSpecificProject(RightsType.ProjectView, _projectUniqueName);

			_groupsAndAccessRightsTab.ClickSaveButton(_groupName);

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.OpenWorkflowSettings();

			_settingsDialog.ClickWorkflowTab();

			_workflowSetUptab
				.AddTask(WorkflowTask.Editing)
				.SaveSettings();

			_projectSettingsHelper
				.AssignTasksOnDocument(document1, AdditionalUser.NickName, _projectUniqueName)
				.AssignTasksOnDocument(document2, AdditionalUser.NickName, _projectUniqueName, taskNumber: 2);

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);
			
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, document1);

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\nНевреное название этапа.");

			_editorPage.ClickHomeButtonExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, document2);

			Assert.AreEqual("Editing (E):", _editorPage.GetStage(),
				"Произошла ошибка:\nНевреное название этапа.");
		}

		[Test]
		[Description("ТС-73")]
		public void NoViewRightToProjectTest()
		{
			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.AssignTasksOnDocument(document1, AdditionalUser.NickName, _projectUniqueName);

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, document1);

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\nНевреное название этапа.");
		}

		[Test, Description("ТС-74")]
		public void ManageProjectAccessOneDocumentTest()
		{
			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroupSpecificProject(RightsType.ProjectResourceManagement, _projectUniqueName);

			_groupsAndAccessRightsTab.ClickSaveButton(_groupName);

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(document1, AdditionalUser.NickName, _projectUniqueName);

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, document1);

			_selectTaskDialog.SelectTask();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\nНевреное название этапа.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(document2);
			
			Assert.IsTrue(_editorPage.IsEditorPageOpened(),
				"Произошла ошибка:\nРедактор не открылся.");
		}

		[Test, Description("ТС-741")]
		public void ManageProjectAccessTwoDocumentTest()
		{
			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroupSpecificProject(RightsType.ProjectResourceManagement, _projectUniqueName);

			_groupsAndAccessRightsTab.ClickSaveButton(_groupName);

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.OpenWorkflowSettings();

			_settingsDialog.ClickWorkflowTab();

			_workflowSetUptab
				.AddTask(WorkflowTask.Editing)
				.SaveSettings();

			_projectSettingsHelper
				.AssignTasksOnDocument(document1, AdditionalUser.NickName, _projectUniqueName)
				.AssignTasksOnDocument(document2, AdditionalUser.NickName, _projectUniqueName, taskNumber: 2);

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, document1);

			_selectTaskDialog.SelectTask();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\nНевреное название этапа.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(document2);

			_selectTaskDialog.SelectTask(TaskMode.Editing);

			Assert.AreEqual("Editing (E):", _editorPage.GetStage(),
				"Произошла ошибка:\nНевреное название этапа.");
		}

		[Test]
		[Description("ТС-742")]
		public void NoManageRightToProjectTest()
		{
			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.AssignTasksOnDocument(
				document1, AdditionalUser.NickName, _projectUniqueName);

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, document1);

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\nНевреное название этапа.");
		}

		[Test, Description("ТС-75")]
		public void CreateProjectAccessOneDocumentTest()
		{
			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroupAnyProject(RightsType.ProjectCreation);

			_groupsAndAccessRightsTab.ClickSaveButton(_groupName);

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(document1, AdditionalUser.NickName, _projectUniqueName);

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, document1);

			_selectTaskDialog.SelectTask();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\nНевреное название этапа.");

			_editorPage.ClickHomeButtonExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, document2);

			Assert.IsTrue(_editorPage.IsEditorPageOpened(),
				"Произошла ошибка:\nРедактор не открылся.");
		}

		[Test, Description("ТС-751")]
		public void CreateProjectAccessTwoDocumentTest()
		{
			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroupAnyProject(RightsType.ProjectCreation);

			_groupsAndAccessRightsTab.ClickSaveButton(_groupName);

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.OpenWorkflowSettings();

			_settingsDialog.ClickWorkflowTab();

			_workflowSetUptab
				.AddTask(WorkflowTask.Editing)
				.SaveSettings();

			_projectSettingsHelper
				.AssignTasksOnDocument(document1, AdditionalUser.NickName, _projectUniqueName)
				.AssignTasksOnDocument(document2, AdditionalUser.NickName, _projectUniqueName, taskNumber: 2);

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, document1);
			
			_selectTaskDialog.SelectTask();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\nНевреное название этапа.");

			_editorPage.ClickHomeButtonExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, document2);

			_selectTaskDialog.SelectTask(TaskMode.Editing);

			Assert.AreEqual("Editing (E):", _editorPage.GetStage(),
				"Произошла ошибка:\nНевреное название этапа.");
		}

		[TearDown]
		public void TearDown()
		{
			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}
		}

		protected string _projectUniqueName;

		protected CreateProjectHelper _createProjectHelper;
		protected ProjectSettingsHelper _projectSettingsHelper;

		protected ProjectsPage _projectsPage;
		protected ProjectSettingsDialog _settingsDialog;
		protected ProjectSettingsPage _projectSettingsPage;
		public SelectTaskDialog _selectTaskDialog;
		protected NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		protected NewProjectWorkflowPage _newProjectWorkflowPage;
		protected NewProjectSettingsPage _newProjectSettingsPage;
		protected EditorPage _editorPage;
		protected WorkspacePage _workspacePage;
		protected DistributeDocumentBetweenAssigneesPage _distributeDocumentBetweenAssigneesPage;
		protected DistributeSegmentsBetweenAssigneesPage _distributeSegmentsBetweenAssigneesPage;
		protected AddFilesStep _documentUploadGeneralInformationDialog;
		protected LoginHelper _loginHelper;
		protected TaskAssignmentPage _taskAssignmentPage;
		protected const string _text = "Translation";
		protected TestUser _secondUser;
		protected UsersTab _usersTab;
		protected AddAccessRightDialog _addAccessRightDialog;
		protected GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		protected NewGroupDialog _newGroupDialog;
		protected WorkflowSetUpTab _workflowSetUptab ;
		private string _groupName;

		private string document1;
		private string document2;
	}
}
