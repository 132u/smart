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
	class ChangeProjectNameTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_deleteDialog = new DeleteDialog(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_newProjectName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test, Ignore("PRX-13070")]
		public void ChangeProjectNameOnNew()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickSettingsLink();

			_newProjectSettingsPage
				.FillProjectName(_newProjectName)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			Assert.IsTrue(_projectsPage.IsProjectExist(_newProjectName),
				"Произошла ошибка: \nне найден проект с новым именем");
		}

		[Test]
		public void ChangeProjectNameOnExisting()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_newProjectName)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickSettingsLink();

			_newProjectSettingsPage
				.FillProjectName(_projectUniqueName)
				.ClickNextButtonExpectingError();

			Assert.IsTrue(_newProjectSettingsPage.IsDuplicateNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилось сообщение о существующем имени");
		}

		[Test, Ignore("PRX-13070")]
		public void ChangeProjectNameOnDeleted()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_newProjectName)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickSettingsLink();

			_newProjectSettingsPage
				.FillProjectName(_projectUniqueName)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			Assert.IsTrue(_projectsPage.IsProjectExist(_newProjectName),
				"Произошла ошибка: \nне найден проект с новым именем");
		}

		private string _projectUniqueName;
		private string _newProjectName;

		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private DeleteDialog _deleteDialog;

		private NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		private NewProjectSettingsPage _newProjectSettingsPage;
		private NewProjectWorkflowPage _newProjectWorkflowPage;
	}
}
