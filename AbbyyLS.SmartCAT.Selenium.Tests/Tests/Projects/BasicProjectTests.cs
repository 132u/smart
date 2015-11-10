using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
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
		}

		[Test]
		public void CreateProjectNoFileTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper.CreateNewProject(projectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", projectUniqueName);
		}

		[Test]
		public void DeleteProjectNoFileTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper.CreateNewProject(projectUniqueName);

			_projectsPage
				.ClickProjectCheckboxInList(projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			Assert.IsFalse(_projectsPage.IsProjectExist(projectUniqueName),
				"Произошла ошибка:\n проект {0} найден в списке проектов", projectUniqueName);
		}

		[Test]
		public void DeleteProjectWithFileTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper.CreateNewProject(projectUniqueName, filePath: PathProvider.DocumentFile);

			_projectsPage
				.OpenProjectInfo(projectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName)
				.ClickDeleteOpenProjectWithFile()
				.ClickDeleteProjectButton();

			Assert.IsFalse(_projectsPage.IsProjectExist(projectUniqueName),
				"Произошла ошибка:\n проект {0} найден в списке проектов", projectUniqueName);
		}

		[Test]
		public void CreateProjectDeletedNameTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper.CreateNewProject(projectUniqueName);

			_projectsPage
				.ClickProjectCheckboxInList(projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			_projectsPage.ClickCreateProjectDialog();

			_createProjectHelper
				.FillGeneralProjectInformation(projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage();
		}

		[Test]
		public void CreateProjectDuplicateNameTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper.CreateNewProject(projectUniqueName);
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage(errorExpected: true)
				.AssertErrorDuplicateName();
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
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(projectUniqueName, 
					sourceLanguage: Language.English, targetLanguage: Language.English)
				.ClickNextOnGeneralProjectInformationPage(errorExpected: true)
				.AssertErrorDuplicateLanguage();
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
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(projectUniqueNameForbidden)
				.ClickNextOnGeneralProjectInformationPage(errorExpected: true)
				.AssertErrorForbiddenSymbols();
		}

		[TestCase("")]
		[TestCase(" ")]
		public void CreateProjectEmptyNameTest(string projectName)
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(projectName)
				.ClickNextOnGeneralProjectInformationPage(errorExpected: true)
				.AssertErrorNoName();
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
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper
				.CreateNewProject(projectUniqueName)
				.GoToProjectSettingsPage(projectUniqueName)
				.UploadDocument(PathProvider.DocumentFile)
				.DeleteDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
		}

		[Test]
		public void CancelCreateProjectOnFirstStepTest()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper.CancelCreateProject();
		}

		[Test]
		public void DeleteFileFromWizard()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.AddFileFromWizard(PathProvider.DocumentFile)
				.DeleteFileFromWizard(PathProvider.DocumentFile);
		}

		private CreateProjectHelper _createProjectHelper;
		private const string _longName = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
		private WorkspaceHelper _workspaceHelper;
		private ProjectsPage _projectsPage;
		private DeleteDialog _deleteDialog;
	}
}