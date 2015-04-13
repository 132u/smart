using System.Threading;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace
{
	public class ProjectsPage : WorkspacePage, IAbstractPage<ProjectsPage>
	{
		public new ProjectsPage GetPage()
		{
			var projectPage = new ProjectsPage();
			InitPage(projectPage);
			LoadPage();

			return projectPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.XPath(CREATE_PROJECT_BTN_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти на вкладку \"Проекты\".");
			}
		}

		/// <summary>
		/// Проверить, загрузился ли проект
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage AssertIsProjectLoaded(string projectName)
		{
			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(PROJECT_LOAD_IMG_XPATH.Replace("*#*", projectName)), 50),
				"Произошла ошибка:\n проект {0} не загрузился.", projectName);

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectSettingsPage ClickProjectRef(string projectName)
		{
			Logger.Debug("Кликнуть по ссылке проекта {0}.", projectName);
			//TODO: убрать слип, если можно (похоже на временные глюки, раньше был не нужен)
			Thread.Sleep(1000);
			ProjectRef = Driver.SetDynamicValue(How.XPath, PROJECT_REF_XPATH, projectName);
			try
			{

				ProjectRef.Click();
			}
			catch (StaleElementReferenceException)
			{
				ClickProjectRef(projectName);
			}
			var projectSettingsPage = new ProjectSettingsPage();

			return projectSettingsPage.GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Создать проект"
		/// </summary>
		public NewProjectGeneralInformationDialog ClickCreateProjectBtn()
		{
			Logger.Debug("Нажать на кнопку 'Создать проект'.");
			CreateProjectBtn.Click();
			var createProjectDialog = new NewProjectGeneralInformationDialog();

			return createProjectDialog.GetPage();
		}

		[FindsBy(How = How.XPath, Using = CREATE_PROJECT_BTN_XPATH)]
		protected IWebElement CreateProjectBtn { get; set; }

		protected IWebElement ProjectRef { get; set; }

		protected const string CREATE_PROJECT_BTN_XPATH = "//span[contains(@class,'js-project-create')]";
		protected const string CREATE_PROJECT_DIALOG_XPATH = "//div[contains(@class,'js-popup-create-project')][2]";
		protected const string PROJECT_REF_XPATH = ".//table[contains(@class,'js-tasks-table')]//tr//a[@class='js-name'][string()='*#*']";
		protected const string PROJECT_LOAD_IMG_XPATH = ".//table[contains(@class,'js-tasks-table')]//tr//a[@class='js-name'][string()='*#*']/..//img[contains(@class,'l-project-doc__progress')]";
	}
}
