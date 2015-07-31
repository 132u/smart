using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	public class EditorCopySourceToTargetTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CopySourceToTargetButtonTest()
		{
			EditorHelper
				.CopySourceToTarget()
				.AssertSourceEqualsTarget();
		}

		[Test]
		[Category("SCAT_102")]
		public void CopySourceToTargetHotkeyTest()
		{
			EditorHelper
				.CopySourceToTargetByHotKey()
				.AssertSourceEqualsTarget();
		}
	}
}
