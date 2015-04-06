﻿using System;
using System.IO;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using NConfiguration;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests
{
	public class BaseTest : BaseObject
	{
		
		protected static readonly string PathTestResults = Directory.GetParent(@"..\..\TestResults\").ToString();

		protected string Url { get; private set; }

		protected string WorkspaceUrl { get; private set; }

		protected string AdminUrl { get; private set; }

		protected string Login { get; private set; }

		protected string Password { get; private set; }

		protected string UserName { get; private set; }

		protected string UserFirstName { get; private set; }

		protected string UserSurname { get; private set; }

		protected string Login2 { get; private set; }

		protected string Password2 { get; private set; }

		protected string UserName2 { get; private set; }

		protected string UserFirstName2 { get; private set; }

		protected string UserSurname2 { get; private set; }

		protected string ProjectName { get; private set; }

		protected AdminHelper AdminHelper { get; private set; }

		protected LoginHelper LoginHelper { get; private set; }

		protected UsersRightsHelper UsersRightsHelper { get; private set; }

		protected CreateProjectHelper CreateProjectsHelper { get; private set; }

		protected ProjectSettingsHelper ProjectSettingsHelper { get; private set; }

		protected  EditorHelper EditorHelper { get; private set; }

		protected DateTime TestBeginTime { get; private set; }

		[TestFixtureSetUp]
		public void BeforeClass()
		{
			var cfgAgentSpecific = TestSettingDefinition.Instance.Get<TargetServerConfig>();
			var cfgUserInfo = TestSettingDefinition.Instance.Get<UserInfoConfig>();

			createDriver();
			CreateUniqueNamesByDatetime();
			initializeRelatedToServerFields(cfgAgentSpecific);
			initializeRelatedToUserFields(cfgUserInfo);
			initializeHelpers();
		}

		[SetUp]
		public virtual void BeforeTest()
		{
			TestBeginTime = DateTime.Now;
			Console.WriteLine(TestContext.CurrentContext.Test.Name + "\nStart: " + TestBeginTime);

			if (Driver == null)
			{
				// Если конструктор заново не вызывался, то надо заполнить Driver
				createDriver();
			}

			Driver.Navigate().GoToUrl(Url);
			LoginHelper.SignIn(Login, Password);
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

					// Создать имя скриншота по имени теста
					var screenName = TestContext.CurrentContext.Test.Name;

					if (screenName.Contains("("))
					{
						// Убрать из названия теста аргументы (файлы)
						screenName = screenName.Substring(0, screenName.IndexOf("("));
					}
					Driver.TakeScreenshot(screenName);
				}
			}
			catch (Exception)
			{
				ExitDriver();
			}

			var testFinishTime = DateTime.Now;
			Console.WriteLine("Finish: " + testFinishTime);
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
			Console.WriteLine(durResult);

			if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
			{
				// Если тест провалился
				Console.WriteLine("Fail!");
			}
		}

		protected void CreateUniqueNamesByDatetime()
		{
			ProjectName = "Selenium Project" + "_" + DateTime.Now.ToString("HHmmss");
		}

		protected void ExitDriver()
		{
			if (Driver != null)
			{
				// Закрыть драйвер
				Driver.Quit();
				// Очистить, чтобы при следующих тестах пересоздавалось
				Driver = null;
			}
		}

		private void createDriver()
		{
			var profile = new FirefoxProfile { AcceptUntrustedCertificates = true };
			profile.SetPreference("intl.accept_languages", "en");
			profile.SetPreference("browser.download.dir", PathTestResults);
			profile.SetPreference("browser.download.folderList", 2);
			profile.SetPreference("browser.download.useDownloadDir", false);
			profile.SetPreference
				("browser.helperApps.neverAsk.saveToDisk", "text/xml, text/csv, text/plain, text/log, application/zip, application/x-gzip, application/x-compressed, application/vnd.openxmlformats-officedocument.wordprocessingml.document, application/msword, application/octet-stream");
			Driver = new FirefoxDriver(profile);
			Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
			Driver.Manage().Window.Maximize();
		}

		private void initializeRelatedToServerFields(TargetServerConfig cfgAgentSpecific)
		{
			Url = "https://" + cfgAgentSpecific.Url;
			WorkspaceUrl = cfgAgentSpecific.Workspace;

			if (string.IsNullOrWhiteSpace(WorkspaceUrl))
			{
				WorkspaceUrl = "https://" + cfgAgentSpecific.Url + "/workspace";
			}

			AdminUrl = "http://" + cfgAgentSpecific.Url + ":81";
		}

		private void initializeRelatedToUserFields(UserInfoConfig cfgUserInfo)
		{
			Login = cfgUserInfo.Login;
			Password = cfgUserInfo.Password;
			UserName = cfgUserInfo.UserName;
			UserFirstName = UserName.Substring(0, UserName.IndexOf(' '));
			UserSurname = UserName.Substring(UserName.IndexOf(' ') + 1);

			Login2 = cfgUserInfo.Login2;
			Password2 = cfgUserInfo.Password2;
			UserName2 = cfgUserInfo.UserName2;
			UserFirstName2 = UserName2.Substring(0, UserName2.IndexOf(' '));
			UserSurname2 = UserName2.Substring(UserName2.IndexOf(' ') + 1);
		}

		private void initializeHelpers()
		{
			AdminHelper = new AdminHelper();
			LoginHelper = new LoginHelper();
			UsersRightsHelper = new UsersRightsHelper();
			CreateProjectsHelper = new CreateProjectHelper();
			ProjectSettingsHelper = new ProjectSettingsHelper();
			EditorHelper = new EditorHelper();
		}
	}
}