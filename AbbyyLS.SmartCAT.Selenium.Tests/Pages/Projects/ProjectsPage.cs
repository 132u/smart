using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
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
			if (!IsProjectsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось перейти на вкладку \"Проекты\".");
			}
		}
		
		#region Простые методы страницы

		/// <summary>
		/// Выбрать тип экспорта
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		public ProjectsPage ClickExportType(ExportType exportType)
		{
			CustomTestContext.WriteLine("Выбрать тип экспорта");
			ExportType = Driver.SetDynamicValue(How.XPath, EXPORT_TYPE, exportType.ToString());
			ExportType.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectSettingsPage ClickProject(string projectName)
		{
			CustomTestContext.WriteLine("Кликнуть по ссылке проекта {0}.", projectName);
			ProjectRef = Driver.SetDynamicValue(How.XPath, PROJECT_REF, projectName);

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
		public NewProjectDocumentUploadPage ClickCreateProjectButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Создать проект'.");
			CreateProjectButton.Click();

			return new NewProjectDocumentUploadPage(Driver).GetPage();
		}

		/// <summary>
		/// Отметить чекбокс проекта в списке
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage ClickProjectCheckboxInList(string projectName)
		{
			CustomTestContext.WriteLine("Отметить чекбокс проекта {0} в списке", projectName);
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
		/// Нажать кнопку Decline.
		/// </summary>
		public TaskDeclineDialog ClickDeclineButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Decline.");
			DeclineButton.Click();

			return new TaskDeclineDialog(Driver).GetPage();
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
			DocumentTaskAssignButton = Driver.SetDynamicValue(How.XPath, DOCUMENT_TASK_ASSIGN_BUTTON, projectName, documentNumber.ToString());
			DocumentTaskAssignButton.Click();

			return new TaskAssignmentPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать на кнопку прав пользователя в свертке проекта
		/// </summary>
		/// <param name="projectName"> Название проекта </param>
		public TaskAssignmentPage ClickProjectAssignButton(string projectName)
		{
			CustomTestContext.WriteLine("Нажать на кнопку прав пользователя в свертке проекта.");
			Driver.SetDynamicValue(How.XPath, PROJECT_TASK_ASSIGN_BUTTON, projectName).Click();

			return new TaskAssignmentPage(Driver).GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке на документ (открыть его) ожидая открытие редактора
		/// </summary>
		public EditorPage ClickDocumentRefExpectingEditorPage(string documentPath)
		{
			var documentname = Path.GetFileNameWithoutExtension(documentPath);
			CustomTestContext.WriteLine("Кликнуть по ссылке на документ {0} (открыть его) ожидая открытие редактора.", documentname);
			DocumentRef = Driver.SetDynamicValue(How.XPath, DOCUMENT_REF, documentname);
			DocumentRef.Click();
			// Sleep нужен, чтоб вторая вкладка успела открыться, иначе количество открытых вкладок посчитается неправильно 
			Thread.Sleep(1000);
			if (Driver.WindowHandles.Count > 1)
			{
				Driver.SwitchTo().Window(Driver.WindowHandles.First()).Close();
				Driver.SwitchTo().Window(Driver.WindowHandles.Last());
			}

			return new EditorPage(Driver).GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке на документ (открыть его) ожидая диалог выбора задачи
		/// </summary>
		public SelectTaskDialog ClickDocumentRefExpectingSelectTaskDialog(string documentPath)
		{
			var documentname = Path.GetFileNameWithoutExtension(documentPath);
			CustomTestContext.WriteLine("Кликнуть по ссылке на документ {0} (открыть его) ожидая диалог выбора задачи.", documentname);
			DocumentRef = Driver.SetDynamicValue(How.XPath, DOCUMENT_REF, documentname);
			DocumentRef.Click();
			// Sleep нужен, чтоб вторая вкладка успела открыться, иначе количество открытых вкладок посчитается неправильно 
			Thread.Sleep(1000);
			if (Driver.WindowHandles.Count > 1)
			{
				Driver.SwitchTo().Window(Driver.WindowHandles.First()).Close();
				Driver.SwitchTo().Window(Driver.WindowHandles.Last());
			}

			return new SelectTaskDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку экспорта в главном меню
		/// </summary>
		public ProjectsPage ClickDownloadInMainMenuButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку экспорта в главном меню");
			DownloadInMainMenuButton.ScrollAndClick();

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
		public DocumentSettingsDialog ClickDocumentSettings(string projectName, int documentNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать кнопку настроек в меню документа №{0} в проекте '{1}'", documentNumber, projectName);
			DocumentSettings = Driver.SetDynamicValue(How.XPath, DOCUMENT_SETTINGS, projectName, documentNumber.ToString());
			DocumentSettings.Click();

			return new DocumentSettingsDialog(Driver).GetPage();
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
		/// Отметить чекбокс документа
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="documentName">имя документа</param>
		public ProjectsPage SelectDocument(string projectName, string documentPath)
		{
			var documentName = Path.GetFileNameWithoutExtension(documentPath);
			CustomTestContext.WriteLine("Отметить чекбокс документа {0} в проекте {1}", documentName, projectName);
			DocumentCheckBox = Driver.SetDynamicValue(How.XPath, DOCUMENT_CHECKBOX, projectName, documentName);
			DocumentCheckBox.Click();

			return GetPage();
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
		/// Нажать кнопку настроек проекта в открытой свёртке.
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public SettingsDialog ClickProjectSettingsButton(string projectName)
		{
			CustomTestContext.WriteLine("Нажать кнопку настроек проекта '{0}' в открытой свёртке.", projectName);
			ProjectSettingsButton = Driver.SetDynamicValue(How.XPath, PROJECT_SETTINGS_BUTTON, projectName);
			ProjectSettingsButton.Click();

			return new SettingsDialog(Driver).GetPage();
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
		/// Получить статус проекта
		/// </summary>
		/// <param name="projectName">название проекта</param>
		public string GetProjectStatus(string projectName)
		{
			CustomTestContext.WriteLine("Получить статус проекта");

			return Driver.SetDynamicValue(How.XPath, PROJECT_STATUS, projectName).GetAttribute("title");
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Отменить задачу в свертке документа.
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <param name="documentNumber">номер документа</param>
		public TaskDeclineDialog DeclineTaskInDocumentInfo(string projectName, int documentNumber = 1)
		{
			CustomTestContext.WriteLine("Отменить задачу в свертке документа.");
			OpenProjectInfo(projectName);
			OpenDocumentInfoForProject(projectName, documentNumber);
			ClickDeclineButton();

			return new TaskDeclineDialog(Driver).GetPage();
		}

		/// Открыть диалог назначения задачи
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public TaskAssignmentPage OpenAssignDialog(string projectName, int documentNumber = 1)
		{
			CustomTestContext.WriteLine("Открыть диалог назначения задачи.");
			OpenProjectInfo(projectName);
			OpenDocumentInfoForProject(projectName, documentNumber);
			var taskAssignmentPage = ClickDocumentAssignButton(projectName, documentNumber);

			return taskAssignmentPage.GetPage();
		}

		/// <summary>
		///  Открыть диалог назначения задачи для нескольких документов
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="filePathList">список документов</param>
		/// <returns></returns>
		public TaskAssignmentPage OpenAssignDialogForSelectedDocuments(string projectName, IList<string> filePathList)
		{
			CustomTestContext.WriteLine("Открыть диалог назначения задачи для нескольких документов.");
			OpenProjectInfo(projectName);

			foreach (var file in filePathList)
			{
				SelectDocument(projectName, file);
			}
			
			ClickProjectAssignButton(projectName);

			return new TaskAssignmentPage(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница 'Проекты'
		/// </summary>
		public bool IsProjectsPageOpened()
		{
			return IsDialogBackgroundDisappeared() &&
				Driver.WaitUntilElementIsDisplay(By.XPath(PROJECTS_TABLE_XPATH), timeout: 45);
		}

		/// <summary>
		/// Проверить, что документ загрузился без критических ошибок, не появился красный восклицательный знак
		/// </summary>
		/// <param name="projectName">название проекта</param>
		public bool IsFatalErrorSignDisplayed(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что документ загрузился в проект {0} без критических ошибок, не появился красный восклицательный знак.", projectName);

			return Driver.GetIsElementExist(By.XPath(PROJECT_CRITICAL_ERROR_LOAD.Replace("*#*", projectName)));
		}

		/// <summary>
		///  Проверить, что документ загрузился без предупреждающей ошибки, не появился желтый восклицательный знак
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public bool IsWarningSignDisplayed(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что документ загрузился в проект {0} без предупреждающей ошибки, не появился желтый восклицательный знак.", projectName);

			return Driver.GetIsElementExist(By.XPath(PROJECT_WARNING_ERROR_LOAD.Replace("*#*", projectName)));
		}

		/// <summary>
		/// Проверить, что кнопка 'Sign in to Connector' отсутствует
		/// </summary>
		public bool IsSignInToConnectorButtonExist()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Sign in to Connector' отсутствует");

			return Driver.GetIsElementExist(By.XPath(SIGN_IN_TO_CONNECTOR_BUTTON));
		}

		/// <summary>
		/// Проверить, загрузился ли проект
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public bool IsProjectLoaded(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, загрузился ли проект {0}.", projectName);
			// Практически все, что сделано в этом методе является быстрым незамысловатым костылем,
			// пока не пофиксили баг с крутилкой. Тройной рефреш позволяет тестам проходить быстрее,
			// т.к. мы не ждем пол минуты или минуту, а делаем проверку через 10 секунд.
			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(PROJECT_LOAD_IMG_XPATH.Replace("*#*", projectName))))
			{
				RefreshPage<ProjectsPage>();
			}

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(PROJECT_LOAD_IMG_XPATH.Replace("*#*", projectName))))
			{
				RefreshPage<ProjectsPage>();
			}

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(PROJECT_LOAD_IMG_XPATH.Replace("*#*", projectName))))
			{
				RefreshPage<ProjectsPage>();
			}

			return Driver.WaitUntilElementIsDisappeared(By.XPath(PROJECT_LOAD_IMG_XPATH.Replace("*#*", projectName)));
		}

		/// <summary>
		/// Проверить, что проект появился в списке проектов
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public bool IsProjectAppearInList(string projectName)
		{
			CustomTestContext.WriteLine("Дождаться появления проекта {0} в списке", projectName);
			ProjectRef = Driver.SetDynamicValue(How.XPath, PROJECT_REF, projectName);

			return ProjectRef.Displayed;
		}

		/// <summary>
		/// Проверить, что проект отсутствует в списке проектов
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public bool IsProjectNotExistInList(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что проект {0} отсутствует в списке проектов.", projectName);

			return Driver.WaitUntilElementIsDisappeared(By.XPath(PROJECT_REF.Replace("*#*", projectName)));
		}

		/// <summary>
		/// Проверить, что проект существует с помощью функции поиска в кате
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <returns> возвращает true, если проект найден, иначе - false</returns>
		public bool IsProjectExist(string projectName)
		{
			CustomTestContext.WriteLine("Ввести в поле поиска по проектам '{0}'", projectName);
			ProjectSearchField.SetText(projectName);
			CustomTestContext.WriteLine("Нажать 'Поиск'");
			ProjectSearchButton.ScrollAndClick();

			var isProjectExist = Driver.WaitUntilElementIsDisplay(By.XPath(PROJECT_REF.Replace("*#*", projectName)), 5);

			CustomTestContext.WriteLine("Очистить поле поиска по проектам");
			ProjectSearchField.Clear();
			CustomTestContext.WriteLine("Нажать 'Поиск'");
			ProjectSearchButton.ScrollAndClick();

			return isProjectExist;
		}

		/// <summary>
		/// Проверить, что существует ссылка ведущая в проект
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public bool IsProjectLinkExist(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что существует ссылка ведущая в проект {0}", projectName);

			return Driver.GetIsElementExist(By.XPath(PROJECT_LINK.Replace("*#*", projectName)));
		}

		/// <summary>
		/// Проверить, что кнопка 'QA Check' у проекта видима
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public bool IsQACheckButtonDisplayed(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'QA Check' у проекта '{0}' видима.", projectName);
			var QACheckButton = Driver.SetDynamicValue(How.XPath, QA_CHECK_BUTTON, projectName);

			return QACheckButton.Displayed;
		}

		/// <summary>
		/// Посчитать количество задач.
		/// </summary>
		public int GetTasksCount()
		{
			CustomTestContext.WriteLine("Посчитать количество задач.");

			return Driver.GetElementsCount(By.XPath(TASK_LIST));
		}

		#endregion

		#region Методы, ожидающие определенного состояния страницы

		/// <summary>
		/// Проверить, что проект загрузился без ошибок
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage WaitUntilProjectLoadSuccessfully(string projectName)
		{
			if (!IsProjectLoaded(projectName))
			{
				throw new Exception("Произошла ошибка: не исчезла пиктограмма загрузки проекта");
			}

			if (IsFatalErrorSignDisplayed(projectName))
			{
				throw new Exception("Произошла ошибка: появилась пиктограмма ошибки напротив проекта");
			}

			if (IsWarningSignDisplayed(projectName))
			{
				throw new Exception("Произошла ошибка: появилась пиктограмма предупреждения напротив проекта");
			}

			return new ProjectsPage(Driver).GetPage();
		}

		/// <summary>
		/// Дождаться закрытия диалога создания проекта
		/// </summary>
		public ProjectsPage WaitCreateProjectDialogDisappear()
		{
			CustomTestContext.WriteLine("Дождаться закрытия диалога создания проекта.");

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(CREATE_PROJECT_DIALOG_XPATH), timeout: 20))
			{
				throw new InvalidElementStateException("Произошла ошибка:\n диалог создания проекта не закрылся");
			}

			return GetPage();
		}

		#endregion

		#region Вспомогательные методы страницы
		
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

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = EXPORT_TYPE)]
		protected IWebElement ExportType { get; set; }

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

		[FindsBy(How = How.XPath, Using = DECLINE_BUTTON)]
		protected IWebElement DeclineButton { get; set; }

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

		#endregion

		#region Описания XPath элементов

		protected const string EXPORT_TYPE = "//div[not(contains(@class,'g-hidden'))]/div[contains(@data-bind,'*#*') and contains(@data-bind, 'export')]";
		protected const string EXPORT_TYPE_TMX = "//div[not(contains(@class,'g-hidden'))]/div[contains(@data-bind,'Tmx')]";

		protected const string CREATE_PROJECT_BTN_XPATH = "//div[contains(@data-bind,'createProject')]";
		protected const string CREATE_PROJECT_DIALOG_XPATH = "//div[contains(@class,'js-popup-create-project')][2]";
		protected const string PROJECT_LOAD_IMG_XPATH = "//*[(local-name() ='a' or local-name() ='span') and text()='*#*']//preceding-sibling::img[contains(@data-bind,'processingInProgress')]";
		protected const string PROJECT_CRITICAL_ERROR_LOAD = "//a[text()='*#*']//preceding-sibling::i[contains(@class,'_critical-error')]";
		protected const string PROJECT_WARNING_ERROR_LOAD = "//a[text()='*#*']//preceding-sibling::i[contains(@class,'_error')]";
		protected const string PROJECTS_TABLE_XPATH = "//table[contains(@class,'js-tasks-table')]";
		protected const string DELETE_BUTTON = "//div[contains(@data-bind,'deleteProjects')]";
		protected const string PROJECT_SEARCH_FIELD = "//input[@name='searchName']";
		protected const string SEARCH_PROJECT_BUTTON = "//div[contains(@class, 'js-search-btn')]";

		protected const string PROJECT_REF = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']";
		protected const string PROJECT_STATUS = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../..//following-sibling::td[contains(@class, 'status-td')]//input";
		protected const string OPEN_PROJECT_FOLDER = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//preceding-sibling::div//i[contains(@class,'closed')]";
		protected const string PROJECT_CHECKBOX = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../../../td[contains(@class,'checkbox')]";
		protected const string OPEN_PROJECT = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/ancestor-or-self::tr";
		protected const string DOCUMENT_REF = "//tr[contains(@class,'js-document-row')]//a[text()='*#*']";
		protected const string DOWNLOAD_MAIN_MENU_BUTTON = "//div[contains(@class,'js-document-export-block')]";
		protected const string DOWNLOAD_IN_PROJECT_BUTTON = "//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//div[contains(@class,'js-buttons-left')]//li/div[contains(@data-bind, 'menuButton')]";
		protected const string DOWNLOAD_IN_DOCUMENT_BUTTON ="//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]//following-sibling::tr[1]//div[contains(@class,'js-buttons-left')]//li";
		protected const string DOCUMENT_ROW = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]";
		protected const string DOCUMENT_PROGRESS = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]//div[@class='ui-progressbar__container']";
		protected const string DOCUMENT_SETTINGS = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]//following-sibling::tr[1]//div[contains(@data-bind, 'actions.edit')]";
		protected const string DOCUMENT_TASK_ASSIGN_BUTTON = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../../../following-sibling::tr[contains(@class, 'js-document-row')][*##*]/following-sibling::tr[contains(@class, 'js-document-panel')]//div[contains(@data-bind, 'click: actions.assign')]//a";
		protected const string UPLOAD_DOCUMENT_BUTTON = "//div[contains(@data-bind, 'click: importDocument')]";
		protected const string PROJECT_TASK_ASSIGN_BUTTON = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../../../following-sibling::tr//div[contains(@data-bind, 'click: assign')]";
		protected const string PROJECT_LINK = ".//table[contains(@class,'js-tasks-table')]//tr//a[@class='js-name'][text()='*#*']";
		protected const string DOCUMENT_CHECKBOX = ".//table[contains(@class,'js-tasks-table')]//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../../../following-sibling::tr[contains(@class, 'document-row')]//preceding-sibling::td//a[@title='*##*']/../..//preceding-sibling::td";
		protected const string SIGN_IN_TO_CONNECTOR_BUTTON = "//span[contains(@class,'login-connector-btn')]";
		protected const string QA_CHECK_BUTTON = "//table[contains(@class,'js-tasks-table')]//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//div[contains(@data-bind,'qaCheck')]";
		protected const string PROJECT_SETTINGS_BUTTON = "//table[contains(@class,'js-tasks-table')]//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//div[contains(@data-bind,'edit')]";
		protected const string PROJECT_ANALYSIS_BUTTON = "//table[contains(@class,'js-tasks-table')]//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//div[contains(@data-bind, 'analysis')]";
		protected const string UPLOAD_DOCUMENT_DIALOG = "//div[contains(@class,'js-popup-import-document')][2]";
		protected const string DELETE_IN_PROJECT_BUTTON = "//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//div[contains(@data-bind, 'deleteProject')]";
		protected const string PREPARING_DOWNLOWD_MESSAGE = "//span[contains(text(), 'Preparing documents for download. Please wait')]";
		protected const string DECLINE_BUTTON = "//div[contains(@data-bind, 'actions.reject')]";
		protected const string TASK_LIST = "//table[contains(@data-bind, 'workflowStagesForCurrentUser')]//tr";
		#endregion
	}
}
