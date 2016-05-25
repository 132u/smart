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
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class ProjectSettingsPage : WorkspacePage, IAbstractPage<ProjectSettingsPage>
	{
		public ProjectSettingsPage(WebDriver driver)
			: base(driver)
		{
		}

		public new ProjectSettingsPage LoadPage()
		{
			if (!IsProjectSettingsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось перейти на страницу проекта.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Pretranslate.
		/// </summary>
		public PretranslationDialog ClickPretranslateButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Pretranslate.");
			PretranslateButton.Click();

			return new PretranslationDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Выбрать тип экспорта
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		public ProjectSettingsPage ClickExportType(ExportType exportType)
		{
			CustomTestContext.WriteLine("Выбрать тип экспорта");
			ExportType = Driver.SetDynamicValue(How.XPath, EXPORT_TYPE, exportType.ToString());
			ExportType.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку статистики.
		/// </summary>
		public BuildStatisticsPage ClickStatisticsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку статистики.");
			StatisticsButton.Click();
			// Sleep нужен, чтоб вторая вкладка успела открыться, иначе количество открытых вкладок посчитается неправильно 
			Thread.Sleep(1000);
			if (Driver.WindowHandles.Count > 1)
			{
				Driver.SwitchTo().Window(Driver.WindowHandles.First()).Close();
				Driver.SwitchTo().Window(Driver.WindowHandles.Last());
			}

			return new BuildStatisticsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку "Загрузить файлы"
		/// </summary>
		public AddFilesStep ClickDocumentUploadButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Загрузить файлы'.");
			AddFilesButton.Click();

			return new AddFilesStep(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку "Settings" в разделе "Documents"
		/// </summary>
		public DocumentSettingsDialog ClickDocumentSettings()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Settings' в разделе 'Documents'.");
			DocumentSettingsButton.JavaScriptClick();

			return new DocumentSettingsDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на чекбокс документа
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsPage ClickProjectsTableCheckbox(string documentName)
		{
			CustomTestContext.WriteLine("Нажать на чекбокс напротив документа {0}.", documentName);
			ProjectsTableCheckbox = Driver.SetDynamicValue(How.XPath, PROJECTS_TABLE_CHECKBOX, documentName);
			ProjectsTableCheckbox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Назначить задачу' в открытой свёртке документа
		/// </summary>
		/// <param name="filePath">путь до документа</param>
		public TaskAssignmentPage ClickAssignButtonInDocumentInfo(string filePath)
		{
			var documentName = Path.GetFileNameWithoutExtension(filePath);
			HoverDocumentRow(documentName);
			CustomTestContext.WriteLine("Нажать на кнопку 'Назначить задачу' в открытой свёртке документа.");
			AssignTasksButtonInDocumentInfo = Driver.SetDynamicValue(How.XPath, ASSIGN_TASKS_BTN_IN_DOCUMENT_INFO, documentName);
			AssignTasksButtonInDocumentInfo.Click();
			Driver.SwitchToNewBrowserTab();

			return new TaskAssignmentPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку "Назначить задачу" на панели
		/// </summary>
		public TaskAssignmentPage ClickAssignButtonOnPanel()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Назначить задачу' на панели");
			AssignTasksButtonOnPanel.Click();
			Driver.SwitchToNewBrowserTab();

			return new TaskAssignmentPage(Driver).LoadPage();
		}


		/// <summary>
		/// Нажать кнопку 'QA Check'.
		/// </summary>
		public QualityAssuranceDialog ClickQACheckButton(string projectName)
		{
			CustomTestContext.WriteLine("Нажать кнопку 'QA Check'.");
			QaCheckButton.Click();

			return new QualityAssuranceDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Навести курсор на строку с документом
		/// </summary>
		/// <param name="documentName">путь до документа</param>
		public ProjectSettingsPage HoverDocumentRow(string documentName)
		{
			CustomTestContext.WriteLine("Навести курсор на строку с документом");
			DocumentRow = Driver.SetDynamicValue(How.XPath, DOCUMENT_ROW, documentName);
			DocumentRow.HoverElement();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по ссылке на документ (открыть его)
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public SelectTaskDialog OpenDocumentInEditorWithTaskSelect(string documentPath)
		{
			var documentName = Path.GetFileNameWithoutExtension(documentPath);
			HoverDocumentRow(documentName);
			CustomTestContext.WriteLine("Кликнуть по ссылке на документ {0} (открыть его).", documentName);
			TranslateButton = Driver.SetDynamicValue(How.XPath, TRANSLATE_BUTTON, documentName);
			TranslateButton.Click();
			Driver.SwitchToNewBrowserTab();

			return new SelectTaskDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Кликнуть по ссылке на документ (открыть его)
		/// </summary>
		/// <param name="documentPath">имя документа</param>
		public EditorPage OpenDocumentInEditorWithoutTaskSelect(string documentPath)
		{
			var documentName = Path.GetFileNameWithoutExtension(documentPath);
			HoverDocumentRow(documentName);
			CustomTestContext.WriteLine("Кликнуть по ссылке на документ {0} (открыть его).", documentName);
			TranslateButton = Driver.SetDynamicValue(How.XPath, TRANSLATE_BUTTON, documentName);
			TranslateButton.Click();
			Driver.SwitchToNewBrowserTab();

			return new EditorPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Удалить'
		/// </summary>
		public DeleteDocumentDialog ClickDeleteButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Удалить'.");
			DeleteButton.Click();

			return new DeleteDocumentDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку экспорта в главном меню
		/// </summary>
		public ProjectSettingsPage ClickDownloadInMainMenuButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку экспорта в главном меню");
			DownloadButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку экспорта в свертке документа.
		/// </summary>
		public ProjectSettingsPage ClickDocumnetDownloadButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку экспорта в свертке документа.");
			DocumentDownloadButton.Click();

			return LoadPage();
		}


		/// <summary>
		/// Кликнуть чекбокс у документа
		/// </summary>
		/// <param name="filePath">путь до документа</param>
		public ProjectSettingsPage ClickDocumentCheckbox(string filePath)
		{
			var documentName = Path.GetFileNameWithoutExtension(filePath);
			CustomTestContext.WriteLine("Кликнуть чекбокс у документа {0}.", documentName);
			DocumentCheckbox = Driver.SetDynamicValue(How.XPath, DOCUMENT_CHECKBOX, documentName);
			DocumentCheckbox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Настройки'
		/// </summary>
		public ProjectSettingsDialog ClickSettingsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Настройки'");
			SettingsButton.JavaScriptClick();

			return new ProjectSettingsDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Снять выделение с документов, если выделены все
		/// </summary>
		public ProjectSettingsPage UncheckAllChecboxesDocumentsTable()
		{
			CustomTestContext.WriteLine("Снять выделение с документов, если выделены все.");
			AllCheckoxes = Driver.SetDynamicValue(How.XPath, PROJECTS_TABLE_ALL_CHECKBOXES, "");

			if (AllCheckoxes.Selected)
			{
				AllCheckoxes.Click();
			}
			else
			{
				AllCheckoxes.Click();
				AllCheckoxes.Click();
			}

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку сортировки по имени документов, ожидая алерт.
		/// </summary>
		public void ClickSortByTranslationDocumentAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать на кнопку сортировки по имени документов, ожидая алерт.");

			SortByTranslationDocument.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по типу документа, ожидая алерт.
		/// </summary>
		public void ClickSortByTypeAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по типу документа, ожидая алерт.");

			SortByType.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по статусу документа, ожидая алерт.
		/// </summary>
		public void ClickSortByStatusAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по статусу документа");

			SortByStatus.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по Target-языку документа, ожидая алерт.
		/// </summary>
		public void ClickSortByTargetAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по Target-языку документа");

			SortByTarget.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по автору документа, ожидая алерт.
		/// </summary>
		public void ClickSortByAuthorAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по автору документа");

			SortByAuthor.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате создания документа, ожидая алерт.
		/// </summary>
		public void ClickSortByCreatedAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по дате создания документа");

			SortByCreated.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по QA документа, ожидая алерт.
		/// </summary>
		public void ClickSortByQAAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по QA документа");

			SortByQA.Click();
		}

		/// <summary>
		/// Нажать кнопку Decline.
		/// </summary>
		public ConfirmDeclineTaskDialog ClickDeclineButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Decline.");
			DeclineButton.Click();

			return new ConfirmDeclineTaskDialog(Driver).LoadPage();
		}
		/// <summary>
		/// Нажать кнопку редактирования памяти перевода.
		/// </summary>
		public EditTranslationMemoryDialog ClickEditTranslatioinMemoryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку редактирования памяти перевода.");
			EditTranslationMemoryButton.Click();

			return new EditTranslationMemoryDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Получить название поректа.
		/// </summary>
		public string GetProjectName()
		{
			CustomTestContext.WriteLine("Получить название поректа.");

			return ProjectName.Text.Trim();
		}

		/// <summary>
		/// Получить статус документа
		/// </summary>
		/// <param name="fileName">название документа</param>
		public string GetDocumentStatus(string fileName)
		{
			CustomTestContext.WriteLine("Получить статус документа.");

			return Driver.SetDynamicValue(How.XPath, DOCUMENT_STATUS, fileName).Text;
		}

		/// <summary>
		/// Кликнуть на статус документа.
		/// </summary>
		public ProjectSettingsPage ClickProjectStatus()
		{
			CustomTestContext.WriteLine("Кликнуть на статус документа.");
			ProjectStatus.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по статусу 'Отменён'.
		/// </summary>
		public ProjectSettingsPage ClickCancelledStatus()
		{
			CustomTestContext.WriteLine("Кликнуть по статусу 'Отменён'.");
			ProjectCancelledStatus.Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить имя группы проектов.
		/// </summary>
		public string GetProjectGroupName()
		{
			CustomTestContext.WriteLine("Получить имя группы проектов.");

			return ProjectGroupName.Text;
		}

		/// <summary>
		/// Получить текст из описания проекта.
		/// </summary>
		public string GetProjectDescription()
		{
			CustomTestContext.WriteLine("Получить текст из описания проекта.");

			return Description.Text;
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Нажать на прогресс в строке документа
		/// </summary>
		/// <param name="filePath">путь до документа</param>
		public ProjectSettingsPage ClickDocumentRow(string filePath)
		{
			var fileName = Path.GetFileNameWithoutExtension(filePath);
			CustomTestContext.WriteLine("Нажать на поле прогресс строке документа {0}.", fileName);
			DocumentProgress = Driver.SetDynamicValue(How.XPath, DOCUMENT_ROW, fileName);
			DocumentProgress.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать глоссарий по названию
		/// </summary>
		/// <param name="nameGlossary">название глоссария</param>
		public ProjectSettingsPage SelectGlossaryByName(string nameGlossary)
		{
			CustomTestContext.WriteLine("Выбрать глоссарий по названию {0}.", nameGlossary);
			var glossaryList = Driver.GetElementList(By.XPath(GLOSSARIES_LIST));
			var glossaryWasSet = false;

			for (var i = 0; i < glossaryList.Count; ++i)
			{
				if (glossaryList[i].Text.Contains(nameGlossary))
				{
					var glossaryWebElement = Driver.FindElement(By.XPath(GLOSSARY_CHECKBOX.Replace("*#*", (i + 1).ToString())));
					glossaryWebElement.AdvancedClick();

					var saveButtonWebElement = Driver.FindElement(By.XPath(EDIT_GLOSSARY_SAVE_BUTTON));
					saveButtonWebElement.ScrollAndClick();

					PretranslateButton.Scroll();

					glossaryWasSet = true;
				}
			}

			if (!glossaryWasSet)
			{
				throw new NoSuchElementException("Произошла ошибка:\n глоссарий не найден.");
			}

			return LoadPage();
		}

		/// <summary>
		/// Установить статус проекта 'Отменён'.
		/// </summary>
		public ProjectSettingsPage SetCancelledStatus()
		{
			ClickProjectStatus();
			ClickCancelledStatus();

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что глоссарий выбран
		/// </summary>
		/// <param name="glossary">глоссарий</param>
		public bool IsGlossaryChecked(string glossary)
		{
			CustomTestContext.WriteLine("Проверить, что глоссарий выбран.");
			GlossaryCheckboxByName = Driver.SetDynamicValue(How.XPath, GLOSSARY_CHECKBOX_BY_NAME, glossary);
			GlossaryCheckboxByName.Scroll();

			return GlossaryCheckboxByName.Selected;
		}

		/// <summary>
		/// Проверить, что для текущего пользователя отображается назначенная на него задача.
		/// </summary>
		public bool IsAssignTaskDisplayedForCurrentUser(TaskMode task)
		{
			CustomTestContext.WriteLine("Проверить, что для текущего пользователя отображается назначенная на него задача {0}.", task);

			return Driver.WaitUntilElementIsDisplay(By.XPath(TASK_FOR_CURRENT_USER.Replace("*#*", task.ToString())));
		}

		/// <summary>
		/// Проверить, что кнопка 'Add Files' отображается
		/// </summary>
		public bool IsAddFilesButtonDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Add Files' отображается.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ADD_FILES_BTN));
		}

		/// <summary>
		/// Проверить, что кнопка 'Assign Task' неактивна.
		/// </summary>
		public bool IsAssignTaskButtonDisabled()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Assign Task' неактивна.");

			return AssignTasksButtonOnPanel.GetAttribute("class").Contains("disable");
		}

		/// <summary>
		/// Проверить, что кнопка Download неактивна.
		/// </summary>
		public bool IsDownloadButtonDisabled()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка Download неактивна.");

			return DownloadButton.GetAttribute("class").Contains("disable");
		}

		/// <summary>
		/// Проверить, открылась ли страница настроек проекта
		/// </summary>
		public bool IsProjectSettingsPageOpened()
		{
			return IsDialogBackgroundDisappeared() &&
				Driver.WaitUntilElementIsDisplay(By.XPath(SETTINGS_BUTTON));
		}

		/// <summary>
		/// Дождаться загрузки документа
		/// </summary>
		public ProjectSettingsPage WaitUntilDocumentProcessed()
		{
			CustomTestContext.WriteLine("Дождаться загрузки документа");
			// Практически все, что сделано в этом методе является быстрым незамысловатым костылем,
			// пока не пофиксили баг с крутилкой. Тройной рефреш позволяет тестам проходить быстрее,
			// т.к. мы не ждем пол минуты или минуту, а делаем проверку через 10 секунд.

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(LOAD_DOC_IMG)))
			{
				RefreshPage<ProjectSettingsPage>();
			}

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(LOAD_DOC_IMG)))
			{
				RefreshPage<ProjectSettingsPage>();
			}

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(LOAD_DOC_IMG)))
			{
				RefreshPage<ProjectSettingsPage>();
			}

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(LOAD_DOC_IMG)))
			{
				throw new InvalidElementStateException("Произошла ошибка:\n документ загружается слишком долго");
			}

			return LoadPage();
		}

		/// <summary>
		/// Проверить, что кнопка 'Назначить задачу' присутствует
		/// </summary>
		public bool IsAssignButtonExist()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Назначить задачу' присутствует");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ASSIGN_TASKS_BTN_IN_DOCUMENT_INFO));
		}

		/// <summary>
		/// Проверить, что документ есть в проекте (для всех языков)
		/// </summary>
		/// <param name="documentPath">путь до документа</param>
		/// <param name="languages">массив языков</param>
		public bool IsDocumentExist(string documentPath, Language[] languages = null)
		{

			var baseDocumentName = Path.GetFileNameWithoutExtension(documentPath);

			IEnumerable<string> documentNames = languages != null && languages.Length > 1
				? languages.Select(l => String.Format("{0}_{1}", baseDocumentName, l.Description()))
				: new[] { baseDocumentName };

			foreach (var documentName in documentNames)
			{
				CustomTestContext.WriteLine("Проверить, что документ '{0}' есть в проекте для всех языков.", documentName);

				if (!Driver.WaitUntilElementIsDisplay(By.XPath(DOCUMENT_LIST_ITEM.Replace("*#*", documentName))))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Проверить, что отображается кнопка статистики.
		/// </summary>
		public bool IsStatisticsButtonDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отображается кнопка статистики.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(STATISTICS_BUTTON));
		}

		/// <summary>
		/// Проверить, что кнопка удаления файлов присутствует
		/// </summary>
		public bool IsDeleteFileButtonDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка удаления файлов присутствует.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_BUTTON));
		}

		/// <summary>
		/// Проверить, что кнопка 'QA Check' присутствует
		/// </summary>
		public bool IsQaCheckButtonDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'QA Check' присутствует.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(QA_CHECK_BUTTON));
		}

		/// <summary>
		/// Проверить, что кнопка 'Repetitions' присутствует
		/// </summary>
		public bool IsRepetitionsButtonDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Repetitions' присутствует.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(REPETITIONS_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = EXPORT_TYPE)]
		protected IWebElement ExportType { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_FILES_BTN)]
		protected IWebElement AddFilesButton { get; set; }

		[FindsBy(How = How.XPath, Using = ASSIGN_TASKS_BTN_ON_PANEL)]
		protected IWebElement AssignTasksButtonOnPanel { get; set; }

		protected IWebElement AssignTasksButtonInDocumentInfo { get; set; }

		[FindsBy(How = How.XPath, Using = DEFAULT_MT_CHECKBOX)]
		protected IWebElement DefaultMTCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_MT_BTN)]
		protected IWebElement SaveMTButton { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_BUTTON)]
		protected IWebElement DeleteButton { get; set; }

		[FindsBy(How = How.XPath, Using = DOWNLOAD_BUTTON)]
		protected IWebElement DownloadButton { get; set; }

		[FindsBy(How = How.XPath, Using = DOCUMENT_DOWNLOAD_BUTTON)]
		protected IWebElement DocumentDownloadButton { get; set; }

		protected IWebElement DocumentCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = SETTINGS_BUTTON)]
		protected IWebElement SettingsButton { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECTS_TABLE_ALL_CHECKBOXES)]
		protected IWebElement AllCheckoxes { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_TRANSLATION_DOCUMENT)]
		protected IWebElement SortByTranslationDocument { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_TYPE)]
		protected IWebElement SortByType { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_STATUS)]
		protected IWebElement SortByStatus { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_TARGET)]
		protected IWebElement SortByTarget { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_AUTHOR)]
		protected IWebElement SortByAuthor { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_CREATED)]
		protected IWebElement SortByCreated { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_QA)]
		protected IWebElement SortByQA { get; set; }

		[FindsBy(How = How.XPath, Using = DOCUMENT_SETTINGS_BUTTON)]
		protected IWebElement DocumentSettingsButton { get; set; }

		[FindsBy(How = How.XPath, Using = PRETRANSLATE_BUTTON)]
		protected IWebElement PretranslateButton { get; set; }

		[FindsBy(How = How.XPath, Using = EDIT_TRANSLATION_MEMORY_BUTTOON)]
		protected IWebElement EditTranslationMemoryButton { get; set; }

		[FindsBy(How = How.XPath, Using = PROGRESS_TOOLTIP)]
		protected IWebElement ProgressTooltip { get; set; }

		[FindsBy(How = How.XPath, Using = QA_CHECK_BUTTON)]
		protected IWebElement QaCheckButton { get; set; }

		[FindsBy(How = How.XPath, Using = STATISTICS_BUTTON)]
		protected IWebElement StatisticsButton { get; set; }

		[FindsBy(How = How.XPath, Using = DECLINE_BUTTON)]
		protected IWebElement DeclineButton { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_NAME)]
		protected IWebElement ProjectName { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_STATUS)]
		protected IWebElement ProjectStatus { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_CANCELLED_STATUS)]
		protected IWebElement ProjectCancelledStatus { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_GROUP_NAME)]
		protected IWebElement ProjectGroupName { get; set; }

		[FindsBy(How = How.XPath, Using = DESCRIPTION)]
		protected IWebElement Description { get; set; }

		protected IWebElement DocumentProgress { get; set; }
		protected IWebElement DocumentRefference { get; set; }
		protected IWebElement ProjectsTableCheckbox { get; set; }
		protected IWebElement TaskForCurrentUser { get; set; }
		protected IWebElement GlossaryCheckboxByName { get; set; }
		protected IWebElement DocumentRow { get; set; }
		protected IWebElement TranslateButton { get; set; }

		#endregion

		#region Описания XPath элементов страницы

		protected const string EXPORT_TYPE = "//div[not(contains(@class,'g-hidden'))]/div[contains(@data-bind,'*#*') and contains(@data-bind, 'export')]";
		protected const string EXPORT_TYPE_TMX = "//div[not(contains(@class,'g-hidden'))]/div[contains(@data-bind,'Tmx')]";

		protected const string ADD_FILES_BTN = "//button[contains(@data-bind, 'importDocument')]";
		protected const string ASSIGN_DIALOG = "//div[contains(@class,'js-popup-assign')][2]";
		protected const string PROJECTS_TABLE_ALL_CHECKBOXES = ".//table[contains(@id,'JColResizer')]//tr[@class = 'js-table-header']//th[1]//input";
		protected const string PROJECTS_TABLE_CHECKBOX = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[1]//input";
		protected const string PROJECTS_TABLE_STATUS_COMPLITED = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[5][contains(string(), 'Completed')]";
		protected const string PROJECT_STATUS = "//div[contains(@class, 'row')]//input[contains(@class, 'status')]";
		protected const string PROJECT_GROUP_NAME = "//div[text()='Project group']//parent::div//div[contains(@class, 'l-project-panel-info_content')]";
		protected const string PROJECT_CANCELLED_STATUS = "//div[contains(@class, 'row')]//ul//li[contains(text(), 'Cancelled')]";
		protected const string ASSIGN_TASKS_BTN_ON_PANEL = "//button[contains(@data-bind, 'click: assign')]";
		protected const string ASSIGN_TASKS_BTN_IN_DOCUMENT_INFO = "//span[text()='*#*']//ancestor::tr//button[@data-bind='click: actions.assign']";
		protected const string LOAD_DOC_IMG = "//img[contains(@data-bind,'processingInProgress')]";
		protected const string DOCUMENT_REF = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[2]//a";
		protected const string SAVE_MT_BTN = ".//span[contains(@data-bind, 'click: saveMTEngines')]//a";
		protected const string DESCRIPTION = "//div[@class='l-project-panel-info_content']";
		protected const string DEFAULT_MT_CHECKBOX = "//tbody[contains(@data-bind,'foreach: machineTranslators')]//tr[contains(string(), 'ABBYY')]//td[1]//input";
		protected const string DEFAULT_MT_CHECKBOX_STATE = "//tbody[contains(@data-bind,'foreach: machineTranslators')]//tr[contains(string(), 'ABBYY')]//td[1]//input[@data-value='true']";
		protected const string DELETE_BUTTON = "//div[contains(@data-bind, 'deleteDocuments')]";
		protected const string DOCUMENT_LIST_ITEM = ".//table[contains(@class,'js-documents-table')]//tbody//tr//a[text()='*#*']";
		protected const string DELETE_DOCUMENT_DIALOG = "//div[contains(@class,'js-popup-confirm')]";
		protected const string DOWNLOAD_BUTTON = "//div[contains(@class,'js-document-export-block')]";
		protected const string DOCUMENT_DOWNLOAD_BUTTON = "//div[contains(@class,'doc-panel')]//div[contains(@class,'js-document-export-block')]";
		protected const string DOCUMENT_CHECKBOX = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[2]//a//ancestor::td//preceding-sibling::td//input";
		protected const string DOCUMENT_ROW = "//span[text()='*#*']//ancestor::tr//td[@class='l-corpr__td']";
		protected const string TRANSLATE_BUTTON = "//span[text()='*#*']//ancestor::tr//a[contains(data-bind, editorUrl)]";
		protected const string DOCUMENT_SETTINGS_BUTTON = "//div[contains(@class, 'doc-panel-btns ')]//a[text()='Settings']";
		protected const string SETTINGS_BUTTON = "//button[contains(@data-bind,'click: edit')]";

		protected const string SORT_BY_TRANSLATION_DOCUMENT = "//th[contains(@data-sort-by,'name')]//a";
		protected const string SORT_BY_TYPE = "//th[contains(@data-sort-by,'fileExtension')]//a";
		protected const string SORT_BY_STATUS = "//th[contains(@data-sort-by,'statusName')]//a";
		protected const string SORT_BY_TARGET = "//th[contains(@data-sort-by,'targetLanguageString')]//a";
		protected const string SORT_BY_AUTHOR = "//th[contains(@data-sort-by,'createdByUserName')]//a";
		protected const string SORT_BY_CREATED = "//th[contains(@data-sort-by,'creationDate')]//a";
		protected const string SORT_BY_QA = "//th[contains(@data-sort-by,'qaErrorCount')]//a";

		protected const string PROJECT_SETTIGS_HEADER = "//div[contains(@class, 'popup-edit')][2]//h2[text()='Project Settings']";
		protected const string GLOSSARIES_LIST = "//div[@class='g-page']//table//tbody[@data-bind='foreach: glossaries']//tr//td[2]";
		protected const string GLOSSARY_CHECKBOX = "//div[@class='g-page']//table//tbody[@data-bind='foreach: glossaries']//tr[*#*]//input";
		protected const string EDIT_GLOSSARY_SAVE_BUTTON = "//div[contains(@data-bind,'click: saveGlossaries')]";
		protected const string PRETRANSLATE_BUTTON = "//div[contains(@data-bind,'click: pretranslate')]";
		protected const string EDIT_TRANSLATION_MEMORY_BUTTOON = "//div[@data-bind='click: editTranslationMemories']//a[contains(@class, 'g-graybtn')]";
		protected const string PROGRESS_TOOLTIP = "//table[@class='l-workflow-progress-tooltip']";

		protected const string QA_CHECK_BUTTON = "//div[contains(@data-bind,'qaCheck')]";
		protected const string STATISTICS_BUTTON = "//div[contains(@data-bind,'openStatistics')]";
		protected const string PROJECT_NAME = "//a[@class='current-doc']";
		protected const string TASK_FOR_CURRENT_USER = "//table[contains(@data-bind, 'workflowStagesForCurrentUser')]//td[contains(@class, 'assignments') and contains(text(),'*#*')]";
		protected const string DECLINE_BUTTON = "//table[contains(@data-bind, 'workflowStagesForCurrentUser')]//div[contains(@data-bind, 'reject')]";
		protected const string DOCUMENT_STATUS = "//tr[contains(@class, 'document-row')]//td//a[contains(text(),'*#*')]/../../..//td[contains(@class,'status')]//p";
		protected const string GLOSSARY_CHECKBOX_BY_NAME = "//td//p[text()='*#*']/../preceding-sibling::td//input";

		protected const string REPETITIONS_BUTTON = "//div[@data-bind='click: setupRepetitions']";

		#endregion
	}
}
