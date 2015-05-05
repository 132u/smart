using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using NUnit.Framework;
using System.Threading;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	/// <summary>
	/// Хелпер страницы ТМ
	/// </summary>
	public class TMPageHelper : CommonHelper
	{
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

		public void WaitPageLoad()
		{
			Logger.Debug("Ожидание загрузки страницы");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(ADD_TM_BTN_XPATH)),
				"Ошибка: страница не загрузилась");
		}

		public void ClickCreateTM()
		{
			Logger.Debug("Нажатие кнопки создания ТМ");
			ClickElement(By.XPath(ADD_TM_BTN_XPATH));
			
			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(CREATE_TM_DIALOG_XPATH)),
				"Ошибка: форма создания ТМ не загрузилась");
		}

		public void ClickOpenClientListCreateTM()
		{
			Logger.Debug("Нажатие кнопки открытия списка клиентов");
			ClickElement(By.XPath(CREATE_TM_CLIENT_XPATH));

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(CREATE_TM_CLIENT_LIST_XPATH)),
				"Ошибка: список клиентов не открылся");
		}

		public void ClickOpenDomainListCreateTM()
		{
			Logger.Debug("Нажатие кнопки открытия списка domain");
			ClickElement(By.XPath(CREATE_TM_DOMAIN_XPATH));

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(CREATE_TM_DOMAIN_LIST_XPATH)),
				"Ошибка: список domain не открылся");
		}

		public bool GetIsClientExistCreateTM(string clientName)
		{
			Logger.Debug(string.Format("Вернуть: есть ли клиент {0} в списке клиентов при создании ТМ", clientName));
			var clientList = GetElementList(By.XPath(CREATE_TM_CLIENT_ITEM_XPATH));

			return clientList.Any(e => e.GetAttribute("innerHTML") == clientName);
		}

		public bool GetIsDomainExistCreateTM(string domainName)
		{
			Logger.Debug(string.Format("Вернуть: есть ли домен {0} в списке доменов при создании ТМ", domainName));

			var domains = GetElementList(By.XPath(CREATE_TM_DOMAIN_ITEMS_XPATH));
			var isDomainExist = false;

			foreach (var domain in domains)
			{
				if (domain.Text == domainName)
				{
					// Если проект в списке
					isDomainExist = true;
					break;
				}
			}

			return isDomainExist;
		}

		public void AssertionDocumentDownloadFinish()
		{
			Logger.Debug("Ожидание загрузки документа");

			var isDisappeared = true;

			if (GetIsElementDisplay(By.XPath(DOWNLOAD_TMX_IMG_PATH)))
			{
				isDisappeared = false;
				for (var i = 0; i < 5; ++i)
				{
					isDisappeared = WaitUntilDisappearElement(By.XPath(DOWNLOAD_TMX_IMG_PATH), 40);
					if (isDisappeared)
					{
						break;
					}
					Driver.Navigate().Refresh();
				}
			}

			Assert.IsTrue(isDisappeared, "Ошибка: документ загружается слишком долго");
		}

		public void ClickSave()
		{
			Logger.Debug("Кликнуть Сохранить и загрузить документ в диалоге создания ТМ");
			ClickElement(By.XPath(SAVE_TM_BTN_XPATH));
		}

		public void WaitUntilUploadDialog()
		{
			Logger.Debug("Ожидание открытия диалога загрузки документа");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(UPLOAD_BTN_XPATH)), 
				"Ошибка: диалог загрузки документа не загрузился");
		}

		public void ClickAddUploadBtn()
		{
			Logger.Debug("Кликнуть Add в диалоге загрузки документа");
			ClickElement(By.XPath(UPLOAD_BTN_XPATH));
		}

		public bool GetIsTMOpened(string tmName)
		{
			Logger.Trace(string.Format("Получить: открыта ли информация о ТМ {0}", tmName));

			return GetElementClass(By.XPath(GetTMRow(tmName))).Contains("opened");
		}

		public void ClickTMRow(string tmName)
		{
			Logger.Debug(string.Format("Кликнуть по строке с ТМ {0}", tmName));
			ClickElement(By.XPath(GetTMRow(tmName)));
		}

		public void ScrollToRequiredTm(string tmName)
		{
			Logger.Debug(string.Format("Прокрутка до необходимой ТМ {0}", tmName));
			ScrollToElement(By.XPath(GetTMRow(tmName)));
		}

		public void ClickTMButton(TM_BTN_TYPE btnType)
		{
			Logger.Debug(string.Format("Кликнуть кнопку {0} в информации о ТМ.", btnType));
			ClickElement(By.XPath(TMButtonDict[btnType]));
		}

		public int GetSegmentCount()
		{
			Logger.Trace("Получить количество сегментов");

			var segmentText = GetTextElement(By.XPath(SEGMENT_SPAN_XPATH));
			Logger.Trace(string.Format("Полученный текст из сегмента: {0}", segmentText));
			// Нужно получить число сегментов из строки "Number of translation units: N", разделитель - ":"
			var splitIndex = segmentText.IndexOf(":");
			// Отступаем двоеточие и пробел
			splitIndex += 2;
			if (segmentText.Length > splitIndex)
			{
				segmentText = segmentText.Substring(splitIndex);
			}

			// Получить число сегментов из строки
			return ParseStrToInt(segmentText);
		}

		public bool GetIsProjectExistInTmInformation(string projectName)
		{
			Logger.Debug(string.Format("Проверка наличия проекта {0} в информации о ТМ", projectName));
			var projectText = GetTextElement(By.XPath(PROJECT_GROUP_SPAN_XPATH));
			Logger.Trace("Полученные группы проектов:" + projectText + "; Ожидаемые группы проектов:" + projectName);

			return projectText == projectName;
		}

		public void ClickImportBtn()
		{
			Logger.Debug("Кликнуть кнопку импорт");
			ClickElement(By.XPath(IMPORT_BTN_XPATH));
		}

		public void ClickOpenSourceLangList()
		{
			Logger.Debug("Кликнуть кнопку открытия списка языков Source");
			ClickElement(By.XPath(OPEN_SRC_LANG_CREATE_TM_XPATH));
		}

		public void SelectSourceLanguage(LANGUAGE lang)
		{
			Logger.Debug(string.Format("Выбрать язык Source {0}", lang));
			var xPath = SOURCE_LANG_ITEM_XPATH + languageID[lang] + "']";
			ClickElement(By.XPath(xPath));
		}

		public void ClickTargetLangList()
		{
			Logger.Debug("Кликнуть открыть/закрыть список target");
			ClickElement(By.XPath(OPEN_TRG_LANG_CREATE_TM_XPATH));
		}

		public void SelectTargetLanguage(LANGUAGE lang)
		{
			Logger.Debug(string.Format("Выбрать язык Target {0}", lang));
			var xPath = TARGET_LANG_ITEM_XPATH + languageID[lang] + "']";
			ClickElement(By.XPath(xPath));
		}

		public void InputNewTMName(string name)
		{
			Logger.Debug("Ввести имя новой ТМ");
			ClearAndAddText(By.XPath(NEW_TM_NAME_XPATH), name);
		}

		public bool GetIsCreateTMInputNameError()
		{
			Logger.Debug("Вернуть, отмечено ли поле имя в форме создания ТМ ошибкой");

			return GetElementClass(By.XPath(NEW_TM_NAME_XPATH)).Contains("error");
		}

		public void InputInTMSearch(string tmName)
		{
			Logger.Trace("Ввод названия ТМ в поиске");
			ClearAndAddText(By.XPath(SEARCH), tmName);
		}

		public void ClearSearchField()
		{
			Logger.Trace("Очистить поле поиска");
			ClearElement(By.XPath(SEARCH));
		}

		public void ClickSearchBtn()
		{
			Logger.Trace("Нажать кнопку поиска (лупа)");
			ClickElement(By.XPath(SEARCH_BTN));
		}

		public void ClickSaveNewTM()
		{
			Logger.Debug("Нажать кнопку Сохранить");
			ClickElement(By.XPath(SAVE_TM_BTN_XPATH));
		}

		public void ClickCancelSavingNewTM()
		{
			Logger.Debug("Нажать кнопку отмены сохранения новой ТМ");
			ClickElement(By.XPath(CANCEL_TM_SAVING_BTN_XPATH));
		}

		public void ClickCancelButtonOnNotificationBaloon()
		{
			Logger.Debug("Нажать кнопку Cancel на информационной плашке");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(NOTIFICATION_BALOON_BUTTON_XPATH)),
				"Ошибка: информационная плашка не была найдена");
			
			ClickElement(By.XPath(NOTIFICATION_BALOON_BUTTON_XPATH));
		}

		public void ClickCanselOnEditionForm()
		{
			Logger.Debug("Нажать кнопку Cancel на форме редактирования ТМ");
			ClickElement(By.XPath(TM_EDIT_CANCEL));
		}

		public bool GetIsExistTM(string tmName)
		{
			Logger.Debug(string.Format("Вернуть, есть ли ТМ {0} в списке", tmName));
			// TODO проверить мб проверить циклом
			return GetIsElementExist(By.XPath(TM_ROW_NAME + "//span[text()='" + tmName + "']"));
		}

		public void AssertionIsCorrectLanguagesForTm(
			string tmName,
			string sourceLanguage,
			string[] targetLanguages)
		{
			var formattedLanguagesString = string.Concat(sourceLanguage, " > ", string.Join(", ", targetLanguages));

			Logger.Debug(string.Format("Проверить, указанны ли для ТМ {0} корректные языки {1}", tmName, formattedLanguagesString));

			Assert.IsTrue(GetTextElement(By.XPath(GetTMRow(tmName))).Contains(formattedLanguagesString),
				"Ошибка: для ТМ неверно отображены исходный язык и язык перевода.");
		}

		public void AssertionIsInformationBaloonNotExist()
		{
			Logger.Trace("Проверка отсутствия информационной плашки");

			Assert.IsFalse(GetIsElementExist(By.XPath(NOTIFICATION_BALOON_TEXT_XPATH)),
				"Ошибка: плашка с информацией о загружаемых ТU не закрыта.");
		}

		public bool IsTextExistInBaloon(string text)
		{
			Logger.Debug(string.Format("Вернуть, есть ли плашка с текстом {0} в окне браузера", text));

			return WaitUntilDisplayElement(
				By.XPath(getNotificationTextXPath(text)), 45);
		}

		public void AssertionBaloonWithSpecificMessageDisappear(string text)
		{
			Logger.Trace(string.Format("Проверить, исчезла ли плашка с текстом {0} из окна браузера", text));

			Assert.IsTrue(WaitUntilDisappearElement(By.XPath(getNotificationTextXPath(text)),60),
				"Ошибка: информационная плашка не исчезла из окна браузера");
		}

		public void OpenCreateTMDialog()
		{
			Logger.Debug("Нажать кнопку открытия диалога создания ТМ");
			ScrollToElement(By.XPath(ADD_TM_BTN_XPATH));
			ClickElement(By.XPath(ADD_TM_BTN_XPATH));
			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(CREATE_TM_DIALOG_XPATH)), 
				"Ошибка: не открылась форма создания ТМ");
		}

		public void AssertionEditTMFormIsOpen()
		{
			Logger.Trace("Дождаться открытия формы редактирования ТМ");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(TM_EDIT_FORM_XPATH)),
				"Ошибка: форма редактирования ТМ не была открыта");
		}

		public void EditTMClearName()
		{
			Logger.Trace("Очистка имени в форме изменения ТМ");
			ClearElement(By.XPath(TM_EDIT_NAME_XPATH));
		}

		public void ClickToProjectsListAtTmEdditForm()
		{
			Logger.Debug("Кликнуть на поле с проектом в форме редактирования ТМ");
			ClickElement(By.XPath(TM_EDIT_PROJECT));
		}

		public bool IsAnyProjectGroupExist()
		{
			Logger.Trace("Вернуть, если при редактировании проекта доступна хотя бы одна проектная группа");
			return GetIsElementExist(By.XPath(PROJECT_TO_ADD_ITEM_XPATH));
		}

		public string EditTMAddProject()
		{
			Logger.Debug("Добавить первую проектную группу в проекте и вернуть ее имя");
			ClickElement(By.XPath(PROJECT_TO_ADD_ITEM_XPATH));

			return GetTextElement(By.XPath(PROJECT_TO_ADD_ITEM_XPATH));
		}

		public void EditTMAddProject(string projectName)
		{
			Logger.Debug(string.Format("Добавить проект {0} к ТМ", projectName));
			ClickElement(By.XPath(DOMAIN_TO_ADD_XPATH + "//li//span[text()='" + projectName + "']"));
		}

		public void EditTMClearComment()
		{
			Logger.Debug("Очистить комментарий в форме изменения ТМ");

			if (GetIsElementExist(By.XPath(TM_EDIT_COMMENT_XPATH)))
			{
				ClearElement(By.XPath(TM_EDIT_COMMENT_XPATH));
			}
		}

		public void ClickToTargetLanguagesAtTmEdditForm()
		{
			Logger.Debug("Кликнуть на поле с языком перевода в форме редактирования ТМ");
			ClickElement(By.XPath(TM_EDIT_TARGET_LANGUAGE));
		}

		public void InputEditTMName(string tmName)
		{
			Logger.Debug(string.Format("Ввести имя {0} в форме редактирования ТМ", tmName));
			SendTextElement(By.XPath(TM_EDIT_NAME_XPATH), tmName);
		}

		public void InputEditTMComment(string tmComment)
		{
			Logger.Debug(string.Format("Ввести комменатрий {0} в форме редактирования ТМ", tmComment));
			SendTextElement(By.XPath(TM_EDIT_COMMENT_XPATH), tmComment);
		}

		public string GetProjectGroupNameForTm()
		{
			Logger.Trace("Получить имя проектной группы для ТМ");

			return GetTextElement(By.XPath(PROJECT_GROUP_FIELD));
		}

		public void ClickEditSaveBtn()
		{
			Logger.Debug("Кликнуть кнопку сохранить в форме редактирования");
			ClickElement(By.XPath(TM_EDIT_SAVE_BTN_XPATH));
		}

		public void AssertionIsErrorExistingNameAppear()
		{
			Logger.Debug("Проверить, что появилось предупреждение о существующем имени при редактировании ТМ");

			Assert.IsTrue(GetIsElementDisplay(By.XPath(ERROR_EDIT_EXIST_NAME_XPATH)),
				"Ошибка: не появилось сообщение о существующем имени при редактировании ТМ");
		}

		public void AssertionIsExistEditErrorNoNameAppear()
		{
			Logger.Trace("Проверить наличие ошибки при некорректном редактировании имени ТМ");

			Assert.IsTrue(GetIsElementDisplay(By.XPath(ERROR_EDIT_NO_NAME_XPATH)),
				"Ошибка: не появилось сообщение об ошибке в имени при редактировании ТМ");
		}

		public bool GetIsCommentExist(string comment)
		{
			Logger.Debug(string.Format("Вернуть, есть ли на странице комментарий {0}", comment));

			return GetElementAttribute(By.XPath(TM_EDIT_COMMENT_XPATH), "value") == comment;
		}

		public void ConfirmTMEdition()
		{
			Logger.Debug("Нажать кнопку подтверждения редактирования ТМ");
			ClickElement(By.XPath(CONFIRM_XPATH));
		}

		public bool GetConfirmWindowExist()
		{
			return GetIsElementExist(By.XPath(CONFIRM_WINDOW));
		}

		public bool GetIsErrorMessageNotTmx()
		{
			Logger.Trace("Запрос на наличие ошибки о неправильном TMX");

			return WaitUntilDisplayElement(By.XPath(NO_TMX_FILE_ERROR_XPATH));
		}

		public void AssertionIsExistNameErrorAppearDuringTmCreation()
		{
			Logger.Trace("Проверка наличия ошибки при создании ТМ с существующим именем");

			Assert.IsTrue(GetIsElementDisplay(By.XPath(ERROR_CREATE_TM_EXIST_NAME_XPATH)),
				"Ошибка: не появилась ошибка создания ТМ с существующим именем");
		}

		public void AssertionNoNameErrorAppearDuringTmCreation()
		{
			Logger.Trace("Проверить, что при создании ТМ с пустым именем появилась ошибка");

			Assert.IsTrue(GetIsElementDisplay(By.XPath(ERROR_CREATE_TM_NO_NAME_XPATH)),
				"Ошибка: не появилось сообщение об ошибке при создании ТМ с пустым именем");
		}

		public void AssertionIsNoTargetErrorAppearDuringTmCreation()
		{
			Logger.Trace("Проверить наличие об ошибке отсутствия таргета");
			Assert.IsTrue(GetIsElementDisplay(By.XPath(ERROR_CREATE_TM_NO_TARGET_XPATH)),
				"Ошибка: не появилось сообщение об ошибке отсутствия таргета");
		}

		public void ClickOpenClientListEditTm()
		{
			Logger.Debug("Кликнуть на список клиентов на форме редактирования ТМ");
			ClickElement(By.XPath(TM_EDIT_CLIENT_LIST_XPATH));
		}

		public void EditTmSelectClient(string clientName)
		{
			Logger.Debug(string.Format("Выбрать клиента {0} в форме редактирования ТМ", clientName));
			ClickElement(By.XPath(TM_EDIT_CLIENT_LIST_XPATH + "//span[@title='" + clientName + "']"));
		}

		public void ClickTopicsListEditTm()
		{
			Logger.Debug("Кликнуть на список топиков на форме редактирования ТМ");
			ClickElement(By.XPath(TM_EDIT_TOPICS_LIST));
		}

		public string GetTopicFromTmEditionDialog()
		{
			Logger.Debug("Получить значение топика из формы редактирования ТМ");

			return GetTextElement(By.XPath(TM_EDIT_TOPIC_NAME_XPATH));
		}

		public void SelectTopicForTm(string topicName)
		{
			Logger.Debug(string.Format("Выбрать топик {0} в форме редактирования TM", topicName));
			ClickElement(By.XPath("//span[contains(@class, 'ui-treeview_nodetext') and text()='" + topicName + "']"));
		}

		public void ClearFiltersPanelIfExist()
		{
			Logger.Debug("Очистить все фильтры ТМ, если они существуют");

			if (GetIsElementExist(By.XPath(CLEAR_FILTERS_XPATH)))
			{
				ClickElement(By.XPath(CLEAR_FILTERS_XPATH));
			}
		}

		public void OpenTmFilters()
		{
			Logger.Debug("Нажать кнопку открытия окна ТМ фильтров");
			ClickElement(By.XPath(OPEN_FILTERS_XPATH));
		}

		public void ClearTmFilters()
		{
			Logger.Debug("Нажать кнопку отчистки фильтров в окне ТМ фильтров");
			ClickElement(By.XPath(CLEAR_FILTERS_IN_DIALOG_XPATH));
		}

		public void ApplyTmFilters()
		{
			Logger.Debug("Нажать кнопку применения ТМ фильтров");
			ClickElement(By.XPath(APPLY_FILTERS_BTN));
		}

		public void CancelTmFiltersCreation()
		{
			Logger.Debug("Нажать кнопку отмены применения ТМ фильтров");
			ClickElement(By.XPath(CANCEL_FILTERS_BTN));
		}

		public void RemoveTmFilterFromPanel(string fullFilterName)
		{
			Logger.Debug(string.Format("Удалить фильтр {0} c панели ТМ", fullFilterName));
			ClickElement(By.XPath(string.Format("//div[contains(@title, '{0}')]//em//img", fullFilterName)));
		}

		public void ClickSourceLanguagesTmFilters()
		{
			Logger.Debug("Нажать кнопку открытия доступных для выбора исходных языков в ТМ фильтрах");
			ClickElement(By.XPath(SOURCE_LANG_FILTER_XPATH));
		}

		public void ClickTargetLanguagesTmFilters()
		{
			Logger.Debug("Нажать кнопку открытия доступных для выбора языков перевода в ТМ фильтрах");
			ClickElement(By.XPath(TARGET_LANG_FILTER_XPATH));
		}

		public void OpenAuthorsTmFilters()
		{
			Logger.Debug("Нажать кнопку открытия доступных авторов в ТМ фильтрах");
			ClickElement(By.XPath(AUTHOR_FILTER_XPATH));
		}

		public void ClickTopicsTmFilters()
		{
			Logger.Debug("Нажать кнопку открытия топиков в ТМ фильтрах");
			ClickElement(By.XPath(TOPICS_XPATH));
		}

		public void ClickProjectGroupTmFilters()
		{
			Logger.Debug("Нажать кнопку открытия проектных групп в ТМ фильтрах");
			ClickElement(By.XPath(PROJECT_GROUP_FILTER_XPATH));
		}

		public void ClickClientsTmFilters()
		{
			Logger.Debug("Нажать кнопку открытия клиентов в ТМ фильтрах");
			ClickElement(By.XPath(CLIENTS_FILTER_XPATH));
		}

		public void SetCreationDateTmFilters(DateTime creationDate)
		{
			var stringDate = string.Format(@"{0}/{1}/{2}", creationDate.Month, creationDate.Day, creationDate.Year);
			Logger.Debug(string.Format("Задать дату создания в ТМ фильтрах. Дата: {0}", stringDate));
			SendTextElement(By.XPath(CREATION_DATE_XPATH), stringDate);
		}

		public void SelectSourceLanguageTmFilter(LANGUAGE language)
		{
			Logger.Debug(string.Format("Выбрать исходный язык {0} в ТМ фильтрах", language));
			ClickElement(By.XPath("//input[contains(@name, 'multiselect_SourceLanguages') and contains(@title, '" + language + "')]"));
		}

		public void SelectTargetLanguageTmFilter(LANGUAGE language)
		{
			Logger.Debug(string.Format("Выбрать язык перевода {0} в ТМ фильтрах", language));
			ClickElement(By.XPath("//input[contains(@name, 'multiselect_TargetLanguages') and contains(@title, '" + language + "')]"));
		}

		public void SelectAuthorTmFilter(string authorName)
		{
			Logger.Debug(string.Format("Выбрать автора {0} в ТМ фильтрах", authorName));
			ClickElement(By.XPath("//span[contains(@class,'ui-multiselect-item-text') and contains(text(),'" + authorName + "')]"));
		}

		public void SelectTopicTmFilter(string topicName)
		{
			Logger.Debug(string.Format("Выбрать топик {0} в ТМ фильтрах", topicName));
			ClickElement(By.XPath("//span[contains(text(), '" + topicName + "')]/..//span[1]//input"));
		}

		public void SelectProjectGroupTmFilter(string projectGroupName)
		{
			Logger.Debug(string.Format("Выбрать проектную группу {0} в ТМ фильтрах", projectGroupName));
			ClickElement(By.XPath("//span[text()='" + projectGroupName + "']"));
		}

		public void SelectClientTmFilter(string clientName)
		{
			Logger.Debug(string.Format("Выбрать клиента {0} в ТМ фильтрах", clientName));
			ClickElement(By.XPath("//span[text()='" + clientName + "']"));
		}
		
		protected string GetTMRow(string tmName)
		{
			Logger.Debug(string.Format("Вернуть XPath строки с ТМ. Имя ТМ: {0}", tmName));

			return TM_ROW_XPATH + "[text()='" + tmName + "']/parent::td/parent::tr";
		}

		public void UploadTMInDoc(string documentName)
		{
			Logger.Debug(string.Format("Загрузка документа {0} в ТМ", documentName));
			UploadTM(documentName, ADD_TMX);
		}

		public void UploadTMXInUpdatePopUp(string documentName)
		{
			Logger.Debug(string.Format("Загрузка ТМХ документа {0} в настройках ТМ (update pop-up)", documentName));

			Assert.IsTrue(GetIsElementExist(By.XPath(ADD_TMX)), "Ошибка: элемент input для загрузки TMX в TM настройках не найден, возможно xpath поменялся");

			((IJavaScriptExecutor)Driver).ExecuteScript("$(\"input:file\").removeClass(\"g-hidden\").css(\"opacity\", 100)");
			
			Driver.FindElement(By.XPath(ADD_TMX)).SendKeys(documentName);
			
			((IJavaScriptExecutor)Driver).ExecuteScript("document.getElementsByClassName('g-iblock g-bold l-editgloss__filelink js-filename-link')[0].innerHTML= '"
				+ Path.GetFileName(documentName) + "';");
		}

		public void UploadTmxInCreateDialog(string documentName)
		{
			Logger.Debug(string.Format("Загрузка ТМX документа {0} во время создания создания ТМ на странице TranslationMemories", documentName));
			UploadTM(documentName, ADD_TMX_IN__CREATE_TM_DIALOG);
		}

		public void CloseAllErrorNotifications()
		{
			Logger.Debug("Закрыть все показанные уведомления");
			WaitUntilDisplayElement(By.XPath(NOTIFICATION_XPATH +"//span[2]/a" ));
			var notificationsCount = GetElementList(By.XPath(NOTIFICATION_XPATH + "//span[2]/a")).Count;

			//закрываем сообщения от самого верхнего к нижнему.(по оси z)
			for (var i = notificationsCount; i > 0; i--)
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

		private string getNotificationTextXPath(string text)
		{
			return NOTIFICATION_BALOON_TEXT_XPATH + "[text()='" + text + "']";
		}

		public enum TM_BTN_TYPE { Update, Export, Delete, Add, Edit, Save };

		protected const string ADD_TM_BTN_XPATH = "//span[contains(@data-bind,'createTm')]";
		protected const string ADD_TMX = "//div[@class=\"g-popup-bd js-popup-bd js-popup-import\"][2]//div[@class=\"g-popupbox l-filtersrc\"]//input[@type=\"file\"]";
		protected const string ADD_TMX_IN__CREATE_TM_DIALOG = ".//div[contains(@class,\"js-popup-create-tm\")][2]//input[@type=\"file\"]";
		protected const string CREATE_TM_DIALOG_XPATH = ".//div[contains(@class,'js-popup-create-tm')][2]";
		protected const string CREATE_TM_CLIENT_XPATH = "//select[contains(@data-bind,'allClientsList')]//following-sibling::span";
		protected const string CREATE_TM_CLIENT_LIST_XPATH = CREATE_TM_CLIENT_XPATH +"[contains(@class,'active')]";
		protected const string CREATE_TM_CLIENT_ITEM_XPATH = "//select[contains(@data-bind,'allClientsList')]/option";
		protected const string CREATE_TM_DOMAIN_XPATH = ".//div[contains(@class,'ui-multiselect-text')]//span[contains(text(), 'Select project group')]";
		protected const string CREATE_TM_DOMAIN_LIST_XPATH = ".//div[contains(@class,'ui-multiselect-menu')][2]";
		protected const string CREATE_TM_DOMAIN_ITEMS_XPATH = ".//div[contains(@class,'ui-multiselect-menu')][2]//ul[contains(@class,'ui-multiselect-checkboxes')]//li//label//span[contains(@class,'ui-multiselect-item-text')]"; // TODO пересмотреть

		protected const string DOWNLOAD_TMX_IMG_PATH = "//img[contains(@class,'js-loading-image')]";

		protected const string CREATE_TM_DIALOG_SAVE_BTN_XPATH = CREATE_TM_DIALOG_XPATH + "//a[contains(text(),'Save')]";
		protected const string UPLOAD_BTN_XPATH = "//div[@class='g-popup-bd js-popup-bd js-popup-import']//a[contains(@class,'js-upload-btn')]";
		protected const string ADD_BTN_IN_UPLOAD_POP_UP = "//div[@class='g-popup-bd js-popup-bd js-popup-import']//a[contains(text(), 'Add')]";
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
		
		protected const string TM_ROW_XPATH = "//td[@class='l-corpr__td tm']/span";

		protected const string BTN_ROW_XPATH = "//tr[@class='js-tm-panel']";
		protected const string UPDATE_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@data-bind, 'switchToEditing')]";
		protected const string EXPORT_BTN_XPATH = BTN_ROW_XPATH + "//a[contains(@href, 'Export')]";
		protected const string DELETE_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@data-bind, 'deleteTranslationMemory')]";
		protected const string ADD_TMX_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@data-bind, 'appendImportFile')]";
		protected const string EDIT_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@data-bind, 'switchToEditing')]";
		protected const string SAVE_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@data-bind, 'save')]";
		// TODO заменить id
		protected const string PROJECT_GROUP_SPAN_XPATH = BTN_ROW_XPATH + "//div[contains(@data-bind,'domainNames')]";
		protected const string SEGMENT_SPAN_XPATH = BTN_ROW_XPATH + "//table[@class='l-tmpanel__table']//div[4]";
		protected const string IMPORT_POPUP_XPATH = "//div[contains(@class,'js-popup-import')][2]";
		protected const string IMPORT_BTN_XPATH = IMPORT_POPUP_XPATH + "//span[contains(@class,'js-import-button')]";

		protected const string OPEN_SRC_LANG_CREATE_TM_XPATH = CREATE_TM_DIALOG_XPATH + "//select[contains(@data-bind,'SourceLanguagesList')]/following-sibling::span";
		protected const string OPEN_TRG_LANG_CREATE_TM_XPATH = CREATE_TM_DIALOG_XPATH + "//select[contains(@data-bind,'TargetLanguagesList')]/following-sibling::div";
		protected const string SOURCE_LANG_ITEM_XPATH = "//span[contains(@class,'js-dropdown__item')][@data-id='";
		protected const string TARGET_LANG_DD_XPATH = CREATE_TM_DIALOG_XPATH + "//select[contains(@data-watermark,'Select language')]";

		protected const string TARGET_LANG_LIST_XPATH =CREATE_TM_DIALOG_XPATH + "//select[contains(@data-watermark,'Select language')]/option";

		protected const string PROJECT_TO_ADD_ITEM_XPATH = "//div[contains(@class,'ui-multiselect')]//ul//li[2]//label//span[2]";
		protected const string DOMAIN_TO_ADD_XPATH = "//div[contains(@class, 'ui-multiselect-menu')][1]//ul[contains(@class, 'multiselect-checkboxes')]";

		protected const string NEW_TM_NAME_XPATH = CREATE_TM_DIALOG_XPATH + "//input[contains(@data-bind,'name')]";
		protected const string SAVE_TM_BTN_XPATH = CREATE_TM_DIALOG_XPATH + "//span[contains(@data-bind, 'save')]";
		protected const string CANCEL_TM_SAVING_BTN_XPATH = CREATE_TM_DIALOG_XPATH + "//a[text()='Cancel']";

		protected const string TM_ROW_NAME = "//tr[@class='l-corpr__trhover clickable']";
		protected const string TM_ROW_LANGUAGES = "//tr[contains(@class,'js-tm-row')]//td[2]/span";
		protected const string TM_EDIT_FORM_XPATH = "//tr[contains(@class,'js-tm-panel')]";
		protected const string TM_EDIT_NAME_XPATH = TM_EDIT_FORM_XPATH + "//input[contains(@data-bind, 'value: name')]";
		protected const string TM_EDIT_COMMENT_XPATH = TM_EDIT_FORM_XPATH + "//textarea";
		protected const string TM_EDIT_SAVE_BTN_XPATH = TM_EDIT_FORM_XPATH + "//span[contains(@data-bind,'click: save')]";
		protected const string TM_EDIT_TARGET_LANGUAGE = TM_EDIT_FORM_XPATH + "//td[2]//div[1]//div[contains(@class,'ui-multiselect')]/div";
		protected const string TM_EDIT_PROJECT = TM_EDIT_FORM_XPATH + "//td[2]//div[3]//div[contains(@class,'ui-multiselect')]/div";
		protected const string TM_EDIT_CANCEL = TM_EDIT_FORM_XPATH + "//span[contains(@class,'js-cancel-btn')]";
		protected const string TM_EDIT_CLIENT_LIST_XPATH = "//select[contains(@data-bind, 'allClientsList')]/following::span[contains(@class, 'dropdown')]";
		protected const string TM_EDIT_TOPICS_LIST = "//tr[contains(@class, 'tm-panel')]//div[contains(@data-bind, 'topicDropdown')]";
		protected const string TM_EDIT_TOPIC_NAME_XPATH = TM_EDIT_TOPICS_LIST + "//div[contains(@class, 'treeview_mainWidget')]";
		
		protected const string NOTIFICATION_BALOON_XPATH = "//div[contains(@class,'g-notifications-container')]";
		protected const string NOTIFICATION_BALOON_TEXT_XPATH = NOTIFICATION_BALOON_XPATH + "//span[1]";
		protected const string NOTIFICATION_BALOON_BUTTON_XPATH = NOTIFICATION_BALOON_XPATH + "//span[2]//a";

		protected const string ERROR_EDIT_EXIST_NAME_XPATH = TM_EDIT_FORM_XPATH + ERROR_EXIST_NAME_EDITION_PANEL;
		protected const string ERROR_EDIT_NO_NAME_XPATH = TM_EDIT_FORM_XPATH + ERROR_NO_NAME_EDITION_PANEL;

		protected const string ERROR_CREATE_TM_EXIST_NAME_XPATH = CREATE_TM_DIALOG_XPATH + ERROR_EXIST_NAME_CREATION_PANEL;
		protected const string ERROR_CREATE_TM_NO_NAME_XPATH = CREATE_TM_DIALOG_XPATH + ERROR_NO_NAME_CREATION_PANEL;
		protected const string ERROR_CREATE_TM_NO_TARGET_XPATH = CREATE_TM_DIALOG_XPATH + ERROR_NO_TARGET_CREATION_PANEL;

		protected const string ERROR_EXIST_NAME_TEXT = "//p[contains(text(),'The name should be unique.')]";
		protected const string ERROR_NO_NAME_TEXT = "//p[contains(@data-message-id, 'name-required')]";
		protected const string ERROR_NO_TARGET_TEXT = "//p[contains(@data-message-id,'target-language-required')]";

		protected const string ERROR_DIV_CREATION_PANEL = "//div[contains(@class,'createtm__error')]";
		protected const string ERROR_DIV_EDITION_PANEL = "//div[contains(@class,'tmpanel__error')]";
		protected const string ERROR_EXIST_NAME_CREATION_PANEL = ERROR_DIV_CREATION_PANEL + ERROR_EXIST_NAME_TEXT;
		protected const string ERROR_NO_NAME_CREATION_PANEL = ERROR_DIV_CREATION_PANEL + ERROR_NO_NAME_TEXT;
		protected const string ERROR_NO_TARGET_CREATION_PANEL = ERROR_DIV_CREATION_PANEL + ERROR_NO_TARGET_TEXT;
		protected const string ERROR_EXIST_NAME_EDITION_PANEL = ERROR_DIV_EDITION_PANEL + ERROR_EXIST_NAME_TEXT;
		protected const string ERROR_NO_NAME_EDITION_PANEL = ERROR_DIV_EDITION_PANEL + ERROR_NO_NAME_TEXT;

		protected const string NO_TMX_FILE_ERROR_XPATH = CREATE_TM_DIALOG_XPATH + ERROR_DIV_CREATION_PANEL + "//p[contains(@data-message-id,'invalid-file-extension')]";

		protected const string CONFIRM_XPATH = CONFIRM_WINDOW + "//input[contains(@type,'submit')]";

		protected const string NOTIFICATION_XPATH = "//div[@class='g-notifications-item']";

		protected Dictionary<TM_BTN_TYPE, string> TMButtonDict;

		protected const string TARGET_LANG_ITEM_XPATH =
			"//div[contains(@class,'ui-multiselect')]//ul[@class='ui-multiselect-checkboxes ui-helper-reset']//li//input[@value='";
		protected const string CONFIRM_WINDOW = "//div[@class='g-popupbox l-confirm']";
		protected const string SEARCH = "//input[contains(@class, 'search-tm')]";
		protected const string SEARCH_BTN = "//a[contains(@class, 'search-by-name')]";
		protected const string PROJECT_GROUP_FIELD = "//select[contains(@data-bind, 'allDomainsList')]/following::div[contains(@class, 'multiselect-choice-block')]";
	}
}