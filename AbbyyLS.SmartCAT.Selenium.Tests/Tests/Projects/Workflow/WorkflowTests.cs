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
			_newProjectSetUpWorkflowDialog = new NewProjectSetUpWorkflowDialog(Driver);
			_newProjectSetUpTMDialog = new NewProjectSetUpTMDialog(Driver);
			_newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog(Driver);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test]
		public void DefaultTaskType()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskCountMatchExpected(taskCount: 1),
				"Произошла ошибка:\n неверное количество задач");

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskMatchExpected(
				WorkflowTask.Translation, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");
		}

		[Test]
		public void ChangeTaskType()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.SelectWorkflowTask(WorkflowTask.Editing);

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskCountMatchExpected(taskCount: 1),
				"Произошла ошибка:\n неверное количество задач");

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskMatchExpected(
				WorkflowTask.Editing, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");

			_newProjectSetUpWorkflowDialog.ClickFinishButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Editing);
		}

		[Test]
		public void NewTaskTypesOnCreate()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.ClickNewTaskButton()
				.ExpandWorkflowDropdown(taskNumber: 2);

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsTaskOptionsCountMatchExpected(taskNumber: 2, expectedCount: 4),
				"Произошла ошибка:\n неверное количество задач в дропдауне на этапе Workflow");
		}

		[Test]
		public void NewTask()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickFinishButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

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
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.SelectWorkflowTask(task, 1)
				.ClickNewTaskButton()
				.SelectWorkflowTask(task, 2)
				.ClickFinishButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

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
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.ClickFinishButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

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
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.ClickFinishButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

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

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, taskNumber: 2)
				.DeleteWorkflowTask(taskNumber: 1);

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskCountMatchExpected(taskCount: 1),
				"Произошла ошибка:\n неверное количество задач");

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskMatchExpected(
				WorkflowTask.Proofreading, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");
		}

		[Test]
		public void DeleteTaskOnExistingProject()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickFinishButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.ClickDeleteTaskButton()
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 1)
				.ClickSaveButton()
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 1);
		}

		[Test]
		public void DeleteAllTasksOnCreate()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.DeleteWorkflowTask(taskNumber: 1)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsEmptyWorkflowErrorMessageDisplayed(),
				"Произошла ошибка:\n Сообщение 'Select at least one task' не появилось");
		}

		[Test]
		public void CancelDeleteTaskInExistingProject()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickFinishButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

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
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>()
				.ClickFinishButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.EditTask(WorkflowTask.Proofreading, taskNumber: 1)
				.ClickSaveButton()
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 1);
		}

		[Test]
		public void BackOnCreate()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickNextButton<NewProjectSetUpTMDialog>();

			_newProjectSetUpTMDialog.ClickBackButton<NewProjectSetUpWorkflowDialog>();

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskCountMatchExpected(taskCount: 2),
				"Произошла ошибка:\n неверное количество задач");

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskMatchExpected(
				WorkflowTask.Translation, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskMatchExpected(
				WorkflowTask.Proofreading, taskNumber: 2), "Произошла ошибка:\n задача не соответствует ожидаемой");
		}

		[Test]
		public void AddingTaskAfterBack()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickNextButton<NewProjectSetUpTMDialog>();

			_newProjectSetUpTMDialog.ClickBackButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Editing, 3);

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskCountMatchExpected(taskCount: 3),
				"Произошла ошибка:\n неверное количество задач");

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskMatchExpected(
				WorkflowTask.Translation, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskMatchExpected(
				WorkflowTask.Proofreading, taskNumber: 2), "Произошла ошибка:\n задача не соответствует ожидаемой");

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskMatchExpected(
				WorkflowTask.Editing, taskNumber: 3), "Произошла ошибка:\n задача не соответствует ожидаемой");

			_newProjectSetUpWorkflowDialog.ClickFinishButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(3)
				.AssertWorkflowTaskMatch(WorkflowTask.Translation, taskNumber: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 2)
				.AssertWorkflowTaskMatch(WorkflowTask.Editing, taskNumber: 3);
		}

		[Test]
		public void ChangingTaskAfterBack()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickNextButton<NewProjectSetUpTMDialog>();

			_newProjectSetUpTMDialog.ClickBackButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.SelectWorkflowTask(WorkflowTask.Editing, 2)
				.ClickBackButton<NewProjectGeneralInformationDialog>();

			_newProjectGeneralInformationDialog.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.SelectWorkflowTask(WorkflowTask.Proofreading, 2);

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskCountMatchExpected(taskCount: 2),
				"Произошла ошибка:\n неверное количество задач");

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskMatchExpected(
				WorkflowTask.Translation, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskMatchExpected(
				WorkflowTask.Proofreading, taskNumber: 2), "Произошла ошибка:\n задача не соответствует ожидаемой");

			_newProjectSetUpWorkflowDialog.ClickFinishButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(2)
				.AssertWorkflowTaskMatch(WorkflowTask.Translation, taskNumber: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 2);
		}

		[Test]
		public void DeletingTaskAfterBack()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, taskNumber: 2)
				.ClickNextButton<NewProjectSetUpTMDialog>();

			_newProjectSetUpTMDialog.ClickBackButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.DeleteWorkflowTask(taskNumber: 1);

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskCountMatchExpected(taskCount: 1),
				"Произошла ошибка:\n неверное количество задач");

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskMatchExpected(
				WorkflowTask.Proofreading, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");

			_newProjectSetUpWorkflowDialog.ClickFinishButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 1);
		}

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private NewProjectSetUpWorkflowDialog _newProjectSetUpWorkflowDialog;
		private NewProjectSetUpTMDialog _newProjectSetUpTMDialog;
		private NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog;
	}
}
