using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageProjects.ProjectSettingsPage
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageProgectTests<TWebDriverProvider> : ManageProgectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_projectsPage.ClickProject(_projectUniqueName);
		}

		[Test]
		public void AddNewTaskTest()
		{
			_projectSettingsPage.ClickSettingsButton();

			_settingsDialog.ClickWorkflowTab();

			_workflowSetUpTab.AddTask(WorkflowTask.Editing);

			_workflowSetUpTab.SaveSettings();

			Assert.IsTrue(_projectSettingsPage.IsProjectSettingsPageOpened(),
				"Произошла ошибка:\n Не котрылась страница проекта.");
		}
	
		[Test]
		public void ChangeTaskTest()
		{
			_projectSettingsPage.ClickSettingsButton();

			_settingsDialog.ClickWorkflowTab();

			_workflowSetUpTab.EditTask(WorkflowTask.Proofreading, taskNumber: 1);
			
			Assert.IsTrue(_projectSettingsPage.IsProjectSettingsPageOpened(),
				"Произошла ошибка:\n Не котрылась страница проекта.");
		}

		[Test]
		public void DeleteTaskTest()
		{
			_projectSettingsPage.ClickSettingsButton();

			_settingsDialog.ClickWorkflowTab();

			_workflowSetUpTab
				.AddTask(WorkflowTask.Proofreading)
				.SaveSettings();

			_projectSettingsPage.ClickSettingsButton();

			_settingsDialog.ClickWorkflowTab();

			_workflowSetUpTab
				.ClickDeleteTaskButton(taskNumber: 2)
				.SaveSettings();

			Assert.IsTrue(_projectSettingsPage.IsProjectSettingsPageOpened(),
				"Произошла ошибка:\n Не котрылась страница проекта.");
		}

		[Test]
		public void AssignTaskTest()
		{
			_projectSettingsPage.ClickSettingsButton();

			_settingsDialog.ClickWorkflowTab();
			_workflowSetUpTab
				.AddTask(WorkflowTask.Editing)
				.SaveSettings();

			_projectSettingsPage
				.ClickDocumentProgress(PathProvider.DocumentFile)
				.ClickAssignButtonInDocumentInfo();

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, isGroup: false, taskNumber: 1)
				.SetResponsible(AdditionalUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButtonProjectSettingsPage();

			_projectSettingsPage.ClickDocumentProgress(PathProvider.DocumentFile);

			Assert.IsTrue(_projectSettingsPage.IsAssignTaskDisplayedForCurrentUser(TaskMode.Translation),
				"Произошла ошибка:\n Нет задачи перевода.");

			Assert.IsTrue(_projectSettingsPage.IsAssignTaskDisplayedForCurrentUser(TaskMode.Editing),
				"Произошла ошибка:\n Нет задачи редактирования.");
		}

		[Test]
		public void ProjectStatusTest()
		{
			var file = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);

			Assert.AreEqual(DocumentStatus.Created.ToString(), _projectSettingsPage.GetProjectStatus(file),
				"Произошла ошибка:\n Неверный статус документа {0}.", file);

			_projectSettingsPage
				.ClickDocumentProgress(PathProvider.DocumentFile)
				.ClickAssignButtonInDocumentInfo();

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, isGroup: false, taskNumber: 1)
				.ClickSaveButtonProjectSettingsPage();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.DocumentFile);
			_selectTaskDialog.SelectTask();

			_editorPage
				.FillTarget("translation")
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			Assert.AreEqual(DocumentStatus.InProgress.ToString().ToLower(), _projectSettingsPage.GetProjectStatus(file).ToLower(),
				"Произошла ошибка:\n Неверный статус документа {0}.", file);
		}

		[Test]
		public void DeclineAssignTaskTest()
		{
			_projectSettingsPage
				.ClickDocumentProgress(PathProvider.DocumentFile)
				.ClickAssignButtonInDocumentInfo();

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, isGroup: false, taskNumber: 1)
				.ClickSaveButtonProjectSettingsPage();

			_projectSettingsPage
				.ClickDocumentProgress(PathProvider.DocumentFile)
				.ClickDeclineButton();

			_confirmDeclineTaskDialog.ClickDeclineButton();
			_projectSettingsPage.ClickDocumentProgress(PathProvider.DocumentFile);

			Assert.IsFalse(_projectSettingsPage.IsAssignTaskDisplayedForCurrentUser(TaskMode.Translation),
				"Произошла ошибка:\n Нет задачи перевода.");
		}
	}
}
