using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
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
			_workspaceHelper.GoToClientsPage();
			_clientsPage = new ClientsPage(Driver).GetPage();
		}

		[Test]
		public void CreateClientTest()
		{
			var clientName = _clientsPage.GetClientUniqueName();

			_clientsPage.CreateNewClient(clientName);

			Assert.IsTrue(_clientsPage.IsSaveButtonDisappear(), "Произошла ошибка: \n кнопка сохранения не исчезла");
			Assert.IsTrue(_clientsPage.IsClientExist(clientName), "Произошла ошибка: \n клиент не создан");
		}

		[Test]
		public void CreateClientExistingNameTest()
		{
			var clientName = _clientsPage.GetClientUniqueName();

			_clientsPage
				.CreateNewClient(clientName)
				.CreateNewClient(clientName);

			Assert.IsTrue(_clientsPage.IsClientNameValidationErrorDisplayed(),
				"Произошла ошибка:\n не появилась ошибка при создании клиента с некорректным именем");
		}

		[TestCase("")]
		[TestCase(" ")]
		public void CreateClientInvalidNameTest(string invalidName)
		{
			_clientsPage.CreateNewClient(invalidName);

			Assert.IsTrue(_clientsPage.IsClientEditModeEnabled(),
				"Произошла ошибка:\n произошел выход из режима редактирования клиента.");
		}

		[Test]
		public void CreateClientCheckCreateTMTest()
		{
			var clientName = _clientsPage.GetClientUniqueName();

			_clientsPage.CreateNewClient(clientName);

			_workspaceHelper
				.GoToTranslationMemoriesPage()
				.AssertClientExistInClientsList(clientName);
		}

		[Test]
		public void CreateClientCheckCreateGlossaryTest()
		{
			var clientName = _clientsPage.GetClientUniqueName();

			_clientsPage.CreateNewClient(clientName);

			_workspaceHelper
				.GoToGlossariesPage()
				.AssertClientExistInClientsList(clientName);
		}

		[Test]
		public void ChangeClientNameTest()
		{
			var clientName = _clientsPage.GetClientUniqueName();
			var clientNewName = _clientsPage.GetClientUniqueName();

			_clientsPage
				.CreateNewClient(clientName)
				.RenameClient(clientName, clientNewName);

			Assert.IsTrue(_clientsPage.IsClientExist(clientNewName), "Произошла ошибка: \n клиент не создан");
			Assert.IsFalse(_clientsPage.IsClientExist(clientName), "Произошла ошибка:\n клиент {0} найден в списке клиентов", clientName);
		}

		[TestCase("")]
		[TestCase(" ")]
		public void ChangeClientInvalidNameTest(string invalidName)
		{
			var clientName = _clientsPage.GetClientUniqueName();

			_clientsPage
				.CreateNewClient(clientName)
				.RenameClient(clientName, invalidName);

			Assert.IsTrue(_clientsPage.IsClientEditModeEnabled(),
				"Произошла ошибка:\n произошел выход из режима редактирования клиента.");
		}

		[Test]
		public void ChangeClientExistingNameTest()
		{
			var clientName = _clientsPage.GetClientUniqueName();
			var clientSecondName = _clientsPage.GetClientUniqueName();

			_clientsPage
				.CreateNewClient(clientName)
				.CreateNewClient(clientSecondName)
				.RenameClient(clientSecondName, clientName);

			Assert.IsTrue(_clientsPage.IsClientNameValidationErrorDisplayed(),
				"Произошла ошибка:\n не появилась ошибка при создании клиента с некорректным именем");
		}

		[Test]
		public void DeleteClientTest()
		{
			var clientName = _clientsPage.GetClientUniqueName();

			_clientsPage
				.CreateNewClient(clientName)
				.DeleteClient(clientName);

			Assert.IsTrue(_clientsPage.IsDeleteButtonDisappear(clientName),
				"Произошла ошибка:\n кнопка удаления клиента не исчезла после сохранения.");

			Assert.IsFalse(_clientsPage.IsClientExist(clientName),
				"Произошла ошибка:\n клиент {0} найден в списке клиентов", clientName);
		}

		[Test]
		public void DeleteClientCheckCreateTM()
		{
			var clientName = _clientsPage.GetClientUniqueName();

			_clientsPage
				.CreateNewClient(clientName)
				.DeleteClient(clientName);
			
			_workspaceHelper
				.GoToTranslationMemoriesPage()
				.AssertClientNotExistInClientsList(clientName);
		}

		[Test]
		public void DeleteClientCheckCreateGlossaryTest()
		{
			var clientName = _clientsPage.GetClientUniqueName();

			_clientsPage
				.CreateNewClient(clientName)
				.DeleteClient(clientName);

			_workspaceHelper
				.GoToGlossariesPage()
				.AssertClientNotExistInClientsList(clientName);
		}

		private ClientsPage _clientsPage;
		private WorkspaceHelper _workspaceHelper;
	}
}
