using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Authorization
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Authorization]
	public class PasswordRecoveryTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> 
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public PasswordRecoveryTests()
		{
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void SetUp()
		{
			_recoveryPasswordPage = new RecoveryPasswordPage(Driver);
			_signInPage = new SignInPage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_adminLingvoProPage = new AdminLingvoProPage(Driver);
			_adminLettersSearchPage = new AdminLettersSearchPage(Driver);
			_adminLetterPage = new AdminLetterPage(Driver);
			_setNewPasswordPage = new SetNewPasswordPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_adminHelper = new AdminHelper(Driver);
			_registrationPage = new RegistrationPage(Driver);
			_userProfileDialog = new UserProfileDialog(Driver);

			_email = "Email" + Guid.NewGuid() + "@mailforspam.com";
			_password = "Password" + Guid.NewGuid();
			_lastName = "LastName" + Guid.NewGuid();
			_firstName = "FirstName" + Guid.NewGuid();
			_firstAndLastName = _firstName + " " + _lastName;

			_adminHelper.CreateNewUser(_email, _firstAndLastName, _password);

			_adminHelper.AddUserToAdminGroupInAccountIfNotAdded(
				_email,
				_firstName,
				_lastName,
				LoginHelper.TestAccountName);

			_registrationPage.GetPageExpectingRedirectToSignInPage(_email);
		}

		[Test, Description("S-7117"), ShortCheckList]
		public void RecoveryMailWithPasswordTest()
		{
			_signInPage.ClickForgotPasswordLink();

			_recoveryPasswordPage
				.SetEmailForRecoveryPassword(_email)
				.ClickChangePasswordButton();

			_loginHelper.Authorize(StartPage.Admin, ThreadUser);

			_adminLingvoProPage.ClickAdminLettersSearchReference();

			_adminLettersSearchPage
				.SetEmail(_email)
				.ClickFindButton()
				.OpenLetter(_email);

			_adminLetterPage.ClickPasswordRecoveryLink();

			Assert.IsTrue(_setNewPasswordPage.IsNewPasswordEntryPageOpened(),
				"Произошла ошибка:\n Не открылась страница для ввода нового пароля.");
		}

		[Test, Description("S-7119"), ShortCheckList]
		public void AuthorizationWithNewPasswordTest()
		{
			var newPassword = "S-7119" + "NewPassword" + Guid.NewGuid();

			_signInPage.ClickForgotPasswordLink();

			_recoveryPasswordPage
				.SetEmailForRecoveryPassword(_email)
				.ClickChangePasswordButton();

			_loginHelper.Authorize(StartPage.Admin, ThreadUser);

			_adminLingvoProPage.ClickAdminLettersSearchReference();

			_adminLettersSearchPage
				.SetEmail(_email)
				.ClickFindButton()
				.OpenLetter(_email);

			_adminLetterPage.ClickPasswordRecoveryLink();

			_setNewPasswordPage
				.SetNewPassword(newPassword)
				.ClickChangePasswordButton();

			Assert.IsTrue(_workspacePage.IsWorkspacePageOpened(),
				"Произошла ошибка:\n После смены пароля и авторизации под ним не отобразилась стартовая страница.");
		}

		[Test, Description("S-7120"), ShortCheckList]
		public void ChangePasswordAfterSignInTest()
		{
			var newPassword = "S-7120 newPassword" + Guid.NewGuid();

			_signInPage.SubmitFormExpectingWorkspacePage(_email, _password);

			_workspacePage
				.ClickAccount()
				.ClickAccessSettings();

			_userProfileDialog.ChangePassword(newPassword, _password);

			_workspacePage.SignOut();

			_signInPage.SubmitFormExpectingErrorMessage(_email, _password);

			Assert.IsTrue(_signInPage.IsWrongPasswordMessageDisplayed(),
				"Произошла ошибка:\n После смены пароля удалось зайти со старым паролем.");

			_signInPage.SubmitFormExpectingWorkspacePage(_email, newPassword);

			Assert.IsTrue(_workspacePage.IsWorkspacePageOpened(),
				"Произошла ошибка:\n Не удалось зайти с новым паролем.");
		}

		private RecoveryPasswordPage _recoveryPasswordPage;
		private SignInPage _signInPage;
		private AdminLingvoProPage _adminLingvoProPage;
		private AdminLettersSearchPage _adminLettersSearchPage;
		private AdminLetterPage _adminLetterPage;
		private SetNewPasswordPage _setNewPasswordPage;
		private WorkspacePage _workspacePage;
		private AdminHelper _adminHelper;
		private RegistrationPage _registrationPage;
		private UserProfileDialog _userProfileDialog;
		private string _email;
		private string _password;
		private string _lastName;
		private string _firstName;
		private string _firstAndLastName;
	}
}