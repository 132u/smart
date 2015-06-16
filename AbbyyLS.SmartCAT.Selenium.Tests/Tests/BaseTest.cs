﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
﻿using System.Drawing;
﻿using System.IO;
using System.Windows.Forms;

using NConfiguration;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests
{
	[TestFixture(typeof(ChromeDriverProvider))]
	public class BaseTest<TWebDriverProvider> : BaseObject where TWebDriverProvider : IWebDriverProvider, new()
	{
		protected string PathTestResults
		{
			get
			{
				return Directory.GetParent(@"..\TestResults\").ToString();
			}
		}

		protected string Url { get; private set; }

		protected string WorkspaceUrl { get; private set; }

		protected string AdminUrl { get; private set; }

		protected string Login { get; private set; }

		protected string Password { get; private set; }

		protected string NickName { get; private set; }

		protected string UserName { get; private set; }

		protected string UserSurname { get; private set; }

		protected string Login2 { get; private set; }

		protected string Password2 { get; private set; }

		protected string NickName2 { get; private set; }

		protected string UserName2 { get; private set; }

		protected string UserSurname2 { get; private set; }

		protected string RightsTestLogin { get; private set; }

		protected string RightsTestPassword { get; private set; }

		protected string RightsTestNickName { get; private set; }

		protected string RightsTestUserName { get; private set; }

		protected string RightsTestSurname { get; private set; }

		protected bool RecreateDriverAfterTest { get; set; }

		protected bool AdminLoginPage { get; set; }

		protected List<TestUser> TestUserList { get; private set; }

		protected List<TestUser> TestCompanyList { get; private set; }

		protected List<TestUser> CourseraUserList { get; private set; }

		protected List<TestUser> AolUserList { get; private set; }

		protected string[] ProcessNames { get; private set; }

		protected AdminHelper AdminHelper { get; private set; }

		protected LoginHelper LoginHelper { get; private set; }

		protected WorkspaceHelper WorkspaceHelper { get; private set; }

		protected CreateProjectHelper CreateProjectHelper { get; private set; }

		protected DateTime TestBeginTime { get; private set; }

		[TestFixtureSetUp]
		public void BeforeClass()
		{
			try
			{
				RecreateDriverAfterTest = true;

				initializeRelatedToUserFields();
				initializeRelatedToServerFields();
				initializeUsersAndCompanyList();
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Произошла ошибка в TestFixtureSetUp", ex);
				throw;
			}
		}

		[SetUp]
		public virtual void BeforeTest()
		{
			try
			{
				TestBeginTime = DateTime.Now;
				Logger.Info("Начало работы теста {0} \nВремя начала: {1}", TestContext.CurrentContext.Test.Name, TestBeginTime);

				if (Driver == null || RecreateDriverAfterTest)
				{
					Driver = new WebDriver(new TWebDriverProvider(), PathProvider.DriversTemporaryFolder);

					initializeHelpers();
					authorize();
				}
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Произошла ошибка в SetUp", ex); 
				throw;
			}
		}

		[TestFixtureTearDown]
		public virtual void AfterClass()
		{
			Driver.Dispose();
		}

		[TearDown]
		public void TeardownBase()
		{
			// При вылете браузера возникает ошибка, пытаемся ее словить
			try
			{
				if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
				{
					Driver.TakeScreenshot(Path.Combine(PathTestResults, "FailedTests"));
				}
			}
			catch (Exception)
			{
				Driver.Dispose();
			}

			if (RecreateDriverAfterTest)
			{
				Driver.Dispose();
			}

			logTestSummary();
		}

		protected void LogInSmartCat(
			string login,
			string password,
			string accountName = "TestAccount")
		{
			Driver.Navigate().GoToUrl(Url + "/sign-in");
			LoginHelper.SignIn(Login, Password, accountName);
		}

		protected void LogInAdmin(
			string login,
			string password)
		{
			Driver.Navigate().GoToUrl(AdminUrl);
			AdminHelper.SignIn(Login, Password);
		}

		private void authorize()
		{
			if (_standalone)
			{
				// Тесты запускаются на ОР
				Driver.Navigate().GoToUrl(WorkspaceUrl);

				return;
			}

			if (AdminLoginPage)
			{
				LogInAdmin(Login, Password);
			}
			else
			{
				LogInSmartCat(Login, Password);
			}
		}

		private void logTestSummary()
		{
			var testFinishTime = DateTime.Now;
			Logger.Info("Время окончания теста: {0}", testFinishTime);
			var duration = TimeSpan.FromTicks(testFinishTime.Ticks - TestBeginTime.Ticks);
			var durResult = "Duration: ";

			if (duration.TotalMinutes > 1)
			{
				durResult += duration.TotalMinutes + "min";
			}
			else
			{
				durResult += duration.TotalSeconds + "sec";
			}

			durResult += " (" + duration.TotalMilliseconds + "ms).";
			Logger.Info("Продолжительность теста {0}", durResult);

			if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
			{
				Logger.Error("Test Failed!");
			}
		}

		private void initializeRelatedToServerFields()
		{
			var config = TestSettingDefinition.Instance.Get<CatServerConfig>();

			var prefix = config.IsHttpsEnabled ? "https://" : "http://";
			_standalone = config.Standalone;

			if (_standalone)
			{
				// доменная авторизация в ОР
				var domainName = Login.Contains("@") ? Login.Substring(0, Login.IndexOf("@")) : Login;
				Url = string.Format("{0}{1}:{2}@{3}", prefix, domainName, Password, config.Url);
			}
			else
			{
				Url = prefix + config.Url;
				AdminUrl = "http://" + config.Url + ":81";
			}

			WorkspaceUrl = string.IsNullOrWhiteSpace(config.Workspace) ? Url + "/workspace" : config.Workspace;
		}

		private void initializeRelatedToUserFields()
		{
			var config = TestSettingDefinition.Instance.Get<UserInfoConfig>();

			Login = config.Login;
			Password = config.Password;
			UserName = config.Name ?? string.Empty;
			UserSurname = config.Surname ?? string.Empty;
			NickName = UserName + " " + UserSurname;

			Login2 = config.Login2;
			Password2 = config.Password2;
			NickName2 = config.NickName2;
			UserName2 = NickName2.Substring(0, NickName2.IndexOf(' '));
			UserSurname2 = NickName2.Substring(NickName2.IndexOf(' ') + 1);

			RightsTestLogin = config.TestRightsLogin;
			RightsTestPassword = config.TestRightsPassword;
			RightsTestNickName = config.TestRightsNickName;
			RightsTestUserName = RightsTestNickName.Substring(0, RightsTestNickName.IndexOf(' '));
			RightsTestSurname = RightsTestNickName.Substring(RightsTestNickName.IndexOf(' ') + 1);
		}

		private void initializeHelpers()
		{
			AdminHelper = new AdminHelper();
			LoginHelper = new LoginHelper();
			WorkspaceHelper = new WorkspaceHelper();
			CreateProjectHelper = new CreateProjectHelper();
		}

		private void initializeUsersAndCompanyList()
		{
			var cfgTestUser = TestSettingDefinition.Instance.TryGet<TestUsersConfig>();
			if (cfgTestUser != null)
			{
				TestUserList = cfgTestUser.Users;
				TestCompanyList = cfgTestUser.Companies;
				CourseraUserList = cfgTestUser.CourseraUsers;
				AolUserList = cfgTestUser.AolUsers;
			}
			else
			{
				TestUserList = new List<TestUser>();
				TestCompanyList = new List<TestUser>();
				CourseraUserList = new List<TestUser>();
				AolUserList = new List<TestUser>();
			}
		}

		private bool _standalone;
	}
}
