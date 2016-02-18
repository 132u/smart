using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Admin.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	[Parallelizable(ParallelScope.Fixtures)]
	//[CreateUsers]
	[Coursera]
	class CreateCrowdsourceCourceraUsers<TWebDriverProvider> : BaseAdminTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void CreateCourseraUsersSetUp()
		{
			_commonHelper = new CommonHelper(Driver);
			_courseraSignInDialog = new CourseraSignInDialog(Driver);
			_courseraHomePage = new CourseraHomePage(Driver);
			_courseraSignUpDialog = new CourseraSignUpDialog(Driver);
		}

		[Test]
		public void CreateCrowdsourceUsers()
		{
			foreach (var user in ConfigurationManager.CourseraCrowdsourceUserList.ToList())
			{
				_adminHelper.CreateNewUser(user.Login, user.Login, user.Password, aolUser: true);
				_commonHelper.GoToCoursera();

				_courseraHomePage.ClickJoinButton();
				_courseraSignInDialog
					.LoginInCoursera(user.Login, user.Password)
					.ClickSigInButtonRedirectionOnRegistartionTab();

				_courseraSignUpDialog
					.FillRegistrationForm(user.Name, user.Surname)
					.ClickSignUpButton();

				_loginHelper.Authorize(StartPage.Admin, ThreadUser);
			}
		}

		protected CourseraSignUpDialog _courseraSignUpDialog;
		protected CourseraSignInDialog _courseraSignInDialog;
		protected CommonHelper _commonHelper;
		protected CourseraHomePage _courseraHomePage;
	}
}
