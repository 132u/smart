using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public abstract class NewProjectCreateBaseDialog : ProjectsPage
	{
		/// <summary>
		/// Нажать 'Finish' в диалоге создания проекта.
		/// </summary>
		public ProjectsPage ClickFinishCreate()
		{
			Logger.Debug("Нажать на кнопку 'Finish'.");
			CreateProjectFinishButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать 'Close'
		/// </summary>
		public ProjectsPage ClickCloseDialog()
		{
			Logger.Debug("Нажать 'Close'.");
			CloseDialogButton.Click();

			return new ProjectsPage().GetPage();
		}

		[FindsBy(How = How.XPath, Using = CREATE_PROJECT_FINISH_BUTTON)]
		protected IWebElement CreateProjectFinishButton { get; set; }

		[FindsBy(How = How.XPath, Using = CLOSE_DIALOG_BTN_XPATH)]
		protected IWebElement CloseDialogButton { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BTN)]
		protected IWebElement NextButton { get; set; }
		
		protected const string CREATE_PROJECT_FINISH_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-finish js-upload-btn')]";
		protected const string CLOSE_DIALOG_BTN_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//img[contains(@class,'js-popup-close')]";
		protected const string NEXT_BTN = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-next')]";

	}
}
