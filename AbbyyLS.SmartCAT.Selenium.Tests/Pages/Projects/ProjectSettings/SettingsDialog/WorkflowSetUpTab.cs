using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog
{
	public class WorkflowSetUpTab : ProjectSettingsDialog, IAbstractPage<WorkflowSetUpTab>
	{
		public WorkflowSetUpTab(WebDriver driver) : base(driver)
		{
		}

		public new WorkflowSetUpTab GetPage()
		{
			var workflowSetUpTab = new WorkflowSetUpTab(Driver);
			InitPage(workflowSetUpTab, Driver);

			return workflowSetUpTab;
		}

		public new void LoadPage()
		{
			if (!IsWorkflowSetUpTabOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Не открылась вкладка 'Workflow SetUp'.");
			}
		}
		
		#region Простые методы страницы
		
		/// <summary>
		/// Нажать кнопку 'New Task'
		/// </summary>
		public ProjectSettingsDialog ClickNewTaskButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'New Task'.");
			NewTaskButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Раскрыть комбобокс с задачами
		/// </summary>
		public ProjectSettingsDialog ExpandTask(int taskNumber)
		{
			CustomTestContext.WriteLine("Раскрыть комбобокс №{0} с задачами.", taskNumber);
			Driver.SetDynamicValue(How.XPath, TASK, taskNumber.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по задаче
		/// </summary>
		public ProjectSettingsDialog ClickTaskInDropdown(WorkflowTask taskName)
		{
			CustomTestContext.WriteLine("Кликнуть по задаче {0}.", taskName);
			var task = Driver.SetDynamicValue(How.XPath, TASK_LIST, taskName.ToString());

			task.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить список задач в настройках Workflow
		/// </summary>
		public List<string> WorkflowTaskList()
		{
			CustomTestContext.WriteLine("Получить список задач в настройках Workflow.");

			return Driver.GetTextListElement(By.XPath(WORKFLOW_LIST));
		}

		/// <summary>
		/// Нажать кнопку удаления задачи
		/// </summary>
		/// <param name="taskNumber"></param>
		public ProjectSettingsDialog ClickDeleteTaskButton(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления задачи №{0}", taskNumber);
			DeleteTaskButton = Driver.SetDynamicValue(How.XPath, DELETE_TASK_BUTTON, taskNumber.ToString());
			DeleteTaskButton.Click();

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Редактировать задачу Workflow
		/// </summary>
		/// <param name="task">новая задача</param>
		/// <param name="taskNumber">номер задачи</param>
		public ProjectSettingsPage EditTask(WorkflowTask task, int taskNumber = 2)
		{
			ExpandTask(taskNumber);
			ClickTaskInDropdown(task);
			var projectSettingsPage = SaveSettings();

			return projectSettingsPage.GetPage();
		}

		/// <summary>
		/// Добавить задачу Workflow
		/// </summary>
		/// <param name="task">задача</param>
		/// <param name="taskNumber">номер задачи</param>
		public ProjectSettingsDialog AddTask(WorkflowTask task, int taskNumber = 2)
		{
			ClickNewTaskButton();
			ExpandTask(taskNumber);
			ClickTaskInDropdown(task);

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что задача Workflow соответствует ожидаемой
		/// </summary>
		/// <param name="workflowTask">ожидаемая задача Workflow</param>
		/// <param name="taskNumber">номер задачи</param>
		public bool IsWorkflowTaskMatchExpected(WorkflowTask workflowTask, int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что задача №{0} в настройках Workflow - это {1}", taskNumber, workflowTask);

			return workflowTask.ToString() == WorkflowTaskList()[taskNumber - 1];
		}

		/// <summary>
		/// Проверить, что кол-во задач Workflow соответствует ожидаемому
		/// </summary>
		/// <param name="taskCount">номер задачи</param>
		public bool IsWorkflowTaskCountMatchExpected(int taskCount = 1)
		{
			CustomTestContext.WriteLine("Проверить, что количество задач в настройках Workflow = {0}.", taskCount);

			return taskCount == WorkflowTaskList().Count;
		}

		/// <summary>
		/// Проверить, что открылась вкладка 'Workflow SetUp'.
		/// </summary>
		public bool IsWorkflowSetUpTabOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(NEW_TASK_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = NEW_TASK_BUTTON)]
		protected IWebElement NewTaskButton { get; set; }

		#endregion
		
		#region Описания XPath элементов

		protected const string NEW_TASK_BUTTON = "//div[contains(@class, 'js-popup-wrapper js-popup-edit') and contains(@style, 'display: block')]//div[contains(@data-bind, 'addWorkflowStage')]//a";
		protected const string TASK = "//div[contains(@class,'js-popup-edit')][2]//tbody[@data-bind='foreach: workflowStages']//tr[*#*]//td[2]//span//span";
		protected const string TASK_LIST = "//span[contains(@class,'js-dropdown__item') and @title='*#*']";

		#endregion
	}
}
