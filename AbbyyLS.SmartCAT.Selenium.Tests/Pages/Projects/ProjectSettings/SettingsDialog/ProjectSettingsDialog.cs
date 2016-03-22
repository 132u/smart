using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog
{
	public class ProjectSettingsDialog : ProjectSettingsPage, IAbstractPage<ProjectSettingsDialog>
	{
		public ProjectSettingsDialog(WebDriver driver) : base(driver)
		{
		}

		public new ProjectSettingsDialog GetPage()
		{
			var settingsDialog = new ProjectSettingsDialog(Driver);
			InitPage(settingsDialog, Driver);

			return settingsDialog;
		}

		public new void LoadPage()
		{
			if (!IsSettingsDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог настроек проекта");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на вкладку 'Workflow'.
		/// </summary>
		public ProjectSettingsDialog ClickWorkflowTab()
		{
			CustomTestContext.WriteLine("Нажать на вкладку 'Workflow'.");
			WorkflowTab.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на вкладку General.
		/// </summary>
		public GeneralTab ClickGeneralTab()
		{
			CustomTestContext.WriteLine("Нажать на вкладку General.");
			GeneralTab.Click();

			return new GeneralTab(Driver).GetPage();
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

			return new ProjectSettingsPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Save и дождаться закрытия окна
		/// </summary>
		public ProjectsPage SaveSettingsExpectingProjectsPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save и дождаться закрытия окна");
			SaveButton.Click();

			return new ProjectsPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel и дождаться закрытия окна
		/// </summary>
		public ProjectSettingsPage CancelSettingsChanges()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel и дождаться закрытия окна");
			CancelButton.Click();

			return new ProjectSettingsPage(Driver).GetPage();
		}
		
		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылся ли диалог настроек проекта
		/// </summary>
		public bool IsSettingsDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SETTINGS_DIALOG));
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

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		protected IWebElement DeleteTaskButton { get; set; }

		[FindsBy(How = How.XPath, Using = GEBERAL_TAB)]
		protected IWebElement GeneralTab { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CANCEL_BUTTON = "//div[contains(@class,'js-popup-edit')][2]//a[contains(@class,'js-popup-close')]";
		protected const string SETTINGS_DIALOG = "(//div[contains(@class,'js-popup-edit')])[2]";
		protected const string WORKFLOW_TAB = "(//div[contains(@class,'js-popup-edit')])[2]//a[contains(@data-bind,'workflowTab')]";
		protected const string DELETE_TASK_BUTTON = "(//div[contains(@class,'js-popup-edit')][2]//a[contains(@data-bind,'deleteWorkflowStage')])[*#*]";
		protected const string CONFIRM_DELETE_DIALOG = "//div[contains(@class,'js-popup-confirm')]";
		protected const string SAVE_BUTTON = "//div[contains(@class, 'js-popup-edit') and contains(@style, 'display: block')]//div[@class='g-popupbox__ft']//div//a";
		protected const string WORKFLOW_LIST = "//div[contains(@class,'js-popup-edit')][2]//tbody[@data-bind='foreach: workflowStages']//tr//td[2]//span//span";
		protected const string GEBERAL_TAB = "(//div[contains(@class,'js-popup-edit')])[2]//a[contains(@data-bind,'general')]";
		#endregion
	}
}