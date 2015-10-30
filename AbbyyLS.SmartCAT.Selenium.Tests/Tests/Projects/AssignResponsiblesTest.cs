using System.IO;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
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
			_projectsHelper = new ProjectsHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_workspaceHelper.GoToProjectsPage();

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_additionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_signInPage = new SignInPage(Driver);
			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			_documentUploadSetUpTMDialog = new DocumentUploadSetUpTMDialog(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_usersRightsPage = new UsersRightsPage(Driver);
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
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage.OpenAssigneeDropbox();

			Assert.IsTrue(_taskAssignmentPage.IsTaskAssigneeListDisplayed(),
				"Произошла ошибка:\n список исполнителей не открылся");
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
			_projectsHelper.GoToUsersRightsPage();
			var usersList = _usersRightsPage.GetUserNameList();

			var groupsList = _usersRightsPage
				.ClickGroupsButton()
				.GetGroupNameList();

			for (var i = 0; i < groupsList.Count; i++)
			{
				groupsList[i] = "Group: " + groupsList[i];
			}

			_projectsHelper.GoToProjectsPage();

			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName);

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

			_projectsHelper.GoToUsersRightsPage();

			_usersRightsPage
				.ClickGroupsButton()
				.CreateGroupIfNotExist(groupName);

			_workspaceHelper.GoToProjectsPage();

			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage.OpenAssigneeDropbox();

			Assert.IsTrue(_taskAssignmentPage.IsGroupExist(groupName),
				"Произошла ошибка:\n В выпадающем списке отсутствует группа: {0}", groupName);
		}

		[Test]
		[Standalone]
		public void AssignUserOneTaskTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper
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
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.OpenAssigneeDropbox(2)
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));
		}

		[Test]
		[Standalone]
		public void DeleteUserTaskTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenWorkflowSettings()
				.ClickDeleteTaskButton()
				.AssertConfirmDeleteDialogDisplay();
		}

		[Test]
		public void AssignDifferentUsersOneTaskTest()
		{
			_projectsHelper.GoToUsersRightsPage();

			Assert.IsTrue(_usersRightsPage.IsUserExistInList(_additionalUser.NickName),
				"Произошла ошибка:\n пользователь '{0}' не найден в списке.", _additionalUser.NickName);

			_workspaceHelper.GoToProjectsPage();

			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.EditorTxtFile)
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.SelectTask()
				.CloseTutorialIfExist()
				.CheckStage("Translation (T):")
				.ClickHomeButton()
				.GoToProjectsPage()
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.ClickCancelAssignButton()
				.ClickConfirmCancelButton()
				.OpenAssigneeDropbox()
				.SetResponsible(_additionalUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<EditorPage>(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.AssertStageNameIsEmpty()
				.ClickHomeButton()
				.SignOut();

			_signInPage
				.SubmitForm(_additionalUser.Login, _additionalUser.Password);

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
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.SelectTask()
				.CloseTutorialIfExist()
				.CheckStage("Translation (T):")
				.ClickHomeButton()
				.GoToProjectsPage()
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.ClickCancelAssignButton()
				.ClickConfirmCancelButton()
				.CloseTaskAssignmentDialog<ProjectsPage>();

			_workspaceHelper
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
				.ClickAssignButtonInDocumentInfo();

			_taskAssignmentPage
				.OpenAssigneeDropbox()
				.SetResponsible(ThreadUser.NickName, false)
				.CloseTaskAssignmentDialog<ProjectSettingsPage>();

			_projectSettingsHelper
				.ClickSaveButton()
				.RefreshPage();

			_projectSettingsHelper.ClickAssignButtonInDocumentInfo();

			_taskAssignmentPage.ClickCancelAssignButton();
		}

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private ProjectsHelper _projectsHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private WorkspaceHelper _workspaceHelper;
		private TestUser _additionalUser;

		private DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
		private DocumentUploadSetUpTMDialog _documentUploadSetUpTMDialog;
		private SignInPage _signInPage;
		private TaskAssignmentPage _taskAssignmentPage;
		private UsersRightsPage _usersRightsPage;
	}
}
