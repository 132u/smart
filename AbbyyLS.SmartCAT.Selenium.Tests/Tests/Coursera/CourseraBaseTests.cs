using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera.CoursePage;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	class CourseraBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public CourseraBaseTests()
		{
			StartPage = StartPage.Coursera;
		}

		[OneTimeSetUp]
		public virtual void OneTimeSetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_loginHelper = new LoginHelper(Driver);
			_lecturesTab = new LecturesTab(Driver);
			_courseraHomePage = new CourseraHomePage(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_header = new HeaderMenu(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_deleteTranslationDialog = new DeleteTranslationDialog(Driver);
			_coursesPage = new CoursesPage(Driver);
			_leaderboardPage = new LeaderboardPage(Driver);
			_coursePage = new CoursePage(Driver);
			_courseraEditorPage = new CourseraEditorPage(Driver);
			_profilePage = new UserProfilePage(Driver);
			_editProfileDialog = new EditProfileDialog(Driver);

			CourseraReviewerUser = TakeUser(ConfigurationManager.CourseraReviewerUsers);

			_courseraProject = "Coursera" + Guid.NewGuid();

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(CourseraReviewerUser.Login, CourseraReviewerUser.Password);

			_courseraHomePage.ClickWorkspaceButton();

			_createProjectHelper.CreateNewProject(
				projectName: _courseraProject,
				filesPaths: PathProvider.GetFilesForProgressTestsCourseraProject(),
				tasks: new[] { WorkflowTask.CrowdTranslation, WorkflowTask.CrowdReview });

			_workspacePage
				.ClickAccount()
				.ClickSignOutExpectingCourseraHomePage();

			ReturnUser(ConfigurationManager.CourseraReviewerUsers, CourseraReviewerUser);
		}

		[SetUp]
		public virtual void SetUp()
		{
			_translationText = "Test" + Guid.NewGuid();

			CourseraCrowdsourceUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);
		}

		[TearDown]
		public void CourseraBaseTestsTearDown()
		{
			if (CourseraReviewerUser != null)
			{
				ReturnUser(ConfigurationManager.CourseraReviewerUsers, CourseraReviewerUser);
			}

			if (CourseraCrowdsourceUser != null)
			{
				ReturnUser(ConfigurationManager.CourseraCrowdsourceUsers, CourseraCrowdsourceUser);
			}

			if (_secondUser != null)
			{
				ReturnUser(ConfigurationManager.CourseraCrowdsourceUsers, _secondUser);
			}

			if (_thirdUser != null)
			{
				ReturnUser(ConfigurationManager.CourseraCrowdsourceUsers, _secondUser);
			}
		}

		protected string _translationText;
		protected string _courseraProject;

		protected CourseraEditorPage _courseraEditorPage;
		protected HeaderMenu _header;
		protected UserProfilePage _profilePage;
		protected CourseraHomePage _courseraHomePage;
		protected LeaderboardPage _leaderboardPage;
		protected CoursesPage _coursesPage;
		protected CoursePage _coursePage;
		protected EditProfileDialog _editProfileDialog;
		protected TestUser _secondUser;
		protected TestUser _thirdUser;
		protected ProjectSettingsHelper _projectSettingsHelper;
		protected LecturesTab _lecturesTab;
		protected NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		protected NewProjectSettingsPage _newProjectSettingsPage;
		protected CreateProjectHelper _createProjectHelper;
		protected NewProjectWorkflowPage _newProjectWorkflowPage;
		protected ProjectsPage _projectsPage;
		protected WorkspacePage _workspacePage;
		protected DeleteTranslationDialog _deleteTranslationDialog;
	}
}
