using System.Collections.Generic;
using System.Windows.Forms;
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
			SendKeys.SendWait("{HOME}");
			ClickElement(By.XPath(ADD_CLIENT_BTN_XPATH));
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

		protected const string EDIT_BTN_XPATH = "//img[contains(@class,'edit client')]";
		protected const string ADD_CLIENT_BTN_XPATH = "//div[contains(@class,'js-clients')]/div/span";
		protected const string SAVE_CLIENT_XPATH = "//img[contains(@class,'client save')]";

		protected const string CLIENT_INPUT_XPATH = "//input[contains(@class,'clienttxtbox')]";

		protected const string CLIENT_LIST_XPATH = ".//table[contains(@class,'js-sortable-table')]//tr";
	}
}