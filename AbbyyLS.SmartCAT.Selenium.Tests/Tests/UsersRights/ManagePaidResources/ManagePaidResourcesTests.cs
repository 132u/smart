using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManagePaidResources
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManagePaidResourcesTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_userRightsHelper = new UserRightsHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_billingPage = new BillingPage(Driver);
			_loginHelper = new LoginHelper(Driver);

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);
			var groupName = Guid.NewGuid().ToString();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_userRightsHelper.CreateGroupWithoutSpecificRight(
				AdditionalUser.NickName,
				groupName,
				RightsType.PaidResources);
		}
		
		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
				_loginHelper.Authorize(StartPage, AdditionalUser);
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}

		[Test]
		public void PaidResourceTest()
		{
			_workspacePage.GoToBillingPage();

			Assert.IsTrue(_billingPage.IsBillingPageOpened(),
				"Произошла ошибка: не открылась страница управления услугами.");
		}

		private BillingPage _billingPage;
		private WorkspacePage _workspacePage;
		private UserRightsHelper _userRightsHelper;
	}
}
