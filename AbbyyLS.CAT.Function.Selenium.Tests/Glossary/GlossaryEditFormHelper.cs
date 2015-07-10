using System.Collections.Generic;
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
		public GlossaryEditFormHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
		}

		public void AssertionPageLoad()
		{
			Logger.Trace("Проверка открытия диалога создания глоссария");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(DIALOG_XPATH)),
					"Ошибка: диалог создания глоссария не открылся");
		}

		public void EnterGlossaryName(string name)
		{
			Logger.Debug(string.Format("Ввести имя глоссария: {0}", name));
			ClearAndAddText(By.XPath(GLOSSARY_NAME_XPATH), name);
		}

		public void EnterComment(string comment)
		{
			Logger.Debug(string.Format("Ввести комментарий {0} для глоссария", comment));
			SendTextElement(By.XPath(GLOSSARY_COMMENT_XPATH), comment);
		}

		public void ClickSaveGlossary()
		{
			Logger.Debug("Нажать кнопку сохранения глоссария");
			ClickElement(By.XPath(GLOSSARY_SAVE_XPATH));
		}

		public void ClickAddLanguage()
		{
			Logger.Debug("Кликнуть кнопку Add для добавления языка");
			ClickElement(By.XPath(ADD_LANG_BTN_XPATH));
		}

		public void ClickLastLangOpenCloseList()
		{
			Logger.Debug("Кликнуть для открытия/закрытия списка для последнего языка");
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

		public void SelectLanguage(LANGUAGE lang)
		{
			Logger.Debug(string.Format("Выбираем язык {0} в выпадающем списке", lang));

			//Делаем видимым нужный элемент из списка
			var langItem = Driver.FindElement(By.XPath(DROPDOWNLIST_XPATH + "//span[@data-id='" + languageID[lang] + "']"));
			((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", langItem);

			ClickElement(By.XPath(DROPDOWNLIST_XPATH + "//span[@data-id='" + languageID[lang] + "']"));

			WaitUntilDisappearElement(By.XPath(DROPDOWNLIST_XPATH));
		}

		public int GetGlossaryLanguageCount()
		{
			Logger.Trace("Получить количество языков в глоссарии");
			return GetElementsCount(By.XPath(LANG_LIST_LANG_XPATH));
		}

		public void ClickDeleteLanguage()
		{
			Logger.Debug("Нажать кнопку удаления языка");
			ClickElement(By.XPath(DELETE_LANG_XPATH));
		}

		public void ClickDeleteGlossary()
		{
			Logger.Debug("Нажать кнопку удаления глоссария");
			ClickElement(By.XPath(DELETE_GLOSSARY_XPATH));
		}

		public void ClickConfirmDeleteGlossary()
		{
			Logger.Debug("Нажать кнопку подтверждения удаления глоссария");
			ClickElement(By.XPath(CONFIRM_DELETE_GLOSSARY_XPATH));
		}

		public void WaitUntilDeleteGlossaryButtonDisplay()
		{
			Logger.Debug("Дождаться появления кнопки подтверждения удаления глоссария");
			
			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(CONFIRM_DELETE_GLOSSARY_XPATH)),
				"Ошибка: не появилась кнопка подтверждения удаления глоссария.");
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
		protected const string DELETE_LANGUAGE_CANCEL_XPATH = DIALOG_XPATH + "//a[contains(@data-bind, 'click: undoDeleteLanguage')]";

		protected const string DELETE_GLOSSARY_XPATH = DIALOG_XPATH + "//span[contains(@data-bind, 'click: deleteGlossary')]";
		protected const string CONFIRM_DELETE_GLOSSARY_XPATH = DIALOG_XPATH + "//a[contains(@data-bind, 'click: deleteGlossary')]";

		protected const string ERROR_EMPTY_NAME_XPATH = DIALOG_XPATH + "//p[contains(@data-message-id,'glossary-name-required')]";
		protected const string ERROR_EXIST_NAME_XPATH = DIALOG_XPATH + "//p[contains(@data-message-id,'glossary-exists')]";
	}
}