using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DriversAndSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[TestFixture]
	[PriorityMajor]
	[Standalone]
	class ChangeProjectNameTests<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			QuitDriverAfterTest = true;
			_createProjectHelper = new CreateProjectHelper();
		}

		[Test]
		public void ChangeProjectNameOnNew()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			var newProjectName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.ClickCreateProjectButton()
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

			_createProjectHelper
				.CreateNewProject(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.ClickCreateProjectButton()
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

			_createProjectHelper
				.CreateNewProject(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.SelectProjectInList(projectUniqueName)
				.DeleteProjectFromList()
				.AssertProjectSuccessfullyDeleted(projectUniqueName)
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(newProjectName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickBackButtonOnWorkflowStep()
				.FillProjectName(projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage();
		}

		private CreateProjectHelper _createProjectHelper;
	}
}
