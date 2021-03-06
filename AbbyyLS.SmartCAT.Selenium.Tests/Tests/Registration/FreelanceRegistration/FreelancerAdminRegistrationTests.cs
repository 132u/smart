﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Registration]
	class FreelancerAdminRegistrationTests<TWebDriverProvider> :
		RegistrationBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public FreelancerAdminRegistrationTests()
		{
			StartPage = StartPage.Admin;
		}

		[Test]
		public void ExistingUserWithoutAccountRegistrationTest()
		{
			_adminHelper.CreateNewUser(_email, _firstAndLastName, _password);

			_registrationPage.GetPageExpectingRedirectToSignInPage(_email);

			Assert.AreEqual(_firstAndLastName + ", you already\r\nhave an account.", _signInPage.GetMessageText(),
				"Произошла ошибка:\n не появилось сообщение о том, что аккаунт уже существует.");

			_signInPage.SubmitForm(_email, _password);

			_selectAccountForm.ClickCreateAccountButton();

			_registrationPage
				.ClickFreelancerForm()
				.ClickConfirmButton();

			Assert.AreEqual(_workspacePage.GetUserName(), _firstAndLastName,
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(LoginHelper.PersonalAccountName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
		}

		[TestCase(false)]
		[TestCase(true), Description("S-7110"), ShortCheckList]
		public void ExistingUserWithPersonalAccountRegistrationTest(bool active)
		{
			_adminHelper
				.CreateNewUser(_email, _firstAndLastName, _password)
				.CreateNewPersonalAccount(_lastName, state: active);

			_registrationPage.GetPageExpectingRedirectToSignInPage(_email);

			Assert.AreEqual(_firstAndLastName + ", you already\r\nhave an account.", _signInPage.GetMessageText(),
				"Произошла ошибка:\n не появилось сообщение о том, что аккаунт уже существует.");

			_signInPage.SubmitFormExpectingWorkspacePage(_email, _password);

			Assert.AreEqual(_workspacePage.GetUserName(), _firstAndLastName,
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(LoginHelper.PersonalAccountName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
		}
	}
}
