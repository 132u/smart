using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
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
		public EditorPageHelper(IWebDriver driver, WebDriverWait wait) 
			: base(driver, wait)
		{
			CATTypeDict = new Dictionary<CAT_TYPE, string>
			{
				{CAT_TYPE.MT, "MT"},
				{CAT_TYPE.TM, "TM"},
				{CAT_TYPE.TB, "TB"}
			};
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
			ClickClearAndAddText(By.XPath(GetTargetCellXPath(rowNum)), text);

			WaitUntilDisplayElement(By.XPath(GetTargetWithTextXpath(rowNum, text)), 1);
			
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
			var xPath = getSegmentRow(rowNum) + SPELLCHECK_TARGET_XPATH;
			var result = "";
			var actions = new OpenQA.Selenium.Interactions.Actions(Driver);
			
			actions.MoveToElement(GetElement(By.XPath(xPath)), 1, 1).ContextClick().Build().Perform();

			var elements = GetElementList(By.XPath(CONTEXT_MENU_SPELLCHECK_ADD_XPATH + "/../../div[" + variantRow + "]/span"));

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
			ClearElement(By.XPath((GetTargetCellXPath(rowNum))));
		}

		/// <summary>
		/// Нажать хоткей поиска
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void SearchByHotkey(int segmentNumber)
		{
			AddTextTarget(segmentNumber, OpenQA.Selenium.Keys.Control + "k");
		}

		/// <summary>
		/// Нажать хоткей для подстановки из кат перевода сегмента
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		/// <param name="catLineNumber">номер строки панели кат</param>
		public void PutCatMatchByHotkey(int segmentNumber, int catLineNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + catLineNumber.ToString());
		}

		/// <summary>
		/// Нажать хоткей для Confirm
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void ConfirmByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Return);
		}

		/// <summary>
		/// Нажать хоткей для изменения регистра
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void ChangeCaseByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.F3);
		}

		/// <summary>
		/// Нажать хоткей отмены
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void UndoByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Control + "z");
		}

		/// <summary>
		/// Нажать хоткей возврата отмененного действия
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void RedoByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + "y");
		}

		/// <summary>
		/// Нажать хоткей Tab
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void TabByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Tab);
		}

		/// <summary>
		/// Нажать хоткей  перехода в начало строки таргет
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void CursorToTargetLineBeginningByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
		}

		/// <summary>
		/// Нажать хоткей  перехода в начало строки сорс
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void CursorToSourceLineBeginningByHotkey(int segmentNumber)
		{
			SendKeysSource(segmentNumber, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
		}

		/// <summary>
		/// Нажать хоткей выделения последнего слова
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void SelectLastWordByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowLeft);
		}

		/// <summary>
		/// Нажать хоткей выделения первого слова таргет
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void SelectFirstWordTargetByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowRight);
		}

		/// <summary>
		/// Нажать хоткей выделения первого слова сорс
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void SelectFirstWordSourceByHotkey(int segmentNumber)
		{
			SendKeysSource(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowRight);
		}

		/// <summary>
		/// Нажать хоткей перемещения курсора после третьего слова от начала строки
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void PutCursorAfterThirdWordByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home + OpenQA.Selenium.Keys.ArrowRight + OpenQA.Selenium.Keys.ArrowRight + OpenQA.Selenium.Keys.ArrowRight);
		}

		/// <summary>
		/// Нажать хоткей выделения следующих трех символов
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void SelectNextThreeSymbolsByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber,
			OpenQA.Selenium.Keys.ArrowRight + OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.ArrowRight + OpenQA.Selenium.Keys.ArrowRight + OpenQA.Selenium.Keys.ArrowRight);
		}

		/// <summary>
		/// Нажать хоткей End
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void EndHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.End);
		}

		/// <summary>
		/// Нажать хоткей вызова формы для добавления термина
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void AddTermFormByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + "e");
		}

		/// <summary>
		/// Нажать хоткей выделения второго и третьего слов
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void SelectSecondThirdWordsByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Home + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowRight + OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.ArrowRight + OpenQA.Selenium.Keys.ArrowRight);
		}

		/// <summary>
		/// Нажать хоткей поиска следующего незаконченного сегмента
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void NextUnfinishedSegmentByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.F9);
		}

		/// <summary>
		/// Нажать хоткей выделения всего содержимого ячейки
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void SelectAlltextByHotkey(int segmentNumber)
		{
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
		}

		/// <summary>
		/// Нажать хоткей копирования из сорс
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void CopySourceByHotkey(int segmentNumber)
		{
			SendKeysSource(segmentNumber, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Insert);
		}

		/// <summary>
		/// Отправить keys в target
		/// </summary>
		/// <param name="row">номер строки</param>
		/// <param name="keys">keys</param>
		public void SendKeysTarget(int row, string keys)
		{
			ClickAndSendTextElement(By.XPath(GetTargetCellXPath(row)), keys);
		}

		/// <summary>
		/// Отправить keys в source
		/// </summary>
		/// <param name="row">номер строки</param>
		/// <param name="keys">keys</param>
		public void SendKeysSource(int row, string keys)
		{
			ClickAndSendTextElement(By.XPath(GetSourceCellXPath(row)), keys);
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
			ClickElement(By.XPath(GetSourceCellXPath(rowNumber)));
		}

		/// <summary>
		/// Кликнуть Target
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public void ClickTargetCell(int rowNumber)
		{
			ClickElement(By.XPath(GetTargetCellXPath(rowNumber)));
		}

		/// <summary>
		/// Получить текст Source
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <returns>текст</returns>
		public string GetSourceText(int rowNumber)
		{
			return GetTextElement(By.XPath(GetSourceCellXPath(rowNumber))).Trim();
		}

		/// <summary>
		/// Получить текст Target
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <returns>текст</returns>
		public string GetTargetText(int rowNumber)
		{
			return GetTextElement(By.XPath(GetTargetCellXPath(rowNumber))).Trim();
		}

		/// <summary>
		/// Возвращает, есть ли tag в переводе для заданного сегмента
		/// </summary>
		/// <param name="rowNumber">Номер сегмента</param>
		/// <returns>Tag есть</returns>
		public bool GetIsTagPresent(int rowNumber)
		{
			var xPath = getSegmentRow(rowNumber) + TAG_TARGET_XPATH;

			return GetIsElementExist(By.XPath(xPath));
		}

		/// <summary>
		/// Получить элемент Source
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <returns>Элемент</returns>
		public IWebElement GetSource(int rowNumber)
		{
			return GetElement(By.XPath(GetSourceCellXPath(rowNumber)));
		}

		/// <summary>
		/// Дождаться, пока сегмент подтвердится
		/// </summary>
		/// <param name="segmentRowNumber">номер сегмента</param>
		/// <returns>сегмент подтвердился</returns>
		public bool WaitSegmentConfirm(int segmentRowNumber)
		{
			var xPath = getSegmentRow(segmentRowNumber) + "//td[contains(@class,'" + INFO_COLUMN_CLASS + "')]//span[contains(@class,'" + CONFIRMED_ICO_CLASS + "')]";

			return WaitUntilDisplayElement(By.XPath(xPath));
		}

		/// <summary>
		/// Возвращает, отображается замок в инфо сегмента
		/// </summary>
		/// <param name="segmentRowNumber">номер сегмента</param>
		/// <returns>Сегмент заблокирован</returns>
		public bool GetIsSegmentLock(int segmentRowNumber)
		{
			var xPath = getSegmentRow(segmentRowNumber) + "//td[contains(@class,'" +
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
			var xPath = getSegmentRow(segmentRowNumber) + "//td[contains(@class,'" +
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
			var segmentCount = GetElementList(By.CssSelector(SEGMENTS_CSS)).Count;

			Console.WriteLine("segmentCount: " + segmentCount);

			return segmentCount;
		}

		/// <summary>
		/// Получить, что панель CAT не пуста
		/// </summary>
		/// <returns>не пуста</returns>
		public bool GetCATPanelNotEmpty()
		{
			var xPath = CAT_PANEL_EXISTENCE_XPATH;

			// проверить, что вообще есть панель
			var isNotEmpty = GetIsElementExist(By.Id(CAT_PANEL_ID));

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
			var textList = GetTextListElement(By.XPath(CAT_PANEL_TYPE_COLUMN_XPATH));
			var typeStr = CATTypeDict[type];

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
			const string xPath = CAT_PANEL_EXISTENCE_XPATH;

			DoubleClickElement(By.XPath(xPath + "[" + rowNumber + "]" + CAT_PANEL_TEXT_COL_PART));
		}

		/// <summary>
		/// Вернуть текст из кат-панели
		/// </summary>
		/// <param name="rowNumber">номер строки в кат-панели</param>
		/// <returns>текст</returns>
		public string GetCatPanelText(int rowNumber)
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
			return GetIsElementActive(By.XPath(GetSourceCellXPath(segmentNumber)));
		}

		/// <summary>
		/// Проверить, активен ли Target
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <returns>активен</returns>
		public bool GetIsCursorInTargetCell(int segmentNumber)
		{
			return GetIsElementActive(By.XPath(GetTargetCellXPath(segmentNumber)));
		}

		/// <summary>
		/// Получить Css Target
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <returns>Css</returns>
		protected string GetTargetCellXPath(int rowNumber)
		{
			return getSegmentRow(rowNumber) + TARGET_CELL_XPATH;
		}
		
		/// <summary>
		/// Получить xPath Target с конкретным текстом
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		/// <param name="text">текст</param>
		/// <returns>xPath</returns>
		protected string GetTargetWithTextXpath(int segmentNumber, string text)
		{
			return "//table[@data-recordindex='" + (segmentNumber - 1) + 
				"' and contains(@id, 'segment')]" + TARGET_TEXT_XPATH + text + "']";
		}

		/// <summary>
		/// Получить xPath Source
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <returns>xPath</returns>
		protected string GetSourceCellXPath(int rowNumber)
		{
			return getSegmentRow(rowNumber) + SOURCE_CELL_XPATH;
		}

		/// <summary>
		/// Возвращает список подсвеченных в сегменте текстов
		/// </summary>
		/// <param name="segmentRowNumber">Номер сегмента</param>
		/// <returns>Список подсвеченных в сегменте текстов</returns>
		public List<string> GetSegmentSelectedTexts(int segmentRowNumber)
		{
			var selectedTexts = new List<string>();

			// Выбор нужного сегмента
			ClickSourceCell(segmentRowNumber);
			Thread.Sleep(1000);

			// Выборка подсвеченных слов в сегменте
			var segmentCss = SEGMENTS_CSS + ":nth-child(" + segmentRowNumber + ") td:nth-child(2) div pre span";

			// Выставляем минимальный таймаут
			SetDriverTimeoutMinimum();

			var segmentCatSelectedList = GetElementList(By.CssSelector(segmentCss));

			// Получаем список в нижнем регистре
			if (segmentCatSelectedList.Count > 0)
			{
				selectedTexts.AddRange(segmentCatSelectedList.Select(item => item.Text.ToLower()));
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
		/// Возвращает появился ли в таргете новый текст
		/// </summary>
		/// <param name="segmentNumber">номер строки таргет</param>
		/// <param name="text">новый текст</param>
		/// <returns>появился новый текст</returns>
		public bool WaitUntilDisplayTargetText(int segmentNumber, string text)
		{
			return WaitUntilDisplayElement(By.XPath(GetTargetWithTextXpath(segmentNumber, text)), 1);
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
			const string xPath = WORDS_TABLE_XPATH + WORD_XPATH;

			var wordList = new List<string>();

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
			SendTextElement(By.XPath(INPUT_WORD_XPATH), word);
			SendKeys.SendWait(@"{Enter}");
		}

		/// <summary>
		/// Возвращает список подчеркнутых слов
		/// </summary>
		/// <param name="rowNumber">Номер строки сегмента</param>
		/// <returns>Список подчеркнутых слов</returns>
		public List<string> GetWordListSpellcheck(int rowNumber)
		{
			var xPath = getSegmentRow(rowNumber) + SPELLCHECK_TARGET_XPATH;

			var wordList = new List<string>();

			if (GetIsElementExist(By.XPath(xPath)))
			{
				wordList = GetTextListElement(By.XPath(xPath));
			}

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

		/// <summary>
		/// Нажать кнопку "перевод" в окошке перед открытием редактора
		/// </summary>
		public void ClickTaskBtn()
		{
			ClickElement(By.XPath(TASK_TRNSLT_BTN_XPATH));
		}

		/// <summary>
		/// Нажать кнопку "перевод" в окошке перед открытием редактора
		/// </summary>
		public void ClickTranslationTaskBtn()
		{
			ClickElement(By.XPath(TASK_TRNSLT_BTN_XPATH));
		}

		/// <summary>
		/// Нажать кнопку "редактура" в окошке перед открытием редактора
		/// </summary>
		public void ClickEditingTaskBtn()
		{
			ClickElement(By.XPath(TASK_EDIT_BTN_XPATH));
		}

		/// <summary>
		/// Нажать кнопку "корректура" в окошке перед открытием редактора
		/// </summary>
		public void ClickProofreadingTaskBtn()
		{
			ClickElement(By.XPath(TASK_PROOFREADING_BTN_XPATH));
		}

		/// <summary>
		/// Убедиться, что появилась кнопка Перевод
		/// </summary>
		/// <returns>появилась</returns>
		public bool GetTranslationTaskBtnIsExist()
		{
			return GetIsElementExist(By.XPath(TASK_TRNSLT_BTN_XPATH));
		}

		/// <summary>
		/// Убедиться, что появилась кнопка Редактура
		/// </summary>
		/// <returns>появилась</returns>
		public bool GetEditingTaskBtnIsExist()
		{
			return GetIsElementExist(By.XPath(TASK_EDIT_BTN_XPATH));
		}

		/// <summary>
		/// Убедиться, что появилась кнопка Корректура
		/// </summary>
		/// <returns>появилась</returns>
		public bool GetProofreadingTaskBtnIsExist()
		{
			return GetIsElementExist(By.XPath(TASK_PROOFREADING_BTN_XPATH));
		}

		/// <summary>
		/// Нажать кнопку "продолжить" в окошке перед открытием редактора
		/// </summary>
		public void ClickContBtn()
		{
			ClickElement(By.XPath(TASK_CONTINUE_BTN_XPATH));
		}

		/// <summary>
		/// Возвращает стоящий в таргете цвет текста колонки match
		/// </summary>
		/// <param name="segmentNumber">номер сегмента таргет</param>
		/// <returns>цвет текста</returns>
		public string GetTargetMatchColor(int segmentNumber)
		{
			//забираем цвет процента
			return GetElementClass(By.XPath(targetMatchColumnPercentXpath(segmentNumber)));
		}

		/// <summary>
		/// Возвращает стоящий в таргете тип подстановки из CAT-панели : МТ\ТМ\TB
		/// </summary> 
		/// <param name="segmentNumber">номер сегмента таргет</param>
		/// <returns>тип подстановки</returns>

		public string GetTargetSubstitutionType(int segmentNumber)
		{
			Thread.Sleep(1000);
			//забираем текст из колонки match таргета
			var targetMatchColumnText = GetTextElement(By.XPath(targetMatchColumnTextXpath(segmentNumber))).Trim();

			//получаем из текста две первые буквы (тип подстановки)
			return targetMatchColumnText.Length >= 2 ? targetMatchColumnText.Substring(0, 2) : "";
		}

		/// <summary>
		/// Возвращает стоящий в таргете процент совпадения
		/// </summary> 
		/// <param name="segmentNumber">номер сегмента таргет</param>
		/// <returns>процент совпадения</returns>
		public int GetTargetMatchPercent(int segmentNumber)
		{
			// берем процент совпадения
			var targetMatchPercent = GetTextElement(By.XPath(targetMatchColumnPercentXpath(segmentNumber)));

			// Переводим в int
			return ParseStrToInt(targetMatchPercent.Remove(targetMatchPercent.IndexOf('%')));
		}

		public void ScrollToRequiredSegment(int segmentNumber)
		{
			while (getIsSegmentVisible(segmentNumber) != true)
			{
				var firstVisibleSegment = Convert.ToInt32(
					Driver.FindElement(
						By.XPath(
							FIRST_VISIBLE_SEGMENT_XPATH)).Text);

				if (firstVisibleSegment < segmentNumber)
				{
					var visibleSegmentsCount = GetElementList(By.XPath(SEGMENTS_TABLE_XPATH)).Count;
					ClickElement(By.XPath(getLastVisiblElementXPath(visibleSegmentsCount)));
				}
				else
				{
					ClickElement(By.XPath(FIRST_VISIBLE_SEGMENT_XPATH));
				}
			}
		}
		
		/// <summary>
		/// XPATH типа подстановки в колонке match таргета у конкретного сегмента
		/// </summary>
		/// <param name="segmentNumber">номер сегмента таргет</param> 
		/// <returns>xpath типа подстановки</returns>
		private static string targetMatchColumnTextXpath(int segmentNumber)
		{
			return "//table[@data-recordindex='" + (segmentNumber - 1) 
				+ "' and contains(@id, 'segment')]" + TARGET_MATCH_COLUMN_XPATH;
		}

		/// <summary>
		/// XPATH процента в колонке match таргета у конкретного сегмента
		/// </summary>
		/// <param name="segmentNumber">номер сегмента таргет</param> 
		/// <returns>xpath процента совпадения</returns>
		private static string targetMatchColumnPercentXpath(int segmentNumber)
		{
			return "//table[@data-recordindex='" + (segmentNumber - 1) 
				+ "' and contains(@id, 'segment')]" + TARGET_MATCH_COLUMN_PERCENT_XPATH;
		}
		
		protected const string TARGET_MATCH_COLUMN_XPATH = "//td[5]//div";
		protected const string TARGET_MATCH_COLUMN_PERCENT_XPATH = TARGET_MATCH_COLUMN_XPATH + "//span";

		protected const string TASK_TRNSLT_BTN_XPATH = "//span[contains(text(),'Translation')]";
		protected const string TASK_EDIT_BTN_XPATH = "//span[contains(text(), 'Editing')]";
		protected const string TASK_PROOFREADING_BTN_XPATH = "//span[contains(text(), 'Proofreading')]";
		protected const string TASK_CONTINUE_BTN_XPATH = "//span[contains(@id, 'continue-btn')]";

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
		protected const string INFO_COLUMN_CLASS = "info-cell";
		protected const string CONFIRMED_ICO_CLASS = "fa-check";
		protected const string LOCKED_ICO_CLASS = "fa-lock";
		protected const string SEGMENT_CAT_SELECTED = "cat-selected";
		protected const string TARGET_XPATH = "//td[3]//div";
		protected const string SOURCE_CELL_XPATH = "//td[2]//div//div";
		protected const string TARGET_CELL_XPATH = "//td[3]//div//pre";
		protected const string FIRST_VISIBLE_SEGMENT_XPATH = "//div[@id='segments-body']//table[1]//td[1]";
		protected const string SEGMENTS_TABLE_XPATH = "//div[@id='segments-body']//div//div[2]//table";

		protected const string TARGET_TEXT_XPATH = TARGET_XPATH + "[string()='";
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

		protected const string WORKFLOW_SELECT_WINDOW = ".//div[@id='workflowselectwindow-1025']";
		protected const string TASK_IN_WORKFLOW_SELECT_WINDOW = "//span[@id='wf-stagenumber-1-btn-btnInnerEl' and text()='Translation']";

		protected const string CONTINUE_BTN_IN_WORKFLOW_SELECT_WINDOW = "//a[@id='wf-continue-btn']";

		public bool GetTaskBtnIsExist()
		{
			return GetIsElementExist(By.XPath(TASK_TRNSLT_BTN_XPATH));
		}

		private static string getSegmentRow(int rowNumber)
		{
			return String.Format(
				"//div[contains(text(), '{0}')]//..//..//..//..//tbody//tr",
				rowNumber);
		}

		private bool getIsSegmentVisible(int rowNumber)
		{
			return GetIsElementExist(By.XPath(getSegmentRow(rowNumber)));
		}

		private string getLastVisiblElementXPath(int tableElementsCount)
		{
			return String.Format(
				"//div[@id='segments-body']//table[{0}]//td[1]", 
				tableElementsCount);
		}
	}
}
