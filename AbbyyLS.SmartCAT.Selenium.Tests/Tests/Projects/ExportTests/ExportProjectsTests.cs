using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class ExportProjectsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpProjectExportTests()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_exportNotification = new ExportNotification(Driver);

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_workspacePage.GoToProjectsPage();

			_exportNotification.CancelAllNotifiers<ProjectsPage>();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { PathProvider.DocumentFileToConfirm1 });

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(PathProvider.DocumentFileToConfirm1, ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1));

			_workspacePage.GoToProjectsPage();
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportDocumentTest(ExportType exportType)
		{
			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<ProjectSettingsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(
				_exportNotification.GetExportFileNameMask(exportType, PathProvider.DocumentFileToConfirm1)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportProjectFromMainMenuTest(ExportType exportType)
		{
			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(
				_exportNotification.GetExportFileNameMask(exportType, PathProvider.DocumentFileToConfirm1)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportProjectFromProjectInfoTest(ExportType exportType)
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDownloadInProjectMenuButton(_projectUniqueName)
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(
				_exportNotification.GetExportFileNameMask(exportType, PathProvider.DocumentFileToConfirm1)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportDocumentFromDocumentInfoTest(ExportType exportType)
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.ClickDownloadInDocumentButton(_projectUniqueName)
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(
				_exportNotification.GetExportFileNameMask(exportType, PathProvider.DocumentFileToConfirm1)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportMultiDocumentsTest(ExportType exportType)
		{
			var document2 = PathProvider.DocumentFile2;
			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsHelper
				.UploadDocument(new[] { document2 })
				.AssignTasksOnDocument(document2, ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(document2));

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(string.Format("Documents_*{0}.zip", exportType)),
				"Произошла ошибка: файл не загрузился");
		}

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private ProjectsPage _projectsPage;
		private WorkspacePage _workspacePage;
		private ProjectSettingsPage _projectSettingsPage;
		private ExportNotification _exportNotification;
	}
}
