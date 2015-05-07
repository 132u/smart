using System;
using System.Diagnostics;
using System.IO;
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

		protected string UserName { get; private set; }

		protected string UserFirstName { get; private set; }

		protected string UserSurname { get; private set; }

		protected string Login2 { get; private set; }

		protected string Password2 { get; private set; }

		protected string UserName2 { get; private set; }

		protected string UserFirstName2 { get; private set; }

		protected string UserSurname2 { get; private set; }

		protected string ProjectName { get; private set; }

		protected bool QuitDriverAfterTest { get; set; }

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
				var cfgAgentSpecific = TestSettingDefinition.Instance.Get<TargetServerConfig>();
				var cfgUserInfo = TestSettingDefinition.Instance.Get<UserInfoConfig>();
				createDriver();
				CreateUniqueNamesByDatetime();
				initializeRelatedToServerFields(cfgAgentSpecific);
				initializeRelatedToUserFields(cfgUserInfo);
				initializeHelpers();

				authorization();
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
				authorization();
			}
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

		protected void CreateUniqueNamesByDatetime()
		{
			ProjectName = "Selenium Project" + "_" + DateTime.Now.ToString("HHmmss");
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
			Driver.Manage().Window.Maximize();
		}

		private void authorization()
		{
			Driver.Navigate().GoToUrl(Url + "/sign-in");
			LoginHelper.SignIn(Login, Password);
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
			WorkspaceHelper = new WorkspaceHelper();
			CreateProjectHelper = new CreateProjectHelper();
		}
	}
}
