using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Coursera]
	class VoteTests<TWebDriverProvider> : CourseraBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void AutoVoteOneselfTest()
		{
			var lecture = "1.1 Introduction";

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage
				.FillTarget(_translationText)
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment();

			Assert.AreEqual(1, _editorPage.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, _translationText),
				"Произошла ошибка:\n Неверное количество голосов за перевод.");

			Assert.IsTrue(_editorPage.IsVoteUpButtonDisabled(CourseraCrowdsourceUser.NickName, _translationText),
				"Произошла ошибка:\n Кнопка голосования за перевод активна.");
		}

		[Test]
		public void EditorVoteUpDownUp()
		{
			var lecture = "1.3 Concepts";

			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.ClickSignOut();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage.ClickOnTargetCellInSegment();

			var voteCount = _editorPage.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, _translationText);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, _translationText);

			Assert.AreEqual(voteCount + 1, _editorPage.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, _translationText),
				"Произошла ошибка:\n Неверное количество голосов за перевод.");

			_editorPage
				.ClickOnTargetCellInSegment()
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, _translationText);

			Assert.AreEqual(voteCount + 1, _editorPage.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, _translationText),
				"Произошла ошибка:\n Неверное количество голосов за перевод.");

			_editorPage.ClickHomeButtonExpectingCourseraCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, _translationText);

			Assert.AreEqual(voteCount + 1, _editorPage.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, _translationText),
				"Произошла ошибка:\n Неверное количество голосов за перевод.");

			var voteCountBeforeDownVote = _editorPage.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, _translationText);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, _translationText);

			Assert.AreEqual(voteCountBeforeDownVote - 2, _editorPage.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, _translationText),
				"Произошла ошибка:\n Неверное количество голосов за перевод.");

			_editorPage.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, _translationText);

			Assert.AreEqual(voteCountBeforeDownVote, _editorPage.ScrollAndGetVoteCount(CourseraCrowdsourceUser.NickName, _translationText),
				"Произошла ошибка:\n Неверное количество голосов за перевод.");
		}

		[Test]
		public void EditorVoteUpCheckUserProfile()
		{
			var lecture = "1.4 Theories";

			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			_courseraHomePage.ClickProfile();

			var upVoteCount = _profilePage.GetVotesUpCount();
			var downVoteCount = _profilePage.GetVotesDownCount();

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.ClickSignOut();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.ClickSignOut();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);

			_header.GoToUserProfile();

			Assert.AreEqual(downVoteCount, _profilePage.GetVotesDownCount(),
				"Произошла ошибка:\n Неверное количество голосов против.");

			Assert.Less(upVoteCount, _profilePage.GetVotesUpCount(),
				"Произошла ошибка:\n Неверное количество голосов за.");
		}

		[Test]
		public void EditorVoteDownCheckUserProfile()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var lecture = "1.7 Cultural approach";

			_courseraHomePage.ClickProfile();

			var upVoteCount = _profilePage.GetVotesUpCount();
			var downVoteCount = _profilePage.GetVotesDownCount();

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.ClickSignOut();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.ClickSignOut();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);

			_header.GoToUserProfile();

			Assert.Less(downVoteCount, _profilePage.GetVotesDownCount(),
				"Произошла ошибка:\n Неверное количество голосов против.");

			Assert.AreEqual(_profilePage.GetVotesUpCount(), upVoteCount,
				"Произошла ошибка:\n Неверное количество голосов за.");
		}

		[Test]
		public void EditorVoteUpCheckLastEvents()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var lecture = "2.1 A short history of Communication Science";

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			var lectureNumber = _lecturesTab.GetLectureNumberWithEmptyCommonProgress();

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.ClickSignOut();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.GoToHomePage();

			Assert.IsTrue(_courseraHomePage.IsVoteActionInLastEventDisplayed(_secondUser.Name, _translationText),
				"Произошла ошибка:\n В списке последних событий нет события голосования за перевод.");
		}

		[Test, Ignore("PRX-17339")]
		public void EditorVoteDownCheckLastEvents()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var lecture = "3.8 Negotiated Effects";

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.ClickSignOut();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.GoToHomePage();

			Assert.IsTrue(_courseraHomePage.IsVoteActionInLastEventDisplayed(_secondUser.Name, _translationText),
				"Произошла ошибка:\n В списке последних событий нет события голосования против перевода.");
		}

		[Test]
		public void VoteUpInEditorCheckLastEventsTest()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var lecture = "1.2 What is communication";

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.ClickSignOut();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.GoToHomePage();

			Assert.IsTrue(_courseraHomePage.IsVoteActionInLastEventDisplayed(_secondUser.Name, _translationText),
				"Произошла ошибка:\n В списке последних событий нет события голосования за перевод.");
		}

		[Test]
		public void LastEventsVoteUpDownUp()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var lecture = "2.4 Rhetorical Theory";

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.ClickSignOut();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_header.GoToHomePage();

			var voteCount = _courseraHomePage.GetVoteCount(_translationText);

			_courseraHomePage.ClickVoteUpButton(_translationText);

			Assert.AreEqual(voteCount + 1, _courseraHomePage.GetVoteCount(_translationText),
				"Произошла ошибка:\n Неверное количество голосов.");

			_courseraHomePage.ClickVoteDownButton(_translationText);

			Assert.AreEqual(voteCount - 1, _courseraHomePage.GetVoteCount(_translationText),
				"Произошла ошибка:\n Неверное количество голосов.");

			_courseraHomePage.RefreshPage<CourseraHomePage>();

			_courseraHomePage.ClickVoteUpButton(_translationText);

			Assert.AreEqual(voteCount + 1, _courseraHomePage.GetVoteCount(_translationText),
				"Произошла ошибка:\n Неверное количество голосов.");

			var voteCountBeforeDownVote = _courseraHomePage.GetVoteCount(_translationText);

			_courseraHomePage
					.ScrollVoteDownButton(_translationText)
					.ClickVoteDownButton(_translationText);

			Assert.AreEqual(voteCountBeforeDownVote - 2, _courseraHomePage.GetVoteCount(_translationText),
				"Произошла ошибка:\n Неверное количество голосов.");

			_courseraHomePage.ClickVoteUpButton(_translationText);

			Assert.AreEqual(voteCountBeforeDownVote, _courseraHomePage.GetVoteCount(_translationText),
				"Произошла ошибка:\n Неверное количество голосов.");
		}

		[Test]
		public void LastEventsVoteUpCheckUserProfile()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var lecture = "2.6 A Renaissance of our field";

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.ClickSignOut();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_header.GoToHomePage();

			var voteCount = _courseraHomePage.GetVoteCount(_translationText);

			_courseraHomePage.ClickVoteUpButton(_translationText);

			Assert.AreEqual(voteCount + 1, _courseraHomePage.GetVoteCount(_translationText),
				"Произошла ошибка:\n Неверное количество голосов.");
		}

		[Test]
		public void LastEventsVoteDownCheckUserProfile()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var lecture = "2.8 Towards a Modern Communication Science";

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.ClickSignOut();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_header.GoToHomePage();

			var voteCount = _courseraHomePage.GetVoteCount(_translationText);

			_courseraHomePage
					.ScrollVoteDownButton(_translationText)
					.ClickVoteUpButton(_translationText);

			Assert.AreEqual(voteCount + 1, _courseraHomePage.GetVoteCount(_translationText),
				"Произошла ошибка:\n Неверное количество голосов.");
		}

		[Test]
		public void LastEventsVoteUpCheckLastEvents()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var lecture = "3.2 The Power of Propaganda and the All-powerfull Media Paradigm";

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.ClickSignOut();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_header.GoToHomePage();

			_courseraHomePage.ClickVoteUpButton(_translationText);

			_courseraHomePage.RefreshPage<CourseraHomePage>();

			Assert.IsTrue(_courseraHomePage.IsVoteActionInLastEventDisplayed(_secondUser.Name, _translationText),
				"Произошла ошибка:\n В списке последних событий нет событие голосования за перевод.");
		}

		[Test, Ignore("PRX-17339")]
		public void VoteDownInLastEventsTest()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var lecture = "2.3 Two Schools of Classical Communication Science";

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.ClickSignOut();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_header.GoToHomePage();

			_courseraHomePage
					.ScrollVoteDownButton(_translationText)
					.ClickVoteDownButton(_translationText);

			_courseraHomePage.RefreshPage<CourseraHomePage>();

			Assert.IsFalse(_courseraHomePage.IsVoteActionInLastEventDisplayed(_secondUser.Name, _translationText),
				"Произошла ошибка:\n В списке последних событий появилось событие голосования против перевода.");
		}
	}
}