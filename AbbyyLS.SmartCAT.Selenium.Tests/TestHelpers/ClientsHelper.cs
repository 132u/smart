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
				.ClickDeleteClientButton(clientName)
				.AssertDeleteButtonDisapear();

			return this;
		}

		public ClientsHelper RenameClient(string clientName, string clientNewName)
		{
			BaseObject.InitPage(_clientsPage);
			_clientsPage
				.HoverCursorToClient(clientName)
				.ClickEditClientButton(clientName)
				.FillClientName(clientNewName, clearFirst: true)
				.ClickSaveClientButton();

			return this;
		}

		public ClientsHelper AssertClientSuccessfullyCreated(string clientName)
		{
			BaseObject.InitPage(_clientsPage);
			_clientsPage
				.AssertSaveButtonDisapear()
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
			_clientsPage.AssertClienEditModeEnabled();

			return this;
		}


		public static string GetClientUniqueName()
		{
			// Sleep вставлен для того, чтобы избежать вероятность генерации одинаковых имен
			// Такая ситуация возможна, если имена генерируются друг за другом
			Thread.Sleep(10);

			return "TestClient" + DateTime.UtcNow.Ticks;
		}

		private readonly ClientsPage _clientsPage = new ClientsPage();
	}
}
