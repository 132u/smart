using System;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class EditorHelper 
	{
		public WebDriver Driver { get; private set; }

		public EditorHelper(WebDriver driver)
		{
			Driver = driver;
			_selectTask = new SelectTaskDialog(Driver);
			_addTermDialog = new AddTermDialog(Driver);
			_editorPage = new EditorPage(Driver);
			_spellcheckDictionaryDialog = new SpellcheckDictionaryDialog(Driver);
		}

		public EditorHelper SelectTask(TaskMode mode = TaskMode.Translation)
		{
			BaseObject.InitPage(_selectTask, Driver);
			switch (mode)
			{
				case TaskMode.Translation:
					_selectTask
						.ClickTranslateButton();
					break;

				case TaskMode.Manager:
					_selectTask
						.ClickManagerButton();
					break;

				case TaskMode.Editing:
					_selectTask
						.ClickEditingButton();
					break;

				default:
					throw new Exception(string.Format("Передан аргумент, который не предусмотрен! Значение аргумента:'{0}'", mode.ToString()));
			}

			_selectTask.ClickContinueButton();

			return this;
		}

		public EditorHelper CloseTutorialIfExist()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.CloseTutorialIfExist();

			return this;
		}

		public EditorHelper CopySourceToTarget(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.ClickCopySourceToTargetButton();

			return this;
		}

		public EditorHelper CopySourceToTargetByHotKey(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.ClickCopySourceToTargetHotkey();

			return this;
		}

		public EditorHelper AssertSourceEqualsTarget(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			var source = _editorPage.SourceText(segmentNumber);
			var target = _editorPage.TargetText(segmentNumber);

			Assert.AreEqual(source, target, 
				string.Format("Произошла ошибка:\n в сегменте #{0} исходный текст не совпадает с таргетом.", segmentNumber));

			return this;
		}

		public EditorHelper AddTextToSegment(string text = "Translation", int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.SendTargetText(text, rowNumber);

			return this;
		}

		public EditorHelper AddTextWithoutClearing(string text = "Translation", int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.AddText(text, rowNumber);

			return this;
		}
		
		public EditorHelper ClickLastUnconfirmedButton()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.ClickLastUnconfirmedButton();

			return this;
		}

		public EditorHelper ClickLastUnconfirmedHotKey()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.ClickF9HotKey();

			return this;
		}
		
		public EditorHelper ConfirmTranslation()
		{
			BaseObject.InitPage(_editorPage, Driver);
			// Без слипа не отрабатывает. Ожидание элемента вставлять смысла нет. Причины не совсем ясны.
			Thread.Sleep(5000);
			_editorPage.ClickConfirmButton();

			return this;
		}

		public EditorHelper ConfirmTranslationByHotkeys(int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.ClickTargetCell(rowNumber)
				.ConfirmSegmentByHotkeys();

			return this;
		}

		public EditorHelper ClickTargetSegment(int rowNumber)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.ClickTargetCell(rowNumber);

			return this;
		}

		public EditorHelper PasteTranslationFromCAT(CatType catType, int targetRowNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			var catRowNumber = _editorPage
									.ClickTargetCell(targetRowNumber)
									.WaitCatTypeDisplayed(catType)
									.CatTypeRowNumber(catType);

			_editorPage.DoubleClickCatPanel(catRowNumber);

			AssertTargetTextAndCatTextMatch(targetRowNumber, catRowNumber);

			return this;
		}

		public int CatRowNumber(CatType catType)
		{
			BaseObject.InitPage(_editorPage, Driver);

			return _editorPage.CatTypeRowNumber(catType);
		}

		public EditorHelper AssertCatPanelExist()
		{
			BaseObject.InitPage(_editorPage, Driver);

			Assert.IsTrue(_editorPage.CatTableExist(),
				"Произошла ошибка:\nCAT-панель пустая.");

			return this;
		}

		public EditorHelper AssertCatPercentMatch(int catRowNumber, int percent)
		{
			CustomTestContext.WriteLine("Проверить, что процент совпадения в CAT-панели равен {0}.", percent);

			Assert.AreEqual(percent, _editorPage.CatTranslationMatchPercent(catRowNumber),
				"Произошла ошибка:\nНеверный процент совпадения в CAT-панели.");

			return this;
		}

		public EditorHelper AssertCatTermsMatchSourceTerms(int segmentNumber)
		{
			BaseObject.InitPage(_editorPage, Driver);
			var catTerms = _editorPage.CatTerms()[0];
			var sourceTerms = _editorPage.SourceText(segmentNumber).Replace("1", String.Empty);

			Assert.AreEqual(catTerms, sourceTerms,
				"Произошла ошибка:\nТермины из CAT-панели не соответствуют терминам в сорсе.");

			return this;
		}

		public EditorHelper AssertCatPanelContainsCatType(CatType catType)
		{
			BaseObject.InitPage(_editorPage, Driver);

			Assert.AreNotEqual(0, _editorPage.CatTypeRowNumber(catType),
				"Произошла ошибка:\nВ CAT-панели отсутствует подстановка {0}.", catType);

			return this;
		}

		public EditorHelper AssertMatchColumnCatTypeMatch(CatType catType, int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что текст в колонке MatchColumn совпадает с {0}.", catType);
			var catTypeColumn = catType != CatType.TB ? catType.ToString() : string.Empty;

			var textInMacthColumn = _editorPage.MatchColumnText(rowNumber).Trim();

			if (textInMacthColumn.Contains("%"))
			{
				textInMacthColumn = textInMacthColumn.Substring(0, 2);
			}

			Assert.AreEqual(textInMacthColumn.Trim(), catTypeColumn,
				"Произошла ошибка:\n тип подстановки в колонке Match Column не совпал с типом перевода {0}.", catType);

			return this;
		}

		public EditorHelper AssertCATPercentMatchTargetPercent(int segmentNumber = 1, int catRowNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что процент совпадения в CAT-панели и в таргете совпадает. Строка в CAT-панели {0}, строка в таргет {1}.", catRowNumber, segmentNumber);
			
			Assert.AreEqual(_editorPage.CatTranslationMatchPercent(catRowNumber),_editorPage.TargetMatchPercent(segmentNumber),
				"Произошла ошибка:\n Процент совпадения в CAT-панели и в таргете не совпадает.");

			return this;
		}

		public EditorHelper AssertTargetTextAndCatTextMatch(int segmentNumber, int catRowNumber)
		{
			CustomTestContext.WriteLine("Проверить, что текст из таргет сегмента совпадает с текстом перевода из CAT-панели.");
			
			Assert.AreEqual(_editorPage.TargetText(segmentNumber), _editorPage.CATTranslationText(catRowNumber),
				"Произошла ошибка:\n текст из таргет сегмента не совпадает с текстом перевода из CAT-панели.");

			return this;
		}

		public string SourceText(int rowNumber)
		{
			CustomTestContext.WriteLine("Получить текст из Source сегмента №{0}.", rowNumber);

			return _editorPage.SourceText(rowNumber);
		}

		public EditorHelper AssertTargetMatchPercentCollorCorrect(int segmentNumber = 1)
		{
			const int yellowUpperBound = 99;
			const int yellowLowerBound = 76;

			const string green = "green";
			const string yellow = "yellow";
			const string red = "red";

			var targetMatchPercent = _editorPage.TargetMatchPercent(segmentNumber);
		 
			if (targetMatchPercent > yellowUpperBound)
			{
				Assert.AreEqual(green, _editorPage.TargetMatchColor(segmentNumber),
					"Произошла ошибка:\n неправильный цвет процента совпадения в сегменте №{0}.", segmentNumber);
			}
			
			if (targetMatchPercent <= yellowUpperBound && targetMatchPercent >= yellowLowerBound)
			{
				Assert.AreEqual(yellow, _editorPage.TargetMatchColor(segmentNumber),
					"Произошла ошибка:\n неправильный цвет процента совпадения в сегменте №{0}.", segmentNumber);
			}

			if (targetMatchPercent < yellowLowerBound)
			{
				Assert.AreEqual(red, _editorPage.TargetMatchColor(segmentNumber),
					"Произошла ошибка:\n неправильный цвет процента совпадения в сегменте №{0}.", segmentNumber);
			}
		 
			return this;
		}

		public EditorHelper ClickFindErrorsButton()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.ClickFindErrorButton();

			return this;
		}

		public EditorHelper FindErrorsByHotkeys()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.FindErrorByHotkey();

			return this;
		}

		public EditorHelper AssertIsSegmentConfirmed(int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.AssertIsSegmentConfirmed(rowNumber);

			return this;
		}
		
		public EditorHelper AssertSegmentIsSelected(int rowNumber)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.AssertSegmentIsSelected(rowNumber);

			return this;
		}
		
		public EditorHelper AssertSaveingStatusIsDisappeared()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.AssertSaveingStatusIsDisappeared();

			return this;
		}

		public EditorHelper AddNewTerm(
			string sourceTerm,
			string targetTerm,
			string comment = null,
			string glossaryName = null)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.ClickAddTermButton()
				.FillSourceTerm(sourceTerm)
				.FillTargetTerm(targetTerm);

			BaseObject.InitPage(_addTermDialog, Driver);

			if (comment != null)
			{
				_addTermDialog.EnterComment(comment);
			}

			if (glossaryName != null)
			{
				_addTermDialog.SelectGlossaryByName(glossaryName);
			}

			_addTermDialog
				.ClickAddButton<EditorPage>(Driver)
				.AssertTermIsSaved()
				.AssertTermIsSavedMessageDisappeared();

			return this;
		}
		
		public ProjectSettingsHelper ClickHomeButton()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.ClickHomeButton();

			return new ProjectSettingsHelper(Driver);
		}

		public EditorHelper RemoveAllWordsFromDictionary()
		{
			BaseObject.InitPage(_editorPage, Driver);

			_spellcheckDictionaryDialog = _editorPage.ClickSpellcheckDictionaryButton();
			var wordsList = _spellcheckDictionaryDialog.GetWordsList();

			wordsList.ForEach(word => _spellcheckDictionaryDialog.ClickDeleteWordButton(word));

			_spellcheckDictionaryDialog.ClickCloseDictionaryButton();

			return this;
		}

		public EditorHelper RollBack(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.ClickTargetCell(segmentNumber)
				.ClickRollbackButton()
				.AssertSegmentIsLocked(segmentNumber);

			return this;
		}

		public EditorHelper AssertSegmentIsNotLocked(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.AssertSegmentIsNotLocked(segmentNumber);

			return this;
		}

		public EditorHelper AddWordToDictionary(string word)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.ClickSpellcheckDictionaryButton()
				.ClickAddWordButton()
				.AddWordToDictionary(word)
				.ConfirmWord<SpellcheckDictionaryDialog>(Driver)
				.ClickCloseDictionaryButton();

			return this;
		}

		public EditorHelper OpenSpecialCharacters()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.ClickCharacterButton();

			return this;
		}

		public EditorHelper OpenSpecialCharactersByHotKey()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.ClickCharacterButtonByHotKey();

			return this;
		}

		public EditorHelper OpenConcordanceSearch()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.ClickConcordanceButton()
				.AssertConcordanceSearchIsDisplayed();

			return this;
		}

		public EditorHelper OpenConcordanceSearchByHotKey()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.ClickConcordanceButtonByHotKey()
				.AssertConcordanceSearchIsDisplayed();

			return this;
		}

		public EditorHelper ReplaceWordInDictionary(string oldWord, string newWord)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.ClickSpellcheckDictionaryButton()
				.HilightWordInDictionary(oldWord)
				.AddWordToDictionary(newWord)
				.ConfirmWord<SpellcheckDictionaryDialog>(Driver)
				.ClickCloseDictionaryButton();

			return this;
		}

		public EditorHelper DeleteWordFromDictionary(string word)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.ClickSpellcheckDictionaryButton()
				.ClickDeleteWordButton(word)
				.ClickCloseDictionaryButton();

			return this;
		}

		public EditorHelper AssertWordInDictionary(string word, bool shouldExist)
		{
			BaseObject.InitPage(_editorPage, Driver);

			switch (shouldExist)
			{
				case true:
					_editorPage
						.ClickSpellcheckDictionaryButton()
						.AssertWordExistInDictionary(word)
						.ClickCloseDictionaryButton();
					break;
				case false:
					_editorPage
						.ClickSpellcheckDictionaryButton()
						.AssertWordNotExistInDictionary(word)
						.ClickCloseDictionaryButton();
					break;
			}

			return this;
		}

		public EditorHelper AssertAutosaveWasComplete()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.AssertLastRevisionEqualTo(RevisionType.ManualInput);

			return this;
		}

		public EditorHelper AssertUnderlineForWord(string word, bool shouldExist)
		{
			BaseObject.InitPage(_editorPage, Driver);

			switch (shouldExist)
			{
				case true:
					_editorPage.AssertUnderlineForWordExist(word);
					break;
				case false:
					_editorPage.AssertUnderlineForWordNotExist(word);
					break;
			}
			
			return this;
		}

		public EditorHelper OpenSpellcheckDictionary()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.ClickSpellcheckDictionaryButton();

			return this;
		}
		
		public EditorHelper AssertSameTerminAdditionNotAllowed(string word)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.ClickSpellcheckDictionaryButton()
				.AssertAddWordButtinEnabled()
				.ClickAddWordButton()
				.AddWordToDictionary(word)
				.ConfirmWord<SpellcheckErrorDialog>(Driver)
				.ClickOkButton()
				.ClickCloseDictionaryButton();

			return this;
		}

		public EditorHelper InsertTag(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.ClickInsertTagButton()
				.AssertTagIsDisplayed(segmentNumber);

			return this;
		}

		public EditorHelper InsertTagByHotKey(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.ClickF8HotKey()
				.AssertTagIsDisplayed(segmentNumber);

			return this;
		}

		public EditorHelper CheckStage(string stage)
		{
			BaseObject.InitPage(_editorPage, Driver);

			Assert.AreEqual(stage, _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");

			return this;
		}

		public EditorHelper AssertStageNameIsEmpty()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.AssertStageNameIsEmpty();

			return this;
		}
		
		public EditorHelper ClickConfirmButton()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.ClickConfirmButton()
				.AssertAllSegmentsSaved();

			return this;
		}
		
		public EditorHelper FillTarget(string text, int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.ClickTargetCell(rowNumber)
				.SendTargetText(text, rowNumber);

			return this;
		}

		public EditorHelper ClickTargetCell(int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.ClickTargetCell(rowNumber);

			return this;
		}

		public EditorHelper AssertTargetDisplayed(int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.AssertTargetDisplayed(rowNumber);

			return this;
		}

		public EditorHelper OpenAddTermDialogWithHotKey()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.SendCtrlE();

			return this;
		}

		public EditorHelper OpenAddTermDialog()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.ClickAddTermButton();

			return this;
		}

		public EditorHelper SelectFirstWordInSegment(int rowNumber, SegmentType segmentType)
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage.SelectFirstWordInSegment(rowNumber, segmentType);

			return this;
		}

		public string GetFirstWordInSegment()
		{
			BaseObject.InitPage(_editorPage, Driver);

			return _editorPage.GetSelectedWordInSegment();
		}

		public EditorHelper CheckAutofillInAddTermDialog(string source = null, string target = null)
		{
			BaseObject.InitPage(_addTermDialog, Driver);

			if (source != null)
			{
				_addTermDialog.AssertTextExistInSourceTerm(source);
			}

			if (target != null)
			{
				_addTermDialog.AssertTextExistInTargetTerm(target);
			}

			return this;
		}

		public EditorHelper FillAddTermForm(string source, string target)
		{
			BaseObject.InitPage(_addTermDialog, Driver);
			_addTermDialog
				.FillSourceTerm(source)
				.FillTargetTerm(target)
				.ClickAddButton<EditorPage>(Driver);

			return this;
		}

		public EditorHelper ConfirmAdditionTermWithoutTranslation()
		{
			BaseObject.InitPage(_addTermDialog, Driver);
			_addTermDialog
				.ClickAddButton<AddTermDialog>(Driver)
				.AssertConfirmSingleTermMessageDisplayed()
				.Confirm()
				.AssertTermIsSaved()
				.AssertTermIsSavedMessageDisappeared();

			return this;
		}

		public EditorHelper ConfirmAdditionExistedTerm()
		{
			BaseObject.InitPage(_editorPage, Driver);
			_editorPage
				.AssertConfirmExistedTermMessageDisplayed()
				.Confirm()
				.AssertTermIsSaved()
				.AssertTermIsSavedMessageDisappeared();

			return this;
		}

		public EditorHelper AssertGlossaryExistInList(string glossaryName)
		{
			BaseObject.InitPage(_addTermDialog, Driver);
			_addTermDialog
				.AssertGlossaryExistInDropdown(glossaryName)
				.ClickGlossarySelect();

			return this;
		}

		public EditorHelper CloseGlossaryDropdown()
		{
			 BaseObject.InitPage(_addTermDialog, Driver);
			_addTermDialog.ClickGlossarySelect();

			return this;
		}

		public EditorHelper ClickCancelAddTerm()
		{
			BaseObject.InitPage(_addTermDialog, Driver);
			_addTermDialog.ClickCancel();

			return this;
		}

		private readonly SelectTaskDialog _selectTask;
		private readonly EditorPage _editorPage;
		private readonly AddTermDialog _addTermDialog;
		private SpellcheckDictionaryDialog _spellcheckDictionaryDialog;
	}
}
