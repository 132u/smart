﻿using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Projects]
	class WorkflowRightsTests<TWebDriverProvider> : WorkflowBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void WorkflowRightsTestsSetUp()
		{
			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_groupName = _groupsAndAccessRightsTab.GetGroupUniqueName();

			_workspacePage.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(AdditionalUser.FullName);

			if (!_groupsAndAccessRightsTab.IsGroupExists(_groupName))
			{
				_groupsAndAccessRightsTab.OpenNewGroupDialog();

				_newGroupDialog.CreateNewGroup(_groupName);
			}

			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(_groupName, AdditionalUser.FullName);

			_workspacePage.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { _editorTxtFile });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(_editorTxtFileName, AdditionalUser.NickName, _projectUniqueName);
		}

		[Test, Description("ТС-56 Пользователь назначен  на один этап одного документа")]
		public void OneTaskTest()
		{
			_projectSettingsPage
				.ClickSettingsButton()
				.ClickWorkflowTab();

			_workflowSetUptab
				.AddTask(WorkflowTask.Editing)
				.SaveSettings();

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);
			
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, _editorTxtFile)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, _editorTxtFile);

			Assert.IsTrue(_editorPage.IsEditorPageOpened(), "Произошла ошибка:\n Редактор не открылся.");
		}

		[Test, Description("ТС-561 Назначить пользователя на несколько этапов одного документа")]
		public void TwoTaskTest()
		{
			_projectSettingsPage
				.ClickSettingsButton()
				.ClickWorkflowTab();

			_workflowSetUptab
				.AddTask(WorkflowTask.Editing)
				.SaveSettings();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, _editorTxtFile)
				.OpenAssignDialog(_projectUniqueName, _editorTxtFile);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton();

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, _editorTxtFile)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, _editorTxtFile);

			Assert.AreEqual(2, _selectTaskDialog.GetTaskCount(),
				"Произошла ошибка:\n Неверное количество задач в диалоге выбора при входе в редактор.");
		}

		[Test, Description("ТС-562 Назначить пользователя на несколько этапов одного документа 2 тест")]
		public void TwoTaskCancelOneTaskTest()
		{
			_projectSettingsPage
				.ClickSettingsButton()
				.ClickWorkflowTab();

			_workflowSetUptab
				.AddTask(WorkflowTask.Editing)
				.SaveSettings();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, _editorTxtFile)
				.OpenAssignDialog(_projectUniqueName, _editorTxtFile);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton();

			_workspacePage
				.GoToProjectsPage()
				.OpenAssignDialog(_projectUniqueName, _editorTxtFile);

			_taskAssignmentPage
				.ClickCancelAssignButton(AdditionalUser.NickName)
				.ClickSaveButton();

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);
			
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, _editorTxtFile)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, _editorTxtFile);

			Assert.IsTrue(_editorPage.IsEditorPageOpened(),
				"Произошла ошибка:\n Редактор не открылся.");
		}

		[Test]
		[Description("ТС-054 Отмена участия исполнителя  с этапа, который в процессе работы")]
		public void DeclineAssigneeInProgressTest()
		{
			var text2 = "Translation 2";

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, _editorTxtFile)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, _editorTxtFile);

			_editorPage
				.FillTarget(_text, rowNumber: 1)
				.ConfirmSegmentTranslation()
				.FillTarget(text2, rowNumber: 2)
				.ConfirmSegmentTranslation();

			_editorPage.ClickHomeButtonExpectingProjectsPage();

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(ThreadUser.Login, ThreadUser.NickName, ThreadUser.Password);

			_projectsPage.OpenAssignDialog(_projectUniqueName, _editorTxtFile);

			_taskAssignmentPage
				.ClickCancelAssignButton(AdditionalUser.NickName)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, _editorTxtFile)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, _editorTxtFile);

			Assert.AreEqual(_text, _editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка:\n Неверное значение в таргете сегмента №1.");

			Assert.AreEqual(text2, _editorPage.GetTargetText(rowNumber: 2),
				"Произошла ошибка:\n Неверное значение в таргете сегмента №2.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			Assert.IsTrue(_projectsPage.IsProjectNotExistInList(_projectUniqueName),
				"Произошла ошибка:\nПроект {0} присутствует в списке.", _projectUniqueName);
		}

		private string _groupName;
	}
}
