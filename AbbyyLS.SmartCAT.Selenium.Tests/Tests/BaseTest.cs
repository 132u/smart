using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests
{
	[TestFixture(typeof(ChromeDriverProvider))]
	public class BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public WebDriver Driver { get; private set; }

		[OneTimeSetUp]
		public void BeforeClass()
		{
			try
			{
				ThreadUser = TakeUser(ConfigurationManager.Users);
				Driver = new WebDriver(new TWebDriverProvider(), Path.Combine(PathProvider.DriversTemporaryFolder, Guid.NewGuid().ToString()), PathProvider.ExportFiles, PathProvider.ImportFiles);
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в TestFixtureSetUp {0}", ex.ToString());
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
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}

		[TearDown]
		public void TeardownBase()
		{
			Driver.CloseAlertIfExist();

			TakeScreenshotIfTestFailed();

			CustomTestContext.WriteLine("Окончание работы теста {0}", TestContext.CurrentContext.Test.Name);

			Driver.SwitchToNewTab();
		}

		[OneTimeTearDown]
		public void TestFixtureTearDown()
		{
			ReturnUser(ConfigurationManager.Users, ThreadUser);

			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}

			if (Driver != null)
			{
				Driver.Dispose();
			}

			if (CourseraReviewerUser != null)
			{
				ReturnUser(ConfigurationManager.CourseraReviewerUsers, CourseraReviewerUser);
			}

			if (CourseraCrowdsourceUser != null)
			{
				ReturnUser(ConfigurationManager.CourseraCrowdsourceUsers, CourseraCrowdsourceUser);
			}
		}

		public TestUser TakeUser(ConcurrentBag<TestUser> users)
		{
			TestUser user = null;
			var timer = 1;

			while (!users.TryTake(out user) && timer <= 300)
			{
				Thread.Sleep(1000);
				timer++;
			}

			if (timer > 300)
			{
				throw new Exception("Произошла ошибка:\n нет пользователей в очереди");
			}

			CustomTestContext.WriteLine("Пользователь {0} взят из очереди после {1} попытки.", user.Login, timer);
			
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

		public void TakeScreenshotIfTestFailed()
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
				CustomTestContext.WriteLine("Ошибка при снятии скриншота {0}", ex.ToString());
			}
		}

		public void ReplaceDrivers(WebDriver wd)
		{
			//метод используется, когда в тесте создаётся новый экземпляр WebDriver
			//необходим для корректного завершения работы тестов.
			Driver = wd;
		}

		public TestUser ThreadUser { get; protected set; }
		public TestUser AdditionalUser { get; protected set; }
		public TestUser CourseraReviewerUser { get; protected set; }
		public TestUser CourseraCrowdsourceUser { get; protected set; }
		protected StartPage StartPage = StartPage.Workspace;
		protected LoginHelper _loginHelper;
	}
}
