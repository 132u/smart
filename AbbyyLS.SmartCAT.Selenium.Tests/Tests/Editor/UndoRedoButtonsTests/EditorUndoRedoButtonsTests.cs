using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class EditorUndoRedoButtonsTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void UndoRedoButtonTest()
		{
			var targetText = "some text";

			_editorPage
				.FillTarget(targetText)
				.ClickOnTargetCellInSegment()
				.ClickUndoButton();

			Assert.AreEqual(_editorPage.GetTargetText(_segmentNumber), String.Empty,
				"Произошла ошибка:\n Таргет для сегмента №{0} не очистился после клика по кнопке Отмены.", _segmentNumber);
			
			_editorPage.ClickRedoButton();

			Assert.AreEqual(targetText, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Текст в таргете для сегмента №{0} не восстановился после клика по кнопке Отмены.", _segmentNumber);
		}

		[Test]
		public void UndoRedoHotkeyTest()
		{
			var targetText = "some text";

			_editorPage
				.FillTarget(targetText)
				.ClickOnTargetCellInSegment()
				.PressUndoHotkey();

			Assert.AreEqual(_editorPage.GetTargetText(_segmentNumber), String.Empty,
				"Произошла ошибка:\n Таргет для сегмента №{0} не очистился после клика по кнопке Отмены.", _segmentNumber);

			_editorPage.PressRedoHotkey();

			Assert.AreEqual(targetText, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Текст в таргете для сегмента №{0} не восстановился после клика по кнопке Отмены.", _segmentNumber);
		}

		[Test]
		public void UndoRedoButtonAfterSegmentConfirmationTest()
		{
			var sourceText = _editorPage.GetSourceText(_segmentNumber);

			_editorPage
				.ClickCopySourceToTargetButton()
				.ConfirmSegmentTranslation()
				.ClickUndoButton();

			Assert.AreEqual(sourceText, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете сегмента №{0}.", _segmentNumber);

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(_segmentNumber),
				"Произошла ошибка:\n Сегмент №{0} подтвержден.", _segmentNumber);

			_editorPage.ClickRedoButton();

			Assert.AreEqual(sourceText, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете сегмента №{0}.", _segmentNumber);

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(_segmentNumber),
				"Произошла ошибка:\n  Сегмент №{0} не подтвержден.", _segmentNumber);
		}

		[Test]
		public void UndoRedoHotkeyAfterSegmentConfirmationTest()
		{
			var sourceText = _editorPage.GetSourceText(_segmentNumber);

			_editorPage
				.ClickCopySourceToTargetButton()
				.ConfirmSegmentTranslation()
				.PressUndoHotkey();

			Assert.AreEqual(sourceText, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете сегмента №{0}.", _segmentNumber);

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(_segmentNumber),
				"Произошла ошибка:\n Сегмент №{0} подтвержден.", _segmentNumber);

			_editorPage.PressRedoHotkey();

			Assert.AreEqual(sourceText, _editorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете сегмента №{0}.", _segmentNumber);

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(_segmentNumber),
				"Произошла ошибка:\n  Сегмент №{0} не подтвержден.", _segmentNumber);
		}

		const int _segmentNumber = 1;
	}
}
