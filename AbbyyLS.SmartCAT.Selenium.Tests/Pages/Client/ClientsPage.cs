using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client
{
	public class ClientsPage : WorkspacePage, IAbstractPage<ClientsPage>
	{
		public new ClientsPage GetPage()
		{
			var clientsPage = new ClientsPage();
			InitPage(clientsPage);
			LoadPage();

			return clientsPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.XPath(CLIENTS_TABLE_BODY)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница с клиентами.");
			}
		}

		/// <summary>
		/// Прокручиваем страницу(если необходимо) и нажимаем кнопку создания клиента
		/// </summary>
		public ClientsPage ScrollAndClickCreateClientButton()
		{
			Logger.Debug("Нажать кнопку создания клиента.");
			AddClientButton.ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Ввести имя клиента
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public ClientsPage FillClientName(string clientName)
		{
			Logger.Debug("Ввести имя клиента {0}.", clientName);
			ClientNameField.SetText(clientName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Сохранить'
		/// </summary>
		public ClientsPage ClickSaveClientButton()
		{
			Logger.Debug("Нажать кнопку 'Сохранить'");
			SaveClientButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Навести курсор на клиент
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public ClientsPage HoverCursorToClient(string clientName)
		{
			Logger.Debug("Навести курсор на клиент {0}", clientName);
			ClientRow = Driver.SetDynamicValue(How.XPath, CLIENT_ROW, clientName);
			ClientRow.HoverElement();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Редактировать' клиента
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public ClientsPage ClickEditClientButton(string clientName)
		{
			Logger.Debug("Нажать кнопку 'Редактировать' клиента {0}", clientName);
			EditClientButton = Driver.SetDynamicValue(How.XPath, EDIT_BUTTON, clientName);
			EditClientButton.Click();
			
			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Удалить' клиента
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public ClientsPage ClickDeleteClientButton(string clientName)
		{
			Logger.Debug("Нажать кнопку 'Удалить' клиента {0}", clientName);
			EditClientButton = Driver.SetDynamicValue(How.XPath, DELETE_BUTTON, clientName);
			EditClientButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка сохранения клиента исчезла
		/// </summary>
		public ClientsPage AssertSaveButtonDisappear()
		{
			Logger.Trace("Проверить, что кнопка сохранения клиента исчезла");

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(SAVE_CLIENT)),
				"Произошла ошибка:\n кнопка сохранения клиента не исчезла после сохранения.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка удаления клиента исчезла
		/// </summary>
		public ClientsPage AssertDeleteButtonDisappear()
		{
			Logger.Trace("Проверить, что кнопка удаления клиента исчезла");

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(DELETE_BUTTON.Replace("#", string.Empty))),
				"Произошла ошибка:\n кнопка удаления клиента не исчезла после сохранения.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что клиент присутствует в списке клиентов
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public ClientsPage AssertClientExist(string clientName)
		{
			Logger.Trace("Проверить, что клиент {0} присутствует в списке клиентов", clientName);

			Assert.IsTrue(Driver.WaitUntilElementIsPresent(By.XPath(getClientPath(clientName))),
				"Произошла ошибка:\n клиент {0} не найден в списке клиентов", clientName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что клиент отсутствует в списке клиентов
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public ClientsPage AssertClientNotExist(string clientName)
		{
			Logger.Trace("Проверить, что клиент {0} отсутствует в списке клиентов", clientName);

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(getClientPath(clientName))),
				"Произошла ошибка:\n клиент {0} найден в списке клиентов", clientName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилась ошибка в имени клиента при создании
		/// </summary>
		public ClientsPage AssertClientNameErrorExist()
		{
			Logger.Trace("Проверить, что появилась ошибка в имени клиента при создании");

			Assert.IsTrue(ErrorClientName.Displayed,
				"Произошла ошибка:\n не появилась ошибка при создании клиента с некорректным именем");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что мы находимся в режиме редактирования клиента
		/// </summary>
		public ClientsPage AssertClientEditModeEnabled()
		{
			Logger.Trace("Проверить, что мы находимся в режиме редактирования клиента");

			Assert.IsTrue(ClientNameField.Displayed,
				"Произошла ошибка:\n произошел выход из режима редактирования клиента.");

			return GetPage();
		}
		
		/// <summary>
		/// Получить xPath клиента
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		private static string getClientPath(string clientName)
		{
			Logger.Trace("Получить xPath клиента {0} ", clientName);

			return CLIENT_ROW.Replace("*#*", clientName);
		}

		[FindsBy(How = How.XPath, Using = ADD_CLIENT_BUTTON)]
		protected IWebElement AddClientButton { get; set; }

		[FindsBy(How = How.XPath, Using = CLIENT_INPUT_NAME)]
		protected IWebElement ClientNameField { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_CLIENT)]
		protected IWebElement SaveClientButton { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_NAME)]
		protected IWebElement ErrorClientName { get; set; }

		protected IWebElement EditClientButton { get; set; }

		protected IWebElement DeleteClientButton { get; set; }

		protected IWebElement ClientRow { get; set; }

		protected const string CLIENTS_TABLE_BODY = "//table[contains(@class,'js-sortable-table')]";

		protected const string CLIENT_ROW = "//table[contains(@class,'js-sortable-table')]//p[contains(string(), '*#*')]//..";
		protected const string DELETE_BUTTON = "//table[contains(@class,'js-sortable-table')]//p[contains(string(), '*#*')]//..//img[contains(@class,'delete client')]";
		protected const string EDIT_BUTTON = "//table[contains(@class,'js-sortable-table')]//p[contains(string(), '*#*')]//..//img[contains(@class,'edit client')]";

		protected const string ADD_CLIENT_BUTTON = "//div[@data-bind='click: addNewClient']//span";
		protected const string SAVE_CLIENT = "//img[contains(@class,'client save')]";
		protected const string ERROR_NAME = "//div[contains(@class,'clienterr') and string()='A client with the same name already exists.']";
		protected const string CLIENT_INPUT_NAME = "//input[contains(@class,'clienttxtbox')]";
		protected const string CLIENT_LIST = ".//table[contains(@class,'js-sortable-table')]//tr";
	}
}
