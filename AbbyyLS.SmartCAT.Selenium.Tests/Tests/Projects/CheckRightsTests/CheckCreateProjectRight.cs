﻿using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
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
			_createProjectHelper = new CreateProjectHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_deleteDialog = new DeleteDialog(Driver);
			_exportNotification = new ExportNotification(Driver);
			_newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog(Driver);
			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			_usersTab = new UsersTab(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			var groupName = Guid.NewGuid().ToString();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(AdditionalUser.NickName)
				.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(groupName);

			_addAccessRightDialog
				.AddRightToGroupAnyProject(RightsType.ProjectCreation)
				.ClickSaveButton(groupName);

			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(groupName, AdditionalUser.NickName);

			_workspacePage.SignOut();
		}

		[SetUp]
		public void SetUp()
		{
			_loginHelper.Authorize(StartPage.Workspace, AdditionalUser);

			_exportNotification.CancelAllNotifiers<ProjectsPage>();

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

			_documentUploadGeneralInformationDialog.UploadDocument(new []{PathProvider.DocumentFileToConfirm1});

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

			_documentUploadGeneralInformationDialog.UploadDocument(new []{PathProvider.DocumentFileToConfirm1});

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsFileUploaded(PathProvider.DocumentFileToConfirm1),
				"Произошла ошибка:\n не удалось загрузить файл.");

			_documentUploadGeneralInformationDialog.ClickFihishUploadOnProjectsPage();

			_projectsPage
				.WaitUntilProjectLoadSuccessfully(_projectUniqueName)
				.SelectDocument(_projectUniqueName, PathProvider.EditorTxtFile)
				.ClickDownloadInProjectButton(_projectUniqueName)
				.ClickExportType(exportType);

			_exportNotification.ClickDownloadNotifier<ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(
				_exportNotification.GetExportFileNameMask(exportType, PathProvider.EditorTxtFile)),
				"Произошла ошибка: файл не загрузился");
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

			_documentUploadGeneralInformationDialog.UploadDocument(new []{PathProvider.DocumentFileToConfirm1});

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

			_projectsPage.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(string.Format("Documents_*{0}.zip", exportType)),
				"Произошла ошибка: файл не загрузился");
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

			_documentUploadGeneralInformationDialog.UploadDocument(new []{PathProvider.DocumentFile2});

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsFileUploaded(PathProvider.DocumentFile2),
				"Произошла ошибка:\n не удалось загрузить файл.");

			_documentUploadGeneralInformationDialog.ClickFihishUploadOnProjectsPage();

			_projectsPage
				.WaitUntilProjectLoadSuccessfully(_projectUniqueName)
				.OpenProjectInfo(_projectUniqueName)
				.SelectDocument(_projectUniqueName, PathProvider.DocumentFile);

			if (allFiles)
			{
				_projectsPage
					.SelectDocument(_projectUniqueName, PathProvider.DocumentFile2)
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

		protected CreateProjectHelper _createProjectHelper;
		protected WorkspacePage _workspacePage;
		protected LoginHelper _loginHelper;
		private string _projectUniqueName;

		private DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
		private NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog;
		private UsersTab _usersTab;
		private AddAccessRightDialog _addAccessRightDialog;
		private ProjectsPage _projectsPage;
		private DeleteDialog _deleteDialog;
		private ExportNotification _exportNotification;
		protected NewGroupDialog _newGroupDialog;
		private GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
	}
}