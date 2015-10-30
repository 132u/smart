using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.ProjectGroups
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[ProjectGroups]
	internal class SortingInProjectGroups<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void Initialization()
		{
			_projectGroupsHelper = new ProjectGroupsHelper(Driver);
			_projectGroupsPage = new ProjectGroupsPage(Driver);
		}

		[Test]
		public void SortByNameTest()
		{
			_projectGroupsHelper
				.GoToProjectGroupsPage()
				.ClickSortByName();

			Assert.IsFalse(_projectGroupsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		private ProjectGroupsHelper _projectGroupsHelper;
		private ProjectGroupsPage _projectGroupsPage;
	}
}
