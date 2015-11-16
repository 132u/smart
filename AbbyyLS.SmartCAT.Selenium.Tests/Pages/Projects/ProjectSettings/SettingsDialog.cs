using System.Collections.Generic;

using NUnit.Framework;
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

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SETTINGS_DIALOG), timeout:60))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог настроек проекта.");
			}
		}

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
		/// Проверить, что 'Workflow Setup' отсутствует в настройках проекта
		/// </summary>
		public SettingsDialog AssertWorkflowSettingsNotExist()
		{
			CustomTestContext.WriteLine("Проверить, что 'Workflow Setup' отсутствует в настройках проекта.");

			Assert.IsFalse(Driver.GetIsElementExist(By.XPath(WORKFLOW_TAB)),
				"Произошла ошибка:\n 'Workflow Setup' присутствует в настройках проекта.");

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
		/// Рааскрыть комбобокс с задачами
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

		/// <summary>
		/// Нажать кнопку Save
		/// </summary>
		public ProjectSettingsPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save.");
			SaveButton.Click();

			return new ProjectSettingsPage(Driver).GetPage();
		}

		/// <summary>
		/// Проверить, что диалог подтверждения удаления задачи появился.
		/// </summary>
		public SettingsDialog AssertConfirmDeleteDialogDislpay()
		{
			CustomTestContext.WriteLine("Проверить, что диалог подтверждения удаления задачи появился.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_DELETE_DIALOG)),
				"Произошла ошибка:\n не появился диалог подтверждения удаления задачи.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel
		/// </summary>
		public ProjectSettingsPage ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel.");
			CancelButton.Click();

			return new ProjectSettingsPage(Driver).GetPage();
		}

		[FindsBy(How = How.XPath, Using = WORKFLOW_TAB)]
		protected IWebElement WorkflowTab { get; set; }

		[FindsBy(How = How.XPath, Using = NEW_TASK_BUTTON)]
		protected IWebElement NewTaskButton { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		protected IWebElement DeleteTaskButton { get; set; }

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

	}
}