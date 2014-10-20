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
using System.Threading;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы workspace
	/// </summary>
	public class WorkSpacePageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public WorkSpacePageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
			exportTypeDict = new Dictionary<EXPORT_TYPE, string>();
			exportTypeDict.Add(EXPORT_TYPE.Original, EXPORT_TYPE_NAME_ORIGINAL);
			exportTypeDict.Add(EXPORT_TYPE.TMX, EXPORT_TYPE_NAME_TMX);
			exportTypeDict.Add(EXPORT_TYPE.Translated, EXPORT_TYPE_NAME_TRANSLATED);
		}

		/// <summary>
		/// Дождаться загрузки страницы
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(CREATE_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться появления ссылки для изменения языка локали
		/// </summary>
		/// <returns>появилась</returns>
		public bool WaitAppearLocaleBtn()
		{
			return WaitUntilDisplayElement(By.XPath(LOCALE_REF_PATH));
		}

		/// <summary>
		/// Выбрать язык локали
		/// </summary>
		/// <param name="langType">язык</param>
		public void SelectLocale(LOCALE_LANGUAGE_SELECT langType)
		{
			string lang = "";
			if (langType == LOCALE_LANGUAGE_SELECT.English)
			{
				lang = LOCALE_EN_LANG;
			}

			string xPath = LOCALE_REF_PATH + "[@data-locale='" + lang + "']";
			SetDriverTimeoutMinimum();
			if (GetIsElementExist(By.XPath(xPath)))
			{
				ClickElement(By.XPath(xPath));
			}
			SetDriverTimeoutDefault();
		}

		/// <summary>
		/// Кликнуть по Create Project
		/// </summary>
		public void ClickCreateProject()
		{
			ClickElement(By.XPath(CREATE_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть галочку у проекта
		/// </summary>
		/// <param name="projectName">название проекта</param>
		public void SelectProject(string projectName)
		{
			ClickElement(By.XPath(GetProjectRefXPath(projectName) + "/../../../td[contains(@class,'checkbox')]"));
		}
		
		/// <summary>
		/// Кликнуть по проекту, чтобы зайти на страницу проекта
		/// </summary>
		/// <param name="projectName">название проекта</param>
		public void OpenProjectPage(string projectName)
		{
			ClickElement(By.XPath(GetProjectRefXPath(projectName)));
		}

		/// <summary>
		/// Кликнуть, чтобы открыть свертку проекта
		/// </summary>
		/// <param name="projectName">название проекта</param>
		public void OpenProjectInfo(string projectName)
		{
			ClickElement(By.XPath(GetProjectRefXPath(projectName) + "/../../../" + OPEN_CLOSE_TD_XPATH));
		}

		/// <summary>
		/// Открыть свертку документа
		/// </summary>
		/// <param name="documentNumber">номер документа</param>
		/// <returns>такой документ есть</returns>
		public bool OpenDocumentInfo(int documentNumber)
		{
			// Кликнуть на открытие информации о документе
			string documentXPath = DOCUMENT_INFO_TR_XPATH + "[" + documentNumber + "]//" + OPEN_CLOSE_TD_XPATH;
			bool isExistDocument = GetIsElementExist(By.XPath(documentXPath));
			if (isExistDocument)
			{
				ClickElement(By.XPath(documentXPath));
			}

			return isExistDocument;
		}

		/// <summary>
		/// Дождаться появления проекта в списке
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <returns>появился</returns>
		public bool WaitProjectAppearInList(string projectName)
		{
			return WaitUntilDisplayElement(By.XPath(GetProjectRefXPath(projectName)));
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
			
			// Находим, существует ли проект в списке
			bool isExist = GetIsElementExist(By.XPath(GetProjectRefXPath(projectName)));
			
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
			ClickElement(By.XPath(GetNotifierXPath(notifierNumberFromTop)));
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
		public void ClickExportBtnProjectInfo()
		{
			ClickElement(By.XPath(EXPORT_PROJECT_INFO_XPATH));
		}

		/// <summary>
		/// Кликнуть красный Export
		/// </summary>
		/// <returns>кнопка не заблокирована</returns>
		public bool ClickExportRedBtn()
		{
			// Кнопка не заблокирована?
			bool isEnabled = !GetElementClass(By.XPath(EXPORT_BTN_XPATH)).Contains(DISABLED_BTN_CLASS);

			if (isEnabled)
			{
				Console.WriteLine("кликнуть красный экспорт\n" + EXPORT_BTN_XPATH);
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
			ClickElement(By.XPath(GetExportTypeRefXPath(type)));
		}

		/// <summary>
		/// Кликнуть Cancel в сообщении об экспорте
		/// </summary>
		public void ClickCancelNotifier()
		{
			ClickElement(By.XPath(NOTIFIER_CANCEL_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Download в сообщении об экспорте
		/// </summary>
		public void ClickDownloadNotifier()
		{
			ClickElement(By.XPath(NOTIFIER_DOWNLOAD_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Restart в сообщении об экспорте
		/// </summary>
		public void ClickRestartNotifier()
		{
			ClickElement(By.XPath(NOTIFIER_RESTART_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться, пока пропадет сообщение о подготовке к экспорту
		/// </summary>
		/// <returns></returns>
		public bool WaitUntilDisappearPrepareNotifier()
		{
			return WaitUntilDisappearElement(By.XPath(NOTIFIER_PREPARE_XPATH), 40);
		}

		/// <summary>
		/// Получить, видно ли сообщение о подготовке к экспорту
		/// </summary>
		/// <returns>видно</returns>
		public bool GetIsPrepareNotifierDisplayed()
		{
			return GetIsElementDisplay(By.XPath(NOTIFIER_PREPARE_XPATH));
		}

		/// <summary>
		/// Получить, видна ли кнопка Загрузить в сообщении об экспорте
		/// </summary>
		/// <returns>видна</returns>
		public bool GetIsDownloadBtnNotifierDisplayed()
		{
			return GetIsElementDisplay(By.XPath(NOTIFIER_DOWNLOAD_BTN_XPATH));
		}

		/// <summary>
		/// Получить, видна ли кнопка Restart в сообщении об экспорте
		/// </summary>
		/// <returns>видна</returns>
		public bool GetIsRestartBtnNotifierDisplayed()
		{
			return GetIsElementDisplay(By.XPath(NOTIFIER_RESTART_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть, видно ли сообщение об экспорте
		/// </summary>
		/// <returns>видно</returns>
		public bool GetIsExistNotifier()
		{
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
			return GetTextElement(By.XPath(NOTIFIER_TEXT_XPATH));
		}

		/// <summary>
		/// Закрыть все сообщения об экспорте
		/// </summary>
		public void CancelAllNotifiers()
		{
			SetDriverTimeoutMinimum();
			bool isExist = GetIsElementDisplay(By.XPath(NOTIFIER_CANCEL_BTN_XPATH));

			while (isExist)
			{
				ClickCancelNotifier();
				Thread.Sleep(2000);
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
			SendTextElement(By.XPath(DOCUMENT_SETTINGS_NAME_XPATH), name);
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
			string notifierText = GetNotifierText();
			// Распарсим дату в сообщении об экспорте
			int startIndex = notifierText.IndexOf("/") - 2;
			string month = notifierText.Substring(startIndex, 2);
			startIndex += 3; // "mm/" = 3
			string day = notifierText.Substring(startIndex, 2);
			startIndex += 3; // "dd/" = 3
			string year = notifierText.Substring(startIndex, 4);
			startIndex += 5; // "yyyy " = 5;
			string hour = notifierText.Substring(startIndex, 2);
			startIndex += 3; // "hh:" = 3
			string min = notifierText.Substring(startIndex, 2);
			Console.WriteLine(month + "/" + day + "/" + year + " " + hour + ":" + min);

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
		public bool ClickConfirmDelete()
		{
			bool isExistForm = WaitUntilDisplayElement(By.XPath(CONFIRM_DELETE_FORM_XPATH));
			if (isExistForm)
			{
				ClickElement(By.XPath(CONFIRM_DELETE_YES_XPATH));
			}
			return isExistForm;
		}

		/// <summary>
		/// Дождаться пропадания форму подтверждения удаления
		/// </summary>
		/// <returns></returns>
		public bool WaitUntilDeleteConfirmFormDisappear()
		{
			return WaitUntilDisappearElement(By.XPath(CONFIRM_DELETE_FORM_XPATH), 30);
		}

		/// <summary>
		/// Дождаться, пока документ загрузится
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <returns>загрузился</returns>
		public bool WaitDocumentProjectDownload(string projectName)
		{
			return WaitUntilDisappearElement(By.XPath(GetProjectRefXPath(projectName) + DOWNLOAD_IMG_XPATH), 50);
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
			bool isBtnExist = GetIsElementExist(By.XPath(DELETE_MODE_DIALOG_DELETE_PROJECT_XPATH));
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
			return PROJECTS_TABLE_XPATH + "//tr//a[@class='js-name'][text()='" + projectName + "']";
		}

		/// <summary>
		/// Получить xPath конкретного сообщения об экспорте
		/// </summary>
		/// <param name="notifierNumberFromTop">номер сообщения сверху</param>
		/// <returns>xPath</returns>
		protected string GetNotifierXPath(int notifierNumberFromTop)
		{
			return NOTIFIER_ITEM_XPATH + "[" + notifierNumberFromTop + "]";
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

		/// <summary>
		/// Ожидание ожидание загрузки проекта
		/// </summary>
		/// <param name="projectName">Имя проекта</param>
		/// <returns>Загрузился ли проект</returns>
		public bool WaitProjectLoad(string projectName)
		{
			// Ожидаем пока загрузится документ. Обновляем страницу, если недождались
			if (!WaitDocumentProjectDownload(projectName))
			{
				Driver.Navigate().Refresh();
				// Внова проверяем загрузился ли документ. Ожидаем пока загрузится.
				if (!WaitDocumentProjectDownload(projectName))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Нажать на прогресс в поле документа
		/// </summary>
		public void ClickDocumentProgress()
		{
			ClickElement(By.XPath(DOCUMENT_INFO_TR_XPATH + DOCUMENT_PROGRESS_XPATH));
		}

		/// <summary>
		/// Нажать на кнопку прав пользователя в инфо документа
		/// </summary>
		public void ClickDocumentAssignBtn()
		{
			ClickElement(By.XPath(DOCUMENT_ASSIGN_RESPONSIBLES_BTN_XPATH));
		}

		/// <summary>
		/// Нажать на кнопку добавления документа в инфо проекта
		/// </summary>
		public void ClickDocumentUploadBtn()
		{
			ClickElement(By.XPath(UPLOAD_DOCUMENT_BTN_XPATH));
		}

		/// <summary>
		/// Нажать на кнопку "Users and rights"	
		/// </summary>
		public void ClickUsersAndRightsBtn()
		{
			ClickElement(By.XPath(USERS_RIGHTS_BTN_XPATH));
		}

		/// <summary>
		/// Возвращает имя текущего пользователя
		/// </summary>
		/// <returns>Имя пользователя</returns>
		public string GetUserName()
		{
			return GetTextElement(By.XPath(USER_NAME_XPATH));
		}

		/// <summary>
		/// Нажать на имя пользователя и аккаунт, чтобы появилась черная плашка Настройки профиля
		/// </summary>
		public void ClickAccount()
		{
			ClickElement(By.XPath(ACCOUNT_XPATH));
		}

		/// <summary>
		/// Разлогинится
		/// </summary>
		public void ClickLogoff()
		{
			ClickElement(By.XPath(LOGOFF_XPATH));
		}

	    public bool CheckAccountList(string accountName)
	    {
	       return GetIsElementDisplay(By.XPath("//li[@class='g-topbox__corpitem' and @title='" + accountName + "']"));
	    }


	    public enum LOCALE_LANGUAGE_SELECT { English, Russian };
		public enum EXPORT_TYPE { Original, TMX, Translated };


		protected const string LOCALE_REF_PATH = ".//a[contains(@class,'js-set-locale')]";
		protected const string LOCALE_EN_LANG = "en";

		protected const string CREATE_BTN_XPATH = ".//span[contains(@class,'js-project-create')]";

		protected const string PROJECTS_TABLE_XPATH = ".//table[contains(@class,'js-tasks-table')]";

		protected const string DOCUMENT_INFO_TR_XPATH = PROJECT_INFO_XPATH + "//following-sibling::tr[contains(@class, 'js-document-row')]";
		protected const string OPEN_CLOSE_TD_XPATH = "td[contains(@class,'openCloseCell')]";

		protected const string NOTIFIER_XPATH = "//div[@id='notifications-block']";
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

		protected const string EXPORT_TYPE_REF_BEGINING = "//div[contains(@class,'js-export-submenu-list') and not(contains(@class,'g-hidden'))]//div[contains(@data-download-type,'";
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
		protected const string DOCUMENT_SETTINGS_NAME_XPATH = DOCUMENT_SETTINGS_DIALOG_XPATH + "//input[contains(@class,'js-name')]";
		protected const string DOCUMENT_SETTINGS_SAVE_BTN_XPATH = DOCUMENT_SETTINGS_DIALOG_XPATH + "//span[contains(@class,'js-save')]";

		protected const string DOCUMENT_PROGRESS_XPATH = "//td//a[contains(@class,'js-progress-link')]";
		protected const string DOCUMENT_ASSIGN_RESPONSIBLES_BTN_XPATH = "//span[contains(@class,'js-assign-btn')]";
		protected const string UPLOAD_DOCUMENT_BTN_XPATH = ".//span[contains(@class,'js-import-btn ')]";

		protected const string USERS_RIGHTS_BTN_XPATH = ".//a[contains(@href,'/Users/Index')]";

		protected const string ACCOUNT_XPATH = ".//div[contains(@class,'js-corp-account')]";
		protected const string USER_NAME_XPATH = ACCOUNT_XPATH + "//span[contains(@class,'nameuser')]";
		protected const string LOGOFF_XPATH = ".//a[contains(@href,'Logout')]";
	}
}