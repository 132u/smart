using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Coursera]
	class LeaderboardTests<TWebDriverProvider> : CourseraBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void LeaderboardTestsSetUp()
		{
			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();

			_editorPage
				.FillTarget(_translationText)
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

		[Test, Ignore("PRX-15404")]
		public void UserPositionNubmerMatchTest()
		{
			var positionNumberInLeaderboard = _leaderboardPage.GetUserLeaderboardPositionNumber(CourseraCrowdsourceUser.NickName);

			_header.GoToUserProfile();

			var positionNumberInProfile = _profilePage.GetUserProfilePositionNumber();

			Assert.AreEqual(positionNumberInLeaderboard, positionNumberInProfile,
				"Произошла ошибка:\nНомер позиции пользователя {0} в списке лидеров и в профиле отличается.", CourseraCrowdsourceUser.NickName);
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
			_leaderboardPage.SelectCourse(_courseraProject);

			Assert.IsTrue(_leaderboardPage.IsUserNameDisplayed(CourseraCrowdsourceUser.NickName),
				"Произошла ошибка:\nПользователя нет в списке лидеров.");
		}

		[Test, Ignore("Необходимо уточнение по логике")]
		public void CourseLeaderboardCompareRating()
		{
			var commonRating = _leaderboardPage.GetUserLeaderboardRating(CourseraCrowdsourceUser.NickName);

			_leaderboardPage.SelectCourse(_courseraProject);

			var courseRating = _leaderboardPage.GetUserLeaderboardRating(CourseraCrowdsourceUser.NickName);

			Assert.Less(courseRating, commonRating,
				"Произошла ошибка:\nОбщий рейтинг меньше рейтинга курса.");
		}

		private string _courseraProject;
	}
}
