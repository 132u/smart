using System;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.ExplicitAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Ignore("PRX-15132")]
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	public class CompanyRegistrationValidDataTests<TWebDriverProvider> : CompanyRegistrationBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
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

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_maximumCompanyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
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

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(minimumCompanyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
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

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_maximumCompanyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");

			_workspacePage.SignOutExpectingAlert();

			_companyRegistrationFirstPage
				.GetPage()
				.ClickExistingAbbyyAccountLink();

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

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(firstNameForSecondCompany + " " + lastNameForSecondCompany),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(companyNameForSecondCompany),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");

			_workspacePage.ClickAccount();

			Assert.IsTrue(_workspacePage.IsAccountListContainsAccountName(_maximumCompanyName),
				"Произошла ошибка:\n список аккаунтов не содержит {0} аккаунт.", _maximumCompanyName);
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

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_maximumCompanyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
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

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_maximumCompanyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
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

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_maximumCompanyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");

			_workspacePage.SignOutExpectingAlert();

			_companyRegistrationFirstPage
				.GetPage()
				.ClickExistingAbbyyAccountLink();

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

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_maximumCompanyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");

			_workspacePage.SignOutExpectingAlert();

			_companyRegistrationFirstPage
				.GetPage()
				.FillCompanyDataFirstStep(_email, _password, _password)
				.ClickContinueButtonExpectingError();

			Assert.IsTrue(_companyRegistrationFirstPage.IsAlreadySignUpMessageDisplayed(),
				"Произошла ошибка:\n сообщение 'You have already signed up for one of the ABBYY services with this email.' не появилось.");

			_signInPage
				.GetPage()
				.SubmitFormExpectingWorkspacePage(_email, _password);

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_maximumCompanyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
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

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstName + " " + _lastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_maximumCompanyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");

			_workspacePage.SignOutExpectingAlert();

			_signInPage.GetPage();

			_loginHelper.LogInSmartCat(_email, _nickName, _password, _maximumCompanyName);
		}
	}
}

