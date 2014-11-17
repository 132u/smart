using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер диалога редактирования назначения на задачу
	/// </summary>
	public class ResponsiblesDialogHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public ResponsiblesDialogHelper(IWebDriver driver, WebDriverWait wait)
			: base (driver, wait)
		{
		}

		/// <summary>
		/// Ожидание загрузки диалога выбора исполнителя
		/// </summary>
		/// <returns>Диалог загрузился</returns>
		public bool WaitUntilResponsiblesDialogDisplay()
		{
			// Ожидаем пока загрузится диалог
			return WaitUntilDisplayElement(By.XPath(RESPONSIBLES_TABLE_XPATH));
		}

		/// <summary>
		/// Ожидание загрузки списка исполнителей
		/// </summary>
		/// <param name="name">Имя одного из исполнителей</param>
		/// <returns>Список загрузился</returns>
		public bool WaitUntilUsersListDisplay(string name)
		{
			// Ожидаем пока загрузится диалог
			return WaitUntilDisplayElement(By.XPath(GetOurUserXpath(name)));
		}

		/// <summary>
		/// Получить xPath нашего пользователя в списке исполнителей
		/// </summary>
		/// <param name="name">Имя одного из исполнителей</param>
		/// <returns>xPath</returns>
		protected string GetOurUserXpath(string name)
		{
			return VISIBLE_RESPONSIBLE_USERS_XPATH + "[@title ='" + name + "']";
		}

		/// <summary>
		/// Ожидание закрытия диалога выбора исполнителя
		/// </summary>
		/// <returns>Диалог закрылся</returns>
		public bool WaitUntilResponsiblesDialogDissapear()
		{
			// Ожидаем пока загрузится диалог
			return WaitUntilDisappearElement(By.XPath(RESPONSIBLES_TABLE_XPATH));
		}

		/// <summary>
		/// Ожидание загрузки диалога выбора исполнителя (мастер)
		/// </summary>
		/// <returns>Диалог загрузился</returns>
		public bool WaitUntilMasterResponsiblesDialogDisplay()
		{
			// Ожидаем пока загрузится диалог
			return WaitUntilDisplayElement(By.XPath(CHOOSE_TASK_STEP__XPATH + "[2]"));
		}

		/// <summary>
		/// Возвращает список исполнителей из выпадающего списка (не включает группы)
		/// </summary>
		/// <param name="rowNumber">Номер строки задачи</param>
		/// <returns>Список исполнителей</returns>
		public List<string> GetResponsibleUsersListByRowNumber(int rowNumber)
		{
			var usersList = new List<string>();

			var xPath = RESPONSIBLES_TABLE_XPATH + "//tr[" + rowNumber + "]" +
				RESPONSIBLE_USERS_XPATH;

			var elementUsersList = GetElementList(By.XPath(xPath));

			foreach (var element in elementUsersList)
			{
				var attr = element.GetAttribute("text");

				if ((attr != "") && (!attr.Contains("Group: ")))
				{
					usersList.Add(attr.Replace("  ", " "));
				}
			}

			return usersList;
		}

		/// <summary>
		/// Возвращает список групп исполнителей из выпадающего списка
		/// </summary>
		/// <param name="rowNumber">Номер строки задачи</param>
		/// <returns>Список групп</returns>
		public List<string> GetResponsibleGroupsListByRowNumber(int rowNumber)
		{
			var groupsList = new List<string>();

			var xPath = RESPONSIBLES_TABLE_XPATH + "//tr[" + rowNumber + "]" +
				RESPONSIBLE_USERS_XPATH;

			var elementUsersList = GetElementList(By.XPath(xPath));

			foreach (var element in elementUsersList)
			{
				var attr = element.GetAttribute("text");

				if ((attr != "") && (attr.Contains("Group: ")))
					groupsList.Add(attr.Replace("  ", " "));
			}

			return groupsList;
		}

		/// <summary>
		/// Открыть выпадающий список по номеру строки задачи
		/// <param name="rowNumber">Номер строки задачи</param>
		/// </summary>
		public void ClickResponsiblesDropboxByRowNumber(int rowNumber)
		{
			var xPath = RESPONSIBLES_TABLE_XPATH + "//tr[" + rowNumber + "]" +
				DROPDOWNLIST_XPATH;

			ClickElement(By.XPath(xPath));
		}

		/// <summary>
		/// Выбирает пользователя из выпадающего списка
		/// </summary>
		/// <param name="rowNumber">Номер строки задачи</param>
		/// <param name="name">Имя исполнителя</param>
		public void SetVisibleResponsible(int rowNumber, string name)
		{
			var xPath = VISIBLE_RESPONSIBLE_USERS_XPATH;

			foreach (var element in GetElementList(By.XPath(xPath)))
			{
				if (element.GetAttribute("title") == name)
				{
					element.Click();
					break;
				}
			}
		}

		/// <summary>
		/// Кликнуть кнопку Assign
		/// </summary>
		/// <param name="rowNumber">Номер строки задачи</param>
		public void ClickAssignBtn(int rowNumber)
		{
			var xPath = RESPONSIBLES_TABLE_XPATH + "//tr[" + rowNumber + "]" +
				ASSIGN_BTN_XPATH;
			
			ClickElement(By.XPath(xPath));
		}

		/// <summary>
		/// Кликнуть кнопку Cancel
		/// </summary>
		/// <param name="rowNumber">Номер строки задачи</param>
		public void ClickCancelBtn(int rowNumber)
		{
			var xPath = RESPONSIBLES_TABLE_XPATH + "//tr[" + rowNumber + "]" +
				CANCEL_BTN_XPATH;

			ClickElement(By.XPath(xPath));
		}

		/// <summary>
		/// Кликнуть кнопку закрытия диалога
		/// </summary>
		public void ClickCloseBtn()
		{
			ClickElement(By.XPath(RESPONSIBLES_FORM_XPATH + CLOSE_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть кнопку YES
		/// </summary>
		public void ClickYesBtn()
		{
			ClickElement(By.XPath(YES_BTN_CONFIRM_XPATH));
		}

		/// <summary>
		/// Ожидание загрузки диалога выбора задачи
		/// </summary>
		/// <returns>Диалог загрузился</returns>
		public bool WaitUntilChooseTaskDialogDisplay()
		{
			// Ожидаем пока загрузится диалог
			if (!WaitUntilDisplayElement(By.XPath(CHOOSE_TASK_FORM_XPATH)))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Выбор задачи по порядковому номеру
		/// </summary>
		public void ClickChoosenTask(int rowNumber)
		{
			var taskList = GetElementList(By.XPath(TASK_XPATH));

			taskList[(rowNumber - 1)].Click();
		}
		
		/// <summary>
		/// Ожидание загрузки диалога подтверждения
		/// </summary>
		/// <returns>Диалог загрузился</returns>
		public bool WaitUntilConfirmDialogDisplay()
		{
			// Ожидаем пока загрузится диалог
			if (!WaitUntilDisplayElement(By.XPath(CONFIRM_RESET_ASSIGNMENT_FORM_XPATH)))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Ожидание закрытия диалога подтверждения
		/// </summary>
		/// <returns>Диалог закрылся</returns>
		public bool WaitUntilConfirmDialogDissapear()
		{
			// Ожидаем пока загрузится диалог
			if (!WaitUntilDisappearElement(By.XPath(CONFIRM_RESET_ASSIGNMENT_FORM_XPATH)))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Ожидание загрузки формы инфо
		/// </summary>
		/// <returns>Форма загрузилась</returns>
		public bool WaitUntilInfoDisplay()
		{
			// Ожидаем пока загрузится диалог
			if (!WaitUntilDisplayElement(By.XPath(INFO_FORM_XPATH)))
			{
				return false;
			}
			return true;
		}



		protected const string RESPONSIBLES_FORM_XPATH = ".//div[contains(@class, 'js-popup-progress')][*//select[@id='responsible']]";
		protected const string RESPONSIBLES_TABLE_XPATH = ".//table[contains(@class, 'js-progress-table')][*//select[@id='responsible']]";
		
		protected const string CHOOSE_TASK_STEP__XPATH = ".//div[contains(@class, 'js-popup-import-document')]";
		
		protected const string DROPDOWNLIST_XPATH = "//td[select[@id='responsible']]/span";
		protected const string RESPONSIBLE_USERS_XPATH = "//select[@id='responsible']/option";
		protected const string VISIBLE_RESPONSIBLE_USERS_XPATH = "//span[contains(@class, 'js-dropdown__list')]/span";
		protected const string ASSIGN_BTN_XPATH = "//span[contains(@class, 'js-assigned-block')]//a[contains(text(), 'Assign')]";
		protected const string CANCEL_BTN_XPATH = "//span[contains(@class, 'js-assigned-block')]//a[contains(text(), 'Cancel')]";
		protected const string CLOSE_BTN_XPATH = "//span[contains(@class, 'js-popup-close')]";

		protected const string YES_BTN_CONFIRM_XPATH = CONFIRM_RESET_ASSIGNMENT_FORM_XPATH + "//input[contains(@class, 'js-submit-btn ')]";

		protected const string CHOOSE_TASK_FORM_XPATH = ".//div[@id='workflowselectwindow-1025']";
		protected const string TASK_XPATH = CHOOSE_TASK_FORM_XPATH + "//table//a";
		
		protected const string CONFIRM_RESET_ASSIGNMENT_FORM_XPATH = ".//div[contains(@class, 'js-popup-confirm')]";
		protected const string INFO_FORM_XPATH = ".//div[contains(@class, 'js-info-popup')]";
	}
}