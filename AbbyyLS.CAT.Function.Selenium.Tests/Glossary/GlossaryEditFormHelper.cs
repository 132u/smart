﻿using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Linq;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер формы редактирования глоссария
	/// </summary>
	public class GlossaryEditFormHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public GlossaryEditFormHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
		}

		/// <summary>
		/// Дождаться открытия формы
		/// </summary>
		/// <returns></returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(DIALOG_XPATH));
		}

		/// <summary>
		/// Дождаться закрытия формы
		/// </summary>
		/// <returns>закрылась</returns>
		public bool WaitPageClose()
		{
			return WaitUntilDisappearElement(By.XPath(DIALOG_XPATH));
		}

		/// <summary>
		/// Ввести название глоссария
		/// </summary>
		/// <param name="name">название</param>
		public void EnterGlossaryName(string name)
		{
			ClearAndAddText(By.XPath(GLOSSARY_NAME_XPATH), name);
		}

		/// <summary>
		/// Ввести комментаний для глоссария
		/// </summary>
		/// <param name="comment">комментарий</param>
		public void EnterComment(string comment)
		{
			SendTextElement(By.XPath(GLOSSARY_COMMENT_XPATH), comment);
		}

		/// <summary>
		/// Кликнуть Сохранить
		/// </summary>
		public void ClickSaveGlossary()
		{
			ClickElement(By.XPath(GLOSSARY_SAVE_XPATH));
		}

		/// <summary>
		/// Открыть список клиентов
		/// </summary>
		/// <returns>>открылся</returns>
		public bool ClickOpenClientList()
		{
			ClickElement(By.XPath(CLIENT_LIST_XPATH));

			return WaitUntilDisplayElement(By.XPath(DROPDOWNLIST_XPATH));
		}

		public void ClickOpenDomainList()
		{
			Logger.Debug("Открыть список доменов");
			ClickElement(By.XPath(DOMAIN_LIST_XPATH));

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(MULTISELECT_LIST_XPATH)),
				"Ошибка: спсиок доменов не открылся");
		}

		/// <summary>
		/// Вернуть: есть ли клиент в списке
		/// </summary>
		/// <param name="clientName">название</param>
		/// <returns>есть</returns>
		public bool GetIsClientInList(string clientName)
		{
			// Получить список клиентов
			var clientList = GetElementList(By.XPath(DROPDOWNLIST_ITEM_XPATH));
			return clientList.Any(e => e.GetAttribute("innerHTML") == clientName);
		}

		/// <summary>
		/// Вернуть: есть ли domain в списке
		/// </summary>
		/// <param name="domainName">название</param>
		/// <returns>есть</returns>
		public bool GetIsDomainInList(string domainName)
		{
			// Получить список клиентов
			var DomainList = GetElementList(By.XPath(MULTISELECT_LIST_XPATH));
			var isDomainExist = false;

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
		/// Кликнуть Add для добавления языка
		/// </summary>
		public void ClickAddLanguage()
		{
			ClickElement(By.XPath(ADD_LANG_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть для открытия/закрытия списка языка для последнего языка
		/// </summary>
		public void ClickLastLangOpenCloseList()
		{
			ClickElement(By.XPath(LAST_LANG_LIST_DROPDOWN_XPATH));
		}

		public void AssertionIsLanguageExistInList(LANGUAGE lang)
		{
			Logger.Trace(string.Format("Проверка наличия языка {0} в списке", lang));
			
			var elementXPath = DROPDOWNLIST_XPATH + "/span[@data-id='" + languageID[lang] + "']";

			Assert.IsTrue(
				GetIsElementExist(By.XPath(elementXPath)),
				"Ошибка: указанного языка нет в списке");
		}

		/// <summary>
		/// Выбрать язык в выпадающем списке
		/// </summary>
		/// <param name="lang">язык</param>
		public void SelectLanguage(LANGUAGE lang)
		{
			//Делаем видимым нужный элемент из списка
			var lang_item = Driver.FindElement(By.XPath(DROPDOWNLIST_XPATH + "//span[@data-id='" + languageID[lang] + "']"));
			((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", lang_item);

			ClickElement(By.XPath(DROPDOWNLIST_XPATH + "//span[@data-id='" + languageID[lang] + "']"));

			WaitUntilDisappearElement(By.XPath(DROPDOWNLIST_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли язык в списке для добавления языка
		/// </summary>
		/// <param name="lang">язык</param>
		/// <returns>есть</returns>
		public bool GetIsExistLanguageInLangList(LANGUAGE lang)
		{
			return GetIsElementDisplay(By.XPath(DROPDOWNLIST_XPATH + 
				"//span[@data-id='" + languageID[lang] + "']"));
		}

		/// <summary>
		/// Вернуть количество языков в глоссарии
		/// </summary>
		/// <returns>количество</returns>
		public int GetGlossaryLanguageCount()
		{
			return GetElementsCount(By.XPath(LANG_LIST_LANG_XPATH));
		}

		/// <summary>
		/// Кликнуть Удалить язык
		/// </summary>
		public void ClickDeleteLanguage()
		{
			ClickElement(By.XPath(DELETE_LANG_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли предупреждение об удалении языка, на котором уже есть термины
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsExistWarningDeleteLanguage()
		{
			return GetIsElementDisplay(By.XPath(WARNING_DELETE_LANGUAGE_XPATH));
		}

		/// <summary>
		/// Отменить удаление языка
		/// </summary>
		public void CancelDeleteLanguage()
		{
			ClickElement(By.XPath(DELETE_LANGUAGE_CANCEL_XPATH));
		}

		/// <summary>
		/// Кликнуть Удалить глоссарий
		/// </summary>
		public void ClickDeleteGlossary()
		{
			ClickElement(By.XPath(DELETE_GLOSSARY_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли предупреждение об удалении глоссария
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsExistWarningDeleteGlossary()
		{
			return GetIsElementDisplay(By.XPath(WARNING_DELETE_GLOSSARY_XPATH));
		}

		/// <summary>
		/// Подтвердить удаление глоссария
		/// </summary>
		public void ClickConfirmDeleteGlossary()
		{
			ClickElement(By.XPath(CONFIRM_DELETE_GLOSSARY_XPAHT));
		}

		/// <summary>
		/// В форме редактора глоссария кликнуть Сохранить и Изменить Структуру
		/// </summary>
		public void ClickSaveAndEditStructureBtn()
		{
			ClickElement(By.XPath(SAVE_AND_EDIT_STRUCTURE_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть, появилось ли сообщение об ошибке (пустое имя глоссария)
		/// </summary>
		/// <returns>появилось</returns>
		public bool GetIsExistErrorMessageEmptyGlossaryName()
		{
			return GetIsElementDisplay(By.XPath(ERROR_EMPTY_NAME_XPATH));
		}

		/// <summary>
		/// Вернуть, появилось ли сообщение об ошибке (существующее имя глоссария)
		/// </summary>
		/// <returns>появилось</returns>
		public bool GetIsExistErrorMessageExistGlossaryName()
		{
			return GetIsElementDisplay(By.XPath(ERROR_EXIST_NAME_XPATH));
		}



		protected const string DIALOG_XPATH = ".//div[contains(@class,'js-popup-edit-glossary')][2]";
		protected const string GLOSSARY_NAME_XPATH = DIALOG_XPATH + "//input[@class='g-bold l-editgloss__nmtext']";
		protected const string GLOSSARY_COMMENT_XPATH = DIALOG_XPATH + "//div[@class='l-editgloss__cont last']//textarea";
		protected const string GLOSSARY_SAVE_XPATH = DIALOG_XPATH + "//span[@class='g-btn g-redbtn ']";
		protected const string SAVE_AND_EDIT_STRUCTURE_BTN_XPATH = DIALOG_XPATH + "//a[contains(@data-bind,'click: saveAndEditStructure')]";

		protected const string CLIENT_LIST_XPATH = "//select[contains(@data-bind,'clientsList')]//following-sibling::span";
		protected const string DOMAIN_LIST_XPATH = DIALOG_XPATH + "//div[@class='l-editgloss__contrbox'][3]//div";

		protected const string DROPDOWNLIST_XPATH = "//body/span[contains(@class,'js-dropdown')]";
		protected const string DROPDOWNLIST_ITEM_XPATH = "//select[contains(@data-bind,'clientsList')]/option";
		protected const string MULTISELECT_LIST_XPATH = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')]";

		protected const string ADD_LANG_BTN_XPATH = DIALOG_XPATH + "//span[@class='g-btn g-bluebtn addlang enabled']";
		protected const string LANG_XPATH = "//span[contains(@class,'js-glossary-language')]";
		protected const string LAST_LANG_LIST_DROPDOWN_XPATH = DIALOG_XPATH + "//span[@class='g-btn g-bluebtn addlang enabled']//preceding-sibling::span[@class='g-iblock l-editgloss__control l-editgloss__lang'][1]//span/span";
		protected const string LANGUAGES_XPATH = "//div[@class='l-editgloss__contrbox'][1]";
		protected const string LANG_LIST_LANG_XPATH = "//div[@class='l-editgloss__contrbox'][1]//span[@class='g-iblock l-editgloss__control l-editgloss__lang']";
		protected const string DELETE_LANG_XPATH = DIALOG_XPATH + LANGUAGES_XPATH + "//em";
		protected const string WARNING_DELETE_LANGUAGE_XPATH = DIALOG_XPATH + "//p[contains(@data-message-id, 'language-deleted-warning')]";
		protected const string DELETE_LANGUAGE_CANCEL_XPATH = DIALOG_XPATH + "//a[contains(@data-bind, 'click: undoDeleteLanguage')]";

		protected const string DELETE_GLOSSARY_XPATH = DIALOG_XPATH + "//span[contains(@data-bind, 'click: deleteGlossary')]";
		protected const string WARNING_DELETE_GLOSSARY_XPATH = DIALOG_XPATH + "//p[contains(@data-message-id, 'confirm-delete-glossary')]";
		protected const string CONFIRM_DELETE_GLOSSARY_XPAHT = DIALOG_XPATH + "//a[contains(@data-bind, 'click: deleteGlossary')]";

		protected const string ERROR_EMPTY_NAME_XPATH = DIALOG_XPATH + "//p[contains(@data-message-id,'glossary-name-required')]";
		protected const string ERROR_EXIST_NAME_XPATH = DIALOG_XPATH + "//p[contains(@data-message-id,'glossary-exists')]";
	}
}