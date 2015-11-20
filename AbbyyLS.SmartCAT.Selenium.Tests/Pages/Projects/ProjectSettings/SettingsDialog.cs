using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class SettingsDialog : ProjectSettingsPage, IAbstractPage<SettingsDialog>
	{
		public SettingsDialog(WebDriver driver) : base(driver)
		{
		}

		public new SettingsDialog GetPage()
		{
			var settingsDialog = new SettingsDialog(Driver);
			InitPage(settingsDialog, Driver);

			return settingsDialog;
		}

		public new void LoadPage()
		{
			Driver.WaitPageTotalLoad();

			if (!IsSettingsDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог настроек проекта");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на вкладку 'Workflow'.
		/// </summary>
		public SettingsDialog ClickWorkflowTab()
		{
			CustomTestContext.WriteLine("Нажать на вкладку 'Workflow'.");
			WorkflowTab.Click();

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
		public SettingsDialog ClickDeleteTaskButton(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления задачи №{0}", taskNumber);
			DeleteTaskButton = Driver.SetDynamicValue(How.XPath, DELETE_TASK_BUTTON, taskNumber.ToString());
			DeleteTaskButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'New Task'
		/// </summary>
		public SettingsDialog ClickNewTaskButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'New Task'.");
			NewTaskButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Раскрыть комбобокс с задачами
		/// </summary>
		public SettingsDialog ExpandTask(int taskNumber)
		{
			CustomTestContext.WriteLine("Раскрыть комбобокс №{0} с задачами.", taskNumber);
			Driver.SetDynamicValue(How.XPath, TASK, taskNumber.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по задаче
		/// </summary>
		public SettingsDialog ClickTaskInDropdown(WorkflowTask taskName)
		{
			CustomTestContext.WriteLine("Кликнуть по задаче {0}.", taskName);
			var task = Driver.SetDynamicValue(How.XPath, TASK_LIST, taskName.ToString());

			task.Click();
			
			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Нажать кнопку Save и дождаться закрытия окна
		/// </summary>
		public ProjectSettingsPage SaveSettings()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save и дождаться закрытия окна");
			SaveButton.Click();
			WaitUntilSettingsDialogDissappear();

			return new ProjectSettingsPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel и дождаться закрытия окна
		/// </summary>
		public ProjectSettingsPage CancelSettingsChanges()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel и дождаться закрытия окна");
			CancelButton.Click();
			WaitUntilSettingsDialogDissappear();

			return new ProjectSettingsPage(Driver).GetPage();
		}

		/// <summary>
		/// Добавить задачу Workflow
		/// </summary>
		/// <param name="task">задача</param>
		/// <param name="taskNumber">номер задачи</param>
		public SettingsDialog AddTask(WorkflowTask task, int taskNumber = 2)
		{
			ClickNewTaskButton();
			ExpandTask(taskNumber);
			ClickTaskInDropdown(task);

			return GetPage();
		}

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

		#endregion

		#region Методы, проверяющие состояние страницы

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
		/// Проверить, открылся ли диалог настроек проекта
		/// </summary>
		public bool IsSettingsDialogOpened()
		{
			CustomTestContext.WriteLine("Проверить, открылся ли диалог настроек проекта");

			return Driver.WaitUntilElementIsDisplay(By.XPath(SETTINGS_DIALOG), timeout: 60);
		}

		/// <summary>
		/// Проверить, что 'Workflow Setup' присутствует в настройках проекта
		/// </summary>
		public bool IsWorkflowSetupExistInSettings()
		{
			CustomTestContext.WriteLine("Проверить, что 'Workflow Setup' присутствует в настройках проекта");

			return Driver.WaitUntilElementIsDisplay(By.XPath(WORKFLOW_TAB));
		}

		/// <summary>
		/// Проверить, что диалог подтверждения удаления задачи появился.
		/// </summary>
		public bool IsConfirmDeleteDialogDislpayed()
		{
			CustomTestContext.WriteLine("Проверить, что диалог подтверждения удаления задачи появился");

			return Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_DELETE_DIALOG));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = WORKFLOW_TAB)]
		protected IWebElement WorkflowTab { get; set; }

		[FindsBy(How = How.XPath, Using = NEW_TASK_BUTTON)]
		protected IWebElement NewTaskButton { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		protected IWebElement DeleteTaskButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CANCEL_BUTTON = "//div[contains(@class,'js-popup-edit')][2]//a[contains(@class,'js-popup-close')]";
		protected const string SETTINGS_DIALOG = "(//div[contains(@class,'js-popup-edit')])[2]";
		protected const string WORKFLOW_TAB = "(//div[contains(@class,'js-popup-edit')])[2]//a[contains(@data-bind,'workflowTab')]";
		protected const string DELETE_TASK_BUTTON = "(//div[contains(@class,'js-popup-edit')][2]//a[contains(@data-bind,'deleteWorkflowStage')])[*#*]";
		protected const string CONFIRM_DELETE_DIALOG = "//div[contains(@class,'js-popup-confirm')]";
		protected const string NEW_TASK_BUTTON = "//div[@class='g-popup-bd js-popup-bd js-popup-edit'][2]//div[contains(@data-bind, 'addWorkflowStage')]";
		protected const string TASK = "//div[contains(@class,'js-popup-edit')][2]//tbody[@data-bind='foreach: workflowStages']//tr[*#*]//td[2]//span//span";
		protected const string TASK_LIST = "//span[contains(@class,'js-dropdown__item') and @title='*#*']";
		protected const string SAVE_BUTTON = "//div[@class='g-popup-bd js-popup-bd js-popup-edit'][2]//div[@class='g-popupbox__ft']//div//a";
		protected const string WORKFLOW_LIST = "//div[contains(@class,'js-popup-edit')][2]//tbody[@data-bind='foreach: workflowStages']//tr//td[2]//span//span";

		#endregion
	}
}