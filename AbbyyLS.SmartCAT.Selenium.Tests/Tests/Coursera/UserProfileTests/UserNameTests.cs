using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Coursera]
	[Ignore("Курсерные тесты отключены за нестабильность")]
	class UserNameTests<TWebDriverProvider> : UserProfileBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
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

			_coursePage.OpenLecture();

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

			_coursePage.OpenLecture();

			_editorPage
				.FillTarget(translationText)
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.EditProfile(_newUserName, _newUserSurname);

			_header.GoToCoursesPage();

			_coursesPage.ClickCourse(CreateProjectHelper.CourseraProjectName);

			_coursePage.OpenLecture();

			_editorPage.ClickOnTargetCellInSegment();

			Assert.AreEqual(_newFullName, _editorPage.GetSegmentTranslationUserName(),
				"Произошла ошибка:\nНеверное имя пользователя в ревизии перевода");
		}

		[Test, Ignore("PRX-14767")]
		public void CheckEventListBeforeEditProfileTest()
		{
			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(CreateProjectHelper.CourseraProjectName);

			_coursePage.OpenLecture();

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

			_coursePage.OpenLecture();

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

		[TearDown]
		public new void TeardownBase()
		{
			TakeScreenshotIfTestFailed();

			if (_editorPage.IsEditorPageOpened())
			{
				_editorPage.ClickHomeButtonExpectingCourseraCoursesPage();
			}
			else if (_courseraHomePage.IsCourseraHomePageOpened())
			{
				_courseraHomePage.GoToHomePage();
			}
			
			_header.GoToUserProfile();
			
			_profilePage.ClickEditProfileButton();

			_editProfileDialog.EditProfile(CourseraReviewerUser.Name, CourseraReviewerUser.Surname);
		}
	}
}
