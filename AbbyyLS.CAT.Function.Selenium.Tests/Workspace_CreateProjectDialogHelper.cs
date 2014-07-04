using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class Workspace_CreateProjectDialogHelper : WorkSpacePageHelper
    {
        public Workspace_CreateProjectDialogHelper(IWebDriver driver, WebDriverWait wait) :
            base(driver, wait)
        {
            MTTypeDict.Add(MT_TYPE.DefaultMT, DEFAULT_MT_TYPE);
            MTTypeDict.Add(MT_TYPE.Google, GOOGLE_MT_TYPE);
            MTTypeDict.Add(MT_TYPE.Bing, BING_MT_TYPE);
            MTTypeDict.Add(MT_TYPE.Yandex, YANDEX_MT_TYPE);
            MTTypeDict.Add(MT_TYPE.Moses, MOSES_MT_TYPE);
        }

        /// <summary>
        /// Дождаться появления диалога создания проекта
        /// </summary>
        /// <returns></returns>
        public bool WaitDialogDisplay()
        {
            return WaitUntilDisplayElement(By.XPath(CREATE_PROJECT_DIALOG_XPATH));
        }

        /// <summary>
        /// Дождаться, пока диалог пропадет
        /// </summary>
        /// <returns>пропал</returns>
        public bool WaitDialogDisappear()
        {
            return WaitUntilDisappearElement(By.XPath(CREATE_PROJECT_DIALOG_XPATH));
        }

        /// <summary>
        /// Ввести deadline дату
        /// </summary>
        /// <param name="date">дата</param>
        public void FillDeadlineDate(string date)
        {
            ClearAndAddText(By.XPath(DEADLINE_DATE_INPUT_XPATH),
                            date);
        }

        /// <summary>
        /// Получить значение deadline
        /// </summary>
        /// <returns>deadline</returns>
        public string GetDeadlineValue()
        {
            return GetElementAttribute(By.XPath(DEADLINE_DATE_INPUT_XPATH), "value");
        }

        /// <summary>
        /// Выбрать source язык
        /// </summary>
        /// <param name="langType">язык</param>
        public void SelectSourceLanguage(LANGUAGE langType)
        {
            // Кликнуть, чтобы выпал список
            ClickElement(By.XPath(SOURCE_LANG_DROPDOWN_XPATH));
            // Кликнуть по языку
            ClickElement(By.XPath(SPAN_DROPDOWN_LIST_XPATH + LANG_ITEM_XPATH + GetLangByTypeWithBracket(langType)));
        }

        /// <summary>
        /// Кликнуть по Target (открыть/закрыть список)
        /// </summary>
        public void ClickTargetList()
        {
            ClickElement(By.XPath(TARGET_MULTISELECT_XPATH));
        }

        /// <summary>
        /// Кликнуть по элементу в списке Target (выбрать/снять выбор)
        /// </summary>
        /// <param name="langType">язык</param>
        public void ClickTargetItem(LANGUAGE langType)
        {
            ClickElement(By.XPath(TARGET_MULTISELECT_ITEM_XPATH + GetLangByTypeWithBracket(langType)));
        }

        /// <summary>
        /// Вернуть значение Target
        /// </summary>
        /// <returns>target</returns>
        public string GetTargetValue()
        {
            return GetTextElement(By.XPath(TARGET_LANG_VALUE_XPATH));
        }

        /// <summary>
        /// Ввести название проекта
        /// </summary>
        /// <param name="projectName"></param>
        public void FillProjectName(string projectName)
        {
            ClearAndAddText(By.XPath(PROJECT_NAME_INPUT_XPATH), projectName);
        }
        
        /// <summary>
        /// Вернуть, отображается ли первый шаг
        /// </summary>
        /// <returns></returns>
        public bool GetIsFirstStep()
        {
            return GetIsElementDisplay(By.XPath(PROJECT_NAME_INPUT_XPATH));
        }

        /// <summary>
        /// Вернуть введенное имя проекта
        /// </summary>
        /// <returns></returns>
        public string GetProjectInputName()
        {
            return GetElementAttribute(By.XPath(PROJECT_NAME_INPUT_XPATH), "value");
        }

        /// <summary>
        /// Кликнуть по Add для загрузки документа на первом шаге
        /// </summary>
        public void ClickAddDocumentBtn()
        {
            ClickElement(By.XPath(ADD_DOCUMENT_BTN_XPATH));
        }

        /// <summary>
        /// Кликнуть Next
        /// </summary>
        public void ClickNextStep()
        {
            ClickElement(By.XPath(NEXT_BTN_XPATH));
        }

        /// <summary>
        /// Кликнуть Создать TM
        /// </summary>
        public void ClickCreateTM()
        {
            ClickElement(By.XPath(CREATE_TM_BTN_XPATH));
        }

        /// <summary>
        /// Вернуть: шаг выбора ТМ?
        /// </summary>
        /// <returns>текущий шаг - шаг выбора ТМ</returns>
        public bool GetIsStepChooseTM()
        {
            return GetIsElementDisplay(By.XPath(CREATE_TM_BTN_XPATH));
        }

        /// <summary>
        /// Вернуть: шаг выбора глоссария?
        /// </summary>
        /// <returns>текущий шаг - шаг выбора глоссария</returns>
        public bool GetIsStepChooseGlossary()
        {
            return GetIsElementDisplay(By.XPath(FIRST_GLOSSARY_XPATH));
        }

        /// <summary>
        /// Дождаться, пока появится диалог создания ТМ
        /// </summary>
        /// <returns>появился</returns>
        public bool WaitCreateTMDialog()
        {
            return WaitUntilDisplayElement(By.XPath(CREATE_TM_DIALOG_XPATH));
        }

        /// <summary>
        /// Заполнить название ТМ
        /// </summary>
        /// <param name="TMName">название</param>
        public void FillTMName(string TMName)
        {
            ClearAndAddText(By.XPath(NEW_TM_NAME_INPUT_XPATH), TMName);
        }

        /// <summary>
        /// Кликнуть на загрузку TMX
        /// </summary>
        public void ClickAddTMX()
        {
            ClickElement(By.XPath(ADD_TMX_BTN_XPATH));
        }

        /// <summary>
        /// Дождаться появления диалога загрузки ТМХ
        /// </summary>
        /// <returns>появился</returns>
        public bool WaitUploadTMXDialog()
        {
            return WaitUntilDisplayElement(By.XPath(UPLOAG_TMX_DIALOG_XPATH));
        }

        /// <summary>
        /// Кликнуть Загрузить в диалоге
        /// </summary>
        public void ClickAddTMXUploadTMXDialog()
        {
            ClickElement(By.XPath(UPLOAD_TMX_BTN_XPATH));
        }

        /// <summary>
        /// Кликнуть Import
        /// </summary>
        public void ClickImportBtn()
        {
            ClickElement(By.XPath(IMPORT_TMX_BTN_XPATH));
        }

        /// <summary>
        /// Дождаться, пока диалог загрузки пропадет
        /// </summary>
        /// <returns>пропал</returns>
        public bool WaitImportDialogDisappear()
        {
            return WaitUntilDisappearElement(By.XPath(UPLOAG_TMX_DIALOG_XPATH));
        }

        /// <summary>
        /// Кликнуть Сохранить ТМ
        /// </summary>
        public void ClickSaveTM()
        {
            ClickElement(By.XPath(SAVE_TM_BTN_XPATH));
        }

        /// <summary>
        /// Дождаться, пока пропадет диалог создания ТМ
        /// </summary>
        /// <returns></returns>
        public bool WaitUntilCreateTMDialogDisappear()
        {
            return WaitUntilDisappearElement(By.XPath(CREATE_TM_DIALOG_XPATH));
        }

        /// <summary>
        /// Получить: не пустая ли таблица TM
        /// </summary>
        /// <returns>не пустая</returns>
        public bool GetIsTMTableNotEmpty()
        {
            bool isExist = WaitUntilDisplayElement(By.XPath(TM_TABLE_XPATH));

            if (isExist)
            {
                isExist = GetIsElementDisplay(By.XPath(TM_TABLE_FIRST_ITEM_XPATH));
            }
            return isExist;
        }

        /// <summary>
        /// Выбрать первую ТМ из списка
        /// </summary>
        public void ClickFirstTMInTable()
        {
            ClickElement(By.XPath(TM_TABLE_FIRST_ITEM_XPATH));
        }

        /// <summary>
        /// Выбрать первый глоссарий
        /// </summary>
        public void ClickFirstGlossaryInTable()
        {
            ClickElement(By.XPath(FIRST_GLOSSARY_XPATH));
        }

        /// <summary>
        /// Нажать галочку у ТМ
        /// </summary>
        /// <param name="mtType"></param>
        public void ChooseMT(MT_TYPE mtType)
        {
            ClickElement(By.XPath(GetMTXpath(mtType)));
        }

        /// <summary>
        /// Вернуть: текущий шаг - шаг выбора МТ?
        /// </summary>
        /// <returns>текущий шаг - выбор МТ</returns>
        public bool GetIsStepChooseMT()
        {
            return GetIsElementDisplay(By.XPath(MT_TABLE_XPATH));
        }

        /// <summary>
        /// Вернуть: отмечена ли МТ
        /// </summary>
        /// <param name="mtType">тип МТ</param>
        /// <returns>отмечена</returns>
        public bool GetIsMTChecked(MT_TYPE mtType)
        {
            return GetIsInputChecked(By.XPath(GetMTXpath(mtType)));
        }

        /// <summary>
        /// Нажать на Finish
        /// </summary>
        public void ClickFinishCreate()
        {
            ClickElement(By.XPath(FINISH_BTN_XPATH));
        }

        /// <summary>
        /// Кликнуть Back
        /// </summary>
        public void ClickBackBtn()
        {
            ClickElement(By.XPath(BACK_BTN_XPATH));
        }

        /// <summary>
        /// Вернуть, есть ли сообщение о неверном формате загружаемого документа
        /// </summary>
        /// <returns>есть</returns>
        public bool GetIsExistErrorFormatDocumentMessage()
        {
            return GetIsElementDisplay(By.XPath(ERROR_FORMAT_DOCUMENT_MESSAGE_XPATH));
        }

        /// <summary>
        /// Вернуть, отмечено ли поле Имя ошибкой
        /// </summary>
        /// <returns>отмечено ошибкой</returns>
        public bool GetIsNameInputError()
        {
            return GetElementClass(By.XPath(PROJECT_NAME_INPUT_XPATH)).Contains("error");
        }

        /// <summary>
        /// Вернуть, есть ли сообщение о существующем имени
        /// </summary>
        /// <returns>есть</returns>
        public bool GetIsExistErrorMessageNameExists()
        {
            return GetIsElementDisplay(By.XPath(ERROR_NAME_EXISTS_XPATH));
        }

        /// <summary>
        /// Вернуть, есть ли сообщение о пустом имени
        /// </summary>
        /// <returns>есть</returns>
        public bool GetIsExistErrorMessageNoName()
        {
            return GetIsElementDisplay(By.XPath(ERROR_NO_NAME_XPATH));
        }

        /// <summary>
        /// Вернуть, есть ли сообщение о запрещенных символах в имени
        /// </summary>
        /// <returns>есть</returns>
        public bool GetIsExistErrorForbiddenSymbols()
        {
            return GetIsElementDisplay(By.XPath(ERROR_FORBIDDEN_SYMBOLS_NAME));
        }

        /// <summary>
        /// Вернуть, есть ли ошибка 
        /// </summary>
        /// <returns></returns>
        public bool GetIsExistErrorDuplicateLanguage()
        {
            return GetIsElementDisplay(By.XPath(ERROR_DUPLICATE_LANG_XPATH));
        }

        /// <summary>
        /// Вернуть, есть ли сообщение об ошибке в загружаемом файле
        /// </summary>
        /// <returns>есть ошибка</returns>
        public bool GetIsExistErrorFileMessage()
        {
            SetDriverTimeoutMinimum();

            bool isExistError = GetIsElementDisplay(By.XPath(ERROR_FILE_TM_XPATH));
            SetDriverTimeoutDefault();

            return isExistError;
        }

        /// <summary>
        /// Кликнуть закрыть диалог
        /// </summary>
        public void ClickCloseDialog()
        {
            ClickElement(By.XPath(CLOSE_DIALOG_BTN_XPATH));
        }

        /// <summary>
        /// Вернуть, отмечен ли checkBox около TM
        /// </summary>
        /// <param name="TMNumber">номер ТМ</param>
        /// <returns>отмечен</returns>
        public bool GetIsTMChecked(int TMNumber)
        {
            return GetIsInputChecked(By.XPath(TM_TABLE_XPATH + "//tr[" + TMNumber + "]" + TM_TABLE_CHECK_TD));
        }

        /// <summary>
        /// Вернуть, отмечен ли radio около TM
        /// </summary>
        /// <param name="TMNumber">номер ТМ</param>
        /// <returns>отмечен</returns>
        public bool GetIsTMRadioChecked(int TMNumber)
        {
            return GetIsInputChecked(By.XPath(TM_TABLE_XPATH + "//tr[" + TMNumber + "]" + TM_TABLE_RADIO_TD));
        }

        /// <summary>
        /// Вернуть, отмечен ли checkbox первого глоссария
        /// </summary>
        /// <returns>отмечен</returns>
        public bool GetIsFirstGlossaryChecked()
        {
            return GetIsInputChecked(By.XPath(FIRST_GLOSSARY_XPATH + "//input"));
        }

        /// <summary>
        /// Открыть список stage
        /// </summary>
        public void OpenStageList()
        {
            ClickElement(By.XPath(STAGE_BTN_XPATH));
        }

        /// <summary>
        /// Вернуть, текущий шаг - выбор stage?
        /// </summary>
        /// <returns>текущий шаг - выбор stage</returns>
        public bool GetIsStepChooseStage()
        {
            return GetIsElementDisplay(By.XPath(STAGE_BTN_XPATH));
        }

        /// <summary>
        /// Выбрать Stage
        /// </summary>
        /// <param name="stageText">название</param>
        /// <returns>есть такой Stage</returns>
        public bool SelectStage(string stageText)
        {
            // TODO неудачный айдишник
            string xPath = STAGE_ITEM_XPATH + "[contains(@title,'" + stageText + "')]";
            bool isExistStage = GetIsElementDisplay(By.XPath(xPath));
            if (isExistStage)
            {
                ClickElement(By.XPath(xPath));
            }
            return isExistStage;
        }

        /// <summary>
        /// Вернуть текущее значение Stage
        /// </summary>
        /// <returns>stage</returns>
        public string GetCurrentStage()
        {
            return GetElementAttribute(By.XPath(STAGE_BTN_XPATH), "title");
        }

        /// <summary>
        /// Вернуть, есть ли ТМ
        /// </summary>
        /// <param name="TMName">название ТМ</param>
        /// <returns>есть</returns>
        public bool GetIsExistTM(string TMName)
        {
            return GetIsElementExist(By.XPath(TM_TABLE_TM_NAME_XPATH + "[text()='" + TMName + "']"));
        }

        /// <summary>
        /// Получить xpath языка с закрывающей скобочкой
        /// </summary>
        /// <param name="langType">тип языка</param>
        /// <returns>xpath</returns>
        protected string GetLangByTypeWithBracket(LANGUAGE langType)
        {
            return languageID[langType] + "']";
        }

        /// <summary>
        /// Получить xPath MT
        /// </summary>
        /// <param name="mtType">тип МТ</param>
        /// <returns>xPath</returns>
        protected string GetMTXpath(MT_TYPE mtType)
        {
            return MT_TABLE_XPATH + "//tr//td[contains(text(),'" + MTTypeDict[mtType] + "')]/../td[1]//input";
        }

		/// <summary>
		/// Возвращает список задач Workflow на этапе создания проекта
		/// </summary>
		/// <returns>Список строк</returns>
		public List<string> GetWFTaskList()
		{
			List<string> wfTaskList = new List<string>();

			// Выборка задач workflow
			string xPath = WF_TABLE_XPATH + "//tr//td[2]//span//span";
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
		/// Возвращает список всех типов workflow для заданной задачи
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		/// <returns>Список типов задачи</returns>
		public List<string> GetWFTaskTypeList(int taskNumber)
		{
			List<string> wfTaskList = new List<string>();

			// Выбор задачи workflow
			string workflowXPath = WF_TABLE_XPATH + "//tr[" + taskNumber.ToString() + "]//td[2]//span//span";

			// Получение выпадающего списка
			ClickElement(By.XPath(workflowXPath));
			IList<IWebElement> wfDropdownIList = GetElementList(By.XPath(WF_DROPDOWNLIST_XPATH));

			// Выбираем заданный
			foreach (IWebElement wfTaskType in wfDropdownIList)
			{
				wfTaskList.Add(wfTaskType.Text);
			}

			return wfTaskList;
		}

		/// <summary>
		/// Задает тип задачи Workflow
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		/// <param name="taskType">Тип задачи из выпадающего списка</param>
		public void SetWFTaskList(int taskNumber, string taskType)
		{
			List<string> wfTaskList = new List<string>();
			
			// Выбор задачи workflow
			string workflowXPath = WF_TABLE_XPATH + "//tr[" + taskNumber.ToString() + "]//td[2]//span//span";
			
			// Получение выпадающего списка
			ClickElement(By.XPath(workflowXPath));
			IList<IWebElement> wfDropdownIList = GetElementList(By.XPath(WF_DROPDOWNLIST_XPATH));

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
		/// Кликаем добавить новую задачу
		/// </summary>
		public void ClickWorkflowNewTask()
		{
			ClickElement(By.XPath(WF_NEW_TASK_BTN));
		}

		/// <summary>
		/// Кликаем кнопку удаления задачи
		/// </summary>
		public void ClickWorkflowDeleteTask(int taskNumber)
		{
			ClickElement(By.XPath(CREATE_PROJECT_DIALOG_XPATH + "//tr[" + taskNumber.ToString() + "]" + WF_DELETE_TASK_BTN));
		}

		/// <summary>
		/// Возвращает отображаемый номер для заданной задачи
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		/// <returns>Отображаемый номер задачи</returns>
		public int GetWFVisibleTaskNumber(int taskNumber)
		{
			// Выбор задачи workflow
			string workflowXPath = WF_TABLE_XPATH + "//tr[" + taskNumber.ToString() + "]//td[1]";

			// Получение номера
			return Int32.Parse(GetTextElement(By.XPath(workflowXPath)));
		}

		/// <summary>
		/// Возвращает, отображается ли сообщение о пустом Workflow
		/// </summary>
		/// <returns>Отображение сообщения об ошибке</returns>
		public bool GetIsErrorWFEmptyDisplayed()
		{
			return GetIsElementDisplay(By.XPath("//div[" + ERROR_WF_EMPTY + "]"));
		}

		/// <summary>
		/// Возвращает, находится ли мастер на шаге Workflow
		/// </summary>
		/// <returns>Мастер находится на шаге Workflow</returns>
		public bool GetIsStepWF()
		{
			return GetIsElementDisplay(By.XPath(WF_NEW_TASK_BTN));
		}

		/// <summary>
		/// Кликнуть Upload TMX (для STAGE3)
		/// </summary>
		public void ClickUploadTMXStage3()
		{
			ClickElement(By.XPath(UPLOAD_TMX_BTN_STAGE3_XPATH));
		}

		/// <summary>
		/// Дождаться появления диалога загрузки ТМХ (для STAGE3)
		/// </summary>
		/// <returns>появился</returns>
		public bool WaitUploadTMXDialogStage3()
		{
			return WaitUntilDisplayElement(By.XPath(IMPORT_TMX_DIALOG_STAGE3_XPATH));
		}

		/// <summary>
		/// Кликнуть Add TMX в окне диалога (для STAGE3)
		/// </summary>
		public void ClickAddTMXDialogStage3()
		{
			ClickElement(By.XPath(ADD_TMX_BTN_STAGE3_XPATH));
		}

		/// <summary>
		/// Кликнуть Save TMX в окне диалога (для STAGE3)
		/// </summary>
		public void ClickSaveTMXDialogStage3()
		{
			ClickElement(By.XPath(SAVE_BTN_STAGE3_XPATH));
		}

		/// <summary>
		/// Заполнить название ТМ в диалоге (Stage3)
		/// </summary>
		/// <param name="TMName">название</param>
		public void FillTMNameDialogStage3(string TMName)
		{
			ClearAndAddText(By.XPath(NAME_TM_STAGE3_XPATH), TMName);
		}


        public enum MT_TYPE { DefaultMT, Google, Bing, Yandex, Moses, None };
        protected Dictionary<MT_TYPE, string> MTTypeDict = new Dictionary<MT_TYPE, string>();
        

        protected const string CREATE_PROJECT_DIALOG_XPATH = "//div[contains(@class,'js-popup-create-project')][2]";
        protected const string DEADLINE_DATE_INPUT_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//input[@name='deadlineDate']";
		protected const string SOURCE_LANG_DROPDOWN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//div[select[@id='sourceLanguage']]/span";
        protected const string SPAN_DROPDOWN_LIST_XPATH = "//span[contains(@class,'js-dropdown__list')]";
        protected const string LANG_ITEM_XPATH = "//span[@data-id='";
        protected const string TARGET_MULTISELECT_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//div[contains(@class,'js-languages-multiselect')]";
        protected const string TARGET_MULTISELECT_ITEM_XPATH = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//li//span[contains(@class,'js-chckbx')]//input[@value='";
        protected const string TARGET_LANG_VALUE_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'ui-multiselect-value')]";
        protected const string PROJECT_NAME_INPUT_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//input[@name='name']";
        protected const string ADD_DOCUMENT_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//div[contains(@class,'js-files-uploader')]//a";// TODO проверить, старая версия: "//div[contains(@class,'js-files-uploader')]//a[contains(@class,'js-add-file')]" и "//div[contains(@class,'js-popup-create-project')][2]//a[contains(@class,'js-add-file')]"
        protected const string NEXT_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-next')]";
        protected const string CREATE_TM_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-tm-create')]";

		
		protected const string UPLOAD_TMX_BTN_STAGE3_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-tm-upload')]";
		protected const string IMPORT_TMX_DIALOG_STAGE3_XPATH = "//div[contains(@class,'js-popup-import-tm')][2]";
		protected const string ADD_TMX_BTN_STAGE3_XPATH = IMPORT_TMX_DIALOG_STAGE3_XPATH + "//a[contains(@class,'js-upload-btn')]";
		protected const string SAVE_BTN_STAGE3_XPATH = IMPORT_TMX_DIALOG_STAGE3_XPATH + "//span[contains(@class,'js-save')]";
		protected const string NAME_TM_STAGE3_XPATH = IMPORT_TMX_DIALOG_STAGE3_XPATH + "//input[contains(@class,'js-tm-name')]";


        protected const string CREATE_TM_DIALOG_XPATH = "//div[contains(@class,'js-popup-create-tm')][2]";
        protected const string NEW_TM_NAME_INPUT_XPATH = CREATE_TM_DIALOG_XPATH +"//input[contains(@class,'js-tm-name')]";
        protected const string ADD_TMX_BTN_XPATH = CREATE_TM_DIALOG_XPATH + "//a[contains(@class,'js-save-and-import')]";
        protected const string UPLOAG_TMX_DIALOG_XPATH = ".//div[contains(@class,'js-popup-import')][2]";
        protected const string UPLOAD_TMX_BTN_XPATH = UPLOAG_TMX_DIALOG_XPATH +"//a[contains(@class,'js-upload-btn')]";
        protected const string IMPORT_TMX_BTN_XPATH = UPLOAG_TMX_DIALOG_XPATH + "//span[contains(@class,'js-import-button')]";
        protected const string SAVE_TM_BTN_XPATH = CREATE_TM_DIALOG_XPATH + "//span[contains(@class,'js-save')]";
        
        protected const string BACK_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-back')]";

        protected const string TM_TABLE_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//table[contains(@class,'js-tms-popup-table')]//tbody";
        protected const string TM_TABLE_FIRST_ITEM_XPATH = TM_TABLE_XPATH + "//tr[1]//td[1]//input";
        protected const string TM_TABLE_CHECK_TD = "//td[contains(@class,'js-checkbox-area')]//input";
        protected const string TM_TABLE_RADIO_TD = "//td[contains(@class,'radio')]//input";
        protected const string TM_TABLE_TM_NAME_XPATH = TM_TABLE_XPATH + "//tr//td[contains(@class,'js-name')]";

        protected const string FIRST_GLOSSARY_XPATH =
			CREATE_PROJECT_DIALOG_XPATH + "//table[contains(@class,'js-glossaries')]//tbody//tr[1]/*/span[contains(@class,'js-chckbx')]";
        protected const string MT_TABLE_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//table[contains(@class,'js-mts')]//tbody";

        protected const string FINISH_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-finish js-upload-btn')]";
        protected const string ERROR_FORMAT_DOCUMENT_MESSAGE_XPATH = "//div[contains(@class,'js-info-popup')]" + CLOSE_BTN_XPATH; // TODO плохой id

        protected const string CLOSE_BTN_XPATH = "//div[2]//div//span[contains(@class,'js-popup-close')]";
        protected const string CLOSE_DIALOG_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + CLOSE_BTN_XPATH;

        protected const string ERROR_NAME_EXISTS_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//p[contains(@class,'js-error-name-exists')]";
        protected const string ERROR_NO_NAME_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//p[contains(@class,'js-error-name-required')]";
        protected const string ERROR_FILE_TM_XPATH = UPLOAG_TMX_DIALOG_XPATH + "//p[contains(@class,'js-error-invalid-file-extension')]";
        protected const string ERROR_DUPLICATE_LANG_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//p[contains(@class,'js-error-sourceLanguage-match-targetLanguage')]";
        protected const string ERROR_FORBIDDEN_SYMBOLS_NAME = CREATE_PROJECT_DIALOG_XPATH + "//p[contains(@class,'js-error-name-invalid-chars')]";

        protected const string STAGE_BTN_XPATH = "//table[contains(@class,'js-workflow-table')]//tbody//td[2]//span[contains(@class,'js-dropdown__text')]";
        protected const string STAGE_ITEM_XPATH = "//span[contains(@class,'js-dropdown__item')]";

        protected const string DEFAULT_MT_TYPE = "Default MT";
        protected const string GOOGLE_MT_TYPE = "Google";
        protected const string BING_MT_TYPE = "Microsoft Bing";
        protected const string YANDEX_MT_TYPE = "Yandex";
        protected const string MOSES_MT_TYPE = "Moses";

		protected const string WF_TABLE_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//table[contains(@class,'js-workflow-table')]//tbody";
		protected const string WF_DROPDOWNLIST_XPATH = "//span[contains(@class,'js-dropdown__item')]";
		protected const string WF_NEW_TASK_BTN = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-new-stage')]";
		protected const string WF_DELETE_TASK_BTN = "//a[contains(@class,'js-delete-workflow')]";
		protected const string ERROR_WF_EMPTY = "//p[contains(@class,'js-error-workflow-empty')]";

        //protected const string MT_TYPE = 
    }
}