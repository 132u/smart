﻿using System.Collections.Generic;
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
				{REVISION_TYPE_MANUAL_INPUT, RevisionType.ManualInput},
				{REVISION_TYPE_CONFIRMED, RevisionType.Confirmed},
				{REVISION_TYPE_INSERT_MT, RevisionType.InsertMT},
				{REVISION_TYPE_INSERT_TM, RevisionType.InsertTM},
				{REVISION_TYPE_ROLLBACK, RevisionType.Restored},
				{REVISION_TYPE_INSERT_TB, RevisionType.InsertTb},
				{REVISION_TYPE_PRETRANSLATION, RevisionType.Pretranslation}
			};
		}

		/// <summary>
		/// Открыть вкладку Ревизии
		/// </summary>
		/// <returns>открылась</returns>
		public bool OpenRevisionTab()
		{
			Logger.Trace("Открыть вкладку Ревизии");
			ClickElement(By.Id(REVISION_BTN_ID));
			Logger.Trace("Ожидание октрыти вкладки Ревизии");
			return WaitUntilDisplayElement(By.Id(REVISION_TAB_ID));
		}

		/// <summary>
		/// Вернуть, открыта ли вкладка Ревизии
		/// </summary>
		/// <returns>открыта</returns>
		public bool GetIsRevisionTabDisplay()
		{
			Logger.Trace("Проверить, открыта ли вкладка Ревизии");
			return GetIsElementDisplay(By.Id(REVISION_TAB_ID));
		}

		/// <summary>
		/// Кликнуть по кнопке Rollback
		/// </summary>
		public void ClickRollbackBtn()
		{
			Logger.Trace("Кликнуть по кнопке Rollback");
			ClickElement(By.Id(ROLLBACK_BTN_ID));
		}

		/// <summary>
		/// Вернуть, доступна ли кнопка Rollback
		/// </summary>
		/// <returns>доступна</returns>
		public bool GetIsRollbackBtnEnabled()
		{
			Logger.Trace("Проверить, доступна ли кнопка Rollback");
			return !GetElementClass(By.Id(ROLLBACK_BTN_ID)).Contains(ROLLBACK_DISABLED_CLASS);
		}

		/// <summary>
		/// Получить тип ревизии
		/// </summary>
		/// <param name="revisionNumber">номер ревизии</param>
		/// <returns>тип</returns>
		public RevisionType GetRevisionType(int revisionNumber)
		{
			Logger.Trace(string.Format("Получаем тип ревизии с номером {0}", revisionNumber));
			var typeStr = GetTextElement(By.XPath(GetRevisionCellXPath(revisionNumber, TYPE_COLUMN_XPATH)));
			return revisionTypeList[typeStr];
		}

		/// <summary>
		/// Получить имя создателя ревизии
		/// </summary>
		/// <param name="revisionNumber">номер ревизии</param>
		/// <returns>имя</returns>
		public string GetRevisionUser(int revisionNumber)
		{
			Logger.Trace("Получить имя создателя ревизии №" + revisionNumber);
			return GetTextElement(By.XPath(GetRevisionCellXPath(revisionNumber, USER_COLUMN_XPATH)));
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

			Logger.Trace("Проверить есть ли ревизия №" + revisionNumber);
			var isExistRevision = GetIsElementExist(By.XPath(revisionXPath));
			
			if (isExistRevision)
			{
				Logger.Trace("Клик по ревизии №" + revisionNumber);
				ClickElement(By.XPath(revisionXPath));
			}

			return isExistRevision;
		}

		public int GetRevisionListCount()
		{
			Logger.Trace("Получить количество ревизий");
			return GetElementsCount(By.XPath(REVISION_LIST_XPATH));
		}

		/// <summary>
		/// Кликнуть User для сортировки
		/// </summary>
		public void ClickUserToSort()
		{
			Logger.Trace("Кликнуть User для сортировки");
			ClickElement(By.XPath(USER_COLUMN_BTN_XPATH));
		}

		/// <summary>
		/// Дождаться появления диалога Rollback
		/// </summary>
		/// <returns>появился</returns>
		public bool WaitRollbackDialogAppear()
		{
			Logger.Trace("Ожидание появления диалога Rollback");
			return WaitUntilDisplayElement(By.Id(ROLLBACK_DIALOG_ID));
		}

		/// <summary>
		/// Дождаться пропадания диалога Rollback
		/// </summary>
		/// <returns>пропал</returns>
		public bool WaitUntilRollbackDialogDisappear()
		{
			Logger.Trace("Ожидание закрытия диалога Rollback");
			return WaitUntilDisappearElement(By.Id(ROLLBACK_DIALOG_ID));
		}

		/// <summary>
		/// Кликнуть No в диалоге rollback
		/// </summary>
		public void ClickNoRollbackDlg()
		{
			Logger.Trace("Кликнуть No в диалоге Rollback");
			ClickElement(By.XPath(ROLLBACK_DLG_NO_BTN_XPATH));
		}

		/// <summary>
		/// Проверить, что в ячейке отмечен удаленный текст
		/// </summary>
		/// <param name="revisionNumber"></param>
		/// <returns>отмечен</returns>
		public bool GetHasRevisionDeletedTextPart(int revisionNumber)
		{
			Logger.Trace("Проверить, что в ячейке отмечен удаленный текст, ревизия №" + revisionNumber);
			return GetIsElementExist(By.XPath(GetRevisionCellXPath(revisionNumber, TEXT_COLUMN_XPATH) + "//del"));
		}

		/// <summary>
		/// Проверить, что в ячейке отмечен добавленный текст
		/// </summary>
		/// <param name="revisionNumber"></param>
		/// <returns>отмечен</returns>
		public bool GetHasRevisionInsertedTextPart(int revisionNumber)
		{
			Logger.Trace("Проверить, что в ячейке отмечен добавленный текст, ревизия №" + revisionNumber);
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
			return REVISION_LIST_XPATH + "[" + revisionNumber + "]" + column;
		}

		public enum RevisionType { ManualInput, Confirmed, InsertMT, InsertTM, Restored, InsertTb, Pretranslation};

		protected const string REVISION_BTN_ID = "revisions-tab";
		protected const string REVISION_TAB_ID = "revisions-body";
		protected const string ROLLBACK_BTN_ID = "revision-rollback-btn";

		protected const string REVISION_LIST_XPATH = "//div[@id='revisions-body']//table";
		protected const string TIME_COLUMN_XPATH = "//td[contains(@class,'revision-date-cell')]";
		protected const string TEXT_COLUMN_XPATH = "//td[contains(@class,'revision-text-cell')]";
		protected const string TYPE_COLUMN_XPATH = "//td[contains(@class,'revision-type-cell')]";
		protected const string USER_COLUMN_XPATH = "//td[contains(@class,'revision-user-cell')]";

		protected const string TIME_COLUMN_BTN_XPATH = ".//div[contains(@class,'revision-date-column')]//span";
		protected const string USER_COLUMN_BTN_XPATH = "//div[@id='gridcolumn-1105']//span";

		protected const string ROLLBACK_DISABLED_CLASS = "x-btn-disabled";

		protected Dictionary<string, RevisionType> revisionTypeList;
		protected const string REVISION_TYPE_MANUAL_INPUT = "Manual input";
		protected const string REVISION_TYPE_CONFIRMED = "Confirmation";
		protected const string REVISION_TYPE_INSERT_MT = "MT insertion";
		protected const string REVISION_TYPE_INSERT_TM = "TM insertion";
		protected const string REVISION_TYPE_ROLLBACK = "Restored";
		protected const string REVISION_TYPE_INSERT_TB = "TB insertion";
		protected const string REVISION_TYPE_PRETRANSLATION = "Pretranslation";

		protected const string ROLLBACK_DIALOG_ID = "rollback";
		protected const string ROLLBACK_DLG_YES_CLASS = "x-btn-blue";
		protected const string ROLLBACK_DLG_NO_CLASS = "x-btn-gray";
		protected const string ROLLBACK_DLG_YES_BTN_XPATH = "//div[@id='" + ROLLBACK_DIALOG_ID + "']//a[contains(@class,'" + ROLLBACK_DLG_YES_CLASS + "')]";
		protected const string ROLLBACK_DLG_NO_BTN_XPATH = "//div[@id='" + ROLLBACK_DIALOG_ID + "']//a[contains(@class,'" + ROLLBACK_DLG_NO_CLASS + "')]";
	}
}