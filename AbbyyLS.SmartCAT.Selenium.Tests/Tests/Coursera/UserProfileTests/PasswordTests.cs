using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	class PasswordTests<TWebDriverProvider> : UserProfileBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase("0123456")]
		[TestCase("012345")]
		public void ChangePasswordTest(string newPassword)
		{
			_oldPassword = newPassword;

			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePassword(oldPassword: CourseraCrowdsourceUser.Password, newPassword: newPassword);

			_header.ClickSignOut();

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, newPassword);

			Assert.AreEqual(CourseraCrowdsourceUser.NickName, _courseraHomePage.GetNickname(),
				"Произошла ошибка:\nНе удалось войти в курсеру с новым паролем.");
		}

		[Test]
		public void ChangeInvalidShortPasswordTest()
		{
			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePasswordExpectingError(oldPassword: CourseraCrowdsourceUser.Password, newPassword: "123");

			Assert.IsTrue(_editProfileDialog.IsShortPasswordErrorDisplayed(),
				"Произошла ошибка:\nНе появилось сообщение 'Password is too short'.");

			Assert.IsTrue(_editProfileDialog.IsPasswordSaveButtonInactive(),
				"Произошла ошибка:\nКнопка сохранения активна.");

			_editProfileDialog.ClickCancelButton();

			_header.ClickSignOut();

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);

			Assert.AreEqual(CourseraCrowdsourceUser.NickName, _courseraHomePage.GetNickname(),
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
			_oldPassword = CourseraCrowdsourceUser.Password;
			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePassword(oldPassword: CourseraCrowdsourceUser.Password, newPassword: CourseraCrowdsourceUser.Password);

			_header.ClickSignOut();

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);

			Assert.AreEqual(CourseraCrowdsourceUser.NickName, _courseraHomePage.GetNickname(),
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
				.ChangePasswordExpectingError(oldPassword: CourseraCrowdsourceUser.Password, newPassword: newPassword, confirmPassword: newPassword + "123")
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

			_oldPassword = _oldPassword ?? CourseraCrowdsourceUser.Password;

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePasswordExpectingError(oldPassword: _oldPassword, newPassword: CourseraCrowdsourceUser.Password, confirmPassword: CourseraCrowdsourceUser.Password)
				.ClickPasswordSaveButton();
		}

		private string _oldPassword;
	}
}
