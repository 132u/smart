using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Ignore("PRX-15132")]
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	class CompanyRegistrationInvalidDataTests<TWebDriverProvider> : CompanyRegistrationBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreateAccountWithEmptyFirstNameTest()
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

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
		}

		[Test]
		public void CreateAccountWithEmptyLastNameTest()
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

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
		}

		[Test]
		public void CreateAccountWithEmptyCompanyNameTest()
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

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
		}

		[Test]
		public void CreateAccountWithEmptySubdomainTest()
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButton();

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
		}

		[Test]
		public void CreateAccountWithEmptyCompanyTypeTest()
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
					companyType: CompanyType.EmptyCompanyType)
				.DoubleClickCompanyTypeDropdown()
				.ClickCreateCorporateAccountButtonExpectingError();

			Assert.IsTrue(_companyRegistrationSecondPage.IsCreateCorporateAccountButtonInactive(),
				"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

			Assert.IsTrue(_companyRegistrationSecondPage.IsSelectCompanyTypeMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'Enter your company type' не появилось.");
		}

		[Test]
		public void CreateAccountWithEmptyPhoneNumberTest()
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
					companyType: CompanyType.LanguageServiceProvider,
					phoneNumber: string.Empty);

			Assert.IsTrue(_companyRegistrationSecondPage.IsCreateCorporateAccountButtonInactive(),
				"Произошла ошибка:\n кнопка 'Create Corporate Account' активна.");

			Assert.IsTrue(_companyRegistrationSecondPage.IsEnterPhoneMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'Enter your international phone number' не появилось.");
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
		public void EmailValidationTest(string email)
		{
			_companyRegistrationFirstPage
				.FillCompanyDataFirstStep(email, _password, _password)
				.ClickContinueButtonExpectingError();

			Assert.IsTrue(_companyRegistrationFirstPage.IsAlreadySignUpMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'You have already signed up for one of the ABBYY services with this email.' не появилось.");
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
	}
}
