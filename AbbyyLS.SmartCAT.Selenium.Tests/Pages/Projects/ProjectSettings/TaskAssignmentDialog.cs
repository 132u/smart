using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class TaskAssignmentDialog : ProjectSettingsPage, IAbstractPage<TaskAssignmentDialog>
	{
		public new TaskAssignmentDialog GetPage()
		{
			var taskAssignmentDialog = new TaskAssignmentDialog();
			InitPage(taskAssignmentDialog);

			return taskAssignmentDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(TABLE_USERNAME)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог назначения задачи на пользователя.");
			}
		}

		/// <summary>
		/// Проверить, назначен ли пользователь на задачу
		/// </summary>
		public bool IsAssignStatusNotAssigned()
		{
			Logger.Trace("Проверить, назначен ли пользователь на задачу.");

			return AssignSpan.GetAttribute("class").Contains("notAssigned");
		}

		/// <summary>
		/// Кликнуть на кнопку "Назначить"
		/// </summary>
		public TaskAssignmentDialog ClickAssignButton()
		{
			Logger.Debug("Нажать на кнопку 'Назначить'.");
			AssignButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать исполнителя из списка
		/// </summary>
		/// <param name="userName">имя исполнителя</param>
		public TaskAssignmentDialog SelectAssignee(string userName)
		{
			Logger.Debug("Выбрать исполнителя {0} из списка.", userName);
			UserList.SelectOptionByText(userName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, удалось ли назначить исполнителя
		/// </summary>
		public TaskAssignmentDialog AssertIsUserAssigned()
		{
			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CLOSE_BTN)),
				"Произошла ошибка:\n не удалось назначить исполнителя на задачу.");

			return GetPage();
		}

		/// <summary>
		/// Закрыть диалог
		/// </summary>
		public ProjectSettingsPage ClickCloseButton()
		{
			Logger.Debug("Закрыть диалог назначения пользователя.");
			CloseButton.Click();
			var projectSettingsPage = new ProjectSettingsPage();

			return projectSettingsPage.GetPage();
		}

		[FindsBy(How = How.XPath, Using = ASSIGN_SPAN)]
		protected IWebElement AssignSpan { get; set; }

		[FindsBy(How = How.XPath, Using = ASSIGN_BTN)]
		protected IWebElement AssignButton { get; set; }

		[FindsBy(How = How.XPath, Using = TABLE_USERNAME)]
		protected IWebElement TableUsername { get; set; }

		[FindsBy(How = How.XPath, Using = CLOSE_BTN)]
		protected IWebElement CloseButton { get; set; }

		[FindsBy(How = How.XPath, Using = USERLIST_SELECT)]
		protected IWebElement UserList { get; set; }

		protected const string ASSIGN_SPAN = "//div[contains(@class,'js-popup-assign')][2]//td//span[contains(@class,'js-assign')]";
		protected const string TABLE_USERNAME = "//div[contains(@class,'js-popup-assign')][2]//table[contains(@class,'js-progress-table')]//tbody[contains(@data-bind, 'foreach: workflowStages')]";
		protected const string CLOSE_BTN = "//div[contains(@class,'js-popup-assign')][2]//span[contains(@class,'js-popup-close')]/span[1]";
		protected const string ASSIGN_BTN = "//div[contains(@class,'js-popup-assign')][2]//td//span[contains(@class,'js-assign')]//a";
		protected const string USERLIST_SELECT = "//div[contains(@class,'js-popup-assign')][2]//table[contains(@class,'js-progress-table')]//tbody[contains(@data-bind, 'foreach: workflowStages')]//select[contains(@data-bind , 'value: assignment')]";
		
		protected const string USER_ITEM_LIST = "//span[contains(@class,'js-dropdown__item') and contains(@title,'*#*')]";
		protected const string CANCEL_BTN = "//span[contains(@class,'js-assigned-cancel')]";
	}
}
