using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera.UserProfileTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Coursera]
	class UserProfileValidTests<TWebDriverProvider> : UserProfileBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void ChangeAboutMeTest()
		{
			string aboutMeInformation = "About Me Info " + Guid.NewGuid();

			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.EditProfile(aboutMe: aboutMeInformation);

			Assert.AreEqual(aboutMeInformation, _profilePage.GetAboutMeInformation(),
				"Произошла ошибка:\nНеверная информация о пользователе.");
		}

		[Test]
		public void ChangeUserNameCheckProfilePageTest()
		{
			_courseraHomePage.ClickProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.EditProfile(_newUserName, _newUserSurname);

			Assert.AreEqual(_newFullName, _profilePage.GetNickname(),
				"Произошла ошибка:\nНеверное имя пользователя в профиле.");
		}

		[Test]
		public void ChangeUserNameCheckHomePageTest()
		{
			_courseraHomePage.ClickProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.EditProfile(_newUserName, _newUserSurname);

			_header.GoToHomePage();

			Assert.AreEqual(_newFullName, _courseraHomePage.GetNickname(),
				"Произошла ошибка:\nНеверное имя пользователя на главной странице.");
		}

		[Test]
		public void ChangeUserNameCheckHeaderMenuTest()
		{
			_courseraHomePage.ClickProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.EditProfile(_newUserName, _newUserSurname);

			Assert.AreEqual(_newFullName, _header.GetNickname(),
				"Произошла ошибка:\nНеверное имя пользователя в меню.");
		}

		[Test]
		public void CheckTranslationAfterEditProfileTest()
		{
			var translationText = "Test" + Guid.NewGuid();

			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.EditProfile(_newUserName, _newUserSurname);

			_header.GoToCoursesPage();

			_coursesPage.ClickCourse(CreateProjectHelper.CourseraProjectName);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();

			_editorPage
				.FillTarget(translationText)
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment();

			Assert.AreEqual(_newFullName, _editorPage.GetSegmentTranslationUserName(),
				"Произошла ошибка:\nНеверное имя пользователя в ревизии перевода");
		}

		[Test]
		public void CheckTranslationBeforeEditProfileTest()
		{
			var translationText = "Test" + Guid.NewGuid();

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(CreateProjectHelper.CourseraProjectName);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();

			_editorPage
				.FillTarget(translationText)
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.EditProfile(_newUserName, _newUserSurname);

			_header.GoToCoursesPage();

			_coursesPage.ClickCourse(CreateProjectHelper.CourseraProjectName);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();

			_editorPage.ClickOnTargetCellInSegment();

			Assert.AreEqual(_newFullName, _editorPage.GetSegmentTranslationUserName(),
				"Произошла ошибка:\nНеверное имя пользователя в ревизии перевода");
		}

		[Test, Ignore("PRX-14767")]
		public void CheckEventListBeforeEditProfileTest()
		{
			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(CreateProjectHelper.CourseraProjectName);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();

			_editorPage
				.FillTarget(_translationText)
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.GoToHomePage();

			Assert.IsTrue(_courseraHomePage.IsTargetTranslationDisplayedInEventList(_translationText),
				"Произошла ошибка:\nПеревод '{0}' не появился в списке событий.", _translationText);

			Assert.IsTrue(CourseraReviewerUser.NickName.Contains(_courseraHomePage.GetAuthorInEventList(_translationText)),
				"Произошла ошибка:\nНеверное имя автора перевода '{0}' в списке событий.", _translationText);

			_courseraHomePage.ClickProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.EditProfile(_newUserName, _newUserSurname);

			_header.GoToHomePage();

			Assert.IsTrue(_newFullName.Contains(_courseraHomePage.GetAuthorInEventList(_translationText)),
				"Произошла ошибка:\nНеверное имя автора перевода '{0}' в списке событий.", _translationText);
		}

		[Test]
		public void CheckEventListAfterEditProfileTest()
		{
			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.EditProfile(_newUserName, _newUserSurname);

			_header.GoToHomePage();

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(CreateProjectHelper.CourseraProjectName);

			_coursePage.ClickLectureTab();

			_lecturesTab.OpenLecture();

			_editorPage
				.FillTarget(_translationText)
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.GoToHomePage();

			Assert.IsTrue(_courseraHomePage.IsTargetTranslationDisplayedInEventList(_translationText),
				"Произошла ошибка:\nПеревод '{0}' не появился в списке событий.", _translationText);

			Assert.IsTrue(_newFullName.Contains(_courseraHomePage.GetAuthorInEventList(_translationText)),
				"Произошла ошибка:\nНеверное имя автора перевода '{0}' в списке событий.", _translationText);
		}

		[TestCase("0123456")]
		[TestCase("012345")]
		public void ChangePasswordTest(string newPassword)
		{
			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePassword(newPassword, _password);

			_header.ClickSignOut();

			_loginHelper.LogInCoursera(_login, newPassword);

			Assert.AreEqual(_newFullName, _courseraHomePage.GetNickname(),
				"Произошла ошибка:\nНе удалось войти в курсеру с новым паролем.");
		}

		[Test]
		public void ChangePasswordNewEqualOldTest()
		{
			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePassword(oldPassword: _password, newPassword: _password);

			_header.ClickSignOut();

			_loginHelper.LogInCoursera(_login, _password);

			Assert.AreEqual(_newFullName, _courseraHomePage.GetNickname(),
				"Произошла ошибка:\nНе удалось войти в курсеру.");
		}
	}
}
