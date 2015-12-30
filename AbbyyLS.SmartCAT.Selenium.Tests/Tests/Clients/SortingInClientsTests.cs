using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

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
			_workspacePage = new WorkspacePage(Driver);
			_clientsPage = new ClientsPage(Driver);

			_workspacePage.GoToClientsPage();
		}

		[Test]
		public void CheckNoErrorsWhenSortByName()
		{
			_clientsPage.ClickSortByNameAssumingAlert();

			Assert.IsFalse(_clientsPage.IsAlertExist(),  "Произошла ошибка: \n при сортировке появился Alert.");
		}

		private ClientsPage _clientsPage;
		private WorkspacePage _workspacePage;
	}
}
