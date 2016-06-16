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

			_courseraEditorPage.ClickHomeButtonExpectingCourseraCoursesPage();

			Assert.IsTrue(_coursesPage.IsCoursesPageOpened(),
				"Произошла ошибка: не открылась страница курсов.");
		}

		[Test]
		public void ToTargetButtonTest()
		{
			_lecturesTab.OpenLecture();

			var sourceText = _courseraEditorPage.GetSourceText(rowNumber: 1);

			_courseraEditorPage
				.ClickOnTargetCellInSegment()
				.ClickCopySourceToTargetButton()
				.ConfirmSegmentTranslation();

			Assert.AreEqual(sourceText, _courseraEditorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка:\n Неверный текст в таргете сегмента №1."); ;
		}

		[Test]
		public void ToTargetHotkeyTest()
		{
			_lecturesTab.OpenLecture();

			var sourceText = _courseraEditorPage.GetSourceText(rowNumber: 1);

			_courseraEditorPage
				.ClickOnTargetCellInSegment()
				.CopySourceToTargetHotkey()
				.ConfirmSegmentTranslation();

			Assert.AreEqual(sourceText, _courseraEditorPage.GetTargetText(rowNumber: 1),
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

			_courseraEditorPage
				.FillSegmentTargetField(sourceText, segmentNumber)
				.SelectAllTextByHotkey(segmentNumber)
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText1, _courseraEditorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_courseraEditorPage
				.SelectAllTextByHotkey(segmentNumber)
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText2, _courseraEditorPage.GetTargetText(segmentNumber),
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

			_courseraEditorPage
				.FillSegmentTargetField(sourceText, segmentNumber)
				.SelectAllTextByHotkey(segmentNumber)
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText1, _courseraEditorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_courseraEditorPage
				.SelectAllTextByHotkey(segmentNumber)
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText2, _courseraEditorPage.GetTargetText(segmentNumber),
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

			_courseraEditorPage
				.FillSegmentTargetField(sourceText, segmentNumber)
				.SelectLastWordByHotkey(segmentNumber)
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText1, _courseraEditorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_courseraEditorPage
				.SelectLastWordByHotkey(segmentNumber)
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText2, _courseraEditorPage.GetTargetText(segmentNumber),
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

			_courseraEditorPage
				.FillSegmentTargetField(sourceText, segmentNumber)
				.SelectLastWordByHotkey(segmentNumber)
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText1, _courseraEditorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_courseraEditorPage
				.SelectLastWordByHotkey(segmentNumber)
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText2, _courseraEditorPage.GetTargetText(segmentNumber),
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

			_courseraEditorPage
				.FillSegmentTargetField(sourceText, segmentNumber)
				.SelectFirstWordInSegment(segmentNumber, segmentType: SegmentType.Target)
				.ClickChangeCaseButton();

			Assert.AreEqual(
				changedText1,
				_courseraEditorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_courseraEditorPage
				.SelectFirstWordInSegment(segmentNumber, segmentType: SegmentType.Target)
				.ClickChangeCaseButton();

			Assert.AreEqual(
				changedText2,
				_courseraEditorPage.GetTargetText(segmentNumber),
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

			_courseraEditorPage
				.FillSegmentTargetField(sourceText, segmentNumber)
				.SelectFirstWordInSegment(segmentNumber, segmentType: SegmentType.Target)
				.PressChangeCaseHotKey();

			Assert.AreEqual(
				changedText1,
				_courseraEditorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_courseraEditorPage
				.SelectFirstWordInSegment(segmentNumber, segmentType: SegmentType.Target)
				.PressChangeCaseHotKey();

			Assert.AreEqual(
				changedText2,
				_courseraEditorPage.GetTargetText(segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void AddLineBreakButtonTest()
		{
			_lecturesTab.OpenLecture();

			_courseraEditorPage
				.FillSegmentTargetField()
				.ClickAddLineBreakButton();

			Assert.IsTrue(_courseraEditorPage.IsLineBreakSymbolDisplayed(),
				"Произошла ошибка: в сегменте не отобразлся символ переноса строки.");
		}

		[Test]
		public void AddLineBreakHotkeyTest()
		{
			_lecturesTab.OpenLecture();

			_courseraEditorPage
				.FillSegmentTargetField()
				.ClickAddLineBreakHotkey();

			Assert.IsTrue(_courseraEditorPage.IsLineBreakSymbolDisplayed(),
				"Произошла ошибка: в сегменте не отобразлся символ переноса строки.");
		}

		[Test]
		public void ConfirmButtonTest()
		{
			_lecturesTab.OpenLecture();

			_courseraEditorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation();

			Assert.IsTrue(_courseraEditorPage.IsSegmentCrowdConfirmed(),
				"Произошла ошибка:\n не удалось подтвердить сегмент");
		}

		[Test]
		public void ConfirmByEnterTest()
		{
			_lecturesTab.OpenLecture();

			_courseraEditorPage
				.FillSegmentTargetField()
				.ConfirmSegmentByEnterHotkeys();

			Assert.IsTrue(_courseraEditorPage.IsSegmentCrowdConfirmed(),
				"Произошла ошибка:\n не удалось подтвердить сегмент");
		}

		[Test]
		public void ConfirmHotkeyCtrlEnterTest()
		{
			_lecturesTab.OpenLecture();

			_courseraEditorPage
				.FillSegmentTargetField()
				.ConfirmSegmentByHotkeys();

			Assert.IsTrue(_courseraEditorPage.IsSegmentCrowdConfirmed(),
				"Произошла ошибка:\n не удалось подтвердить сегмент");
		}
	}
}

