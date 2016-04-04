using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectCreateBaseDialog : ProjectsPage, IAbstractPage<NewProjectCreateBaseDialog>
	{
		public NewProjectCreateBaseDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewProjectCreateBaseDialog LoadPage()
		{
			if (!IsProjectCreateDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось загрузить диалог создания проекта");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать 'Next'
		/// </summary>
		public T ClickNextButton<T>() where T: class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Нажать 'Next'.");
			NextButton.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.LoadPage();
		}

		/// <summary>
		/// Нажать 'Back'
		/// </summary>
		public T ClickBackButton<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Нажать 'Back'.");
			BackButton.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.LoadPage();
		}

		/// <summary>
		/// Нажать на ссылку Files в навигационном меню
		/// </summary>
		public NewProjectDocumentUploadPage ClickFilesLink()
		{
			CustomTestContext.WriteLine("Нажать на ссылку Files в навигационном меню");
			FilesLink.Click();

			return new NewProjectDocumentUploadPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на ссылку Settings в навигационном меню
		/// </summary>
		public NewProjectSettingsPage ClickSettingsLink()
		{
			CustomTestContext.WriteLine("Нажать на ссылку Settings в навигационном меню");
			SettingsLink.Click();

			return new NewProjectSettingsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на ссылку Workflow в навигационном меню
		/// </summary>
		public NewProjectWorkflowPage ClickWorkflowLink()
		{
			CustomTestContext.WriteLine("Нажать на ссылку Workflow в навигационном меню");
			WorkflowLink.Click();

			return new NewProjectWorkflowPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Нажать кнопку 'Готово'
		/// </summary>
		public ProjectsPage ClickFinishButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Готово'.");
			CreateProjectFinishButton.HoverElement();
			CreateProjectFinishButton.JavaScriptClick();

			WaitCreateProjectDialogDisappear();

			return new ProjectsPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыт ли диалог создания нового проекта
		/// </summary>
		public bool IsProjectCreateDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_PROJECT_DIALOG));
		}

		/// <summary>
		/// Проверить, что кнопка 'Готова' доступна
		/// </summary>
		public bool IsFinishButtonEnabled()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Готово' доступна");

			return Driver.WaitUntilElementIsEnabled(By.XPath(CREATE_PROJECT_FINISH_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CREATE_PROJECT_FINISH_BUTTON)]
		protected IWebElement CreateProjectFinishButton { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON)]
		protected IWebElement NextButton { get; set; }

		[FindsBy(How = How.XPath, Using = BACK_BUTTON)]
		protected IWebElement BackButton { get; set; }

		[FindsBy(How = How.XPath, Using = FILES_LINK)]
		protected IWebElement FilesLink { get; set; }

		[FindsBy(How = How.XPath, Using = SETTINGS_LINK)]
		protected IWebElement SettingsLink { get; set; }

		[FindsBy(How = How.XPath, Using = WORKFLOW_LINK)]
		protected IWebElement WorkflowLink { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CREATE_PROJECT_DIALOG = "//div[contains(@class,'js-popup-create-project')][2]";
		protected const string CREATE_PROJECT_FINISH_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-finish js-upload-btn')]";
		protected const string CLOSE_DIALOG_BTN = "//div[contains(@class,'js-popup-create-project')][2]//img[contains(@class,'js-popup-close')]";
		protected const string NEXT_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-next')]";
		protected const string BACK_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-back')]";

		protected const string FILES_LINK = "//ul[contains(@data-bind, 'steps')]//li[1]//span";
		protected const string SETTINGS_LINK = "//ul[contains(@data-bind, 'steps')]//li[3]//span";
		protected const string WORKFLOW_LINK = "//ul[contains(@data-bind, 'steps')]//li[5]//span";

		#endregion
	}
}
