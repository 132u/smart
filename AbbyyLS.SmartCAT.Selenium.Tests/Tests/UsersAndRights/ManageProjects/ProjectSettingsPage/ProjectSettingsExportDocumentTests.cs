using System.IO;

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
			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			if (!_exportNotification.IsExportNotificationDisappeared())
			{
				_exportNotification.ClickDownloadNotifier<Pages.Projects.ProjectSettings.ProjectSettingsPage>();
			}
		}

		[Ignore("PRX-17075")]
		[TestCase(ExportType.Original)]
		[TestCase(ExportType.Translation)]
		[TestCase(ExportType.TMX)]
		public void DocumentDownloadButtonInDocumentPanelTest(ExportType exportType)
		{
			var documetName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);

			_projectSettingsPage
				.HoverDocumentRow(documetName)
				.ClickDocumnetDownloadButton(documetName)
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

		[TestCase(ExportType.Original)]
		[TestCase(ExportType.Translation)]
		[TestCase(ExportType.TMX)]
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

		[TestCase(ExportType.Original)]
		[TestCase(ExportType.Translation)]
		[TestCase(ExportType.TMX)]
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
