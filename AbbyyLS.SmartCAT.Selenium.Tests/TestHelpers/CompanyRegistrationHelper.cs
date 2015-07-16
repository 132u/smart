﻿using System.Windows.Forms;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class CompanyRegistrationHelper
	{
		public CompanyRegistrationHelper FillCompanyDataFirstStep(
			string email,
			string password,
			string confirmPassword)
		{
			BaseObject.InitPage(_companyRegistrationFirstPage);
			_companyRegistrationFirstPage
				.FillEmail(email)
				.FillPassword(password)
				.FillConfirmPassword(confirmPassword);

			return this;
		}

		public CompanyRegistrationHelper FillSignInData(
			string email,
			string password)
		{
			BaseObject.InitPage(_companyRegistrationSignInPage);
			_companyRegistrationSignInPage
				.FillEmail(email)
				.FillPassword(password);

			return this;
		}

		public CompanyRegistrationHelper ClickSignInButton()
		{
			BaseObject.InitPage(_companyRegistrationSignInPage);
			_companyRegistrationSignInPage.ClickSignInButton();

			return this;
		}

		public CompanyRegistrationHelper ClickContinueButton(bool errorExpected = false)
		{
			BaseObject.InitPage(_companyRegistrationFirstPage);

			if (errorExpected)
			{
				_companyRegistrationFirstPage.ClickContinueButton<CompanyRegistrationFirstPage>();
			}
			else
			{
				_companyRegistrationFirstPage.ClickContinueButton<CompanyRegistrationSecondPage>();
			}

			return this;
		}

		public CompanyRegistrationHelper AssertContinueButtonInactive()
		{
			BaseObject.InitPage(_companyRegistrationFirstPage);
			_companyRegistrationFirstPage.AssertContinueButtonInactive();

			return this;
		}

		public CompanyRegistrationHelper FillCompanyDataSecondStep(
			string firstName,
			string lastName,
			string companyName,
			string subDomain,
			CompanyType companyType,
			string phoneNumber = "12312312312312")
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage
				.FillFirstName(firstName)
				.FillLastName(lastName)
				.FillCompanyName(companyName)
				.FillSubdomain(subDomain)
				.FillPhoneNumber(phoneNumber)
				.SelectCompanyType(companyType.Description());
			
			return this;
		}

		public WorkspaceHelper ClickCreateCorporateAccountButton()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage
				.WaitCreateCorporateAccountButtonBecomeActive()
				.ClickCreateCorporateAccountButton<WorkspacePage>();

			return new WorkspaceHelper();
		}

		public CompanyRegistrationHelper AssertCreateCorporateAccountButtonInactive()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertCreateCorporateAccountButtonInactive();

			return this;
		}

		public CompanyRegistrationHelper AssertCompanyNameMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertEnterCompanyNameMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper ClickExistingAbbyyAccountLink()
		{
			BaseObject.InitPage(_companyRegistrationFirstPage);
			_companyRegistrationFirstPage.ClickExistingAbbyyAccountLink();

			return this;
		}

		public CompanyRegistrationHelper AssertInvalidEmailMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationFirstPage);
			_companyRegistrationFirstPage.AssertInvalidEmailMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertMinimumLenghPasswordMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationFirstPage);
			_companyRegistrationFirstPage.AssertMinimumLenghPasswordMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertOnlySpacesPasswordMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationFirstPage);
			_companyRegistrationFirstPage.AssertOnlySpacesPasswordMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertPasswordMatchMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationFirstPage);
			_companyRegistrationFirstPage.AssertPasswordMatchMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertAlreadySignUpMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationFirstPage);
			_companyRegistrationFirstPage.AssertAlreadySignUpMessageDisplayed();

			return this;
		}
		
		public CompanyRegistrationHelper AssertInvalidPasswordMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationFirstPage);
			_companyRegistrationFirstPage.AssertInvalidPasswordMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertCompanyNameLenght(int maximumCompanyNameLenght = 40)
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertCompanyNameLenght(maximumCompanyNameLenght);

			return this;
		}

		public CompanyRegistrationHelper AssertSubdomainMinimumLenghtMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertSubdomainLenghtMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertCompanyNamePatternInvalidMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertCompanyNamePatternMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertCompanyNameLengthInvalidMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertCompanyNameLengthInvalidMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertSubdomainPatternMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertSubdomainPatternMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertCompanyNameAlreadyInUseMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertCompanyNameMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertEnterDomainMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertEnterDomainMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertEnterNameMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertEnterNameMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertEnterLastNameMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertEnterLastNameMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertEnterCompanyNameMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertEnterCompanyNameMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper AssertEnterPhoneMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertEnterPhoneMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper DoubleClickCompanyTypeDropdown()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage
				.ClickCompanyTypeDropdown()
				.ClickCompanyTypeDropdown();

			return this;
		}

		public CompanyRegistrationHelper AssertSelectCompanyTypeMessageDisplayed()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage.AssertSelectCompanyTypeMessageDisplayed();

			return this;
		}

		public CompanyRegistrationHelper ClickCorporateAccountButton()
		{
			BaseObject.InitPage(_companyRegistrationSecondPage);
			_companyRegistrationSecondPage
				.WaitCreateCorporateAccountButtonBecomeActive()
				.ClickCreateCorporateAccountButton<CompanyRegistrationSecondPage>();

			return this;
		}

		private readonly CompanyRegistrationSecondPage _companyRegistrationSecondPage = new CompanyRegistrationSecondPage();
		private readonly CompanyRegistrationFirstPage _companyRegistrationFirstPage = new CompanyRegistrationFirstPage();
		private readonly CompanyRegistrationSignInPage _companyRegistrationSignInPage = new CompanyRegistrationSignInPage();
	}
}