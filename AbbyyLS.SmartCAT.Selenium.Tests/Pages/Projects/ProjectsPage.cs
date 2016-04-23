using System;
using System.Collections.Generic;
using System.IO;

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
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class ProjectsPage : WorkspacePage, IAbstractPage<ProjectsPage>
	{
		public ProjectsPage(WebDriver driver) : base(driver)
		{
		}

		public new ProjectsPage LoadPage()
		{
			if (!IsProjectsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось перейти на вкладку \"Проекты\".");
			}

			return this;
		}
		
		#region Простые методы страницы

		/// <summary>
		/// Нажать зеленую кнопку создания проекта.
		/// </summary>
		public NewProjectDocumentUploadPage ClickGreenCreateProjectButton()
		{
			CustomTestContext.WriteLine("Нажать зеленую кнопку создания проекта.");
			OpenHideMenuIfClosed();
			GreenCreateProjectButton.Click();

			return new NewProjectDocumentUploadPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'QA Check'.
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public QualityAssuranceDialog ClickQACheckButton(string projectName)
		{
			CustomTestContext.WriteLine("Нажать кнопку 'QA Check' у проекта '{0}'.", projectName);
			QualityAssuranceCheckButton = Driver.SetDynamicValue(How.XPath, QA_CHECK_BUTTON, projectName);
			QualityAssuranceCheckButton.Click();

			return new QualityAssuranceDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Выбрать тип экспорта
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		public ProjectsPage ClickExportType(ExportType exportType)
		{
			CustomTestContext.WriteLine("Выбрать тип экспорта");
			ExportType = Driver.SetDynamicValue(How.XPath, EXPORT_TYPE, exportType.ToString());
			ExportType.Click();

			return LoadPage();
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

			return new ProjectSettingsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Кликнуть по ссылке проекта без открытия страницы проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage ClickProjectWithoutProjectSettingPageOpened(string projectName)
		{
			CustomTestContext.WriteLine("ККликнуть по ссылке проекта без открытия страницы проекта {0}.", projectName);
			ProjectRef = Driver.SetDynamicValue(How.XPath, PROJECT_REF, projectName);
			ProjectRef.JavaScriptClick();
			
			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Создать проект'
		/// </summary>
		public NewProjectDocumentUploadPage ClickCreateProjectButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Создать проект'.");
			CreateProjectButton.Click();

			return new NewProjectDocumentUploadPage(Driver).LoadPage();
		}

		/// <summary>
		/// Поставить галочку в главном чекбоксе.
		/// </summary>
		public ProjectsPage ClickMainCheckbox()
		{
			CustomTestContext.WriteLine("Поставить галочку в главном чекбоксе.");
			MainCheckboxe.Click();

			return LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Удалить'
		/// </summary>
		public DeleteDialog ClickDeleteButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Удалить'.");
			DeleteButton.ScrollAndClick();

			return new DeleteDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Удалить' с открытой свёрткой проекта и одним файлом в меню проекта
		/// </summary>
		/// <param name="projectName">название проекта</param>
		public DeleteProjectOrFileDialog ClickDeleteOpenProjectWithOneFileInProjectMenu(string projectName)
		{
			CustomTestContext.WriteLine(
				"Нажать на кнопку 'Удалить' с открытой свёрткой проекта и одним файлом в меню проекта");
			DeleteInProjectButton = Driver.SetDynamicValue(How.XPath, DELETE_IN_PROJECT_BUTTON, projectName);
			DeleteInProjectButton.Click();

			return new DeleteProjectOrFileDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Decline.
		/// </summary>
		public TaskDeclineDialog ClickDeclineButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Decline.");
			DeclineButton.Click();

			return new TaskDeclineDialog(Driver).LoadPage();
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

			return LoadPage();
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

			return LoadPage();
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

			return new TaskAssignmentPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку прав пользователя в свертке проекта
		/// </summary>
		/// <param name="projectName"> Название проекта </param>
		public TaskAssignmentPage ClickProjectAssignButton(string projectName)
		{
			CustomTestContext.WriteLine("Нажать на кнопку прав пользователя в свертке проекта.");
			Driver.SetDynamicValue(How.XPath, PROJECT_TASK_ASSIGN_BUTTON, projectName).Click();

			return new TaskAssignmentPage(Driver).LoadPage();
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

			Driver.SwitchToNewBrowserTab();

			return new EditorPage(Driver).LoadPage();
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

			Driver.SwitchToNewBrowserTab();

			return new SelectTaskDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку экспорта в главном меню
		/// </summary>
		public ProjectsPage ClickDownloadInMainMenuButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку экспорта в главном меню");
			DownloadInMainMenuButton.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку экспорта в меню проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage ClickDownloadInProjectMenuButton(string projectName)
		{
			CustomTestContext.WriteLine("Нажать кнопку экспорта в меню проекта");
			DownloadInProjectButton = Driver.SetDynamicValue(How.XPath, DOWNLOAD_IN_PROJECT_BUTTON, projectName);
			DownloadInProjectButton.Click();

			return LoadPage();
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

			return LoadPage();
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

			return LoadPage();
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

			return new DocumentSettingsDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку загрузки файла
		/// </summary>
		public AddFilesStep ClickDocumentUploadButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку загрузки файла");
			UploadDocumentButton.Click();

			return new AddFilesStep(Driver).LoadPage();
		}

		/// <summary>
		/// Отметить чекбокс документа
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="documentName">имя документа</param>
		/// <param name="jobs">наличие джоб</param>
		public ProjectsPage SelectDocument(string projectName, string documentPath, bool jobs = false)
		{
			var documentName = Path.GetFileNameWithoutExtension(documentPath);
			CustomTestContext.WriteLine("Отметить чекбокс документа {0} в проекте {1}", documentName, projectName);
			if (jobs)
			{
				DocumentCheckBox = Driver.SetDynamicValue(How.XPath, DOCUMENT_WITH_JOBS_CHECKBOX, projectName, documentName);
			}
			else
			{
				DocumentCheckBox = Driver.SetDynamicValue(How.XPath, DOCUMENT_CHECKBOX, projectName, documentName);
			}
			DocumentCheckBox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Отметить чекбокс джоба документа
		/// <param name="projectName">имя проекта</param>
		/// <param name="documentName">имя документа</param>
		/// <param name="jobLanguage">язык джобы</param>
		public ProjectsPage SelectDocumentJob(string projectName, string documentPath, Language jobLanguage)
		{
			var documentName = Path.GetFileNameWithoutExtension(documentPath);
			CustomTestContext.WriteLine("Отметить чекбокс джоба языка {2} документа {0} в проекте {1}", documentName, projectName, jobLanguage.Description());
			DocumentJob = Driver.SetDynamicValue(How.XPath, DOCUMENT_JOB, projectName, documentName, jobLanguage.Description());
			DocumentJob.Click();
			
			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку удаления в меню проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public DeleteDialog ClickDeleteInProjectMenuButton(string projectName)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления в меню проекта {0}", projectName);
			DeleteInProjectButton = Driver.SetDynamicValue(How.XPath, DELETE_IN_PROJECT_BUTTON, projectName);
			DeleteInProjectButton.Click();

			return new DeleteDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку настроек проекта в открытой свёртке.
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectSettingsDialog ClickProjectSettingsButton(string projectName)
		{
			CustomTestContext.WriteLine("Нажать кнопку настроек проекта '{0}' в открытой свёртке.", projectName);
			ProjectSettingsButton = Driver.SetDynamicValue(How.XPath, PROJECT_SETTINGS_BUTTON, projectName);
			ProjectSettingsButton.Click();

			return new ProjectSettingsDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку анализа проекта в открытой свёртке.
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public BuildStatisticsPage ClickProjectStatisticsButtonExpectingBuildStatisticsPage(string projectName)
		{
			CustomTestContext.WriteLine("Нажать кнопку статистики проекта '{0}' в открытой свёртке, ожидая, что откроестя страница построения статистики.", projectName);
			ProjectStatisticsButton = Driver.SetDynamicValue(How.XPath, PROJECT_STATISTICS_BUTTON, projectName);
			ProjectStatisticsButton.Click();

			Driver.SwitchToNewBrowserTab();

			return new BuildStatisticsPage(Driver).LoadPage();
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

		/// <summary>
		/// Получить статус проекта
		/// </summary>
		/// <param name="projectName">название проекта</param>
		public string GetProjectStatusRights(string projectName)
		{
			CustomTestContext.WriteLine("Получить статус проекта");
			ProjectStatusRights = Driver.SetDynamicValue(How.XPath, PROJECT_STATUS_RIGHTS, projectName);
			
			return ProjectStatusRights.Text;
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

			return new TaskDeclineDialog(Driver).LoadPage();
		}

		///<summary>
		/// Открыть диалог назначения задачи
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="documentNumber">номер документа</param>
		public TaskAssignmentPage OpenAssignDialog(string projectName, int documentNumber = 1)
		{
			CustomTestContext.WriteLine("Открыть диалог назначения задачи.");
			OpenProjectInfo(projectName);
			OpenDocumentInfoForProject(projectName, documentNumber);
			var taskAssignmentPage = ClickDocumentAssignButton(projectName, documentNumber);

			return taskAssignmentPage.LoadPage();
		}

		/// <summary>
		///  Открыть диалог назначения задачи для нескольких документов
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="filePathList">список документов</param>
		public TaskAssignmentPage OpenAssignDialogForSelectedDocuments(string projectName, IList<string> filePathList)
		{
			CustomTestContext.WriteLine("Открыть диалог назначения задачи для нескольких документов.");
			OpenProjectInfo(projectName);

			foreach (var file in filePathList)
			{
				SelectDocument(projectName, file);
			}
			
			ClickProjectAssignButton(projectName);

			return new TaskAssignmentPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что кнопка удаления неактивна в панели проекта.
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public bool IsDeleteButtonInProjectPanelDisabled(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что кнопка удаления неактивна в панели проекта '{0}'.", projectName);
			DeleteButtonInProjectPanel = Driver.SetDynamicValue(How.XPath, DELETE_BUTTON_IN_PROJECT_PANEL, projectName);
			DeleteButtonInProjectPanel.Scroll();

			return DeleteButtonInProjectPanel.GetAttribute("class").Contains("disable");
		}

		/// <summary>
		/// Проверить, что отображается кнопка загрузки документа.
		/// </summary>
		public bool IsDocumentUploadButtonDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отображается кнопка загрузки документа.");
			
			return Driver.WaitUntilElementIsDisplay(By.XPath(UPLOAD_DOCUMENT_BUTTON));
		}

		/// <summary>
		/// Проверить, что все чекбоксы чекнуты
		/// </summary>
		public bool AreAllCheckboxesChecked()
		{
			CustomTestContext.WriteLine("Проверить, что все чекбоксы чекнуты.");
			foreach (var checkbox in Driver.GetElementList(By.XPath(ALL_CHECKBOXES)))
			{
				if (!checkbox.Selected)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Проверить, открылась ли страница 'Проекты'
		/// </summary>
		public bool IsProjectsPageOpened()
		{
			return IsDialogBackgroundDisappeared() &&
				Driver.WaitUntilElementIsDisplay(By.XPath(PROJECTS_TABLE_XPATH), timeout: 45);
		}

		/// <summary>
		/// Проверить, что кноgка Assig Task активна
		/// </summary>
		public bool IsAssignTaskButtonInDocumentPanelDisabled(string projectName, int documentNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что кноgка Assig Task активна на вкладке документа.");
			DocumentTaskAssignButton = Driver.SetDynamicValue(How.XPath, DOCUMENT_TASK_ASSIGN_BUTTON, projectName, documentNumber.ToString());

			return DocumentTaskAssignButton.GetAttribute("class").Contains("disable");
		}

		/// <summary>
		/// Проверить, что кноgка Assig Task активна
		/// </summary>
		public bool IsAssignTaskButtonInProjectPanelDisabled(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что кноgка Assig Task активна на вкладке проекта.");
			ProjectAssignTaskButton = Driver.SetDynamicValue(How.XPath, PROJECT_TASK_ASSIGN_BUTTON, projectName);

			return ProjectAssignTaskButton.GetAttribute("class").Contains("disable");
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

			var isProjectExist = Driver.WaitUntilElementIsAppear(By.XPath(PROJECT_REF.Replace("*#*", projectName)), 5);

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
		public bool IsQualityAssuranceCheckButtonDisplayed(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'QA Check' у проекта '{0}' видима.", projectName);
			QualityAssuranceCheckButton = Driver.SetDynamicValue(How.XPath, QA_CHECK_BUTTON, projectName);

			return QualityAssuranceCheckButton.Displayed;
		}

		/// <summary>
		/// Посчитать количество задач.
		/// </summary>
		public int GetTasksCount()
		{
			CustomTestContext.WriteLine("Посчитать количество задач.");

			return Driver.GetElementsCount(By.XPath(TASK_LIST));
		}

		/// <summary>
		/// Проверить, присутствует ли ссылка на документ на странице
		/// </summary>
		/// <param name="documentPath">имя документа</param>
		/// <param name="projectName">имя проекта</param>
		public bool IsDocumentRemovedFromProject(string projectName, string documentPath)
		{
			var documentName = Path.GetFileName(documentPath);

			CustomTestContext.WriteLine(
				"Проверить, присутствует ли документ {0} на в проекте {1}", documentName, projectName);

			return Driver.WaitUntilElementIsDisappeared(
				By.XPath(DOCUMENT_REF_IN_PROJECT.Replace("*#*", projectName).Replace("*##*", documentName)));
		}


		/// <summary>
		/// Проверить, что кнопка 'Add Files' отображается
		/// </summary>
		public bool IsAddFilesButtonDisplayed()
		{
			CustomTestContext.WriteLine(" Проверить, что кнопка 'Add Files' отображается.");
			
			return Driver.ElementIsDisplayed(By.XPath(UPLOAD_DOCUMENT_BUTTON));
		}

		/// <summary>
		/// Проверить, что отображается кнопка 'Создать проект'
		/// </summary>
		public bool IsCreateProjectButtonDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отображается кнопка 'Создать проект'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_PROJECT_BTN_XPATH));
		}

		/// <summary>
		/// Проверить, что отображается зеленая кнопка 'Создать проект'
		/// </summary>
		public bool IsGreenCreateProjectButtonDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отображается зеленая нопка 'Создать проект'.");
			OpenHideMenuIfClosed();

			return Driver.WaitUntilElementIsDisplay(By.XPath(GREEN_CREATE_PROJECT_BUTTON));
		}

		/// <summary>
		/// Проверить, что раскрыта вкладка проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public bool IsProjectPanelExpanded(string projectName)
		{
			CustomTestContext.WriteLine("Проверить, что раскрыта вкладка проекта {0}", projectName);

			return getClassAttributeProjectInfo(projectName).Contains("opened");
		}

		/// <summary>
		/// Проверить, что отображается кнопка удалить
		/// </summary>
		public bool IsProjectDeleteButtonDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отображается кнопка удалить");

			return Driver.GetIsElementExist(By.XPath(DELETE_BUTTON));
		}
		
		public bool IsMyTaskDisplayed(string projectName, int documentNumber = 1, WorkflowTask task = WorkflowTask.Translation)
		{
			CustomTestContext.WriteLine("Проверить, что отображается задача {0} для текущего пользователя в проекте {1}.", task, projectName);
			
			return Driver.WaitUntilElementIsDisplay(By.XPath(
				MY_TASK.Replace("*#*", projectName).Replace("*##*", documentNumber.ToString()).Replace("*###*", task.ToString())));
		}

		/// <summary>
		/// Проверить, что кнопка Delete отображается
		/// </summary>
		public bool IsDeleteProjectButtonDisplayed()
		{
			CustomTestContext.WriteLine(" Проверить, что кнопка Delete отображается.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_BUTTON));
		}

		/// <summary>
		/// Проверить, что кнопка Delete отображается
		/// </summary>
		public bool IsDeleteFileButtonDisplayed(string projectName)
		{
			CustomTestContext.WriteLine(" Проверить, что кнопка Delete отображается.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_IN_PROJECT_BUTTON.Replace("*#*", projectName)));
		}

		/// <summary>
		/// Проверить, что отображается кнопка экспорта для документа
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <param name="documentNumber">номер документа</param>
		public bool IsDocumentDownloadButtonDisplayed(string projectName, int documentNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что отображается кнопка экспорта для документа {0} в проекте {1}.", documentNumber, projectName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(DOWNLOAD_IN_DOCUMENT_BUTTON.Replace("*#*", projectName).Replace("*##*", documentNumber.ToString())));
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

			return new ProjectsPage(Driver).LoadPage();
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

			return LoadPage();
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

		/// <summary>
		/// Получить спсиок джобов проекта
		/// </summary>
		/// <param name="projectName">название проекта</param>
		public IList<string> GetJobList(string projectName)
		{
			CustomTestContext.WriteLine("Получить спсиок джобов проекта '{0}'.", projectName);
			var jobList = Driver.GetTextListElement(By.XPath(JOB_LIST.Replace("*#*", projectName)));
			jobList.Sort();

			return jobList;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = MAIN_CHECKBOXE)]
		protected IWebElement MainCheckboxe { get; set; }

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

		[FindsBy(How = How.XPath, Using = PROJECT_STATISTICS_BUTTON)]
		protected IWebElement ProjectStatisticsButton { get; set; }

		[FindsBy(How = How.XPath, Using = DECLINE_BUTTON)]
		protected IWebElement DeclineButton { get; set; }

		[FindsBy(How = How.XPath, Using = GREEN_CREATE_PROJECT_BUTTON)]
		protected IWebElement GreenCreateProjectButton { get; set; }

		protected IWebElement DownloadInProjectButton { get; set; }

		protected IWebElement DownloadInDocumentButton { get; set; }

		protected IWebElement ProjectRef { get; set; }

		protected IWebElement ProjectCheckbox { get; set; }

		protected IWebElement OpenProjectFolder { get; set; }

		protected IWebElement DocumentRow { get; set; }

		protected IWebElement DocumentProgress { get; set; }

		protected IWebElement DocumentTaskAssignButton {get; set;}
		protected IWebElement MyTask {get; set;}
		protected IWebElement DocumentRef { get; set; }
		protected IWebElement DocumentCheckBox { get; set; }
		protected IWebElement JobList { get; set; }
		protected IWebElement DocumentJob { get; set; }
		protected IWebElement DeleteInProjectButton { get; set; }
		protected IWebElement ProjectAssignTaskButton { get; set; }
		protected IWebElement QualityAssuranceCheckButton { get; set; }
		protected IWebElement ProjectStatusRights { get; set; }

		protected IWebElement DeleteButtonInProjectPanel { get; set; }

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
		protected const string OPEN_PROJECT_FOLDER = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//preceding-sibling::div";
		protected const string PROJECT_CHECKBOX = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../../../../td[contains(@class,'checkbox')]";
		protected const string OPEN_PROJECT = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/ancestor-or-self::tr";
		protected const string DOCUMENT_REF = "//tr[contains(@class,'js-document-row')]//a[text()='*#*']";
		protected const string DOCUMENT_LINK = "//a[text()='*#*']/../../../following-sibling::tr[contains(@class, 'l-project-row l-corpr__trhover clickable') and not(contains(@class, 'document-row '))]//span[text()='*##*']";
		protected const string DOCUMENT_JOB = "//a[text()='*#*']/../../../../following-sibling::tr//*[string()='*##*']/../../../following-sibling::tr[contains(@class, 'document-row') and contains(@class,'l-project-row')]//a[contains(text(),'*##*') and contains(text(),'*###*')]/../../..//input";
		protected const string DOCUMENT_REF_IN_PROJECT = "//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//..//..//..//..//tr[contains(@class,'js-document-row')]//a[text()='*##*']";
		protected const string DOWNLOAD_MAIN_MENU_BUTTON = "//div[contains(@class,'js-document-export-block')]";
		protected const string DOWNLOAD_IN_PROJECT_BUTTON = "//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//div[contains(@class,'js-buttons-left')]//li/div[contains(@data-bind, 'menuButton')]";
		protected const string DOWNLOAD_IN_DOCUMENT_BUTTON ="//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]//following-sibling::tr[1]//div[contains(@class,'js-buttons-left')]//li";
		protected const string DOCUMENT_ROW = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]";
		protected const string DOCUMENT_PROGRESS = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]//div[@class='ui-progressbar__container']";
		protected const string DOCUMENT_SETTINGS = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][*##*]//following-sibling::tr[1]//div[contains(@data-bind, 'actions.edit')]";
		protected const string DOCUMENT_TASK_ASSIGN_BUTTON = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../../../../following-sibling::tr[contains(@class, 'js-document-row')][*##*]/following-sibling::tr[contains(@class, 'js-document-panel')]//div[contains(@data-bind, 'click: actions.assign')]//a";
		protected const string UPLOAD_DOCUMENT_BUTTON = "//div[contains(@data-bind, 'click: importDocument')]";
		protected const string PROJECT_TASK_ASSIGN_BUTTON = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../../../../following-sibling::tr//div[contains(@data-bind, 'click: assign')]";
		protected const string PROJECT_LINK = ".//table[contains(@class,'js-tasks-table')]//tr//a[@class='js-name'][text()='*#*']";
		protected const string DOCUMENT_CHECKBOX = ".//table[contains(@class,'js-tasks-table')]//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../../../../following-sibling::tr[contains(@class, 'document-row')]//preceding-sibling::td//a[@title='*##*']/../..//preceding-sibling::td";
		protected const string DOCUMENT_WITH_JOBS_CHECKBOX = "//a[text()='*#*']/../../../../following-sibling::tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*##*']/../..//preceding-sibling::td//input";
		protected const string SIGN_IN_TO_CONNECTOR_BUTTON = "//span[contains(@class,'login-connector-btn')]";
		protected const string QA_CHECK_BUTTON = "//table[contains(@class,'js-tasks-table')]//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//div[contains(@data-bind,'qaCheck')]";
		protected const string PROJECT_SETTINGS_BUTTON = "//table[contains(@class,'js-tasks-table')]//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//div[contains(@data-bind,'edit')]";
		protected const string PROJECT_STATISTICS_BUTTON = "//table[contains(@class,'js-tasks-table')]//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//div[contains(@data-bind, 'Statistics')]";
		protected const string UPLOAD_DOCUMENT_DIALOG = "//div[contains(@class,'js-popup-import-document')][2]";
		protected const string DELETE_IN_PROJECT_BUTTON = "//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']//ancestor::tr//following-sibling::tr[1]//div[contains(@data-bind, 'deleteProject')]";
		protected const string PREPARING_DOWNLOWD_MESSAGE = "//span[contains(text(), 'Preparing documents for download. Please wait')]";
		protected const string DECLINE_BUTTON = "//div[contains(@data-bind, 'actions.reject')]";
		protected const string TASK_LIST = "//table[contains(@data-bind, 'workflowStagesForCurrentUser')]//tr";
		protected const string GREEN_CREATE_PROJECT_BUTTON = "//div[@class='g-page']//div[contains(@class, 'corprmenu')]//a[contains(@href, 'NewProject') and contains(@class, 'corprmenu__project-btn')]";
		protected const string MY_TASK = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../../../../following-sibling::tr[contains(@class, 'js-document-row')][*##*]/following-sibling::tr[contains(@class, 'js-document-panel')]//td[contains(@class,'my-assignments') and contains(text(),'*###*')]";
		protected const string JOB_LIST = "//a[text()='*#*']/../../../../following-sibling::tr//../../../../following-sibling::tr[contains(@class, 'js-document-row')]//a[contains(@class, 'project__doc-link')]";
		protected const string ALL_CHECKBOXES = "//input[@type='checkbox']";
		protected const string MAIN_CHECKBOXE = "//thead//tr[1]//input[@type='checkbox' and contains(@data-bind, 'allProjectsChecked')]";
		protected const string PROJECT_STATUS_RIGHTS = ".//table[contains(@class,'js-tasks-table')]//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='*#*']/../../..//following-sibling::td//p[contains(@data-bind, 'displayStatus')]";
		protected const string DELETE_BUTTON_IN_PROJECT_PANEL = "//a[text()='*#*']/../../../../following-sibling::tr//div[contains(@class, 'project__panel')]//div[contains(@data-bind, 'click: deleteProject')]";

		#endregion
	}
}
