using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorChangeCaseButtonsTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpEditorChangeCaseButtonsTests()
		{
			_segmentNumber = 1;
		}

		[Test]
		public void ChangeCaseByButtonTest()
		{
			var sourceText = "the example sentence";
			var changedText1 = "The Example Sentence";
			var changedText2 = "THE EXAMPLE SENTENCE";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectAllTextByHotkey(_segmentNumber)
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.ClickChangeCaseButton();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseByHotkeyTest()
		{
			var sourceText = "the example sentence";
			var changedText1 = "The Example Sentence";
			var changedText2 = "THE EXAMPLE SENTENCE";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectAllTextByHotkey(_segmentNumber)
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.PressChangeCaseHotKey();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseLastWordByButtonTest()
		{
			var sourceText = "some words for example";
			var changedText1 = "some words for Example";
			var changedText2 = "some words for EXAMPLE";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectLastWordByHotkey(_segmentNumber)
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.ClickChangeCaseButton();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseLastWordByHotkeyTest()
		{
			var sourceText = "some words for example";
			var changedText1 = "some words for Example";
			var changedText2 = "some words for EXAMPLE";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectLastWordByHotkey(_segmentNumber)
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.PressChangeCaseHotKey();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}
		
		[Test]
		public void ChangeCaseFirstWordByButtonTest()
		{
			var sourceText = "some words for example";
			var changedText1 = "Some words for example";
			var changedText2 = "SOME words for example";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectFirstWordInSegment(_segmentNumber, segmentType: SegmentType.Target)
				.ClickChangeCaseButton();

			Assert.AreEqual(
				changedText1,
				_editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.ClickChangeCaseButton();

			Assert.AreEqual(
				changedText2,
				_editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseFirstWordByHotkeyTest()
		{
			var sourceText = "some words for example";
			var changedText1 = "Some words for example";
			var changedText2 = "SOME words for example";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectFirstWordInSegment(_segmentNumber, segmentType: SegmentType.Target)
				.PressChangeCaseHotKey();

			Assert.AreEqual(
				changedText1,
				_editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.PressChangeCaseHotKey();

			Assert.AreEqual(
				changedText2,
				_editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseHyphenWordTestByHotKey()
		{
			var sourceText = "some-words for example";
			var changedText1 = "Some-Words for example";
			var changedText2 = "SOME-WORDS for example";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectWordPartBeforeSpaceByHotkey("some-words for example")
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.PressChangeCaseHotKey();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseHyphenWordTestByButton()
		{
			var sourceText = "some-words for example";
			var changedText1 = "Some-Words for example";
			var changedText2 = "SOME-WORDS for example";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectWordPartBeforeSpaceByHotkey("some-words for example")
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.ClickChangeCaseButton();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCasePartWordTestByButton()
		{
			var sourceText = "some words for example";
			var changedText1 = "some words for eXAMple";
			var changedText2 = "some words for example";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectFewSymbolsInLastWordByHotkey(_segmentNumber, symbolsCount: 3)
				.ClickChangeCaseButton();

			Assert.AreEqual(
				changedText1,
				_editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.ClickChangeCaseButton();

			Assert.AreEqual(
				changedText2,
				_editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCasePartWordTestByHotkey()
		{
			var sourceText = "some words for example";
			var changedText1 = "some words for eXAMple";
			var changedText2 = "some words for example";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectFewSymbolsInLastWordByHotkey(_segmentNumber, symbolsCount: 3)
				.PressChangeCaseHotKey();

			Assert.AreEqual(
				changedText1,
				_editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.PressChangeCaseHotKey();

			Assert.AreEqual(
				changedText2,
				_editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}
		
		[Test]
		public void ChangeCaseSecondThirdWordsByHotkeyNonStandardTest()
		{
			var sourceText = "some wOrDs fOr example";
			var changedText1 = "some words for example";
			var changedText2 = "some Words For example";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectSecondThirdWordsByHotkey(_segmentNumber)
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.PressChangeCaseHotKey();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseSecondThirdWordsByButtonNonStandardTest()
		{
			var sourceText = "some wOrDs fOr example";
			var changedText1 = "some words for example";
			var changedText2 = "some Words For example";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectSecondThirdWordsByHotkey(_segmentNumber)
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.ClickChangeCaseButton();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Ignore("PRX-4034")]
		[Test]
		public void ChangeCaseFirstWordByButtonNonStandardTest()
		{
			var sourceText = "sOMe words for example";
			var changedText1 = "Some words for example";
			var changedText2 = "SOME Words For example";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectFirstWordInSegment(_segmentNumber, segmentType: SegmentType.Target)
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.ClickChangeCaseButton();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseFirstWordByButtonNonStandardTestCurrentRealizatioin()
		{
			var sourceText = "sOMe words for example";
			var changedText1 = "SOME words for example";
			var changedText2 = "some words for example";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectFirstWordInSegment(_segmentNumber, segmentType: SegmentType.Target)
				.ClickChangeCaseButton();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.ClickChangeCaseButton();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Test]
		public void ChangeCaseFirstWordByHotkeyNonStandardTestCurrentRealizatioin()
		{
			var sourceText = "sOMe words for example";
			var changedText1 = "SOME words for example";
			var changedText2 = "some words for example";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectFirstWordInSegment(_segmentNumber, segmentType: SegmentType.Target)
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.PressChangeCaseHotKey();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		[Ignore("PRX-4034")]
		[Test]
		public void ChangeCaseFirstWordByHotkeyNonStandardTest()
		{
			var sourceText = "sOMe words for example";
			var changedText1 = "Some words for example";
			var changedText2 = "SOME Words For example";

			_editorPage
				.FillSegmentTargetField(sourceText, _segmentNumber)
				.SelectFirstWordInSegment(_segmentNumber, segmentType: SegmentType.Target)
				.PressChangeCaseHotKey();

			Assert.AreEqual(changedText1, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");

			_editorPage.PressChangeCaseHotKey();

			Assert.AreEqual(changedText2, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете.");
		}

		private int _segmentNumber;
		private string _sourceText;
		private string _textAfterFirstChange;
		private string _textAfterSecondChange;
	}
}
