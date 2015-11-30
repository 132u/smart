﻿using System.IO;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Ignore("Требуют актуализации в связи с большим изменением функциональности")]
	class AssignResponsiblesTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void Setup()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_workspaceHelper.GoToProjectsPage();

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_additionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_signInPage = new SignInPage(Driver);
			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			_documentUploadSetUpTMDialog = new DocumentUploadSetUpTMDialog(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_usersRightsPage = new UsersRightsPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_newProjectSetUpWorkflowDialog = new NewProjectSetUpWorkflowDialog(Driver);
			_newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog(Driver);
			_settingsDialog = new SettingsDialog(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
		}

		[TearDown]
		public void TearDown()
		{
			ReturnUser(ConfigurationManager.AdditionalUsers, _additionalUser);
		}

		[Test]
		[Standalone]
		public void ResponsiblesWorkspaceOnAssignTaskButtonTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage.OpenAssigneeDropbox();

			Assert.IsTrue(_taskAssignmentPage.IsTaskAssigneeListDisplayed(),
				"Произошла ошибка:\n список исполнителей не открылся");
		}

		[Test]
		[Standalone]
		public void AssignDialogInWorkspaceVisibleTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage.OpenAssignDialog(_projectUniqueName);
		}

		[Test]
		[Standalone]
		public void WorkflowStepVisibleForAddedDocumentTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog.UploadDocument(PathProvider.DocumentFileToConfirm1);

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsFileUploaded(PathProvider.DocumentFileToConfirm1),
				"Произошла ошибка:\n не удалось загрузить файл.");

			_documentUploadGeneralInformationDialog.ClickNext<DocumentUploadSetUpTMDialog>();

			_documentUploadSetUpTMDialog.ClickNext<DocumentUploadTaskAssignmentDialog>();
		}

		[Test]
		[Standalone]
		public void ResponsiblesProjectOnProgressLinkTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.GoToProjectSettingsPage(_projectUniqueName);
			_projectSettingsPage
				.ClickDocumentProgress(PathProvider.EditorTxtFile)
				.ClickAssignButtonInDocumentInfo();
		}

		[Test]
		[Standalone]
		public void ResponsiblesProjectOnAssignButtonTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.GoToProjectSettingsPage(_projectUniqueName);
			_projectSettingsPage
				.ClickDocumentProgress(PathProvider.EditorTxtFile)
				.ClickAssignButtonInDocumentInfo();
		}

		[Test]
		[Standalone]
		public void ResponsiblesProjectUploadDocumentTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog.UploadDocument(PathProvider.DocumentFileToConfirm1);

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsFileUploaded(PathProvider.DocumentFileToConfirm1),
				"Произошла ошибка:\n не удалось загрузить файл.");

			_documentUploadGeneralInformationDialog.ClickNext<DocumentUploadSetUpTMDialog>();

			_documentUploadSetUpTMDialog.ClickNext<DocumentUploadTaskAssignmentDialog>();
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

			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

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

			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage.OpenAssigneeDropbox();

			Assert.IsTrue(_taskAssignmentPage.IsGroupExist(groupName),
				"Произошла ошибка:\n В выпадающем списке отсутствует группа: {0}", groupName);
		}

		[Test]
		[Standalone]
		public void AssignUserOneTaskTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");
		}

		[Test]
		[Standalone]
		public void AssignUserFewTasksTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName, PathProvider.EditorTxtFile)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.ClickNewTaskButton()
				.ClickFinishButton();

			_projectsPage
				.WaitUntilProjectLoadSuccessfully(_projectUniqueName)
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.OpenAssigneeDropbox(2)
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));
		}

		[Test]
		[Standalone]
		public void DeleteUserTaskTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings();

			_settingsDialog.ClickDeleteTaskButton();

			Assert.IsTrue(_settingsDialog.IsConfirmDeleteDialogDislpayed(),
				"Произошла ошибка:\n не появился диалог подтверждения удаления задачи");
		}

		[Test]
		public void AssignDifferentUsersOneTaskTest()
		{
			_workspaceHelper.GoToUsersRightsPage();

			Assert.IsTrue(_usersRightsPage.IsUserExistInList(_additionalUser.NickName),
				"Произошла ошибка:\n пользователь '{0}' не найден в списке.", _additionalUser.NickName);

			_workspaceHelper.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

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
				.ClickConfirmCancelButton()
				.OpenAssigneeDropbox()
				.SetResponsible(_additionalUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper
				.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));
			Assert.IsFalse(_editorPage.IsStageNameIsEmpty(),
				"Произошла ошибка:\n название этапа проставлено.");

			_editorPage.ClickHomeButton();

			_workspaceHelper.SignOut();

			_signInPage.SubmitForm(_additionalUser.Login, _additionalUser.Password);

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();

			Assert.AreEqual("Translation (T):", _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");
		}

		[Test]
		[Standalone]
		public void UnAssignUserTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

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
				.ClickConfirmCancelButton()
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			Assert.IsFalse(_editorPage.IsStageNameIsEmpty(),
				"Произошла ошибка:\n название этапа проставлено.");
		}

		[Test]
		[Standalone]
		public void ReassignDocumentToUserTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName)
				.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog.UploadDocument(PathProvider.DocumentFile);

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsFileUploaded(PathProvider.DocumentFile),
				"Произошла ошибка:\n не удалось загрузить файл.");

			_documentUploadGeneralInformationDialog.ClickFinish<ProjectSettingsPage>();

			_projectSettingsPage
				.ClickDocumentProgress(PathProvider.DocumentFile)
				.ClickAssignButtonInDocumentInfo();

			_taskAssignmentPage
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectSettingsPage>();

			_settingsDialog
				.SaveSettings()
				.RefreshPage<SettingsDialog>();

			_projectSettingsPage.ClickAssignButtonInDocumentInfo();

			_taskAssignmentPage.ClickCancelAssignButton();
		}

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private WorkspaceHelper _workspaceHelper;
		private TestUser _additionalUser;

		private DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
		private DocumentUploadSetUpTMDialog _documentUploadSetUpTMDialog;
		private SettingsDialog _settingsDialog;
		private SignInPage _signInPage;
		private TaskAssignmentPage _taskAssignmentPage;
		private UsersRightsPage _usersRightsPage;
		private ProjectsPage _projectsPage;
		private NewProjectSetUpWorkflowDialog _newProjectSetUpWorkflowDialog;
		private NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog;
		private ProjectSettingsPage _projectSettingsPage;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
	}
}
