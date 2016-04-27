using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	public class BaseAdminTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public BaseAdminTest()
		{
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void SetUpInitialDataTests()
		{
			_adminHelper = new AdminHelper(Driver);
			_adminCreateAccountPage = new AdminCreateAccountPage(Driver);
			_vendorsManagementPage = new VendorsManagementPage(Driver);
		}

		protected AdminHelper _adminHelper;
		protected AdminCreateAccountPage _adminCreateAccountPage;
		protected VendorsManagementPage _vendorsManagementPage;
	}
}