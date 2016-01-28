﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Admin.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Admin.Tests.InitialCreateUsers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[CreateUsers]
	public class CreateReviewerCourceraUsers<TWebDriverProvider> : BaseAdminTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
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
		public void CreateReviewerUsers()
		{
			foreach (var user in ConfigurationManager.CourseraReviewerUserList)
			{
				_adminHelper
					.CreateNewUser(user.Login, user.Login, user.Password, aolUser: true)
					.AddUserToSpecificAccount(user.Login, LoginHelper.CourseraAccountName);
			}
		}

		protected CourseraSignUpDialog _courseraSignUpDialog;
		protected CourseraSignInDialog _courseraSignInDialog;
		protected CommonHelper _commonHelper;
		protected CourseraHomePage _courseraHomePage;
	}
}