using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Authorization
{
	[Parallelizable(ParallelScope.Fixtures)]
	class AuthorizationWithIncorectCredentials<TWebDriverProvider> : AuthorizationBaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public AuthorizationWithIncorectCredentials()
		{
			StartPage = StartPage.SignIn;
		}

		[Test]
		public void AuthorizationWithWrongPassword()
		{
			var email = "ringo123@mailforspam.com";
			var password = "0000";

			_signInPage
				.SetLogin(email)
				.SetPassword(password)
				.ClickSubmitButtonExpectingError()
				.WaitEmptyPasswordMessageDisappeared();

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
				.ClickSubmitButtonExpectingError()
				.WaitEmptyPasswordMessageDisappeared();

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
	}
}
