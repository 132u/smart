using System;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Standalone]
	internal class ExportProjectSettingsPageTests<TWebDriverProvider> : ExportProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportDocumentTest(ExportType exportType)
		{
			_exportFileHelper.CancelAllNotifiers<ProjectSettingsPage>();

			_projectSettingsHelper
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectSettingsPage>(exportType)
				.ClickDownloadNotifier<ProjectSettingsPage>()
				.AssertFileDownloaded(_exportFileHelper.GetExportFileNameMask(exportType, PathProvider.DocumentFileToConfirm1));
		}

		[TestCase(PlaceSearchNotifier.ProjectsPage)]
		[TestCase(PlaceSearchNotifier.ProjectSettingsPage)]
		[TestCase(PlaceSearchNotifier.GlossariesPage)]
		public void ExportSaveNotifier(PlaceSearchNotifier placeSearch)
		{
			_exportFileHelper.CancelAllNotifiers<ProjectSettingsPage>();

			_projectSettingsHelper
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectSettingsPage>(ExportType.Source);

			switch (placeSearch)
			{
				case PlaceSearchNotifier.ProjectsPage:
					_projectsHelper.GoToProjectsPage();
					break;

				case PlaceSearchNotifier.ProjectSettingsPage:
					_projectsHelper.RefreshPage();
					break;

				case PlaceSearchNotifier.GlossariesPage:
					_projectsHelper.GoToGlossariesPage();
					_exportFileHelper.AssertNotificationNotExist();
					_projectsHelper
						.GoToProjectsPage()
						.GoToProjectSettingsPage(_projectUniqueName);
					break;

				default:
					throw new Exception(string.Format("Передан неверный аргумент:'{0}'", placeSearch));
			}

			_exportFileHelper.AssertCountExportNotifiers(expectedCount: 1);
		}

		[Test]
		public void ExportSaveNotifierAnotherProjectPage()
		{
			_projectSettingsHelper.GoToProjectsPage();
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(projectUniqueName2, PathProvider.DocumentFileToConfirm1)
				.AssertIsProjectLoaded(projectUniqueName2)
				.GoToProjectSettingsPage(projectUniqueName2)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1), NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1))
				.UploadDocument(PathProvider.DocumentFileToConfirm2)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2), NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.GoToProjectsPage();

			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectSettingsPage>(ExportType.Source)
				.GoToProjectsPage()
				.GoToProjectSettingsPage(projectUniqueName2);

			_exportFileHelper.AssertCountExportNotifiers(expectedCount: 1);
		}

		[Test]
		public void ExportDocumentFromProjectCheckNotifierText()
		{
			_exportFileHelper.CancelAllNotifiers<ProjectSettingsPage>();

			_projectSettingsHelper
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectSettingsPage>(ExportType.Source);

			_exportFileHelper.AssertContainsText(Path.GetFileName(PathProvider.DocumentFileToConfirm1));
		}

		[Test]
		public void ExportDocumentsFromProjectCheckNotifierText()
		{
			_projectSettingsHelper
				.UploadDocument(PathProvider.DocumentFileToConfirm2)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2), NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2));

			_exportFileHelper.CancelAllNotifiers<ProjectSettingsPage>();

			_projectSettingsHelper
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm2)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectSettingsPage>(ExportType.Source);

			_exportFileHelper.AssertContainsText("Documents");
		}

		[Test]
		public void ExportDocumentCheckNotifierDate()
		{
			_exportFileHelper.CancelAllNotifiers<ProjectSettingsPage>();

			_projectSettingsHelper
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectSettingsPage>(ExportType.Source);

			_exportFileHelper.AssertContainsCurrentDate();
		}
	}
}
