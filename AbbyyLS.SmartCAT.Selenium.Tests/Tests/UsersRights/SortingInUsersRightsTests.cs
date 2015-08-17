using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights
{
	[Standalone]
	internal class SortingInUsersRightsTests<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSortingInUsersRightsTests()
		{
			_usersRightsHelper.GoToUsersRightsPage();
		}

		[Test]
		public void SortByFirstNameTest()
		{
			_usersRightsHelper
				.ClickSortByFirstName()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByLastNameTest()
		{
			_usersRightsHelper
				.ClickSortByLastName()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByShortNameTest()
		{
			_usersRightsHelper
				.ClickSortByShortName()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByEmailAddressTest()
		{
			_usersRightsHelper
				.ClickSortByEmailAddress()
				.AssertAlertNoExist();
		}

		[Test, Explicit("[PRX-9311]")]
		public void SortByGroupsTest()
		{
			_usersRightsHelper
				.ClickSortByGroups()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByCreatedTest()
		{
			_usersRightsHelper
				.ClickSortByCreated()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByStatusTest()
		{
			_usersRightsHelper
				.ClickSortByStatus()
				.AssertAlertNoExist();
		}

		private UsersRightsHelper _usersRightsHelper = new UsersRightsHelper();
	}
}