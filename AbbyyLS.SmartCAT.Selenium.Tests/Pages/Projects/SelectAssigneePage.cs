using NUnit.Framework;
using OpenQA.Selenium;
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
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ANOTHER_ASSIGNEE_BUTTON)))
			{
				Assert.Fail("Произошла ошибка:\n не открылась страница назначения пользователя.");
			}
		}

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
		/// Проверить, что кнопка Assign исчезла
		/// </summary>
		public SelectAssigneePage AssertAssignButtonDisappeared()
		{
			CustomTestContext.WriteLine("Нажать на кнопку Cancel.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(ASSIGN_BUTTON)),
				"Произошла ошибка:\nКнопка Assign не исчезла.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилась кнопка Cancel
		/// </summary>
		public SelectAssigneePage AssertCancelAssigneeButtonDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилась кнопка Cancel.");

			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(CANCEL_ASSIGNEE_BUTTON)),
				"Произошла ошибка:\nКнопка Cancel не появилась.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Закрыть'
		/// </summary>
		public TaskAssignmentPage ClickCloseTaskAssignmentPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Закрыть'");
			CloseButton.Click();

			return new TaskAssignmentPage(Driver).GetPage();
		}

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
		
		protected const string ANOTHER_ASSIGNEE_BUTTON = "//a[contains(@class, 'a-user')]";
		protected const string ASSIGNEE_DOPDOWN = "//label[contains(@class, 'selector newDropdown')]";
		protected const string ASSIGNEE_OPTION = "//ul[contains(@class, 'list newDropdown')]//li[@title='*#*']";
		protected const string ASSIGN_BUTTON = "//a[contains(@data-bind, 'assign') and @class='red-dotted-link']";
		protected const string CANCEL_ASSIGNEE_BUTTON = "//a[contains(@data-bind, 'removeExecutive')]";
		protected const string CLOSE_BUTTON = "//span[contains(@data-bind, 'click: close')]//a";
	}
}
