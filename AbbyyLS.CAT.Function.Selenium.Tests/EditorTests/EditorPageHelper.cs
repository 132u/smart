using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

using NLog;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

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

		public void AssertionIsPageLoad()
		{
			Logger.Debug("Проверяем, загрузилась ли страница");

			bool isPageLoad;

			try
			{
				isPageLoad = Wait.Until((d) => d.Url.Contains(TITLE_TEXT));
			}
			catch (WebDriverTimeoutException)
			{
				isPageLoad = false;
			}

			Assert.IsTrue(isPageLoad, "Ошибка: страница редактора не открылась.");
		}
		
		/// <summary>
		/// Возвращает задачу
		/// </summary>
		/// <returns>Задача</returns>
		public string GetStageName()
		{
			var stage = "";

			if (GetIsElementExist(By.XPath(STAGE_NAME_XPATH)))
				stage = GetTextElement(By.XPath(STAGE_NAME_XPATH));

			return stage;
		}

		public bool GetSegmentsExist()
		{
			Log.Trace("Получение существования сегментов");
			return GetIsElementExist(By.CssSelector(SEGMENTS_CSS));
		}

		public void CloseTutorial()
		{
			if (tutorialExist())
			{
				Driver.FindElement(By.XPath(FINISH_TUTORIAL_BUTTON)).Click();
			}
		}

		private bool tutorialExist()
		{
			return GetIsElementDisplay(By.XPath(FINISH_TUTORIAL_BUTTON));
		}

		public void SelectWordPartBeforeSpaceByShiftArrowRight(string text)
		{
			Logger.Trace("Выделение части строки до первого пробела");
			HotKey.Home();
			var arr = text.Split(' ');
			for (int i = 0; i <= arr[0].Length; i++)
				HotKey.ShiftRight();
		}

		public void SelectSecondThirdWordsByShiftArrowRight(string text)
		{
			Logger.Trace("Выделение второго и третьего слова");
			HotKey.Home();
			HotKey.CtrlRight();
			var arr = text.Split(' ');
			for (int i = 0; i <= arr[1].Length + arr[2].Length; i++)
				HotKey.ShiftRight();
		}

		public void AddTextTarget(int rowNum, string text)
		{
			Logger.Trace(string.Format("Добавить текст {0} в таргет №{1}", text, rowNum));

			var targerCellForInput = By.XPath(GetTargetCellXPath(rowNum));

			if (!GetIsElementExist(targerCellForInput))
			{
				var errorMessage = string.Format(
					"Не удается найти таргет сегмент для вставки текста. Путь к элементу: {0}",
					targerCellForInput);
				Logger.Error(errorMessage);
				throw new NoSuchElementException(errorMessage);
			}
			ClickClearAndAddText(targerCellForInput, text);
			WaitUntilDisplayElement(By.XPath(GetTargetWithTextXpath(rowNum, text)), 1);
			Logger.Trace("добавили текст: " + text);
		}

		public void PressHotKey(int rowNumber, string hotKey)
		{
			Driver.FindElement(By.XPath(GetTargetCellXPath(rowNumber))).Click();
			Actions actions = new Actions(Driver);
			actions.SendKeys(hotKey);
			actions.Perform();
		}

		public void ClickInSegment(int rowNumber)
		{
			Logger.Trace("Клик по сегменту № " + rowNumber);
			Driver.FindElement(By.XPath(GetTargetCellXPath(rowNumber))).Click();
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
			var actions = new Actions(Driver);
			Driver.FindElement(By.XPath(getSegmentRow(rowNum) + TARGET_XPATH)).Click();
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
 
		public void ClearTarget(int rowNum)
		{
			Log.Trace(string.Format("Очистить таргет. Номер строки: {0}", rowNum));
			ClearElement(By.XPath((GetTargetCellXPath(rowNum))));
		}

		protected void SendHotKey(int segmentNumber, string hotKey)
		{
			ClickInSegment(segmentNumber);
			SendKeys.SendWait(hotKey);
		}

		public void PutCatMatchByHotkey(int segmentNumber, int catLineNumber)
		{
			Logger.Trace(string.Format("Нажать хоткей Ctrl " + catLineNumber
				+ " для подстановки из кат перевода сегмента. Номер строки: {0}, номер строки панели кат: {1}",
				segmentNumber, catLineNumber));
			SendKeys.SendWait(@"^{" + catLineNumber + "}");
		}

		public void ConfirmByHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей для Confirm. Номер строки: {0}", segmentNumber));
			ClickInSegment(segmentNumber);
			HotKey.CtrlEnter();

		}

		public void ChangeCaseByHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей Shift F3 для изменения регистра. Номер строки: {0}", segmentNumber));
			HotKey.ShiftF3();
		}

		/// <summary>
		/// Нажать хоткей отмены CTRL z
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void UndoByHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей отмены. Номер строки: {0}", segmentNumber));
			ClickInSegment(segmentNumber);
			HotKey.CtrlZ();
		}

		/// <summary>
		/// Нажать хоткей возврата отмененного действия CTRL y
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void RedoByHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей возврата отмененного действия. Номер строки: {0}", segmentNumber));
			ClickInSegment(segmentNumber);
			HotKey.CtrlY();
		}

		public void TabByHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей Tab. Номер строки: {0}", segmentNumber));
			ClickInSegment(segmentNumber);
			HotKey.Tab();
		}

		public void CursorToTargetLineBeginningByHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей  перехода в начало строки таргет. Номер строки: {0}", segmentNumber));
			SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Home);
		}
		
		public void SelectLastWordByHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей выделения последнего слова. Номер строки: {0}", segmentNumber));
			ClickInSegment(segmentNumber);
			HotKey.CtrlShiftLeft();
		}

		public void SelectFirstWordSourceByAction(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей выделения первого слова сорс. Номер строки: {0}", segmentNumber));
			var segment = Driver.FindElement(By.XPath(GetSourceCellXPath(segmentNumber)));
			var builder = new Actions(Driver);
			Logger.Trace("Произвести двойной клик по сегменту.");
			builder.MoveToElement(segment, 0, 0).DoubleClick().Perform();
		}

		public void SelectFirstWordTargetByAction(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей выделения первого слова в таргете. Номер строки: {0}", segmentNumber));
			var segment = Driver.FindElement(By.XPath(this.GetTargetCellXPath(segmentNumber)));

			var builder = new Actions(Driver);
			builder.MoveToElement(segment, 0, 0).DoubleClick().Perform();
		}

		public void SelectNextThreeSymbolsByHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей выделения следующих трех символов. Номер строки: {0}", segmentNumber));
			HotKey.ShiftRight(3);
		}

		public void EndHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей End. Номер строки: {0}", segmentNumber));
			ClickInSegment(segmentNumber);
			HotKey.End();
		}

		public void AddTermFormByHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей вызова формы для добавления термина. Номер строки: {0}", segmentNumber));
			ClickInSegment(segmentNumber);
			HotKey.CtrlE();
		}

		public void SelectSecondThirdWordsByHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей выделения второго и третьего слов. Номер строки: {0}", segmentNumber));
			SendHotKey(segmentNumber, @"{RIGHT}^{RIGHT}+{RIGHT}{RIGHT}");
		}

		public void NextUnfinishedSegmentByHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей поиска следующего незаконченного сегмента. Номер строки: {0}", segmentNumber));
			ClickInSegment(segmentNumber);
			HotKey.F9();
		}

		public void SelectAlltextByHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей выделения всего содержимого ячейки. Номер строки: {0}", segmentNumber));
			ClickInSegment(segmentNumber);
			HotKey.CtrlShiftHome();
		}

		/// <summary>
		/// Нажать хоткей копирования из сорс CTRL Insert
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public void CopySourceByHotkey(int segmentNumber)
		{
			Log.Trace(string.Format("Нажать хоткей копирования из сорс. Номер строки: {0}", segmentNumber));
			ClickInSegment(segmentNumber);
			HotKey.CtrlInsert();
		}

		public void SendKeysTarget(int row, string keys)
		{
			Log.Trace(string.Format("Отправить keys в target. Номер строки: {0}, keys: {1}", row, keys));
			ClickAndSendTextElement(By.XPath(GetTargetCellXPath(row)), keys);
		}

		public void PasteTranslationToTargetByHotkey(int row, string catPanelRowNumber)
		{
			Log.Trace(string.Format("Отправить перевод в target из CAT-панели с помощью хоткея. Номер строки: {0}, номер строки в CAT - панеле: {1}", row, catPanelRowNumber));
			ClickElement(By.XPath(GetTargetCellXPath(row)));
			SendKeys.SendWait(@"^{" + catPanelRowNumber + "}");
		}

		public void SendKeysSource(int row, string keys)
		{
			Log.Trace(string.Format("Отправить keys в source. Номер строки: {0}, keys: {1}", row, keys));
			ClickAndSendTextElement(By.XPath(GetSourceCellXPath(row)), keys);
		}

		public void ClickConfirmBtn()
		{
			Logger.Debug("Кликнуть Confirm");
			ClickElement(By.Id(CONFIRM_BTN_ID));
		}

		public void ClickUndoBtn()
		{
			Log.Trace("Кликнуть Undo");
			ClickElement(By.Id(UNDO_BTN_ID));
		}

		public bool IsMessageBoxDisplay()
		{
			Logger.Trace("Проверить, появилось ли сообщение 'Translation is different from the context match in the TM'");
			WaitUntilDisplayElement(By.XPath(MESSAGE_BOX));
			return GetIsElementDisplay(By.XPath(MESSAGE_BOX));
		}

		public void ClickConfirmButtonInMessageBox()
		{
			Logger.Debug("Нажать Confirm в сообщении");
			ClickElement(By.XPath(CONFIRM_BUTTON_MESSAGE_BOX));
		}

		public void ClickRedoBtn()
		{
			Log.Trace("Кликнуть Redo");
			ClickElement(By.Id(REDO_BTN_ID));
		}

		public void ClickAddTermBtn()
		{
			Log.Trace("Кликнуть Add Term");
			ClickElement(By.Id(ADD_TERM_BTN_ID));
		}

		public void ClickUnfinishedBtn()
		{
			Log.Trace("Кликнуть Unfinished");
			ClickElement(By.Id(UNFINISHED_BTN_ID));
		}

		public void ClickHomeBtn()
		{
			Log.Trace("Кликнуть по Home");
			ClickElement(By.Id(HOME_BTN_ID));
			if (Driver.WindowHandles.Count > 1)
			{
				Driver.SwitchTo().Window(Driver.CurrentWindowHandle).Close();
				Driver.SwitchTo().Window(Driver.WindowHandles.Last());
			}
		}

		public void ClickToggleBtn()
		{
			Log.Trace("Кликнуть по toggle");
			ClickElement(By.Id(TOGGLE_BTN_ID));
		}

		public void ClickCopyBtn()
		{
			Log.Trace("Кликнуть кнопку Copy");
			ClickElement(By.Id(COPY_BTN_ID));
		}

		public void ClickRollbackBtn()
		{
			Log.Trace("Кликнуть по кнопке отката");
			ClickElement(By.Id(ROLLBACK_BTN_ID));
		}

		public void ClickInsertTagBtn()
		{
			Log.Trace("Кликнуть вставить Tag");
			ClickElement(By.Id(INSERT_TAG_BTN_ID));
		}

		public void ClickDictionaryBtn()
		{
			Log.Trace("Кликнуть словарь");
			ClickElement(By.Id(DICTIONARY_BTN_ID));
		}

		public void ClickFindErrorBtn()
		{
			Log.Trace("Кликнуть поиск ошибки");
			ClickElement(By.Id(FIND_ERROR_BTN_ID));
		}

		public void ClickChangeCaseBtn()
		{
			Logger.Trace("Кликнуть кнопку изменения регистра");
			ClickElement(By.Id(CHANGE_CASE_BTN_ID));
		}

		public void ClickConcordanceBtn()
		{
			Log.Trace("Кликнуть конкордный поиск");
			ClickElement(By.Id(CONCORDANCE_BTN_ID));
		}

		public void ClickCharacterBtn()
		{
			Log.Trace("Кликнуть кнопку вставки спецсимвола");
			ClickElement(By.Id(CHARACTER_BTN_ID));
		}

		public void ClickSourceCell(int rowNumber)
		{
			Log.Trace(string.Format("Кликнуть Source в строке №{0}", rowNumber));
			ClickElement(By.XPath(GetSourceCellXPath(rowNumber)));
		}

		public void ClickTargetCell(int rowNumber)
		{
			Log.Trace(string.Format("Кликнуть Target в строке №{0}", rowNumber));
			ClickElement(By.XPath(GetTargetCellXPath(rowNumber)));
		}

		public string GetSourceText(int rowNumber)
		{
			Log.Trace(string.Format("Получить текст Source из строки №{0}", rowNumber));
			return GetTextElement(By.XPath(GetSourceCellXPath(rowNumber))).Trim();
		}

		public string GetTargetText(int rowNumber)
		{
			Log.Trace(string.Format("Получить текст Target из строки №{0}", rowNumber));
			var targetText = GetTextElement(By.XPath(GetTargetCellXPath(rowNumber))).Trim();
			Logger.Trace("Текст в таргете #" + rowNumber + " = " + targetText);
			return targetText;
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

			Logger.Trace("segmentCount: " + segmentCount);

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

		public int GetCatTranslationRowNumber(CAT_TYPE type)
		{
			Log.Trace(string.Format("Получить номер строки с нужным типом перевода. Искомый тип - {0}", type));
			// Не убирать Sleep, так как термин не сразу появляется в CAT-панели
			Thread.Sleep(2000);
			var rowNum = 0;
			var textList = GetTextListElement(By.XPath(CAT_PANEL_TYPE_COLUMN_XPATH));
			var typeStr = CATTypeDict[type];

			for (var i = 0; i < textList.Count; ++i)
			{
				if (textList[i].Contains(typeStr))
				{
					rowNum = i + 1;
					break;
				}
			}

			Assert.IsTrue(rowNum != 0, "Ошибка: нужная " + type + " подстановка отсутствует в CAT панели");
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
			var catText = GetTextElement(By.XPath(CAT_PANEL_EXISTENCE_XPATH + "[" + rowNumber + "]" + CAT_PANEL_TEXT_COL_PART));
			Logger.Trace("Текст в CAT панели #" + rowNumber + " = " + catText);
			return catText;
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
			return getSegmentRow(rowNumber) + TARGET_CELL_XPATH_FOR_INPUT;
		}
		
		/// <summary>
		/// Получить xPath Target с конкретным текстом
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		/// <param name="text">текст</param>
		/// <returns>xPath</returns>
		protected string GetTargetWithTextXpath(int segmentNumber, string text)
		{
			return "//table[@data-recordindex='" + (segmentNumber - 1) + "']"+ TARGET_TEXT_XPATH + text + "']";
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

		public void AssertionIsDictionaryFormDisplayed()
		{
			Logger.Trace("Проверить, открылась ли форма словаря");

			Assert.IsTrue(
				WaitUntilDisplayElement(By.Id(DICTIONARY_FORM_ID)),
				"Ошибка: Форма со словарем не открылась.");
		}

		public void AssertionIsDictionaryListLoad()
		{
			Logger.Trace("Ожидание окончания загрузки словаря");

			Assert.IsTrue(
				WaitUntilDisappearElement(By.XPath(DICTIONARY_LOADING_WORDS_XPATH)),
				"Ошибка: Не удалось дождаться окончания загрузки словаря.");
		}

		/// <summary>
		/// Возвращает открылась ли форма добавления термина
		/// </summary>
		/// <returns>Форма открылась</returns>
		public bool WaitAddTermFormDisplay()
		{
			Logger.Trace("Ожидаем открытия формы добавления термина.");
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
			ClickElement(By.XPath(DICTIONARY_FORM_XPATH));
		}

		/// <summary>
		/// Возвращает список подчеркнутых слов
		/// </summary>
		/// <param name="rowNumber">Номер строки сегмента</param>
		/// <returns>Список подчеркнутых слов</returns>
		public List<string> GetWordListSpellcheck(int rowNumber)
		{
			var segmentRow = getSegmentRow(rowNumber);

			ClickElement(By.XPath(segmentRow));

			var xPath = segmentRow + SPELLCHECK_TARGET_XPATH;
			var wordList = new List<string>();

			if (GetIsElementExist(By.XPath(xPath)))
			{
				wordList = GetTextListElement(By.XPath(xPath));
			}

			return wordList;
		}
		public bool WaitUntilAllSegmentsSave()
		{
			Logger.Trace("Возвращает, исчезла ли надпись 'Saving...' над окном с сегментами");
			return WaitUntilDisappearElement(By.XPath(AUTOSAVING_XPATH));
		}
		
		public void ClickTranslationTaskBtn()
		{
			Log.Trace("Нажать кнопку 'перевод' в окошке перед открытием редактора");
			ClickElement(By.XPath(TASK_TRNSLT_BTN_XPATH));
		}

		public void ClickEditingTaskBtn()
		{
			Log.Trace("Нажать кнопку 'редактура' в окошке перед открытием редактора");
			ClickElement(By.XPath(TASK_EDIT_BTN_XPATH));
		}

		public void ClickProofreadingTaskBtn()
		{
			Log.Trace("Нажать кнопку 'корректура' в окошке перед открытием редактора");
			ClickElement(By.XPath(TASK_PROOFREADING_BTN_XPATH));
		}

		public void AssertionTranslationTaskBtnIsExist()
		{
			Logger.Trace("Проверка наличия кнопки 'Перевод'");
			Assert.True(GetIsElementExist(By.XPath(TASK_TRNSLT_BTN_XPATH)), "Ошибка: Неверный этап в окне выбора ");
		}

		public void AssertionEditingTaskBtnIsExist()
		{
			Logger.Trace("Проверка наличия кнопки 'Редактура'");
			Assert.True(GetIsElementExist(By.XPath(TASK_EDIT_BTN_XPATH)), "Ошибка: Неверный этап в окне выбора ");
		}

		public void AssertionProofreadingTaskBtnIsExist()
		{
			Logger.Trace("Проверка наличия кнопки 'Корректура'");
			Assert.True(GetIsElementExist(By.XPath(TASK_PROOFREADING_BTN_XPATH)), "Ошибка: Неверный этап в окне выбора ");
		}

		public void ClickContBtn()
		{
			Log.Trace("Нажать кнопку 'продолжить' в окошке перед открытием редактора");
			ClickElement(By.XPath(TASK_CONTINUE_BTN_XPATH));
		}

		public string GetTargetMatchColor(int segmentNumber)
		{
			Log.Debug(string.Format(@"Возвращает стоящий в таргете цвет текста колонки match. Номер сегмента таргет {0}", segmentNumber));
			//забираем цвет процента
			return GetElementClass(By.XPath(targetMatchColumnPercentXpath(segmentNumber)));
		}

		public string GetTargetSubstitutionType(int segmentNumber)
		{
			Log.Debug(string.Format(@"Вернуть стоящий в таргете тип подстановки из CAT-панели : МТ\ТМ\TB. Номер сегмента таргет {0}", segmentNumber));
			
			Thread.Sleep(1000);
			//забираем текст из колонки match таргета
			var targetMatchColumnText = GetTextElement(By.XPath(targetMatchColumnTextXpath(segmentNumber))).Trim();
			
			//получаем из текста две первые буквы (тип подстановки)
			return targetMatchColumnText.Length >= 2 ? targetMatchColumnText.Substring(0, 2) : "";
		}

		public int GetTargetMatchPercent(int segmentNumber)
		{
			Log.Debug(string.Format(@"Вернуть стоящий в таргете процент совпадения. Номер сегмента таргет {0}", segmentNumber));
			
			// берем процент совпадения
			var targetMatchPercent = GetTextElement(By.XPath(targetMatchColumnPercentXpath(segmentNumber)));
			
			return ParseStrToInt(targetMatchPercent.Remove(targetMatchPercent.IndexOf('%')));
		}

		public void ClickLastVisibleSegment()
		{
			Logger.Trace("Кликнуть по последнему видимому сегменту");
			var visibleSegmentsCount = GetElementList(By.XPath(SEGMENTS_TABLE_XPATH)).Count;
			ClickElement(By.XPath(getLastVisiblElementXPath(visibleSegmentsCount)));
		}

		public void ScrollToRequiredSegment(int segmentNumber)
		{
			Logger.Debug(string.Format("Прокрутить до сегмента #{0}", segmentNumber));
			while (getIsSegmentVisible(segmentNumber) != true)
			{
				Logger.Trace("Определить номер первого видимого сегмента");
				var firstVisibleSegment = Convert.ToInt32(
					Driver.FindElement(
						By.XPath(
							FIRST_VISIBLE_SEGMENT_XPATH)).Text);

				if (firstVisibleSegment < segmentNumber)
				{
					ClickLastVisibleSegment();
				}
				else
				{
					Logger.Trace("Кликнуть по первому видимому сегменту");
					ClickElement(By.XPath(FIRST_VISIBLE_SEGMENT_XPATH));
				}
			}
		}
		
		/// <summary>
		/// Очищает поле со словом в словаре
		/// </summary>
		public void ClearInputWordDictionary(string word)
		{
			ClearElement(By.XPath(INPUT_WORD_XPATH));
		}

		public void ClickInSourceSegment(int rowNumber)
		{
			Driver.FindElement(By.XPath(GetSourceCellXPath(rowNumber))).Click();
		}

		/// <summary>
		/// XPATH типа подстановки в колонке match таргета у конкретного сегмента
		/// </summary>
		/// <param name="segmentNumber">номер сегмента таргет</param> 
		/// <returns>xpath типа подстановки</returns>
		private static string targetMatchColumnTextXpath(int segmentNumber)
		{
			return "//table[@data-recordindex='" + (segmentNumber - 1)
				+ "' and contains(@id, 'gridview')]" + TARGET_MATCH_COLUMN_XPATH;
		}

		/// <summary>
		/// XPATH процента в колонке match таргета у конкретного сегмента
		/// </summary>
		/// <param name="segmentNumber">номер сегмента таргет</param> 
		/// <returns>xpath процента совпадения</returns>
		private static string targetMatchColumnPercentXpath(int segmentNumber)
		{
			return "//table[@data-recordindex='" + (segmentNumber - 1)
				+ "' and contains(@id, 'gridview')]" + TARGET_MATCH_COLUMN_PERCENT_XPATH;
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
		protected const string UNDO_BTN_ID = "undo-btn-btnEl";
		protected const string REDO_BTN_ID = "redo-btn-btnEl";
		protected const string UNFINISHED_BTN_ID = "unfinished-btn";
		protected const string COPY_BTN_ID = "copy-btn-btnEl";
		protected const string TOGGLE_BTN_ID = "toggle-source-btn";
		protected const string ROLLBACK_BTN_ID = "step-rollback-btn";
		protected const string INSERT_TAG_BTN_ID = "tag-insert-btn";
		protected const string DICTIONARY_BTN_ID = "dictionary-btn";
		protected const string FIND_ERROR_BTN_ID = "qa-error-btn";
		protected const string CHANGE_CASE_BTN_ID = "change-case-btn";
		protected const string CONCORDANCE_BTN_ID = "concordance-search-btn";
		protected const string CHARACTER_BTN_ID = "charmap-btn";
		protected const string ADD_TERM_BTN_ID = "add-term-btn";

		protected const string AUTOSAVING_XPATH = "//div[contains(text(), 'Saving…')]";
		
		protected const string DICTIONARY_FORM_ID = "dictionary";
		protected const string ADD_TERM_FORM_ID = "term-window";
		protected const string MESSAGEBOX_FORM_ID = "messagebox";
		protected const string CONCORDANCE_SEARCH_ID = "concordance-search";
		protected const string CHAR_FORM_ID = "charmap";

		protected const string DICTIONARY_FORM_XPATH = ".//div[@id='" + DICTIONARY_FORM_ID + "']";
		protected const string CLOSE_DICTIONARY_BTN_XPATH = DICTIONARY_FORM_XPATH + "//span[contains(@class, 'x-tool-close')]";
		protected const string ADD_WORD_BTN_XPATH = DICTIONARY_FORM_XPATH + "//span[contains(@id, 'btnInnerEl')]";
		protected const string WORDS_TABLE_XPATH = DICTIONARY_FORM_XPATH + "//table";
		protected const string WORD_XPATH = "//tr//td[1]//div";
		protected const string DELETE_WORD_XPATH = "//td[2]//span[contains(@class, 'fa-trash')]";
		protected const string INPUT_WORD_XPATH = DICTIONARY_FORM_XPATH + "//input[contains(@id, 'textfield')]";
		protected const string DICTIONARY_LOADING_WORDS_XPATH = "//div[@id='dictionary-body']//div//div//div[contains(@id,'msgTextEl')]";

		protected const string ERROR_MESSAGE_XPATH = ".//div[contains(@id, 'messagebox')][contains(text(), 'is already in the dictionary')]";

		protected const string SEGMENTS_BODY_ID = "segments-body";
		protected const string INFO_COLUMN_CLASS = "info-cell";
		protected const string CONFIRMED_ICO_CLASS = "fa-check";
		protected const string LOCKED_ICO_CLASS = "fa-lock";
		protected const string SEGMENT_CAT_SELECTED = "cat-selected";
		protected const string TARGET_XPATH = "//td[3]//div";
		protected const string SOURCE_CELL_XPATH = "//td[2]//div//pre";
		protected const string TARGET_CELL_XPATH_FOR_INPUT = "//td[3]//div//div";
		protected const string TARGET_CELL_XPATH = "//td[3]//div//div//pre";
		protected const string TARGET_CELL_TEXT_XPATH = "//td[3]//div//pre";
		protected const string FIRST_VISIBLE_SEGMENT_XPATH = "//div[@id='segments-body']//table[1]//td[1]";
		protected const string SEGMENTS_TABLE_XPATH = "//div[@id='segments-body']//div//div[2]//table";

		protected const string TARGET_TEXT_XPATH = TARGET_CELL_TEXT_XPATH + "[text()='";
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

		protected const string FINISH_TUTORIAL_BUTTON = "//span[contains(text(),'Finish') and contains(@id, 'button')]";
		
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		public enum CAT_TYPE { MT, TM, TB };

		protected Dictionary<CAT_TYPE, string> CATTypeDict;

		protected const string WORKFLOW_SELECT_WINDOW = ".//div[@id='workflowselectwindow-1025']";
		protected const string TASK_IN_WORKFLOW_SELECT_WINDOW = "//span[@id='wf-stagenumber-1-btn-btnInnerEl' and text()='Translation']";

		protected const string CONTINUE_BTN_IN_WORKFLOW_SELECT_WINDOW = "//a[@id='wf-continue-btn']";

		protected const string MESSAGE_BOX = ".//div[@id='messagebox']";
		protected const string CONFIRM_BUTTON_MESSAGE_BOX = "//div[@id='messagebox-toolbar-targetEl']//a[@id='button-1020']";
		public bool GetTaskBtnIsExist()
		{
			return GetIsElementExist(By.XPath(TASK_TRNSLT_BTN_XPATH));
		}

		private static string getSegmentRow(int rowNumber)
		{
			return String.Format(
				"//div[contains(text(), '{0}')]//..//..//..//..//tbody//tr[1]",
				rowNumber);
		}

		private bool getIsSegmentVisible(int rowNumber)
		{
			Logger.Trace(string.Format("Определить видимость сегмента #{0}", rowNumber));
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
