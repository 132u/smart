using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	public class BaseProjectTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_deleteDialog = new DeleteDialog(Driver);
			_editorPage = new EditorPage(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_documentUploadGeneralInformationDialog = new AddFilesStep(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			_dublicateFileErrorDialog = new DublicateFileErrorDialog(Driver);
			_projectSettingsDialog = new ProjectSettingsDialog(Driver);
			_translationMemoriesPage = new TranslationMemoriesPage(Driver);
			_newLanguageSettingsDialog = new NewLanguageSettingsDialog(Driver);
			_glossariesHelper = new GlossariesHelper(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_glossaryPage = new GlossaryPage(Driver);
			_cancelledProjectsPage = new CancelledProjectsPage(Driver);
			_adminHelper = new AdminHelper(Driver);
			_adminSignInPage = new AdminSignInPage(Driver);

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_workspacePage.GoToProjectsPage();
		}

		protected string _projectUniqueName;
		protected const string _longName = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";

		protected AdminSignInPage _adminSignInPage;
		protected AdminHelper _adminHelper;
		protected NewLanguageSettingsDialog _newLanguageSettingsDialog;
		protected ProjectSettingsDialog _projectSettingsDialog;
		protected DublicateFileErrorDialog _dublicateFileErrorDialog;
		protected WorkspacePage _workspacePage;
		protected CreateProjectHelper _createProjectHelper;
		protected ProjectSettingsHelper _projectSettingsHelper;
		protected ProjectsPage _projectsPage;
		protected CancelledProjectsPage _cancelledProjectsPage;
		protected DeleteDialog _deleteDialog;
		protected EditorPage _editorPage;
		protected NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		protected NewProjectSettingsPage _newProjectSettingsPage;
		protected ProjectSettingsPage _projectSettingsPage;
		protected AddFilesStep _documentUploadGeneralInformationDialog;
		protected NewProjectWorkflowPage _newProjectWorkflowPage;
		protected TranslationMemoriesPage _translationMemoriesPage;
		protected GlossariesHelper _glossariesHelper;
		protected GlossariesPage _glossariesPage;
		protected GlossaryPage _glossaryPage;
	}
}