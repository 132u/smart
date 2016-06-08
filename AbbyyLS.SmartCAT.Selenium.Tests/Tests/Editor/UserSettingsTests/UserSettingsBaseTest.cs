using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor.UserSettingsTests
{
	public class UserSettingsBaseTest<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public UserSettingsBaseTest()
		{
			StartPage = StartPage.Admin;
		}

		[OneTimeSetUp]
		public void OniTimeSetUp()
		{
			_signInPage = new SignInPage(Driver);
			_selectAccountForm = new SelectAccountForm(Driver);
			_editorPage = new EditorPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_userPreferencesDialog = new UserPreferencesDialog(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);

			_adminHelper = new AdminHelper(Driver);
			_loginHelper = new LoginHelper(Driver);

			_password = Guid.NewGuid().ToString();
			_name = Guid.NewGuid().ToString();
			_surName = Guid.NewGuid().ToString();
			_mail = Guid.NewGuid() + "@forspam.com";
			_nickName = _name + _surName;
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_loginHelper.Authorize(StartPage, ThreadUser);

			_adminHelper.CreateUserWithSpecificAndPersonalAccount(
				_mail,
				_name,
				_surName,
				_nickName,
				_password,
				aolUser: false);

			_user = new TestUser
			{
				Login = _mail,
				Name = _name,
				Surname = _surName,
				NickName = _nickName,
				Password = _password
			};

			StartPage = StartPage.Workspace;
		}

		[SetUp]
		public void SetUp()
		{
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_loginHelper.Authorize(StartPage , _user);
		}

		protected LoginHelper _loginHelper;
		protected AdminHelper _adminHelper;
		protected EditorPage _editorPage;
		protected ProjectsPage _projectsPage;
		protected CreateProjectHelper _createProjectHelper;
		protected UserPreferencesDialog _userPreferencesDialog;
		protected SignInPage _signInPage;
		protected SelectAccountForm _selectAccountForm;
		protected NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		protected NewProjectSettingsPage _newProjectSettingsPage;
		protected NewProjectWorkflowPage _newProjectWorkflowPage;
		private TestUser _user;

		protected static readonly string _insertTMDescription = RevisionType.InsertTM.Description();
		protected static readonly string _sourceInsertionDescription = RevisionType.SourceInsertion.Description();
		protected static readonly string _MTDescription = RevisionType.InsertMT.Description();

		private string _password;
		private string _name;
		private string _surName;
		private string _nickName;
		private string _mail;
		protected string _projectUniqueName;
		protected string _document;
		protected string _documentName;
	}
}