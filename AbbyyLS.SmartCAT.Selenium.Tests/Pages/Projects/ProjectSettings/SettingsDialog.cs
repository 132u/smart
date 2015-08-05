using System.Linq;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class SettingsDialog : ProjectSettingsPage, IAbstractPage<SettingsDialog>
	{
		public new SettingsDialog GetPage()
		{
			var settingsDialog = new SettingsDialog();
			InitPage(settingsDialog);

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
			Logger.Debug("Нажать на вкладку 'Workflow'.");
			WorkflowTab.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку удаления задачи
		/// </summary>
		/// <param name="taskNumber"></param>
		public SettingsDialog ClickDeleteTaskButton(int taskNumber = 1)
		{
			Logger.Debug("Нажать кнопку удаления задачи №{0}", taskNumber);

			DeleteTaskButton = Driver.SetDynamicValue(How.XPath, DELETE_TASK_BUTTON, taskNumber.ToString());
			DeleteTaskButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'New Task'
		/// </summary>
		public SettingsDialog ClickNewTaskButton()
		{
			Logger.Debug("Нажать кнопку 'New Task'.");
			NewTaskButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Рааскрыть комбобокс с задачами
		/// </summary>
		public SettingsDialog ExpandTask(int taskNumber)
		{
			Logger.Debug("Раскрыть комбобокс №{0} с задачами.", taskNumber);
			Driver.SetDynamicValue(How.XPath, TASK, taskNumber.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по задаче
		/// </summary>
		public SettingsDialog ClickTaskInDropdown(TaskMode taskName)
		{
			Logger.Debug("Кликнуть по задаче {0}.", taskName);
			var task = Driver.GetElementList(By.XPath(TASK_LIST)).FirstOrDefault(t => t.Text == taskName.ToString());

			Assert.NotNull(task, "Произошла ошибка:\n задача {0} не найдена, вовзращено значение null.", taskName);

			task.Click();
			
			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Save
		/// </summary>
		public ProjectSettingsPage ClickSaveButton()
		{
			Logger.Debug("Нажать кнопку Save.");
			SaveButton.Click();

			return new ProjectSettingsPage().GetPage();
		}

		/// <summary>
		/// Проверить, что диалог подтверждения удаления задачи появился.
		/// </summary>
		public SettingsDialog AssertConfirmDeleteDialogDislpay()
		{
			Logger.Trace("Проверить, что диалог подтверждения удаления задачи появился.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_DELETE_DIALOG)),
				"Произошла ошибка:\n не появился диалог подтверждения удаления задачи.");

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = WORKFLOW_TAB)]
		protected IWebElement WorkflowTab { get; set; }

		[FindsBy(How = How.XPath, Using = NEW_TASK_BUTTON)]
		protected IWebElement NewTaskButton { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		protected IWebElement DeleteTaskButton { get; set; }

		protected const string SETTINGS_DIALOG = "(//div[contains(@class,'js-popup-edit')])[2]";
		protected const string WORKFLOW_TAB = "(//div[contains(@class,'js-popup-edit')])[2]//a[contains(@data-bind,'workflowTab')]";
		protected const string DELETE_TASK_BUTTON = "(//div[contains(@class,'js-popup-edit')][2]//a[contains(@data-bind,'deleteWorkflowStage')])[*#*]";
		protected const string CONFIRM_DELETE_DIALOG = "//div[contains(@class,'js-popup-confirm')]";
		protected const string NEW_TASK_BUTTON = "//div[@class='g-popup-bd js-popup-bd js-popup-edit'][2]//span[contains(@data-bind, 'addWorkflowStage')]";
		protected const string TASK = "//div[contains(@class,'js-popup-edit')][2]//tbody[@data-bind='foreach: workflowStages']//tr[*#*]//td[2]//span//span";
		protected const string TASK_LIST = "//span[contains(@class,'js-dropdown__item')]";
		protected const string SAVE_BUTTON = "//div[@class='g-popup-bd js-popup-bd js-popup-edit'][2]//div[@class='g-popupbox__ft']//span//span/a";
	}
}