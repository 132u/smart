using System;
using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class GroupsAndAccessRightsTab : UsersAndRightsBasePage, IAbstractPage<GroupsAndAccessRightsTab>
	{
		public GroupsAndAccessRightsTab(WebDriver driver) : base(driver)
		{
		}

		public new GroupsAndAccessRightsTab LoadPage()
		{
			if (!IsGroupsAndAccessRightsTabOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось открыть вкладку Группы и права.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку "Создать группу"
		/// </summary>
		public NewGroupDialog ClickCreateGroupButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Создать группу'.");
			CreateGroupButton.Click();

			return new NewGroupDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Кликнуть на нужную группу в таблице
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public GroupsAndAccessRightsTab ClickGroupRow(string groupName)
		{
			CustomTestContext.WriteLine("Кликнуть на группу {0} в таблице, чтобы появился выпадающий список с пользователями и правами.", groupName);
			Group = Driver.SetDynamicValue(How.XPath, GROUP, groupName);
			Group.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку "Редактировать группу"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public GroupsAndAccessRightsTab ClickEditGroupButton(string groupName)
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Редактировать группу'.");
			var editGroupButtonXpathReplaced = EDIT_GROUP_BUTTON.Replace("*#*", groupName);
			if (Driver.WaitUntilElementIsDisplay(By.XPath(editGroupButtonXpathReplaced)))
			{
				EditGroupButton = Driver.FindElement(By.XPath(editGroupButtonXpathReplaced));
				EditGroupButton.ScrollAndClick();
			}
			else
			{
				throw new Exception("Кнопка 'Редактировать кнопку' не стала видимой.");
			}

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку "Сохранить"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public GroupsAndAccessRightsTab ClickSaveButton(string groupName)
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Сохранить' ( настройки группы {0}).", groupName);
			SaveButton = Driver.SetDynamicValue(How.XPath, SAVE_BUTTON, groupName);
			SaveButton.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на кнопку удаления пользователя из группы(крестик)
		/// </summary>
		/// <param name="groupName">имя группы</param>
		/// <param name="userName">имя пользователя</param>
		public GroupsAndAccessRightsTab ClickDeleteUserButton(string groupName, string userName)
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку удаления юзера {0} из группы {1}", userName, groupName);
			DeleteUserButton = Driver.SetDynamicValue(How.XPath, DELETE_USER_BUTTON, groupName, userName);
			DeleteUserButton.ScrollAndClick();

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы
		/// <summary>
		/// Открыть диалог добавления прав
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public AddAccessRightDialog OpenAddRightsDialogForGroup(string groupName)
		{
			if (!IsGroupInfoOpened(groupName))
			{
				ClickGroupRow(groupName);
			}

			if (IsEditGroupButtonDisplayed(groupName))
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
		public NewGroupDialog OpenNewGroupDialog()
		{
			OpenHideMenuIfClosed();
			ClickUsersRightsButton();
			ClickGroupsButton();
			ClickCreateGroupButton();
			
			return new NewGroupDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Добавить пользователя в группу, если он еще не добавлен (выполняется проверка)
		/// </summary>
		/// <param name="groupName">имя группы</param>
		/// <param name="userName">имя пользователя</param>
		public GroupsAndAccessRightsTab AddUserToGroupIfNotAlredyAdded(string groupName, string userName)
		{
			if (!IsGroupInfoOpened(groupName))
			{
				ClickGroupRow(groupName);
			}

			if (IsEditGroupButtonDisplayed(groupName))
			{
				ClickEditGroupButton(groupName);
			}
			
			if (!IsUserExistInGroup(groupName, userName))
			{
				ClickAddUsersSearchbox(groupName);
				ClickAddGroupUserButton(groupName, userName);
				ClickSaveButton(groupName);
			}

			return LoadPage();
		}

		/// <summary>
		/// Удалить пользователя из всех групп
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public GroupsAndAccessRightsTab RemoveUserFromAllGroups(string userName)
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

			return LoadPage();
		}


		/// <summary>
		/// Нажать кнопку "Добавить права"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public AddAccessRightDialog ClickAddRightsButton(string groupName)
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Добавить права' для группы {0}.", groupName);
			Driver.WaitUntilElementIsDisplay(By.XPath(ADD_RIGHTS_BUTTON.Replace("*#*", groupName)));
			AddRightsButton = Driver.SetDynamicValue(How.XPath, ADD_RIGHTS_BUTTON, groupName);
			AddRightsButton.Click();

			return new AddAccessRightDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Кликнуть по строке поиска пользователей для появления выпадающего списка с пользователями аккаунта
		/// </summary>
		public GroupsAndAccessRightsTab ClickAddUsersSearchbox(string groupName)
		{
			CustomTestContext.WriteLine("Кликнуть по строке поиска пользователей для появления выпадающего списка с пользователями аккаунта.");
			AddGroupUsersInput = Driver.SetDynamicValue(How.XPath, ADD_GROUP_USERS_INPUT, groupName);
			AddGroupUsersInput.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать пользователя из списка и нажать кнопку "Добавить"
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <param name="groupName">имя группы</param>
		public GroupsAndAccessRightsTab ClickAddGroupUserButton(string groupName, string userName)
		{
			CustomTestContext.WriteLine("Выбрать пользователя {0} из списка и нажать кнопку 'Добавить' в группу {1}.", userName, groupName);
			AddGroupUserButton = Driver.SetDynamicValue(How.XPath, ADD_GROUP_USER_BUTTON, groupName, userName);
			AddGroupUserButton.Click();

			return LoadPage();
		}
		
		#endregion
		
		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// проверить, что открылась вкладка "Группы и права"
		/// </summary>
		public bool IsGroupsAndAccessRightsTabOpened()
		{
			return IsDialogBackgroundDisappeared() &&
				Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_GROUP_BUTTON));
		}

		/// <summary>
		/// Проверить, есть ли пользователь в группе
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <param name="groupName">имя группы</param>
		public bool IsUserExistInGroup(string groupName, string userName)
		{
			CustomTestContext.WriteLine("Проверить, есть ли пользователь {0} в группе {1}.", userName, groupName);
			
			return Driver.GetIsElementExist(By.XPath(GROUP_USER.Replace("*#*", groupName).Replace("*##*", userName)));
		}
		
		/// <summary>
		/// Проверить, существует ли группа
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsGroupExists(string groupName)
		{
			CustomTestContext.WriteLine("Проверить, существует ли группа {0}.", groupName);

			return Driver.ElementIsDisplayed(By.XPath(GROUP.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Проверить, присуствует ли кнопка "Редактировать группу"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsEditGroupButtonDisplayed(string groupName)
		{
			CustomTestContext.WriteLine("Проверить, доступна ли кнопка 'Редактировать группу'");

			return Driver.WaitUntilElementIsDisplay(By.XPath(EDIT_GROUP_BUTTON.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Проверить, удалось ли добавить группе право на управление проектами
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsManageProjectsRightAdded(string groupName)
		{
			CustomTestContext.WriteLine("Проверить, удалось ли добавить группе право на управление проектами");

			return  Driver.ElementIsDisplayed(By.XPath(MANAGE_PROJECTS_RIGHT_TEXT.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Проверить, удалось ли добавить группе право на просмотр проектов
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsViewProjectsRightAdded(string groupName)
		{
			CustomTestContext.WriteLine("Проверить, удалось ли добавить группе право на просмотр проектов");

			return Driver.SetDynamicValue(How.XPath, VIEW_PROJECTS_RIGHT_TEXT, groupName).Displayed;
		}

		/// <summary>
		/// Проверить, удалось ли добавить группе право на создание проектов
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsCreateProjectsRightAdded(string groupName)
		{
			CustomTestContext.WriteLine("Проверить, удалось ли добавить группе право на создание проектов");

			return  Driver.SetDynamicValue(How.XPath, CREATE_PROJECTS_RIGHT_TEXT, groupName).Displayed;
		}

		/// <summary>
		/// Проверить,  что свертка группы раскрыта
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsGroupInfoOpened(string groupName)
		{
			CustomTestContext.WriteLine("Проверить, что свертка группы {0} раскрыта", groupName);

			return Driver.SetDynamicValue(How.XPath, GROUP_ROW, groupName)
				.GetAttribute("class")
				.Contains("opened");
		}

		#endregion

		#region Вспомогательные методы

		public string GetGroupUniqueName()
		{
			return "GroupTest - " + Guid.NewGuid();

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

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CREATE_GROUP_BUTTON)]
		protected IWebElement CreateGroupButton { get; set; }
		
		[FindsBy(How = How.XPath, Using = NEW_GROUP_NAME_INPUT)]
		protected IWebElement NewGroupNameInput { get; set; }
		
		protected IWebElement SaveButton { get; set; }

		protected IWebElement EditGroupButton { get; set; }

		protected IWebElement Group { get; set; }

		protected IWebElement AddRightsButton { get; set; }

		protected IWebElement AddGroupUsersInput { get; set; }

		protected IWebElement AddGroupUserButton { get; set; }

		protected IWebElement DeleteUserButton { get; set; }

		protected IWebElement GroupUser { get; set; }

		protected IWebElement ManageProjectsRightText { get; set; }

		protected IWebElement ViewProjectsRightText { get; set; }

		protected IWebElement CreateProjectsRightText { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string GROUP_NAME_LIST = "//tbody[@data-bind='foreach: filteredGroups']//tr[contains(@class, 'clickable')]//td[@data-bind='text: name']";
		protected const string GROUP = "//td[contains(@data-bind, 'text: name')][string()='*#*']";
		protected const string GROUP_ROW = "//td[contains(@data-bind, 'text: name')][string()='*#*']//..";
		protected const string CREATE_GROUP_BUTTON = "//div[contains(@data-bind, 'click: addGroup')]//a[contains(string(),'Create Group')]";
		protected const string NEW_GROUP_NAME_INPUT = "//input[contains(@class, 'add-group-popup')]";
		protected const string EDIT_GROUP_BUTTON = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//div[contains(@data-bind, 'click: edit')]//a";
		protected const string ADD_RIGHTS_BUTTON = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//div[contains(@data-bind,'click: addAccessRight')]";
		protected const string GROUP_USER = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//ul[contains(@data-bind, 'foreach: users')]//li//span[text()='*##*']";
		protected const string DELETE_USER_BUTTON = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//ul[contains(@data-bind, 'foreach: users')]//li//span[text()='*##*']/..//span[contains(@data-bind,'removeUser')]";
		protected const string ADD_GROUP_USERS_INPUT = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')][1]//input[contains(@class, 'tblgrp_finduser')]";
		protected const string ADD_GROUP_USER_BUTTON = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//div[contains(@class, 'users-add-list')]//table//tr//td[string()='*##*']//following-sibling::td//a[text() = 'Add']";

		protected const string VIEW_PROJECTS_RIGHT_TEXT = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//li[contains(string(), 'View all projects')]";
		protected const string MANAGE_PROJECTS_RIGHT_TEXT = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//li[contains(string(), 'Manage all projects')]";
		protected const string CREATE_PROJECTS_RIGHT_TEXT = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//li[contains(string(), 'Create any projects')]";
		protected const string SAVE_BUTTON = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//div[contains(@data-bind,'click: save')]//a";
		
		#endregion
	}
}
