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
	class WorkflowTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test]
		public void DefaultTaskType()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(task: WorkflowTask.Translation, taskNumber: 1);
		}

		[Test]
		public void ChangeTaskType()
		{
			_projectsPage.ClickCreateProjectDialog();

			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.SelectWorkflowTask(WorkflowTask.Editing)
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(task: WorkflowTask.Editing, taskNumber: 1)
				.ClickFinishOnProjectSetUpWorkflowDialog();

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
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.ExpandWorkflowDropdown(taskNumber: 2)
				.AssertTaskOptionsCountMatch(taskNumber: 2, count: 4);
		}

		[Test]
		public void NewTask()
		{
			_projectsPage.ClickCreateProjectDialog();

			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickFinishOnProjectSetUpWorkflowDialog();

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
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.SelectWorkflowTask(task, 1)
				.ClickNewTaskButton()
				.SelectWorkflowTask(task, 2)
				.ClickFinishOnProjectSetUpWorkflowDialog();

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
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog();

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
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog();

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
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, taskNumber: 2)
				.DeleteWorkflowTask(taskNumber: 1)
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 1);
		}

		[Test]
		public void DeleteTaskOnExistingProject()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickFinishOnProjectSetUpWorkflowDialog();

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
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.DeleteWorkflowTask(taskNumber: 1)
				.ClickNextOnWorkflowPage(errorExpected: true)
				.AssertEmptyWorkflowErrorDisplayed();
		}

		[Test]
		public void CancelDeleteTaskInExistingProject()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickFinishOnProjectSetUpWorkflowDialog();

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
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog();

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
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickNextOnWorkflowPage()
				.ClickBackButtonOnTMStep()
				.AssertWorkflowTaskCountMatch(2)
				.AssertWorkflowTaskMatch(WorkflowTask.Translation, taskNumber: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 2);
		}

		[Test]
		public void AddingTaskAfterBack()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickNextOnWorkflowPage()
				.ClickBackButtonOnTMStep()
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Editing, 3)
				.AssertWorkflowTaskCountMatch(3)
				.AssertWorkflowTaskMatch(WorkflowTask.Translation, taskNumber: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 2)
				.AssertWorkflowTaskMatch(WorkflowTask.Editing, taskNumber: 3)
				.ClickFinishOnProjectSetUpWorkflowDialog();

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
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickNextOnWorkflowPage()
				.ClickBackButtonOnTMStep()
				.SelectWorkflowTask(WorkflowTask.Editing, 2)
				.ClickBackButtonOnWorkflowStep()
				.ClickNextOnGeneralProjectInformationPage()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.AssertWorkflowTaskCountMatch(2)
				.AssertWorkflowTaskMatch(WorkflowTask.Translation, taskNumber: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 2)
				.ClickFinishOnProjectSetUpWorkflowDialog();

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
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, taskNumber: 2)
				.ClickNextOnWorkflowPage()
				.ClickBackButtonOnTMStep()
				.DeleteWorkflowTask(taskNumber: 1)
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 1)
				.ClickFinishOnProjectSetUpWorkflowDialog();

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
	}
}
