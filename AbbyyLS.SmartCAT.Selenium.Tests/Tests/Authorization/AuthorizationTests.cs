using System;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Authorization
{
	[Parallelizable(ParallelScope.Fixtures)]
	internal class AuthorizationTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public AuthorizationTests()
		{
			StartPage = StartPage.SignIn;
		}

		[SetUp]
		public void Initialization()
		{
			_adminHelper = new AdminHelper(Driver);
			_commonHelper = new CommonHelper(Driver);

			_signInPage = new SignInPage(Driver);
			_facebookPage = new FacebookPage(Driver);
			_googlePage = new GooglePage(Driver);
			_linkedInPage = new LinkedInPage(Driver);
			_selectAccountForm = new SelectAccountForm(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_selectProfileForm=new SelectProfileForm(Driver);
		}

		[TestCase("Personal", LoginHelper.EuropeTestServerName)]
		[TestCase("Personal", LoginHelper.USATestServerName)]
		[TestCase("TestAccount", LoginHelper.EuropeTestServerName)]
		[TestCase("TestAccount", LoginHelper.USATestServerName)]
		public void AuthorizationWithCorrectCredentials(string account, string dataServer)
		{
			_signInPage.SubmitForm(ThreadUser.Login, ThreadUser.Password);

			_selectAccountForm.SelectAccount(account, dataServer);

			_workspacePage.SetLocale();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(ThreadUser.NickName),
				"Произошла ошибка:\n имя пользователя в черной плашке не совпадает с ожидаемым именем.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(account),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");
		}

		[Test]
		public void AuthorizationWithWrongPassword()
		{
			var email = "ringo123@mailforspam.com";
			var password = "0000";

			_signInPage
				.SetLogin(email)
				.SetPassword(password)
				.ClickSubmitButtonExpectingError();

			Assert.IsTrue(_signInPage.IsWrongPasswordMessageDisplayed(),
				"Произошла ошибка: \n на странице не появилось сообщение о неправильном пароле.");
		}

		[Test]
		public void AuthorizationWithUnregisteredUser()
		{
			var email = "ringo@mailforspam.com";
			var password = "31415926";

			_signInPage
				.SetLogin(email)
				.SetPassword(password)
				.ClickSubmitButtonExpectingError();

			Assert.IsTrue(_signInPage.IsUserNotFoundMessageDisplayed(),
				"Произошла ошибка: \n на странице не появилось сообщение о ненайденном пользователе.");
		}

		[Test]
		public void AuthorizationWithEmptyPassword()
		{
			var email = "ringo123@mailforspam.com";
			var password = String.Empty;

			_signInPage
				.SetLogin(email)
				.SetPassword(password)
				.ClickSubmitButtonExpectingError();

			Assert.IsTrue(_signInPage.IsEmptyPasswordMessageDisplayed(),
				"Произошла ошибка: \n на странице не появилось сообщение о незаполненном пароле.");
		}


		[Test]
		public void AuthorizationWithInvalidEmail()
		{
			var email = "ringo123";
			var password = "31415926";

			_signInPage
				.SetLogin(email)
				.SetPassword(password)
				.ClickSubmitButtonExpectingError();

			Assert.IsTrue(_signInPage.IsInvalidEmailMessageDisplayed(),
				"Произошла ошибка: \n на странице не появилось сообщение о невалидном email.");
		}


		[Test]
		public void AuthorizationViaFacebook()
		{
			_signInPage.ClickFacebookIcon();

			_facebookPage.SubmitForm(ConfigurationManager.SocialNetworksUserList[0].Login, ConfigurationManager.SocialNetworksUserList[0].Password);

			_selectAccountForm.SelectAccount(LoginHelper.TestAccountName);

			_workspacePage.SetLocale();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(ConfigurationManager.SocialNetworksUserList[0].NickName),
				"Произошла ошибка:\n имя пользователя в черной плашке не совпадает с ожидаемым именем.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(LoginHelper.TestAccountName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");
		}

		[Test]
		public void AuthorizationViaGooglePlus()
		{
			_signInPage.ClickGooglePlusIcon();

			_googlePage.SubmitForm(ConfigurationManager.SocialNetworksUserList[0].Login, ConfigurationManager.SocialNetworksUserList[0].Password);

			_selectAccountForm.SelectAccount(LoginHelper.TestAccountName);

			_workspacePage.SetLocale();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(ConfigurationManager.SocialNetworksUserList[0].NickName),
				"Произошла ошибка:\n имя пользователя в черной плашке не совпадает с ожидаемым именем.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(LoginHelper.TestAccountName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");
		}

		[Test]
		public void AuthorizationViaLinkedIn()
		{
			_signInPage.ClickLinkedInIcon();

			_linkedInPage.SubmitForm(ConfigurationManager.SocialNetworksUserList[0].Login, ConfigurationManager.SocialNetworksUserList[0].Password);

			_selectAccountForm.SelectAccount(LoginHelper.TestAccountName);

			_workspacePage.SetLocale();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(ConfigurationManager.SocialNetworksUserList[0].NickName),
				"Произошла ошибка:\n имя пользователя в черной плашке не совпадает с ожидаемым именем.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(LoginHelper.TestAccountName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");
		}

		[Test]
		public void SignOutTest()
		{
			_signInPage.SubmitForm(ThreadUser.Login, ThreadUser.Password);

			_selectAccountForm.SelectAccount();

			_workspacePage
				.ClickAccount()
				.ClickSignOut();

			Assert.IsTrue(_signInPage.IsSignInPageOpened(), "Произошла ошибка: \n не открылась страница авторизации");
		}

		[TestCase("ssssssss213123s@mail.ru", "aol3mailru@mail.ru")] // Активный AOL-аккаунт
		[TestCase("AolUserivanpetrov2@mailforspam.com", "12trC89p", Ignore = "PRX-10821")] // Неактивный AOL-аккаунт
		public void SignInWithAolAccount(string email, string password)
		{
			_signInPage.SubmitFormExpectingSelectProfileForm(email, password);

			Assert.IsTrue(_selectProfileForm.IsSelectProfileFormOpened(),
				"Произошла ошибка:\n страница с выбором профиля не открылась.");
		}

		[Test]
		public void SignInWithPerevedemAccount()
		{
			var accountName = "Perevedem";
			var email = "testuserperevedem@mailforspam.com";
			var nickName = "testuserperevedem@mailforspam.com";
			var password = "43abC12z";

			_commonHelper.GoToAdminUrl();
			_adminHelper
				.SignIn(ThreadUser.Login, ThreadUser.Password)
				.CreateUserWithSpecificAccount(email, nickName, password, accountName);
			_commonHelper.GoToSignInPage();

			_signInPage.SubmitForm(email, password);

			_selectAccountForm.SelectAccount(accountName);

			_workspacePage.SetLocale();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(nickName),
				"Произошла ошибка:\n имя пользователя в черной плашке не совпадает с ожидаемым именем.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(accountName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");
		}

		[Test]
		public void SignInWithCourseraAccount()
		{
			var accountName= "Coursera";
			var email= "testusercoursera@mailforspam.com";
			var nickName= "testusercoursera@mailforspam.com";
			var password= "13grC89p";

			_commonHelper.GoToAdminUrl();
			_adminHelper
				.SignIn(ThreadUser.Login, ThreadUser.Password)
				.CreateUserWithSpecificAccount(email, nickName, password, accountName);
			_commonHelper.GoToSignInPage();

			_signInPage.SubmitFormExpectingSelectProfileForm(email, password);

			Assert.IsTrue(_selectProfileForm.IsSelectProfileFormOpened(),
				"Произошла ошибка:\n страница с выбором профиля не открылась");
		}

		private AdminHelper _adminHelper;
		private CommonHelper _commonHelper;

		private SignInPage _signInPage;
		private FacebookPage _facebookPage;
		private GooglePage _googlePage;
		private LinkedInPage _linkedInPage;
		private SelectAccountForm _selectAccountForm;
		private SelectProfileForm _selectProfileForm;
		private WorkspacePage _workspacePage;
	}
}