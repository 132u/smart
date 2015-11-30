using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class EditorPage : BaseObject, IAbstractPage<EditorPage>
	{
		public WebDriver Driver { get; private set; }

		public EditorPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public EditorPage GetPage()
		{
			var editorPage = new EditorPage(Driver);
			InitPage(editorPage, Driver);

			return editorPage;
		}

		public void LoadPage()
		{
			if (!IsEditorPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось открыть документ в редакторе");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку "Домой"
		/// </summary>
		public ProjectSettingsPage ClickHomeButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Домой'.");
			HomeButton.Click();

			return new ProjectSettingsPage(Driver).GetPage();
		}

		/// <summary>
		/// Подтвердить текст с помощью грячих клавиш Ctrl+Enter
		/// </summary>
		public EditorPage ConfirmSegmentByHotkeys()
		{
			CustomTestContext.WriteLine("Подтвердить сегмент с помощью горячих клавиш Ctrl+Enter");
			Driver.SendHotKeys(Keys.Enter, control: true);

			return GetPage();
		}

		/// <summary>
		/// Вставить тег нажатием клавиши F8
		/// </summary>
		public EditorPage InsertTagByHotKey()
		{
			CustomTestContext.WriteLine("Вставить тег нажатием клавиши F8");
			Driver.SendHotKeys(Keys.F8);

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Подтвердить сегмент"
		/// </summary>
		public EditorPage ConfirmSegmentTranslation()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Подтвердить сегмент'.");
			ConfirmButton.AdvancedClick();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку поиска ошибки в терминологии
		/// </summary>
		public ErrorsDialog ClickFindErrorButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку поиска ошибки в терминологии");
			FindErrorButton.Click();

			return new ErrorsDialog(Driver).GetPage();
		}

		/// <summary>
		/// Вызвать окно поиска ошибок в терминологии с помощью хоткея F7
		/// </summary>
		public ErrorsDialog OpenFindErrorsDialogByHotkey()
		{
			CustomTestContext.WriteLine("Вызвать окно поиска ошибок в терминологии с помощью хоткея F7");
			Driver.SendHotKeys(OpenQA.Selenium.Keys.F7);

			return new ErrorsDialog(Driver).GetPage();
		}

		/// <summary>
		/// Кликнуть по таргету сегмента
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage ClickOnTargetCellInSegment(int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Кликнуть по таргету сегмента {0}.", rowNumber);
			TargetCell = Driver.SetDynamicValue(How.XPath, TARGET_CELL, (rowNumber - 1).ToString());
			TargetCell.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Специальные символы'
		/// </summary>
		public SpecialCharactersForm ClickCharacterButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Специальные символы'");
			CharacterButton.Click();

			return new SpecialCharactersForm(Driver).GetPage();
		}

		/// <summary>
		/// Открыть форму 'Специальные символы' с помощью сочетания клавиш Ctrl+Shift+I
		/// </summary>
		public SpecialCharactersForm OpenSpecialCharacterFormByHotKey()
		{
			CustomTestContext.WriteLine("Открыть форму 'Специальные символы' с помощью сочетания клавиш Ctrl+Shift+I");
			Driver.SendHotKeys("I", true, true);

			return new SpecialCharactersForm(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку открытия словаря
		/// </summary>
		public SpellcheckDictionaryDialog ClickSpellcheckDictionaryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку открытия словаря");
			SpellcheckDictionaryButton.Click();

			return new SpellcheckDictionaryDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Go to Last Unconfirmed Segment'
		/// </summary>
		public EditorPage ClickLastUnconfirmedButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Go to Last Unconfirmed Segment'.");
			LastUnconfirmedButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Вставить тег'
		/// </summary>
		public EditorPage ClickInsertTag()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Вставить тег'.");
			InsertTagButton.Click();

			return GetPage();
		}
		
		/// <summary>
		/// Выделить последний неподтвержденный сегмент нажатием F9
		/// </summary>
		public EditorPage SelectLastUnconfirmedSegmentByHotKey()
		{
			CustomTestContext.WriteLine("Выделить последний неподтвержденный сегмент нажатием F9");
			Driver.SendHotKeys(OpenQA.Selenium.Keys.F9);

			return GetPage();
		}

		/// <summary>
		/// Получить название этапа
		/// </summary>
		public string GetStage()
		{
			CustomTestContext.WriteLine("Получить название этапа");

			return StageName.Text;
		}

		/// <summary>
		/// Вернуть процент совпадения в таргет
		/// </summary>
		public int TargetMatchPercent(int segmentNumber)
		{
			CustomTestContext.WriteLine("Вернуть процент совпадения в таргет №{0}.", segmentNumber);
			var percentMatchColumn = Driver.SetDynamicValue(How.XPath, TARGET_MATCH_COLUMN_PERCENT, (segmentNumber - 1).ToString()).Text.Replace("%", "");
			int result;

			if (!int.TryParse(percentMatchColumn, out result))
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось преобразование процента совпадения {0} в число.", percentMatchColumn));
			}

			return result;
		}

		/// <summary>
		/// Получить номер строки в CAT-панели
		/// </summary>
		/// <param name="catType">тип перевода</param>
		/// <returns>номер строки в CAT-панели</returns>
		public int CatTypeRowNumber(CatType catType)
		{
			CustomTestContext.WriteLine("Получить номер строки для {0} в CAT-панели.", catType);
			var rowNumber = 0;
			var catTypeList = Driver.GetTextListElement(By.XPath(CAT_TYPE_LIST_IN_PANEL));

			for (var i = 0; i < catTypeList.Count; ++i)
			{
				if (catTypeList[i].Contains(catType.ToString()))
				{
					rowNumber = i + 1;
					break;
				}
			}

			if (rowNumber == 0)
			{
				throw new Exception(string.Format("Произошла ошибка:\n подстановка {0} отсутствует в CAT-панели.", catType));
			}

			return rowNumber;
		}

		/// <summary>
		/// Двойной клик по переводу в CAT-панели
		/// </summary>
		/// <param name="rowNumber">номер строки в CAT-панели</param>
		public EditorPage DoubleClickCatPanel(int rowNumber)
		{
			CustomTestContext.WriteLine("Двойной клик по строке №{0} с переводом в CAT-панели.", rowNumber);
			var cat = Driver.SetDynamicValue(How.XPath, CAT_TRANSLATION, rowNumber.ToString());

			cat.Scroll();
			// Sleep не убирать, без него не скролится
			Thread.Sleep(1000);
			cat.HoverElement();
			cat.DoubleClick();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Add Term'
		/// </summary>
		public AddTermDialog ClickAddTermButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Add Term'");
			AddTermButton.Click();

			return new AddTermDialog(Driver).GetPage();
		}

		/// <summary>
		/// Получить текст из таргет сегмента
		/// </summary>
		/// <param name="segmentNumber">номер строки сегмента</param>
		public string GetTargetText(int rowNumber)
		{
			CustomTestContext.WriteLine("Получить текст из таргет сегмента №{0}.", rowNumber);
			var target = Driver.SetDynamicValue(How.XPath, TARGET_CELL, (rowNumber - 1).ToString());

			return target.Text;
		}

		/// <summary>
		/// Получить текст из source сегмента
		/// </summary>
		/// <param name="rowNumber">номер строки сегмента</param>
		public string GetSourceText(int rowNumber)
		{
			CustomTestContext.WriteLine("Получить текст из source сегмента №{0}.", rowNumber);
			var source = Driver.SetDynamicValue(How.XPath, SOURCE_CELL, (rowNumber - 1).ToString());

			return source.Text;
		}

		/// <summary>
		/// Получить текст из CAT-панели
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public string GetCatTranslationText(int rowNumber)
		{
			CustomTestContext.WriteLine("Получить текст из CAT-панели, строка №{0}.", rowNumber);
			var catRowText = Driver.SetDynamicValue(How.XPath, CAT_TRANSLATION, rowNumber.ToString());

			return catRowText.Text;
		}

		/// <summary>
		/// Получить текст из колонки MatchColumn
		/// </summary>
		/// <param name="segmentNumber">номер строки</param>
		public string GetMatchColumnText(int segmentNumber)
		{
			CustomTestContext.WriteLine("Получить текст из колонки MatchColumn, строка №{0}.", segmentNumber);
			var matchColumn = Driver.SetDynamicValue(How.XPath, MATCH_COLUMN, segmentNumber.ToString());

			return matchColumn.Text;
		}

		/// <summary>
		/// Закрыть туториал, если он виден
		/// </summary>
		public EditorPage CloseTutorialIfExist()
		{
			CustomTestContext.WriteLine("Проверить, видна ли подсказка");

			if (Driver.WaitUntilElementIsDisplay(By.XPath(FINISH_TUTORIAL_BUTTON), timeout: 5))
			{
				CustomTestContext.WriteLine("Закрыть подсказку.");
				FinishTutorialButton.Click();
			}

			return GetPage();
		}
		
		/// <summary>
		/// Нажать кнопку 'Копировать оригинал в перевод'
		/// </summary>
		public EditorPage ClickCopySourceToTargetButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Копировать оригинал в перевод'.");
			CopyButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Копировать текст из сорса в таргет с помощью сочетания клавиш Ctrl+Insert
		/// </summary>
		public EditorPage CopySourceToTargetHotkey()
		{
			CustomTestContext.WriteLine("Копировать текст из сорса в таргет с помощью сочетания клавиш Ctrl+Insert");
			Driver.SendHotKeys(Keys.Insert, control: true);

			return GetPage();
		}

		/// <summary>
		/// Вернуть процент совпадений в CAT-панели
		/// </summary>
		public int CatTranslationMatchPercent(int rowNumber)
		{
			CustomTestContext.WriteLine("Вернуть процент совпадений в CAT-панели. Номер строки CAT: {0}.", rowNumber);
			var catPanelPercentMatch = Driver.SetDynamicValue(How.XPath, CAT_PANEL_PERCENT_MATCH, rowNumber.ToString()).Text.Replace("%", "");
			int result;

			if (!int.TryParse(catPanelPercentMatch, out result))
			{
				throw new Exception(string.Format(
					"Произошла ошибка:\n не удалось преобразование процента совпадения {0} в CAT-панели в число.",
					catPanelPercentMatch));
			}
		
			return result;
		}

		/// <summary>
		/// Вернуть цвет процента совпадения в колонке Match
		/// </summary>
		public string TargetMatchColor(int segmentNumber)
		{
			CustomTestContext.WriteLine("Вернуть цвет процента совпадения в колонке Match в сегменте №{0}.", segmentNumber);
			var percentColor = Driver.SetDynamicValue(How.XPath, PERCENT_COLOR, (segmentNumber - 1).ToString());

			return percentColor.GetAttribute("class");
		}

		/// <summary>
		/// Нажать кнопку 'Конкордансный поиск'
		/// </summary>
		public EditorPage ClickConcordanceSearchButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Конкордансный поиск'");
			ConcordanceButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Открыть 'Конкордансный поиск' с помощью сочетания клавиш Ctrl+k
		/// </summary>
		public EditorPage OpenConcordanceSearchByHotKey()
		{
			CustomTestContext.WriteLine("Открыть 'Конкордансный поиск' с помощью сочетания клавиш Ctrl+k");
			Driver.SendHotKeys("k", control: true);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Rollback
		/// </summary>
		public EditorPage ClickRollbackButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Rollback.");
			RollbackButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Открть диалог добавления термина, нажав сочетание клавиш Ctrl+E
		/// </summary>
		public AddTermDialog OpenAddTermDialogByHotKey()
		{
			CustomTestContext.WriteLine("Открть диалог добавления термина, нажав сочетание клавиш Ctrl+E");
			Driver.SendHotKeys("e", true);

			return new AddTermDialog(Driver).GetPage();
		}

		/// <summary>
		/// Выделить первое слово в сегменте
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <param name="segmentType">тип сегмента</param>
		public EditorPage SelectFirstWordInSegment(int rowNumber, SegmentType segmentType = SegmentType.Source)
		{
			CustomTestContext.WriteLine("Произвести двойной клик по первому слову сегмента {1} в строке : {0}", rowNumber, segmentType);

			IWebElement segment;

			switch (segmentType)
			{
				case SegmentType.Source:
					segment = Driver.SetDynamicValue(How.XPath, SOURCE_CELL, (rowNumber - 1).ToString());
					break;

				case SegmentType.Target:
					segment = Driver.SetDynamicValue(How.XPath, TARGET_CELL_VALUE, (rowNumber - 1).ToString());
					break;

				default:
					throw new ArgumentException("Указан некорректный тип сегмента");
			}

			segment.DoubleClickElementAtPoint(0, 0);

			return GetPage();
		}

		/// <summary>
		/// Получить выделенное слово
		/// </summary>
		public string GetSelectedWordInSegment()
		{
			CustomTestContext.WriteLine("Получить выделенное слово в сегменте");

			try
			{
				return Driver.ExecuteScript("return window.getSelection().toString();").ToString().Trim();
			}
			catch
			{
				throw new Exception("Произошла ошибка:\n не удалось получить выделенное слово");
			}
		}

		/// <summary>
		/// Нажать Yes в окне подтверждения.
		/// </summary>
		public EditorPage Confirm()
		{
			CustomTestContext.WriteLine("Нажать Yes в окне подтверждения.");
			ConfirmYesButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить список терминов из кат панели
		/// </summary>
		public List<string> GetCatTerms()
		{
			CustomTestContext.WriteLine("Получить список терминов из кат панели");
			var terms = Driver.GetTextListElement(By.XPath(CAT_PANEL_TERM));

			return terms.Select(g => g.Trim()).ToList();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Ввести текст в таргет сегмента
		/// </summary>
		/// <param name="text">текст</param>
		/// <param name="rowNumber">номер сегмента</param>
		/// <param name="clearField">очистить поле перед вводу (default)</param>
		public EditorPage FillSegmentTargetField(string text = "Translation", int rowNumber = 1, bool clearField = true)
		{
			CustomTestContext.WriteLine("Ввести текст в таргет сегмента {0}.", rowNumber);
			TargetCell = Driver.SetDynamicValue(How.XPath, TARGET_CELL, (rowNumber - 1).ToString());
			TargetCell.Click();

			if (clearField)
			{
				TargetCell.SetText(text);
			}
			else
			{
				TargetCell.SendKeys(text);
			}

			return GetPage();
		}

		/// <summary>
		/// Заполнить таргет
		/// </summary>
		/// <param name="text">текст</param>
		/// <param name="rowNumber">номер строки</param>
		public EditorPage FillTarget(string text, int rowNumber = 1)
		{
			ClickOnTargetCellInSegment(rowNumber);
			FillSegmentTargetField(text, rowNumber);

			return GetPage();
		}

		/// <summary>
		/// Откат сегмента
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public EditorPage RollBack(int segmentNumber = 1)
		{
			ClickOnTargetCellInSegment(segmentNumber);
			ClickRollbackButton();

			return GetPage();
		}

		/// <summary>
		/// Вставить перевод из CAT-панели
		/// </summary>
		/// <param name="catType">CAT-тип</param>
		/// <param name="targetRowNumber">номер строки таргета</param>
		public EditorPage PasteTranslationFromCAT(CatType catType, int targetRowNumber = 1)
		{
			ClickOnTargetCellInSegment(targetRowNumber);

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CAT_TYPE.Replace("*#*", catType.ToString()))))
			{
				throw new XPathLookupException(
					string.Format("Произошла ошибка:\n Не появился тип {0} в CAT-панели", catType));
			}

			var catRowNumber = CatTypeRowNumber(catType);
			DoubleClickCatPanel(catRowNumber);

			if (GetTargetText(targetRowNumber) != GetCatTranslationText(catRowNumber))
			{
				throw new Exception("Текст из таргет сегмента совпадает с текстом перевода из CAT-панели");
			}

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что появился диалог подтверждения сохранения уже существующего термина
		/// </summary>
		public bool IsConfirmExistedTermMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появился диалог подтверждения сохранения уже существующего термина");

			return Driver.WaitUntilElementIsDisplay(By.XPath(EXISTING_TERM_MESSAGE));
		}

		/// <summary>
		/// Проверить, статус 'Saving...' исчез
		/// </summary>
		public bool IsSavingStatusDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, статус 'Saving...' исчез.");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(SAVING_STATUS));
		}

		/// <summary>
		/// Проверить, что Конкордансный поиск появился
		/// </summary>
		public bool IsConcordanceSearchDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что Конкордансный поиск появился.");

			return ConcordanceSearch.Displayed;
		}

		/// <summary>
		/// Проверить, что термины из CAT-панели соответствуют терминам в сорсе
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public bool IsCatTermsMatchSourceTerms(int segmentNumber)
		{
			CustomTestContext.WriteLine("Проверить, что термины из CAT-панели соответствуют терминам в сорсе");

			var catTerms = GetCatTerms()[0];
			var sourceTerms = GetSourceText(segmentNumber);

			return catTerms == sourceTerms;
		}

		/// <summary>
		/// Проверить, что сегмент не залочен
		/// </summary>
		public bool IsSegmentLocked(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что сегмент №{0} не залочен.", segmentNumber);

            return Driver.WaitUntilElementIsDisplay(By.XPath(SEGMENT_LOCK.Replace("*#*", segmentNumber.ToString())));
		}

		/// <summary>
		/// Проверить, присутствует ли таблица в CAT-панели
		/// </summary>
		public bool IsCatTableExist()
		{
			CustomTestContext.WriteLine("Проверить, присутствует ли таблица в CAT-панели");

			return Driver.GetIsElementExist(By.XPath(CAT_TABLE));
		}

		/// <summary>
		/// Проверить, открылся ли редактор
		/// </summary>
		public bool IsEditorPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SEGMENTS_BODY), timeout: 60);
		}

		/// <summary>
		/// Проверить, совпадает ли текст в колонке MatchColumn с ожидаемым
		/// </summary>
		/// <param name="catType">CAT-тип</param>
		/// <param name="rowNumber">номер строки</param>
		public bool IsMatchColumnCatTypeMatch(CatType catType, int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что текст в колонке MatchColumn совпадает с {0}.", catType);
			var catTypeColumn = catType != CatType.TB ? catType.ToString() : string.Empty;

			var textInMacthColumn = GetMatchColumnText(rowNumber).Trim();

			if (textInMacthColumn.Contains("%"))
			{
				textInMacthColumn = textInMacthColumn.Substring(0, 2);
			}

			return textInMacthColumn.Trim() == catTypeColumn;
		}

		/// <summary>
		/// Проверить, подтвердился ли сегмент
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public bool IsSegmentConfirmed(int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, подтвердился ли сегмент {0}.", rowNumber);

            return Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRMED_ICO.Replace("*#*", (rowNumber - 1).ToString())));
		}

		/// <summary>
		/// Проверить, что тип последней ревизии соответствует ожидаемому
		/// </summary>
		public bool IsLastRevisionEqualToExpected(RevisionType expectedRevisionType)
		{
			CustomTestContext.WriteLine("Проверить, что тип последней ревизии соответствует ожидаемому типу {0}", expectedRevisionType);

            return Driver.WaitUntilElementIsDisplay(By.XPath(REVISION_PATH.Replace("*#*", expectedRevisionType.Description())));
		}

		/// <summary>
		/// Проверить, что цветовая схема таргета соотносится с кол-вом процентов
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public bool IsTargetMatchPercentCollorCorrect(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что цветовая схема таргета соотносится с кол-вом процентов");

			const int yellowUpperBound = 99;
			const int yellowLowerBound = 76;

			const string green = "green";
			const string yellow = "yellow";
			const string red = "red";

			var targetMatchPercent = TargetMatchPercent(segmentNumber);

			if (targetMatchPercent > yellowUpperBound)
			{
				return green == TargetMatchColor(segmentNumber);
			}

			if (targetMatchPercent <= yellowUpperBound && targetMatchPercent >= yellowLowerBound)
			{
				return yellow == TargetMatchColor(segmentNumber);
			}

			if (targetMatchPercent < yellowLowerBound)
			{
				return red == TargetMatchColor(segmentNumber);
			}

			return false;
		}

		/// <summary>
		/// Проверить, что процент совпадения в CAT-панели и в таргете совпадает
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <param name="catRowNumber">номер строки в CAT-панели</param>
		public bool IsCATPercentMatchTargetPercent(int segmentNumber = 1, int catRowNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что процент совпадения в CAT-панели и в таргете совпадает. Строка в CAT-панели {0}, строка в таргет {1}.", catRowNumber, segmentNumber);

			return CatTranslationMatchPercent(catRowNumber) == TargetMatchPercent(segmentNumber);
		}

		/// <summary>
		/// Проверить, что слово подчеркнуто в сегменте
		/// </summary>
		public bool IsUnderlineForWordExist(string word)
		{
			CustomTestContext.WriteLine("Проверить, что слово {0} подчеркнуто в сегменте", word);

            return Driver.WaitUntilElementIsDisplay(By.XPath(SPELLCHECK_PATH.Replace("*#*", word)));
		}

		/// <summary>
		/// Проверить, что таргет сегмента виден
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public bool IsTargetDisplayed(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что таргет сегмента №{0} виден", segmentNumber);

            return Driver.WaitUntilElementIsDisplay(By.XPath(TARGET_CELL.Replace("*#*", (segmentNumber - 1).ToString())));
		}

		/// <summary>
		/// Проверить, что название этапа пустое.
		/// </summary>
		public bool IsStageNameIsEmpty()
		{
			CustomTestContext.WriteLine("Проверить, что название этапа пустое");

			return Driver.ElementIsDisplayed(By.XPath(STAGE_NAME));
		}

		/// <summary>
		/// Проверить, что сегмент активен (подсвечен голубым цветом)
		/// </summary>
		public bool IsSegmentSelected(int segmentNumber)
		{
			CustomTestContext.WriteLine("Проверить, что сегмент №{0} активен (подсвечен голубым цветом).", segmentNumber);

            return Driver.WaitUntilElementIsDisplay(By.XPath(SELECTED_SEGMENT.Replace("*#*", segmentNumber.ToString())));
		}

		/// <summary>
		/// Проверить, что появился тег в таргете
		/// </summary>
		public bool IsTagDisplayed(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что появился тег в таргете.");
			var targetTag = Driver.SetDynamicValue(How.XPath, TAG, segmentNumber.ToString());

			return targetTag.Displayed;
		}

		/// <summary>
		/// Проверить, что новый термин сохранен
		/// </summary>
		public bool IsTermSaved()
		{
			CustomTestContext.WriteLine("Проверить, что новый термин сохранен");

			return Driver.WaitUntilElementIsDisplay(By.XPath(TERM_SAVED_MESSAGE));
		}

		/// <summary>
		/// Проверить, что сообщение о том, что термин сохранен, исчезло
		/// </summary>
		public bool IsTermSavedMessageDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение о том, что термин сохранен, исчезло");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(TERM_SAVED_MESSAGE), timeout: 30);
		}

		#endregion

		#region Объявление элементов страницы

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
		
		[FindsBy(How = How.XPath, Using = STAGE_NAME)]
		protected IWebElement StageName { get; set; }
		
		[FindsBy(How = How.Id, Using = LAST_CONFIRMED_BUTTON)]
		protected IWebElement LastUnconfirmedButton { get; set; }
		
		[FindsBy(How = How.Id, Using = CHARACTER_BUTTON)]
		protected IWebElement CharacterButton { get; set; }
		
		[FindsBy(How = How.Id, Using = ADD_TERM_BUTTON)]
		protected IWebElement AddTermButton { get; set; }

		[FindsBy(How = How.Id, Using = TARGET_CELL)]
		protected IWebElement TargetCell { get; set; }
		
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

		[FindsBy(How = How.Id, Using = CAT_PANEL_PERCENT_MATCH)]
		protected IWebElement CatPanelPercentMatch { get; set; }
		
		[FindsBy(How = How.Id, Using = PERCENT_COLOR)]
		protected IWebElement PercentColor { get; set; }

		[FindsBy(How = How.Id, Using = ALL_SEGMENTS_SAVED_STATUS)]
		protected IWebElement AllSegmentsSavedStatus { get; set; }

		[FindsBy(How = How.XPath, Using = СONFIRM_YES_BTN)]
		protected IWebElement ConfirmYesButton { get; set; }

		[FindsBy(How = How.XPath, Using = CAT_TABLE)]
		protected IWebElement CatTable { get; set; }

		#endregion

		#region Описание XPath элементов страницы

		protected const string CONFIRM_BTN = "//a[@id='confirm-btn']";
		protected const string FIND_ERROR_BTN_ID = "qa-error-btn";
		protected const string FINISH_TUTORIAL_BUTTON = "//span[contains(text(),'Finish') and contains(@id, 'button')]";
		protected const string AUTOSAVING = "//div[contains(text(), 'Saving')]";
        protected const string SPELLCHECK_PATH = "//div[contains(text(), '1')]/ancestor::tr//span[contains(@class,'spellcheck') and contains(string(), '*#*')]";
		protected const string REVISION_PATH = "//div[@id='revisions-body']//table[1]//td[contains(@class,'revision-type-cell')]//div[text()='*#*']";
		protected const string STAGE_NAME = "//h1/span[contains(@class, 'workflow')]";
		protected const string LAST_CONFIRMED_BUTTON = "unfinished-btn";
		protected const string ADD_TERM_BUTTON = "add-term-btn";
		protected const string SELECTED_SEGMENT = "//table[*#*]//tr[@aria-selected='true']";
		protected const string CHARACTER_BUTTON = "charmap-btn";
		protected const string INSERT_TAG_BUTTON = "tag-insert-btn";
		protected const string COPY_BUTTON = "copy-btn-btnEl";
		protected const string CONCORDANCE_BUTTON = "concordance-search-btn";
		protected const string ROLLBACK_BUTTON = "step-rollback-btn";
		protected const string HOME_BUTTON = "back-btn";
		protected const string DICTIONARY_BUTTON = "dictionary-btn";

		protected const string ROW_NUMBER_ACTIVE_XPATH = ".//div[@id='segments-body']//table//td[contains(@class, 'x-grid-item-focused')]/../td[1]//div[contains(@class, 'row-numberer')]";
	
		protected const string SEGMENTS_TABLE_XPATH = "//div[@id='segments-body']//div//div[2]//table";
		protected const string SEGMENTS_BODY = "//div[@id='segments-body']//table";
		protected const string CONFIRMED_ICO = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[contains(@class,'info-cell')]//div[contains(@class,'fa-check')]";
		protected const string TARGET_CELL = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[4]//div//div";
		protected const string TARGET_CELL_VALUE = "//table[@data-recordindex='*#*']//td[4]//div[contains(@id, 'segmenteditor')]";
		protected const string SOURCE_CELL = "//table[@data-recordindex='*#*']//td[3]//div[contains(@id, 'segmenteditor')]";
		protected const string TAG = "//div[contains(text(), '1')]//..//..//..//..//tr[1]//td[4]//div//img[contains(@class,'tag')]";
		protected const string SEGMENT_LOCK = "//div[contains(text(), '1')]//..//..//..//div[contains(@class,'fa-lock')][not(contains(@class,'inactive'))]";

		protected const string CHARACTER_FORM = "charmap";
		protected const string CONCORDANCE_SEARCH= "concordance-search";

		protected const string ALL_SEGMENTS_SAVED_STATUS = "//div[text()='All segments are saved.']";
		protected const string SAVING_STATUS = "//div[@id='segmentsavingindicator-1048-innerCt' and contains(text(),'Saving')]";
		protected const string MATCH_COLUMN = "//div[@id='segments-body']//table[*#*]//tbody//td[contains(@class,'matchcolum')]";
		protected const string TARGET_MATCH_COLUMN_PERCENT = "//table[@data-recordindex='*#*' and contains(@id, 'tableview')]//td[6]//div//span";
		protected const string CAT_PANEL_PERCENT_MATCH = ".//div[@id='cat-body']//table[*#*]//tbody//tr//td[3]//div//span";
		protected const string CAT_PANEL_TERM = ".//div[@id='cat-body']//table//tbody//tr//td[2]//div";
		protected const string CAT_TYPE_LIST_IN_PANEL = ".//div[@id='cat-body']//table//td[3]/div";
		protected const string CAT_TRANSLATION = ".//div[@id='cat-body']//table[*#*]//td[4]/div";
		protected const string CAT_TYPE = ".//div[@id='cat-body']//table//td[3]/div[text()='*#*']";

		protected const string PERCENT_COLOR = "//table[@data-recordindex='*#*' and contains(@id, 'tableview')]//td[6]//div//span";

		protected const string TERM_SAVED_MESSAGE = ".//div[text()='The term has been saved.']";

		protected const string EXISTING_TERM_MESSAGE = "//div[contains(@id, 'messagebox') and contains(string(), 'This glossary already contains term(s)')]";
		protected const string СONFIRM_YES_BTN = "//div[contains(@id, 'messagebox')]//span[contains(string(), 'Yes')]";
		protected const string CAT_TABLE = ".//div[@id='cat-body']//table";

		#endregion
	}
}
