using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class ExportDocumentTests<TWebDriverProvider> : FormsAndButtonsAvailabilityBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_projectUniqueNameWithOneFile = _createProjectHelper.GetProjectUniqueName();
			_document = PathProvider.DocumentFile;

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			_workspacePage.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(
				_projectUniqueNameWithOneFile, filesPaths: new[] { _document });
			
			_workspacePage.SignOut();
		}

		[TestCase(ExportType.Original)]
		[TestCase(ExportType.TMX)]
		[TestCase(ExportType.Translation)]
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
				_exportNotification.GetExportFileNameMask(exportType, _document)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Original)]
		[TestCase(ExportType.TMX)]
		[TestCase(ExportType.Translation)]
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
				_exportNotification.GetExportFileNameMask(exportType, _document)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Original)]
		[TestCase(ExportType.TMX)]
		[TestCase(ExportType.Translation)]
		public void ExportDocumentFromDocumentInfoTest(ExportType exportType)
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueNameWithOneFile)
				.HoverDocumentRow(_projectUniqueNameWithOneFile, _document)
				.ClickDownloadInDocumentButton(_document)
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<Pages.Projects.ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(
				_exportNotification.GetExportFileNameMask(exportType, _document)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Original)]
		[TestCase(ExportType.TMX)]
		[TestCase(ExportType.Translation)]
		public void ExportMultiDocumentsTest(ExportType exportType)
		{
			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<Pages.Projects.ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(string.Format("Documents_*{0}.zip", exportType.Description())),
				"Произошла ошибка: файл не загрузился");
		}

		private string _projectUniqueNameWithOneFile;
		private string _document;
	}
}
