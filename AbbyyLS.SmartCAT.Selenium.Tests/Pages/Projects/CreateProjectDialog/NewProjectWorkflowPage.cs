using System.Collections.Generic;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectWorkflowPage : WorkspacePage, IAbstractPage<NewProjectWorkflowPage>
	{
		public NewProjectWorkflowPage(WebDriver driver)
			: base(driver)
		{
		}

		public new NewProjectWorkflowPage GetPage()
		{
			var newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			InitPage(newProjectWorkflowPage, Driver);

			return newProjectWorkflowPage;
		}

		public new void LoadPage()
		{
			Driver.WaitPageTotalLoad();
			if (!IsNewProjectWorkflowPageOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылась страница Workflow для создаваемого проекта");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Получить список добавленных задач на этапе Workflow
		/// </summary>
		public List<string> GetWorkflowAddedTaskList()
		{
			CustomTestContext.WriteLine("Получить список добавленных задач на этапе Workflow.");

			return Driver.GetTextListElement(By.XPath(WORKFLOW_ADDED_TASK_LIST));
		}

		/// <summary>
		/// Нажать на кнопку 'Create Project'
		/// </summary>
		public ProjectsPage ClickCreateProjectButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Create Project'");
			CreateProjectButton.Click();

			return new ProjectsPage(Driver).GetPage();
		}

		/// <summary>
		/// Получить список возможноых типов этапов Workflow
		/// </summary>
		public IEnumerable<string> GetTaskTipesList()
		{
			CustomTestContext.WriteLine("Получить список возможноых типов этапов Workflow");
			var workflowList = Driver.GetElementList(By.XPath(WORKFLOW_TASK_TYPE_LIST));

			return workflowList.Select(taskOption => taskOption.Text);
		}

		/// <summary>
		/// Кликнуть на кнопку назначения новой задачи
		/// </summary>
		/// <param name="task">тип новой задачи</param>
		public NewProjectWorkflowPage ClickNewTaskButton(WorkflowTask task)
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку назначения новой задачи типа {0}.", task);
			WorkflowTaskTypeItem = Driver.SetDynamicValue(How.XPath, WORKFLOW_TASK_TYPE_ITEM, task.ToString());
			WorkflowTaskTypeItem.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть на кнопку удаления добавленной задачи
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public NewProjectWorkflowPage ClickDeleteTaskButton(int taskNumber)
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку удаления добавленной задачи №{0}.", taskNumber);
			DeleteWorkflowTaskButton = Driver.SetDynamicValue(How.XPath, DELETE_TASK_BUTTON, taskNumber.ToString());
			DeleteWorkflowTaskButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Навести курсор на добавленную задачу
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public NewProjectWorkflowPage HoverCursorToTask(int taskNumber)
		{
			CustomTestContext.WriteLine("Навести курсор на добавленную задачу №{0}.", taskNumber);
			AddedWorkflowTaskItem = Driver.SetDynamicValue(How.XPath, WORKFLOW_ADDED_TASK_ITEM, taskNumber.ToString());
			AddedWorkflowTaskItem.HoverElement();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть на кнопку очистки списка активных задач
		/// </summary>
		public NewProjectWorkflowPage ClickClearButton()
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку очистки списка активных задач");
			ClearButton.JavaScriptClick();

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Удалить добавленную задачу
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public NewProjectWorkflowPage DeleteWorkflowTask(int taskNumber)
		{
			CustomTestContext.WriteLine("Удалить добавленную задачу №{0}", taskNumber);
			HoverCursorToTask(taskNumber);
			ClickDeleteTaskButton(taskNumber);

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, совпадает ли кол-во возможных типов задач в списке с ожидаемым на этапе Workflow
		/// </summary>
		/// <param name="expectedCount">ожидаемое кол-во задач</param>
		public bool IsTaskTypesCountMatchExpected(int expectedCount)
		{
			CustomTestContext.WriteLine("Проверить, совпадает ли кол-во возможных типов задач в списке с ожидаемым на этапе Workflow");

			return expectedCount == GetTaskTipesList().Count();
		}

		/// <summary>
		/// Проверить, открылась ли страница Workflow для создаваемого проекта
		/// </summary>
		public bool IsNewProjectWorkflowPageOpened()
		{
			CustomTestContext.WriteLine("Проверить, открылась ли страница Workflow для создаваемого проекта");

			return Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_PROJECT_BUTTON)); 
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Add at least one task to complete project creation'
		/// </summary>
		public bool IsEmptyWorkflowErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Add at least one task to complete project creation'");

			return Driver.WaitUntilElementIsDisplay(By.XPath(EMPTY_WORKFLOW_ERROR));
		}

		/// <summary>
		/// Проверить, что задача на этапе Workflow соответствует ожидаемой
		/// </summary>
		/// <param name="task">ожидаемая задача</param>
		/// <param name="taskNumber">номер задачи</param>
		public bool IsWorkflowTaskMatchExpected(WorkflowTask task, int taskNumber)
		{
			CustomTestContext.WriteLine("Проверить, что задача №{0} на этапе Workflow - это {1}", taskNumber, task);

			return task.ToString() == GetWorkflowAddedTaskList()[taskNumber - 1];
		}

		/// <summary>
		/// Проверить, что количество добавленных задач на этапе Workflow равно ожидаемому
		/// </summary>
		/// <param name="expectedTaskCount">ожидаемое кол-во задач</param>
		public bool IsWorkflowAddedTaskCountMatchExpected(int expectedTaskCount)
		{
			CustomTestContext.WriteLine("Проверить, что количество задач на этапе Workflow = {0}", expectedTaskCount);

			return expectedTaskCount == GetWorkflowAddedTaskList().Count;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CREATE_PROJECT_BUTTON)]
		protected IWebElement CreateProjectButton { get; set; }

		[FindsBy(How = How.XPath, Using = CLEAR_BUTTON)]
		protected IWebElement ClearButton { get; set; }

		protected IWebElement WorkflowTaskTypeItem { get; set; }

		protected IWebElement DeleteWorkflowTaskButton { get; set; }

		protected IWebElement AddedWorkflowTaskItem { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CREATE_PROJECT_BUTTON = "//div[contains(@data-bind,'WorkflowStep')]";
		protected const string WORKFLOW_ADDED_TASK_LIST = "//td[@class='task-item']/div";
		protected const string WORKFLOW_ADDED_TASK_ITEM = "(//td[@class='task-item']/div)[*#*]";
		protected const string WORKFLOW_TASK_TYPE_LIST = "//div[@class='clearfix']//div[contains(@class,'task-name')]";
		protected const string WORKFLOW_TASK_TYPE_ITEM = "//div[@class='clearfix']//div[contains(@class,'task-name')][text()='*#*']";
		protected const string CLEAR_BUTTON = "//a[contains(@data-bind,'clearTasks')]";
		protected const string DELETE_TASK_BUTTON = "//td[@class='task-num'][text()='*#*']//following-sibling::td//i[@class='remove']";
		protected const string EMPTY_WORKFLOW_ERROR = "//div[contains(text(), 'Add at least one task to complete project creation.')]";

		#endregion
	}
}
