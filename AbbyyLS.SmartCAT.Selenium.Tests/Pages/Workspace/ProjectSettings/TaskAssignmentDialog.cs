using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.ProjectSettings
{
	public class TaskAssignmentDialog : ProjectSettingsPage, IAbstractPage<TaskAssignmentDialog>
	{
		public new TaskAssignmentDialog GetPage()
		{
			var taskAssignmentDialog = new TaskAssignmentDialog();
			InitPage(taskAssignmentDialog);
			LoadPage();

			return taskAssignmentDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(TABLE_USERNAME_XPATH)))
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
		public TaskAssignmentDialog ClickAssignBtn()
		{
			Logger.Debug("Нажать на кнопку 'Назначить'.");
			AssignBtn.Click();

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
			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CLOSE_BTN_XPATH)),
				"Произошла ошибка:\n не удалось назначить исполнителя на задачу.");

			return GetPage();
		}

		/// <summary>
		/// Закрыть диалог
		/// </summary>
		public ProjectSettingsPage ClickCloseBtn()
		{
			Logger.Debug("Закрыть диалог назначения пользователя.");
			CloseBtn.Click();
			var projectSettingsPage = new ProjectSettingsPage();

			return projectSettingsPage.GetPage();
		}

		[FindsBy(How = How.XPath, Using = ASSIGN_SPAN_XPATH)]
		protected IWebElement AssignSpan { get; set; }

		[FindsBy(How = How.XPath, Using = ASSIGN_BTN_XPATH)]
		protected IWebElement AssignBtn { get; set; }

		[FindsBy(How = How.XPath, Using = TABLE_USERNAME_XPATH)]
		protected IWebElement TableUsername { get; set; }

		[FindsBy(How = How.XPath, Using = CLOSE_BTN_XPATH)]
		protected IWebElement CloseBtn { get; set; }

		[FindsBy(How = How.XPath, Using = USERLIST_SELECT_XPATH)]
		protected IWebElement UserList { get; set; }

		protected const string ASSIGN_SPAN_XPATH = ASSIGN_DIALOG_XPATH + "//td//span[contains(@class,'js-assign')]";
		protected const string USERLIST_SELECT_XPATH = TABLE_USERNAME_XPATH + "//select[contains(@data-bind , 'value: assignment')]";
		protected const string TABLE_USERNAME_XPATH = ASSIGN_DIALOG_XPATH + "//table[contains(@class,'js-progress-table')]//tbody[contains(@data-bind, 'foreach: workflowStages')]";
		protected const string ASSIGN_BTN_XPATH = ASSIGN_SPAN_XPATH + "//a";
		protected const string USER_ITEM_LIST_XPATH = "//span[contains(@class,'js-dropdown__item') and contains(@title,'*#*')]";
		protected const string CANCEL_BTN_XPATH = "//span[contains(@class,'js-assigned-cancel')]";
		protected const string CLOSE_BTN_XPATH = ASSIGN_DIALOG_XPATH + "//span[contains(@class,'js-popup-close')]/span[1]";
	}
}
