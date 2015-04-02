using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.CreateProjectDialog
{	
	public class NewProjectSetUpMTDialog : ProjectsPage, IAbstractPage<NewProjectSetUpMTDialog>
	{
		public new NewProjectSetUpMTDialog GetPage()
		{
			var newProjectSetUpMTDialog = new NewProjectSetUpMTDialog();
			InitPage(newProjectSetUpMTDialog);
			LoadPage();

			return newProjectSetUpMTDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.XPath(MT_TABLE_XPATH), 8))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти к четвертому шагу создания проекта (выбор МТ).");
			}
		}

		/// <summary>
		/// Нажать кнопку "Далее"
		/// </summary>
		public NewProjectSetUpWorkflowDialog ClickNextBtn()
		{
			Logger.Trace("Нажимаем кнопку 'Далее'.");
			NextBtn.Click();
			var newProjectSetUpWorkflowDialog= new NewProjectSetUpWorkflowDialog();

			return newProjectSetUpWorkflowDialog.GetPage();
		}

		[FindsBy(How = How.XPath, Using = NEXT_BTN_XPATH)]
		protected IWebElement NextBtn { get; set; }

		[FindsBy(How = How.XPath, Using = FINISH_BTN_XPATH)]
		protected IWebElement FinishBtn { get; set; }

		protected const string MT_TABLE_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//table[contains(@class,'js-mts-body')]//tbody";
		protected const string NEXT_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-next')]";
		protected const string FINISH_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-finish js-upload-btn')]";
	}
}
