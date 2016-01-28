using System.IO;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class EditorRevisionTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void EditorRevisionTestsSetUp()
		{
			_secondUser = null;
		}

		[Test]
		public void RollbackButtonDisabledTest()
		{
			_editorPage
				.FillSegmentTargetField()
				.ClickOnTargetCellInSegment()
				.OpenRevisionTab();

			Assert.AreEqual(1, _editorPage.GetRevisionsCount(),
				"Произошла ошибка:\nПеревод не появился в ревизиях");

			Assert.IsTrue(_editorPage.IsRestoreButtonDisabled(),
				"Произошла ошибка:\nКнопка Restore активна.");
		}

		[Test]
		public void RollbackButtonEnabledTest()
		{
			_editorPage
				.FillSegmentTargetField()
				.ClickOnTargetCellInSegment()
				.OpenRevisionTab()
				.SelectRevision();

			Assert.AreEqual(1, _editorPage.GetRevisionsCount(),
				"Произошла ошибка:\nПеревод не появился в ревизиях");

			Assert.IsFalse(_editorPage.IsRestoreButtonDisabled(),
				"Произошла ошибка:\nКнопка Restore неактивна.");
		}

		[Test]
		public void ConfirmAfterSaveTest()
		{
			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment()
				.OpenRevisionTab();

			Assert.AreEqual(1, _editorPage.GetRevisionsCount(),
				"Произошла ошибка:\nНеверное количество ревизий.");

			Assert.AreEqual(RevisionType.ManualInput.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка:\nНеверный тип ревизии.");
		}

		[Test]
		public void TwoRevisionsWithoutConfirmationTest()
		{
			_editorPage
				.FillSegmentTargetField()
				.FillSegmentTargetField("Translation 2")
				.ClickOnTargetCellInSegment()
				.OpenRevisionTab()
				.SelectRevision();

			Assert.AreEqual(1, _editorPage.GetRevisionsCount(),
				"Произошла ошибка:\nПеревод не появился в ревизиях");

			Assert.AreEqual(RevisionType.ManualInput.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка:\nНеверный тип ревизии.");
		}

		[Test]
		public void TwoConfirmedRevisionsTest()
		{
			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField("Translation 2")
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment()
				.OpenRevisionTab()
				.SelectRevision();

			Assert.AreEqual(2, _editorPage.GetRevisionsCount(),
				"Произошла ошибка:\nПеревод не появился в ревизиях");

			Assert.AreEqual(RevisionType.ManualInput.Description(), _editorPage.GetRevisionType(revisionNumber: 1),
				"Произошла ошибка:\nНеверный тип ревизии.");

			Assert.AreEqual(RevisionType.ManualInput.Description(), _editorPage.GetRevisionType(revisionNumber: 2),
				"Произошла ошибка:\nНеверный тип ревизии.");
		}

		[Test]
		public void RestoreRevisionTest()
		{
			var translationForSecondRevision = "Translation 2";

			_editorPage
					.FillSegmentTargetField()
					.ConfirmSegmentTranslation()
					.FillSegmentTargetField(translationForSecondRevision)
					.ConfirmSegmentTranslation()
					.ClickOnTargetCellInSegment()
					.OpenRevisionTab()
					.SelectRevision()
					.ClickRestoreButton();

			//Sleep нужен, так как ревизия с задержкой появляется
			Thread.Sleep(1000);
			Assert.AreEqual(RevisionType.Restored.Description(), _editorPage.GetRevisionType(revisionNumber: 1),
				"Произошла ошибка:\nНеверный тип ревизии.");

			Assert.AreEqual(translationForSecondRevision, _editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка:\nНеверный текст в таргете.");
		}
		
		[Test]
		public void RemovePartTextTest()
		{
			var word = "Translation";
			var changePart = word.Substring(2, 2);
			var resultWord = word.Replace(changePart, "");

			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(resultWord)
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment()
				.OpenRevisionTab();

			Assert.AreEqual(2, _editorPage.GetRevisionsCount(),
				"Произошла ошибка:\nПеревод не появился в ревизиях");

			Assert.AreEqual(RevisionType.ManualInput.Description(), _editorPage.GetRevisionType(revisionNumber: 1),
				"Произошла ошибка:\nНеверный тип ревизии.");
		
			Assert.AreEqual(word, _editorPage.GetRevisionDeleteChangedPart(),
				"Произошла ошибка:\nНеверно выделена часть измененного слова.");

			Assert.AreEqual(resultWord, _editorPage.GetRevisionInsertChangedPart(),
				"Произошла ошибка:\nНеверно выделена часть измененного слова.");
		}

		[Test]
		public void InsertPartTextTest()
		{
			var firstWord = "firstWord";
			var secondWord = "secondWord";

			_editorPage
				.FillSegmentTargetField(firstWord)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(secondWord, clearField: false)
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment()
				.OpenRevisionTab();

			Assert.AreEqual(2, _editorPage.GetRevisionsCount(),
				"Произошла ошибка:\nПеревод не появился в ревизиях");

			Assert.AreEqual(RevisionType.ManualInput.Description(), _editorPage.GetRevisionType(revisionNumber: 1),
				"Произошла ошибка:\nНеверный тип ревизии.");

			Assert.AreEqual(secondWord + firstWord, _editorPage.GetRevisionInsertChangedPart(),
				"Произошла ошибка:\nНеверно выделена часть измененного слова.");

			Assert.AreEqual(firstWord, _editorPage.GetRevisionDeleteChangedPart(),
				"Произошла ошибка:\nНеверно выделена часть удаленного слова.");
		}

		[Test, Ignore("PRX-14628")]
		public void TwoUsersRevisionsTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);

			var firstWord = "firstWord";
			var secondWord = "secondWord";
			var thirdWord = "thirdWord";

			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(firstWord)
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment()
				.OpenRevisionTab();

			Assert.AreEqual(2, _editorPage.GetRevisionsCount(),
				"Произошла ошибка:\nНеверное количество ревизий.");

			Assert.AreEqual(ThreadUser.NickName, _editorPage.GetRevisionUserName(),
				"Произошла ошибка:\nНеверное имя пользовтаеля.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsHelper.AssignTasksOnDocument(
				Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile),
				_secondUser.NickName);

			_workspacePage.SignOut();

			_signInPage
				.SubmitForm(_secondUser.Login, _secondUser.Password)
				.SelectAccount();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			_selectTaskDialog.SelectTask();

			_editorPage
				.FillSegmentTargetField(secondWord)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(thirdWord)
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment()
				.OpenRevisionTab();

			Assert.AreEqual(4, _editorPage.GetRevisionsCount(),
				"Произошла ошибка:\nНеверное количество ревизий.");

			Assert.AreEqual(_secondUser.NickName, _editorPage.GetRevisionUserName(),
				"Произошла ошибка:\nНеверное имя пользовтаеля.");
		}

		[TearDown]
		public void TearDown()
		{
			if (_secondUser != null)
			{
				ReturnUser(ConfigurationManager.Users, _secondUser);
			}
		}

		private TestUser _secondUser;
	}
}
