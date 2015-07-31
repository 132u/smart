using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	public class EditorLastUnconfirmedSegmentTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void LastUnconfirmedSegmentButtonTest()
		{
			EditorHelper
				.AddTextToSegment()
				.ConfirmTranslation()
				.AssertIsSegmentConfirmed()
				.ClickTargetCell()
				.ClickLastUnconfirmedButton()
				.AssertSegmentIsSelected(2);
		}

		[Test]
		[Explicit("Тесты, использующие hotkey, не работают на тимсити")]
		public void LastUnconfirmedSegmentHotKeyTest()
		{
			EditorHelper
				.AddTextToSegment()
				.ConfirmTranslation()
				.AssertIsSegmentConfirmed()
				.ClickTargetCell()
				.ClickLastUnconfirmedHotKey()
				.AssertSegmentIsSelected(rowNumber: 2);
		}

		[Test]
		public void LastUnconfirmedButtonSkipSegmentTest()
		{
			EditorHelper
				.AddTextToSegment(rowNumber: 2)
				.ConfirmTranslation()
				.AssertIsSegmentConfirmed(rowNumber: 2)
				.ClickTargetCell(rowNumber: 1)
				.ClickLastUnconfirmedButton()
				.AssertSegmentIsSelected(rowNumber: 3);
		}

		[Test]
		[Explicit("Тесты, использующие hotkey, не работают на тимсити")]
		public void LastUnconfirmedHotkeySkipSegmentTest()
		{
			EditorHelper
				.AddTextToSegment(rowNumber: 2)
				.ConfirmTranslation()
				.AssertIsSegmentConfirmed(rowNumber: 2)
				.ClickTargetCell(rowNumber: 1)
				.ClickLastUnconfirmedHotKey()
				.AssertSegmentIsSelected(rowNumber: 3);
		}
	}
}
