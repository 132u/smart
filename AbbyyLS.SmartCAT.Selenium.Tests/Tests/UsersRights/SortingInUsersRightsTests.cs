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
			_usersRightsPage = new UsersRightsPage(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_workspaceHelper.GoToUsersRightsPage();
		}

		[Test]
		public void SortByFirstNameTest()
		{
			_usersRightsPage.ClickSortByFirstNameAssumingAlert();

			Assert.IsFalse(_usersRightsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByLastNameTest()
		{
			_usersRightsPage.ClickSortByLastNameAssumingAlert();

			Assert.IsFalse(_usersRightsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByShortNameTest()
		{
			_usersRightsPage.ClickSortByShortNameAssumingAlert();

			Assert.IsFalse(_usersRightsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByEmailAddressTest()
		{
			_usersRightsPage.ClickSortByEmailAddressAssumingAlert();

			Assert.IsFalse(_usersRightsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test, Ignore("PRX-9311")]
		public void SortByGroupsTest()
		{
			_usersRightsPage.ClickSortByGroupsAssumingAlert();

			Assert.IsFalse(_usersRightsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByCreatedTest()
		{
			_usersRightsPage.ClickSortByCreatedAssumingAlert();

			Assert.IsFalse(_usersRightsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		[NotSupportedWithDomainAuthentication]
		public void SortByStatusTest()
		{
			_usersRightsPage.ClickSortByStatusAssumingAlert();

			Assert.IsFalse(_usersRightsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		private WorkspaceHelper _workspaceHelper;
		private UsersRightsPage _usersRightsPage;
	}
}