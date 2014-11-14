using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер для истории версий редактора
	/// </summary>
	public class Editor_RevisionPageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public Editor_RevisionPageHelper(IWebDriver driver, WebDriverWait wait) 
			: base(driver, wait)
		{
			revisionTypeList = new Dictionary<string,RevisionType>
			{
				{REVISION_TYPE_AUTOSAVE, RevisionType.AutoSave},
				{REVISION_TYPE_CONFIRMED, RevisionType.Confirmed},
				{REVISION_TYPE_INSERT_MT, RevisionType.InsertMT},
				{REVISION_TYPE_INSERT_TM, RevisionType.InsertTM},
				{REVISION_TYPE_ROLLBACK, RevisionType.Rollback}
			};
		}

		/// <summary>
		/// Открыть вкладку Ревизии
		/// </summary>
		/// <returns>открылась</returns>
		public bool OpenRevisionTab()
		{
			// Открыть вкладку Ревизии
			ClickElement(By.Id(REVISION_BTN_ID));

			// Дождаться открытия
			return WaitUntilDisplayElement(By.Id(REVISION_TAB_ID));
		}

		/// <summary>
		/// Вернуть, открыта ли вкладка Ревизии
		/// </summary>
		/// <returns>открыта</returns>
		public bool GetIsRevisionTabDisplay()
		{
			return GetIsElementDisplay(By.Id(REVISION_TAB_ID));
		}

		/// <summary>
		/// Дождаться появления ревизии
		/// </summary>
		/// <param name="revisionNumber">номер ревизии, которую ожидаем</param>
		/// <returns>появилась</returns>
		public bool WaitRevisionAppear(int revisionNumber)
		{
			var revisionXPath = "";

			revisionXPath = REVISION_LIST_XPATH;

			return WaitUntilDisplayElement(By.XPath(revisionXPath + "[" + revisionNumber + "]"));
		}

		/// <summary>
		/// Получить время ревизии
		/// </summary>
		/// <param name="revisionNumber">номер ревизии</param>
		/// <returns>время (текст в столбце Время)</returns>
		public string GetRevisionTime(int revisionNumber)
		{
			return GetTextElement(By.XPath(GetRevisionCellXPath(revisionNumber, TIME_COLUMN_XPATH)));
		}

		/// <summary>
		/// Получить текст ревизии
		/// </summary>
		/// <param name="revisionNumber">номер ревизии</param>
		/// <returns>текст</returns>
		public string GetRevisionText(int revisionNumber)
		{
			return GetTextElement(By.XPath(GetRevisionCellXPath(revisionNumber, TEXT_COLUMN_XPATH)));
		}
		
		/// <summary>
		/// Кликнуть по кнопке Rollback
		/// </summary>
		public void ClickRollbackBtn()
		{
			ClickElement(By.Id(ROLLBACK_BTN_ID));
		}

		/// <summary>
		/// Вернуть, доступна ли кнопка Rollback
		/// </summary>
		/// <returns>доступна</returns>
		public bool GetIsRollbackBtnEnabled()
		{
			return !GetElementClass(By.Id(ROLLBACK_BTN_ID)).Contains(ROLLBACK_DISABLED_CLASS);
		}

		/// <summary>
		/// Получить тип ревизии
		/// </summary>
		/// <param name="revisionNumber">номер ревизии</param>
		/// <returns>тип</returns>
		public RevisionType GetRevisionType(int revisionNumber)
		{
			var typeStr = GetTextElement(By.XPath(GetRevisionCellXPath(revisionNumber, TYPE_COLUMN_XPATH)));
			
			return revisionTypeList[typeStr];
		}

		/// <summary>
		/// Выделить ревизию
		/// </summary>
		/// <param name="revisionNumber">номер ревизии</param>
		/// <returns>есть ревизия</returns>
		public bool ClickRevision(int revisionNumber)
		{
			var revisionXPath = "";
			
			revisionXPath = REVISION_LIST_XPATH + "[" + revisionNumber + "]//td";

			// Есть ли ревизия
			var isExistRevision = GetIsElementExist(By.XPath(revisionXPath));
			
			if (isExistRevision)
			{
				// Кликнуть ревизию
				ClickElement(By.XPath(revisionXPath));
			}

			return isExistRevision;
		}

		/// <summary>
		/// Вернуть количество ревизий
		/// </summary>
		/// <returns>количество</returns>
		public int GetRevisionListCount()
		{
			var revisionXPath = "";

			revisionXPath = REVISION_LIST_XPATH;

			return GetElementList(By.XPath(revisionXPath)).Count;
		}

		/// <summary>
		/// Кликнуть Time для сортировки
		/// </summary>
		public void ClickTimeToSort()
		{
			ClickElement(By.XPath(TIME_COLUMN_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться появления диалога Rollback
		/// </summary>
		/// <returns>появился</returns>
		public bool WaitRollbackDialogAppear()
		{
			return WaitUntilDisplayElement(By.Id(ROLLBACK_DIALOG_ID));
		}

		/// <summary>
		/// Дождаться пропадания диалога Rollback
		/// </summary>
		/// <returns>пропал</returns>
		public bool WaitUntilRollbackDialogDisappear()
		{
			return WaitUntilDisappearElement(By.Id(ROLLBACK_DIALOG_ID));
		}

		/// <summary>
		/// Кликнуть Yes в диалоге rollback
		/// </summary>
		public void ClickYesRollbackDlg()
		{
			ClickElement(By.XPath(ROLLBACK_DLG_YES_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть No в диалоге rollback
		/// </summary>
		public void ClickNoRollbackDlg()
		{
			ClickElement(By.XPath(ROLLBACK_DLG_NO_BTN_XPATH));
		}

		/// <summary>
		/// Проверить, что в ячейке отмечен удаленный текст
		/// </summary>
		/// <param name="revisionNumber"></param>
		/// <returns>отмечен</returns>
		public bool GetHasRevisionDeletedTextPart(int revisionNumber)
		{
			return GetIsElementExist(By.XPath(GetRevisionCellXPath(revisionNumber, TEXT_COLUMN_XPATH) + "//del"));
		}

		/// <summary>
		/// Проверить, что в ячейке отмечен добавленный текст
		/// </summary>
		/// <param name="revisionNumber"></param>
		/// <returns>отмечен</returns>
		public bool GetHasRevisionInsertedTextPart(int revisionNumber)
		{
			return GetIsElementExist(By.XPath(GetRevisionCellXPath(revisionNumber, TEXT_COLUMN_XPATH) + "//ins"));
		}

		/// <summary>
		/// Вернуть XPath ячейки ревизий
		/// </summary>
		/// <param name="revisionNumber">номер ревизии</param>
		/// <param name="column">название столбика</param>
		/// <returns>xPath</returns>
		protected string GetRevisionCellXPath(int revisionNumber, string column)
		{
			var revisionXPath = "";

			revisionXPath = REVISION_LIST_XPATH;

			return revisionXPath + "[" + revisionNumber + "]" + column;
		}



		public enum RevisionType { AutoSave, Confirmed, InsertMT, InsertTM, Rollback };

		protected const string REVISION_BTN_ID = "revisions-tab";
		protected const string REVISION_TAB_ID = "revisions-body";
		protected const string ROLLBACK_BTN_ID = "revision-rollback-btn";

		protected const string REVISION_LIST_XPATH = "//div[@id='revisions-body']//table";
		protected const string TIME_COLUMN_XPATH = "//td[contains(@class,'revision-date-cell')]";
		protected const string TEXT_COLUMN_XPATH = "//td[contains(@class,'revision-text-cell')]";
		protected const string TYPE_COLUMN_XPATH = "//td[contains(@class,'revision-type-cell')]";

		protected const string TIME_COLUMN_BTN_XPATH = ".//div[contains(@class,'revision-date-column')]//span";

		protected const string ROLLBACK_DISABLED_CLASS = "x-btn-disabled";
		
		protected Dictionary<string, RevisionType> revisionTypeList;
		protected const string REVISION_TYPE_AUTOSAVE = "Autosave";
		protected const string REVISION_TYPE_CONFIRMED = "Confirmation";
		protected const string REVISION_TYPE_INSERT_MT = "MT insertion";
		protected const string REVISION_TYPE_INSERT_TM = "TM insertion";
		protected const string REVISION_TYPE_ROLLBACK = "Restored";

		protected const string ROLLBACK_DIALOG_ID = "rollback";
		protected const string ROLLBACK_DLG_YES_CLASS = "x-btn-blue";
		protected const string ROLLBACK_DLG_NO_CLASS = "x-btn-gray";
		protected const string ROLLBACK_DLG_YES_BTN_XPATH = "//div[@id='" + ROLLBACK_DIALOG_ID + "']//a[contains(@class,'" + ROLLBACK_DLG_YES_CLASS + "')]";
		protected const string ROLLBACK_DLG_NO_BTN_XPATH = "//div[@id='" + ROLLBACK_DIALOG_ID + "']//a[contains(@class,'" + ROLLBACK_DLG_NO_CLASS + "')]";
	}
}