using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	
	/// <summary>
	/// Хелпер страницы проекта
	/// </summary>
	public class ProjectPageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public ProjectPageHelper(IWebDriver driver, WebDriverWait wait) 
			: base(driver, wait)
		{
		}

		/// <summary>
		/// Дождаться загрузки проекта
		/// </summary>
		/// <returns></returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(PROJECT_TABLE_XPATH));
		}

		/// <summary>
		/// Нажать на кнопку Progress
		/// </summary>
		public void ClickProgressBtn()
		{
			ClickElement(By.XPath(PROGRESS_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться, пока появится диалог Progress
		/// </summary>
		/// <returns>открылся</returns>
		public bool WaitProgressDialogOpen()
		{
			return WaitUntilDisplayElement(By.XPath(PROGRESS_DIALOG_XPATH));
		}

		/// <summary>
		/// Кликнуть по Assign Responsibles документа
		/// </summary>
		public void ClickAssignRessponsibleBtn()
		{
			ClickElement(By.XPath(DOCUMENT_ASSIGN_RESPONSIBLES_BTN_XPATH));
		}

		/// <summary>
		/// Проверить, что кнопки Assign Tasks нет на стр проектов
		/// </summary>
		public bool GetIsAssignRessponsibleBtnExist()
		{
			return GetIsElementExist(By.XPath(DOCUMENT_ASSIGN_RESPONSIBLES_BTN_XPATH));
		}

		/// <summary>
		/// Проверить, что панель с кнопками (Assign Tasks, Delete, Download, Settings) отображается
		/// </summary>
		/// <returns></returns>
		public bool GetIsPanelDisplay()
		{
			return this.GetIsElementExist(By.XPath(PANEL_BTNS_XPATH));
		}

		/// <summary>
		/// Кликнуть по ячейке с пользователем в диалоге
		/// </summary>
		public void ClickUserNameCell()
		{
			ClickElement(By.XPath(PROGRESS_DIALOG_TABLE_USERNAME_XPATH));
		}

		/// <summary>
		/// Дождаться появления списка пользователей
		/// </summary>
		/// <returns>появился</returns>
		public bool WaitAssignUserList()
		{
			return WaitUntilDisplayElement(By.XPath(PROGRESS_DIALOG_TABLE_USERNAME_XPATH));
		}

		/// <summary>
		/// Кликнуть по пользователю в списке
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public void ClickAssignUserListUser(string userName)
		{
			var name = userName.Replace(" ","  ");
			var xPath = PROGRESS_DIALOG_TABLE_USERNAME_XPATH + "[text()='" + name + "']";
			ClickElement(By.XPath(xPath));
		}

		/// <summary>
		/// Кликнуть по Assign
		/// </summary>
		public void ClickAssignBtn()
		{
			ClickElement(By.XPath(PROGRESS_DIALOG_ASSIGN_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться появления кнопки Assign
		/// </summary>
		/// <returns></returns>
		public bool WaitAssignBtnDisplay()
		{
			return WaitUntilDisplayElement(By.XPath(PROGRESS_DIALOG_ASSIGN_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться появления кнопки Cancel
		/// </summary>
		/// <returns></returns>
		public bool WaitCancelAssignBtnDisplay()
		{
			return WaitUntilDisplayElement(By.XPath(PROGRESS_DIALOG_CANCEL_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Отмену назначения
		/// </summary>
		public void ClickCancelAssignBtn()
		{
			ClickElement(By.XPath(PROGRESS_DIALOG_CANCEL_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Close в диалоге
		/// </summary>
		public void CloseAssignDialogClick()
		{
			ClickElement(By.XPath(PROGRESS_DIALOG_CLOSE_BTN_XPATH));
		}

		/// <summary>
		/// Нажать все Accept
		/// </summary>
		public void ClickAllAcceptBtns()
		{
			// Нажать на Accept
			var acceptList = GetElementList(By.XPath(ACCEPT_BTN_XPATH));

			foreach (var el in acceptList)
			{
				el.Click();
			}
		}

		/// <summary>
		/// Нажать кнопку претранслейт
		/// </summary>
		public void ClickPretranslateBtn()
		{
			ClickElement(By.XPath(PRETRANSLATE_BTN_XPATH));
		}

		/// <summary>
		/// Нажать кнопку новое правило претранслейта
		/// </summary>
		public void ClickNewRuleBtn()
		{
			ClickElement(By.XPath(NEW_RULE_BTN_XPATH));
		}

		/// <summary>
		/// Нажать кнопку выбора источника претранслейта
		/// </summary>
		public void ClickSourcePretranslateBtn()
		{
			ClickElement(By.XPath(SOURCE_PRETRANSLATE_BTN_XPATH));
		}

		/// <summary>
		/// Выбрать TM как источник претранслейта
		/// </summary>
		/// <param name="tmName"> название ТМ</param>
		public void ClickTmForPretranslateBtn(string tmName)
		{
			string xpath = TM_PRETRANSLATE_BTN_XPATH + "and contains(@title, '" + tmName + "')]";

			ClickElement(By.XPath(xpath));
		}

		/// <summary>
		/// Выбрать ТМ источник по услочанию (ABBYY)
		/// </summary>
		public void ClickAbbyyMtForPretranslate()
		{
			ClickElement(By.XPath(ABBYY_MT_PRETRANSLATE_BTN_XPATH));
		}

		/// <summary>
		/// Нажать кнопку сохранить настройки претранслейта
		/// </summary>
		public void ClickSavePretranslateBtn()
		{
			ClickElement(By.XPath(SAVE_PRETRANSLATE_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли такой документ
		/// </summary>
		/// <param name="documentNumber">номер документа</param>
		/// <returns>есть</returns>
		public bool GetIsExistDocument(int documentNumber)
		{
			return WaitUntilDisplayElement(
				By.XPath(DOCUMENT_LIST_XPATH + "//tr[" + documentNumber + "]"),
				maxWait: 5);
		}

		/// <summary>
		/// Выделить документ
		/// </summary>
		/// <param name="documentNumber">номер документа</param>
		public bool SelectDocument(int documentNumber)
		{
			var isDocumentExist = GetIsExistDocument(documentNumber);

			if (isDocumentExist)
			{
				// Нажать галочку у документа
				ClickElement(By.XPath(DOCUMENT_LIST_XPATH
					+ "//tr[" + documentNumber + "]//td[contains(@class,'checkbox')]//input"));
			}

			return isDocumentExist;
		}

		/// <summary>
		/// Открыть документ
		/// </summary>
		/// <param name="documentNumber">номер документа</param>
		public bool OpenDocument(int documentNumber)
		{
			var isDocumentExist = GetIsExistDocument(documentNumber);

			if (isDocumentExist)
			{
				ClickElement(By.XPath(DOCUMENT_ROW_XPATH
					+ "[" + documentNumber + "]"
					+ DOCUMENT_ROW_EDITOR_LINK_XPATH));
				Driver.SwitchTo().Window(Driver.CurrentWindowHandle).Close();
				Driver.SwitchTo().Window(Driver.WindowHandles.Last());
			}

			return isDocumentExist;
		}

		/// <summary>
		/// Кликнуть Import
		/// </summary>
		public void ClickImportBtn()
		{
			ClickElement(By.XPath(IMPORT_DOCUMENT_BTN_XPATH));
			// TODO проверить, в оригинале:
			//ждем когда окно с настройками загрузится
			//WaitUntilDisplayElement(".//span[contains(@class,'js-document-import')]");
			// Кликнуть по Импорт
			//Driver.FindElement(By.XPath(".//span[contains(@class,'js-document-import')]")).Click();
		}

		/// <summary>
		/// Дождаться открытия диалога импорта
		/// </summary>
		/// <returns></returns>
		public bool WaitImportDialogDisplay()
		{
			return WaitUntilDisplayElement(By.XPath(IMPORT_DIALOG_XPATH));
		}

		/// <summary>
		/// Кликнуть Add в диалоге Импорт
		/// </summary>
		public void ClickAddDocumentInImport()
		{
			ClickElement(By.XPath(IMPORT_ADD_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Next в диалоге импорта
		/// </summary>
		public void ClickNextImportDialog()
		{
			ClickElement(By.XPath(IMPORT_NEXT_BTN_XPATH));
		}

		/// <summary>
		/// Проверить , что открыть Pretranslate step в окне создания проекта
		/// </summary>
		public bool GetPretranslateTitleDisplay()
		{
			return GetIsElementDisplay(By.XPath(PRETRANSLATE_TITLE_IN_DIALOG_XPATH));
		}
		/// <summary>
		/// Кликнуть Next кнопка в диалоге импорта
		/// </summary>
		public void ClickNextBtn()
		{
			ClickElement(By.XPath(IMPORT_NEXT_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться появления таблицы ТМ в диалоге импорта
		/// </summary>
		/// <returns></returns>
		public bool WaitImportTMTableDisplay()
		{
			return WaitUntilDisplayElement(By.XPath(IMPORT_TM_TABLE_XPATH));
		}

		/// <summary>
		/// Проверить отображается ли таблица Assignee в диалоге импорта документа (на странице проект)
		/// </summary>
		/// <returns></returns>
		public bool GetAssigneeTableDisplay()
		{
			return GetIsElementDisplay(By.XPath(ASSIGNEE_TABLE));
		}
		
		/// <summary>
		/// Кликнуть finish
		/// </summary>
		public void ClickFinishImportDialog()
		{
			ClickElement(By.XPath(IMPORT_FINISH_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть по кнопке Выгрузка логов
		/// </summary>
		public void ClickDownloadLogs()
		{
			ClickElement(By.XPath(DOWNLOAD_LOGS_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться загрузки
		/// </summary>
		/// <returns>загрузился документ</returns>
		public bool WaitDocumentDownloadFinish()
		{
			var isDisappeared = true;

			if (GetIsElementDisplay(By.XPath(DOWNLOAD_DOC_IMG_XPATH)))
			{
				isDisappeared = false;
				for (var i = 0; i < 5; ++i)
				{
					isDisappeared = WaitUntilDisappearElement(By.XPath(DOWNLOAD_DOC_IMG_XPATH), 40);
					
					if (isDisappeared)
					{
						break;
					}

					Driver.Navigate().Refresh();
				}
			}
			return isDisappeared;
		}

		/// <summary>
		/// Кликнуть кнопку Edit TM
		/// </summary>
		public void ClickEditTMBtn()
		{
			ClickElement(By.XPath(EDIT_TM_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться появления диалога изменения ТМ
		/// </summary>
		/// <returns>появился</returns>
		public bool WaitEditTMDialogAppear()
		{
			return WaitUntilDisplayElement(By.XPath(EDIT_TM_DIALOG_XPATH));
		}

		/// <summary>
		/// Дождаться, пока диалога изменения ТМ пропадет
		/// </summary>
		/// <returns>пропал</returns>
		public bool WaitUntilEditTMDialogDisappear()
		{
			return WaitUntilDisappearElement(By.XPath(EDIT_TM_DIALOG_XPATH));
		}

		/// <summary>
		/// Нажать Create в диалоге редактирования TM
		/// </summary>
		public void ClickCreateTMBtn()
		{
			ClickElement(By.XPath(EDIT_TM_CREATE_BTN_XPATH));
		}

		/// <summary>
		/// Нажать Save в диалоге редактирования TM
		/// </summary>
		public void ClickSaveTMBtn()
		{
			ClickElement(By.XPath(EDIT_TM_SAVE_BTN_XPATH));
		}

		/// <summary>
		/// Заполнить название ТМ в диалоге
		/// </summary>
		/// <param name="tmName">название</param>
		public void FillTmNameDialog(string tmName)
		{
			ClearAndAddText(By.XPath(EDIT_TM_NAME_INPUT), tmName);
		}

		/// <summary>
		/// Нажать Загрузить файл в диалоге редактирования TM
		/// </summary>
		public void ClickUploadTMBtn()
		{
			ClickElement(By.XPath(EDIT_TM_UPLOAD));
		}

		/// <summary>
		/// Нажать Импорт в диалоге редактирования TM
		/// </summary>
		public void ClickSaveAndImportTMBtn()
		{
			ClickElement(By.XPath(EDIT_TM_SAVE_AND_IMPORT_BTN_XPATH));
		}

		/// <summary>
		/// Нажать Подтверждение импортирования в диалоге редактирования TM
		/// </summary>
		public void ClickConfirmImportBtn()
		{
			ClickElement(By.XPath(EDIT_TM_IMPORT_BTN));
		}

		/// <summary>
		/// Дождаться появления сообщение об ошибке при импорте документа
		/// </summary>
		/// <returns>появилось</returns>
		public bool WaitImportDocumentErrorMessage()
		{
			// TODO плохой id
			return WaitUntilDisplayElement(By.XPath(IMPORT_DOCUMENT_ERROR_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли ошибка - не указан файл
		/// </summary>
		/// <returns>есть ошибка</returns>
		public bool GetIsExistNoFileError()
		{
			SetDriverTimeoutMinimum();
			var isExistError = GetIsElementDisplay(By.XPath(NO_FILE_ERROR_XPATH));
			SetDriverTimeoutDefault();

			return isExistError;
		}

		/// <summary>
		/// Кликнуть Delete
		/// </summary>
		public void ClickDeleteBtn()
		{
			ClickElement(By.XPath(DELETE_BTN_XPATH));
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Подтвердить
		/// </summary>
		public void ConfirmClickYes()
		{
			if (WaitUntilDisplayElement(By.XPath(CONFIRM_DIALOG_XPATH)))
			{
				ClickElement(By.XPath(CONFIRM_YES_XPATH));
				WaitUntilDisappearElement(By.XPath(CONFIRM_DIALOG_XPATH));
			}
		}

		/// <summary>
		/// Вернуть: текущий статус not Assign?
		/// </summary>
		/// <returns>да</returns>
		public bool GetIsAssignStatusNotAssigned()
		{
			return GetElementClass(By.XPath(PROGRESS_DIALOG_ASSIGN_SPAN_XPATH)).Contains("notAssigned");
		}

		/// <summary>
		/// Находит глоссарий по имени и выбирает его
		/// </summary>
		/// <param name="nameGlossary"></param>
		/// <returns>Найдена переменная или нет</returns>
		public void SetGlossaryByName(string nameGlossary)
		{
			// Выборка имен глоссариев
			var glossaryList = GetElementList(By.XPath(GLOSSARY_LIST_XPATH + "//td[2]"));
			for (var i = 0; i < glossaryList.Count; ++i)
			{
				if (glossaryList[i].Text.Contains(nameGlossary))
				{
					// Включаем требуемый глоссарий
					ClickElement(By.XPath(GLOSSARY_LIST_XPATH + "[" + (i + 1) + "]//td[1]//input"));
					Thread.Sleep(1000);
					ClickElement(By.XPath(EDIT_GLOSSARY_SAVE_BTN_XPATH));
					Thread.Sleep(1000);
				}
			}
		}

		/// <summary>
		/// Кликаем настройки проекта
		/// </summary>
		public void ClickProjectSettings()
		{
			ClickElement(By.XPath(PROJECT_SETTINGS_BTN_XPATH));
		}

		/// <summary>
		/// Кликаем настройки проекта
		/// </summary>
		public void ClickProjectSettingsWorkflow()
		{
			ClickElement(By.XPath(PROJECT_SETTINGS_WORKFLOW_XPATH));
		}

		/// <summary>
		/// Проверить отображеются ли Workflow в настройках проекта 
		/// </summary>
		public bool GetSettingsWorkflowDisplay()
		{
			return GetIsElementDisplay(By.XPath(PROJECT_SETTINGS_WORKFLOW_XPATH));
		}

		/// <summary>
		/// Возвращает список задач Workflow в настройках проекта
		/// </summary>
		/// <returns>Список строк</returns>
		public List<string> GetWFTaskListProjectSettings()
		{
			var wfTaskList = new List<string>();

			// Выборка задач workflow
			var xPath = PROJECT_SETTINGS_WF_TABLE_XPATH + "//tr//td[2]//span//span";
			var wfTaskIList = GetElementList(By.XPath(xPath));

			if (wfTaskIList.Count > 0)
			{
				wfTaskList.AddRange(wfTaskIList.Select(item => item.Text));
			}
			return wfTaskList;
		}

		/// <summary>
		/// Задает тип задачи Workflow в настройках проекта
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		/// <param name="taskType">Тип задачи из выпадающего списка</param>
		public void SetWFTaskListProjectSettings(int taskNumber, string taskType)
		{
			// Выбор задачи workflow
			var workflowXPath = PROJECT_SETTINGS_WF_TABLE_XPATH + "//tr[" + taskNumber.ToString() + "]//td[2]//span//span";

			// Получение выпадающего списка
			ClickElement(By.XPath(workflowXPath));
			var wfDropdownIList = GetElementList(By.XPath(PROJECT_SETTINGS_WF_DROPDOWNLIST_XPATH));

			// Выбираем заданный
			foreach (var wfTaskType in wfDropdownIList)
			{
				if (wfTaskType.Text == taskType)
				{
					wfTaskType.Click();
					break;
				}
			}
			
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Кликаем добавить новую задачу в настройках проекта
		/// </summary>
		public void ClickProjectSettingsWorkflowNewTask()
		{
			ClickElement(By.XPath(PROJECT_SETTINGS_WF_NEW_TASK_BTN));
		}

		/// <summary>
		/// Кликаем сохранение настроек проекта
		/// </summary>
		public void ClickProjectSettingsSave()
		{
			ClickElement(By.XPath(PROJECT_SETTINGS_SAVE_BTN));
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Кликаем отмену настроек проекта
		/// </summary>
		public void ClickProjectSettingsCancel()
		{
			ClickElement(By.XPath(PROJECT_SETTINGS_CANCEL_BTN));
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Кликаем кнопку удаления задачи в настройках проекта
		/// </summary>
		public void ClickProjectSettingsWFDeleteTask(int taskNumber)
		{
			ClickElement(By.XPath(SETTING_POPUP + "//tr[" + taskNumber + "]" + PROJECT_SETTINGS_WF_DELETE_TASK_BTN));
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Возвращает отображаемый номер для заданной задачи
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		/// <returns>Отображаемый номер задачи</returns>
		public int GetProjectSettingsWFVisibleTaskNumber(int taskNumber)
		{
			// Выбор задачи workflow
			var workflowXPath = PROJECT_SETTINGS_WF_TABLE_XPATH + "//tr[" + taskNumber + "]//td[1]";

			// Получение номера
			return Int32.Parse(GetTextElement(By.XPath(workflowXPath)));
		}

		/// <summary>
		/// Нажать на прогресс в поле документа
		/// </summary>
		public void ClickDocumentProgress()
		{
			ClickElement(By.XPath(DOCUMENT_PROGRESS_XPATH));
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Открыть свертку документа
		/// </summary>
		/// <param name="documentNumber">номер документа</param>
		/// <returns>такой документ есть</returns>
		public bool OpenDocumentInfo(int documentNumber)
		{
			// Кликнуть на открытие информации о документе
			var documentXPath = DOCUMENT_ROW_XPATH + "[" + documentNumber + "]//" + OPEN_CLOSE_TD_XPATH;
			var isExistDocument = GetIsElementExist(By.XPath(documentXPath));
			if (isExistDocument)
			{
				ClickElement(By.XPath(documentXPath));
			}

			return isExistDocument;
		}

		/// <summary>
		/// Возвращает задачу по номеру документа
		/// </summary>
		/// <param name="documentNumber">Номер документа</param>
		/// <returns>Задача</returns>
		public string GetDocumentTask(int documentNumber)
		{
			// Кликнуть на открытие информации о документе
			var XPath = DOCUMENT_ROW_XPATH + "[" + documentNumber + "]//" + TASK_NAME_XPATH;

			return GetTextElement(By.XPath(XPath)).Trim();
		}

		public bool ClickRadioBtm()
		{
			
			DoubleClickElement(By.XPath(RADIO_BTN));
			return GetIsInputChecked(By.XPath(RADIO_BTN));
		}

		/// <summary>
		/// Загрузка документа на странице проекта
		/// </summary>
		/// <param name="fileName"> название документа </param>
		public void UploadFileOnProjectPage(string fileName)
		{
			UploadDocument(fileName, ADD_FILE_ON_PROJECT_PAGE);
		}

		public void UploadFileNativeAction(string fileName)
		{
			UploadDocNativeAction(fileName);
		}

		protected const string ADD_FILE_ON_PROJECT_PAGE = "//div[contains(@class,\"js-popup-import-document\")][2]//input[@type=\"file\"]"; // добавление документа уже сущестующему проекту на стр проекта

		protected const string PROJECT_TABLE_XPATH = "//table[contains(@class,'l-project-panel-tbl')]";
		protected const string PROGRESS_BTN_XPATH = "//div[@class='ui-progressbar__container']";
		protected const string PANEL_BTNS_XPATH = "//div[@class='l-corpr__btnterm__left js-buttons-left']";
		protected const string PROGRESS_DIALOG_XPATH = "//div[contains(@style,'display: block')]//div[contains(@class,'g-popupbox l-editgloss l-progress')]";
		protected const string PROGRESS_DIALOG_TABLE_USERNAME_XPATH =
			".//table[contains(@class,'js-progress-table')]//tr[1]//td[3]//select/option";

		protected const string PROGRESS_DIALOG_USERLIST_XPATH = "//span[contains(@class,'js-dropdown__list')]";
		protected const string PROGRESS_DIALOG_USER_ITEM_LIST_XPATH = "//span[contains(@class,'js-dropdown__item')]";
		protected const string PROGRESS_DIALOG_ASSIGN_SPAN_XPATH = "//div[@class='g-popup-bd js-popup-bd js-popup-assign'][2]//div[@class='g-popupbox__ft js-popup-buttons']//span";
		protected const string PROGRESS_DIALOG_ASSIGN_BTN_XPATH = "//div[@class='g-popup-bd js-popup-bd js-popup-assign'][2]//span/a[contains(@data-bind, 'assign')]";
		protected const string PROGRESS_DIALOG_CANCEL_BTN_XPATH = "//div[@class='g-popup-bd js-popup-bd js-popup-assign'][2]//span/a[@data-bind='click: cancel']";
		protected const string PROGRESS_DIALOG_CLOSE_BTN_XPATH = "//div[@class='g-popup-bd js-popup-bd js-popup-assign'][2]//div[@class='g-popupbox__ft js-popup-buttons']//span/a";

		protected const string ACCEPT_BTN_XPATH = ".//span[contains(@class,'js-accept')]";

		protected const string DOCUMENT_LIST_XPATH = ".//table[contains(@class,'js-documents-table')]//tbody";
		protected const string DOCUMENT_ROW_XPATH = ".//tr[contains(@class,'js-document-row')]";
		protected const string DOCUMENT_ROW_EDITOR_LINK_XPATH = "//td[2]//a[contains(@class,'js-name l-project__doc-link')]";

		protected const string IMPORT_DOCUMENT_BTN_XPATH = ".//span[contains(@class,'js-document-import')]";
		protected const string IMPORT_DIALOG_XPATH = ".//div[contains(@class,'js-popup-import-document')][2]";
		protected const string IMPORT_ADD_BTN_XPATH = IMPORT_DIALOG_XPATH + "//a[contains(@class,'js-add-file')]";
		protected const string IMPORT_NEXT_BTN_XPATH = IMPORT_DIALOG_XPATH + "//span[contains(@class,'js-next')]";
		protected const string IMPORT_TM_TABLE_XPATH = IMPORT_DIALOG_XPATH + "//table[contains(@class,'js-tms-table')]";
		protected const string IMPORT_FINISH_BTN_XPATH = IMPORT_DIALOG_XPATH + "//span[contains(@class,'js-finish js-upload-btn')]"; // TODO проверить, в оригинале:  ".//div[contains(@class,'js-step last active')]
		protected const string DOWNLOAD_DOC_IMG_XPATH = "//img[contains(@title,'Processing translation document')]";
		protected const string NO_FILE_ERROR_XPATH = IMPORT_DIALOG_XPATH + "//p[contains(@class,'js-error-files-required')]";

		protected const string DOWNLOAD_LOGS_BTN_XPATH = "//span[contains(@class,'js-document-export-logs')]";

		protected const string EDIT_TM_BTN_XPATH = "/html/body/div[6]/div[1]/div[2]/div[3]/div/form/div[5]/span[2]/span[2]/a";
		protected const string EDIT_TM_DIALOG_XPATH = "//div[contains(@class,'js-popup-tm')][2]";
		protected const string EDIT_TM_CREATE_BTN_XPATH = EDIT_TM_DIALOG_XPATH + "//span[contains(@class,'js-tm-create')]";
		protected const string EDIT_TM_SAVE_BTN_XPATH = EDIT_TM_DIALOG_XPATH + "//span[contains(@class,'js-submit-btn')]";
		protected const string EDIT_TM_CREATE_TM_XPATH = "//div[contains(@class,'js-popup-create-tm')][2]";
		protected const string EDIT_TM_NAME_INPUT = EDIT_TM_CREATE_TM_XPATH + "//input[contains(@class,'js-tm-name')]";
		protected const string EDIT_TM_SAVE_AND_IMPORT_BTN_XPATH = EDIT_TM_CREATE_TM_XPATH + "//a[contains(@class,'js-save-and-import')]";
		protected const string EDIT_TM_IMPORT_DIALOG_XPATH = "//div[contains(@class,'js-popup-import')][2]";
		protected const string EDIT_TM_UPLOAD = EDIT_TM_IMPORT_DIALOG_XPATH + "//a[contains(@class,'js-upload-btn')]";
		protected const string EDIT_TM_IMPORT_BTN = EDIT_TM_IMPORT_DIALOG_XPATH + "//input[contains(@value, 'Import')]";

		protected const string IMPORT_DOCUMENT_ERROR_XPATH = "//div[contains(@class,'js-info-popup')]";
		protected const string DELETE_BTN_XPATH = "//span[contains(@class,'js-document-delete')]";

		protected const string CONFIRM_DIALOG_XPATH = "//div[contains(@class,'js-popup-confirm')]";
		protected const string CONFIRM_YES_XPATH = CONFIRM_DIALOG_XPATH + "//input[contains(@class,'js-submit-btn')]";

		protected const string GLOSSARY_LIST_XPATH = "//div[@class='g-page']//table//tbody[@data-bind='foreach: glossaries']//tr";
		protected const string EDIT_GLOSSARY_SAVE_BTN_XPATH = "//span[contains(@data-bind,'click: saveGlossaries')]";

		protected const string PROJECT_SETTINGS_BTN_XPATH = "//span[contains(@class,'js-project-edit')]";
		protected const string PROJECT_SETTINGS_WORKFLOW_XPATH = SETTING_POPUP + "//a[contains(@data-bind, 'activeTab(workflowTab);')]";
		protected const string SETTING_POPUP = "//div[contains(@class,'js-popup-edit')][2]";
		protected const string PROJECT_SETTINGS_WF_TABLE_XPATH = "//table[contains(@class,'l-corpr__tbl')]//tbody[@data-bind='foreach: workflowStages']";
		protected const string PROJECT_SETTINGS_WF_DROPDOWNLIST_XPATH = "//span[contains(@class,'js-dropdown__item')]";
		protected const string PROJECT_SETTINGS_WF_NEW_TASK_BTN = "//div[@class='g-popup-bd js-popup-bd js-popup-edit'][2]//span[contains(@data-bind, 'addWorkflowStage')]";
		protected const string PROJECT_SETTINGS_WF_DELETE_TASK_BTN = "//a[@class='g-iblock g-corprAction']";

		protected const string PROJECT_SETTINGS_CANCEL_BTN = "//div[contains(@class,'js-popup-edit')]//a[contains(@class,'js-popup-close')]";
		protected const string PROJECT_SETTINGS_SAVE_BTN = "//div[@class='g-popup-bd js-popup-bd js-popup-edit'][2]//div[@class='g-popupbox__ft']//span//span/a";

		protected const string DOCUMENT_PROGRESS_XPATH = "//div[contains(@class,'ui-progressbar__container')]";
		protected const string DOCUMENT_ASSIGN_RESPONSIBLES_BTN_XPATH = "//span[contains(@class,'js-assign-btn')]";
		protected const string OPEN_CLOSE_TD_XPATH = "td[contains(@class,'openCloseCell')]";
		protected const string TASK_NAME_XPATH = OPEN_CLOSE_TD_XPATH + "//div[contains(@class,'js-text-overflow')]";
		protected const string UPLOAD_DOCUMENT_BTN_XPATH = ".//span[contains(@class,'js-import-btn ')]";
		protected const string PRETRANSLATE_TITLE_IN_DIALOG_XPATH = "//span[text()='Set Up Pretranslation']";

		protected const string PRETRANSLATE_BTN_XPATH = "//span[contains(@class,'js-project-pretranslate')]";
		protected const string NEW_RULE_BTN_XPATH = "//div[contains(@class,'pretranslate')][2]//span[contains(@class,'js-new-rule')]";
		protected const string SOURCE_PRETRANSLATE_BTN_XPATH = "//div[contains(@class,'pretranslate')]//span[contains(@class, 'js-dropdown')]";
		protected const string TM_PRETRANSLATE_BTN_XPATH = "//span[contains(@class, 'js-dropdown')";
		protected const string ABBYY_MT_PRETRANSLATE_BTN_XPATH = "//span[contains(@data-id, '2_f42671d9-df7e-4678-a846-d9143011cd2c')]";
		protected const string SAVE_PRETRANSLATE_BTN_XPATH = "//div[contains(@class,'pretranslate')][2]//span[contains(@class, 'js-save')]";
		protected const string RADIO_BTN = "//table[@class='l-corpr__tbl js-tms-popup-table']//td[@class='l-corpr__td l-project-td radio']//input[@type='radio']";
		protected const string ASSIGNEE_TABLE = ".//div[contains(@class,'js-popup-import-document')][2]//th[text()='Assignee']";
	}
}
