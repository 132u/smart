using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Clients
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Clients]
	internal class SortingInClientsTests<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void Initialization()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);
			_workspaceHelper.GoToClientsPage();
			_clientsPage = new ClientsPage(Driver).GetPage();
		}

		[Test]
		public void CheckNoErrorsWhenSortByName()
		{
			_clientsPage
				.ClickSortByName()
				.AssertAlertNoExist();
		}

		private ClientsPage _clientsPage;
		private WorkspaceHelper _workspaceHelper;
	}
}
