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
			_workspaceHelper.GoToProjectsPage();
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog(Driver);
		}

		[Test]
		public void AutofillProjectName()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog.UploadFile(_filePath);

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsFileUploaded(_filePath),
				"Произошла ошибка:\n не удалось загрузить файл {0}.", _filePath);

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsProjectNameMatchExpected(_fileName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		[Test, Ignore("Проверить тест: загружаются 2 одинаковых файла. Что проверяет тест?")]
		public void AutofillProjectNameAddTwoFiles()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.UploadFile(_filePath)
				.UploadFile(_filePath);

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsFileUploaded(_filePath),
				"Произошла ошибка:\n не удалось загрузить файл {0}.", _filePath);

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsProjectNameMatchExpected(_fileName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		[Test]
		public void AutofillProjectNameDeleteFile()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.UploadFile(_filePath)
				.ClickDeleteFile(_filePath);

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsFileDeleted(_filePath),
				"Произошла ошибка:\n файл {0} не удалился.", _filePath);

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsProjectNameMatchExpected(_fileName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		private CreateProjectHelper _createProjectHelper;
		private string _fileName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);
		private string _filePath = PathProvider.DocumentFile;
		private WorkspaceHelper _workspaceHelper;
		private NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog;
		private ProjectsPage _projectsPage;
	}
}
