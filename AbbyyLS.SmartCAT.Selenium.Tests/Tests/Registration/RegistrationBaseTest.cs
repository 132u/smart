using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	public class RegistrationBaseTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public RegistrationBaseTest()
		{
			StartPage = StartPage.Registration;
		}

		[SetUp]
		public void SetUp()
		{
			_registrationPage = new RegistrationPage(Driver);
			_signInPage =new SignInPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_selectAccountForm = new SelectAccountForm(Driver);
			_adminHelper = new AdminHelper(Driver);

			_firstName = "firstName" + Guid.NewGuid();
			_lastName = "lastName" + Guid.NewGuid();
			_email = Guid.NewGuid() +"@mailforspam.com";
			_password = "password" + Guid.NewGuid();
			_maximumCompanyName = ("companyName" + Guid.NewGuid()).Substring(0, _companyNameMaxLenght);
			_phoneNumber = "893628" + new Random(10000);
			_companyName = "C" + Guid.NewGuid();
			_wrongPassword = "wrongPassword";
			_firstAndLastName = _firstName + " " + _lastName;
		}

		protected string _email;
		protected string _password;
		protected string _maximumCompanyName;
		protected string _phoneNumber;
		protected string _companyName;
		protected string _wrongPassword;
		protected string _firstAndLastName;
		protected string _firstName;
		protected string _lastName;
		protected int _companyNameMaxLenght = 40;

		protected SignInPage _signInPage;
		protected WorkspacePage _workspacePage;
		protected AdminHelper _adminHelper;
		protected LoginHelper _loginHelper;
		protected RegistrationPage _registrationPage;
		protected SelectAccountForm _selectAccountForm;
	}
}
