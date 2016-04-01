using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	class CreateProjectSaveSettingsByBackTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_newProjectSelectGlossariesDialog = new NewProjectSelectGlossariesDialog(Driver);
			_newProjectSetUpTMDialog = new NewProjectSetUpTMDialog(Driver);
			_newProjectSetUpPretranslationDialog = new NewProjectSetUpPretranslationDialog(Driver);
		}

		[Test, Standalone]
		public void BackFromWorkflowStepTest()
		{
			var deadlineDate = "11/28/2015";

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(
					_projectUniqueName,
					sourceLanguage: Language.Japanese,
					targetLanguage: Language.Lithuanian,
					deadline: Deadline.FillDeadlineDate,
					date: deadlineDate)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickSettingsLink();

			Assert.IsTrue(_newProjectSettingsPage.IsProjectNameMatchExpected(_projectUniqueName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");

			Assert.AreEqual(deadlineDate, _newProjectSettingsPage.GetDeadlineDate(),
				"Произошла ошибка:\n в дэдлайне указана неверная дата");

			Assert.IsTrue(_newProjectSettingsPage.IsSourceLanguageMatchExpected(Language.Japanese),
				"Произошла ошибка:\n в сорс-языке указан неправильный язык");

			Assert.IsTrue(_newProjectSettingsPage.IsTargetLanguageMatchExpected(Language.Lithuanian),
				"Произошла ошибка:\n в таргет-языке указан неправильный язык");
		}

		[Test, Standalone, Ignore("В новом визарде нет отдельной страницы на глоссарии")]
		public void BackFromGlossarySetUpStepTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickNextButton<NewProjectSetUpTMDialog>();

			_newProjectSetUpTMDialog
				.SelectFirstTM()
				.ClickNextButton<NewProjectSelectGlossariesDialog>();

			_newProjectSelectGlossariesDialog.ClickBackButton<NewProjectSetUpTMDialog>();

			Assert.IsTrue(_newProjectSetUpTMDialog.IsFirstTMSelected(),
				"Произошла ошибка:\n первая ТМ не выбрана");
		}

		[Test, Standalone, Ignore("В новом визарде нет отдельной страницы на ТМ")]
		public void BackFromWorkflowToTMStepTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickNextButton<NewProjectSetUpTMDialog>();

			_newProjectSetUpTMDialog
				.SelectFirstTM()
				.ClickBackButton<NewProjectSetUpWorkflowDialog>();

			_newProjectWorkflowPage.ClickNextButton<NewProjectSetUpTMDialog>();

			Assert.IsTrue(_newProjectSetUpTMDialog.IsFirstTMSelected(),
				"Произошла ошибка:\n первая ТМ не выбрана");
		}

		[Test, Standalone, Ignore("В новом визарде нет отдельной страницы на Pretranslate")]
		public void BackFormPretranslateStepTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickNextButton<NewProjectSetUpTMDialog>();

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

		[Test, Standalone, Ignore("В новом визарде нет отдельной страницы на ТМ и глоссарии")]
		public void BackToGlossaryStepFromTMStepTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickNextButton<NewProjectSetUpTMDialog>();

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

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton();

			_newProjectWorkflowPage
				.ClickClearButton()
				.ClickNewTaskButton(WorkflowTask.Editing)
				.ClickSettingsLink();

			_newProjectSettingsPage.ClickNextButton();

			Assert.IsTrue(_newProjectWorkflowPage.IsWorkflowTaskMatchExpected(
				WorkflowTask.Editing, taskNumber: 1), "Произошла ошибка:\n задача не соответствует ожидаемой");
		}

		private NewProjectSelectGlossariesDialog _newProjectSelectGlossariesDialog;
		private NewProjectSetUpTMDialog _newProjectSetUpTMDialog;
		private NewProjectSetUpPretranslationDialog _newProjectSetUpPretranslationDialog;
	}
}
