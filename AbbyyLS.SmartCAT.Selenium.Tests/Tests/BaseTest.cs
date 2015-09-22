using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests
{
	[TestFixture(typeof(ChromeDriverProvider))]
	public class BaseTest<TWebDriverProvider> : BaseObject where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestFixtureSetUp]
		public void BeforeClass()
		{
			try
			{
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
				Logger.Info("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);

				ThreadUser = TakeUser(ConfigurationManager.Users);

				_loginHelper.Authorize(StartPage, ThreadUser);
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
			try
			{
				if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
				{
					Driver.TakeScreenshot(Path.Combine(PathProvider.ResultsFolderPath, "FailedTests"));
				}
			}
			catch (Exception ex)
			{
				Logger.WarnException("Ошибка при снятии скриншота", ex);
			}

			Driver.SwitchToNewTab();
			ReturnUser(ConfigurationManager.Users, ThreadUser);
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			if (Driver != null)
			{
				Driver.Dispose();
			}
		}

		public TestUser TakeUser(ConcurrentBag<TestUser> users)
		{
			TestUser user = null;

			while (user == null)
			{
				users.TryTake(out user);
			}

			Logger.Info("Пользователь {0} взят из очереди.", user.Login);

			return user;
		}

		public void ReturnUser(ConcurrentBag<TestUser> users, TestUser user)
		{
			try
			{
				users.Add(user);
				Logger.Info("Пользователя {0} возвращен в очередь.", user.Login);
			}
			catch
			{
				Logger.Error("Не удалось вернуть пользователя {0} в очередь.", user.Login);
			}
		}

		public TestUser ThreadUser { get; protected set; }
		protected StartPage StartPage = StartPage.Workspace;
		private readonly LoginHelper _loginHelper = new LoginHelper();
	}
}
