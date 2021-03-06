﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class ManageProjectTests<TWebDriverProvider> : ManageProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void SearchProjectTest()
		{
			Assert.IsTrue(_projectsPage.IsProjectExist(_projectUniqueName),
				"Произошла ошибка:\n Не удалось найти проект.");
		}

		[Test]
		public void AddWorkflowTaskTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			Assert.IsTrue(_settingsDialog.IsSettingsDialogOpened(),
				"Произошла ошибка:\n Не открылся диалог настроек проекта.");

			_settingsDialog.ClickWorkflowTab();

			_workflowSetUpTab
				.AddTask(WorkflowTask.Proofreading)
				.SaveSettingsExpectingProjectsPage();

			Assert.IsFalse(_settingsDialog.IsSettingsDialogOpened(),
				"Произошла ошибка:\n Не закрылся диалог настроек проекта.");
		}
		
		[Test]
		public void DeleteWorkflowTaskTest()
		{
			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.OpenWorkflowSettings();

			_settingsDialog.ClickWorkflowTab();

			_workflowSetUpTab
				.AddTask(WorkflowTask.Proofreading)
				.SaveSettings();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			Assert.IsTrue(_settingsDialog.IsSettingsDialogOpened(),
				"Произошла ошибка:\n Не открылся диалог настроек проекта.");

			_settingsDialog.ClickWorkflowTab();

			_workflowSetUpTab
				.ClickDeleteTaskButton()
				.SaveSettingsExpectingProjectsPage();

			Assert.IsTrue(_projectsPage.IsProjectsPageOpened(),
				"Произошла ошибка:\n Не закрылся диалог настроек проекта.");
		}

		[Test]
		public void AssignUserOneTaskTest()
		{
			_projectsPage.OpenAssignDialog(_projectUniqueName, _document2);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, _document2);

			_selectTaskDialog.SelectTask();
			
			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");
		}

		[Test]
		public void ProjectStatusTest()
		{
			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.AreEqual(ProjectStatus.Created.ToString(), _projectsPage.GetProjectStatus(_projectUniqueName),
				"Произошла ошибка:\n Неверный статус проекта {0}.", _projectUniqueName);

			_projectsPage.ClickDocumentRefExpectingEditorPage(_projectUniqueName, _document1);
			
			_editorPage
				.FillTarget("translation")
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToProjectsPage();

			Assert.AreEqual(ProjectStatus.InProgress.Description(), _projectsPage.GetProjectStatus(_projectUniqueName),
				"Произошла ошибка:\n Неверный статус проекта {0}.", _projectUniqueName);
		}

		[Test]
		public void DeclineAssignTaskTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.SelectDocument(_projectUniqueName, _document2)
				.OpenAssignDialog(_projectUniqueName, _document2);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, _document2)
				.ClickDocumentRow(_projectUniqueName, _document2);

			Assert.IsTrue(_projectsPage.IsMyTaskDisplayed(_document2),
				"Произошла ошибка:\n Задача перевода не отображается для текущего пользователя.");

			_projectsPage.ClickDeclineButton();

			_confirmDeclineTaskDialog.ClickDeclineButtonExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, _document2);

			Assert.IsFalse(_projectsPage.IsMyTaskDisplayed(_document2),
				"Произошла ошибка:\n Задача перевода отображается для текущего пользователя.");
		}
	}
}
