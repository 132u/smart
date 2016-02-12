using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera.UserProfileTests
{
	class UserProfileBaseTests<TWebDriverProvider> : CourseraBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public UserProfileBaseTests()
		{
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void UserProfileBaseTestsSetUp()
		{
			_editProfileDialog = new EditProfileDialog(Driver);
			_newUserName = "n" + Guid.NewGuid().ToString().Substring(0, 10);
			_newUserSurname = "s" + Guid.NewGuid().ToString().Substring(0, 10);
			_newFullName = _newUserName + " " + _newUserSurname;
			_translationText = "Test" + Guid.NewGuid();
		
			_adminHelper= new AdminHelper(Driver);
			_courseraHomePage = new CourseraHomePage(Driver);
			_courseraSignInDialog = new CourseraSignInDialog(Driver);
			_commonHelper = new CommonHelper(Driver);
			_courseraSignUpDialog = new CourseraSignUpDialog(Driver);

			_login = Guid.NewGuid() + "@mailforspam.com";
			_password = Guid.NewGuid().ToString();
			
			_adminHelper.CreateNewUser(_login, _login, _password, aolUser: false);

			_commonHelper.GoToCoursera();

			_courseraHomePage.ClickJoinButton();

			_courseraSignInDialog
				.LoginInCoursera(_login, _password)
				.ClickSigInButtonRedirectionOnRegistartionTab();

			_courseraSignUpDialog
				.FillRegistrationForm(_newUserName, _newUserSurname)
				.ClickSignUpButton();

			Assert.IsTrue(_courseraSignUpDialog.IsThanksMessageDisplayed(),
				"Произошла ошибка:\n Не появилось сообщение о окончании регистрации.");

			_courseraSignUpDialog.ClickCancelButton();

			_loginHelper.LogInCoursera(_login, _password);
		}

		protected string _login;
		protected string _password;
		protected string _newUserName;
		protected string _newUserSurname;
		protected string _newFullName;
		protected string _translationText;

		protected AdminHelper _adminHelper;
		protected CourseraHomePage _courseraHomePage;
		protected CourseraSignInDialog _courseraSignInDialog;
		protected CommonHelper _commonHelper;
		protected CourseraSignUpDialog _courseraSignUpDialog;
		protected EditProfileDialog _editProfileDialog;
	}
}
