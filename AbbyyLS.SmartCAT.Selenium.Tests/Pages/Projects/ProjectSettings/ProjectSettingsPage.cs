using System;
using System.Linq;
using System.Threading;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
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
			Driver.WaitPageTotalLoad();
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ADD_FILES_BTN)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти на вкладку проекта.");
			}
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
		/// Проверить, загрузился ли документ
		/// </summary>
		public ProjectSettingsPage AssertIsDocumentProcessed()
		{
			CustomTestContext.WriteLine("Проверить загрузился ли документ.");

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(LOAD_DOC_IMG), 320))
			{
				Driver.Navigate().Refresh();

				Assert.IsFalse(Driver.ElementIsDisplayed(By.XPath(LOAD_DOC_IMG)),
					"Произошла ошибка:\n документ загружается слишком долго.");
			}

			return GetPage();
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
		/// Проверить, что кнопка 'Назначить задачу' отсутствует
		/// </summary>
		public ProjectSettingsPage AssertAssignButtonNotExist()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Назначить задачу' отсутствует.");

			Assert.IsFalse(Driver.GetIsElementExist(By.XPath(ASSIGN_TASKS_BTN_IN_DOCUMENT_INFO)),
				"Произошла ошибка:\n кнопка 'Назначить задачу' отображается в открытой свёртке документа.");

			return GetPage();
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
		public T ClickDocument<T>(string documentName, WebDriver driver) where T: class, IAbstractPage<T>
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

			var instance = Activator.CreateInstance(typeof(T), new object[] { driver }) as T;
			return instance.GetPage();
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
		/// Проверить, что документа нет в проекте
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsPage AssertDocumentNotExist(string documentName)
		{
			CustomTestContext.WriteLine("Проверить, что документа '{0}' нет в проекте.", documentName);

			Assert.IsFalse(Driver.WaitUntilElementIsEnabled(By.XPath(DOCUMENT_LIST.Replace("*#*", documentName)), 5),
				"Произошла ошибка:\n документ {0} присутствует в проекте.", documentName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что документ есть в проекте
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsPage AssertDocumentExist(string documentName)
		{
			CustomTestContext.WriteLine("Проверить, что документ '{0}' есть в проекте.", documentName);

			Assert.IsTrue(Driver.WaitUntilElementIsEnabled(By.XPath(DOCUMENT_LIST.Replace("*#*", documentName)), 5),
				"Произошла ошибка:\n документ {0} отсутствует в проекте.", documentName);

			return GetPage();
		}

		/// <summary>
		/// Ожидаем закрытия диалога удаления документа
		/// </summary>
		public ProjectSettingsPage WaitDeleteDocumentDialogDissappeared()
		{
			CustomTestContext.WriteLine("Дождаться закрытия диалога удаления документа.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_DOCUMENT_DIALOG)),
				"Произошла ошибка:\n диалог удаления документа не закрылся.");

			return new ProjectSettingsPage(Driver);
		}

		/// <summary>
		/// Дождаться закрытия диалога импорта документа
		/// </summary>
		public ProjectSettingsPage WaitUntilUploadDocumentDialogDissapeared()
		{
			CustomTestContext.WriteLine("Дождаться закрытия диалога импорта документа.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(IMPORT_DIALOG)),
				"Произошла ошибка:\n диалог импорта документа не закрылся.");

			return GetPage();
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
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsPage ClickDocumentCheckbox(string documentName)
		{
			CustomTestContext.WriteLine("Кликнуть чекбокс у документа");
			
			DocumentCheckbox = Driver.SetDynamicValue(How.XPath, DOCUMENT_CHECKBOX, documentName);
			DocumentCheckbox.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на прогресс в строке документа
		/// </summary>
		/// <param name="documentName">имя документа(без расширения)</param>
		public ProjectSettingsPage ClickDocumentProgress(string documentName)
		{
			CustomTestContext.WriteLine("Нажать на поле прогресс строке документа {0}.", documentName);
			//Sleep необходим для предотвращения ошибки "stale element reference: element is not attached to the page document"
			Thread.Sleep(1000);
			DocumentProgress = Driver.SetDynamicValue(How.XPath, DOCUMENT_PROGRESS, documentName);
			DocumentProgress.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Настройки'
		/// </summary>
		public SettingsDialog ClickSettingsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Настройки'");
			SettingsButton.Click();

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
		/// Нажать на кнопку сортировки по имени документов.
		/// </summary>
		public ProjectSettingsPage ClickSortByTranslationDocument()
		{
			CustomTestContext.WriteLine("Нажать на кнопку сортировки по имени документов.");
			SortByTranslationDocument.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по типу документа
		/// </summary>
		public ProjectSettingsPage ClickSortByType()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по типу документа");
			SortByType.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по статусу документа
		/// </summary>
		public ProjectSettingsPage ClickSortByStatus()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по статусу документа");
			SortByStatus.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по Target-языку документа
		/// </summary>
		public ProjectSettingsPage ClickSortByTarget()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по Target-языку документа");
			SortByTarget.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по автору документа
		/// </summary>
		public ProjectSettingsPage ClickSortByAuthor()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по автору документа");
			SortByAuthor.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате создания документа
		/// </summary>
		public ProjectSettingsPage ClickSortByCreated()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по дате создания документа");
			SortByCreated.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по QA документа
		/// </summary>
		public ProjectSettingsPage ClickSortByQA()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по QA документа");
			SortByQA.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что диалог настроек проекта закрылся
		/// </summary>
		public ProjectSettingsPage AssertSettingsDialogDissappear()
		{
			CustomTestContext.WriteLine("Проверить, что диалог настроек проекта закрылся.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(PROJECT_SETTIGS_HEADER)), "Произошла ошибка:\n Диалог настроек проекта не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Settings" в разделе "Documents"
		/// </summary>
		public DocumentSettings ClickDocumentSettings()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Settings' в разделе 'Documents'.");
			DocumentSettingsButton.JavaScriptClick();

			return new DocumentSettings(Driver).GetPage();
		}

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

		protected IWebElement DocumentProgress { get; set; }

		protected IWebElement DocumentRefference { get; set; }
		
		protected IWebElement ProjectsTableCheckbox { get; set; }
		
		protected const string ADD_FILES_BTN = ".//span[contains(@class,'js-document-import')]";
		protected const string IMPORT_DIALOG = ".//div[contains(@class,'js-popup-import-document')][2]";
		protected const string ASSIGN_DIALOG = "//div[contains(@class,'js-popup-assign')][2]";
		protected const string PROJECTS_TABLE_ALL_CHECKBOXES = ".//table[contains(@id,'JColResizer')]//tr[@class = 'js-table-header']//th[1]//input";
		protected const string PROJECTS_TABLE_CHECKBOX = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[1]//input";
		protected const string PROJECTS_TABLE_STATUS_COMPLITED = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[5][contains(string(), 'Completed')]";
		protected const string ASSIGN_TASKS_BTN_ON_PANEL = "//div[@class='l-corpr__hd']//span[contains(@data-bind,'click: assign')]//a";
		protected const string ASSIGN_TASKS_BTN_IN_DOCUMENT_INFO = "//div[contains(@class,'doc-panel-btns')]//span[contains(@data-bind,'click: actions.assign')]//a";
		protected const string LOAD_DOC_IMG = "//img[contains(@data-bind,'processingInProgress')]";
		protected const string DOCUMENT_REF = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[2]//a";
		protected const string SAVE_MT_BTN = ".//span[contains(@data-bind, 'click: saveMTEngines')]//a";
		protected const string DEFAULT_MT_CHECKBOX = "//tbody[contains(@data-bind,'foreach: machineTranslators')]//tr[contains(string(), 'ABBYY')]//td[1]//input";
		protected const string DEFAULT_MT_CHECKBOX_STATE = "//tbody[contains(@data-bind,'foreach: machineTranslators')]//tr[contains(string(), 'ABBYY')]//td[1]//input[@data-value='true']";
		protected const string DELETE_BTN = "//span[contains(@class,'js-document-delete')]";
		protected const string DOCUMENT_LIST = ".//table[contains(@class,'js-documents-table')]//tbody//tr//a[text()='*#*']";
		protected const string DELETE_DOCUMENT_DIALOG = "//div[contains(@class,'js-popup-confirm')]";
		protected const string DOWNLOAD_MAIN_MENU_BUTTON = "//span[contains(@class,'js-document-export-block')]";
		protected const string DOCUMENT_CHECKBOX = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[2]//a//ancestor::td//preceding-sibling::td/input";
		protected const string DOCUMENT_PROGRESS = "//td[div[a[text()='*#*']]]//following-sibling::td//div[contains(@class,'ui-progressbar__container')]";
		protected const string DOCUMENT_SETTINGS_BUTTON = "//a[@class='intable g-bluebtn__text g-btn__text' and text()='Settings']";
        protected const string SETTINGS_BUTTON = "(//span[contains(@data-bind,'click: edit')])[1]";

		protected const string SORT_BY_TRANSLATION_DOCUMENT = "//th[contains(@data-sort-by,'name')]//a";
		protected const string SORT_BY_TYPE = "//th[contains(@data-sort-by,'fileExtension')]//a";
		protected const string SORT_BY_STATUS = "//th[contains(@data-sort-by,'workflowStatus')]//a";
		protected const string SORT_BY_TARGET = "//th[contains(@data-sort-by,'targetLanguageString')]//a";
		protected const string SORT_BY_AUTHOR = "//th[contains(@data-sort-by,'createdByUserName')]//a";
		protected const string SORT_BY_CREATED = "//th[contains(@data-sort-by,'creationDate')]//a";
		protected const string SORT_BY_QA = "//th[contains(@data-sort-by,'qaErrorCount')]//a";

		protected const string PROJECT_SETTIGS_HEADER = "//div[contains(@class, 'popup-edit')][2]//h2[text()='Project Settings']";
	}
}
