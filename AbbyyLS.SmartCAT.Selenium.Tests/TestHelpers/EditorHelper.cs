using System;
using System.Threading;

using NLog;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class EditorHelper 
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();

		public EditorHelper SelectTask(TaskMode mode = TaskMode.Translation)
		{
			BaseObject.InitPage(_selectTask);
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
			BaseObject.InitPage(_editorPage);
			_editorPage.CloseTutorialIfExist();

			return this;
		}

		public EditorHelper CopySourceToTarget(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickCopySourceToTargetButton();

			return this;
		}

		public EditorHelper CopySourceToTargetByHotKey(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickCopySourceToTargetHotkey();

			return this;
		}

		public EditorHelper AssertSourceEqualsTarget(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			var source = _editorPage.SourceText(segmentNumber);
			var target = _editorPage.TargetText(segmentNumber);

			Assert.AreEqual(source, target, 
				string.Format("Произошла ошибка:\n в сегменте #{0} исходный текст не совпадает с таргетом.", segmentNumber));

			return this;
		}

		public EditorHelper AddTextToSegment(string text = "Translation", int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.SendTargetText(text, rowNumber);

			return this;
		}

		public EditorHelper AddTextWithoutClearing(string text = "Translation", int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.AddText(text, rowNumber);

			return this;
		}
		
		public EditorHelper ClickLastUnconfirmedButton()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickLastUnconfirmedButton();

			return this;
		}

		public EditorHelper ClickLastUnconfirmedHotKey()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickF9HotKey();

			return this;
		}
		
		public EditorHelper ConfirmTranslation()
		{
			BaseObject.InitPage(_editorPage);
			// Без слипа не отрабатывает. Ожидание элемента вставлять смысла нет. Причины не совсем ясны.
			Thread.Sleep(5000);
			_editorPage.ClickConfirmButton();

			return this;
		}

		public EditorHelper ConfirmTranslationByHotkeys(int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickTargetCell(rowNumber)
				.ConfirmSegmentByHotkeys();

			return this;
		}

		public EditorHelper ClickTargetSegment(int rowNumber)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickTargetCell(rowNumber);

			return this;
		}

		public EditorHelper PasteTranslationFromCAT(CatType catType, int targetRowNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			var catRowNumber = _editorPage
									.ClickTargetCell(targetRowNumber)
									.WaitCatTypeDisplayed(catType)
									.CatTypeRowNumber(catType);

			_editorPage.DoubleClickCatPanel(catRowNumber);

			AssertTargetTextAndCatTextMatch(targetRowNumber, catRowNumber);

			return this;
		}

		public int CarRowNumber(int targetRowNumber, CatType catType)
		{
			BaseObject.InitPage(_editorPage);

			return _editorPage
						.ClickTargetCell(targetRowNumber)
						.CatTypeRowNumber(catType);
		}

		public int CATRowNumber(CatType catType)
		{
			BaseObject.InitPage(_editorPage);

			return _editorPage.CatTypeRowNumber(catType);
		}

		public EditorHelper AssertMatchColumnCatTypeMatch(CatType catType, int rowNumber = 1)
		{
			Logger.Trace("Проверить, что текст в колонке MatchColumn совпадает с {0}.", catType);
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
			Logger.Trace("Проверить, что процент совпадения в CAT-панели и в таргете совпадает. Строка в CAT-панели {0}, строка в таргет {1}.", catRowNumber, segmentNumber);
			
			Assert.AreEqual(_editorPage.CatTranslationMatchPercent(catRowNumber),_editorPage.TargetMatchPercent(segmentNumber),
				"Произошла ошибка:\n Процент совпадения в CAT-панели и в таргете не совпадает.");

			return this;
		}

		public EditorHelper AssertTargetTextAndCatTextMatch(int segmentNumber, int catRowNumber)
		{
			Logger.Trace("Проверить, что текст из таргет сегмента совпадает с текстом перевода из CAT-панели.");
			
			Assert.AreEqual(_editorPage.TargetText(segmentNumber), _editorPage.CATTranslationText(catRowNumber),
				"Произошла ошибка:\n текст из таргет сегмента не совпадает с текстом перевода из CAT-панели.");

			return this;
		}

		public string SourceText(int rowNumber)
		{
			Logger.Trace("Получить текст из Source сегмента №{0}.", rowNumber);

			return _editorPage.SourceText(rowNumber);
		}

		public EditorHelper AssertTargetMatchPercenrCollorCorrect(int segmentNumber = 1)
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
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickFindErrorButton();

			return this;
		}

		public EditorHelper FindErrorsByHotkeys()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.FindErrorByHotkey();

			return this;
		}

		public EditorHelper AssertIsSegmentConfirmed(int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.AssertIsSegmentConfirmed(rowNumber);

			return this;
		}
		
		public EditorHelper AssertSegmentIsSelected(int rowNumber)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.AssertSegmentIsSelected(rowNumber);

			return this;
		}
		
		public EditorHelper AssertSaveingStatusIsDisappeared()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.AssertSaveingStatusIsDisappeared();

			return this;
		}

		public EditorHelper AddNewTerm(
			string sourceTerm,
			string targetTerm,
			string comment = null,
			string glossaryName = null)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickAddTermButton()
				.FillSourceTerm(sourceTerm)
				.FillTargetTerm(targetTerm);

			BaseObject.InitPage(_addTermDialog);

			if (comment != null)
			{
				_addTermDialog.EnterComment(comment);
			}

			if (glossaryName != null)
			{
				_addTermDialog.SelectGlossaryByName(glossaryName);
			}

			_addTermDialog
				.ClickAddButton<EditorPage>()
				.AssertTermIsSaved();

			return this;
		}
		
		public ProjectSettingsHelper ClickHomeButton()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickHomeButton();

			return new ProjectSettingsHelper();
		}

		public EditorHelper RemoveAllWordsFromDictionary()
		{
			BaseObject.InitPage(_editorPage);

			_spellcheckDictionaryDialog = _editorPage.ClickSpellcheckDictionaryButton();
			var wordsList = _spellcheckDictionaryDialog.GetWordsList();

			wordsList.ForEach(word => _spellcheckDictionaryDialog.ClickDeleteWordButton(word));

			_spellcheckDictionaryDialog.ClickCloseDictionaryButton();

			return this;
		}

		public EditorHelper RollBack(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickTargetCell(segmentNumber)
				.ClickRollbackButton()
				.AssertSegmentIsLocked(segmentNumber);

			return this;
		}

		public EditorHelper AssertSegmentIsNotLocked(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.AssertSegmentIsNotLocked(segmentNumber);

			return this;
		}

		public EditorHelper AddWordToDictionary(string word)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickSpellcheckDictionaryButton()
				.ClickAddWordButton()
				.AddWordToDictionary(word)
				.ConfirmWord<SpellcheckDictionaryDialog>()
				.ClickCloseDictionaryButton();

			return this;
		}

		public EditorHelper OpenSpecialCharacters()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickCharacterButton();

			return this;
		}

		public EditorHelper OpenSpecialCharactersByHotKey()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickCharacterButtonByHotKey();

			return this;
		}

		public EditorHelper OpenConcordanceSearch()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickConcordanceButton()
				.AssertConcordanceSearchIsDisplayed();

			return this;
		}

		public EditorHelper OpenConcordanceSearchByHotKey()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickConcordanceButtonByHotKey()
				.AssertConcordanceSearchIsDisplayed();

			return this;
		}

		public EditorHelper ReplaceWordInDictionary(string oldWord, string newWord)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickSpellcheckDictionaryButton()
				.HilightWordInDictionary(oldWord)
				.AddWordToDictionary(newWord)
				.ConfirmWord<SpellcheckDictionaryDialog>()
				.ClickCloseDictionaryButton();

			return this;
		}

		public EditorHelper DeleteWordFromDictionary(string word)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickSpellcheckDictionaryButton()
				.ClickDeleteWordButton(word)
				.ClickCloseDictionaryButton();

			return this;
		}

		public EditorHelper AssertWordInDictionary(string word, bool shouldExist)
		{
			BaseObject.InitPage(_editorPage);

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
			BaseObject.InitPage(_editorPage);
			_editorPage.AssertLastRevisionEqualTo(RevisionType.ManualInput);

			return this;
		}

		public EditorHelper AssertUnderlineForWord(string word, bool shouldExist)
		{
			BaseObject.InitPage(_editorPage);

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
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickSpellcheckDictionaryButton();

			return this;
		}
		
		public EditorHelper AssertSameTerminAdditionNotAllowed(string word)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickSpellcheckDictionaryButton()
				.AssertAddWordButtinEnabled()
				.ClickAddWordButton()
				.AddWordToDictionary(word)
				.ConfirmWord<SpellcheckErrorDialog>()
				.ClickOkButton()
				.ClickCloseDictionaryButton();

			return this;
		}

		public EditorHelper InsertTag(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickInsertTagButton()
				.AssertTagIsDisplayed(segmentNumber);

			return this;
		}

		public EditorHelper InsertTagByHotKey(int segmentNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickF8HotKey()
				.AssertTagIsDisplayed(segmentNumber);

			return this;
		}

		public EditorHelper CheckStage(string stage)
		{
			BaseObject.InitPage(_editorPage);

			Assert.AreEqual(stage, _editorPage.GetStage(),
				"Произошла ошибка:\n В шапке редактора отсутствует нужная задача.");

			return this;
		}

		public EditorHelper AssertStageNameIsEmpty()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.AssertStageNameIsEmpty();

			return this;
		}
		
		public EditorHelper ClickConfirmButton()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickConfirmButton()
				.AssertAllSegmentsSaved();

			return this;
		}
		
		public EditorHelper FillTarget(string text, int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickTargetCell(rowNumber)
				.SendTargetText(text, rowNumber);

			return this;
		}

		public EditorHelper ClickTargetCell(int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickTargetCell(rowNumber);

			return this;
		}

		public EditorHelper AssertTargetDisplayed(int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.AssertTargetDisplayed(rowNumber);

			return this;
		}

		public EditorHelper OpenAddTermDialogWithHotKey()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.SendCtrlE();

			return this;
		}

		public EditorHelper OpenAddTermDialog()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickAddTermButton();

			return this;
		}

		public EditorHelper SelectFirstWordInSegment(int rowNumber, SegmentType segmentType)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.SelectFirstWordInSegment(rowNumber, segmentType);

			return this;
		}

		public string GetFirstWordInSegment()
		{
			BaseObject.InitPage(_editorPage);

			return _editorPage.GetSelectedWordInSegment();
		}

		public EditorHelper CheckAutofillInAddTermDialog(string source = null, string target = null)
		{
			BaseObject.InitPage(_addTermDialog);

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
			BaseObject.InitPage(_addTermDialog);
			_addTermDialog
				.FillSourceTerm(source)
				.FillTargetTerm(target)
				.ClickAddButton<EditorPage>();

			return this;
		}

		public EditorHelper ConfirmAdditionTermWithoutTranslation()
		{
			BaseObject.InitPage(_addTermDialog);
			_addTermDialog
				.ClickAddButton<AddTermDialog>()
				.AssertConfirmSingleTermMessageDisplayed()
				.Confirm()
				.AssertTermIsSaved();

			return this;
		}

		public EditorHelper ConfirmAdditionExistedTerm()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.AssertConfirmExistedTermMessageDisplayed()
				.Confirm()
				.AssertTermIsSaved();

			return this;
		}

		public EditorHelper AssertGlossaryExistInList(string glossaryName)
		{
			BaseObject.InitPage(_addTermDialog);
			_addTermDialog.AssertGlossaryExistInDropdown(glossaryName);

			return this;
		}

		public EditorHelper ClickCancelAddTerm()
		{
			BaseObject.InitPage(_addTermDialog);
			_addTermDialog.ClickCancel();

			return this;
		}

		private readonly SelectTaskDialog _selectTask = new SelectTaskDialog();
		private readonly EditorPage _editorPage = new EditorPage();
		private readonly AddTermDialog _addTermDialog = new AddTermDialog();
		private SpellcheckDictionaryDialog _spellcheckDictionaryDialog = new SpellcheckDictionaryDialog();
	}
}
