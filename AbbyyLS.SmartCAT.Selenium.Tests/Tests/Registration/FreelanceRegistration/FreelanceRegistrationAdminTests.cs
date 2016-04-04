using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Ignore("PRX-15132")]
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

			_freelanceRegistrationFirstPage
				.GetPage()
				.ClickExistAccountAbbyyOnlineLink();

			_freelanceRegistrationSignInPage
				.FillSignInForm(_email, _password)
				.ClickSignInButtonWithInactivePersonalAccount();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsNewProjectDocumentUploadPageOpened(),
				"Произошла ошибка:\nСтраница поректов не открылась.");

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_nickName),
				"Произошла ошибка:\nНеверное имя фрилансера в верхней панели.");
		}

		[Test]
		public void CreateNewUserLoginWrongPasswordRegistration()
		{
			_adminHelper.CreateNewUser(_email, _nickName, _password);

			_freelanceRegistrationFirstPage
				.GetPage()
				.ClickExistAccountAbbyyOnlineLink();

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
				.AddUserToAdminGroupInAccountIfNotAdded(_email, _nickName, _nickName, LoginHelper.TestAccountName);

			_freelanceRegistrationFirstPage
				.GetPage()
				.ClickExistAccountAbbyyOnlineLink();

			_freelanceRegistrationSignInPage
				.FillSignInForm(_email, _password)
				.ClickSignInButtonWithInactivePersonalAccount();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_nickName),
				"Произошла ошибка:\nНеверное имя фрилансера в верхней панели.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(LoginHelper.PersonalAccountName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с названием.");
		}

		[Test]
		public void CreateNewUserWithActivePersonalAccountRegistration()
		{
			_adminHelper.CreateUserWithSpecificAndPersonalAccount(
				_email, _firstName, _lastName, _nickName, _password, aolUser: false);

			_freelanceRegistrationFirstPage
				.GetPage()
				.ClickExistAccountAbbyyOnlineLink();

			_freelanceRegistrationSignInPage
				.FillSignInForm(_email, _password)
				.ClickSignInButtonWithActivePersonalAccount();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_nickName),
				"Произошла ошибка:\nНеверное имя фрилансера в верхней панели.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(LoginHelper.PersonalAccountName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с названием.");
		}
		
		private AdminHelper _adminHelper;
	}
}
