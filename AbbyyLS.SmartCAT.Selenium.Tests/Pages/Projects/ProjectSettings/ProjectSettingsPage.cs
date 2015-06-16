using System.Threading;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class ProjectSettingsPage : WorkspacePage, IAbstractPage<ProjectSettingsPage>
	{
		//TODO: дописать функционал по назначению задачи на пользователя 
		public new ProjectSettingsPage GetPage()
		{
			var projectSettingsPage = new ProjectSettingsPage();
			InitPage(projectSettingsPage);

			return projectSettingsPage;
		}

		public new void LoadPage()
		{
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
			Logger.Debug("Нажать на кнопку 'Загрузить файлы'.");
			AddFilesButton.Click();

			return new DocumentUploadGeneralInformationDialog().GetPage();
		}

		/// <summary>
		/// Проверить, загрузился ли документ
		/// </summary>
		public ProjectSettingsPage AssertIsDocumentProcessed()
		{
			Logger.Trace("Проверить загрузился ли документ.");
			if (!Driver.WaitUntilElementIsDissapeared(By.XPath(LOAD_DOC_IMG), 320))
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
			Logger.Debug("Нажать на чекбокс напротив документа {0}.", documentName);
			ProjectsTableCheckbox = Driver.SetDynamicValue(How.XPath, PROJECTS_TABLE_CHECKBOX, documentName);
			ProjectsTableCheckbox.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, завершен ли перевод документа
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsPage AssertIsStatusCompleted(string documentName)
		{
			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(PROJECTS_TABLE_STATUS_COMPLITED.Replace("*#*", documentName))),
				"Произошла ошибка:\n перевод документа {0} не завершен.", documentName);

			return GetPage();
		}

		/// <summary>
		/// Обновить страницу проекта
		/// </summary>
		public ProjectSettingsPage RefreshPage()
		{
			Logger.Debug("Обновить страницу.");
			Driver.Navigate().Refresh();

			return GetPage();
		}

		/// <summary>
		/// Проверить, выбран ли движок МТ
		/// </summary>
		public bool IsDefaultMTSelected()
		{
			Logger.Debug("Проверить выбран ли движок МТ.");

			return Driver.ElementIsDisplayed(By.XPath(DEFAULT_MT_CHECKBOX_STATE));
		}

		/// <summary>
		/// Выбрать ABBYY из таблицы движков
		/// </summary>
		public ProjectSettingsPage ClickDefaultMTCheckbox()
		{
			Logger.Debug("Выбрать ABBYY из таблицы МТ движков.");
			Thread.Sleep(1000);
			DefaultMTCheckbox.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Сохранить МТ"
		/// </summary>
		public ProjectSettingsPage ClickSaveMtButton()
		{
			Logger.Debug("Нажать кнопку 'Сохранить МТ'.");
			SaveMTButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Назначить задачу' на панели
		/// </summary>
		public TaskAssignmentDialog ClickAssignButtonOnPanel()
		{
			Logger.Debug("Нажать кнопку 'Назначить задачу'.");
			AssignTasksButtonOnPanel.Click();

			return new TaskAssignmentDialog().GetPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Назначить задачу' в открытой свёртке документа
		/// </summary>
		public TaskAssignmentDialog ClickAssignButtonInDocumentInfo()
		{
			Logger.Debug("Нажать на кнопку 'Назначить задачу' в открытой свёртке документа.");
			AssignTasksButtonInDocumentInfo.Click();

			return new TaskAssignmentDialog().GetPage();
		}

		/// <summary>
		/// Снять выделение с документов, если выделены все
		/// </summary>
		public ProjectSettingsPage UncheckAllChecboxesDocumentsTable()
		{
			Logger.Debug("Снять выделение с документов, если выделены все.");
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
		/// Кликнуть по ссылке на документ (открыть его)
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public T ClickDocument<T>(string documentName) where T: class, IAbstractPage<T>, new()
		{
			Logger.Debug("Кликнуть по ссылке на документ {0} (открыть его).", documentName);
			DocumentRef = Driver.SetDynamicValue(How.XPath, DOCUMENT_REF, documentName);
			DocumentRef.Click();
			Driver.SwitchTo().Window(Driver.WindowHandles[1]);

			return new T().GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Принять задачу'
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsPage ClickAcceptButton(string documentName)
		{
			Logger.Debug("Нажать кнопку 'Принять задачу'.");
			Driver.Navigate().Refresh();
			AcceptButton = Driver.SetDynamicValue(How.XPath, ACCEPT_BTN, documentName);
			AcceptButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Удалить'
		/// </summary>
		public DeleteDocumentDialog ClickDeleteButton()
		{
			Logger.Debug("Нажать кнопку 'Удалить'.");
			DeleteButton.Click();

			return new DeleteDocumentDialog().GetPage();
		}
		
		/// <summary>
		/// Проверить, что документа нет в проекте
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsPage AssertDocumentNotExist(string documentName)
		{
			Logger.Debug(string.Format("Проверка существования документа {0}", documentName));

			Assert.IsFalse(Driver.WaitUntilElementIsEnabled(By.XPath(DOCUMENT_LIST + documentName + "']"), 5),
				string.Format("Произошла ошибка:\n документ {0} присутствует в проекте.", documentName));

			return GetPage();
		}

		/// <summary>
		/// Ожидаем закрытия диалога удаления документа
		/// </summary>
		public ProjectSettingsPage WaitDeleteDocumentDialogDissappeared()
		{
			Logger.Trace("Дождаться закрытия диалога удаления документа.");

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(DELETE_DOCUMENT_DIALOG)),
				"Произошла ошибка:\n диалог удаления документа не закрылся.");

			return new ProjectSettingsPage();
		}

		/// <summary>
		/// Дождаться закрытия диалога импорта документа
		/// </summary>
		public ProjectSettingsPage WaitUntilUploadDocumentDialogDissapeared()
		{
			Logger.Trace("Дождаться закрытия диалога импорта документа.");

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(IMPORT_DIALOG)),
				"Произошла ошибка:\n диалог импорта документа не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Нажать на прогресс в строке документа
		/// </summary>
		/// <param name="documentName">имя документа(без расширения)</param>
		public ProjectSettingsPage ClickDocumentProgress(string documentName)
		{
			Logger.Debug("Нажать на поле прогресс строке документа.");

			DocumentProgress = Driver.SetDynamicValue(How.XPath, DOCUMENT_PROGRESS, documentName);
			DocumentProgress.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Настройки'
		/// </summary>
		public SettingsDialog ClickSettingsButton()
		{
			Logger.Debug("Нажать кнопку 'Настройки'");
			SettingsButton.Click();

			return new SettingsDialog().GetPage();
		}

		/// <summary>
		/// Подтвердить все назначения для документа
		/// </summary>
		public ProjectSettingsPage AcceptAllTasksForDocument(string documentName)
		{
			Logger.Debug("Подтвердить все назначения");

			var acceptButtonsList = Driver.GetElementList(By.XPath(ACCEPT_BUTTONS_LIST_FOR_DOCUMENT.Replace("*#*", documentName)));

			foreach (var item in acceptButtonsList)
			{
				item.Click();
			}

			return GetPage();
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

		[FindsBy(How = How.XPath, Using = SETTINGS_BUTTON)]
		protected IWebElement SettingsButton { get; set; }

		protected IWebElement DocumentProgress { get; set; }

		protected IWebElement AllCheckoxes { get; set; }

		protected IWebElement DocumentRef { get; set; }

		protected IWebElement AcceptButton { get; set; }

		protected IWebElement ProjectsTableCheckbox { get; set; }

		protected const string ADD_FILES_BTN = ".//span[contains(@class,'js-document-import')]";
		protected const string IMPORT_DIALOG = ".//div[contains(@class,'js-popup-import-document')][2]";
		protected const string ASSIGN_DIALOG = "//div[contains(@class,'js-popup-assign')][2]";
		protected const string PROJECTS_TABLE_ALL_CHECKBOXES = ".//table[contains(@id,'JColResizer')]//tr[@class = 'js-table-header']//th[1]//input";
		protected const string PROJECTS_TABLE_CHECKBOX = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[1]//input";
		protected const string PROJECTS_TABLE_STATUS_COMPLITED = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[5][contains(string(), 'Completed')]";
		protected const string ASSIGN_TASKS_BTN_ON_PANEL = "//div[@class='l-corpr__hd']//span[contains(@data-bind,'click: assign')]//a";
		protected const string ASSIGN_TASKS_BTN_IN_DOCUMENT_INFO = "//div[contains(@class,'doc-panel-btns')]//span[contains(@data-bind,'click: assign')]//a";
		protected const string ACCEPT_BTN = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[10]//span[contains(@class,'js-accept')]";
		protected const string LOAD_DOC_IMG = "//img[contains(@title,'Processing translation document')]";
		protected const string DOCUMENT_REF = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[2]//a";
		protected const string SAVE_MT_BTN = ".//span[contains(@data-bind, 'click: saveMTEngines')]//a";
		protected const string DEFAULT_MT_CHECKBOX = "//tbody[contains(@data-bind,'foreach: machineTranslators')]//tr[contains(string(), 'ABBYY')]//td[1]//input";
		protected const string DEFAULT_MT_CHECKBOX_STATE = "//tbody[contains(@data-bind,'foreach: machineTranslators')]//tr[contains(string(), 'ABBYY')]//td[1]//input[@data-value='true']";
		protected const string DELETE_BTN = "//span[contains(@class,'js-document-delete')]";
		protected const string DOCUMENT_LIST = ".//table[contains(@class,'js-documents-table')]//tbody//tr//a[text()='";
		protected const string DELETE_DOCUMENT_DIALOG = "//div[contains(@class,'js-popup-confirm')]";
		protected const string DOCUMENT_PROGRESS = "//td[div[a[text()='*#*']]]//following-sibling::td//div[contains(@class,'ui-progressbar__container')]";
		protected const string SETTINGS_BUTTON = "(//span[contains(@data-bind,'click: edit')])[1]";
		protected const string ACCEPT_BUTTONS_LIST_FOR_DOCUMENT = "//tr[td[div[a[text()='*#*']]]]//following-sibling::tr[1]//span[contains(@data-bind,'click: $parent.accept')]";
	}
}
