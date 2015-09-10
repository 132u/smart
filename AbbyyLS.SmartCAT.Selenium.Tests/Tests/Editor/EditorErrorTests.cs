using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[TestFixture]
	[Standalone]
	public class EditorErrorTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void FindErrorButtonTest()
		{
			EditorHelper.ClickFindErrorsButton();
		}

		[Test]
		[Explicit("Тест использует хоткеи и не прогоняется в тимсити")]
		public void FindErrorHotkeyTest()
		{
			EditorHelper.FindErrorsByHotkeys();
		}
	}
}
