using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Projects]
	class WorkflowTests<TWebDriverProvider> : WorkflowBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void DefaultTaskType()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton();

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
				.ClickNextButton();

			Assert.IsTrue(_newProjectWorkflowPage.IsTaskTypesCountMatchExpected(expectedCount: 4),
				"Произошла ошибка:\n неверное количество возможных типов задач показано в списке задач.");
		}

		[Test]
		public void NewTask()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				tasks: new[] { WorkflowTask.Translation, WorkflowTask.Proofreading });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.OpenWorkflowSettings();

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskCountMatchExpected(taskCount: 2),
				"Произошла ошибка:\n неверное количество добавленных задач");

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskMatchExpected(
				WorkflowTask.Translation, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskMatchExpected(
				WorkflowTask.Proofreading, taskNumber: 2), "Произошла ошибка:\n задача не соответствует ожидаемой");
		}

		[TestCase(WorkflowTask.Translation)]
		[TestCase(WorkflowTask.Editing)]
		[TestCase(WorkflowTask.Proofreading)]
		[TestCase(WorkflowTask.Postediting)]
		public void NewTaskSameType(WorkflowTask task)
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, tasks: new[] { task, task });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.OpenWorkflowSettings();

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskCountMatchExpected(taskCount: 2),
				"Произошла ошибка:\n неверное количество добавленных задач");

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskMatchExpected(
				task, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskMatchExpected(
				task, taskNumber: 2), "Произошла ошибка:\n задача не соответствует ожидаемой");
		}

		[Test, Description("S-7186"), ShortCheckList]
		public void AddTaskForExistingProject()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.OpenWorkflowSettings();

			_projectSettingsDialog.ClickWorkflowTab();

			_workflowSetUptab
				.AddTask(WorkflowTask.Proofreading)
				.SaveSettings();

			_projectSettingsHelper.OpenWorkflowSettings();

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskCountMatchExpected(taskCount: 2),
				"Произошла ошибка:\n неверное количество добавленных задач");

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskMatchExpected(
				WorkflowTask.Translation, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskMatchExpected(
				WorkflowTask.Proofreading, taskNumber: 2), "Произошла ошибка:\n задача не соответствует ожидаемой");
		}

		[Test]
		public void CancelAddingTask()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.OpenWorkflowSettings();

			_projectSettingsDialog.ClickWorkflowTab();

			_workflowSetUptab
				.AddTask(WorkflowTask.Proofreading)
				.CancelSettingsChanges();

			_projectSettingsHelper.OpenWorkflowSettings();

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskCountMatchExpected(taskCount: 1),
				"Произошла ошибка:\n неверное количество добавленных задач");

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskMatchExpected(
				WorkflowTask.Translation, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");
		}

		[Test]
		public void ChangeTaskTypeOnProjectCreate()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton();

			_newProjectWorkflowPage
				.ClickNewTaskButton(WorkflowTask.Proofreading)
				.DeleteWorkflowTask(taskNumber: 1);

			Assert.IsTrue(_newProjectWorkflowPage.IsWorkflowAddedTaskCountMatchExpected(expectedTaskCount: 1),
				"Произошла ошибка:\n неверное количество добавленных задач");

			Assert.IsTrue(_newProjectWorkflowPage.IsWorkflowTaskMatchExpected(WorkflowTask.Proofreading, taskNumber: 1),
				"Произошла ошибка:\n добавленная задача не соответствует ожидаемой");
		}

		[Test, Description("S-7187"), ShortCheckList]
		public void DeleteTaskOnExistingProject()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				tasks: new[] { WorkflowTask.Translation, WorkflowTask.Proofreading });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.OpenWorkflowSettings();

			_workflowSetUptab.ClickDeleteTaskButton(taskNumber: 1);

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskCountMatchExpected(taskCount: 1),
				"Произошла ошибка:\n неверное количество добавленных задач");

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskMatchExpected(
				WorkflowTask.Proofreading, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");

			_projectSettingsDialog.SaveSettings();

			_projectSettingsHelper.OpenWorkflowSettings();

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskCountMatchExpected(taskCount: 1),
				"Произошла ошибка:\n неверное количество добавленных задач");

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskMatchExpected(
				WorkflowTask.Proofreading, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");
		}

		[Test]
		public void DeleteAllTasksOnCreate()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton();

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

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.OpenWorkflowSettings();

			_workflowSetUptab
				.ClickDeleteTaskButton()
				.CancelSettingsChanges();

			_projectSettingsHelper.OpenWorkflowSettings();

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskCountMatchExpected(taskCount: 2),
				"Произошла ошибка:\n неверное количество добавленных задач");

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskMatchExpected(
				WorkflowTask.Translation, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskMatchExpected(
				WorkflowTask.Proofreading, taskNumber: 2), "Произошла ошибка:\n задача не соответствует ожидаемой");
		}

		[Test]
		public void ChangeTaskInExistingProject()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.OpenWorkflowSettings();

			_workflowSetUptab.EditTask(WorkflowTask.Proofreading, taskNumber: 1);

			_projectSettingsHelper.OpenWorkflowSettings();

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskCountMatchExpected(taskCount: 1),
				"Произошла ошибка:\n неверное количество добавленных задач");

			Assert.IsTrue(_workflowSetUptab.IsWorkflowTaskMatchExpected(
				WorkflowTask.Proofreading, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");
		}
	}
}