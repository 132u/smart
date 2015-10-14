using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client
{
	public class ClientsPage : WorkspacePage, IAbstractPage<ClientsPage>
	{
		public ClientsPage(WebDriver driver) : base(driver)
		{
		}

		public new ClientsPage GetPage()
		{
			var clientsPage = new ClientsPage(Driver);
			InitPage(clientsPage, Driver);

			return clientsPage;
		}

		public new void LoadPage()
		{
			if (!IsClientsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница с клиентами.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Прокручиваем страницу(если необходимо) и нажимаем кнопку создания клиента
		/// </summary>
		public ClientsPage ScrollAndClickCreateClientButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку создания клиента.");
			AddClientButton.ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Ввести имя клиента
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public ClientsPage FillClientName(string clientName)
		{
			CustomTestContext.WriteLine("Ввести имя клиента {0}.", clientName);
			ClientNameField.SetText(clientName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Сохранить'
		/// </summary>
		public ClientsPage ClickSaveClientButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Сохранить'");
			SaveClientButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Навести курсор на клиент
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public ClientsPage HoverCursorToClient(string clientName)
		{
			CustomTestContext.WriteLine("Навести курсор на клиент {0}", clientName);
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
			CustomTestContext.WriteLine("Нажать кнопку 'Редактировать' клиента {0}", clientName);
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
			CustomTestContext.WriteLine("Нажать кнопку 'Удалить' клиента {0}", clientName);
			EditClientButton = Driver.SetDynamicValue(How.XPath, DELETE_BUTTON, clientName);
			EditClientButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по имени
		/// </summary>
		public ClientsPage ClickSortByName()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по имени");
			SortByName.Click();

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Создать нового клиента
		/// </summary>
		/// <param name="clientName"></param>
		public ClientsPage CreateNewClient(string clientName)
		{
			ScrollAndClickCreateClientButton();
			FillClientName(clientName);
			ClickSaveClientButton();

			return GetPage();
		}

		/// <summary>
		/// Переименовать клиента
		/// </summary>
		/// <param name="clientName"></param>
		/// <param name="clientNewName"></param>
		public ClientsPage RenameClient(string clientName, string clientNewName)
		{
			HoverCursorToClient(clientName);
			ClickEditClientButton(clientName);
			FillClientName(clientNewName);
			ClickSaveClientButton();

			return GetPage();
		}

		/// <summary>
		/// Удалить клиента
		/// </summary>
		/// <param name="clientName"></param>
		public ClientsPage DeleteClient(string clientName)
		{
			HoverCursorToClient(clientName);
			ClickDeleteClientButton(clientName);

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что кнопка сохранения клиента исчезла
		/// </summary>
		public bool IsSaveButtonDisappear()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка сохранения клиента исчезла");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(SAVE_CLIENT));
		}

		/// <summary>
		/// Проверить, что кнопка удаления клиента исчезла
		/// </summary>
		/// <param name="clientName"></param>
		public bool IsDeleteButtonDisappear(string clientName)
		{
			CustomTestContext.WriteLine("Проверить, что кнопка удаления клиента исчезла");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_BUTTON.Replace("*#*", clientName)));
		}

		/// <summary>
		/// Проверить, что клиент присутствует в списке клиентов
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public bool IsClientExist(string clientName)
		{
			CustomTestContext.WriteLine("Проверить, что клиент {0} присутствует в списке клиентов", clientName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(CLIENT_ROW.Replace("*#*", clientName)));
		}

		/// <summary>
		/// Проверить, что появилась ошибка в имени клиента при создании
		/// </summary>
		public bool IsClientNameValidationErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилась ошибка в имени клиента при создании");

			return Driver.WaitUntilElementIsDisplay(ErrorClientName);
		}

		/// <summary>
		/// Проверить, что мы находимся в режиме редактирования клиента
		/// </summary>
		public bool IsClientEditModeEnabled()
		{
			CustomTestContext.WriteLine("Проверить, что мы находимся в режиме редактирования клиента");

			return Driver.WaitUntilElementIsDisplay(ClientNameField);
		}

		/// <summary>
		/// Проверить, открылась ли страница
		/// </summary>
		public bool IsClientsPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CLIENTS_TABLE_BODY));
		}

		#endregion

		#region Вспомогательные методы

		/// <summary>
		/// Получить уникальное имя клиента
		/// </summary>
		public string GetClientUniqueName()
		{
			return "TestClient-" + Guid.NewGuid();
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = ADD_CLIENT_BUTTON)]
		protected IWebElement AddClientButton { get; set; }

		[FindsBy(How = How.XPath, Using = CLIENT_INPUT_NAME)]
		protected IWebElement ClientNameField { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_CLIENT)]
		protected IWebElement SaveClientButton { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_NAME)]
		protected IWebElement SortByName { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_NAME)]
		protected IWebElement ErrorClientName { get; set; }

		protected IWebElement EditClientButton { get; set; }

		protected IWebElement DeleteClientButton { get; set; }

		protected IWebElement ClientRow { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string CLIENTS_TABLE_BODY = "//table[contains(@class,'js-sortable-table')]";

		protected const string CLIENT_ROW = "//table[contains(@class,'js-sortable-table')]//p[contains(string(), '*#*')]//..";
		protected const string DELETE_BUTTON = "//table[contains(@class,'js-sortable-table')]//p[contains(string(), '*#*')]/../div[@class='l-corpr__clientbtns']/a[@title='Delete']";
		protected const string EDIT_BUTTON = "//table[contains(@class,'js-sortable-table')]//p[contains(string(), '*#*')]/../div[@class='l-corpr__clientbtns']/a[@title='Edit']";

		protected const string ADD_CLIENT_BUTTON = "//div[@data-bind='click: addNewClient']//span";
		protected const string SAVE_CLIENT = "//img[contains(@class,'client save')]";
		protected const string ERROR_NAME = "//div[contains(@class,'clienterr') and string()='A client with the same name already exists.']";
		protected const string CLIENT_INPUT_NAME = "//input[contains(@class,'clienttxtbox')]";
		protected const string CLIENT_LIST = ".//table[contains(@class,'js-sortable-table')]//tr";
		protected const string SORT_BY_NAME = "//th[contains(@data-sort-by,'Name')]//a";

		#endregion
	}
}
