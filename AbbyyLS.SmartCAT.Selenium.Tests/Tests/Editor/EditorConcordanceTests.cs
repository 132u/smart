using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	public class EditorConcordanceTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void ConcordanceSearchButtonTest()
		{
			EditorHelper.OpenConcordanceSearch();
		}

		[Test]
		[Category("SCAT_102")]
		public void ConcordanceSearchHotkeyTest()
		{
			EditorHelper.OpenConcordanceSearchByHotKey();
		}
	}
}
