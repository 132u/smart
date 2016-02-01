using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	class ChangeProjectNameTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			_newProjectName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test, Ignore("PRX-14306")]
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

		[Test, Ignore("PRX-14306")]
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

		private string _newProjectName;
	}
}
