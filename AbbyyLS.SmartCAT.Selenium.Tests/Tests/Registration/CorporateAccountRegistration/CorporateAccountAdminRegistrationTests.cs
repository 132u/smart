using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	public class CorporateAccountAdminRegistrationTests<TWebDriverProvider> :
		RegistrationBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public CorporateAccountAdminRegistrationTests()
		{
			StartPage = StartPage.Admin;
		}

		[Test]
		public void ExistingCorporateAccountRegistrationTest()
		{
			_adminHelper
				.CreateNewUser(_email, _firstAndLastName, _password)
				.CreateAccountIfNotExist(accountName: _companyName, workflow: true)
				.AddUserToAdminGroupInAccountIfNotAdded(_email, _firstName, _lastName, _companyName);

			_registrationPage.GetPageExpectingRedirectToSignInPage(_email);

			Assert.AreEqual(_firstAndLastName + ", you already\r\nhave an account.", _signInPage.GetMessageText(),
				"Произошла ошибка:\n не появилось сообщение о том, что аккаунт уже существует.");

			_signInPage.SubmitFormExpectingWorkspacePage(_email, _password);

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstAndLastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_companyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
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
				.ClickCorporateAccountForm()
				.FillCorporateAccountPhone(_phoneNumber)
				.FillCorporateAccountCompanyName(_companyName)
				.ClickConfirmButton();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstAndLastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_companyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
		}

		[Test]
		public void LoginAndLogoutCompanyRegistrationTest()
		{
			_adminHelper
				.CreateNewUser(_email, _firstAndLastName, _password)
				.CreateAccountIfNotExist(accountName: _companyName, workflow: true)
				.AddUserToAdminGroupInAccountIfNotAdded(_email, _firstName, _lastName, _companyName);

			_signInPage
				.GetPage()
				.SubmitFormExpectingWorkspacePage(_email, _password);

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_companyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");

			_workspacePage.SignOutExpectingAlert();

			_signInPage.SubmitFormExpectingWorkspacePage(_email, _password);

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n имя пользователя в черной плашке не совпадает с ожидаемым именем.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_companyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");
		}
	}
}
