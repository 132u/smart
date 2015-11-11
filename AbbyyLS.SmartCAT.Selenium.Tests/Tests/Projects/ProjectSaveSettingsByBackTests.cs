using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ProjectSaveSettingsByBackTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_newProjectSelectGlossariesDialog = new NewProjectSelectGlossariesDialog(Driver);
			_newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog(Driver);
			_newProjectSetUpTMDialog = new NewProjectSetUpTMDialog(Driver);
			_newProjectSetUpPretranslationDialog = new NewProjectSetUpPretranslationDialog(Driver);
			_newProjectSetUpWorkflowDialog = new NewProjectSetUpWorkflowDialog(Driver);

			_projectsPage = new ProjectsPage(Driver);
			_workspaceHelper.GoToProjectsPage();
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test, Standalone]
		public void BackFromWorkflowStepTest()
		{
			var deadlineDate = "11/28/2015";

			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(
					_projectUniqueName,
					sourceLanguage: Language.Japanese,
					targetLanguage: Language.Lithuanian,
					deadline: Deadline.FillDeadlineDate,
					date: deadlineDate)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.ClickBackButton<NewProjectGeneralInformationDialog>();

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsProjectNameMatchExpected(_projectUniqueName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsDeadlineDateMatchExpected(deadlineDate),
				"Произошла ошибка:\n в дэдлайне указана неверная дата");

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsSourceLanguageMatchExpected(Language.Japanese),
				"Произошла ошибка:\n в сорс-языке указан неправильный язык");

			Assert.IsTrue(_newProjectGeneralInformationDialog.IsTargetLanguageMatchExpected(Language.Lithuanian),
				"Произошла ошибка:\n в таргет-языке указан неправильный язык");
		}

		[Test, Standalone]
		public void BackFromGlossarySetUpStepTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.ClickNextButton<NewProjectSetUpTMDialog>();

			_newProjectSetUpTMDialog
				.SelectFirstTM()
				.ClickNextButton<NewProjectSelectGlossariesDialog>();

			_newProjectSelectGlossariesDialog.ClickBackButton<NewProjectSetUpTMDialog>();

			Assert.IsTrue(_newProjectSetUpTMDialog.IsFirstTMSelected(),
				"Произошла ошибка:\n первая ТМ не выбрана");
		}

		[Test, Standalone]
		public void BackFromWorkflowToTMStepTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.ClickNextButton<NewProjectSetUpTMDialog>();

			_newProjectSetUpTMDialog
				.SelectFirstTM()
				.ClickBackButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.ClickNextButton<NewProjectSetUpTMDialog>();

			Assert.IsTrue(_newProjectSetUpTMDialog.IsFirstTMSelected(),
				"Произошла ошибка:\n первая ТМ не выбрана");
		}

		[Test, Standalone]
		public void BackFormPretranslateStepTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.ClickNextButton<NewProjectSetUpTMDialog>();

			_newProjectSetUpTMDialog
				.SelectFirstTM()
				.ClickNextButton<NewProjectSelectGlossariesDialog>();

			_newProjectSelectGlossariesDialog
				.ClickFirstGlossary()
				.ClickNextButton<NewProjectSetUpPretranslationDialog>();

			_newProjectSetUpPretranslationDialog.ClickBackButton<NewProjectSelectGlossariesDialog>();

			Assert.IsTrue(_newProjectSelectGlossariesDialog.IsFirstGlossarySelected(),
				"Произошла ошибка:\n первый глоссарий не выбран");
		}

		[Test, Standalone]
		public void BackToGlossaryStepFromTMStepTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog.ClickNextButton<NewProjectSetUpTMDialog>();

			_newProjectSetUpTMDialog
				.SelectFirstTM()
				.ClickNextButton<NewProjectSelectGlossariesDialog>();

			_newProjectSelectGlossariesDialog
				.ClickFirstGlossary()
				.ClickBackButton<NewProjectSetUpTMDialog>();

			_newProjectSetUpTMDialog.ClickNextButton<NewProjectSelectGlossariesDialog>();

			Assert.IsTrue(_newProjectSelectGlossariesDialog.IsFirstGlossarySelected(),
				"Произошла ошибка:\n первый глоссарий не выбран");
		}

		[Test]
		public void BackToChooseWorkflowTaskStepFromGeneraProjectInformationTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			_newProjectSetUpWorkflowDialog
				.SelectWorkflowTask(WorkflowTask.Editing)
				.ClickBackButton<NewProjectGeneralInformationDialog>();

			_newProjectGeneralInformationDialog.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			Assert.IsTrue(_newProjectSetUpWorkflowDialog.IsWorkflowTaskMatchExpected(
				WorkflowTask.Editing, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");
		}

		private string _projectUniqueName;

		private CreateProjectHelper _createProjectHelper;
		private WorkspaceHelper _workspaceHelper;
		private ProjectsPage _projectsPage;
		private NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog;
		private NewProjectSelectGlossariesDialog _newProjectSelectGlossariesDialog;
		private NewProjectSetUpTMDialog _newProjectSetUpTMDialog;
		private NewProjectSetUpPretranslationDialog _newProjectSetUpPretranslationDialog;
		private NewProjectSetUpWorkflowDialog _newProjectSetUpWorkflowDialog;
	}
}
