using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Projects]
	class CreateProjectSaveSettingsByBackTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Standalone]
		public void BackFromWorkflowStepTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(
					_projectUniqueName,
					sourceLanguage: Language.Japanese,
					targetLanguages: new []{ Language.Lithuanian },
					deadline: Deadline.CurrentDate)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickSettingsLink();

			Assert.IsTrue(_newProjectSettingsPage.IsProjectNameMatchExpected(_projectUniqueName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");

			Assert.AreEqual(DateTime.Now.Date, _newProjectSettingsPage.GetDeadlineDate(),
				"Произошла ошибка:\n в дэдлайне указана неверная дата");

			Assert.IsTrue(_newProjectSettingsPage.IsSourceLanguageMatchExpected(Language.Japanese),
				"Произошла ошибка:\n в сорс-языке указан неправильный язык");

			Assert.IsTrue(_newProjectSettingsPage.IsTargetLanguageMatchExpected(Language.Lithuanian),
				"Произошла ошибка:\n в таргет-языке указан неправильный язык");
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
	}
}
