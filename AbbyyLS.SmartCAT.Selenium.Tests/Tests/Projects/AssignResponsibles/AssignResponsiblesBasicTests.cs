using System.IO;
using System.Linq;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects.AssignResponsibles;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	public class AssignResponsiblesBasicTests<TWebDriverProvider> : AssignResponsiblesBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpAssignResponsiblesBasicTests()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);
		}

		[Test(Description = "ТС-11")]
		[Standalone]
		public void OpenAssignDialogDocumentInfoOnProjectsPageTest()
		{
			_projectsPage.OpenAssignDialog(_projectUniqueName);

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
			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.ClickDocumentProgress(PathProvider.EditorTxtFile)
				.ClickAssignButtonInDocumentInfo();

			Assert.IsTrue(_taskAssignmentPage.IsTaskAssignmentPageOpened(), "Произошла ошибка:\nНе открылся диалог назначения пользователя.");
		}

		[Test(Description = "ТС-11")]
		[Standalone]
		public void OpenAssignDialogOnProjectSettingsPageTest()
		{
			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.ClickProjectsTableCheckbox(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.ClickAssignButtonOnPanel();

			Assert.IsTrue(_taskAssignmentPage.IsTaskAssignmentPageOpened(), "Произошла ошибка:\nНе открылся диалог назначения пользователя.");
		}
		
		[Test]
		[Standalone]
		public void VerifyUsersAndGroupsListsTest()
		{
			_workspaceHelper.GoToUsersRightsPage();
			var usersList = _usersRightsPage.GetUserNameList();

			var groupsList = _usersRightsPage
				.ClickGroupsButton()
				.GetGroupNameList();

			for (var i = 0; i < groupsList.Count; i++)
			{
				groupsList[i] = "Group: " + groupsList[i];
			}

			_workspaceHelper.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			var responsibleUsersList = _taskAssignmentPage
				.OpenAssigneeDropbox()
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
			var groupName = _usersRightsPage.GetGroupUniqueName();

			_workspaceHelper.GoToUsersRightsPage();

			_usersRightsPage
				.ClickGroupsButton()
				.CreateGroupIfNotExist(groupName);

			_workspaceHelper.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage.OpenAssigneeDropbox();

			Assert.IsTrue(_taskAssignmentPage.IsGroupExist(groupName),
				"Произошла ошибка:\n В выпадающем списке отсутствует группа: {0}", groupName);
		}

		[Test(Description = "ТС-21")]
		[Standalone]
		public void AssignUserOneTaskTest()
		{
			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false)
				.ClickSaveButton();

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");
		}

		[Test]
		[Standalone]
		public void DeleteUserTaskTest()
		{
			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false)
				.ClickSaveButton();

			_workspaceHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings();

			_settingsDialog.ClickDeleteTaskButton();

			Assert.IsTrue(_settingsDialog.IsConfirmDeleteDialogDislpayed(),
				"Произошла ошибка:\n не появился диалог подтверждения удаления задачи");
		}

		[Test(Description = "ТС-271")]
		public void AssignDifferentUsersOneTaskTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);

			_workspaceHelper.GoToUsersRightsPage();

			Assert.IsTrue(_usersRightsPage.IsUserExistInList(_secondUser.NickName),
				"Произошла ошибка:\n пользователь '{0}' не найден в списке.", _secondUser.NickName);

			_workspaceHelper.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, false)
				.ClickSaveButton();

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));
			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");

			_editorPage.ClickHomeButton();

			_workspaceHelper.GoToProjectsPage();
			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.ClickCancelAssignButton()
				.SetResponsible(_secondUser.NickName, false)
				.ClickSaveButton();

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			Assert.IsFalse(_editorPage.IsStageNameIsEmpty(),
				"Произошла ошибка:\n название этапа проставлено.");

			_editorPage.ClickHomeButton();

			_workspaceHelper.SignOut();

			_loginHelper.LogInSmartCat(_secondUser.Login, _secondUser.NickName, _secondUser.Password);

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");
		}

		[Test(Description = "ТС-26")]
		[Standalone]
		public void UnAssignUserTest()
		{
			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, false)
				.ClickSaveButton();
			
			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);
			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));
			
			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");

			_editorPage.ClickHomeButton();
			
			_workspaceHelper
				.GoToProjectsPage()
				.OpenAssignDialog(_projectUniqueName);
			
			_taskAssignmentPage
				.ClickCancelAssignButton()
				.ClickSaveButton();

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));
			Assert.IsFalse(_editorPage.IsStageNameIsEmpty(), "Произошла ошибка:\n название этапа проставлено.");
		}

		[Test(Description = "ТС-25")]
		public void AddNewAssigneeTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, false)
				.SelectAssigneesForEntireDocument()
				.ClickAnotherAssigneeButton()
				.ExpandAssigneeDropdown()
				.SelectAssigneeInDropdown(_secondUser.NickName)
				.ClickAssignButton();

			_workspaceHelper.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			Assert.AreEqual(ThreadUser.NickName+", "+_secondUser.NickName, _taskAssignmentPage.GetAssignneeName(taskNumber: 1),
				"Произошла ошибка:\nНеверные имена в колонке исполнителя.");
		}

		[Test(Description = "ТС-26")]
		public void CancelAssigneeTest()
		{
			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, false)
				.ClickCancelAssignButton();

			Assert.IsTrue(_taskAssignmentPage.IsAssignButtonDisplayed(taskNumber: 1),
				"Произошла ошибка:\nКнопка Assign для задачи не отображается.");
		}

		[Test(Description = "ТС-272")]
		public void ReassigneDocumentInProgressTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false)
				.ClickSaveButton();

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);
			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");

			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation()
				.ClickHomeButton();

			_workspaceHelper.GoToProjectsPage();
			
			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.ClickCancelAssignButton()
				.SetResponsible(_secondUser.NickName, isGroup: false)
				.ClickSaveButton();

			Assert.AreEqual("In Progress", _projectsPage.GetProjectStatus(_projectUniqueName),
				"Произошла ошибка:\nНеверный статус проекта {0}.", _projectUniqueName);
		}

		[Test]
		[Standalone]
		public void ReassignDocumentToUserTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);
			_createProjectHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog.UploadDocument(new[] { PathProvider.DocumentFile });

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsFileUploaded(PathProvider.DocumentFile),
				"Произошла ошибка:\n не удалось загрузить файл.");

			_documentUploadGeneralInformationDialog.ClickFinish<ProjectSettingsPage>();

			_workspaceHelper.RefreshPage();

			_projectSettingsPage
				.WaitUntilDocumentProcessed()
				.ClickDocumentProgress(PathProvider.DocumentFile)
				.ClickAssignButtonInDocumentInfo();

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, false)
				.ClickSaveAssignButtonProjectSettingPage();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));

			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");

			_editorPage.ClickHomeButton();

			_projectSettingsPage
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile))
				.ClickAssignButtonInDocumentInfo();

			_taskAssignmentPage
				.ClickCancelAssignButton()
				.SetResponsible(_secondUser.NickName, false)
				.ClickSaveAssignButtonProjectSettingPage();

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
		}
	}
}
