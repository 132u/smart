using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera.UserProfileTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Coursera]
	class UserProfileInvalidTests<TWebDriverProvider> : UserProfileBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase(" ")]
		[TestCase("")]
		public void FillInvalidNameTest(string name)
		{
			_courseraHomePage.ClickProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.FillName(name: name);

			Assert.IsTrue(_editProfileDialog.IsNameErrorDisplayed(),
				"Произошла ошибка:\nНе появилась ошибка 'Please fill in the \"First name\" field'.");

			Assert.IsTrue(_editProfileDialog.IsNameRedBorderDisplayed(),
				"Произошла ошибка:\nНе появилась красная рамка у поля имени.");

			Assert.IsTrue(_editProfileDialog.IsSaveButtonInactive(),
				"Произошла ошибка:\nКнопка сохранения активна.");
		}

		[TestCase(" ")]
		[TestCase("")]
		public void FillInvalidSurnameTest(string surname)
		{
			_courseraHomePage.ClickProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.FillSurname(surname: surname);

			Assert.IsTrue(_editProfileDialog.IsSurnameErrorDisplayed(),
				"Произошла ошибка:\nНе появилась ошибка 'Please fill in the \"First name\" field'.");

			Assert.IsTrue(_editProfileDialog.IsSurnameRedBorderDisplayed(),
				"Произошла ошибка:\nНе появилась красная рамка у поля имени.");

			Assert.IsTrue(_editProfileDialog.IsSaveButtonInactive(),
				"Произошла ошибка:\nКнопка сохранения активна.");
		}
		[Test]
		public void ChangeInvalidShortPasswordTest()
		{
			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePasswordExpectingError(oldPassword: _password, newPassword: "123");

			Assert.IsTrue(_editProfileDialog.IsShortPasswordErrorDisplayed(),
				"Произошла ошибка:\nНе появилось сообщение 'Password is too short'.");

			Assert.IsTrue(_editProfileDialog.IsPasswordSaveButtonInactive(),
				"Произошла ошибка:\nКнопка сохранения активна.");

			_editProfileDialog.ClickCancelButton();

			_header.ClickSignOut();

			_loginHelper.LogInCoursera(_login, _password);

			Assert.AreEqual(_newFullName, _courseraHomePage.GetNickname(),
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
		public void ChangePasswordMismatchTest()
		{
			var newPassword = "newPassword";

			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog
				.ClickChangePasswordTab()
				.ChangePasswordExpectingError(oldPassword: _password, newPassword: newPassword, confirmPassword: newPassword + "123")
				.ClickPasswordSaveButton();

			Assert.IsTrue(_editProfileDialog.IsPasswordMismatchErrorDisplayed(),
				"Произошла ошибка:\nНе появилось сообщение 'Passwords do not match'.");
		}
	}
}
