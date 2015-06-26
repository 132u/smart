using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Ignore("Требуют актуализации в связи с большим изменением функциональности")]
	class AssignResponsiblesTest<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void Setup()
		{
			WorkspaceHelper.GoToProjectsPage();
			_createProjectHelper = new CreateProjectHelper();
			_usersRightsHelper = new UsersRightsHelper();
			_projectsHelper = new ProjectsHelper();
			_taskAssignmentDialogHelper = new TaskAssignmentDialogHelper();
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test]
		[Standalone]
		public void ResponsiblesWorkspaceOnAssignTaskButtonTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenAssigneeDropbox()
				.AssertTaskAssigneeListDisplay();
		}

		[Test]
		[Standalone]
		public void AssignDialogInWorkspaceVisibleTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName);
		}

		[Test]
		[Standalone]
		public void WorkflowStepVisibleForAddedDocumentTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton()
				.UploadDocument(PathProvider.DocumentFileToConfirm1)
				.ClickNextOnGeneralInformationPage()
				.ClickNextOnSetUpTMPage();
		}

		[Test]
		[Standalone]
		public void ResponsiblesProjectOnProgressLinkTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.GoToProjectSettingsPage(_projectUniqueName)
				.ClickDocumentProgress(PathProvider.EditorTxtFile)
				.ClickAssignButtonInDocumentInfo();
		}

		[Test]
		[Standalone]
		public void ResponsiblesProjectOnAssignButtonTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.GoToProjectSettingsPage(_projectUniqueName)
				.ClickDocumentProgress(PathProvider.EditorTxtFile)
				.ClickAssignButtonInDocumentInfo();
		}

		[Test]
		[Standalone]
		public void ResponsiblesProjectUploadDocumentTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.GoToProjectSettingsPage(_projectUniqueName)
				.ClickDocumentUploadButton()
				.UploadDocument(PathProvider.DocumentFileToConfirm1)
				.ClickNextOnGeneralInformationPage()
				.ClickNextOnSetUpTMPage();
		}

		[Test]
		[Standalone]
		public void VerifyUsersAndGroupsListsTest()
		{
			var usersList = _projectsHelper
				.GoToUsersRightsPage()
				.GetUserNameList();

			var groupsList = _usersRightsHelper
				.ClickGroupsButton()
				.GetGroupNameList();

			for (var i = 0; i < groupsList.Count; i++)
			{
				groupsList[i] = "Group: " + groupsList[i];
			}

			_projectsHelper.GoToProjectsPage();

			var responsibleUsersList = _createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenAssigneeDropbox()
				.GetResponsibleUsersList();

			var responsibleGroupList = _taskAssignmentDialogHelper.GetResponsibleGroupsList();

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
			var groupName = _usersRightsHelper.GetGroupUniqueName();

			_projectsHelper
				.GoToUsersRightsPage()
				.ClickGroupsButton()
				.CheckOrCreateGroup(groupName)
				.GoToProjectsPage();

			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenAssigneeDropbox()
				.AssertGroupExist(groupName);
		}

		[Test]
		[Standalone]
		public void AssignUserOneTaskTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenAssigneeDropbox()
				.SetResponsible(NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<SelectTaskDialog>(PathProvider.EditorTxtFile)
				.SelectTask()
				.CloseTutorialIfExist()
				.CheckStage("Translation (T):");
		}

		[Test]
		[Standalone]
		public void AssignUserFewTasksTest()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName, PathProvider.EditorTxtFile)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.WaitCreateProjectDialogDisappear()
				.OpenAssignDialog(_projectUniqueName)
				.OpenAssigneeDropbox()
				.SetResponsible(NickName, false)
				.OpenAssigneeDropbox(2)
				.SetResponsible(NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<SelectTaskDialog>(PathProvider.EditorTxtFile);
		}

		[Test]
		[Standalone]
		public void DeleteUserTaskTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenAssigneeDropbox()
				.SetResponsible(NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.ClickDeleteTaskButton()
				.AssertConfirmDeleteDialogDisplay();
		}

		[Test]
		public void AssignDifferentUsersOneTaskTest()
		{
			_projectsHelper
				.GoToUsersRightsPage()
				.AssertIsUserExist(NickName2)
				.GoToProjectsPage();

			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenAssigneeDropbox()
				.SetResponsible(NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.AcceptAllTasks(PathProvider.EditorTxtFile)
				.OpenDocument<SelectTaskDialog>(PathProvider.EditorTxtFile)
				.SelectTask()
				.CloseTutorialIfExist()
				.CheckStage("Translation (T):")
				.ClickHomeButton()
				.GoToProjectsPage()
				.OpenAssignDialog(_projectUniqueName)
				.ClickCancelAssignButton()
				.ConfirmCancel()
				.OpenAssigneeDropbox()
				.SetResponsible(NickName2, false)
				.CloseTaskAssignmentDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<EditorPage>(PathProvider.EditorTxtFile)
				.AssertStageNameIsEmpty()
				.ClickHomeButton()
				.LogOff()
				.SignIn(Login2, Password2);

			_projectsHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.AcceptAllTasks(PathProvider.EditorTxtFile)
				.OpenDocument<SelectTaskDialog>(PathProvider.EditorTxtFile)
				.SelectTask()
				.CloseTutorialIfExist()
				.CheckStage("Translation (T):");
		}

		[Test]
		[Standalone]
		public void UnAssignUserTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenAssigneeDropbox()
				.SetResponsible(NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.AcceptAllTasks(PathProvider.EditorTxtFile)
				.OpenDocument<SelectTaskDialog>(PathProvider.EditorTxtFile)
				.SelectTask()
				.CloseTutorialIfExist()
				.CheckStage("Translation (T):")
				.ClickHomeButton()
				.GoToProjectsPage()
				.OpenAssignDialog(_projectUniqueName)
				.ClickCancelAssignButton()
				.ConfirmCancel()
				.CloseTaskAssignmentDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<EditorPage>(PathProvider.EditorTxtFile)
				.AssertStageNameIsEmpty();
		}

		[Test]
		[Standalone]
		public void ReassignDocumentToUserTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName)
				.GoToProjectSettingsPage(_projectUniqueName)
				.ClickDocumentUploadButton()
				.UploadDocument(PathProvider.DocumentFile)
				.ClickFihishUploadOnProjectSettingsPage()
				.ClickDocumentProgress(PathProvider.DocumentFile)
				.ClickAssignButtonInDocumentInfo()
				.OpenAssigneeDropbox()
				.SetResponsible(NickName, false)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.RefreshPage()
				.AcceptAllTasks(PathProvider.DocumentFile)
				.ClickAssignButtonInDocumentInfo()
				.ClickCancelAssignButton();
		}

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private ProjectsHelper _projectsHelper;
		private UsersRightsHelper _usersRightsHelper;
		private TaskAssignmentDialogHelper _taskAssignmentDialogHelper;
	}
}
