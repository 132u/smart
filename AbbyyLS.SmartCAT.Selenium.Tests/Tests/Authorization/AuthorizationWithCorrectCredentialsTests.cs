using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Authorization
{
	[Parallelizable(ParallelScope.Fixtures)]
	internal class AuthorizationWithCorrectCredentialsTests<TWebDriverProvider> : AuthorizationBaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public AuthorizationWithCorrectCredentialsTests()
		{
			StartPage = StartPage.SignIn;
		}
		
		[TestCase("Personal", LoginHelper.EuropeTestServerName)]
		[TestCase("Personal", LoginHelper.USATestServerName, IgnoreReason = "SCAT-757")]
		[TestCase("TestAccount", LoginHelper.EuropeTestServerName)]
		[TestCase("TestAccount", LoginHelper.USATestServerName, IgnoreReason = "SCAT-757")]
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
			_signInPage.SubmitFormExpectingCreateAccountForm(email, password);

			_createAccountPage.ClickCreateAccountButton();

			Assert.IsTrue(_selectProfileForm.IsSelectProfileFormOpened(),
				"Произошла ошибка:\n страница с выбором профиля не открылась.");
		}

		[Ignore("PRX-15311")]
		[Test]
		public void SignInWithPerevedemAccount()
		{
			var accountName = "Perevedem";
			var email = "testuserperevedem@mailforspam.com";
			var nickName = "testuserperevedem@mailforspam.com";
			var password = "43abC12z";

			_commonHelper.GoToAdminUrl();

			_adminSignInPage.SignIn(ThreadUser.Login, ThreadUser.Password);

			_adminHelper.CreateUserWithSpecificAndPersonalAccount(
				email: email,
				name: nickName,
				surname: nickName,
				nickName: nickName,
				password: password,
				accountName: accountName,
				personalAccountActiveState: false,
				aolUser: true);

			_commonHelper.GoToSignInPage();

			_signInPage.SubmitFormExpectingWorkspacePage(email, password);
			
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
			var email = "testusercoursera@mailforspam.com";
			var nickName = "testusercoursera@mailforspam.com";
			var name = "testusercoursera";
			var surname = "testusercoursera";
			var password= "13grC89p";

			_commonHelper.GoToAdminUrl();

			_adminSignInPage.SignIn(ThreadUser.Login, ThreadUser.Password);

			_adminHelper
				.CreateNewUser(
					email: email,
					nickName: nickName,
					password: password,
					aolUser: true)
				.AddUserToAdminGroupInAccountIfNotAdded(email, name, surname, accountName); ;

			_commonHelper.GoToSignInPage();

			_signInPage.SubmitFormExpectingCreateAccountForm(email, password);

			_createAccountPage.ClickCreateAccountButton();

			Assert.IsTrue(_selectProfileForm.IsSelectProfileFormOpened(),
				"Произошла ошибка:\n страница с выбором профиля не открылась");
		}
	}
}