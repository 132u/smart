using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер вкладки Clients
	/// </summary>
	public class ClientPageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public ClientPageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
		}

		/// <summary>
		/// Дождаться загрузки страницы
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(ADD_CLIENT_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть: есть ли такой клиент
		/// </summary>
		/// <param name="clientName">имя</param>
		/// <returns>есть</returns>
		public bool GetIsClientExist(string clientName)
		{
			return GetIsElementDisplay(By.XPath(GetClientRowXPath(clientName)));
		}

		/// <summary>
		/// Кликнуть Создать клиента
		/// </summary>
		public void ClickCreateClientBtn()
		{
			ClickElement(By.XPath(ADD_CLIENT_BTN_XPATH));
		}

		/// <summary>
		/// Проявить кнопку Edit и кликнуть
		/// </summary>
		/// <param name="clientName">название</param>
		public void ClickEdit(string clientName)
		{
			// Найти номер нужной строки
			var clientXPath = GetClientRowXPath(clientName);
			// Навести курсор мыши на строку
			HoverElement(By.XPath(clientXPath));
			// Кликнуть Edit
			ClickElement(By.XPath(clientXPath + EDIT_BTN_XPATH));
		}

		/// <summary>
		/// Ввести новое имя
		/// </summary>
		/// <param name="newName">новое имя</param>
		public void EnterNewName(string newName)
		{
			ClearAndAddText(By.XPath(CLIENT_INPUT_XPATH), newName);
		}

		/// <summary>
		/// Проявить кнопку Delete и кликнуть
		/// </summary>
		/// <param name="clientName">название</param>
		public bool DeleteClient(string clientName)
		{
			// Найти номер нужной строки
			var clientXPath = GetClientRowXPath(clientName);
			// Навести курсор мыши на строку
			HoverElement(By.XPath(clientXPath));
			// Кликнуть Delete
			ClickElement(By.XPath(clientXPath + DELETE_BTN_XPATH));
			// Проверить, что клиент убран из списка клиентов
			return WaitUntilDisappearElement(By.XPath(clientXPath + DELETE_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Save
		/// </summary>
		public void ClickSaveBtn()
		{
			ClickElement(By.XPath(SAVE_CLIENT_XPATH));
		}

		/// <summary>
		/// Дождаться, пока пропадет кнопка Сохранить
		/// </summary>
		/// <returns>пропала</returns>
		public bool WaitSaveBtnDisappear()
		{
			return WaitUntilDisappearElement(By.XPath(SAVE_CLIENT_XPATH));
		}

		/// <summary>
		/// Вернуть, остался ли режим редактирования
		/// </summary>
		/// <returns>режим редактирования</returns>
		public bool GetIsEditMode()
		{
			return GetIsElementDisplay(By.XPath(CLIENT_INPUT_XPATH));
		}

		/// <summary>
		/// Вернуть, не сохранился новый клиент
		/// </summary>
		/// <returns>режим нового клиента (не сохранился)</returns>
		public bool GetIsNewClientEditMode()
		{
			return GetIsElementDisplay(By.XPath(CLIENT_INPUT_XPATH));			
		}

		/// <summary>
		/// Вернуть, отображается ли строка с ошибкой
		/// </summary>
		/// <returns>отображается</returns>
		public bool GetIsNameErrorExist()
		{
			return WaitUntilDisplayElement(By.XPath(ERROR_NAME_XPATH));
		}

		/// <summary>
		/// Ввести имя нового клиента
		/// </summary>
		/// <param name="name"></param>
		public void EnterNewClientName(string name)
		{
			SendTextElement(By.XPath(CLIENT_INPUT_XPATH), name);
		}

		/// <summary>
		/// Вернуть xPath строки с клиентом
		/// </summary>
		/// <param name="clientName">название</param>
		/// <returns>XPath</returns>
		protected string GetClientRowXPath(string clientName)
		{
			var rowNum = 0;
			IList<IWebElement> clientList = GetElementList(By.XPath(CLIENT_LIST_XPATH));
			for (int i = 0; i < clientList.Count; ++i )
			{
				if (clientList[i].Text.Contains(clientName))
				{
					Logger.Trace(clientList[i].Text);
					Logger.Trace(i);
					rowNum = i + 1;
					break;
				}
			}
			return CLIENT_LIST_XPATH + "[" + rowNum + "]";
		}



		protected const string DELETE_BTN_XPATH = "//img[contains(@class,'delete client')]";
		protected const string EDIT_BTN_XPATH = "//img[contains(@class,'edit client')]";
		protected const string ADD_CLIENT_BTN_XPATH = "//div[contains(@class,'js-clients')]/div/span";
		protected const string SAVE_CLIENT_XPATH = "//img[contains(@class,'client save')]";

		protected const string ERROR_NAME_XPATH = "//div[contains(@class,'clienterr') and text()='A client with the same name already exists.']";

		protected const string CLIENT_INPUT_XPATH = "//input[contains(@class,'clienttxtbox')]";

		protected const string CLIENT_LIST_XPATH = ".//table[contains(@class,'js-sortable-table')]//tr";
	}
}