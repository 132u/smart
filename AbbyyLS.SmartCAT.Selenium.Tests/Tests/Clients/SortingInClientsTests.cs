using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
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
			_clientsHelper = new ClientsHelper(Driver);
		}

		[Test]
		public void SortByNameTest()
		{
			_clientsHelper
				.GoToClientsPage()
				.ClickSortByName()
				.AssertAlertNoExist();
		}

		private ClientsHelper _clientsHelper;
	}
}
