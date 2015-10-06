using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	class ExportProjectBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpProjectExportTests()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_exportFileHelper = new ExportFileHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_projectsHelper = new ProjectsHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);

			_workspaceHelper.GoToProjectsPage();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(_projectUniqueName, filePath: PathProvider.DocumentFileToConfirm1)
				.AssertIsProjectLoadedSuccessfully(_projectUniqueName)
				.GoToProjectSettingsPage(_projectUniqueName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1));
		}

		protected string _projectUniqueName;
		protected CreateProjectHelper _createProjectHelper;
		protected ExportFileHelper _exportFileHelper;
		protected ProjectSettingsHelper _projectSettingsHelper;
		protected ProjectsHelper _projectsHelper;
		private WorkspaceHelper _workspaceHelper;
	}
}
