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

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class EditorPageHelper : CommonHelper
    {
        public EditorPageHelper(IWebDriver driver, WebDriverWait wait):
            base (driver, wait)
        {
            CATTypeDict = new Dictionary<CAT_TYPE, string>();
            CATTypeDict.Add(CAT_TYPE.MT, "MT");
            CATTypeDict.Add(CAT_TYPE.TM, "TM");
			CATTypeDict.Add(CAT_TYPE.TB, "TB");
        }

        /// <summary>
        /// Дождаться, пока загрузится страница
        /// </summary>
        public void WaitPageLoad()
        {
            // Дождаться загрузки страницы
            Wait.Until((d) => d.Title.Contains(TITLE_TEXT));
        }

        /// <summary>
        /// Получить, есть ли сегменты
        /// </summary>
        /// <returns>есть сегменты</returns>
        public bool GetSegmentsExist()
        {
			if (Driver.Url.Contains("stage3") || Driver.Url.Contains("stage1"))
				return GetIsElementExist(By.CssSelector(SEGMENTS_STAGE3_CSS));
			else
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
        /// Кликнуть по Back
        /// </summary>
        public void ClickBackBtn()
        {
            ClickElement(By.Id(BACK_BTN_ID));
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
        /// Дождаться, пока сегмент подтвердится
        /// </summary>
        /// <param name="segmentRowNumber">номер сегмента</param>
        /// <returns>сегмент подтвердился</returns>
        public bool WaitSegmentConfirm(int segmentRowNumber)
        {
			string xPath = "";
			if (Driver.Url.Contains("stage3") || Driver.Url.Contains("stage1"))
				xPath = SEGMENT_ROW_STAGE3_XPATH + "[" + segmentRowNumber + "]//td[contains(@class,'" + INFO_COLUMN_CLASS + "')]//span[contains(@class,'" + CONFIRMED_ICO_CLASS + "')]";
			else
				xPath = SEGMENT_ROW_XPATH + "[" + segmentRowNumber + "]//td[contains(@class,'" + INFO_COLUMN_CLASS + "')]//span[contains(@class,'" + CONFIRMED_ICO_CLASS + "')]";

			return WaitUntilDisplayElement(By.XPath(xPath));
        }

        /// <summary>
        /// Вернуть количество сегментов
        /// </summary>
        /// <returns>количество</returns>
        public int GetSegmentsNumber()
        {
            int segmentCount;
			if (Driver.Url.Contains("stage3") || Driver.Url.Contains("stage1"))
				segmentCount = GetElementList(By.CssSelector(SEGMENTS_STAGE3_CSS)).Count;
			else
				segmentCount = GetElementList(By.CssSelector(SEGMENTS_CSS)).Count;

            Console.WriteLine("segmentCount: " + segmentCount);
            return segmentCount;
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
        /// Получить, что панель CAT не пуста
        /// </summary>
        /// <returns>не пуста</returns>
        public bool GetCATPanelNotEmpty()
        {
			string xPath;
			if (Driver.Url.Contains("stage3") || Driver.Url.Contains("stage1"))
				xPath = CAT_PANEL_EXISTENCE_STAGE3_XPATH;
			else
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
			if (Driver.Url.Contains("stage3") || Driver.Url.Contains("stage1"))
				xPath = CAT_PANEL_EXISTENCE_STAGE3_XPATH;
			else
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
        /// Дождаться, пока кнопка Save заблокируется
        /// </summary>
        /// <returns>заблокировалась</returns>
        public bool WaitSaveBtnDisabled()
        {
            return WaitUntilDisplayElement(By.XPath(SAVE_BTN_UNAVAILABLE_XPATH));
        }

        /// <summary>
        /// Кликнуть кнопку Save
        /// </summary>
        public void ClickSaveBtn()
        {
            ClickElement(By.Id(SAVE_BTN_ID));
        }

        /// <summary>
        /// Кликнуть Изменить регистр
        /// </summary>
        public void ClickChangeCaseBtn()
        {
            ClickElement(By.Id(CHANGE_CASE_BTN_ID));
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
        /// Получить xPath Target
        /// </summary>
        /// <param name="rowNumber">номер строки</param>
        /// <returns>xPath</returns>
        protected string GetTargetCellCss(int rowNumber)
        {
			if (Driver.Url.Contains("stage3") || Driver.Url.Contains("stage1"))
				return SEGMENTS_STAGE3_CSS + ":nth-child(" + rowNumber + ")" + " td." + TARGET_CELL_CLASS + " div";
			else
				return SEGMENTS_CSS + ":nth-child(" + rowNumber + ")" + " td." + TARGET_CELL_CLASS + " div";
        }

        /// <summary>
        /// Получить xPath Source
        /// </summary>
        /// <param name="rowNumber">номер строки</param>
        /// <returns>xPath</returns>
        protected string GetSourceCellCss(int rowNumber)
        {
			if (Driver.Url.Contains("stage3") || Driver.Url.Contains("stage1"))
				return SEGMENTS_STAGE3_CSS + ":nth-child(" + rowNumber + ")" + " td." + SOURCE_CELL_CLASS +" div";
			else
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
			if (Driver.Url.Contains("stage3") || Driver.Url.Contains("stage1"))
				segmentCSS = SEGMENTS_STAGE3_CSS + ":nth-child(" + segmentRowNumber + ") td:nth-child(2) div pre span";
			else
				segmentCSS = SEGMENTS_CSS + ":nth-child(" + segmentRowNumber + ") td:nth-child(2) div pre span";

			IList<IWebElement> segmentCatSelectedList = GetElementList(By.CssSelector(segmentCSS));
			
			if (segmentCatSelectedList.Count > 0)
			{
				foreach (IWebElement item in segmentCatSelectedList)
				{
					selectedTexts.Add(item.Text.ToLower());
				}
			}
			return selectedTexts;
		}


        protected const string TITLE_TEXT = "Editor";
        protected const string SEGMENTS_CSS = "#segments-body div table tr";
		protected const string SEGMENTS_STAGE3_CSS = "#segments-body div div table";
        protected const string FIRST_SOURCE_CSS = SEGMENTS_CSS + ":nth-child(1)";

        protected const string CONFIRM_BTN_ID = "confirm-btn";
        protected const string BACK_BTN_ID = "back-btn";
        protected const string TOGGLE_BTN_ID = "toggle-btn";
        protected const string COPY_BTN_ID = "copy-btn";
        protected const string UNDO_BTN_ID = "undo-btn";
        protected const string REDO_BTN_ID = "redo-btn";
        protected const string SAVE_BTN_ID = "save-btn";
        protected const string CHANGE_CASE_BTN_ID = "change-case-btn";

        protected const string SEGMENTS_BODY_ID = "segments-body";
        protected const string SEGMENT_ROW_XPATH = ".//div[@id='" + SEGMENTS_BODY_ID + "']//table//tbody//tr";
		protected const string SEGMENT_ROW_STAGE3_XPATH = ".//div[@id='" + SEGMENTS_BODY_ID + "']//table";
        protected const string INFO_COLUMN_CLASS = "info-cell";
        protected const string CONFIRMED_ICO_CLASS = "fa-check";
        protected const string TARGET_CELL_CLASS = "target-cell";
        protected const string SOURCE_CELL_CLASS = "source-cell";
		protected const string SEGMENT_CAT_SELECTED = "cat-selected";

        protected const string SPELL_CHECK_CONTEXT_CLASS = "div[contains(@class,'mce-container-body')]";
        protected const string FIRST_SPELL_CHECK_ROW_XPATH = SPELL_CHECK_CONTEXT_CLASS + "//div[1]//span";

        protected const string CAT_PANEL_ID = "cat-body";
        protected const string CAT_PANEL_EXISTENCE_XPATH = ".//div[@id='" + CAT_PANEL_ID + "']//table//tbody//tr";
		protected const string CAT_PANEL_EXISTENCE_STAGE3_XPATH = ".//div[@id='" + CAT_PANEL_ID + "']//table";
        protected const string CAT_PANEL_TYPE_COLUMN_XPATH = CAT_PANEL_EXISTENCE_XPATH + "//td[3]/div";
        protected const string CAT_PANEL_TEXT_COL_PART = "//td[4]/div";
        protected const string CAT_PANEL_TEXT_COLUMN_XPATH = CAT_PANEL_EXISTENCE_XPATH + CAT_PANEL_TEXT_COL_PART;
		protected const string CAT_PANEL_TYPE_COLUMN_MATCH_XPATH = CAT_PANEL_EXISTENCE_XPATH + "//td[3]//div//span";

        protected const string SAVE_BTN_UNAVAILABLE_XPATH = ".//a[@id='" + SAVE_BTN_ID + "' and contains(@class,'x-btn-disabled')]";

        public enum CAT_TYPE {MT, TM, TB};
        protected Dictionary<CAT_TYPE, string> CATTypeDict;
    }
}
