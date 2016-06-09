using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Coursera]
	class TranslationTests<TWebDriverProvider> : CourseraBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void AddTranslationNoTranslationsTests()
		{
			var lecture = "1.1 Introduction";

			_header.GoToUserProfile();

			var sentencesCount = _profilePage.GetTranslatedSentencesCount();

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();
		
			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.GoToUserProfile();

			Assert.AreEqual(sentencesCount + 1, _profilePage.GetTranslatedSentencesCount(),
				"Произошла ошибка:\n Количество переведенных предложений не увеличилось.");
		}

		[Test]
		public void AddTranslationExistTranslationsTests()
		{
			var translationText2 = "Test" + Guid.NewGuid();
			var lecture = "1.3 Concepts";

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();
		
			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.GoToUserProfile();

			var sentencesCount = _profilePage.GetTranslatedSentencesCount();

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(translationText2);

			_header.GoToUserProfile();

			Assert.AreEqual(sentencesCount + 1, _profilePage.GetTranslatedSentencesCount(),
				"Произошла ошибка:\n Количество переведенных предложений не увеличилось.");
		}

		[Test]
		public void AddTranslationExistTranslationsAnotherUsersTests()
		{
			var translationText2 = "Test" + Guid.NewGuid();
			var lecture = "1.4 Theories";

			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);
			
			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();
		
			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header
				.ClickSignOut()
				.GoToHomePage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_header.GoToUserProfile();

			var sentencesCount = _profilePage.GetTranslatedSentencesCount();

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(translationText2);

			_header.GoToUserProfile();

			Assert.AreEqual(sentencesCount + 1, _profilePage.GetTranslatedSentencesCount(),
				"Произошла ошибка:\n Количество переведенных предложений не увеличилось.");
		}

		[Test]
		public void DeleteTranslationTests()
		{
			var lecture = "1.7 Cultural approach";

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.GoToUserProfile();

			var sentencesCount = _profilePage.GetTranslatedSentencesCount();

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture(lecture);
	
			_editorPage
				.ClickOnTargetCellInSegment()
				.ClickDeleteTranslateButton(CourseraCrowdsourceUser.NickName, _translationText);

			_deleteTranslationDialog.ClickYesButton();

			_editorPage.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.GoToUserProfile();

			Assert.AreEqual(sentencesCount, _profilePage.GetTranslatedSentencesCount(),
				"Произошла ошибка:\n Количество переведенных предложений изменилось.");
		}

		[Test]
		public void AddTwoIdenticalTranslationsTests()
		{
			var lecture = "2.1 A short history of Communication Science";

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();
			
			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.GoToUserProfile();

			var sentencesCount = _profilePage.GetTranslatedSentencesCount();

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.GoToUserProfile();

			Assert.AreEqual(sentencesCount, _profilePage.GetTranslatedSentencesCount(),
				"Произошла ошибка:\n Количество переведенных предложений изменилось.");
		}

		[Test]
		public void AddTranslationDiffererntLecturesTests()
		{
			var lecture = "3.8 Negotiated Effects";
			var lecture2 = "1.2 What is communication";

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.GoToUserProfile();

			var sentencesCount = _profilePage.GetTranslatedSentencesCount();

			_header.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraProject);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture(lecture2);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_header.GoToUserProfile();

			Assert.AreEqual(sentencesCount + 1, _profilePage.GetTranslatedSentencesCount(),
				"Произошла ошибка:\n Количество переведенных предложений не увеличилось.");
		}

		private string _courseraProject;
	}
}
