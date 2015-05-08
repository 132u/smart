﻿using System.Threading;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class ProjectsPage : WorkspacePage, IAbstractPage<ProjectsPage>
	{
		public new ProjectsPage GetPage()
		{
			var projectPage = new ProjectsPage();
			InitPage(projectPage);

			return projectPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_PROJECT_BTN_XPATH)))
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
		public ProjectSettingsPage ClickProject(string projectName)
		{
			Logger.Debug("Кликнуть по ссылке проекта {0}.", projectName);
			ProjectRef = Driver.SetDynamicValue(How.XPath, PROJECT_REF_XPATH, projectName);
			try
			{
				ProjectRef.Click();
			}
			catch (StaleElementReferenceException)
			{
				ClickProject(projectName);
			}

			return new ProjectSettingsPage().GetPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Создать проект'
		/// </summary>
		public NewProjectGeneralInformationDialog ClickCreateProjectButton()
		{
			Logger.Debug("Нажать на кнопку 'Создать проект'.");
			CreateProjectButton.Click();

			return new NewProjectGeneralInformationDialog().GetPage();
		}

		/// <summary>
		/// Проверить, что проект появился в списке проектов
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage CheckProjectAppearInList(string projectName)
		{
			Logger.Trace("Дождаться появления проекта {0} в списке", projectName);
			ProjectRef = Driver.SetDynamicValue(How.XPath, PROJECT_REF_XPATH, projectName);

			Assert.IsTrue(ProjectRef.Displayed,
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", projectName);

			return GetPage();
		}

		/// <summary>
		/// Дождаться закрытия диалога создания проекта
		/// </summary>
		public ProjectsPage WaitCreateProjectDialogDissapeared()
		{
			Logger.Trace("Дождаться закрытия диалога создания проекта.");

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(CREATE_PROJECT_DIALOG_XPATH)),
				"Произошла ошибка:\n диалог создания проекта не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Отметить чекбокс проекта в списке
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage SelectProjectInList(string projectName)
		{
			ProjectCheckbox = Driver.SetDynamicValue(How.XPath, PROJECT_CHECKBOX, projectName);
			ProjectCheckbox.ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Удалить'
		/// </summary>
		public DeleteProjectDialog ClickDeleteProject()
		{
			Logger.Debug("Нажать на кнопку 'Удалить'.");
			DeleteButton.Click();

			return new DeleteProjectDialog().GetPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Удалить' с открытой свёрткой проекта и файлами.
		/// </summary>
		public DeleteOpenProjectWithFileDialog ClickDeleteOpenProjectWithFile()
		{
			Logger.Debug("Нажать на кнопку 'Удалить'.");
			DeleteButton.Click();

			return new DeleteOpenProjectWithFileDialog().GetPage();
		}

		/// <summary>
		/// Проверить, что проект отсутствует в списке проектов
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage AssertProjectNotExist(string projectName)
		{
			Logger.Trace("Проверить, что проект {0} отсутствует в списке проектов", projectName);

			Assert.IsFalse(getIsProjectExist(projectName: projectName),
				"Произошла ошибка:\n проект {0} найден в списке проектов", projectName);

			return GetPage();
		}

		/// <summary>
		/// Открыть свёртку проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage OpenProjectInfo(string projectName)
		{
			Logger.Trace(string.Format("Открыть свертку проекта {0}", projectName));

			if (!getClassAttributeProjectInfo(projectName).Contains("opened"))
			{
				OpenProjectFolder = Driver.SetDynamicValue(How.XPath, OPEN_PROJECT_FOLDER, projectName);
				OpenProjectFolder.Click();
			}

			return GetPage();
		}

		/// <summary>
		/// Дождаться закрытия диалога удаления проекта с файлом
		/// </summary>
		public ProjectsPage WaitDeleteProjectWithFileDialogDissapeared()
		{
			Logger.Trace("Дождаться закрытия диалога удаления проекта.");

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(DELETE_DIALOG_WITH_FILE)),
				"Произошла ошибка:\n диалог удаления проекта не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Ожидаем закрытия диалога удаления проекта
		/// </summary>
		public ProjectsPage WaitDeleteProjectDialogDissapeared()
		{
			Logger.Trace("Дождаться закрытия диалога удаления проекта.");

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(DELETE_DIALOG)),
				"Произошла ошибка:\n диалог удаления проекта не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Получить класс элемента, где отображается имя проекта.
		/// Используется, чтобы понять, открыта ли свёртка проекта.
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		private string getClassAttributeProjectInfo(string projectName)
		{
			return Driver
				.FindElement(By.XPath(OPEN_PROJECT.Replace("*#*", projectName)))
				.GetElementAttribute("class");
		}

		/// <summary>
		/// Получить bool значение о наличии проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		private bool getIsProjectExist(string projectName)
		{
			Logger.Debug("Ввести в поле поиска по проектам '{0}'", projectName);
			ProjectSearchField.SendKeys(projectName);
			Logger.Debug("Нажать 'Поиск'");
			ProjectSearchButton.Click();

			var isProjectExist = Driver.WaitUntilElementIsDisplay(By.XPath(PROJECT_REF.Replace("*#*", projectName)), 5);

			Logger.Debug("Очистить поле поиска по проектам");
			ProjectSearchField.Clear();
			Logger.Debug("Нажать 'Поиск'");
			ProjectSearchButton.Click();

			return isProjectExist;
		}

		[FindsBy(How = How.XPath, Using = CREATE_PROJECT_BTN_XPATH)]
		protected IWebElement CreateProjectButton { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_BUTTON)]
		protected IWebElement DeleteButton { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_SEARCH_FIELD)]
		protected IWebElement ProjectSearchField { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_PROJECT_BUTTON)]
		protected IWebElement ProjectSearchButton { get; set; }

		protected IWebElement ProjectRef { get; set; }

		protected IWebElement ProjectCheckbox { get; set; }

		protected IWebElement OpenProjectFolder { get; set; }
		
		protected const string CREATE_PROJECT_BTN_XPATH = "//span[contains(@class,'js-project-create')]";
		protected const string CREATE_PROJECT_DIALOG_XPATH = "//div[contains(@class,'js-popup-create-project')][2]";
		protected const string PROJECT_REF_XPATH = ".//table[contains(@class,'js-tasks-table')]//tr//a[@class='js-name'][string()='*#*']";
		protected const string PROJECT_LOAD_IMG_XPATH = ".//table[contains(@class,'js-tasks-table')]//tr//a[@class='js-name'][string()='*#*']/..//img[contains(@class,'l-project-doc__progress')]";
		protected const string PROJECTS_TABLE_XPATH = ".//table[contains(@class,'js-tasks-table')]";
		protected const string DELETE_BUTTON = "//span[contains(@class,'js-delete-btn')]";
		protected const string PROJECT_SEARCH_FIELD = "//input[@name='searchName']";
		protected const string SEARCH_PROJECT_BUTTON = "//a[contains(@class, 'js-search-btn')]/img";
		protected const string DELETE_DIALOG_WITH_FILE = "//div[contains(@class,'js-popup-delete-mode')]";
		protected const string DELETE_DIALOG = "//div[contains(@class,'js-popup-confirm')]";

		protected const string PROJECT_REF = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']";
		protected const string OPEN_PROJECT_FOLDER = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//preceding-sibling::div//img";
		protected const string PROJECT_CHECKBOX = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../../../td[contains(@class,'checkbox')]";
		protected const string OPEN_PROJECT = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/ancestor-or-self::tr";
	}
}