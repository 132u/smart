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
	class WorkflowTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test]
		public void DefaultTaskType()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickWorkflowButton();

			Assert.IsTrue(_newProjectWorkflowPage.IsWorkflowAddedTaskCountMatchExpected(expectedTaskCount: 1),
				"Произошла ошибка:\n неверное количество добавленных задач");

			Assert.IsTrue(_newProjectWorkflowPage.IsWorkflowTaskMatchExpected(
				WorkflowTask.Translation, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");
		}

		[Test]
		public void NewTaskTypesOnCreate()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickWorkflowButton();

			Assert.IsTrue(_newProjectWorkflowPage.IsTaskTypesCountMatchExpected(expectedCount: 4),
				"Произошла ошибка:\n неверное количество возможных типов задач показано в списке задач.");
		}

		[Test]
		public void NewTask()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				tasks: new[] { WorkflowTask.Translation, WorkflowTask.Proofreading });

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(2)
				.AssertWorkflowTaskMatch(WorkflowTask.Translation, taskNumber: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 2);
		}

		[TestCase(WorkflowTask.Translation)]
		[TestCase(WorkflowTask.Editing)]
		[TestCase(WorkflowTask.Proofreading)]
		[TestCase(WorkflowTask.Postediting)]
		public void NewTaskSameType(WorkflowTask task)
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, tasks: new[] { task, task });

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(2)
				.AssertWorkflowTaskMatch(task, taskNumber: 1)
				.AssertWorkflowTaskMatch(task, taskNumber: 2);
		}

		[Test]
		public void AddTaskForExistingProject()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.AddTask(WorkflowTask.Proofreading)
				.ClickSaveButton()
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(2)
				.AssertWorkflowTaskMatch(WorkflowTask.Translation, taskNumber: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 2);
		}

		[Test]
		public void CancelAddingTask()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.AddTask(WorkflowTask.Proofreading)
				.ClickCancelButton()
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Translation, taskNumber: 1);
		}

		[Test]
		public void ChangeTaskTypeOnProjectCreate()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickWorkflowButton();

			_newProjectWorkflowPage
				.ClickNewTaskButton(WorkflowTask.Proofreading)
				.DeleteWorkflowTask(taskNumber: 1);

			Assert.IsTrue(_newProjectWorkflowPage.IsWorkflowAddedTaskCountMatchExpected(expectedTaskCount: 1),
				"Произошла ошибка:\n неверное количество добавленных задач");

			Assert.IsTrue(_newProjectWorkflowPage.IsWorkflowTaskMatchExpected(WorkflowTask.Proofreading, taskNumber: 1), 
				"Произошла ошибка:\n добавленная задача не соответствует ожидаемой");
		}

		[Test]
		public void DeleteTaskOnExistingProject()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName, 
				tasks: new[] { WorkflowTask.Translation, WorkflowTask.Proofreading });

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.ClickDeleteTaskButton()
				.AssertWorkflowTaskCountMatch(1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 1)
				.ClickSaveButton()
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 1);
		}

		[Test]
		public void DeleteAllTasksOnCreate()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickWorkflowButton();

			_newProjectWorkflowPage.DeleteWorkflowTask(taskNumber: 1);

			Assert.IsTrue(_newProjectWorkflowPage.IsEmptyWorkflowErrorMessageDisplayed(),
				"Произошла ошибка:\n Сообщение 'Add at least one task to complete project creation' не появилось");
		}

		[Test]
		public void CancelDeleteTaskInExistingProject()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				tasks: new[] { WorkflowTask.Translation, WorkflowTask.Proofreading });

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.ClickDeleteTaskButton()
				.ClickCancelButton()
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(2)
				.AssertWorkflowTaskMatch(WorkflowTask.Translation, taskNumber: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 2);
		}

		[Test]
		public void ChangeTaskInExistingProject()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.EditTask(WorkflowTask.Proofreading, taskNumber: 1)
				.ClickSaveButton()
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 1);
		}

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		private NewProjectWorkflowPage _newProjectWorkflowPage;
		private NewProjectSettingsPage _newProjectSettingsPage;
	}
}
