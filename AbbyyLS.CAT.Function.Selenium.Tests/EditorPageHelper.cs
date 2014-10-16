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
using System.Windows.Forms;
using System.Threading;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы редактора
	/// </summary>
	public class EditorPageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public EditorPageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
			CATTypeDict = new Dictionary<CAT_TYPE, string>();
			CATTypeDict.Add(CAT_TYPE.MT, "MT");
			CATTypeDict.Add(CAT_TYPE.TM, "TM");
			CATTypeDict.Add(CAT_TYPE.TB, "TB");
		}

		/// <summary>
		/// Дождаться, пока загрузится страница
		/// </summary>
		public bool WaitPageLoad()
		{
			// Дождаться загрузки страницы
			return Wait.Until((d) => d.Url.Contains(TITLE_TEXT));
		}

		/// <summary>
		/// Возвращает задачу
		/// </summary>
		/// <returns>Задача</returns>
		public string GetStageName()
		{
			string stage = "";

			if (GetIsElementExist(By.XPath(STAGE_NAME_XPATH)))
				stage = GetTextElement(By.XPath(STAGE_NAME_XPATH));

			return stage;
		}

		/// <summary>
		/// Получить, есть ли сегменты
		/// </summary>
		/// <returns>есть сегменты</returns>
		public bool GetSegmentsExist()
		{
			return GetIsElementExist(By.CssSelector(SEGMENTS_CSS));
		}

		/// <summary>
		/// Ввести текст в target
		/// </summary>
		/// <param name="rowNum">номер строки</param>
		/// <param name="text">текст</param>
		public void AddTextTarget(int rowNum, string text)
		{
			ClickClearAndAddText(By.CssSelector(GetTargetCellCss(rowNum)), text);
			Console.WriteLine("добавили текст: " + text);
		}

		/// <summary>
		/// Открыть контекстное меню для спелчека и выбрать вариант по номеру
		/// </summary>
		/// <param name="rowNum">Номер сегмента</param>
		/// <param name="variantRow">Номер варианта из списка</param>
		/// <returns>Строка выбора</returns>
		public string RightClickSpellcheck(int rowNum, int variantRow)
		{
			string xPath = SEGMENT_ROW_XPATH + "[" + rowNum + "]" +
				SPELLCHECK_TARGET_XPATH;
			string result = "";

			var actions = new OpenQA.Selenium.Interactions.Actions(Driver);
			actions.MoveToElement(GetElement(By.XPath(xPath)), 1, 1).ContextClick().Build().Perform();

			IList<IWebElement> elements = GetElementList(By.XPath(CONTEXT_MENU_SPELLCHECK_ADD_XPATH + "/../../div[" + variantRow + "]/span"));

			foreach (IWebElement element in elements)
			{
				if (element.Enabled && element.Displayed)
				{
					result = element.Text;
					element.Click();
				}
			}

			return result;
		}

		/// <summary>
		/// Очистить таргет
		/// </summary>
		/// <param name="rowNum">номер строки</param>	   
		public void ClearTarget(int rowNum)
		{
			ClearElement(By.CssSelector((GetTargetCellCss(rowNum))));
		}

		/// <summary>
		/// Отправить keys в target
		/// </summary>
		/// <param name="row">номер строки</param>
		/// <param name="keys">keys</param>
		public void SendKeysTarget(int row, string keys)
		{
			ClickAndSendTextElement(By.CssSelector(GetTargetCellCss(row)), keys);
		}

		/// <summary>
		/// Отправить keys в source
		/// </summary>
		/// <param name="row">номер строки</param>
		/// <param name="keys">keys</param>
		public void SendKeysSource(int row, string keys)
		{
			ClickAndSendTextElement(By.CssSelector(GetSourceCellCss(row)), keys);
		}

		/// <summary>
		/// Кликнуть Confirm
		/// </summary>
		public void ClickConfirmBtn()
		{
			ClickElement(By.Id(CONFIRM_BTN_ID));
		}

		/// <summary>
		/// Кликнуть Undo
		/// </summary>
		public void ClickUndoBtn()
		{
			ClickElement(By.Id(UNDO_BTN_ID));
		}

		/// <summary>
		/// Кликнуть Redo
		/// </summary>
		public void ClickRedoBtn()
		{
			ClickElement(By.Id(REDO_BTN_ID));
		}

		/// <summary>
		/// Кликнуть Add Term
		/// </summary>
		public void ClickAddTermBtn()
		{
			ClickElement(By.Id(ADD_TERM_BTN_ID));
		}

		/// <summary>
		/// Кликнуть Unfinished
		/// </summary>
		public void ClickUnfinishedBtn()
		{
			ClickElement(By.Id(UNFINISHED_BTN_ID));
		}

		/// <summary>
		/// Кликнуть по Back
		/// </summary>
		public void ClickHomeBtn()
		{
			ClickElement(By.Id(HOME_BTN_ID));
		}

		/// <summary>
		/// Кликнуть по toggle
		/// </summary>
		public void ClickToggleBtn()
		{
			ClickElement(By.Id(TOGGLE_BTN_ID));
		}

		/// <summary>
		/// Кликнуть кнопку Copy
		/// </summary>
		public void ClickCopyBtn()
		{
			ClickElement(By.Id(COPY_BTN_ID));
		}

		/// <summary>
		/// Кликнуть по кнопке отката
		/// </summary>
		public void ClickRollbackBtn()
		{
			ClickElement(By.Id(ROLLBACK_BTN_ID));
		}

		/// <summary>
		/// Кликнуть вставить Tag
		/// </summary>
		public void ClickInsertTagBtn()
		{
			ClickElement(By.Id(INSERT_TAG_BTN_ID));
		}

		/// <summary>
		/// Кликнуть словарь
		/// </summary>
		public void ClickDictionaryBtn()
		{
			ClickElement(By.Id(DICTIONARY_BTN_ID));
		}

		/// <summary>
		/// Кликнуть поиск ошибки
		/// </summary>
		public void ClickFindErrorBtn()
		{
			ClickElement(By.Id(FIND_ERROR_BTN_ID));
		}

		/// <summary>
		/// Кликнуть Изменить регистр
		/// </summary>
		public void ClickChangeCaseBtn()
		{
			ClickElement(By.Id(CHANGE_CASE_BTN_ID));
		}

		/// <summary>
		/// Кликнуть конкордный поиск
		/// </summary>
		public void ClickConcordanceBtn()
		{
			ClickElement(By.Id(CONCORDANCE_BTN_ID));
		}

		/// <summary>
		/// Кликнуть кнопку вставки спецсимвола
		/// </summary>
		public void ClickCharacterBtn()
		{
			ClickElement(By.Id(CHARACTER_BTN_ID));
		}

		/// <summary>
		/// Кликнуть Source
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public void ClickSourceCell(int rowNumber)
		{
			ClickElement(By.CssSelector(GetSourceCellCss(rowNumber)));
		}

		/// <summary>
		/// Кликнуть Target
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public void ClickTargetCell(int rowNumber)
		{
			ClickElement(By.CssSelector(GetTargetCellCss(rowNumber)));
		}

		/// <summary>
		/// Получить текст Source
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <returns>текст</returns>
		public string GetSourceText(int rowNumber)
		{
			return GetTextElement(By.CssSelector(GetSourceCellCss(rowNumber))).Trim();
		}

		/// <summary>
		/// Получить текст Target
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <returns>текст</returns>
		public string GetTargetText(int rowNumber)
		{
			return GetTextElement(By.CssSelector(GetTargetCellCss(rowNumber))).Trim();
		}

		/// <summary>
		/// Возвращает, есть ли tag в переводе для заданного сегмента
		/// </summary>
		/// <param name="rowNumber">Номер сегмента</param>
		/// <returns>Tag есть</returns>
		public bool GetIsTagPresent(int rowNumber)
		{
			string xPath = SEGMENT_ROW_XPATH +
				"[" + rowNumber + "]" + TAG_TARGET_XPATH;

			return GetIsElementExist(By.XPath(xPath));
		}

		/// <summary>
		/// Получить элемент Source
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <returns>Элемент</returns>
		public IWebElement GetSource(int rowNumber)
		{
			return GetElement(By.CssSelector(GetSourceCellCss(rowNumber)));
		}

		/// <summary>
		/// Дождаться, пока сегмент подтвердится
		/// </summary>
		/// <param name="segmentRowNumber">номер сегмента</param>
		/// <returns>сегмент подтвердился</returns>
		public bool WaitSegmentConfirm(int segmentRowNumber)
		{
			string xPath = "";
			xPath = SEGMENT_ROW_XPATH + "[" + segmentRowNumber + "]//td[contains(@class,'" + INFO_COLUMN_CLASS + "')]//span[contains(@class,'" + CONFIRMED_ICO_CLASS + "')]";

			return WaitUntilDisplayElement(By.XPath(xPath));
		}

		/// <summary>
		/// Возвращает, отображается замок в инфо сегмента
		/// </summary>
		/// <param name="segmentRowNumber">номер сегмента</param>
		/// <returns>Сегмент заблокирован</returns>
		public bool GetIsSegmentLock(int segmentRowNumber)
		{
			string xPath = "";
			xPath = SEGMENT_ROW_XPATH + "[" + segmentRowNumber + "]//td[contains(@class,'" +
				INFO_COLUMN_CLASS + "')]//span[contains(@class,'" +
				LOCKED_ICO_CLASS + "')][not(contains(@class,'inactive'))]";

			return GetIsElementExist(By.XPath(xPath));
		}

		/// <summary>
		/// Возвращает, отображается ли Confirm сегмента
		/// </summary>
		/// <param name="segmentRowNumber">Номер сегмента</param>
		/// <returns>Сегмент подтвержден</returns>
		public bool GetIsSegmentConfirm(int segmentRowNumber)
		{
			string xPath = "";
			xPath = SEGMENT_ROW_XPATH + "[" + segmentRowNumber + "]//td[contains(@class,'" +
				INFO_COLUMN_CLASS + "')]//span[contains(@class,'" +
				CONFIRMED_ICO_CLASS + "')]";

			return GetIsElementExist(By.XPath(xPath));
		}

		/// <summary>
		/// Возвращает, заблокирована ли кнопка отката изменений сегмента
		/// </summary>
		/// <returns>Кнопка отката заблокирована</returns>
		public bool GetIsRollbackBtnLock()
		{
			return GetElementClass(By.Id(ROLLBACK_BTN_ID)).Contains("disabled");
		}

		/// <summary>
		/// Вернуть количество сегментов
		/// </summary>
		/// <returns>количество</returns>
		public int GetSegmentsNumber()
		{
			int segmentCount;
			segmentCount = GetElementList(By.CssSelector(SEGMENTS_CSS)).Count;

			Console.WriteLine("segmentCount: " + segmentCount);
			return segmentCount;
		}

		/// <summary>
		/// Получить, что панель CAT не пуста
		/// </summary>
		/// <returns>не пуста</returns>
		public bool GetCATPanelNotEmpty()
		{
			string xPath;
			xPath = CAT_PANEL_EXISTENCE_XPATH;

			// проверить, что вообще есть панель
			bool isNotEmpty = GetIsElementExist(By.Id(CAT_PANEL_ID));
			if (isNotEmpty)
			{
				// Проверить, что в панели есть содержимое
				isNotEmpty = GetIsElementExist(By.XPath(xPath));
			}
			return isNotEmpty;
		}

		/// <summary>
		/// Получить номер строки с нужным типом перевода
		/// </summary>
		/// <param name="type">тип</param>
		/// <returns>номер строки</returns>
		public int GetCATTranslationRowNumber(CAT_TYPE type)
		{
			int rowNum = 0;
			// Список текстов
			List<string> textList = GetTextListElement(By.XPath(CAT_PANEL_TYPE_COLUMN_XPATH));
			string typeStr = CATTypeDict[type];
			for (int i = 0; i < textList.Count; ++i)
			{
				if (textList[i].Contains(typeStr))
				{
					rowNum = i + 1;
					break;
				}
			}
			return rowNum;
		}

		/// <summary>
		/// Двойной щелчок по CAT-панели
		/// </summary>
		/// <param name="rowNumber">номер строки в панели</param>
		public void DoubleClickCATPanel(int rowNumber)
		{
			string xPath;
			xPath = CAT_PANEL_EXISTENCE_XPATH;

			DoubleClickElement(By.XPath(xPath + "[" + rowNumber + "]" + CAT_PANEL_TEXT_COL_PART));
		}

		/// <summary>
		/// Вернуть текст из кат-панели
		/// </summary>
		/// <param name="rowNumber">номер строки в кат-панели</param>
		/// <returns>текст</returns>
		public string GetCATPanelText(int rowNumber)
		{
			return GetTextElement(By.XPath(CAT_PANEL_TEXT_COLUMN_XPATH));
		}

		/// <summary>
		/// Проверить, активен ли Source
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <returns>активен</returns>
		public bool GetIsCursorInSourceCell(int segmentNumber)
		{
			return GetIsElementActive(By.CssSelector(GetSourceCellCss(segmentNumber)));
		}

		/// <summary>
		/// Проверить, активен ли Target
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <returns>активен</returns>
		public bool GetIsCursorInTargetCell(int segmentNumber)
		{
			return GetIsElementActive(By.CssSelector(GetTargetCellCss(segmentNumber)));
		}

		/// <summary>
		/// Получить xPath Target
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <returns>xPath</returns>
		protected string GetTargetCellCss(int rowNumber)
		{
			return SEGMENTS_CSS + ":nth-child(" + rowNumber + ")" + " td." + TARGET_CELL_CLASS + " div";
		}

		/// <summary>
		/// Получить xPath Source
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <returns>xPath</returns>
		protected string GetSourceCellCss(int rowNumber)
		{
			return SEGMENTS_CSS + ":nth-child(" + rowNumber + ")" + " td." + SOURCE_CELL_CLASS + " div";
		}

		/// <summary>
		/// Возвращает список подсвеченных в сегменте текстов
		/// </summary>
		/// <param name="segmentRowNumber">Номер сегмента</param>
		/// <returns>Список подсвеченных в сегменте текстов</returns>
		public List<string> GetSegmentSelectedTexts(int segmentRowNumber)
		{
			List<string> selectedTexts = new List<string>();
			// Выбор нужного сегмента
			ClickSourceCell(segmentRowNumber);
			Thread.Sleep(1000);

			// Выборка подсвеченных слов в сегменте
			string segmentCSS;
			segmentCSS = SEGMENTS_CSS + ":nth-child(" + segmentRowNumber + ") td:nth-child(2) div pre span";

			// Выставляем минимальный таймаут
			SetDriverTimeoutMinimum();

			IList<IWebElement> segmentCatSelectedList = GetElementList(By.CssSelector(segmentCSS));

			// Получаем список в нижнем регистре
			if (segmentCatSelectedList.Count > 0)
			{
				foreach (IWebElement item in segmentCatSelectedList)
				{
					selectedTexts.Add(item.Text.ToLower());
				}
			}

			// Выставляем дефолтное значение таймаута
			SetDriverTimeoutDefault();

			return selectedTexts;
		}

		/// <summary>
		/// Возвращает открылась ли форма сообщения
		/// </summary>
		/// <returns>Форма открылась</returns>
		public bool WaitMessageFormDisplay()
		{
			return WaitUntilDisplayElement(By.Id(MESSAGEBOX_FORM_ID));
		}

		/// <summary>
		/// Возвращает открылся ли поиск
		/// </summary>
		/// <returns>Поиск открылся</returns>
		public bool WaitConcordanceSearchDisplay()
		{
			return WaitUntilDisplayElement(By.Id(CONCORDANCE_SEARCH_ID));
		}

		/// <summary>
		/// Возвращает открылась ли форма выбора спецсимвола
		/// </summary>
		/// <returns>Форма открылась</returns>
		public bool WaitCharFormDisplay()
		{
			return WaitUntilDisplayElement(By.Id(CHAR_FORM_ID));
		}

		/// <summary>
		/// Возвращает открылась ли форма словаря
		/// </summary>
		/// <returns>Форма открылась</returns>
		public bool WaitDictionaryFormDisplay()
		{
			return WaitUntilDisplayElement(By.Id(DICTIONARY_FORM_ID));
		}

		/// <summary>
		/// Возвращает открылась ли форма добавления термина
		/// </summary>
		/// <returns>Форма открылась</returns>
		public bool WaitAddTermFormDisplay()
		{
			return WaitUntilDisplayElement(By.Id(ADD_TERM_FORM_ID));
		}

		/// <summary>
		/// Возвращает закрылась ли форма словаря
		/// </summary>
		/// <returns>Форма закрылась</returns>
		public bool WaitDictionaryFormDisappear()
		{
			return WaitUntilDisappearElement(By.Id(DICTIONARY_FORM_ID));
		}

		/// <summary>
		/// Возвращает открылось ли сообщение об ошибке повторного добавления слова
		/// </summary>
		/// <returns>Сообщение открылось</returns>
		public bool WaitAlreadyExistInDictionaryMessageDisplay()
		{
			return WaitUntilDisplayElement(By.XPath(ERROR_MESSAGE_XPATH));
		}

		/// <summary>
		/// Кликнуть кнопку добавления слова в словарь
		/// </summary>
		public void ClickAddWordDictionaryBtn()
		{
			ClickElement(By.XPath(ADD_WORD_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть кнопку закрытия словаря
		/// </summary>
		public void ClickCloseDictionaryBtn()
		{
			ClickElement(By.XPath(CLOSE_DICTIONARY_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть кнопку удаления слова из словаря
		/// </summary>
		/// <param name="word">Слово, которое необходимо удалить</param>
		public void ClickDeleteWordDictionaryBtn(string word)
		{
			// Получение xpath кнопки удаления для заданного слова
			string xPath = WORDS_TABLE_XPATH + WORD_XPATH +
				"[text()='" + word + "']/../.." + DELETE_WORD_XPATH;

			ClickElement(By.XPath(xPath));
		}

		/// <summary>
		/// Дважды кликнуть по слове из словаря
		/// </summary>
		/// <param name="word">Слово</param>
		public void DoubleClickWordDictionary(string word)
		{
			// Получение xpath заданного слова
			string xPath = WORDS_TABLE_XPATH + WORD_XPATH +
				"[text()='" + word + "']";

			DoubleClickElement(By.XPath(xPath));
		}

		/// <summary>
		/// Возвращает список слов из словаря
		/// </summary>
		/// <returns>Список слов из словаря</returns>
		public List<string> GetWordListDictionary()
		{
			string xPath = WORDS_TABLE_XPATH + WORD_XPATH;

			List<string> wordList = new List<string>();

			if (GetIsElementExist(By.XPath(xPath)))
				wordList = GetTextListElement(By.XPath(xPath));

			return wordList;
		}

		/// <summary>
		/// Добавляет слово в словарь
		/// </summary>
		/// <param name="word">Слово</param>
		public void AddWordDictionary(string word)
		{
			ClickClearAndAddText(By.XPath(INPUT_WORD_XPATH), word);
			SendKeys.SendWait(@"{Enter}");
		}

		/// <summary>
		/// Возвращает список подчеркнутых слов
		/// </summary>
		/// <param name="rowNumber">Номер строки сегмента</param>
		/// <returns>Список подчеркнутых слов</returns>
		public List<string> GetWordListSpellcheck(int rowNumber)
		{
			string xPath = SEGMENT_ROW_XPATH + "[" + rowNumber + "]" +
				SPELLCHECK_TARGET_XPATH;

			List<string> wordList = new List<string>();

			if (GetIsElementExist(By.XPath(xPath)))
				wordList = GetTextListElement(By.XPath(xPath));

			return wordList;
		}

		/// <summary>
		/// Возвращает сохранились ли сегменты
		/// </summary>
		/// <returns>Сегменты сохранились</returns>
		public bool WaitUntilAllSegmentsSave()
		{
			return WaitUntilDisappearElement(By.XPath(AUTOSAVING_XPATH));
		}



		protected const string TITLE_TEXT = "editor";
		protected const string SEGMENTS_CSS = "#segments-body div div table";
		protected const string FIRST_SOURCE_CSS = SEGMENTS_CSS + ":nth-child(1)";

		protected const string STAGE_NAME_XPATH = ".//h1/span[contains(@class, 'workflow')]";

		protected const string HOME_BTN_ID = "back-btn";
		protected const string CONFIRM_BTN_ID = "confirm-btn";
		protected const string UNDO_BTN_ID = "undo-btn";
		protected const string REDO_BTN_ID = "redo-btn";
		protected const string UNFINISHED_BTN_ID = "unfinished-btn";
		protected const string COPY_BTN_ID = "copy-btn";
		protected const string TOGGLE_BTN_ID = "toggle-source-btn";
		protected const string ROLLBACK_BTN_ID = "step-rollback-btn";
		protected const string INSERT_TAG_BTN_ID = "tag-insert-btn";
		protected const string DICTIONARY_BTN_ID = "dictionary-btn";
		protected const string FIND_ERROR_BTN_ID = "qa-error-btn";
		protected const string CHANGE_CASE_BTN_ID = "change-case-btn";
		protected const string CONCORDANCE_BTN_ID = "concordance-search-btn";
		protected const string CHARACTER_BTN_ID = "charmap-btn";
		protected const string ADD_TERM_BTN_ID = "add-term-btn";

		protected const string AUTOSAVING_XPATH = ".//div[contains(text(), 'Saving')]";

		protected const string DICTIONARY_FORM_ID = "dictionary";
		protected const string ADD_TERM_FORM_ID = "term-window";
		protected const string MESSAGEBOX_FORM_ID = "messagebox";
		protected const string CONCORDANCE_SEARCH_ID = "concordance-search";
		protected const string CHAR_FORM_ID = "charmap";

		protected const string DICTIONARY_FORM_XPATH = ".//div[@id='" + DICTIONARY_FORM_ID + "']";
		protected const string CLOSE_DICTIONARY_BTN_XPATH = DICTIONARY_FORM_XPATH + "//span[contains(@class, 'fa-times')]";
		protected const string ADD_WORD_BTN_XPATH = DICTIONARY_FORM_XPATH + "//span[contains(@id, 'btnInnerEl')]";
		protected const string WORDS_TABLE_XPATH = DICTIONARY_FORM_XPATH + "//table";
		protected const string WORD_XPATH = "//td[1]/div";
		protected const string DELETE_WORD_XPATH = "//td[2]//span[contains(@class, 'fa-trash')]";
		protected const string INPUT_WORD_XPATH = DICTIONARY_FORM_XPATH + "//input[contains(@id, 'textfield')]";

		protected const string ERROR_MESSAGE_XPATH = ".//div[contains(@id, 'messagebox')][contains(text(), 'is already in the dictionary')]";

		protected const string SEGMENTS_BODY_ID = "segments-body";
		protected const string SEGMENT_ROW_XPATH = ".//div[@id='" + SEGMENTS_BODY_ID + "']//table";
		protected const string INFO_COLUMN_CLASS = "info-cell";
		protected const string CONFIRMED_ICO_CLASS = "fa-check";
		protected const string LOCKED_ICO_CLASS = "fa-lock";
		protected const string TARGET_CELL_CLASS = "target-cell";
		protected const string SOURCE_CELL_CLASS = "source-cell";
		protected const string SEGMENT_CAT_SELECTED = "cat-selected";
		protected const string TARGET_XPATH = "//td[3]//div";
		protected const string TAG_TARGET_XPATH = TARGET_XPATH + "//img[contains(@class,'tag')]";
		protected const string SPELLCHECK_TARGET_XPATH = TARGET_XPATH + "//span[contains(@class,'spellcheck')]";
		protected const string CONTEXT_MENU_SPELLCHECK_ADD_XPATH = "//span[contains(string(), 'Add to dictionary')]";

		protected const string SPELL_CHECK_CONTEXT_CLASS = "div[contains(@class,'mce-container-body')]";
		protected const string FIRST_SPELL_CHECK_ROW_XPATH = SPELL_CHECK_CONTEXT_CLASS + "//div[1]//span";

		protected const string CAT_PANEL_ID = "cat-body";
		protected const string CAT_PANEL_EXISTENCE_XPATH = ".//div[@id='" + CAT_PANEL_ID + "']//table";
		protected const string CAT_PANEL_TYPE_COLUMN_XPATH = CAT_PANEL_EXISTENCE_XPATH + "//td[3]/div";
		protected const string CAT_PANEL_TEXT_COL_PART = "//td[4]/div";
		protected const string CAT_PANEL_TEXT_COLUMN_XPATH = CAT_PANEL_EXISTENCE_XPATH + CAT_PANEL_TEXT_COL_PART;
		protected const string CAT_PANEL_TYPE_COLUMN_MATCH_XPATH = CAT_PANEL_EXISTENCE_XPATH + "//td[3]//div//span";

		public enum CAT_TYPE { MT, TM, TB };
		protected Dictionary<CAT_TYPE, string> CATTypeDict;
	}
}
