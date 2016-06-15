using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.MachineTranslation;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.MachineTranslations
{
	public class BaseMTTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBaseMTTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_documentSettingsDialog = new DocumentSettingsDialog(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_fastMTAddFilesPage = new FastMTAddFilesPage(Driver);
			_fastMTAddFilesSettingsPage = new FastMTAddFilesSettingsPage(Driver);
			_adminHelper = new AdminHelper(Driver);
			_selectAccountForm = new SelectAccountForm(Driver);
			_signInPage = new SignInPage(Driver);

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		protected string _projectUniqueName;

		protected CreateProjectHelper _createProjectHelper;
		protected AdminHelper _adminHelper;
		protected ProjectSettingsHelper _projectSettingsHelper;

		protected WorkspacePage _workspacePage;
		protected ProjectsPage _projectsPage;
		protected SignInPage _signInPage;
		protected ProjectSettingsPage _projectSettingsPage;
		protected EditorPage _editorPage;
		protected FastMTAddFilesPage _fastMTAddFilesPage;
		protected FastMTAddFilesSettingsPage _fastMTAddFilesSettingsPage;

		protected DocumentSettingsDialog _documentSettingsDialog;
		protected SelectTaskDialog _selectTaskDialog;
		protected SelectAccountForm _selectAccountForm;
	}
}
