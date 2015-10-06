using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorErrorTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void FindErrorButtonTest()
		{
			EditorHelper.ClickFindErrorsButton();
		}

		[Test]
		public void FindErrorHotkeyTest()
		{
			EditorHelper.FindErrorsByHotkeys();
		}
	}
}
