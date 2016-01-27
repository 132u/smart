using System;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	internal class ExportProjectListTests<TWebDriverProvider> : ExportProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportProjectFromMainMenuTest(ExportType exportType)
		{
			WorkspaceHelper.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();
			
			ProjectsPage
				.ClickProjectCheckboxInList(ProjectUniqueName)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper
				.SelectExportType<ProjectsPage>(exportType)
				.AssertPreparingDownloadMessageDisappeared()
				.ClickDownloadNotifier<ProjectsPage>()
				.AssertFileDownloaded(ExportFileHelper.GetExportFileNameMask(exportType, PathProvider.DocumentFileToConfirm1));
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportProjectFromProjectInfoTest(ExportType exportType)
		{
			WorkspaceHelper.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.OpenProjectInfo(ProjectUniqueName)
				.ClickDownloadInProjectButton(ProjectUniqueName);

			ExportFileHelper
				.SelectExportType<ProjectsPage>(exportType)
				.ClickDownloadNotifier<ProjectsPage>()
				.AssertFileDownloaded(ExportFileHelper.GetExportFileNameMask(exportType, PathProvider.DocumentFileToConfirm1));
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportDocumentFromDocumentInfoTest(ExportType exportType)
		{
			WorkspaceHelper.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.OpenProjectInfo(ProjectUniqueName)
				.OpenDocumentInfoForProject(ProjectUniqueName)
				.ClickDownloadInDocumentButton(ProjectUniqueName);

			ExportFileHelper
				.SelectExportType<ProjectsPage>(exportType)
				.AssertPreparingDownloadMessageDisappeared()
				.ClickDownloadNotifier<ProjectsPage>()
				.AssertFileDownloaded(ExportFileHelper.GetExportFileNameMask(exportType, PathProvider.DocumentFileToConfirm1));
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportMultiDocumentsTest(ExportType exportType)
		{
			ProjectSettingsHelper
				.UploadDocument(new []{PathProvider.DocumentFileToConfirm2})
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.ClickProjectCheckboxInList(ProjectUniqueName)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper
				.SelectExportType<ProjectsPage>(exportType)
				.AssertPreparingDownloadMessageDisappeared()
				.ClickDownloadNotifier<ProjectsPage>()
				.AssertFileDownloaded(string.Format("Documents_*{0}.zip", exportType));
		}

		[Test]
		public void ExportCloseNotifier()
		{
			WorkspaceHelper.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.OpenProjectInfo(ProjectUniqueName)
				.ClickDownloadInProjectButton(ProjectUniqueName);

			ExportFileHelper
				.SelectExportType<ProjectsPage>(ExportType.Source)
				.ClickCancelInDowloadNotifier<ProjectsPage>()
				.AssertNotificationNotExist();
		}

		[TestCase(PlaceSearchNotifier.ProjectsPage)]
		[TestCase(PlaceSearchNotifier.ProjectSettingsPage)]
		[TestCase(PlaceSearchNotifier.GlossariesPage)]
		public void ExportSaveNotifier(PlaceSearchNotifier placeSearch)
		{
			WorkspaceHelper.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.OpenProjectInfo(ProjectUniqueName)
				.ClickDownloadInProjectButton(ProjectUniqueName);

			ExportFileHelper.SelectExportType<ProjectsPage>(ExportType.Source);

			switch (placeSearch)
			{
				case PlaceSearchNotifier.ProjectsPage:
					ProjectsPage.RefreshPage<ProjectsPage>();
					break;

				case PlaceSearchNotifier.ProjectSettingsPage:
					WorkspaceHelper.GoToProjectSettingsPage(ProjectUniqueName);
					break;

				case PlaceSearchNotifier.GlossariesPage:
					WorkspaceHelper.GoToGlossariesPage();
					ExportFileHelper.AssertNotificationNotExist();
					WorkspaceHelper.GoToProjectsPage();
					break;

				default:
					throw new Exception(string.Format("Передан неверный аргумент:'{0}'", placeSearch));
			}

			ExportFileHelper.AssertCountExportNotifiers(expectedCount: 1);
		}

		[Test]
		public void CheckNotifierText()
		{
			WorkspaceHelper.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.OpenProjectInfo(ProjectUniqueName)
				.OpenDocumentInfoForProject(ProjectUniqueName)
				.ClickDownloadInDocumentButton(ProjectUniqueName);

			ExportFileHelper.SelectExportType<ProjectsPage>(ExportType.Source);

			ExportFileHelper.AssertContainsText(Path.GetFileName(PathProvider.DocumentFileToConfirm1));
		}

		[Test]
		public void ExportProjectOneDocCheckNotifierText()
		{
			WorkspaceHelper.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.ClickProjectCheckboxInList(ProjectUniqueName)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper.SelectExportType<ProjectsPage>(ExportType.Source);

			ExportFileHelper.AssertContainsText(Path.GetFileName(PathProvider.DocumentFileToConfirm1));
		}

		[Test]
		public void ExportProjectMultiDocCheckNotifierText()
		{
			ProjectSettingsHelper
				.UploadDocument(new []{PathProvider.DocumentFileToConfirm2})
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.ClickProjectCheckboxInList(ProjectUniqueName)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper.SelectExportType<ProjectsPage>(ExportType.Source);

			ExportFileHelper.AssertContainsText("Documents");
		}

		[Test]
		public void ExportProjectsCheckNotifierText()
		{
			var projectUniqueName2 = CreateProjectHelper.GetProjectUniqueName();

			WorkspaceHelper.GoToProjectsPage();

			CreateProjectHelper.CreateNewProject(projectUniqueName2, filePath: PathProvider.DocumentFileToConfirm1);

			WorkspaceHelper
				.GoToProjectSettingsPage(projectUniqueName2)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1))
				.UploadDocument(new []{PathProvider.DocumentFileToConfirm2})
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.ClickProjectCheckboxInList(ProjectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper.SelectExportType<ProjectsPage>(ExportType.Source);

			ExportFileHelper.AssertContainsText("Documents");
		}

		[Test]
		public void ExportChangeNotifiers()
		{
			var projectUniqueName2 = CreateProjectHelper.GetProjectUniqueName();

			WorkspaceHelper.GoToProjectsPage();

			CreateProjectHelper.CreateNewProject(projectUniqueName2, filePath: PathProvider.DocumentFileToConfirm1);

			WorkspaceHelper
				.GoToProjectSettingsPage(projectUniqueName2)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1))
				.UploadDocument(new []{PathProvider.DocumentFileToConfirm2})
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.ClickProjectCheckboxInList(ProjectUniqueName)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper.SelectExportType<ProjectsPage>(ExportType.Source);

			ExportFileHelper.AssertCountExportNotifiers(expectedCount: 1);

			ProjectsPage
				.ClickProjectCheckboxInList(ProjectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper.SelectExportType<ProjectsPage>(ExportType.Source);

			var message1 = ExportFileHelper
				.AssertCountExportNotifiers(expectedCount: 2)
				.GetTextNotificationByNumber(1);

			var message2 = ExportFileHelper.GetTextNotificationByNumber(1);

			Assert.AreNotEqual(message1, message2,
				"Произошла ошибка:\n тесты сообщений обоих уведомлений совпадают.");
		}

		[Test]
		public void ExportLimitNotifiers()
		{
			WorkspaceHelper.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage.ClickProjectCheckboxInList(ProjectUniqueName);

			for (int i = 0; i < _maxNotifierNumber; i++)
			{
				ProjectsPage.ClickDownloadInMainMenuButton();
				ExportFileHelper.SelectExportType<ProjectsPage>(ExportType.Source);
			}

			ExportFileHelper.AssertCountExportNotifiers(_maxNotifierNumber);
		}

		[Test]
		public void ExportMoreLimitNotifiers()
		{
			WorkspaceHelper.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage.ClickProjectCheckboxInList(ProjectUniqueName);

			for (int i = 0; i <= _maxNotifierNumber; i++)
			{
				ProjectsPage.ClickDownloadInMainMenuButton();
				ExportFileHelper.SelectExportType<ProjectsPage>(ExportType.Source);
			}

			var notificationCount = ExportFileHelper.GetCountExportNotifiers();

			Assert.IsFalse(notificationCount > _maxNotifierNumber,
				"Произошла ошибка:\n сообщений об экспорте показано {0}, ожидалось не больше {1}.",
				notificationCount, _maxNotifierNumber);
		}

		[Test]
		public void ExportCheckNotifiersFreshAbove()
		{
			var projectUniqueName2 = CreateProjectHelper.GetProjectUniqueName();

			WorkspaceHelper.GoToProjectsPage();

			CreateProjectHelper.CreateNewProject(projectUniqueName2, filePath: PathProvider.DocumentFileToConfirm1);

			WorkspaceHelper
				.GoToProjectSettingsPage(projectUniqueName2)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1))
				.UploadDocument(new []{PathProvider.DocumentFileToConfirm2})
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.ClickProjectCheckboxInList(ProjectUniqueName)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper.SelectExportType<ProjectsPage>(ExportType.Source);

			var message1 = ExportFileHelper.GetTextNotificationByNumber(1);

			ProjectsPage
				.ClickProjectCheckboxInList(ProjectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper
				.SelectExportType<ProjectsPage>(ExportType.Source);

			var message2 = ExportFileHelper.GetTextNotificationByNumber(2);

			Assert.AreNotEqual(message1, message2,
				"Произошла ошибка:\n сообщение не изменилось, свежее сообщение должно быть сверху");
		}

		[Test]
		public void ExportChangeFirstToThird()
		{
			var projectUniqueName2 = CreateProjectHelper.GetProjectUniqueName();

			WorkspaceHelper.GoToProjectsPage();

			CreateProjectHelper.CreateNewProject(projectUniqueName2, filePath: PathProvider.DocumentFileToConfirm1);

			WorkspaceHelper
				.GoToProjectSettingsPage(projectUniqueName2)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1))
				.UploadDocument(new []{PathProvider.DocumentFileToConfirm2})
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.ClickProjectCheckboxInList(ProjectUniqueName)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper.SelectExportType<ProjectsPage>(ExportType.Source);

			ProjectsPage
				.ClickProjectCheckboxInList(ProjectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper
				.SelectExportType<ProjectsPage>(ExportType.Source)
				.AssertCountExportNotifiers(expectedCount:2);

			ProjectsPage
				.ClickProjectCheckboxInList(projectUniqueName2)
				.OpenProjectInfo(projectUniqueName2)
				.OpenDocumentInfoForProject(projectUniqueName2, documentNumber: 2)
				.ClickDownloadInDocumentButton(projectUniqueName2, documentNumber: 2);

			ExportFileHelper
				.SelectExportType<ProjectsPage>(ExportType.Source)
				.AssertCountExportNotifiers(expectedCount: 3);

			var message = ExportFileHelper.GetTextNotificationByNumber(1);

			Assert.IsTrue(message.Contains(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1)),
				"Произошла ошибка:\n кликнули по верхнему сообщению - появилось не первое!");
		}

		[Test]
		public void ExportChangeFirstToSecond()
		{
			var secondNotifierDocName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1);
			var projectUniqueName2 = CreateProjectHelper.GetProjectUniqueName();

			WorkspaceHelper.GoToProjectsPage();

			CreateProjectHelper.CreateNewProject(projectUniqueName2, filePath: PathProvider.DocumentFileToConfirm1);

			WorkspaceHelper
				.GoToProjectSettingsPage(projectUniqueName2)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1))
				.UploadDocument(new []{PathProvider.DocumentFileToConfirm2})
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2), ThreadUser.NickName)
				.CreateRevision(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.GoToProjectsPage();

			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper.SelectExportType<ProjectsPage>(ExportType.Source);

			Assert.IsFalse(ExportFileHelper.GetTextNotificationByNumber(1).Contains(secondNotifierDocName),
				"Произошла ошибка:\n в первом сообщении об экспорте есть название документа '{0}'", secondNotifierDocName);

			ProjectsPage
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickProjectCheckboxInList(ProjectUniqueName)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper
				.SelectExportType<ProjectsPage>(ExportType.Source)
				.AssertCountExportNotifiers(expectedCount: 2);

			Assert.IsTrue(ExportFileHelper.GetTextNotificationByNumber(2).Contains(secondNotifierDocName),
				"Ошибка: во втором сообщении об экспорте нет названия документа '{0}'", secondNotifierDocName);

			ProjectsPage
				.ClickProjectCheckboxInList(ProjectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton();

			ExportFileHelper
				.SelectExportType<ProjectsPage>(ExportType.Source)
				.AssertCountExportNotifiers(expectedCount:3);

			Assert.IsFalse(ExportFileHelper.GetTextNotificationByNumber(3).Contains(secondNotifierDocName),
				"Ошибка: в третьем сообщении об экспорте есть название документа '{0}'", secondNotifierDocName);

			Assert.IsTrue(ExportFileHelper.GetTextNotificationByNumber(2).Contains(secondNotifierDocName),
				"Ошибка: кликнули по верхнему сообщению - появилось не второе.");
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportRenamedDocument(ExportType exportType)
		{
			var newDocumentName = "docName" + DateTime.Now.Ticks;

			WorkspaceHelper.GoToProjectsPage();
			ExportFileHelper.CancelAllNotifiers<ProjectsPage>();

			ProjectsPage
				.OpenProjectInfo(ProjectUniqueName)
				.OpenDocumentInfoForProject(ProjectUniqueName)
				.ClickDocumentSettings(ProjectUniqueName);

			DocumentSettings
				.SetDocumentName(newDocumentName)
				.ClickSaveButton<ProjectsPage>(Driver)
				.AssertDialogBackgroundDisappeared<ProjectsPage>(Driver);

			ProjectsPage.ClickDownloadInDocumentButton(ProjectUniqueName);

			var message = ExportFileHelper
				.SelectExportType<ProjectsPage>(exportType)
				.AssertPreparingDownloadMessageDisappeared()
				.GetTextNotificationByNumber(1);

			Assert.IsTrue(message.Contains(newDocumentName),
				"Произошла ошибка:\n экспортируется документ со старым названием");
		}
		
		private const int _maxNotifierNumber = 5;
	}
}
