using System;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
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
			_projectHeper = new ProjectsHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_loginHelper = new LoginHelper(Driver);

			AdditionalThreadUser = TakeUser(ConfigurationManager.AdditionalUsers);

			var groupName = Guid.NewGuid().ToString();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspaceHelper
				.CloseTour()
				.GoToUsersRightsPage()
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(AdditionalThreadUser.NickName)
				.CheckOrCreateGroup(groupName)
				.CheckOrAddRightsToGroup(groupName, RightsType.ProjectCreation)
				.CheckOrAddUserToGroup(groupName, AdditionalThreadUser.NickName)
				.SignOut();
		}

		[SetUp]
		public void SetUp()
		{
			_loginHelper.Authorize(StartPage.Workspace, AdditionalThreadUser);
			_workspaceHelper.CloseTour();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();
		}

		[Test]
		public void CheckCreateProject()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName);
		}

		[Test]
		public void CheckAddDocumentInProject()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName, PathProvider.EditorTxtFile)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.OpenProjectInfo(projectUniqueName)
				.ClickDocumentUploadButton()
				.UploadDocument(PathProvider.DocumentFileToConfirm1)
				.AssertFileUploaded(PathProvider.DocumentFileToConfirm1)
				.ClickFihishUploadOnProjectsPage()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName);
		}

		[Test]
		public void CheckLinkInProjectNotExist()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.AssertLinkProjectNotExist(projectUniqueName);
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Target)]
		public void CheckExportDocument(ExportType exportType)
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName, PathProvider.EditorTxtFile)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.OpenProjectInfo(projectUniqueName)
				.ClickDocumentUploadButton()
				.UploadDocument(PathProvider.DocumentFileToConfirm1)
				.AssertFileUploaded(PathProvider.DocumentFileToConfirm1)
				.ClickFihishUploadOnProjectsPage()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName)
				.SelectDocument(projectUniqueName, Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.ClickDownloadInProjectButton(projectUniqueName)
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
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName, PathProvider.EditorTxtFile)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.OpenProjectInfo(projectUniqueName)
				.ClickDocumentUploadButton()
				.UploadDocument(PathProvider.DocumentFileToConfirm1)
				.AssertFileUploaded(PathProvider.DocumentFileToConfirm1)
				.ClickFihishUploadOnProjectsPage()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName)
				.SelectProjectInList(projectUniqueName);

			if (dowloadInProjectClick)
			{
				_projectHeper.ClickDownloadInProjectButton(projectUniqueName);
			}
			else
			{
				_projectHeper.ClickDownloadInMainMenuButton();
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
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName, PathProvider.EditorTxtFile)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.SelectProjectInList(projectUniqueName);

			if (!closeProject)
			{
				_projectHeper
					.OpenProjectInfo(projectUniqueName);
			}

			_projectHeper
				.DeleteFromList()
				.AssertProjectSuccessfullyDeleted(projectUniqueName);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void CheckDeleteDocument(bool allFiles)
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName, PathProvider.DocumentFile)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.OpenProjectInfo(projectUniqueName)
				.ClickDocumentUploadButton()
				.UploadDocument(PathProvider.DocumentFile2)
				.AssertFileUploaded(PathProvider.DocumentFile2)
				.ClickFihishUploadOnProjectsPage()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName)
				.SelectDocument(projectUniqueName, Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));

			if (allFiles)
			{
				_projectHeper
					.SelectDocument(projectUniqueName, Path.GetFileNameWithoutExtension(PathProvider.DocumentFile2))
					.DeleteFromList();
			}
			else
			{
				_projectHeper.DeleteInProjectMenu(projectUniqueName);
			}
		}

		[Test]
		public void CheckConnectorButtonNotExist()
		{
			_projectHeper.AssertSignInToConnectorButtonNotExist();
		}

		[Test]
		public void QACheckButtonExist()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName, PathProvider.DocumentFile)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.OpenProjectInfo(projectUniqueName)
				.AssertQACheckButtonExist(projectUniqueName);
		}

		[Test]
		public void CheckSettingsFormExist()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName, PathProvider.DocumentFile)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.OpenProjectInfo(projectUniqueName)
				.ClickProjectSettingsButton(projectUniqueName);
		}

		[Test]
		public void CheckAnalysisFormExist()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName, PathProvider.DocumentFile)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickFinishOnProjectSetUpWorkflowDialog()
				.AssertIsProjectLoadedSuccessfully(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.OpenProjectInfo(projectUniqueName)
				.ClickProjectAnalysisButton(projectUniqueName);
		}

		protected ExportFileHelper _exportFileHelper;
		protected CreateProjectHelper _createProjectHelper;
		protected ProjectsHelper _projectHeper;
		protected WorkspaceHelper _workspaceHelper;
		protected LoginHelper _loginHelper;
	}
}
