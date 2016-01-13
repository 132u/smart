using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration.FreelanceRegistration;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Parallelizable(ParallelScope.Fixtures)]
	class FreelanceRegistrationBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void FreelanceRegistrationBaseTestsSetUp()
		{
			_freelanceRegistrationFirstPage = new FreelanceRegistrationFirstPage(Driver);
			_freelanceRegistrationSignInPage = new FreelanceRegistrationSignInPage(Driver);
			_signInPage = new SignInPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_adminHelper = new AdminHelper(Driver);
			_commonHelper = new CommonHelper(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);

			_email = "e" + Guid.NewGuid().ToString().Substring(0, 8) + "@mailforspam.com";
			_password = "password" + Guid.NewGuid();
			_firstName = "firstName" + Guid.NewGuid();
			_lastName = "lastName" + Guid.NewGuid();
			_nickName = _firstName + " " + _lastName;
			_wrongPassword = "wrongPassword";
		}

		protected string _email;
		protected string _password;
		protected string _firstName;
		protected string _lastName;
		protected string _maximumCompanyName;
		protected string _subDomain;
		protected string _nickName;
		protected int _companyNameMaxLenght = 40;
		protected string _wrongPassword;

		protected ProjectsPage _projectsPage;
		protected WorkspacePage _workspacePage;
		protected FreelanceRegistrationFirstPage _freelanceRegistrationFirstPage;
		protected FreelanceRegistrationSignInPage _freelanceRegistrationSignInPage;
		protected SignInPage _signInPage;
		protected LoginHelper _loginHelper;
		protected AdminHelper _adminHelper;
		protected CommonHelper _commonHelper;
		protected NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
	}
}
