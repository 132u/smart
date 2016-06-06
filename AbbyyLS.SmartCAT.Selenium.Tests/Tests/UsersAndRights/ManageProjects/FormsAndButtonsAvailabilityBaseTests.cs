using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.AssignmentPages;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class FormsAndButtonsAvailabilityBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
				_document1 = PathProvider.DocumentFile;
				_document2 = PathProvider.DocumentFile2;

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
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
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
			_datePicker = new DatePicker(Driver);
			_documentUploadGeneralInformationDialog = new AddFilesStep(Driver);

			// Нужен лог для отладки тестов SCAT-938
			CustomTestContext.WriteLine("OneTimeSetUp FormsAndButtonsAvailabilityBaseTests.");

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			var groupName = Guid.NewGuid().ToString();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_userRightsHelper.CreateGroupWithSpecificRights(
				AdditionalUser.FullName,
				groupName,
				new List<RightsType>{RightsType.ProjectResourceManagement});
			
			_workspacePage.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { _document1, _document2 });

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

		protected UserRightsHelper _userRightsHelper;
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
		protected DatePicker _datePicker;
		protected QualityAssuranceSettings _qualityAssuranceSettings;
		protected CancelConfirmationDialog _cancelConfirmationDialog;
		protected TaskAssignmentPage _taskAssignmentPage;
		protected ConfirmDeclineTaskDialog _confirmDeclineTaskDialog;
		protected DocumentSettingsDialog _documentSettingsDialog;
		protected SelectTaskDialog _selectTaskDialog;
		protected string _document1;
		protected string _document2;
	}
}
