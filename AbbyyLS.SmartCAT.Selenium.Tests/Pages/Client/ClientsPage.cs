using System;
using System.Reflection;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Internal;
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
			if (!Driver.WaitUntilElementIsPresent(By.XPath(CLIENTS_TABLE_BODY_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница с клиентами.");
			}
		}

		/// <summary>
		/// Прокручиваем страницу(если необходимо) и нажтмаем кнопку создания клиента
		/// </summary>
		public ClientsPage ScrollAndClickCreateClientButton()
		{
			Logger.Debug("Нажтмаем кнопку создания клиента");

			Driver.ScrollAndClick(AddClientButton);
			
			return GetPage();
		}

		/// <summary>
		/// Ввести имя клиента
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		/// <param name="clearFirst">предварительно отчистить поле</param>
		public ClientsPage FillClientName(string clientName, bool clearFirst = false)
		{
			Logger.Debug("Ввести имя клиента {0}. Предварительно отчистить поле: {1}.", clientName, clearFirst);
			ClientNameField.SetText(clientName, clearFirst);

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
			ClientRow = Driver.SetDynamicValue(How.XPath, CLIENT_ROW_XPATH, clientName);
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
			EditClientButton = Driver.SetDynamicValue(How.XPath, EDIT_BTN_XPATH, clientName);
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
			EditClientButton = Driver.SetDynamicValue(How.XPath, DELETE_BTN_XPATH, clientName);
			EditClientButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверяем, что кнопка сохранения клиента исчезла
		/// </summary>
		public ClientsPage AssertSaveButtonDisapear()
		{
			Logger.Trace("Проверяем, что кнопка сохранения клиента исчезла");

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(SAVE_CLIENT_XPATH)),
				"Произошла ошибка:\n кнопка сохранения клиента не исчезла после сохранения.");

			return GetPage();
		}

		/// <summary>
		/// Проверяем, что кнопка сохранения клиента исчезла
		/// </summary>
		public ClientsPage AssertDeleteButtonDisapear()
		{
			Logger.Trace("Проверяем, что кнопка сохранения клиента исчезла");

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(DELETE_BTN_XPATH.Replace("#", string.Empty))),
				"Произошла ошибка:\n кнопка сохранения клиента не исчезла после сохранения.");

			return GetPage();
		}

		/// <summary>
		/// Проверяем, что клиент присутствует в списке клиентов
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public ClientsPage AssertClientExist(string clientName)
		{
			Logger.Trace("Проверяем, что клиент {0} присутствует в списке клиентов", clientName);

			Assert.IsTrue(getIsClientExist(clientName),
				"Произошла ошибка:\n клиент {0} не найден в списке клиентов", clientName);

			return GetPage();
		}

		/// <summary>
		/// Проверяем, что клиент отсутствует в списке клиентов
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public ClientsPage AssertClientNotExist(string clientName)
		{
			Logger.Trace("Проверяем, что клиент {0} отсутствует в списке клиентов", clientName);

			Assert.IsFalse(getIsClientExist(clientName),
				"Произошла ошибка:\n клиент {0} найден в списке клиентов", clientName);

			return GetPage();
		}

		/// <summary>
		/// Проверяем, что появилась ошибка в имени клиента при создании
		/// </summary>
		public ClientsPage AssertClientNameErrorExist()
		{
			Logger.Trace("Проверяем, что появилась ошибка в имени клиента при создании");

			Assert.IsTrue(ErrorClientName.Displayed,
				"Произошла ошибка:\n не появилась ошибка при создании клиента с некорректным именем");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что мы находимся в режиме редактирования
		/// </summary>
		public ClientsPage AssertClienEditModeEnabled()
		{
			Logger.Trace("Проверить, что мы находимся в режиме редактирования");

			Assert.IsTrue(ClientNameField.Displayed,
				"Произошла ошибка:\n произошел выход из режима редактирования.");

			return GetPage();
		}
		
		/// <summary>
		/// Полчить bool значение о наличии клиента
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		private static bool getIsClientExist(string clientName)
		{
			Logger.Trace("Полчить bool значение о наличии клиента {0} ", clientName);
			
			return Driver.ElementIsPresent(By.XPath(CLIENT_ROW_XPATH.Replace("*#*", clientName)));
		}

		[FindsBy(How = How.XPath, Using = ADD_CLIENT_BTN)]
		protected IWebElement AddClientButton { get; set; }

		[FindsBy(How = How.XPath, Using = CLIENT_INPUT_NAME_XPATH)]
		protected IWebElement ClientNameField { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_CLIENT_XPATH)]
		protected IWebElement SaveClientButton { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_NAME_XPATH)]
		protected IWebElement ErrorClientName { get; set; }

		protected IWebElement EditClientButton { get; set; }

		protected IWebElement DeleteClientButton { get; set; }

		protected IWebElement ClientRow { get; set; }

		protected const string CLIENTS_TABLE_BODY_XPATH = "//table[contains(@class,'js-sortable-table')]";

		protected const string CLIENT_ROW_XPATH = "//table[contains(@class,'js-sortable-table')]//p[contains(string(), '*#*')]//..";
		protected const string DELETE_BTN_XPATH = "//table[contains(@class,'js-sortable-table')]//p[contains(string(), '*#*')]//..//img[contains(@class,'delete client')]";
		protected const string EDIT_BTN_XPATH = "//table[contains(@class,'js-sortable-table')]//p[contains(string(), '*#*')]//..//img[contains(@class,'edit client')]";

		protected const string ADD_CLIENT_BTN = "//div[@data-bind='click: addNewClient']//span";
		protected const string SAVE_CLIENT_XPATH = "//img[contains(@class,'client save')]";
		protected const string ERROR_NAME_XPATH = "//div[contains(@class,'clienterr') and string()='A client with the same name already exists.']";
		protected const string CLIENT_INPUT_NAME_XPATH = "//input[contains(@class,'clienttxtbox')]";
		protected const string CLIENT_LIST_XPATH = ".//table[contains(@class,'js-sortable-table')]//tr";
	}
}
