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

namespace AbbyyLs.CAT.Function.Selenium.Tests
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
			TMButtonDict = new Dictionary<TM_BTN_TYPE,string>();
			TMButtonDict.Add(TM_BTN_TYPE.Update, UPDATE_BTN_XPATH);
			TMButtonDict.Add(TM_BTN_TYPE.Export, EXPORT_BTN_XPATH);
			TMButtonDict.Add(TM_BTN_TYPE.Delete, DELETE_BTN_XPATH);
			TMButtonDict.Add(TM_BTN_TYPE.Add, ADD_TMX_BTN_XPATH);
			TMButtonDict.Add(TM_BTN_TYPE.Edit, EDIT_BTN_XPATH);
			TMButtonDict.Add(TM_BTN_TYPE.Save, SAVE_BTN_XPATH);
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
			IList<IWebElement> clientList = GetElementList(By.XPath(CREATE_TM_CLIENT_LIST_XPATH + CREATE_TM_CLIENT_ITEM_XPATH));
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

		/// <summary>
		/// Ввести имя Тм в форме редактирования ТМ
		/// </summary>
		/// <param name="TMName">название ТМ</param>
		public void InputEditTMName(string TMName)
		{
			SendTextElement(By.XPath(TM_EDIT_NAME_XPATH), TMName);
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
		/// Подтвердить удаление
		/// </summary>
		public void ConfirmDelete()
		{// TODO убрать Wait
			WaitUntilDisplayElement(By.XPath(CONFIRM_XPATH));
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
		/// Вернуть XPath строки с ТМ
		/// </summary>
		/// <param name="TMName">название ТМ</param>
		/// <returns>xPath</returns>
		protected string GetTMRow(string TMName)
		{
			return TM_ROW_XPATH + "[text()='" + TMName + "']/parent::td/parent::tr";
		}



		public enum TM_BTN_TYPE { Update, Export, Delete, Add, Edit, Save };

		protected const string ADD_TM_BTN_XPATH = ".//span[contains(@class,'js-create-tm')]";

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

		protected const string TM_ROW_XPATH = "//tr[contains(@class, 'js-tm-row')]/td/span";

		protected const string BTN_ROW_XPATH = "//tr[@class='js-tm-panel']";
		protected const string UPDATE_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@class,'js-upload-btn')]//a";
		protected const string EXPORT_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@class,'js-export-btn')]//a";
		protected const string DELETE_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@class,'js-delete-btn')]";
		protected const string ADD_TMX_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@class,'js-add-tmx-btn')]";
		protected const string EDIT_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@class,'js-edit-btn')]";
		protected const string SAVE_BTN_XPATH = BTN_ROW_XPATH + "//span[contains(@class,'js-save-btn')]";
		// TODO заменить id
		protected const string SEGMENT_SPAN_XPATH = BTN_ROW_XPATH + "//table[@class='l-tmpanel__table']//div[4]";
		protected const string IMPORT_POPUP_XPATH = "//div[contains(@class,'js-popup-import')][2]";
		protected const string IMPORT_BTN_XPATH = IMPORT_POPUP_XPATH + "//span[contains(@class,'js-import-button')]";

		protected const string OPEN_SRC_LANG_CREATE_TM_XPATH = CREATE_TM_DIALOG_XPATH + "//span[contains(@class,'l-createtm__srclnl_drpdwn')]";
		protected const string OPEN_TRG_LANG_CREATE_TM_XPATH = CREATE_TM_DIALOG_XPATH + "//div[contains(@class,'js-languages-multiselect')]";

		protected const string SOURCE_LANG_ITEM_XPATH = "//span[contains(@class,'js-dropdown__item')][@data-id='";
		protected const string TARGET_LANG_ITEM_XPATH = "//div[contains(@class,'ui-multiselect-menu') and contains(@class,'js-languages-multiselect')]//li//input[@value='";

		protected const string NEW_TM_NAME_XPATH = CREATE_TM_DIALOG_XPATH + "//input[contains(@class,'js-tm-name')]";
		protected const string SAVE_TM_BTN_XPATH = CREATE_TM_DIALOG_XPATH + "//span[contains(@class,'js-save')]";

		protected const string TM_ROW_NAME = "//tr[contains(@class,'js-tm-row')]//td/span";
		protected const string TM_EDIT_FORM_XPATH = "//tr[contains(@class,'js-tm-panel js-editing')]";
		protected const string TM_EDIT_NAME_XPATH = TM_EDIT_FORM_XPATH + "//input[contains(@class, 'js-tm-name')]";
		protected const string TM_EDIT_SAVE_BTN_XPATH = TM_EDIT_FORM_XPATH + "//span[contains(@class,'js-save-btn')]";

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

		protected Dictionary<TM_BTN_TYPE, string> TMButtonDict;
	}
}