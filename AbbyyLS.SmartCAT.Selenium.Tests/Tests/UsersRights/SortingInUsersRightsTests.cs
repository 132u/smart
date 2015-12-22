using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	internal class SortingInUsersRightsTests<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSortingInUsersRightsTests()
		{
			_usersTab = new UsersTab(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_workspaceHelper.GoToUsersPage();
		}

		[Test]
		public void SortByFirstNameTest()
		{
			_usersTab.ClickSortByFirstName();

			Assert.IsFalse(_usersTab.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByLastNameTest()
		{
			_usersTab.ClickSortByLastName();

			Assert.IsFalse(_usersTab.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByShortNameTest()
		{
			_usersTab.ClickSortByShortName();

			Assert.IsFalse(_usersTab.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByEmailAddressTest()
		{
			_usersTab.ClickSortByEmailAddress();

			Assert.IsFalse(_usersTab.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test, Ignore("PRX-9311")]
		public void SortByGroupsTest()
		{
			_usersTab.ClickSortByGroups();

			Assert.IsFalse(_usersTab.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByCreatedTest()
		{
			_usersTab.ClickSortByCreated();

			Assert.IsFalse(_usersTab.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		[NotSupportedWithDomainAuthentication]
		public void SortByStatusTest()
		{
			_usersTab.ClickSortByStatus();

			Assert.IsFalse(_usersTab.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		private WorkspaceHelper _workspaceHelper;
		private UsersTab _usersTab;
	}
}