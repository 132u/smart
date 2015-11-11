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

		public new NewProjectCreateBaseDialog GetPage()
		{
			var createProjectDialog = new NewProjectCreateBaseDialog(Driver);
			InitPage(createProjectDialog, Driver);

			return createProjectDialog;
		}

		public new void LoadPage()
		{
			if (!IsProjectCreateDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось загрузить диалог создания проекта");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать 'Close'
		/// </summary>
		public ProjectsPage ClickCloseDialog()
		{
			CustomTestContext.WriteLine("Нажать 'Close'.");
			CloseDialogButton.Click();

			return new ProjectsPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать 'Next'
		/// </summary>
		public T ClickNextButton<T>() where T: class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Нажать 'Next'.");
			NextButton.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.GetPage();
		}

		/// <summary>
		/// Нажать 'Back'
		/// </summary>
		public T ClickBackButton<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Нажать 'Back'.");
			BackButton.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Отменить создание проекта
		/// </summary>
		public ProjectsPage CancelCreateProject()
		{
			var projectsPage = ClickCloseDialog()
				.WaitCreateProjectDialogDisappear()
				.AssertDialogBackgroundDisappeared<ProjectsPage>(Driver);

			return projectsPage;
		}

		/// <summary>
		/// Нажать кнопку 'Готово'
		/// </summary>
		public ProjectsPage ClickFinishButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Готово'.");
			CreateProjectFinishButton.HoverElement();
			CreateProjectFinishButton.JavaScriptClick();

			WaitCreateProjectDialogDisappear();
			AssertDialogBackgroundDisappeared<ProjectsPage>(Driver);

			return new ProjectsPage(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыт ли диалог создания нового проекта
		/// </summary>
		public bool IsProjectCreateDialogOpened()
		{
			CustomTestContext.WriteLine("Проверить, открыт ли диалог создания нового проекта");

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

		[FindsBy(How = How.XPath, Using = CLOSE_DIALOG_BTN)]
		protected IWebElement CloseDialogButton { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON)]
		protected IWebElement NextButton { get; set; }

		[FindsBy(How = How.XPath, Using = BACK_BUTTON)]
		protected IWebElement BackButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CREATE_PROJECT_DIALOG = "//div[contains(@class,'js-popup-create-project')][2]";
		protected const string CREATE_PROJECT_FINISH_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-finish js-upload-btn')]";
		protected const string CLOSE_DIALOG_BTN = "//div[contains(@class,'js-popup-create-project')][2]//img[contains(@class,'js-popup-close')]";
		protected const string NEXT_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-next')]";
		protected const string BACK_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-back')]";

		#endregion
	}
}
