using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	class BasicProjectPersonalAccountTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public BasicProjectPersonalAccountTests()
		{
			StartPage = StartPage.PersonalAccount;
		}

		[SetUp]
		public void SetUpBasicProjectPersonalAccountTests()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);
			_workspaceHelper.GoToProjectsPage();
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog(Driver);
		}


		[Test]
		public void CreateProjectNoFileTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName, personalAccount: true);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", projectUniqueName);
		}

		[Test]
		public void CreateProjectDuplicateNameTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName, personalAccount: true);

			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(projectUniqueName)
				.ClickNextButton<NewProjectGeneralInformationDialog>();

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsDuplicateNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилось сообщение о существующем имени");
		}

		[Test]
		public void CreateProjectLongNameTest()
		{
			var longProjectUniqueName = _createProjectHelper.GetProjectUniqueName() + _longName;

			_createProjectHelper.CreateNewProject(longProjectUniqueName, personalAccount: true);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(longProjectUniqueName.Substring(0, 100)),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", longProjectUniqueName.Substring(0, 100));
		}

		[Test]
		public void AssignTaskButtonTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(projectUniqueName, personalAccount: true)
				.GoToProjectSettingsPage(projectUniqueName)
				.UploadDocument(PathProvider.DocumentFile)
				.ClickDocumentProgress(Path.GetFileName(PathProvider.DocumentFile))
				.AssertAssignButtonNotDisplayed();
		}

		[Test]
		public void DeleteDocumentFromProjectTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(projectUniqueName, personalAccount: true)
				.GoToProjectSettingsPage(projectUniqueName)
				.UploadDocument(PathProvider.DocumentFile)
				.DeleteDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
		}

		[Test]
		public void CancelCreateProjectOnFirstStepTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog.ClickCancelLink();
		}

		private CreateProjectHelper _createProjectHelper;
		private const string _longName = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
		private WorkspaceHelper _workspaceHelper;
		private ProjectsPage _projectsPage;
		private NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog;
	}
}
