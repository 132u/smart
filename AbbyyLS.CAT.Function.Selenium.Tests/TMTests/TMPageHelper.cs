﻿using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Windows.Forms;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	/// <summary>
	/// Хелпер страницы ТМ
	/// </summary>
	public class TMPageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public TMPageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
			TMButtonDict = new Dictionary<TM_BTN_TYPE,string>
			{
				{TM_BTN_TYPE.Update, UPDATE_BTN_XPATH},
				{TM_BTN_TYPE.Export, EXPORT_BTN_XPATH},
				{TM_BTN_TYPE.Delete, DELETE_BTN_XPATH},
				{TM_BTN_TYPE.Add, ADD_TMX_BTN_XPATH},
				{TM_BTN_TYPE.Edit, EDIT_BTN_XPATH},
				{TM_BTN_TYPE.Save, SAVE_BTN_XPATH}
			};
			// TODO заполнить все
		}

		/// <summary>
		/// Дождаться загрузки страницы
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(ADD_TM_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Создать TM
		/// </summary>
		/// <returns>форма создания открылась</returns>
		public bool ClickCreateTM()
		{
			// Нажать кнопку Создать TM
			ClickElement(By.XPath(ADD_TM_BTN_XPATH));
			// ждем загрузку формы
			return WaitUntilDisplayElement(By.XPath(CREATE_TM_DIALOG_XPATH));
		}

		/// <summary>
		/// Кликнуть для открытия списка клиентов
		/// </summary>
		/// <returns>открылся</returns>
		public bool ClickOpenClientListCreateTM()
		{
			// Нажать на открытие списка клиентов
			ClickElement(By.XPath(CREATE_TM_CLIENT_XPATH));
			// Дождаться открытия
			return WaitUntilDisplayElement(By.XPath(CREATE_TM_CLIENT_LIST_XPATH));
		}

		/// <summary>
		/// Кликнуть для открытия списка domain
		/// </summary>
		/// <returns>открылся</returns>
		public bool ClickOpenDomainListCreateTM()
		{
			// Нажать на открытие списка domain
			ClickElement(By.XPath(CREATE_TM_DOMAIN_XPATH));
			// Дождаться открытия
			return WaitUntilDisplayElement(By.XPath(CREATE_TM_DOMAIN_LIST_XPATH));
		}

		/// <summary>
		/// Вернуть: есть ли клиент в списке клиентов при создании ТМ
		/// </summary>
		/// <param name="clientName">название</param>
		/// <returns>есть</returns>
		public bool GetIsClientExistCreateTM(string clientName)
		{
			// Получить список клиентов
			IList<IWebElement> clientList = GetElementList(
				By.XPath(CREATE_TM_CLIENT_LIST_XPATH + CREATE_TM_CLIENT_ITEM_XPATH));
			bool bClientExist = false;
			foreach (IWebElement el in clientList)
			{
				if (el.GetAttribute("title") == clientName)
				{
					// Если клиент в списке
					bClientExist = true;
					break;
				}
			}
			return bClientExist;
		}

		/// <summary>
		/// Вернуть, есть ли domain в списке
		/// </summary>
		/// <param name="domainName">название</param>
		/// <returns>есть</returns>
		public bool GetIsDomainExistCreateTM(string domainName)
		{
			// Получить список проектов
			IList<IWebElement> DomainList = GetElementList(By.XPath(CREATE_TM_DOMAIN_ITEMS_XPATH));
			bool isDomainExist = false;
			foreach (IWebElement el in DomainList)
			{
				if (el.Text == domainName)
				{
					// Если проект в списке
					isDomainExist = true;
					break;
				}
			}

			return isDomainExist;
		}

		/// <summary>
		/// Дождаться загрузки
		/// </summary>
		/// <returns>загрузился документ</returns>
		public bool WaitDocumentDownloadFinish()
		{
			bool isDisappeared = true;
			if (GetIsElementDisplay(By.XPath(DOWNLOAD_TMX_IMG_PATH)))
			{
				isDisappeared = false;
				for (int i = 0; i < 5; ++i)
				{
					isDisappeared = WaitUntilDisappearElement(By.XPath(DOWNLOAD_TMX_IMG_PATH), 40);
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
		/// Кликнуть Сохранить и загрузить документ в диалоге создания ТМ
		/// </summary>
		public void ClickSaveAndImportCreateTM()
		{
			ClickElement(By.XPath(CREATE_TM_DIALOG_SAVE_AND_IMPORT_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться открытия диалога загрузки документа
		/// </summary>
		/// <returns>открылся</returns>
		public bool WaitUntilUploadDialog()
		{
			return WaitUntilDisplayElement(By.XPath(UPLOAD_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Add в диалоге загрузки документа
		/// </summary>
		public void ClickAddUploadBtn()
		{
			ClickElement(By.XPath(UPLOAD_BTN_XPATH));
		}

		/// <summary>
		/// Получить: открыта ли информация о ТМ
		/// </summary>
		/// <param name="TMName"></param>
		/// <returns></returns>
		public bool GetIsTMOpened(string TMName)
		{
			return GetElementClass(By.XPath(GetTMRow(TMName))).Contains("opened");
		}

		/// <summary>
		/// Кликнуть по строке с ТМ
		/// </summary>
		/// <param name="TMName">название ТМ</param>
		public void ClickTMRow(string TMName)
		{
			ClickElement(By.XPath(GetTMRow(TMName)));
		}

		/// <summary>
		/// Кликнуть кнопку в информации о ТМ
		/// </summary>
		public void ClickTMButton(TM_BTN_TYPE btnType)
		{
			ClickElement(By.XPath(TMButtonDict[btnType]));
		}

		/// <summary>
		/// Получить количество сегментов
		/// </summary>
		/// <returns>количество</returns>
		public int GetSegmentCount()
		{
			string segmentText = GetTextElement(By.XPath(SEGMENT_SPAN_XPATH));

			// Нужно получить число сегментов из строки "Segments count: N", разделитель - ":"
			int splitIndex = segmentText.IndexOf(":");
			// Отступаем двоеточие и пробел
			splitIndex += 2;
			if (segmentText.Length > splitIndex)
			{
				segmentText = segmentText.Substring(splitIndex);
			}

			// Получить число сегментов из строки
			return ParseStrToInt(segmentText);
		}

		///<summary>
		/// Вернуть true, если проект projectName указан в информации о ТМ
		/// </summary>
		public bool GetIsProjectExistInTmInformation(string tmName, string projectName)
		{
			var projectText = GetTextElement(By.XPath(PROJECT_GROUP_SPAN_XPATH));

			var splitIndex = projectText.IndexOf(":") + 3;

			if (projectText.Length > splitIndex)
			{
				projectText = projectText.Substring(splitIndex);
			}
			else
			{
				projectText = string.Empty;
			}

			return projectText == projectName;
		}

		/// <summary>
		/// Кликнуть Импорт
		/// </summary>
		public void ClickImportBtn()
		{
			ClickElement(By.XPath(IMPORT_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть для открытия списка языков Source
		/// </summary>
		public void ClickOpenSourceLangList()
		{
			ClickElement(By.XPath(OPEN_SRC_LANG_CREATE_TM_XPATH));
		}

		/// <summary>
		/// Выбрать язык Source
		/// </summary>
		/// <param name="lang">язык</param>
		public void SelectSourceLanguage(LANGUAGE lang)
		{
			string xPath = SOURCE_LANG_ITEM_XPATH + languageID[lang] + "']";
			ClickElement(By.XPath(xPath));
		}

		/// <summary>
		/// Кликнуть открыть/закрыть список target
		/// </summary>
		public void ClickTargetLangList()
		{
			ClickElement(By.XPath(OPEN_TRG_LANG_CREATE_TM_XPATH));
		}

		/// <summary>
		/// Выбрать язык Target
		/// </summary>
		/// <param name="lang">язык</param>
		public void SelectTargetLanguage(LANGUAGE lang)
		{
			string xPath = TARGET_LANG_ITEM_XPATH + languageID[lang] + "']";

			ClickElement(By.XPath(xPath));
		}

		/// <summary>
		/// Ввести название нового ТМ
		/// </summary>
		/// <param name="name">название</param>
		public void InputNewTMName(string name)
		{
			SendTextElement(By.XPath(NEW_TM_NAME_XPATH), name);
		}

		/// <summary>
		/// Вернуть, в форме создания ТМ поле имя отмечено ошибкой?
		/// </summary>
		/// <returns>отмечено</returns>
		public bool GetIsCreateTMInputNameError()
		{
			return GetElementClass(By.XPath(NEW_TM_NAME_XPATH)).Contains("error");
		}

		/// <summary>
		/// Нажать Сохранить новый ТМ
		/// </summary>
		public void ClickSaveNewTM()
		{
			// Нажать кнопку Сохранить
			ClickElement(By.XPath(SAVE_TM_BTN_XPATH));
		}

		/// <summary>
		/// Отменить сохранение новой ТМ
		/// </summary>
		public void ClickCancelSavingNewTM()
		{
			// Нажать кнопку Сохранить
			ClickElement(By.XPath(CANCEL_TM_SAVING_BTN_XPATH));
		}

		/// <summary>
		/// Нажать кнопку Cancel на информационной плашке
		/// </summary>
		public void ClickCancelButtonOnNotificationBaloon()
		{
			ClickElement(By.XPath(NOTIFICATION_BALOON_BUTTON_XPATH));
		}

		/// <summary>
		/// Нажать кнопку Cancel на форме редактирования ТМ
		/// </summary>
		public void ClickCanselOnEditionForm()
		{
			ClickElement(By.XPath(TM_EDIT_CANCEL));
		}

		/// <summary>
		/// Вернуть, есть ли ТМ в списке
		/// </summary>
		/// <param name="TMName">название ТМ</param>
		/// <returns>есть</returns>
		public bool GetIsExistTM(string TMName)
		{
			// TODO проверить мб проверить циклом
			return GetIsElementExist(By.XPath(TM_ROW_NAME + "[text()='" + TMName + "']"));
		}

		/// <summary>
		/// Вернуть, если для данной ТМ указаны корректные языки
		/// </summary>
		public bool GetIsCorrectLanguagesForTm(string TMName, string formattedLanguagesString)
		{
			return GetTextElement(By.XPath(GetTMRow(TMName))).Contains(formattedLanguagesString);
		}

		/// <summary>
		/// Вернуть true, если информационная плашка существует
		/// </summary>
		public bool IsInformationBaloonExist()
		{
			return GetIsElementExist(By.XPath(NOTIFICATION_BALOON_TEXT_XPATH));
		}

		/// <summary>
		/// Вернуть true, если плашка с текстом text появилась в окне браузера
		/// <param name="text">текст сообщения</param>
		/// </summary>
		public bool IsBaloonWithSpecificMessageExist(string text)
		{
			return WaitUntilDisplayElement(
				By.XPath(
					NOTIFICATION_BALOON_TEXT_XPATH + "[text()='" + text + "']"), 
					45);
		}

		/// <summary>
		/// Открыть диалог создания ТМ
		/// </summary>
		/// <returns>открылся</returns>
		public bool OpenCreateTMDialog()
		{
			ClickElement(By.XPath(ADD_TM_BTN_XPATH));
			return WaitUntilDisplayElement(By.XPath(CREATE_TM_DIALOG_XPATH));
		}

		/// <summary>
		/// Дождаться открытия форму редактирования ТМ
		/// </summary>
		/// <returns>открылась</returns>
		public bool WaitUntilEditTMOpen()
		{
			return WaitUntilDisplayElement(By.XPath(TM_EDIT_FORM_XPATH));
		}

		/// <summary>
		/// Очистить имя в форме изменения ТМ
		/// </summary>
		public void EditTMClearName()
		{
			ClearElement(By.XPath(TM_EDIT_NAME_XPATH));
		}

		///<summary>
		/// Кликнуть на поле с проектом в форме редактирования ТМ
		/// </summary>
		public void ClickToProjectsListAtTmEdditForm()
		{
			ClickElement(By.XPath(TM_EDIT_PROJECT));
		}

		/// <summary>
		/// Вернуть true если при редактировании проекта доступна хотя бы одна проектная группа
		/// </summary>
		/// <returns></returns>
		public bool IsAnyProjectGroupExist()
		{
			return GetIsElementExist(By.XPath(PROJECT_TO_ADD_ITEM_XPATH));
		}

		///<summary>
		/// Добавить проект projectName к ТМ
		/// </summary>
		public string EditTMAddProject()
		{
			ClickElement(By.XPath(PROJECT_TO_ADD_ITEM_XPATH));

			return GetTextElement(By.XPath(PROJECT_TO_ADD_ITEM_XPATH));
		}

		///<summary>
		/// Добавить проект projectName к ТМ
		/// </summary>
		public void EditTMAddProject(string projectName)
		{
			ClickElement(By.XPath(DOMAIN_TO_ADD_XPATH + "[@title='" + projectName + "']"));
		}

		/// <summary>
		/// Очистить комментарий в форме изменения ТМ
		/// </summary>
		public void EditTMClearComment()
		{
			if (GetIsElementExist(By.XPath(TM_EDIT_COMMENT_XPATH)))
			{
				ClearElement(By.XPath(TM_EDIT_COMMENT_XPATH));
			}
		}

		///<summary>
		/// Кликнуть на поле с языком перевода в форме редактирования ТМ
		/// </summary>
		public void ClickToTargetLanguagesAtTmEdditForm()
		{
			ClickElement(By.XPath(TM_EDIT_TARGET_LANGUAGE));
		}

		/// <summary>
		/// Ввести имя Тм в форме редактирования ТМ
		/// </summary>
		/// <param name="TMName">название ТМ</param>
		public void InputEditTMName(string TMName)
		{
			SendTextElement(By.XPath(TM_EDIT_NAME_XPATH), TMName);
		}

		/// <summary>
		/// Ввести комментарий Тм в форме редактирования ТМ
		/// </summary>
		/// <param name="tmComment">комментарий к ТМ</param>
		public void InputEditTMComment(string tmComment)
		{
			SendTextElement(By.XPath(TM_EDIT_COMMENT_XPATH), tmComment);
		}

		/// <summary>
		/// Получить имя проектной группы для ТМ
		/// </summary>
		public string GetProjectGroupNameForTm()
		{
			return GetTextElement(By.XPath("//div[contains(@class,'js-domains-multiselect')]//div//span"));
		}

		/// <summary>
		/// Вернуть, отмечено ли поле имя в форме редактирования ТМ ошибкой
		/// </summary>
		/// <returns>отмечено</returns>
		public bool GetIsEditTMNameWithError()
		{
			return GetElementClass(By.XPath(TM_EDIT_NAME_XPATH)).Contains("error");
		}

		/// <summary>
		/// Кликнуть Сохранить в форме редактирования
		/// </summary>
		public void ClickEditSaveBtn()
		{
			ClickElement(By.XPath(TM_EDIT_SAVE_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли ошибка при редактировании ТМ существующего имени
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsExistEditErrorExistName()
		{
			return GetIsElementDisplay(By.XPath(ERROR_EDIT_EXIST_NAME_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли ошибка при редактировании ТМ пустого имени
		/// </summary>
		/// <returns></returns>
		public bool GetIsExistEditErrorNoName()
		{
			return GetIsElementDisplay(By.XPath(ERROR_EDIT_NO_NAME_XPATH));
		}

		/// <summary>
		/// Вернуть true, если для машины tmName найден комментарий comment
		/// </summary>
		/// <returns></returns>
		public bool GetIsCommentExist(string comment)
		{
			return GetTextElement(By.XPath(TM_COMMENT_SPAN_XPATH)) == comment;
		}

		/// <summary>
		/// Подтвердить удаление
		/// </summary>
		public void ConfirmTMEdition()
		{
			ClickElement(By.XPath(CONFIRM_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли ошибка о неправильном ТМХ
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsErrorMessageNotTMX()
		{
			return WaitUntilDisplayElement(By.XPath(NO_TMX_FILE_ERROR_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли ошибка при создании ТМ существующего имени
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsExistCreateTMErrorExistName()
		{
			return GetIsElementDisplay(By.XPath(ERROR_CREATE_TM_EXIST_NAME_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли ошибка при создании ТМ пустого имени
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsExistCreateTMErrorNoName()
		{
			return GetIsElementDisplay(By.XPath(ERROR_CREATE_TM_NO_NAME_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли ошибка отсутствия таргета
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsExistCreateTMErrorNoTarget()
		{
			return GetIsElementDisplay(By.XPath(ERROR_CREATE_TM_NO_TARGET_XPATH));
		}

		/// <summary>
		/// Кликнуть на список клиентов на форме редактирования ТМ
		/// </summary>
		public void ClickOpenClientListEditTm()
		{
			ClickElement(By.XPath(TM_EDIT_CLIENT_LIST_XPATH));
		}

		/// <summary>
		///	Выбрать клиента в форме редактирования ТМ
		/// </summary>
		public void EditTmSelectClient(string clientName)
		{
			ClickElement(By.XPath(TM_EDIT_CLIENT_LIST_XPATH + "[@title='" + clientName + "']"));
		}

		/// <summary>
		/// Кликнуть на список топиков на форме редактирования ТМ
		/// </summary>
		public void ClickTopicsListEditTm()
		{
			ClickElement(By.XPath(TM_EDIT_TOPIC_NAME_XPATH));
		}

		/// <summary>
		/// Получить значение топика из формы редактирования ТМ
		/// </summary>
		public string GetTopicFromTmEditionDialog()
		{
			return GetTextElement(By.XPath(TM_EDIT_TOPIC_NAME_XPATH));
		}

		/// <summary>
		///Выбрать топик в форме редактирования TM
		/// </summary>
		public void SelectTopicForTm(string topicName)
		{
			ClickElement(By.XPath("//span[contains(@class, 'ui-treeview_nodetext') and text()='" + topicName + "']"));
		}

		/// <summary>
		/// Очистить все фильтры ТМ если они существуют
		/// </summary>
		public void ClearFiltersPanelIfExist()
		{
			if (GetIsElementExist(By.XPath(CLEAR_FILTERS_XPATH)))
			{
				ClickElement(By.XPath(CLEAR_FILTERS_XPATH));
			}
		}

		/// <summary>
		/// Открыть окно ТМ фильтров
		/// </summary>
		public void OpenTmFilters()
		{
			ClickElement(By.XPath(OPEN_FILTERS_XPATH));
		}

		/// <summary>
		/// Очистить фильтры в окне ТМ фильтров
		/// </summary>
		public void ClearTmFilters()
		{
			ClickElement(By.XPath(CLEAR_FILTERS_IN_DIALOG_XPATH));
		}

		/// <summary>
		/// Применить ТМ фильтры
		/// </summary>
		public void ApplyTmFilters()
		{
			ClickElement(By.XPath(APPLY_FILTERS_BTN));
		}

		/// <summary>
		/// Применить ТМ фильтры
		/// </summary>
		public void CancelTmFiltersCreation()
		{
			ClickElement(By.XPath(CANCEL_FILTERS_BTN));
		}

		/// <summary>
		/// Удалить ТМ фильтр с панели ТМ
		/// </summary>
		public void RemoveTmFilterFromPanel(string fullFilterName)
		{
			ClickElement(By.XPath(string.Format("//div[contains(@title, '{0}')]//em//img", fullFilterName)));
		}

		/// <summary>
		/// Открыть доступные для выбора исходные языки в ТМ фильтрах
		/// </summary>
		public void OpenSourceLanguagesTmFilters()
		{
			ClickElement(By.XPath(SOURCE_LANG_FILTER_XPATH));
		}

		/// <summary>
		/// Открыть доступные для выбора языки перевода ТМ фильтрах
		/// </summary>
		public void OpenTargetLanguagesTmFilters()
		{
			ClickElement(By.XPath(TARGET_LANG_FILTER_XPATH));
		}

		/// <summary>
		/// Открыть доступных авторов в ТМ фильтрах
		/// </summary>
		public void OpenAuthorsTmFilters()
		{
			ClickElement(By.XPath(AUTHOR_FILTER_XPATH));
		}

		/// <summary>
		/// Открыть топики в ТМ фильтрах
		/// </summary>
		public void OpenTopicsTmFilters()
		{
			ClickElement(By.XPath(TOPICS_XPATH));
		}

		/// <summary>
		/// Открыть проектные группы в ТМ фильтрах
		/// </summary>
		public void OpenProjectGroupTmFilters()
		{
			ClickElement(By.XPath(PROJECT_GROUP_FILTER_XPATH));
		}

		/// <summary>
		/// Открыть клиентов в ТМ фильтрах
		/// </summary>
		public void OpenClientsTmFilters()
		{
			ClickElement(By.XPath(CLIENTS_FILTER_XPATH));
		}

		/// <summary>
		/// Задать дату создания в ТМ фильтрах
		/// </summary>
		public void SetCreationDateTmFilters(DateTime creationDate)
		{
			var stringDate = string.Format(@"{0}/{1}/{2}", creationDate.Month, creationDate.Day, creationDate.Year);

			SendTextElement(By.XPath(CREATION_DATE_XPATH), stringDate);
		}

		/// <summary>
		/// Выбрать исходный язык в ТМ фильтрах
		/// </summary>
		public void SelectSourceLanguageTmFilter(LANGUAGE language)
		{
			ClickElement(By.XPath("//input[contains(@name, 'multiselect_SourceLanguages') and contains(@title, '" + language + "')]"));
		}

		/// <summary>
		/// Выбрать язык перевода в ТМ фильтрах
		/// </summary>
		public void SelectTargetLanguageTmFilter(LANGUAGE language)
		{
			ClickElement(By.XPath("//input[contains(@name, 'multiselect_TargetLanguages') and contains(@title, '" + language + "')]"));
		}

		/// <summary>
		/// Выбрать автора в ТМ фильтрах
		/// </summary>
		public void SelectAuthorTmFilter(string authorName)
		{
			ClickElement(By.XPath("//span[contains(@class,'ui-multiselect-item-text') and contains(text(),'" + authorName + "')]"));
		}

		/// <summary>
		/// Выбрать топик в ТМ фильтрах
		/// </summary>
		public void SelectTopicTmFilter(string topicName)
		{
			ClickElement(By.XPath("//span[contains(text(), '" + topicName + "')]/..//span[1]//input"));
		}

		/// <summary>
		/// Выбрать проектную группу в ТМ фильтрах
		/// </summary>
		public void SelectProjectGroupTmFilter(string projectGroupName)
		{
			ClickElement(By.XPath("//span[text()='" + projectGroupName + "']"));
		}

		/// <summary>
		/// Выбрать клиента в ТМ фильтрах
		/// </summary>
		public void SelectClientTmFilter(string clientName)
		{
			ClickElement(By.XPath("//span[text()='" + clientName + "']"));
		}
		
		/// <summary>
		/// Вернуть XPath строки с ТМ
		/// </summary>
		/// <param name="TMName">название ТМ</param>
		/// <returns>xPath</returns>
		protected string GetTMRow(string TMName)
		{
			return TM_ROW_XPATH + "[text()='" + TMName + "']/parent::span/parent::td/parent::tr";
		}

		/// <summary>
		/// Загрузка документ в TM
		/// </summary>
		public void UploadTMInDoc(string DocumentName)
		{
			UploadTM(DocumentName, ADD_TMX);
		}

		/// <summary>
		/// Закрыть все уведомления,которые показываются сейчас
		/// </summary>
		public void CloseAllErrorNotifications()
		{
			var notificationsCount = GetElementList(By.XPath(NOTIFICATION_XPATH + "//span[2]/a")).Count;
			//закрываем сообщения от самого верхнего к нижнему.(по оси z)
			for (int i = notificationsCount; i > 0; i--)
			{
				Logger.Trace("Закрываем сообщение:" + NOTIFICATION_XPATH + "[" + i + "]");
				var currentElement = GetElement(By.XPath(NOTIFICATION_XPATH + "[" + i + "]//span[2]/a"));
				currentElement.Click();
				//даём открыться диалоговому окну загрузки файлов
				System.Threading.Thread.Sleep(3000);
				//отменяем загрузку, закрывая диалоговое окно
				SendKeys.SendWait(@"{Esc}");
			}
		}

		public enum TM_BTN_TYPE { Update, Export, Delete, Add, Edit, Save };

		protected const string ADD_TM_BTN_XPATH = ".//span[contains(@class,'js-create-tm')]";
		protected const string ADD_TMX = "html/body/div[11]/div[2]/div[2]/form/div[1]/div/div/input";
		protected const string CREATE_TM_DIALOG_XPATH = ".//div[contains(@class,'js-popup-create-tm')][2]";
		protected const string CREATE_TM_CLIENT_XPATH = ".//span[contains(@class,'js-client-select') and contains(@class,'js-dropdown')]";
		protected const string CREATE_TM_CLIENT_LIST_XPATH = ".//span[contains(@class,'js-client-select') and contains(@class,'g-drpdwn__list')]";
		protected const string CREATE_TM_CLIENT_ITEM_XPATH = "//span[contains(@class,'js-dropdown__item')]";
		protected const string CREATE_TM_DOMAIN_XPATH = ".//div[contains(@class,'js-domains-multiselect')]";
		protected const string CREATE_TM_DOMAIN_LIST_XPATH = ".//div[contains(@class,'ui-multiselect-menu')][2]";
		protected const string CREATE_TM_DOMAIN_ITEMS_XPATH = ".//div[contains(@class,'ui-multiselect-menu')][2]//ul[contains(@class,'ui-multiselect-checkboxes')]//li//label//span[contains(@class,'ui-multiselect-item-text')]"; // TODO пересмотреть

		protected const string DOWNLOAD_TMX_IMG_PATH = "//img[contains(@class,'js-loading-image')]";

		protected const string CREATE_TM_DIALOG_SAVE_AND_IMPORT_BTN_XPATH = CREATE_TM_DIALOG_XPATH + "//a[contains(@class,'js-save-and-import')]";
		protected const string UPLOAD_BTN_XPATH = "//a[contains(@class,'js-upload-btn')]";

		protected const string CLEAR_FILTERS_XPATH = "//img[contains(@class, 'js-clear-filter')]";
		protected const string OPEN_FILTERS_XPATH = "//span[contains(@class, 'js-set-filter')]";
		protected const string CLEAR_FILTERS_IN_DIALOG_XPATH = "//a[contains(@class, 'js-clear-all')]";
		protected const string APPLY_FILTERS_BTN = "//input[@type='submit' and @value='Apply']";
		protected const string CANCEL_FILTERS_BTN = "//div[contains(@class,'l-filtersrc__section')]//a[contains(text(), 'Cancel')]";
		protected const string SOURCE_LANG_FILTER_XPATH = "//p[text()='Source language']/..//div[contains(@class, 'ui-multiselect')]";
		protected const string TARGET_LANG_FILTER_XPATH = "//p[text()='Target languages']/..//div[contains(@class, 'ui-multiselect')]";
		protected const string PROJECT_GROUP_FILTER_XPATH = "//p[text()='Project groups']/..//div[contains(@class, 'ui-multiselect')]";
		protected const string CLIENTS_FILTER_XPATH = "//p[text()='Clients']/..//div[contains(@class, 'ui-multiselect')]";
		protected const string TOPICS_XPATH = "//p[text()='Topics']/..//div//div//div[1]";
		protected const string AUTHOR_FILTER_XPATH = "//p[text()='Author']/..//div[contains(@class, 'ui-multiselect')]";
		protected const string CREATION_DATE_XPATH = "//input[contains(@class, 'js-from-date')]";
		
		protected const string TM_ROW_XPATH = "//tr[contains(@class, 'js-tm-row')]/td/span/span";

		protected const string BTN_ROW_XPATH = "//tr[@class='js-tm-panel']";
		protected const string UPDATE_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@class,'js-upload-btn')]//a";
		protected const string EXPORT_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@class,'js-export-btn')]//a";
		protected const string DELETE_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@class,'js-delete-btn')]";
		protected const string ADD_TMX_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@class,'js-add-tmx-btn')]";
		protected const string EDIT_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@class,'js-edit-btn')]";
		protected const string SAVE_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@class,'js-save-btn')]";
		// TODO заменить id
		protected const string PROJECT_GROUP_SPAN_XPATH = BTN_ROW_XPATH + "//table[@class='l-tmpanel__table']//div[1]";
		protected const string SEGMENT_SPAN_XPATH = BTN_ROW_XPATH + "//table[@class='l-tmpanel__table']//div[4]";
		protected const string TM_COMMENT_SPAN_XPATH = BTN_ROW_XPATH + "//td[@class='l-tmpanel__td ']";
		protected const string IMPORT_POPUP_XPATH = "//div[contains(@class,'js-popup-import')][2]";
		protected const string IMPORT_BTN_XPATH = IMPORT_POPUP_XPATH + "//span[contains(@class,'js-import-button')]";

		protected const string OPEN_SRC_LANG_CREATE_TM_XPATH = CREATE_TM_DIALOG_XPATH + "//span[contains(@class,'l-createtm__srclnl_drpdwn')]";
		protected const string OPEN_TRG_LANG_CREATE_TM_XPATH = CREATE_TM_DIALOG_XPATH + "//div[contains(@class,'js-languages-multiselect')]";

		protected const string SOURCE_LANG_ITEM_XPATH = "//span[contains(@class,'js-dropdown__item')][@data-id='";
		protected const string TARGET_LANG_ITEM_XPATH = "//div[contains(@class,'ui-multiselect-menu') and contains(@class,'js-languages-multiselect')]//li//input[@value='";

		protected const string PROJECT_TO_ADD_ITEM_XPATH = "//div[contains(@class,'js-domains-multiselect')]//ul//li[2]//label//span[2]";
		protected const string DOMAIN_TO_ADD_XPATH = "//div[contains(@class,'js-domains-multiselect')]//ul//input";

		protected const string NEW_TM_NAME_XPATH = CREATE_TM_DIALOG_XPATH + "//input[contains(@class,'js-tm-name')]";
		protected const string SAVE_TM_BTN_XPATH = CREATE_TM_DIALOG_XPATH + "//span[contains(@class,'js-save')]";
		protected const string CANCEL_TM_SAVING_BTN_XPATH = CREATE_TM_DIALOG_XPATH + "//a[contains(@class,'js-cancel')]";
		
		protected const string TM_ROW_NAME = "//tr[contains(@class,'js-tm-row')]//td/span/span";
		protected const string TM_ROW_LANGUAGES = "//tr[contains(@class,'js-tm-row')]//td[2]/span";
		protected const string TM_EDIT_FORM_XPATH = "//tr[contains(@class,'js-tm-panel js-editing')]";
		protected const string TM_EDIT_NAME_XPATH = TM_EDIT_FORM_XPATH + "//input[contains(@class, 'js-tm-name')]";
		protected const string TM_EDIT_COMMENT_XPATH = TM_EDIT_FORM_XPATH + "//textarea";
		protected const string TM_EDIT_SAVE_BTN_XPATH = TM_EDIT_FORM_XPATH + "//span[contains(@class,'js-save-btn')]";
		protected const string TM_EDIT_TARGET_LANGUAGE = TM_EDIT_FORM_XPATH + "//div[contains(@class,'js-languages-multiselect')]";
		protected const string TM_EDIT_PROJECT = TM_EDIT_FORM_XPATH + "//div[contains(@class,'js-domains-multiselect')]";
		protected const string TM_EDIT_CANCEL = TM_EDIT_FORM_XPATH + "//span[contains(@class,'js-cancel-btn')]";
		protected const string TM_EDIT_CLIENT_LIST_XPATH = "//span[contains(@class, 'js-client-select')]";
		protected const string TM_EDIT_TOPICS_LIST = "//div[contains (@class, 'js-topics')]";
		protected const string TM_EDIT_TOPIC_NAME_XPATH = TM_EDIT_TOPICS_LIST + "//div//div[1]//span//span";
		
		protected const string NOTIFICATION_BALOON_XPATH = "//div[contains(@class,'g-notifications-container')]";
		protected const string NOTIFICATION_BALOON_TEXT_XPATH = NOTIFICATION_BALOON_XPATH + "//span[1]";
		protected const string NOTIFICATION_BALOON_BUTTON_XPATH = NOTIFICATION_BALOON_XPATH + "//span[2]//a";

		protected const string ERROR_EDIT_EXIST_NAME_XPATH = TM_EDIT_FORM_XPATH + ERROR_EXIST_NAME;
		protected const string ERROR_EDIT_NO_NAME_XPATH = TM_EDIT_FORM_XPATH + ERROR_NO_NAME;

		protected const string ERROR_CREATE_TM_EXIST_NAME_XPATH = CREATE_TM_DIALOG_XPATH + ERROR_EXIST_NAME;
		protected const string ERROR_CREATE_TM_NO_NAME_XPATH = CREATE_TM_DIALOG_XPATH + ERROR_NO_NAME;
		protected const string ERROR_CREATE_TM_NO_TARGET_XPATH = CREATE_TM_DIALOG_XPATH + ERROR_NO_TARGET;

		protected const string ERROR_DIV = "//div[contains(@class,'js-dynamic-errors')]";
		protected const string ERROR_EXIST_NAME = ERROR_DIV + "//p[contains(@class,'js-error-from-server') and @data-key='name']";
		protected const string ERROR_NO_NAME = ERROR_DIV + "//p[contains(@class,'js-error-tm-name-required')]";
		protected const string ERROR_NO_TARGET = ERROR_DIV + "//p[contains(@class,'js-error-targetLanguage-required')]";

		protected const string CONFIRM_XPATH = "//div[contains(@class,'js-popup-confirm')]//input[contains(@type,'submit')]";
		protected const string NO_TMX_FILE_ERROR_XPATH = IMPORT_POPUP_XPATH + "//p[contains(@class,'js-error-invalid-file-extension')]";

		protected const string NOTIFICATION_XPATH = "//div[@class='g-notifications-item']";

		protected Dictionary<TM_BTN_TYPE, string> TMButtonDict;
	}
}