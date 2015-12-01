using System;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	internal class ExportProjectSettingsPageTests<TWebDriverProvider> : ExportProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportDocumentTest(ExportType exportType)
		{
			ExportNotification.CancelAllNotifiers<ProjectSettingsPage>();

			ProjectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
                .ClickDownloadInMainMenuButton()
                .ClickExportType(exportType);

			ExportNotification.ClickDownloadNotifier<ProjectSettingsPage>();

            Assert.IsTrue(ExportNotification.IsFileDownloaded(
                ExportNotification.GetExportFileNameMask(exportType, PathProvider.DocumentFileToConfirm1)),
                "Произошла ошибка: файл не загрузился");
		}

		[TestCase(PlaceSearchNotifier.ProjectsPage)]
		[TestCase(PlaceSearchNotifier.ProjectSettingsPage)]
		[TestCase(PlaceSearchNotifier.GlossariesPage)]
		public void ExportSaveNotifier(PlaceSearchNotifier placeSearch)
		{
            ExportNotification.CancelAllNotifiers<ProjectSettingsPage>();

			ProjectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
                .ClickDownloadInMainMenuButton()
                .ClickExportType(ExportType.Source);

			switch (placeSearch)
			{
				case PlaceSearchNotifier.ProjectsPage:
					WorkspaceHelper.GoToProjectsPage();
					break;

				case PlaceSearchNotifier.ProjectSettingsPage:
					WorkspaceHelper.RefreshPage();
					break;

				case PlaceSearchNotifier.GlossariesPage:
					WorkspaceHelper.GoToGlossariesPage();

                    Assert.AreEqual(WorkspacePage.GetCountExportNotifiers(), 0,
                        "Произошла ошибка:\n остались открытые уведомления");

					WorkspaceHelper.GoToProjectsPage();
					WorkspaceHelper.GoToProjectSettingsPage(ProjectUniqueName);
					break;

				default:
					throw new Exception(string.Format("Передан неверный аргумент:'{0}'", placeSearch));
			}
			
			Assert.IsTrue(ExportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 1),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");
		}

		[Ignore("PRX-11419")]
		[Test]
		public void ExportSaveNotifierAnotherProjectPage()
		{
			ProjectSettingsHelper.GoToProjectsPage();
			var projectUniqueName2 = CreateProjectHelper.GetProjectUniqueName();

			CreateProjectHelper.CreateNewProject(projectUniqueName2, filePath: PathProvider.DocumentFileToConfirm1);

			WorkspaceHelper
				.GoToProjectSettingsPage(projectUniqueName2)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1))
				.UploadDocument(PathProvider.DocumentFileToConfirm2)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.GoToProjectsPage();

            ExportNotification.CancelAllNotifiers<ProjectsPage>();

			WorkspaceHelper.GoToProjectSettingsPage(ProjectUniqueName);

			ProjectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
                .ClickDownloadInMainMenuButton()
                .ClickExportType(ExportType.Source);
				
			WorkspaceHelper.GoToProjectsPage();
            WorkspaceHelper.GoToProjectSettingsPage(projectUniqueName2);

            Assert.IsTrue(ExportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 1),
                "Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");
		}

		[Test]
		public void ExportDocumentFromProjectCheckNotifierText()
		{
            ExportNotification.CancelAllNotifiers<ProjectSettingsPage>();

			ProjectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
                .ClickDownloadInMainMenuButton()
                .ClickExportType(ExportType.Source);

            Assert.IsTrue(ExportNotification.IsNotificationContainsText(Path.GetFileName(PathProvider.DocumentFileToConfirm1)),
				"Произошла ошибка:\n сообщение не содержит искомый текст");
		}

		[Ignore("PRX-11419")]
		[Test]
		public void ExportDocumentsFromProjectCheckNotifierText()
		{
			ProjectSettingsHelper
				.UploadDocument(PathProvider.DocumentFileToConfirm2)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2));

            ExportNotification.CancelAllNotifiers<ProjectSettingsPage>();

			ProjectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm2)
                .ClickDownloadInMainMenuButton()
                .ClickExportType(ExportType.Source);

            Assert.IsTrue(ExportNotification.IsNotificationContainsText("Documents"),
                "Произошла ошибка:\n сообщение не содержит искомый текст");
		}

		[Test]
		public void ExportDocumentCheckNotifierDate()
		{
            ExportNotification.CancelAllNotifiers<ProjectSettingsPage>();

			ProjectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
                .ClickDownloadInMainMenuButton()
                .ClickExportType(ExportType.Source);

			Assert.IsTrue(ExportNotification.IsNotificationContainsCurrentDate(),
				"Произошла ошибка:\n сообщение не содержит требуемую дату.");
		}
	}
}
