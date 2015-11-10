using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
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
		}

		[Test]
		public void ChangeProjectNameOnNew()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			var newProjectName = _createProjectHelper.GetProjectUniqueName();

			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickBackButtonOnWorkflowStep()
				.FillProjectName(newProjectName)
				.ClickNextOnGeneralProjectInformationPage();
		}

		[Test]
		public void ChangeProjectNameOnExisting()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			var newProjectName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName);
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(newProjectName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickBackButtonOnWorkflowStep()
				.FillProjectName(projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage(errorExpected: true)
				.AssertErrorDuplicateName();
		}

		[Test]
		public void ChangeProjectNameOnDeleted()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			var newProjectName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName);

			_projectsPage
				.ClickProjectCheckboxInList(projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(newProjectName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickBackButtonOnWorkflowStep()
				.FillProjectName(projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage();
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private DeleteDialog _deleteDialog;
	}
}
