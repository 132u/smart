using NLog.Fluent;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Authorization
{
	[Parallelizable(ParallelScope.Fixtures)]
	class AuthorizationViaSocialNetworksTests<TWebDriverProvider> : AuthorizationBaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public AuthorizationViaSocialNetworksTests()
		{
			StartPage = StartPage.SignIn;
		}

		[Test, Description("S-13738")]
		public void AuthorizationViaFacebook()
		{
			var login = ConfigurationManager.SocialNetworksUserList[0].Login;
			var password = ConfigurationManager.SocialNetworksUserList[0].Password;
			var nickname = ConfigurationManager.SocialNetworksUserList[0].NickName;

			_signInPage.ClickFacebookIcon();

			_facebookPage.SubmitForm(login, password);

			_selectAccountForm.SelectAccount(LoginHelper.TestAccountName);

			_workspacePage.SetLocale();

			Assert.AreEqual(_workspacePage.GetUserName(), nickname,
				"Произошла ошибка:\n имя пользователя в черной плашке не совпадает с ожидаемым именем.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(LoginHelper.TestAccountName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");
		}

		[Test, Description("S-13737")]
		public void AuthorizationViaGooglePlus()
		{
			var login = ConfigurationManager.SocialNetworksUserList[0].Login;
			var password = ConfigurationManager.SocialNetworksUserList[0].Password;
			var nickname = ConfigurationManager.SocialNetworksUserList[0].NickName;

			_signInPage.ClickGooglePlusIcon();

			_googlePage.SubmitForm(login, password);

			_selectAccountForm.SelectAccount(LoginHelper.TestAccountName);

			_workspacePage.SetLocale();

			Assert.AreEqual(_workspacePage.GetUserName(), nickname,
				"Произошла ошибка:\n имя пользователя в черной плашке не совпадает с ожидаемым именем.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(LoginHelper.TestAccountName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");
		}

		[Test, Description("S-13740")]
		public void AuthorizationViaLinkedIn()
		{
			var login = ConfigurationManager.SocialNetworksUserList[0].Login;
			var password = ConfigurationManager.SocialNetworksUserList[0].Password;
			var nickname = ConfigurationManager.SocialNetworksUserList[0].NickName;

			_signInPage.ClickLinkedInIcon();

			_linkedInPage.SubmitForm(login, password);

			_selectAccountForm.SelectAccount(LoginHelper.TestAccountName);

			_workspacePage.SetLocale();

			Assert.AreEqual(_workspacePage.GetUserName(), nickname,
				"Произошла ошибка:\n имя пользователя в черной плашке не совпадает с ожидаемым именем.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(LoginHelper.TestAccountName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");
		}
	}
}
