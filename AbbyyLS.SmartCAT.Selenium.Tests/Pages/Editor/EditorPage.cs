using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Keys = OpenQA.Selenium.Keys;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
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
			if (!IsEditorDialogBackgroundDesappeared())
			{
				CloseTutorial();
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку "Домой" для перехода на страницу проекта
		/// </summary>
		public ProjectSettingsPage ClickHomeButtonExpectingProjectSettingsPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Домой' для перехода на страницу проекта.");
			HomeButton.Click();

			return new ProjectSettingsPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Домой" для перехода на страницу проектов
		/// </summary>
		public ProjectsPage ClickHomeButtonExpectingProjectsPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Домой' для перехода на страницу проектов.");
			HomeButton.Click();

			return new ProjectsPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Домой" для перехода на страницу курсов курсеры.
		/// </summary>
		public CoursesPage ClickHomeButtonExpectingCourseraCoursesPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Домой' для перехода на страницу курсов курсеры.");
			HomeButton.Click();

			return new CoursesPage(Driver).GetPage();
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
			Driver.WaitUntilElementIsDisplay(By.XPath(ALL_SEGMENTS_SAVED_STATUS));

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
			TargetCell.JavaScriptClick();

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
		/// Нажать кнопку отмены.
		/// </summary>
		public EditorPage ClickUndoButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку отмены.");
			Driver.WaitUntilElementIsDisplay(By.XPath(UNDO_BUTTON));
			UndoButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку возврата.
		/// </summary>
		public EditorPage ClickRedoButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку возврата.");
			RedoButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать хоткей кнопки отмены Ctrl Z.
		/// </summary>
		public EditorPage PressUndoHotkey()
		{
			CustomTestContext.WriteLine("Нажать хоткей кнопки отмены Ctrl Z.");
			Driver.WaitUntilElementIsDisplay(By.XPath(UNDO_BUTTON));
			Driver.SendHotKeys("z", control: true);

			return GetPage();
		}

		/// <summary>
		/// Нажать хоткей кнопки возврата Ctrl Y.
		/// </summary>
		public EditorPage PressRedoHotkey()
		{
			CustomTestContext.WriteLine("Нажать хоткей кнопки возврата Ctrl Y.");
			Driver.SendHotKeys("y", control: true);

			return GetPage();
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
		/// Нажать кнопку 'Add Term'
		/// </summary>
		public AddTermDialog ClickAddTermButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Add Term'");
			AddTermButton.Click();

			return new AddTermDialog(Driver).GetPage();
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
		/// Нажать хоткей Shift F3 для изменения регистра.
		/// </summary>
		public EditorPage PressChangeCaseHotKey()
		{
			CustomTestContext.WriteLine("Нажать хоткей Shift F3 для изменения регистра.");
			Driver.SendHotKeys(Keys.F3, shift: true);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку изменения регистра.
		/// </summary>
		public EditorPage ClickChangeCaseButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку изменения регистра.");
			ChangeCaseButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Restore.
		/// </summary>
		public EditorPage ClickRestoreButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Restore.");
			RestoreButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить процентр прогресс бара
		/// </summary>
		public float GetPercentInProgressBar()
		{
			CustomTestContext.WriteLine("Получить процентр прогресс бара.");
			var percentValue = ProgressBar.GetAttribute("style").Split(new [] { ':', '%' });
			float result;

			try
			{
				result = Convert.ToSingle(percentValue[1], new CultureInfo("en-US"));
			}
			catch (Exception)
			{
				throw new Exception("Произошла ошибка:\n не удалось преобразование процента прогресс бара в число.");
			}

			return result;
		}

		/// <summary>
		/// Кликнуть по вкладке ревизий
		/// </summary>
		public EditorPage ClickRevisionTab()
		{
			CustomTestContext.WriteLine("Кликнуть по вкладке ревизий");
			RevisionTab.Click();

			return GetPage();
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
		/// <param name="revisionNumber">номер ревизии</param>
		public string GetSegmentTranslationUserName(int revisionNumber = 1)
		{
			CustomTestContext.WriteLine("Получить имя пользователя на вкладке 'Segment translation'.");
			SegmentTranslationUserColumn = Driver.SetDynamicValue(How.XPath, SEGMENT_TRANSLATION_USER_COLUMN, revisionNumber.ToString());
			
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
			
			return GetPage();
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

			return GetPage();
		}

		/// <summary>
		/// Прокрутить страницу до перевода в кат панели
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage ScrollToTranslationInCatPanel(int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Прокрутить страницу до перевода в сегменте №{0} в кат панели.", rowNumber);
			var cat = Driver.SetDynamicValue(How.XPath, CAT_TRANSLATION, rowNumber.ToString());
			cat.Scroll();

			return GetPage();
		}

		/// <summary>
		/// Навести курсор на перевод в кат панели
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorPage HoverToTranslationInCatPanel(int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Навести курсор на перевод сегмента №{0} в кат панели.", rowNumber);
			var cat = Driver.SetDynamicValue(How.XPath, CAT_TRANSLATION, rowNumber.ToString());
			cat.HoverElement();

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Двойной клик по переводу в CAT-панели
		/// </summary>
		/// <param name="rowNumber">номер строки в CAT-панели</param>
		public EditorPage DoubleClickCatPanel(int rowNumber)
		{
			ScrollToTranslationInCatPanel(rowNumber);
			// Sleep не убирать, без него не скролится
			Thread.Sleep(1000);
			HoverToTranslationInCatPanel(rowNumber);
			
			CustomTestContext.WriteLine("Двойной клик по строке №{0} с переводом в CAT-панели.", rowNumber);

			var cat = Driver.SetDynamicValue(How.XPath, CAT_TRANSLATION, rowNumber.ToString());
			cat.DoubleClick();

			return GetPage();
		}

		/// <summary>
		/// Получить текст из таргет сегмента
		/// </summary>
		/// <param name="rowNumber">номер строки сегмента</param>
		public string GetTargetText(int rowNumber)
		{
			CustomTestContext.WriteLine("Получить текст из таргет сегмента №{0}.", rowNumber);
			ScrollToTarget(rowNumber);
			TargetCell = Driver.SetDynamicValue(How.XPath, TARGET_CELL, (rowNumber - 1).ToString());
			TargetCell.Click();

			return TargetCell.Text.Trim();
		}

		/// <summary>
		/// Получить список подсвеченных в сегменте слов
		/// </summary>
		/// <param name="segmentNumber">Номер сегмента</param>
		/// <returns>Список подсвеченных в сегменте слов</returns>
		public List<string> GetHighlightedWords(int segmentNumber)
		{
			CustomTestContext.WriteLine("Получить список подсвеченных в сегменте №{0} слов.", segmentNumber);
			var highlightedWords = new List<string>();

			Driver.SetDynamicValue(How.XPath, SOURCE_CELL, (segmentNumber - 1).ToString()).ScrollAndClick();
			var segmentCatSelectedList = Driver.GetElementList(By.XPath(HIGHLIGHTED_SEGMENT.Replace("*#*", segmentNumber.ToString())));

			if (segmentCatSelectedList.Count > 0)
			{
				highlightedWords.AddRange(segmentCatSelectedList.Select(item => item.Text.ToLower()));
			}

			highlightedWords.Sort();

			return highlightedWords;
		}
		
		/// <summary>
		/// Открыть вкладку ревизий
		/// </summary>
		public EditorPage OpenRevisionTab()
		{
			CustomTestContext.WriteLine("Открыть вкладку ревизий");
			if (!IsRevisionTableDisplayed())
			{
				ClickRevisionTab();
			}

			return GetPage();
		}

		/// <summary>
		/// Выделение части строки до первого пробела Home+Shift+Right
		/// </summary>
		/// <param name="text">текст</param>
		public EditorPage SelectWordPartBeforeSpaceByHotkey(string text)
		{
			CustomTestContext.WriteLine("Выделение части строки до первого пробела Home+Shift+Right.");
			Driver.SendHotKeys(Keys.Home);
			var array = text.Split(' ');
			for (int i = 0; i <= array[0].Length; i++)
			{
				Driver.SendHotKeys(Keys.Right, shift: true);
			}

			return GetPage();
		}

		/// <summary>
		/// Нажать хоткей выделения последнего слова Ctrl+Shift+Left
		/// </summary>
		public EditorPage SelectLastWordByHotkey(int segmentNumber)
		{
			CustomTestContext.WriteLine("Нажать хоткей выделения последнего слова в таргете Ctrl+Shift+Left. Номер строки: {0}", segmentNumber);
			ClickOnTargetCellInSegment(segmentNumber);
			Driver.SendHotKeys(Keys.Left, control: true, shift: true);

			return GetPage();
		}

		/// <summary>
		/// Нажать хоткей выделения нескольких символов Ctrl+Left, Ctrl+Right, symbolsCount+Shift.
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <param name="symbolsCount">количество символов</param>
		public EditorPage SelectFewSymbolsInLastWordByHotkey(int segmentNumber, int symbolsCount)
		{
			CustomTestContext.WriteLine("Нажать хоткей выделения {0} символов в последнем слове. Номер строки: {1}", symbolsCount, segmentNumber);
			Driver.SendHotKeys(Keys.Left, control: true);
			Driver.SendHotKeys(Keys.ArrowRight);
			for (int i = 0; i < symbolsCount; i++)
			{
				Driver.SendHotKeys(Keys.Right, shift: true);
			}
			
			return GetPage();
		}

		/// <summary>
		/// Нажать хоткей выделения второго и третьего слов Ctrl+Home, Ctrl+Right, Ctrl+Shift+Right, Ctrl+Shift+Right, Ctrl+Right.
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public EditorPage SelectSecondThirdWordsByHotkey(int segmentNumber)
		{
			CustomTestContext.WriteLine("Нажать хоткей выделения второго и третьего слов. Номер строки: {0}", segmentNumber);
			ClickOnTargetCellInSegment(segmentNumber);
			Driver.SendHotKeys(Keys.Home);
			Driver.SendHotKeys(Keys.Right, control: true);
			Driver.SendHotKeys(Keys.Right,control: true, shift: true);
			Driver.SendHotKeys(Keys.Right, control: true, shift: true);
			
			return GetPage();
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
			Driver.SendHotKeys(Keys.Home, control: true, shift: true);

			return GetPage();
		}

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
			TargetCell.JavaScriptClick();

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

			if (!Driver.WaitUntilElementIsAppear(By.XPath(CAT_TYPE.Replace("*#*", catType.ToString()))))
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

		/// <summary>
		/// Вставить перевод из CAT-панели с помощью хоткея Ctrl + "номер подстановки"
		/// </summary>
		/// <param name="catType">CAT-тип</param>
		/// <param name="targetRowNumber">номер строки таргета</param>
		public EditorPage PasteTranslationFromCATByHotkey(CatType catType, int targetRowNumber = 1)
		{
			ClickOnTargetCellInSegment(targetRowNumber);
			
			var catRowNumber = CatTypeRowNumber(catType);

			Driver.SendHotKeys(catRowNumber.ToString(), control: true);

			if (GetTargetText(targetRowNumber) != GetCatTranslationText(catRowNumber))
			{
				throw new Exception("Текст из таргет сегмента совпадает с текстом перевода из CAT-панели");
			}

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что все термины сохранены
		/// </summary>
		public bool IsAllSegmentsSavedMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что все термины сохранены.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ALL_SEGMENTS_SAVED_STATUS));
		}

		/// <summary>
		/// Проверить, что отображается таблица ревизий.
		/// </summary>
		private bool IsRevisionTableDisplayed()
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
		/// <param name="type">типа</param>
		public bool IsCatTypeExist(CatType type)
		{
			CustomTestContext.WriteLine("Проверить, что подстановка типа {0} есть в CAT-панели", type);

			return Driver.GetIsElementExist(By.XPath(CAT_TYPE.Replace("*#*", type.ToString())));
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
		/// Проверить, закрылся ли фон диалога в редакторе
		/// </summary>
		public bool IsEditorDialogBackgroundDesappeared()
		{
			return Driver.WaitUntilElementIsDisappeared(By.XPath(EDITOR_DIALOG_BACKGROUND), 5);
		}

		/// <summary>
		/// Проверить, открылся ли редактор
		/// </summary>
		public bool IsEditorPageOpened()
		{
			return IsSavingStatusDisappeared() &&
				Driver.WaitUntilElementIsDisplay(By.XPath(SEGMENTS_BODY), timeout: 60);
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

			return Driver.WaitUntilElementIsAppear(By.XPath(TERM_SAVED_MESSAGE));
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

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CONFIRM_BTN)]
		protected IWebElement ConfirmButton { get; set; }

		[FindsBy(How = How.XPath, Using = PROGRESS_BAR)]
		protected IWebElement ProgressBar { get; set; }

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

		[FindsBy(How = How.Id, Using = CHANGE_CASE_BUTTON)]
		protected IWebElement ChangeCaseButton { get; set; }

		[FindsBy(How = How.Id, Using = WORKFLOW_COLUMN)]
		protected IWebElement WorkflowColumn { get; set; }
		
		[FindsBy(How = How.XPath, Using = UNDO_BUTTON)]
		protected IWebElement UndoButton { get; set; }

		[FindsBy(How = How.Id, Using = REDO_BUTTON)]
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

		[FindsBy(How = How.XPath, Using = SEGMENTS_BODY)]
		protected IWebElement SegmentsTable { get; set; }
		protected IWebElement SegmentTranslationUserColumn { get; set; }
		protected IWebElement Revision;
		protected IWebElement DeleteChangedPart;
		protected IWebElement InsertChangedPart;
		protected IWebElement User;
		protected IWebElement Type;
		#endregion

		#region Описание XPath элементов страницы

		protected const string CONFIRM_BTN = "//a[@id='confirm-btn']";
		protected const string FIND_ERROR_BTN_ID = "qa-error-btn";
		protected const string FINISH_TUTORIAL_BUTTON = "//span[contains(text(),'Finish')]";
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
		protected const string CHANGE_CASE_BUTTON = "change-case-btn";
		protected const string UNDO_BUTTON = "//a[@id='undo-btn' and @aria-disabled='false']";
		protected const string REDO_BUTTON = "redo-btn-btnEl";

		protected const string ROW_NUMBER_ACTIVE_XPATH = ".//div[@id='segments-body']//table//td[contains(@class, 'x-grid-item-focused')]/../td[1]//div[contains(@class, 'row-numberer')]";
	
		protected const string SEGMENTS_TABLE_XPATH = "//div[@id='segments-body']//div//div[2]//table";
		protected const string SEGMENTS_BODY = "//div[@id='segments-body']//table";
		protected const string CONFIRMED_ICO = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[contains(@class,'info-cell')]//div[contains(@class,'sci-check')]";
		protected const string TARGET_CELL = "//div[@id='segments-body']//table[@data-recordindex = '*#*']//td[contains(@class, ' test-segment-target')]//div//div";
		protected const string TARGET_CELL_VALUE = "//table[@data-recordindex='*#*']//td[contains(@class, ' test-segment-target')]//div[contains(@id, 'segmenteditor')]";
		protected const string SOURCE_CELL = "//table[@data-recordindex='*#*']//td[contains(@class, 'test-segment-source')]//div[contains(@id, 'segmenteditor')]";
		protected const string TAG = "//div[contains(text(), '1')]//..//..//..//..//tr[1]//td[4]//div//img[contains(@class,'tag')]";
		protected const string SEGMENT_LOCK = "//div[contains(text(), '*#*')]//..//..//..//div[contains(@class,'lock')][not(contains(@class,'inactive'))]";

		protected const string CHARACTER_FORM = "charmap";
		protected const string CONCORDANCE_SEARCH= "concordance-search";

		protected const string ALL_SEGMENTS_SAVED_STATUS = "//div[text()='All segments are saved.']";
		protected const string SAVING_STATUS = "//divc[contains(@id, 'segmentsavingindicator') and contains(text(),'Saving')]";
		protected const string MATCH_COLUMN = "//div[@id='segments-body']//table[*#*]//tbody//td[contains(@class,'matchcolum')]";
		protected const string TARGET_MATCH_COLUMN_PERCENT = "//table[@data-recordindex='*#*' and contains(@id, 'tableview')]//td[6]//div//span";
		protected const string CAT_PANEL_PERCENT_MATCH = ".//div[@id='cat-body']//table[*#*]//tbody//tr//td[3]//div//span";
		protected const string MT_SOURCE_TEXT_IN_CAT_PANEL = ".//div[@id='cat-body']//table//tbody//tr//div[text()='MT']//..//preceding-sibling::td[contains(@class, 'test-cat-source')]/div";
		protected const string CAT_TYPE_LIST_IN_PANEL = ".//div[@id='cat-body']//table//td[3]/div";
		protected const string CAT_TRANSLATION = ".//div[@id='cat-body']//table[*#*]//td[contains(@class, 'test-cat-target')]/div";
		protected const string CAT_TYPE = ".//div[@id='cat-body']//table//td[3]/div[text()='*#*']";
		protected const string CAT_SOURCE = ".//div[@id='cat-body']//table//td[contains(@class, 'test-cat-source')]/div[text()='*#*']";

		protected const string PERCENT_COLOR = "//table[@data-recordindex='*#*' and contains(@id, 'tableview')]//td[6]//div//span";

		protected const string TERM_SAVED_MESSAGE = ".//div[text()='The term has been saved.']";

		protected const string EXISTING_TERM_MESSAGE = "//div[contains(@id, 'messagebox') and contains(string(), 'This glossary already contains term(s)')]";
		protected const string СONFIRM_YES_BTN = "//div[contains(@id, 'messagebox')]//span[contains(string(), 'Yes')]";
		protected const string CAT_TABLE = ".//div[@id='cat-body']//table";

		protected const string WORKFLOW_COLUMN = "//td[contains(@class, 'segmentworkflowcolumn')]";

		protected const string PROGRESS_BAR = "//div[contains(@class, 'x-progress-bar x-progress-bar-default')]";
		protected const string HIGHLIGHTED_SEGMENT = "//*[@id='segments-body']//div//div//table[*#*]//td[3]//div//span";
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
		protected const string SEGMENT_TRANSLATION_USER_COLUMN = ".//div[@id='translations-body']//table[*#*]//td[2]//div";
		protected const string EDITOR_DIALOG_BACKGROUND = "//div[contains(@class,'x-mask callout-mask')]";

		#endregion
	}
}
