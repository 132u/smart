using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog.AdvancedSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	public class EditorBaseTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_settingsDialog = new ProjectSettingsDialog(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_signInPage = new SignInPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_workflowSetUptab = new WorkflowSetUpTab(Driver);
			_pretranslateSettingsSection = new PretranslateSettingsSection(Driver);
		}

		[SetUp]
		public virtual void SetupTest()
		{
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_document = PathProvider.EditorTxtFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { _document });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.AssignTasksOnDocument(_document, ThreadUser.FullName, _projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(_document);

			_selectTaskDialog.SelectTask();
		}

		protected CreateProjectHelper _createProjectHelper;
		protected ProjectSettingsHelper _projectSettingsHelper;
		protected ProjectSettingsPage _projectSettingsPage;
		protected ProjectSettingsDialog _settingsDialog;
		protected EditorPage _editorPage;
		protected SelectTaskDialog _selectTaskDialog;
		protected SignInPage _signInPage;
		protected ProjectsPage _projectsPage;
		protected string _projectUniqueName;
		protected WorkspacePage _workspacePage;
		protected WorkflowSetUpTab _workflowSetUptab;
		protected TaskAssignmentPage _taskAssignmentPage;
		protected PretranslateSettingsSection _pretranslateSettingsSection;

		protected string _document;
	}
}
