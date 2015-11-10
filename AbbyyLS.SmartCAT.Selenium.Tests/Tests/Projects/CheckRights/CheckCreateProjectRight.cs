using System;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects.CheckRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class CheckCreateProjectRight<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public override void BeforeTest()
		{
			CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
		}

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_exportFileHelper = new ExportFileHelper(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_loginHelper = new LoginHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_deleteDialog = new DeleteDialog(Driver);

			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			_usersRightsPage = new UsersRightsPage(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);

			AdditionalThreadUser = TakeUser(ConfigurationManager.AdditionalUsers);

			var groupName = Guid.NewGuid().ToString();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspaceHelper
				.CloseTour()
				.GoToUsersRightsPage();

			_usersRightsPage
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(AdditionalThreadUser.NickName)
				.CreateGroupIfNotExist(groupName)
				.OpenAddRightsDialogForGroup(groupName);

			_addAccessRightDialog.AddRightToGroup(RightsType.ProjectCreation);

			_usersRightsPage.AddUserToGroupIfNotAlredyAdded(groupName, AdditionalThreadUser.NickName);

			_workspaceHelper.SignOut();
		}

		[SetUp]
		public void SetUp()
		{
			_loginHelper.Authorize(StartPage.Workspace, AdditionalThreadUser);
			_workspaceHelper.CloseTour();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test]
		public void CheckCreateProject()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectLoaded(_projectUniqueName),
				"Произошла ошибка: не исчезла пиктограмма загрузки проекта");

			Assert.IsFalse(_projectsPage.IsFatalErrorSignDisplayed(_projectUniqueName),
				"Произошла ошибка: появилась пиктограмма ошибки напротив проекта");

			Assert.IsFalse(_projectsPage.IsWarningSignDisplayed(_projectUniqueName),
				"Произошла ошибка: появилась пиктограмма предупреждения напротив проекта");
		}

		[Test]
		public void CheckAddDocumentInProject()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog.UploadDocument(PathProvider.DocumentFileToConfirm1);

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsFileUploaded(PathProvider.DocumentFileToConfirm1),
				"Произошла ошибка:\n не удалось загрузить файл.");

			_documentUploadGeneralInformationDialog.ClickFihishUploadOnProjectsPage();

			Assert.IsTrue(_projectsPage.IsProjectLoaded(_projectUniqueName),
				"Произошла ошибка: не исчезла пиктограмма загрузки проекта");

			Assert.IsFalse(_projectsPage.IsFatalErrorSignDisplayed(_projectUniqueName),
				"Произошла ошибка: появилась пиктограмма ошибки напротив проекта");

			Assert.IsFalse(_projectsPage.IsWarningSignDisplayed(_projectUniqueName),
				"Произошла ошибка: появилась пиктограмма предупреждения напротив проекта");
		}

		[Test]
		public void CheckLinkInProjectNotExist()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			Assert.IsFalse(_projectsPage.IsProjectLinkExist(_projectUniqueName),
				"Произошла ошибка:\n не должно быть ссылки на проект {0}", _projectUniqueName);
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Target)]
		public void CheckExportDocument(ExportType exportType)
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog.UploadDocument(PathProvider.DocumentFileToConfirm1);

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsFileUploaded(PathProvider.DocumentFileToConfirm1),
				"Произошла ошибка:\n не удалось загрузить файл.");

			_documentUploadGeneralInformationDialog.ClickFihishUploadOnProjectsPage();

			_projectsPage
				.WaitUntilProjectLoadSuccessfully(_projectUniqueName)
				.SelectDocument(_projectUniqueName, Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.ClickDownloadInProjectButton(_projectUniqueName);

			_exportFileHelper
				.SelectExportType<ProjectsPage>(exportType)
				.ClickDownloadNotifier<ProjectsPage>()
				.AssertFileDownloaded(_exportFileHelper.GetExportFileNameMask(exportType, PathProvider.EditorTxtFile));
		}

		[TestCase(ExportType.Target, true)]
		[TestCase(ExportType.Target, false)]
		[TestCase(ExportType.Source, true)]
		[TestCase(ExportType.Source, false)]
		public void CheckDownloadProject(ExportType exportType, bool dowloadInProjectClick)
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog.UploadDocument(PathProvider.DocumentFileToConfirm1);

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsFileUploaded(PathProvider.DocumentFileToConfirm1),
				"Произошла ошибка:\n не удалось загрузить файл.");

			_documentUploadGeneralInformationDialog.ClickFihishUploadOnProjectsPage();

			_projectsPage
				.WaitUntilProjectLoadSuccessfully(_projectUniqueName)
				.ClickProjectCheckboxInList(_projectUniqueName);

			if (dowloadInProjectClick)
			{
				_projectsPage.ClickDownloadInProjectButton(_projectUniqueName);
			}
			else
			{
				_projectsPage.ClickDownloadInMainMenuButton();
			}

			_exportFileHelper
				.SelectExportType<ProjectsPage>(exportType)
				.AssertPreparingDownloadMessageDisappeared()
				.ClickDownloadNotifier<ProjectsPage>()
				.AssertFileDownloaded(string.Format("Documents_*{0}.zip", exportType));
		}

		[TestCase(true)]
		[TestCase(false)]
		public void CheckDeleteProject(bool closeProject)
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage.ClickProjectCheckboxInList(_projectUniqueName);

			if (!closeProject)
			{
				_projectsPage.OpenProjectInfo(_projectUniqueName);
			}

			_projectsPage.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			Assert.IsFalse(_projectsPage.IsProjectExist(_projectUniqueName),
				"Произошла ошибка:\n проект {0} найден в списке проектов", _projectUniqueName);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void CheckDeleteDocument(bool allFiles)
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.DocumentFile);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog.UploadDocument(PathProvider.DocumentFile2);

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsFileUploaded(PathProvider.DocumentFile2),
				"Произошла ошибка:\n не удалось загрузить файл.");

			_documentUploadGeneralInformationDialog.ClickFihishUploadOnProjectsPage();

			_projectsPage
				.WaitUntilProjectLoadSuccessfully(_projectUniqueName)
				.SelectDocument(_projectUniqueName, Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));

			if (allFiles)
			{
				_projectsPage
					.SelectDocument(_projectUniqueName, Path.GetFileNameWithoutExtension(PathProvider.DocumentFile2))
					.ClickDeleteButton();
			}
			else
			{
				_projectsPage.ClickDeleteInProjectButton(_projectUniqueName);
			}

			_deleteDialog.ClickConfirmDeleteButton();
		}

		[Test]
		public void CheckConnectorButtonNotExist()
		{
			Assert.IsFalse(_projectsPage.IsSignInToConnectorButtonExist(),
				"Произошла ошибка:\n кнопка 'Sign in to Connector' не должна быть видна.");
		}

		[Test]
		public void QACheckButtonExist()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.DocumentFile);

			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsQACheckButtonDisplayed(_projectUniqueName),
				"Произошла ошибка:\n кнопка 'QA Check' у проекта '{0}' отсутствует", _projectUniqueName);
		}

		[Test]
		public void CheckSettingsFormExist()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.DocumentFile);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);
		}

		[Test]
		public void CheckAnalysisFormExist()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.DocumentFile);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectAnalysisButton(_projectUniqueName);
		}

		protected ExportFileHelper _exportFileHelper;
		protected CreateProjectHelper _createProjectHelper;
		protected WorkspaceHelper _workspaceHelper;
		protected LoginHelper _loginHelper;
		private string _projectUniqueName;

		private DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
		private UsersRightsPage _usersRightsPage;
		private AddAccessRightDialog _addAccessRightDialog;
		private ProjectsPage _projectsPage;
		private DeleteDialog _deleteDialog;
	}
}
