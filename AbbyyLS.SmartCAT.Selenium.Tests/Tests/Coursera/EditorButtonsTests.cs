using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Coursera]
	class EditorButtonsTests<TWebDriverProvider> : CourseraBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void CursorToEmptyTranslationRow()
		{
			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();
		}

		[Test]
		public void BackButtonTest()
		{
			_lecturesTab.OpenLecture();

			_editorPage.ClickHomeButtonExpectingCourseraCoursesPage();

			Assert.IsTrue(_coursesPage.IsCoursesPageOpened(),
				"Произошла ошибка: не открылась страница курсов.");
		}

		[Test]
		public void ToTargetButtonTest()
		{
			_lecturesTab.OpenLecture();

			var sourceText = _editorPage.GetSourceText(rowNumber: 1);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ClickCopySourceToTargetButton()
				.ConfirmSegmentTranslation();

			Assert.AreEqual(sourceText, _editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка:\n Неверный текст в таргете сегмента №1."); ;
		}

		[Test]
		public void ToTargetHotkeyTest()
		{
			_lecturesTab.OpenLecture();

			var sourceText = _editorPage.GetSourceText(rowNumber: 1);

			_editorPage
				.ClickOnTargetCellInSegment()
				.CopySourceToTargetHotkey()
				.ConfirmSegmentTranslation();

			Assert.AreEqual(sourceText, _editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка:\n Неверный текст в таргете сегмента №1."); ;
		}

		[Test]
		public void ChangeCaseTextButtonTest()
		{
			var sourceText = "the example sentence";
			var changedText1 = "The Example Sentence";
			var changedText2 = "THE EXAMPLE SENTENCE";
			var segmentNumber = 1;

			_lecturesTab.OpenLecture();

			_editorPage
				.FillSegmentTargetField(sourceText, segmentNumber)
				.SelectAllTextByHotkey(segmentNumber)
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage
				.SelectAllTextByHotkey(segmentNumber)
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseTextHotkeyTest()
		{
			var sourceText = "the example sentence";
			var changedText1 = "The Example Sentence";
			var changedText2 = "THE EXAMPLE SENTENCE";
			var segmentNumber = 1;

			_lecturesTab.OpenLecture();

			_editorPage
				.FillSegmentTargetField(sourceText, segmentNumber)
				.SelectAllTextByHotkey(segmentNumber)
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage
				.SelectAllTextByHotkey(segmentNumber)
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseSomeWordButtonTest()
		{
			var sourceText = "some words for example";
			var changedText1 = "some words for Example";
			var changedText2 = "some words for EXAMPLE";
			var segmentNumber = 1;

			_lecturesTab.OpenLecture();

			_editorPage
				.FillSegmentTargetField(sourceText, segmentNumber)
				.SelectLastWordByHotkey(segmentNumber)
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage
				.SelectLastWordByHotkey(segmentNumber)
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseSomeWordHotkeyTest()
		{
			var sourceText = "some words for example";
			var changedText1 = "some words for Example";
			var changedText2 = "some words for EXAMPLE";
			var segmentNumber = 1;

			_lecturesTab.OpenLecture();

			_editorPage
				.FillSegmentTargetField(sourceText, segmentNumber)
				.SelectLastWordByHotkey(segmentNumber)
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage
				.SelectLastWordByHotkey(segmentNumber)
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseFirstWordButtonTest()
		{
			var sourceText = "some words for example";
			var changedText1 = "Some words for example";
			var changedText2 = "SOME words for example";
			var segmentNumber = 1;

			_lecturesTab.OpenLecture();

			_editorPage
				.FillSegmentTargetField(sourceText, segmentNumber)
				.SelectFirstWordInSegment(segmentNumber, segmentType: SegmentType.Target)
				.ClickChangeCaseButton();

			Assert.AreEqual(
				changedText1,
				_editorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage
				.SelectFirstWordInSegment(segmentNumber, segmentType: SegmentType.Target)
				.ClickChangeCaseButton();

			Assert.AreEqual(
				changedText2,
				_editorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseFirstWordHotkeyTest()
		{
			var sourceText = "some words for example";
			var changedText1 = "Some words for example";
			var changedText2 = "SOME words for example";
			var segmentNumber = 1;

			_lecturesTab.OpenLecture();

			_editorPage
				.FillSegmentTargetField(sourceText, segmentNumber)
				.SelectFirstWordInSegment(segmentNumber, segmentType: SegmentType.Target)
				.PressChangeCaseHotKey();

			Assert.AreEqual(
				changedText1,
				_editorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage
				.SelectFirstWordInSegment(segmentNumber, segmentType: SegmentType.Target)
				.PressChangeCaseHotKey();

			Assert.AreEqual(
				changedText2,
				_editorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void AddLineBreakButtonTest()
		{
			_lecturesTab.OpenLecture();

			_editorPage
				.FillSegmentTargetField()
				.ClickAddLineBreakButton();

			Assert.IsTrue(_editorPage.IsLineBreakSymbolDisplayed(),
				"Произошла ошибка: в сегменте не отобразлся символ переноса строки.");
		}

		[Test]
		public void AddLineBreakHotkeyTest()
		{
			_lecturesTab.OpenLecture();

			_editorPage
				.FillSegmentTargetField()
				.ClickAddLineBreakHotkey();

			Assert.IsTrue(_editorPage.IsLineBreakSymbolDisplayed(),
				"Произошла ошибка: в сегменте не отобразлся символ переноса строки.");
		}

		[Test]
		public void ConfirmButtonTest()
		{
			_lecturesTab.OpenLecture();

			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation();

			Assert.IsTrue(_editorPage.IsSegmentCrowdConfirmed(),
				"Произошла ошибка:\n не удалось подтвердить сегмент");
		}

		[Test]
		public void ConfirmByEnterTest()
		{
			_lecturesTab.OpenLecture();

			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentByEnterHotkeys();

			Assert.IsTrue(_editorPage.IsSegmentCrowdConfirmed(),
				"Произошла ошибка:\n не удалось подтвердить сегмент");
		}

		[Test]
		public void ConfirmHotkeyCtrlEnterTest()
		{
			_lecturesTab.OpenLecture();

			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentByHotkeys();

			Assert.IsTrue(_editorPage.IsSegmentCrowdConfirmed(),
				"Произошла ошибка:\n не удалось подтвердить сегмент");
		}

		[Test]
		public void ConfirmByTickButtonTest()
		{
			_lecturesTab.OpenLecture();

			_editorPage
				.FillSegmentTargetField()
				.ClickSegmentConfirmTick();

			Assert.IsTrue(_editorPage.IsSegmentCrowdConfirmed(),
				"Произошла ошибка:\n не удалось подтвердить сегмент");
		}
	}
}

