using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class TaskAssignmentPage : ProjectsPage, IAbstractPage<TaskAssignmentPage>
	{
		public TaskAssignmentPage(WebDriver driver) : base(driver)
		{
		}

		public new TaskAssignmentPage GetPage()
		{
			var taskAssignmentDialog = new TaskAssignmentPage(Driver);
			InitPage(taskAssignmentDialog, Driver);

			return taskAssignmentDialog;
		}

		public new void LoadPage()
		{
			if (!IsTaskAssignmentPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось открыть страницу назначения задач");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Открыть выпадющий список для задачи с заданным номером
		/// </summary>
		/// <param name="taskNumber"> номер задачи</param>
		public TaskAssignmentPage OpenAssigneeDropbox(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Открыть выпадающий список для задачи с номером строки {0}", taskNumber);
			AssigneeDropbox = Driver.SetDynamicValue(How.XPath, TASK_ASSIGN_DROPDOWN, taskNumber.ToString());
			AssigneeDropbox.Click();

			return GetPage();
		}
		
		/// <summary>
		/// Получить список исполнителей
		/// </summary>
		public List<string> GetResponsibleUsersList()
		{
			CustomTestContext.WriteLine("Получить список исполнителей.");

			var elementUsersList = Driver.GetTextListElement(By.XPath(ASSIGNEE_LIST));

			return (from element in elementUsersList
					where !element.Contains("Group: ")
					select element).ToList();
		}

		/// <summary>
		/// Получить список групп исполнителей
		/// </summary>
		public List<string> GetResponsibleGroupsList()
		{
			CustomTestContext.WriteLine("Получить список групп исполнителей.");

			var elementUsersList = Driver.GetTextListElement(By.XPath(ASSIGNEE_LIST));

			return (from element in elementUsersList
					where element.Contains("Group: ")
					select element).ToList();
		}

		/// <summary>
		/// Нажать кнопку 'Назначить'
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public TaskAssignmentPage ClickAssignButton(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Назначить'");

			AssignButton = Driver.SetDynamicValue(How.XPath, ASSIGN_BUTTON, taskNumber.ToString());
			AssignButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку подтверждения удаления назначения пользователя
		/// </summary>
		public TaskAssignmentPage ClickConfirmCancelButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку подтверждения удаления назначения пользователя");

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_CANCEL_BUTTON)))
			{
				throw new XPathLookupException("Не появилась кнопка подтверждения удаления назначения пользователя");
			}

			ConfirmCancelButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку отмены назначения исполнителя задачи
		/// </summary>
		/// <param name="taskNumber"> номер задачи </param>
		public TaskAssignmentPage ClickCancelAssignButton(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать кнопку отмены назначения исполнителя для задачи с номером '{0}'", taskNumber);

			CancelAssignButton = Driver.SetDynamicValue(How.XPath, CANCEL_ASSIGN_BUTTON, taskNumber.ToString());
			CancelAssignButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения исполнителя задачи
		/// </summary>
		public ProjectsPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения исполнителя задачи");
			Thread.Sleep(1000);
			SaveButton.Click();

			return new ProjectsPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения исполнителя задачи
		/// </summary>
		public ProjectSettingsPage ClickSaveButtonProjectSettingsPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения исполнителя задачи");
			Thread.Sleep(1000);
			SaveButton.Click();

			return new ProjectSettingsPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения исполнителя задачи, ожидается валидационная ошибка
		/// </summary>
		public TaskAssignmentPage ClickSaveAssignButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения исполнителя задачи, ожидается валидационная ошибка.");
			Thread.Sleep(1000);
			SaveButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка назначения отображается для задачи
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public bool IsAssignButtonDisplayed(int taskNumber)
		{
			CustomTestContext.WriteLine("Проверить, что кнопка назначения отображается для задачи №{0}.", taskNumber);

			return Driver.SetDynamicValue(How.XPath, ASSIGN_BUTTON, taskNumber.ToString()).Displayed;
		}

		/// <summary>
		/// Кликнуть кнопку Cancel.
		/// </summary>
		public ProjectsPage ClickCancelButton()
		{
			CustomTestContext.WriteLine("Кликнуть кнопку Cancel.");
			Thread.Sleep(1000);
			CancelButton.Click();

			return new ProjectsPage(Driver).GetPage();
		}
		
		/// <summary>
		/// Ввести дату вручную.
		/// </summary>
		/// <param name="dateTime">дата</param>
		public TaskAssignmentPage FillDeadlineManually(string dateTime, int taskNumber = 1 )
		{
			CustomTestContext.WriteLine("Ввести дату {0} вручную.", dateTime);
			Driver.SetDynamicValue(How.XPath, DEADLINE, taskNumber.ToString()).SetText(dateTime);
			CloseDeadlineCalendar();

			return GetPage();
		}

		/// <summary>
		/// Раскрыть календарь дедлайна.
		/// </summary>
		public TaskAssignmentPage OpenDeadlineDatepicker()
		{
			CustomTestContext.WriteLine("Раскрыть календарь дедлайна.");
			Deadline.Click();
			
			return GetPage();
		}

		/// <summary>
		/// Нажать на пустую область в футуре, чтоб закрыть календарь дедлайна.
		/// </summary>
		public TaskAssignmentPage CloseDeadlineCalendar()
		{
			CustomTestContext.WriteLine("Нажать на пустую область в футуре, чтоб закрыть календарь дедлайна.");
			Footer.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения исполнителя задачи
		/// </summary>
		public ProjectSettingsPage ClickSaveAssignButtonProjectSettingPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения исполнителя задачи");
			SaveButton.Click();

			return new ProjectSettingsPage(Driver).GetPage();

		}

		/// <summary>
		/// Получить имя из колонки Assignnees.
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public string GetAssignneeName(int taskNumber)
		{
			CustomTestContext.WriteLine("Получить имя из колонки Assignnees.");

			return Driver.SetDynamicValue(How.XPath, ASSIGNEE_NAME, taskNumber.ToString()).Text;
		}

		/// <summary>
		/// Получить значение из колонки Assignnees.
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public string GetAssignneeColumn(int taskNumber)
		{
			CustomTestContext.WriteLine("Получить текст из колонки Assignnees.");

			return Driver.SetDynamicValue(How.XPath, ASSIGNEES_COLUMN, taskNumber.ToString()).Text;
		}
		
		/// <summary>
		/// Раскрыть 'Select Assignees' дропдаун.
		/// </summary>
		/// <param name="taskNumber">номер таски</param>
		public TaskAssignmentPage ExpandSelectAssigneesDropdown(int taskNumber)
		{
			CustomTestContext.WriteLine("Раскрыть 'Select Assignees' дропдаун.");
			var assigneesDropdownForTask = Driver.SetDynamicValue(How.XPath, SELECT_ASSIGNEES_DROPDOWN, taskNumber.ToString());
			assigneesDropdownForTask.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать тип назначения 'На весь документ' 
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public SelectAssigneePage SelectAssignmentForEntireDocumentType(int taskNumber)
		{
			CustomTestContext.WriteLine("Выбрать тип назначения 'На весь документ' для задачи №{0}.", taskNumber);
			Driver.FindElement(By.XPath(SIMPLE_ASSIGNMENT_OPTION.Replace("*#*", taskNumber.ToString()))).Click();
		
			return new SelectAssigneePage(Driver).GetPage();
		}

		/// <summary>
		/// Выбрать тип назначения 'Распределить сегменты между исполнителями'
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public DistributeDocumentBetweenAssigneesPage SelectDistributeDocumentAssignmentType(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Выбрать тип назначения 'Распределить сегменты между исполнителями' для задачи №{0}.", taskNumber);
			Driver.FindElement(By.XPath(SPLIT_ASSIGNMENT_OPTION.Replace("*#*", taskNumber.ToString()))).Click();

			return new DistributeDocumentBetweenAssigneesPage(Driver).GetPage();
		}

		/// <summary>
		/// Выбрать из выпадающего списка пользователя или группу по имени
		/// </summary>
		/// <param name="name">Имя пользователя или группы</param>
		/// <param name="isGroup">Выбор группы</param>
		public TaskAssignmentPage SelectResponsible(string name, bool isGroup)
		{
			CustomTestContext.WriteLine("Выбрать из выпадающего списка {0}. Это группа: {1}", name, isGroup);

			var fullName = isGroup ? "Group: " + name : name;

			if (!IsUserInAssigneeOptionsExist(fullName))
			{
				throw new XPathLookupException(string.Format("Произошла ошибка:\n пользователь {0} не найден в выпадающем"
				+ " списке при назначении исполнителя на задачу", fullName));
			}

			Driver.SetDynamicValue(How.XPath, ASSIGNEE_OPTION, fullName).Click();

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Ввести невалидную дату вручную.
		/// </summary>
		/// <param name="dateTime">дата</param>
		public TaskAssignmentPage FillInvalidDeadlineManually(string dateTime, int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Ввести дату вручную.");
			Driver.SetDynamicValue(How.XPath, DEADLINE, taskNumber.ToString()).SendKeys(dateTime);
			CloseDeadlineCalendar();

			return GetPage();
		}

		/// <summary>
		/// Выбрать дату в календаре дедлайна.
		/// </summary>
		/// <param name="day">день</param>
		public TaskAssignmentPage SelectDateInDeadlineDatepicker(int day = 30)
		{
			CustomTestContext.WriteLine("Выбрать дату в календаре дедлайна.");
			OpenDeadlineDatepicker();
			Driver.SetDynamicValue(How.XPath, DAY, day.ToString()).Click();
			CloseDeadlineCalendar();

			return GetPage();
		}

		/// <summary>
		/// Назначить исполнителя
		/// </summary>
		/// <param name="name">имя</param>
		/// <param name="isGroup">является ли группой</param>
		/// <param name="taskNumber">номер задачи</param>
		public TaskAssignmentPage SetResponsible(string name, bool isGroup, int taskNumber = 1)
		{
			OpenAssigneeDropbox(taskNumber);
			SelectResponsible(name, isGroup);
			ClickAssignButton(taskNumber);

			return this;
		}

		/// <summary>
		/// Получить значение из дедлайна.
		/// </summary>
		public DateTime? GetDeadlineDate(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Получить значение из дедлайна.");
			var value = Driver.FindElement(By.XPath(DEADLINE_VALUE.Replace("*#*", taskNumber.ToString()))).GetAttribute("value");

			if (value == String.Empty)
			{
				return null;
			}

			return DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture).Date;
		}

		/// <summary>
		/// Выбрать исполнителя на весь документ
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public SelectAssigneePage SelectAssigneesForEntireDocument(int taskNumber = 1)
		{
			ExpandSelectAssigneesDropdown(taskNumber);
			SelectAssignmentForEntireDocumentType(taskNumber);

			return new SelectAssigneePage(Driver).GetPage();
		}

		/// <summary>
		/// Выбрать исполнителя на cегмент документ
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public DistributeDocumentBetweenAssigneesPage SelectAssigneesForSegmentsDocument(int taskNumber = 1)
		{
			ExpandSelectAssigneesDropdown(taskNumber);
			SelectDistributeDocumentAssignmentType(taskNumber);

			return new DistributeDocumentBetweenAssigneesPage(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что отображается сообщение 'Please, specify the deadline in the format: MM/DD/YYYY'.
		/// </summary>
		public bool IsWrongDeadlineFormatErrorMessage()
		{
			CustomTestContext.WriteLine("Проверить, что отображается сообщение 'Please, specify the deadline in the format: MM/DD/YYYY'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(WRONG_DEADLINE_FORMAT_ERROR));
		}

		/// <summary>
		/// Проверить, что пользователь есть в списке на назначение.
		/// </summary>
		/// <param name="fullName">полное имя пользователя</param>
		public bool IsUserInAssigneeOptionsExist(string fullName)
		{
			CustomTestContext.WriteLine("Проверить, что пользователь '{0}' есть в списке на назначение.", fullName);
			Driver.FindElement(By.XPath(ASSIGNEE_OPTION.Replace("*#*", fullName))).Scroll();

			return Driver.WaitUntilElementIsAppear(By.XPath(ASSIGNEE_OPTION.Replace("*#*", fullName)));
		}

		/// <summary>
		/// Проверить, что отображается сообщение 'The task deadline precedes the deadline for the previous task'.
		/// </summary>
		public bool IsDeadlineIsEarlierThatPreviousError()
		{
			CustomTestContext.WriteLine("Проверить, что отображается сообщение 'The task deadline precedes the deadline for the previous task'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(DEADLINE_IS_EARLIER_THAT_PREVIOUS_ERROR));
		}

		/// <summary>
		/// Проверить, что кнопка назначения отображается для задачи
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public bool IsCancelButtonDisplayed(int taskNumber)
		{
			CustomTestContext.WriteLine("Проверить, что кнопка Cancel отображается для задачи №{0}.", taskNumber);

			return Driver.SetDynamicValue(How.XPath, CANCEL_ASSIGN_BUTTON, taskNumber.ToString()).Displayed;
		}

		/// <summary>
		/// Проверить, есть ли группа в списке
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsGroupExist(string groupName)
		{
			CustomTestContext.WriteLine("Проверить, есть ли группа в списке");

			return GetResponsibleGroupsList().Any(i => i == "Group: " + groupName);
		}

		/// <summary>
		/// Проверить, открыта ли страница назначения задач
		/// </summary>
		public bool IsTaskAssignmentPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(TASK_ASSIGNMENT_TABLE));
		}

		/// <summary>
		/// Проверить, что открылся выпадающий список для задачи
		/// </summary>
		/// <param name="taskRowNumber">номер задачи</param>
		public bool IsTaskAssigneeListDisplayed(int taskRowNumber = 1)
		{
			CustomTestContext.WriteLine("Подтвердить, что открылся выпадающий список для задачи с номером строки {0}", taskRowNumber);

			return Driver.WaitUntilElementIsDisplay(By.XPath(TASK_ASSIGN_DROPBOX_OPTION.Replace("*#*", taskRowNumber.ToString())));
		}

		/// <summary>
		/// Проверить, отображается ли надпись 'Different deadlines'.
		/// </summary>
		public bool IsDifferentDeadlineDisplayed(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, отображается ли надпись 'Different deadlines'.");

			return Driver.SetDynamicValue(How.XPath, DIFFERENT_DEADLINE, taskNumber.ToString()).Displayed;
		}

		/// <summary>
		/// Проверить, отображается ли восклицательный знак в поле дедлайна
		/// </summary>
		public bool IsAttentionIconInDatepickerDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, отображается ли восклицательный знак в поле дедлайна.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ATTENTION_ICON_IN_DATEPICKER));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CONFIRM_CANCEL_BUTTON)]
		protected IWebElement ConfirmCancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }
		
		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }


		[FindsBy(How = How.XPath, Using = DEADLINE_VALUE)]
		protected IWebElement Deadline { get; set; }

		[FindsBy(How = How.XPath, Using = FOOTER)]
		protected IWebElement Footer { get; set; }
		
		protected IWebElement AssigneeDropbox { get; set; }

		protected IWebElement AssignButton { get; set; }

		protected IWebElement CancelAssignButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string SAVE_BUTTON = "//div[contains(@data-bind, 'click: saveDeadlines')]//a";
		protected const string SELECT_ASSIGNEES_DROPDOWN = "//tr[*#*]//td[2]//div[contains(@data-bind, 'setAssignment()')]";
		protected const string SIMPLE_ASSIGNMENT_OPTION = "//tr[*#*]//div[contains(@class, 'sublist assignExecutives')]//div[contains(@class,'first')]";
		protected const string SPLIT_ASSIGNMENT_OPTION = "//tr[*#*]//div[contains(@class, 'sublist assignExecutives')]//div[contains(@class,'last')]";
		protected const string CHANGE_ASSIGNEES_BUTTON = "//tr[*#*]//a[contains(@data-bind, 'setAssignmentsButtonTitle')]";
		protected const string TASK_ASSIGNMENT_TABLE = "//table[contains(@class, 'assignment-table')]";
		protected const string TASK_ASSIGN_DROPDOWN = "//tr[*#*]//div[contains(@class, 'assignment_dropdown')]//input";
		protected const string ASSIGNEE_LIST = "//div[contains(@class, 'assignment_dropdown')]//ul//li[not(@title='')]";
		protected const string ASSIGNEE_OPTION = "//div[contains(@class, 'assignment_dropdown')]//ul//li[text()='*#*']";
		protected const string ASSIGN_BUTTON = "//tr[*#*]//a[contains(@data-bind, 'click: assignSingleExecutive')]";
		protected const string CANCEL_ASSIGN_BUTTON = "//tr[*#*]//a[contains(@data-bind, 'click: unassignSingleExecutive')]";
		protected const string CONFIRM_CANCEL_BUTTON = "//div[contains(@class,'l-confirm')]//span[span[input[@value='Cancel Assignment']]]";
		protected const string CANCEL_BUTTON = "//div[contains(@data-bind, 'click: close')]//a";
		protected const string ASSIGN_STATUS = "(//span[@data-bind='text: status()' and text()='*#*'])[*##*]";
		protected const string TASK_ASSIGN_DROPBOX_OPTION = "//table[*#*]//ul[contains(@class, 'list newDropdown')]";
		protected const string ASSIGNEES_COLUMN = "//tr[*#*]//td[contains(@data-bind, 'l-assignments-different-values')][1]";
		protected const string ASSIGNEE_NAME = "//tr[*#*]//td[contains(@data-bind, 'l-assignments-different-values')][1]//p[contains(@class, 'a-user_table-name')]";
		protected const string SELECT_SEVERAL_ASSIGNEES_DROPDOWN = "//tr[*#*]//a[contains(@data-bind, 'setAssignmentsButtonTitle')]";

		protected const string DIFFERENT_DEADLINE = "//tr[*#*]//td[contains(@class, 'l-assignments-different-values')]";
		protected const string DEADLINE_VALUE = "//tr[*#*]//div[contains(@data-bind, 'datepicker')]//input[@class='js-submit-input']";
		protected const string DEADLINE = "//tr[*#*]//div[contains(@data-bind, 'datepicker')]//input[contains(@class, 'hasDatepicker')]";
		protected const string DAY = "//table[contains(@class, 'datepicker-calendar')]//a[text()='*#*']";
		protected const string FOOTER = "//li[contains(@class, 'footer')]";
		protected const string WRONG_DEADLINE_FORMAT_ERROR = "//p[@data-message-id='wrong-deadline-format']";
		protected const string DEADLINE_IS_EARLIER_THAT_PREVIOUS_ERROR = "//p[contains(@data-message-id, 'deadline-is-earlier-that-previous')]";
		protected const string DEADLINE_IS_LATER_THAT_PROJECT_DEADLINE_POP_UP = "//div[contains(text(), 'later than the project deadline')]";
		protected const string ATTENTION_ICON_IN_DATEPICKER = "//div[contains(@data-bind, 'marked: isDeadlineLaterThanProject()')]";

		#endregion
	}
}
