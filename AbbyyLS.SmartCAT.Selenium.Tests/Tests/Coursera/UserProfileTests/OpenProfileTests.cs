using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Coursera]
	class OpenProfileTests<TWebDriverProvider> : CourseraBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void OpenProfileTestsSetUp()
		{
			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);
		}
		
		[Test]
		public void OpenProfileFromHomePage()
		{
			_courseraHomePage.ClickProfile();

			Assert.IsTrue(_profilePage.IsUserProfilePageOpened(),
				"Произошла ошибка:\nНе открылась страница пользователя.");
		}

		[Test]
		public void OpenProfileFromHeaderMenuPage()
		{
			_header.GoToUserProfile();

			Assert.IsTrue(_profilePage.IsUserProfilePageOpened(),
				"Произошла ошибка:\nНе открылась страница пользователя.");
		}

		[Test]
		public void OpenProfileFromAnotherUserPage()
		{
			_courseraHomePage.ClickProfile();
			_header.GoToLeaderboardPage();

			var userName = _leaderboardPage.GetUserNameInList();
			_leaderboardPage.ClickUserName(userName);

			Assert.AreEqual(userName, _profilePage.GetNickname(),
				"Произошла ошибка:\nНеверное имя пользователя.");

			_header.GoToUserProfile();

			Assert.AreEqual(CourseraCrowdsourceUser.NickName, _profilePage.GetNickname(),
				"Произошла ошибка:\nНеверное имя пользователя.");
		}

		[Test]
		public void OpenProfileFromLeaderboard()
		{
			_courseraHomePage.ClickSelectCourse();

			_coursesPage.GoToLeaderboardPage();

			_leaderboardPage.ClickUserName(CourseraCrowdsourceUser.NickName);

			Assert.IsTrue(_profilePage.IsUserProfilePageOpened(),
				"Произошла ошибка:\nНе открылась страница пользователя.");
		}
	}
}
