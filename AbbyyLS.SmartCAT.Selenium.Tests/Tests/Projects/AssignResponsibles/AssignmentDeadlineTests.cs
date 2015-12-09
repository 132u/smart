using System;
using System.Globalization;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	class AssignmentDeadlineTests<TWebDriverProvider> : AssignResponsiblesBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test(Description = "ТС-140")]
		public void CancelDeadlineTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.FillDeadlineManually(DateTime.Now.ToString("d", CultureInfo.InvariantCulture))
				.ClickCancelButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			Assert.AreEqual(null, _taskAssignmentPage.GetDeadlineDate(),
				"Произошла ошибка:\nНеверное значение в поле дедлайна.");
		}

		[Test(Description = "ТС-141")]
		public void SaveDeadlineTest()
		{
			var date = DateTime.Now;

			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.FillDeadlineManually(date.ToString("d", CultureInfo.InvariantCulture))
				.ClickSaveButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			Assert.AreEqual(date.Date, _taskAssignmentPage.GetDeadlineDate(),
				"Произошла ошибка:\nНеверное значение в поле дедлайна.");
		}

		[Test(Description = "ТС-143")]
		[TestCase("123123123")]
		[TestCase("123-123-123")]
		[TestCase("90.90.90")]
		[TestCase("12/12/17")]
		[TestCase("12.12.2017")]
		[TestCase("12.12.17")]
		public void SaveInvalidDeadlineTest(string deadline)
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.FillInvalidDeadlineManually(deadline)
				.ClickSaveAssignButtonExpectingError();

			Assert.IsTrue(_taskAssignmentPage.IsWrongDeadlineFormatErrorMessage(),
				"Произошла ошибка:\nСообщение 'Please, specify the deadline in the format: MM/DD/YYYY' не появилось.");
		}

		[Test(Description = "ТС-144")]
		public void SaveDeadlineInRussianLocaleTest()
		{
			var date = DateTime.Now.ToString("d");

			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_workspaceHelper.SelectLocale(Language.Russian);

			_taskAssignmentPage
				.FillDeadlineManually(date)
				.ClickSaveButton();

			Assert.IsTrue(_projectsPage.IsProjectsPageOpened(),
				"Произошла ошибка:\nНе открылась страница проектов.");
		}

		[Test(Description = "ТС-144")]
		[TestCase("123123123")]
		[TestCase("123-123-123")]
		[TestCase("90.90.90")]
		[TestCase("12.12.17")]
		[TestCase("12/12/17")]
		[TestCase("12/12/2017")]
		public void SaveInvalidDeadlineInRussianLocaleTest(string deadline)
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile);
			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_workspaceHelper.SelectLocale(Language.Russian);

			_taskAssignmentPage
				.FillInvalidDeadlineManually(deadline)
				.ClickSaveAssignButtonExpectingError();

			Assert.IsTrue(_taskAssignmentPage.IsWrongDeadlineFormatErrorMessage(),
				"Произошла ошибка:\nСообщение 'Please, specify the deadline in the format: MM/DD/YYYY' не появилось.");
		}

		[Test(Description = "ТС-15")]
		public void DeadlineViewTest()
		{
			var document1 = PathProvider.EditorTxtFile;
			var document2 = PathProvider.DocumentFile;
			var document3 = PathProvider.DocumentFile2;

			var date1 = DateTime.Now.AddDays(2);
			var date2 = DateTime.Now.AddDays(3);

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filePath: document1,
				tasks: new[] { 
					WorkflowTask.Translation,
					WorkflowTask.Proofreading,
					WorkflowTask.Editing,
					WorkflowTask.Postediting},
				deadline:Deadline.NextMonth
				);

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.ClickDocumentUploadButton()
				.UploadDocument(new[] { document2, document3 });

			_documentUploadGeneralInformationDialog
				.ClickFinish<ProjectSettingsPage>()
				.WaitUntilUploadDocumentDialogDissapeared()
				.WaitUntilDocumentProcessed();

			_workspaceHelper.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.FillDeadlineManually(date1.ToString("d", CultureInfo.InvariantCulture), taskNumber: 1)
				.FillDeadlineManually(date1.ToString("d", CultureInfo.InvariantCulture), taskNumber: 2)
				.FillDeadlineManually(date1.ToString("d", CultureInfo.InvariantCulture), taskNumber: 3)
				.ClickSaveButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName, documentNumber: 2);

			_taskAssignmentPage
				.FillDeadlineManually(date1.ToString("d", CultureInfo.InvariantCulture), taskNumber: 1)
				.FillDeadlineManually(date1.ToString("d", CultureInfo.InvariantCulture), taskNumber: 2)
				.FillDeadlineManually(date1.ToString("d", CultureInfo.InvariantCulture), taskNumber: 3)
				
				.ClickSaveButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName, documentNumber: 3);

			_taskAssignmentPage
				.FillDeadlineManually(date1.ToString("d", CultureInfo.InvariantCulture), taskNumber: 1)
				.FillDeadlineManually(date2.ToString("d", CultureInfo.InvariantCulture), taskNumber: 2)
				.ClickSaveButton();

			_projectsPage.OpenAssignDialogForSelectedDocuments(_projectUniqueName, new[] { document1, document2, document3 });
				
			Assert.AreEqual(date1.Date, _taskAssignmentPage.GetDeadlineDate(taskNumber: 1),
				"Произошла ошибка:\n Неверная дата в дедалйне для задачи №1.");

			Assert.IsTrue(_taskAssignmentPage.IsDifferentDeadlineDisplayed(taskNumber: 2),
				"Произошла ошибка:\n Для задачи №2 не отображается надпись 'Different deadlines'.");

			Assert.IsTrue(_taskAssignmentPage.IsDifferentDeadlineDisplayed(taskNumber: 3),
				"Произошла ошибка:\n Для задачи №3 не отображается надпись 'Different deadlines'.");
		}

		[Test(Description = "ТС-16")]
		public void DeadlineIsEarlierThatPreviousValidationTest()
		{
			var date1 = DateTime.Now.AddDays(4).ToString("d", CultureInfo.InvariantCulture);
			var date2 = DateTime.Now.AddDays(1).ToString("d", CultureInfo.InvariantCulture);

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filePath: PathProvider.EditorTxtFile,
				tasks: new[] { 
					WorkflowTask.Translation,
					WorkflowTask.Proofreading,
					WorkflowTask.Editing},
				deadline:Deadline.NextMonth
				);

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_workspaceHelper.GoToProjectsPage();
			
			_projectsPage.OpenAssignDialog(_projectUniqueName);
			
			_taskAssignmentPage
				.FillDeadlineManually(date1, taskNumber: 1)
				.FillDeadlineManually(date2, taskNumber: 2)
				.ClickSaveAssignButtonExpectingError();

			Assert.IsTrue(_taskAssignmentPage.IsDeadlineIsEarlierThatPreviousError(),
				"Произошла ошибка:\n Не отображается сообщение 'The task deadline precedes the deadline for the previous task'.");
		}

		[Test(Description = "ТС-16")]
		public void DeadlineIsEqualToPreviousValidationTest()
		{
			var date = DateTime.Now.ToString("d", CultureInfo.InvariantCulture);

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filePath: PathProvider.EditorTxtFile,
				tasks: new[] { 
					WorkflowTask.Translation,
					WorkflowTask.Proofreading,
					WorkflowTask.Editing}
				);

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_workspaceHelper.GoToProjectsPage();
			
			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.FillDeadlineManually(date, taskNumber: 1)
				.FillDeadlineManually(date, taskNumber: 2)
				.ClickSaveButton();

			Assert.IsTrue(_projectsPage.IsProjectsPageOpened(),
				"Произошла ошибка:\n Не открылась страница проектов.");
		}

		[Test(Description = "ТС-16")]
		public void DeadlineIsLaterThatProjectDeadlineCancelTest()
		{
			var date = DateTime.Now.AddYears(1).ToString("d", CultureInfo.InvariantCulture);

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filePath: PathProvider.EditorTxtFile,
				deadline: Deadline.CurrentDate,
				tasks: new[] { 
					WorkflowTask.Translation,
					WorkflowTask.Proofreading,
					WorkflowTask.Editing}
				);

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_workspaceHelper.GoToProjectsPage();
			
			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.FillDeadlineManually(date, taskNumber: 3)
				.ClickSaveAssignButtonExpectingError();

			Assert.IsTrue(_attentionPopup.IsAttentionPopupOpened(),
				"Произошла ошибка:\n Не открылось сообщением о том что дедлайн позже дедлайна проекта.");

			_attentionPopup.ClickCancelButton();

			Assert.IsTrue(_taskAssignmentPage.IsAttentionIconInDatepickerDisplayed(),
				"Произошла ошибка:\n Не отображается предупреждение(желтый восклицательный знак) в поле дедлайна.");
		}

		[Test(Description = "ТС-16")]
		public void DeadlineIsLaterThatProjectDeadlineSaveTest()
		{
			var date = DateTime.Now.AddYears(1);

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filePath: PathProvider.EditorTxtFile,
				deadline: Deadline.CurrentDate,
				tasks: new[] { 
					WorkflowTask.Translation,
					WorkflowTask.Proofreading,
					WorkflowTask.Editing}
				);

			_workspaceHelper.GoToProjectSettingsPage(_projectUniqueName);

			_workspaceHelper.GoToProjectsPage();
			
			_projectsPage.OpenAssignDialog(_projectUniqueName);
			
			_taskAssignmentPage
				.FillDeadlineManually(date.ToString("d", CultureInfo.InvariantCulture), taskNumber: 3)
				.ClickSaveAssignButtonExpectingError();

			Assert.IsTrue(_attentionPopup.IsAttentionPopupOpened(),
				"Произошла ошибка:\n Не открылось сообщением о том что дедлайн позже дедлайна проекта.");

			_attentionPopup
				.ClickSaveButton()
				.WaitUntilDialogBackgroundDisappeared();

			Assert.IsTrue(_projectsPage.IsProjectsPageOpened(), "Произошла ошибка:\n Не открылась страница проектов.");

			_projectsPage.OpenAssignDialog(_projectUniqueName);
			
			Assert.AreEqual(date.Date, _taskAssignmentPage.GetDeadlineDate(taskNumber: 3),
				"Произошла ошибка:\n Неверное значение даты в поле дедлайн.");

			Assert.IsTrue(_taskAssignmentPage.IsAttentionIconInDatepickerDisplayed(),
				"Произошла ошибка:\n Не отображается предупреждение(желтый восклицательный знак) в поле дедлайна.");
		}
	}
}
