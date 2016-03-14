using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.CreateProjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.CreateProjects
{
	[Parallelizable(ParallelScope.Fixtures)]
	class CreateProjectWithRight<TWebDriverProvider> : CreateProjectWithRightBaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreateProjectTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectLoaded(_projectUniqueName),
				"Произошла ошибка: не исчезла пиктограмма загрузки проекта");

			Assert.IsFalse(_projectsPage.IsFatalErrorSignDisplayed(_projectUniqueName),
				"Произошла ошибка: появилась пиктограмма ошибки напротив проекта");

			Assert.IsFalse(_projectsPage.IsWarningSignDisplayed(_projectUniqueName),
				"Произошла ошибка: появилась пиктограмма предупреждения напротив проекта");
		}

		[Test]
		public void AddDocumentToProjectTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog
				.UploadDocument(new []{PathProvider.DocumentFileToConfirm1})
				.ClickFihishUploadOnProjectsPage();

			Assert.IsTrue(_projectsPage.IsProjectLoaded(_projectUniqueName),
				"Произошла ошибка: не исчезла пиктограмма загрузки проекта");

			Assert.IsFalse(_projectsPage.IsFatalErrorSignDisplayed(_projectUniqueName),
				"Произошла ошибка: появилась пиктограмма ошибки напротив проекта");

			Assert.IsFalse(_projectsPage.IsWarningSignDisplayed(_projectUniqueName),
				"Произошла ошибка: появилась пиктограмма предупреждения напротив проекта");
		}

		[TestCase(ExportType.Source)]
		[TestCase(ExportType.Target)]
		public void DownloadDocumentTest(ExportType exportType)
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog
				.UploadDocument(new []{PathProvider.DocumentFileToConfirm1})
				.ClickFihishUploadOnProjectsPage();

			_projectsPage
				.WaitUntilProjectLoadSuccessfully(_projectUniqueName)
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName, documentNumber: 2)
				.SelectDocument(_projectUniqueName, PathProvider.EditorTxtFile)
				.ClickDownloadInProjectMenuButton(_projectUniqueName)
				.ClickExportType(exportType);

			_exportNotification.ClickDownloadNotifier<ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(
				_exportNotification.GetExportFileNameMask(exportType, PathProvider.EditorTxtFile)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Target)]
		[TestCase(ExportType.Source)]
		public void DownloadAllProjectDocumentsFromProjectMenuTest(ExportType exportType)
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog
				.UploadDocument(new []{PathProvider.DocumentFileToConfirm1})
				.ClickFihishUploadOnProjectsPage();

			_projectsPage
				.WaitUntilProjectLoadSuccessfully(_projectUniqueName)
				.ClickProjectCheckboxInList(_projectUniqueName)
				.OpenProjectInfo(_projectUniqueName)
				.ClickDownloadInProjectMenuButton(_projectUniqueName)
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(string.Format("Documents_*{0}.zip", exportType)),
				"Произошла ошибка: файл не загрузился");
		}

		[TestCase(ExportType.Target)]
		[TestCase(ExportType.Source)]
		public void DownloadAllDocumentsFromProjectTest(ExportType exportType)
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog
				.UploadDocument(new[] { PathProvider.DocumentFileToConfirm1 })
				.ClickFihishUploadOnProjectsPage();

			_projectsPage
				.WaitUntilProjectLoadSuccessfully(_projectUniqueName)
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDownloadInMainMenuButton()
				.ClickExportType(exportType);

			Assert.IsTrue(_exportNotification.IsExportNotificationDisplayed(),
				"Произошла ошибка:\n сообщение со ссылкой на скачивание документа не появилось");

			_exportNotification.ClickDownloadNotifier<ProjectsPage>();

			Assert.IsTrue(_exportNotification.IsFileDownloaded(string.Format("Documents_*{0}.zip", exportType)),
				"Произошла ошибка: файл не загрузился");
		}

		[Test]
		public void DeleteProjectTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			Assert.IsFalse(_projectsPage.IsProjectExist(_projectUniqueName),
				"Произошла ошибка:\n проект {0} найден в списке проектов", _projectUniqueName);
		}

		[Test]
		public void DeleteProjectWhenProjectInfoOpenedTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.OpenProjectInfo(_projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			Assert.IsFalse(_projectsPage.IsProjectExist(_projectUniqueName),
				"Произошла ошибка:\n проект {0} найден в списке проектов", _projectUniqueName);
		}

		[Test]
		public void DeleteOneDocumentInProjectTest()
		{
			var document = PathProvider.DocumentFile;

			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: document);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.SelectDocument(_projectUniqueName, document)
				.ClickDeleteOpenProjectWithOneFileInProjectMenu(_projectUniqueName);

			_deleteProjectOrFileDialog.ClickDeleteFileButton();

			Assert.IsTrue(
				_projectsPage.IsDocumentRemovedFromProject(_projectUniqueName, document),
				"Произошла ошибка: документ {0} присутствует в проекте {1}", document, _projectUniqueName);
		}

		[Test]
		public void DeleteAllDocumentsInProjectTest()
		{
			var document1 = PathProvider.DocumentFile;
			var document2 = PathProvider.DocumentFile2;

			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: document1);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog
				.UploadDocument(new[] {document2})
				.ClickFihishUploadOnProjectsPage();

			_projectsPage
				.WaitUntilProjectLoadSuccessfully(_projectUniqueName)
				.OpenProjectInfo(_projectUniqueName)
				.SelectDocument(_projectUniqueName, document1)
				.SelectDocument(_projectUniqueName, document2)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			Assert.IsTrue(
				_projectsPage.IsDocumentRemovedFromProject(_projectUniqueName, document1),
				"Произошла ошибка: документ {0} присутствует в проекте {1}", document1, _projectUniqueName);

			Assert.IsTrue(
				_projectsPage.IsDocumentRemovedFromProject(_projectUniqueName, document2),
				"Произошла ошибка: документ {0} присутствует в проекте {1}", document2, _projectUniqueName);
		}
	}
}
