using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[TestFixture]
	[Category("Standalone")]
	class ProjectDeadlineTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpProjectDeadlineTest()
		{
			WorkspaceHelper.GoToProjectsPage();
			_createProjectHelper = new CreateProjectHelper();
			_projectsHelper = new ProjectsHelper();
		}

		[Test]
		[TestCase(Deadline.CurrentDate)]
		[TestCase(Deadline.NextMonth)]
		[TestCase(Deadline.PreviousMonth)]
		public void CreateProjectWithDeadline(Deadline deadline)
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName, deadline: deadline)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.WaitCreateProjectDialogDisappear();

			_projectsHelper.CheckProjectAppearInList(projectUniqueName);
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

			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName, deadline: Deadline.FillDeadlineDate, date: dateFormat)
				.ClickNextOnGeneralProjectInformationPage(errorExpected: true)
				.AssertErrorDeadlineDate(dateFormat);
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectsHelper _projectsHelper;
	}
}