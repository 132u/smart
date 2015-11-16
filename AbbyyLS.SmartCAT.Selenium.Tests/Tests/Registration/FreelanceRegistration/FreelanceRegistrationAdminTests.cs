﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration.FreelanceRegistration;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Parallelizable(ParallelScope.Fixtures)]
	class FreelanceRegistrationAdminTests<TWebDriverProvider> : FreelanceRegistrationBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public FreelanceRegistrationAdminTests()
		{
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void AdminFreelanceRegistrationTestsSetUp()
		{
			_adminHelper = new AdminHelper(Driver);
		}

		[Test]
		public void CreateUserInAdminRegistration()
		{
			_adminHelper.CreateNewUser(_email, _nickName, _password, aolUser: false);

			_commonHelper.GoToFreelanceRegistratioin();

			_freelanceRegistrationFirstPage.ClickExistAccountAbbyyOnlineLink();

			_freelanceRegistrationSignInPage
				.FillSignInForm(_email, _password)
				.ClickSignInButtonWithInactivePersonalAccount();

			_freelanceRegistrationSecondPage
				.FillFreelanceRegistrationSecondStep(
					firstName: _firstName,
					lastName: _lastName)
				.ClickCreateAccountButton();

			Assert.IsTrue(_projectsPage.IsProjectsPageOpened(),
				"Произошла ошибка:\nСтраница поректов не открылась.");

			Assert.IsTrue(_workspacePage.IsNickNameMatch(_nickName),
				"Произошла ошибка:\nНеверное имя фрилансера в верхней панели.");
		}

		[Test]
		public void CreateNewUserLoginWrongPasswordRegistration()
		{
			_adminHelper.CreateNewUser(_email, _nickName, _password);

			_commonHelper.GoToFreelanceRegistratioin();

			_freelanceRegistrationFirstPage.ClickExistAccountAbbyyOnlineLink();

			_freelanceRegistrationSignInPage
				.FillSignInForm(_email, _wrongPassword)
				.ClickSignInButtonExpectingError();
			
			Assert.IsTrue(_freelanceRegistrationSignInPage.IsWrongPasswordErrorMessageDisplayed(),
				"Произошла ошибка:\nНе появилось валидационное сообщение о неверном пароле.");
		}

		[Test]
		public void CreateNewUserAddInCompanyRegistration()
		{
			_adminHelper
				.CreateNewUser(_email, _nickName, _password)
				.CreateAccountAdminIfNotExist(_email, _nickName, _nickName, LoginHelper.TestAccountName);

			_commonHelper.GoToFreelanceRegistratioin();

			_freelanceRegistrationFirstPage.ClickExistAccountAbbyyOnlineLink();

			_freelanceRegistrationSignInPage
				.FillSignInForm(_email, _password)
				.ClickSignInButtonWithInactivePersonalAccount();

			_freelanceRegistrationSecondPage
				.FillFreelanceRegistrationSecondStep(
					firstName: _firstName,
					lastName: _lastName)
				.ClickCreateAccountButton();

			_workspacePage.CloseHelpIfOpened();

			Assert.IsTrue(_workspacePage.IsNickNameMatch(_nickName),
				"Произошла ошибка:\nНеверное имя фрилансера в верхней панели.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatch(LoginHelper.PersonalAccountName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с названием.");
		}

		[Test]
		public void CreateNewUserWithActivePersonalAccountRegistration()
		{
			_adminHelper.CreateUserWithPersonalAccount(_email, _nickName, _password, aolUser: false);

			_commonHelper.GoToFreelanceRegistratioin();

			_freelanceRegistrationFirstPage.ClickExistAccountAbbyyOnlineLink();

			_freelanceRegistrationSignInPage
				.FillSignInForm(_email, _password)
				.ClickSignInButtonWithActivePersonalAccount();

			_workspacePage.CloseHelpIfOpened();

			Assert.IsTrue(_workspacePage.IsNickNameMatch(_nickName),
				"Произошла ошибка:\nНеверное имя фрилансера в верхней панели.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatch(LoginHelper.PersonalAccountName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с названием.");
		}

		[Test, Ignore("PRX-10800")]
		public void RegisterNewUserWithInActivePersAcc()
		{
			_adminHelper.CreateUserWithPersonalAccount(_email, _nickName, _password, activeState: false);

			_commonHelper.GoToFreelanceRegistratioin();

			_freelanceRegistrationFirstPage.ClickExistAccountAbbyyOnlineLink()
				.FillSignInForm(_email, _password)
				.ClickSignInButtonWithInactivePersonalAccount();

			_freelanceRegistrationSecondPage
				.FillFreelanceRegistrationSecondStep(_firstName, _lastName)
				.ClickCreateAccountButton();
			
			//TODO ЕЩЕ НЕ РЕАЛИЗОВАНО, должно появится сообщение, есть тикет, но сейчас не сделано так - PRX-5533
		}

		private AdminHelper _adminHelper;
	}
}