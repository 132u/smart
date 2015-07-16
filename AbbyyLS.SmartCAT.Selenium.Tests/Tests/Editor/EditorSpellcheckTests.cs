﻿using System.IO;
using NUnit.Framework;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[TestFixture]
	[Standalone]
	public class EditorSpellcheckTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			var projectName = CreateProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(projectName, createNewTm: true, filePath: PathProvider.DocumentFile)
				.CheckProjectAppearInList(projectName)
				.GoToProjectSettingsPage(projectName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), NickName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile))
				.SelectTask()
				.CloseTutorialIfExist()
				.RemoveAllWordsFromDictionary();
		}

		[Test]
		public void AddNewWord()
		{
			_editorHelper
				.AddWordToDictionary(_wordsToAdd[0])
				.AssertWordInDictionary(_wordsToAdd[0], shouldExist: true)
				.ClickHomeButton()
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile))
				.SelectTask()
				.AssertWordInDictionary(_wordsToAdd[0], shouldExist: true);
		}

		[Test]
		public void DeleteWord()
		{
			_editorHelper
				.AddWordToDictionary(_wordsToAdd[1])
				.DeleteWordFromDictionary(_wordsToAdd[1])
				.AssertWordInDictionary(_wordsToAdd[1], shouldExist: false)
				.ClickHomeButton()
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile))
				.SelectTask()
				.AssertWordInDictionary(_wordsToAdd[1], shouldExist: false);
		}

		[Test]
		public void UnderlineBeforeAddWord()
		{
			_editorHelper
				.AddTextToSegment(_wordsToAdd[2])
				.AssertAutosaveWasComplete()
				.AssertUnderlineForWord(_wordsToAdd[2], shouldExist: true);
		}

		[Test]
		public void UnderlineAfterAddWord()
		{
			_editorHelper
				.AddWordToDictionary(_wordsToAdd[3])
				.AddTextToSegment(_wordsToAdd[3])
				.AssertAutosaveWasComplete()
				.AssertUnderlineForWord(_wordsToAdd[3], shouldExist: false);
			
		}

		[Test]
		public void UnderlineAfterDeleteWord()
		{
			_editorHelper
				.AddWordToDictionary(_wordsToAdd[4])
				.DeleteWordFromDictionary(_wordsToAdd[4])
				.AddTextToSegment(_wordsToAdd[4])
				.AssertAutosaveWasComplete()
				.AssertUnderlineForWord(_wordsToAdd[4], shouldExist: true);
		}

		[TestCase("Планета")]
		[TestCase("Чуть-чуть")]
		public void UnderlineWord(string word)
		{
			var wrongWord = string.Format("Ы{0}", word);

			_editorHelper
				.AddTextToSegment(word)
				.AssertAutosaveWasComplete()
				.AssertUnderlineForWord(word, shouldExist: false)
				.AddTextToSegment(wrongWord)
				.AssertAutosaveWasComplete()
				.AssertUnderlineForWord(wrongWord, shouldExist: true);
		}

		[Test]
		public void AddSameWord()
		{
			_editorHelper
				.AddWordToDictionary(_wordsToAdd[5])
				.AssertSameTerminAdditionNotAllowed(_wordsToAdd[5]);
		}

		[Test]
		public void EditWord()
		{
			_editorHelper
				.AddWordToDictionary(_wordsToAdd[6])
				.ReplaceWordInDictionary(oldWord: _wordsToAdd[6], newWord: _wordsToAdd[7])
				.AssertWordInDictionary(_wordsToAdd[6], shouldExist: false)
				.AssertWordInDictionary(_wordsToAdd[7], shouldExist: true);
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

		private readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
		private readonly  EditorHelper _editorHelper = new EditorHelper();
		
	}
}