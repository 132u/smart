using System;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class EditorHelper 
	{

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

		public EditorHelper AddTextToSegment(string text = "Translation", int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.SendTargetText(text, rowNumber);

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

		public ProjectSettingsHelper ClickHomeButton()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickHomeButton();

			return new ProjectSettingsHelper();
		}

		public EditorHelper RemoveAllWordsFromDictionary()
		{
			BaseObject.InitPage(_editorPage);

			_spellcheckDictionaryDialog = _editorPage.ClickSpelcheckDictionaryButton();
			var wordsList = _spellcheckDictionaryDialog.GetWordsList();

			wordsList.ForEach(word => _spellcheckDictionaryDialog.ClickDeleteWordButton(word));

			_spellcheckDictionaryDialog.ClickCloseDictionaryButton();

			return this;
		}

		public EditorHelper AddWordToDictionary(string word)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickSpelcheckDictionaryButton()
				.ClickAddWordButton()
				.AddWordToDictionary(word)
				.ConfirmWord<SpellcheckDictionaryDialog>()
				.ClickCloseDictionaryButton();

			return this;
		}

		public EditorHelper ReplaceWordInDictionary(string oldWord, string newWord)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickSpelcheckDictionaryButton()
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
				.ClickSpelcheckDictionaryButton()
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
						.ClickSpelcheckDictionaryButton()
						.AssertWordExistInDictionary(word)
						.ClickCloseDictionaryButton();
					break;
				case false:
					_editorPage
						.ClickSpelcheckDictionaryButton()
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

		public EditorHelper AssertSameTerminAdditionNotAllowed(string word)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickSpelcheckDictionaryButton()
				.AssertAddWordButtinEnabled()
				.ClickAddWordButton()
				.AddWordToDictionary(word)
				.ConfirmWord<SpellcheckErrorDialog>()
				.ClickOkButton()
				.ClickCloseDictionaryButton();

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
			_editorPage.ClickConfirmButton();

			return this;
		}

		public EditorHelper FillTarget(string text = "Translation", int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage
				.ClickTargetCell(rowNumber)
				.SendTargetText(text, rowNumber);

			return this;
		}

		public EditorHelper AssertTargetDisplayed(int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.AssertTargetDisplayed(rowNumber);

			return this;
		}

		private readonly SelectTaskDialog _selectTask = new SelectTaskDialog();
		private readonly EditorPage _editorPage = new EditorPage();
		
		private SpellcheckDictionaryDialog _spellcheckDictionaryDialog = new SpellcheckDictionaryDialog();
	}
}
