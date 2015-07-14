using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.CompanyRegistration
{
	[TestFixture]
	[PriorityMajor]
	public class CompanyRegistrationTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public CompanyRegistrationTests()
		{
			StartPage = StartPage.CompanyRegistration;
		}

		[SetUp]
		public void SetUpCompanyRegistration()
		{
			_email = "e" + Guid.NewGuid().ToString().Substring(0, 8) +"@mailforspam.com";
			_password = "password" + Guid.NewGuid();
			_firstName = "firstName" + Guid.NewGuid();
			_lastName = "lastName" + Guid.NewGuid();
			_maximumCompanyName = ("companyName" + Guid.NewGuid()).Substring(0, _companyNameMaxLenght);
			_subDomain = "subDomainl" + Guid.NewGuid();
			_minimumCompanyName = ("c" + Guid.NewGuid()).Substring(0, 2);
		}

		[Test]
		public void CompanyRegistrationTest()
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton()
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					_maximumCompanyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton()
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName);
		}

		[Test]
		public void MinimumCompanyNameTest()
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_minimumCompanyName + "@mailforspam.com", _password, _password)
				.ClickContinueButton()
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					_maximumCompanyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton()
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName);
		}

		[Test]
		public void TwoCompaniesRegistrationTest()
		{
			var firstNameForSecondCompany = "firstName" + Guid.NewGuid();
			var lastNameForSecondCompany = "lastName" + Guid.NewGuid();
			var companyNameForSecondCompany = ("companyName" + Guid.NewGuid()).Substring(0, _companyNameMaxLenght);
			var subDomainForSecondCompany = "subDomain" + Guid.NewGuid();

			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton()
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					_maximumCompanyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton()
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName)
				.SignOut();

			GoToCompanyRegistration();

			_companyRegistrationHelper
				.ClickExistingAbbyyAccountLink()
				.FillSignInData(_email, _password)
				.ClickSignInButton()
				.FillCompanyDataSecondStep(
					firstNameForSecondCompany,
					lastNameForSecondCompany,
					companyNameForSecondCompany,
					subDomainForSecondCompany,
					companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton()
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(
					firstNameForSecondCompany + " " + lastNameForSecondCompany,
					companyNameForSecondCompany)
				.AssertAccountExistInList(_maximumCompanyName);
		}

		/// <summary>
		/// Тест регистрации юзера с существующим активным или неактивным аккаунтом в coursera/aol ( or log in with your ABBYY Online account )
		/// </summary>
		[Explicit("Тест для локального прогона, так как требует ввод данных юзеров в конфиг.")]
		[TestCase(0, "Активный юзер аол RegistrationTestUsers.xml")]
		[TestCase(1, "Неактивный юзер аол RegistrationTestUsers.xml")]
		[TestCase(2, "Активный юзер курсера RegistrationTestUsers.xml")]
		[TestCase(3, "Неактивный юзер курсера RegistrationTestUsers.xml")]
		public void CourseraAolUsersCompanyRegistration(int userNumber, string userTest)
		{
			if (TestCompanyList.Count == 0)
			{
				Assert.Ignore("Данные о тестовых пользователях для регистрации компаний отсутствуют.");
			}

			_companyRegistrationHelper.ClickExistingAbbyyAccountLink()
				.FillSignInData(TestCompanyList[userNumber].Login, TestCompanyList[userNumber].Password)
				.ClickSignInButton()
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton()
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName);
		}

		[TestCase(CompanyRegistrationField.FirstName)]
		[TestCase(CompanyRegistrationField.LastName)]
		[TestCase(CompanyRegistrationField.CompanyName)]
		[TestCase(CompanyRegistrationField.Subdomain)]
		[TestCase(CompanyRegistrationField.CompanyType)]
		[TestCase(CompanyRegistrationField.PhoneNumber)]
		public void CreateAccountButtonTest(CompanyRegistrationField field)
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

			switch (field)
			{
				case CompanyRegistrationField.FirstName:
					_companyRegistrationHelper
						.FillCompanyDataSecondStep(
							string.Empty,
							_lastName,
							_maximumCompanyName,
							_subDomain,
							companyType: CompanyType.LanguageServiceProvider)
						.AssertCreateCorporateAccountButtonInactive()
						.AssertEnterNameMessageDisplayed();
					break;

				case CompanyRegistrationField.LastName:
					_companyRegistrationHelper
						.FillCompanyDataSecondStep(
							_firstName,
							string.Empty,
							_maximumCompanyName,
							_subDomain,
							companyType: CompanyType.LanguageServiceProvider)
						.AssertCreateCorporateAccountButtonInactive()
						.AssertEnterLastNameMessageDisplayed();
					break;

				case CompanyRegistrationField.CompanyName:
					_companyRegistrationHelper
						.FillCompanyDataSecondStep(
							_firstName,
							_lastName,
							string.Empty,
							_subDomain,
							companyType: CompanyType.LanguageServiceProvider)
						.AssertCreateCorporateAccountButtonInactive()
						.AssertEnterCompanyNameMessageDisplayed();
					break;

				case CompanyRegistrationField.Subdomain:
					_companyRegistrationHelper
						.FillCompanyDataSecondStep(
							_firstName,
							_lastName,
							_maximumCompanyName,
							string.Empty,
							companyType: CompanyType.LanguageServiceProvider)
						.AssertCreateCorporateAccountButtonInactive()
						.AssertEnterDomainMessageDisplayed();
					break;

				case CompanyRegistrationField.PhoneNumber:
					_companyRegistrationHelper
						.FillCompanyDataSecondStep(
							_firstName,
							_lastName,
							_maximumCompanyName,
							_subDomain,
							companyType: CompanyType.LanguageServiceProvider,
							phoneNumber: string.Empty)
						.AssertCreateCorporateAccountButtonInactive()
						.AssertEnterPhoneMessageDisplayed();
					break;

				case CompanyRegistrationField.CompanyType:
					_companyRegistrationHelper
						.FillCompanyDataSecondStep(
							_firstName,
							_lastName,
							_maximumCompanyName,
							_subDomain,
							companyType: CompanyType.EmptyCompanyType)
						.DoubleClickCompanyTypeDropdown()
						.ClickCorporateAccountButton()
						.AssertCreateCorporateAccountButtonInactive()
						.AssertSelectCompanyTypeMessageDisplayed();
					break;

				default: 
					Assert.Fail("Произошла ошибка:\n недопустимое поле на странице регистрации компании.");
					break;
			}
		}

		[TestCase("a@a")]
		[TestCase("_@a")]
		[TestCase("-@a")]
		[TestCase(".@a")]
		[TestCase("a@:")]
		[TestCase("a@_")]
		[TestCase("a@-")]
		[TestCase("a@[")]
		[TestCase("a@]")]
		[TestCase("a@.")]
		[TestCase("dfsdfsdf@google.com")]
		//64 - local part, 253 - domen
		[Explicit("PRX-10569"), TestCase("64charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr@253charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr.com")]
		public void EmailValidTest(string email)
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(email, _password, _password)
				.ClickContinueButton(errorExpected:true)
				.AssertAlreadySignUpMessageDisplayed();
		}

		[TestCase("'@a")]
		[TestCase("\\@a")]
		[TestCase("!@a")]
		[TestCase("@@a")]
		[TestCase("\"@a")]
		[TestCase("#@a")]
		[TestCase("№@a")]
		[TestCase(";@a")]
		[TestCase("$@a")]
		[TestCase(":@a")]
		[TestCase("&@a")]
		[TestCase("?@a")]
		[TestCase("*@a")]
		[TestCase("=@a")]
		[TestCase("`@a")]
		[TestCase("[@a")]
		[TestCase("]@a")]
		[TestCase("{@a")]
		[TestCase("}@a")]
		[TestCase("|@a")]
		[TestCase("/@a")]
		[TestCase(",@a")]
		[TestCase("a@'")]
		[TestCase("a@\\")]
		[TestCase("a@!")]
		[TestCase("a@@")]
		[TestCase("a@\"")]
		[TestCase("a@#")]
		[TestCase("a@№")]
		[TestCase("a@;")]
		[TestCase("a@$")]
		[TestCase("a@&")]
		[TestCase("a@?")]
		[TestCase("a@*")]
		[TestCase("a@=")]
		[TestCase("a@`")]
		[TestCase("a@{")]
		[TestCase("a@}")]
		[TestCase("a@|")]
		[TestCase("a@/")]
		[TestCase("a@,")]
		[TestCase("emailgoogle.com")]
		[TestCase("emaifuyyl@goog le.com")]
		[TestCase("")]
		[TestCase(" ")]
		/*65 - local part, 253 - domen*/[TestCase("65charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr@253charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr.com")]
		/*64 - local part, 254 - domen*/[TestCase("64charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr@254charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr.com")]
		[TestCase("email@goo@sfd@ffgl@khhje.com")]
		[TestCase("gfgfgfgfgfgfgggfgfgf")]
		public void EmailInvalidTest(string email)
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(email, _password, _password)
				.AssertInvalidEmailMessageDisplayed()
				.AssertContinueButtonInactive();
		}

		[TestCase(" ")]
		[TestCase("1")]
		[TestCase("12")]
		[TestCase("123")]
		[TestCase("1234")]
		[TestCase("12345")]
		// 101 символ
		[Explicit("SCAT-10569"), TestCase("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111")]
		public void InvalidPasswordTest(string password)
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, password, password)
				.AssertMinimumLenghPasswordMessageDisplayed()
				.AssertContinueButtonInactive();
		}

		[TestCase("")]
		public void PasswordRequiredTest(string password)
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, password, password)
				.AssertInvalidPasswordMessageDisplayed()
				.AssertContinueButtonInactive();
		}

		[Test]
		public void SixSpacesPasswordValidationTest()
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, "      ", "      ")
				.AssertOnlySpacesPasswordMessageDisplayed()
				.AssertContinueButtonInactive();
		}

		[Test]
		public void PasswordMatchTest()
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, _password, "incorrectPassword")
				.ClickContinueButton(errorExpected: true)
				.AssertPasswordMatchMessageDisplayed()
				.AssertContinueButtonInactive();
		}

		[TestCase("1kjgkjg")]
		[TestCase("'kjgkjg")]
		public void CompanyNamePatternValidationTest(string companyName)
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton()
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					companyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider)
				.AssertCreateCorporateAccountButtonInactive()
				.AssertCompanyNamePatternInvalidMessageDisplayed();
		}

		[TestCase("s")]
		public void CompanyNameLenghtValidationTest(string companyName)
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton()
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					companyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider)
				.AssertCreateCorporateAccountButtonInactive()
				.AssertCompanyNameLengthInvalidMessageDisplayed();
		}

		[Test]
		public void MaximumCompanyNameTest()
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton()
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName + "extraCharacters", _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.AssertCompanyNameLenght()
				.ClickCreateCorporateAccountButton()
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName);
		}

		[TestCase("wwwasddddddd")]
		[TestCase("www.asddddddd")]
		[TestCase("-asddddddd")]
		[TestCase("_asddddddd")]
		[TestCase("2asddddddd")]
		public void SubdomainInvalidPatternTest(string subdomain)
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton()
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, subdomain, companyType: CompanyType.LanguageServiceProvider)
				.AssertCreateCorporateAccountButtonInactive()
				.AssertSubdomainPatternMessageDisplayed();
		}

		[TestCase("as")]
		[TestCase("a")]
		public void SubdomainInvalidLenghtTest(string subdomain)
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton()
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, subdomain, companyType: CompanyType.LanguageServiceProvider)
				.AssertCreateCorporateAccountButtonInactive()
				.AssertSubdomainMinimumLenghtMessageDisplayed();
		}

		[Test]
		public void TwoTheSameCompaniesRegistrationTest()
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton()
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton()
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName)
				.SignOut();

			GoToCompanyRegistration();

			_companyRegistrationHelper
				.ClickExistingAbbyyAccountLink()
				.FillSignInData(_email, _password)
				.ClickSignInButton()
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.AssertCreateCorporateAccountButtonInactive()
				.AssertCompanyNameAlreadyInUseMessageDisplayed();
		}

		[Test]
		public void AlreadySignUpTest()
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton()
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton()
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName)
				.SignOut();

			GoToCompanyRegistration();

			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton(errorExpected: true)
				.AssertAlreadySignUpMessageDisplayed();

			LogInSmartCat(_email, _password, _maximumCompanyName);

			_workspaceHelper.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName);
		}

		[Test]
		public void LoginRegisteredUser()
		{
			_companyRegistrationHelper
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton()
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton()
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName)
				.SignOut();

			LogInSmartCat(_email, _password, _maximumCompanyName);
		}

		private string _email;
		private string _password;
		private string _firstName;
		private string _lastName;
		private string _maximumCompanyName;
		private string _subDomain;
		private string _minimumCompanyName;
		private int _companyNameMaxLenght = 40;

		private readonly CompanyRegistrationHelper _companyRegistrationHelper = new CompanyRegistrationHelper();
		private readonly WorkspaceHelper _workspaceHelper = new WorkspaceHelper();
	}
}

