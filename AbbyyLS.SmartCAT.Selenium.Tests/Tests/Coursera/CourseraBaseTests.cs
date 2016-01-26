using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;

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
		public void CourseraBaseTestsSetUp()
		{
			_courseraHomePage = new CourseraHomePage(Driver);
			_leaderboardPage = new LeaderboardPage(Driver);
			_coursesPage = new CoursesPage(Driver);
			_header = new HeaderMenu(Driver);
			_coursePage = new CoursePage(Driver);
			_editorPage = new EditorPage(Driver);
			_profilePage = new UserProfilePage(Driver);

			CourseraReviewerUser = TakeUser(ConfigurationManager.CourseraReviewerUsers);
			CourseraCrowdsourceUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);
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
		}

		protected EditorPage _editorPage;
		protected HeaderMenu _header;
		protected UserProfilePage _profilePage;
		protected CourseraHomePage _courseraHomePage;
		protected LeaderboardPage _leaderboardPage;
		protected CoursesPage _coursesPage;
		protected CoursePage _coursePage;
	}
}
