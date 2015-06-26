using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
    public class EditorErrorTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void FindErrorButtonTest()
		{
			EditorHelper
				.ClickFindErrorsButton();
		}

		[Test]
		public void FindErrorHotkeyTest()
		{
			EditorHelper
				.FindErrorsByHotkeys();
		}
	}
}
