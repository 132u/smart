using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorConcordanceSearchTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void ConcordanceSearchButtonTest()
		{
			EditorHelper.OpenConcordanceSearch();
		}

		[Test]
		public void ConcordanceSearchHotkeyTest()
		{
			EditorHelper.OpenConcordanceSearchByHotKey();
		}
	}
}
