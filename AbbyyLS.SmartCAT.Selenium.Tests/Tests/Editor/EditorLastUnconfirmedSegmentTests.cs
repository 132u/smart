using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorLastUnconfirmedSegmentTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test(Description = "Проверяет переход к следующему неподтвержденному сегменту с помощью кнопки (при 2 сегментах)")]
		public void LastUnconfirmedSegmentButtonTest()
		{
			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment()
				.ClickLastUnconfirmedButton();

			Assert.IsTrue(_editorPage.IsSegmentSelected(segmentNumber: 2),
				"Произошла ошибка:\n сегмент не выделен, не подсвечен голубым цветом.");
		}

		[Test(Description = "Проверяет переход к следующему неподтвержденному нажатием F9 (при 2 сегментах)")]
		public void LastUnconfirmedSegmentHotKeyTest()
		{
			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment()
				.SelectLastUnconfirmedSegmentByHotKey();

			Assert.IsTrue(_editorPage.IsSegmentSelected(segmentNumber: 2),
				"Произошла ошибка:\n сегмент не выделен, не подсвечен голубым цветом.");
		}

		[Test(Description = "Проверяет переход к следующему неподтвержденному сегменту с помощью кнопки (при 3 сегментах)")]
		public void LastUnconfirmedButtonSkipSegmentTest()
		{
			_editorPage
				.FillSegmentTargetField(rowNumber: 2)
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment()
				.ClickLastUnconfirmedButton();

			Assert.IsTrue(_editorPage.IsSegmentSelected(segmentNumber: 3),
				"Произошла ошибка:\n сегмент не выделен, не подсвечен голубым цветом.");
		}

		[Test(Description = "Проверяет переход к следующему неподтвержденному нажатием F9 (при 3 сегментах)")]
		public void LastUnconfirmedHotkeySkipSegmentTest()
		{
			_editorPage
				.FillSegmentTargetField(rowNumber: 2)
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment()
				.SelectLastUnconfirmedSegmentByHotKey();

			Assert.IsTrue(_editorPage.IsSegmentSelected(segmentNumber: 3),
				"Произошла ошибка:\n сегмент не выделен, не подсвечен голубым цветом.");
		}
	}
}
