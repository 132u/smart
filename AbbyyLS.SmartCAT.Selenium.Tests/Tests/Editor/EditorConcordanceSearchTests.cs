using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Standalone]
	public class EditorConcordanceSearchTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void ConcordanceSearchButtonTest()
		{
			EditorHelper.OpenConcordanceSearch();
		}

		[Test]
		[Explicit("Тесты, использующие hotkey, не работают на тимсити")]
		public void ConcordanceSearchHotkeyTest()
		{
			EditorHelper.OpenConcordanceSearchByHotKey();
		}
	}
}
