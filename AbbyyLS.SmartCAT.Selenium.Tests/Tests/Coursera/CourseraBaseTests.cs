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
		
		[SetUp]
		public void SetUp()
		{
			_courseraHomePage = new CourseraHomePage(Driver);
			_leaderboardPage = new LeaderboardPage(Driver);
			_coursesPage = new CoursesPage(Driver);
			_header = new HeaderMenu(Driver);
			_coursePage = new CoursePage(Driver);
			_editorPage = new EditorPage(Driver);
			_profilePage = new UserProfilePage(Driver);
			_editProfileDialog = new EditProfileDialog(Driver);
			_lecturesTab = new LecturesTab(Driver);

			CourseraCrowdsourceUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);
			CourseraReviewerUser = TakeUser(ConfigurationManager.CourseraReviewerUsers);
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
		
		protected EditorPage _editorPage;
		protected HeaderMenu _header;
		protected UserProfilePage _profilePage;
		protected CourseraHomePage _courseraHomePage;
		protected LeaderboardPage _leaderboardPage;
		protected CoursesPage _coursesPage;
		protected CoursePage _coursePage;
		protected EditProfileDialog _editProfileDialog;
		protected TestUser _secondUser;
		protected TestUser _thirdUser;
		protected LecturesTab _lecturesTab;
		protected ProjectSettingsHelper _projectSettingsHelper;
		protected string _courseNameForCompleteCourseTest;
		protected string _courseraNameProgressTests;
		protected LecturesTab _lectureTab;
		protected NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		protected NewProjectSettingsPage _newProjectSettingsPage;
		protected CreateProjectHelper _createProjectHelper;
		protected NewProjectWorkflowPage _newProjectWorkflowPage;
		protected ProjectsPage _projectsPage;
		protected WorkspacePage _workspacePage;
	}
}
