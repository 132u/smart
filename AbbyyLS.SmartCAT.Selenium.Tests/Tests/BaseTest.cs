using System;
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
				_loginHelper.Authorize(StartPage);
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
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			if (Driver != null)
			{
				Driver.Dispose();
			}
		}

		protected StartPage StartPage = StartPage.Workspace;
		private readonly LoginHelper _loginHelper = new LoginHelper();
	}
}
