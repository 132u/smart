using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.ProjectGroups
{
	[Standalone]
	internal class SortingInProjectGroups<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[Test]
		public void SortByNameTest()
		{
			_projectGroupsHelper
				.GoToProjectGroupsPage()
				.ClickSortByName()
				.AssertAlertNoExist();
		}

		private ProjectGroupsHelper _projectGroupsHelper = new ProjectGroupsHelper();
	}
}
