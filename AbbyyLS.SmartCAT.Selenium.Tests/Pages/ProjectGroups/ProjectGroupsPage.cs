using System;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups
{
	public class ProjectGroupsPage : WorkspacePage, IAbstractPage<ProjectGroupsPage>
	{
		public ProjectGroupsPage(WebDriver driver) : base(driver)
		{
		}

		public new ProjectGroupsPage GetPage()
		{
			var projectGroupsPage = new ProjectGroupsPage(Driver);
			InitPage(projectGroupsPage, Driver);

			return projectGroupsPage;
		}

		public new void LoadPage()
		{
			if (!IsProjectGroupsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница 'Группы проектов'");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Прокручиваем страницу(если необходимо) и нажимаем кнопку создания группы проектов
		/// </summary>
		public ProjectGroupsPage ScrollAndClickCreateProjectGroupsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку создания группы проектов.");
			AddProjectGroupsButton.ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Ввести имя группы проектов
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage FillProjectGroupName(string projectGroupName)
		{
			CustomTestContext.WriteLine("Ввести имя группы проектов {0}.", projectGroupName);

			NewProjectGroupRow.SetText(projectGroupName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Сохранить'
		/// </summary>
		public ProjectGroupsPage ClickSaveProjectGroups()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Сохранить'.");
			SaveProjectGroupsButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Отмена'
		/// </summary>
		public ProjectGroupsPage ClickCancelProjectGroups()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Отмена'.");
			CancelProjectGroupsButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку удаления
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage ClickDeleteButton(string projectGroupName)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления.");

			var deleteButton = Driver.SetDynamicValue(How.XPath, DELETE_PROJECT_GROUP_BUTTON, projectGroupName);

			deleteButton.JavaScriptClick();
			//Sleep нужен для предотвращения появления unexpected alert
			Thread.Sleep(1000);

			return GetPage();
		}

		/// <summary>
		/// Прокрутить страницу и кликнуть по строке группы проектов
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage ScrollAndClickProjectGroup(string projectGroupName)
		{
			CustomTestContext.WriteLine("Прокрутить страницу и кликнуть по строке группы проектов {0}.", projectGroupName);
			Driver.SetDynamicValue(How.XPath, PROJECT_GROUP_ROW, projectGroupName).ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку редактирования
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage ClickEditButton(string projectGroupName)
		{
			CustomTestContext.WriteLine("Нажать кнопку редактирования группы проектов {0}.", projectGroupName);
			var editButton = Driver.SetDynamicValue(How.XPath, EDIT_PROJECT_GROUP_BUTTON, projectGroupName);
			editButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Навести курсор на группу проектов
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public ProjectGroupsPage HoverCursorToProjectGroup(string projectGroupName)
		{
			CustomTestContext.WriteLine("Навести курсор на группу проектов {0}.", projectGroupName);
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
			CustomTestContext.WriteLine("Ввести новое имя группы проектов {0}.", newProjectGroupName);
			EditNameField.SetText(newProjectGroupName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по именам
		/// </summary>
		public ProjectGroupsPage ClickSortByName()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по именам");
			SortByName.Click();

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Создать группу проектов
		/// </summary>
		/// <param name="projectGroup">название группы проектов</param>
		public ProjectGroupsPage CreateProjectGroup(string projectGroup)
		{
			ScrollAndClickCreateProjectGroupsButton();

			if (!IsGroupProjectEmptyRowDisplayed())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не появилась пустая строка для создания группы проектов");
			}

			FillProjectGroupName(projectGroup);
			ClickSaveProjectGroups();

			return GetPage();
		}

		/// <summary>
		/// Переименовать группу проектов
		/// </summary>
		/// <param name="projectGroupsName">название группы проектов</param>
		/// <param name="newProjectGroupName">новое название группы проектов</param>
		public ProjectGroupsPage RenameProjectGroup(string projectGroupsName, string newProjectGroupName)
		{
			ScrollAndClickProjectGroup(projectGroupsName);
			HoverCursorToProjectGroup(projectGroupsName);

			if (!IsEditButtonDisplay(projectGroupsName))
			{
				CustomTestContext.WriteLine("Необходимо повторно навести курсор на группу проектов {0}, чтобы кнопка редактирования стала видна",
					projectGroupsName);
				HoverCursorToProjectGroup(projectGroupsName);
			}

			if (!IsEditButtonDisplay(projectGroupsName))
			{
				throw new XPathLookupException("Произошла ошибка:\nКнопка редактирования группы проектов не появилась.");
			}

			ClickEditButton(projectGroupsName);
			FillNewName(newProjectGroupName);
			ClickSaveProjectGroups();

			return GetPage();
		}

		/// <summary>
		/// Удалить группу проектов
		/// </summary>
		/// <param name="projectGroupName">название группы проектов</param>
		public ProjectGroupsPage DeleteProjectGroup(string projectGroupName)
		{
			ScrollAndClickProjectGroup(projectGroupName);
			HoverCursorToProjectGroup(projectGroupName);

			if (!IsDeleteButtonDisplay(projectGroupName))
			{
				CustomTestContext.WriteLine("Необходимо повторно навести курсор на группу проектов {0}, чтобы кнопка удаления стала видна",
					projectGroupName);
				HoverCursorToProjectGroup(projectGroupName);
			}

			if (!IsDeleteButtonDisplay(projectGroupName))
			{
				throw new XPathLookupException("Произошла ошибка:\nКнопка удаления не появилась для группы проектов");
			}

			ClickDeleteButton(projectGroupName);

			if (IsDeleteButtonDisplay(projectGroupName))
			{
				throw new Exception("Произошла ошибка:\nКнопка удаления не исчезла после удаления группы проектов");
			}

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница 'Группы проектов'
		/// </summary>
		public bool IsProjectGroupsPageOpened()
		{
			CustomTestContext.WriteLine("Проверить, открыта ли страница 'Группы проектов'");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ADD_PROJECT_GROUP_BUTTON));
		}

		/// <summary>
		/// Проверить, что мы находимся в режиме редактирования группы проектов
		/// </summary>
		public bool IsEditModeEnabled()
		{
			CustomTestContext.WriteLine("Проверить, что мы находимся в режиме редактирования группы проектов.");

			return EditNameField.Displayed;
		}

		/// <summary>
		/// Проверить, что кнопка сохранения группы проектов исчезла
		/// </summary>
		public bool IsSaveButtonDisappear()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка сохранения группы проектов исчезла.");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(SAVE_PROJECT_GROUP));
		}

		/// <summary>
		/// Проверить, что появилась ошибка в имени группы проектов при создании группы проектов
		/// </summary>
		public bool IsProjectGrouptNameErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилась ошибка в имени группы проектов при создании группы проектов.");

			return Driver.WaitUntilElementIsDisplay(NameError);
		}

		/// <summary>
		/// Проверить, что группа проектов присутствует в списке
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public bool IsProjectGroupExist(string projectGroupName)
		{
			CustomTestContext.WriteLine("Определить, что группа проектов {0} присутствует в списке.", projectGroupName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(PROJECT_GROUP_ROW.Replace("*#*", projectGroupName)));
		}

		/// <summary>
		/// Проверить, что кнопка удаления появилась
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public bool IsDeleteButtonDisplay(string projectGroupName)
		{
			CustomTestContext.WriteLine("Проверить, что кнопка удаления появилась.");

			return Driver.WaitUntilElementIsDisplay(
				By.XPath(DELETE_PROJECT_GROUP_BUTTON.Replace("*#*", projectGroupName)));
		}

		/// <summary>
		/// Проверить, что кнопка редактирования появилась
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public bool IsEditButtonDisplay(string projectGroupName)
		{
			CustomTestContext.WriteLine("Проверить, что кнопка редактирования группы проектов {0} появилась.", projectGroupName);

			return Driver.WaitUntilElementIsDisplay(
				By.XPath(EDIT_PROJECT_GROUP_BUTTON.Replace("*#*", projectGroupName)));
		}

		/// <summary>
		/// Проверить, что пустая строка для создания группы проектов появилась
		/// </summary>
		public bool IsGroupProjectEmptyRowDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что пустая строка для создания группы проектов появилась.");

			return Driver.WaitUntilElementIsDisplay(NewProjectGroupRow);
		}

		#endregion

		#region Вспомогательные методы

		/// <summary>
		/// Получить уникальное название группы проектов
		/// </summary>
		public string GetProjectGroupUniqueName()
		{
			return "TestProjectGroup - " + Guid.NewGuid();
		}

		#endregion

		#region Объявление элементов страницы

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

		[FindsBy(How = How.XPath, Using = SORT_BY_NAME)]
		protected IWebElement SortByName { get; set; }

		protected IWebElement ProjectGroupRow { get; set; }

		#endregion

		#region Описание XPath элементов

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
		protected const string SORT_BY_NAME = "//th[contains(@data-sort-by,'Name')]//a";

		#endregion
	}
}
