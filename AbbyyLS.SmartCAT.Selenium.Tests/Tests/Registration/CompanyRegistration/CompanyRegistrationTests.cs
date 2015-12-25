using System;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.ExplicitAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Parallelizable(ParallelScope.Fixtures)]
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
			_companyRegistrationFirstPage = new CompanyRegistrationFirstPage(Driver);
			_companyRegistrationSecondPage = new CompanyRegistrationSecondPage(Driver);
			_companyRegistrationSignInPage = new CompanyRegistrationSignInPage(Driver);

			_workspaceHelper = new WorkspaceHelper(Driver);
			_commonHelper = new CommonHelper(Driver);
			_loginHelper = new LoginHelper(Driver);

			_email = "e" + Guid.NewGuid().ToString().Substring(0, 8) +"@mailforspam.com";
			_password = "password" + Guid.NewGuid();
			_firstName = "firstName" + Guid.NewGuid();
			_lastName = "lastName" + Guid.NewGuid();
			_maximumCompanyName = ("companyName" + Guid.NewGuid()).Substring(0, _companyNameMaxLenght);
			_subDomain = "subDomainl" + Guid.NewGuid();
			_nickName = _firstName + " " + _lastName;
		}

		[Test]
		public void CompanyRegistrationTest()
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					_maximumCompanyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton();

			_workspaceHelper
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName);
		}

		[Test]
		public void MinimumCompanyNameTest()
		{
			var minimumCompanyName = new String(Guid.NewGuid().ToString().Where(Char.IsLetter).Take(2).ToArray());

			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					minimumCompanyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton();

			_workspaceHelper
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, minimumCompanyName);
		}

		[Test]
		public void TwoCompaniesRegistrationTest()
		{
			var firstNameForSecondCompany = "firstName" + Guid.NewGuid();
			var lastNameForSecondCompany = "lastName" + Guid.NewGuid();
			var companyNameForSecondCompany = ("companyName" + Guid.NewGuid()).Substring(0, _companyNameMaxLenght);
			var subDomainForSecondCompany = "subDomain" + Guid.NewGuid();

			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					_maximumCompanyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton();

			_workspaceHelper
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName)
				.SignOutAssumingAlert();

			_commonHelper.GoToCompanyRegistration();

			_companyRegistrationFirstPage.ClickExistingAbbyyAccountLink();

			_companyRegistrationSignInPage
				.FillSignInData(_email, _password)
				.ClickSignInButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(
					firstNameForSecondCompany,
					lastNameForSecondCompany,
					companyNameForSecondCompany,
					subDomainForSecondCompany,
					companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton();

			_workspaceHelper
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(
					firstNameForSecondCompany + " " + lastNameForSecondCompany,
					companyNameForSecondCompany)
				.AssertAccountExistInList(_maximumCompanyName);
		}

		/// <summary>
		/// Тест регистрации юзера с существующим активным или неактивным аккаунтом в coursera/aol ( or log in with your ABBYY Online account )
		/// </summary>
		[UserDataExplicit]
		[TestCase(0, "Активный юзер аол RegistrationTestUsers.xml")]
		[TestCase(1, "Неактивный юзер аол RegistrationTestUsers.xml")]
		[TestCase(2, "Активный юзер курсера RegistrationTestUsers.xml")]
		[TestCase(3, "Неактивный юзер курсера RegistrationTestUsers.xml")]
		public void CourseraAolUsersCompanyRegistration(int userNumber, string userTest)
		{
			if (ConfigurationManager.TestCompanyList.Count == 0)
			{
				Assert.Ignore("Данные о тестовых пользователях для регистрации компаний отсутствуют.");
			}

			_companyRegistrationFirstPage.ClickExistingAbbyyAccountLink();

			_companyRegistrationSignInPage
				.FillSignInData(ConfigurationManager.TestCompanyList[userNumber].Login, ConfigurationManager.TestCompanyList[userNumber].Password)
				.ClickSignInButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton();

			_workspaceHelper
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
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

			switch (field)
			{
				case CompanyRegistrationField.FirstName:
					_companyRegistrationSecondPage
						.FillCompanyDataSecondStep(
							string.Empty,
							_lastName,
							_maximumCompanyName,
							_subDomain,
							companyType: CompanyType.LanguageServiceProvider);

					Assert.IsTrue(_companyRegistrationSecondPage.IsCreateCorporateAccountButtonInactive(),
						"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

					Assert.IsTrue(_companyRegistrationSecondPage.IsEnterNameMessageDisplayed(),
						"Произошла ошибка:\n сообщение 'Enter your name' не появилось.");

					break;

				case CompanyRegistrationField.LastName:
					_companyRegistrationSecondPage
						.FillCompanyDataSecondStep(
							_firstName,
							string.Empty,
							_maximumCompanyName,
							_subDomain,
							companyType: CompanyType.LanguageServiceProvider);

					Assert.IsTrue(_companyRegistrationSecondPage.IsCreateCorporateAccountButtonInactive(),
						"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

					Assert.IsTrue(_companyRegistrationSecondPage.IsEnterLastNameMessageDisplayed(),
						"Произошла ошибка:\n сообщение 'Enter your last name' не появилось.");

					break;

				case CompanyRegistrationField.CompanyName:
					_companyRegistrationSecondPage
						.FillCompanyDataSecondStep(
							_firstName,
							_lastName,
							string.Empty,
							_subDomain,
							companyType: CompanyType.LanguageServiceProvider);

					Assert.IsTrue(_companyRegistrationSecondPage.IsCreateCorporateAccountButtonInactive(),
						"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

					Assert.IsTrue(_companyRegistrationSecondPage.IsEnterCompanyNameMessageDisplayed(),
						"Произошла ошибка:\n сообщение 'Enter your company name' не появилось.");

					break;

				case CompanyRegistrationField.Subdomain:
					_companyRegistrationSecondPage
						.FillCompanyDataSecondStep(
							_firstName,
							_lastName,
							_maximumCompanyName,
							string.Empty,
							companyType: CompanyType.LanguageServiceProvider);

					Assert.IsTrue(_companyRegistrationSecondPage.IsCreateCorporateAccountButtonInactive(),
						"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

					Assert.IsTrue(_companyRegistrationSecondPage.IsEnterDomainMessageDisplayed(),
						"Произошла ошибка:\n сообщение 'Enter your domain name' не появилось.");

					break;

				case CompanyRegistrationField.PhoneNumber:
					_companyRegistrationSecondPage
						.FillCompanyDataSecondStep(
							_firstName,
							_lastName,
							_maximumCompanyName,
							_subDomain,
							companyType: CompanyType.LanguageServiceProvider,
							phoneNumber: string.Empty);

					Assert.IsTrue(_companyRegistrationSecondPage.IsCreateCorporateAccountButtonInactive(),
						"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

					Assert.IsTrue(_companyRegistrationSecondPage.IsEnterPhoneMessageDisplayed(),
						"Произошла ошибка:\n сообщение 'Enter your international phone number' не появилось.");

					break;

				case CompanyRegistrationField.CompanyType:
					_companyRegistrationSecondPage
						.FillCompanyDataSecondStep(
							_firstName,
							_lastName,
							_maximumCompanyName,
							_subDomain,
							companyType: CompanyType.EmptyCompanyType)
						.DoubleClickCompanyTypeDropdown()
						.ClickCreateCorporateAccountButton();

					Assert.IsTrue(_companyRegistrationSecondPage.IsCreateCorporateAccountButtonInactive(),
						"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

					Assert.IsTrue(_companyRegistrationSecondPage.IsSelectCompanyTypeMessageDisplayed(),
						"Произошла ошибка:\n сообщение 'Enter your company type' не появилось.");

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
		[TestCase("64charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr@" +
			"253charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr" +
			"rrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr" +
			"rrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr" +
			"rrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr.com", Ignore = "PRX-10569")]
		public void EmailValidTest(string email)
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(email, _password, _password)
				.ClickContinueButtonExpectingError();

			Assert.IsTrue(_companyRegistrationFirstPage.IsAlreadySignUpMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'You have already signed up for one of the ABBYY services with this email.' не появилось.");
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
		/*65 - local part, 253 - domen*/
		[TestCase("65charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr@253charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr.com")]
		/*64 - local part, 254 - domen*/
		[TestCase("64charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr@254charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr.com")]
		[TestCase("email@goo@sfd@ffgl@khhje.com")]
		[TestCase("gfgfgfgfgfgfgggfgfgf")]
		public void EmailInvalidTest(string email)
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(email, _password, _password);

			Assert.IsTrue(_companyRegistrationFirstPage.IsInvalidEmailMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'Invalid email' не появилось.");

			Assert.IsTrue(_companyRegistrationFirstPage.IsContinueButtonInactive(),
				"Произошла ошибка:\n кнопка Continue активна.");
		}

		[TestCase(" ")]
		[TestCase("1")]
		[TestCase("12")]
		[TestCase("123")]
		[TestCase("1234")]
		[TestCase("12345")]
		// 101 символ
		[TestCase("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111",
			Ignore = "PRX-10569")]
		public void InvalidPasswordTest(string password)
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, password, password);

			Assert.IsTrue(_companyRegistrationFirstPage.IsMinimumLenghPasswordMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'The password must have at least 6 characters' не появилось.");

			Assert.IsTrue(_companyRegistrationFirstPage.IsContinueButtonInactive(),
				"Произошла ошибка:\n кнопка Continue активна.");
		}

		[TestCase("")]
		public void PasswordRequiredTest(string password)
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, password, password);

			Assert.IsTrue(_companyRegistrationFirstPage.IsInvalidPasswordMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'The password must have at least 6 characters.' не появилось.");

			Assert.IsTrue(_companyRegistrationFirstPage.IsContinueButtonInactive(),
				"Произошла ошибка:\n кнопка Continue активна.");
		}

		[Test]
		public void SixSpacesPasswordValidationTest()
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, "      ", "      ");

			Assert.IsTrue(_companyRegistrationFirstPage.IsOnlySpacesPasswordMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'The password cannot consist of spaces only' не появилось.");

			Assert.IsTrue(_companyRegistrationFirstPage.IsContinueButtonInactive(),
				"Произошла ошибка:\n кнопка Continue активна.");
		}

		[Test]
		public void PasswordMatchTest()
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, "incorrectPassword")
				.ClickContinueButtonExpectingError();

			Assert.IsTrue(_companyRegistrationFirstPage.IsPasswordMatchMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'The passwords do not match' не появилось.");

			Assert.IsTrue(_companyRegistrationFirstPage.IsContinueButtonInactive(),
				"Произошла ошибка:\n кнопка Continue активна.");
		}

		[TestCase("1kjgkjg")]
		[TestCase("'kjgkjg")]
		public void CompanyNamePatternValidationTest(string companyName)
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					companyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider);

			Assert.IsTrue(_companyRegistrationSecondPage.IsCreateCorporateAccountButtonInactive(),
						"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

			Assert.IsTrue(_companyRegistrationSecondPage.IsCompanyNameInvalidPatternMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'The company name must begin with a letter or a quotation mark' не появилось.");
		}

		[TestCase("s")]
		public void CompanyNameLenghtValidationTest(string companyName)
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(
					_firstName,
					_lastName,
					companyName,
					_subDomain,
					companyType: CompanyType.LanguageServiceProvider);

			Assert.IsTrue(_companyRegistrationSecondPage.IsCreateCorporateAccountButtonInactive(),
						"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

			Assert.IsTrue(_companyRegistrationSecondPage.IsCompanyNameLengthInvalidMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'The company name must contain at least 2 characters.' не появилось.");
		}

		[Test]
		public void MaximumCompanyNameTest()
		{
			int maximumCompanyNameLenght = 40;

			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName + "extraCharacters", _subDomain, companyType: CompanyType.LanguageServiceProvider);

			Assert.IsTrue(_companyRegistrationSecondPage.IsCompanyNameCutting(maximumCompanyNameLenght),
				"Произошла ошибка:\n название компании не обрезается до максимально допустимой длины в {0} символов.", maximumCompanyNameLenght);

			_companyRegistrationSecondPage.ClickCreateCorporateAccountButton();

			_workspaceHelper
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
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, subdomain, companyType: CompanyType.LanguageServiceProvider);

			Assert.IsTrue(_companyRegistrationSecondPage.IsCreateCorporateAccountButtonInactive(),
						"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

			Assert.IsTrue(_companyRegistrationSecondPage.IsSubdomainPatternMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'Use only latin letters, digits, hyphen and underscope...' не появилось.");
		}

		[TestCase("as")]
		[TestCase("a")]
		public void SubdomainInvalidLenghtTest(string subdomain)
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, subdomain,
					companyType: CompanyType.LanguageServiceProvider);

			Assert.IsTrue(_companyRegistrationSecondPage.IsCreateCorporateAccountButtonInactive(),
						"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

			Assert.IsTrue(_companyRegistrationSecondPage.IsSubdomainLenghtMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'The domain name must contain at least 3 characters' не появилось.");
		}

		[Test]
		public void TwoTheSameCompaniesRegistrationTest()
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton();

			_workspaceHelper
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName)
				.SignOutAssumingAlert();

			_commonHelper.GoToCompanyRegistration();

			_companyRegistrationFirstPage.ClickExistingAbbyyAccountLink();

			_companyRegistrationSignInPage
				.FillSignInData(_email, _password)
				.ClickSignInButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, _subDomain, companyType: CompanyType.LanguageServiceProvider);

			Assert.IsTrue(_companyRegistrationSecondPage.IsCreateCorporateAccountButtonInactive(),
						"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

			Assert.IsTrue(_companyRegistrationSecondPage.IsCompanyNameAlredyInUseMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'A company with this name already exists' не появилось.");
		}

		[Test]
		public void AlreadySignUpTest()
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton();

			_workspaceHelper
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName)
				.SignOutAssumingAlert();

			_commonHelper.GoToCompanyRegistration();

			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButtonExpectingError();

			Assert.IsTrue(_companyRegistrationFirstPage.IsAlreadySignUpMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'You have already signed up for one of the ABBYY services with this email.' не появилось.");

			_commonHelper.GoToSignInPage();
			_loginHelper.LogInSmartCat(_email, _nickName, _password, _maximumCompanyName);

			_workspaceHelper.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName);
		}

		[Test]
		public void LoginRegisteredUser()
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

			_companyRegistrationSecondPage
				.FillCompanyDataSecondStep(_firstName, _lastName, _maximumCompanyName, _subDomain, companyType: CompanyType.LanguageServiceProvider)
				.ClickCreateCorporateAccountButton();

			_workspaceHelper
				.CloseTour()
				.AssertUserNameAndAccountNameCorrect(_firstName + " " + _lastName, _maximumCompanyName)
				.SignOutAssumingAlert();

			_commonHelper.GoToSignInPage();
			_loginHelper.LogInSmartCat(_email, _nickName, _password, _maximumCompanyName);
		}

		private string _email;
		private string _password;
		private string _firstName;
		private string _lastName;
		private string _maximumCompanyName;
		private string _subDomain;
		private string _nickName;
		private int _companyNameMaxLenght = 40;

		private WorkspaceHelper _workspaceHelper;
		private CommonHelper _commonHelper;
		private LoginHelper _loginHelper;
		private CompanyRegistrationFirstPage _companyRegistrationFirstPage;
		private CompanyRegistrationSecondPage _companyRegistrationSecondPage;
		private CompanyRegistrationSignInPage _companyRegistrationSignInPage;
	}
}

