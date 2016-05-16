using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageСlientsTests<TWebDriverProvider> : ManageСlientsAndPprojectGroupsBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void BeforeTest()
		{
			_clientName = _clientsPage.GetClientUniqueName();

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName);
		}

		[Test]
		public void CreateClientTest()
		{
			Assert.IsTrue(_clientsPage.IsSaveButtonDisappear(), "Произошла ошибка: \n кнопка сохранения не исчезла");
			Assert.IsTrue(_clientsPage.IsClientExist(_clientName), "Произошла ошибка: \n клиент не создан");
		}

		[Test]
		public void ChangeClientNameTest()
		{
			var clientNewName = _clientsPage.GetClientUniqueName();

			_clientsPage.RenameClient(_clientName, clientNewName);

			Assert.IsTrue(_clientsPage.IsClientExist(clientNewName), "Произошла ошибка: \n клиент не создан");
			Assert.IsFalse(_clientsPage.IsClientExist(_clientName), "Произошла ошибка:\n клиент {0} найден в списке клиентов", _clientName);
		}

		[Test]
		public void DeleteClientTest()
		{
			_clientsPage.DeleteClient(_clientName);

			Assert.IsTrue(_clientsPage.IsDeleteButtonDisappear(_clientName),
				"Произошла ошибка:\n кнопка удаления клиента не исчезла после сохранения.");

			Assert.IsFalse(_clientsPage.IsClientExist(_clientName),
				"Произошла ошибка:\n клиент {0} найден в списке клиентов", _clientName);
		}
	}
}
