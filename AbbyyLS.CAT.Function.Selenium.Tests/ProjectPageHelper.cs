using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

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
		public ProjectPageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
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
			return WaitUntilDisplayElement(By.XPath(PROGRESS_DIALOG_USERLIST_XPATH));
		}

		/// <summary>
		/// Кликнуть по пользователю в списке
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public void ClickAssignUserListUser(string userName)
		{
			string xPath = PROGRESS_DIALOG_USER_ITEM_LIST_XPATH + "[contains(@title,'" + userName + "')]";
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
			IList<IWebElement> acceptList = GetElementList(By.XPath(ACCEPT_BTN_XPATH));
			foreach (IWebElement el in acceptList)
			{
				el.Click();
			}
		}

		/// <summary>
		/// Вернуть, есть ли такой документ
		/// </summary>
		/// <param name="documentNumber">номер документа</param>
		/// <returns>есть</returns>
		public bool GetIsExistDocument(int documentNumber)
		{
			return GetIsElementExist(By.XPath(DOCUMENT_LIST_XPATH + "//tr[" + documentNumber + "]"));
		}

		/// <summary>
		/// Выделить документ
		/// </summary>
		/// <param name="documentNumber">номер документа</param>
		public bool SelectDocument(int documentNumber)
		{
			bool isDocumentExist = GetIsExistDocument(documentNumber);
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
			bool isDocumentExist = GetIsExistDocument(documentNumber);
			if (isDocumentExist)
			{
				ClickElement(By.XPath(DOCUMENT_ROW_XPATH
					+ "[" + documentNumber + "]"
					+ DOCUMENT_ROW_EDITOR_LINK_XPATH));
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
		/// Дождаться появления таблицы ТМ в диалоге импорта
		/// </summary>
		/// <returns></returns>
		public bool WaitImportTMTableDisplay()
		{
			return WaitUntilDisplayElement(By.XPath(IMPORT_TM_TABLE_XPATH));
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
			bool isDisappeared = true;
			if (GetIsElementDisplay(By.XPath(DOWNLOAD_DOC_IMG_XPATH)))
			{
				isDisappeared = false;
				for (int i = 0; i < 5; ++i)
				{
					isDisappeared = WaitUntilDisappearElement(By.XPath(DOWNLOAD_DOC_IMG_XPATH), 40);
					if (isDisappeared)
					{
						break;
					}
					else
					{
						Driver.Navigate().Refresh();
					}
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

			bool isExistError = GetIsElementDisplay(By.XPath(NO_FILE_ERROR_XPATH));
			SetDriverTimeoutDefault();

			return isExistError;
		}

		/// <summary>
		/// Кликнуть Delete
		/// </summary>
		public void ClickDeleteBtn()
		{
			ClickElement(By.XPath(DELETE_BTN_XPATH));
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
			IList<IWebElement> glossaryList = GetElementList(By.XPath(GLOSSARY_LIST_XPATH + "//td[2]"));
			for (int i = 0; i < glossaryList.Count; ++i)
			{
				if (glossaryList[i].Text.Contains(nameGlossary))
				{
					// Включаем требуемый глоссарий
					ClickElement(By.XPath(GLOSSARY_LIST_XPATH + "[" + (i + 1).ToString() + "]//td[1]"));
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
		/// Возвращает список задач Workflow в настройках проекта
		/// </summary>
		/// <returns>Список строк</returns>
		public List<string> GetWFTaskListProjectSettings()
		{
			List<string> wfTaskList = new List<string>();

			// Выборка задач workflow
			string xPath = PROJECT_SETTINGS_WF_TABLE_XPATH + "//tr//td[2]//span//span";
			IList<IWebElement> wfTaskIList = GetElementList(By.XPath(xPath));

			if (wfTaskIList.Count > 0)
			{
				foreach (IWebElement item in wfTaskIList)
				{
					wfTaskList.Add(item.Text);
				}
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
			List<string> wfTaskList = new List<string>();

			// Выбор задачи workflow
			string workflowXPath = PROJECT_SETTINGS_WF_TABLE_XPATH + "//tr[" + taskNumber.ToString() + "]//td[2]//span//span";

			// Получение выпадающего списка
			ClickElement(By.XPath(workflowXPath));
			IList<IWebElement> wfDropdownIList = GetElementList(By.XPath(PROJECT_SETTINGS_WF_DROPDOWNLIST_XPATH));

			// Выбираем заданный
			foreach (IWebElement wfTaskType in wfDropdownIList)
			{
				if (wfTaskType.Text == taskType)
				{
					wfTaskType.Click();
					break;
				}
			}
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
		}

		/// <summary>
		/// Кликаем отмену настроек проекта
		/// </summary>
		public void ClickProjectSettingsCancel()
		{
			ClickElement(By.XPath(PROJECT_SETTINGS_CANCEL_BTN));
		}

		/// <summary>
		/// Кликаем кнопку удаления задачи в настройках проекта
		/// </summary>
		public void ClickProjectSettingsWFDeleteTask(int taskNumber)
		{
			ClickElement(By.XPath("//div[contains(@class,'js-popup-edit')]//tr[" + taskNumber.ToString() + "]" + PROJECT_SETTINGS_WF_DELETE_TASK_BTN));
		}

		/// <summary>
		/// Возвращает отображаемый номер для заданной задачи
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		/// <returns>Отображаемый номер задачи</returns>
		public int GetProjectSettingsWFVisibleTaskNumber(int taskNumber)
		{
			// Выбор задачи workflow
			string workflowXPath = PROJECT_SETTINGS_WF_TABLE_XPATH + "//tr[" + taskNumber.ToString() + "]//td[1]";

			// Получение номера
			return Int32.Parse(GetTextElement(By.XPath(workflowXPath)));
		}

		/// <summary>
		/// Нажать на прогресс в поле документа
		/// </summary>
		public void ClickDocumentProgress()
		{
			ClickElement(By.XPath(DOCUMENT_PROGRESS_XPATH));
		}

		/// <summary>
		/// Открыть свертку документа
		/// </summary>
		/// <param name="documentNumber">номер документа</param>
		/// <returns>такой документ есть</returns>
		public bool OpenDocumentInfo(int documentNumber)
		{
			// Кликнуть на открытие информации о документе
			string documentXPath = DOCUMENT_ROW_XPATH + "[" + documentNumber + "]//" + OPEN_CLOSE_TD_XPATH;
			bool isExistDocument = GetIsElementExist(By.XPath(documentXPath));
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
			string XPath = DOCUMENT_ROW_XPATH + "[" + documentNumber + "]//" + TASK_NAME_XPATH;

			return GetTextElement(By.XPath(XPath)).Trim();
		}



		protected const string PROJECT_TABLE_XPATH = "//table[contains(@class,'l-project-panel-tbl')]";
		protected const string PROGRESS_BTN_XPATH = "//span[contains(@class,'js-document-progress')]";
		protected const string PROGRESS_DIALOG_XPATH = "//div[contains(@class,'js-popup-progress')][2]";
		protected const string PROGRESS_DIALOG_TABLE_USERNAME_XPATH =
			".//table[contains(@class,'js-progress-table')]//tr[1]//td[3]//span";
		protected const string PROGRESS_DIALOG_USERLIST_XPATH = "//span[contains(@class,'js-dropdown__list')]";
		protected const string PROGRESS_DIALOG_USER_ITEM_LIST_XPATH = "//span[contains(@class,'js-dropdown__item')]";
		protected const string PROGRESS_DIALOG_ASSIGN_SPAN_XPATH = "//td[contains(@class,'js-status')]//span[contains(@class,'js-assign')]";
		protected const string PROGRESS_DIALOG_ASSIGN_BTN_XPATH = PROGRESS_DIALOG_ASSIGN_SPAN_XPATH + "//a";
		protected const string PROGRESS_DIALOG_CANCEL_BTN_XPATH = "//span[contains(@class,'js-assigned-cancel')]";
		protected const string PROGRESS_DIALOG_CLOSE_BTN_XPATH = "//div[contains(@class,'js-popup-progress')][2]//span[contains(@class,'js-popup-close')]/span[1]";

		protected const string ACCEPT_BTN_XPATH = ".//span[contains(@class,'js-accept')]";

		protected const string DOCUMENT_LIST_XPATH = ".//table[contains(@class,'js-documents-table')]//tbody";
		protected const string DOCUMENT_ROW_XPATH = ".//tr[contains(@class,'js-document-row')]";
		protected const string DOCUMENT_ROW_EDITOR_LINK_XPATH = "//td[2]//p//a[contains(@class,'js-name l-project__doc-link')]";

		protected const string IMPORT_DOCUMENT_BTN_XPATH = ".//span[contains(@class,'js-document-import')]";
		protected const string IMPORT_DIALOG_XPATH = ".//div[contains(@class,'js-popup-import-document')][2]";
		protected const string IMPORT_ADD_BTN_XPATH = IMPORT_DIALOG_XPATH + "//a[contains(@class,'js-add-file')]";
		protected const string IMPORT_NEXT_BTN_XPATH = IMPORT_DIALOG_XPATH + "//span[contains(@class,'js-next')]";
		protected const string IMPORT_TM_TABLE_XPATH = IMPORT_DIALOG_XPATH + "//table[contains(@class,'js-tms-table')]";
		protected const string IMPORT_FINISH_BTN_XPATH = IMPORT_DIALOG_XPATH + "//span[contains(@class,'js-finish js-upload-btn')]"; // TODO проверить, в оригинале:  ".//div[contains(@class,'js-step last active')]
		protected const string DOWNLOAD_DOC_IMG_XPATH = "//img[contains(@title,'Processing translation document')]";
		protected const string NO_FILE_ERROR_XPATH = IMPORT_DIALOG_XPATH + "//p[contains(@class,'js-error-files-required')]";

		protected const string DOWNLOAD_LOGS_BTN_XPATH = "//span[contains(@class,'js-document-export-logs')]";

		protected const string EDIT_TM_BTN_XPATH = "//div[contains(@class,'l-project-panel')]//span[contains(@class,'js-tm-edit')]";
		protected const string EDIT_TM_DIALOG_XPATH = "//div[contains(@class,'js-popup-tm')][2]";
		protected const string EDIT_TM_CREATE_BTN_XPATH = EDIT_TM_DIALOG_XPATH + "//span[contains(@class,'js-tm-create')]";
		protected const string EDIT_TM_SAVE_BTN_XPATH = EDIT_TM_DIALOG_XPATH + "//span[contains(@class,'js-submit-btn')]";

		protected const string IMPORT_DOCUMENT_ERROR_XPATH = "//div[contains(@class,'js-info-popup')]";
		protected const string DELETE_BTN_XPATH = "//span[contains(@class,'js-document-delete')]";

		protected const string CONFIRM_DIALOG_XPATH = "//div[contains(@class,'js-popup-confirm')]";
		protected const string CONFIRM_YES_XPATH = CONFIRM_DIALOG_XPATH + "//input[contains(@class,'js-submit-btn')]";

		protected const string GLOSSARY_LIST_XPATH = "//table[contains(@class,'js-glossaries-table')]//tbody//tr";
		protected const string EDIT_GLOSSARY_SAVE_BTN_XPATH = "//span[contains(@class,'js-glossaries-save')]";

		protected const string PROJECT_SETTINGS_BTN_XPATH = "//span[contains(@class,'js-project-edit')]";
		protected const string PROJECT_SETTINGS_WORKFLOW_XPATH = "//a[contains(@class,'js-workflow-tab')]";

		protected const string PROJECT_SETTINGS_WF_TABLE_XPATH = "//table[contains(@class,'js-workflow-table')]//tbody";
		protected const string PROJECT_SETTINGS_WF_DROPDOWNLIST_XPATH = "//span[contains(@class,'js-dropdown__item')]";
		protected const string PROJECT_SETTINGS_WF_NEW_TASK_BTN = "//span[contains(@class,'js-new-stage')]";
		protected const string PROJECT_SETTINGS_WF_DELETE_TASK_BTN = "//a[contains(@class,'js-delete-workflow')]";

		protected const string PROJECT_SETTINGS_CANCEL_BTN = "//div[contains(@class,'js-popup-edit')]//a[contains(@class,'js-popup-close')]";
		protected const string PROJECT_SETTINGS_SAVE_BTN = "//div[contains(@class,'js-popup-edit')]//span[contains(@class,'js-save')]";

		protected const string DOCUMENT_PROGRESS_XPATH = "//td//a[contains(@class,'js-progress-link')]";
		protected const string DOCUMENT_ASSIGN_RESPONSIBLES_BTN_XPATH = "//span[contains(@class,'js-assign-btn')]";
		protected const string OPEN_CLOSE_TD_XPATH = "td[contains(@class,'openCloseCell')]";
		protected const string TASK_NAME_XPATH = OPEN_CLOSE_TD_XPATH + "//div[contains(@class,'js-text-overflow')]";
		protected const string UPLOAD_DOCUMENT_BTN_XPATH = ".//span[contains(@class,'js-import-btn ')]";
	}
}
