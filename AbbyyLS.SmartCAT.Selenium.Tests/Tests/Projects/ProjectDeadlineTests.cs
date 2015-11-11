using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
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
			_newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog(Driver);
			_newProjectSetUpWorkflowDialog = new NewProjectSetUpWorkflowDialog(Driver);

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[TestCase(Deadline.CurrentDate)]
		[TestCase(Deadline.NextMonth)]
		[TestCase(Deadline.PreviousMonth)]
		public void CreateProjectWithDeadline(Deadline deadline)
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName, deadline: deadline)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.ClickFinishButton();

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", _projectUniqueName);
		}

		[TestCase("03/03/20166")]
		[TestCase("03 03/2016")]
		[TestCase("0303/2016")]
		[TestCase("033/03/2016")]
		[TestCase("03/033/2016")]
		[TestCase("03/03/201")]
		public void InvalidDeadlineDateFormat(string dateFormat)
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName, deadline: Deadline.FillDeadlineDate, date: dateFormat)
				.ClickNextButton<NewProjectGeneralInformationDialog>();

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsErrorDeadlineDateMessageDisplayed(),
				"Произошла ошибка:\n При введении некорректной даты '{0}' не было сообщения о неверном формате даты", dateFormat);
		}

		private string _projectUniqueName;

		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private WorkspaceHelper _workspaceHelper;
		private NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog;
		private NewProjectSetUpWorkflowDialog _newProjectSetUpWorkflowDialog;
	}
}
