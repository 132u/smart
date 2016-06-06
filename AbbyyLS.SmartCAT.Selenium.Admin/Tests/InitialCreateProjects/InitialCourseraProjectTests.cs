using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Admin.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[CreateProject]
	[Coursera]
	class InitialCourseraProjectTests<TWebDriverProvider> : InitialProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public InitialCourseraProjectTests()
		{
			StartPage = StartPage.Coursera;
		}

		[SetUp]
		public void InitialCourseraProjectTestsSetUp()
		{
			_courseraHomePage = new CourseraHomePage(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);

			_loginHelper.LogInCoursera(ThreadUser.Login, ThreadUser.Password);
		}

		[Test]
		public void CreateCourseraProject()
		{
			var projectName = CreateProjectHelper.CourseraProjectName;

			_courseraHomePage.ClickWorkspaceButtonWithoutWaiting();

			if (!_newProjectDocumentUploadPage.IsNewProjectDocumentUploadPageOpened())
			{
				_projectsPage.ClickCreateProjectButton();
			}

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { PathProvider.DocumentFile })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(projectName)
				.ClickNextButton();

			_newProjectWorkflowPage
				.ClickClearButton()
				.ClickNewTaskButton(WorkflowTask.CrowdTranslation)
				.ClickNewTaskButton(WorkflowTask.CrowdReview)
				.ClickCreateProjectButton();

			Assert.IsTrue(_projectsPage.IsProjectExist(projectName),
				"Произошла ошибка: \nне найден проект с новым именем '{0}'.", projectName);

			_projectsPage.OpenProjectSettingsPage(projectName);

			_projectSettingsHelper.UploadDocument(PathProvider.GetFilesFromCourseraFolder());
		}

		private ProjectSettingsHelper _projectSettingsHelper;
		private CourseraHomePage _courseraHomePage;
	}
}
