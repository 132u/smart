using System;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects.CheckRights
{
	class CheckCreateProjectRight<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			var groupName = Guid.NewGuid().ToString();
			WorkspaceHelper
				.CloseTour()
				.GoToUsersRightsPage()
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(ConfigurationManager.RightsTestNickName)
				.CheckOrCreateGroup(groupName)
				.CheckOrAddRightsToGroup(groupName, RightsType.ProjectCreation)
				.CheckOrAddUserToGroup(groupName, ConfigurationManager.RightsTestNickName)
				.SignOut()
				.SignIn(ConfigurationManager.RightsTestLogin, ConfigurationManager.RightsTestPassword)
				.SelectAccount()
				.CloseTour();

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

		protected readonly ExportFileHelper _exportFileHelper = new ExportFileHelper();
		protected readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
		protected readonly ProjectsHelper _projectHeper = new ProjectsHelper();
		protected readonly WorkspaceHelper WorkspaceHelper = new WorkspaceHelper();
	}
}
