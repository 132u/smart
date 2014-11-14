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

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы предложенных терминов глоссария
	/// </summary>
	public class GlossarySuggestPageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public GlossarySuggestPageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
			buttonsDict = new Dictionary<BUTTON_ID, string>();
			buttonsDict.Add(BUTTON_ID.AcceptSuggestTerm, BUTTON_ACCEPT_XPATH);
			buttonsDict.Add(BUTTON_ID.EditSuggestTerm, BUTTON_EDIT_XPATH);
			buttonsDict.Add(BUTTON_ID.RejectSuggestTerm, BUTTON_REJECT_XPATH);
		}

		/// <summary>
		/// Дождаться загрузки страницы
		/// </summary>
		/// <returns></returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(SUGGEST_TABLE_XPATH));
		}

		/// <summary>
		/// Открыть текущий глоссарий
		/// </summary>
		public void OpenCurrentGlossary()
		{
			ClickElement(By.XPath(CURRENT_GLOSSARY_REF_XPATH));
		}

		/// <summary>
		/// Вернуть количество предложенных терминов
		/// </summary>
		/// <returns>количество</returns>
		public int GetSuggestTermsCount()
		{
			SetDriverTimeoutMinimum();
			int count = GetElementsCount(By.XPath(TERM_ROW_XPATH));
			SetDriverTimeoutDefault();
			return count;
		}

		/// <summary>
		/// Вернуть количество терминов с нужным глоссарием
		/// </summary>
		/// <returns>количество</returns>
		public int GetSuggestTermsCurrentGlossaryCount(string glossaryName)
		{
			List<string> glossaryNameList = GetTextListElement(By.XPath(ROW_GLOSSARY_NAME_XPATH));
			int glossaryCount = 0;
			foreach (string glName in glossaryNameList)
			{
				if (glName.Trim() == glossaryName)
				{
					++glossaryCount;
				}
			}

			return glossaryCount;
		}

		/// <summary>
		/// Вернуть номер строки термина с нужным глоссарием
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		/// <returns>номер термина</returns>
		public int GetTermRowNumberByGlossaryName(string glossaryName)
		{
			List<string> glossaryNameList = GetTextListElement(By.XPath(ROW_GLOSSARY_NAME_XPATH));
			int rowNumber = 0;
			for (int i = 0; i < glossaryNameList.Count; ++i)
			{
				if (glossaryNameList[i].Trim() == glossaryName)
				{
					rowNumber = i + 1;
				}
			}

			return rowNumber;
		}

		/// <summary>
		/// Выделить строку
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public void SelectRow(int rowNumber)
		{
			// Если списко длинный - первый клик прокручивает страницу
			ClickElement(By.XPath(GetRowXPath(rowNumber)));
			// А второй клик выделяет
			ClickElement(By.XPath(GetRowXPath(rowNumber)));
		}

		/// <summary>
		/// Нажать на кнопку в строке
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <param name="btn">id кнопки</param>
		public void ClickRowButton(int rowNumber, BUTTON_ID btn)
		{
			ClickElement(By.XPath(GetRowXPath(rowNumber) + "//a[contains(@class, '" + buttonsDict[btn] + "')]"));
		}

		/// <summary>
		/// Дождаться появления форму выбора глоссария
		/// </summary>
		/// <returns>появилась</returns>
		public bool WaitChooseGlossaryForm()
		{
			return WaitUntilDisplayElement(By.XPath(CHOOSE_GLOSSARY_FORM_XPATH));
		}

		/// <summary>
		/// В форме назначения глоссария нажать для выпадения списка глоссариев
		/// </summary>
		public void ClickChooseGlossaryFormDropdownGlossaryList()
		{
			ClickElement(By.XPath(CLICK_GLOSSARY_LIST_DROPDOWN));
		}

		/// <summary>
		/// Кликнуть для выпадения списка
		/// </summary>
		public void ClickDropdown()
		{
			ClickElement(By.XPath(DROPDOWN_XPATH));
		}

		/// <summary>
		/// Выбрать элемент выпадающего списка
		/// </summary>
		/// <param name="item">текст элемента</param>
		public void SelectDropdownItem(string item)
		{
			ClickElement(By.XPath(DROPDOWN_ITEM + "[contains(text(),'" + item + "')]"));
		}

		/// <summary>
		/// Кликнуть Ok при выборе глоссария
		/// </summary>
		public void ClickOkChooseGlossary()
		{
			ClickElement(By.XPath(CHOOSE_GLOSSARY_OK_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться открытия редактирования термина
		/// </summary>
		/// <returns>открылось</returns>
		public bool WaitEditTermFillAppear()
		{
			return WaitUntilDisplayElement(By.XPath(EDIT_TERM_BOX), 30);
		}

		/// <summary>
		/// Дождаться закрытия редактирования термина
		/// </summary>
		/// <returns>закрылось</returns>
		public bool WaitUntilEditTermFillDisappear()
		{
			return WaitUntilDisappearElement(By.XPath(EDIT_TERM_BOX), 30);
		}

		/// <summary>
		/// При редактировании термина заполнить текст
		/// </summary>
		/// <param name="itemNumber">номер языка</param>
		/// <param name="text">текст</param>
		public void FillEditTermItem(int itemNumber, string text)
		{
			ClearAndAddText(By.XPath(EDIT_TERM_BOX + "[" + itemNumber + "]" + EDITOR_INPUT_XPATH), text);
		}

		/// <summary>
		/// При редактировании термина - кликнуть по термину
		/// </summary>
		/// <param name="itemNumber">номер языка</param>
		public void ClickEditTermItem(int itemNumber)
		{
			ClickElement(By.XPath(EDIT_TERM_BOX + "[" + itemNumber + "]" + VIEWER_XPATH));
		}

		/// <summary>
		/// Кликнуть Добавить синоним при редактирования термина
		/// </summary>
		/// <param name="itemNumber">номер языка</param>
		public void ClickAddSynonymEditTerm(int itemNumber)
		{
			ClickElement(By.XPath(EDIT_TERM_BOX + "[" + itemNumber + "]" + ADD_SYNONYM_XPATH));
		}

		/// <summary>
		/// Кликнуть сохранить при изменении термина
		/// </summary>
		public void ClickSaveEditTerm()
		{
			ClickElement(By.XPath(SAVE_EDIT_TERM_XPATH));
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
		protected const string SAVE_EDIT_TERM_XPATH = "//span[contains(@class,'js-save-btn')]";
		protected const string ADD_SYNONYM_XPATH = "//span[contains(@class,'js-add-term')]";
	}
}