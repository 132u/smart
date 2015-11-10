using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
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
			_workspaceHelper = new WorkspaceHelper(Driver);
			_workspaceHelper.GoToProjectsPage();
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			_projectsPage = new ProjectsPage(Driver);
		}

		[Test]
		public void ImportUnsupportedFileTest()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.UploadFileExpectingError(PathProvider.AudioFile)
				.AssertErrorFormatDocument();
		}

		[Test]
		public void ImportSomeFilesTest()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.UploadFile(PathProvider.DocumentFile)
				.UploadFile(PathProvider.TtxFile)
				.AssertNoErrorFormatDocument();
		}

		[Test]
		public void ImportTtxFileTest()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.UploadFile(PathProvider.TtxFile)
				.AssertNoErrorFormatDocument();
		}

		[Test]
		public void ImportTxtFileTest()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.UploadFile(PathProvider.TxtFile)
				.AssertNoErrorFormatDocument();
		}

		[Test]
		public void ImportSrtFileTest()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.UploadFile(PathProvider.SrtFile)
				.AssertNoErrorFormatDocument();
		}

		[Test]
		public void ImportDocumentAfterCreationTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName)
				.GoToProjectSettingsPage(_projectUniqueName)
				.UploadDocument(PathProvider.DocumentFile)
				.AssertDocumentExist(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
		}

		[Test]
		public void ImportDuplicateDocumentTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.DocumentFile)
				.GoToProjectSettingsPage(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog
				.UploadDocument(PathProvider.DocumentFile)
				.UploadDocument(PathProvider.DocumentFile);

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsDuplicateDocumentNameErrorExist(),
				"Произошла ошибка:\n нет появилась ошибка о том, что в проекте уже есть файл с таким именем.");
		}

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private WorkspaceHelper _workspaceHelper;

		private DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
		private ProjectsPage _projectsPage;
	}
}
