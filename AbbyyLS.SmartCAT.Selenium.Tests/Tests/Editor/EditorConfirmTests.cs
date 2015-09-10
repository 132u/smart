using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[TestFixture]
	[Standalone]
	public class EditorConfirmTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{

		[Test]
		public void ConfirmButtonTest()
		{
			EditorHelper
				.AddTextToSegment()
				.CloseTutorialIfExist()
				.ConfirmTranslation()
				.AssertIsSegmentConfirmed();
		}

		[Test]
		[Explicit("Тесты, использующие hotkey, не работают на тимсити")]
		public void ConfirmHotkeyTest()
		{
			EditorHelper
				.AddTextToSegment("Some words for example")
				.ConfirmTranslationByHotkeys()
				.AssertIsSegmentConfirmed();
		}

	}
}
