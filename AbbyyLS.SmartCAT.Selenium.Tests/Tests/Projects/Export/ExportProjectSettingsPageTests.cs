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
			ExportFileHelper.CancelAllNotifiers<ProjectSettingsPage>();

			ProjectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton();
			ExportFileHelper
				.SelectExportType<ProjectSettingsPage>(exportType)
				.ClickDownloadNotifier<ProjectSettingsPage>()
				.AssertFileDownloaded(ExportFileHelper.GetExportFileNameMask(exportType, PathProvider.DocumentFileToConfirm1));
		}

		[TestCase(PlaceSearchNotifier.ProjectsPage)]
		[TestCase(PlaceSearchNotifier.ProjectSettingsPage)]
		[TestCase(PlaceSearchNotifier.GlossariesPage)]
		public void ExportSaveNotifier(PlaceSearchNotifier placeSearch)
		{
			ExportFileHelper.CancelAllNotifiers<ProjectSettingsPage>();

			ProjectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper.SelectExportType<ProjectSettingsPage>(ExportType.Source);

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
					ExportFileHelper.AssertNotificationNotExist();
					WorkspaceHelper.GoToProjectsPage();
					WorkspaceHelper.GoToProjectSettingsPage(ProjectUniqueName);
					break;

				default:
					throw new Exception(string.Format("Передан неверный аргумент:'{0}'", placeSearch));
			}

			ExportFileHelper.AssertCountExportNotifiers(expectedCount: 1);
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

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			WorkspaceHelper.GoToProjectSettingsPage(ProjectUniqueName);

			ProjectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper
				.SelectExportType<ProjectSettingsPage>(ExportType.Source)
				.GoToProjectsPage();
			WorkspaceHelper
				.GoToProjectSettingsPage(projectUniqueName2);

			ExportFileHelper.AssertCountExportNotifiers(expectedCount: 1);
		}

		[Test]
		public void ExportDocumentFromProjectCheckNotifierText()
		{
			ExportFileHelper.CancelAllNotifiers<ProjectSettingsPage>();

			ProjectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper.SelectExportType<ProjectSettingsPage>(ExportType.Source);

			ExportFileHelper.AssertContainsText(Path.GetFileName(PathProvider.DocumentFileToConfirm1));
		}

		[Ignore("PRX-11419")]
		[Test]
		public void ExportDocumentsFromProjectCheckNotifierText()
		{
			ProjectSettingsHelper
				.UploadDocument(PathProvider.DocumentFileToConfirm2)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2));

			ExportFileHelper.CancelAllNotifiers<ProjectSettingsPage>();

			ProjectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm2)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper.SelectExportType<ProjectSettingsPage>(ExportType.Source);

			ExportFileHelper.AssertContainsText("Documents");
		}

		[Test]
		public void ExportDocumentCheckNotifierDate()
		{
			ExportFileHelper.CancelAllNotifiers<ProjectSettingsPage>();

			ProjectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper.SelectExportType<ProjectSettingsPage>(ExportType.Source);

			ExportFileHelper.AssertContainsCurrentDate();
		}
	}
}
