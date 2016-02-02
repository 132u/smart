using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Coursera]
	[Ignore("Курсерные тесты отключены за нестабильность")]
	class LeaderboardTests<TWebDriverProvider> : CourseraBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void LeaderboardTestsSetUp()
		{
			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);
			_courseraHomePage.ClickSelectCourse();
			_coursesPage.ClickCourse(CreateProjectHelper.CourseraProjectName);
			_coursePage.OpenLecture();

			_editorPage.FillTarget("coursera test")
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.GoToLeaderboardPage();
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

			_header.GoToUserProfile();

			var positionNumberInProfile = _profilePage.GetUserProfilePositionNumber();

			Assert.AreEqual(positionNumberInLeaderboard, positionNumberInProfile,
				"Произошла ошибка:\nНомер позиции пользователя в списке лидеров и в профиле отличается.");
		}

		[Test]
		public void UserRatingMatchTest()
		{
			var ratingInLeaderboard = _leaderboardPage.GetUserLeaderboardRating(CourseraCrowdsourceUser.NickName);

			_header.GoToUserProfile();

			var ratingInProfile = _profilePage.GetUserProfileRating();

			Assert.AreEqual(ratingInLeaderboard, ratingInProfile,
				"Произошла ошибка:\nРейтинг пользователя в списке лидеров и в профиле отличается.");
		}

		[Test, Ignore("Необходимо уточнение по логике")]
		public void CourseLeaderboardTest()
		{
			_header.GoToLeaderboardPage();
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
	}
}
