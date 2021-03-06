﻿using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class ManageProjectProjectSettingsTests<TWebDriverProvider> : ManageProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);
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

			_projectSettingsPage.ClickAssignButtonInDocumentInfo(_document1);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, isGroup: false, taskNumber: 1)
				.SetResponsible(AdditionalUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton();

			_workspacePage.ClickProjectLink(_projectUniqueName);

			_projectSettingsPage
				.HoverDocumentRow(Path.GetFileNameWithoutExtension(_document1))
				.ClickDocumentRow(Path.GetFileNameWithoutExtension(_document1));

			Assert.IsTrue(_projectSettingsPage.IsAssignTaskDisplayedForCurrentUser(TaskMode.Translation),
				"Произошла ошибка:\n Нет задачи перевода.");

			Assert.IsTrue(_projectSettingsPage.IsAssignTaskDisplayedForCurrentUser(TaskMode.Editing),
				"Произошла ошибка:\n Нет задачи редактирования.");
		}

		[Test]
		public void DocumentStatusTest()
		{
			var fileName = Path.GetFileNameWithoutExtension(_document1);

			Assert.AreEqual(DocumentStatus.Created.ToString(), _projectSettingsPage.GetDocumentStatus(fileName),
				"Произошла ошибка:\n Неверный статус документа {0}.", fileName);

			_projectSettingsPage.ClickAssignButtonInDocumentInfo(_document1);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, isGroup: false, taskNumber: 1)
				.ClickSaveButton();

			_workspacePage.ClickProjectLink(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(_document1);

			_selectTaskDialog.SelectTask();

			_editorPage
				.FillTarget("translation")
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			Assert.AreEqual(DocumentStatus.InProgress.Description(), _projectSettingsPage.GetDocumentStatus(fileName),
				"Произошла ошибка:\n Неверный статус документа {0}.", fileName);
		}

		[Test]
		public void DeclineAssignTaskTest()
		{
			_projectSettingsPage.ClickAssignButtonInDocumentInfo(_document1);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, isGroup: false, taskNumber: 1)
				.ClickSaveButton();

			_workspacePage.ClickProjectLink(_projectUniqueName);

			_projectSettingsPage
				.HoverDocumentRow(Path.GetFileNameWithoutExtension(_document1))
				.ClickDocumentRow(Path.GetFileNameWithoutExtension(_document1))
				.ClickDeclineButton();

			_confirmDeclineTaskDialog.ClickDeclineButton();

			_projectSettingsPage
				.HoverDocumentRow(Path.GetFileNameWithoutExtension(_document1))
				.ClickDocumentRow(Path.GetFileNameWithoutExtension(_document1));

			Assert.IsFalse(_projectSettingsPage.IsAssignTaskDisplayedForCurrentUser(TaskMode.Translation),
				"Произошла ошибка:\n Нет задачи перевода.");
		}
	}
}
