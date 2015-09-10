using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.CompanyRegistration
{
	[TestFixture]
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

			GoToCompanyRegistration();

			_companyRegistrationHelper
				.ClickExistingAbbyyAccountLink()
				.FillSignInData(_email, _password)
				.ClickSignInButton()
				.FillCompanyDataSecondStep(_firstName, _lastName, _companyName, _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton()
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _companyName);
		}
		
		[Test]
		public void ExistingUserWithoutAccountRegistrationTest()
		{
			_adminHelper.CreateNewUser(_email, _nickName, _password);

			GoToCompanyRegistration();

			_companyRegistrationHelper
				.ClickExistingAbbyyAccountLink()
				.FillSignInData(_email, _password)
				.ClickSignInButton()
				.FillCompanyDataSecondStep(_firstName, _lastName, _companyName, _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton()
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _companyName);
		}

		[TestCase(false)]
		[TestCase(true)]
		public void ExistingUserWithPersonalAccountRegistrationTest(bool active)
		{
			_adminHelper
				.CreateNewUser(_email, _nickName, _password)
				.CreateNewPersonalAccount(_lastName, state: active);

			GoToCompanyRegistration();

			_companyRegistrationHelper
				.ClickExistingAbbyyAccountLink()
				.FillSignInData(_email, _password)
				.ClickSignInButton()
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					_companyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton()
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _companyName);
		}

		[Test]
		public void LoginAndLogoutCompanyRegistrationTest()
		{
			_adminHelper
				.CreateNewUser(_email, _nickName, _password)
				.CreateAccountIfNotExist(accountName: _accountUniqueName, workflow: true)
				.AddUserToSpecificAccount(_email, _accountUniqueName);

			GoToCompanyRegistration();

			_companyRegistrationHelper
				.ClickExistingAbbyyAccountLink()
				.FillSignInData(_email, _password)
				.ClickSignInButton()
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					_companyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton()
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _companyName)
				.SignOut()
				.SignIn(_email, _password)
				.SelectAccount(_companyName)
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _companyName);
		}

		private string _accountUniqueName;
		private string _email;
		private string _password;
		private string _firstName;
		private string _lastName;
		private string _companyName;
		private string _subDomain;
		private string _nickName;

		private int _companyNameMaxLenght = 40;

		private readonly CompanyRegistrationHelper _companyRegistrationHelper = new CompanyRegistrationHelper();
		private readonly AdminHelper _adminHelper = new AdminHelper();
	}
}
