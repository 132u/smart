﻿using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Clients
{
	internal class SortingInClientsTests<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[Test]
		public void SortByNameTest()
		{
			_clientsHelper
				.GoToClientsPage()
				.ClickSortByName()
				.AssertAlertNoExist();
		}

		private ClientsHelper _clientsHelper = new ClientsHelper();
	}
}