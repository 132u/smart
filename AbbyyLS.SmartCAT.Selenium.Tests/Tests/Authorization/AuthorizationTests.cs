using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Authorization
{
	internal class AuthorizationTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public AuthorizationTests()
		{
			StartPage = StartPage.SignIn;
			_adminHelper = new AdminHelper();
			_commonHelper = new CommonHelper();
			_loginHelper = new LoginHelper();
		}

		[TestCase("Personal", LoginHelper.EuropeTestServerName)]
		[TestCase("Personal", LoginHelper.USATestServerName)]
		[TestCase("TestAccount", LoginHelper.EuropeTestServerName)]
		[TestCase("TestAccount", LoginHelper.USATestServerName)]
		public void AuthorizationWithCorrectCredentials(string account, string dataServer)
		{
			_loginHelper
				.SignIn(ConfigurationManager.Login, ConfigurationManager.Password)
				.SelectAccount(account, dataServer)
				.SetUp(ConfigurationManager.NickName, account);
		}

		[TestCase("ringo123@mailforspam.com", "0000", SignInErrorMessageType.WrongPassword)]
		[TestCase("ringo@mailforspam.com", "31415926", SignInErrorMessageType.UserNotFound)]
		[TestCase("ringo123@mailforspam.com", "", SignInErrorMessageType.EmptyPassword)]
		[TestCase("ringo123", "31415926", SignInErrorMessageType.InvalidEmail)]
		public void AuthorizationWithIncorrectCredentials(string email, string password, SignInErrorMessageType signInErrorMessageType)
		{
			_loginHelper.TryToSignIn(email, password);

			switch (signInErrorMessageType)
			{
				case SignInErrorMessageType.WrongPassword:
					_loginHelper.CheckWrongPasswordMessageDisplayed();
					break;

				case SignInErrorMessageType.UserNotFound:
					_loginHelper.CheckUserNotFoundMessageDisplayed();
					break;

				case SignInErrorMessageType.EmptyPassword:
					_loginHelper.CheckEmptyPasswordMessageDisplayed();
					break;

				case SignInErrorMessageType.InvalidEmail:
					_loginHelper.CheckInvalidEmailMessageDisplayed();
					break;

				default:
					throw new Exception(String.Format("Передан неправильный тип ошибки: {0}", signInErrorMessageType));
			}
		}

		[TestCase("margarita.kolly@yandex.ru", "Margarita Kolly", "0onWolkap", SocialNetworks.Facebook)]
		[TestCase("smaartcat@gmail.com", "smart cat", "smaartcattest", SocialNetworks.GooglePlus)]
		[TestCase("margarita.kolly@yandex.ru", "Margarita Kolly", "0onWolkap", SocialNetworks.LinkedIn)]
		public void AuthorizationViaSocialNetworks(
			string email,
			string nickName,
			string password,
			SocialNetworks socialNetworks)
		{
			сreateUserWithAccount(email, nickName, password);

			switch (socialNetworks)
			{
				case SocialNetworks.Facebook:
					_loginHelper.SignInViaFacebook(email, password);
					break;

				case SocialNetworks.GooglePlus:
					_loginHelper.SignInViaGooglePlus(email, password);
					break;

				case SocialNetworks.LinkedIn:
					_loginHelper.SignInViaLinkedIn(email, password);
					break;

				default:
					throw new Exception(String.Format("Передано неправильное название сайта: {0}", socialNetworks));
			}

			_loginHelper
				.SelectAccount()
				.SetUp(nickName);
		}

		[Test]
		public void SignOutTest()
		{
			_loginHelper
				.SignIn(ConfigurationManager.Login, ConfigurationManager.Password)
				.SelectAccount()
				.SetUp(ConfigurationManager.NickName)
				.SignOut();
		}

		[TestCase("ssssssss213123s@mail.ru", "aol3mailru@mail.ru")] // Активный AOL-аккаунт
		[TestCase("AolUserivanpetrov2@mailforspam.com", "12trC89p"), Ignore("PRX-10821")] // Неактивный AOL-аккаунт
		public void SignInWithAolAccount(string email, string password)
		{
			_loginHelper
				.SignIn(email, password)
				.AssertAccountNotFoundMessageDisplayed();
		}

		[Test]
		public void SignInWithPerevedemAccount()
		{
			var accountName = "Perevedem";
			var email = "testuserperevedem@mailforspam.com";
			var nickName = "testuserperevedem@mailforspam.com";
			var password = "43abC12z";

			сreateUserWithAccount(email, nickName, password, accountName);
			_loginHelper
				.SignIn(email, password)
				.SelectAccount(accountName)
				.SetUp(nickName, accountName);
		}

		[Test]
		public void SignInWithCourseraAccount()
		{
			var accountName= "Coursera";
			var email= "testusercoursera@mailforspam.com";
			var nickName= "testusercoursera@mailforspam.com";
			var password= "13grC89p";

			сreateUserWithAccount(email, nickName, password, accountName);
			_loginHelper
				.SignIn(email, password)
				.AssertAccountNotFoundMessageDisplayed();
		}

		private void сreateUserWithAccount(
			string email,
			string nickName,
			string password,
			string accountName = LoginHelper.TestAccountName)
		{
			_commonHelper.GoToAdminUrl();
			_adminHelper
				.SignIn(ConfigurationManager.Login, ConfigurationManager.Password)
				.CreateNewUser(email, nickName, password, admin: true, aolUser: true)
				.FindUser(email)
				.CheckAdminCheckbox()
				.AddUserToSpecificAccount(email, accountName);
			_commonHelper.GoToSignInPage();
		}

		private AdminHelper _adminHelper;
		private CommonHelper _commonHelper;
		private LoginHelper _loginHelper;
	}
}