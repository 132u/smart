using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Clients
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	[Clients]
	class DeleteClientTests<TWebDriverProvider> : BaseClientsTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
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

			_workspacePage.GoToTranslationMemoriesPage();

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

			_workspacePage.GoToGlossariesPage();

			_glossariesPage
				.ClickCreateGlossaryButton()
				.ClickClientsList();

			Assert.IsFalse(_newGlossaryDialog.IsClientExistInList(_clientName),
				"Произошла ошибка:\n клиент отображается в списке клиентов при создании глоссария");
		}
	}
}
