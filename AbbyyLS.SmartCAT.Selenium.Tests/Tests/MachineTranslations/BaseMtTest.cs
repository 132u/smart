using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
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

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		protected string _projectUniqueName;
		protected WorkspacePage _workspacePage;
		protected CreateProjectHelper _createProjectHelper;
		protected ProjectsPage _projectsPage;
		protected DocumentSettingsDialog _documentSettingsDialog;
		protected ProjectSettingsPage _projectSettingsPage;
		protected EditorPage _editorPage;
		protected SelectTaskDialog _selectTaskDialog;
		protected ProjectSettingsHelper _projectSettingsHelper;
		protected FastMTAddFilesPage _fastMTAddFilesPage;
		protected FastMTAddFilesSettingsPage _fastMTAddFilesSettingsPage;
	}
}
