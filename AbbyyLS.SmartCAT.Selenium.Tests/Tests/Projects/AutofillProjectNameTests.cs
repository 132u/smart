using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	class AutofillProjectNameTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);

			_filePath = PathProvider.DocumentFile;
			_fileName = Path.GetFileName(PathProvider.DocumentFile);
			_projectName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);

			_workspaceHelper.GoToProjectsPage();
		}

		[Test]
		public void AutofillProjectName()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.UploadDocument(_filePath);

			Assert.IsTrue(_newProjectDocumentUploadPage.IsFileUploaded(_filePath),
				"Произошла ошибка:\n не удалось загрузить файл {0}.", _filePath);

			_newProjectDocumentUploadPage.ClickSettingsButton();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsProjectNameMatchExpected(_projectName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		[Test]
		public void AutofillProjectNameAddTwoFiles()
		{
			var secondFilePath = PathProvider.DocumentFile2;

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocument(_filePath)
				.UploadDocument(secondFilePath);

			Assert.IsTrue(_newProjectDocumentUploadPage.IsFileUploaded(secondFilePath),
				"Произошла ошибка:\n не удалось загрузить файл {0}.", _filePath);

			_newProjectDocumentUploadPage.ClickSettingsButton();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsProjectNameMatchExpected(_projectName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		[Test]
		public void AutofillProjectNameDeleteFile()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocument(_filePath)
				.DeleteDocument(_fileName);

			Assert.IsTrue(_newProjectDocumentUploadPage.IsFileDeleted(_filePath),
				"Произошла ошибка:\n файл {0} не удалился.", _filePath);

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsProjectNameMatchExpected(string.Empty),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		private string _fileName;
		private string _filePath;
		private string _projectName;

		private WorkspaceHelper _workspaceHelper;
		private NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		private ProjectsPage _projectsPage;
	}
}
