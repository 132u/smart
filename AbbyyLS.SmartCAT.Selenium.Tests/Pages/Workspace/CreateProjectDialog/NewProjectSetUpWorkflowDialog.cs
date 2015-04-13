using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.CreateProjectDialog
{
	public class NewProjectSetUpWorkflowDialog : ProjectsPage, IAbstractPage<NewProjectSetUpWorkflowDialog>
	{
		public new NewProjectSetUpWorkflowDialog GetPage()
		{
			var newProjectSetUpWorkflowDialog = new NewProjectSetUpWorkflowDialog();
			InitPage(newProjectSetUpWorkflowDialog);
			LoadPage();

			return newProjectSetUpWorkflowDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.XPath(WF_TABLE_FIRST_TASK_XPATH), 7))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти к этапу Workflow создания проекта.");
			}
		}

		/// <summary>
		/// Нажать на первую задачу в проекте (чтобы выпал выбадающий список с заданиями )
		/// </summary>
		public NewProjectSetUpWorkflowDialog ClickFirstTask()
		{
			Logger.Debug("Нажать на первую задачу в проекте (чтобы выпал выбадающий список с заданиями ).");
			WFTableFirstTask.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Готово"
		/// </summary>
		public ProjectsPage ClickFinishBtn()
		{
			Logger.Debug("Нажать кнопку 'Готово'.");
			FinishBtn.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Далее"
		/// </summary>
		public NewProjectSetUpTMDialog ClickNextBtn()
		{
			Logger.Debug("Нажать кнопку 'Далее'.");
			NextBtn.Click();
			var newProjectSetUpTMDialog = new NewProjectSetUpTMDialog();

			return newProjectSetUpTMDialog.GetPage();
		}

		[FindsBy(How = How.XPath, Using = WF_TABLE_FIRST_TASK_XPATH)]
		protected IWebElement WFTableFirstTask { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BTN_XPATH)]
		protected IWebElement NextBtn { get; set; }

		[FindsBy(How = How.XPath, Using = FINISH_BTN_XPATH)]
		protected IWebElement FinishBtn { get; set; }

		protected const string WF_TABLE_FIRST_TASK_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//table[contains(@class,'js-workflow-table')]//tr[1]/td[2]//span//span";
		protected const string FINISH_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-finish js-upload-btn')]";
		protected const string NEXT_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-next')]";
	}
}
