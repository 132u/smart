using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Clients
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	[Clients]
	class ClientsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpClientTest()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);
			_clientsHelper = _workspaceHelper.GoToClientsPage();
		}

		[Test]
		public void CreateClientTest()
		{
			var clientName = _clientsHelper.GetClientUniqueName();

			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName);
		}

		[Test]
		public void CreateClientExistingNameTest()
		{
			var clientName = _clientsHelper.GetClientUniqueName();

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
			var clientName = _clientsHelper.GetClientUniqueName();

			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.GoToTranslationMemoriesPage()
				.AssertClientExistInClientsList(clientName);
		}

		[Test]
		public void CreateClientCheckCreateGlossaryTest()
		{
			var clientName = _clientsHelper.GetClientUniqueName();

			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.GoToGlossariesPage()
				.AssertClientExistInClientsList(clientName);
		}

		[Test]
		public void ChangeClientNameTest()
		{
			var clientName = _clientsHelper.GetClientUniqueName();
			var clientNewName = _clientsHelper.GetClientUniqueName();

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
			var clientName = _clientsHelper.GetClientUniqueName();

			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.RenameClient(clientName, invalidName)
				.AssertClienEditModeEnabled();
		}

		[Test]
		public void ChangeClientExistingNameTest()
		{
			var clientName = _clientsHelper.GetClientUniqueName();
			var clientSecondName = _clientsHelper.GetClientUniqueName();

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
			var clientName = _clientsHelper.GetClientUniqueName();

			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.DeleteClient(clientName)
				.AssertClientNotExist(clientName);
		}

		[Test]
		public void DeleteClientCheckCreateTM()
		{
			var clientName = _clientsHelper.GetClientUniqueName();

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
			var clientName = _clientsHelper.GetClientUniqueName();

			_clientsHelper
				.CreateNewClient(clientName)
				.AssertClientSuccessfullyCreated(clientName)
				.DeleteClient(clientName)
				.AssertClientNotExist(clientName)
				.GoToGlossariesPage()
				.AssertClientNotExistInClientsList(clientName);
		}

		private ClientsHelper _clientsHelper;
		private WorkspaceHelper _workspaceHelper;
	}
}
