using System;
using System.Threading;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class ClientsHelper : WorkspaceHelper
	{
		public ClientsHelper CreateNewClient(string clientName)
		{
			BaseObject.InitPage(_clientsPage);
			_clientsPage
				.ScrollAndClickCreateClientButton()
				.FillClientName(clientName)
				.ClickSaveClientButton();

			return this;
		}

		public ClientsHelper DeleteClient(string clientName)
		{
			BaseObject.InitPage(_clientsPage);
			_clientsPage
				.HoverCursorToClient(clientName)
				.AssertDeleteButtonAppear(clientName)
				.ClickDeleteClientButton(clientName)
				.AssertDeleteButtonDisappear(clientName);

			return this;
		}

		public ClientsHelper RenameClient(string clientName, string clientNewName)
		{
			BaseObject.InitPage(_clientsPage);
			_clientsPage
				.HoverCursorToClient(clientName)
				.AssertEditButtonAppear(clientName)
				.ClickEditClientButton(clientName)
				.FillClientName(clientNewName)
				.ClickSaveClientButton();

			return this;
		}

		public ClientsHelper AssertClientSuccessfullyCreated(string clientName)
		{
			BaseObject.InitPage(_clientsPage);
			_clientsPage
				.AssertSaveButtonDisappear()
				.AssertClientExist(clientName);

			return this;
		}

		public ClientsHelper AssertClientNotExist(string clientName)
		{
			BaseObject.InitPage(_clientsPage);
			_clientsPage.AssertClientNotExist(clientName);

			return this;
		}

		public ClientsHelper AssertClientNameErrorExist()
		{
			BaseObject.InitPage(_clientsPage);
			_clientsPage.AssertClientNameErrorExist();

			return this;
		}

		public ClientsHelper AssertClienEditModeEnabled()
		{
			BaseObject.InitPage(_clientsPage);
			_clientsPage.AssertClientEditModeEnabled();

			return this;
		}

		public string GetClientUniqueName()
		{
			return "TestClient-" + Guid.NewGuid();
		}

		public ClientsHelper ClickSortByName()
		{
			BaseObject.InitPage(_clientsPage);
			_clientsPage.ClickSortByName();

			return this;
		}

		private readonly ClientsPage _clientsPage = new ClientsPage();
	}
}
