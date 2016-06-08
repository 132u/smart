using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	class UserProfileBaseTests<TWebDriverProvider> : CourseraBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public override void SetUp()
		{
			_editProfileDialog = new EditProfileDialog(Driver);
			_newUserName = "n" + Guid.NewGuid().ToString().Substring(0, 10);
			_newUserSurname = "s" + Guid.NewGuid().ToString().Substring(0, 10);
			_newFullName = _newUserName + " " + _newUserSurname;
			_translationText = "Test" + Guid.NewGuid();
			_login = Guid.NewGuid() + "@mailforspam.com";
			_password = Guid.NewGuid().ToString();

			_adminHelper = new AdminHelper(Driver);
			_courseraHomePage = new CourseraHomePage(Driver);
			_courseraSignInDialog = new CourseraSignInDialog(Driver);
			_courseraSignUpDialog = new CourseraSignUpDialog(Driver);
			_adminSignInPage = new AdminSignInPage(Driver);
			
			_adminSignInPage
				.GetPage()
				.SignIn(ThreadUser.Login, ThreadUser.Password);

			_adminHelper.CreateNewUser(_login, _login, _password, aolUser: false);

			_courseraHomePage
				.GetPage()
				.ClickJoinButton();

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

		protected string _courseraProject;
		protected string _login;
		protected string _password;
		protected string _newUserName;
		protected string _newUserSurname;
		protected string _newFullName;
		protected string _translationText;

		protected AdminHelper _adminHelper;
		protected AdminSignInPage _adminSignInPage;
		protected CourseraHomePage _courseraHomePage;
		protected CourseraSignInDialog _courseraSignInDialog;
		protected CourseraSignUpDialog _courseraSignUpDialog;
		protected EditProfileDialog _editProfileDialog;
	}
}
