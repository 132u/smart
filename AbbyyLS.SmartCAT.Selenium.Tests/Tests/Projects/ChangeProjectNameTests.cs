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
			_newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog(Driver);
			_newProjectSetUpWorkflowDialog = new NewProjectSetUpWorkflowDialog(Driver);

			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_newProjectName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test]
		public void ChangeProjectNameOnNew()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.ClickBackButton<NewProjectGeneralInformationDialog>();

			_newProjectGeneralInformationDialog
				.FillProjectName(_newProjectName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();
		}

		[Test]
		public void ChangeProjectNameOnExisting()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_newProjectName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.ClickBackButton<NewProjectGeneralInformationDialog>();

			_newProjectGeneralInformationDialog
				.FillProjectName(_projectUniqueName)
				.ClickNextButton<NewProjectGeneralInformationDialog>();

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsDuplicateNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилось сообщение о существующем имени");
		}

		[Test]
		public void ChangeProjectNameOnDeleted()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_newProjectName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.ClickBackButton<NewProjectGeneralInformationDialog>();

			_newProjectGeneralInformationDialog
				.FillProjectName(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();
		}

		private string _projectUniqueName;
		private string _newProjectName;

		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private DeleteDialog _deleteDialog;
		private NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog;
		private NewProjectSetUpWorkflowDialog _newProjectSetUpWorkflowDialog;
	}
}
