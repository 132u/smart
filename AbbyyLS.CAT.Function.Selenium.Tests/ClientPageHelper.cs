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
			var clientXPath = GetClientRowXPath(clientName);
			// Кликнуть по строке						
			ClickElement(By.XPath(clientXPath));
			var editXPath = clientXPath + EDIT_BTN_XPATH;
			// Дождаться появления Edit
			WaitUntilDisplayElement(By.XPath(editXPath));
			// Кликнуть Edit
			ClickElement(By.XPath(editXPath));
		}

		/// <summary>
		/// Ввести новое имя
		/// </summary>
		/// <param name="newName">новое имя</param>
		public void EnterNewName(string newName)
		{
			ClearAndAddText(By.XPath(ENTER_NAME_XPATH),
				newName);
		}

		/// <summary>
		/// Кликнуть Удалить
		/// </summary>
		/// <param name="clientName">название</param>
		public bool ClickDelete(string clientName)
		{
			var clientXPath = GetClientRowXPath(clientName);
			// Кликнуть по строке
			ClickElement(By.XPath(clientXPath));
			var deleteXPath = clientXPath + DELETE_BTN_XPATH;
			// Дождаться появления Delete
			WaitUntilDisplayElement(By.XPath(deleteXPath));
			// Кликнуть Delete
			ClickElement(By.XPath(deleteXPath));

			return WaitUntilDisappearElement(By.XPath(deleteXPath));
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
			return GetIsElementDisplay(By.XPath(ENTER_NAME_XPATH));
		}

		/// <summary>
		/// Вернуть, не сохранился новый клиент
		/// </summary>
		/// <returns>режим нового клиента (не сохранился)</returns>
		public bool GetIsNewClientEditMode()
		{
			return GetIsElementDisplay(By.XPath(NEW_CLIENT_INPUT_XPATH));			
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
			SendTextElement(By.XPath(NEW_CLIENT_INPUT_XPATH), name);
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
					Console.WriteLine(clientList[i].Text);
					Console.WriteLine(i);
					rowNum = i + 1;
					break;
				}
			}
			return NOT_HIDDEN_TR + "[contains(@class,'js-row')][" + rowNum + "]";
		}



		protected const string DELETE_BTN_XPATH = "//a[contains(@class,'js-delete-client')]";
		protected const string EDIT_BTN_XPATH = "//a[contains(@class,'js-edit-client')]";
		protected const string SAVE_BTN_XPATH = "//a[contains(@class,'js-save-client')]";
		protected const string ADD_CLIENT_BTN_XPATH = "//span[contains(@class,'js-add-client')]";

		protected const string ENTER_NAME_XPATH = "//td[contains(@class,'clientEdit')]//div[contains(@class,'" + EDIT_CLIENT_CLASS + "')]" + NAME_INPUT_XPATH;
		protected const string NAME_INPUT_XPATH = "//input[contains(@class,'js-client-name-input')]";
		protected const string EDIT_CLIENT_CLASS = "js-edit-mode";
		protected const string NOT_HIDDEN_TR = "//tr[not(contains(@class,'g-hidden'))]";
		protected const string SAVE_CLIENT_XPATH = NOT_HIDDEN_TR + "//div[contains(@class,'" + EDIT_CLIENT_CLASS + "') and not(contains(@class,'g-hidden'))]" + SAVE_BTN_XPATH;

		protected const string ERROR_NAME_XPATH = "//div[contains(@class,'js-error-text g-hidden')]";

		protected const string NEW_CLIENT_INPUT_XPATH = NOT_HIDDEN_TR + "//td[contains(@class,'clientNew')]//div[contains(@class,'" + EDIT_CLIENT_CLASS + "')]" + NAME_INPUT_XPATH;

		protected const string CLIENT_LIST_XPATH = ".//table[contains(@class,'js-clients')]//tr[contains(@class,'js-row') and not(contains(@class,'g-hidden'))]";
	}
}