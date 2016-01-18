using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Clients
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	[Clients]
	class ChangeClientTests<TWebDriverProvider> : BaseClientsTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
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
	}
}
