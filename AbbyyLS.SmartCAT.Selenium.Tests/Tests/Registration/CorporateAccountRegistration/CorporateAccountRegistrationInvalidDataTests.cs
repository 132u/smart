using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	class CorporateAccountRegistrationInvalidDataTests<TWebDriverProvider> :
		RegistrationBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-7101")]
		public void TwoTheSameCorporateUsersRegistrationTest()
		{
			var email = Guid.NewGuid() + "@mailforspam.com";

			_registrationPage
				.GetPage(email)
				.FillCorporateAccountRegistrationForm(
					password: _password,
					firstAndLastName: _firstAndLastName,
					phone: _phoneNumber,
					companyName: _companyName)
				.ClickConfirmButton();

			_workspacePage.SignOutExpectingAlert();

			_registrationPage.GetPageExpectingRedirectToSignInPage(email);

			Assert.AreEqual(_firstAndLastName + ", you already\r\nhave an account.", _signInPage.GetMessageText(),
				"Произошла ошибка:\n не появилось сообщение о том, что аккаунт уже существует.");
		}

		[TestCase("", Description = "S-13720")]
		[TestCase("      ")]
		public void EmptyPasswordTest(string password)
		{
			_registrationPage
				.ClickCorporateAccountForm()
				.FillCorporateAccountPassword(password)
				.FillCorporateAccountFirstAndLastName(_firstAndLastName)
				.FillCorporateAccountPhone(_phoneNumber)
				.FillCorporateAccountCompanyName(_companyName)
				.ClickConfirmButtonExpectingError();

			Assert.IsTrue(_registrationPage.IsEnterPasswordErrorMessageForCorporateAccountDisplayed(),
				"Произошла ошибка: не появилось сообщение об ошибке в поле 'Password'");
		}

		[Test, Description("S-13721")]
		public void EmptyFirstAndLastNameTest()
		{
			_registrationPage
				.ClickCorporateAccountForm()
				.FillCorporateAccountPassword(_password)
				.FillCorporateAccountPhone(_phoneNumber)
				.FillCorporateAccountCompanyName(_companyName)
				.ClickConfirmButtonExpectingError();

			Assert.IsTrue(_registrationPage.IsEnterFirstAndLastNameErrorMessageForCorporateAccountDisplayed(),
				"Произошла ошибка: не появилось сообщение об ошибке в поле 'First and last name'");
		}

		[Test, Description("S-13722")]
		public void EmptyPhoneTest()
		{
			_registrationPage
				.ClickCorporateAccountForm()
				.FillCorporateAccountPassword(_password)
				.FillCorporateAccountFirstAndLastName(_firstAndLastName)
				.FillCorporateAccountCompanyName(_companyName)
				.ClickConfirmButtonExpectingError();

			Assert.IsTrue(_registrationPage.IsEnterPhoneErrorMessageForCorporateAccountDisplayed(),
				"Произошла ошибка: не появилось сообщение об ошибке в поле 'Phone'");
		}

		[Test, Description("S-13723")]
		public void EmptyCompanyNameTest()
		{
			_registrationPage
				.ClickCorporateAccountForm()
				.FillCorporateAccountPassword(_password)
				.FillCorporateAccountFirstAndLastName(_firstAndLastName)
				.FillCorporateAccountPhone(_phoneNumber)
				.ClickConfirmButtonExpectingError();

			Assert.IsTrue(_registrationPage.IsEnterCompanyNameErrorMessageForCorporateAccountDisplayed(),
				"Произошла ошибка: не появилось сообщение об ошибке в поле 'Company Name'");
		}

		[Test, Description("S-13727")]
		public void ShortPasswordTest()
		{
			var password = "12345";

			_registrationPage
				.ClickCorporateAccountForm()
				.FillCorporateAccountPassword(password)
				.FillCorporateAccountFirstAndLastName(_firstAndLastName)
				.FillCorporateAccountPhone(_phoneNumber)
				.FillCorporateAccountCompanyName(_companyName)
				.ClickConfirmButtonExpectingError();

			Assert.IsTrue(_registrationPage.IsShortPasswordErrorMessageForCorporateAccountDisplayed(),
				"Произошла ошибка: не появилось сообщение о слишком коротком пароле");
		}

		[Test, Description("S-13729")]
		public void ShortCompanyNameTest()
		{
			var companyName = "A";

			_registrationPage
				.ClickCorporateAccountForm()
				.FillCorporateAccountPassword(_password)
				.FillCorporateAccountFirstAndLastName(_firstAndLastName)
				.FillCorporateAccountPhone(_phoneNumber)
				.FillCorporateAccountCompanyName(companyName)
				.ClickConfirmButtonExpectingError();

			Assert.IsTrue(_registrationPage.IsShortCompanyNameErrorMessageForCorporateAccountDisplayed(),
				"Произошла ошибка: не появилось сообщение о слишком коротком названии компании");
		}
	}
}
