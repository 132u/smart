using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Authorization
{
	[Parallelizable(ParallelScope.Fixtures)]
	internal class AuthorizationTests<TWebDriverProvider> : AuthorizationBaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public AuthorizationTests()
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
	}
}