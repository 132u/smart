using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	public class EditorLastUnconfirmedButtonTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void LastUnconfirmedButtonNextSegmentTest()
		{
			EditorHelper
				.AddTextToSegment()
				.ConfirmTranslation()
				.ClickTargetCell()
				.AssertIsSegmentConfirmed()
				.ClickLastUnconfirmedButton()
				.AssertSegmentIsSelected(2);
		}

		[Test]
		[Category("SCAT_102")]
		public void LastUnconfirmedButtonNextSegmentByHotKeyTest()
		{
			EditorHelper
				.AddTextToSegment()
				.ConfirmTranslation()
				.ClickTargetCell()
				.AssertIsSegmentConfirmed()
				.ClickLastUnconfirmedButtonByHotKey()
				.AssertSegmentIsSelected(rowNumber: 2);
		}

		[Test]
		public void LastUnconfirmedButtonSkipSegmentTest()
		{
			EditorHelper
				.AddTextToSegment(rowNumber: 2)
				.ConfirmTranslation()
				.ClickTargetCell(rowNumber: 2)
				.AssertIsSegmentConfirmed(rowNumber: 2)
				.ClickLastUnconfirmedButton()
				.AssertSegmentIsSelected(rowNumber: 3);
		}

		[Test]
		[Category("SCAT_102")]
		public void LastUnconfirmedButtonHotkeySkipSegmentTest()
		{
			EditorHelper
				.AddTextToSegment(rowNumber: 2)
				.ConfirmTranslation()
				.ClickTargetCell(rowNumber: 2)
				.AssertIsSegmentConfirmed(rowNumber: 2)
				.ClickLastUnconfirmedButtonByHotKey()
				.AssertSegmentIsSelected(rowNumber: 3);

		}
	}
}
