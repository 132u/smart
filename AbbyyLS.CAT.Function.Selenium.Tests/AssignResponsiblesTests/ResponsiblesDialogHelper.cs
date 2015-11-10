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

		/// <summary>
		/// Дождаться загрузки страницы назначения пользователей
		/// </summary>
		public void WaitUntilTaskAssignPageLoad()
		{
			Logger.Trace("Дождаться загрузки страницы назначения пользователей");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(SELECT_ASSIGNEES_DROPDOWN)),
				"Произошла ошибка:\n страница назначения пользователей не загрузилась.");
		}

		/// <summary>
		/// Нажать кнопку 'SelectAssignees'
		/// </summary>
		public void ClickSelectAssigneesButton()
		{
			Logger.Debug("Нажать кнопку 'SelectAssignees'");
			ClickElement(By.XPath(SELECT_ASSIGNEES_DROPDOWN));
		}

		/// <summary>
		/// Нажать кнопку назначения на весь документ
		/// </summary>
		public void ClickAssigneesForEntireDocumentButton()
		{
			Logger.Debug("Нажать кнопку назначения на весь документ");
			ClickElement(By.XPath(ASSIGNEES_FOR_ENTIRE_DOCUMENT));
		}

		/// <summary>
		/// Дождаться загрузки страницы выбора пользователей для назначения
		/// </summary>
		public void WaitUntilTaskAssignEditingPageLoad()
		{
			Logger.Trace("Дождаться загрузки страницы выбора пользователей для назначения");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(ANOTHER_ASSIGNEE)),
				"Произошла ошибка:\n страница выбора пользователей для назначения не загрузилась.");
		}

		/// <summary>
		/// Нажать кнопку добавления пользователей
		/// </summary>
		public void ClickAnotherAssigneeButton()
		{
			Logger.Debug("Нажать кнопку добавления пользователей");
			ClickElement(By.XPath(ANOTHER_ASSIGNEE));
		}

		/// <summary>
		/// Развернуть список исполнителей
		/// </summary>
		public void OpenAssignList()
		{
			Logger.Debug("Развернуть список исполнителей.");
			ClickElement(By.XPath(ASSIGN_LIST));
		}

		/// <summary>
		/// Выбрать исполнителя
		/// </summary>
		/// <param name="name">имя исполнителя</param>
		public void SelectAssigned(string name)
		{
			Logger.Debug("Выбрать исполнителя по имени:'{0}'", name);
			ClickElement(By.XPath(ASSIGN_LIST_ITEM.Replace("*#*", name)));
		}

		/// <summary>
		/// Нажать кнопку назначения пользователя.
		/// </summary>
		public void ClickAssignButton()
		{
			Logger.Debug("Нажать кнопку назначения пользователя.");
			ClickElement(By.XPath(ASSIGN_BUTTON));
		}

		/// <summary>
		/// Дождаться появления кнопки отмены
		/// </summary>
		public void WaitUntilCancelButtonDisplay()
		{
			Logger.Trace("Дождаться появления кнопки отмены");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(CANCEL_BUTTON)),
				"Произошла ошибка:\n кнопка отмены назначения не появилась.");
		}

		/// <summary>
		/// Нажать кнопку 'Закрыть'
		/// </summary>
		public void ClickCloseButton()
		{
			Logger.Debug("Нажать кнопку 'Закрыть'");
			ClickElement(By.XPath(CLOSE_BUTTON));
		}

		/// <summary>
		/// Нажать кнопку сохранения на странице Task Assignment
		/// </summary>
		public void ClickSaveButton()
		{
			Logger.Debug("Нажать кнопку сохранения на странице Task Assignment");
			ClickElement(By.XPath(SAVE_BUTTON));
			
		}

		protected const string RESPONSIBLES_FORM_XPATH = "(//div[contains(@class, 'js-popup-assign')])[2]";
		protected const string RESPONSIBLES_TABLE_XPATH = "(//table[contains(@class, 'js-progress-table')]//table)[2]";
		protected const string DROPDOWNLIST_XPATH = "//td[contains(@class, 'assineer')]//select";
		protected const string ASSIGN_BTN_XPATH = "//span[contains(@class, 'js-assign')]//a[contains(text(), 'Assign')]";
		protected const string CLOSE_BTN_XPATH = "//span/a[@class='h30 g-redbtn__text g-btn__text']";
		protected const string CHOOSE_TASK_FORM_XPATH = ".//div[@id='workflow-select-window']";
		protected const string SELECT_ASSIGNEES_DROPDOWN = "//a[contains(@data-bind,'setAssignmentsButtonTitle')]";
		protected const string ASSIGNEES_FOR_ENTIRE_DOCUMENT = "//a[contains(@data-bind,'goToStage(false)')]";
		protected const string ANOTHER_ASSIGNEE = "//a[contains(@data-bind,'addExecutive')]";
		protected const string ASSIGN_LIST = "//input[contains(@class,'newDropdown')]";
		protected const string ASSIGN_LIST_ITEM = "//li[contains(@class,'newDropdown') and text()='*#*']";
		protected const string ASSIGN_BUTTON = "//a[contains(@data-bind,'parent.assign')]";
		protected const string CANCEL_BUTTON = "//a[contains(@data-bind,'$parent.removeExecutive')]";
		protected const string CLOSE_BUTTON = "(//div[contains(@data-bind,'click: close')])[1]";
		protected const string SAVE_BUTTON = "(//div[contains(@data-bind,'saveDeadlines')])";

		private static readonly Logger Log = LogManager.GetCurrentClassLogger();
	}
}