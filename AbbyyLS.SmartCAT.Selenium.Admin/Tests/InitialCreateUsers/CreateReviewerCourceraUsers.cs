using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Admin.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[CreateUsers]
	[Coursera]
	public class CreateReviewerCourceraUsers<TWebDriverProvider> : BaseAdminTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void CreateCourseraUsersSetUp()
		{
			_courseraSignInDialog = new CourseraSignInDialog(Driver);
			_courseraHomePage = new CourseraHomePage(Driver);
			_courseraSignUpDialog = new CourseraSignUpDialog(Driver);
		}

		[Test]
		public void CreateReviewerUsers()
		{
			foreach (var user in ConfigurationManager.CourseraReviewerUserList.ToList())
			{
				_adminHelper
					.CreateNewUser(user.Login, user.Login, user.Password, aolUser: true)
					.AddUserToAdminGroupInAccountIfNotAdded(
						user.Login, user.Name, user.Surname, LoginHelper.CourseraAccountName);
			}
		}

		protected CourseraSignUpDialog _courseraSignUpDialog;
		protected CourseraSignInDialog _courseraSignInDialog;
		protected CourseraHomePage _courseraHomePage;
	}
}
