using System.IO;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
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
			_usersRightsHelper = new UsersRightsHelper(Driver);
			_projectsHelper = new ProjectsHelper(Driver);
			_taskAssignmentDialogHelper = new TaskAssignmentDialogHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_workspaceHelper.GoToProjectsPage();

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_additionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			_documentUploadSetUpTMDialog = new DocumentUploadSetUpTMDialog(Driver);
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
				.ClickDocumentUploadButton();

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
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
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
				.OpenAssignDialog(_projectUniqueName)
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.OpenAssigneeDropbox(2)
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));
		}

		[Test]
		[Standalone]
		public void DeleteUserTaskTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
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
				.AssertIsUserExist(_additionalUser.NickName)
				.GoToProjectsPage();

			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName)
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.SelectTask()
				.CloseTutorialIfExist()
				.CheckStage("Translation (T):")
				.ClickHomeButton()
				.GoToProjectsPage()
				.OpenAssignDialog(_projectUniqueName)
				.ClickCancelAssignButton()
				.ConfirmCancel()
				.OpenAssigneeDropbox()
				.SetResponsible(_additionalUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<EditorPage>(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.AssertStageNameIsEmpty()
				.ClickHomeButton()
				.SignOut()
				.SignIn(_additionalUser.Login, _additionalUser.Password);

			_projectsHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
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
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
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
				.OpenDocument<EditorPage>(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.AssertStageNameIsEmpty();
		}

		[Test]
		[Standalone]
		public void ReassignDocumentToUserTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName)
				.GoToProjectSettingsPage(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog.UploadDocument(PathProvider.DocumentFile);

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsFileUploaded(PathProvider.DocumentFile),
				"Произошла ошибка:\n не удалось загрузить файл.");

			_documentUploadGeneralInformationDialog.ClickFinish<ProjectSettingsPage>();

			_projectSettingsHelper
				.ClickDocumentProgress(PathProvider.DocumentFile)
				.ClickAssignButtonInDocumentInfo()
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.RefreshPage();

			_projectSettingsHelper
				.ClickAssignButtonInDocumentInfo()
				.ClickCancelAssignButton();
		}

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private ProjectsHelper _projectsHelper;
		private UsersRightsHelper _usersRightsHelper;
		private TaskAssignmentDialogHelper _taskAssignmentDialogHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private WorkspaceHelper _workspaceHelper;
		private TestUser _additionalUser;

		private DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
		private DocumentUploadSetUpTMDialog _documentUploadSetUpTMDialog;
	}
}
