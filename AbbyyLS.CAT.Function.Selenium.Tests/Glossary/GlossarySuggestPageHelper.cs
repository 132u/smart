using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы предложенных терминов глоссария
	/// </summary>
	public class GlossarySuggestPageHelper : CommonHelper
	{
		public GlossarySuggestPageHelper(IWebDriver driver, WebDriverWait wait)
			: base(driver, wait)
		{
			buttonsDict = new Dictionary<BUTTON_ID, string>
			{
				{BUTTON_ID.AcceptSuggestTerm, BUTTON_ACCEPT_XPATH},
				{BUTTON_ID.EditSuggestTerm, BUTTON_EDIT_XPATH},
				{BUTTON_ID.RejectSuggestTerm, BUTTON_REJECT_XPATH}
			};
		}

		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(SUGGEST_TABLE_XPATH));
		}

		public void OpenCurrentGlossary()
		{
			Logger.Debug("Открыть текущий глоссарий");
			ClickElement(By.XPath(CURRENT_GLOSSARY_REF_XPATH));
		}

		public int GetSuggestTermsCount()
		{
			Logger.Debug("Получить количество предложенных терминов");
			SetDriverTimeoutMinimum();
			var count = GetElementsCount(By.XPath(TERM_ROW_XPATH));
			SetDriverTimeoutDefault();

			return count;
		}

		public int GetSuggestTermsCurrentGlossaryCount(string glossaryName)
		{
			Logger.Trace("Получить список имен глоссариев");
			var glossaryNameList = GetTextListElement(By.XPath(ROW_GLOSSARY_NAME_XPATH));

			Logger.Debug(string.Format("Вернуть количество терминов с глоссарием {0}", glossaryName));
			return glossaryNameList.Count(glName => glName.Trim() == glossaryName);
		}

		public int GetTermRowNumberByGlossaryName(string glossaryName)
		{
			Logger.Debug(string.Format("Получить номер строки термина с глоссарием {0}", glossaryName));

			Logger.Trace("Получить список имен глоссариев");
			var glossaryNameList = GetTextListElement(By.XPath(ROW_GLOSSARY_NAME_XPATH));
			var rowNumber = 0;

			for (var i = 0; i < glossaryNameList.Count; ++i)
			{
				if (glossaryNameList[i].Trim() == glossaryName)
				{
					rowNumber = i + 1;
				}
			}

			return rowNumber;
		}

		public void SelectRow(int rowNumber)
		{
			Logger.Trace(string.Format("Выделить строку #{0}", rowNumber));

			// Если список длинный - первый клик прокручивает страницу
			ClickElement(By.XPath(GetRowXPath(rowNumber)));
			HoverElement(By.XPath(GetRowXPath(rowNumber)));
		}

		public void ClickRowButton(int rowNumber, BUTTON_ID btn)
		{
			Logger.Debug(string.Format("Нажать на кнопку {0} в строке #{1}", btn, rowNumber));
			ClickElement(By.XPath(GetRowXPath(rowNumber) + "//a[contains(@class, '" + buttonsDict[btn] + "')]"));
		}

		public void WaitChooseGlossaryForm()
		{
			Logger.Trace("Проверка появления формы выбора глоссария");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(CHOOSE_GLOSSARY_FORM_XPATH)),
				"Ошибка: форма выбора глоссария не появилась");
		}

		public void ClickChooseGlossaryFormDropdownGlossaryList()
		{
			Logger.Debug("В форме назначения глоссария нажать кнопку выпадения списка глоссариев");
			ClickElement(By.XPath(CLICK_GLOSSARY_LIST_DROPDOWN));
		}

		public void ClickDropdown()
		{
			Logger.Debug("Нажать конпку выпадения списка");
			ClickElement(By.XPath(DROPDOWN_XPATH));
		}

		public void SelectDropdownItem(string item)
		{
			Logger.Debug(string.Format("Выбрать элемент {0} выпадающего списка", item));
			ClickElement(By.XPath(DROPDOWN_ITEM + "[contains(text(),'" + item + "')]"));
		}

		public void ClickOkChooseGlossary()
		{
			Logger.Debug("Нажать конпку Ok при выборе глоссария");
			ClickElement(By.XPath(CHOOSE_GLOSSARY_OK_BTN_XPATH));
		}

		public void AssertionEditTermFillAppear()
		{
			Logger.Trace("Проверить, что окно редактирования термина открыто");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(EDIT_TERM_BOX), 30),
				"Ошибка: окно редактирования термина не открылось");
		}

		public void FillEditTermItem(int itemNumber, string text)
		{
			Logger.Debug(string.Format("При редактировании термина заполнить текст. Номер языка: {0}, текст: {1}", itemNumber, text));
			ClearAndAddText(By.XPath(EDIT_TERM_BOX + "[" + itemNumber + "]" + EDITOR_INPUT_XPATH), text);
		}

		public void ClickEditTermItem(int itemNumber)
		{
			Logger.Debug(string.Format("При редактировании термина - кликнуть по термину. Номер языка: {0}", itemNumber));
			ClickElement(By.XPath(EDIT_TERM_BOX + "[" + itemNumber + "]" + VIEWER_XPATH));
		}

		public void ClickAddSynonymEditTerm(int itemNumber)
		{
			Logger.Debug(string.Format("Кликнуть Добавить синоним при редактирования термина. Номер языка: {0}", itemNumber));
			ClickElement(By.XPath(EDIT_TERM_BOX + "[" + itemNumber + "]" + ADD_SYNONYM_XPATH));
		}

		public void ClickSaveTerm()
		{
			Logger.Debug("Кликнуть кнопку добавления термина");
			ClickElement(By.XPath(SAVE_EDIT_TERM_XPATH));
		}

		/// <summary>
		/// Дождаться окончания сохранения термина.
		/// </summary>
		public void WaitSaveTerm()
		{
			WaitUntilDisappearElement(By.XPath(SAVE_EDIT_TERM_XPATH));
		}

		public void AssertionEditTermFillDisappear()
		{
			Logger.Trace("Проверка закрытия формы управления терминами");

			Assert.IsTrue(WaitUntilDisappearElement(By.XPath(EDIT_TERM_BOX), 30),
				"Ошибка: форма управления терминами не закрылась");
		}

		public void ClickAcceptTerm()
		{
			Logger.Trace("Кликнуть кнопку Принять термин");
			ClickElement(By.XPath(ACCEPT_TERM_XPATH));
		}

		/// <summary>
		/// Получить xPath строки
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <returns>xPath</returns>
		protected string GetRowXPath(int rowNumber)
		{
			return TERM_ROW_XPATH + "[" + rowNumber + "]" + "//td";
		}

		public enum BUTTON_ID { AcceptSuggestTerm, EditSuggestTerm, RejectSuggestTerm };

		protected Dictionary<BUTTON_ID, string> buttonsDict;
		protected const string BUTTON_ACCEPT_XPATH = "js-accept-suggest";
		protected const string BUTTON_EDIT_XPATH = "js-edit-suggest";
		protected const string BUTTON_REJECT_XPATH = "js-reject-suggest";

		protected const string SUGGEST_TABLE_XPATH = "//table[contains(@class,'js-suggests')]";
		protected const string CURRENT_GLOSSARY_REF_XPATH = "//a[contains(@href,'/Enterprise/Concepts')]";
		protected const string TERM_ROW_XPATH = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))]";
		protected const string ROW_GLOSSARY_NAME_XPATH = TERM_ROW_XPATH + "//td[contains(@class, 'js-glossary-cell')]//p";

		protected const string CHOOSE_GLOSSARY_FORM_XPATH = "//div[contains(@class,'js-select-glossary-popup')]";
		protected const string DROPDOWN_XPATH = "//span[contains(@class,'js-dropdown')]";
		protected const string CLICK_GLOSSARY_LIST_DROPDOWN = CHOOSE_GLOSSARY_FORM_XPATH + DROPDOWN_XPATH;
		protected const string DROPDOWN_ITEM = "//span[contains(@class,'js-dropdown__item')]";
		protected const string CHOOSE_GLOSSARY_OK_BTN_XPATH = "//input[contains(@class, 'js-glossary-selected-button')]";

		protected const string EDIT_TERM_BOX = "//div[contains(@class,'l-corprtree__langbox')]";
		protected const string EDITOR_INPUT_XPATH = "//span[contains(@class,'js-term-editor')]//input";
		protected const string VIEWER_XPATH = "//span[contains(@class,'js-term-viewer')]";
		protected const string SAVE_EDIT_TERM_XPATH = "//a[contains(@class, 'js-save-btn')]";
		protected const string ADD_SYNONYM_XPATH = "//span[contains(@class,'js-add-term')]";
		protected const string NEW_TERM_ADDED_XPATH = "//div[@class ='l-corprtree']/div[1]/div[3]/div[3]/span/span[contains(@class,'js-text') and text()='";
		protected const string ACCEPT_TERM_XPATH = "//span[@class = 'js-save-text']";
	}
}