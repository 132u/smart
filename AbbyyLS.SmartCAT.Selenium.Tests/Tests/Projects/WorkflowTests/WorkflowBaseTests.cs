﻿using System.IO;

using NUnit.Framework;

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

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class WorkflowBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			AdditionalUser = null;

			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);

			_projectsPage = new ProjectsPage(Driver);
			_projectSettingsDialog = new ProjectSettingsDialog(Driver);
			_informationFromWfSetupTabDialog = new InformationFromWFSetupTabDialog(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_editorPage = new EditorPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_taskDeclineDialog = new TaskDeclineDialog(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_usersTab = new UsersTab(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);
			_documentUploadGeneralInformationDialog = new AddFilesStep(Driver);
			_distributeSegmentsBetweenAssigneesPage = new DistributeSegmentsBetweenAssigneesPage(Driver);
			_distributeDocumentBetweenAssigneesPage = new DistributeDocumentBetweenAssigneesPage(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_workflowSetUptab = new WorkflowSetUpTab(Driver);

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_editorTxtFile = PathProvider.EditorTxtFile;
			_editorTxtFileName = Path.GetFileNameWithoutExtension(_editorTxtFile);

			_repetitionsTxtFile = PathProvider.RepetitionsTxtFile;
			_repetitionsTxtFileName = Path.GetFileNameWithoutExtension(_editorTxtFile);
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
		protected ProjectSettingsDialog _projectSettingsDialog;
		protected InformationFromWFSetupTabDialog _informationFromWfSetupTabDialog;
		protected ProjectSettingsPage _projectSettingsPage;
		protected SelectTaskDialog _selectTaskDialog;
		protected NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		protected NewProjectWorkflowPage _newProjectWorkflowPage;
		protected NewProjectSettingsPage _newProjectSettingsPage;
		protected EditorPage _editorPage;
		protected DistributeDocumentBetweenAssigneesPage _distributeDocumentBetweenAssigneesPage;
		protected DistributeSegmentsBetweenAssigneesPage _distributeSegmentsBetweenAssigneesPage;
		protected AddFilesStep _documentUploadGeneralInformationDialog;
		protected LoginHelper _loginHelper;
		protected TaskAssignmentPage _taskAssignmentPage;
		protected TaskDeclineDialog _taskDeclineDialog;
		protected const string _text = "Translation";
		protected TestUser _secondUser;
		protected AddAccessRightDialog _addAccessRightDialog;
		protected UsersTab _usersTab;
		protected NewGroupDialog _newGroupDialog;
		protected GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		protected WorkspacePage _workspacePage;
		protected WorkflowSetUpTab _workflowSetUptab;
		protected string _editorTxtFile;
		protected string _editorTxtFileName;
		protected string _repetitionsTxtFile;
		protected string _repetitionsTxtFileName;
	}
}
