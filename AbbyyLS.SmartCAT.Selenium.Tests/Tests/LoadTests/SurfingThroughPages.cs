using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.LoadTests
{
	[LoadTests]
	[Parallelizable(ParallelScope.Fixtures)]
	class SurfingThroughPages<TWebDriverProvider>
		: BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_workspacePage = new WorkspacePage(Driver);
		}

		[Test]
		public void SurfingThroughPagesTest()
		{
			_workspacePage
				.GoToGlossariesPage()
				.GoToTranslationMemoriesPage()
				.GoToSearchPage()
				.GoToLingvoDictionariesPage()
				.GoToUsersPage();
		}

		private WorkspacePage _workspacePage;
	}
}
