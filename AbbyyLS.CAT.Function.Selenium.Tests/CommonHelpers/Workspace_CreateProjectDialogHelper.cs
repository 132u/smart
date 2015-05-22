using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер диалога создания проекта
	/// </summary>
	public class Workspace_CreateProjectDialogHelper : WorkSpacePageHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public Workspace_CreateProjectDialogHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
			MTTypeDict.Add(MT_TYPE.ABBYY, DEFAULT_MT_TYPE);
			MTTypeDict.Add(MT_TYPE.Google, GOOGLE_MT_TYPE);
			MTTypeDict.Add(MT_TYPE.Bing, BING_MT_TYPE);
			MTTypeDict.Add(MT_TYPE.Yandex, YANDEX_MT_TYPE);
			MTTypeDict.Add(MT_TYPE.Moses, MOSES_MT_TYPE);
		}

		//названия задач в списке на этапе настройки workflow в визарде
		private const string translationTaskType = "Translation";
		private const string editingTaskType = "Editing";
		private const string proofreadingTaskType = "Proofreading";

		public bool WaitDialogDisplay()
		{
			Logger.Debug("Дождаться появления диалога создания проекта");
			return WaitUntilDisplayElement(By.XPath(CREATE_PROJECT_DIALOG_XPATH));
		}

		public bool WaitDialogDisappear()
		{
			Logger.Debug("Дождаться, пока пропадет диалог создания проекта.");
			return WaitUntilDisappearElement(By.XPath(CREATE_PROJECT_DIALOG_XPATH));
		}

		public void FillDeadlineDate(string date)
		{
			Logger.Debug(String.Format("Добавление deadline даты: {0}", date));
			ClearAndAddText(By.XPath(DEADLINE_DATE_INPUT_XPATH), date);
		}

		/// <summary>
		/// Получить значение deadline
		/// </summary>
		/// <returns>deadline</returns>
		public string GetDeadlineValue()
		{
			return GetElementAttribute(By.XPath(DEADLINE_DATE_INPUT_XPATH), "value");
		}

		public void SelectSourceLanguage(LANGUAGE langType)
		{
			Logger.Debug(string.Format("Выбор исходного (source) языка: {0}", langType));

			Logger.Trace("Кликнуть на выпадающий список");
			ClickElement(By.XPath(SOURCE_LANG_DROPDOWN_XPATH));

			Logger.Trace("В выпадающем списке выбрать необходимый язык");
			ClickElement(By.XPath(SPAN_DROPDOWN_LIST_XPATH + LANG_ITEM_XPATH + GetLangByTypeWithBracket(langType)));
		}

		public void ClickTargetList()
		{
			Logger.Trace("Кликнуть по Target (открыть/закрыть список)");
			ClickElement(By.XPath(TARGET_MULTISELECT_XPATH));
		}

		public void ClickTargetItem(LANGUAGE langType)
		{
			Logger.Debug(string.Format("Кликнуть по элементу в списке Target. Язык: {0}", langType));
			ClickElement(By.XPath(TARGET_MULTISELECT_ITEM_XPATH + GetLangByTypeWithBracket(langType) + "]"));
		}

		public void ClickAllSelectedTargetItems()
		{
			Logger.Debug("Кликнуть по всем выбранным элементам в списке Target для отчистки поля");
			
			SetDriverTimeoutMinimum();

			foreach (var element in GetElementList(By.XPath(TARGET_MULTISELECT_ITEM_SELECTED_XPATH)))
			{
				element.Click();
			}

			SetDriverTimeoutDefault();
		}

		public List<string> GetTargetLanguageList()
		{
			Logger.Trace("Получение значения языка перевода (target)");
			return GetTextListElement(By.XPath(TARGET_LANG_VALUE_XPATH));
		}

		public void FillProjectName(string projectName)
		{
			Logger.Debug(String.Format("Добавление имени проекта: {0}", projectName));
			ClearAndAddText(By.XPath(PROJECT_NAME_INPUT_XPATH), projectName);
		}

		public string GetProjectName()
		{
			Logger.Trace("Получить название проекта, введенное в поле 'Name'");
			return GetElementAttribute(By.XPath(PROJECT_NAME_INPUT_XPATH), "value");
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

		public bool WaitDocumentAppear(string file)
		{
			Logger.Debug(string.Format("Ожидание появления загруженного документа {0} в списке", file));
			return WaitUntilDisplayElement(By.XPath(GetUploadedDocumentListXpath(file)));
		}

		/// <summary>
		/// xpath списка загруженного файла
		/// </summary>
		/// <returns>xpath файла</returns>
		public string GetUploadedDocumentListXpath(string file)
		{
			return UPLOADED_DOCUMENTS_LIST_XPATH + "//span[text()='" + file + "']";
		}

		public void ClickNextStep()
		{
			Log.Trace("Переход к следующему шагу (NEXT).");
			ClickElement(By.XPath(NEXT_BTN_XPATH));
			Thread.Sleep(1000);
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

		public bool WaitCreateTMDialog()
		{
			Log.Trace("Дождаться, пока появится диалог создания ТМ");
			return WaitUntilDisplayElement(By.XPath(CREATE_TM_DIALOG_XPATH));
		}

		public void FillTMName(string tmName)
		{
			Log.Trace(string.Format("Заполнить имя ТМ. Имя: {0}", tmName));
			ClearAndAddText(By.XPath(NEW_TM_NAME_INPUT_XPATH), tmName);
		}

		public void ClickImportBtn()
		{
			Log.Trace("Кликаем по кнопке Import");
			ClickElement(By.XPath(IMPORT_TMX_BTN_XPATH));
		}

		public bool WaitImportDialogDisappear()
		{
			Log.Trace("Дожидаемся закрытия диалога загрузки");
			return WaitUntilDisappearElement(By.XPath(CREATE_TM_DIALOG_XPATH));
		}

		public bool WaitTmxAppear(string tmName)
		{
			Log.Trace(string.Format("Дожидаемся появления новой ТМ в списке. Имя ТМ: {0}", tmName));
			return WaitUntilDisplayElement(By.XPath(GetTmxXpath(tmName)), 1);
		}

		/// <summary>
		/// xpath новой ТМ в списке
		/// </summary>
		/// <returns>xpath TM в списке</returns>
		public string GetTmxXpath(string tmName)
		{
			return TM_TABLE_TM_NAME_XPATH + "[text()='" + tmName + "']";
		}

		public bool WaitNewGlossaryAppear(string glossaryName)
		{
			Log.Trace(string.Format("Дождаться, пока новый глоссарий {0} появится в списке", glossaryName));
			return WaitUntilDisplayElement(By.XPath(GetNewGlossaryXpath(glossaryName)), 1);
		}

		/// <summary>
		/// xpath нового глоссария в списке
		/// </summary>
		/// <returns>xpath глоссария в списке</returns>
		public string GetNewGlossaryXpath(string glossaryName)
		{
			return FIRST_GLOSSARY_NAME_XPATH + "//p[text()='" + glossaryName + "']";
		}

		public void ClickSaveTM()
		{
			Log.Trace("Кликнуть кнопку сохранить ТМ");
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

		public bool GetIsTMTableNotEmpty()
		{
			Log.Trace("Получить: не пустая ли таблица TM");
			return WaitUntilDisplayElement(By.XPath(TM_TABLE_XPATH)) && GetIsElementExist(By.XPath(TM_TABLE_FIRST_ITEM_XPATH));
		}

		public void ClickFirstTMInTable()
		{
			Log.Trace("Выбраем первую ТМ из списка");
			ClickElement(By.XPath(TM_TABLE_FIRST_ITEM_XPATH));
			Thread.Sleep(500);
		}

		public void ClickFirstGlossaryInTable()
		{
			Log.Trace("Выбирается первый глоссарий в списке");
			ClickElement(By.XPath(FIRST_GLOSSARY_INPUT_XPATH));
		}

		public void ClickGlossaryByName(string glossaryName)
		{
			Log.Trace(string.Format("Выбрать глоссарий с именем {0}", glossaryName));

			ClickElement(By.XPath(GLOSSARY_BY_NAME_XPATH.Replace("#", glossaryName)));
		}

		/// <summary>
		/// Нажать галочку у ТМ
		/// </summary>
		/// <param name="mtType"></param>
		public void ChooseMT(MT_TYPE mtType)
		{
			ClickElement(By.XPath(GetMtXpath(mtType)));
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
		/// Вернуть: текущий шаг - шаг заполнения основной информации
		/// </summary>
		/// <returns>текущий шаг - шаг заполнения основной информации</returns>
		public bool GetIsGeneralInformationStep()
		{
			return GetIsElementDisplay(By.XPath(PROJECT_NAME_INPUT_XPATH));
		}

		/// <summary>
		/// Вернуть: отмечена ли МТ
		/// </summary>
		/// <param name="mtType">тип МТ</param>
		/// <returns>отмечена</returns>
		public bool GetIsMTChecked(MT_TYPE mtType)
		{
			return GetIsInputChecked(By.XPath(GetMtXpath(mtType)));
		}

		public void ClickFinishCreate()
		{
			Log.Trace("Нажать на кнопку Finish");
			ClickElement(By.XPath(FINISH_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Back
		/// </summary>
		public void ClickBackBtn()
		{
			ClickElement(By.XPath(BACK_BTN_XPATH));
			//даём форме время обновиться, иначе может произойти клик по кнопке Next до реакции на кнопку Back.
			Thread.Sleep(1000);
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
			var nameInputWebElement = By.XPath(PROJECT_NAME_INPUT_XPATH);
			WaitUntilDisplayElement(nameInputWebElement);

			return GetElementClass(nameInputWebElement).Contains("error");
		}

		/// <summary>
		/// Вернуть, есть ли сообщение о существующем имени
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsExistErrorMessageNameExists()
		{
			var errorMessageWebElement = By.XPath(ERROR_NAME_EXISTS_XPATH);
			WaitUntilDisplayElement(errorMessageWebElement);

			return GetIsElementDisplay(errorMessageWebElement);
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
		/// Вернуть, есть ли сообщение о неверной дате дедлайна
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsErrorMessageInvalidDeadlineDate()
		{
			WaitUntilDisplayElement(By.XPath(ERROR_DEADLINE_DATE_XPATH));
			return GetIsElementDisplay(By.XPath(ERROR_DEADLINE_DATE_XPATH));
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

			var isExistError = GetIsElementDisplay(By.XPath(ERROR_FILE_TM_XPATH));
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
			return GetIsInputChecked(By.XPath(FIRST_GLOSSARY_INPUT_XPATH));
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
			var xPath = STAGE_ITEM_XPATH + "[contains(@title,'" + stageText + "')]";
			var isExistStage = GetIsElementDisplay(By.XPath(xPath));

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

		public bool GetIsExistTM(string tmName)
		{
			Logger.Debug(string.Format("Вернуть, существует ли ТМ {0}", tmName));

			return GetIsElementExist(By.XPath(TM_TABLE_TM_NAME_XPATH + "[text()='" + tmName + "']"));
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
		protected string GetMtXpath(MT_TYPE mtType)
		{
			var typemt = "";
			switch (MTTypeDict[mtType])
			{
				case "Default MT":
					typemt = "f42671d9-df7e-4678-a846-d9143011cd2c";
					break;
				case "Google":
					typemt = "754204d5-3c49-4bc0-a5aa-28adf2e34b98";
					break;
				case "Microsoft Bing":
					typemt = "97ad5dd3-e547-4aaa-8f9b-3cfc530907ca";
					break;
				case "Yandex":
					typemt = "33baaffd-dee6-4a91-9abd-e591968f32f2";
					break;
			}

			return "//tr[@data-id='" + typemt + "']//input[contains(@name,'mt-checkbox')]";
		}

		/// <summary>
		/// Возвращает список задач Workflow на этапе создания проекта
		/// </summary>
		/// <returns>Список строк</returns>
		public List<string> GetWFTaskList()
		{
			var wfTaskList = new List<string>();

			// Выборка задач workflow
			var xPath = WF_TABLE_XPATH + "//tr//td[2]//span//span";
			var wfTaskIList = GetElementList(By.XPath(xPath));

			if (wfTaskIList.Count > 0)
			{
				wfTaskList.AddRange(wfTaskIList.Select(item => item.Text));
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
			// Выбор задачи workflow
			var workflowXPath = WF_TABLE_XPATH + "//tr[" + taskNumber.ToString() + "]//td[2]//span//span";

			// Получение выпадающего списка
			ClickElement(By.XPath(workflowXPath));
			var wfDropdownIList = GetElementList(By.XPath(WF_DROPDOWNLIST_XPATH));

			// Выбираем заданный

			return wfDropdownIList.Select(wfTaskType => wfTaskType.Text).ToList();
		}

		/// <summary>
		/// Задает тип задачи Workflow
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		/// <param name="taskType">Тип задачи из выпадающего списка</param>
		public void SetWFTaskList(int taskNumber, string taskType)
		{
			// Выбор задачи workflow
			var workflowXPath = WF_TABLE_XPATH + "//tr[" + taskNumber + "]//td[2]//span//span";
			
			// Получение выпадающего списка
			ClickElement(By.XPath(workflowXPath));
			var wfDropdownIList = GetElementList(By.XPath(WF_DROPDOWNLIST_XPATH));

			// Выбираем заданный
			foreach (var wfTaskType in wfDropdownIList)
			{
				if (wfTaskType.Text == taskType)
				{
					wfTaskType.Click();
					break;
				}
			}
			// Ожидание свертки дропдауна
			Thread.Sleep(1000);
		}
		/// <summary>
		/// Возвращает появилась ли задача Translation в списке
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		/// <returns>появилась ли задача</returns>
		public bool WaitUntilTranslationTaskDisplayed(int taskNumber)
		{
			// Выбор задачи workflow
			string workflowXPath = WF_TABLE_XPATH +
				"//tr[" + taskNumber + "]//td[2]//span//span" +
				"[contains(text(), '" + translationTaskType + "')]";

			return WaitUntilDisplayElement(By.XPath(workflowXPath), 1);
		}

		/// <summary>
		/// Возвращает появилась ли задача Editing в списке
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		/// <returns>появилась ли задача</returns>
		public bool WaitUntilEditingTaskDisplayed(int taskNumber)
		{
			// Выбор задачи workflow
			string workflowXPath = WF_TABLE_XPATH +
				"//tr[" + taskNumber.ToString() + "]//td[2]//span//span" +
				"[contains(text(), '" + editingTaskType + "')]";

			return WaitUntilDisplayElement(By.XPath(workflowXPath), 1);
		}

		/// <summary>
		/// Возвращает появилась ли задача Proofreading в списке
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		/// <returns>появилась ли задача</returns>
		public bool WaitUntilProofreadingTaskDisplayed(int taskNumber)
		{
			// Выбор задачи workflow
			var workflowXPath = WF_TABLE_XPATH +
				"//tr[" + taskNumber.ToString() + "]//td[2]//span//span" +
				"[contains(text(), '" + proofreadingTaskType + "')]";

			return WaitUntilDisplayElement(By.XPath(workflowXPath), 1);
		}

		/// <summary>
		/// Задает тип задачи Workflow - Translation
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		public void SetWorkflowTranslationTask(int taskNumber)
		{
			Logger.Trace("Получение выпадающего списка");
			ClickElement(By.XPath(GetWfTaskNumberXpath(taskNumber)));
			var wfDropdownIList = GetElementList(By.XPath(WF_DROPDOWNLIST_XPATH));

			Logger.Trace("Выбираем заданный workflow - перевод");
			var wf = wfDropdownIList.FirstOrDefault(wfTaskType => wfTaskType.Text == translationTaskType);
			if (wf != null)
			{
				wf.Click();
			}
			else
			{
				Logger.Error("Заданный workflow (перевод) не найден.");
				throw new NullReferenceException("Невозможно найти необходимый workflow (перевод).");
			}
			
			WaitUntilTranslationTaskDisplayed(taskNumber);
		}

		/// <summary>
		/// Задает тип задачи Workflow - Editing
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		public void SetWorkflowEditingTask(int taskNumber)
		{
			Logger.Trace("Получение выпадающего списка");
			ClickElement(By.XPath(GetWfTaskNumberXpath(taskNumber)));
			var wfDropdownIList = GetElementList(By.XPath(WF_DROPDOWNLIST_XPATH));

			Logger.Trace("Выбираем заданный workflow - редактура");
			var wf = wfDropdownIList.FirstOrDefault(wfTaskType => wfTaskType.Text == editingTaskType);
			if (wf != null)
			{
				wf.Click();
			}
			else
			{
				Logger.Error("Заданный workflow (редактура) не найден.");
				throw new NullReferenceException("Невозможно найти необходимый workflow (редактура).");
			}

			WaitUntilEditingTaskDisplayed(taskNumber);
		}

		/// <summary>
		/// Задает тип задачи Workflow - Proofreading
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		public void SetWorkflowProofreadingTask(int taskNumber)
		{
			Logger.Trace("Получение выпадающего списка");
			ClickElement(By.XPath(GetWfTaskNumberXpath(taskNumber)));
			var wfDropdownIList = GetElementList(By.XPath(WF_DROPDOWNLIST_XPATH));

			Logger.Trace("Выбираем заданный workflow - proofreading");
			var wf = wfDropdownIList.FirstOrDefault(wfTaskType => wfTaskType.Text == proofreadingTaskType);
			if (wf != null)
			{
				wf.Click();
			}
			else
			{
				Logger.Error("Заданный workflow (proofreading) не найден.");
				throw new NullReferenceException("Невозможно найти необходимый workflow (proofreading).");
			}

			WaitUntilProofreadingTaskDisplayed(taskNumber);
		}

		/// <summary>
		/// Вернуть xPath строки с нужным этапом
		/// </summary>
		/// <returns>xPath</returns>
		protected string GetWfTaskNumberXpath(int taskNumber)
		{
			return WF_TABLE_XPATH + "//tr[" + taskNumber + "]//td[2]//span//span";
		}

		public void ClickWorkflowNewTask()
		{
			Logger.Trace("Кликаем добавить новую задачу");
			ClickElement(By.XPath(WF_NEW_TASK_BTN));
		}

		/// <summary>
		/// Кликаем кнопку удаления задачи
		/// </summary>
		public void ClickWorkflowDeleteTask(int taskNumber)
		{
			ClickElement(By.XPath(CREATE_PROJECT_DIALOG_XPATH + "//tr[" 
				+ taskNumber + "]" + WF_DELETE_TASK_BTN));

			// Ожидание выполнения удаления
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Возвращает отображаемый номер для заданной задачи
		/// </summary>
		/// <param name="taskNumber">Номер задачи</param>
		/// <returns>Отображаемый номер задачи</returns>
		public int GetWFVisibleTaskNumber(int taskNumber)
		{
			// Выбор задачи workflow
			var workflowXPath = WF_TABLE_XPATH + "//tr[" + taskNumber + "]//td[1]";

			// Получение номера
			return Int32.Parse(GetTextElement(By.XPath(workflowXPath)));
		}

		/// <summary>
		/// Проверяет имя проекта
		/// </summary>
		/// <param name="projectName">Имя проекта</param>
		/// <returns>Нужное ли имя</returns>
		public bool CheckProjectName(string projectName)
		{
			var name = GetElementAttribute(By.XPath(PROJECT_NAME_INPUT_XPATH), "value");
			
				return (name == projectName);			
		}

		/// <summary>
		/// Возвращает, отображается ли сообщение о пустом Workflow
		/// </summary>
		/// <returns>Отображение сообщения об ошибке</returns>
		public bool GetIsErrorWFEmptyDisplayed()
		{
			return GetIsElementDisplay(By.XPath(CREATE_PROJECT_DIALOG_XPATH + ERROR_WF_EMPTY));
		}

		/// <summary>
		/// Возвращает, находится ли мастер на шаге Workflow
		/// </summary>
		/// <returns>Мастер находится на шаге Workflow</returns>
		public bool GetIsStepWF()
		{
			return GetIsElementDisplay(By.XPath(WF_NEW_TASK_BTN));
		}

		public void ClickUploadTMX()
		{
			Log.Trace("Кликнуть кнопку Upload TMX");
			ClickElement(By.XPath(UPLOAD_TMX_BTN_XPATH));
		}

		public void ClickCreateGlossary()
		{
			Log.Trace("Кликнуть кнопку создания голоссария");
			WaitUntilDisplayElement(By.XPath(CREATE_GLOSSARY_BTN_XPATH));
			ClickElement(By.XPath(CREATE_GLOSSARY_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть по кнопке "Удалить файл"
		/// </summary>
		/// <param name="fileName">имя файла</param>
		public void ClickDeleteFile(string fileName)
		{
			ClickElement(By.XPath(DELETE_FILE_BTN_XPATH.Replace("#", fileName)));
		}

		/// <summary>
		/// Кликнуть по полю Имя
		/// </summary>	   
		public void ClickProjectNameInput()
		{
			ClickElement(By.XPath(PROJECT_NAME_INPUT_XPATH));
		}

		/// <summary>
		/// Кликнуть по полю Дедлайн, чтобы появился календарь
		/// </summary>		
		public void ClickDeadlineInput()
		{
			ClickElement(By.XPath(DEADLINE_DATE_INPUT_XPATH));
		}

		/// <summary>
		/// Кликнуть по текущей дате в календаре
		/// </summary>		
		public void ClickDeadlineCurrentDate()
		{
			ClickElement(By.XPath(DEADLINE_DATE_CURRENT_XPATH));
		}

		/// <summary>
		/// Перейти на следующий месяц календаря
		/// </summary>		
		public void ClickNextMonthPicker()
		{
			ClickElement(By.XPath(DEADLINE_DATE_NEXT_MONTH_XPATH));
		}

		/// <summary>
		/// Перейти на предыдущий месяц календаря
		/// </summary>		
		public void ClickPreviousMonthPicker()
		{
			ClickElement(By.XPath(DEADLINE_DATE_PREV_MONTH_XPATH));
		}

		/// <summary>
		/// Кликнуть по произвольной дате в календаре
		/// </summary>		
		public void ClickDeadlineSomeDate()
		{
			ClickElement(By.XPath(DEADLINE_DATE_XPATH));
		}

		/// <summary>
		/// Дождаться, пока появится календарь
		/// </summary>	   
		/// <returns>появился календарь</returns>
		public bool WaitUntilDisplayCalendar()
		{
			return WaitUntilDisplayElement(By.XPath(DEADLINE_DATE_CURRENT_XPATH));
		}

		/// <summary>
		/// Дождаться, пока заданный файл исчезнет из списка загруженных файлов
		/// </summary>
		/// <param name="fileName">имя файла</param>
		/// <returns>файла в списке нет</returns>
		public bool WaitUntilFileDisappear(string fileName)
		{
			return WaitUntilDisappearElement(By.XPath(DELETE_FILE_BTN_XPATH.Replace("#", fileName)));			
		}

		public bool WaitUntilConfirmTMDialogDisplay()
		{
			Log.Trace("Дождаться, пока появится диалог подтверждения не выбранной ТМ");
			return WaitUntilDisplayElement(By.XPath(CONFIRM_NOT_SELECTED_TM_DIALOG_XPATH), 3);
		}
	
		public void ClickSkipBtn()
		{
			Log.Trace("Кликнуть по кнопке Skip");
			ClickElement(By.XPath(CONFIRM_DIALOG_SKIP_BTN_XPATH));
		}

		public void AssertionConfirmTMDialogDisappear()
		{
			Logger.Trace("Дожидаемся исчезновения диалога подтверждения не выбранной ТМ ");

			Assert.IsTrue(
				WaitUntilDisappearElement(By.XPath(CONFIRM_NOT_SELECTED_TM_DIALOG_XPATH), 3),
				"Ошибка: после нажатия кнопки Skip диалог подтверждения не выбранной ТМ не закрылся.");
		}

		public void ClickSaveNewGlossary()
		{
			Log.Trace("Кликнуть по кнопке сохранения голоссария");
			ClickElement(By.XPath(SAVE_GLOSSARY_BTN_XPATH));
		}

		public void SetNewGlossaryName(string internalGlossaryName)
		{
			Log.Trace("Ввести имя голоссария");
			WaitUntilDisplayElement(By.XPath(NEW_GLOSSARY_NAME_INPUT_XPATH));
			ClearAndAddText(By.XPath(NEW_GLOSSARY_NAME_INPUT_XPATH), internalGlossaryName);
		}

		public void UploadTMInNewProject(string documentName)
		{
			Log.Trace(string.Format("Загрузка TM во время создания проекта. Имя документа: {0}", documentName));
			UploadTM(documentName, TM_UPLOAD);
		}

		public void UploadFileToNewProject(string fileName)
		{
			Logger.Debug(string.Format("Загрузка документа во время создания проекта. Имя файла: {0}", fileName));
			UploadDocument(fileName, UPLOAD_FILE_TO_NEW_PROJECT);
		}

		public bool IsWorkflowStepPresented()
		{
			return GetIsElementDisplay(By.XPath(WORKFLOW_SETUP_XPATH));
		}

		protected const string UPLOAD_FILE_TO_NEW_PROJECT = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-files-uploader')]//input"; // добавление документа при создании проекта

		public enum SetGlossary { New, First, ByName, None };
		public enum MT_TYPE { ABBYY, Google, Bing, Yandex, Moses, None };
		protected Dictionary<MT_TYPE, string> MTTypeDict = new Dictionary<MT_TYPE, string>();
		protected const string TM_UPLOAD = "//div[contains(@class,\"js-popup-create-tm\")][2]//input[@type=\"file\"]";
		protected const string CREATE_PROJECT_DIALOG_XPATH = "//div[contains(@class,'js-popup-create-project')][2]";

		protected const string DEADLINE_DATE_INPUT_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//input[contains(@class, 'l-project__date')]";
		protected const string SOURCE_LANG_DROPDOWN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//div[select[@id='sourceLanguage']]/span";
		protected const string SPAN_DROPDOWN_LIST_XPATH = "//span[contains(@class,'js-dropdown__list')]";
		protected const string LANG_ITEM_XPATH = "//span[@data-id='";
		protected const string TARGET_MULTISELECT_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//div[contains(@class,'js-languages-multiselect')]";
		protected const string TARGET_MULTISELECT_ITEM_XPATH = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//li//span[contains(@class,'js-chckbx')][input[@value='";
		protected const string TARGET_MULTISELECT_ITEM_SELECTED_XPATH = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//li//span[contains(@class,'js-chckbx')]//input[@aria-selected='true']";
		protected const string TARGET_LANG_VALUE_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'ui-multiselect-value')]";
		protected const string PROJECT_NAME_INPUT_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//input[@name='name']";
		protected const string ADD_DOCUMENT_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//div[contains(@class,'js-files-uploader')]//a";// TODO проверить, старая версия: "//div[contains(@class,'js-files-uploader')]//a[contains(@class,'js-add-file')]" и "//div[contains(@class,'js-popup-create-project')][2]//a[contains(@class,'js-add-file')]"
		protected const string UPLOADED_DOCUMENTS_LIST_XPATH = "//li[@class='js-file-list-item']";
		protected const string NEXT_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-next')]";
		protected const string CREATE_TM_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-tm-create')]";
		protected const string DELETE_FILE_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//li[contains(@class, 'js-file-list') and contains(string(), '#')]//span[contains(@class, 'btn')]//a[contains(@class, 'js-remove-file')]";
		protected const string DEADLINE_DATE_CURRENT_XPATH = "//div[contains(@id, 'ui-datepicker-div')]//table[contains(@class, 'ui-datepicker-calendar')]//td[contains(@class, 'ui-datepicker-today')]//a";
		protected const string DEADLINE_DATE_NEXT_MONTH_XPATH = "//div[contains(@id, 'ui-datepicker-div')]//a[contains(@class, 'ui-datepicker-next')]";
		protected const string DEADLINE_DATE_PREV_MONTH_XPATH = "//div[contains(@id, 'ui-datepicker-div')]//a[contains(@class, 'ui-datepicker-prev')]";
		protected const string DEADLINE_DATE_XPATH = "//div[contains(@id, 'ui-datepicker-div')]//table[contains(@class, 'ui-datepicker-calendar')]//tr[2]//td[2]";
		protected const string WORKFLOW_SETUP_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//div[@data-step-id='workflow']";
		
		protected const string UPLOAD_TMX_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-tm-upload')]";
		protected const string CREATE_GLOSSARY_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-glossary-create')]";
		
		protected const string CONFIRM_NOT_SELECTED_TM_DIALOG_XPATH = "//div[contains(@class,'js-incorrect-tm-popup')][2]";
		protected const string CONFIRM_DIALOG_SKIP_BTN_XPATH = CONFIRM_NOT_SELECTED_TM_DIALOG_XPATH + "//input[contains(@class,'js-skip-btn')]";

		protected const string CREATE_TM_DIALOG_XPATH = "//div[contains(@class,'js-popup-create-tm')][2]";
		protected const string NEW_TM_NAME_INPUT_XPATH = CREATE_TM_DIALOG_XPATH + "//input[contains(@class,'l-createtm__nmtext')]";
		protected const string IMPORT_TMX_BTN_XPATH = CREATE_TM_DIALOG_XPATH + "//span[contains(@class,'js-import-button')]";
		protected const string SAVE_TM_BTN_XPATH = CREATE_TM_DIALOG_XPATH + "//span[contains(@data-bind,'click: save')]";
		
		protected const string BACK_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-back')]";

		protected const string TM_TABLE_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//table[contains(@class,'js-tms-popup-table')]//tbody";
		protected const string TM_TABLE_FIRST_ITEM_XPATH = TM_TABLE_XPATH + "//tr[1]//td[1]//span//input";
		protected const string TM_TABLE_CHECK_TD = "//td[contains(@class,'js-checkbox-area')]//input";
		protected const string TM_TABLE_RADIO_TD = "//td[contains(@class,'radio')]//input";
		protected const string TM_TABLE_TM_NAME_XPATH = TM_TABLE_XPATH + "//tr//td[contains(@class,'js-name')]";

		protected const string FIRST_GLOSSARY_XPATH =
			CREATE_PROJECT_DIALOG_XPATH + "//table[contains(@class,'js-glossaries')]//tbody//tr[1]/*/span[contains(@class,'js-chckbx')]";
		protected const string FIRST_GLOSSARY_INPUT_XPATH = FIRST_GLOSSARY_XPATH + "//input";
		protected const string FIRST_GLOSSARY_NAME_XPATH = "//table[contains(@class,'js-glossaries')]//tbody//tr[1]//td[2]";
		protected const string GLOSSARY_BY_NAME_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//table[contains(@class,'js-glossaries')]//tbody//tr[contains(string(), '#')]/*/span[contains(@class,'js-chckbx')]//input";
		protected const string GLOSSARY_BY_NAME_TR_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//table[contains(@class,'js-glossaries')]//tbody//tr[contains(string(), '#')]";
		protected const string CREATE_GLOSSARY_DIALOG_XPATH = "//div[contains(@class,'js-popup-edit-glossary')][2]";
		protected const string NEW_GLOSSARY_NAME_INPUT_XPATH = CREATE_GLOSSARY_DIALOG_XPATH + "//input[contains(@class,'l-editgloss__nmtext')]";
		protected const string SAVE_GLOSSARY_BTN_XPATH = CREATE_GLOSSARY_DIALOG_XPATH + "//span[@data-bind='click: save']";

		protected const string MT_TABLE_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//table[contains(@class,'js-mts-body')]//tbody";

		protected const string FINISH_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-finish js-upload-btn')]";
		protected const string ERROR_FORMAT_DOCUMENT_MESSAGE_XPATH = "//div[contains(@class,'js-info-popup')]"; // TODO плохой id

		protected const string CLOSE_BTN_XPATH = "//img[contains(@class,'js-popup-close')]";
		protected const string CLOSE_DIALOG_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + CLOSE_BTN_XPATH;

		protected const string ERROR_NAME_EXISTS_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//p[contains(@class,'js-error-name-exists')]";
		protected const string ERROR_NO_NAME_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//p[contains(@class,'js-error-name-required')]";
		protected const string ERROR_FILE_TM_XPATH = CREATE_TM_DIALOG_XPATH + "//p[contains(@class,'js-error-invalid-file-extension')]";
		protected const string ERROR_DUPLICATE_LANG_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//p[contains(@class,'js-error-sourceLanguage-match-targetLanguage')]";
		protected const string ERROR_FORBIDDEN_SYMBOLS_NAME = CREATE_PROJECT_DIALOG_XPATH + "//p[contains(@class,'js-error-name-invalid-chars')]";
		protected const string ERROR_DEADLINE_DATE_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//p[contains(@class,'js-error-date-incorrect')]";

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

		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

	}
}