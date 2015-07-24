using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectCreateBaseDialog : ProjectsPage, IAbstractPage<NewProjectCreateBaseDialog>
	{
		public new NewProjectCreateBaseDialog GetPage()
		{
			var createProjectDialog = new NewProjectCreateBaseDialog();
			InitPage(createProjectDialog);

			return createProjectDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_PROJECT_DIALOG)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось загрузить диалог создания проекта.");
			}
		}

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

		/// <summary>
		/// Нажать 'Next'
		/// </summary>
		public T ClickNextButton<T>() where T: class, IAbstractPage<T>, new()
		{
			Logger.Debug("Нажать 'Next'.");
			NextButton.Click();

			return new T().GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка 'Готова' доступна
		/// </summary>
		public NewProjectCreateBaseDialog AssertFinishButtonEnabled()
		{
			Logger.Debug("Проверить, что кнопка 'Готово' доступна");

			Assert.IsTrue(Driver.WaitUntilElementIsEnabled(By.XPath(CREATE_PROJECT_FINISH_BUTTON)) && Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_PROJECT_FINISH_BUTTON)),
				"Ошибка: \n кнопка 'Готово' недоступна.");

			return GetPage();
		}


		/// <summary>
		/// Нажать кнопку 'Готово'
		/// </summary>
		public ProjectsPage ClickFinishButton()
		{
			Logger.Debug("Нажать кнопку 'Готово'.");
			CreateProjectFinishButton.HoverElement();
			CreateProjectFinishButton.JavaScriptClick();

			return new ProjectsPage().GetPage();
		}

		[FindsBy(How = How.XPath, Using = CREATE_PROJECT_FINISH_BUTTON)]
		protected IWebElement CreateProjectFinishButton { get; set; }

		[FindsBy(How = How.XPath, Using = CLOSE_DIALOG_BTN)]
		protected IWebElement CloseDialogButton { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON)]
		protected IWebElement NextButton { get; set; }

		protected const string CREATE_PROJECT_DIALOG = "//div[contains(@class,'js-popup-create-project')][2]";
		protected const string CREATE_PROJECT_FINISH_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-finish js-upload-btn')]";
		protected const string CLOSE_DIALOG_BTN = "//div[contains(@class,'js-popup-create-project')][2]//img[contains(@class,'js-popup-close')]";
		protected const string NEXT_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-next')]";
	}
}
