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
			_createProjectHelper = new CreateProjectHelper(Driver);

			_projectsPage = new ProjectsPage(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_workspaceHelper.GoToProjectsPage();
		}

		[TestCase(Deadline.CurrentDate)]
		[TestCase(Deadline.NextMonth)]
		[TestCase(Deadline.PreviousMonth)]
		public void CreateProjectWithDeadline(Deadline deadline)
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName, deadline: deadline)
				.ClickWorkflowButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

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

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage.FillGeneralProjectInformation(_projectUniqueName,
				deadline: Deadline.FillDeadlineDate,
				date: dateFormat);

			Assert.IsTrue(_newProjectSettingsPage.IsErrorDeadlineDateMessageDisplayed(),
				"Произошла ошибка:\n При введении некорректной даты '{0}' не было сообщения о неверном формате даты", dateFormat);
		}

		private string _projectUniqueName;

		private CreateProjectHelper _createProjectHelper;
		private WorkspaceHelper _workspaceHelper;

		private ProjectsPage _projectsPage;
		private NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		private NewProjectSettingsPage _newProjectSettingsPage;
		private NewProjectWorkflowPage _newProjectWorkflowPage;
	}
}
