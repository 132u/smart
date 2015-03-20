﻿using System.Collections.Generic;
using System.Linq;
using NLog;
using NUnit.Framework;
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

		public void WaitUntilResponsiblesDialogDisplay()
		{
			Logger.Trace("Ожидание загрузки диалога выбора исполнителя");

			Assert.IsTrue(
				WaitUntilDisplayElement(By.XPath(RESPONSIBLES_TABLE_XPATH)),
				"Ошибка: Диалог выбора исполнителя не открылся.");
		}

		/// <summary>
		/// Ожидание загрузки списка исполнителей
		/// </summary>
		/// <param name="name">Имя одного из исполнителей</param>
		/// <returns>Список загрузился</returns>
		public bool WaitUntilUserInListDisplay(string name)
		{
			// Ожидаем пока загрузится диалог
			return WaitUntilDisplayElement(By.XPath(GetUserXpath(name)), 5);
		}

		/// <summary>
		/// Получить xPath нашего пользователя в списке исполнителей
		/// </summary>
		/// <param name="name">Имя одного из исполнителей</param>
		/// <returns>xPath</returns>
		protected string GetUserXpath(string name)
		{
			return RESPONSIBLES_TABLE_XPATH + "//select/option[contains(text() , '" + name + "')]";
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
			Logger.Trace("Ожидаем, пока загрузится диалог выбора исполнителя");
			return WaitUntilDisplayElement(By.XPath(CHOOSE_TASK_STEP__XPATH + "[2]"));
		}

		/// <summary>
		/// Возвращает список исполнителей из выпадающего списка (не включает группы)
		/// </summary>
		/// <param name="rowNumber">Номер строки задачи</param>
		/// <returns>Список исполнителей</returns>
		public List<string> GetResponsibleUsersList()
		{
			var elementUsersList = GetElementList(By.XPath(PATH_TO_USERS_LIST));

			return (from element in elementUsersList
					where !element.GetAttribute("innerHTML").Contains("Group: ")
					select element.GetAttribute("innerHTML").Replace("  ", " ")).ToList();
		}

		/// <summary>
		/// Возвращает список групп исполнителей из выпадающего списка
		/// </summary>
		/// <param name="rowNumber">Номер строки задачи</param>
		/// <returns>Список групп</returns>
		public List<string> GetResponsibleGroupsList()
		{
			Logger.Trace("Получаем список групп исполнителей из выпадающего списка");
			var elementUsersList = GetElementList(By.XPath(PATH_TO_USERS_LIST));

			return (from element in elementUsersList
					where element.GetAttribute("innerHTML").Contains("Group: ")
					select element.GetAttribute("innerHTML").Replace("  ", " ")).ToList();
		}

		public void ClickResponsiblesDropboxByRowNumber(int taskRowNumber)
		{
			Log.Trace(string.Format("Открытие выпадающего списка для задачи с номером строки {0}", taskRowNumber));
			
			ClickElement(By.XPath(GetResponsiblesDropboxByTaskNumberXpath(taskRowNumber)));
		}

		/// <summary>
		/// Получить XPath выпадающего списка в диалоге назначения пользователя
		/// </summary>
		/// <param name="taskRowNumber"> Номер строки таски</param>
		/// <returns> XPath </returns>
		public string GetResponsiblesDropboxByTaskNumberXpath(int taskRowNumber)
		{
			return RESPONSIBLES_TABLE_XPATH + "//tr[" + taskRowNumber + "]" +
				DROPDOWNLIST_XPATH;
		}

		/// <summary>
		/// Выбирает пользователя из выпадающего списка
		/// </summary>
		/// <param name="rowNumber">Номер строки задачи</param>
		/// <param name="name">Имя исполнителя</param>
		public void SetVisibleResponsible(int rowNumber, string name)
		{
			Log.Trace(string.Format("Выбираем исполнителя {0} для задачи №{1}", name, rowNumber));
			var xPath = RESPONSIBLES_TABLE_XPATH + DROPDOWNLIST_XPATH + "/option";
			var el1 = xPath + "[contains(text(),'" + name + "')]";

			ClickElement(By.XPath(RESPONSIBLES_TABLE_XPATH + DROPDOWNLIST_XPATH));

			while (!Driver.FindElement(By.XPath(el1)).Selected)
			{
				Driver.FindElement(By.XPath(RESPONSIBLES_TABLE_XPATH + DROPDOWNLIST_XPATH)).SendKeys(Keys.Down);
			}
		}

		public void ClickAssignBtn(int rowNumber)
		{
			Log.Trace(string.Format("Кликнуть кнопку Assign для задачи на строке {0}", rowNumber));

			var xPath = RESPONSIBLES_TABLE_XPATH + "//tr[" + rowNumber + "]" + ASSIGN_BTN_XPATH;
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

		public void ClickCloseBtn()
		{
			Log.Trace("Кликнуть кнопку закрытия диалога");
			ClickElement(By.XPath(RESPONSIBLES_FORM_XPATH + CLOSE_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть кнопку YES
		/// </summary>
		public void ClickYesBtn()
		{
			ClickElement(By.XPath(YES_BTN_CONFIRM_XPATH));
		}

		public bool WaitUntilChooseTaskDialogDisplay()
		{
			Log.Trace("Ожидание загрузки диалога выбора задачи");
			return WaitUntilDisplayElement(By.XPath(CHOOSE_TASK_FORM_XPATH));
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

		protected const string RESPONSIBLES_FORM_XPATH = "(//div[contains(@class, 'js-popup-assign')])[2]";
		protected const string RESPONSIBLES_TABLE_XPATH = "(//table[contains(@class, 'js-progress-table')]//table)[2]";
		
		protected const string CHOOSE_TASK_STEP__XPATH = ".//div[contains(@class, 'js-popup-import-document')]";

		protected const string DROPDOWNLIST_XPATH = "//td[contains(@class, 'assineer')]//select";
		protected const string ASSIGN_BTN_XPATH = "//span[contains(@class, 'js-assign')]//a[contains(text(), 'Assign')]";
		protected const string CANCEL_BTN_XPATH = "//span[contains(@class, 'js-assigned-cancel')]//a[contains(text(), 'Cancel')]";
		protected const string CLOSE_BTN_XPATH = "//span/a[@class='h30 g-redbtn__text g-btn__text']";

		protected const string YES_BTN_CONFIRM_XPATH = CONFIRM_RESET_ASSIGNMENT_FORM_XPATH + "//input[contains(@class, 'js-submit-btn ')]";

		protected const string CHOOSE_TASK_FORM_XPATH = ".//div[@id='workflow-select-window']";
		
		protected const string CONFIRM_RESET_ASSIGNMENT_FORM_XPATH = ".//div[contains(@class, 'js-popup-confirm')]";
		protected const string INFO_FORM_XPATH = ".//div[contains(@class, 'js-info-popup')]";
		protected const string PATH_TO_USERS_LIST = RESPONSIBLES_FORM_XPATH + "//td[contains(@class, 'assineer')]//select//option";

		private static readonly Logger Log = LogManager.GetCurrentClassLogger();
	}
}