using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Authorization
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Authorization]
	class AuthorizationCookiesTests<TWebDriverProvider> : RegistrationBaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public AuthorizationCookiesTests()
		{
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void SetUpAuthorizationCookiesTests()
		{
			_adminHelper
				.CreateNewUser(_email, _firstAndLastName, _password)
				.CreateNewPersonalAccount(_lastName, true);

			_registrationPage.GetPageExpectingRedirectToSignInPage(_email);

			_tempFolder = Driver.TempFolder;
		}

		[TearDown]
		public void TearDown()
		{
			if (_newDriver != null)
			{
				ReplaceDrivers(_newDriver);
			}
		}

		[Test, Description("S-7121"), ShortCheckList]
		public void RemeberMeCheckBoxTest()
		{
			_signInPage
				.ClickRememberMeCheckBox()
				.SubmitFormExpectingWorkspacePage(_email, _password);

			var sessionCookie = _signInPage.Driver.Manage().Cookies.GetCookieNamed("session");

			Assert.IsNotNull(sessionCookie.Expiry,
				"Произошла ошибка:\n Для отмеченного чекбокса 'RememberMe' не  заполнилось поле 'Expiry' для куки 'Session'.");

			_workspacePage.Driver.Close();

			_newDriver = new WebDriver(new TWebDriverProvider(), _tempFolder, PathProvider.ExportFiles);
			var newWorkspacePage = new WorkspacePage(_newDriver);
			newWorkspacePage.GetPage(ConfigurationManager.Url);

			Assert.IsTrue(newWorkspacePage.IsWorkspacePageOpened(),
				"Произошла ошибка:\n Не произошла авторизация после того, как отметили чекбокс 'RememberMe'.");
		}

		[Test, Description("S-15983"), ShortCheckList]
		public void NoRemeberMeCheckBoxTest()
		{
			_signInPage.SubmitFormExpectingWorkspacePage(_email, _password);

			var sessionCookie = _signInPage.Driver.Manage().Cookies.GetCookieNamed("session");

			Assert.IsNull(sessionCookie.Expiry,
				"Произошла ошибка:\n Для отмеченного чекбокса 'RememberMe'  заполнилось поле 'Expiry' для куки 'Session'.");

			_workspacePage.Driver.Close();

			_newDriver = new WebDriver(new TWebDriverProvider(), _tempFolder, PathProvider.ExportFiles);
			var newSignInPage = new SignInPage(_newDriver);
			var newWorkspacePage = new WorkspacePage(_newDriver);

			newWorkspacePage.GetPage(ConfigurationManager.Url);

			Assert.IsTrue(newSignInPage.IsSignInPageOpened(),
				"Произошла ошибка:\n Страница авторизации не открылась.");
		}

		private string _tempFolder;
		private WebDriver _newDriver;
	}
}