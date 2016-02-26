using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Ignore("PRX-15132")]
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	public class AdminCompanyRegistrationTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public AdminCompanyRegistrationTests()
		{
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void SetUpAdminCompanyRegistration()
		{
			_companyRegistrationFirstPage = new CompanyRegistrationFirstPage(Driver);
			_companyRegistrationSecondPage = new CompanyRegistrationSecondPage(Driver);
			_companyRegistrationSignInPage = new CompanyRegistrationSignInPage(Driver);

			_adminHelper = new AdminHelper(Driver);
			_commonHelper = new CommonHelper(Driver);

			int _companyNameMaxLenght = 40;

			_signInPage = new SignInPage(Driver);
			_workspacePage = new WorkspacePage(Driver);

			_accountUniqueName = "AccountUniqueName" + Guid.NewGuid().ToString().Substring(0, 5);
			_email = Guid.NewGuid().ToString().Substring(0, 8) + "@mailforspam.com";
			_password = "password" + Guid.NewGuid();
			_firstName = "firstName" + Guid.NewGuid();
			_lastName = "lastName" + Guid.NewGuid();
			_companyName = ("companyName" + Guid.NewGuid()).Substring(0, _companyNameMaxLenght);
			_subDomain = "subDomainl" + Guid.NewGuid();
			_nickName = "nickName" + Guid.NewGuid();
		}

		[Test]
		public void ExistingCorporateAccountRegistrationTest()
		{
			_adminHelper
				.CreateNewUser(_email, _nickName, _password)
				.CreateAccountIfNotExist(accountName: _accountUniqueName, workflow: true)
				.AddUserToSpecificAccount(_email, _accountUniqueName);

			_commonHelper.GoToCompanyRegistration();

			_companyRegistrationFirstPage.ClickExistingAbbyyAccountLink();

			_companyRegistrationSignInPage
				.FillSignInData(_email, _password)
				.ClickSignInButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(_firstName, _lastName, _companyName, _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_companyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
		}
		
		[Test]
		public void ExistingUserWithoutAccountRegistrationTest()
		{
			_adminHelper.CreateNewUser(_email, _nickName, _password);

			_commonHelper.GoToCompanyRegistration();

			_companyRegistrationFirstPage.ClickExistingAbbyyAccountLink();

			_companyRegistrationSignInPage
				.FillSignInData(_email, _password)
				.ClickSignInButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(_firstName, _lastName, _companyName, _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_companyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
		}

		[TestCase(false)]
		[TestCase(true)]
		public void ExistingUserWithPersonalAccountRegistrationTest(bool active)
		{
			_adminHelper
				.CreateNewUser(_email, _nickName, _password)
				.CreateNewPersonalAccount(_lastName, state: active);

			_commonHelper.GoToCompanyRegistration();

			_companyRegistrationFirstPage.ClickExistingAbbyyAccountLink();

			_companyRegistrationSignInPage
				.FillSignInData(_email, _password)
				.ClickSignInButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					_companyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_companyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
		}

		[Test]
		public void LoginAndLogoutCompanyRegistrationTest()
		{
			_adminHelper
				.CreateNewUser(_email, _nickName, _password)
				.CreateAccountIfNotExist(accountName: _accountUniqueName, workflow: true)
				.AddUserToSpecificAccount(_email, _accountUniqueName);

			_commonHelper.GoToCompanyRegistration();

			_companyRegistrationFirstPage.ClickExistingAbbyyAccountLink();

			_companyRegistrationSignInPage
				.FillSignInData(_email, _password)
				.ClickSignInButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					_companyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_companyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");

			_workspacePage.SignOutExpectingAlert();

			_signInPage
				.SubmitForm(_email, _password)
				.SelectAccount(_companyName);

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n имя пользователя в черной плашке не совпадает с ожидаемым именем.");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_companyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");
		}

		private string _accountUniqueName;
		private string _email;
		private string _password;
		private string _firstName;
		private string _lastName;
		private string _companyName;
		private string _subDomain;
		private string _nickName;

		private AdminHelper _adminHelper;
		private CommonHelper _commonHelper;
		private CompanyRegistrationFirstPage _companyRegistrationFirstPage;
		private CompanyRegistrationSecondPage _companyRegistrationSecondPage;
		private CompanyRegistrationSignInPage _companyRegistrationSignInPage;
		private SignInPage _signInPage;
		private WorkspacePage _workspacePage;
	}
}
