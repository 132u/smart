using System;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class EditorPage : BaseObject, IAbstractPage<EditorPage>
	{
		public EditorPage GetPage()
		{
			var editorPage = new EditorPage();
			InitPage(editorPage);

			return editorPage;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SEGMENTS_BODY_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось открыть документ в редакторе.");
			}
		}


		/// <summary>
		/// Нажать кнопку "Домой"
		/// </summary>
		public ProjectSettingsPage ClickHomeButton()
		{
			Logger.Debug("Нажать кнопку 'Домой'.");
			HomeButton.Click();
			Driver.SwitchTo().Window(Driver.WindowHandles[1]).Close();
			Driver.SwitchTo().Window(Driver.WindowHandles[0]);
			var projectSettingsPage = new ProjectSettingsPage();

			return projectSettingsPage.GetPage();
		}

		/// <summary>
		/// Проверить, заблокирован ли сегмент
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public bool IsSegmentLocked(int rowNumber)
		{
			Logger.Trace("Проверить, заблокирован ли сегмент {0}.", rowNumber);

			return Driver.ElementIsDisplayed(By.XPath(LOCK_ICO_XPATH.Replace("*#*", (rowNumber - 1).ToString())));
		}

		/// <summary>
		/// Нажать на кнопку "Подтвердить сегмент"
		/// </summary>
		public EditorPage ClickConfirmButton()
		{
			Logger.Debug("Нажать кнопку 'Подтвердить сегмент'.");
			ConfirmButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, подтвердился ли сегмент
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage AssertIsSegmentConfirmed(int rowNumber)
		{
			Logger.Debug("Проверить, подтвердился ли сегмент {0}.", rowNumber);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay
				(By.XPath(CONFIRMED_ICO_XPATH.Replace("*#*", (rowNumber - 1).ToString())), 6),
				"Произошла ошибка:\n не удалось подтвердить сегмент с номером {0}.", rowNumber);

			return GetPage();
		}

		/// <summary>
		/// Проверить, сохранились ли сегменты
		/// </summary>
		public EditorPage WaitUntillAllSegmentsSaved()
		{
			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(AUTOSAVING_XPATH)),
				"Произошла ошибка:\n не удалось дождаться сохранения сегментаю");

			return GetPage();
		}

		/// <summary>
		/// Получить номер активного сегмента
		/// </summary>
		public int GetRowNumberActiveSegment()
		{
			Logger.Debug("Получить номер активного сегмента.");
			return Convert.ToInt32(Driver.SetDynamicValue(How.XPath, ROW_NUMBER_ACTIVE_XPATH, "").Text);
		}

		/// <summary>
		/// Получить номер первого видимого сегмента
		/// </summary>
		public int GetRowNumberFirstVisibleSegment()
		{
			Logger.Debug("Получить номер первого видимого сегмента.");
			try
			{
				return Convert.ToInt32(Driver.SetDynamicValue(How.XPath, FIRST_VISIBLE_SEGMENT_XPATH, "").Text);
			}
			catch (StaleElementReferenceException)
			{

				return Convert.ToInt32(Driver.SetDynamicValue(How.XPath, FIRST_VISIBLE_SEGMENT_XPATH, "").Text);
			}
		}

		/// <summary>
		/// Кликнуть по первому видимому элементу
		/// </summary>
		public EditorPage ClickFirstVisibleSegment()
		{
			Logger.Debug("Кликнуть по первому видимому элементу.");
			FirstVisibleSegment = Driver.SetDynamicValue(How.XPath, FIRST_VISIBLE_SEGMENT_XPATH, "");
			FirstVisibleSegment.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по последнему видимому элементу
		/// </summary>
		public EditorPage ClickLastVisibleSegment()
		{
			Logger.Debug("Кликнуть по последнему видимому элементу.");
			var visibleSegmentsCount = Driver.GetElementList(By.XPath(SEGMENTS_TABLE_XPATH)).Count;
			LastVisibleSegment = Driver.SetDynamicValue(How.XPath, LAST_VISIBLE_SEGMENT_XPATH, visibleSegmentsCount.ToString());
			LastVisibleSegment.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по таргету сегмента
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage ClickTargetCell(int rowNumber)
		{
			Logger.Debug("Кликнуть по таргету сегмента {0}.", rowNumber);
			TargetCell = Driver.SetDynamicValue(How.XPath,
				TARGET_CELL_XPATH, (rowNumber - 1).ToString());
			TargetCell.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку перехода к следующему неподтвержденному сегменту
		/// </summary>
		public EditorPage ClickUnfinishedButton()
		{
			Logger.Debug("Нажать кнопку перехода к следующему неподтвержденному сегменту.");
			UnfinishedButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Обновить страницу редактора
		/// </summary>
		public SelectTaskDialog RefreshPage()
		{
			Logger.Debug("Обновить страницу.");
			Driver.Navigate().Refresh();
			var taskDialog = new SelectTaskDialog();

			return taskDialog.GetPage();
		}

		/// <summary>
		/// Проверить, виден ли данный сегмент
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public bool IsSegmentVisible(int rowNumber)
		{
			Logger.Trace("Проверить, виден ли сегмент {0}.", rowNumber);

			return Driver.ElementIsDisplayed(By.XPath(TARGET_CELL_XPATH.Replace("*#*", (rowNumber - 1).ToString())));
		}

		/// <summary>
		/// Перевод курсора в конец строки сегмента
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage CursorToTargetLineEndingByHotkey(int rowNumber)
		{
			Logger.Debug("Перевести курсор в конец строки сегмента {0}.", rowNumber);
			TargetCell = Driver.SetDynamicValue(How.XPath, TARGET_CELL_XPATH, (rowNumber - 1).ToString());
			TargetCell.SendKeys(Keys.Control + Keys.End);

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в таргет сегмента
		/// </summary>
		/// <param name="text">текст</param>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage SendTargetText(string text, int rowNumber)
		{
			Logger.Debug("Ввести текст в таргет сегмента {0}.", rowNumber);
			TargetCell = Driver.SetDynamicValue(How.XPath, TARGET_CELL_XPATH, (rowNumber - 1).ToString());
			TargetCell.SendKeys(text);

			return GetPage();
		}


		/// <summary>
		/// Проверить, что панель CAT не пустая
		/// </summary>
		public bool IsCatPanelNotEmpty()
		{
			Logger.Trace("Проверить, что панель CAT не пустая.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(CAT_PANEL_EXISTENCE_XPATH), 180);
		}

		[FindsBy(Using = CONFIRM_BTN_ID)]
		protected IWebElement ConfirmButton { get; set; }

		[FindsBy(Using = UNFINISHED_BTN_ID)]
		protected IWebElement UnfinishedButton { get; set; }

		[FindsBy(Using = HOME_BTN_ID)]
		protected IWebElement HomeButton { get; set; }
		
		protected IWebElement TargetCell { get; set; }

		protected IWebElement FirstVisibleSegment { get; set; }

		protected IWebElement LastVisibleSegment { get; set; }

		protected const string SEGMENTS_BODY_XPATH = ".//div[@id='segments-body']//table";
		protected const string ERROR_MESSAGE_UNFINISHED_MT_XPATH = "//div[contains(string(), 'Machine translation of the block has not finished yet.')]";
		protected const string MESSAGE_ALL_SEGMENTS_CONFIRMED_XPATH = "//div[contains(string(), 'All segments are confirmed or blocked.')]";
		protected const string MESSAGE_OK_BTN_XPATH_XPATH = "//div[contains(@id, 'messagebox')]//a[contains(string(), 'OK')]";
		protected const string CAT_PANEL_EXISTENCE_XPATH = "//div[@id='cat-body']//table";

		protected const string UNFINISHED_BTN_ID = "unfinished-btn";
		protected const string HOME_BTN_ID = "back-btn";
		
		protected const string CONFIRM_BTN_ID = "confirm-btn";
		protected const string CONFIRMED_ICO_XPATH = ".//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[contains(@class,'info-cell')]//span[contains(@class,'fa-check')]";
		protected const string LOCK_ICO_XPATH = ".//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[contains(@class,'info-cell')]//span[contains(@class,'fa-lock')][not(contains(@class,'inactive'))]";
		protected const string AUTOSAVING_XPATH = ".//div[contains(text(), 'Saving')]";

		protected const string ROW_NUMBER_ACTIVE_XPATH = ".//div[@id='segments-body']//table//td[contains(@class, 'x-grid-item-focused')]/../td[1]//div[contains(@class, 'row-numberer')]";
		protected const string FIRST_VISIBLE_SEGMENT_XPATH = "//div[@id='segments-body']//table[1]//td[1]//div[contains(@class, 'row-numberer')]";
		protected const string LAST_VISIBLE_SEGMENT_XPATH = "//div[@id='segments-body']//table[*#*]//td[1]//div[contains(@class, 'row-numberer')]";
		protected const string TARGET_CELL_XPATH = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[3]//div//pre";
		protected const string SEGMENTS_TABLE_XPATH = "//div[@id='segments-body']//div//div[2]//table";
	}
}
