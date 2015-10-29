﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class SelectAssigneePage : ProjectsPage, IAbstractPage<SelectAssigneePage>
	{
		public SelectAssigneePage(WebDriver driver) : base(driver)
		{
		}

		public new SelectAssigneePage GetPage()
		{
			var selectAssigneePage = new SelectAssigneePage(Driver);
			InitPage(selectAssigneePage, Driver);

			return selectAssigneePage;
		}

		public new void LoadPage()
		{
			if (!IsSelectAssigneePageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылась страница назначения пользователя");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на 'Another Assignee'
		/// </summary>
		public SelectAssigneePage ClickAnotherAssigneeButton(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать на 'Another Assignee'.");

			AnotherAssigneeButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Раскрыть Assignee дропдаун
		/// </summary>
		public SelectAssigneePage ExpandAssigneeDropdown()
		{
			CustomTestContext.WriteLine("Раскрыть Assignee дропдаун.");

			AssigneeDopdown.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать пользователя на назначение
		/// </summary>
		public SelectAssigneePage SelectAssigneeInDropdown(string assigneeName)
		{
			CustomTestContext.WriteLine("Выбрать пользователя {0} на назначение.", assigneeName);
			var assigneeOption = Driver.SetDynamicValue(How.XPath, ASSIGNEE_OPTION, assigneeName);

			assigneeOption.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку Assign
		/// </summary>
		public SelectAssigneePage ClickAssignButton(int taskNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать на кнопку Assign.");

			AssignButton.Click();
			
			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Закрыть'
		/// </summary>
		public TaskAssignmentPage ClickClose()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Закрыть'");
			CloseButton.Click();

			return new TaskAssignmentPage(Driver).GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Назначить исполнителя
		/// </summary>
		/// <param name="assigneeName">имя пользователя</param>
		public SelectAssigneePage SelectAssignee(string assigneeName)
		{
			ClickAnotherAssigneeButton();
			ExpandAssigneeDropdown();
			SelectAssigneeInDropdown(assigneeName);
			ClickAssignButton();

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница выбора исполнителя
		/// </summary>
		public bool IsSelectAssigneePageOpened()
		{
			CustomTestContext.WriteLine("Проверить, открыта ли страница выбора исполнителя");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ANOTHER_ASSIGNEE_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CLOSE_BUTTON)]
		protected IWebElement CloseButton { get; set; }

		[FindsBy(How = How.XPath, Using = ANOTHER_ASSIGNEE_BUTTON)]
		protected IWebElement AnotherAssigneeButton { get; set; }

		[FindsBy(How = How.XPath, Using = ASSIGNEE_DOPDOWN)]
		protected IWebElement AssigneeDopdown { get; set; }

		[FindsBy(How = How.XPath, Using = ASSIGNEE_OPTION)]
		protected IWebElement AssigneeOption { get; set; }

		[FindsBy(How = How.XPath, Using = ASSIGN_BUTTON)]
		protected IWebElement AssignButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_ASSIGNEE_BUTTON)]
		protected IWebElement CancelAssigneeButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string ANOTHER_ASSIGNEE_BUTTON = "//a[contains(@class, 'a-user')]";
		protected const string ASSIGNEE_DOPDOWN = "//label[contains(@class, 'selector newDropdown')]";
		protected const string ASSIGNEE_OPTION = "//ul[contains(@class, 'list newDropdown')]//li[@title='*#*']";
		protected const string ASSIGN_BUTTON = "//a[contains(@data-bind, 'assign') and @class='red-dotted-link']";
		protected const string CANCEL_ASSIGNEE_BUTTON = "//a[contains(@data-bind, 'removeExecutive')]";
		protected const string CLOSE_BUTTON = "//span[contains(@data-bind, 'click: close')]//a";

		#endregion
	}
}
