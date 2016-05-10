using NUnit.Framework;

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
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	public class ManageGlossariesCommonBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
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
			_glossaryPropertiesDialog = new GlossaryPropertiesDialog(Driver);
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
			_newGlossaryDialog = new Pages.Glossaries.NewGlossaryDialog(Driver);
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
		protected string _projectGroup;
		protected string _commonClientName;
		protected string _commonClientName2;
		protected string _glossaryUniqueName;
		protected string _commonGlossaryUniqueName;

		protected Pages.Glossaries.NewGlossaryDialog _newGlossaryDialog;
		protected AddTermDialog _addTermDialog;
		protected SuggestedTermsGlossaryPage _suggestedTermsPageForCurrentGlossaries;
		protected string _clientName;
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
