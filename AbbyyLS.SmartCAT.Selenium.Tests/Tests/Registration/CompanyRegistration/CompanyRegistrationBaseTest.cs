using System;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	public class CompanyRegistrationBaseTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public CompanyRegistrationBaseTest()
		{
			StartPage = StartPage.CompanyRegistration;
		}

		[SetUp]
		public void SetUpCompanyRegistration()
		{
			_companyRegistrationFirstPage = new CompanyRegistrationFirstPage(Driver);
			_companyRegistrationSecondPage = new CompanyRegistrationSecondPage(Driver);
			_companyRegistrationSignInPage = new CompanyRegistrationSignInPage(Driver);

			_signInPage=new SignInPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
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

		protected string _email;
		protected string _password;
		protected string _firstName;
		protected string _lastName;
		protected string _maximumCompanyName;
		protected string _subDomain;
		protected string _nickName;
		protected int _companyNameMaxLenght = 40;

		protected SignInPage _signInPage;
		protected WorkspacePage _workspacePage;
		protected CommonHelper _commonHelper;
		protected LoginHelper _loginHelper;
		protected CompanyRegistrationFirstPage _companyRegistrationFirstPage;
		protected CompanyRegistrationSecondPage _companyRegistrationSecondPage;
		protected CompanyRegistrationSignInPage _companyRegistrationSignInPage;
	}
}
