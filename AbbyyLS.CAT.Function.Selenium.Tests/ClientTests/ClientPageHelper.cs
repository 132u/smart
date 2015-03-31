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
			Logger.Trace("Ожидание открытия страницы Clients");
			return WaitUntilDisplayElement(By.XPath(ADD_CLIENT_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть: есть ли такой клиент
		/// </summary>
		/// <param name="clientName">имя</param>
		/// <returns>есть</returns>
		public bool GetIsClientExist(string clientName)
		{
			Logger.Trace("Проверка, есть ли клиент " + clientName);
			return GetIsElementDisplay(By.XPath(GetClientRowXPath(clientName)));
		}

		/// <summary>
		/// Кликнуть Создать клиента
		/// </summary>
		public void ClickCreateClientBtn()
		{
			Logger.Trace("Клик по кнопке Create Client");
			ClickElement(By.XPath(ADD_CLIENT_BTN_XPATH));
		}

		/// <summary>
		/// Проявить кнопку Edit и кликнуть
		/// </summary>
		/// <param name="clientName">название</param>
		public void HoverAndClickEdit(string xpathClientRow)
		{
			Logger.Trace("Поместить курсор мыши на нужной строке");
			HoverElement(By.XPath(xpathClientRow));
			Logger.Trace("Клик по кнопке Edit клиента ");
			ClickElement(By.XPath(xpathClientRow + EDIT_BTN_XPATH));
		}

		/// <summary>
		/// Ввести новое имя
		/// </summary>
		/// <param name="newName">новое имя</param>
		public void EnterNewName(string newName)
		{
			Logger.Trace("Ввод имени " + newName);
			ClearAndAddText(By.XPath(CLIENT_INPUT_XPATH), newName);
		}

		/// <summary>
		/// Проявить кнопку Delete и кликнуть
		/// </summary>
		/// <param name="clientName">название</param>
		public bool DeleteClient(string clientName)
		{
			Logger.Trace("Получение xPath клиента " + clientName);
			var clientXPath = GetClientRowXPath(clientName);
			Logger.Trace("Поместить курсор мыши на нужной строке");
			HoverElement(By.XPath(clientXPath));
			Logger.Trace("Клик по кнопке Delete");
			ClickElement(By.XPath(clientXPath + DELETE_BTN_XPATH));
			Logger.Trace("Ожидание удаления строки");
			return WaitUntilDisappearElement(By.XPath(clientXPath + DELETE_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Save
		/// </summary>
		public void ClickSaveBtn()
		{
			Logger.Trace("Кликнуть Save");
			ClickElement(By.XPath(SAVE_CLIENT_XPATH));
		}

		/// <summary>
		/// Дождаться, пока пропадет кнопка Сохранить
		/// </summary>
		/// <returns>пропала</returns>
		public bool WaitSaveBtnDisappear()
		{
			Logger.Trace("Дождаться, пока пропадет кнопка Save");
			return WaitUntilDisappearElement(By.XPath(SAVE_CLIENT_XPATH));
		}

		/// <summary>
		/// Вернуть, остался ли режим редактирования
		/// </summary>
		/// <returns>режим редактирования</returns>
		public bool GetIsEditMode()
		{
			Logger.Trace("Проверить, остался ли режим редактирования");
			return GetIsElementDisplay(By.XPath(CLIENT_INPUT_XPATH));
		}

		/// <summary>
		/// Вернуть, не сохранился новый клиент
		/// </summary>
		/// <returns>режим нового клиента (не сохранился)</returns>
		public bool GetIsNewClientEditMode()
		{
			Logger.Trace("Проверить, закончилось ли редактирование клиента");
			return GetIsElementDisplay(By.XPath(CLIENT_INPUT_XPATH));			
		}

		/// <summary>
		/// Вернуть, отображается ли строка с ошибкой
		/// </summary>
		/// <returns>отображается</returns>
		public bool GetIsNameErrorExist()
		{
			Logger.Trace("Проверка, отображается ли сообщение 'A client with the same name already exists.'");
			return WaitUntilDisplayElement(By.XPath(ERROR_NAME_XPATH));
		}

		/// <summary>
		/// Ввести имя нового клиента
		/// </summary>
		/// <param name="name"></param>
		public void EnterNewClientName(string name)
		{
			Logger.Trace("Ввод имени при создании клиента");
			SendTextElement(By.XPath(CLIENT_INPUT_XPATH), name);
		}

		/// <summary>
		/// Вернуть xPath строки с клиентом
		/// </summary>
		/// <param name="clientName">название</param>
		/// <returns>XPath</returns>
		public string GetClientRowXPath(string clientName)
		{
			Logger.Trace("Получить xPath строки клиента");
			var rowNum = 0;
			IList<IWebElement> clientList = GetElementList(By.XPath(CLIENT_LIST_XPATH));
			for (int i = 0; i < clientList.Count; ++i )
			{
				if (clientList[i].Text.Contains(clientName))
				{
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