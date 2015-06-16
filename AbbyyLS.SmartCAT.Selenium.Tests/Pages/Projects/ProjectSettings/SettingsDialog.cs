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
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SETTINGS_DIALOG)))
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

		protected IWebElement DeleteTaskButton { get; set; }

		protected const string SETTINGS_DIALOG = "(//div[contains(@class,'js-popup-edit')])[2]";
		protected const string WORKFLOW_TAB = "(//div[contains(@class,'js-popup-edit')])[2]//a[contains(@data-bind,'workflowTab')]";
		protected const string DELETE_TASK_BUTTON = "(//div[contains(@class,'js-popup-edit')][2]//a[contains(@data-bind,'deleteWorkflowStage')])[*#*]";
		protected const string CONFIRM_DELETE_DIALOG = "//div[contains(@class,'js-popup-confirm')]";
	}
}