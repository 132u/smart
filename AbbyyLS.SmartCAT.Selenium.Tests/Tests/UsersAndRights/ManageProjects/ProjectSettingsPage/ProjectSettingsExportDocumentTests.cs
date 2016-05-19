using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class ProjectSettingsExportDocumentTests<TWebDriverProvider> : FormsAndButtonsAvailabilityBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_workspacePage.GoToProjectsPage();
			_projectsPage.ClickProject(_projectUniqueName);

			if (!_exportNotification.IsExportNotificationDisappeared())
			{
				_exportNotification.ClickDownloadNotifier<Pages.Projects.ProjectSettings.ProjectSettingsPage>();
			}
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Target)]
		[TestCase(ExportType.Tmx)]
		public void DocumentDownloadButtoninDocumentPanelTest(ExportType exportType)
		{
			_projectSettingsPage
				.ClickDocumentRow(PathProvider.DocumentFile)
				.ClickDocumnetDownloadButton()
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 1),
				"Произошла ошибка:\n Не появилось ожидаемое кол-во уведомлений");

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<Pages.Projects.ProjectSettings.ProjectSettingsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(
				_exportNotification.GetExportFileNameMask(exportType, PathProvider.DocumentFile)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Target)]
		[TestCase(ExportType.Tmx)]
		public void DocumentDownloadButtonTest(ExportType exportType)
		{
			_projectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFile)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 1),
				"Произошла ошибка:\n Не появилось ожидаемое кол-во уведомлений");


			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<Pages.Projects.ProjectSettings.ProjectSettingsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(
				_exportNotification.GetExportFileNameMask(exportType, PathProvider.DocumentFile)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Target)]
		[TestCase(ExportType.Tmx)]
		public void MultiDocumentDownloadButtonTest(ExportType exportType)
		{
			_projectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFile)
				.ClickDocumentCheckbox(PathProvider.DocumentFile2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 1),
				"Произошла ошибка:\n Не появилось ожидаемое кол-во уведомлений");


			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(timeout: 90),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<Pages.Projects.ProjectSettings.ProjectSettingsPage>();
		}
	}
}
