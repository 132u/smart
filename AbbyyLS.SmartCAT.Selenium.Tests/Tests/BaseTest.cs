﻿﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
﻿using System.Drawing;
﻿using System.IO;
using System.Linq;

using NConfiguration;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DriversAndSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests
{
	[TestFixture(typeof(ChromeWebDriverSettings))]
	public class BaseTest<TWebDriverSettings> : BaseObject where TWebDriverSettings : IWebDriverSettings, new()
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
		protected string ProjectName { get; private set; }

		protected bool QuitDriverAfterTest { get; set; }

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
				var cfgAgentSpecific = TestSettingDefinition.Instance.Get<CatServerConfig>();
				var cfgUserInfo = TestSettingDefinition.Instance.Get<UserInfoConfig>();
				createDriver();
				CreateUniqueNamesByDatetime();
				initializeRelatedToUserFields(cfgUserInfo);
				initializeRelatedToServerFields(cfgAgentSpecific);
				initializeUsersAndCompanyList();
				initializeHelpers();
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Произошла ошибка в TestFixtureSetUp:\n", ex); 
				throw;
			}
		}

		[SetUp]
		public virtual void BeforeTest()
		{
			QuitDriverAfterTest = true;
			TestBeginTime = DateTime.Now;
			Logger.Info("Начало работы теста {0} \nВремя начала: {1}", TestContext.CurrentContext.Test.Name, TestBeginTime);

			if (Driver == null)
			{
				createDriver();
			}

			authorization();
		}

		[TestFixtureTearDown]
		public virtual void AfterClass()
		{
			ExitDriver();
		}

		[TearDown]
		public void TeardownBase()
		{
			// При вылете браузера возникает ошибка, пытаемся ее словить
			try
			{
				if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
				{
					// Создать папку для скриншотов провалившихся тестов
					var failResultPath = Path.Combine(PathTestResults, "FailedTests");
					Directory.CreateDirectory(failResultPath);

					var nameParts = TestContext.CurrentContext.Test.FullName.Split('.');
					var className = nameParts[nameParts.Length - 2].Replace('<', '(').Replace('>', ')');
					// Создать имя скриншота по имени теста
					var screenName = TestContext.CurrentContext.Test.Name;

					if (screenName.Contains("("))
					{
						// Убрать из названия теста аргументы (файлы)
						screenName = screenName.Substring(0, screenName.IndexOf("("));
					}

					screenName = className + "." + screenName;
					screenName = Path.Combine(failResultPath, screenName);
					Driver.TakeScreenshot(screenName);
				}
			}
			catch (Exception)
			{
				ExitDriver();
			}

			if (QuitDriverAfterTest)
			{
				ExitDriver();
			}

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

		public bool TestUserFileExist()
		{
			return File.Exists(PathProvider.TestUserFile);
		}

		protected void LogInAdmin(
			string login,
			string password)
		{
			Driver.Navigate().GoToUrl(AdminUrl);
			AdminHelper.SignIn(Login, Password);
		}

		protected void CreateUniqueNamesByDatetime()
		{
			ProjectName = "Selenium Project" + "_" + DateTime.Now.ToString("HHmmss");
		}

		protected void LogInSmartCat(
			string login,
			string password,
			string accountName = "TestAccount")
		{
			Driver.Navigate().GoToUrl(Url + "/sign-in");
			LoginHelper.SignIn(Login, Password, accountName);
		}

		protected void ExitDriver()
		{
			if (Driver != null)
			{
				Driver.Quit();
				Driver = null;
			}

			foreach (var item in ProcessNames.Select(Process.GetProcessesByName).SelectMany(processArray => processArray))
			{
				try
				{
					if (!item.HasExited)
					{
						item.Kill();
					}
				}
				catch (Exception ex)
				{
					Logger.ErrorException("Ошибка при завершении процесса (" + item.ProcessName + "): " + ex.Message, ex);
				}
			}
		}

		private void createDriver()
		{
			if (Driver == null)
			{
				var webDriverSettings = new TWebDriverSettings();
				Driver = webDriverSettings.Driver;
				ProcessNames = webDriverSettings.ProcessNames;
			}

			Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(15));
			Driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(60));
			Driver.Manage().Window.Maximize();
		}

		private void authorization()
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

		private void initializeRelatedToServerFields(CatServerConfig config)
		{
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

		private void initializeRelatedToUserFields(UserInfoConfig config)
		{
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
