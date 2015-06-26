using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	public class EditorConfirmTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{

		[Test]
		public void ConfirmButtonTest()
		{
			EditorHelper
				.AddTextToSegment()
				.ConfirmTranslation()
				.AssertIsSegmentConfirmed();
		}

		[Test]
		[Explicit("Тест использует хоткеи и не прогоняется в тимсити")]
		public void ConfirmHotkeyTest()
		{
			EditorHelper
				.AddTextToSegment("Some words for example")
				.ConfirmTranslationByHotkeys()
				.AssertIsSegmentConfirmed();
		}

	}
}
