using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.DriversAndSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[TestFixture]
	[Category("Standalone")]
	class ProjectDeadlineTests<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[SetUp]
		public void SetUpProjectDeadlineTest()
		{
			QuitDriverAfterTest = true;
			WorkspaceHelper.GoToProjectsPage();
			_createProjectHelper = new CreateProjectHelper();
			_projectsHelper = new ProjectsHelper();
		}

		[Test]
		public void CreateProjectCurrentDeadlineDate()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.WaitCreateProjectDialogDissapear();

			_projectsHelper.CheckProjectAppearInList(projectUniqueName);
		}

		[Test]
		public void CreateProjectFutureDeadlineDate()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName, deadline: Deadline.NextMonth)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.WaitCreateProjectDialogDissapear();

			_projectsHelper.CheckProjectAppearInList(projectUniqueName);
		}

		[Test]
		public void CreateProjectPastDeadlineDate()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName, deadline: Deadline.PreviousMonth)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.WaitCreateProjectDialogDissapear();

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