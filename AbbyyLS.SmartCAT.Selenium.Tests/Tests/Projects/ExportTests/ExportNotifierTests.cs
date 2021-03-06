﻿using System;
using System.Globalization;
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
	[Projects]
	class ExportNotifierTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpExportNotifierTests()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_documentSettingsDialog = new DocumentSettingsDialog(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_exportNotification = new ExportNotification(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_addFilesStep = new AddFilesStep(Driver);

			_document1 = PathProvider.DocumentFileToConfirm1;
			_document2 = PathProvider.DocumentFileToConfirm2;

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_workspacePage.GoToProjectsPage();

			_exportNotification.CancelAllNotifiers<ProjectsPage>();

			_createProjectHelper.CreateNewProject( 
				_projectUniqueName, filesPaths: new[] { _document1 });
		}

		[Test]
		public void ExportCloseNotifier()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDownloadInProjectMenuButton(_projectUniqueName)
				.ClickExportType(ExportType.Translation);

			_exportNotification.CancelAllNotifiers<ProjectsPage>();

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
				.ClickDownloadInProjectMenuButton(_projectUniqueName)
				.ClickExportType(ExportType.Original);

			switch (placeSearch)
			{
				case PlaceSearchNotifier.ProjectsPage:
					_projectsPage.RefreshPage<ProjectsPage>();
					break;

				case PlaceSearchNotifier.ProjectSettingsPage:
					_projectsPage.OpenProjectSettingsPage(_projectUniqueName);
					break;

				case PlaceSearchNotifier.GlossariesPage:
					_workspacePage.GoToGlossariesPage();

					Assert.IsTrue(_workspacePage.GetCountExportNotifiers() == 0,
						"Произошла ошибка:\n остались открытые уведомления");

					_workspacePage.GoToProjectsPage();
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
				.HoverDocumentRow(_projectUniqueName, _document1)
				.ClickDownloadInDocumentButton(_document1)
				.ClickExportType(ExportType.Original);

			Assert.IsTrue(_exportNotification.IsUpperNotificationContainsText(Path.GetFileName(_document1)),
				"Произошла ошибка:\n сообщение не содержит искомый текст");
		}

		[Test]
		public void ExportProjectOneDocCheckNotifierText()
		{
			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			Assert.IsTrue(_exportNotification.IsUpperNotificationContainsText(Path.GetFileName(_document1)),
				"Произошла ошибка:\n сообщение не содержит искомый текст");
		}

		[Test, Ignore("PRX-16925")]
		public void ExportProjectMultiDocCheckNotifierText()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton()
				.UploadDocument(new[] { _document2 });

			_addFilesStep
				.ClickNextButton()
				.ClickFinish<ProjectsPage>()
				.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			string expextedText = "Documents";
			Assert.IsTrue(_exportNotification.IsUpperNotificationContainsText(expextedText),
				"Произошла ошибка:\n сообщение не содержит искомый текст: {0}", expextedText);
		}

		[Test]
		public void ExportProjectsCheckNotifierText()
		{
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();
			var expextedText = "Documents";

			_createProjectHelper.CreateNewProject(
				projectUniqueName2, filesPaths: new[] { _document1, _document2 });

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			Assert.IsTrue(_exportNotification.IsUpperNotificationContainsText(expextedText),
				"Произошла ошибка:\n сообщение не содержит искомый текст: {0}", expextedText);
		}

		[Test]
		public void ExportChangeNotifiers()
		{
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(
				projectUniqueName2, filesPaths: new[] { _document1 });

			_projectsPage.OpenProjectSettingsPage(projectUniqueName2);

			_projectSettingsHelper.UploadDocument(new[] { _document2 });

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 1),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

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
					.ClickExportType(ExportType.Original);
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
					.ClickExportType(ExportType.Original);
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

			_createProjectHelper.CreateNewProject(
				projectUniqueName2, filesPaths: new[] { _document1, _document2 });

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			var message1 = _exportNotification.GetTextNotificationByNumber(1);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			var message2 = _exportNotification.GetTextNotificationByNumber(2);

			Assert.AreNotEqual(message1, message2,
				"Произошла ошибка:\n сообщение не изменилось, свежее сообщение должно быть сверху");
		}

		[Test]
		public void ExportChangeFirstToThird()
		{
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(
				projectUniqueName2, filesPaths: new[] { _document1, _document2 });

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 2),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");

			_projectsPage
				.ClickProjectCheckboxInList(projectUniqueName2)
				.OpenProjectInfo(projectUniqueName2)
				.HoverDocumentRow(projectUniqueName2, _document2)
				.ClickDownloadInDocumentButton(_document2)
				.ClickExportType(ExportType.Original);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 3),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");

			var message = _exportNotification.GetTextNotificationByNumber(1);

			Assert.IsTrue(message.Contains(Path.GetFileNameWithoutExtension(_document1)),
				"Произошла ошибка:\n кликнули по верхнему сообщению - появилось не первое!");
		}

		[Test]
		public void ExportChangeFirstToSecond()
		{
			var secondNotifierDocName = Path.GetFileNameWithoutExtension(_document1);
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(
				projectUniqueName2, filesPaths: new[] { _document1, _document2 });

			_projectsPage
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			Assert.IsFalse(_exportNotification.GetTextNotificationByNumber(1).Contains(secondNotifierDocName),
				"Произошла ошибка:\n в первом сообщении об экспорте есть название документа '{0}'", secondNotifierDocName);

			_projectsPage
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 2),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");

			Assert.IsTrue(_exportNotification.GetTextNotificationByNumber(2).Contains(secondNotifierDocName),
				"Ошибка: во втором сообщении об экспорте нет названия документа '{0}'", secondNotifierDocName);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 3),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");

			Assert.IsFalse(_exportNotification.GetTextNotificationByNumber(3).Contains(secondNotifierDocName),
				"Произошла ошибка: в третьем сообщении об экспорте есть название документа '{0}'", secondNotifierDocName);

			Assert.IsTrue(_exportNotification.GetTextNotificationByNumber(2).Contains(secondNotifierDocName),
				"Произошла ошибка: кликнули по верхнему сообщению - появилось не второе.");
		}

		[TestCase(ExportType.Original)]
		[TestCase(ExportType.TMX)]
		[TestCase(ExportType.Translation)]
		public void ExportRenamedDocument(ExportType exportType)
		{
			var newDocumentName = "docName" + DateTime.Now.Ticks;

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, _document1)
				.ClickDocumentSettings(_projectUniqueName, _document1);

			_documentSettingsDialog
				.SetDocumentName(newDocumentName)
				.ClickSaveButtonExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, newDocumentName)
				.ClickDownloadInDocumentButton(newDocumentName)
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			Assert.IsTrue(_exportNotification.IsUpperNotificationContainsText(newDocumentName),
				"Произошла ошибка:\n экспортируется документ со старым названием");
		}

		[TestCase(PlaceSearchNotifier.ProjectsPage)]
		[TestCase(PlaceSearchNotifier.ProjectSettingsPage)]
		[TestCase(PlaceSearchNotifier.GlossariesPage)]
		public void ExportSaveNotifierOnProjectSettingsPage(PlaceSearchNotifier placeSearch)
		{
			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.ClickDocumentCheckbox(_document1)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			switch (placeSearch)
			{
				case PlaceSearchNotifier.ProjectsPage:
					_workspacePage.GoToProjectsPage();
					break;

				case PlaceSearchNotifier.ProjectSettingsPage:
					_workspacePage.RefreshPage<WorkspacePage>();
					break;

				case PlaceSearchNotifier.GlossariesPage:
					_workspacePage.GoToGlossariesPage();

					Assert.AreEqual(_workspacePage.GetCountExportNotifiers(), 0,
						"Произошла ошибка:\n остались открытые уведомления");

					_workspacePage.GoToProjectsPage();

					_projectsPage.OpenProjectSettingsPage(_projectUniqueName);
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

			_createProjectHelper.CreateNewProject(
				projectUniqueName2, filesPaths: new[] { _document1, _document2 });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.ClickDocumentCheckbox(_document1)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenProjectSettingsPage(projectUniqueName2);

			Assert.IsTrue(_exportNotification.IsExportNotifiersCountMatchExpected(expectedCount: 1),
				"Произошла ошибка:\n не появилось ожидаемое кол-во уведомлений");
		}

		[Test]
		public void ExportDocumentFromProjectCheckNotifierText()
		{
			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.ClickDocumentCheckbox(_document1)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			Assert.IsTrue(_exportNotification.IsUpperNotificationContainsText(Path.GetFileName(_document1)),
				"Произошла ошибка:\n сообщение не содержит искомый текст");
		}

		[Ignore("PRX-11419")]
		[Test]
		public void ExportDocumentsFromProjectCheckNotifierText()
		{
			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.UploadDocument(new[] { _document2 });

			_projectSettingsPage
				.ClickDocumentCheckbox(_document1)
				.ClickDocumentCheckbox(_document2)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			Assert.IsTrue(_exportNotification.IsUpperNotificationContainsText("Documents"),
				"Произошла ошибка:\n сообщение не содержит искомый текст");
		}

		[Test]
		public void ExportDocumentCheckNotifierDate()
		{
			var expectedDate = DateTime.Now.ToString("MM/dd/yyyy hh", CultureInfo.InvariantCulture);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.ClickDocumentCheckbox(_document1)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(ExportType.Original);

			Assert.IsTrue(_exportNotification.IsUpperNotificationContainsText(expectedDate),
				"Произошла ошибка:\n сообщение не содержит требуемую дату.");
		}

		private const int _maxNotifierNumber = 5;

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private ProjectsPage _projectsPage;
		private DocumentSettingsDialog _documentSettingsDialog;
		private WorkspacePage _workspacePage;
		private ProjectSettingsPage _projectSettingsPage;
		private ExportNotification _exportNotification;
		private AddFilesStep _addFilesStep;
		private string _document1;
		private string _document2;
	}
}
