using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.AssignmentPages;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects.AssignResponsibles
{
	public class AssignResponsiblesBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void Setup()
		{
			_loginHelper = new LoginHelper(Driver);
			_commonHelper = new CommonHelper(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_workspaceHelper.GoToProjectsPage();
			_createProjectHelper = new CreateProjectHelper(Driver);
			_distributeDocumentBetweenAssigneesPage = new DistributeDocumentBetweenAssigneesPage(Driver);
			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_usersRightsPage = new UsersRightsPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_settingsDialog = new SettingsDialog(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_attentionPopup = new AttentionPopup(Driver);
			_distributeSegmentsBetweenAssigneesPage = new DistributeSegmentsBetweenAssigneesPage(Driver);
			_editorPage = new EditorPage(Driver);
			_reassigneDialog = new ReassignDialog(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_additionalUser = TakeUser(ConfigurationManager.AdditionalUsers);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[TearDown]
		public void TearDown()
		{
			if (_additionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, _additionalUser);
			}
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
		protected WorkspaceHelper _workspaceHelper;
		protected TestUser _additionalUser;
		protected CommonHelper _commonHelper;
		protected LoginHelper _loginHelper;
		protected DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
		protected SettingsDialog _settingsDialog;
		protected TaskAssignmentPage _taskAssignmentPage;
		protected UsersRightsPage _usersRightsPage;
		protected ProjectsPage _projectsPage;
		protected ProjectSettingsPage _projectSettingsPage;
		protected CreateProjectHelper _сreateProjectHelper;
		protected DistributeDocumentBetweenAssigneesPage _distributeDocumentBetweenAssigneesPage;
		protected DistributeSegmentsBetweenAssigneesPage _distributeSegmentsBetweenAssigneesPage;
		protected AttentionPopup _attentionPopup;
		protected EditorPage _editorPage;
		protected SelectTaskDialog _selectTaskDialog;
		protected ReassignDialog _reassigneDialog;
		protected TestUser _secondUser;
		protected TestUser _thirdUser;
	}
}
