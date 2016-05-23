using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Projects]
	public class AssignResponsiblesBasicTests<TWebDriverProvider> : AssignResponsiblesBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpAssignResponsiblesBasicTests()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new []{ PathProvider.EditorTxtFile });
		}

		[Test(Description = "ТС-11")]
		[Standalone]
		public void OpenAssignDialogDocumentInfoOnProjectsPageTest()
		{
			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			Assert.IsTrue(_taskAssignmentPage.IsTaskAssignmentPageOpened(), "Произошла ошибка:\nНе открылся диалог назначения пользователя.");
		}
		
		[Test(Description = "ТС-11")]
		[Standalone]
		public void OpenAssignDialogProjectInfoOnProjectsPageTest()
		{
			_projectsPage.OpenAssignDialogForSelectedDocuments(_projectUniqueName, new[] { PathProvider.EditorTxtFile });

			Assert.IsTrue(_taskAssignmentPage.IsTaskAssignmentPageOpened(), "Произошла ошибка:\nНе открылся диалог назначения пользователя.");
		}

		[Test(Description = "ТС-11")]
		[Standalone]
		public void OpenAssignDialogDocumentInfoOnProjectSettingsPageTest()
		{
			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.ClickDocumentRow(PathProvider.EditorTxtFile)
				.ClickAssignButtonInDocumentInfo();

			Assert.IsTrue(_taskAssignmentPage.IsTaskAssignmentPageOpened(), "Произошла ошибка:\nНе открылся диалог назначения пользователя.");
		}

		[Test(Description = "ТС-11")]
		[Standalone]
		public void OpenAssignDialogOnProjectSettingsPageTest()
		{
			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.ClickProjectsTableCheckbox(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.ClickAssignButtonOnPanel();

			Assert.IsTrue(_taskAssignmentPage.IsTaskAssignmentPageOpened(), "Произошла ошибка:\nНе открылся диалог назначения пользователя.");
		}
		
		[Test]
		[Standalone]
		public void VerifyUsersAndGroupsListsTest()
		{
			_workspacePage.GoToUsersPage();
			var usersList = _userTab.GetUserNameList();

			var groupsList = _groupsAndAccessRightsTab
				.ClickGroupsButton()
				.GetGroupNameList();

			for (var i = 0; i < groupsList.Count; i++)
			{
				groupsList[i] = "Group: " + groupsList[i];
			}

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			var responsibleUsersList = _taskAssignmentPage
				.ClickAssigneeDropboxButton()
				.GetResponsibleUsersList();

			var responsibleGroupList = _taskAssignmentPage.GetResponsibleGroupsList();

			Assert.IsFalse(
				responsibleGroupList.Except(groupsList).Any(),
				"Ошибка: Ожидаемый и представленный списки групп не совпадают.");

			Assert.IsFalse(
				responsibleUsersList.Except(usersList).Any(),
				"Ошибка: Ожидаемый и представленный списки пользователей не совпадают.");
		}

		[Test]
		[Standalone]
		public void AddNewGroupTest()
		{
			var groupName = _groupsAndAccessRightsTab.GetGroupUniqueName();

			_workspacePage.GoToUsersPage();

			_groupsAndAccessRightsTab
				.ClickGroupsButton()
				.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(groupName);

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_taskAssignmentPage.ClickAssigneeDropboxButton();

			Assert.IsTrue(_taskAssignmentPage.IsGroupExist(groupName),
				"Произошла ошибка:\n В выпадающем списке отсутствует группа: {0}", groupName);
		}

		[Test(Description = "ТС-21")]
		[Standalone]
		public void AssignUserOneTaskTest()
		{
			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.ClickSaveButton()
				.ClickBackToProjectButton();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);

			_selectTaskDialog.SelectTask();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");
		}

		[Test]
		[Standalone]
		public void DeleteUserTaskTest()
		{
			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.ClickSaveButton()
				.ClickBackToProjectButton();

			_projectSettingsHelper.OpenWorkflowSettings();

			_workflowSetUptab.ClickDeleteTaskButton();

			Assert.IsTrue(_settingsDialog.IsConfirmDeleteDialogDislpayed(),
				"Произошла ошибка:\n не появился диалог подтверждения удаления задачи");
		}

		[Test(Description = "ТС-271")]
		public void AssignDifferentUsersOneTaskTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);

			_workspacePage.GoToUsersPage();

			Assert.IsTrue(_userTab.IsUserExistInList(_secondUser.NickName),
				"Произошла ошибка:\n пользователь '{0}' не найден в списке.", _secondUser.NickName);

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.ClickSaveButton()
				.ClickBackToProjectButton();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);
			_selectTaskDialog.SelectTask();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_taskAssignmentPage
				.ClickCancelAssignButton(ThreadUser.NickName)
				.SetResponsible(_secondUser.NickName)
				.ClickSaveButton()
				.ClickBackToProjectButton();

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(PathProvider.EditorTxtFile);

			Assert.IsFalse(_editorPage.IsStageNameIsEmpty(),
				"Произошла ошибка:\n название этапа проставлено.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(_secondUser.Login, _secondUser.NickName, _secondUser.Password);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);

			_selectTaskDialog.SelectTask();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");
		}

		[Test(Description = "ТС-26")]
		[Standalone]
		public void UnAssignUserTest()
		{
			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.ClickSaveButton()
				.ClickBackToProjectButton();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);
			
			_selectTaskDialog.SelectTask();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage
				.GoToProjectsPage()
				.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);
			
			_taskAssignmentPage
				.ClickCancelAssignButton(ThreadUser.NickName)
				.ClickSaveButton()
				.ClickBackToProjectButton();

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(PathProvider.EditorTxtFile);
			Assert.IsFalse(_editorPage.IsStageNameIsEmpty(), "Произошла ошибка:\n название этапа проставлено.");
		}

		[Test(Description = "ТС-25")]
		public void AddNewAssigneeTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.SetResponsible(_secondUser.NickName)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			var assignees = new List<string> { ThreadUser.NickName, _secondUser.NickName };
			assignees.Sort();

			Assert.AreEqual(assignees, _taskAssignmentPage.GetAssignneeName(taskNumber: 1),
				"Произошла ошибка:\nНеверные имена в колонке исполнителя.");
		}

		[Test(Description = "ТС-272")]
		public void ReassigneDocumentInProgressTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.ClickSaveButton()
				.ClickBackToProjectButton();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);

			_selectTaskDialog.SelectTask();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");

			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_taskAssignmentPage
				.ClickCancelAssignButton(ThreadUser.NickName)
				.SetResponsible(_secondUser.NickName)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			Assert.AreEqual("In Progress", _projectsPage.GetProjectStatus(_projectUniqueName),
				"Произошла ошибка:\nНеверный статус проекта {0}.", _projectUniqueName);
		}

		[Test]
		[Standalone]
		public void ReassignDocumentToUserTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.UploadDocument(new[] { PathProvider.DocumentFile });

			_workspacePage.RefreshPage<WorkspacePage>();

			_projectSettingsPage
				.WaitUntilDocumentProcessed()
				.ClickDocumentRow(PathProvider.DocumentFile)
				.ClickAssignButtonInDocumentInfo();

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.ClickSaveButton()
				.ClickBackToProjectButton();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.DocumentFile);

			_selectTaskDialog.SelectTask();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage
				.ClickDocumentRow(PathProvider.DocumentFile)
				.ClickAssignButtonInDocumentInfo();

			_taskAssignmentPage
				.ClickCancelAssignButton(ThreadUser.NickName)
				.SetResponsible(_secondUser.NickName)
				.ClickSaveButton()
				.ClickBackToProjectButton();

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(PathProvider.DocumentFile);
		}
	}
}
