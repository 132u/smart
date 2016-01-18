using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Admin.Tests.InitialProjectTests;
using AbbyyLS.SmartCAT.Selenium.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests.InitialCourseraProjectTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	class InitialCourseraProjectTests<TWebDriverProvider> : InitialProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public InitialCourseraProjectTests()
		{
			GlobalSetup.SetUp();
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
			_courseraHomePage.ClickWorkspaceButton();

			_createProjectHelper.CreateNewProject(
				CreateProjectHelper.CourseraProjectName,
				tasks: new[] { 
					WorkflowTask.CrowdTranslation,
					WorkflowTask.CrowdReview});

			_projectsPage.ClickProject(CreateProjectHelper.CourseraProjectName);

			_projectSettingsHelper.UploadDocument(PathProvider.GetFilesFromCourseraFolder());
		}

		private ProjectSettingsHelper _projectSettingsHelper;
		private CourseraHomePage _courseraHomePage;
	}
}
