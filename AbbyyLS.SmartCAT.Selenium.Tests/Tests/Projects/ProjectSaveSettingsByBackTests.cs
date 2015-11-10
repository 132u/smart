﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
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
			_projectsPage = new ProjectsPage(Driver);
			_workspaceHelper.GoToProjectsPage();
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test, Standalone]
		public void BackFromWorkflowStepTest()
		{
			var deadlineDate = "11/28/2015";

			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(
					_projectUniqueName,
					sourceLanguage: Language.Japanese,
					targetLanguage: Language.Lithuanian,
					deadline: Deadline.FillDeadlineDate,
					date: deadlineDate)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickBackButtonOnWorkflowStep()
				.AssertProjectNameMatch(_projectUniqueName)
				.AssertDeadlineDateMatch(deadlineDate)
				.AssertSourceLanguageMatch(Language.Japanese)
				.AssertTargetLanguageMatch(Language.Lithuanian);
		}

		[Test, Standalone]
		public void BackFromGlossarySetUpStepTest()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNextOnWorkflowPage()
				.SelectFirstTM()
				.ClickNextButtonOnTMStep()
				.ClickBackButtonOnGlossaryStep()
				.AssertFirstTMSelected();
		}

		[Test, Standalone]
		public void BackFromWorkflowToTMStepTest()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNextOnWorkflowPage()
				.SelectFirstTM()
				.ClickBackButtonOnTMStep()
				.ClickNextOnWorkflowPage()
				.AssertFirstTMSelected();
		}

		[Test, Standalone]
		public void BackFormPretranslateStepTest()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNextOnWorkflowPage()
				.SelectFirstTM()
				.ClickNextButtonOnTMStep()
				.SelectFirstGlossary()
				.ClickNextOnSelectGlossaryStep()
				.ClickBackButtonOnPreranslationStep()
				.AssertFirstGlossarySelected();
		}

		[Test, Standalone]
		public void BackToGlossaryStepFromTMStepTest()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNextOnWorkflowPage()
				.SelectFirstTM()
				.ClickNextButtonOnTMStep()
				.SelectFirstGlossary()
				.ClickBackButtonOnGlossaryStep()
				.ClickNextButtonOnTMStep()
				.AssertFirstGlossarySelected();
		}

		[Test]
		public void BackToChooseWorkflowTaskStepFromGeneraProjectInformationTest()
		{
			_projectsPage.ClickCreateProjectDialog();
			_createProjectHelper
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.SelectWorkflowTask(WorkflowTask.Editing)
				.ClickBackButtonOnWorkflowStep()
				.ClickNextOnGeneralProjectInformationPage()
				.AssertWorkflowTaskMatch(WorkflowTask.Editing, taskNumber: 1);
		}

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper;
		private WorkspaceHelper _workspaceHelper;
		private ProjectsPage _projectsPage;
	}
}
