using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	class InitialProjectTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public InitialProjectTests()
		{
			GlobalSetup.SetUp();
			StartPage = StartPage.Workspace;
		}

		[SetUp]
		public void SeInitialProjectTests()
		{
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
		}

		[Test, Category("Project tests")]
		public void CreateFirstProject()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsNewProjectDocumentUploadPageOpened(),
				"Произошла ошибка:\nНе открылась первая страница визарда создания проекта.");

			_newProjectDocumentUploadPage
				.UploadDocumentFile(PathProvider.DocumentFile)
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(
					projectUniqueName,
					sourceLanguage: Language.English,
					targetLanguage: Language.Russian)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			Assert.IsTrue(_projectsPage.IsProjectExist(projectUniqueName),
				"Произошла ошибка: \nне найден проект с новым именем");
		}

		private LoginHelper _loginHelper;
		private NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		private NewProjectSettingsPage _newProjectSettingsPage;
		private CreateProjectHelper _createProjectHelper;
		private NewProjectWorkflowPage _newProjectWorkflowPage;
		private ProjectsPage _projectsPage;
	}
}
