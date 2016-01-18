using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Clients
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	[Clients]
	class CreateClientTests<TWebDriverProvider> : BaseClientsTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
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

			_workspacePage.GoToTranslationMemoriesPage();

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

			_workspacePage.GoToGlossariesPage();

			_glossariesPage
				.ClickCreateGlossaryButton()
				.OpenClientsList();

			Assert.IsTrue(_newGlossaryDialog.IsClientExistInList(_clientName),
				"Произошла ошибка:\n клиент не отображается в списке клиентов при создании глоссария");
		}
	}
}
