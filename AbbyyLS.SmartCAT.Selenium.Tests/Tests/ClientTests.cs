using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests;
using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Clients
{
	[TestFixture]
	class ClientTests : BaseTest
	{
		[SetUp]
		public void SetUpClientTest()
		{
			QuitDriverAfterTest = true;
			_clientsHelper = WorkspaceHelper.GoToClientsPage();
		}

		[Test]
		public void CreateClientTest()
		{
			var clientName = ClientsHelper.GetClientUniqueName();
			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName);
		}

		[Test]
		public void CreateClientExistingNameTest()
		{
			var clientName = ClientsHelper.GetClientUniqueName();
			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.CreateNewClient(clientName)
				.AssertClientNameErrorExist();
		}

		[TestCase("")]
		[TestCase(" ")]
		public void CreateClientInvalidNameTest(string invalidName)
		{
			_clientsHelper
				.CreateNewClient(invalidName)
				.AssertClienEditModeEnabled();
		}

		[Test]
		public void CreateClientCheckCreateTMTest()
		{
			var clientName = ClientsHelper.GetClientUniqueName();
			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.GoToTranslationMemoriesPage()
				.AssertClientExistInClientsList(clientName);
		}

		[Test]
		public void CreateClientCheckCreateGlossaryTest()
		{
			var clientName = ClientsHelper.GetClientUniqueName();
			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.GoToGlossariesPage()
				.AssertClientExistInClientsList(clientName);
		}

		[Test]
		public void ChangeClientNameTest()
		{
			var clientName = ClientsHelper.GetClientUniqueName();
			var clientNewName = ClientsHelper.GetClientUniqueName();
			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.RenameClient(clientName, clientNewName)
				.AssertClientSuccessfullyCreated(clientNewName)
				.AssertClientNotExist(clientName);
		}

		[TestCase("")]
		[TestCase(" ")]
		public void ChangeClientInvalidNameTest(string invalidName)
		{
			var clientName = ClientsHelper.GetClientUniqueName();
			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.RenameClient(clientName, invalidName)
				.AssertClienEditModeEnabled();
		}

		[Test]
		public void ChangeClientExistingNameTest()
		{
			var clientName = ClientsHelper.GetClientUniqueName();
			var clientSecondName = ClientsHelper.GetClientUniqueName();
			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.CreateNewClient(clientSecondName)
				.AssertClientSuccessfullyCreated(clientSecondName)
				.RenameClient(clientSecondName, clientName)
				.AssertClientNameErrorExist();
		}

		[Test]
		public void DeleteClientTest()
		{
			var clientName = ClientsHelper.GetClientUniqueName();
			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.DeleteClient(clientName)
				.AssertClientNotExist(clientName);
		}

		[Test]
		public void DeleteClientCheckCreateTM()
		{
			var clientName = ClientsHelper.GetClientUniqueName();
			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.DeleteClient(clientName)
				.AssertClientNotExist(clientName)
				.GoToTranslationMemoriesPage()
				.AssertClientNotExistInClientsList(clientName);
		}

		[Test]
		public void DeleteClientCheckCreateGlossaryTest()
		{
			var clientName = ClientsHelper.GetClientUniqueName();
			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.DeleteClient(clientName)
				.AssertClientNotExist(clientName)
				.GoToGlossariesPage()
				.AssertClientNotExistInClientsList(clientName);
		}

		private ClientsHelper _clientsHelper;
	}
}
