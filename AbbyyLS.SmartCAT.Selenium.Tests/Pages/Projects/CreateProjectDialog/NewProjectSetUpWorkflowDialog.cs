using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

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
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(WF_TABLE_FIRST_TASK), 7))
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
		public ProjectsPage ClickFinishButton()
		{
			Logger.Debug("Нажать кнопку 'Готово'.");
			CreateProjectFinishButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Далее"
		/// </summary>
		public NewProjectSetUpTMDialog ClickNextButton()
		{
			Logger.Debug("Нажать кнопку 'Далее'.");
			NextButton.Click();
			var newProjectSetUpTMDialog = new NewProjectSetUpTMDialog();

			return newProjectSetUpTMDialog.GetPage();
		}

		[FindsBy(How = How.XPath, Using = WF_TABLE_FIRST_TASK)]
		protected IWebElement WFTableFirstTask { get; set; }

		protected const string WF_TABLE_FIRST_TASK = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-workflow-table')]//tr[1]/td[2]//span//span";
		
	}
}
