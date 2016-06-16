using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries.SuggestedTerms;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageGlossaryContent
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageGlossaryContentBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_clientsPage = new ClientsPage(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_glossariesAdvancedSettingsSection = new GlossariesAdvancedSettingsSection(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_exportNotification = new ExportNotification(Driver);
			_settingsDialog = new ProjectSettingsDialog(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
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
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			_glossariesHelper = new GlossariesHelper(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_glossaryPropertiesDialog=new GlossaryPropertiesDialog(Driver);
			_projectGroupsPage = new ProjectGroupsPage(Driver);
			_glossaryStructureDialog = new GlossaryStructureDialog(Driver);
			_glossaryImportDialog = new GlossaryImportDialog(Driver);
			_glossarySuccessImportDialog = new GlossarySuccessImportDialog(Driver);
			_suggestTermDialog = new SuggestTermDialog(Driver);
			_suggestTermDialog = new SuggestTermDialog(Driver);
			_suggestedTermsPageForAllGlossaries = new SuggestedTermsGlossariesPage(Driver);
			_suggestedTermsPageForCurrentGlossaries = new SuggestedTermsGlossaryPage(Driver);
			_addTermDialog = new AddTermDialog(Driver);
			_glossaryPage = new GlossaryPage(Driver);

			_clientName = _clientsPage.GetClientUniqueName();

			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			_glossaryUniqueName2 = GlossariesHelper.UniqueGlossaryName();
			_glossaryUniqueName3 = GlossariesHelper.UniqueGlossaryName();
			_glossaryUniqueName4 = GlossariesHelper.UniqueGlossaryName();
			_glossaryUniqueName5 = GlossariesHelper.UniqueGlossaryName();
			
			_term1 = "term-" + Guid.NewGuid();
			_term2 = "term-" + Guid.NewGuid();
			_term3 = "term-" + Guid.NewGuid();
			_term4 = "term-" + Guid.NewGuid();
			_term5 = "term-" + Guid.NewGuid();
			_term6 = "term-" + Guid.NewGuid();

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			var groupName = Guid.NewGuid().ToString();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_userRightsHelper.CreateGroupWithSpecificRights(
				AdditionalUser.NickName,
				groupName,
				new List<RightsType> { RightsType.GlossaryContentManagement });

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName);

			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);
			_glossaryPage.CreateTerm(_term1, _term2);

			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(_glossaryUniqueName2, client: _clientName);
			_glossaryPage.CreateTerm(_term3, _term4);

			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(_glossaryUniqueName3, client: _clientName);
			_glossaryPage.CreateTerm(_term5, _term6);
		}

		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
				_loginHelper.Authorize(StartPage.Workspace, AdditionalUser);

				_workspacePage.GoToGlossariesPage();

				_exportNotification.CloseAllNotifications<GlossariesPage>();
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}
		}

		protected string _term1;
		protected string _term2;
		protected string _term3;
		protected string _term4;
		protected string _term5;
		protected string _term6;

		protected string _glossaryUniqueName;
		protected string _glossaryUniqueName2;
		protected string _glossaryUniqueName3;
		protected string _glossaryUniqueName4;
		protected string _glossaryUniqueName5;

		protected string _projectGroup;

		protected string _clientName;

		protected AddTermDialog _addTermDialog;
		protected SuggestedTermsGlossaryPage _suggestedTermsPageForCurrentGlossaries;
		protected ProjectGroupsPage _projectGroupsPage;
		protected GlossaryPropertiesDialog _glossaryPropertiesDialog;
		protected ClientsPage _clientsPage;
		protected GlossariesPage _glossariesPage;
		protected GlossaryPage _glossaryPage;
		protected GlossariesAdvancedSettingsSection _glossariesAdvancedSettingsSection;
		protected NewProjectWorkflowPage _newProjectWorkflowPage;
		protected NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		protected NewProjectSettingsPage _newProjectSettingsPage;
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
		protected ProjectsPage _projectsPage;
		protected ExportNotification _exportNotification;
		protected NewGroupDialog _newGroupDialog;
		protected GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		protected ProjectSettingsPage _projectSettingsPage;
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
		protected GlossariesHelper _glossariesHelper;
		protected GlossaryStructureDialog _glossaryStructureDialog;
		protected GlossaryImportDialog _glossaryImportDialog;
		protected GlossarySuccessImportDialog _glossarySuccessImportDialog;
		protected SuggestTermDialog _suggestTermDialog;
		protected SuggestedTermsGlossariesPage _suggestedTermsPageForAllGlossaries;
	}
}
