using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects.ChangeProjectStatusTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Projects]
	class ChangeProjectStatusByExecutiveTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> 
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public ChangeProjectStatusByExecutiveTests()
		{
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void SetUp()
		{
			_name = Guid.NewGuid() + "name";
			_surname = Guid.NewGuid() + "surname";
			_nickName = _name + " " + _surname;
			_password = Guid.NewGuid() + "password";
			_mail = Guid.NewGuid() + "@mailforspam.com";
			_projectUniqueName = Guid.NewGuid() + "project name";

			_projectsPage = new ProjectsPage(Driver);
			_adminHelper = new AdminHelper(Driver);
			_signInPage = new SignInPage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
		}

		[Test, Description("S-7160")]
		public void ManagerReviewStatusTest()
		{
			var targetSegment = "первое предложение.";

			_adminHelper
				.CreateNewUser(_mail, _nickName, _password)
				.AddUserToAdminGroupInAccountIfNotAdded(
					_mail,
					_name,
					_surname,
					LoginHelper.TestAccountName);

			_signInPage.GetPage();

			_loginHelper.LogInSmartCat(
				ThreadUser.Login,
				ThreadUser.NickName,
				ThreadUser.Password);

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				new[] { PathProvider.OneLineTxtFile },
				tasks: new[] { WorkflowTask.Translation });

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsPage.ClickAssignButtonOnPanel();

			_taskAssignmentPage
				.SetResponsible(_nickName, false)
				.ClickSaveButton()
				.GoToProjectsPage()
				.SignOut();

			_signInPage.SubmitFormExpectingWorkspacePage(_mail, _password);

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.OneLineTxtFile);

			_selectTaskDialog.SelectTask();

			_editorPage
				.FillSegmentTargetField(targetSegment)
				.ConfirmSegmentTranslation()
				.ClickDoneButton()
				.ClickHomeButtonExpectingProjectSettingsPage()
				.GoToProjectsPage();

			Assert.AreEqual(_projectsPage.GetProjectStatus(_projectUniqueName), ProjectStatus.ManagerReview.Description(),
				"Произошла ошибка:\n статус проекта не {0}", ProjectStatus.ManagerReview.Description());
		}

		private string _mail;
		private string _password;
		private string _name;
		private string _surname;
		private string _nickName;
		private string _projectUniqueName;

		private ProjectsPage _projectsPage;
		private ProjectSettingsPage _projectSettingsPage;
		private AdminHelper _adminHelper;
		private SignInPage _signInPage;
		private CreateProjectHelper _createProjectHelper;
		private TaskAssignmentPage _taskAssignmentPage;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
	}
}