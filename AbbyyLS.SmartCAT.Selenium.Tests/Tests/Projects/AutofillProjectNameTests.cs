using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	class AutofillProjectNameTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);
			_workspaceHelper.GoToProjectsPage();
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
		}

		[Test]
		public void AutofillProjectName()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.AddFileFromWizard(_filePath)
				.AssertProjectNameMatch(_fileName);
		}

		[Test]
		public void AutofillProjectNameAddTwoFiles()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.AddFileFromWizard(_filePath)
				.AssertProjectNameMatch(_fileName)
				.AddFileFromWizard(_filePath)
				.AssertProjectNameMatch(_fileName);
		}

		[Test]
		public void AutofillProjectNameDeleteFile()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.AddFileFromWizard(_filePath)
				.AssertProjectNameMatch(_fileName)
				.DeleteFileFromWizard(_filePath)
				.AssertProjectNameMatch(_fileName);
		}

		private CreateProjectHelper _createProjectHelper;
		private string _fileName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);
		private string _filePath = PathProvider.DocumentFile;
		private WorkspaceHelper _workspaceHelper;
		private ProjectsPage _projectsPage;
	}
}
