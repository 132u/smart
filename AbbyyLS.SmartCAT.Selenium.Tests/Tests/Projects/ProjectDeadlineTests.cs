using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class ProjectDeadlineTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpProjectDeadlineTest()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);
			_workspaceHelper.GoToProjectsPage();
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
		}

		[TestCase(Deadline.CurrentDate)]
		[TestCase(Deadline.NextMonth)]
		[TestCase(Deadline.PreviousMonth)]
		public void CreateProjectWithDeadline(Deadline deadline)
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_projectsPage.ClickCreateProjectDialog();

			_createProjectHelper
				.FillGeneralProjectInformation(projectUniqueName, deadline: deadline)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog();

			_projectsPage.WaitUntilProjectLoadSuccessfully(projectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", projectUniqueName);
		}

		[TestCase("03/03/20166")]
		[TestCase("03 03/2016")]
		[TestCase("0303/2016")]
		[TestCase("033/03/2016")]
		[TestCase("03/033/2016")]
		[TestCase("03/03/201")]
		public void InvalidDeadlineDateFormat(string dateFormat)
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(projectUniqueName, deadline: Deadline.FillDeadlineDate, date: dateFormat)
				.ClickNextOnGeneralProjectInformationPage(errorExpected: true)
				.AssertErrorDeadlineDate(dateFormat);
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private WorkspaceHelper _workspaceHelper;
	}
}