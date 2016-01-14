using System.IO;
using System.Linq;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class ProjectSettingsPage : WorkspacePage, IAbstractPage<ProjectSettingsPage>
	{
		public ProjectSettingsPage(WebDriver driver) : base(driver)
		{
		}

		public new ProjectSettingsPage GetPage()
		{
			var projectSettingsPage = new ProjectSettingsPage(Driver);
			InitPage(projectSettingsPage, Driver);

			return projectSettingsPage;
		}

		public new void LoadPage()
		{
			if (!IsProjectSettingsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось перейти на вкладку проекта.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Pretranslate.
		/// </summary>
		public PretranslationDialog ClickPretranslateButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Pretranslate.");
			PretranslateButton.Click();

			return new PretranslationDialog(Driver).GetPage();
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

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Загрузить файлы"
		/// </summary>
		public DocumentUploadGeneralInformationDialog ClickDocumentUploadButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Загрузить файлы'.");
			AddFilesButton.Click();

			return new DocumentUploadGeneralInformationDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Settings" в разделе "Documents"
		/// </summary>
		public DocumentSettingsDialog ClickDocumentSettings()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Settings' в разделе 'Documents'.");
			DocumentSettingsButton.JavaScriptClick();

			return new DocumentSettingsDialog(Driver).GetPage();
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

			return GetPage();
		}
		
		/// <summary>
		/// Нажать на кнопку 'Назначить задачу' в открытой свёртке документа
		/// </summary>
		public TaskAssignmentPage ClickAssignButtonInDocumentInfo()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Назначить задачу' в открытой свёртке документа.");
			AssignTasksButtonInDocumentInfo.Click();

			return new TaskAssignmentPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Назначить задачу" на панели
		/// </summary>
		public TaskAssignmentPage ClickAssignButtonOnPanel()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Назначить задачу' на панели");
			AssignTasksButtonOnPanel.Click();

			return new TaskAssignmentPage(Driver).GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке на документ (открыть его)
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public SelectTaskDialog OpenDocumentInEditorWithTaskSelect(string documentPath)
		{
			var documentName = Path.GetFileNameWithoutExtension(documentPath);
			CustomTestContext.WriteLine("Кликнуть по ссылке на документ {0} (открыть его).", documentName);
			DocumentRefference = Driver.SetDynamicValue(How.XPath, DOCUMENT_REF, documentName);
			DocumentRefference.Click();
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
		/// Кликнуть по ссылке на документ (открыть его)
		/// </summary>
		/// <param name="documentPath">имя документа</param>
		public EditorPage OpenDocumentInEditorWithoutTaskSelect(string documentPath)
		{
			var documetnName = Path.GetFileNameWithoutExtension(documentPath);
			CustomTestContext.WriteLine("Кликнуть по ссылке на документ {0} (открыть его).", documetnName);
			DocumentRefference = Driver.SetDynamicValue(How.XPath, DOCUMENT_REF, documetnName);
			DocumentRefference.Click();
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
		/// Кликнуть по ссылке на документ
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsPage ClickOnDocumentExpectingError(string documentName)
		{
			CustomTestContext.WriteLine("Кликнуть по ссылке на документ {0} (открыть его).", documentName);
			DocumentRefference = Driver.SetDynamicValue(How.XPath, DOCUMENT_REF, documentName);
			DocumentRefference.Click();
			// Sleep нужен, чтоб вторая вкладка успела открыться, иначе количество открытых вкладок посчитается неправильно 
			Thread.Sleep(1000);
			if (Driver.WindowHandles.Count > 1)
			{
				Driver.SwitchTo().Window(Driver.WindowHandles.First()).Close();
				Driver.SwitchTo().Window(Driver.WindowHandles.Last());
			}

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Удалить'
		/// </summary>
		public DeleteDocumentDialog ClickDeleteButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Удалить'.");
			DeleteButton.Click();

			return new DeleteDocumentDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку экспорта в главном меню
		/// </summary>
		public ProjectSettingsPage ClickDownloadInMainMenuButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку экспорта в главном меню");
			DownloadInMainMenuButton.Click();

			return GetPage();
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

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Настройки'
		/// </summary>
		public SettingsDialog ClickSettingsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Настройки'");
			SettingsButton.JavaScriptClick();

			return new SettingsDialog(Driver).GetPage();
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

			return GetPage();
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
		/// Нажать кнопку редактирования памяти перевода.
		/// </summary>
		public EditTranslationMemoryDialog ClickEditTranslatioinMemoryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку редактирования памяти перевода.");
			EditTranslationMemoryButton.Click();

			return new EditTranslationMemoryDialog(Driver).GetPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Нажать на прогресс в строке документа
		/// </summary>
		/// <param name="filePath">путь до документа</param>
		public ProjectSettingsPage ClickDocumentProgress(string filePath)
		{
			var fileName = Path.GetFileNameWithoutExtension(filePath);
			CustomTestContext.WriteLine("Нажать на поле прогресс строке документа {0}.", fileName);
			DocumentProgress = Driver.SetDynamicValue(How.XPath, DOCUMENT_PROGRESS, fileName);
			DocumentProgress.HoverElement();

			Driver.WaitUntilElementIsDisplay(By.XPath(PROGRESS_TOOLTIP));

			DocumentProgress = Driver.SetDynamicValue(How.XPath, DOCUMENT_PROGRESS, fileName);
			DocumentProgress.Click();

			return GetPage();
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

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

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

			return GetPage();
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
		/// Проверить, что документ есть в проекте
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public bool IsDocumentExist(string documentName)
		{
			CustomTestContext.WriteLine("Проверить, что документ '{0}' есть в проекте.", Path.GetFileNameWithoutExtension(documentName));

			return Driver.WaitUntilElementIsDisplay(By.XPath(DOCUMENT_LIST_ITEM.Replace("*#*", Path.GetFileNameWithoutExtension(documentName))));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = EXPORT_TYPE)]
		protected IWebElement ExportType { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_FILES_BTN)]
		protected IWebElement AddFilesButton { get; set; }

		[FindsBy(How = How.XPath, Using = ASSIGN_TASKS_BTN_ON_PANEL)]
		protected IWebElement AssignTasksButtonOnPanel { get; set; }

		[FindsBy(How = How.XPath, Using = ASSIGN_TASKS_BTN_IN_DOCUMENT_INFO)]
		protected IWebElement AssignTasksButtonInDocumentInfo { get; set; }

		[FindsBy(How = How.XPath, Using = DEFAULT_MT_CHECKBOX)]
		protected IWebElement DefaultMTCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_MT_BTN)]
		protected IWebElement SaveMTButton { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_BTN)]
		protected IWebElement DeleteButton { get; set; }

		[FindsBy(How = How.XPath, Using = DOWNLOAD_MAIN_MENU_BUTTON)]
		protected IWebElement DownloadInMainMenuButton { get; set; }

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

		protected IWebElement DocumentProgress { get; set; }

		protected IWebElement DocumentRefference { get; set; }
		
		protected IWebElement ProjectsTableCheckbox { get; set; }
		
		#endregion

		#region Описания XPath элементов страницы

		protected const string EXPORT_TYPE = "//div[not(contains(@class,'g-hidden'))]/div[contains(@data-bind,'*#*') and contains(@data-bind, 'export')]";
		protected const string EXPORT_TYPE_TMX = "//div[not(contains(@class,'g-hidden'))]/div[contains(@data-bind,'Tmx')]";

		protected const string ADD_FILES_BTN = "//div[contains(@data-bind, 'importDocument')]";
		protected const string IMPORT_DIALOG = ".//div[contains(@class,'js-popup-import-document')][2]";
		protected const string ASSIGN_DIALOG = "//div[contains(@class,'js-popup-assign')][2]";
		protected const string PROJECTS_TABLE_ALL_CHECKBOXES = ".//table[contains(@id,'JColResizer')]//tr[@class = 'js-table-header']//th[1]//input";
		protected const string PROJECTS_TABLE_CHECKBOX = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[1]//input";
		protected const string PROJECTS_TABLE_STATUS_COMPLITED = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[5][contains(string(), 'Completed')]";
		protected const string ASSIGN_TASKS_BTN_ON_PANEL = "//div[contains(@data-bind, 'click: assign')]";
		protected const string ASSIGN_TASKS_BTN_IN_DOCUMENT_INFO = "//div[contains(@class,'doc-panel-btns')]//div[@data-bind='click: actions.assign']//a";
		protected const string LOAD_DOC_IMG = "//img[contains(@data-bind,'processingInProgress')]";
		protected const string DOCUMENT_REF = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[2]//a";
		protected const string SAVE_MT_BTN = ".//span[contains(@data-bind, 'click: saveMTEngines')]//a";
		protected const string DEFAULT_MT_CHECKBOX = "//tbody[contains(@data-bind,'foreach: machineTranslators')]//tr[contains(string(), 'ABBYY')]//td[1]//input";
		protected const string DEFAULT_MT_CHECKBOX_STATE = "//tbody[contains(@data-bind,'foreach: machineTranslators')]//tr[contains(string(), 'ABBYY')]//td[1]//input[@data-value='true']";
		protected const string DELETE_BTN = "//div[contains(@data-bind, 'deleteDocuments')]";
		protected const string DOCUMENT_LIST_ITEM = ".//table[contains(@class,'js-documents-table')]//tbody//tr//a[text()='*#*']";
		protected const string DELETE_DOCUMENT_DIALOG = "//div[contains(@class,'js-popup-confirm')]";
		protected const string DOWNLOAD_MAIN_MENU_BUTTON = "//div[contains(@class,'js-document-export-block')]";
		protected const string DOCUMENT_CHECKBOX = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[2]//a//ancestor::td//preceding-sibling::td//input";
		protected const string DOCUMENT_PROGRESS = "//td[div[a[text()='*#*']]]//following-sibling::td//div[contains(@class,'ui-progressbar__container')]";
		protected const string DOCUMENT_SETTINGS_BUTTON = "//div[contains(@class, 'doc-panel-btns ')]//a[text()='Settings']";
		protected const string SETTINGS_BUTTON = "//i[contains(@data-bind,'click: edit')]";
		
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

		#endregion
	}
}
