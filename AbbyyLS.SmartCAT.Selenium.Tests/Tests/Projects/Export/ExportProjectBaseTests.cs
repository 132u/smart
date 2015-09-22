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
			_workspaceHelper = new WorkspaceHelper();
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
		protected readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
		protected readonly ExportFileHelper _exportFileHelper = new ExportFileHelper();
		protected readonly ProjectSettingsHelper _projectSettingsHelper = new ProjectSettingsHelper();
		protected readonly ProjectsHelper _projectsHelper = new ProjectsHelper();
		private WorkspaceHelper _workspaceHelper;
	}
}
