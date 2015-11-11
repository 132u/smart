using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	class BasicProjectTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);
			_workspaceHelper.GoToProjectsPage();
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_deleteDialog = new DeleteDialog(Driver);
			_newProjectGeneralInformationDialog= new NewProjectGeneralInformationDialog(Driver);

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test]
		public void CreateProjectNoFileTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", _projectUniqueName);
		}

		[Test]
		public void DeleteProjectNoFileTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			Assert.IsFalse(_projectsPage.IsProjectExist(_projectUniqueName),
				"Произошла ошибка:\n проект {0} найден в списке проектов", _projectUniqueName);
		}

		[Test]
		public void DeleteProjectWithFileTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.DocumentFile);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDeleteOpenProjectWithFile()
				.ClickDeleteProjectButton();

			Assert.IsFalse(_projectsPage.IsProjectExist(_projectUniqueName),
				"Произошла ошибка:\n проект {0} найден в списке проектов", _projectUniqueName);
		}

		[Test]
		public void CreateProjectDeletedNameTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectGeneralInformationDialog>();
		}

		[Test]
		public void CreateProjectDuplicateNameTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectGeneralInformationDialog>();

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsDuplicateNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилось сообщение о существующем имени");

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsNameInputValidationMarkerDisplayed(),
				"Произошла ошибка:\n поле 'Название' не отмечено ошибкой");
		}

		[Test]
		public void CreateProjectLongNameTest()
		{
			var longProjectUniqueName = _createProjectHelper.GetProjectUniqueName() + _longName;
			_createProjectHelper
				.CreateNewProject(longProjectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(longProjectUniqueName.Substring(0, 100)),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", longProjectUniqueName.Substring(0, 100));
		}

		[Test]
		public void CreateProjectEqualLanguagesTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName, sourceLanguage: Language.English, targetLanguage: Language.English)
				.ClickNextButton<NewProjectGeneralInformationDialog>();

			Assert.True(_newProjectGeneralInformationDialog.IsDuplicateLanguageErrorMessageDisplayed(),
				"Произошла ошибка:\n не отображается сообщение о том, что source и target языки совпадают");
		}

		[TestCase("*")]
		[TestCase("|")]
		[TestCase("\\")]
		[TestCase(":")]
		[TestCase("\"")]
		[TestCase("<\\>")]
		[TestCase("?")]
		[TestCase("/")]
		public void CreateProjectForbiddenSymbolsTest(string forbiddenChar)
		{
			var projectUniqueNameForbidden = _createProjectHelper.GetProjectUniqueName() + forbiddenChar;

			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(projectUniqueNameForbidden)
				.ClickNextButton<NewProjectGeneralInformationDialog>();

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsForbiddenSymbolsInNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилось сообщение о недопустимых символах в имени");

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsNameInputValidationMarkerDisplayed(),
				"Произошла ошибка:\n поле 'Название' не отмечено ошибкой");
		}

		[TestCase("")]
		[TestCase(" ")]
		public void CreateProjectEmptyNameTest(string projectName)
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(projectName)
				.ClickNextButton<NewProjectGeneralInformationDialog>();

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsEmptyNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилось сообщение о пустом имени проекта");

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsNameInputValidationMarkerDisplayed(),
				"Произошла ошибка:\n поле 'Название' не отмечено ошибкой");
		}

		[Test]
		public void CreateProjectTwoWordsNameTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName() + " " + "SpacePlusSymbols";

			_createProjectHelper.CreateNewProject(projectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", projectUniqueName);
		}

		[Test]
		public void DeleteDocumentFromProject()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName)
				.GoToProjectSettingsPage(_projectUniqueName)
				.UploadDocument(PathProvider.DocumentFile)
				.DeleteDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
		}

		[Test]
		public void CancelCreateProjectOnFirstStepTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog.CancelCreateProject();
		}

		[Test]
		public void DeleteFileFromWizard()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.UploadFile(PathProvider.DocumentFile)
				.ClickDeleteFile(PathProvider.DocumentFile);

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsFileDeleted(PathProvider.DocumentFile),
				"Произошла ошибка:\n файл {0} не удалился.", PathProvider.DocumentFile);
		}

		private string _projectUniqueName;
		private const string _longName = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";

		private WorkspaceHelper _workspaceHelper;
		private CreateProjectHelper _createProjectHelper;

		private ProjectsPage _projectsPage;
		private DeleteDialog _deleteDialog;
		private NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog;
	}
}
