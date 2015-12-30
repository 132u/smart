using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class ProjectImportTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void Setup()
		{
			_workspacePage = new WorkspacePage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_projectsPage = new ProjectsPage(Driver);

			_workspacePage.GoToProjectsPage();
		}

		[Test]
		public void ImportUnsupportedFileTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.UploadDocumentExpectingError(PathProvider.AudioFile);

			Assert.IsTrue(_newProjectDocumentUploadPage.IsWrongDocumentFormatErrorDisplayed(PathProvider.AudioFile),
				"Произошла ошибка:\n не появилось сообщение о неверном формате загружаемого документа");
		}

		[Test]
		public void ImportSomeFilesTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFile(PathProvider.DocumentFile)
				.UploadDocumentFile(PathProvider.TtxFile);

			Assert.IsFalse(_newProjectDocumentUploadPage.IsWrongDocumentFormatErrorDisplayed(PathProvider.TtxFile),
				"Произошла ошибка:\n появилось сообщение о неверном формате загружаемого документа");
		}

		[Test]
		public void ImportTtxFileTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.UploadDocumentFile(PathProvider.TtxFile);

			Assert.IsFalse(_newProjectDocumentUploadPage.IsWrongDocumentFormatErrorDisplayed(PathProvider.TtxFile),
				"Произошла ошибка:\n появилось сообщение о неверном формате загружаемого документа");
		}

		[Test]
		public void ImportTxtFileTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.UploadDocumentFile(PathProvider.TxtFile);

			Assert.IsFalse(_newProjectDocumentUploadPage.IsWrongDocumentFormatErrorDisplayed(PathProvider.TxtFile),
				"Произошла ошибка:\n появилось сообщение о неверном формате загружаемого документа");
		}

		[Test]
		public void ImportSrtFileTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.UploadDocumentFile(PathProvider.SrtFile);

			Assert.IsFalse(_newProjectDocumentUploadPage.IsWrongDocumentFormatErrorDisplayed(PathProvider.SrtFile),
				"Произошла ошибка:\n появилось сообщение о неверном формате загружаемого документа");
		}

		[Test]
		public void ImportDocumentAfterCreationTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsHelper.UploadDocument(new []{PathProvider.DocumentFile});

			Assert.IsTrue(_projectSettingsPage.IsDocumentExist(PathProvider.DocumentFile),
				"Произошла ошибка:\n документ {0} отсутствует в проекте.", PathProvider.DocumentFile);
		}

		[Test]
		public void ImportDuplicateDocumentTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, PathProvider.DocumentFile);

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsPage.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog.UploadDocument(
				new[] { PathProvider.DocumentFile, PathProvider.DocumentFile });

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsDuplicateDocumentNameErrorExist(),
				"Произошла ошибка:\n нет появилась ошибка о том, что в проекте уже есть файл с таким именем.");
		}

		private string _projectUniqueName;

		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private WorkspacePage _workspacePage;
		private DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
		private NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		private ProjectsPage _projectsPage;
		private ProjectSettingsPage _projectSettingsPage;
	}
}
