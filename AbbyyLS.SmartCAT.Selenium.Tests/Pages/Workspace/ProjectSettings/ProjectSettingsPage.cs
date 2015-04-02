using System.Threading;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.ProjectSettings
{
	public class ProjectSettingsPage : WorkspacePage, IAbstractPage<ProjectSettingsPage>
	{
		//TODO: дописать функционал по назначению задачи на пользователя 
		public new ProjectSettingsPage GetPage()
		{
			var projectSettingsPage = new ProjectSettingsPage();
			InitPage(projectSettingsPage);
			LoadPage();
			return projectSettingsPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.XPath(ADD_FILES_BTN_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти на вкладку проекта.");
			}
		}

		/// <summary>
		/// Нажать кнопку "Загрузить файлы"
		/// </summary>
		public ImportDialog ClickAddFilesBtn()
		{
			Logger.Trace("Нажимаем на кнопку 'Загрузить файлы'.");
			AddFilesBtn.Click();
			var importDialig = new ImportDialog();
			return importDialig.GetPage();
		}

		/// <summary>
		/// Проверка, загрузился ли документ
		/// </summary>
		public ProjectSettingsPage AssertIsDocumentProcessed()
		{
			Logger.Trace("Проверяем загрузился ли документ.");
			if (!Driver.WaitUntilElementIsDissapeared(By.XPath(LOAD_DOC_IMG_XPATH), 320))
			{
				Driver.Navigate().Refresh();
				Assert.IsFalse(Driver.ElementIsPresent(By.XPath(LOAD_DOC_IMG_XPATH)),
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
			Logger.Trace("Нажимаем на чекбокс напротив документа {0}.", documentName);
			Thread.Sleep(1000);
			ProjectsTableCheckbox = Driver.SetDynamicValue(How.XPath, PROJECTS_TABLE_CHECKBOX_XPATH, documentName);
			ProjectsTableCheckbox.Click();
			return GetPage();
		}

		/// <summary>
		/// Проверка, завершен ли перевод документа
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsPage AssertIsStatusCompleted(string documentName)
		{
			Assert.IsTrue(Driver.ElementIsPresent(By.XPath(PROJECTS_TABLE_STATUS_COMPLITED_XPATH.Replace("*#*", documentName))),
				"Произошла ошибка:\n перевод документа {0} не завершен.", documentName);
			return GetPage();
		}

		/// <summary>
		/// Обновить страницу проекта
		/// </summary>
		public ProjectSettingsPage RefreshPage()
		{
			Logger.Trace("Обновляем страницу.");
			Driver.Navigate().Refresh();
			return GetPage();
		}

		/// <summary>
		/// Проверка, выбран ли движок МТ
		/// </summary>
		public bool IsDefaultMTSelected()
		{
			Logger.Trace("Проверяем выбран ли движок МТ.");
			return Driver.ElementIsPresent(By.XPath(DEFAULT_MT_CHECKBOX_STATE_XPATH));
		}

		/// <summary>
		/// Выбрать ABBYY из таблицы движков
		/// </summary>
		public ProjectSettingsPage ClickDefaultMTCheckbox()
		{
			Logger.Trace("Выбираем ABBYY из таблицы МТ движков.");
			Thread.Sleep(1000);
			DefaultMTCheckbox.Click();
			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Сохранить МТ"
		/// </summary>
		public ProjectSettingsPage ClickSaveMtBtn()
		{
			Logger.Trace("Нажимаем кнопку 'Сохранить МТ'.");
			SaveMTBtn.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Назначить задачу"
		/// </summary>
		public TaskAssignmentDialog ClickAssignBtn()
		{
			Logger.Trace("Нажимаем кнопку 'Назначить задачу'.");
			AssignTasksBtn.Click();
			var taskAssigmentDialog = new TaskAssignmentDialog();
			return taskAssigmentDialog.GetPage();
		}

		/// <summary>
		/// Снять выделение с документов, если выделены все
		/// </summary>
		public ProjectSettingsPage UncheckAllChecboxesDocumentsTable()
		{
			Logger.Trace("Снять выделение с документов, если выделены все.");
			AllCheckoxes = Driver.SetDynamicValue(How.XPath, PROJECTS_TABLE_ALL_CHECKBOXES_XPATH, "");
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
		/// Кликнуть по ссылке на документ( открыть его)
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public SelectTaskDialog ClickDocumentRef(string documentName)
		{
			Logger.Trace("Кликнуть по ссылке на документ {0}( открыть его).", documentName);
			DocumentRef = Driver.SetDynamicValue(How.XPath, DOCUMENT_REF_XPATH, documentName);
			DocumentRef.Click();
			Driver.SwitchTo().Window(Driver.WindowHandles[1]);
			var selectTaskDialog = new SelectTaskDialog();
			return selectTaskDialog.GetPage();
		}

		/// <summary>
		/// Нажать кнопку " Принять задачу"
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsPage ClickAcceptBtn(string documentName)
		{
			Logger.Trace("Нажимаем кнопку 'Принять задачу'.");
			Driver.Navigate().Refresh();
			AcceptBtn = Driver.SetDynamicValue(How.XPath, ACCEPT_BTN_XPATH, documentName);
			AcceptBtn.Click();
			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = ADD_FILES_BTN_XPATH)]
		protected IWebElement AddFilesBtn { get; set; }

		[FindsBy(How = How.XPath, Using = ASSIGN_TASKS_BTN_XPATH)]
		protected IWebElement AssignTasksBtn { get; set; }

		[FindsBy(How = How.XPath, Using = DEFAULT_MT_CHECKBOX_XPATH)]
		protected IWebElement DefaultMTCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_MT_BTN)]
		protected IWebElement SaveMTBtn { get; set; }

		protected IWebElement AllCheckoxes { get; set; }

		protected IWebElement DocumentRef { get; set; }

		protected IWebElement AcceptBtn { get; set; }

		protected IWebElement ProjectsTableCheckbox { get; set; }

		protected const string ADD_FILES_BTN_XPATH = ".//span[contains(@class,'js-document-import')]";
		protected const string IMPORT_DIALOG_XPATH = ".//div[contains(@class,'js-popup-import-document')][2]";
		protected const string ASSIGN_DIALOG_XPATH = "//div[contains(@class,'js-popup-assign')][2]";
		protected const string PROJECTS_TABLE_ALL_CHECKBOXES_XPATH = ".//table[contains(@id,'JColResizer')]//tr[@class = 'js-table-header']//th[1]//input";
		protected const string PROJECTS_TABLE_CHECKBOX_XPATH = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[1]//input";
		protected const string PROJECTS_TABLE_STATUS_COMPLITED_XPATH = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[5][contains(string(), 'Completed')]";
		protected const string ASSIGN_TASKS_BTN_XPATH = "//span[contains(@data-bind,'click: assign')]//a";
		protected const string ACCEPT_BTN_XPATH = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[10]//span[contains(@class,'js-accept')]";
		protected const string LOAD_DOC_IMG_XPATH = "//img[contains(@title,'Processing translation document')]";
		protected const string DOCUMENT_REF_XPATH = ".//table[contains(@id,'JColResizer')]//tr[contains(string(), '*#*')]//td[2]//a";
		protected const string SAVE_MT_BTN = ".//span[contains(@data-bind, 'click: saveMTEngines')]//a";
		protected const string DEFAULT_MT_CHECKBOX_XPATH = "//tbody[contains(@data-bind,'foreach: machineTranslators')]//tr[contains(string(), 'ABBYY')]//td[1]//input";
		protected const string DEFAULT_MT_CHECKBOX_STATE_XPATH = "//tbody[contains(@data-bind,'foreach: machineTranslators')]//tr[contains(string(), 'ABBYY')]//td[1]//input[@data-value='true']";
	}
}
