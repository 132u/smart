using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectSetUpWorkflowDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectSetUpWorkflowDialog>
	{
		public new NewProjectSetUpWorkflowDialog GetPage()
		{
			var newProjectSetUpWorkflowDialog = new NewProjectSetUpWorkflowDialog();
			InitPage(newProjectSetUpWorkflowDialog);

			return newProjectSetUpWorkflowDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(NEW_TASK_BUTTON), 7))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти к этапу Workflow создания проекта.");
			}
		}

		/// <summary>
		/// Нажать на первую задачу в проекте (чтобы выпал выпадающий список с заданиями )
		/// </summary>
		public NewProjectSetUpWorkflowDialog ClickFirstTask()
		{
			Logger.Debug("Нажать на первую задачу в проекте (чтобы выпал выпадающий список с заданиями ).");
			WFTableFirstTask.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Далее"
		/// </summary>
		public NewProjectSetUpTMDialog ClickNextButton()
		{
			Logger.Debug("Нажать кнопку 'Далее'.");
			NextButton.Click();

			return new NewProjectSetUpTMDialog().GetPage();
		}
		
		/// <summary>
		/// Нажать кнопку 'Новая задача'
		/// </summary>
		public NewProjectSetUpWorkflowDialog ClickNewTaskButton()
		{
			Logger.Debug("Нажать кнопку 'Новая задача'");
			NewTaskButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить список задач на этапе Workflow
		/// </summary>
		public List<string> WorkflowTaskList()
		{
			Logger.Trace("Получить список задач на этапе Workflow.");

			return Driver.GetTextListElement(By.XPath(WORKFLOW_TASKS));
		}

		/// <summary>
		/// Раскрыть дропдаун на этапе Workflow
		/// </summary>
		public NewProjectSetUpWorkflowDialog ExpandWorkflowDropdown(int taskNumber)
		{
			Logger.Trace("Раскрыть дропдаун для задачи №{0}", taskNumber);
			var task = Driver.SetDynamicValue(How.XPath, WORKFLOW_TASK, taskNumber.ToString());
			task.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить список опций для задачи
		/// </summary>
		public List<string> TaskOptionsList(int taskNumber)
		{
			Logger.Trace("Получить список опций для задачи №{0}", taskNumber);
			var workflowList = Driver.GetElementList(By.XPath(WORKFLOW_DROPDOWN_LIST));
			
			return workflowList.Select(taskOption => taskOption.Text).ToList();
		}

		/// <summary>
		/// Выбрать задачу на этапе Workflow
		/// </summary>
		public NewProjectSetUpWorkflowDialog SelectTask(WorkflowTask workflowTask)
		{
			Logger.Trace("Выбрать {0}.", workflowTask);
			var taskOption = Driver.GetElementList(By.XPath(WORKFLOW_DROPDOWN_LIST)).FirstOrDefault(item => item.Text == workflowTask.ToString());
			
			Assert.NotNull(taskOption, "Произошла ошибка:\n {0} задача не найдена, вовзращено значение null.", taskOption);

			taskOption.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку удаления
		/// </summary>
		public NewProjectSetUpWorkflowDialog ClickDeleteButton(int taskNumber)
		{
			Logger.Debug("Удалить задачу №{0}.", taskNumber);
			var deleteButton = Driver.SetDynamicValue(How.XPath, DELETE_TASK_BUTTON, taskNumber.ToString());
			deleteButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Select at least one task'
		/// </summary>
		public NewProjectSetUpWorkflowDialog AssertEmptyWorkflowErrorDisplayed()
		{
			Logger.Trace("Проверить, что появилось сообщение 'Select at least one task'.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(EMPTY_WORKFLOW_ERROR)),
				"Произошла ошибка:\n Сообщение 'Select at least one task' не появилось.");

			return GetPage();
		}
		
		[FindsBy(How = How.XPath, Using = WF_TABLE_FIRST_TASK)]
		protected IWebElement WFTableFirstTask { get; set; }
		
		[FindsBy(How = How.XPath, Using = NEW_TASK_BUTTON)]
		protected IWebElement NewTaskButton { get; set; }

		protected const string WF_TABLE_FIRST_TASK = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-workflow-table')]//tr[1]/td[2]//span//span";
		protected const string NEW_TASK_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-new-stage')]";
		protected const string WORKFLOW_TASKS = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-workflow-table')]//tbody//tr//td[2]//span//span";
		protected const string WORKFLOW_TASK = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-workflow-table')]//tbody//tr[*#*]//td[2]//span//span";
		protected const string WORKFLOW_DROPDOWN_LIST = "//span[contains(@class,'js-dropdown__item')]";
		protected const string DELETE_TASK_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//tr[*#*]//a[contains(@class,'js-delete-workflow')]";
		protected const string EMPTY_WORKFLOW_ERROR = "//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-workflow-empty')]";
	}
}
