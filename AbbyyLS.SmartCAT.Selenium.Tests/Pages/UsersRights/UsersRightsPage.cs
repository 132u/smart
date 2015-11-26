using System;
using System.Collections.Generic;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class UsersRightsPage : WorkspacePage, IAbstractPage<UsersRightsPage>
	{
		public UsersRightsPage(WebDriver driver) : base(driver)
		{
		}

		public new UsersRightsPage GetPage()
		{
			var usersRightsPage = new UsersRightsPage(Driver);
			InitPage(usersRightsPage, Driver);

			return usersRightsPage;
		}

		public new void LoadPage()
		{
			if (!IsUsersRightsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось перейти на вкладку 'Пользователи и права'.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Перейти на вкладку "Группы и права"
		/// </summary>
		public UsersRightsPage ClickGroupsButton()
		{
			CustomTestContext.WriteLine("Перейти на вкладку 'Группы и права'.");
			GroupsButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Создать группу"
		/// </summary>
		public UsersRightsPage ClickCreateGroupButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Создать группу'.");
			CreateGroupButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Ввести имя создаваемой группы
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage SetNewGroupName(string groupName)
		{
			CustomTestContext.WriteLine("Ввести имя создаваемой группы: {0}.", groupName);

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(NEW_GROUP_NAME_INPUT_XPATH)))
			{
				throw new XPathLookupException("Не появилось поле ввода для имени создаваемой группы");
			}

			NewGroupNameInput.SetText(groupName);

			return GetPage();
		}

		/// <summary>
		/// Сохранить новую группу
		/// </summary>
		public UsersRightsPage ClickSaveNewGroupButton()
		{
			CustomTestContext.WriteLine("Сохранить новую группу.");
			SaveNewGroupButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть на нужную группу в таблице
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage ClickGroupRow(string groupName)
		{
			CustomTestContext.WriteLine("Кликнуть на группу {0} в таблице, чтобы появился выпадающий список с пользователями и правами.", groupName);
			Group = Driver.SetDynamicValue(How.XPath, GROUP_XPATH, groupName);
			Group.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Редактировать группу"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage ClickEditGroupButton(string groupName)
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Редактировать группу'.");
			EditGroupButton = Driver.SetDynamicValue(How.XPath, EDIT_GROUP_BTN_XPATH, groupName);
			EditGroupButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по имени
		/// </summary>
		public UsersRightsPage ClickSortByFirstName()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по имени.");
			SortByFirstName.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки фамилии
		/// </summary>
		public UsersRightsPage ClickSortByLastName()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки фамилии");
			SortByLastName.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по ShortName
		/// </summary>
		public UsersRightsPage ClickSortByShortName()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по ShortName");
			SortByShortName.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по адресу почты
		/// </summary>
		public UsersRightsPage ClickSortByEmailAddress()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по адресу почты");
			SortByEmail.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по группе
		/// </summary>
		public UsersRightsPage ClickSortByGroups()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по группе");
			SortByGroups.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате создания
		/// </summary>
		public UsersRightsPage ClickSortByCreated()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по дате создания");
			SortByCreated.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по статусу
		/// </summary>
		public UsersRightsPage ClickSortByStatus()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по статусу");
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
			CustomTestContext.WriteLine("Кликнуть на кнопку удаления юзера {0} из группы {1}", userName, groupName);

			DeleteUserButton = Driver.SetDynamicValue(How.XPath, DELETE_USER_BUTTON, groupName, userName);
			DeleteUserButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Добавить права"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public AddAccessRightDialog ClickAddRightsButton(string groupName)
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Добавить права' для группы {0}.", groupName);
			AddRightsButton = Driver.SetDynamicValue(How.XPath, ADD_RIGHTS_BTN_XPATH, groupName);
			AddRightsButton.Click();

			return new AddAccessRightDialog(Driver).GetPage();
		}

		/// <summary>
		/// Кликнуть по строке поиска пользователей для появления выпадающего списка с пользователями аккаунта
		/// </summary>
		public UsersRightsPage ClickAddUsersSearchbox(string groupName)
		{
			CustomTestContext.WriteLine("Кликнуть по строке поиска пользователей для появления выпадающего списка с пользователями аккаунта.");
			AddGroupUsersInput = Driver.SetDynamicValue(How.XPath, ADD_GROUP_USERS_INPUT_XPATH, groupName);
			AddGroupUsersInput.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать пользователя из списка и нажать кнопку "Добавить"
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage ClickAddGroupUserButton(string groupName, string userName)
		{
			CustomTestContext.WriteLine("Выбрать пользователя {0} из списка и нажать кнопку 'Добавить' в группу {1}.", userName, groupName);
			AddGroupUserButton = Driver.SetDynamicValue(How.XPath, ADD_GROUP_USER_BTN_XPATH, groupName, userName);
			AddGroupUserButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Сохранить"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage ClickSaveButton(string groupName)
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Сохранить' ( настройки группы {0}).", groupName);
			SaveButton = Driver.SetDynamicValue(How.XPath, SAVE_BTN_XPATH, groupName);
			SaveButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить список пользователей
		/// </summary>
		public List<string> GetUserNameList()
		{
			CustomTestContext.WriteLine("Получить список пользователей");

			var nameList = Driver.GetTextListElement(By.XPath(USER_NAME_LIST));
			var surnameList = Driver.GetTextListElement(By.XPath(USER_SURNAME_LIST));

			if (nameList.Count != surnameList.Count)
			{
				throw new Exception("Произошла ошибка:\n размеры списка фамилий и списка имён не совпадают.");
			}

			return nameList.Select((t, i) => (t + " " + surnameList[i]).Trim()).ToList();
		}

		/// <summary>
		/// Получить список имен групп пользователей
		/// </summary>
		public List<string> GetGroupNameList()
		{
			CustomTestContext.WriteLine("Получить список имен групп пользователей");
			return Driver.GetTextListElement(By.XPath(GROUP_NAME_LIST));
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Открыть диалог добавления прав
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public AddAccessRightDialog OpenAddRightsDialogForGroup(string groupName)
		{
			ClickGroupRow(groupName);

			if (IsEditGroupButtonEnabled(groupName))
			{
				ClickEditGroupButton(groupName);
			}

			var addAccessRightDialog = ClickAddRightsButton(groupName);

			return addAccessRightDialog;
		}

		/// <summary>
		/// Создать группу, если она еще не создана (выполняется проверка)
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage CreateGroupIfNotExist(string groupName)
		{
			OpenHideMenuIfClosed();
			ClickUsersRightsButton();
			ClickGroupsButton();

			if (!IsGroupExists(groupName))
			{
				ClickCreateGroupButton();
				SetNewGroupName(groupName);
				ClickSaveNewGroupButton();
				WaitUntilDialogBackgroundDisappeared();
			}

			return GetPage();
		}

		/// <summary>
		/// Добавить пользователя в группу, если он еще не добавлен (выполняется проверка)
		/// </summary>
		/// <param name="groupName">имя группы</param>
		/// <param name="userName">имя пользователя</param>
		public UsersRightsPage AddUserToGroupIfNotAlredyAdded(string groupName, string userName)
		{
			ClickGroupRow(groupName);

			if (IsEditGroupButtonEnabled(groupName))
			{
				ClickEditGroupButton(groupName);
			}
			if (!IsUserExistInGroup(groupName, userName))
			{
				ClickAddUsersSearchbox(groupName);
				ClickAddGroupUserButton(groupName, userName);
				ClickSaveButton(groupName);
			}

			return GetPage();
		}

		/// <summary>
		/// Удалить пользователя из всех групп
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public UsersRightsPage RemoveUserFromAllGroups(string userName)
		{
			var groups = GetGroupNameList();

			foreach (var group in groups)
			{
				ClickGroupRow(group);

				if (IsUserExistInGroup(group, userName))
				{
					ClickEditGroupButton(group);
					ClickDeleteUserButton(group, userName);
					ClickSaveButton(group);
				}

				ClickGroupRow(group);
			}

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, есть ли пользователь в группе
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <param name="groupName">имя группы</param>
		public bool IsUserExistInGroup(string groupName, string userName)
		{
			CustomTestContext.WriteLine("Проверить, есть ли пользователь {0} в группе {1}.", userName, groupName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(GROUP_USER_XPATH.Replace("*#*", groupName).Replace("*##*", userName)), 3);
		}

		/// <summary>
		/// Проверить, открыта ли страница Users and Rights
		/// </summary>
		public bool IsUsersRightsPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(GROUPS_RIGHTS_BTN_XPATH));
		}

		/// <summary>
		/// Проверить, существует ли группа
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsGroupExists(string groupName)
		{
			CustomTestContext.WriteLine("Проверить, существует ли группа {0}.", groupName);

			return Driver.ElementIsDisplayed(By.XPath(GROUP_XPATH.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Проверить, присуствует ли кнопка "Редактировать группу"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsEditGroupButtonEnabled(string groupName)
		{
			CustomTestContext.WriteLine("Проверить, доступна ли кнопка 'Редактировать группу'");

			return Driver.ElementIsEnabled(By.XPath(EDIT_GROUP_BTN_XPATH.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Проверить, удалось ли добавить группе право на управление проектами
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsManageProjectsRightAdded(string groupName)
		{
			CustomTestContext.WriteLine("Проверить, удалось ли добавить группе право на управление проектами");

			return Driver.ElementIsDisplayed(By.XPath(MANAGE_PROJECTS_RIGHT_TEXT_XPATH.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Проверить, удалось ли добавить группе право на просмотр проектов
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsViewProjectsRightAdded(string groupName)
		{
			CustomTestContext.WriteLine("Проверить, удалось ли добавить группе право на просмотр проектов");

			return Driver.ElementIsDisplayed(By.XPath(VIEW_PROJECTS_RIGHT_TEXT_XPATH.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Проверить, удалось ли добавить группе право на создание проектов
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsCreateProjectsRightAdded(string groupName)
		{
			CustomTestContext.WriteLine("Проверить, удалось ли добавить группе право на создание проектов");

			return Driver.ElementIsDisplayed(By.XPath(CREATE_PROJECTS_RIGHT_TEXT_XPATH.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Проверить, что пользователь есть в списке
		/// </summary>
		/// <param name="username">имя пользователя</param>
		public bool IsUserExistInList(string username)
		{
			CustomTestContext.WriteLine("Проверить, что пользователь есть в списке");

			return GetUserNameList().Contains(username);
		}

		#endregion

		#region Вспомогательные методы

		public string GetGroupUniqueName()
		{
			return "GroupTest - " + Guid.NewGuid();
		}

		#endregion

		#region Объявление элементов страницы

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

		#endregion

		#region Описание XPath элементов

		protected const string GROUPS_RIGHTS_BTN_XPATH = "//a[contains(@href,'/Groups/Index')]";
		protected const string GROUP_XPATH = "//td[contains(@data-bind, 'text: name')][string()='*#*']";
		protected const string CREATE_GROUP_BTN_XPATH = "//div[contains(@data-bind, 'click: addGroup')]//a[contains(string(),'Create Group')]";
		protected const string NEW_GROUP_NAME_INPUT_XPATH = "//input[contains(@class, 'add-group-popup')]";
		protected const string SAVE_NEW_GROUP_BTN_XPATH = "//div[contains(@class, 'g-popupbox__ft')]//a[contains(string(),'Create Group')]";
		protected const string EDIT_GROUP_BTN_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//div[contains(@data-bind, 'click: edit')]//a";
		protected const string ADD_RIGHTS_BTN_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//div[contains(@data-bind,'click: addAccessRight')]";
		protected const string GROUP_USER_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//ul[contains(@data-bind, 'foreach: users')]//li[contains(string(), '*##*')]";
		protected const string DELETE_USER_BUTTON = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//ul[contains(@data-bind, 'foreach: users')]//li[contains(string(), '*##*')]//span[contains(@data-bind,'removeUser')]";
		protected const string ADD_GROUP_USERS_INPUT_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//input[contains(@class, 'tblgrp_finduser')]";
		protected const string ADD_GROUP_USER_BTN_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//div[contains(@class, 'users-add-list')]//table//tr[contains(string(),'*##*')]//a[string() = 'Add']";
		
		protected const string VIEW_PROJECTS_RIGHT_TEXT_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//li[contains(string(), 'View all projects')]";
		protected const string MANAGE_PROJECTS_RIGHT_TEXT_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//li[contains(string(), 'Manage all projects')]";
		protected const string CREATE_PROJECTS_RIGHT_TEXT_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//li[contains(string(), 'Create any projects')]";
		protected const string SAVE_BTN_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//div[contains(@data-bind,'click: save')]//a";

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

		#endregion
	}
}
