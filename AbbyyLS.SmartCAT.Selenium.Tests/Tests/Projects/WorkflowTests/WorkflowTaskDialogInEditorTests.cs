﻿using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects.WorkflowTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Projects]
	class WorkflowTaskDialogInEditorTests<TWebDriverProvider> : WorkflowBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public new void SetUp()
		{
			_groupName = Guid.NewGuid().ToString();
			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);
			_createProjectHelper = new CreateProjectHelper(Driver);

			_tasksList = new List<WorkflowTask>
			{
				WorkflowTask.Translation,
				WorkflowTask.Editing,
				WorkflowTask.Proofreading
			};

			_workspacePage.ClickUsersRightsButton();

			_usersTab.ClickGroupsButton();

			_groupsAndAccessRightsTab
				.RemoveUserFromAllGroups(AdditionalUser.FullName)
				.ClickCreateGroupButton()
				.SetNewGroupName(_groupName)
				.ClickSaveNewGroupButton()
				.AddUserToGroupIfNotAlredyAdded(_groupName, AdditionalUser.FullName);
		}

		[Test, Description("S-13767"), ShortCheckList]
		public void WorkflowTaskDialogForUserWithProjectManagementRightsTest()
		{
			_groupsAndAccessRightsTab
				.ClickEditGroupButton(_groupName)
				.ClickAddRightsButton(_groupName);

			_addAccessRightDialog
				.ClickRightRadio(RightsType.ProjectResourceManagement)
				.ClickNextButton()
				.ClickForAnyProjectRadio()
				.ClickAddRightButton();

			_groupsAndAccessRightsTab
				.ClickSaveButton(_groupName)
				.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				new []{_editorTxtFile},
				tasks: _tasksList);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickAssignButtonOnPanel();

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, taskNumber: 1)
				.SetResponsible(AdditionalUser.NickName, taskNumber: 2)
				.SetResponsible(AdditionalUser.NickName, taskNumber: 3)
				.ClickSaveButton()
				.SignOut();

			_loginHelper.Authorize(StartPage.Workspace, AdditionalUser);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, _editorTxtFile);

			var tasksListFromDialog = _selectTaskDialog.GetTaskList();

			Assert.AreEqual(tasksListFromDialog[0], WorkflowTask.Translation.ToString(),
				"Произошла ошибка:\n Этап {0} не соответствует заданному во время создания проекта ",
				WorkflowTask.Translation.Description());

			Assert.AreEqual(tasksListFromDialog[1], WorkflowTask.Editing.ToString(),
				"Произошла ошибка:\n Этап {0} не соответствует заданному во время создания проекта ",
				WorkflowTask.Editing.Description());

			Assert.AreEqual(tasksListFromDialog[2], WorkflowTask.Proofreading.ToString(),
				"Произошла ошибка:\n Этап {0} не соответствует заданному во время создания проекта ",
				WorkflowTask.Proofreading.Description());

			Assert.IsTrue(_selectTaskDialog.IsManagerButtonDisplayed(),
				"Произошла ошибка:\n Не отобразилась кнопка входа для менеджера.");
		}

		[Test, Description("S-7206"), ShortCheckList]
		public void WorkflowTaskDialogForUserWithoutProjectManagementRightsTest()
		{
			_groupsAndAccessRightsTab.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				new[] {_editorTxtFile},
				tasks: _tasksList);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickAssignButtonOnPanel();

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, taskNumber: 1)
				.SetResponsible(AdditionalUser.NickName, taskNumber: 2)
				.SetResponsible(AdditionalUser.NickName, taskNumber: 3)
				.ClickSaveButton()
				.SignOut();

			_loginHelper.Authorize(StartPage.Workspace, AdditionalUser);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, _editorTxtFile);

			var tasksListFromDialog = _selectTaskDialog.GetTaskList();

			Assert.AreEqual(tasksListFromDialog[0], WorkflowTask.Translation.ToString(),
				"Произошла ошибка:\n Этап {0} не соответствует заданному во время создания проекта ",
				WorkflowTask.Translation.Description());

			Assert.AreEqual(tasksListFromDialog[1], WorkflowTask.Editing.ToString(),
				"Произошла ошибка:\n Этап {0} не соответствует заданному во время создания проекта ",
				WorkflowTask.Editing.Description());

			Assert.AreEqual(tasksListFromDialog[2], WorkflowTask.Proofreading.ToString(),
				"Произошла ошибка:\n Этап {0} не соответствует заданному во время создания проекта ",
				WorkflowTask.Proofreading.Description());

			Assert.IsFalse(_selectTaskDialog.IsManagerButtonDisplayed(),
				"Произошла ошибка:\n Отобразилась кнопка входа для исполнителя как менеджера.");
		}

		[Test, Description("S-13766"), ShortCheckList]
		public void WorkflowDeleteStageTest()
		{
			_groupsAndAccessRightsTab.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				new[] { _editorTxtFile },
				tasks: _tasksList);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickAssignButtonOnPanel();

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, taskNumber: 1)
				.SetResponsible(AdditionalUser.NickName, taskNumber: 2)
				.SetResponsible(AdditionalUser.NickName, taskNumber: 3)
				.ClickSaveButton()
				.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, _editorTxtFile);

			_editorPage
				.FillTarget("первое предложение.")
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.ClickSettingsButton();

			_projectSettingsDialog.ClickWorkflowTab();

			_workflowSetUptab.ClickDeleteTaskButton(taskNumber: 1);

			Assert.IsTrue(_informationFromWfSetupTabDialog.IsInformationFromWFSetupTabDialogOpened(),
				"Произошла ошибка:\n Не открылся информационный диалог при попытке удаления этапа WF.");
		}

		private List<WorkflowTask> _tasksList;
		private string _groupName;
	}
}