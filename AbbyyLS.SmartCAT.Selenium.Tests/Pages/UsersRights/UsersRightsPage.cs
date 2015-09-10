using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class UsersRightsPage : WorkspacePage, IAbstractPage<UsersRightsPage>
	{
		public new UsersRightsPage GetPage()
		{
			var usersRightsPage = new UsersRightsPage();
			InitPage(usersRightsPage);

			return usersRightsPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(GROUPS_RIGHTS_BTN_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти на вкладку 'Пользователи и права'.");
			}
		}

		/// <summary>
		/// Перейти на вкладку "Группы и права"
		/// </summary>
		public UsersRightsPage ClickGroupsButton()
		{
			Logger.Debug("Перейти на вкладку 'Группы и права'.");
			GroupsButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, существует ли группа
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsGroupExists(string groupName)
		{
			Logger.Trace("Проверить, существует ли группа {0}.", groupName);

			return Driver.ElementIsDisplayed(By.XPath(GROUP_XPATH.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Нажать кнопку "Создать группу"
		/// </summary>
		public UsersRightsPage ClickCreateGroupButton()
		{
			Logger.Debug("Нажать на кнопку 'Создать группу'.");
			CreateGroupButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, появилась ли форма для добавления новой группы
		/// </summary>
		public UsersRightsPage AssertAddNewGroupForm()
		{
			Logger.Trace("Проверить, появилась ли форма для добавления новой группы.");
			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(NEW_GROUP_NAME_INPUT_XPATH)),
				"Произошла ошибка:\n не появилась форма для добавления новой группы.");

			return GetPage();
		}

		/// <summary>
		/// Ввести имя создаваемой группы
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage SetNewGroupName(string groupName)
		{
			Logger.Debug("Ввести имя создаваемой группы: {0}.", groupName);
			NewGroupNameInput.SetText(groupName);

			return GetPage();
		}

		/// <summary>
		/// Сохранить новую группу
		/// </summary>
		public UsersRightsPage ClickSaveNewGroupButton()
		{
			Logger.Debug("Сохранить новую группу.");
			SaveNewGroupButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, создалась ли новая группа
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage AssertIsGroupCreated(string groupName)
		{
			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(GROUP_XPATH.Replace("*#*", groupName))),
				"Произошла ошибка:\n не удалось создать группу " + groupName);

			return GetPage();
		}

		/// <summary>
		/// Кликнуть на нужную группу в таблице
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage SelectGroup(string groupName)
		{
			Logger.Debug("Кликнуть на группу {0} в таблице, чтобы появился выпадающий список с пользователями и правами.", groupName);
			Group = Driver.SetDynamicValue(How.XPath, GROUP_XPATH, groupName);
			Group.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, присуствует ли кнопка "Редактировать группу"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsEditGroupButtonDisplayed(string groupName)
		{
			return Driver.ElementIsDisplayed(By.XPath(EDIT_GROUP_BTN_XPATH.Replace("*#*", groupName))) && Driver.ElementIsEnabled(By.XPath(EDIT_GROUP_BTN_XPATH.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Нажать кнопку "Редактировать группу"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage ClickEditGroupButton(string groupName)
		{
			Logger.Debug("Нажать кнопку 'Редактировать группу'.");
			EditGroupButton = Driver.SetDynamicValue(How.XPath, EDIT_GROUP_BTN_XPATH, groupName);
			EditGroupButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, есть ли пользователь в группе
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <param name="groupName">имя группы</param>
		public bool IsGroupUserAdded(string groupName, string userName)
		{
			Logger.Trace("Проверить, есть ли пользователь {0} в группе {1}.", userName, groupName);

			return Driver.ElementIsDisplayed(By.XPath(GROUP_USER_XPATH.Replace("*#*", groupName).Replace("*##*", userName)));
		}

		/// <summary>
		/// Проверить, удалось ли добавить пользователя в группу
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage AssertIsGroupUserAdded(string groupName, string userName)
		{
			Logger.Trace("Проверить удалось ли добавить пользователя {0} в группу {1}", userName, groupName);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(
					By.XPath(GROUP_USER_XPATH.Replace("*#*", groupName).Replace("*##*", userName)),
					timeout: 10),
				"Произошла ошибка:\n не удалось добавить пользователя {0} в группу {1}.",userName, groupName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Добавить права"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public AddAccessRightDialog ClickAddRightsButton(string groupName)
		{
			Logger.Debug("Нажать на кнопку 'Добавить права' для группы {0}.", groupName);
			AddRightsButton = Driver.SetDynamicValue(How.XPath, ADD_RIGHTS_BTN_XPATH, groupName);
			AddRightsButton.Click();

			return new AddAccessRightDialog().GetPage();
		}

		/// <summary>
		/// Кликнуть по строке поиска пользователей для появления выпадающего списка с пользователями аккаунта
		/// </summary>
		public UsersRightsPage ClickAddUsersSearchbox(string groupName)
		{
			Logger.Debug("Кликнуть по строке поиска пользователей для появления выпадающего списка с пользователями аккаунта.");
			AddGroupUsersInput = Driver.SetDynamicValue(How.XPath, ADD_GROUP_USERS_INPUT_XPATH, groupName);
			AddGroupUsersInput.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что данный пользователь есть в списке с пользователями аккаунта
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage AssertIsAddGroupUserButtonExists(string groupName, string userName)
		{
			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(ADD_GROUP_USER_BTN_XPATH.Replace("*#*", groupName).Replace("*##*", userName))),
				"Произошла ошибка:\n не появился выпадающий список с пользователями аккаунта, либо {0} нет в списке", userName);

			return GetPage();
		}

		/// <summary>
		/// Выбрать пользователя из списка и нажать кнопку "Добавить"
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage ClickAddGroupUserButton(string groupName, string userName)
		{
			Logger.Debug("Выбрать пользователя {0} из списка и нажать кнопку 'Добавить' в группу {1}.", userName, groupName);
			AddGroupUserButton = Driver.SetDynamicValue(How.XPath, ADD_GROUP_USER_BTN_XPATH, groupName, userName);
			AddGroupUserButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, удалось ли добавить группе право на управление проектами
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage AssertIsManageProjectsRightAdded(string groupName)
		{
			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(MANAGE_PROJECTS_RIGHT_TEXT_XPATH.Replace("*#*", groupName))),
				"Произошла ошибка:\n не удалось добавить право на управление проектами ");
			
			return GetPage();
		}

		/// <summary>
		/// Проверить, удалось ли добавить группе право на просмотр проектов
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage AssertIsViewProjectsRightAdded(string groupName)
		{
			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(VIEW_PROJECTS_RIGHT_TEXT_XPATH.Replace("*#*", groupName))),
				"Произошла ошибка:\n не удалось добавить право на просмотр проектов ");

			return GetPage();
		}

		/// <summary>
		/// Проверить, удалось ли добавить группе право на создание проектов
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage AssertIsCreateProjectsRightAdded(string groupName)
		{
			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(CREATE_PROJECTS_RIGHT_TEXT_XPATH.Replace("*#*", groupName))),
				"Произошла ошибка:\n не удалось добавить право на создание проектов ");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Сохранить"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage ClickSaveButton(string groupName)
		{
			Logger.Debug("Нажать кнопку 'Сохранить' ( настройки группы {0}).", groupName);
			SaveButton = Driver.SetDynamicValue(How.XPath, SAVE_BTN_XPATH, groupName);
			SaveButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить список пользователей
		/// </summary>
		public List<string> GetUserNameList()
		{
			Logger.Trace("Получить список пользователей");

			var nameList = Driver.GetTextListElement(By.XPath(USER_NAME_LIST));
			var surnameList = Driver.GetTextListElement(By.XPath(USER_SURNAME_LIST));

			Assert.IsTrue(nameList.Count == surnameList.Count,
				"Произошла ошибка:\n размеры списка фамилий и списка имён не совпадают.");

			return nameList.Select((t, i) => (t + " " + surnameList[i]).Trim()).ToList();
		}

		/// <summary>
		/// Получить список имен групп пользователей
		/// </summary>
		public List<string> GetGroupNameList()
		{
			Logger.Trace("Получить список имен групп пользователей");
			return Driver.GetTextListElement(By.XPath(GROUP_NAME_LIST));
		}

		/// <summary>
		/// Проверить, что пользователь есть в списке
		/// </summary>
		/// <param name="username">имя пользователя</param>
		public UsersRightsPage AssertIsUserExist(string username)
		{
			Assert.IsTrue(GetUserNameList().Contains(username),
				string.Format("Произошла ошибка:\n пользователь '{0}' не найден в списке.", username));

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по имени
		/// </summary>
		public UsersRightsPage ClickSortByFirstName()
		{
			Logger.Debug("Нажать кнопку сортировки по имени.");
			SortByFirstName.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки фамилии
		/// </summary>
		public UsersRightsPage ClickSortByLastName()
		{
			Logger.Debug("Нажать кнопку сортировки фамилии");
			SortByLastName.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по ShortName
		/// </summary>
		public UsersRightsPage ClickSortByShortName()
		{
			Logger.Debug("Нажать кнопку сортировки по ShortName");
			SortByShortName.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по адресу почты
		/// </summary>
		public UsersRightsPage ClickSortByEmailAddress()
		{
			Logger.Debug("Нажать кнопку сортировки по адресу почты");
			SortByEmail.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по группе
		/// </summary>
		public UsersRightsPage ClickSortByGroups()
		{
			Logger.Debug("Нажать кнопку сортировки по группе");
			SortByGroups.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате создания
		/// </summary>
		public UsersRightsPage ClickSortByCreated()
		{
			Logger.Debug("Нажать кнопку сортировки по дате создания");
			SortByCreated.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по статусу
		/// </summary>
		public UsersRightsPage ClickSortByStatus()
		{
			Logger.Trace("Нажать кнопку сортировки по статусу");
			SortByStatus.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть на кнопку удаления пользователя из группы(крестик)
		/// </summary>
		/// <param name="groupName">имя группы</param>
		/// <param name="userName">имя пользователя</param>
		public UsersRightsPage ClickDeleteUserButton(string groupName, string userName)
		{
			Logger.Debug("Кликнуть на кнопку удаления юзера {0} из группы {1}", userName, groupName);

			DeleteUserButton = Driver.SetDynamicValue(How.XPath, DELETE_USER_BUTTON, groupName, userName);
			DeleteUserButton.Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = GROUPS_RIGHTS_BTN_XPATH)]
		protected IWebElement GroupsButton { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_GROUP_BTN_XPATH)]
		protected IWebElement CreateGroupButton { get; set; }

		[FindsBy(How = How.XPath, Using = NEW_GROUP_NAME_INPUT_XPATH)]
		protected IWebElement NewGroupNameInput { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_NEW_GROUP_BTN_XPATH)]
		protected IWebElement SaveNewGroupButton { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_FIRST_NAME)]
		protected IWebElement SortByFirstName { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_LAST_NAME)]
		protected IWebElement SortByLastName { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_SHORT_NANE)]
		protected IWebElement SortByShortName { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_EMAIL)]
		protected IWebElement SortByEmail { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_GROUPS)]
		protected IWebElement SortByGroups { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_CREATED)]
		protected IWebElement SortByCreated { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_STATUS)]
		protected IWebElement SortByStatus { get; set; }

		protected IWebElement SaveButton { get; set; }

		protected IWebElement EditGroupButton { get; set; }

		protected IWebElement Group { get; set; }

		protected IWebElement AddRightsButton { get; set; }

		protected IWebElement AddGroupUsersInput { get; set; }

		protected IWebElement AddGroupUserButton { get; set; }

		protected IWebElement DeleteUserButton { get; set; }

		protected const string GROUPS_RIGHTS_BTN_XPATH = "//a[contains(@href,'/Groups/Index')]";
		protected const string GROUP_XPATH = "//td[contains(@data-bind, 'text: name')][string()='*#*']";
		protected const string CREATE_GROUP_BTN_XPATH = "//span[contains(@data-bind, 'click: addGroup')]//a[contains(string(),'Create Group')]";
		protected const string NEW_GROUP_NAME_INPUT_XPATH = "//input[contains(@class, 'add-group-popup')]";
		protected const string SAVE_NEW_GROUP_BTN_XPATH = "//div[contains(@class, 'g-popupbox__ft')]//a[contains(string(),'Create Group')]";
		protected const string EDIT_GROUP_BTN_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//span[contains(@data-bind, 'click: edit')]//a";
		protected const string ADD_RIGHTS_BTN_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//span[contains(@data-bind,'click: addAccessRight')]";
		protected const string GROUP_USER_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//ul[contains(@data-bind, 'foreach: users')]//li[contains(string(), '*##*')]";
		protected const string DELETE_USER_BUTTON = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//ul[contains(@data-bind, 'foreach: users')]//li[contains(string(), '*##*')]//span[contains(@data-bind,'removeUser')]";
		protected const string ADD_GROUP_USERS_INPUT_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//input[contains(@class, 'tblgrp_finduser')]";
		protected const string ADD_GROUP_USER_BTN_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//div[contains(@class, 'users-add-list')]//table//tr[contains(string(),'*##*')]//a[string() = 'Add']";
		
		protected const string VIEW_PROJECTS_RIGHT_TEXT_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//li[contains(string(), 'View all projects')]";
		protected const string MANAGE_PROJECTS_RIGHT_TEXT_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//li[contains(string(), 'Manage all projects')]";
		protected const string CREATE_PROJECTS_RIGHT_TEXT_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//li[contains(string(), 'Create any projects')]";
		protected const string SAVE_BTN_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//span[contains(@data-bind,'click: save')]//a";

		protected const string USER_SURNAME_LIST = ".//table[contains(@class, 'js-users')]//tr[contains(@class, 'js-users-trwork')]//td[contains(@class, 'js-user-surname')]/p";
		protected const string USER_NAME_LIST = ".//table[contains(@class, 'js-users')]//tr[contains(@class, 'js-users-trwork')]//td[contains(@class, 'js-user-name')]/p";

		protected const string GROUP_NAME_LIST = "//tbody[@data-bind='foreach: filteredGroups']//tr[contains(@class, 'clickable')]//td[@data-bind='text: name']";

		protected const string SORT_BY_FIRST_NAME = "(//th[contains(@data-sort-by,'Name')]//a)[1]";
		protected const string SORT_BY_LAST_NAME = "//th[contains(@data-sort-by,'Surname')]//a";
		protected const string SORT_BY_SHORT_NANE = "(//th[contains(@data-sort-by,'Name')]//a)[2]";
		protected const string SORT_BY_EMAIL = "//th[contains(@data-sort-by,'EMail')]//a";
		protected const string SORT_BY_GROUPS = "";
		protected const string SORT_BY_CREATED = "//th[contains(@data-sort-by,'CreatedDate')]//a";
		protected const string SORT_BY_STATUS = "//th[contains(@data-sort-by,'Status')]//a";
	}
}
