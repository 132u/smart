using System;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class ExportNotifierTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpExportNotifierTests()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_documentSettingsDialog = new DocumentSettingsDialog(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_exportNotification = new ExportNotification(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_workspaceHelper.GoToProjectsPage();

			_exportNotification.CancelAllNotifiers<ProjectsPage>();

			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.DocumentFileToConfirm1);
		}

		[Test]
		public void ExportCloseNotifier()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDownloadInProjectButton(_projectUniqueName)
				.ClickExportType(ExportType.Target);

			_exportNotification.ClickCancelNotifier<ProjectsPage>();

			Assert.IsTrue(_workspacePage.GetCountExportNotifiers() == 0,
				"Произошла ошибка:\n остались открытые уведомления");
		}

		[TestCase(PlaceSearchNotifier.ProjectsPage)]
		[TestCase(PlaceSearchNotifier.ProjectSettingsPage)]
		[TestCase(PlaceSearchNotifier.GlossariesPage)]
		public void ExportSaveNotifier(PlaceSearchNotifier placeSearch)
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDownloadInProjectButton(_projectUniqueName)
				.ClickExportType(ExportType.Source);

			switch (placeSearch)
			{
				case PlaceSearchNotifier.ProjectsPage:
					_projectsPage.RefreshPage<ProjectsPage>();
					break;

				case PlaceSearchNotifier.ProjectSettingsPage:
					_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);
					break;

				case PlaceSearchNotifier.GlossariesPage:
					_workspaceHelper.GoToGlossariesPage();

					Assert.IsTrue(_workspacePage.GetCountExportNotifiers() == 0,
						"Произошла ошибка:\n остались открытые уведомления");

					_workspaceHelper.GoToProjectsPage();
					break;

				default:
					throw new Exception(string.Format("Передан неверный аргумент:'{0}'", placeSearch));
			}

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 1),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");
		}

		[Test]
		public void CheckNotifierText()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.ClickDownloadInDocumentButton(_projectUniqueName)
				.ClickExportType(ExportType.Source);

			Assert.IsTrue(_exportNotification.IsNotificationContainsText(Path.GetFileName(PathProvider.DocumentFileToConfirm1)),
				"Произошла ошибка:\n сообщение не содержит искомый текст");
		}

		[Test]
		public void ExportProjectOneDocCheckNotifierText()
		{
			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			Assert.IsTrue(_exportNotification.IsNotificationContainsText(Path.GetFileName(PathProvider.DocumentFileToConfirm1)),
				"Произошла ошибка:\n сообщение не содержит искомый текст");
		}

		[Test]
		public void ExportProjectMultiDocCheckNotifierText()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog.UploadDocument(new[] { PathProvider.DocumentFileToConfirm2 });

			if (!_documentUploadGeneralInformationDialog.IsFileUploaded(PathProvider.DocumentFileToConfirm2))
			{
				throw new Exception("Произошла ошибка: \n не удалось дождаться загрузки документа.");
			}

			_documentUploadGeneralInformationDialog.ClickFihishUploadOnProjectsPage();

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			string expextedText = "Documents";
			Assert.IsTrue(_exportNotification.IsNotificationContainsText(expextedText),
				"Произошла ошибка:\n сообщение не содержит искомый текст: {0}", expextedText);
		}

		[Test]
		public void ExportProjectsCheckNotifierText()
		{
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName2, filePath: PathProvider.DocumentFileToConfirm1);

			_workspaceHelper
				.GoToProjectSettingsPage(projectUniqueName2)
				.UploadDocument(new[] {PathProvider.DocumentFileToConfirm2})
				.GoToProjectsPage();

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			string expextedText = "Documents";
			Assert.IsTrue(_exportNotification.IsNotificationContainsText(expextedText),
				"Произошла ошибка:\n сообщение не содержит искомый текст: {0}", expextedText);
		}

		[Test]
		public void ExportChangeNotifiers()
		{
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName2, filePath: PathProvider.DocumentFileToConfirm1);

			_workspaceHelper
				.GoToProjectSettingsPage(projectUniqueName2)
				.UploadDocument(new[] {PathProvider.DocumentFileToConfirm2})
				.GoToProjectsPage();

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 1),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 2),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");

			var message1 = _exportNotification.GetTextNotificationByNumber(2);
			var message2 = _exportNotification.GetTextNotificationByNumber(1);

			Assert.AreNotEqual(message1, message2,
				"Произошла ошибка:\n тесты сообщений обоих уведомлений совпадают.");
		}

		[Test]
		public void ExportLimitNotifiers()
		{
			_projectsPage.ClickProjectCheckboxInList(_projectUniqueName);

			for (int i = 0; i < _maxNotifierNumber; i++)
			{
				_projectsPage
					.ClickDownloadInMainMenuButton()
					.ClickExportType(ExportType.Source);
			}

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: _maxNotifierNumber),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");
		}

		[Test]
		public void ExportMoreLimitNotifiers()
		{
			_projectsPage.ClickProjectCheckboxInList(_projectUniqueName);

			for (int i = 0; i <= _maxNotifierNumber; i++)
			{
				_projectsPage
					.ClickDownloadInMainMenuButton()
					.ClickExportType(ExportType.Source);
			}

			var notificationCount = _workspacePage.GetCountExportNotifiers();

			Assert.IsFalse(notificationCount > _maxNotifierNumber,
				"Произошла ошибка:\n сообщений об экспорте показано {0}, ожидалось не больше {1}.",
				notificationCount, _maxNotifierNumber);
		}

		[Test]
		public void ExportCheckNotifiersFreshAbove()
		{
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName2, filePath: PathProvider.DocumentFileToConfirm1);

			_workspaceHelper
				.GoToProjectSettingsPage(projectUniqueName2)
				.UploadDocument(new[] {PathProvider.DocumentFileToConfirm2})
				.GoToProjectsPage();

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			var message1 = _exportNotification.GetTextNotificationByNumber(1);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			var message2 = _exportNotification.GetTextNotificationByNumber(2);

			Assert.AreNotEqual(message1, message2,
				"Произошла ошибка:\n сообщение не изменилось, свежее сообщение должно быть сверху");
		}

		[Test]
		public void ExportChangeFirstToThird()
		{
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName2, filePath: PathProvider.DocumentFileToConfirm1);

			_workspaceHelper
				.GoToProjectSettingsPage(projectUniqueName2)
				.UploadDocument(new[] {PathProvider.DocumentFileToConfirm2})
				.GoToProjectsPage();

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 2),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");

			_projectsPage
				.ClickProjectCheckboxInList(projectUniqueName2)
				.OpenProjectInfo(projectUniqueName2)
				.OpenDocumentInfoForProject(projectUniqueName2, documentNumber: 2)
				.ClickDownloadInDocumentButton(projectUniqueName2, documentNumber: 2)
				.ClickExportType(ExportType.Source);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 3),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");

			var message = _exportNotification.GetTextNotificationByNumber(1);

			Assert.IsTrue(message.Contains(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1)),
				"Произошла ошибка:\n кликнули по верхнему сообщению - появилось не первое!");
		}

		[Test]
		public void ExportChangeFirstToSecond()
		{
			var secondNotifierDocName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1);
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName2, filePath: PathProvider.DocumentFileToConfirm1);

			_workspaceHelper
				.GoToProjectSettingsPage(projectUniqueName2)
				.UploadDocument(new[] {PathProvider.DocumentFileToConfirm2})
				.GoToProjectsPage();

			_projectsPage
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			Assert.IsFalse(_exportNotification.GetTextNotificationByNumber(1).Contains(secondNotifierDocName),
				"Произошла ошибка:\n в первом сообщении об экспорте есть название документа '{0}'", secondNotifierDocName);

			_projectsPage
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 2),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");

			Assert.IsTrue(_exportNotification.GetTextNotificationByNumber(2).Contains(secondNotifierDocName),
				"Ошибка: во втором сообщении об экспорте нет названия документа '{0}'", secondNotifierDocName);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 3),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");

			Assert.IsFalse(_exportNotification.GetTextNotificationByNumber(3).Contains(secondNotifierDocName),
				"Ошибка: в третьем сообщении об экспорте есть название документа '{0}'", secondNotifierDocName);

			Assert.IsTrue(_exportNotification.GetTextNotificationByNumber(2).Contains(secondNotifierDocName),
				"Ошибка: кликнули по верхнему сообщению - появилось не второе.");
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Tmx)]
		[TestCase(ExportType.Target)]
		public void ExportRenamedDocument(ExportType exportType)
		{
			var newDocumentName = "docName" + DateTime.Now.Ticks;

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.ClickDocumentSettings(_projectUniqueName);

			_documentSettingsDialog
				.SetDocumentName(newDocumentName)
				.ClickSaveButton<ProjectsPage>(Driver)
				.AssertDialogBackgroundDisappeared<ProjectsPage>(Driver);

			_projectsPage
				.ClickDownloadInDocumentButton(_projectUniqueName)
				.ClickExportType(exportType);

			Assert.IsTrue(_projectsPage.IsPreparingDownloadMessageDisappeared(),
				"Произошла ошибка:\n сообщение 'Preparing documents for download. Please wait ...' не исчезло");

			var message = _exportNotification.GetTextNotificationByNumber(1);

			Assert.IsTrue(message.Contains(newDocumentName),
				"Произошла ошибка:\n экспортируется документ со старым названием");
		}

		[TestCase(PlaceSearchNotifier.ProjectsPage)]
		[TestCase(PlaceSearchNotifier.ProjectSettingsPage)]
		[TestCase(PlaceSearchNotifier.GlossariesPage)]
		public void ExportSaveNotifierOnProjectSettingsPage(PlaceSearchNotifier placeSearch)
		{
			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			switch (placeSearch)
			{
				case PlaceSearchNotifier.ProjectsPage:
					_workspaceHelper.GoToProjectsPage();
					break;

				case PlaceSearchNotifier.ProjectSettingsPage:
					_workspaceHelper.RefreshPage();
					break;

				case PlaceSearchNotifier.GlossariesPage:
					_workspaceHelper.GoToGlossariesPage();

					Assert.AreEqual(_workspacePage.GetCountExportNotifiers(), 0,
						"Произошла ошибка:\n остались открытые уведомления");

					_workspaceHelper.GoToProjectsPage();
					_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);
					break;

				default:
					throw new Exception(string.Format("Передан неверный аргумент:'{0}'", placeSearch));
			}

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 1),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");
		}

		[Ignore("PRX-11419")]
		[Test]
		public void ExportSaveNotifierAnotherProjectPage()
		{
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName2, filePath: PathProvider.DocumentFileToConfirm1);

			_workspaceHelper
				.GoToProjectSettingsPage(projectUniqueName2)
				.UploadDocument(new[] {PathProvider.DocumentFileToConfirm2})
				.GoToProjectsPage();

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			_workspaceHelper.GoToProjectsPage();
			_workspaceHelper.GoToProjectSettingsPage(projectUniqueName2);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 1),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");
		}

		[Test]
		public void ExportDocumentFromProjectCheckNotifierText()
		{
			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			Assert.IsTrue(_exportNotification.IsNotificationContainsText(Path.GetFileName(PathProvider.DocumentFileToConfirm1)),
				"Произошла ошибка:\n сообщение не содержит искомый текст");
		}

		[Ignore("PRX-11419")]
		[Test]
		public void ExportDocumentsFromProjectCheckNotifierText()
		{
			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsHelper
				.UploadDocument(new[] {PathProvider.DocumentFileToConfirm2});

			_projectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			Assert.IsTrue(_exportNotification.IsNotificationContainsText("Documents"),
				"Произошла ошибка:\n сообщение не содержит искомый текст");
		}

		[Test]
		public void ExportDocumentCheckNotifierDate()
		{
			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsPage
				.ClickDocumentCheckbox(PathProvider.DocumentFileToConfirm1)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Source);

			Assert.IsTrue(_exportNotification.IsNotificationContainsCurrentDate(),
				"Произошла ошибка:\n сообщение не содержит требуемую дату.");
		}

		private const int _maxNotifierNumber = 5;

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private ProjectsPage _projectsPage;
		private DocumentSettingsDialog _documentSettingsDialog;
		private WorkspaceHelper _workspaceHelper;
		private ProjectSettingsPage _projectSettingsPage;
		private ExportNotification _exportNotification;
		private WorkspacePage _workspacePage;
		private DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
	}
}
