using System.Collections.Generic;
using System.Linq;
using NLog.Fluent;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class TaskAssignmentDialog : ProjectsPage, IAbstractPage<TaskAssignmentDialog>
	{
		public new TaskAssignmentDialog GetPage()
		{
			var taskAssignmentDialog = new TaskAssignmentDialog();
			InitPage(taskAssignmentDialog);

			return taskAssignmentDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(TASK_ASSIGN_TABLE)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось открыть диалог выбора исполнителя.");
			}
		}

		/// <summary>
		/// Открыть выпадющий список для задачи с заданным номером
		/// </summary>
		/// <param name="taskRowNumber"> номер задачи</param>
		public TaskAssignmentDialog OpenAssigneeDropbox(int taskRowNumber = 1)
		{
			Logger.Trace("Открыть выпадающий список для задачи с номером строки {0}", taskRowNumber);

			AssigneeDropbox = Driver.SetDynamicValue(How.XPath, TASK_ASSIGN_DROPBOX, taskRowNumber.ToString());
			AssigneeDropbox.Click();

			return GetPage();
		}

		/// <summary>
		/// Подтвердить, что открылся выпадающий список для задачи
		/// </summary>
		/// <param name="taskRowNumber">номер задачи</param>
		public TaskAssignmentDialog AssertTaskAssigneeListDisplay(int taskRowNumber = 1)
		{
			Logger.Trace("Подтвердить, что открылся выпадающий список для задачи с номером строки {0}", taskRowNumber);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(TASK_ASSIGN_DROPBOX_OPTION.Replace("*#*", taskRowNumber.ToString()))),
				"Произошла ошибка:\n список исполнителей не открылся");

			return GetPage();
		}

		/// <summary>
		/// Получить список исполнителей
		/// </summary>
		public List<string> GetResponsibleUsersList()
		{
			Logger.Trace("Получить список исполнителей.");

			var elementUsersList = Driver.GetTextListElement(By.XPath(ASSIGNEE_LIST.Replace("*#*", "")));

			return (from element in elementUsersList
					where !element.Contains("Group: ")
					select element.Replace("  ", " ")).ToList();
		}

		/// <summary>
		/// Получить список групп исполнителей
		/// </summary>
		public List<string> GetResponsibleGroupsList()
		{
			Logger.Trace("Получить список групп исполнителей.");

			var elementUsersList = Driver.GetTextListElement(By.XPath(ASSIGNEE_LIST.Replace("*#*", "")));

			return (from element in elementUsersList
					where element.Contains("Group: ")
					select element.Replace("  ", " ")).ToList();
		}

		/// <summary>
		/// Выбрать из выпадающего списка пользователя или группу по имени
		/// </summary>
		/// <param name="name">Имя пользователя или группы</param>
		/// <param name="isGroup">Выбор группы</param>
		public TaskAssignmentDialog SelectResponsible(string name, bool isGroup)
		{
			Logger.Debug("Выбрать из выпадающего списка {1}. Это группа: {2}", name, isGroup);

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
		public TaskAssignmentDialog ClickAssignButton(int taskNumber = 1)
		{
			Logger.Debug("Нажать кнопку 'Назначить'");

			AssignButton = Driver.SetDynamicValue(How.XPath, ASSIGN_BUTTON, taskNumber.ToString());
			AssignButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Закрыть'
		/// </summary>
		public T ClickCloseTaskAssignmentDialog<T>() where T : class, IAbstractPage<T>, new()
		{
			Logger.Debug("Нажать кнопку 'Закрыть'");
			CloseButton.Click();

			return new T().GetPage();
		}

		/// <summary>
		/// Дождаться закрытия диалога назначения пользователей
		/// </summary>
		public T AssertTaskAssignmentDialogDisappear<T>() where T : class, IAbstractPage<T>, new()
		{
			Logger.Debug("Дождаться закрытия диалога назначения пользователей.");

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(TASK_ASSIGN_TABLE)),
				"Произошла ошибка:\n диалог назначения пользователей не закрылся.");

			return new T().GetPage();
		}

		/// <summary>
		/// Нажать кнопку отмены назначения исполнителя задачи
		/// </summary>
		/// <param name="taskNumber"> номер задачи </param>
		public TaskAssignmentDialog ClickCancelAssignButton(int taskNumber = 1)
		{
			Logger.Trace("Нажать кнопку отмены назначения исполнителя для задачи с номером '{0}'", taskNumber);

			CancelAssignButton = Driver.SetDynamicValue(How.XPath, CANCEL_ASSIGN_BUTTON, taskNumber.ToString());
			CancelAssignButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Дождаться поялвения кнопки подтверждения удаления назначения пользователя
		/// </summary>
		public TaskAssignmentDialog WaitCancelConfirmButtonDisplay()
		{
			Logger.Trace("Дождаться появления кнопки подтверждения удаления назначения пользователя");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_CANCEL_BUTTON)),
				"Произошла ошибка:\n не появилась кнопка подтверждения удаления назначения пользователя");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку подтверждения удаления назначения пользователя
		/// </summary>
		public TaskAssignmentDialog ClickConfirmCancelButton()
		{
			Logger.Trace("Нажать кнопку подтверждения удаления назначения пользователя");
			ConfirmCancelButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Подтвердить, что стоит ожидаемый статус задачи.
		/// </summary>
		/// <param name="expectedStatus">ожидаемый статус</param>
		/// <param name="taskNumber">номер задачи</param>
		public TaskAssignmentDialog AssertAssignStatus(string expectedStatus, int taskNumber)
		{
			Logger.Trace("Подтвердить, что статус задачи: '{0}'", expectedStatus);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ASSIGN_STATUS.Replace("*#*", expectedStatus).Replace("*##*",taskNumber.ToString()))),
				"Произошла ошибка:\n статус задачи не {0}", expectedStatus);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка отмены назначения на задачу появилась
		/// </summary>
		public TaskAssignmentDialog AssertCancelAssignButtonExist(int taskNumber = 1)
		{
			Logger.Trace("Проверить, что кнопка отмены назначения на задачу появилась");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CANCEL_ASSIGN_BUTTON.Replace("*#*", taskNumber.ToString()))),
				"Произошла ошибка:\n кнопка отмены назначения на задачу не появилась.");

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = CLOSE_BUTTON)]
		protected IWebElement CloseButton { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_CANCEL_BUTTON)]
		protected IWebElement ConfirmCancelButton { get; set; }

		protected IWebElement AssigneeDropbox { get; set; }

		protected IWebElement AssignButton { get; set; }

		protected IWebElement CancelAssignButton { get; set; }

		protected const string TASK_ASSIGN_TABLE = "(//table[contains(@class, 'js-progress-table')]//table)[2]";
		protected const string TASK_ASSIGN_DROPBOX = "(//table[contains(@class, 'js-progress-table')]//table)[2]//tr[*#*]//td[contains(@class, 'assineer')]//select";
		protected const string ASSIGNEE_LIST = "(//div[contains(@class, 'js-popup-assign')])[2]//td[contains(@class, 'assineer')]//select//option[contains(text() , '*#*')]";
		protected const string ASSIGN_BUTTON = "((//table[contains(@class, 'js-progress-table')]//table)[2]//a[contains(@data-bind, 'click: assign')])[*#*]";
		protected const string CLOSE_BUTTON = "(//div[contains(@class, 'js-popup-assign')])[2]//span[contains(@class, 'js-popup-close')]";
		protected const string CANCEL_ASSIGN_BUTTON = "((//table[contains(@class, 'js-progress-table')]//table)[2]//span[contains(@class,'js-assigned-cancel')])[*#*]";
		protected const string CONFIRM_CANCEL_BUTTON = "//div[contains(@class,'l-confirm')]//span[span[input[@value='Cancel Assignment']]]";
		protected const string ASSIGN_STATUS = "(//span[@data-bind='text: status()' and text()='*#*'])[*##*]";
		protected const string TASK_ASSIGN_DROPBOX_OPTION = "(//table[contains(@class, 'js-progress-table')]//table)[2]//tr[1]//td[contains(@class, 'assineer')]//select/option[1]";
	}
}
