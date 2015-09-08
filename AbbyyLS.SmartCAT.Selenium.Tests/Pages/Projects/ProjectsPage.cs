using System.Linq;
using System.Threading;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
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
			Driver.WaitPageTotalLoad();
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_PROJECT_BTN_XPATH), timeout: 45))
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
			Logger.Debug("Проверить, загрузился ли проект {0}.", projectName);

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(PROJECT_LOAD_IMG_XPATH.Replace("*#*", projectName)), 120))
			{
				Logger.Debug("Обновить страницу.");
				Driver.Navigate().Refresh();

				Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(PROJECT_LOAD_IMG_XPATH.Replace("*#*", projectName)), 3),
				"Произошла ошибка:\n проект {0} не загрузился.", projectName);
			}
			
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
				ProjectRef.JavaScriptClick();
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
		public ProjectsPage AssertProjectAppearInList(string projectName)
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
		public ProjectsPage WaitCreateProjectDialogDissapear()
		{
			Logger.Trace("Дождаться закрытия диалога создания проекта.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(CREATE_PROJECT_DIALOG_XPATH), timeout: 20),
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

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(PROJECT_REF_XPATH.Replace("*#*", projectName)), timeout: 20))
			{
				Assert.Fail("Произошла ошибка:\n проект {0} не удалился.", projectName);
			}

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
		/// Нажать кнопку Progress определенного документа в проекте
		/// </summary>
		/// <param name="projectName"> Название проекта </param>
		/// <param name="documentNumber"> Номер документа </param>
		public ProjectsPage ClickProgressDocument(string projectName, int documentNumber = 1)
		{
			Logger.Debug("Нажать кнопку Progress документа №{0} в проекте '{1}'.", documentNumber, projectName);

			DocumentProgress = Driver.SetDynamicValue(How.XPath, DOCUMENT_PROGRESS, projectName, documentNumber.ToString());
			DocumentProgress.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку прав пользователя в свертке документа
		/// </summary>
		/// <param name="projectName"> Название проекта </param>
		/// <param name="documentNumber"> Номер документа </param>
		public TaskAssignmentPage ClickDocumentAssignButton(string projectName, int documentNumber = 1)
		{
			Logger.Debug("Нажать на кнопку прав пользователя в свертке документа");

			DocumentTaskAssignButton = Driver.SetDynamicValue(How.XPath, DOCUMENT_TASK_ASSIGN_BUTTON, projectName, (documentNumber + 1).ToString());
			DocumentTaskAssignButton.Click();

			return new TaskAssignmentPage().GetPage();
		}

		/// <summary>
		/// Дождаться закрытия диалога удаления проекта с файлом
		/// </summary>
		public ProjectsPage WaitDeleteProjectWithFileDialogDissapeared()
		{
			Logger.Trace("Дождаться закрытия диалога удаления проекта.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_DIALOG_WITH_FILE)),
				"Произошла ошибка:\n диалог удаления проекта не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Ожидаем закрытия диалога удаления проекта
		/// </summary>
		public ProjectsPage WaitDeleteProjectDialogDissapeared()
		{
			Logger.Trace("Дождаться закрытия диалога удаления проекта.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_DIALOG)),
				"Произошла ошибка:\n диалог удаления проекта не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке на документ (открыть его)
		/// </summary>
		public SelectTaskDialog ClickDocumentRef(string documentName)
		{
			Logger.Debug("Кликнуть по ссылке на документ {0} (открыть его).", documentName);
			DocumentRef = Driver.SetDynamicValue(How.XPath, DOCUMENT_REF, documentName);
			DocumentRef.Click();
			// Sleep нужен, чтоб вторая вкладка успела открыться, иначе количество открытых вкладок посчитается неправильно 
			Thread.Sleep(1000);
			if (Driver.WindowHandles.Count > 1)
			{
				Driver.SwitchTo().Window(Driver.WindowHandles.First()).Close();
				Driver.SwitchTo().Window(Driver.WindowHandles.Last());
			}

			return new SelectTaskDialog();
		}

		/// <summary>
		/// Нажать кнопку экспорта в главном меню
		/// </summary>
		public ProjectsPage ClickDownloadInMainMenuButton()
		{
			Logger.Debug("Нажать кнопку экспорта в главном меню");
			DownloadInMainMenuButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку экспорта в меню проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage ClickDownloadInProjectButton(string projectName)
		{
			Logger.Debug("Нажать кнопку экспорта в меню проекта");

			DownloadInProjectButton = Driver.SetDynamicValue(How.XPath, DOWNLOAD_IN_PROJECT_BUTTON, projectName);
			DownloadInProjectButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку экспорта в меню документа
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="documentNumber">номер документа</param>
		public ProjectsPage ClickDownloadInDocumentButton(string projectName, int documentNumber = 1)
		{
			Logger.Debug("Нажать кнопку экспорта в меню документа");

			DownloadInDocumentButton = Driver.SetDynamicValue(How.XPath, DOWNLOAD_IN_DOCUMENT_BUTTON, projectName, documentNumber.ToString());
			DownloadInDocumentButton.Click();

			return GetPage();
		}
		
		/// <summary>
		/// Открыть свёртку документа
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="documentNumber">номер документа</param>
		public ProjectsPage OpenDocumentInfoForProject(string projectName, int documentNumber = 1)
		{
			Logger.Debug("Открыть свёртку документа №{0} в проекте '{1}'", documentNumber, projectName);

			if (!getDocumentPanelIsOpened(projectName, documentNumber))
			{
				ClickProgressDocument(projectName, documentNumber);
			}

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку настроек в меню документа
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="documentNumber">номер документа</param>
		public DocumentSettings ClickDocumentSettings(string projectName, int documentNumber = 1)
		{
			Logger.Debug("Нажать кнопку настроек в меню документа №{0} в проекте '{1}'", documentNumber, projectName);
			
			DocumentSettings = Driver.SetDynamicValue(How.XPath, DOCUMENT_SETTINGS, projectName, documentNumber.ToString());
			DocumentSettings.Click();

			return new DocumentSettings().GetPage();
		}

		/// <summary>
		/// Получить класс элемента, где отображается имя проекта.
		/// Используется, чтобы понять, открыта ли свёртка проекта.
		/// Проверить наличие проекта с помощью поиска по имени
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		private bool getIsProjectExist(string projectName)
		{
			Logger.Debug("Ввести в поле поиска по проектам '{0}'", projectName);
			ProjectSearchField.SetText(projectName);
			Logger.Debug("Нажать 'Поиск'");
			ProjectSearchButton.Click();

			var isProjectExist = Driver.WaitUntilElementIsDisplay(By.XPath(PROJECT_REF.Replace("*#*", projectName)), 5);

			Logger.Debug("Очистить поле поиска по проектам");
			ProjectSearchField.Clear();
			Logger.Debug("Нажать 'Поиск'");
			ProjectSearchButton.Click();

			return isProjectExist;
		}

		/// <summary>
		/// Нажать на кнопку загрузки файла
		/// </summary>
		public DocumentUploadGeneralInformationDialog ClickDocumentUploadButton()
		{
			Logger.Debug("Нажать на кнопку звгрузки файла");
			UploadDocumentButton.Click();

			return new DocumentUploadGeneralInformationDialog().GetPage();
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
		/// Получить открыта ли свертка документа
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="documentNumber">номер документа в проекте</param>
		private bool getDocumentPanelIsOpened(string projectName, int documentNumber = 1)
		{
			Logger.Trace("Получить, открыта ли свёртка документа №{0} проекта '{1}'.", documentNumber, projectName);
			DocumentRow = Driver.SetDynamicValue(How.XPath, DOCUMENT_ROW, projectName, documentNumber.ToString());

			return DocumentRow.GetElementAttribute("class").Contains("opened");
		}

		[FindsBy(How = How.XPath, Using = CREATE_PROJECT_BTN_XPATH)]
		protected IWebElement CreateProjectButton { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_BUTTON)]
		protected IWebElement DeleteButton { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_SEARCH_FIELD)]
		protected IWebElement ProjectSearchField { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_PROJECT_BUTTON)]
		protected IWebElement ProjectSearchButton { get; set; }

		[FindsBy(How = How.XPath, Using = UPLOAD_DOCUMENT_BUTTON)]
		protected IWebElement UploadDocumentButton { get; set; }

		[FindsBy(How = How.XPath, Using = DOWNLOAD_MAIN_MENU_BUTTON)]
		protected IWebElement DownloadInMainMenuButton { get; set; }

		[FindsBy(How = How.XPath, Using = DOCUMENT_SETTINGS)]
		protected IWebElement DocumentSettings { get; set; }

		protected IWebElement DownloadInProjectButton { get; set; }

		protected IWebElement DownloadInDocumentButton { get; set; }

		protected IWebElement ProjectRef { get; set; }

		protected IWebElement ProjectCheckbox { get; set; }

		protected IWebElement OpenProjectFolder { get; set; }

		protected IWebElement DocumentRow { get; set; }

		protected IWebElement DocumentProgress { get; set; }

		protected IWebElement DocumentTaskAssignButton {get; set;}

		protected IWebElement DocumentRef { get; set; }

		protected const string CREATE_PROJECT_BTN_XPATH = "//span[contains(@class,'js-project-create')]";
		protected const string CREATE_PROJECT_DIALOG_XPATH = "//div[contains(@class,'js-popup-create-project')][2]";
		protected const string PROJECT_REF_XPATH = "//table[contains(@class,'js-tasks-table')]//tr//a[@class='js-name'][string()='*#*']";
		protected const string PROJECT_LOAD_IMG_XPATH = "//a[text()='*#*']//preceding-sibling::img[contains(@data-bind,'processingInProgress')]";
		protected const string PROJECTS_TABLE_XPATH = "//table[contains(@class,'js-tasks-table')]";
		protected const string DELETE_BUTTON = "//span[contains(@class,'js-delete-btn')]";
		protected const string PROJECT_SEARCH_FIELD = "//input[@name='searchName']";
		protected const string SEARCH_PROJECT_BUTTON = "//a[contains(@class, 'js-search-btn')]/img";
		protected const string DELETE_DIALOG_WITH_FILE = "//div[contains(@class,'js-popup-delete-mode')]";
		protected const string DELETE_DIALOG = "//div[contains(@class,'js-popup-confirm')]";
		protected const string PROJECT_REF = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']";
		protected const string OPEN_PROJECT_FOLDER = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//preceding-sibling::div//img";
		protected const string PROJECT_CHECKBOX = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../../../td[contains(@class,'checkbox')]";
		protected const string OPEN_PROJECT = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/ancestor-or-self::tr";
		protected const string DOCUMENT_REF = "//tr[contains(@class,'js-document-row')]//a[text()='*#*']";
		protected const string DOWNLOAD_MAIN_MENU_BUTTON = "//span[contains(@class,'js-document-export-block')]";
		protected const string DOWNLOAD_IN_PROJECT_BUTTON = "//table[contains(@class,'js-tasks-table')]//tr//a[@class='js-name'][string()='*#*']//ancestor::tr//following-sibling::tr[1]//div[contains(@class,'js-buttons-left')]//li/span";
		protected const string DOWNLOAD_IN_DOCUMENT_BUTTON =".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]//following-sibling::tr[1]//div[contains(@class,'js-buttons-left')]//li";
		protected const string DOCUMENT_ROW = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]";
		protected const string DOCUMENT_PROGRESS = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]//div[@class='ui-progressbar__container']";
		protected const string DOCUMENT_SETTINGS = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]//following-sibling::tr[1]//span[3]";
		protected const string DOCUMENT_TASK_ASSIGN_BUTTON = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/ancestor::tr/following-sibling::tr[*##*]/following-sibling::tr[1][@class='js-document-panel l-project__doc-panel']//span[contains(@class, 'js-assign-btn') and @data-bind='click: assign']";
		protected const string UPLOAD_DOCUMENT_BUTTON = "//span[contains(@data-bind, 'click: importJob')]";
	}
}
