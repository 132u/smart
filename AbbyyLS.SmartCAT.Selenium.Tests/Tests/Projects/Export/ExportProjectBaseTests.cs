using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	class ExportProjectBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpProjectExportTests()
		{
			CreateProjectHelper = new CreateProjectHelper(Driver);
			ProjectSettingsHelper = new ProjectSettingsHelper(Driver);
			WorkspaceHelper = new WorkspaceHelper(Driver);
			ProjectsPage = new ProjectsPage(Driver);
			DocumentSettingsDialog = new DocumentSettingsDialog(Driver);
			ProjectSettingsPage = new ProjectSettingsPage(Driver);
            ExportNotification = new ExportNotification(Driver);
            WorkspacePage = new WorkspacePage(Driver);

            ProjectUniqueName = CreateProjectHelper.GetProjectUniqueName();

			WorkspaceHelper.GoToProjectsPage();

            ExportNotification.CancelAllNotifiers<ProjectsPage>();

			CreateProjectHelper.CreateNewProject(ProjectUniqueName, filePath: PathProvider.DocumentFileToConfirm1);

			WorkspaceHelper
				.GoToProjectSettingsPage(ProjectUniqueName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1));
		}

		protected string ProjectUniqueName;
		protected CreateProjectHelper CreateProjectHelper;
		protected ProjectSettingsHelper ProjectSettingsHelper;
		protected ProjectsPage ProjectsPage;
		protected DocumentSettingsDialog DocumentSettingsDialog;
		protected WorkspaceHelper WorkspaceHelper;
		protected ProjectSettingsPage ProjectSettingsPage;
	    protected ExportNotification ExportNotification;
	    protected WorkspacePage WorkspacePage;
	}
}
