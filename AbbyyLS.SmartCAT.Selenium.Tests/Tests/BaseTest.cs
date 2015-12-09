using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NLog;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

using OpenQA.Selenium;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests
{
	[TestFixture(typeof(ChromeDriverProvider))]
	public class BaseTest<TWebDriverProvider> : BaseObject where TWebDriverProvider : IWebDriverProvider, new()
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();
		public WebDriver Driver { get; private set; }

		[OneTimeSetUp]
		public void BeforeClass()
		{
			try
			{
				ThreadUser = TakeUser(ConfigurationManager.Users);
				Driver = new WebDriver(new TWebDriverProvider(), PathProvider.DriversTemporaryFolder, PathProvider.ExportFiles);
			}
			catch (Exception e)
			{
				Logger.ErrorException("Произошла ошибка в TestFixtureSetUp", e);
				throw;
			}
		}

		[SetUp]
		public virtual void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);

				_loginHelper = new LoginHelper(Driver);
				_loginHelper.Authorize(StartPage, ThreadUser);
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp", ex.ToString());
				throw;
			}
		}

		[TearDown]
		public void TeardownBase()
		{
			try
			{
				if (TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed))
				{
					Driver.TakeScreenshot(Path.Combine(PathProvider.ResultsFolderPath, "FailedTests"));
				}
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Ошибка при снятии скриншота", ex.ToString());
			}

			CustomTestContext.WriteLine("Окончание работы теста {0}", TestContext.CurrentContext.Test.Name);

			Driver.SwitchToNewTab();
		}

		[OneTimeTearDown]
		public void TestFixtureTearDown()
		{
			ReturnUser(ConfigurationManager.Users, ThreadUser);

			if (AdditionalThreadUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalThreadUser);
			}

			if (Driver != null)
			{
				Driver.Dispose();
			}
		}

		public TestUser TakeUser(ConcurrentBag<TestUser> users)
		{
			TestUser user = null;
			var timer = 0;

			while (!users.TryTake(out user) && timer <= 300)
			{
				Thread.Sleep(1000);
				timer++;
			}

			if (timer > 300)
			{
				throw new Exception("Произошла ошибка:\n нет пользователей в очереди");
			}
			
			CustomTestContext.WriteLine("Пользователь {0} взят из очереди.", user.Login);
			
			return user;
		}

		public void ReturnUser(ConcurrentBag<TestUser> users, TestUser user)
		{
			try
			{
				if (!users.Contains(user))
				{
					users.Add(user);
					CustomTestContext.WriteLine("Пользователь {0} возвращен в очередь.", user.Login);
				}
			}
			catch
			{
				CustomTestContext.WriteLine("Произошла ошибка: \nне удалось вернуть пользователя {0} в очередь.", user.Login);
			}
		}

		public TestUser ThreadUser { get; protected set; }
		public TestUser AdditionalThreadUser { get; protected set; }
		protected StartPage StartPage = StartPage.Workspace;
		private LoginHelper _loginHelper;
	}
}
