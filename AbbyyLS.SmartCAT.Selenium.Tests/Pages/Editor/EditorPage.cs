using System.Linq;
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
		/// Нажать хоткей F8
		/// </summary>
		public EditorPage ClickF8HotKey()
		{
			Logger.Debug("Нажать хоткей F8.");
			SendKeys.SendWait("{F8}");

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

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRMED_ICO.Replace("*#*", (rowNumber - 1).ToString()))),
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
		/// Нажать кнопку 'Специальные символы'
		/// </summary>
		public SpecialCharactersForm ClickCharacterButton()
		{
			Logger.Debug("Нажать кнопку 'Специальные символы'.");
			CharacterButton.Click();

			return new SpecialCharactersForm().GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Специальные символы' с помощью хоткея Ctrl+Shift+I
		/// </summary>
		public SpecialCharactersForm ClickCharacterButtonByHotKey()
		{
			Logger.Debug("Нажать кнопку 'Специальные символы' с помощью хоткея Ctrl+Shift+I.");
			SendKeys.SendWait(@"^+{i}");

			return new SpecialCharactersForm().GetPage();
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
		/// Нажать кнопку 'Вставить тег'
		/// </summary>
		public EditorPage ClickInsertTagButton()
		{
			Logger.Debug("Нажать кнопку 'Вставить тег'.");
			InsertTagButton.Click();

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
		/// Проверить, что появился тег в таргете
		/// </summary>
		public EditorPage AssertTagIsDisplayed(int segmentNumber)
		{
			Logger.Trace("Проверить, что появился тег в таргете.");
			var targetTag = Driver.SetDynamicValue(How.XPath, TAG, segmentNumber.ToString());

			Assert.IsTrue(targetTag.Displayed, "Произошла ошибка:\n тег не появился в таргете.");

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


		/// <summary>
		/// Получить текст из оригинала
		/// </summary>
		public string SourceText(int segmentNumber)
		{
			Logger.Trace("Получить текст из оригинала №{0}.", segmentNumber);

			return Driver.SetDynamicValue(How.XPath, SOURCE_CELL, segmentNumber.ToString()).Text;
		}

		/// <summary>
		/// Нажать кнопку 'Копировать оригинал в перевод'
		/// </summary>
		public EditorPage ClickCopyButton()
		{
			Logger.Debug("Нажать кнопку 'Копировать оригинал в перевод'.");
			CopyButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать хоткей кнопки 'Копировать оригинал в перевод'
		/// </summary>
		public EditorPage ClickCopyButtonByHotkey()
		{
			Logger.Debug("Нажать хоткей кнопки 'Копировать оригинал в перевод' - Ctrl+Insert.");
			SendKeys.SendWait("^{INSERT}");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что Конкордансный поиск появился
		/// </summary>
		public EditorPage AssertConcordanceSearchIsDisplayed()
		{
			Logger.Trace("Проверить, что Конкордансный поиск появился.");
			
			Assert.IsTrue(ConcordanceSearch.Displayed, "Произошла ошибка:\n Конкордансный поиск не появился.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сегмент залочен
		/// </summary>
		public EditorPage AssertSegmentIsLocked(int segmentNumber)
		{
			Logger.Trace("Проверить, что сегмент №{0} залочен.", segmentNumber);
			var segmentLock = Driver.SetDynamicValue(How.XPath, SEGMENT_LOCK, segmentNumber.ToString());

			Assert.IsTrue(segmentLock.Displayed, "Произошла ошибка:\n сегмент не залочен.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сегмент не залочен
		/// </summary>
		public EditorPage AssertSegmentIsNotLocked(int segmentNumber)
		{
			Logger.Trace("Проверить, что сегмент №{0} не залочен.", segmentNumber);

			Assert.IsFalse(Driver.GetIsElementExist(By.XPath(SEGMENT_LOCK.Replace("*#*", segmentNumber.ToString()))),
				"Произошла ошибка:\n сегмент залочен.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, статус 'Saving...' исчез
		/// </summary>
		public EditorPage AssertSaveingStatusIsDisappeared()
		{
			Logger.Trace("Проверить, статус 'Saving...' исчез.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(SAVING_STATUS)), "Произошла ошибка:\n  статус 'Saving...' не исчез.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Конкордансный поиск'
		/// </summary>
		public EditorPage ClickConcordanceButton()
		{
			Logger.Debug("Нажать кнопку 'Конкордансный поиск'.");
			ConcordanceButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать хоткей кнопки 'Конкордансный поиск' - Ctrl+k
		/// </summary>
		public EditorPage ClickConcordanceButtonByHotKey()
		{
			Logger.Debug("Нажать хоткей кнопки 'Конкордансный поиск' - Ctrl+k.");
			SendKeys.SendWait(@"^{k}");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Rollback
		/// </summary>
		public EditorPage ClickRollbackButton()
		{
			Logger.Debug("Нажать кнопку Rollback.");
			RollbackButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить текст из таргета
		/// </summary>
		public string TargetText(int segmentNumber)
		{
			Logger.Trace("Получить текст из таргета №{0}.", segmentNumber);

			return Driver.SetDynamicValue(How.XPath, TARGET_CELL, (segmentNumber-1).ToString()).Text;
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

		[FindsBy(How = How.Id, Using = HOME_BUTTON)]
		protected IWebElement HomeButton { get; set; }

		[FindsBy(How = How.XPath, Using = FINISH_TUTORIAL_BUTTON)]
		protected IWebElement FinishTutorialButton { get; set; }

		[FindsBy(Using = DICTIONARY_BUTTON)]
		protected IWebElement SpellcheckDictionaryButton { get; set; }

		[FindsBy(Using = REVISION_PATH, How = How.XPath)]
		protected IWebElement RevisionType { get; set; }
		
		[FindsBy(How = How.XPath, Using = STAGE_NAME)]
		protected IWebElement StageName { get; set; }

		[FindsBy(How = How.Id, Using = LAST_CONFIRMED_BUTTON)]
		protected IWebElement LastUnconfirmedButton { get; set; }
		
		[FindsBy(How = How.Id, Using = CHARACTER_BUTTON)]
		protected IWebElement CharacterButton { get; set; }


		[FindsBy(How = How.Id, Using = CHARACTER_FORM)]
		protected IWebElement CharacterForm { get; set; }

		[FindsBy(How = How.Id, Using = INSERT_TAG_BUTTON)]
		protected IWebElement InsertTagButton { get; set; }

		[FindsBy(How = How.XPath, Using = TAG)]
		protected IWebElement Tag { get; set; }

		[FindsBy(How = How.Id, Using = COPY_BUTTON)]
		protected IWebElement CopyButton { get; set; }

		[FindsBy(How = How.Id, Using = CONCORDANCE_BUTTON)]
		protected IWebElement ConcordanceButton { get; set; }

		[FindsBy(How = How.Id, Using = CONCORDANCE_SEARCH)]
		protected IWebElement ConcordanceSearch { get; set; }

		[FindsBy(How = How.Id, Using = ROLLBACK_BUTTON)]
		protected IWebElement RollbackButton { get; set; }

		[FindsBy(How = How.Id, Using = SAVING_STATUS)]
		protected IWebElement SavingStatus { get; set; }

		protected IWebElement TargetCell { get; set; }

		protected const string HOME_BUTTON = "back-btn";
		protected const string DICTIONARY_BUTTON = "dictionary-btn";
		protected const string CONFIRM_BTN = "//a[@id='confirm-btn']";
		protected const string FIND_ERROR_BTN_ID = "qa-error-btn";
		protected const string FINISH_TUTORIAL_BUTTON = "//span[contains(text(),'Finish') and contains(@id, 'button')]";
		protected const string AUTOSAVING = "//div[contains(text(), 'Saving')]";
		protected const string SPELLCHECK_PATH = "//div[contains(text(), '1')]//..//..//..//..//tbody//tr[1]//span[contains(@class,'spellcheck') and contains(string(), '*#*')]";
		protected const string REVISION_PATH = "//div[@id='revisions-body']//table[1]//td[contains(@class,'revision-type-cell')]";
		protected const string STAGE_NAME = "//h1/span[contains(@class, 'workflow')]";
		protected const string LAST_CONFIRMED_BUTTON = "unfinished-btn";
		protected const string SELECTED_SEGMENT = "//table[2]//tr[@aria-selected='true']";
		protected const string CHARACTER_BUTTON = "charmap-btn";
		protected const string INSERT_TAG_BUTTON = "tag-insert-btn";
		protected const string COPY_BUTTON = "copy-btn-btnEl";
		protected const string CONCORDANCE_BUTTON = "concordance-search-btn";
		protected const string ROLLBACK_BUTTON = "step-rollback-btn";


		protected const string ROW_NUMBER_ACTIVE_XPATH = ".//div[@id='segments-body']//table//td[contains(@class, 'x-grid-item-focused')]/../td[1]//div[contains(@class, 'row-numberer')]";
		protected const string FIRST_VISIBLE_SEGMENT_XPATH = "//div[@id='segments-body']//table[1]//td[1]//div[contains(@class, 'row-numberer')]";
		protected const string LAST_VISIBLE_SEGMENT_XPATH = "//div[@id='segments-body']//table[*#*]//td[1]//div[contains(@class, 'row-numberer')]";
		protected const string TARGET_CELL_XPATH = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[3]//div//div";
		protected const string SEGMENTS_TABLE_XPATH = "//div[@id='segments-body']//div//div[2]//table";
		protected const string SEGMENTS_BODY = "//div[@id='segments-body']//table";
		protected const string CONFIRMED_ICO = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[contains(@class,'info-cell')]//div[contains(@class,'fa-check')]";
		protected const string TARGET_CELL = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[3]//div//div";
		protected const string SOURCE_CELL = "//div[contains(text(), '*#*')]//..//..//..//..//tbody//tr[1]//td[2]//div//pre";
		protected const string TAG = "//div[contains(text(), '*#*')]//..//..//..//..//tbody//tr[1]//td[3]//div//img[contains(@class,'tag')]";
		protected const string SEGMENT_LOCK = "//div[contains(text(), '1')]//..//..//..//..//tbody//tr[1]//td[contains(@class,'info-cell')]//div[contains(@class,'fa-lock')][not(contains(@class,'inactive'))]";

		protected const string CHARACTER_FORM = "charmap";
		protected const string CONCORDANCE_SEARCH= "concordance-search";

		protected const string SAVING_STATUS = "//div[@id='segmentsavingindicator-1048-innerCt' and contains(text(),'Saving')]";
	}
}
