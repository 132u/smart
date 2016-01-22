using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Authorization
{
	[Parallelizable(ParallelScope.Fixtures)]
	class AuthorizationViaSocialNetworksTests<TWebDriverProvider> : AuthorizationBaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
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
	}
}
