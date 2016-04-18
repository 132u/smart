using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ExportDocumentTests<TWebDriverProvider> : FormsAndButtonsAvailabilityBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_projectUniqueNameWithOneFile = _createProjectHelper.GetProjectUniqueName();
			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			_workspacePage.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(
				_projectUniqueNameWithOneFile, filesPaths: new[] { PathProvider.DocumentFile });
			
			_workspacePage.SignOut();
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportProjectFromMainMenuTest(ExportType exportType)
		{
			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueNameWithOneFile)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<Pages.Projects.ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(
				_exportNotification.GetExportFileNameMask(exportType, PathProvider.DocumentFile)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportProjectFromProjectInfoTest(ExportType exportType)
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueNameWithOneFile)
				.ClickDownloadInProjectMenuButton(_projectUniqueNameWithOneFile)
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<Pages.Projects.ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(
				_exportNotification.GetExportFileNameMask(exportType, PathProvider.DocumentFile)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportDocumentFromDocumentInfoTest(ExportType exportType)
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueNameWithOneFile)
				.OpenDocumentInfoForProject(_projectUniqueNameWithOneFile)
				.ClickDownloadInDocumentButton(_projectUniqueNameWithOneFile)
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<Pages.Projects.ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(
				_exportNotification.GetExportFileNameMask(exportType, PathProvider.DocumentFile)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportMultiDocumentsTest(ExportType exportType)
		{
			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<Pages.Projects.ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(string.Format("Documents_*{0}.zip", exportType)),
				"Произошла ошибка: файл не загрузился");
		}

		private string _projectUniqueNameWithOneFile;
	}
}
