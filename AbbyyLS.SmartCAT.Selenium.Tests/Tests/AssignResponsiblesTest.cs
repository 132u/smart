using System.Linq;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Standalone]
	class AssignResponsiblesTest<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void Setup()
		{
			WorkspaceHelper.GoToProjectsPage();
			_createProjectHelper = new CreateProjectHelper();
			_usersRightsHelper = new UsersRightsHelper();
			_projectsHelper = new ProjectsHelper();
			_responsiblesDialogHelper = new ResponsiblesDialogHelper();
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test]
		public void ResponsiblesWorkspaceOnAssignTaskButtonTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenTaskResponsibles()
				.AssertTaskResponsiblesListDisplay();
		}

		[Test]
		public void AssignDialogInWorkspaceVisibleTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName);
		}

		[Test]
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
		public void ResponsiblesProjectOnProgressLinkTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.GoToProjectSettingsPage(_projectUniqueName)
				.ClickDocumentProgress(PathProvider.EditorTxtFile)
				.ClickAssignButtonInDocumentInfo();
		}

		[Test]
		public void ResponsiblesProjectOnAssignButtonTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.GoToProjectSettingsPage(_projectUniqueName)
				.ClickDocumentProgress(PathProvider.EditorTxtFile)
				.ClickAssignButtonInDocumentInfo();
		}

		[Test]
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
				.OpenTaskResponsibles()
				.GetResponsibleUsersList();

			var responsibleGroupList = _responsiblesDialogHelper.GetResponsibleGroupsList();

			Assert.IsFalse(
				responsibleGroupList.Except(groupsList).Any(),
				"Ошибка: Ожидаемый и представленный списки групп не совпадают.");

			Assert.IsFalse(
				responsibleUsersList.Except(usersList).Any(),
				"Ошибка: Ожидаемый и представленный списки пользователей не совпадают.");
		}

		[Test]
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
				.OpenTaskResponsibles()
				.AssertGroupExist(groupName);
		}

		[Test]
		public void AssignUserOneTaskTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenTaskResponsibles()
				.SetResponsible(UserName, false)
				.ClickCloseAssignDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<SelectTaskDialog>(PathProvider.EditorTxtFile)
				.SelectTask()
				.CloseTutorialIfExist()
				.CheckStage("Translation (T):");
		}

		[Test]
		public void AssignUserFewTasksTest()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(_projectUniqueName, PathProvider.EditorTxtFile)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNewTaskButton()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.OpenAssignDialog(_projectUniqueName)
				.OpenTaskResponsibles()
				.SetResponsible(UserName, false)
				.OpenTaskResponsibles(2)
				.SetResponsible(UserName, false)
				.ClickCloseAssignDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<SelectTaskDialog>(PathProvider.EditorTxtFile);
		}

		[Test]
		public void DeleteUserTaskTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenTaskResponsibles()
				.SetResponsible(UserName, false)
				.ClickCloseAssignDialog<ProjectsPage>()
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
				.AssertIsUserExist(UserName2)
				.GoToProjectsPage();

			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenTaskResponsibles()
				.SetResponsible(UserName, false)
				.ClickCloseAssignDialog<ProjectsPage>()
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
				.OpenTaskResponsibles()
				.SetResponsible(UserName2, false)
				.ClickCloseAssignDialog<ProjectsPage>()
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
		public void UnAssignUserTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenTaskResponsibles()
				.SetResponsible(UserName, false)
				.ClickCloseAssignDialog<ProjectsPage>()
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
				.ClickCloseAssignDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<EditorPage>(PathProvider.EditorTxtFile)
				.AssertStageNameIsEmpty();
		}

		[Test]
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
				.OpenTaskResponsibles()
				.SetResponsible(UserName, false)
				.ClickCloseAssignDialog<ProjectSettingsPage>()
				.RefreshPage()
				.AcceptAllTasks(PathProvider.DocumentFile)
				.ClickAssignButtonInDocumentInfo()
				.ClickCancelAssignButton()
				.ConfirmCancel()
				.AssertAssignStatus("Not assigned")
				.OpenTaskResponsibles()
				.SetResponsible(UserName, false)
				.AssertCancelAssignButtonExist()
				.ClickCloseAssignDialog<ProjectSettingsPage>();
		}

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private ProjectsHelper _projectsHelper;
		private UsersRightsHelper _usersRightsHelper;
		private ResponsiblesDialogHelper _responsiblesDialogHelper;
	}
}
