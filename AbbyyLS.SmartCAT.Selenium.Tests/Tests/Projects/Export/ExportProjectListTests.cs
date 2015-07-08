using System;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Standalone]
	internal class ExportProjectListTests<TWebDriverProvider> : ExportProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportProjectFromMainMenuTest(ExportType exportType)
		{
			_projectSettingsHelper.GoToProjectsPage();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();
			
			_projectsHelper
				.SelectProjectInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(exportType)
				.ClickDownloadNotifier<ProjectsPage>()
				.AssertFileDownloaded(_exportFileHelper.GetExportFileNameMask(exportType, PathProvider.DocumentFileToConfirm1));
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportProjectFromProjectInfoTest(ExportType exportType)
		{
			_projectSettingsHelper.GoToProjectsPage();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.OpenProjectInfo(_projectUniqueName)
				.ClickDownloadInProjectButton(_projectUniqueName)
				.SelectExportType<ProjectsPage>(exportType)
				.ClickDownloadNotifier<ProjectsPage>()
				.AssertFileDownloaded(_exportFileHelper.GetExportFileNameMask(exportType, PathProvider.DocumentFileToConfirm1));
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportDocumentFromDocumentInfoTest(ExportType exportType)
		{
			_projectSettingsHelper.GoToProjectsPage();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.ClickDownloadInDocumentButton(_projectUniqueName)
				.SelectExportType<ProjectsPage>(exportType)
				.ClickDownloadNotifier<ProjectsPage>()
				.AssertFileDownloaded(_exportFileHelper.GetExportFileNameMask(exportType, PathProvider.DocumentFileToConfirm1));
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportMultiDocumentsTest(ExportType exportType)
		{
			_projectSettingsHelper
				.UploadDocument(PathProvider.DocumentFileToConfirm2)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.OpenDocument<SelectTaskDialog>(PathProvider.DocumentFileToConfirm2)
				.SelectTask()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton()
				.GoToProjectsPage();

			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.SelectProjectInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(exportType)
				.ClickDownloadNotifier<ProjectsPage>()
				.AssertFileDownloaded(string.Format("Documents_*{0}.zip", exportType));
		}

		[Test]
		public void ExportCloseNotifier()
		{
			_projectSettingsHelper.GoToProjectsPage();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.OpenProjectInfo(_projectUniqueName)
				.ClickDownloadInProjectButton(_projectUniqueName)
				.SelectExportType<ProjectsPage>(ExportType.Source)
				.ClickCancelInDowloadNotifier<ProjectsPage>()
				.AssertNotificationNotExist();
		}

		[TestCase(PlaceSearchNotifier.ProjectsPage)]
		[TestCase(PlaceSearchNotifier.ProjectSettingsPage)]
		[TestCase(PlaceSearchNotifier.GlossariesPage)]
		public void ExportSaveNotifier(PlaceSearchNotifier placeSearch)
		{
			_projectSettingsHelper.GoToProjectsPage();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.OpenProjectInfo(_projectUniqueName)
				.ClickDownloadInProjectButton(_projectUniqueName)
				.SelectExportType<ProjectsPage>(ExportType.Source);

			switch (placeSearch)
			{
				case PlaceSearchNotifier.ProjectsPage:
					_projectsHelper.RefreshPage();
					break;

				case PlaceSearchNotifier.ProjectSettingsPage:
					_projectsHelper.GoToProjectSettingsPage(_projectUniqueName);
					break;

				case PlaceSearchNotifier.GlossariesPage:
					_projectsHelper.GoToGlossariesPage();
					_exportFileHelper.AssertNotificationNotExist();
					_projectsHelper.GoToProjectsPage();
					break;

				default:
					throw new Exception(string.Format("Передан неверный аргумент:'{0}'", placeSearch));
			}

			_exportFileHelper.AssertCountExportNotifiers(expectedCount: 1);
		}

		[Test]
		public void CheckNotifierText()
		{
			_projectSettingsHelper.GoToProjectsPage();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.ClickDownloadInDocumentButton(_projectUniqueName)
				.SelectExportType<ProjectsPage>(ExportType.Source);

			_exportFileHelper.AssertContainsText(Path.GetFileName(PathProvider.DocumentFileToConfirm1));
		}

		[Test]
		public void ExportProjectOneDocCheckNotifierText()
		{
			_projectSettingsHelper.GoToProjectsPage();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.SelectProjectInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(ExportType.Source);

			_exportFileHelper.AssertContainsText(Path.GetFileName(PathProvider.DocumentFileToConfirm1));
		}

		[Test]
		public void ExportProjectMultiDocCheckNotifierText()
		{
			_projectSettingsHelper
				.UploadDocument(PathProvider.DocumentFileToConfirm2)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.OpenDocument<SelectTaskDialog>(PathProvider.DocumentFileToConfirm2)
				.SelectTask()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton()
				.GoToProjectsPage();

			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.SelectProjectInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(ExportType.Source);

			_exportFileHelper.AssertContainsText("Documents");
		}

		[Test]
		public void ExportProjectsCheckNotifierText()
		{
			_projectSettingsHelper.GoToProjectsPage();
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(projectUniqueName2, PathProvider.DocumentFileToConfirm1)
				.AssertIsProjectLoaded(projectUniqueName2)
				.GoToProjectSettingsPage(projectUniqueName2)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.OpenDocument<SelectTaskDialog>(PathProvider.DocumentFileToConfirm1)
				.SelectTask()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton()
				.UploadDocument(PathProvider.DocumentFileToConfirm2)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.OpenDocument<SelectTaskDialog>(PathProvider.DocumentFileToConfirm2)
				.SelectTask()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton()
				.GoToProjectsPage();

			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.SelectProjectInList(_projectUniqueName)
				.SelectProjectInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(ExportType.Source);

			_exportFileHelper.AssertContainsText("Documents");
		}

		[Test]
		public void ExportChangeNotifiers()
		{
			_projectSettingsHelper.GoToProjectsPage();
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(projectUniqueName2, PathProvider.DocumentFileToConfirm1)
				.AssertIsProjectLoaded(projectUniqueName2)
				.GoToProjectSettingsPage(projectUniqueName2)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.OpenDocument<SelectTaskDialog>(PathProvider.DocumentFileToConfirm1)
				.SelectTask()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton()
				.UploadDocument(PathProvider.DocumentFileToConfirm2)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.OpenDocument<SelectTaskDialog>(PathProvider.DocumentFileToConfirm2)
				.SelectTask()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton()
				.GoToProjectsPage();

			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.SelectProjectInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(ExportType.Source);

			_exportFileHelper.AssertCountExportNotifiers(expectedCount: 1);

			_projectsHelper
				.SelectProjectInList(_projectUniqueName)
				.SelectProjectInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(ExportType.Source);

			var message1 = _exportFileHelper
				.AssertCountExportNotifiers(expectedCount: 2)
				.GetTextNotificationByNumber(1);

			var message2 = _exportFileHelper.GetTextNotificationByNumber(1);
			Assert.AreNotEqual(message1, message2,
				"Произошла ошибка:\n тесты сообщений обоих уведомлений совпадают.");
		}

		[Test]
		public void ExportLimitNotifiers()
		{
			_projectSettingsHelper.GoToProjectsPage();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();
			_projectsHelper.SelectProjectInList(_projectUniqueName);

			for (int i = 0; i < _maxNotifierNumber; i++)
			{
				_projectsHelper
					.ClickDownloadInMainMenuButton()
					.SelectExportType<ProjectsPage>(ExportType.Source);
			}
		}

		[Test]
		public void ExportMoreLimitNotifiers()
		{
			_projectSettingsHelper.GoToProjectsPage();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper.SelectProjectInList(_projectUniqueName);

			for (int i = 0; i <= _maxNotifierNumber; i++)
			{
				_projectsHelper
					.ClickDownloadInMainMenuButton()
					.SelectExportType<ProjectsPage>(ExportType.Source);
			}

			var notificationCount =_exportFileHelper.GetCountExportNotifiers();

			Assert.IsFalse(notificationCount > _maxNotifierNumber,
				"Произошла ошибка:\n сообщений об экспорте показано {0}, ожидалось не больше {1}.",
				notificationCount,
				_maxNotifierNumber);
		}

		[Test]
		public void ExportCheckNotifiersFreshAbove()
		{
			_projectSettingsHelper.GoToProjectsPage();
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(projectUniqueName2, PathProvider.DocumentFileToConfirm1)
				.AssertIsProjectLoaded(projectUniqueName2)
				.GoToProjectSettingsPage(projectUniqueName2)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.OpenDocument<SelectTaskDialog>(PathProvider.DocumentFileToConfirm1)
				.SelectTask()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton()
				.UploadDocument(PathProvider.DocumentFileToConfirm2)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.OpenDocument<SelectTaskDialog>(PathProvider.DocumentFileToConfirm2)
				.SelectTask()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton()
				.GoToProjectsPage();

			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.SelectProjectInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(ExportType.Source);

			var message1 = _exportFileHelper.GetTextNotificationByNumber(1);

			_projectsHelper
				.SelectProjectInList(_projectUniqueName)
				.SelectProjectInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(ExportType.Source);

			var message2 = _exportFileHelper.GetTextNotificationByNumber(2);

			Assert.AreNotEqual(
				message1,
				message2,
				"Произошла ошибка:\n сообщение не изменилось, свежее сообщение должно быть сверху");
		}

		[Test]
		public void ExportChangeFirstToThird()
		{
			_projectSettingsHelper.GoToProjectsPage();
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(projectUniqueName2, PathProvider.DocumentFileToConfirm1)
				.AssertIsProjectLoaded(projectUniqueName2)
				.GoToProjectSettingsPage(projectUniqueName2)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.OpenDocument<SelectTaskDialog>(PathProvider.DocumentFileToConfirm1)
				.SelectTask()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton()
				.UploadDocument(PathProvider.DocumentFileToConfirm2)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.OpenDocument<SelectTaskDialog>(PathProvider.DocumentFileToConfirm2)
				.SelectTask()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton()
				.GoToProjectsPage();

			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.SelectProjectInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(ExportType.Source);

			_projectsHelper
				.SelectProjectInList(_projectUniqueName)
				.SelectProjectInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(ExportType.Source)
				.AssertCountExportNotifiers(expectedCount:2);

			_projectsHelper
				.SelectProjectInList(projectUniqueName2)
				.OpenProjectInfo(projectUniqueName2)
				.OpenDocumentInfoForProject(projectUniqueName2, documentNumber: 2)
				.ClickDownloadInDocumentButton(projectUniqueName2, documentNumber: 2)
				.SelectExportType<ProjectsPage>(ExportType.Source)
				.AssertCountExportNotifiers(expectedCount: 3);

			var message = _exportFileHelper.GetTextNotificationByNumber(1);

			Assert.IsTrue(message.Contains(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1)),
				"Произошла ошибка:\n кликнули по верхнему сообщению - появилось не первое!");
		}

		[Test]
		public void ExportChangeFirstToSecond()
		{
			var secondNotifierDocName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1);
			_projectSettingsHelper.GoToProjectsPage();
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(projectUniqueName2, PathProvider.DocumentFileToConfirm1)
				.AssertIsProjectLoaded(projectUniqueName2)
				.GoToProjectSettingsPage(projectUniqueName2)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.OpenDocument<SelectTaskDialog>(PathProvider.DocumentFileToConfirm1)
				.SelectTask()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton()
				.UploadDocument(PathProvider.DocumentFileToConfirm2)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm2))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.OpenDocument<SelectTaskDialog>(PathProvider.DocumentFileToConfirm2)
				.SelectTask()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton()
				.GoToProjectsPage();

			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			_projectsHelper
				.SelectProjectInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(ExportType.Source);

			Assert.IsFalse(_exportFileHelper.GetTextNotificationByNumber(1).Contains(secondNotifierDocName),
				"Произошла ошибка:\n в первом сообщении об экспорте есть название документа '{0}'", secondNotifierDocName);

			_projectsHelper
				.SelectProjectInList(projectUniqueName2)
				.SelectProjectInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(ExportType.Source)
				.AssertCountExportNotifiers(expectedCount: 2);

			Assert.IsTrue(_exportFileHelper.GetTextNotificationByNumber(2).Contains(secondNotifierDocName),
				"Ошибка: во втором сообщении об экспорте нет названия документа '{0}'", secondNotifierDocName);

			_projectsHelper
				.SelectProjectInList(_projectUniqueName)
				.SelectProjectInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.SelectExportType<ProjectsPage>(ExportType.Source)
				.AssertCountExportNotifiers(expectedCount:3);

			Assert.IsFalse(_exportFileHelper.GetTextNotificationByNumber(3).Contains(secondNotifierDocName),
				"Ошибка: в третьем сообщении об экспорте есть название документа '{0}'", secondNotifierDocName);

			Assert.IsTrue(_exportFileHelper.GetTextNotificationByNumber(2).Contains(secondNotifierDocName),
				"Ошибка: кликнули по верхнему сообщению - появилось не второе.");
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportRenamedDocument(ExportType exportType)
		{
			_projectSettingsHelper.GoToProjectsPage();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();

			var newDocumentName = "docName" + DateTime.Now.Ticks;

			var message = _projectsHelper
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.RenameDocumnet(_projectUniqueName, newDocumentName)
				.ClickDownloadInDocumentButton(_projectUniqueName)
				.SelectExportType<ProjectsPage>(exportType)
				.GetTextNotificationByNumber(1);

			Assert.IsTrue(message.Contains(newDocumentName),
				"Произошла ошибка:\n экспортируется документ со старым названием");
		}
		
		private const int _maxNotifierNumber = 5;
	}
}