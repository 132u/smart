using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera.UserProfileTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	class PasswordTests<TWebDriverProvider> : UserProfileBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void PasswordTestsSetUp()
		{
			_loginHelper.LogInCoursera(CourseraReviewerUser.Login, CourseraReviewerUser.Password);
		}

		[TestCase("0123456")]
		[TestCase("012345")]
		public void ChangePasswordTest(string newPassword)
		{
			_oldPassword = newPassword;

			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePassword(oldPassword: CourseraReviewerUser.Password, newPassword: newPassword);

			_header.ClickSignOut();

			_loginHelper.LogInCoursera(CourseraReviewerUser.Login, newPassword);

			Assert.AreEqual(CourseraReviewerUser.NickName, _courseraHomePage.GetNickname(),
				"Произошла ошибка:\nНе удалось войти в курсеру с новым паролем.");
		}

		[Test]
		public void ChangeInvalidShortPasswordTest()
		{
			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePasswordExpectingError(oldPassword: CourseraReviewerUser.Password, newPassword: "123");

			Assert.IsTrue(_editProfileDialog.IsShortPasswordErrorDisplayed(),
				"Произошла ошибка:\nНе появилось сообщение 'Password is too short'.");

			Assert.IsTrue(_editProfileDialog.IsPasswordSaveButtonInactive(),
				"Произошла ошибка:\nКнопка сохранения активна.");

			_editProfileDialog.ClickCancelButton();

			_header.ClickSignOut();

			_loginHelper.LogInCoursera(CourseraReviewerUser.Login, CourseraReviewerUser.Password);

			Assert.AreEqual(CourseraReviewerUser.NickName, _courseraHomePage.GetNickname(),
				"Произошла ошибка:\nНе удалось войти в курсеру.");
		}

		[Test]
		public void ChangePasswordWrongOldPasswordTest()
		{
			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePasswordExpectingError(oldPassword: "wrongPassword", newPassword: "newPassword")
				.ClickPasswordSaveButton();

			Assert.IsTrue(_editProfileDialog.IsInvalidPasswordErrorDisplayed(),
				"Произошла ошибка:\nНе появилось сообщение 'Invalid password'.");
		}

		[Test]
		public void ChangePasswordNewEqualOldTest()
		{
			_oldPassword = CourseraReviewerUser.Password;
			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePassword(oldPassword: CourseraReviewerUser.Password, newPassword: CourseraReviewerUser.Password);

			_header.ClickSignOut();

			_loginHelper.LogInCoursera(CourseraReviewerUser.Login, CourseraReviewerUser.Password);

			Assert.AreEqual(CourseraReviewerUser.NickName, _courseraHomePage.GetNickname(),
				"Произошла ошибка:\nНе удалось войти в курсеру.");
		}

		[Test]
		public void ChangePasswordMismatchTest()
		{
			var newPassword = "newPassword";

			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePasswordExpectingError(oldPassword: CourseraReviewerUser.Password, newPassword: newPassword, confirmPassword: newPassword + "123")
				.ClickPasswordSaveButton();

			Assert.IsTrue(_editProfileDialog.IsPasswordMismatchErrorDisplayed(),
				"Произошла ошибка:\nНе появилось сообщение 'Passwords do not match'.");
		}

		[TearDown]
		public void TearDown()
		{
			TakeScreenshotIfTestFailed();

			if (_editProfileDialog.IsEditProfileDialogOpened())
			{
				_editProfileDialog.ClickCancelButton();
			}

			if (_courseraHomePage.IsCourseraHomePageOpened())
			{
				_courseraHomePage.ClickProfile();
			}
			else
			{
				_header.GoToUserProfile();
			}

			_profilePage.ClickEditProfileButton();

			_oldPassword = _oldPassword ?? CourseraReviewerUser.Password;

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePasswordExpectingError(oldPassword: _oldPassword, newPassword: CourseraReviewerUser.Password, confirmPassword: CourseraReviewerUser.Password)
				.ClickPasswordSaveButton();
		}

		private string _oldPassword;
	}
}
