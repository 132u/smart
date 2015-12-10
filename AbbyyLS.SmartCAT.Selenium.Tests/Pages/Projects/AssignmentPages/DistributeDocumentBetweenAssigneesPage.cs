using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.AssignmentPages;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages
{
	public class DistributeDocumentBetweenAssigneesPage : ProjectsPage, IAbstractPage<DistributeDocumentBetweenAssigneesPage>
	{
		public DistributeDocumentBetweenAssigneesPage(WebDriver driver)
			: base(driver)
		{
		}

		public new DistributeDocumentBetweenAssigneesPage GetPage()
		{
			var distributeSegmentsBetweenAssigneesPage = new DistributeDocumentBetweenAssigneesPage(Driver);
			InitPage(distributeSegmentsBetweenAssigneesPage, Driver);

			return distributeSegmentsBetweenAssigneesPage;
		}

		public new void LoadPage()
		{
			if (!IsDistributeSegmentsBetweenAssigneesPageOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылась страница распределения документа между исполнителями.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на ссылку 'Select Segments And Assign'.
		/// <param name="assigneeNumber">номер исполнителя</param>
		/// </summary>
		public DistributeSegmentsBetweenAssigneesPage ClickSelectSegmentsAndAssignLink(int assigneeNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать на ссылку 'Select Segments And Assign' для исполнителя №{0}.", assigneeNumber);
			Driver.SetDynamicValue(How.XPath, SELECT_SEGMENTS_AND_ASSIGN_LINK, assigneeNumber.ToString()).Click();

			return new DistributeSegmentsBetweenAssigneesPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Back to task'.
		/// </summary>
		public TaskAssignmentPage ClickBackToTaskButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Back to task'.");
			BackToTaskButton.Click();

			return new TaskAssignmentPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Another Assignee'.
		/// </summary>
		public DistributeDocumentBetweenAssigneesPage ClickAnotherAssigneeButton()
		{
			CustomTestContext.WriteLine("Нажать на ссылку 'Another Assignee'.");
			AnotherAssigneeButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку удаления в режме редактирования
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public DistributeDocumentBetweenAssigneesPage ClickDeleteButtonEditMode(int rowNumber)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления в режме редактирования.");
			Driver.SetDynamicValue(How.XPath, DELETE_ASSIGNEE_BUTTON_EDIT_MODE, rowNumber.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку удаления исполнителя
		/// </summary>
		/// <param name="assigneeName">имя исполнителя</param>
		public DistributeDocumentBetweenAssigneesPage ClickDeleteAssigneeButton(string assigneeName)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления исполнителя {0}.", assigneeName);
			Driver.SetDynamicValue(How.XPath, DELETE_ASSIGNEE_BUTTON, assigneeName).Click();

			return GetPage();
		}

		/// <summary>
		/// Получить значение из столбца диапазон сегментов
		/// </summary>
		/// <param name="assigneeNumber">номер исполнителя</param>
		public string GetSegmentsRange(int assigneeNumber = 1)
		{
			CustomTestContext.WriteLine("Получить значение из столбца диапазон сегментов для исполнителя №{0}.", assigneeNumber);

			return Driver.SetDynamicValue(How.XPath, RANGE_COLUMN, assigneeNumber.ToString()).Text;
		}

		/// <summary>
		/// Получить значение из столбца Assignee
		/// </summary>
		/// <param name="assigneeNumber">номер исполнителя</param>
		public string GetAssigneeName(int assigneeNumber = 1)
		{
			CustomTestContext.WriteLine("Получить значение из столбца Assignee в строке №{0}.", assigneeNumber);

			return Driver.SetDynamicValue(How.XPath, ASSIGNEE_COLUMN, assigneeNumber.ToString()).Text;
		}

		/// <summary>
		/// Получить значение из столбца 'Words Count'
		/// </summary>
		/// <param name="assigneeNumber">номер исполнителя</param>
		public int GetWordsCount(int assigneeNumber = 1)
		{
			CustomTestContext.WriteLine("Получить значение из столбца 'Words Count' в строке №{0}.", assigneeNumber);
			int count;

			if (!int.TryParse(Driver.SetDynamicValue(How.XPath, WORDS_COUNT_COLUMN, assigneeNumber.ToString()).Text, out count))
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось преобразование значения из столбца количества слов в число."));
			}
			
			return count;
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать исполнителя
		/// </summary>
		/// <param name="assigneeName">имя исполнителя</param>
		/// <param name="assigneeNumber">номер исполнителя</param>
		public DistributeDocumentBetweenAssigneesPage SelectAssignee(string assigneeName, int assigneeNumber = 1)
		{
			CustomTestContext.WriteLine("Выбрать исполнителя {0}.", assigneeName);
			Driver.SetDynamicValue(How.XPath, ASSIGNEE_DROPDOWN, assigneeNumber.ToString()).Click();
			Driver.SetDynamicValue(How.XPath, ASSIGNEE_OPTION, assigneeName).Click();

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		public bool IsDistributeSegmentsBetweenAssigneesPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(TASK__DISTRIBUTE_ASSIGNMENT_TABLE));
		}

		/// <summary>
		/// Проверить, отображется ли кнопка удаления исполнителя
		/// </summary>
		/// <param name="assigneeName">имя исполнителя</param>
		public bool IsDeleteAssigneeButtonDisplayed(string assigneeName)
		{
			CustomTestContext.WriteLine("Проверить, отображется ли кнопку удаления исполнителя {0}.", assigneeName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_ASSIGNEE_BUTTON.Replace("*#*", assigneeName)));
		}

		/// <summary>
		/// Проверить, что кнопка 'Another Assignee' неактивна
		/// </summary>
		public bool IsAnotherAssigneeButtonInactive()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Another Assignee' неактивна.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ANOTHER_ASSIGNEE_INACTIVE_BUTTON));
		}

		/// <summary>
		/// Проверить, что отображаестя дропдаун в строке
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public bool IsAssigneeEmptyDropdownDisplayed(int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Проверить, что отображаестя пустой дропдаун в строке №{0}.", rowNumber);

			return Driver.SetDynamicValue(How.XPath, ASSIGNEE_DROPDOWN, rowNumber.ToString()).Displayed;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = ASSIGNEE_DROPDOWN)]
		protected IWebElement AssigneeDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = SELECT_SEGMENTS_AND_ASSIGN_LINK)]
		protected IWebElement SelectSegmentsAndAssignLink { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_ASSIGNEE_BUTTON)]
		protected IWebElement DeleteAssigneeButton { get; set; }

		[FindsBy(How = How.XPath, Using = ANOTHER_ASSIGNEE_BUTTON)]
		protected IWebElement AnotherAssigneeButton { get; set; }

		[FindsBy(How = How.XPath, Using = BACK_TO_TASK_BUTTON)]
		protected IWebElement BackToTaskButton { get; set; }
		
		#endregion

		#region Описание XPath элементов

		protected const string TASK__DISTRIBUTE_ASSIGNMENT_TABLE = "//table[contains(@class, 'table assign-user')]";
		protected const string ASSIGNEE_DROPDOWN = "//tr[*#*]//input[contains(@class, 'newDropdown')]";
		protected const string ASSIGNEE_OPTION = "//li[@title = '*#*']";
		protected const string SELECT_SEGMENTS_AND_ASSIGN_LINK = "//tr[*#*]//a[contains(@data-bind, 'Select an assignee')]";
		protected const string DELETE_ASSIGNEE_BUTTON = "//td[@class='executor']//span[text()='*#*']/following-sibling::i[contains(@class, 'executive-item-delete')]";
		protected const string RANGE_COLUMN = "//tr[*#*]//span[@class='segments-item']";
		protected const string DELETE_ASSIGNEE_BUTTON_EDIT_MODE = "//tr[*#*]//i[contains(@class, 'icon-delete')]";
		protected const string ASSIGNEE_COLUMN = "//tr[*#*]//span[contains(@data-bind, 'text: executive().name')]";
		protected const string ANOTHER_ASSIGNEE_BUTTON = "//div[contains(@data-bind, 'click: addNewExecutive')]//a";
		protected const string ANOTHER_ASSIGNEE_INACTIVE_BUTTON = "//div[contains(@data-bind, 'click: addNewExecutive') and contains(@class, 'disable')]//a";
		protected const string BACK_TO_TASK_BUTTON = "//div[@data-bind='click: close']";
		protected const string WORDS_COUNT_COLUMN = "//tr[*#*]//span[contains(@data-bind, 'wordsCount')]";

		#endregion
	}
}
