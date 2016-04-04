using System.Collections.Generic;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectSetUpWorkflowDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectSetUpWorkflowDialog>
	{
		public NewProjectSetUpWorkflowDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewProjectSetUpWorkflowDialog LoadPage()
		{
			if (!IsNewProjectSetUpWorkflowDialogOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не удалось перейти к этапу Workflow создания проекта.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на первую задачу в проекте (чтобы выпал выпадающий список с заданиями )
		/// </summary>
		public NewProjectSetUpWorkflowDialog ClickFirstTask()
		{
			CustomTestContext.WriteLine("Нажать на первую задачу в проекте (чтобы выпал выпадающий список с заданиями ).");
			WFTableFirstTask.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку "Далее"
		/// </summary>
		public NewProjectSetUpTMDialog ClickNextButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Далее'.");
			NextButton.Click();

			return new NewProjectSetUpTMDialog(Driver).LoadPage();
		}
		
		/// <summary>
		/// Нажать кнопку 'Новая задача'
		/// </summary>
		public NewProjectSetUpWorkflowDialog ClickNewTaskButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Новая задача'");
			NewTaskButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить список задач на этапе Workflow
		/// </summary>
		public List<string> WorkflowTaskList()
		{
			CustomTestContext.WriteLine("Получить список задач на этапе Workflow.");

			return Driver.GetTextListElement(By.XPath(WORKFLOW_TASKS));
		}

		/// <summary>
		/// Раскрыть дропдаун на этапе Workflow
		/// </summary>
		public NewProjectSetUpWorkflowDialog ExpandWorkflowDropdown(int taskNumber)
		{
			CustomTestContext.WriteLine("Раскрыть дропдаун для задачи №{0}", taskNumber);
			var task = Driver.SetDynamicValue(How.XPath, WORKFLOW_TASK, taskNumber.ToString());
			task.Click();

			return LoadPage();
		}

		/// <summary>
		/// Удалить задачу, нажав на кнопку удаления
		/// </summary>
		public NewProjectSetUpWorkflowDialog DeleteWorkflowTask(int taskNumber)
		{
			CustomTestContext.WriteLine("Удалить задачу №{0}.", taskNumber);
			var deleteButton = Driver.SetDynamicValue(How.XPath, DELETE_TASK_BUTTON, taskNumber.ToString());
			deleteButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить список опций для задачи
		/// </summary>
		public List<string> TaskOptionsList(int taskNumber)
		{
			CustomTestContext.WriteLine("Получить список опций для задачи №{0}", taskNumber);
			var workflowList = Driver.GetElementList(By.XPath(WORKFLOW_DROPDOWN_LIST));
			
			return workflowList.Select(taskOption => taskOption.Text).ToList();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать задачу
		/// </summary>
		/// <param name="task">тип задачи</param>
		/// <param name="taskNumber">номер задачи</param>
		public NewProjectSetUpWorkflowDialog SelectWorkflowTask(WorkflowTask task, int taskNumber = 1)
		{
			ExpandWorkflowDropdown(taskNumber);

			CustomTestContext.WriteLine("Выбрать {0}.", task);
			var taskOption = Driver.GetElementList(By.XPath(WORKFLOW_DROPDOWN_LIST)).FirstOrDefault(item => item.Text == task.ToString());

			if (taskOption == null)
			{
				throw new XPathLookupException("Произошла ошибка:\n задача не найдена");
			}

			taskOption.Click();

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что появилось сообщение 'Select at least one task'
		/// </summary>
		public bool IsEmptyWorkflowErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Select at least one task'");

			return Driver.WaitUntilElementIsDisplay(By.XPath(EMPTY_WORKFLOW_ERROR));
		}

		/// <summary>
		/// Проверить, совпадает ли кол-во задач в дропдауне с ожидаемым на этапе Workflow
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		/// <param name="expectedCount">ожидаемое кол-во задач</param>
		public bool IsTaskOptionsCountMatchExpected(int taskNumber, int expectedCount)
		{
			CustomTestContext.WriteLine("Проверить, совпадает ли кол-во задач в дропдауне с ожидаемым на этапе Workflow");

			return expectedCount == TaskOptionsList(taskNumber).Count;
		}

		/// <summary>
		/// Проверить, что задача на этапе Workflow соответствует ожидаемой
		/// </summary>
		/// <param name="task">ожидаемая задача</param>
		/// <param name="taskNumber">номер задачи</param>
		public bool IsWorkflowTaskMatchExpected(WorkflowTask task, int taskNumber)
		{
			CustomTestContext.WriteLine("Проверить, что задача №{0} на этапе Workflow - это {1}", taskNumber, task);

			return task.ToString() == WorkflowTaskList()[taskNumber - 1];
		}

		/// <summary>
		/// Проверить, что количество задач на этапе Workflow равно ожидаемому
		/// </summary>
		/// <param name="taskCount">ожидаемое кол-во задач</param>
		public bool IsWorkflowTaskCountMatchExpected(int taskCount)
		{
			CustomTestContext.WriteLine("Проверить, что количество задач на этапе Workflow = {0}", taskCount);

			return taskCount == WorkflowTaskList().Count;
		}

		/// <summary>
		/// Проверить, открыт ли этап Workflow диалога создания проекта
		/// </summary>
		public bool IsNewProjectSetUpWorkflowDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(NEW_TASK_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = WF_TABLE_FIRST_TASK)]
		protected IWebElement WFTableFirstTask { get; set; }
		
		[FindsBy(How = How.XPath, Using = NEW_TASK_BUTTON)]
		protected IWebElement NewTaskButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string WF_TABLE_FIRST_TASK = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-workflow-table')]//tr[1]/td[2]//span//span";
		protected const string NEW_TASK_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-new-stage')]";
		protected const string WORKFLOW_TASKS = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-workflow-table')]//tbody//tr//td[2]//span//span";
		protected const string WORKFLOW_TASK = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-workflow-table')]//tbody//tr[*#*]//td[2]//span//span";
		protected const string WORKFLOW_DROPDOWN_LIST = "//span[contains(@class,'js-dropdown__item')]";
		protected const string DELETE_TASK_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//tr[*#*]//a[contains(@class,'js-delete-workflow')]";
		protected const string EMPTY_WORKFLOW_ERROR = "//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-workflow-empty')]";

		#endregion
	}
}
