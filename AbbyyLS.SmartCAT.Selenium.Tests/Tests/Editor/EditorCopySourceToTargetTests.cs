using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Standalone]
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
		[Explicit("Тесты, использующие hotkey, не работают на тимсити")]
		public void CopySourceToTargetHotkeyTest()
		{
			EditorHelper
				.CopySourceToTargetByHotKey()
				.AssertSourceEqualsTarget();
		}
	}
}
