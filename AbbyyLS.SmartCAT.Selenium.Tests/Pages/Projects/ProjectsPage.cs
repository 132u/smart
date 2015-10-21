using System.Linq;
using System.Threading;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
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
		public ProjectsPage(WebDriver driver) : base(driver)
		{
		}

		public new ProjectsPage GetPage()
		{
			var projectPage = new ProjectsPage(Driver);
			InitPage(projectPage, Driver);

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
			CustomTestContext.WriteLine("Проверить, загрузился ли проект {0}.", projectName);

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(PROJECT_LOAD_IMG_XPATH.Replace("*#*", projectName)), 30))
			{
				CustomTestContext.WriteLine("Обновить страницу.");
				Driver.Navigate().Refresh();

				Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(PROJECT_LOAD_IMG_XPATH.Replace("*#*", projectName)), 3),
				"Произошла ошибка:\n проект {0} не загрузился.", projectName);
			}
			
			return GetPage();
		}

		/// <summary>
		/// Проверить, что документ загрузился без критических ошибок, не появился красный восклицательный знак
		/// </summary>
		/// <param name="projectName">название проекта</param>
		public ProjectsPage AssertProjectLoadFatalErrorNotDisplayed(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что документ загрузился в проект {0} без критических ошибок, не появился красный восклицательный знак.", projectName);

			Assert.IsFalse(Driver.GetIsElementExist(By.XPath(PROJECT_CRITICAL_ERROR_LOAD.Replace("*#*", projectName))),
				"Произошла ошибка:\nДокумент загрузился с ошибокой.");

			return GetPage();
		}

		/// <summary>
		///  Проверить, что документ загрузился без предупреждающей ошибки, не появился желтый восклицательный знак
		/// </summary>
		/// <param name="projectName"></param>
		/// <returns></returns>
		public ProjectsPage AssertProjectLoadWarningErrorNotDisplayed(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что документ загрузился в проект {0} без предупреждающей ошибки, не появился желтый восклицательный знак.", projectName);

			Assert.IsFalse(Driver.GetIsElementExist(By.XPath(PROJECT_WARNING_ERROR_LOAD.Replace("*#*", projectName))),
				"Произошла ошибка:\nДокумент загрузился с предупреждающей ошибкой.");

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectSettingsPage ClickProject(string projectName)
		{
			CustomTestContext.WriteLine("Кликнуть по ссылке проекта {0}.", projectName);
			ProjectRef = Driver.SetDynamicValue(How.XPath, PROJECT_REF_XPATH, projectName);

			try
			{
				ProjectRef.JavaScriptClick();
			}
			catch (StaleElementReferenceException)
			{
				ClickProject(projectName);
			}

			return new ProjectSettingsPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Создать проект'
		/// </summary>
		public NewProjectGeneralInformationDialog ClickCreateProjectButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Создать проект'.");
			CreateProjectButton.Click();

			return new NewProjectGeneralInformationDialog(Driver).GetPage();
		}

		/// <summary>
		/// Проверить, что проект появился в списке проектов
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage AssertProjectAppearInList(string projectName)
		{
			CustomTestContext.WriteLine("Дождаться появления проекта {0} в списке", projectName);
			ProjectRef = Driver.SetDynamicValue(How.XPath, PROJECT_REF_XPATH, projectName);

			Assert.IsTrue(ProjectRef.Displayed,
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", projectName);

			return GetPage();
		}

		/// <summary>
		/// Дождаться закрытия диалога создания проекта
		/// </summary>
		public ProjectsPage WaitCreateProjectDialogDisappear()
		{
			CustomTestContext.WriteLine("Дождаться закрытия диалога создания проекта.");

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
		public DeleteDialog ClickDeleteButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Удалить'.");
			DeleteButton.Click();

			return new DeleteDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Удалить' с открытой свёрткой проекта и файлами.
		/// </summary>
		public DeleteOpenProjectWithFileDialog ClickDeleteOpenProjectWithFile()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Удалить'.");
			DeleteButton.Click();

			return new DeleteOpenProjectWithFileDialog(Driver).GetPage();
		}

		/// <summary>
		/// Проверить, что проект отсутствует в списке проектов
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage AssertProjectNotExist(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что проект {0} отсутствует в списке проектов", projectName);

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
			CustomTestContext.WriteLine(string.Format("Открыть свертку проекта {0}", projectName));

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
			CustomTestContext.WriteLine("Нажать кнопку Progress документа №{0} в проекте '{1}'.", documentNumber, projectName);

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
			CustomTestContext.WriteLine("Нажать на кнопку прав пользователя в свертке документа");

			DocumentTaskAssignButton = Driver.SetDynamicValue(How.XPath, DOCUMENT_TASK_ASSIGN_BUTTON, projectName, (documentNumber + 1).ToString());
			DocumentTaskAssignButton.Click();

			return new TaskAssignmentPage(Driver).GetPage();
		}

		/// <summary>
		/// Дождаться закрытия диалога удаления проекта с файлом
		/// </summary>
		public ProjectsPage WaitDeleteProjectWithFileDialogDissapeared()
		{
			CustomTestContext.WriteLine("Дождаться закрытия диалога удаления проекта.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_DIALOG_WITH_FILE)),
				"Произошла ошибка:\n диалог удаления проекта не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Дождаться закрытия диалога удаления
		/// </summary>
		public ProjectsPage WaitDeleteDialogDissapeared()
		{
			CustomTestContext.WriteLine("Дождаться закрытия диалога удаления.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_DIALOG)),
				"Произошла ошибка:\n диалог удаления не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке на документ (открыть его)
		/// </summary>
		public SelectTaskDialog ClickDocumentRef(string documentName)
		{
			CustomTestContext.WriteLine("Кликнуть по ссылке на документ {0} (открыть его).", documentName);
			DocumentRef = Driver.SetDynamicValue(How.XPath, DOCUMENT_REF, documentName);
			DocumentRef.Click();
			// Sleep нужен, чтоб вторая вкладка успела открыться, иначе количество открытых вкладок посчитается неправильно 
			Thread.Sleep(1000);
			if (Driver.WindowHandles.Count > 1)
			{
				Driver.SwitchTo().Window(Driver.WindowHandles.First()).Close();
				Driver.SwitchTo().Window(Driver.WindowHandles.Last());
			}

			return new SelectTaskDialog(Driver);
		}

		/// <summary>
		/// Нажать кнопку экспорта в главном меню
		/// </summary>
		public ProjectsPage ClickDownloadInMainMenuButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку экспорта в главном меню");
			DownloadInMainMenuButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку экспорта в меню проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage ClickDownloadInProjectButton(string projectName)
		{
			CustomTestContext.WriteLine("Нажать кнопку экспорта в меню проекта");

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
			CustomTestContext.WriteLine("Нажать кнопку экспорта в меню документа");

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
			CustomTestContext.WriteLine("Открыть свёртку документа №{0} в проекте '{1}'", documentNumber, projectName);

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
			CustomTestContext.WriteLine("Нажать кнопку настроек в меню документа №{0} в проекте '{1}'", documentNumber, projectName);
			
			DocumentSettings = Driver.SetDynamicValue(How.XPath, DOCUMENT_SETTINGS, projectName, documentNumber.ToString());
			DocumentSettings.Click();

			return new DocumentSettings(Driver).GetPage();
		}

		/// <summary>
		/// Получить класс элемента, где отображается имя проекта.
		/// Используется, чтобы понять, открыта ли свёртка проекта.
		/// Проверить наличие проекта с помощью поиска по имени
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		private bool getIsProjectExist(string projectName)
		{
			CustomTestContext.WriteLine("Ввести в поле поиска по проектам '{0}'", projectName);
			ProjectSearchField.SetText(projectName);
			CustomTestContext.WriteLine("Нажать 'Поиск'");
			ProjectSearchButton.Click();

			var isProjectExist = Driver.WaitUntilElementIsDisplay(By.XPath(PROJECT_REF.Replace("*#*", projectName)), 5);

			CustomTestContext.WriteLine("Очистить поле поиска по проектам");
			ProjectSearchField.Clear();
			CustomTestContext.WriteLine("Нажать 'Поиск'");
			ProjectSearchButton.Click();

			return isProjectExist;
		}

		/// <summary>
		/// Нажать на кнопку загрузки файла
		/// </summary>
		public DocumentUploadGeneralInformationDialog ClickDocumentUploadButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку загрузки файла");
			UploadDocumentButton.Click();

			return new DocumentUploadGeneralInformationDialog(Driver).GetPage();
		}

		/// <summary>
		/// Проверить, что ссылка ведущая в проект остутствует
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage AssertLinkProjectNotExist(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что ссылка ведущая в проект {0} остутствует", projectName);

			Assert.IsFalse(Driver.GetIsElementExist(By.XPath(PROJECT_LINK.Replace("*#*", projectName))),
				"Произошла ошибка:\n не должно быть ссылки на проект {0}",
				projectName);

			return GetPage();
		}

		/// <summary>
		/// Отметить чекбокс документа
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="documentName">имя документа</param>
		public ProjectsPage SelectDocument(string projectName, string documentName)
		{
			CustomTestContext.WriteLine("Отметить чекбокс документа {0} в проекте {1}", documentName, projectName);

			DocumentCheckBox = Driver.SetDynamicValue(How.XPath, DOCUMENT_CHECKBOX, projectName, documentName);
			DocumentCheckBox.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка 'Sign in to Connector' отсутствует
		/// </summary>
		public ProjectsPage AssertSignInToConnectorButtonNotExist()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Sign in to Connector' отсутствует");

			Assert.IsFalse(Driver.GetIsElementExist(By.XPath(SIGN_IN_TO_CONNECTOR_BUTTON)),
				"Произошла ошибка:\n кнопка 'Sign in to Connector' не должна быть видна.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка 'QA Check' у проекта видима
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage AssertQACheckButtonExist(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'QA Check' у проекта '{0}' видима.", projectName);

			Assert.IsTrue(Driver.GetIsElementExist(By.XPath(QA_CHECK_BUTTON.Replace("*#*", projectName))),
				"Произошла ошибка:\n кнопка 'QA Check' у проекта '{0}'. отсутствует", projectName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сообщение 'Preparing documents for download. Please wait ...' исчезло
		/// </summary>
		public ProjectsPage AssertPreparingDownloadMessageDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'Preparing documents for download. Please wait ...' исчезло.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(PREPARING_DOWNLOWD_MESSAGE), timeout: 30),
				"Произошла ошибка:\n сообщение 'Preparing documents for download. Please wait ...' не исчезло.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку настроек проекта в открытой свёртке.
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public SettingsDialog ClickProjectSettingsButton(string projectName)
		{
			CustomTestContext.WriteLine("Нажать кнопку настроек проекта '{0}' в открытой свёртке.", projectName);

			ProjectSettingsButton = Driver.SetDynamicValue(How.XPath, PROJECT_SETTINGS_BUTTON, projectName);
			ProjectSettingsButton.Click();

			return  new SettingsDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку анализа проекта в открытой свёртке.
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public AnalysisDialog ClickProjectAnalysisButton(string projectName)
		{
			CustomTestContext.WriteLine("Нажать кнопку анализа проекта '{0}' в открытой свёртке.", projectName);

			ProjectAnalysisButton = Driver.SetDynamicValue(How.XPath, PROJECT_ANALYSIS_BUTTON, projectName);
			ProjectAnalysisButton.Click();

			return new AnalysisDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку удаления в меню проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public DeleteDialog ClickDeleteInProjectButton(string projectName)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления в меню проекта {0}", projectName);

			DeleteInProjectButton = Driver.SetDynamicValue(How.XPath, DELETE_IN_PROJECT_BUTTON, projectName);
			DeleteInProjectButton.Click();

			return new DeleteDialog(Driver).GetPage();
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
			CustomTestContext.WriteLine("Получить, открыта ли свёртка документа №{0} проекта '{1}'.", documentNumber, projectName);
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

		[FindsBy(How = How.XPath, Using = PROJECT_SETTINGS_BUTTON)]
		protected IWebElement ProjectSettingsButton { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_ANALYSIS_BUTTON)]
		protected IWebElement ProjectAnalysisButton { get; set; }

		protected IWebElement DownloadInProjectButton { get; set; }

		protected IWebElement DownloadInDocumentButton { get; set; }

		protected IWebElement ProjectRef { get; set; }

		protected IWebElement ProjectCheckbox { get; set; }

		protected IWebElement OpenProjectFolder { get; set; }

		protected IWebElement DocumentRow { get; set; }

		protected IWebElement DocumentProgress { get; set; }

		protected IWebElement DocumentTaskAssignButton {get; set;}

		protected IWebElement DocumentRef { get; set; }

		protected IWebElement DocumentCheckBox { get; set; }

		protected IWebElement DeleteInProjectButton { get; set; }

		protected const string CREATE_PROJECT_BTN_XPATH = "//span[contains(@class,'js-project-create')]";
		protected const string CREATE_PROJECT_DIALOG_XPATH = "//div[contains(@class,'js-popup-create-project')][2]";
		protected const string PROJECT_REF_XPATH = "//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and string()='*#*']";
		protected const string PROJECT_LOAD_IMG_XPATH = "//*[(local-name() ='a' or local-name() ='span') and text()='*#*']//preceding-sibling::img[contains(@data-bind,'processingInProgress')]";
		protected const string PROJECT_CRITICAL_ERROR_LOAD = "//a[text()='*#*']//preceding-sibling::img[contains(@data-bind,'processingFatalError')]";
		protected const string PROJECT_WARNING_ERROR_LOAD = "//a[text()='*#*']//preceding-sibling::img[contains(@data-bind,'processingError')]";
		protected const string PROJECTS_TABLE_XPATH = "//table[contains(@class,'js-tasks-table')]";
		protected const string DELETE_BUTTON = "//span[contains(@class,'js-delete-btn')]";
		protected const string PROJECT_SEARCH_FIELD = "//input[@name='searchName']";
		protected const string SEARCH_PROJECT_BUTTON = "//a[contains(@class, 'js-search-btn')]/img";
		protected const string DELETE_DIALOG_WITH_FILE = "//div[contains(text(), 'Delete the selected project')]";
		protected const string DELETE_DIALOG = "//div[contains(@class,'js-popup-confirm')]";
		protected const string PROJECT_REF = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']";
		protected const string OPEN_PROJECT_FOLDER = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//preceding-sibling::div//img";
		protected const string PROJECT_CHECKBOX = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../../../td[contains(@class,'checkbox')]";
		protected const string OPEN_PROJECT = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/ancestor-or-self::tr";
		protected const string DOCUMENT_REF = "//tr[contains(@class,'js-document-row')]//a[text()='*#*']";
		protected const string DOWNLOAD_MAIN_MENU_BUTTON = "//span[contains(@class,'js-document-export-block')]";
		protected const string DOWNLOAD_IN_PROJECT_BUTTON = "//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//div[contains(@class,'js-buttons-left')]//li/span";
		protected const string DOWNLOAD_IN_DOCUMENT_BUTTON ="//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]//following-sibling::tr[1]//div[contains(@class,'js-buttons-left')]//li";
		protected const string DOCUMENT_ROW = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]";
		protected const string DOCUMENT_PROGRESS = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]//div[@class='ui-progressbar__container']";
		protected const string DOCUMENT_SETTINGS = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]//following-sibling::tr[1]//span[contains(@data-bind, 'actions.edit')]";
		protected const string DOCUMENT_TASK_ASSIGN_BUTTON = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/ancestor::tr/following-sibling::tr[*##*]/following-sibling::tr[1][@class='js-document-panel l-project__doc-panel']//span[contains(@class, 'js-assign-btn') and @data-bind='click: assign']";
		protected const string UPLOAD_DOCUMENT_BUTTON = "//span[contains(@data-bind, 'click: importDocument')]";
		protected const string PROJECT_LINK = ".//table[contains(@class,'js-tasks-table')]//tr//a[@class='js-name'][text()='*#*']";
		protected const string DOCUMENT_CHECKBOX = ".//table[contains(@class,'js-tasks-table')]//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[td[div[a[contains(@class,'doc-link')][text()='*##*']]]]/td[1]/input";
		protected const string SIGN_IN_TO_CONNECTOR_BUTTON = "//span[contains(@class,'login-connector-btn')]";
		protected const string QA_CHECK_BUTTON = "//table[contains(@class,'js-tasks-table')]//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//span[contains(@data-bind,'qaCheck')]";
		protected const string PROJECT_SETTINGS_BUTTON = "//table[contains(@class,'js-tasks-table')]//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//span[contains(@data-bind,'edit')]";
		protected const string PROJECT_ANALYSIS_BUTTON = "//table[contains(@class,'js-tasks-table')]//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//span[contains(@data-bind,'analysis')]";
		protected const string UPLOAD_DOCUMENT_DIALOG = "//div[contains(@class,'js-popup-import-document')][2]";
		protected const string DELETE_IN_PROJECT_BUTTON = "//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//div[contains(@class,'js-buttons-left')]//span[3]";
		protected const string PREPARING_DOWNLOWD_MESSAGE = "//span[contains(text(), 'Preparing documents for download. Please wait')]";
	}
}
