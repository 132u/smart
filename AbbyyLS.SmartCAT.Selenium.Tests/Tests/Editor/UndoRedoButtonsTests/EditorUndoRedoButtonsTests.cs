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

			EditorPage
				.FillTarget(targetText)
				.ClickOnTargetCellInSegment()
				.ClickUndoButton();

			Assert.AreEqual(EditorPage.GetTargetText(_segmentNumber), String.Empty,
				"Произошла ошибка:\n Таргет для сегмента №{0} не очистился после клика по кнопке Отмены.", _segmentNumber);
			
			EditorPage.ClickRedoButton();

			Assert.AreEqual(targetText, EditorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Текст в таргете для сегмента №{0} не восстановился после клика по кнопке Отмены.", _segmentNumber);
		}

		[Test]
		public void UndoRedoHotkeyTest()
		{
			var targetText = "some text";

			EditorPage
				.FillTarget(targetText)
				.ClickOnTargetCellInSegment()
				.PressUndoHotkey();

			Assert.AreEqual(EditorPage.GetTargetText(_segmentNumber), String.Empty,
				"Произошла ошибка:\n Таргет для сегмента №{0} не очистился после клика по кнопке Отмены.", _segmentNumber);

			EditorPage.PressRedoHotkey();

			Assert.AreEqual(targetText, EditorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Текст в таргете для сегмента №{0} не восстановился после клика по кнопке Отмены.", _segmentNumber);
		}

		[Test]
		public void UndoRedoButtonAfterSegmentConfirmationTest()
		{
			var sourceText = EditorPage.GetSourceText(_segmentNumber);

			EditorPage
				.ClickCopySourceToTargetButton()
				.ConfirmSegmentTranslation()
				.ClickUndoButton();

			Assert.AreEqual(sourceText, EditorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете сегмента №{0}.", _segmentNumber);

			Assert.IsFalse(EditorPage.IsSegmentConfirmed(_segmentNumber),
				"Произошла ошибка:\n Сегмент №{0} подтвержден.", _segmentNumber);

			EditorPage.ClickRedoButton();

			Assert.AreEqual(sourceText, EditorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете сегмента №{0}.", _segmentNumber);

			Assert.IsTrue(EditorPage.IsSegmentConfirmed(_segmentNumber),
				"Произошла ошибка:\n  Сегмент №{0} не подтвержден.", _segmentNumber);
		}

		[Test]
		public void UndoRedoHotkeyAfterSegmentConfirmationTest()
		{
			var sourceText = EditorPage.GetSourceText(_segmentNumber);

			EditorPage
				.ClickCopySourceToTargetButton()
				.ConfirmSegmentTranslation()
				.PressUndoHotkey();

			Assert.AreEqual(sourceText, EditorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете сегмента №{0}.", _segmentNumber);

			Assert.IsFalse(EditorPage.IsSegmentConfirmed(_segmentNumber),
				"Произошла ошибка:\n Сегмент №{0} подтвержден.", _segmentNumber);

			EditorPage.PressRedoHotkey();

			Assert.AreEqual(sourceText, EditorPage.GetTargetText(_segmentNumber),
				"Произошла ошибка:\n Неверный текст в таргете сегмента №{0}.", _segmentNumber);

			Assert.IsTrue(EditorPage.IsSegmentConfirmed(_segmentNumber),
				"Произошла ошибка:\n  Сегмент №{0} не подтвержден.", _segmentNumber);
		}

		const int _segmentNumber = 1;
	}
}
