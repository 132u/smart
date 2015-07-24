using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
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
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SEGMENTS_BODY)))
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
		
			return new ProjectSettingsPage().GetPage();
		}

		/// <summary>
		/// Подтвердить текст с помощью грячих клавиш
		/// </summary>
		public EditorPage ConfirmSegmentByHotkeys()
		{
			Logger.Debug("Подтвердить сегмент с помощью горячих клавиш.");
			SendKeys.SendWait("^{ENTER}");

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Подтвердить сегмент"
		/// </summary>
		public EditorPage ClickConfirmButton()
		{
			Logger.Debug("Нажать кнопку 'Подтвердить сегмент'.");
			ConfirmButton.AdvancedClick();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку поиска ошибки в терминологии
		/// </summary>
		public ErrorsDialog ClickFindErrorButton()
		{
			Logger.Debug("Нажать кнопку поиска ошибки в терминологии");
			FindErrorButton.Click();

			return new ErrorsDialog().GetPage();
		}

		/// <summary>
		/// Вызвать окно поиска ошибок в терминологии с помощью хоткея
		/// </summary>
		public ErrorsDialog FindErrorByHotkey()
		{
			Logger.Debug("Вызвать окно поиска ошибок в терминологии с помощью хоткея F7");
			SendKeys.SendWait("{F7}");

			return new ErrorsDialog().GetPage();
		}

		/// <summary>
		/// Проверить, подтвердился ли сегмент
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage AssertIsSegmentConfirmed(int rowNumber)
		{
			Logger.Trace("Проверить, подтвердился ли сегмент {0}.", rowNumber);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay
				(By.XPath(CONFIRMED_ICO.Replace("*#*", (rowNumber - 1).ToString())), 6),
				"Произошла ошибка:\n не удалось подтвердить сегмент с номером {0}.", rowNumber);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что тип последней ревизии соответствует ожидаемому
		/// </summary>
		public EditorPage AssertLastRevisionEqualTo(RevisionType expectedRevisionType)
		{
			Logger.Trace("Проверить, что тип последней ревизии соответствует ожидаемому типу {0}", expectedRevisionType);

			Assert.IsTrue(DataDictionaries.RevisionDictionary[RevisionType.Text] == expectedRevisionType,
				"Произошла ошибка:\n тип последней ревизии  не соответствует ожидаемому.");

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по таргету сегмента
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage ClickTargetCell(int rowNumber)
		{
			Logger.Debug("Кликнуть по таргету сегмента {0}.", rowNumber);
			TargetCell = Driver.SetDynamicValue(How.XPath, TARGET_CELL, (rowNumber - 1).ToString());
			TargetCell.Click();

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
			TargetCell = Driver.SetDynamicValue(How.XPath, TARGET_CELL, (rowNumber - 1).ToString());
			TargetCell.Click();
			TargetCell.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку открытия словаря
		/// </summary>
		public SpellcheckDictionaryDialog ClickSpellcheckDictionaryButton()
		{
			Logger.Debug("Нажать кнопку открытия словаря");
			SpellcheckDictionaryButton.Click();

			return new SpellcheckDictionaryDialog().GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Go to Last Unconfirmed Segment'
		/// </summary>
		public EditorPage ClickLastUnconfirmedButton()
		{
			Logger.Debug("Нажать кнопку 'Go to Last Unconfirmed Segment'.");
			LastUnconfirmedButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать хоткей F9
		/// </summary>
		public EditorPage ClickF9HotKey()
		{
			Logger.Debug("Нажать хоткей F9.");
			SendKeys.SendWait("{F9}");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что слово подчеркнуто в сегменте
		/// </summary>
		public EditorPage AssertUnderlineForWordExist(string word)
		{
			Logger.Trace("Проверить, что слово {0} подчеркнуто в сегменте", word);
			var underlineWordPath = SPELLCHECK_PATH.Replace("*#*", word);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(underlineWordPath)),
				"Произошла ошибка:\n не удалось найти слово {0} подчеркнутых.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что слово не подчеркнуто в сегменте
		/// </summary>
		public EditorPage AssertUnderlineForWordNotExist(string word)
		{
			Logger.Trace("Проверить, что слово {0} не подчеркнуто в сегменте", word);
			var wordsWithUnderline = Driver.GetElementList(By.XPath(SPELLCHECK_PATH)).Select(item => item.Text);

			Assert.IsFalse(wordsWithUnderline.Contains(word),
				"Произошла ошибка:\n слово {0} найдено среди подчеркнутых.");

			return GetPage();
		}

		/// <summary>
		/// Получить название этапа
		/// </summary>
		public string GetStage()
		{
			Logger.Trace("Получить название этапа");

			return StageName.Text;
		}

		/// <summary>
		/// Проверить, что таргет сегмента виден
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage AssertTargetDisplayed(int rowNumber)
		{
			Logger.Trace("Проверить, что таргет сегмента №{0} виден", rowNumber);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(TARGET_CELL_XPATH.Replace("*#*", (rowNumber - 1).ToString()))),
				"Произошла ошибка:\n сегмент с номером {0} не появился.", rowNumber);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что название этапа пустое.
		/// </summary>
		public EditorPage AssertStageNameIsEmpty()
		{
			Logger.Trace("Проверить, что название этапа пустое.");

			Assert.IsFalse(Driver.ElementIsDisplayed(By.XPath(STAGE_NAME)),
				"Произошла ошибка:\n название этапа проставлено.");

			return GetPage();
		}
		
		/// <summary>
		/// Проверить, что сегмент активен (подсвечен голубым цветом)
		/// </summary>
		public EditorPage AssertSegmentIsSelected(int segmentNumber)
		{
			Logger.Trace("Проверить, что сегмент №{0} активен (подсвечен голубым цветом).", segmentNumber);

			Assert.IsTrue(Driver.GetIsElementExist(By.XPath(SELECTED_SEGMENT.Replace("*#*", segmentNumber.ToString()))),
				"Произошла ошибка:\n сегмент №{0} не выделен, не подсвечен голубым цветом.", segmentNumber);

			return GetPage();
		}

		/// <summary>
		/// Закрыть туториал, если он виден.
		/// </summary>
		public EditorPage CloseTutorialIfExist()
		{
			Logger.Trace("Проверить, видна ли подсказка.");

			if (tutorialExist())
			{
				Logger.Debug("Закрыть подсказку.");
				FinishTutorialButton.Click();
			}

			return GetPage();
		}

		private static bool tutorialExist()
		{
			Logger.Trace("Проверить, открыта ли подсказка");
			return Driver.WaitUntilElementIsDisplay(By.XPath(FINISH_TUTORIAL_BUTTON), timeout: 5);
		}

		[FindsBy(How = How.XPath, Using = CONFIRM_BTN)]
		protected IWebElement ConfirmButton { get; set; }

		[FindsBy(Using = FIND_ERROR_BTN_ID)]
		protected IWebElement FindErrorButton { get; set; }

		[FindsBy(How = How.Id, Using = HOME_BTN_ID)]
		protected IWebElement HomeButton { get; set; }

		[FindsBy(How = How.XPath, Using = FINISH_TUTORIAL_BUTTON)]
		protected IWebElement FinishTutorialButton { get; set; }

		[FindsBy(Using = DICTIONARY_BTN_ID)]
		protected IWebElement SpellcheckDictionaryButton { get; set; }

		[FindsBy(Using = REVISION_PATH, How = How.XPath)]
		protected IWebElement RevisionType { get; set; }
		
		[FindsBy(How = How.XPath, Using = STAGE_NAME)]
		protected IWebElement StageName { get; set; }

		[FindsBy(How = How.Id, Using = LAST_CONFIRMED_BUTTON)]
		protected IWebElement LastUnconfirmedButton { get; set; }
		
		protected IWebElement TargetCell { get; set; }

		protected const string HOME_BTN_ID = "back-btn";
		protected const string DICTIONARY_BTN_ID = "dictionary-btn";
		protected const string CONFIRM_BTN = "//a[@id='confirm-btn']";
		protected const string FIND_ERROR_BTN_ID = "qa-error-btn";
		protected const string FINISH_TUTORIAL_BUTTON = "//span[contains(text(),'Finish') and contains(@id, 'button')]";
		protected const string AUTOSAVING = "//div[contains(text(), 'Saving')]";
		protected const string SPELLCHECK_PATH = "//div[contains(text(), '1')]//..//..//..//..//tbody//tr[1]//span[contains(@class,'spellcheck') and contains(string(), '*#*')]";
		protected const string REVISION_PATH = "//div[@id='revisions-body']//table[1]//td[contains(@class,'revision-type-cell')]";
		protected const string STAGE_NAME = "//h1/span[contains(@class, 'workflow')]";
		protected const string LAST_CONFIRMED_BUTTON = "unfinished-btn";
		protected const string SELECTED_SEGMENT = "//table[2]//tr[@aria-selected='true']";

		protected const string ROW_NUMBER_ACTIVE_XPATH = ".//div[@id='segments-body']//table//td[contains(@class, 'x-grid-item-focused')]/../td[1]//div[contains(@class, 'row-numberer')]";
		protected const string FIRST_VISIBLE_SEGMENT_XPATH = "//div[@id='segments-body']//table[1]//td[1]//div[contains(@class, 'row-numberer')]";
		protected const string LAST_VISIBLE_SEGMENT_XPATH = "//div[@id='segments-body']//table[*#*]//td[1]//div[contains(@class, 'row-numberer')]";
		protected const string TARGET_CELL_XPATH = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[3]//div//div";
		protected const string SEGMENTS_TABLE_XPATH = "//div[@id='segments-body']//div//div[2]//table";
		protected const string SEGMENTS_BODY = "//div[@id='segments-body']//table";
		protected const string CONFIRMED_ICO = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[contains(@class,'info-cell')]//div[contains(@class,'fa-check')]";
		protected const string TARGET_CELL = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[3]//div//div";
	}
}
