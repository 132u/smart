using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.AssignmentPages;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class TaskAssignmentPage : ProjectsPage, IAbstractPage<TaskAssignmentPage>
	{
		public TaskAssignmentPage(WebDriver driver) : base(driver)
		{
		}

		public new TaskAssignmentPage LoadPage()
		{
			if (!IsTaskAssignmentPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось открыть страницу назначения задач");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Кликнуть кнопку открытия\закрытия дропбокса списка пользователей для назначения на задачу с заданным номером
		/// </summary>
		/// <param name="taskNumber"> номер задачи</param>
		public TaskAssignmentPage ClickAssigneeDropboxButton(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Кликнуть кнопку открытия\\закрытия дропбокса списка пользователей для назначения на задачу № {0}.", taskNumber);
			AssigneeDropbox = Driver.SetDynamicValue(How.XPath, SELECT_ASSIGNEES_DROPDOWN, taskNumber.ToString());
			AssigneeDropbox.Click();

			return LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку отмены назначения исполнителя задачи
		/// </summary>
		/// <param name="nickName"> nickName пользователя </param>
		/// <param name="taskNumber"> номер задачи </param>
		public TaskAssignmentPage ClickCancelAssignButton(string nickName, int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать кнопку отмены назначения исполнителя задачи с номером '{0}' для пользователя {1}", taskNumber, nickName);

			CancelAssignButton = Driver.SetDynamicValue(How.XPath, CANCEL_ASSIGN_BUTTON, taskNumber.ToString(), nickName);
			CancelAssignButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку возврата к проекту
		/// </summary>
		public ProjectSettingsPage ClickBackToProjectButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку возврата к проекту");

			BackToProjectButton.Click();

			return new ProjectSettingsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку возврата к проекту, ожидая алерт
		/// </summary>
		public void ClickBackToProjectButtonExpectingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку возврата к проекту, ожидая алерт");

			BackToProjectButton.Click();
		}

		/// <summary>
		/// Нажать кнопку сохранения исполнителя задачи
		/// </summary>
		public TaskAssignmentPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения исполнителя задачи");
			Thread.Sleep(1000);
			SaveButton.Click();
			Thread.Sleep(1000);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения исполнителя задачи, ожидается валидационная ошибка
		/// </summary>
		public TaskAssignmentPage ClickSaveAssignButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения исполнителя задачи, ожидается валидационная ошибка.");
			Thread.Sleep(1000);
			SaveButton.Click();

			return LoadPage();
		}
		
		/// <summary>
		/// Отркыть календарь для задачи.
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public DatePicker OpenDatePicker(int taskNumber = 1 )
		{
			CustomTestContext.WriteLine("Отркыть календарь для задачи №{0}.", taskNumber);
			DeadlineIcon = Driver.SetDynamicValue(How.XPath, DEADLINE, taskNumber.ToString());
			DeadlineIcon.Click();

			return new DatePicker(Driver).LoadPage();
		}

		/// <summary>
		/// Раскрыть календарь дедлайна.
		/// </summary>
		public TaskAssignmentPage OpenDeadlineDatepicker()
		{
			CustomTestContext.WriteLine("Раскрыть календарь дедлайна.");
			Deadline.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на пустую область в футуре, чтоб закрыть дродаун.
		/// </summary>
		public TaskAssignmentPage ClickFooter()
		{
			CustomTestContext.WriteLine("Нажать на пустую область в футуре, чтоб закрыть дродаун.");
			Footer.Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить имена из колонки Assignnees для задачи с нужным номером.
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public List<string> GetAssignneeName(int taskNumber)
		{
			CustomTestContext.WriteLine("Получить имена из колонки Assignnees для задачи №{0}.", taskNumber);

			var assignees = Driver.GetTextListElement(By.XPath(ASSIGNEE_NAME_LIST.Replace("*#*", taskNumber.ToString())));
			assignees.Sort();

			return assignees;
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
		/// Выбрать тип назначения 'Распределить сегменты между исполнителями'
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public DistributeDocumentBetweenAssigneesPage SelectDistributeDocumentAssignmentType(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Выбрать тип назначения 'Распределить сегменты между исполнителями' для задачи №{0}.", taskNumber);
			Driver.FindElement(By.XPath(SPLIT_ASSIGNMENT_OPTION.Replace("*#*", taskNumber.ToString()))).Click();

			return new DistributeDocumentBetweenAssigneesPage(Driver).LoadPage();
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

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Назначить исполнителя
		/// </summary>
		/// <param name="name">имя</param>
		/// <param name="isGroup">является ли группой</param>
		/// <param name="taskNumber">номер задачи</param>
		public TaskAssignmentPage SetResponsible(string name, bool isGroup = false, int taskNumber = 1)
		{
			ClickAssigneeDropboxButton(taskNumber);
			SelectResponsible(name, isGroup);
			ClickFooter();

			return LoadPage();
		}

		/// <summary>
		/// Получить значение из дедлайна.
		/// </summary>
		public DateTime? GetDeadlineDate(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Получить значение из дедлайна.");
			if (!Driver.GetIsElementExist(By.XPath(DEADLINE_VALUE.Replace("*#*", taskNumber.ToString()))))
			{
				return null;
			}

			var dateTime = Driver.FindElement(By.XPath(DEADLINE_VALUE.Replace("*#*", taskNumber.ToString()))).Text;
			var date = dateTime.Split(new[] { ' ' })[0];

			return DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture).Date;
		}

		#endregion

		#region Методы, проверяющие состояние страницы

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
		/// Проверить, что кнопка Cancel отображается для задачи у нужного пользователя
		/// </summary>
		/// <param name="nickName"> nickName пользователя </param>
		/// <param name="taskNumber">номер задачи</param>
		public bool IsCancelButtonDisplayed(string nickName, int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что кнопка Cancel отображается для задачи №{0} у пользователя {1}.", taskNumber, nickName);

			return Driver.SetDynamicValue(How.XPath, CANCEL_ASSIGN_BUTTON, taskNumber.ToString(), nickName).Displayed;
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

		/// <summary>
		/// Проверить, что отображается дропдаун выбора исполнителя
		/// </summary>
		/// <param name="taskNumber"> номер задачи</param>
		public bool IsAssigneeDropdownDisplayed(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что отображается дропдаун выбора исполнителя для задачи № {0}.", taskNumber);

			return Driver.WaitUntilElementIsDisplay(By.XPath(SELECT_ASSIGNEES_DROPDOWN.Replace("*#*", taskNumber.ToString())));
		}

		/// <summary>
		/// Проверить, что пользователь не назначен на задачу
		/// </summary>
		/// <param name="nickName">nickName пользователя</param>
		/// <param name="taskNumber"> номер задачи</param>
		public bool IsAssignedUserDisappeared(string nickName, int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что пользователь {0} не назначен на задачу № {1}.", nickName, taskNumber);

			return Driver.WaitUntilElementIsDisappeared(By.XPath(ASSIGNEE_NAME_EDIT.Replace("*#*", taskNumber.ToString()).Replace("*##*", nickName)));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CONFIRM_CANCEL_BUTTON)]
		protected IWebElement ConfirmCancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_VALUE)]
		protected IWebElement Deadline { get; set; }

		[FindsBy(How = How.XPath, Using = FOOTER)]
		protected IWebElement Footer { get; set; }

		[FindsBy(How = How.XPath, Using = BACK_TO_PROJECT_BUTON)]
		protected IWebElement BackToProjectButton { get; set; }
		
		protected IWebElement AssigneeDropbox { get; set; }

		protected IWebElement AssignButton { get; set; }

		protected IWebElement CancelAssignButton { get; set; }
		protected IWebElement DeadlineIcon { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string SAVE_BUTTON = "//div[contains(@data-bind, 'save')]//a";
		protected const string SELECT_ASSIGNEES_DROPDOWN = "//tr[*#*]//td[2]//button[@data-bind='click: switchToSelectMode']";
		protected const string SPLIT_ASSIGNMENT_OPTION = "//div[contains(@data-bind,'click: goToSplitAssignment')]/a";
		protected const string CHANGE_ASSIGNEES_BUTTON = "//tr[*#*]//a[contains(@data-bind, 'setAssignmentsButtonTitle')]";
		protected const string TASK_ASSIGNMENT_TABLE = "//table[contains(@class, 'at-assignment-table')]";
		protected const string TASK_ASSIGN_DROPDOWN = "//tr[*#*]//div[contains(@class, 'assignment_dropdown')]//input";
		protected const string ASSIGNEE_LIST = "//div[contains(@class, 'g-dropbox__body')]//ul//li";
		protected const string ASSIGNEE_OPTION = "//div[contains(@class, 'g-dropbox__body')]//ul//li[@title='*#*']";
		protected const string CANCEL_ASSIGN_BUTTON = "//tbody[@data-bind='foreach: workflowStages']//tr[*#*]//span[@data-title='*##*']//following-sibling::span/button[@data-bind='click: $parent.deselectExecutive']";
		protected const string CONFIRM_CANCEL_BUTTON = "//div[contains(@class,'l-confirm')]//span[span[input[@value='Cancel Assignment']]]";
		protected const string ASSIGN_STATUS = "(//span[@data-bind='text: status()' and text()='*#*'])[*##*]";
		protected const string TASK_ASSIGN_DROPBOX_OPTION = "//table[*#*]//ul[contains(@class, 'list newDropdown')]";
		protected const string ASSIGNEES_COLUMN = "//tbody[@data-bind='foreach: workflowStages']//tr[*#*]/td[2]";
		protected const string ASSIGNEE_NAME_LIST = "//tbody[@data-bind='foreach: workflowStages']//tr[*#*]//span[@data-bind='text: name, titleOnOverflow']";
		protected const string ASSIGNEE_NAME_EDIT = "//tr[*#*]//td[2]//span[@class='g-tag__name'][text()='*##*']";
		protected const string SELECT_SEVERAL_ASSIGNEES_DROPDOWN = "//tr[*#*]//a[contains(@data-bind, 'setAssignmentsButtonTitle')]";

		protected const string DIFFERENT_DEADLINE = "//tr[*#*]//td[contains(@class, 'l-assignments-different-values')]";
		protected const string DEADLINE_VALUE = "//tr[*#*]//datetimepicker//span[contains(@data-bind, 'formatDateTime')]";
		protected const string DEADLINE = "//tr[*#*]//datetimepicker//span[contains(@data-bind, 'showCalendarIcon')]";
		protected const string DAY = "//table[contains(@class, 'datepicker-calendar')]//a[text()='*#*']";
		protected const string FOOTER = "//li[contains(@class, 'footer')]";
		protected const string WRONG_DEADLINE_FORMAT_ERROR = "//p[@data-message-id='wrong-deadline-format']";
		protected const string DEADLINE_IS_EARLIER_THAT_PREVIOUS_ERROR = "//p[contains(@data-message-id, 'deadline-is-earlier-that-previous')]";
		protected const string DEADLINE_IS_LATER_THAT_PROJECT_DEADLINE_POP_UP = "//div[contains(text(), 'later than the project deadline')]";
		protected const string ATTENTION_ICON_IN_DATEPICKER = "//div[@class='g-datetimepicker g-datetimepicker_stage g-datetimepicker_marked']";
		protected const string BACK_TO_PROJECT_BUTON = "//button[@data-bind='click: close']";

		#endregion
	}
}
