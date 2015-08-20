using System;
using System.Collections.Generic;
using System.IO;

using NConfiguration;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Configs;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
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

		protected StartPage StartPage = StartPage.Workspace;
		
		protected bool Standalone { get; private set; }

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

		protected List<TestUser> TestUserList { get; private set; }

		protected List<TestUser> TestCompanyList { get; private set; }

		protected List<TestUser> CourseraUserList { get; private set; }

		protected List<TestUser> AolUserList { get; private set; }

		protected List<TestUser> SocialNetworksUserList { get; private set; }

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

				Driver = new WebDriver(new TWebDriverProvider(), PathProvider.DriversTemporaryFolder, PathProvider.ExportFiles);

				initializeHelpers();
				authorize();
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Произошла ошибка в SetUp", ex); 
				throw;
			}
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
			catch (Exception ex)
			{
				Logger.WarnException("Ошибка при снятии скриншота", ex);
			}

			logTestSummary();

			if (Driver != null)
			{
				Driver.Dispose();
			}
		}

		protected void LogInSmartCat(
			string login,
			string nickName,
			string password,
			string accountName = "TestAccount")
		{
			Driver.Navigate().GoToUrl(Url + RelativeUrlProvider.SignIn);
			LoginHelper
				.SignIn(login, password)
				.SelectAccount(accountName)
				.SetUp(nickName, accountName);
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
			if (Standalone)
			{
				// Тесты запускаются на ОР
				Driver.Navigate().GoToUrl(WorkspaceUrl);

				return;
			}

			switch (StartPage)
			{
				case StartPage.Admin:
					LogInAdmin(Login, Password);
					break;

				case StartPage.CompanyRegistration:
					GoToCompanyRegistration();
					break;

				case StartPage.SignIn:
					GoToSignInPage();
					break;

				case StartPage.Workspace:
					LogInSmartCat(Login, NickName, Password);
					break;

				default:
					LogInSmartCat(Login, NickName, Password);
					break;
			}
		}

		protected void GoToSignInPage()
		{
			Driver.Navigate().GoToUrl(Url + RelativeUrlProvider.SignIn);
		}

		protected void GoToCompanyRegistration()
		{
			Driver.Navigate().GoToUrl(Url + RelativeUrlProvider.CorpReg);
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
			Standalone = config.Standalone;

			if (Standalone)
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

			WorkspaceUrl = string.IsNullOrWhiteSpace(config.Workspace) ? Url + RelativeUrlProvider.Workspace : config.Workspace;
		}

		private void initializeRelatedToUserFields()
		{
			var config = TestSettingDefinition.Instance.Get<UserInfoConfig>();

			Login = config.Login;
			Password = config.Password;
			UserName = config.Name ?? string.Empty;
			UserSurname = config.Surname ?? string.Empty;
			NickName = UserName;
			if (!string.IsNullOrEmpty(UserSurname))
			{
				NickName += " " + UserSurname;
			}

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
				SocialNetworksUserList = cfgTestUser.SocialNetworksUsers;
			}
			else
			{
				TestUserList = new List<TestUser>();
				TestCompanyList = new List<TestUser>();
				CourseraUserList = new List<TestUser>();
				AolUserList = new List<TestUser>();
				SocialNetworksUserList = new List<TestUser>();
			}
		}
	}
}
