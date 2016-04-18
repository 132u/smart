using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.AssignmentPages;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	public class AssignResponsiblesBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void Setup()
		{
			_loginHelper = new LoginHelper(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_distributeDocumentBetweenAssigneesPage = new DistributeDocumentBetweenAssigneesPage(Driver);
			_documentUploadGeneralInformationDialog = new AddFilesStep(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_settingsDialog = new ProjectSettingsDialog(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_attentionPopup = new AttentionPopup(Driver);
			_distributeSegmentsBetweenAssigneesPage = new DistributeSegmentsBetweenAssigneesPage(Driver);
			_editorPage = new EditorPage(Driver);
			_reassigneDialog = new ReassignDialog(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_userTab = new UsersTab(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_workflowSetUptab = new WorkflowSetUpTab(Driver);
			_datePicker = new DatePicker(Driver);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_secondUser = null;
			_thirdUser = null;

			_workspacePage.GoToProjectsPage();
		}

		[TearDown]
		public void TearDown()
		{
			if (_secondUser != null)
			{
				ReturnUser(ConfigurationManager.Users, _secondUser);
			}
			if (_thirdUser != null)
			{
				ReturnUser(ConfigurationManager.Users, _thirdUser);
			}
		}

		protected string _projectUniqueName;
		protected CreateProjectHelper _createProjectHelper;
		protected WorkspacePage _workspacePage;
		protected LoginHelper _loginHelper;
		protected AddFilesStep _documentUploadGeneralInformationDialog;
		protected ProjectSettingsDialog _settingsDialog;
		protected TaskAssignmentPage _taskAssignmentPage;
		protected GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		protected ProjectsPage _projectsPage;
		protected ProjectSettingsPage _projectSettingsPage;
		protected CreateProjectHelper _сreateProjectHelper;
		protected ProjectSettingsHelper _projectSettingsHelper;
		protected DistributeDocumentBetweenAssigneesPage _distributeDocumentBetweenAssigneesPage;
		protected DistributeSegmentsBetweenAssigneesPage _distributeSegmentsBetweenAssigneesPage;
		protected AttentionPopup _attentionPopup;
		protected EditorPage _editorPage;
		protected SelectTaskDialog _selectTaskDialog;
		protected ReassignDialog _reassigneDialog;
		protected TestUser _secondUser;
		protected TestUser _thirdUser;
		protected UsersTab _userTab;
		protected NewGroupDialog _newGroupDialog;
		protected WorkflowSetUpTab _workflowSetUptab;
		protected DatePicker _datePicker;
	}
}
