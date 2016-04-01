using System;
using System.Globalization;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	class AssignmentDeadlineTests<TWebDriverProvider> : AssignResponsiblesBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test(Description = "ТС-140")]
		public void CancelDeadlineTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new []{ PathProvider.EditorTxtFile});

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage.OpenDatePicker();
			_datePicker.SetDate<TaskAssignmentPage>();

			_taskAssignmentPage.ClickCancelButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			Assert.AreEqual(null, _taskAssignmentPage.GetDeadlineDate(),
				"Произошла ошибка:\nНеверное значение в поле дедлайна.");
		}

		[Test(Description = "ТС-141")]
		public void SaveDeadlineTest()
		{
			var date = DateTime.Now;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { PathProvider.EditorTxtFile });

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage.OpenDatePicker();
			_datePicker.SetDate<TaskAssignmentPage>(date.Day);

			_taskAssignmentPage.ClickSaveButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			Assert.AreEqual(date.Date, _taskAssignmentPage.GetDeadlineDate(),
				"Произошла ошибка:\nНеверное значение в поле дедлайна.");
		}

		[Test(Description = "ТС-144")]
		public void SaveDeadlineInRussianLocaleTest()
		{
			var date = DateTime.Now;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { PathProvider.EditorTxtFile });

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_workspacePage.SelectLocale(Language.Russian);

			_taskAssignmentPage.OpenDatePicker();
			_datePicker.SetDate<TaskAssignmentPage>(date.Day);

			_taskAssignmentPage.ClickSaveButton();

			Assert.IsTrue(_projectsPage.IsProjectsPageOpened(),
				"Произошла ошибка:\nНе открылась страница проектов.");
		}

		[Test(Description = "ТС-15"), Ignore("PRX-16000")]
		public void DeadlineViewTest()
		{
			var nextMonth1 = false;
			var nextMonth2 = false;
			var document1 = PathProvider.EditorTxtFile;
			var document2 = PathProvider.DocumentFile;
			var document3 = PathProvider.DocumentFile2;

			var today = DateTime.Now;
			var date1 = today.AddDays(2);
			var date2 = today.AddDays(3);

			if (today.Month < date1.Month)
			{
				nextMonth1 = true;
			}

			if (today.Month < date2.Month)
			{
				nextMonth2 = true;
			}
			
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new []{ document1, document2, document3 },
				tasks: new[] { 
					WorkflowTask.Translation,
					WorkflowTask.Proofreading,
					WorkflowTask.Editing,
					WorkflowTask.Postediting},
				deadline: Deadline.FillDeadlineDate,
				deadlineDate: DateTime.Now.AddMonths(2).ToString("d", CultureInfo.InvariantCulture)
				);
			
			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage.OpenDatePicker();
			_datePicker.SetDate<TaskAssignmentPage>(date1.Day, nextMonth: nextMonth1);

			_taskAssignmentPage.OpenDatePicker(taskNumber: 2);
			_datePicker.SetDate<TaskAssignmentPage>(date1.Day, nextMonth: nextMonth1);

			_taskAssignmentPage.OpenDatePicker(taskNumber: 3);
			_datePicker.SetDate<TaskAssignmentPage>(date1.Day, nextMonth: nextMonth1);

			_taskAssignmentPage.ClickSaveButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName, documentNumber: 2);

			_taskAssignmentPage.OpenDatePicker();
			_datePicker.SetDate<TaskAssignmentPage>(date1.Day, nextMonth: nextMonth1);

			_taskAssignmentPage.OpenDatePicker(taskNumber: 2);
			_datePicker.SetDate<TaskAssignmentPage>(date1.Day, nextMonth: nextMonth1);

			_taskAssignmentPage.OpenDatePicker(taskNumber: 3);
			_datePicker.SetDate<TaskAssignmentPage>(date1.Day, nextMonth: nextMonth1);

			_taskAssignmentPage.ClickSaveButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName, documentNumber: 3);

			_taskAssignmentPage.OpenDatePicker();
			_datePicker.SetDate<TaskAssignmentPage>(date1.Day, nextMonth: nextMonth1);

			_taskAssignmentPage.OpenDatePicker(taskNumber: 2);
			_datePicker.SetDate<TaskAssignmentPage>(date2.Day, nextMonth: nextMonth2);
			
			_taskAssignmentPage.ClickSaveButton();

			_projectsPage.OpenAssignDialogForSelectedDocuments(_projectUniqueName, new[] { document1, document2, document3 });

			Assert.AreEqual(date1, _taskAssignmentPage.GetDeadlineDate(taskNumber: 1),
				"Произошла ошибка:\n Неверная дата в дедалйне для задачи №1.");

			Assert.IsTrue(_taskAssignmentPage.IsDifferentDeadlineDisplayed(taskNumber: 2),
				"Произошла ошибка:\n Для задачи №2 не отображается надпись 'Different deadlines'.");

			Assert.IsTrue(_taskAssignmentPage.IsDifferentDeadlineDisplayed(taskNumber: 3),
				"Произошла ошибка:\n Для задачи №3 не отображается надпись 'Different deadlines'.");
		}

		[Test(Description = "ТС-16")]
		public void DeadlineIsEarlierThatPreviousValidationTest()
		{
			var nextMonth1 = false;
			var nextMonth2 = false;
			var today = DateTime.Now;
			var date1 = today.AddDays(4);
			var date2 = today.AddDays(1);

			if (today.Month < date1.Month)
			{
				nextMonth1 = true;
			}

			if (today.Month < date2.Month)
			{
				nextMonth2 = true;
			}

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new []{ PathProvider.EditorTxtFile },
				tasks: new[] { 
					WorkflowTask.Translation,
					WorkflowTask.Proofreading,
					WorkflowTask.Editing},
				deadline:Deadline.NextMonth
				);

			_projectsPage.OpenAssignDialog(_projectUniqueName);
			
			_taskAssignmentPage.OpenDatePicker();
			_datePicker.SetDate<TaskAssignmentPage>(date1.Day, nextMonth: nextMonth1);

			_taskAssignmentPage.OpenDatePicker(taskNumber: 2);
			_datePicker.SetDate<TaskAssignmentPage>(date2.Day, nextMonth: nextMonth2);

			_taskAssignmentPage.ClickSaveAssignButtonExpectingError();

			Assert.IsTrue(_taskAssignmentPage.IsDeadlineIsEarlierThatPreviousError(),
				"Произошла ошибка:\n Не отображается сообщение 'The task deadline precedes the deadline for the previous task'.");
		}

		[Test(Description = "ТС-16")]
		public void DeadlineIsEqualToPreviousValidationTest()
		{
			var date = DateTime.Now;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new []{ PathProvider.EditorTxtFile },
				tasks: new[] { 
					WorkflowTask.Translation,
					WorkflowTask.Proofreading,
					WorkflowTask.Editing}
				);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage.OpenDatePicker(taskNumber: 1);
			_datePicker.SetDate<TaskAssignmentPage>(date.Day);

			_taskAssignmentPage.OpenDatePicker(taskNumber: 2);
			_datePicker.SetDate<TaskAssignmentPage>(date.Day);

			_taskAssignmentPage.ClickSaveButton();

			Assert.IsTrue(_projectsPage.IsProjectsPageOpened(),
				"Произошла ошибка:\n Не открылась страница проектов.");
		}
		
		[Test(Description = "ТС-16"), Ignore("Контрол не доделан")]
		public void DeadlineIsLaterThatProjectDeadlineCancelTest()
		{
			var date = DateTime.Now;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new []{ PathProvider.EditorTxtFile },
				deadline: Deadline.CurrentDate,
				tasks: new[] { 
					WorkflowTask.Translation,
					WorkflowTask.Proofreading,
					WorkflowTask.Editing}
				);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage.OpenDatePicker(taskNumber: 3);
			_datePicker
				.ClickNextMonthArrow(count: 12)
				.SetDate<TaskAssignmentPage>(date.Day);

			_taskAssignmentPage.ClickSaveAssignButtonExpectingError();

			Assert.IsTrue(_attentionPopup.IsAttentionPopupOpened(),
				"Произошла ошибка:\n Не открылось сообщением о том что дедлайн позже дедлайна проекта.");

			_attentionPopup.ClickCancelButton();

			Assert.IsTrue(_taskAssignmentPage.IsAttentionIconInDatepickerDisplayed(),
				"Произошла ошибка:\n Не отображается предупреждение(желтый восклицательный знак) в поле дедлайна.");
		}

		[Test(Description = "ТС-16"), Ignore("Контрол не доделан")]
		public void DeadlineIsLaterThatProjectDeadlineSaveTest()
		{
			var date = DateTime.Now.AddYears(1);

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new []{ PathProvider.EditorTxtFile },
				deadline: Deadline.CurrentDate,
				tasks: new[] { 
					WorkflowTask.Translation,
					WorkflowTask.Proofreading,
					WorkflowTask.Editing}
				);

			_projectsPage.OpenAssignDialog(_projectUniqueName);
			
			_taskAssignmentPage.OpenDatePicker(taskNumber: 3);
			_datePicker
				.ClickNextMonthArrow(count: 12)
				.SetDate<TaskAssignmentPage>(date.Day);

			_taskAssignmentPage.ClickSaveAssignButtonExpectingError();

			Assert.IsTrue(_attentionPopup.IsAttentionPopupOpened(),
				"Произошла ошибка:\n Не открылось сообщением о том что дедлайн позже дедлайна проекта.");

			_attentionPopup.ClickSaveButton();

			Assert.IsTrue(_projectsPage.IsProjectsPageOpened(), "Произошла ошибка:\n Не открылась страница проектов.");

			_projectsPage.OpenAssignDialog(_projectUniqueName);
			
			Assert.AreEqual(date.Date, _taskAssignmentPage.GetDeadlineDate(taskNumber: 3),
				"Произошла ошибка:\n Неверное значение даты в поле дедлайн.");

			Assert.IsTrue(_taskAssignmentPage.IsAttentionIconInDatepickerDisplayed(),
				"Произошла ошибка:\n Не отображается предупреждение(желтый восклицательный знак) в поле дедлайна.");
		}
	}
}
