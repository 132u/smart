using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorSpellcheckTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_editorPage = new EditorPage(Driver);
			_spellcheckDictionaryDialog = new SpellcheckDictionaryDialog(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_spellcheckErrorDialog = new SpellcheckErrorDialog(Driver);

			var projectName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(projectName, createNewTm: true, filePath: PathProvider.DocumentFile)
				.GoToProjectSettingsPage(projectName)
				.AssignTasksOnDocument(PathProvider.DocumentFile, ThreadUser.NickName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(PathProvider.DocumentFile);

			_selectTaskDialog.SelectTask();

			_editorPage
				.CloseTutorialIfExist()
				.ClickSpellcheckDictionaryButton();

			_spellcheckDictionaryDialog.RemoveAllWordsFromDictionary();
		}

		[Test]
		public void AddNewWord()
		{
			_editorPage.ClickSpellcheckDictionaryButton();

			_spellcheckDictionaryDialog.AddWordToDictionary(_wordsToAdd[0]);

			_editorPage.ClickSpellcheckDictionaryButton();

			Assert.IsTrue(_spellcheckDictionaryDialog.IsWordExistInDictionary(_wordsToAdd[0]),
				"Произошла ошибка:\n слово {0} не найдено в словаре.", _wordsToAdd[0]);

			_spellcheckDictionaryDialog.ClickCloseDictionaryButton();

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(PathProvider.DocumentFile);

			_selectTaskDialog.SelectTask();

			_editorPage.ClickSpellcheckDictionaryButton();

			Assert.IsTrue(_spellcheckDictionaryDialog.IsWordExistInDictionary(_wordsToAdd[0]),
				"Произошла ошибка:\n слово {0} не найдено в словаре.", _wordsToAdd[0]);
		}

		[Test]
		public void DeleteWord()
		{
			_editorPage.ClickSpellcheckDictionaryButton();

			_spellcheckDictionaryDialog.AddWordToDictionary(_wordsToAdd[1]);

			_editorPage.ClickSpellcheckDictionaryButton();

			_spellcheckDictionaryDialog.DeleteWordFromDictionary(_wordsToAdd[1]);

			_editorPage.ClickSpellcheckDictionaryButton();

			Assert.IsFalse(_spellcheckDictionaryDialog.IsWordExistInDictionary(_wordsToAdd[1]),
				"Произошла ошибка:\n слово {0} найдено в словаре.", _wordsToAdd[1]);

			_spellcheckDictionaryDialog.ClickCloseDictionaryButton();

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(PathProvider.DocumentFile);

			_selectTaskDialog.SelectTask();

			_editorPage.ClickSpellcheckDictionaryButton();

			Assert.IsFalse(_spellcheckDictionaryDialog.IsWordExistInDictionary(_wordsToAdd[1]),
				"Произошла ошибка:\n слово {0} найдено в словаре.", _wordsToAdd[1]);
		}

		[Test]
		public void UnderlineBeforeAddWord()
		{
			_editorPage.FillSegmentTargetField(_wordsToAdd[2]);

			Assert.IsTrue(_editorPage.IsLastRevisionEqualToExpected(RevisionType.ManualInput),
				"Произошла ошибка:\n тип последней ревизии  не соответствует ожидаемому");

			Assert.IsTrue(_editorPage.IsUnderlineForWordExist(_wordsToAdd[2]),
				"Произошла ошибка:\n не удалось найти слово {0} подчеркнутых.", _wordsToAdd[2]);
		}

		[Test]
		public void UnderlineAfterAddWord()
		{
			_editorPage.ClickSpellcheckDictionaryButton();

			_spellcheckDictionaryDialog.AddWordToDictionary(_wordsToAdd[3]);

			_editorPage.FillSegmentTargetField(_wordsToAdd[3]);

			Assert.IsTrue(_editorPage.IsLastRevisionEqualToExpected(RevisionType.ManualInput),
				"Произошла ошибка:\n тип последней ревизии не соответствует ожидаемому");

			Assert.IsFalse(_editorPage.IsUnderlineForWordExist(_wordsToAdd[3]),
				"Произошла ошибка:\n слово {0} подчеркнуто.", _wordsToAdd[3]);
		}

		[Test]
		public void UnderlineAfterDeleteWord()
		{
			_editorPage.ClickSpellcheckDictionaryButton();

			_spellcheckDictionaryDialog.AddWordToDictionary(_wordsToAdd[4]);

			_editorPage.ClickSpellcheckDictionaryButton();

			_spellcheckDictionaryDialog.DeleteWordFromDictionary(_wordsToAdd[4]);

			_editorPage.FillSegmentTargetField(_wordsToAdd[4]);

			Assert.IsTrue(_editorPage.IsLastRevisionEqualToExpected(RevisionType.ManualInput),
				"Произошла ошибка:\n тип последней ревизии не соответствует ожидаемому");

			Assert.IsTrue(_editorPage.IsUnderlineForWordExist(_wordsToAdd[4]),
				"Произошла ошибка:\n не удалось найти слово {0} подчеркнутых.", _wordsToAdd[4]);
		}

		[TestCase("Планета")]
		[TestCase("Чуть-чуть")]
		public void UnderlineWord(string word)
		{
			var wrongWord = string.Format("Ы{0}", word);

			_editorPage.FillSegmentTargetField(word);

			Assert.IsTrue(_editorPage.IsLastRevisionEqualToExpected(RevisionType.ManualInput),
				"Произошла ошибка:\n тип последней ревизии  не соответствует ожидаемому");

			Assert.IsFalse(_editorPage.IsUnderlineForWordExist(word),
				"Произошла ошибка:\n слово {0} подчеркнуто.", word);

			_editorPage.FillSegmentTargetField(wrongWord);

			Assert.IsTrue(_editorPage.IsLastRevisionEqualToExpected(RevisionType.ManualInput),
				"Произошла ошибка:\n тип последней ревизии  не соответствует ожидаемому");

			Assert.IsTrue(_editorPage.IsUnderlineForWordExist(wrongWord),
				"Произошла ошибка:\n не удалось найти слово {0} подчеркнутых.", wrongWord);
		}

		[Test]
		public void AddSameWord()
		{
			_editorPage.ClickSpellcheckDictionaryButton();

			_spellcheckDictionaryDialog.AddWordToDictionary(_wordsToAdd[5]);

			_editorPage.ClickSpellcheckDictionaryButton();

			_spellcheckDictionaryDialog
				.ClickAddWordButton()
				.FillWordField(_wordsToAdd[5])
				.ConfirmWord<SpellcheckErrorDialog>(Driver);

			Assert.IsTrue(_spellcheckErrorDialog.IsSpellcheckErrorDialogOpened(),
				"Произошла ошибка: \nне появилось сообщение об ошибке.");

			_spellcheckErrorDialog
				.ClickOkButton()
				.ClickCloseDictionaryButton();
		}

		[Test]
		public void EditWord()
		{
			_editorPage.ClickSpellcheckDictionaryButton();

			_spellcheckDictionaryDialog.AddWordToDictionary(_wordsToAdd[6]);

			_editorPage.ClickSpellcheckDictionaryButton();

			_spellcheckDictionaryDialog.ReplaceWordInDictionary(oldWord: _wordsToAdd[6], newWord: _wordsToAdd[7]);

			_editorPage.ClickSpellcheckDictionaryButton();

			Assert.IsFalse(_spellcheckDictionaryDialog.IsWordExistInDictionary(_wordsToAdd[6]),
				"Произошла ошибка:\n слово {0} найдено в словаре.", _wordsToAdd[6]);

			Assert.IsTrue(_spellcheckDictionaryDialog.IsWordExistInDictionary(_wordsToAdd[7]),
				"Произошла ошибка:\n слово {0} не найдено в словаре.", _wordsToAdd[7]);
		}
		
		[Test]
		public void SpellcheckButtonTest()
		{
			_editorPage.ClickSpellcheckDictionaryButton();

			Assert.IsTrue(_spellcheckDictionaryDialog.IsSpellcheckDictionaryDialogOpened(),
				"Произошла ошибка: \nне появился словарь");
		}

		private static readonly string[] _wordsToAdd =
		{
			"Ккапучино",
			"Ллатте",
			"Ээспрессо",
			"Аамерикано",
			"Ммокка",
			"Ббариста",
			"Рристретто",
			"Ррристретто"
		};

		private CreateProjectHelper _createProjectHelper;

		private ProjectSettingsPage _projectSettingsPage;
		private EditorPage _editorPage;
		private SpellcheckDictionaryDialog _spellcheckDictionaryDialog;
		private SelectTaskDialog _selectTaskDialog;
		private SpellcheckErrorDialog _spellcheckErrorDialog;
	}
}
