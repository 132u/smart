using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
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
			_clientsPage = new ClientsPage(Driver);
			_translationMemoriesPage = new TranslationMemoriesPage(Driver);
			_newTranslationMemoryDialog = new NewTranslationMemoryDialog(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_newGlossaryDialog = new NewGlossaryDialog(Driver);

			_clientName = _clientsPage.GetClientUniqueName();
		}

		[Test]
		public void CreateClientTest()
		{
			_clientsPage.CreateNewClient(_clientName);

			Assert.IsTrue(_clientsPage.IsSaveButtonDisappear(), "Произошла ошибка: \n кнопка сохранения не исчезла");
			Assert.IsTrue(_clientsPage.IsClientExist(_clientName), "Произошла ошибка: \n клиент не создан");
		}

		[Test]
		public void CreateClientExistingNameTest()
		{
			_clientsPage
				.CreateNewClient(_clientName)
				.CreateNewClient(_clientName);

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
			_clientsPage.CreateNewClient(_clientName);

			_workspaceHelper.GoToTranslationMemoriesPage();

			_translationMemoriesPage
				.ClickCreateNewTmButton()
				.OpenClientsList();

			Assert.IsTrue(_newTranslationMemoryDialog.IsClientExistInList(_clientName),
				"Произошла ошибка:\n клиент {0} не отображен в списке клиентов", _clientName);
		}

		[Test]
		public void CreateClientCheckCreateGlossaryTest()
		{
			_clientsPage.CreateNewClient(_clientName);

			_workspaceHelper.GoToGlossariesPage();

			_glossariesPage
				.ClickCreateGlossaryButton()
				.OpenClientsList();

			Assert.IsTrue(_newGlossaryDialog.IsClientExistInList(_clientName),
				"Произошла ошибка:\n клиент не отображается в списке клиентов при создании глоссария");
		}

		[Test]
		public void ChangeClientNameTest()
		{
			var clientNewName = _clientsPage.GetClientUniqueName();

			_clientsPage
				.CreateNewClient(_clientName)
				.RenameClient(_clientName, clientNewName);

			Assert.IsTrue(_clientsPage.IsClientExist(clientNewName), "Произошла ошибка: \n клиент не создан");
			Assert.IsFalse(_clientsPage.IsClientExist(_clientName), "Произошла ошибка:\n клиент {0} найден в списке клиентов", _clientName);
		}

		[TestCase("")]
		[TestCase(" ")]
		public void ChangeClientInvalidNameTest(string invalidName)
		{
			_clientsPage
				.CreateNewClient(_clientName)
				.RenameClient(_clientName, invalidName);

			Assert.IsTrue(_clientsPage.IsClientEditModeEnabled(),
				"Произошла ошибка:\n произошел выход из режима редактирования клиента.");
		}

		[Test]
		public void ChangeClientExistingNameTest()
		{
			var clientSecondName = _clientsPage.GetClientUniqueName();

			_clientsPage
				.CreateNewClient(_clientName)
				.CreateNewClient(clientSecondName)
				.RenameClient(clientSecondName, _clientName);

			Assert.IsTrue(_clientsPage.IsClientNameValidationErrorDisplayed(),
				"Произошла ошибка:\n не появилась ошибка при создании клиента с некорректным именем");
		}

		[Test]
		public void DeleteClientTest()
		{
			_clientsPage
				.CreateNewClient(_clientName)
				.DeleteClient(_clientName);

			Assert.IsTrue(_clientsPage.IsDeleteButtonDisappear(_clientName),
				"Произошла ошибка:\n кнопка удаления клиента не исчезла после сохранения.");

			Assert.IsFalse(_clientsPage.IsClientExist(_clientName),
				"Произошла ошибка:\n клиент {0} найден в списке клиентов", _clientName);
		}

		[Test]
		public void DeleteClientCheckCreateTM()
		{
			_clientsPage
				.CreateNewClient(_clientName)
				.DeleteClient(_clientName);
			
			_workspaceHelper.GoToTranslationMemoriesPage();

			_translationMemoriesPage
				.ClickCreateNewTmButton()
				.OpenClientsList();

			Assert.IsFalse(_newTranslationMemoryDialog.IsClientExistInList(_clientName),
				"Произошла ошибка:\n клиент {0} отображен в списке клиентов", _clientName);
		}

		[Test]
		public void DeleteClientCheckCreateGlossaryTest()
		{
			_clientsPage
				.CreateNewClient(_clientName)
				.DeleteClient(_clientName);

			_workspaceHelper.GoToGlossariesPage();

			_glossariesPage
				.ClickCreateGlossaryButton()
				.OpenClientsList();

			Assert.IsFalse(_newGlossaryDialog.IsClientExistInList(_clientName),
				"Произошла ошибка:\n клиент отображается в списке клиентов при создании глоссария");
		}

		private ClientsPage _clientsPage;
		private TranslationMemoriesPage _translationMemoriesPage;
		private NewTranslationMemoryDialog _newTranslationMemoryDialog;
		private WorkspaceHelper _workspaceHelper;
		private GlossariesPage _glossariesPage;
		private NewGlossaryDialog _newGlossaryDialog;

		private string _clientName;
	}
}
