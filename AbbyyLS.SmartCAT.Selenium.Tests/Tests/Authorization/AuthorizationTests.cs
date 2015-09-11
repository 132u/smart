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
		}

		[TestCase("Personal", LoginHelper.EuropeTestServerName)]
		[TestCase("Personal", LoginHelper.USATestServerName)]
		[TestCase("TestAccount", LoginHelper.EuropeTestServerName)]
		[TestCase("TestAccount", LoginHelper.USATestServerName)]
		public void AuthorizationWithCorrectCredentials(string account, string dataServer)
		{
			LoginHelper
				.SignIn(Login, Password)
				.SelectAccount(account, dataServer)
				.SetUp(NickName, account);
		}

		[TestCase("ringo123@mailforspam.com", "0000", SignInErrorMessageType.WrongPassword)]
		[TestCase("ringo@mailforspam.com", "31415926", SignInErrorMessageType.UserNotFound)]
		[TestCase("ringo123@mailforspam.com", "", SignInErrorMessageType.EmptyPassword)]
		[TestCase("ringo123", "31415926", SignInErrorMessageType.InvalidEmail)]
		public void AuthorizationWithIncorrectCredentials(string email, string password, SignInErrorMessageType signInErrorMessageType)
		{
			LoginHelper.TryToSignIn(email, password);

			switch (signInErrorMessageType)
			{
				case SignInErrorMessageType.WrongPassword:
					LoginHelper.CheckWrongPasswordMessageDisplayed();
					break;

				case SignInErrorMessageType.UserNotFound:
					LoginHelper.CheckUserNotFoundMessageDisplayed();
					break;

				case SignInErrorMessageType.EmptyPassword:
					LoginHelper.CheckEmptyPasswordMessageDisplayed();
					break;

				case SignInErrorMessageType.InvalidEmail:
					LoginHelper.CheckInvalidEmailMessageDisplayed();
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
					LoginHelper.SignInViaFacebook(email, password);
					break;

				case SocialNetworks.GooglePlus:
					LoginHelper.SignInViaGooglePlus(email, password);
					break;

				case SocialNetworks.LinkedIn:
					LoginHelper.SignInViaLinkedIn(email, password);
					break;

				default:
					throw new Exception(String.Format("Передано неправильное название сайта: {0}", socialNetworks));
			}

			LoginHelper
				.SelectAccount()
				.SetUp(nickName);
		}

		[Test]
		public void SignOutTest()
		{
			LoginHelper
				.SignIn(Login, Password)
				.SelectAccount()
				.SetUp(NickName)
				.SignOut();
		}

		[TestCase("ssssssss213123s@mail.ru", "aol3mailru@mail.ru")] // Активный AOL-аккаунт
		[TestCase("AolUserivanpetrov2@mailforspam.com", "12trC89p"), Ignore("PRX-10821")] // Неактивный AOL-аккаунт
		public void SignInWithAolAccount(string email, string password)
		{
			LoginHelper
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
			LoginHelper
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
			LoginHelper
				.SignIn(email, password)
				.AssertAccountNotFoundMessageDisplayed();
		}

		private void сreateUserWithAccount(
			string email,
			string nickName,
			string password,
			string accountName = LoginHelper.TestAccountName)
		{
			LogInAdmin(Login, Password);
			AdminHelper
				.CreateNewUser(email, nickName, password, admin: true, aolUser: true)
				.FindUser(email)
				.CheckAdminCheckbox()
				.AddUserToSpecificAccount(email, accountName);
			Driver.Navigate().GoToUrl(Url + RelativeUrlProvider.SignIn);
		}
	}
}