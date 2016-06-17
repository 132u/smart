using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Keys = OpenQA.Selenium.Keys;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class EditorPage : IAbstractPage<EditorPage>
	{
		public WebDriver Driver { get; set; }

		protected bool _needCloseTutorial { get; set; }

		public EditorPage(WebDriver driver, bool needCloseTutorial = true)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
			_needCloseTutorial = needCloseTutorial;
		}

		public EditorPage LoadPage()
		{
			if (!IsEditorPageOpened(_needCloseTutorial))
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось открыть документ в редакторе.");
			}

			return this;
		}

		public EditorPage LoadPageFromAnotherPage()
		{
			Driver.WaitUntilElementIsDisplay(By.XPath(FINISH_TUTORIAL_BUTTON), 3);

			if (IsCloseTutorialButtonDisplay() && _needCloseTutorial)
			{
				CloseTutorial();
			}

			return LoadPage();
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать ссылку 'Open in new tab' на вкладке 'Dictionaries'
		/// </summary>
		public SearchPage ClickOpenTranslationInNewTabLink()
		{
			CustomTestContext.WriteLine("Нажать ссылку 'Open in new tab' на вкладке 'Dictionaries'");
			OpenTranslationInNewTabLink.Click();
			Driver.SwitchToNewBrowserTab();

			return new SearchPage(Driver).LoadPage();
		}

		/// <summary>
		/// Получить текст из поля поиска на вкладке 'Dictionaries'
		/// </summary>
		public string GetTextFromSearchFieldInDictionariesTab()
		{
			CustomTestContext.WriteLine("Получить текст из поля поиска на вкладке 'Dictionaries'");

			return DictionariesSearchInput.GetAttribute("value");
		}

		/// <summary>
		/// Получить направление перевода на вкладке 'Dictionaries'
		/// </summary>
		public string GetTranslationDirectionInDictionariesTab()
		{
			CustomTestContext.WriteLine("Получить направление перевода на вкладке 'Dictionaries'");

			return DictionariesTranslationDirection.Text;
		}

		/// <summary>
		/// Нажать кнопку поиска в словорях Лингво
		/// </summary>
		public EditorPage ClickSearchInLingvoDictionariesButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку поиска в словорях Лингво");
			SearchInLingvoDictionariesButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку поиска в словорях Лингво c помощью хоткея Ctrl+I
		/// </summary>
		public EditorPage ClickSearchInLingvoDictionariesButtonByHotkey()
		{
			CustomTestContext.WriteLine("Нажать кнопку поиска в словорях Лингво c помощью хоткея Ctrl+I");
			Driver.SendHotKeys("i", control: true);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку смены направления перевода на вкладке 'Dictionaries' в боковой панели
		/// </summary>
		public EditorPage ClickTranslationDirectionSwitchOnDictionariesTab()
		{
			CustomTestContext.WriteLine("Нажать кнопку смены направления перевода на вкладке 'Dictionaries' в боковой панели");
			DictionariesTranslationDirectionSwitch.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на вкладку 'Dictionaries' в боковой панели
		/// </summary>
		public EditorPage ClickDictionariesTab()
		{
			CustomTestContext.WriteLine("Нажать на вкладку 'Dictionaries' в боковой панели");
			DictionariesTab.Click();

			return LoadPage();
		}

		/// <summary>
		/// Ввести поисковый запрос на вкладке 'Dictionaries' в боковой панели
		/// </summary>
		/// <param name="searchQuery">поисковый запрос</param>
		public EditorPage FillSearchQueryInDictionariesTab(string searchQuery)
		{
			CustomTestContext.WriteLine("Ввести поисковый запрос на вкладке 'Dictionaries' в боковой панели");
			DictionariesSearchInput.SetText(searchQuery);

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку поиска на вкладке 'Dictionaries' в боковой панели
		/// </summary>
		public EditorPage ClickSearchButtonInDictionariesTab()
		{
			CustomTestContext.WriteLine("Нажать на кнопку поиска на вкладке 'Dictionaries' в боковой панели");
			DictionariesSearchButton.Click();
			Thread.Sleep(3000); // Для срабатывания автоподстановки

			return LoadPage();
		}

		/// <summary>
		/// Нажать стрелку перехода к следующей терминологической ошибке
		/// </summary>
		/// <param name="term">термин</param>
		public EditorPage ClickNextTerminologyErrorArrow(string term)
		{
			CustomTestContext.WriteLine("Нажать стрелку перехода к следующей терминологической ошибке , термин {0}.", term);
			NextTerminologyErrorArrow = Driver.SetDynamicValue(How.XPath, NEXT_TERMINOLOGY_ERROR_ARROW, term);
			NextTerminologyErrorArrow.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать стрелку перехода к предыдущей терминологической ошибке
		/// </summary>
		/// <param name="term">термин</param>
		public EditorPage ClickPreviousTerminologyErrorArrow(string term)
		{
			CustomTestContext.WriteLine("Нажать стрелку перехода к предыдущей терминологической ошибке , термин {0}.", term);
			PreviousTerminologyErrorArrow = Driver.SetDynamicValue(How.XPath, PREVIOUS_TERMINOLOGY_ERROR_ARROW, term);
			PreviousTerminologyErrorArrow.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на поле комментария
		/// </summary>
		public EditorPage ClickDocumentCommentTextarea()
		{
			CustomTestContext.WriteLine("Нажать на поле комментария.");
			DocumentCommentsTextarea.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Done', подтвердить готовность проекта.
		/// </summary>
		public EditorPage ClickDoneButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Done', подтвердить готовность проекта.");
			DoneButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Ввести комментарий документа
		/// </summary>
		/// <param name="comment">комментарий</param>
		public EditorPage FillDocumentComment(string comment)
		{
			CustomTestContext.WriteLine("Ввести комментарий документа '{0}'.", comment);
			DocumentCommentsTextarea.SetText(comment);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку отправки комментария документа
		/// </summary>
		public EditorPage ClickSendDocumentCommentButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку отправки комментария документа.");
			SendButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Ввести комментарий сегмента
		/// </summary>
		/// <param name="comment">комментарий</param>
		public EditorPage FillSegmentComment(string comment)
		{
			CustomTestContext.WriteLine("Ввести комментарий сегмента '{0}'.", comment);
			SegmentCommentsTextarea.SetText(comment);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку отправки комментария сегмента
		/// </summary>
		public EditorPage ClickSegmentSendCommentButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку отправки комментария сегмента.");
			SegmentSendButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на стрелку повтора
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public EditorPage ClickRepetitionArrow(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать на стрелку повтора в сегменте №{0}.", segmentNumber);
			RepetitionArrow = Driver.SetDynamicValue(How.XPath, REPETITION_ARROW, (segmentNumber - 1).ToString());
			RepetitionArrow.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на вкладку комментариев сегмента
		/// </summary>
		public EditorPage ClickSegmentCommentTab()
		{
			CustomTestContext.WriteLine("Нажать на вкладку комментариев сегмента.");
			SegmentCommentsTab.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на вкладку комментария
		/// </summary>
		public EditorPage ClickDocumentCommentTab()
		{
			CustomTestContext.WriteLine("Нажать на вкладку комментариев документа.");
			DocumentCommentsTab.Click();

			return LoadPage();
		}

		/// <summary>
		/// Навести курсор на знак ошибки (желтый треугольник)
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public EditorPage HoverSegmentErrorLogo(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Навести курсор на знак ошибки (желтый треугольник) сегмента №{0}.", segmentNumber);
			SegmentErrorLogo = Driver.SetDynamicValue(How.XPath, SEGMENT_ERROR_LOGO, (segmentNumber - 1).ToString());
			SegmentErrorLogo.HoverElement();

			if (!IsErrorsPopupExist())
			{
				throw new Exception("Произошла ошибка: не появился попап с ошибками.");
			}

			return LoadPage();
		}

		/// <summary>
		/// Нажать на желтый треугольник
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public EditorPage ClickYellowTriangle(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать на желтый треугольник сегмента №{0}.", segmentNumber);
			SegmentErrorLogo = Driver.SetDynamicValue(How.XPath, SEGMENT_ERROR_LOGO, (segmentNumber - 1).ToString());
			SegmentErrorLogo.HoverElement();
			SegmentErrorLogo = Driver.SetDynamicValue(How.XPath, SEGMENT_ERROR_LOGO, (segmentNumber - 1).ToString());
			SegmentErrorLogo.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку "Домой" для перехода на страницу проекта
		/// </summary>
		public ProjectSettingsPage ClickHomeButtonExpectingProjectSettingsPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Домой' для перехода на страницу проекта.");
			HomeButton.Click();

			return new ProjectSettingsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку удаления комментария.
		/// </summary>
		public EditorPage ClickDeleteCommentButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления комментария.");
			DeleteCommentButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Навести курсор на комментарий
		/// </summary>
		/// <param name="author">автор</param>
		/// <param name="comment">комментарий</param>
		public EditorPage HoverComment(string author, string comment)
		{
			CustomTestContext.WriteLine("Навести курсор на комментарий.");
			Comment = Driver.SetDynamicValue(How.XPath, COMMENT, author, comment);
			Comment.HoverElement();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку "Домой" для перехода на страницу проектов
		/// </summary>
		public ProjectsPage ClickHomeButtonExpectingProjectsPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Домой' для перехода на страницу проектов.");
			HomeButton.Click();

			return new ProjectsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Подтвердить текст с помощью грячих клавиш Ctrl+Enter
		/// </summary>
		public EditorPage ConfirmSegmentByHotkeys()
		{
			CustomTestContext.WriteLine("Подтвердить сегмент с помощью горячих клавиш Ctrl+Enter");
			Driver.SendHotKeys(Keys.Enter, control: true);

			return LoadPage();
		}

		/// <summary>
		/// Нажать хоткей переноса строки Ctrl+Q
		/// </summary>
		public EditorPage ClickAddLineBreakHotkey()
		{
			CustomTestContext.WriteLine("Нажать хоткей переноса строки Ctrl+Q.");
			Driver.SendHotKeys("q", control: true);

			return LoadPage();
		}

		/// <summary>
		/// Подтвердить текст с помощью клавиши Enter
		/// </summary>
		public EditorPage ConfirmSegmentByEnterHotkeys()
		{
			CustomTestContext.WriteLine("Подтвердить сегмент с помощью клавиши Enter");
			Driver.SendHotKeys(Keys.Enter);

			return LoadPage();
		}

		/// <summary>
		/// "Нажать на галочку подтверждения в сегменте
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public EditorPage ClickSegmentConfirmTick(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать на галочку подтверждения в сегменте №{0}", segmentNumber);
			ConfirmTick = Driver.SetDynamicValue(How.XPath, CONFIRM_TICK, segmentNumber.ToString());
			ConfirmTick.Click();

			return LoadPage();
		}

		/// <summary>
		/// Вставить тег нажатием клавиши F8
		/// </summary>
		public EditorPage InsertTagByHotKey()
		{
			CustomTestContext.WriteLine("Вставить тег нажатием клавиши F8");
			Driver.SendHotKeys(Keys.F8);

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку "Подтвердить сегмент"
		/// </summary>
		public EditorPage ConfirmSegmentTranslation()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Подтвердить сегмент'.");
			ConfirmButton.AdvancedClick();
			Driver.WaitUntilElementIsDisplay(By.XPath(ALL_SEGMENTS_SAVED_STATUS), timeout: 30);

			return LoadPage();
		}

		/// <summary>
		/// Закрыть сообщение с критической ошибкой
		/// </summary>
		public EditorPage CloseCriticalErrorMessageIfExist()
		{
			CustomTestContext.WriteLine("Закрыть сообщение с критической ошибкой");
			if (IsMessageWithCrititcalErrorDisplayed())
			{
				CloseCriticalErrorMessageButton.Click();
			}

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку поиска ошибки в терминологии
		/// </summary>
		public ErrorsDialog ClickFindErrorButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку поиска ошибки в терминологии");
			FindErrorButton.Click();

			return new ErrorsDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Вызвать окно поиска ошибок в терминологии с помощью хоткея F7
		/// </summary>
		public ErrorsDialog OpenFindErrorsDialogByHotkey()
		{
			CustomTestContext.WriteLine("Вызвать окно поиска ошибок в терминологии с помощью хоткея F7");
			Driver.SendHotKeys(OpenQA.Selenium.Keys.F7);

			return new ErrorsDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Кликнуть по таргету сегмента
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage ClickOnTargetCellInSegment(int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Кликнуть по таргету сегмента {0}.", rowNumber);
			TargetCell = Driver.SetDynamicValue(How.XPath, TARGET_CELL, (rowNumber - 1).ToString());
			TargetCell.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по результату конкордансного поиска
		/// </summary>
		public EditorPage DoubleClickConcordanceSearchResult()
		{
			CustomTestContext.WriteLine("Кликнуть по результату конкордансного поиска.");
			ConcordanceSearchResult.DoubleClick();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по сорсу сегмента
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage ClickOnSourceCellInSegment(int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Кликнуть по сорсу сегмента {0}.", rowNumber);
			SourceCell = Driver.SetDynamicValue(How.XPath, SOURCE_CELL, (rowNumber - 1).ToString());
			SourceCell.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Специальные символы'
		/// </summary>
		public SpecialCharactersForm ClickCharacterButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Специальные символы'");
			CharacterButton.Click();

			return new SpecialCharactersForm(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку отмены.
		/// </summary>
		public EditorPage ClickUndoButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку отмены.");
			Driver.WaitUntilElementIsDisplay(By.XPath(UNDO_BUTTON));
			UndoButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку возврата.
		/// </summary>
		public EditorPage ClickRedoButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку возврата.");
			RedoButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать хоткей кнопки отмены Ctrl Z.
		/// </summary>
		public EditorPage PressUndoHotkey()
		{
			CustomTestContext.WriteLine("Нажать хоткей кнопки отмены Ctrl Z.");
			Driver.WaitUntilElementIsDisplay(By.XPath(UNDO_BUTTON));
			Driver.SendHotKeys("z", control: true);

			return LoadPage();
		}

		/// <summary>
		/// Нажать хоткей кнопки возврата Ctrl Y.
		/// </summary>
		public EditorPage PressRedoHotkey()
		{
			CustomTestContext.WriteLine("Нажать хоткей кнопки возврата Ctrl Y.");
			Driver.SendHotKeys("y", control: true);

			return LoadPage();
		}

		/// <summary>
		/// Открыть форму 'Специальные символы' с помощью сочетания клавиш Ctrl+Shift+I
		/// </summary>
		public SpecialCharactersForm OpenSpecialCharacterFormByHotKey()
		{
			CustomTestContext.WriteLine("Открыть форму 'Специальные символы' с помощью сочетания клавиш Ctrl+Shift+I");
			Driver.SendHotKeys("I", true, true);

			return new SpecialCharactersForm(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку открытия словаря
		/// </summary>
		public SpellcheckDictionaryDialog ClickSpellcheckDictionaryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку открытия словаря");
			SpellcheckDictionaryButton.Click();

			return new SpellcheckDictionaryDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Search' в конкордансном поиске.
		/// </summary>
		public EditorPage ClickSearchButtonInConcordanceTable()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Search' в конкордансном поиске.");
			ConcordanceSearchButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Go to Last Unconfirmed Segment'
		/// </summary>
		public EditorPage ClickLastUnconfirmedButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Go to Last Unconfirmed Segment'.");
			LastUnconfirmedButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Вставить тег'
		/// </summary>
		public EditorPage ClickInsertTag()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Вставить тег'.");
			InsertTagButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выделить последний неподтвержденный сегмент нажатием F9
		/// </summary>
		public EditorPage SelectLastUnconfirmedSegmentByHotKey()
		{
			CustomTestContext.WriteLine("Выделить последний неподтвержденный сегмент нажатием F9");
			Driver.SendHotKeys(OpenQA.Selenium.Keys.F9);

			return LoadPage();
		}

		/// <summary>
		/// Ввести текст в поле поиска конкордансного поиска.
		/// </summary>
		/// <param name="searchText">текст поиска</param>
		public EditorPage SetConcordanceSearch(string searchText)
		{
			CustomTestContext.WriteLine("Ввести текст '{0}' в поле поиска конкордансного поиска.", searchText);
			ConcordanceSearchText.SetText(searchText);

			return LoadPage();
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
		///Получить текст из сорса в результирующей таблице конкордансного поиска.
		/// </summary>
		public string GetSourceConcordanceResultBySourceSearch()
		{
			CustomTestContext.WriteLine("Получить текст из сорса в результирующей таблице конкордансного поиска.");
			var sourceElemntList = Driver.GetTextListElement(By.XPath(CONCORDANCE_SEARCH_SOURCE_RESULT));
			StringBuilder builder = new StringBuilder();

			foreach (var el in sourceElemntList)
			{
				builder.Append(el).Append(" ");
			}

			return builder.ToString().Trim();
		}

		/// <summary>
		///Получить текст из таргета в результирующей таблице конкордансного поиска по сорсу..
		/// </summary>
		public string GetTargetConcordanceResultBySourceSearch()
		{
			CustomTestContext.WriteLine("Получить текст из таргета в результирующей таблице конкордансного поиска по сорсу.");

			return ConcordanceSearchTargetResult.Text;
		}

		/// <summary>
		///Получить текст из сорса в результирующей таблице конкордансного поиска по таргету.
		/// </summary>
		public string GetSourceConcordanceResultByTargetSearch()
		{
			CustomTestContext.WriteLine("Получить текст из сорса в результирующей таблице конкордансного поиска по таргету.");

			return ConcordanceSearchSourceResultByTarget.Text.Replace(".", "");
		}

		/// <summary>
		///Получить текст из таргета в результирующей таблице конкордансного поиска.
		/// </summary>
		public string GetTargetConcordanceResultByTargetSearch()
		{
			CustomTestContext.WriteLine("Получить текст из таргета в результирующей таблице конкордансного поиска.");
			var targetElemntList = Driver.GetTextListElement(By.XPath(CONCORDANCE_SEARCH_TARGET_RESULT));
			StringBuilder builder = new StringBuilder();

			foreach (var el in targetElemntList)
			{
				builder.Append(el).Append(" ");
			}

			return builder.ToString().Trim();
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
		/// <param name="startNumber">номер строки, с которой начинать поиск</param>
		/// <returns>номер строки в CAT-панели</returns>
		public int CatTypeRowNumber(CatType catType, int startNumber = 0)
		{
			CustomTestContext.WriteLine("Получить номер строки для {0} в CAT-панели.", catType);

			// Sleep нужен чтобы все элементы CAT-панели успели появиться
			Thread.Sleep(3000);

			var rowNumber = 0;
			var catTypeList = Driver.GetTextListElement(By.XPath(CAT_TYPE_LIST_IN_PANEL));

			CustomTestContext.WriteLine("Количество элементов в CAT-панели - {0}", catTypeList.Count);

			for (var i = startNumber; i < catTypeList.Count; ++i)
			{
				if (catTypeList[i].Contains(catType.ToString()))
				{
					rowNumber = i + 1;
					break;
				}
			}

			CustomTestContext.WriteLine("Номер строки с нужным типом {0} в CAT-панели - {1}", catType, rowNumber);

			if (rowNumber == 0)
			{
				throw new Exception(string.Format("Произошла ошибка:\n подстановка {0} отсутствует в CAT-панели.", catType));
			}

			return rowNumber;
		}

		/// <summary>
		/// Нажать кнопку 'Add Term'
		/// </summary>
		public AddTermDialog ClickAddTermButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Add Term'");
			AddTermButton.Click();

			return new AddTermDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку переноса строки
		/// </summary>
		public EditorPage ClickAddLineBreakButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку переноса строки.'");
			AddLineBreakButton.Click();

			return LoadPage();
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
		/// Получить текст  в конкордансном поиске
		/// </summary>
		public string GetTextFromConcordanceSearchField()
		{
			CustomTestContext.WriteLine("Получить текст  в конкордансном поиске.");

			return ConcordanceSearchText.GetAttribute("value");
		}

		/// <summary>
		/// Получить текст из CAT-панели для таргета
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public string GetTargetCatTranslationText(int rowNumber)
		{
			CustomTestContext.WriteLine("Получить текст таргета из CAT-панели, строка №{0}.", rowNumber);
			var catRowText = Driver.SetDynamicValue(How.XPath, TARGET_CAT_TRANSLATION, rowNumber.ToString());

			return catRowText.Text;
		}

		/// <summary>
		/// Получить текст из CAT-панели для сорса
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public string GetSourceCatTranslationText(int rowNumber)
		{
			CustomTestContext.WriteLine("Получить текст сорса из CAT-панели, строка №{0}.", rowNumber);
			var catRowText = Driver.SetDynamicValue(How.XPath, SOURCE_CAT_TRANSLATION, rowNumber.ToString());

			return catRowText.Text;
		}

		/// <summary>
		/// Получить термины первой колонкив в Cat-панели.
		/// </summary>
		public List<string> GetCatSourceTerms()
		{
			CustomTestContext.WriteLine("Получить термины первой колонки в Cat-панели.");
			var catSoursesTerms = Driver.GetTextListElement(By.XPath(SOURCE_CAT_TERMS));
			catSoursesTerms.Sort();

			return catSoursesTerms;
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
		public EditorPage CloseTutorial()
		{
			CustomTestContext.WriteLine("Проверить, видна ли подсказка");
			FinishTutorialButton.Click();

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(FINISH_TUTORIAL_BUTTON), timeout: 3))
			{
				CustomTestContext.WriteLine("Вторая попытка закрыть подсказку.");
				FinishTutorialButton.Click();
			}

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть Next в окне подсказки к ячейке исходного языка
		/// </summary>
		public EditorPage ClickNextButtonOnSourceFieldHelp()
		{
			CustomTestContext.WriteLine("Кликнуть Next в окне подсказки к ячейке исходного языка");
			NextButtonOnSourceFieldHelp.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть Next в окне подсказки к ячейке целевого языка
		/// </summary>
		public EditorPage ClickNextButtonOnTargetFieldHelp()
		{
			CustomTestContext.WriteLine("Кликнуть Next в окне подсказки к ячейке целевого языка");
			NextButtonOnTargetFieldHelp.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть Next в окне подсказки к Cat-панели
		/// </summary>
		public EditorPage ClickNextButtonOnCatPanelHelp()
		{
			CustomTestContext.WriteLine("Кликнуть Next в окне подсказки к Cat-панели");
			NextButtonOnCatPanelHelp.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть Next в окне подсказки к кнопке подтверждения
		/// </summary>
		public EditorPage ClickNextButtonOnConfirmHelp()
		{
			CustomTestContext.WriteLine("Кликнуть Next в окне подсказки к кнопке подтверждения.");
			NextButtonOnConfirmHelp.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть Next в окне подсказки к панели кнопок
		/// </summary>
		public EditorPage ClickNextButtonOnButtonBarHelp()
		{
			CustomTestContext.WriteLine("Кликнуть Next в окне подсказки к панели кнопок.");
			NextButtonOnButtonBarHelp.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть Finish в окне подсказки к инструментам обратной связи
		/// </summary>
		public EditorPage ClickFinishButtonOnFeedbackHelp()
		{
			CustomTestContext.WriteLine("Кликнуть Next в окне подсказки к инструментам обратной связи.");
			FinishButtonOnFeedbackHelp.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Копировать оригинал в перевод'
		/// </summary>
		public EditorPage ClickCopySourceToTargetButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Копировать оригинал в перевод'.");
			CopyButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на вкладку 'QA Check'
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public EditorPage ClickQualityAssuranceCheckTab(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать на вкладку 'QA Check'.");
			ClickOnTargetCellInSegment(segmentNumber);
			QACheckTab.Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить текст ошибки (вкладка 'QA Check').
		/// </summary>
		/// <param name="errorNumber">номер ошибки</param>
		public string GetErrorTextFromQaCheckTab(int errorNumber = 1)
		{
			CustomTestContext.WriteLine("Получить текст ошибки (вкладка 'QA Check').");
			QAError = Driver.SetDynamicValue(How.XPath, QA_ERROR, errorNumber.ToString());

			return QAError.Text;
		}

		/// <summary>
		/// Копировать текст из сорса в таргет с помощью сочетания клавиш Ctrl+Insert
		/// </summary>
		public EditorPage CopySourceToTargetHotkey()
		{
			CustomTestContext.WriteLine("Копировать текст из сорса в таргет с помощью сочетания клавиш Ctrl+Insert");
			Driver.SendHotKeys(Keys.Insert, control: true);

			return LoadPage();
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

			return percentColor.GetAttribute("class").Split(new [] { '-' }).Last();
		}

		/// <summary>
		/// Нажать кнопку 'конкордансный поиск'
		/// </summary>
		public EditorPage ClickConcordanceSearchButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'конкордансный поиск'");
			ConcordanceButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Открыть 'конкордансный поиск' с помощью сочетания клавиш Ctrl+k
		/// </summary>
		public EditorPage OpenConcordanceSearchByHotKey()
		{
			CustomTestContext.WriteLine("Открыть 'конкордансный поиск' с помощью сочетания клавиш Ctrl+k");
			Driver.SendHotKeys("k", control: true);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Rollback
		/// </summary>
		public EditorPage ClickRollbackButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Rollback.");
			RollbackButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Открть диалог добавления термина, нажав сочетание клавиш Ctrl+E
		/// </summary>
		public AddTermDialog OpenAddTermDialogByHotKey()
		{
			CustomTestContext.WriteLine("Открть диалог добавления термина, нажав сочетание клавиш Ctrl+E");
			Driver.SendHotKeys("e", true);

			return new AddTermDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Получить тип подстановки
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public string GetInsertResource(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Получить тип подстановки сегмента {0}", segmentNumber);
			InsertResource = Driver.SetDynamicValue(How.XPath, INSERT_RESOURCE, segmentNumber.ToString());

			return InsertResource.Text.Substring(0, 2);
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

			return LoadPage();
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

			return LoadPage();
		}

		public EditorPage ScrollToVoteCount(string author, string translation)
		{
			Driver.WaitUntilElementIsAppear(By.XPath(VOTE_COUNT.Replace("*#*", author).Replace("*##*", translation)));
			VoteCount = Driver.SetDynamicValue(How.XPath, VOTE_COUNT, author, translation);
			VoteCount.Scroll();

			return LoadPage();
		}

		/// <summary>
		/// Получить количество голосов перевода
		/// </summary>
		/// <param name="author">автор</param>
		/// <param name="translation">перевод</param>
		public int GetVoteCount(string author, string translation)
		{
			CustomTestContext.WriteLine("Получить количество голосов перевода '{0}' автора {1}.", translation, author);
			VoteCount = Driver.SetDynamicValue(How.XPath, VOTE_COUNT, author, translation);
			var count = VoteCount.Text;
			int result;

			if (!int.TryParse(count, out result))
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось преобразование количества голосов {0} в число.", count));
			}

			return result;
		}

		/// <summary>
		/// Нажать хоткей Shift F3 для изменения регистра.
		/// </summary>
		public EditorPage PressChangeCaseHotKey()
		{
			CustomTestContext.WriteLine("Нажать хоткей Shift F3 для изменения регистра.");
			Driver.SendHotKeys(Keys.F3, shift: true);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку изменения регистра.
		/// </summary>
		public EditorPage ClickChangeCaseButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку изменения регистра.");
			ChangeCaseButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Restore.
		/// </summary>
		public EditorPage ClickRestoreButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Restore.");
			RestoreButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить проценты из прогресс-бара.
		/// </summary>
		public float GetPercentInProgressBar()
		{
			CustomTestContext.WriteLine("Получить проценты из прогресс-бара.");
			var percentValue = ProgressBarPercents.GetAttribute("style").Split(new [] { ':', '%' });
			float result;

			try
			{
				result = Convert.ToSingle(percentValue[1], new CultureInfo("en-US"));
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format(
					"Произошла ошибка:\n не удалось преобразование процента(строка: {0}) прогресс-бара в число. Ошибка: {1}",
					percentValue[0], ex.Message));
			}

			return result;
		}

		/// <summary>
		/// Получить количество переведенных слов(в сорсе) из тултипа прогресс-бара.
		/// </summary>
		public int GetTranslatedWordCount()
		{
			CustomTestContext.WriteLine("Получить количество переведенных слов(в сорсе) из тултипа прогресс-бара.");
			string words = null;

			try
			{
				HoverProgressBar();
				var infoString = ProgressBarInfoString.Text;
				words = infoString.Remove(infoString.IndexOf(' '));
				return Convert.ToInt32(words);
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine(ex.Message);

				throw new Exception(string.Format(
					"Произошла ошибка:\n Не удалось конвертировать строку - {0} в число. Ошибка:{1}",
					words, ex.Message));
			}
		}

		/// <summary>
		/// Кликнуть по вкладке ревизий
		/// </summary>
		public EditorPage ClickRevisionTab()
		{
			CustomTestContext.WriteLine("Кликнуть по вкладке ревизий");
			RevisionTab.Click();

			return LoadPage();
		}

		/// <summary>
		/// Посчитать количество ревизий.
		/// </summary>
		public int GetRevisionsCount()
		{
			CustomTestContext.WriteLine("Посчитать количество ревизий.");

			return Driver.GetElementsCount(By.XPath(REVISION_LIST));
		}

		/// <summary>
		/// Получить текст удаленной части слова.
		/// </summary>
		/// <param name="revisionNumber">номер ревизии</param>
		public string GetRevisionDeleteChangedPart(int revisionNumber = 1)
		{
			CustomTestContext.WriteLine("Получить текст удаленной части слова.");
			DeleteChangedPart = Driver.SetDynamicValue(How.XPath, REVISION_DELETE_CHANGE_PART, revisionNumber.ToString());

			return DeleteChangedPart.Text;
		}

		/// <summary>
		/// Получить текст добавленной части слова.
		/// </summary>
		/// <param name="revisionNumber">номер ревизии</param>
		public string GetRevisionInsertChangedPart(int revisionNumber = 1)
		{
			CustomTestContext.WriteLine("Получить текст добавленной части слова.");
			InsertChangedPart = Driver.SetDynamicValue(How.XPath, REVISION_INSERT_CHANGE_PART, revisionNumber.ToString());
			InsertChangedPart.Scroll();

			return InsertChangedPart.Text;
		}

		/// <summary>
		/// Получить имя пользователя, создавшего ревизию.
		/// </summary>
		/// <param name="revisionNumber">номер ревизии</param>
		public string GetRevisionUserName(int revisionNumber = 1)
		{
			CustomTestContext.WriteLine("Получить имя пользователя, создавшего ревизию.");
			User = Driver.SetDynamicValue(How.XPath, REVISION_USER_COLUNM, revisionNumber.ToString());
			
			return User.Text;
		}

		/// <summary>
		/// Получить имя пользователя, создавшего ревизию.
		/// </summary>
		/// <param name="translation">перевод</param>
		public string GetSegmentTranslationUserName(string translation)
		{
			CustomTestContext.WriteLine("Получить имя пользователя на вкладке 'Segment translation'.");
			SegmentTranslationUserColumn = Driver.SetDynamicValue(How.XPath, SEGMENT_TRANSLATION_USER_COLUMN, translation);
			
			return SegmentTranslationUserColumn.Text;
		}

		/// <summary>
		/// Выделить ревизию
		/// </summary>
		/// <param name="revisionNumber">номер ревизии</param>
		public EditorPage SelectRevision(string revisionText = "Translation", int revisionNumber = 1)
		{
			CustomTestContext.WriteLine("Выделить ревизию №{0}.", revisionNumber);
			Driver.WaitUntilElementIsDisplay(By.XPath(REVISION_IN_LIST.Replace("*#*", revisionNumber.ToString()).Replace("*##*", revisionText)));
			Revision = Driver.SetDynamicValue(How.XPath, REVISION_IN_LIST, revisionNumber.ToString(), revisionText);
			Revision.Click();
			
			return LoadPage();
		}

		/// <summary>
		/// Получить тип ревизии
		/// </summary>
		/// <param name="revisionNumber">номер ревизии</param>
		public string GetRevisionType(int revisionNumber = 1)
		{
			CustomTestContext.WriteLine("Получить тип ревизии №{0}.", revisionNumber);
			Driver.WaitUntilElementIsDisplay(By.XPath(REVISION_TYPE.Replace("*#*", revisionNumber.ToString())));
			Type = Driver.SetDynamicValue(How.XPath, REVISION_TYPE, revisionNumber.ToString());

			return Type.Text;
		}

		/// <summary>
		/// Прокрутить страницу до нужного таргета по номеру сегмента
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public EditorPage ScrollToTarget(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Прокрутить страницу до таргета №{0}.", segmentNumber);
			TargetCell = Driver.SetDynamicValue(How.XPath, TARGET_CELL, (segmentNumber - 1).ToString());
			TargetCell.Scroll();

			return LoadPage();
		}

		/// <summary>
		/// Прокрутить страницу до перевода в кат панели
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage ScrollToTranslationInCatPanel(int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Прокрутить страницу до перевода в сегменте №{0} в кат панели.", rowNumber);
			var cat = Driver.SetDynamicValue(How.XPath, TARGET_CAT_TRANSLATION, rowNumber.ToString());
			cat.Scroll();

			return LoadPage();
		}

		/// <summary>
		/// Навести курсор на перевод в кат панели
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage HoverToTranslationInCatPanel(int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Навести курсор на перевод сегмента №{0} в кат панели.", rowNumber);
			var cat = Driver.SetDynamicValue(How.XPath, TARGET_CAT_TRANSLATION, rowNumber.ToString());
			cat.HoverElement();

			return LoadPage();
		}

		/// <summary>
		/// Отфильтровать сегменты по слову в source
		/// </summary>
		/// <param name="text">слово в source</param>
		public EditorPage FilterSegmentsBySource(string text)
		{
			CustomTestContext.WriteLine("Отфильтровать сегменты по слову {0} в source", text);
			SourceFilterField.SetText(text);
			Thread.Sleep(3000); // Фильтрация происходит автоматически после ввода слова

			return LoadPage();
		}

		/// <summary>
		/// Отфильтровать сегменты по слову в target
		/// </summary>
		/// <param name="text">слово в target</param>
		public EditorPage FilterSegmentsByTarget(string text)
		{
			CustomTestContext.WriteLine("Отфильтровать сегменты по слову {0} в target", text);
			TargetFilterField.SetText(text);
			Thread.Sleep(3000); // Фильтрация происходит автоматически после ввода слова

			return LoadPage();
		}

		/// <summary>
		/// Вернуть кол-во видимых сегментов
		/// </summary>
		public int GetVisibleSegmentsCount()
		{
			CustomTestContext.WriteLine("Вернуть кол-во видимых сегментов");

			return Driver.GetElementsCount(By.XPath(VISIBLE_SEGMENTS_COUNT));
		}

		/// <summary>
		/// Открыть меню замены
		/// </summary>
		public EditorPage OpenReplaceMenu()
		{
			CustomTestContext.WriteLine("Открыть меню замены");
			ReplaceMenuButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Заменить'
		/// </summary>
		public EditorPage ClickReplaceButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Заменить'");
			ReplaceButton.Click();
			Thread.Sleep(1000); // Для выделения слова
			ReplaceButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Заменить все'
		/// </summary>
		public EditorPage ClickReplaceAllButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Заменить все'");
			ReplaceAllButton.HoverElement();
			Thread.Sleep(1000); // Нужен чтобы успел исчезнуть tooltip
			ReplaceAllButton.Click();
			Driver.WaitUntilElementIsDisplay(By.XPath(REPLACE_NOTIFICATION));

			return LoadPage();
		}

		/// <summary>
		/// Ввести слово в поле для замены
		/// </summary>
		/// <param name="text">слово в target</param>
		public EditorPage FillTextForReplace(string text)
		{
			CustomTestContext.WriteLine("Ввести слово {0} в поле для замены", text);
			ReplaceField.SetText(text);

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по кнопке пользовательских настроек.
		/// </summary>
		public UserPreferencesDialog ClickUserPreferencesButton()
		{
			CustomTestContext.WriteLine("Кликнуть по кнопке пользовательских настроек.");
			UserPreferencesButton.Click();

			return new UserPreferencesDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Навести курсор на прогресс-бар.
		/// </summary>
		public EditorPage HoverProgressBar()
		{
			CustomTestContext.WriteLine("Навести курсор на прогресс-бар.");
			ProgressBar.HoverElement();
			Driver.WaitUntilElementIsDisplay(By.XPath(PROGRESS_BAR_TOOLTIP));

			return LoadPage();
		}

		/// <summary>
		/// Получить список текстов из прогресс-бара.
		/// </summary>
		public string GetTextFromProgressBar()
		{
			CustomTestContext.WriteLine("Получить список текстов из прогресс-бара.");
			HoverProgressBar();
			var listTexts = Driver.GetTextListElement(By.XPath(PROGRESS_BAR_TOOLTIP));
			//1. Translation (T): 0 words out of 13 (0%)

			return String.Concat(listTexts).Replace("\r\n", " ");
		}

		#endregion

		#region Составные методы страницы
		
		/// <summary>
		/// Проскролить и получить количество голосов.
		/// </summary>
		/// <param name="author">автор</param>
		/// <param name="translation">перевод</param>
		public int ScrollAndGetVoteCount(string author, string translation)
		{
			ScrollToVoteCount(author, translation);

			return GetVoteCount(author, translation);
		}

		/// <summary>
		/// Отправить комментарий документа
		/// </summary>
		/// <param name="comment">комментарий</param>
		public EditorPage SendDocumentComment(string comment)
		{
			FillDocumentComment(comment);
			ClickSendDocumentCommentButton();

			return LoadPage();
		}

		/// <summary>
		/// Отправить комментарий сегмента
		/// </summary>
		/// <param name="comment">комментарий</param>
		public EditorPage SendSegmentComment(string comment)
		{
			FillSegmentComment(comment);
			ClickSegmentSendCommentButton();
			
			return LoadPage();
		}
		/// <summary>
		/// Отправить комментарий
		/// </summary>
		/// <param name="author">автор</param>
		/// <param name="comment">комментарий</param>
		public EditorPage DeleteComment(string author, string comment)
		{
			HoverComment(author, comment);
			ClickDeleteCommentButton();

			return LoadPage();
		}
		/// <summary>
		/// Двойной клик по переводу в CAT-панели
		/// </summary>
		/// <param name="rowNumber">номер строки в CAT-панели</param>
		public EditorPage DoubleClickCatPanelRow(int rowNumber)
		{
			ScrollToTranslationInCatPanel(rowNumber);
			// Sleep не убирать, без него не скролится
			Thread.Sleep(1000);
			HoverToTranslationInCatPanel(rowNumber);
			
			CustomTestContext.WriteLine("Двойной клик по строке №{0} с переводом в CAT-панели.", rowNumber);

			var cat = Driver.SetDynamicValue(How.XPath, TARGET_CAT_TRANSLATION, rowNumber.ToString());
			cat.DoubleClick();

			return LoadPage();
		}

		/// <summary>
		/// Получить текст из таргет сегмента
		/// </summary>
		/// <param name="rowNumber">номер строки сегмента</param>
		/// <param name="empty">ожидается пустая строка</param>
		public string GetTargetText(int rowNumber, bool empty = false)
		{
			CustomTestContext.WriteLine("Получить текст из таргет сегмента №{0}.", rowNumber);
			ScrollToTarget(rowNumber);
			TargetCell = Driver.SetDynamicValue(How.XPath, TARGET_CELL, (rowNumber - 1).ToString());

			if (!empty)
			{
				Driver.WaitUntilTextToBePresentInElementLocated(TargetCell);
			}

			return TargetCell.Text.Trim();
		}

		/// <summary>
		/// Получить список подсвеченных в сегменте слов
		/// </summary>
		/// <param name="segmentNumber">Номер сегмента</param>
		/// <returns>Список подсвеченных в сегменте слов</returns>
		public List<string> GetHighlightedWords(int segmentNumber)
		{
			
			var highlightedWords = new List<string>();

			var targetCell = Driver.SetDynamicValue(How.XPath, TARGET_CELL, (segmentNumber - 1).ToString());
			CustomTestContext.WriteLine("Проскролить до таргет ячейки сегмента №{0}.", segmentNumber);
			targetCell.Scroll();
			CustomTestContext.WriteLine("Кликнуть по таргет ячейке сегмента №{0}.", segmentNumber);
			targetCell.Click();

			CustomTestContext.WriteLine("Получить список подсвеченных в сегменте №{0} слов.", segmentNumber);
			var segmentCatSelectedList = Driver.GetElementList(By.XPath(HIGHLIGHTED_SEGMENT.Replace("*#*", segmentNumber.ToString())));

			if (segmentCatSelectedList.Count > 0)
			{
				CustomTestContext.WriteLine("Кол-во подсвеченных в сегменте слов - {0}", segmentCatSelectedList.Count);
				highlightedWords.AddRange(segmentCatSelectedList.Select(item => item.Text.ToLower()));
			}

			highlightedWords.Remove(string.Empty);
			highlightedWords.Sort();

			return highlightedWords;
		}
		
		/// <summary>
		/// Открыть вкладку ревизий
		/// </summary>
		public EditorPage OpenRevisionTab()
		{
			CustomTestContext.WriteLine("Открыть вкладку ревизий");
			if (!isRevisionTableDisplayed())
			{
				ClickRevisionTab();
			}

			return LoadPage();
		}

		/// <summary>
		/// Выделение части строки до первого пробела Home+Shift+Right
		/// </summary>
		/// <param name="text">текст</param>
		public EditorPage SelectWordPartBeforeSpaceByHotkey(string text)
		{
			CustomTestContext.WriteLine("Выделение части строки до первого пробела Home+Shift+Right.");
			Driver.SendHotKeys(Keys.Home, control:true);
			var array = text.Split(' ');
			while (GetSelectedWordInSegment().Length != array[0].Length)
			{
				Driver.SendHotKeys(Keys.Right, shift: true);
			}
			
			return LoadPage();
		}

		/// <summary>
		/// Нажать хоткей выделения последнего слова Ctrl+Shift+Left
		/// </summary>
		public EditorPage SelectLastWordByHotkey(int segmentNumber)
		{
			CustomTestContext.WriteLine("Нажать хоткей выделения последнего слова в таргете Ctrl+Shift+Left. Номер строки: {0}", segmentNumber);
			ClickOnTargetCellInSegment(segmentNumber);
			Driver.SendHotKeys(Keys.End);
			Driver.SendHotKeys(Keys.Left, control: true, shift: true);

			return LoadPage();
		}

		/// <summary>
		/// Нажать хоткей выделения нескольких символов Ctrl+Left, Ctrl+Right, symbolsCount+Shift.
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <param name="symbolsCount">количество символов</param>
		public EditorPage SelectFewSymbolsInLastWordByHotkey(int segmentNumber, int symbolsCount)
		{
			CustomTestContext.WriteLine("Нажать хоткей выделения {0} символов в последнем слове. Номер строки: {1}", symbolsCount, segmentNumber);
			var text = GetTargetText(segmentNumber);
			var array = text.Split(' ');
			var symbols = array.Last();

			Driver.SendHotKeys(Keys.End, control: true);
			Driver.SendHotKeys(Keys.Left, control: true);
			Driver.SendHotKeys(Keys.Right);

			for (int i = 0; i < symbolsCount; i++)
			{
				Driver.SendHotKeys(Keys.Right, shift: true);
			}

			var selectedSymbols = GetSelectedWordInSegment();

			if (!symbols.Contains(selectedSymbols) && GetSelectedWordInSegment().Length != symbolsCount)
			{
				Driver.SendHotKeys(Keys.End, control: true);
				Driver.SendHotKeys(Keys.Left, control: true);
				Driver.SendHotKeys(Keys.Right);
				for (int i = 0; i < symbolsCount; i++)
				{
					Driver.SendHotKeys(Keys.Right, shift: true);
				}
			}

			return LoadPage();
		}

		/// <summary>
		/// Нажать хоткей выделения второго и третьего слов Ctrl+Home, Ctrl+Right, Ctrl+Shift+Right, Ctrl+Shift+Right, Ctrl+Right.
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public EditorPage SelectSecondThirdWordsByHotkey(int segmentNumber)
		{
			CustomTestContext.WriteLine("Нажать хоткей выделения второго и третьего слов. Номер строки: {0}", segmentNumber);
			var text = GetTargetText(segmentNumber);
			var array = text.Split(' ');
			var lenght = array[1].Length + array[2].Length + 1;
			var selectedText = array[1] + " " + array[2];

			ClickOnTargetCellInSegment(segmentNumber);
			Driver.SendHotKeys(Keys.Home, control: true);
			Driver.SendHotKeys(Keys.Right, control: true);

			for (int i = 0; i <= lenght; i++)
			{
				Driver.SendHotKeys(Keys.Right, shift: true);
			}

			if (GetSelectedWordInSegment() != selectedText)
			{
				ClickOnTargetCellInSegment(segmentNumber);
				Driver.SendHotKeys(Keys.Home, control: true);
				Driver.SendHotKeys(Keys.Right, control: true);
				for (int i = 0; i <= lenght; i++)
				{
					Driver.SendHotKeys(Keys.Right, shift: true);
				}
			}

			return LoadPage();
		}

		/// <summary>
		/// Нажать хоткей выделения всего содержимого ячейки Ctrl+Shift+Home
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public EditorPage SelectAllTextByHotkeyInSource(int segmentNumber)
		{
			CustomTestContext.WriteLine(
				"Нажать хоткей выделения всего содержимого ячейки Ctrl+Shift+Home. Номер строки: {0}",
				segmentNumber);
			ClickOnSourceCellInSegment(segmentNumber);
			Driver.SendHotKeys(Keys.End);
			Driver.SendHotKeys(Keys.Home, control: true, shift: true);

			return LoadPage();
		}

		/// <summary>
		/// Нажать хоткей выделения комментария.
		/// </summary>
		public EditorPage SelectCommentText(string author, string comment)
		{
			CustomTestContext.WriteLine("Нажать хоткей выделения комментария.");
			Comment = Driver.SetDynamicValue(How.XPath, COMMENT, author, comment);
			Comment.Click();
			Comment.DoubleClick();
			
			return LoadPage();
		}

		/// <summary>
		/// Нажать хоткей выделения всего содержимого ячейки Ctrl+Shift+Home
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public EditorPage SelectAllTextByHotkey(int segmentNumber)
		{
			CustomTestContext.WriteLine(
				"Нажать хоткей выделения всего содержимого ячейки Ctrl+Shift+Home. Номер строки: {0}",
				segmentNumber);
			ClickOnTargetCellInSegment(segmentNumber);
			Driver.SendHotKeys(Keys.End);
			Driver.SendHotKeys(Keys.Home, control: true, shift: true);

			return LoadPage();
		}

		/// <summary>
		/// Нажать хоткей копирования Ctrl+c.
		/// </summary>
		public EditorPage CopyByHotkey()
		{
			CustomTestContext.WriteLine("Нажать хоткей копирования Ctrl+c.");
			Driver.SendHotKeys("c", control: true);

			return LoadPage();
		}

		/// <summary>
		/// Нажать хоткей вставки Ctrl+v.
		/// </summary>
		public EditorPage PasteByHotkey()
		{
			CustomTestContext.WriteLine("Нажать хоткей вставки Ctrl+v.");
			Driver.SendHotKeys("v", control: true);

			return LoadPage();
		}

		/// <summary>
		/// Ввести текст в таргет сегмента
		/// </summary>
		/// <param name="text">текст</param>
		/// <param name="rowNumber">номер сегмента</param>
		/// <param name="clearField">очистить поле перед вводом (default)</param>
		public EditorPage FillSegmentTargetField(string text = "Translation", int rowNumber = 1, bool clearField = true)
		{
			ClickOnTargetCellInSegment(rowNumber);

			CustomTestContext.WriteLine("Ввести текст в таргет сегмента {0}.", rowNumber);
			TargetCellValue = Driver.SetDynamicValue(How.XPath, TARGET_CELL_VALUE, (rowNumber - 1).ToString());

			if (clearField)
			{
				TargetCellValue.SetText(text, expectedText: text);
			}
			else
			{
				TargetCellValue.SendKeys(text);
			}

			return LoadPage();
		}

		/// <summary>
		/// Удалить текст из таргет сегмента.
		/// </summary>
		/// <param name="rowNumber">номер ряда</param>
		public EditorPage RemoveTextFromTargetSegment(int rowNumber = 1)
		{
			ClickOnTargetCellInSegment(rowNumber);
			CustomTestContext.WriteLine("Удалить текст из таргет сегмента, кликнув на клавишу - {0}", Keys.Delete);
			SelectAllTextByHotkey(rowNumber).Driver.SendHotKeys(Keys.Delete);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить таргет
		/// </summary>
		/// <param name="text">текст</param>
		/// <param name="rowNumber">номер строки</param>
		public EditorPage FillTarget(string text, int rowNumber = 1, bool clearField = true)
		{
			ClickOnTargetCellInSegment(rowNumber);
			FillSegmentTargetField(text, rowNumber, clearField);

			return LoadPage();
		}

		/// <summary>
		/// Откат сегмента
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public EditorPage RollBack(int segmentNumber = 1)
		{
			ClickOnTargetCellInSegment(segmentNumber);
			ClickRollbackButton();

			return LoadPage();
		}

		/// <summary>
		/// Вставить перевод из CAT-панели
		/// </summary>
		/// <param name="catType">CAT-тип</param>
		/// <param name="targetRowNumber">номер строки таргета</param>
		public EditorPage PasteTranslationFromCAT(CatType catType, int targetRowNumber = 1)
		{
			ClickOnTargetCellInSegment(targetRowNumber);

			var catRowNumber = CatTypeRowNumber(catType);

			DoubleClickCatPanelRow(catRowNumber);

			if (!GetTargetText(targetRowNumber).Equals(GetTargetCatTranslationText(catRowNumber), StringComparison.OrdinalIgnoreCase))
			{
				throw new Exception(
					string.Format("Текст из таргет сегмента {0} не совпадает с текстом перевода из CAT-панели {1}",
					GetTargetText(targetRowNumber), GetTargetCatTranslationText(catRowNumber)));
			}

			return LoadPage();
		}

		/// <summary>
		/// Вставить перевод из CAT-панели с помощью хоткея Ctrl + "номер подстановки"
		/// </summary>
		/// <param name="catType">CAT-тип</param>
		/// <param name="targetRowNumber">номер строки таргета</param>
		public EditorPage PasteTranslationFromCATByHotkey(CatType catType, int targetRowNumber = 1)
		{
			ClickOnTargetCellInSegment(targetRowNumber);

			if (!IsCatTableExist())
			{
				throw new Exception(String.Format("Кат панель не появилась после клика по таргету сегмента №{0}", targetRowNumber));
			}

			if (!IsCatTypeExist(catType))
			{
				throw new Exception("Не удалось дождаться появления подстановки нужного вида в кат панели");
			}

			var catRowNumber = CatTypeRowNumber(catType);

			Driver.SendHotKeys(catRowNumber.ToString(), control: true);

			if (!GetTargetText(targetRowNumber).Equals(GetTargetCatTranslationText(catRowNumber), StringComparison.OrdinalIgnoreCase))
			{
				throw new Exception(
					string.Format("Текст из таргет сегмента {0} не совпадает с текстом перевода из CAT-панели {1}",
					GetTargetText(targetRowNumber), GetTargetCatTranslationText(catRowNumber)));
			}

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что кнопка голосования за перевод неактивна.
		/// </summary>
		/// <param name="author">автор</param>
		/// <param name="translatioin">перевод</param>
		public bool IsVoteUpButtonDisabled(string author, string translatioin)
		{
			CustomTestContext.WriteLine("Проверить, что кнопка голосования за перевод {0} автора {1} неактивна.", translatioin, author);
			VoteUpDisabledButton = Driver.SetDynamicValue(How.XPath, VOTE_DOWN_BUTTON_INACTIVE, author, translatioin);

			return VoteUpDisabledButton.Displayed;
		}

		/// <summary>
		/// Проверить, что отображается терминологическая ошибка
		/// </summary>
		/// <param name="term">термин</param>
		public bool IsTerminilogyErrorDisplayed(string term)
		{
			CustomTestContext.WriteLine("Проверить, что отображается терминологическая ошибка термина '{0}'.", term);

			return Driver.WaitUntilElementIsAppear(By.XPath(TERM_ERROR.Replace("*#*", term)));
		}

		/// <summary>
		/// Проверить, что отображается комментарий.
		/// </summary>
		/// <param name="author">автор</param>
		/// <param name="comment">комментарий</param>
		public bool IsCommentDisplayed(string author, string comment)
		{
			CustomTestContext.WriteLine("Проверить, что отображается комментарий '{0}' автора {1}.", comment, author);
			Comment = Driver.SetDynamicValue(How.XPath, COMMENT, author, comment);

			return Comment.Displayed;
		}

		/// <summary>
		/// Проверить, что не отображается комментарий.
		/// </summary>
		/// <param name="author">автор</param>
		/// <param name="comment">комментарий</param>
		public bool IsCommentNotDisplayed(string author, string comment)
		{
			CustomTestContext.WriteLine("Проверить, что не отображается комментарий '{0}' автора {1}.", comment, author);

			return Driver.WaitUntilElementIsDisappeared(By.XPath(COMMENT.Replace("*#*", author).Replace("*##*", comment)));
		}

		/// <summary>
		/// Проверить, что вкладка комментариев сегмента подсвечена.
		/// </summary>
		public bool IsSegmentCommentTabHighlighted()
		{
			CustomTestContext.WriteLine("Проверить, что вкладка комментариев сегмента подсвечена.");

			return SegmentCommentsTab.GetAttribute("class").Contains("highlight");
		}

		/// <summary>
		/// Проверить, что вкладка комментариев документа подсвечена.
		/// </summary>
		public bool IsDocumentCommentTabHighlighted()
		{
			CustomTestContext.WriteLine("Проверить, что вкладка комментариев документа подсвечена.");

			return DocumentCommentsTab.GetAttribute("class").Contains("highlight");
		}

		/// <summary>
		/// Проверить, что отображается таблица с результатами конкордансного поиска.
		/// </summary>
		public bool IsConcordanceSearchResultTableDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отображается таблица с результатами конкордансного поиска.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(CONCORDANCE_SEARCH_RESULT_TABLE));
		}

		/// <summary>
		/// Проверить, что радиокнопка Source отменчена в конкордансном поиске.
		/// </summary>
		public bool IsSourceRadioButtonChecked()
		{
			CustomTestContext.WriteLine("Проверить, что радиокнопка Source отменчена в конкордансном поиске.");

			return SourceRadiobutton.GetAttribute("aria-checked") == "true";
		}

		/// <summary>
		/// Проверить, что радиокнопка Target отменчена в конкордансном поиске.
		/// </summary>
		public bool IsTargetRadioButtonChecked()
		{
			CustomTestContext.WriteLine("Проверить, что радиокнопка Target отменчена в конкордансном поиске.");

			return TargetRadiobutton.GetAttribute("aria-checked") == "true";
		}

		/// <summary>
		/// Проверить, что все термины сохранены
		/// </summary>
		public bool IsAllSegmentsSavedMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что все термины сохранены.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ALL_SEGMENTS_SAVED_STATUS));
		}

		/// <summary>
		/// Дождаться сообщения, что все термины сохранены.
		/// </summary>
		public EditorPage WaitAllSegmentsSavedMessageDisplayed()
		{
			CustomTestContext.WriteLine("Дождаться сообщения, что все термины сохранены.");
			int t = 0;
			while(t < 60 && !Driver.WaitUntilElementIsDisplay(By.XPath(ALL_SEGMENTS_SAVED_STATUS)))
			{
				Thread.Sleep(1000);
				t++;
			}

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ALL_SEGMENTS_SAVED_STATUS)))
			{
				throw new Exception("Термины в редакторе не сохранились.");
			}

			return LoadPage();
		}

		/// <summary>
		/// Проверить, что отображается таблица ревизий.
		/// </summary>
		private bool isRevisionTableDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отображается таблица ревизий.");

			return RevisionTable.Displayed;
		}

		/// <summary>
		/// Проверить, что кнопка Restore неактивна
		/// </summary>
		public bool IsRestoreButtonDisabled()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка Restore неактивна.");

			return RestoreButton.GetAttribute("class").Contains("x-btn-disabled");
		}

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
			return Driver.WaitUntilElementIsDisappeared(By.XPath(SAVING_STATUS));
		}

		/// <summary>
		/// Проверить, что конкордансный поиск появился
		/// </summary>
		public bool IsConcordanceSearchDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что конкордансный поиск появился.");

			return ConcordanceSearch.Displayed;
		}

		/// <summary>
		/// Проверить, что текст из CAT-подстановки МТ соответствуют тексту в сорсе сегмента 
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public bool IsMTSourceTextMatchSourceText(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что текст из CAT-подстановки МТ соответствуют тексту в сорсе сегмента №{0}.", segmentNumber);
			var sourceText = GetSourceText(segmentNumber);

			return Driver.GetIsTextToBePresentInElementLocated(By.XPath(MT_SOURCE_TEXT_IN_CAT_PANEL), sourceText);
		}

		/// <summary>
		/// Проверить, что сегмент не залочен
		/// </summary>
		public bool IsSegmentLocked(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что сегмент №{0} не залочен.", segmentNumber);

			try
			{
				return Driver.WaitUntilElementIsDisplay(By.XPath(SEGMENT_LOCK.Replace("*#*", segmentNumber.ToString())));
			}
			catch (Exception)
			{
				throw new XPathLookupException("Произошла ошибка:\nЗначок блокировки сегмента не отображается в редакторе.");
			}
		}

		/// <summary>
		/// Проверить, что только все переданные слова есть в CAT-панели
		/// </summary>
		/// <param name="words">список слов</param>
		public bool IsWordsMatchCatWords(List<string> words)
		{
			CustomTestContext.WriteLine("Проверить, что только все переданные слова есть в CAT-панели");

			return Driver.GetElementsCount(By.XPath(CAT_TYPE_LIST_IN_PANEL)) == words.Count 
				&& words.All(word =>
				{
					if (Driver.GetIsElementExist(By.XPath(CAT_SOURCE.Replace("*#*", word))))
					{
						CustomTestContext.WriteLine("Слово '{0}' есть в CAT-панели", word);
						return true;
					}
					else
					{
						CustomTestContext.WriteLine("Слово '{0}' отсутствует в CAT-панели", word);
						return false;
					}
				});
		}

		/// <summary>
		/// Проверить, что подстановка нужного типа есть в CAT-панели
		/// </summary>
		/// <param name="type">название типа</param>
		public bool IsCatTypeExist(CatType type)
		{
			CustomTestContext.WriteLine("Проверить, что подстановка типа {0} есть в CAT-панели", type);
			
			return Driver.WaitUntilElementIsAppear(By.XPath(CAT_TYPE.Replace("*#*", type.ToString())));
		}

		/// <summary>
		/// Проверить, присутствует ли таблица в CAT-панели
		/// </summary>
		public bool IsCatTableExist()
		{
			CustomTestContext.WriteLine("Проверить, присутствует ли таблица в CAT-панели");

			return Driver.WaitUntilElementIsAppear(By.XPath(CAT_TABLE));
		}

		/// <summary>
		/// Проверить, видно ли кнопку закрытия туториала
		/// </summary>
		public bool IsCloseTutorialButtonDisplay()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(FINISH_TUTORIAL_BUTTON));
		}

		/// <summary>
		/// Проверить, открылся ли редактор
		/// </summary>
		public bool IsEditorPageOpened(bool closedTutorial = true)
		{
			if (!closedTutorial)
			{
				return IsSavingStatusDisappeared() &&
						Driver.WaitUntilElementIsAppear(By.XPath(SEGMENTS_BODY), timeout: 60);
			}
			else
			{
				return IsSavingStatusDisappeared() &&
						Driver.WaitUntilElementIsAppear(By.XPath(SEGMENTS_BODY), timeout: 60) &&
						Driver.WaitUntilElementIsDisappeared(By.XPath(EDITOR_DIALOG_BACKGROUND));
			}
		}

		/// <summary>
		/// Проверить, совпадает ли текст в колонке MatchColumn с ожидаемым
		/// </summary>
		/// <param name="catType">CAT-тип</param>
		/// <param name="rowNumber">номер строки</param>
		public bool IsMatchColumnCatTypeMatch(CatType catType, int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что текст в колонке MatchColumn совпадает с {0}.", catType);
			var textInMacthColumn = GetMatchColumnText(rowNumber).Trim();

			if (textInMacthColumn.Contains("%"))
			{
				textInMacthColumn = textInMacthColumn.Substring(0, 2);
			}

			return textInMacthColumn.Trim() == catType.ToString();
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
		/// Проверить, подтвердился ли сегмент краудсорсером
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public bool IsSegmentCrowdConfirmed(int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, подтвердился ли сегмент {0} краудсорсером.", rowNumber);

			return Driver.WaitUntilElementIsDisplay(By.XPath(CROWD_CONFIRMED_ICO.Replace("*#*", (rowNumber - 1).ToString())));
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
			// нужен сли из-за sre
			Thread.Sleep(1000);

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

			return Driver.WaitUntilElementIsAppear(By.XPath(TERM_SAVED_MESSAGE));
		}

		/// <summary>
		/// Проверить, что появилось сообщение о том, что перевод содержит критическую ошибку
		/// </summary>
		public bool IsMessageWithCrititcalErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение о том, что перевод содержит критическую ошибку.");

			return Driver.WaitUntilElementIsAppear(By.XPath(MESSAGE_WITH_CRITICAL_ERROR), timeout: 15);
		}


		/// <summary>
		/// Проверить, что сообщение о том, что термин сохранен, исчезло
		/// </summary>
		public bool IsTermSavedMessageDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение о том, что термин сохранен, исчезло");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(TERM_SAVED_MESSAGE), timeout: 30);
		}

		/// <summary>
		/// Проверить, что присутствует колонка этапа
		/// </summary>
		public bool IsWorkflowColumnDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что присутствует колонка этапа.");

			return Driver.GetIsElementExist(By.XPath(WORKFLOW_COLUMN));
		}

		/// <summary>
		/// Проверить, что появился треугольник с ошибкой, после подтверждения сегмента.
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public bool IsSegmentErrorLogoDisplayed(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что появился треугольник с ошибкой, после подтверждения сегмента №{0}.", segmentNumber);

			return Driver.WaitUntilElementIsDisplay(By.XPath(SEGMENT_ERROR_LOGO.Replace("*#*", (segmentNumber-1).ToString())));
		}

		/// <summary>
		/// Проверить, треугольник с ошибкой неактивен.
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public bool IsYellowTriangleErrorInactive(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, треугольник с ошибкой неактивен для сегмента №{0}.", segmentNumber);

			return Driver.FindElement(By.XPath(SEGMENT_ERROR_LOGO.Replace("*#*", (segmentNumber-1).ToString()))).GetAttribute("style").Contains("display: none;");
		}

		/// <summary>
		/// Проверить, что показывается стрелка, говорящая, что в этом сегменте находится повтор.
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public bool IsRepetitionArrowDisplayed(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить видимость стрелки, говорящей, что в сегменте №{0} находится повтор.", segmentNumber);

			return Driver.WaitUntilElementIsDisplay(Driver.GetValueOfDynamicBy(How.XPath, REPETITION_ARROW, (segmentNumber - 1).ToString()));
		}

		/// <summary>
		/// Проверить, что повтор исключён.
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public bool IsRepetitionExcluded(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что повтор исключён для сегмента №{0}.", segmentNumber);

			return Driver.WaitUntilElementIsDisplay(Driver.GetValueOfDynamicBy(How.XPath, REPETITION_ARROW_EXCLUDED, (segmentNumber - 1).ToString()));
		}

		/// <summary>
		/// Проверить, что появился попап со списком ошибок.
		/// </summary>
		public bool IsErrorsPopupExist()
		{
			CustomTestContext.WriteLine("Проверить, что появился попап со списком ошибок.");

			return Driver.WaitUntilElementIsAppear(By.XPath(ERRORS_POPUP));
		}

		/// <summary>
		/// Проверить, что попап со списком ошибок содержит правильную ошибку.
		/// </summary>
		/// <param name="error">ошибка</param>
		public bool IsErrorsPopupContainsCorrectError(string error)
		{
			CustomTestContext.WriteLine("Проверить, что попап со списком ошибок содержит правильную ошибку '{0}'.", error);
			var textInPopup = Driver.ExecuteScript("return document.getElementById('ext-quicktips-tip-innerCt').innerHTML");

			return textInPopup.ToString().Contains(error);
		}

		/// <summary>
		/// Проверить, что ошибока в попапе критическая.
		/// </summary>
		/// <param name="error">ошибка</param>
		public bool IsCriticalError(string error)
		{
			CustomTestContext.WriteLine("Проверить, что попап со списком ошибок содержит правильную ошибку '{0}'.", error);
			var textInPopup = Driver.ExecuteScript("return document.getElementById('ext-quicktips-tip-innerCt').innerHTML");

			return textInPopup.ToString().Contains(error + " (critical)");
		}

		/// <summary>
		/// Проверить, что таблица с ошибками отсутствует (вкладка 'QA Check').
		/// </summary>
		public bool IsQAErrorTableDissapeared()
		{
			CustomTestContext.WriteLine("Проверить, что таблица с ошибками отсутствует (вкладка 'QA Check').");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(QA_ERROR_TABLE));
		}

		/// <summary>
		/// Проверить, что таблица с ошибками приотсутствует (вкладка 'QA Check').
		/// </summary>
		public bool IsQAErrorTableDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что таблица с ошибками приотсутствует (вкладка 'QA Check').");

			return Driver.WaitUntilElementIsAppear(By.XPath(QA_ERROR_TABLE));
		}

		/// <summary>
		/// Проверить, что есть подсказка к ячейке исходного текста.
		/// </summary>
		public bool IsSourceColumnHelpDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что есть подсказка к ячейке исходного текста.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(NEXT_BUTTON_ON_SOURCE_FIELD_HELP));
		}

		/// <summary>
		/// Проверить, что есть подсказка к ячейке целевого текста.
		/// </summary>
		public bool IsTargetColumnHelpDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что есть подсказка к ячейке целевого текста.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(NEXT_BUTTON_ON_TARGET_FIELD_HELP));
		}

		/// <summary>
		/// Проверить, что есть подсказка к Cat-панели.
		/// </summary>
		public bool IsCatPanelHelpDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что есть подсказка к Cat-панели.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(NEXT_BUTTON_ON_CAT_PANEL_HELP));
		}

		/// <summary>
		/// Проверить, что есть подсказка к кнопке подтверждения.
		/// </summary>
		public bool IsConfirmHelpDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что есть подсказка к кнопке подтверждения.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(NEXT_BUTTON_ON_CONFIRM_HELP));
		}

		/// <summary>
		/// Проверить, что есть подсказка к панели кнопок.
		/// </summary>
		public bool IsButtonBarHelpDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что есть подсказка к панели кнопок.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(NEXT_BUTTON_ON_BUTTON_BAR_HELP));
		}

		/// <summary>
		/// Проверить, что есть подсказка к инструментам обратной связи.
		/// </summary>
		public bool IsFeedbackHelpDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что есть подсказка к инструментам обратной связи.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(FINISH_BUTTON_ON_FEEDBACK_HELP));
		}

		/// <summary>
		/// Проверить, что закрылась подсказка к инструментам обратной связи.
		/// </summary>
		public bool IsFeedbackHelpDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что закрылась подсказка к инструментам обратной связи.");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(FINISH_BUTTON_ON_FEEDBACK_HELP));
		}

		/// <summary>
		/// Проверить, появились ли результаты поиска
		/// </summary>
		/// <param name="serachQuery"></param>
		/// <returns></returns>
		public bool IsDictionariesSearchResultsAppeared(string serachQuery)
		{
			CustomTestContext.WriteLine("Проверить, появились ли результаты поиска по запросу {0}", serachQuery);
			var elements = Driver.FindElements(By.XPath(SIDE_PANEL_DICTIONARIES_SEARCH_RESULTS.Replace("*#*", serachQuery)));

			return elements.Count > 0;
		}
		
		/// <summary>
		/// Проверить, что в таргете присутствует указанный текст.
		/// </summary>
		/// <param name="text">текст который должен содержаться в таргет сегменте</param>
		/// <param name="rowNumber">номер сегмента</param>
		public bool IsTargetContainsText(int rowNumber, string text)
		{
			CustomTestContext.WriteLine("Проверить, что в таргете - {0} присутствует указанный текст - {1}",
				rowNumber , text);

			return Driver.WaitUntilElementIsDisplay(
					By.XPath(TEXT_IN_TARGET_CELL.Replace("*#*", rowNumber.ToString()).Replace("*##*", text)));
		}

		public bool IsDictionariesTabOpened()
		{
			CustomTestContext.WriteLine("Проверить, открыта ли вкладка Dictionaries");

			return Driver.WaitUntilElementIsClickable(By.XPath(SIDE_PANEL_DICTIONARIES_SEARCH_BUTTON)) != null;
		}

		/// <summary>
		/// Проверить, появилось ли сообщение во вкладке 'Dictionaries', что слово не найдено
		/// </summary>
		public bool IsWordNotFoundInDictionaryMessageVisible()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение во вкладке 'Dictionaries', что слово не найдено");

			return Driver.WaitUntilElementIsDisplay(By.XPath(SIDE_PANEL_DICTIONARIES_WORD_NOT_FOUND));
		}

		/// <summary>
		/// Проверить, что прогресс-бар в редакторе отобразился.
		/// </summary>
		public bool IsProgressBarDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что прогресс-бар в редакторе отобразился.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(PROGRESS_BAR));
		}

		/// <summary>
		/// Проверить, что отобразился тултип прогресс-бара.
		/// </summary>
		public bool IsProgressBarToolTipDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отобразился тултип прогресс-бара.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(PROGRESS_BAR_TOOLTIP));
		}

		/// <summary>
		/// Проверить, что отобразился символ переноса строки.
		/// </summary>
		public bool IsLineBreakSymbolDisplayed(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что отобразился символ переноса строки в сегменте {0}.", segmentNumber);

			return Driver.WaitUntilElementIsDisplay(By.XPath(LINE_BREAK_SYMBOL.Replace("*#*", segmentNumber.ToString())));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = DONE_BUTTON)]
		protected IWebElement DoneButton { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_BTN)]
		protected IWebElement ConfirmButton { get; set; }

		[FindsBy(How = How.XPath, Using = PROGRESS_BAR)]
		protected IWebElement ProgressBar { get; set; }

		[FindsBy(How = How.XPath, Using = PROGRESS_BAR_INFO_STRING)]
		protected IWebElement ProgressBarInfoString { get; set; }

		[FindsBy(How = How.XPath, Using = PROGRESS_BAR_PERCENTS)]
		protected IWebElement ProgressBarPercents { get; set; }

		[FindsBy(Using = FIND_ERROR_BTN_ID)]
		protected IWebElement FindErrorButton { get; set; }

		[FindsBy(How = How.Id, Using = HOME_BUTTON)]
		protected IWebElement HomeButton { get; set; }

		[FindsBy(How = How.XPath, Using = FINISH_TUTORIAL_BUTTON)]
		protected IWebElement FinishTutorialButton { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON_ON_SOURCE_FIELD_HELP)]
		protected IWebElement NextButtonOnSourceFieldHelp { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON_ON_TARGET_FIELD_HELP)]
		protected IWebElement NextButtonOnTargetFieldHelp { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON_ON_CAT_PANEL_HELP)]
		protected IWebElement NextButtonOnCatPanelHelp { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON_ON_CONFIRM_HELP)]
		protected IWebElement NextButtonOnConfirmHelp { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON_ON_BUTTON_BAR_HELP)]
		protected IWebElement NextButtonOnButtonBarHelp { get; set; }

		[FindsBy(How = How.XPath, Using = FINISH_BUTTON_ON_FEEDBACK_HELP)]
		protected IWebElement FinishButtonOnFeedbackHelp { get; set; }

		[FindsBy(Using = DICTIONARY_BUTTON)]
		protected IWebElement SpellcheckDictionaryButton { get; set; }
		
		[FindsBy(How = How.XPath, Using = STAGE_NAME)]
		protected IWebElement StageName { get; set; }
		
		[FindsBy(How = How.Id, Using = LAST_CONFIRMED_BUTTON)]
		protected IWebElement LastUnconfirmedButton { get; set; }
		
		[FindsBy(How = How.XPath, Using = CHARACTER_BUTTON)]
		protected IWebElement CharacterButton { get; set; }
		
		[FindsBy(How = How.Id, Using = ADD_TERM_BUTTON)]
		protected IWebElement AddTermButton { get; set; }
		
		[FindsBy(How = How.Id, Using = CHARACTER_FORM)]
		protected IWebElement CharacterForm { get; set; }

		[FindsBy(How = How.Id, Using = INSERT_TAG_BUTTON)]
		protected IWebElement InsertTagButton { get; set; }

		[FindsBy(How = How.XPath, Using = TAG)]
		protected IWebElement Tag { get; set; }

		[FindsBy(How = How.XPath, Using = COPY_BUTTON)]
		protected IWebElement CopyButton { get; set; }

		[FindsBy(How = How.Id, Using = CONCORDANCE_BUTTON)]
		protected IWebElement ConcordanceButton { get; set; }

		[FindsBy(How = How.Id, Using = CONCORDANCE_SEARCH)]
		protected IWebElement ConcordanceSearch { get; set; }

		[FindsBy(How = How.XPath, Using = CONCORDANCE_SEARCH_RESULT)]
		protected IWebElement ConcordanceSearchResult { get; set; }

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

		[FindsBy(How = How.Id, Using = CHANGE_CASE_BUTTON)]
		protected IWebElement ChangeCaseButton { get; set; }

		[FindsBy(How = How.Id, Using = WORKFLOW_COLUMN)]
		protected IWebElement WorkflowColumn { get; set; }
		
		[FindsBy(How = How.XPath, Using = UNDO_BUTTON)]
		protected IWebElement UndoButton { get; set; }

		[FindsBy(How = How.XPath, Using = REDO_BUTTON)]
		protected IWebElement RedoButton { get; set; }

		[FindsBy(How = How.Id, Using = RESTORE_BUTTON)]
		protected IWebElement RestoreButton { get; set; }

		[FindsBy(How = How.Id, Using = REVISION_TAB)]
		protected IWebElement RevisionTab { get; set; }

		[FindsBy(How = How.Id, Using = REVISION_TABLE)]
		protected IWebElement RevisionTable { get; set; }

		[FindsBy(How = How.Id, Using = REVISION_LIST)]
		protected IWebElement RevisionList { get; set; }

		[FindsBy(How = How.XPath, Using = REVISION_DELETE_CHANGE_PART)]
		protected IWebElement RevisionDeleteChangePart { get; set; }

		[FindsBy(How = How.XPath, Using = REVISION_INSERT_CHANGE_PART)]
		protected IWebElement RevisionInsertChangePart { get; set; }

		[FindsBy(How = How.XPath, Using = REVISION_USER_COLUNM)]
		protected IWebElement RevisionUserColunm { get; set; }

		[FindsBy(How = How.XPath, Using = QA_CHECK_TAB)]
		protected IWebElement QACheckTab { get; set; }

		[FindsBy(How = How.XPath, Using = SEGMENTS_BODY)]
		protected IWebElement SegmentsTable { get; set; }

		[FindsBy(How = How.XPath, Using = QA_ERROR_TABLE)]
		protected IWebElement QAErrorTable { get; set; }

		[FindsBy(How = How.XPath, Using = CONCORDANCE_SEARCH_TEXT)]
		protected IWebElement ConcordanceSearchText { get; set; }
		
		protected IWebElement SegmentErrorLogo { get; set; }
		protected IWebElement Error { get; set; }
		[FindsBy(How = How.XPath, Using = CONCORDANCE_SEARCH_BUTTON)]
		protected IWebElement ConcordanceSearchButton { get; set; }

		[FindsBy(How = How.XPath, Using = SOURCE_RADIO_BUTTON)]
		protected IWebElement SourceRadiobutton { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_RADIO_BUTTON)]
		protected IWebElement TargetRadiobutton { get; set; }

		[FindsBy(How = How.XPath, Using = CONCORDANCE_SEARCH_RESULT_TABLE)]
		protected IWebElement ConcordanceSearchResultTable { get; set; }

		[FindsBy(How = How.XPath, Using = CONCORDANCE_SEARCH_TARGET_RESULT)]
		protected IWebElement ConcordanceSearchTargetResult { get; set; }

		[FindsBy(How = How.XPath, Using = CONCORDANCE_SEARCH_SOURCE_RESULT_BY_TARGET)]
		protected IWebElement ConcordanceSearchSourceResultByTarget { get; set; }

		[FindsBy(How = How.XPath, Using = SEGMENT_COMMENTS_TAB)]
		protected IWebElement SegmentCommentsTab { get; set; }

		[FindsBy(How = How.XPath, Using = DOCUMENT_COMMENTS_TAB)]
		protected IWebElement DocumentCommentsTab { get; set; }

		[FindsBy(How = How.XPath, Using = DOCUMENT_COMMENTS_TEXTAREA)]
		protected IWebElement DocumentCommentsTextarea { get; set; }

		[FindsBy(How = How.XPath, Using = SEGMENTS_COMMENTS_TEXTAREA)]
		protected IWebElement SegmentCommentsTextarea { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_COMMENT_BUTTON)]
		protected IWebElement DeleteCommentButton { get; set; }

		[FindsBy(How = How.XPath, Using = SEND_BUTTON)]
		protected IWebElement SendButton { get; set; }

		[FindsBy(How = How.XPath, Using = SEGMENTS_SEND_BUTTON)]
		protected IWebElement SegmentSendButton { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_TERMINOLOGY_ERROR_ARROW)]
		protected IWebElement NextTerminologyErrorArrow { get; set; }
		
		[FindsBy(How = How.XPath, Using = SOURCE_FILTER_FIELD)]
		protected IWebElement SourceFilterField { get; set; }

		[FindsBy(How = How.XPath, Using = PREVIOUS_TERMINOLOGY_ERROR_ARROW)]
		protected IWebElement PreviousTerminologyErrorArrow { get; set; }
		
		[FindsBy(How = How.XPath, Using = TARGET_FILTER_FIELD)]
		protected IWebElement TargetFilterField { get; set; }

		[FindsBy(How = How.XPath, Using = REPLACE_MENU_BUTTON)]
		protected IWebElement ReplaceMenuButton { get; set; }

		[FindsBy(How = How.XPath, Using = REPLACE_BUTTON)]
		protected IWebElement ReplaceButton { get; set; }

		[FindsBy(How = How.XPath, Using = REPLACE_ALL_BUTTON)]
		protected IWebElement ReplaceAllButton { get; set; }

		[FindsBy(How = How.XPath, Using = REPLACE_FIELD)]
		protected IWebElement ReplaceField { get; set; }

		[FindsBy(How = How.XPath, Using = CLOSE_MESSAGE_WITH_CRITICAL_ERROR_BUTTON)]
		protected IWebElement CloseCriticalErrorMessageButton { get; set; }
		
		[FindsBy(How = How.XPath, Using = USER_PREF_BTN)]
		protected IWebElement UserPreferencesButton { get; set; }

		[FindsBy(How = How.XPath, Using = SIDE_PANEL_DICTIONARIES_TAB)]
		protected IWebElement DictionariesTab { get; set; }

		[FindsBy(How = How.XPath, Using = SIDE_PANEL_DICTIONARIES_SEARCH_INPUT)]
		protected IWebElement DictionariesSearchInput { get; set; }

		[FindsBy(How = How.XPath, Using = SIDE_PANEL_DICTIONARIES_SEARCH_BUTTON)]
		protected IWebElement DictionariesSearchButton { get; set; }

		[FindsBy(How = How.XPath, Using = SIDE_PANEL_DICTIONARIES_TRANSLATION_DIRECTION_REF)]
		protected IWebElement DictionariesTranslationDirectionSwitch { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_IN_LINGVO_DICTIONARIES)]
		protected IWebElement SearchInLingvoDictionariesButton { get; set; }

		[FindsBy(How = How.XPath, Using = SIDE_PANEL_DICTIONARIES_TRANSLATION_DIRECTION)]
		protected IWebElement DictionariesTranslationDirection { get; set; }

		[FindsBy(How = How.XPath, Using = SIDE_PANEL_DICTIONARIES_OPEN_IN_NEW_TAB_LINK)]
		protected IWebElement OpenTranslationInNewTabLink { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_LINE_BREAK_BUTTON)]
		protected IWebElement AddLineBreakButton { get; set; }

		protected IWebElement VoteUpDisabledButton { get; set; }
		protected IWebElement ProgressBarWidth { get; set; }
		protected IWebElement TextInTarget { get; set; }
		protected IWebElement Comment { get; set; }
		protected IWebElement CommentCell { get; set; }
		protected IWebElement VoteDownButton { get; set; }
		protected IWebElement VoteUpButton { get; set; }
		protected IWebElement VoteCount{ get; set; }
		protected IWebElement SourceCell { get; set; }
		protected IWebElement SegmentTranslationUserColumn { get; set; }
		protected IWebElement Revision { get; set; }
		protected IWebElement DeleteChangedPart { get; set; }
		protected IWebElement InsertChangedPart { get; set; }
		protected IWebElement User { get; set; }
		protected IWebElement Type { get; set; }
		protected IWebElement QAError { get; set; }
		protected IWebElement InsertResource { get; set; }
		protected IWebElement TargetCell { get; set; }
		protected IWebElement TargetCellValue { get; set; }
		protected IWebElement DictionariesSearchResults { get; set; }
		protected IWebElement ConfirmTick { get; set; }
		protected IWebElement LineBreakSymbol { get; set; }


		protected IWebElement RepetitionArrow { get; set; }

		#endregion

		#region Описание XPath элементов страницы

		protected const string DONE_BUTTON = "//a[contains(@aria-disabled, 'false')]//span[contains(text(), 'Done')]";
		protected const string CONFIRM_BTN = "//a[contains(@class, 'confirm-btn')]";
		protected const string ADD_LINE_BREAK_BUTTON = "//*[@id='tag-insert-btn']";
		protected const string FIND_ERROR_BTN_ID = "qa-error-btn";
		protected const string FINISH_TUTORIAL_BUTTON = "//span[contains(text(),'Finish')]";
		protected const string AUTOSAVING = "//div[contains(text(), 'Saving')]";
		protected const string SPELLCHECK_PATH = "//div[contains(text(), '1')]/ancestor::tr//span[contains(@class,'spellcheck') and contains(string(), '*#*')]";
		protected const string REVISION_PATH = "//div[@id='revisions-body']//table[1]//td[contains(@class,'revision-type-cell')]//div[text()='*#*']";
		protected const string STAGE_NAME = "//h1/span[contains(@class, 'workflow')]";
		protected const string LAST_CONFIRMED_BUTTON = "unfinished-btn";
		protected const string ADD_TERM_BUTTON = "add-term-btn";
		protected const string SELECTED_SEGMENT = "//div[contains(@id, 'center-body')]//div[contains(@class, 'x-grid-item-container')]//table[contains(@class, 'x-grid-item-selected')]//td//div[contains(text(), '*#*')]";
		protected const string CHARACTER_BUTTON = "//a[@data-qtip='Insert Special Character (Ctrl+Shift+I)']";
		protected const string INSERT_TAG_BUTTON = "tag-insert-btn";
		protected const string COPY_BUTTON = "//span[contains(@id, 'copysourcebutton')]";
		protected const string CONCORDANCE_BUTTON = "concordance-search-btn";
		protected const string CONCORDANCE_SEARCH_TEXT = "//input[contains(@id,'smartcatsearchfield')]";
		protected const string CONCORDANCE_SEARCH_RESULT_TABLE = "//div[@id='concordance-search-body']//table[contains(@id, 'tableview')]";
		protected const string CONCORDANCE_SEARCH_RESULT = "//div[@id='concordance-search-body']//table[contains(@id, 'tableview')]//tr//td[2]";
		protected const string CONCORDANCE_SEARCH_SOURCE_RESULT = "//div[@id='concordance-search-body']//table[contains(@id, 'tableview')]//div[contains(@class, 'cell-inner')]/div//span";
		protected const string CONCORDANCE_SEARCH_SOURCE_RESULT_BY_TARGET = "//div[@id='concordance-search-body']//table[contains(@id, 'tableview')]//div[contains(@class, 'cell-inner')]/div";
		protected const string CONCORDANCE_SEARCH_TARGET_RESULT = "(//div[@id='concordance-search-body']//table[contains(@id, 'tableview')]//div[contains(@class, 'cell-inner')]//div)[2]";
		protected const string SOURCE_RADIO_BUTTON = "//label[text()='Source']/..//input";
		protected const string TARGET_RADIO_BUTTON = "//label[text()='Target']/..//input";
		protected const string ROLLBACK_BUTTON = "step-rollback-btn";
		protected const string HOME_BUTTON = "back-btn";
		protected const string DICTIONARY_BUTTON = "dictionary-btn";
		protected const string CHANGE_CASE_BUTTON = "change-case-btn";
		protected const string UNDO_BUTTON = "//a[contains(@id, 'undo') and @aria-disabled='false']";
		protected const string REDO_BUTTON = "//a[contains(@id, 'redo') and @aria-disabled='false']";

		protected const string ROW_NUMBER_ACTIVE_XPATH = ".//div[@id='segments-body']//table//td[contains(@class, 'x-grid-item-focused')]/../td[1]//div[contains(@class, 'row-numberer')]";

		protected const string SEGMENTS_TABLE_XPATH = "//div[@id='segments-body']//div//div[2]//table";
		protected const string SEGMENTS_BODY = "//div[@id='segments-body']//table";
		protected const string CONFIRMED_ICO = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//div[contains(@class,'x-segment-target-info')]//span[contains(@class, 'confirmed')]";
		protected const string TARGET_CELL = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[contains(@class, ' test-segment-target')]//div//div[contains(@id,'segmenteditor')]/parent::div";
		protected const string CROWD_CONFIRMED_ICO = "//table[@data-recordindex = '*#*']//div[contains(@class, 'crowd-status-confirmed')]";
		protected const string TEXT_IN_TARGET_CELL = "//div[contains(@id, 'segments-body')]//table[*#*]//td[contains(@class, 'x-targeteditor-default-cell x-grid-item-focused')]//p[contains(text(), '*##*')]";
		protected const string TARGET_CELL_VALUE = "//table[@data-recordindex='*#*']//td[contains(@class, ' test-segment-target')]//div[contains(@id, 'segmenteditor')]";
		protected const string SOURCE_CELL = "//table[@data-recordindex='*#*']//td[contains(@class, 'test-segment-source')]//div[contains(@id, 'segmenteditor')]";
		protected const string TAG = "//div[contains(@id, 'targeteditor')][*#*]//img[contains(@class,'tag')]";
		protected const string SEGMENT_LOCK = "//div[contains(text(), '*#*')]//..//..//..//div[contains(@class,'lock')][not(contains(@class,'inactive'))]";
		protected const string SEGMENT_ERROR_LOGO = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//div[contains(@class,'x-segment-target-info')]//span[contains(@class, 'errors')]/ancestor::a";
		protected const string REPETITION_ARROW = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//div[contains(@class,'repetition')]";
		protected const string REPETITION_ARROW_EXCLUDED = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//div[contains(@class,'repetition-exclude')]";
		protected const string CONFIRM_TICK = "//table[*#*]//div[@data-qtip='Save Translation (Enter)']";
		protected const string LINE_BREAK_SYMBOL = "//table[*#*]//td[3][contains(@class, 'segmenteditor')]//img[@class='tag tag-srt']";

		protected const string CHARACTER_FORM = "charmap";
		protected const string CONCORDANCE_SEARCH= "concordance-search";
		protected const string CONCORDANCE_SEARCH_BUTTON = "(//span[text()='Search'])[2]";

		protected const string ALL_SEGMENTS_SAVED_STATUS = "//div[text()='All segments are saved.']";
		protected const string SAVING_STATUS = "//divc[contains(@id, 'segmentsavingindicator') and contains(text(),'Saving')]";
		protected const string MATCH_COLUMN = "//div[@id='segments-body']//table[*#*]//tbody//div[contains(@class,'insert-resource')]";
		protected const string TARGET_MATCH_COLUMN_PERCENT = "//table[@data-recordindex='*#*' and contains(@id, 'tableview')]//span[contains(@class, 'match-percentage')]";
		protected const string CAT_PANEL_PERCENT_MATCH = ".//div[@id='cat-body']//table[*#*]//td[3]//div[contains(@class, 'x-match-percentage')]";
		protected const string MT_SOURCE_TEXT_IN_CAT_PANEL = ".//div[@id='cat-body']//table//tbody//tr//div[text()='MT']//ancestor::tr//preceding-sibling::td[contains(@class, 'test-cat-source')]/div";
		protected const string CAT_TYPE_LIST_IN_PANEL = ".//div[@id='cat-body']//table//td[3]/div";
		protected const string TARGET_CAT_TRANSLATION = ".//div[@id='cat-body']//table[*#*]//td[contains(@class, 'test-cat-target')]/div";
		protected const string SOURCE_CAT_TRANSLATION = ".//div[@id='cat-body']//table[*#*]//td[contains(@class, 'test-cat-source')]/div";
		protected const string CAT_TYPE = ".//div[@id='cat-body']//table//div[contains(text(),'*#*')]";
		protected const string CAT_SOURCE = ".//div[@id='cat-body']//table//td[contains(@class, 'test-cat-source')]/div[text()='*#*']";

		protected const string PERCENT_COLOR = "//table[@data-recordindex='*#*' and contains(@id, 'tableview')]//span[contains(@class, 'match-percentage')]";

		protected const string TERM_SAVED_MESSAGE = ".//div[text()='The term has been saved.']";

		protected const string EXISTING_TERM_MESSAGE = "//div[contains(@id, 'messagebox') and contains(string(), 'This glossary already contains term(s)')]";
		protected const string СONFIRM_YES_BTN = "//div[contains(@id, 'messagebox')]//span[contains(string(), 'Yes')]";
		protected const string CAT_TABLE = ".//div[@id='cat-body']//table";

		protected const string WORKFLOW_COLUMN = "//td[contains(@class, 'segmentworkflowcolumn')]";

		protected const string HIGHLIGHTED_SEGMENT = "//*[@id='segments-body']//div//div//table[*#*]//td[2]//div//span";
		protected const string PROGRESS_BAR = "//div[text()='Progress']//parent::div//div[@class='x-progress-ct-default']";
		protected const string PROGRESS_BAR_INFO_STRING = "//div[contains(@class, 'workflow-progress-tip') and contains(@aria-hidden, 'false')]//table//td[4]";
		protected const string PROGRESS_BAR_TOOLTIP = "//div[contains(@class, 'workflow-progress-tip') and contains(@aria-hidden, 'false')]";
		protected const string PROGRESS_BAR_PERCENTS = "//div[contains(@class, 'x-progress-bar x-progress-bar-default')]";
		protected const string SOURCE_CAT_TERMS = ".//div[@id='cat-body']//table//tbody//tr//td[2]//div";

		protected const string RESTORE_BUTTON = "revision-rollback-btn";
		protected const string REVISION_TAB = "revisions-tab";
		protected const string REVISION_TABLE = "revisions-body";
		protected const string REVISION_LIST = "//div[@id='revisions-body']//table";
		protected const string REVISION_IN_LIST = "//div[@id='revisions-body']//table[*#*]//td[2]//div//pre[contains(text(),'*##*')]";
		protected const string REVISION_TYPE = "//div[@id='revisions-body']//table[*#*]//td[contains(@class,'revision-type-cell')]";
		protected const string REVISION_DELETE_CHANGE_PART = "//div[@id='revisions-body']//table[*#*]//td[contains(@class,'revision-text-cell')]//del";
		protected const string REVISION_INSERT_CHANGE_PART = "//div[@id='revisions-body']//table[*#*]//td[contains(@class,'revision-text-cell')]//ins";
		protected const string REVISION_USER_COLUNM = "//div[@id='revisions-body']//table[*#*]//td[contains(@class,'revision-user-cell')]";
		protected const string USER_COLUMN = "//div[@id='gridcolumn-1105']//span";
		protected const string SEGMENT_TRANSLATION_USER_COLUMN = ".//div[@id='translations-body']//table//td[3]//div[contains(text(), '*#*')]/../..//td[2]";
		protected const string USER_PREF_BTN = "//span[contains(@class, 'sci-settings')]";
		protected const string EDITOR_DIALOG_BACKGROUND = "//div[contains(@class,'x-mask callout-mask')]";

		protected const string VOTE_DOWN_BUTTON = "//div[@id='translations-body']//tbody//div[contains(text(), '*#*')]//../following-sibling::td//div[contains(text(), '*##*')]//../following-sibling::td//span[contains(@class,'minus')]";
		protected const string VOTE_UP_BUTTON = "//div[@id='translations-body']//tbody//div[contains(text(), '*#*')]//../following-sibling::td//div[contains(text(), '*##*')]//../following-sibling::td//span[contains(@class,'plus')]";
		protected const string VOTE_COUNT = "//div[@id='translations-body']//tbody//div[contains(text(), '*#*')]/ancestor::td//following-sibling::td//div[contains(text(), '*##*')]//ancestor::td//following-sibling::td//span[@class='rating-count']";
		protected const string VOTE_DOWN_BUTTON_INACTIVE = "//div[@id='translations-body']//tbody//div[contains(text(), '*#*')]/ancestor::td//following-sibling::td//div[contains(text(), '*##*')]/ancestor::td//following-sibling::td//span[contains(@class,'minus')]";
		protected const string VOTE_UP_BUTTON_INACTIVE = "//div[@id='translations-body']//tbody//div[contains(text(), '*#*')]/ancestor::td//following-sibling::td//div[contains(text(), '*##*')]/ancestor::td//following-sibling::td//span[contains(@class,'plus')]";

		protected const string MESSAGE_WITH_CRITICAL_ERROR = "//div[contains(text(),'contains critical errors.')]";
		protected const string CLOSE_MESSAGE_WITH_CRITICAL_ERROR_BUTTON = "//div[contains(text(),'contains critical errors.')]/../../..//div[@data-qtip='Close panel']";
		protected const string ERRORS_POPUP = "//div[contains(text(),'Translation errors')]";
		protected const string ERROR = "//h3[contains(text(),'Translation errors')]/../..";

		protected const string QA_CHECK_TAB = "//span[contains(@id, 'errors-tab')]//span[contains(text(), 'QA')]";
		protected const string QA_ERROR = "//div[@id='errors-body']//tr[*#*]//td[2]//div";
		protected const string QA_ERROR_TABLE = "//div[@id='errors-body']//table";
		protected const string TERM_ERROR = "//div[contains(text(),'No translation of the source term') and contains(text(), '*#*')]";

		protected const string NEXT_TERMINOLOGY_ERROR_ARROW = "//div[contains(text(),'No translation of the source term') and contains(text(), '*#*')]/../..//div[contains(@class, 'next-error-action')]";
		protected const string PREVIOUS_TERMINOLOGY_ERROR_ARROW = "//div[contains(text(),'No translation of the source term') and contains(text(), '*#*')]/../..//div[contains(@class, 'prev-error-action')]";
		protected const string SEGMENT_COMMENTS_TAB = "//a[@id='segment-comments-tab']";
		protected const string DOCUMENT_COMMENTS_TAB = "//a[@id='document-comments-tab']";
		protected const string DOCUMENT_COMMENTS_TEXTAREA = "//div[contains(@class, 'comments')]//textarea";
		protected const string SEGMENTS_COMMENTS_TEXTAREA = "//div[contains(@id,'segment-comments')]//textarea";
		protected const string SEGMENTS_SEND_BUTTON = "//div[contains(@id,'segment-comments')]//span[text()='Send']";
		protected const string SEND_BUTTON = "//div[contains(@class, 'comments')]//span[text()='Send']";
		protected const string COMMENT = "//div[text()='*#*']/../..//div[text()= '*##*']";
		protected const string COMMENT_CELL = "//div[text()='*#*']";
		protected const string DELETE_COMMENT_BUTTON = "//div[@data-qtip='Delete' and @role='button']";

		protected const string SOURCE_FILTER_FIELD = "//input[@name='source-text']";
		protected const string TARGET_FILTER_FIELD = "//input[@name='target-text']";
		protected const string VISIBLE_SEGMENTS_COUNT = "//div[@class='number']";
		protected const string REPLACE_MENU_BUTTON = "//span[@class='x-btn-icon-el x-btn-icon-el-default-medium x-sci sci-replace ']";
		protected const string REPLACE_FIELD = "//input[@name='replace-text']";
		protected const string REPLACE_BUTTON = "//span[text()='Replace']";
		protected const string REPLACE_ALL_BUTTON = "//span[text()='Replace All']";
		protected const string REPLACE_NOTIFICATION = "//span[text()='Undo replacement']";

		protected const string NEXT_BUTTON_ON_SOURCE_FIELD_HELP = "//div[text()='Source Column']//ancestor::div[4]//following-sibling::div//span[text()='Next']";
		protected const string NEXT_BUTTON_ON_TARGET_FIELD_HELP = "//div[text()='Target Field']//ancestor::div[4]//following-sibling::div//span[text()='Next']";
		protected const string NEXT_BUTTON_ON_CAT_PANEL_HELP = "//div[text()='CAT Pane']//ancestor::div[4]//following-sibling::div//span[text()='Next']";
		protected const string NEXT_BUTTON_ON_CONFIRM_HELP = "//div[text()='Confirm Button']//ancestor::div[4]//following-sibling::div//span[text()='Next']";
		protected const string NEXT_BUTTON_ON_BUTTON_BAR_HELP = "//div[text()='Button Bar']//ancestor::div[4]//following-sibling::div//span[text()='Next']";
		protected const string FINISH_BUTTON_ON_FEEDBACK_HELP = "//div[text()='Feedback']//ancestor::div[4]//following-sibling::div//span[text()='Finish']";

		protected const string SIDE_PANEL_DICTIONARIES_TAB = "//span[text()='Dictionaries']//ancestor::span//ancestor::a";
		protected const string SIDE_PANEL_DICTIONARIES_SEARCH_INPUT = "//div[@id='lingvo-search-body']//input[@name='searchText']";
		protected const string SIDE_PANEL_DICTIONARIES_SEARCH_BUTTON = "//div[@id='lingvo-search-body']//span[text()='Search']//ancestor::span//ancestor::a";
		protected const string SIDE_PANEL_DICTIONARIES_SEARCH_RESULTS = "//div[@id='lingvo-search-body']//h2//span[@class='Bold' and text()='*#*']";
		protected const string SIDE_PANEL_DICTIONARIES_TRANSLATION_DIRECTION_REF = "//div[@id='lingvo-search-body']//label[contains(@id,'lingvosearchdirection')]";
		protected const string SIDE_PANEL_DICTIONARIES_OPEN_IN_NEW_TAB_LINK = "//div[@id='lingvo-search-body']//a[contains(@href, 'Translate?searchSrcLang')]";
		protected const string SIDE_PANEL_DICTIONARIES_WORD_NOT_FOUND = "//div[@id='lingvo-search-body']//span[@class='not-found']";
		protected const string SIDE_PANEL_DICTIONARIES_TRANSLATION_DIRECTION = "//div[@id='lingvo-search-innerCt']//label[@data-ref='boxLabelEl']";

		protected const string SEARCH_IN_LINGVO_DICTIONARIES = "//a[@id='lingvo-lookup-btn']";
		protected const string INSERT_RESOURCE = "//table[*#*]//div[@class='insert-resource']";
		#endregion
	}
}
