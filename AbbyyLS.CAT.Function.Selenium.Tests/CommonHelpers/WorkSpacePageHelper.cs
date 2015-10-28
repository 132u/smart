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

		public string GetCompanyName()
		{
			Logger.Trace("Возвращаем название компании в вехней панели рядом с именем текущего пользователя");
			WaitUntilDisplayElement(By.XPath(COMPANY_NAME_PANEL_WS));
			return GetTextElement(By.XPath(COMPANY_NAME_PANEL_WS));
		}

		public void ClickAccount()
		{
			Logger.Trace("Нажимаем на имя пользователя и аккаунт, чтобы появилась черная плашка Настройки профиля");
			WaitUntilDisplayElement(By.XPath(ACCOUNT_XPATH));
			ClickElement(By.XPath(ACCOUNT_XPATH));
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

		protected const string ADD_FILE_TO_PROJECT = "//div[contains(@class, 'popup-import-document')][2]//input[@type='file']"; // добавление документа уже сущестующему проекту на стр WS

		public enum EXPORT_TYPE { Original, TMX, Translated };

		public void CloseTour()
		{
			if (isTourOpen())
			{
				clickCloseTourButton();
			}
		}

		private void clickCloseTourButton()
		{
			Logger.Debug("Нажать кноку закрытия Инструкции");
			Driver.FindElement(By.XPath(CLOSE_TOUR_BUTTON)).Click();
		}

		private bool isTourOpen()
		{
			Logger.Trace("Проверка, что окно инструкции открыто");
			return GetIsElementDisplay(By.XPath(CLOSE_TOUR_BUTTON));
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
		protected const string CLOSE_TOUR_BUTTON = "//div[@class='hopscotch-bubble animated']//button[contains(@class, 'cta')]";
	}
}