using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Authorization
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Authorization]
	class PasswordRecoveryWithoutSetNewPasswordTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public PasswordRecoveryWithoutSetNewPasswordTests()
		{
			StartPage = StartPage.SignIn;
		}

		[SetUp]
		public void SetUp()
		{
			_workspacePage = new WorkspacePage(Driver);
			_signInPage = new SignInPage(Driver);
			_recoveryPasswordPage = new RecoveryPasswordPage(Driver);
		}

		[Test, Description("S-7049")]
		public void PasswordRecoveryValidEmailTest()
		{
			_signInPage.ClickForgotPasswordLink();

			_recoveryPasswordPage
				.SetEmailForRecoveryPassword(ThreadUser.Login)
				.ClickChangePasswordButton();

			Assert.IsTrue(_recoveryPasswordPage.IsRecoveryNoticeDisplayed(),
				"Произошла ошибка:\n Не отобразилась справка для дальнейших действий по смене пароля.");
		}

		[Test, Description("S-11724")]
		public void PasswordRecoveryNonExistEmailTest()
		{
			var invalidEmail = "email" + Guid.NewGuid() + "@forspam.com";

			_signInPage.ClickForgotPasswordLink();

			_recoveryPasswordPage
				.SetEmailForRecoveryPassword(invalidEmail)
				.ClickChangePasswordButton();

			Assert.IsTrue(_recoveryPasswordPage.IsErrorMessageDisplayed(),
				"Произошла ошибка:\n Не отобразилось сообщение об ошибке при восстановлении пароля с несуществующим email.");
		}

		[Test, Description("S-7118")]
		public void SignInWithOldPasswordWithoutGenerateNewPasswordTest()
		{
			_signInPage.ClickForgotPasswordLink();

			_recoveryPasswordPage
				.SetEmailForRecoveryPassword(ThreadUser.Login)
				.ClickChangePasswordButton()
				.ClickOnSmartCatLogo();

			_signInPage
				.SubmitForm(ThreadUser.Login, ThreadUser.Password)
				.SelectAccount();

			Assert.IsTrue(_workspacePage.IsWorkspacePageOpened(),
				"Произошла ошибка:\n Не произошла авторизация с неизменённым паролем, но высланным письмом восстановления.");
		}

		private WorkspacePage _workspacePage;
		private SignInPage _signInPage;
		private RecoveryPasswordPage _recoveryPasswordPage;
	}
}