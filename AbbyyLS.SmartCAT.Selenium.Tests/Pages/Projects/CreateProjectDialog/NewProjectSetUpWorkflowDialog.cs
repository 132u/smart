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
		/// Нажать кнопку 'Назад'
		/// </summary>
		public NewProjectGeneralInformationDialog ClickBack()
		{
			Logger.Debug("Нажать кнопку 'Назад'.");
			BackButton.Click();

			return new NewProjectGeneralInformationDialog().GetPage();
		}

		[FindsBy(How = How.XPath, Using = BACK_BUTTON)]
		protected IWebElement BackButton { get; set; }


		[FindsBy(How = How.XPath, Using = WF_TABLE_FIRST_TASK)]
		protected IWebElement WFTableFirstTask { get; set; }

		protected const string WF_TABLE_FIRST_TASK = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-workflow-table')]//tr[1]/td[2]//span//span";
		protected const string BACK_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-back')]";

	}
}
