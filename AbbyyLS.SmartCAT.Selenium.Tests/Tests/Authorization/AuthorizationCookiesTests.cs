using System;

using NUnit.Framework;
using OpenQA.Selenium;

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

		[Test, Description("S-7121"), ShortCheckList]
		public void RemeberMeCheckBoxTest()
		{
			var data = DateTime.Now.AddYears(2);
			var cookieName = "session";
			var reloadPageJavaScript = "location.reload()";

			_adminHelper
				.CreateNewUser(_email, _firstAndLastName, _password)
				.CreateNewPersonalAccount(_lastName, true);

			_registrationPage.GetPageExpectingRedirectToSignInPage(_email);

			_signInPage
				.ClickRememberMeCheckBox()
				.SubmitFormExpectingWorkspacePage(_email, _password);

			var sessionCookie = _signInPage.Driver.Manage().Cookies.GetCookieNamed(cookieName);
			var fakeSessionCookie = new Cookie(sessionCookie.Name, sessionCookie.Value, sessionCookie.Path, data);

			Assert.IsNotNull(sessionCookie.Expiry,
				"Произошла ошибка:\n Для отмеченного чекбокса 'RememberMe' не  заполнилось поле 'Expiry' для куки 'Session'.");

			_workspacePage.Driver.Quit();

			var newDriver = new WebDriver(new TWebDriverProvider(), PathProvider.DriversTemporaryFolder, PathProvider.ExportFiles, PathProvider.ImportFiles);
			var newSignInPage = new SignInPage(newDriver);
			var newWorkspacePage = new WorkspacePage(newDriver);
			
			newSignInPage.GetPage();

			newDriver.Manage().Cookies.AddCookie(fakeSessionCookie);

			newSignInPage.Driver.ExecuteScript(reloadPageJavaScript);

			Assert.IsTrue(newWorkspacePage.IsWorkspacePageOpened(),
				"Произошла ошибка:\n Не произошла авторизация после добавления куки 'session', с указанным параметром Expire.");

			ReplaceDrivers(newDriver);
		}
	}
}