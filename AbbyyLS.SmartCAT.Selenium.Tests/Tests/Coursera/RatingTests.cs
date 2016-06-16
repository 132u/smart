using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Coursera]
	class RatingTests<TWebDriverProvider> : CourseraBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void RatingTestsSetUp()
		{
			_courseraHomePage.ClickProfile();
		}

		[Test, Ignore("PRX-14792")]
		public void RatingAfterTranslationTest()
		{
			var ratingBeforeTranslation = _profilePage.GetUserProfileRating();
			var translationText = "Test" + Guid.NewGuid();

			_header.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();

			_courseraEditorPage
				.FillAndConfirmTarget(translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.GoToUserProfile();
			
			Assert.Less(ratingBeforeTranslation, _profilePage.GetUserProfileRating(),
				"Произошла ошибка:\n Рейтинг пользователя не изменился.");
		}

		[Test, Ignore("PRX-17445")]
		public void VoteDownTest()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var translationText = "Test" + Guid.NewGuid();

			var votesUpCountBefore = _profilePage.GetVotesUpCount();
			var votesDownCountBefore = _profilePage.GetVotesDownCount();
			var traslatedSentencesCountBefore = _profilePage.GetTranslatedSentencesCount();
			
			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();

			_courseraEditorPage
				.FillAndConfirmTarget(translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header
				.GoToUserProfile()
				.ClickSignOut();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);
			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();
			
			var translationVotesCountBefore = _courseraEditorPage.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, translationText);

			_courseraEditorPage.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, translationText);

			var voteCountAfter = _courseraEditorPage.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, translationText);

			Assert.AreEqual(translationVotesCountBefore - 1, voteCountAfter,
				"Произошла ошибка:\n Количество голосов не уменьшилось.");

			_courseraEditorPage.ClickHomeButtonExpectingCourseraCoursesPage();

			_header
				.GoToUserProfile()
				.ClickSignOut();

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);
			_header.GoToUserProfile();
			
			Assert.AreEqual(votesDownCountBefore + 1, _profilePage.GetVotesDownCount(),
				"Произошла ошибка:\n Количество голосов Против не увеличилось.");

			Assert.AreEqual(votesUpCountBefore, _profilePage.GetVotesUpCount(),
				"Произошла ошибка:\n Количество голосов За изменилось.");

			Assert.AreEqual(traslatedSentencesCountBefore + 1, _profilePage.GetTranslatedSentencesCount(),
				"Произошла ошибка:\n Количество переведенных предложений не увеличилось.");
		}

		[Test, Ignore("PRX-17445")]
		public void VoteUpTest()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var translationText = "Test" + Guid.NewGuid();
			
			var votesUpCountBefore = _profilePage.GetVotesUpCount();
			var votesDownCountBefore = _profilePage.GetVotesDownCount();
			var traslatedSentencesCountBefore = _profilePage.GetTranslatedSentencesCount();

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();

			_courseraEditorPage
				.FillAndConfirmTarget(translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header
				.GoToUserProfile()
				.ClickSignOut();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);
			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();
			
			var voteCountBefore = _courseraEditorPage
				.ClickOnTargetCellInSegment()
				.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, translationText);

			_courseraEditorPage.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, translationText);

			var voteCountAfter = _courseraEditorPage.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, translationText);

			Assert.AreEqual(voteCountBefore + 1, voteCountAfter,
				"Произошла ошибка:\n Количество голосов не увеличилось.");

			_courseraEditorPage.ClickHomeButtonExpectingCourseraCoursesPage();

			_header
				.GoToUserProfile()
				.ClickSignOut();

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);
			_header.GoToUserProfile();
			
			Assert.AreEqual(votesDownCountBefore, _profilePage.GetVotesDownCount(),
				"Произошла ошибка:\n Количество голосов Против изменилось.");

			Assert.AreEqual(votesUpCountBefore + 1, _profilePage.GetVotesUpCount(),
				"Произошла ошибка:\n Количество голосов За не увеличилось.");

			Assert.AreEqual(traslatedSentencesCountBefore + 1, _profilePage.GetTranslatedSentencesCount(),
				"Произошла ошибка:\n Количество переведенных предложений не увеличилось.");
		}

		[Test, Ignore("PRX-14792, PRX-17445")]
		public void RatingTest()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var translationText = "Test" + Guid.NewGuid();
			
			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();

			_courseraEditorPage
				.FillAndConfirmTarget(translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.GoToUserProfile();

			var ratingBeforeRate = _profilePage.GetUserProfileRating();

			_header.ClickSignOut();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);
			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();

			var translationVotesCountBefore = _courseraEditorPage.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, translationText);

			_courseraEditorPage.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, translationText);

			var voteCountAfter = _courseraEditorPage.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, translationText);

			Assert.AreEqual(translationVotesCountBefore - 1, voteCountAfter,
				"Произошла ошибка:\n Количество голосов не уменьшилось.");

			_courseraEditorPage.ClickHomeButtonExpectingCourseraCoursesPage();

			_header
				.GoToUserProfile()
				.ClickSignOut();

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);
			_header.GoToUserProfile();

			var ratingAfterRate = _profilePage.GetUserProfileRating();

			Assert.AreNotEqual(ratingBeforeRate, ratingAfterRate, "Произошла ошибка:\n Рейтинг не изменился.");
		}

		[Test, Ignore("PRX-14792, PRX-17445")]
		public void VoteUpViaEventsListTest()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var translationText = "Test" + Guid.NewGuid();

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();

			_courseraEditorPage
				.FillAndConfirmTarget(translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.GoToUserProfile();

			var ratingBeforeRate = _profilePage.GetUserProfileRating();

			_header.ClickSignOut();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			var voteCount = _courseraHomePage.GetVoteCount(translationText);

			_courseraHomePage.ClickVoteUpButton(translationText);

			Assert.AreEqual(voteCount + 1, _courseraHomePage.GetVoteCount(translationText),
				"Произошла ошибка:\n Количество голосов для перевода '{0}' не увеличилось.", translationText);

			_header
				.GoToUserProfile()
				.ClickSignOut();

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);

			_header.GoToUserProfile();

			Assert.Less(ratingBeforeRate, _profilePage.GetUserProfileRating(),
				"Произошла ошибка:\n Рейтинг не увеличился.");
		}

		[Test, Ignore("PRX-14792, PRX-17445")]
		public void VoteDownViaEventsListTest()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var translationText = "Test" + Guid.NewGuid();

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();

			_courseraEditorPage
				.FillAndConfirmTarget(translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.GoToUserProfile();

			var ratingBeforeRate = _profilePage.GetUserProfileRating();

			_header.ClickSignOut();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_coursesPage.GoToHomePage();

			var voteCount = _courseraHomePage.GetVoteCount(translationText);

			_courseraHomePage.ClickVoteDownButton(translationText);

			Assert.AreEqual(voteCount - 1, _courseraHomePage.GetVoteCount(translationText),
				"Произошла ошибка:\n Количество голосов для перевода '{0}' не уменьшилось.", translationText);

			_header
				.GoToUserProfile()
				.ClickSignOut();

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);

			_header.GoToUserProfile();

			Assert.Less(ratingBeforeRate, _profilePage.GetUserProfileRating(),
				"Произошла ошибка:\n Рейтинг не уменьшился.");
		}

		private string _courseraProject;
	}
}
