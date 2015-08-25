using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class TaskAssignmentPage : ProjectsPage, IAbstractPage<TaskAssignmentPage>
	{
		public new TaskAssignmentPage GetPage()
		{
			var taskAssignmentDialog = new TaskAssignmentPage();
			InitPage(taskAssignmentDialog);

			return taskAssignmentDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(TASK_ASSIGNMENT_TABLE)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось открыть диалог выбора исполнителя.");
			}
		}

		/// <summary>
		/// Открыть выпадющий список для задачи с заданным номером
		/// </summary>
		/// <param name="taskRowNumber"> номер задачи</param>
		public TaskAssignmentPage OpenAssigneeDropbox(int taskRowNumber = 1)
		{
			Logger.Debug("Открыть выпадающий список для задачи с номером строки {0}", taskRowNumber);

			AssigneeDropbox = Driver.SetDynamicValue(How.XPath, TASK_ASSIGN_DROPBOX, taskRowNumber.ToString());
			AssigneeDropbox.Click();

			return GetPage();
		}

		/// <summary>
		/// Подтвердить, что открылся выпадающий список для задачи
		/// </summary>
		/// <param name="taskRowNumber">номер задачи</param>
		public TaskAssignmentPage AssertTaskAssigneeListDisplay(int taskRowNumber = 1)
		{
			Logger.Debug("Подтвердить, что открылся выпадающий список для задачи с номером строки {0}", taskRowNumber);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(TASK_ASSIGN_DROPBOX_OPTION.Replace("*#*", taskRowNumber.ToString()))),
				"Произошла ошибка:\n список исполнителей не открылся");

			return GetPage();
		}

		/// <summary>
		/// Получить список исполнителей
		/// </summary>
		public List<string> GetResponsibleUsersList()
		{
			Logger.Debug("Получить список исполнителей.");

			var elementUsersList = Driver.GetTextListElement(By.XPath(ASSIGNEE_LIST.Replace("*#*", "")));

			return (from element in elementUsersList
					where !element.Contains("Group: ")
					select element).ToList();
		}

		/// <summary>
		/// Получить список групп исполнителей
		/// </summary>
		public List<string> GetResponsibleGroupsList()
		{
			Logger.Debug("Получить список групп исполнителей.");

			var elementUsersList = Driver.GetTextListElement(By.XPath(ASSIGNEE_LIST.Replace("*#*", "")));

			return (from element in elementUsersList
					where element.Contains("Group: ")
					select element).ToList();
		}

		/// <summary>
		/// Выбрать из выпадающего списка пользователя или группу по имени
		/// </summary>
		/// <param name="name">Имя пользователя или группы</param>
		/// <param name="isGroup">Выбор группы</param>
		public TaskAssignmentPage SelectResponsible(string name, bool isGroup)
		{
			Logger.Debug("Выбрать из выпадающего списка {0}. Это группа: {1}", name, isGroup);

			var fullName = isGroup ? "Group: " + name : name;

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ASSIGNEE_LIST.Replace("*#*", fullName))),
				"Произошла ошибка:\n пользователь {0} не найден в выпадающем"
				+ " списке при назначении исполнителя на задачу", fullName);

			AssigneeDropbox.SelectOptionByText(fullName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Назначить'
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public TaskAssignmentPage ClickAssignButton(int taskNumber = 1)
		{
			Logger.Debug("Нажать кнопку 'Назначить'");

			AssignButton = Driver.SetDynamicValue(How.XPath, ASSIGN_BUTTON, taskNumber.ToString());
			AssignButton.Click();

			return GetPage();
		}
		
		/// <summary>
		/// Дождаться поялвения кнопки подтверждения удаления назначения пользователя
		/// </summary>
		public TaskAssignmentPage WaitCancelConfirmButtonDisplay()
		{
			Logger.Debug("Дождаться появления кнопки подтверждения удаления назначения пользователя");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_CANCEL_BUTTON)),
				"Произошла ошибка:\n не появилась кнопка подтверждения удаления назначения пользователя");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку подтверждения удаления назначения пользователя
		/// </summary>
		public TaskAssignmentPage ClickConfirmCancelButton()
		{
			Logger.Debug("Нажать кнопку подтверждения удаления назначения пользователя");
			ConfirmCancelButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку отмены назначения исполнителя задачи
		/// </summary>
		/// <param name="taskNumber"> номер задачи </param>
		public TaskAssignmentPage ClickCancelAssignButton(int taskNumber = 1)
		{
			Logger.Debug("Нажать кнопку отмены назначения исполнителя для задачи с номером '{0}'", taskNumber);

			CancelAssignButton = Driver.SetDynamicValue(How.XPath, CANCEL_ASSIGN_BUTTON, taskNumber.ToString());
			CancelAssignButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения исполнителя задачи
		/// </summary>
		public ProjectSettingsPage ClicSaveAssignButton()
		{
			Logger.Debug("Нажать кнопку сохранения исполнителя задачи");
			SaveButton.Click();

			return new ProjectSettingsPage().GetPage();
		}

		/// <summary>
		/// Раскрыть 'Select Assignees' дропдаун.
		/// </summary>
		/// <param name="taskNumber">номер таски</param>
		public TaskAssignmentPage ExpandSelectAssigneesDropdown(int taskNumber)
		{
			Logger.Debug("Раскрыть 'Select Assignees' дропдаун.");
			var assigneesDropdownForTask = Driver.SetDynamicValue(How.XPath, SELECT_ASSIGNEES_DROPDOWN, taskNumber.ToString());
			assigneesDropdownForTask.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать тип назначения
		/// </summary>
		/// <param name="assignmentType">тип назначения</param>
		public SelectAssigneePage SelectAssignmentType(AssignmentType assignmentType, int taskNumber)
		{
			Logger.Debug("Выбрать {0} тип назначения.", assignmentType);
			IWebElement assignmentTypeOption;

			if (assignmentType == AssignmentType.Simple)
			{
				assignmentTypeOption =
					Driver.FindElement(By.XPath(SIMPLE_ASSIGNMENT_OPTION.Replace("*#*", taskNumber.ToString())));
			}
			else
			{
				assignmentTypeOption =
					Driver.FindElement(By.XPath(SPLIT_ASSIGNMENT_OPTION.Replace("*#*", taskNumber.ToString())));
			}

			assignmentTypeOption.Click();

			return new SelectAssigneePage().GetPage();
		}

		[FindsBy(How = How.XPath, Using = CONFIRM_CANCEL_BUTTON)]
		protected IWebElement ConfirmCancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		protected IWebElement AssigneeDropbox { get; set; }

		protected IWebElement AssignButton { get; set; }

		protected IWebElement CancelAssignButton { get; set; }

		protected const string SAVE_BUTTON = "//span[contains(@data-bind, 'click: saveDeadlines')]//a"; 
		protected const string SELECT_ASSIGNEES_DROPDOWN = "//tr[*#*]//td[2]//span[contains(@class, 'bluebtn expandable')]";
		protected const string SIMPLE_ASSIGNMENT_OPTION = "//tr[*#*]//div[contains(@class, 'sublist assignExecutives')]//div[contains(@class,'first')]";
		protected const string SPLIT_ASSIGNMENT_OPTION = "//tr[*#*]//div[contains(@class, 'sublist assignExecutives')]//div[contains(@class,'last')]";
		protected const string CHANGE_ASSIGNEES_BUTTON = "//tr[*#*]//a[contains(@data-bind, 'setAssignmentsButtonTitle')]";
		protected const string TASK_ASSIGNMENT_TABLE = "//table[contains(@class, 'assignment-table')]";
		protected const string TASK_ASSIGN_DROPBOX = "(//table[contains(@class, 'js-progress-table')]//table)[2]//tr[*#*]//td[contains(@class, 'assineer')]//select";
		protected const string ASSIGNEE_LIST = "(//div[contains(@class, 'js-popup-assign')])[2]//td[contains(@class, 'assineer')]//select//option[contains(text() , '*#*')]";
		protected const string ASSIGN_BUTTON = "((//table[contains(@class, 'js-progress-table')]//table)[2]//a[contains(@data-bind, 'click: assign')])[*#*]";
		protected const string CANCEL_ASSIGN_BUTTON = "((//table[contains(@class, 'js-progress-table')]//table)[2]//span[contains(@class,'js-assigned-cancel')])[*#*]";
		protected const string CONFIRM_CANCEL_BUTTON = "//div[contains(@class,'l-confirm')]//span[span[input[@value='Cancel Assignment']]]";
		protected const string ASSIGN_STATUS = "(//span[@data-bind='text: status()' and text()='*#*'])[*##*]";
		protected const string TASK_ASSIGN_DROPBOX_OPTION = "(//table[contains(@class, 'js-progress-table')]//table)[2]//tr[1]//td[contains(@class, 'assineer')]//select/option[1]";

	}
}
