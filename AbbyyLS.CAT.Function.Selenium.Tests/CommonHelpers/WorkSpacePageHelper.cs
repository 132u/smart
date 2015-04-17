using System;
using System.Collections.Generic;
using NLog;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы workspace
	/// </summary>
	public class WorkSpacePageHelper : MainHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public WorkSpacePageHelper(IWebDriver driver, WebDriverWait wait) 
			: base(driver, wait)
		{
			exportTypeDict = new Dictionary<EXPORT_TYPE, string>
			{
				{EXPORT_TYPE.Original, EXPORT_TYPE_NAME_ORIGINAL},
				{EXPORT_TYPE.TMX, EXPORT_TYPE_NAME_TMX},
				{EXPORT_TYPE.Translated, EXPORT_TYPE_NAME_TRANSLATED}
			};
		}

		/// <summary>
		/// Дождаться загрузки страницы
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitPageLoad()
		{
			Logger.Trace("Ожидаем загрузки страницы Workspace.");
			return WaitUntilDisplayElement(By.XPath(CREATE_BTN_XPATH));
		}

		public void WaitAppearLocaleBtn()
		{
			Logger.Trace("Дождаемся появления ссылки для изменения языка локали");

			Assert.IsTrue(
				WaitUntilDisplayElement(By.XPath(LANGUAGE_SWITCHER)),
				"Не дождались загрузки страницы со ссылкой для изменения языка");
		}

		/// <summary>
		/// Выбрать язык локали
		/// </summary>
		/// <param name="langType">язык</param>
		public void SelectLocale(LOCALE_LANGUAGE_SELECT langType)
		{
			var lang = langType == LOCALE_LANGUAGE_SELECT.English 
									? LOCALE_EN_LANG 
									: LOCALE_RU_LANG;
			var xPath = LOCALE_REF_PATH + "[@data-locale='" + lang + "']";
			ClickLanguagSwitcher();
			if (WaitUntilDisplayElement(By.XPath(xPath), 1))
			{
				ClickElement(By.XPath(xPath));
			}
		}

		public LOCALE_LANGUAGE_SELECT GetCurrentLocale()
		{
			Logger.Debug("Получение текущей локализации");

			if (GetIsElementExist(By.XPath(string.Format(
						"{0}[@data-locale='{1}']", LOCALE_REF_PATH, LOCALE_RU_LANG))))
			{
				return LOCALE_LANGUAGE_SELECT.English;
			}
			
			return LOCALE_LANGUAGE_SELECT.Russian;
		}

		public void ClickCreateProject()
		{
			Logger.Debug("Кликнуть по кнопке Create Project");
			ClickElement(By.XPath(CREATE_BTN_XPATH));
		}

		/// <summary>
		/// Прокрутить до проекта и кликнуть галочку
		/// </summary>
		/// <param name="projectName">название проекта</param>
		public void SelectProject(string projectName)
		{
			var projectPath = GetProjectRefXPath(projectName) + "/../../../td[contains(@class,'checkbox')]";
			ScrollToElement(By.XPath(projectPath));
			ClickElement(By.XPath(projectPath));
		}
		
		public void OpenProjectPage(string projectName)
		{
			Logger.Trace(string.Format("Кликнуть по проекту {0}, чтобы зайти на страницу проекта", projectName));
			ClickElement(By.XPath(GetProjectRefXPath(projectName)));
		}

		public void OpenProjectInfo(string projectName)
		{
			Logger.Trace(string.Format("Открытие свертки проекта {0}", projectName));
			
			if (!GetClassAttrProjectInfo(projectName).Contains("opened"))
			{
				ClickElement(By.XPath(GetProjectRefXPath(projectName) + FOLDER_SIGN));
			}
		}

		public bool OpenDocumentInfo(int documentNumber)
		{
			Logger.Trace(string.Format("Открытие свертки документа {0}", documentNumber));

			// Кликнуть на открытие информации о документе
			var documentXPath = DOCUMENT_INFO_TR_XPATH + "[" + documentNumber + "]" + FOLDER_SIGN;
			var isExistDocument = GetIsElementExist(By.XPath(documentXPath));

			if (isExistDocument)
			{
				ClickElement(By.XPath(documentXPath));
			}

			return isExistDocument;
		}

		public void WaitProjectAppearInList(string projectName)
		{
			Logger.Trace(string.Format("Ожидаем появление проекта {0} в списке", projectName));

			Assert.IsTrue(
				WaitUntilDisplayElement(By.XPath(GetProjectRefXPath(projectName))), 
				"Ошибка: проект " + projectName + " не появился в списке Workspace");
		}

		/// <summary>
		/// Вернуть, есть ли проект в списке
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <returns>есть</returns>
		public bool GetIsProjectInList(string projectName)
		{
			// Выставляем минимальный таймаут
			SetDriverTimeoutMinimum();

			var projectSearchField = Driver.FindElement(By.XPath(PROJECT_SEARCH_FIELD));
			var projectSearchButton = Driver.FindElement(By.XPath(SEARCH_PROJECT_BUTTON));
			projectSearchField.SendKeys(projectName);
			projectSearchButton.Click();

			// Находим, существует ли проект в списке
			var isExist = WaitUntilDisplayElement(By.XPath(GetProjectRefXPath(projectName)), maxWait: 5);

			projectSearchField = Driver.FindElement(By.XPath(PROJECT_SEARCH_FIELD));
			projectSearchButton = Driver.FindElement(By.XPath(SEARCH_PROJECT_BUTTON));
			projectSearchField.Clear();
			projectSearchButton.Click();
			// Возвращаем дефолтное значение таймаута
			SetDriverTimeoutDefault();
			
			return isExist;
		}

		/// <summary>
		/// Кликнуть по сообщению об экспорте
		/// </summary>
		/// <param name="notifierNumberFromTop">номер сообщения об экспорте сверху (сверху самое старой позади остальных новых)</param>
		public void ClickNotifier(int notifierNumberFromTop)
		{
			((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click()", Driver.FindElement(By.XPath(GetNotifierXPath(notifierNumberFromTop))));
		}

		/// <summary>
		/// Дождаться появления сообщения об экспорте под номером N
		/// </summary>
		/// <param name="notifierNumber">номер сообщения об экспорте</param>
		/// <returns>появилось</returns>
		public bool WaitNotifierAppear(int notifierNumber)
		{
			return WaitUntilDisplayElement(By.XPath(GetNotifierXPath(notifierNumber)));
		}

		/// <summary>
		/// Кликнуть Экспорт в свертке документа
		/// </summary>
		public void ClickExportBtnDocumentInfo()
		{
			ClickElement(By.XPath(EXPORT_DOCUMENT_INFO_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Экспорт в свертке проекта
		/// </summary>
		public void ClickExportBtnProjectInfo(string projectName)
		{
			ClickElement(By.XPath(GetProjectRefXPath(projectName) + "/ancestor::tr/following-sibling::tr[1]//li[@class='l-project-export-block']"));
		}

		/// <summary>
		/// Кликнуть красный Export
		/// </summary>
		/// <returns>кнопка не заблокирована</returns>
		public bool ClickExportRedBtn()
		{
			Logger.Trace("Проверка, активна ли красная кнопка экспорта");
			var isEnabled = !GetElementClass(By.XPath(EXPORT_BTN_XPATH)).Contains(DISABLED_BTN_CLASS);

			if (isEnabled)
			{
				Logger.Trace("Красная кнопка экспорта активна");
				Logger.Trace("Клик по красной кнопке экспорт, XPath = " + EXPORT_BTN_XPATH);
				ScrollToElement(By.XPath(EXPORT_BTN_XPATH));
				ClickElement(By.XPath(EXPORT_BTN_XPATH));
			}

			return isEnabled;
		}

		/// <summary>
		/// Выбрать тип экспорта
		/// </summary>
		/// <param name="type">тип</param>
		public void SelectExportType(EXPORT_TYPE type)
		{
			Logger.Trace("Выбрали тип экспорта " + type);
			ClickElement(By.XPath(GetExportTypeRefXPath(type)));
		}

		/// <summary>
		/// Кликнуть Cancel в сообщении об экспорте
		/// </summary>
		public void ClickCancelNotifier()
		{
			Logger.Trace("Клик по кнопке Cancel в окне сообщения экспорта");
			ClickElement(By.XPath(NOTIFIER_CANCEL_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Download в сообщении об экспорте
		/// </summary>
		public void ClickDownloadNotifier()
		{
			Logger.Trace("Клик по кнопке Download в окне сообщения о экспорте");
			ClickElement(By.XPath(NOTIFIER_DOWNLOAD_BTN_XPATH));
			Logger.Trace("Время клика по кнопке Download = " + DateTime.Now.ToString("MM.dd.yyyy HH:mm:ss"));
		}

		/// <summary>
		/// Кликнуть Restart в сообщении об экспорте
		/// </summary>
		public void ClickRestartNotifier()
		{
			Logger.Trace("Клик по кнопке 'Restart'");
			ClickElement(By.XPath(NOTIFIER_RESTART_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться, пока пропадет сообщение о подготовке к экспорту
		/// </summary>
		/// <returns></returns>
		public bool WaitUntilDisappearPrepareNotifier()
		{
			Logger.Trace("Ожидание, когда пропадет сообщение 'Prepare...'");
			return WaitUntilDisappearElement(By.XPath(NOTIFIER_PREPARE_XPATH), 40);
		}

		/// <summary>
		/// Получить, видно ли сообщение о подготовке к экспорту
		/// </summary>
		/// <returns>видно</returns>
		public bool GetIsPrepareNotifierDisplayed()
		{
			Logger.Trace("Получить, видно ли сообщение о подготовке к экспорту");
			return GetIsElementDisplay(By.XPath(NOTIFIER_PREPARE_XPATH));
		}

		/// <summary>
		/// Получить, видна ли кнопка Загрузить в сообщении об экспорте
		/// </summary>
		/// <param name="numberNotification"> номер сообщения о экспорте </param>
		/// <returns>видна</returns>
		public bool GetIsDownloadBtnNotifierDisplayed(int numberNotification = 1)
		{
			Logger.Trace("Проверка появилась ли кнопка 'Download' в сообщении о экспорте");
			return GetIsElementDisplay(By.XPath(NOTIFIER_XPATH
				+ ONE_NOTIFIER_POP_UP + "[" + numberNotification + "]" + DOWNLOAD_BTN_IN_POP_UP));
		}

		/// <summary>
		/// Получить, видна ли кнопка Restart в сообщении об экспорте
		/// </summary>
		/// <returns>видна</returns>
		public bool GetIsRestartBtnNotifierDisplayed()
		{
			Logger.Trace("Проверка, есть ли кнопка 'Restart' в сообщение о экспорте");
			return GetIsElementDisplay(By.XPath(NOTIFIER_RESTART_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть, видно ли сообщение об экспорте
		/// </summary>
		/// <returns>видно</returns>
		public bool GetIsExistNotifier()
		{
			Logger.Trace("Проверка, появилось ли сообщение о экспорте");
			return GetIsElementDisplay(By.XPath(NOTIFIER_ITEM_XPATH));
		}

		/// <summary>
		/// Дождаться, пока пропадет сообщение об экспорте
		/// </summary>
		/// <returns>пропало</returns>
		public bool WaitUntilDisappearNotifier()
		{
			return WaitUntilDisappearElement(By.XPath(NOTIFIER_ITEM_XPATH));
		}

		/// <summary>
		/// Вернуть текст из сообщения об экспорте
		/// </summary>
		/// <returns>текст</returns>
		public string GetNotifierText()
		{
			Logger.Trace("Получить текст из сообщения о экспорте");
			return GetTextElement(By.XPath(NOTIFIER_TEXT_XPATH));
		}

		/// <summary>
		/// Закрыть все сообщения об экспорте
		/// </summary>
		public void CancelAllNotifiers()
		{
			SetDriverTimeoutMinimum();
			Logger.Trace("Проверка, есть ли сообщения экспорта");
			var isExist = GetIsElementDisplay(By.XPath(NOTIFIER_CANCEL_BTN_XPATH));

			while (isExist)
			{
				Logger.Trace("Сообщения экспорта есть на странице");
				ClickCancelNotifier();
				Thread.Sleep(2000);
				Logger.Trace("Проверка, есть ли еще сообщения экспорта");
				isExist = GetIsElementDisplay(By.XPath(NOTIFIER_CANCEL_BTN_XPATH));
			}

			SetDriverTimeoutDefault();
		}

		/// <summary>
		/// Получить количество сообщений об экспорте
		/// </summary>
		/// <returns>количество</returns>
		public int GetNotifierNumber()
		{
			return GetElementsCount(By.XPath(NOTIFIER_ITEM_XPATH));
		}

		/// <summary>
		/// Кликнуть кнопку настроек документа
		/// </summary>
		public void ClickDocumentSettingsBtn()
		{
			ClickElement(By.XPath(DOCUMENT_SETTINGS_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться открытия диалога настройки документа
		/// </summary>
		/// <returns>открылся</returns>
		public bool WaitDocumentSettingsDialogAppear()
		{
			return WaitUntilDisplayElement(By.XPath(DOCUMENT_SETTINGS_DIALOG_XPATH));
		}

		/// <summary>
		/// Дождаться закрытия диалога настроек документа
		/// </summary>
		/// <returns></returns>
		public bool WaitUntilDocumentSettingsDialogDisappear()
		{
			return WaitUntilDisappearElement(By.XPath(DOCUMENT_SETTINGS_DIALOG_XPATH));
		}

		/// <summary>
		/// Ввести название документа (в диалоге изменения документа)
		/// </summary>
		/// <param name="name">название</param>
		public void DocumentSettingsAddName(string name)
		{
			ClickClearAndAddText(By.XPath(DOCUMENT_SETTINGS_NAME_XPATH), name);
		}

		/// <summary>
		/// Кликнуть Сохранить
		/// </summary>
		public void DocumentSettingsClickSave()
		{
			ClickElement(By.XPath(DOCUMENT_SETTINGS_SAVE_BTN_XPATH));
		}

		/// <summary>
		/// Распарсить дату из сообщения об экспорте
		/// </summary>
		/// <returns>дата (в DateTime)</returns>
		public DateTime GetDateFromNotifier()
		{
			var notifierText = GetNotifierText();
			// Распарсим дату в сообщении об экспорте
			var startIndex = notifierText.IndexOf("/") - 2;
			var month = notifierText.Substring(startIndex, 2);
			startIndex += 3; // "mm/" = 3
			var day = notifierText.Substring(startIndex, 2);
			startIndex += 3; // "dd/" = 3
			var year = notifierText.Substring(startIndex, 4);
			startIndex += 5; // "yyyy " = 5;
			var hour = notifierText.Substring(startIndex, 2);
			startIndex += 3; // "hh:" = 3
			var min = notifierText.Substring(startIndex, 2);
			Logger.Trace("Распарсили дату из сообщения о экспорте = " + month + "/" + day + "/" + year + " " + hour + ":" + min);

			// Получили дату
			return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(min), 0);
		}

		/// <summary>
		/// Кликнуть удалить проект
		/// </summary>
		public void ClickDeleteProjectBtn()
		{
			ClickElement(By.XPath(DELETE_BTN_XPATH));
		}

		/// <summary>
		/// Потвердить удаление
		/// </summary>
		/// <returns>форма открылась</returns>
		public void ClickConfirmDelete()
		{
			var isExistForm = WaitUntilDisplayElement(By.XPath(CONFIRM_DELETE_FORM_XPATH));

			if (isExistForm)
			{
				ClickElement(By.XPath(CONFIRM_DELETE_YES_XPATH));
			}

			Assert.IsTrue(isExistForm, "Ошибка: не появилась форма подтверждения удаления проекта");
		}

		/// <summary>
		/// Дождаться пропадания форму подтверждения удаления
		/// </summary>
		/// <returns></returns>
		public void WaitUntilDeleteConfirmFormDisappear()
		{
			Assert.IsTrue(
				WaitUntilDisappearElement(By.XPath(CONFIRM_DELETE_FORM_XPATH), 30),
				"Ошибка: не скрылась форма подтверждения удаления проекта");
		}

		/// <summary>
		/// Дождаться, пока документ загрузится
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <returns>загрузился</returns>
		public bool WaitDocumentProjectDownload(string projectName)
		{
			return WaitUntilDisappearElement(
				By.XPath(GetProjectRefXPath(projectName) + DOWNLOAD_IMG_XPATH),
				maxWaitSeconds: 50);
		}

		/// <summary>
		/// Дождаться открытия диалога выбора режима удаления проекта
		/// </summary>
		/// <returns></returns>
		public bool WaitDeleteModeDialog()
		{
			return WaitUntilDisplayElement(By.XPath(DELETE_MODE_DIALOG_XPATH));
		}

		/// <summary>
		/// Кликнуть удалить проект
		/// </summary>
		/// <returns>кнопка Удалить проекта есть</returns>
		public bool ClickDeleteProjectDeleteMode()
		{
			var isBtnExist = GetIsElementExist(By.XPath(DELETE_MODE_DIALOG_DELETE_PROJECT_XPATH));
			
			if (isBtnExist)
			{
				ClickElement(By.XPath(DELETE_MODE_DIALOG_DELETE_PROJECT_XPATH));
				WaitUntilDisappearElement(By.XPath(DELETE_MODE_DIALOG_XPATH));
			}
			return isBtnExist;
		}

		/// <summary>
		/// Получить xPath ссылки на проект
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <returns>xPath</returns>
		protected string GetProjectRefXPath(string projectName)
		{
			// В зависимости от права тег может быть равен "a" или "span"
			return PROJECTS_TABLE_XPATH + "//tr//*[@class='js-name'][(local-name() ='a' or local-name() ='span') and text()='" + projectName + "']";
		}

		public string GetClassAttrProjectInfo(string projectName)
		{
			return GetElementAttribute(By.XPath(GetProjectRefXPath(projectName) + "/ancestor-or-self::tr"), "class");
		}

		/// <summary>
		/// Получить xPath конкретного сообщения об экспорте
		/// </summary>
		/// <param name="notifierNumberFromTop">номер сообщения сверху</param>
		/// <returns>xPath</returns>
		protected string GetNotifierXPath(int notifierNumberFromTop)
		{
			return NOTIFIER_ITEM_XPATH + "[contains(@style, 'top: " + (notifierNumberFromTop - 1) * 10 + "px')]";////preceding-sibling::div[" + notifierNumberFromTop + "]";
		}


		/// <summary>
		/// Вернуть xPath строки с типом экспорта
		/// </summary>
		/// <param name="type">тип</param>
		/// <returns>xPath</returns>
		protected string GetExportTypeRefXPath(EXPORT_TYPE type)
		{
			return EXPORT_TYPE_REF_BEGINING + exportTypeDict[type] + "')]//a";
		}

		public void WaitProjectLoad(string projectName)
		{
			Logger.Trace(string.Format("Ожидание ожидание загрузки проекта {0}", projectName));
			
			if (!WaitDocumentProjectDownload(projectName))
			{
				Driver.Navigate().Refresh();

				if (!WaitDocumentProjectDownload(projectName))
				{
					var errorMessage = string.Format("Ошибка: документ не загрузился в проект {0}", projectName);

					Logger.Error(errorMessage);
					throw new NotFoundException(errorMessage);
				}
			}
		}

		public void ClickDocumentProgress()
		{
			Logger.Trace("Нажать на прогресс в поле документа");
			ClickElement(By.XPath(DOCUMENT_INFO_TR_XPATH + DOCUMENT_PROGRESS_XPATH));
		}

		/// <summary>
		/// Получить xPath ссылки на документ
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <returns>xPath</returns>
		protected string GetDocumentRefXPath(string projectName, int documentNumber=1)
		{
			return GetProjectRefXPath(projectName) + "/ancestor::tr/following-sibling::tr[" + (documentNumber * 2) +"]//a";
		}

		/// <summary>
		/// Кликнуть Progress кнопку определенного документа и проекта
		/// </summary>
		/// <param name="projectName"> Название проекта </param>
		/// <param name="documentNumber"> Номер документа </param>
		public void ClickProgressDocument(string projectName, int documentNumber = 1)
		{
			ClickElement(By.XPath(GetProjectRefXPath(projectName) + "/ancestor::tr/following-sibling::tr[" + (documentNumber * 2) + "]" + DOCUMENT_PROGRESS_XPATH));
		}
		
		/// <summary>
		/// Получить открыта ли свертка документа
		/// </summary>
		/// <param name="projectName"></param>
		/// <returns></returns>
		public bool GetDocumentPanelIsOpened(string projectName, int docNumber = 1)
		{
			string xpath = GetProjectRefXPath(projectName)
						+ "//ancestor::tr/following-sibling::tr[contains(@class,'js-document-row')][" + docNumber + "]";
			return GetElementAttribute(By.XPath(xpath), "class").Contains("opened");
		}

		/// <summary>
		/// Раскрыть свертку документа
		/// </summary>
		/// <param name="projectName"> Имя проекта </param>
		public void OpenDocumnetInfoForProject(string projectName)
		{
			// Если свертка документа не открыта - открываем
			if (!GetDocumentPanelIsOpened(projectName)) 
				ClickProgressDocument(projectName);
		}

		public void ClickDocumentAssignBtn(string projectName, int documentNumber=1)
		{
			Logger.Trace("Нажать на кнопку прав пользователя в свертке документа");
			ClickElement(By.XPath(GetProjectRefXPath(projectName) + "/ancestor::tr/following-sibling::tr[" + (documentNumber * 2) + "]/following-sibling::tr[1][@class='js-document-panel l-project__doc-panel']" + DOCUMENT_ASSIGN_RESPONSIBLES_BTN_XPATH));

		}

		public void ClickDocumentUploadBtn()
		{
			Logger.Trace("Нажать на кнопку добавления документа в свертке проекта");
			ClickElement(By.XPath(UPLOAD_DOCUMENT_BTN_XPATH));
		}

		public void ClickUsersAndRightsBtn()
		{
			if (!GetIsLeftMenuDisplay())
				OpenHideMenu();
			Logger.Trace("Клик по пункту 'Users and rights' в меню слева");
			ClickElement(By.XPath(USERS_RIGHTS_BTN_XPATH));
		}

		public string GetUserName()
		{
			Logger.Trace("Возвращаем имя текущего пользователя");

			if (!WaitUntilDisplayElement(By.XPath(USER_NAME_XPATH)))
			{
				var errorMessage = string.Format("Невозможно получить имя текущего пользователя. Путь к искомому элементу: {0}", USER_NAME_XPATH);
				Logger.Error(errorMessage);

				throw new NoSuchElementException(errorMessage);
			}

			var textFromAccountField =  GetTextElement(By.XPath(USER_NAME_XPATH));
			var index = textFromAccountField.IndexOf("\r", StringComparison.Ordinal);

			if (index > 0)
			{
				textFromAccountField = textFromAccountField.Substring(0, index);
			}

			return textFromAccountField;
		}

		public string GetCompanyName()
		{
			Logger.Trace("Возвращаем название компании в вехней панели рядом с именем текущего пользователя");
			WaitUntilDisplayElement(By.XPath(COMPANY_NAME_PANEL_WS));
			return GetTextElement(By.XPath(COMPANY_NAME_PANEL_WS));
		}

		public void ClickAccount()
		{
			Logger.Trace("Нажимаем на имя пользователя и аккаунт, чтобы появилась черная плашка Настройки профиля");
			ClickElement(By.XPath(ACCOUNT_XPATH));
		}

		/// <summary>
		/// Выбрать пункт для перехода к управлению лицензиями в меню профиля
		/// </summary>
		public void ClickLicensesAndServices()
		{
			Logger.Trace("Выбираем в меню профиля пунтк для управления лицензиями.");
			ClickElement(By.XPath(LICENSES_AND_SERVICES_MENU_ITEM));
		}

		/// <summary>
		/// Разлогинится
		/// </summary>
		public void ClickLogoff()
		{
			Logger.Trace("Клик по кнопке 'Sign Out'");
			ClickElement(By.XPath(LOGOFF_XPATH));
		}

		public bool CheckAccountList(string accountName)
		{
			return GetIsElementDisplay(By.XPath("//li[@class='g-topbox__corpitem' and @title='" + accountName + "']"));
		}

		/// <summary>
		/// Проверить, находимся ли мы на странице воркспейса
		/// </summary>
		public bool CheckIfWorkspace()
		{
			return GetIsElementDisplay(By.XPath(CREATE_BTN_XPATH));
		}

		/// <summary>
		/// Загрузка документа в настройках проекта (на стр WS)
		/// </summary>
		/// <param name="fileName"></param>
		public void UploadFileInProjectSettings(string fileName)
		{
			UploadDocument(fileName, ADD_FILE_TO_PROJECT);
		}

		/// <summary>
		/// Раскрыть пункт Resources в главном меню слева
		/// </summary>
		public void ClickOpenResourcesInMenu()
		{
			if (!GetIsLeftMenuDisplay())
				OpenHideMenu();
			Logger.Trace("Раскрыть пункт Resources в главном меню слева");
			ClickElement(By.XPath(RESOURCES_IN_MENU_XPATH));
		}

		protected const string ADD_FILE_TO_PROJECT = "//div[contains(@class, 'popup-import-document')][2]//input[@type='file']"; // добавление документа уже сущестующему проекту на стр WS

		public enum EXPORT_TYPE { Original, TMX, Translated };

		public void ClickLanguagSwitcher()
		{
			Driver.FindElement(By.XPath(LANGUAGE_SWITCHER)).Click();
		}

		public bool GetWarningIsDisplayForProject(string projectName)
		{
			Logger.Trace("Проверка, отображается ли треуголник с восклицательным знаком рядом с названием проекта");
			return GetIsElementDisplay(By.XPath(GetProjectRefXPath(projectName) + "//preceding-sibling::" + WARNING_SIGN_TRIANGLE));
		}

		public const string LOCALE_EN_LANG = "en";
		public const string LOCALE_RU_LANG = "ru";
		
		protected const string LOCALE_REF_PATH = ".//a[contains(@class,'js-set-locale')]";

		protected const string CREATE_BTN_XPATH = ".//span[contains(@class,'js-project-create')]";

		protected const string PROJECTS_TABLE_XPATH = ".//table[contains(@class,'js-tasks-table')]";

		protected const string DOCUMENT_INFO_TR_XPATH = "//following-sibling::tr[contains(@class, 'js-document-row')]";
		protected const string OPEN_CLOSE_TD_XPATH = "div[contains(@class,'l-corpr__threeDots')]";
		protected const string FOLDER_SIGN = "//preceding-sibling::div//img";
		protected const string NOTIFIER_XPATH = "//div[@id='notifications-block']";
		protected const string DOWNLOAD_BTN_IN_POP_UP = "//a[1]";
		protected const string ONE_NOTIFIER_POP_UP= "//div[@class='g-notifications-item']";
		protected const string NOTIFIER_ITEM_XPATH = NOTIFIER_XPATH + "//div[contains(@class,'notifications-item')]";
		protected const string NOTIFIER_CANCEL_BTN_XPATH = NOTIFIER_VISIBLE_XPATH + "//a[contains(text(),'Close')]";
		protected const string NOTIFIER_DOWNLOAD_BTN_XPATH = NOTIFIER_XPATH + "//a[contains(text(),'Download')]";
		// TODO изменить айдишники
		protected const string NOTIFIER_PREPARE_XPATH = NOTIFIER_XPATH + "//span[contains(text(),'Preparing')]";
		protected const string NOTIFIER_RESTART_BTN_XPATH = NOTIFIER_XPATH + "//a[@class='js-restart-task']";
		protected const string NOTIFIER_VISIBLE_XPATH = NOTIFIER_ITEM_XPATH + "//div[not(contains(@style,'none'))]";
		protected const string NOTIFIER_TEXT_XPATH = NOTIFIER_VISIBLE_XPATH + "/span";

		protected const string EXPORT_DOCUMENT_INFO_BTN_XPATH = DOCUMENT_INFO_XPATH + EXPORT_BTN_XPATH;
		protected const string EXPORT_PROJECT_INFO_XPATH = PROJECT_INFO_XPATH + EXPORT_BTN_XPATH;

		protected const string PROJECT_INFO_XPATH = "//tr[@class='js-project-panel']";
		protected const string DOCUMENT_INFO_XPATH = "//tr[contains(@class,'js-document-panel')]";
		protected const string EXPORT_BTN_XPATH = "//span[contains(@class,'js-document-export-block')]//a";
		protected const string DOCUMENT_SETTINGS_BTN_XPATH = DOCUMENT_INFO_XPATH + "//span[contains(@class,'js-settings-btn')]";
		protected const string DISABLED_BTN_CLASS = "disable";

		protected const string EXPORT_TYPE_REF_BEGINING = "//div[contains(@class,'js-export-submenu-list') and not(contains(@class,'g-hidden'))]//div[contains(@data-bind,'";
		protected const string EXPORT_TYPE_NAME_ORIGINAL = "Source";
		protected const string EXPORT_TYPE_NAME_TMX = "Tmx";
		protected const string EXPORT_TYPE_NAME_TRANSLATED = "Target";

		protected const string DELETE_BTN_XPATH = "//span[contains(@class,'js-delete-btn')]";
		protected const string CONFIRM_DELETE_FORM_XPATH = "//div[contains(@class,'js-popup-confirm')]";
		protected const string CONFIRM_DELETE_YES_XPATH = CONFIRM_DELETE_FORM_XPATH + "//input[contains(@class,'js-submit-btn')]";

		protected const string DOWNLOAD_IMG_XPATH = "/..//img[contains(@class,'l-project-doc__progress')]";

		protected const string DELETE_MODE_DIALOG_XPATH = "//div[contains(@class,'js-popup-delete-mode')]";
		protected const string DELETE_MODE_DIALOG_DELETE_PROJECT_XPATH = DELETE_MODE_DIALOG_XPATH + "//input[contains(@class,'js-delete-project-btn')]";


		protected Dictionary<EXPORT_TYPE, string> exportTypeDict;

		protected const string DOCUMENT_SETTINGS_DIALOG_XPATH = ".//div[contains(@class,'js-popup-document-settings')][2]";
		protected const string DOCUMENT_SETTINGS_NAME_XPATH = DOCUMENT_SETTINGS_DIALOG_XPATH + "//input[contains(@class,'nmtext')]";
		protected const string DOCUMENT_SETTINGS_SAVE_BTN_XPATH = DOCUMENT_SETTINGS_DIALOG_XPATH + "//a[contains(text(), 'Save')]";

		protected const string DOCUMENT_PROGRESS_XPATH = "//div[@class='ui-progressbar__container']";
		protected const string DOCUMENT_ASSIGN_RESPONSIBLES_BTN_XPATH = "//span[contains(@class, 'js-assign-btn') and @data-bind='click: assign']";
		protected const string UPLOAD_DOCUMENT_BTN_XPATH = "//a[contains(text(), 'Add File')]";

		protected const string USERS_RIGHTS_BTN_XPATH = ".//a[contains(@href,'/Users/Index')]";

		protected const string ACCOUNT_XPATH = "//div[contains(@class,'js-usermenu')]";
		protected const string USER_NAME_XPATH = ACCOUNT_XPATH + "//span[contains(@class,'g-topbox__nameuser')]//b";
		protected const string LOGOFF_XPATH = "//div[@class='g-topbox__popup accountTools']//a[contains(@class, 'logout')]";

		protected const string TOP_BOX_COMPANY_NAME = "//div[@class='g-topbox__currentAccount__nickname']";
		protected const string COMPANY_NAME_PANEL_WS = "//span[contains(@class,'g-topbox__nameuser')]//small";

		protected const string LANGUAGE_SWITCHER = "//span[contains(@class,'js-language-button')]";
		protected const string RESOURCES_IN_MENU_XPATH = "//li[contains(@class,'js-menuitem-Resources')]";
		protected const string LICENSES_AND_SERVICES_MENU_ITEM = "//a[contains(@class,'billing')]";

		protected const string WARNING_SIGN_TRIANGLE = "img[contains(@class, 'doc__error')]";

		protected const string PROJECT_SEARCH_FIELD = "//input[@name='searchName']";
		protected const string SEARCH_PROJECT_BUTTON = "//a[contains(@class, 'js-search-btn')]/img";
	}
}