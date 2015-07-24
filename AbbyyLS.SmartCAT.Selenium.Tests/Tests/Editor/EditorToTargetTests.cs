using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	public class EditorToTargetTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void ToTargetButtonTest()
		{
			EditorHelper.CopySourceToTarget();
		}

		[Test]
		[Category("SCAT_102")]
		public void ToTargetHotkeyTest()
		{
			EditorHelper.CopySourceToTargetByHotKey();
		}
	}
}
