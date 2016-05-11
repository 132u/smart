using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Registration]
	class FreelancerRegistrationInvalidDataTests<TWebDriverProvider> :
		RegistrationBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public FreelancerRegistrationInvalidDataTests()
		{
			StartPage = StartPage.Registration;
		}

		[Test, Description("S-7106"), ShortCheckList]
		public void TwoTheSameFreelancersRegistrationTest()
		{
			var email = Guid.NewGuid() + "@mailforspam.com";

			_registrationPage
				.GetPage(email)
				.FillFreelancerRegistrationForm(password: _password, firstAndLastName: _firstAndLastName)
				.ClickConfirmButton();

			_workspacePage.SignOutExpectingAlert();

			_registrationPage.GetPageExpectingRedirectToSignInPage(email);

			Assert.AreEqual(_firstAndLastName + ", you already\r\nhave an account.", _signInPage.GetMessageText(),
				"Произошла ошибка:\n не появилось сообщение о том, что аккаунт уже существует.");
		}

		[Test, Description("S-13734"), ShortCheckList]
		public void EmptyPasswordTest()
		{
			_registrationPage
				.ClickFreelancerForm()
				.FillFreelancerFirstAndLastName(_firstAndLastName)
				.ClickConfirmButtonExpectingError();

			Assert.IsTrue(_registrationPage.IsEnterPasswordErrorMessageForFreelancerDisplayed(),
				"Произошла ошибка: не появилось сообщение об ошибке в поле 'Password'");
		}

		[Test, Description("S-13735"), ShortCheckList]
		public void EmptyFirstAndLastNameTest()
		{
			_registrationPage
				.ClickFreelancerForm()
				.FillFreelancerPassword(_password)
				.ClickConfirmButtonExpectingError();

			Assert.IsTrue(_registrationPage.IsEnterFirstAndLastNameErrorMessageForFreelancerDisplayed(),
				"Произошла ошибка: не появилось сообщение об ошибке в поле 'Password'");
		}

		[Test, Description("S-13736"), ShortCheckList]
		public void ShortPasswordTest()
		{
			var password = "12345";

			_registrationPage
				.ClickFreelancerForm()
				.FillFreelancerPassword(password)
				.FillFreelancerFirstAndLastName(_firstAndLastName)
				.ClickConfirmButtonExpectingError();

			Assert.IsTrue(_registrationPage.IsShortPasswordErrorMessageForFreelancerDisplayed(),
				"Произошла ошибка: не появилось сообщение об ошибке в поле 'Password'");
		}
	}
}
