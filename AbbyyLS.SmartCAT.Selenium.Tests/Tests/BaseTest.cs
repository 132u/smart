﻿using System;
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
	public class BaseTest<TWebDriverProvider> : BaseObject where TWebDriverProvider : IWebDriverProvider, new()
	{
		public WebDriver Driver { get; private set; }

		[OneTimeSetUp]
		public void BeforeClass()
		{
			try
			{
				ThreadUser = TakeUser(ConfigurationManager.Users);
				Driver = new WebDriver(new TWebDriverProvider(), PathProvider.DriversTemporaryFolder, PathProvider.ExportFiles);
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

		public TestUser ThreadUser { get; protected set; }
		public TestUser AdditionalUser { get; protected set; }
		public TestUser CourseraUser { get; protected set; }
		protected StartPage StartPage = StartPage.Workspace;
		protected LoginHelper _loginHelper;
	}
}
