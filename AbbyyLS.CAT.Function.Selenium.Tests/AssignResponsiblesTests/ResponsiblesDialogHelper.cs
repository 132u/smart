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
			Logger.Trace("Ожидаем, пока раскроется список пользователей");
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

		public void ClickResponsiblesDropboxByRowNumber(int taskRowNumber)
		{
			Logger.Trace(string.Format("Открытие выпадающего списка для задачи с номером строки {0}", taskRowNumber));
			
			ClickElement(By.XPath(GetResponsiblesDropboxByTaskNumberXpath(taskRowNumber)));
		}

		/// <summary>
		/// Получить XPath выпадающего списка в диалоге назначения пользователя
		/// </summary>
		/// <param name="taskRowNumber"> Номер строки таски</param>
		/// <returns> XPath </returns>
		public string GetResponsiblesDropboxByTaskNumberXpath(int taskRowNumber)
		{
			return RESPONSIBLES_TABLE_XPATH + "//tr[" + taskRowNumber + "]" + DROPDOWNLIST_XPATH;
		}

		/// <summary>
		/// Выбирает пользователя из выпадающего списка
		/// </summary>
		/// <param name="rowNumber">Номер строки задачи</param>
		/// <param name="name">Имя исполнителя</param>
		public void SetVisibleResponsible(int rowNumber, string name)
		{
			Logger.Trace(string.Format("Выбираем исполнителя {0} для задачи №{1}", name, rowNumber));
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

		public void ClickCloseBtn()
		{
			Log.Trace("Кликнуть кнопку закрытия диалога");
			ClickElement(By.XPath(RESPONSIBLES_FORM_XPATH + CLOSE_BTN_XPATH));
		}

		public bool WaitUntilChooseTaskDialogDisplay()
		{
			Log.Trace("Ожидание загрузки диалога выбора задачи");
			return WaitUntilDisplayElement(By.XPath(CHOOSE_TASK_FORM_XPATH));
		}

		protected const string RESPONSIBLES_FORM_XPATH = "(//div[contains(@class, 'js-popup-assign')])[2]";
		protected const string RESPONSIBLES_TABLE_XPATH = "(//table[contains(@class, 'js-progress-table')]//table)[2]";
		protected const string DROPDOWNLIST_XPATH = "//td[contains(@class, 'assineer')]//select";
		protected const string ASSIGN_BTN_XPATH = "//span[contains(@class, 'js-assign')]//a[contains(text(), 'Assign')]";
		protected const string CLOSE_BTN_XPATH = "//span/a[@class='h30 g-redbtn__text g-btn__text']";
		protected const string CHOOSE_TASK_FORM_XPATH = ".//div[@id='workflow-select-window']";

		private static readonly Logger Log = LogManager.GetCurrentClassLogger();
	}
}