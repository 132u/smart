using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups
{
	public class ProjectGroupsPage : WorkspacePage, IAbstractPage<ProjectGroupsPage>
	{
		public new ProjectGroupsPage GetPage()
		{
			var projectGroupsPage = new ProjectGroupsPage();
			InitPage(projectGroupsPage);
			LoadPage();

			return projectGroupsPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.XPath(ADD_PROJECT_GROUP_BUTTON)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница 'Группы проектов'.");
			}
		}

		/// <summary>
		/// Проверить, что группа проектов присутствует в списке
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage AssertProjectGroupExist(string projectGroupName)
		{
			Logger.Trace("Проверить, что группа проектов {0} присутствует в списке.", projectGroupName);

			Assert.IsTrue(projectGroupIsPresent(projectGroupName),
				"Произошла ошибка:\n группа проектов {0} отсутствует.", projectGroupName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что группа проектов отсутствует в списке
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage AssertProjectGroupNotExist(string projectGroupName)
		{
			Logger.Trace("Проверить, что группа проектов {0} отсутствует в списке.", projectGroupName);
			Driver.WaitUntilElementIsDissapeared(By.XPath(PROJECT_GROUP_ROW.Replace("*#*", projectGroupName)));

			Assert.IsFalse(projectGroupIsPresent(projectGroupName),
				"Произошла ошибка:\n Группа проектов {0} найдена.", projectGroupName);

			return GetPage();
		}

		/// <summary>
		/// Прокручиваем страницу(если необходимо) и нажимаем кнопку создания группы проектов
		/// </summary>
		public ProjectGroupsPage ScrollAndClickCreateProjectGroupsButton()
		{
			Logger.Debug("Нажать кнопку создания группы проектов.");
			AddProjectGroupsButton.ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Ввести имя группы проектов
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage FillProjectGroupName(string projectGroupName)
		{
			Logger.Debug("Ввести имя группы проектов {0}.", projectGroupName);
			NewProjectGroupRow.SetText(projectGroupName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Сохранить'
		/// </summary>
		public ProjectGroupsPage ClickSaveProjectGroups()
		{
			Logger.Debug("Нажать кнопку 'Сохранить'.");
			SaveProjectGroupsButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Отмена'
		/// </summary>
		public ProjectGroupsPage ClickCancelProjectGroups()
		{
			Logger.Debug("Нажать кнопку 'Отмена'.");
			CancelProjectGroupsButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка сохранения группы проектов исчезла
		/// </summary>
		public ProjectGroupsPage AssertSaveButtonDisappear()
		{
			Logger.Trace("Проверить, что кнопка сохранения группы проектов исчезла.");

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(SAVE_PROJECT_GROUP)),
				"Произошла ошибка:\n кнопка сохранения группы проектов не исчезла после сохранения.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что мы находимся в режиме редактирования группы проектов
		/// </summary>
		public ProjectGroupsPage AssertIsEditMode()
		{
			Logger.Trace("Проверить, что мы находимся в режиме редактирования группы проектов.");

			Assert.IsTrue(EditNameField.Displayed,
				"Произошла ошибка:\n произошел выход из режима редактирования группы проектов.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что мы находимся в режиме редактирования только что созданной группы проектов
		/// </summary>
		public ProjectGroupsPage AssertNewProjectGroupEditMode()
		{
			Logger.Trace("Проверить, что мы находимся в режиме редактирования только что созданной группы проектов.");

			Assert.IsTrue(NewProjectGroupRow.Displayed,
				"Произошла ошибка:\n  Произошел выход из режима редактирования только что созданной группы проектов.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку удаления
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage ClickDeleteButton(string projectGroupName)
		{
			Logger.Debug("Нажать кнопку удаления.");
			var deleteXPath = DELETE_PROJECT_GROUP_BUTTON.Replace("*#*", projectGroupName);

			Driver.FindElement(By.XPath(deleteXPath)).Click();

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(deleteXPath)),
				"Произошла ошибка:\nКнопка удаления не исчезла после удаления группы проектов.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка удаления появилась
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage AssertDeleteButtonDisplay(string projectGroupName)
		{
			Logger.Trace("Проверить, что кнопка удаления появилась.");

			Assert.IsTrue(Driver.WaitUntilElementIsPresent(By.XPath(DELETE_PROJECT_GROUP_BUTTON.Replace("*#*", projectGroupName))),
				"Произошла ошибка:\nКнопка удаления не появилась для группы проектов {0}.", projectGroupName);

			return GetPage();

		}

		/// <summary>
		/// Прокрутить страницу и кликнуть по строке группы проектов
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage ScrollAndClickProjectGroup(string projectGroupName)
		{
			Logger.Debug("Прокрутить страницу и кликнуть по строке группы проектов {0}.", projectGroupName);
			Driver.FindElement(By.XPath(PROJECT_GROUP_ROW.Replace("*#*", projectGroupName))).ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку редактирования
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage ClickEditButton(string projectGroupName)
		{
			Logger.Debug("Нажать кнопку редактирования группы проектов {0}.", projectGroupName);
			Driver.FindElement(By.XPath(EDIT_PROJECT_GROUP_BUTTON.Replace("*#*", projectGroupName))).Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка редактирования появилась
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage AssertEditButtonDisplay(string projectGroupName)
		{
			Logger.Trace("Проверить, что кнопка редактирования группы проектов {0} появилась.", projectGroupName);

			Assert.IsTrue(Driver.WaitUntilElementIsPresent(By.XPath(EDIT_PROJECT_GROUP_BUTTON.Replace("*#*", projectGroupName))),
				"Произошла ошибка:\nКнопка редактирования группы проектов не появилась.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилась ошибка в имени группы проектов при создании группы проектов
		/// </summary>
		public ProjectGroupsPage AssertProjectGrouptNameErrorExist()
		{
			Logger.Trace("Проверить, что появилась ошибка в имени группы проектов при создании группы проектов.");

			Assert.IsTrue(NameError.Displayed,
				"Произошла ошибка:\n не появилась ошибка при создании группы проектов с некорректным именем.");

			return GetPage();
		}

		/// <summary>
		/// Навести курсор на группу проектов
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage HoverCursorToProjectGroup(string projectGroupName)
		{
			Logger.Debug("Навести курсор на группу проектов {0}.", projectGroupName);
			ProjectGroupRow = Driver.SetDynamicValue(How.XPath, PROJECT_GROUP_ROW, projectGroupName);
			ProjectGroupRow.HoverElement();

			return GetPage();
		}

		/// <summary>
		/// Ввести новое имя группы проектов
		/// </summary>
		/// <param name="newProjectGroupName">имя группы проектов</param>
		public ProjectGroupsPage FillNewName(string newProjectGroupName)
		{
			Logger.Debug("Ввести новое имя группы проектов {0}.", newProjectGroupName);

			EditNameField.SetText(newProjectGroupName);

			return GetPage();
		}

		/// <summary>
		/// Определить, что группа проектов присутствует в списке
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		private bool projectGroupIsPresent(string projectGroupName)
		{
			Logger.Trace("Определить, что группа проектов {0} присутствует в списке.", projectGroupName);
			Driver.WaitUntilElementIsPresent(By.XPath(PROJECT_GROUP_ROW.Replace("*#*", projectGroupName)));

			return Driver.ElementIsPresent(By.XPath(PROJECT_GROUP_ROW.Replace("*#*", projectGroupName)));
		}

		[FindsBy(How = How.XPath, Using = CANCEL_PROJECT_GROUP)]
		protected IWebElement CancelProjectGroupsButton { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_PROJECT_GROUP)]
		protected IWebElement SaveProjectGroupsButton { get; set; }

		[FindsBy(How = How.XPath, Using = NEW_PROJECT_GROUP_ROW)]
		protected IWebElement NewProjectGroupRow { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_PROJECT_GROUP_BUTTON)]
		protected IWebElement AddProjectGroupsButton { get; set; }

		[FindsBy(How = How.XPath, Using = EDIT_NAME_FIELD)]
		protected IWebElement EditNameField { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_NAME)]
		protected IWebElement NameError { get; set; }

		protected IWebElement ProjectGroupRow { get; set; }

		protected const string PROJECT_GROUP_ROW = "//table[contains(@class,'js-sortable-table')]//p[contains(string(), '*#*')]//..";
		protected const string PROJECT_GROUPS_LIST = ".//table[contains(@class,'js-domains')]//tr[contains(@class,'js-row') and not(contains(@class,'g-hidden'))]";

		protected const string ADD_PROJECT_GROUP_BUTTON = ".//span[contains(@class,'js-add-domain')]";
		protected const string NEW_PROJECT_GROUP_ROW = "//tr[not(contains(@class,'g-hidden'))]//td[contains(@class,'domainNew')]//div[contains(@class,'js-edit-mode')]//input[contains(@class,'js-domain-name-input')]";

		protected const string SAVE_PROJECT_GROUP = "//tr[not(contains(@class,'g-hidden'))]//div[contains(@class,'js-edit-mode') and not(contains(@class,'g-hidden'))]//a[contains(@class,'js-save-domain')]";
		protected const string DELETE_PROJECT_GROUP_BUTTON = "//table[contains(@class,'js-sortable-table')]//p[contains(string(), '*#*')]//..//a[contains(@class,'js-delete-domain')]";
		protected const string EDIT_PROJECT_GROUP_BUTTON = "//table[contains(@class,'js-sortable-table')]//p[contains(string(), '*#*')]//..//a[contains(@class,'js-edit-domain')]";
		protected const string CANCEL_PROJECT_GROUP = "//tr[not(contains(@class,'g-hidden'))]//tr[not(contains(@class,'g-hidden'))]//div[contains(@class,'js-edit-mode') and not(contains(@class,'g-hidden'))]//a[contains(@class,'js-revert-domain')]//a[contains(@class,'js-revert-domain')]";

		protected const string NAME_INPUT = "//input[contains(@class,'js-domain-name-input')]";
		protected const string EDIT_NAME_FIELD = "//td[contains(@class,'domainEdit')]//div[contains(@class,'js-edit-mode')]//input[contains(@class,'js-domain-name-input')]";
		protected const string ERROR_NAME = "//div[contains(@class,'js-error-text g-hidden')]";
	}
}
