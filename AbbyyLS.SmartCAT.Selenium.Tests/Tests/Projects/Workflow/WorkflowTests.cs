using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Standalone]
	class WorkflowTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test]
		public void DefaultTaskType()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(task: WorkflowTask.Translation, taskNumber: 1);
		}

		[Test]
		public void ChangeTaskType()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.SelectWorkflowTask(WorkflowTask.Editing)
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(task: WorkflowTask.Editing, taskNumber: 1)
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Editing);
		}

		[Test]
		public void NewTaskTypesOnCreate()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.ExpandWorkflowDropdown(taskNumber: 2)
				.AssertTaskOptionsCountMatch(taskNumber: 2, count: 4);
		}

		[Test]
		public void NewTask()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickFinishOnProjectSetUpWorkflowDialog()
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
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.SelectWorkflowTask(task, 1)
				.ClickNewTaskButton()
				.SelectWorkflowTask(task, 2)
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(2)
				.AssertWorkflowTaskMatch(task, taskNumber: 1)
				.AssertWorkflowTaskMatch(task, taskNumber: 2);
		}

		[Test]
		public void AddTaskForExistingProject()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
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
			_createProjectHelper.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
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
			_createProjectHelper
				.ClickCreateProjectButton()
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
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickFinishOnProjectSetUpWorkflowDialog()
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
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.DeleteWorkflowTask(taskNumber: 1)
				.ClickNextOnWorkflowPage(errorExpected: true)
				.AssertEmptyWorkflowErrorDisplayed();
		}

		[Test]
		public void CancelDeleteTaskInExistingProject()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, 2)
				.ClickFinishOnProjectSetUpWorkflowDialog()
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
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
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
			_createProjectHelper
				.ClickCreateProjectButton()
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
			_createProjectHelper
				.ClickCreateProjectButton()
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
				.ClickFinishOnProjectSetUpWorkflowDialog()
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
			_createProjectHelper
				.ClickCreateProjectButton()
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
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(2)
				.AssertWorkflowTaskMatch(WorkflowTask.Translation, taskNumber: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 2);
		}

		[Test]
		public void DeletingTaskAfterBack()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.SelectWorkflowTask(WorkflowTask.Proofreading, taskNumber: 2)
				.ClickNextOnWorkflowPage()
				.ClickBackButtonOnTMStep()
				.DeleteWorkflowTask(taskNumber: 1)
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 1)
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.AssertWorkflowTaskCountMatch(taskCount: 1)
				.AssertWorkflowTaskMatch(WorkflowTask.Proofreading, taskNumber: 1);
		}

		private string _projectUniqueName;
		private readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
	}
}
