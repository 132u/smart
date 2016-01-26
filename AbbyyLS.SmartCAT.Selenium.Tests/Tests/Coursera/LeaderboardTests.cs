using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	class LeaderboardTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public LeaderboardTests()
		{
			StartPage = StartPage.Coursera;
		}

		[SetUp]
		public void LeaderboardTestsSetUp()
		{
			_courseraHomePage = new CourseraHomePage(Driver);
			_leaderboardPage = new LeaderboardPage(Driver);
			_coursesPage = new CoursesPage(Driver);
			_header = new HeaderMenu(Driver);
			_coursePage = new CoursePage(Driver);
			_editorPage = new EditorPage(Driver);
			_profilePage = new ProfilePage(Driver);

			CourseraCrowdsourceUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);

			_courseraHomePage.ClickSelectCourse();
			_coursesPage.ClickCourse(CreateProjectHelper.CourseraProjectName);
			_coursePage.OpenLecture();

			_editorPage
				.FillTarget("coursera test")
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.ClickLeaderboardMenu();
		}
		
		[Test]
		public void UserNameExistInLeaderboardTest()
		{
			Assert.IsTrue(_leaderboardPage.IsUserNameDisplayed(CourseraCrowdsourceUser.NickName),
				"Произошла ошибка:\nПользователь {0} отсутствует в списке лидеров.", CourseraCrowdsourceUser.NickName);
		}

		[Test]
		public void UserPositionNubmerMatchTest()
		{
			var positionNumberInLeaderboard = _leaderboardPage.GetUserLeaderboardPositionNumber(CourseraCrowdsourceUser.NickName);

			_header.ClickProfile();

			var positionNumberInProfile = _profilePage.GetUserProfilePositionNumber();

			Assert.AreEqual(positionNumberInLeaderboard, positionNumberInProfile,
				"Произошла ошибка:\nНомер позиции пользователя в списке лидеров и в профиле отличается.");
		}

		[Test]
		public void UserRatingMatchTest()
		{
			var ratingInLeaderboard = _leaderboardPage.GetUserLeaderboardRating(CourseraCrowdsourceUser.NickName);

			_header.ClickProfile();

			var ratingInProfile = _profilePage.GetUserProfileRating();

			Assert.AreEqual(ratingInLeaderboard, ratingInProfile,
				"Произошла ошибка:\nРейтинг пользователя в списке лидеров и в профиле отличается.");
		}

		[Test, Ignore("Необходимо уточнение по логике")]
		public void CourseLeaderboardTest()
		{
			_leaderboardPage.SelectCourse(CreateProjectHelper.CourseraProjectName);

			Assert.IsTrue(_leaderboardPage.IsUserNameDisplayed(CourseraCrowdsourceUser.NickName),
				"Произошла ошибка:\nПользователя нет в списке лидеров.");
		}

		[Test, Ignore("Необходимо уточнение по логике")]
		public void CourseLeaderboardCompareRating()
		{
			var commonRating = _leaderboardPage.GetUserLeaderboardRating(CourseraCrowdsourceUser.NickName);

			_leaderboardPage.SelectCourse(CreateProjectHelper.CourseraProjectName);

			var courseRating = _leaderboardPage.GetUserLeaderboardRating(CourseraCrowdsourceUser.NickName);

			Assert.Less(courseRating, commonRating,
				"Произошла ошибка:\nОбщий рейтинг меньше рейтинга курса.");
		}

		private EditorPage _editorPage;
		private HeaderMenu _header;
		private ProfilePage _profilePage;
		private CourseraHomePage _courseraHomePage;
		private LeaderboardPage _leaderboardPage;
		private CoursesPage _coursesPage;
		private CoursePage _coursePage;
	}
}
