﻿using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace
{
	public class UsersRightsPage : WorkspacePage, IAbstractPage<UsersRightsPage>
	{
		public new UsersRightsPage GetPage()
		{
			var usersRightsPage = new UsersRightsPage();
			InitPage(usersRightsPage);
			LoadPage();
			return usersRightsPage;
		}

		public new void LoadPage()
		{
			if (!Driver.ElementIsPresent(By.XPath(GROUPS_RIGHTS_BTN_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти на вкладку 'Пользователи и права'.");
			}
		}

		/// <summary>
		/// Перейти на вкладку "Группы и права"
		/// </summary>
		public UsersRightsPage ClickGroupsBtn()
		{
			Logger.Trace("Переходим на вкладку 'Группы и права'.");
			GroupsBtn.Click();
			return GetPage();
		}

		/// <summary>
		/// Проверить, существует ли группа
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsGroupExists(string groupName)
		{
			Logger.Trace("Проверяем существует ли группа {0}.", groupName);
			return Driver.ElementIsPresent(By.XPath(GROUP_XPATH.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Нажать кнопку "Создать группу"
		/// </summary>
		public UsersRightsPage ClickCreateGroupBtn()
		{
			Logger.Trace("Нажимаем на кнопку 'Создать группу'.");
			CreateGroupBtn.Click();
			return GetPage();
		}

		/// <summary>
		/// Проверяем, появилась ли форма для добавления новой группы
		/// </summary>
		public UsersRightsPage AssertAddNewGroupForm()
		{
			Logger.Trace("Проверяем, появилась ли форма для добавления новой группы.");
			Assert.IsTrue(Driver.WaitUntilElementIsPresent(By.XPath(NEW_GROUP_NAME_INPUT_XPATH)),
				"Произошла ошибка:\n не появилась форма для добавления новой группы.");
			return GetPage();
		}

		/// <summary>
		/// Ввести имя создаваемой группы
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage SetNewGroupName(string groupName)
		{
			Logger.Trace("Вводим имя создаваемой группы: {0}.", groupName);
			NewGroupNameInput.SetText(groupName);
			return GetPage();
		}

		/// <summary>
		/// Сохранить новую группу
		/// </summary>
		public UsersRightsPage ClickSaveNewGroupBtn()
		{
			Logger.Trace("Сохраняем новую группу.");
			SaveNewGroupBtn.Click();
			return GetPage();
		}

		/// <summary>
		/// Проверка, создалась ли новая группа
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage AssertIsGroupCreated(string groupName)
		{
			Assert.IsTrue(Driver.WaitUntilElementIsPresent(By.XPath(GROUP_XPATH.Replace("*#*", groupName))),
				"Произошла ошибка:\n не удалось создать группу " + groupName);
			return GetPage();
		}

		/// <summary>
		/// Кликаем на нужную группу в таблице
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage SelectGroup(string groupName)
		{
			Logger.Trace("Кликаем на группу {0} в таблице, чтобы появился выпадающий список с пользователями и правами.", groupName);
			Group = Driver.SetDynamicValue(How.XPath, GROUP_XPATH, groupName);
			Group.Click();
			return GetPage();
		}

		/// <summary>
		/// Проверка, присуствует ли кнопка "Редактировать группу"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsEditGroupBtnDisplayed(string groupName)
		{
			return Driver.ElementIsPresent(By.XPath(EDIT_GROUP_BTN_XPATH.Replace("*#*", groupName))) && Driver.ElementIsEnabled(By.XPath(EDIT_GROUP_BTN_XPATH.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Нажать кнопку "Редактировать группу"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage ClickEditGroupBtn(string groupName)
		{
			Logger.Trace("Нажимаем кнопку 'Редактировать группу'.");
			EditGroupBtn = Driver.SetDynamicValue(How.XPath, EDIT_GROUP_BTN_XPATH, groupName);
			EditGroupBtn.Click();
			return GetPage();
		}

		/// <summary>
		/// Проверка, есть ли пользователь в группе
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <param name="groupName">имя группы</param>
		public bool IsGroupUserAdded(string groupName, string userName)
		{
			Logger.Trace("Проверяем, есть ли пользователь {0} в группе {1}.", userName, groupName);
			return Driver.ElementIsPresent(By.XPath(GROUP_USER_XPATH.Replace("*#*", groupName).Replace("*##*", userName)));
		}

		/// <summary>
		/// Проверка, удалось ли добавить пользователя в группу
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage AssertIsGroupUserAdded(string groupName, string userName)
		{
			Assert.IsTrue(Driver.ElementIsPresent(By.XPath(GROUP_USER_XPATH.Replace("*#*", groupName).Replace("*##*", userName))),
				"Произошла ошибка:\n не удалось добавить пользователя в группу проектами");
			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Добавить права"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage ClickAddRightsBtn(string groupName)
		{
			Logger.Trace("Нажимаем на кнопку 'Добавить права' для группы {0}.", groupName);
			AddRightsBtn = Driver.SetDynamicValue(How.XPath, ADD_RIGHTS_BTN_XPATH, groupName);
			AddRightsBtn.Click();
			return GetPage();
		}

		/// <summary>
		/// Кликаем по строке поиска пользователей для появления выпадающего списка с пользователями аккаунта
		/// </summary>
		public UsersRightsPage ClickAddUsersSearchbox(string groupName)
		{
			Logger.Trace("Кликаем по строке поиска пользователей для появления выпадающего списка с пользователями аккаунта.");
			AddGroupUsersInput = Driver.SetDynamicValue(How.XPath, ADD_GROUP_USERS_INPUT_XPATH, groupName);
			AddGroupUsersInput.Click();
			return GetPage();
		}

		/// <summary>
		/// Проверка, что данный пользователь есть в списке с пользователями аккаунта
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage AssertIsAddGroupUserBtnExists(string groupName, string userName)
		{
			Assert.IsTrue(Driver.ElementIsPresent(By.XPath(ADD_GROUP_USER_BTN_XPATH.Replace("*#*", groupName).Replace("*##*", userName))),
				"Произошла ошибка:\n не появился выпадающий список с пользователями аккаунта, либо {0} нет в списке", userName);
			return GetPage();
		}

		/// <summary>
		/// Выбрать пользователя из списка и нажать кнопку "Добавить"
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage ClickAddGroupUserBtn(string groupName, string userName)
		{
			Logger.Trace("Выбираем пользователя {0} из списка и нажать кнопку 'Добавить' в группу {1}.", userName, groupName);
			AddGroupUserBtn = Driver.SetDynamicValue(How.XPath, ADD_GROUP_USER_BTN_XPATH, groupName, userName);
			AddGroupUserBtn.Click();
			return GetPage();
		}

		/// <summary>
		/// Проверка, есть у группы право на управление проектами
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsManageProjectsRightAdded(string groupName)
		{
			Logger.Trace("Проверяем, есть у группы {0} право на управление проектами.", groupName);
			return Driver.ElementIsPresent(By.XPath(MANAGE_PROJECTS_RIGHT_TEXT_XPATH.Replace("*#*", groupName)));
		}
		/// <summary>
		/// Проверка, удалось ли добавить группе право на управление проектами
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage AssertIsManageProjectsRightAdded(string groupName)
		{
			Assert.IsTrue(Driver.ElementIsPresent(By.XPath(MANAGE_PROJECTS_RIGHT_TEXT_XPATH.Replace("*#*", groupName))),
				"Произошла ошибка:\n не удалось добавить право на управление проектами ");
			return GetPage();
		}

		/// <summary>
		/// Проверка, есть у группы право на просмотр проектов
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsViewProjectsRightAdded(string groupName)
		{
			Logger.Trace("Проверяем, есть у группы {0} право на просмотр проектов.", groupName);
			return Driver.ElementIsPresent(By.XPath(VIEW_PROJECTS_RIGHT_TEXT_XPATH.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Проверка, удалось ли добавить группе право на просмотр проектов
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage AssertIsViewProjectsRightAdded(string groupName)
		{
			Assert.IsTrue(Driver.ElementIsPresent(By.XPath(VIEW_PROJECTS_RIGHT_TEXT_XPATH.Replace("*#*", groupName))),
				"Произошла ошибка:\n не удалось добавить право на просмотр проектов ");
			return GetPage();
		}

		/// <summary>
		/// Проверка, есть у группы право на создание проектов
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public bool IsCreateProjectsRightAdded(string groupName)
		{
			Logger.Trace("Проверяем, есть у группы {0} право на создание проектов.", groupName);
			return Driver.ElementIsPresent(By.XPath(CREATE_PROJECTS_RIGHT_TEXT_XPATH.Replace("*#*", groupName)));
		}

		/// <summary>
		/// Проверка, удалось ли добавить группе право на создание проектов
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage AssertIsCreateProjectsRightAdded(string groupName)
		{
			Assert.IsTrue(Driver.ElementIsPresent(By.XPath(CREATE_PROJECTS_RIGHT_TEXT_XPATH.Replace("*#*", groupName))),
				"Произошла ошибка:\n не удалось добавить право на создание проектов ");
			return GetPage();
		}

		/// <summary>
		/// Выбрать из списка право на создание проектов
		/// </summary>
		public UsersRightsPage ClickCreateProjectsRadio()
		{
			Logger.Trace("Выбираем из списка право на создание проектов.");
			CreateProjectRadio.Click();
			return GetPage();
		}

		/// <summary>
		/// Выбрать из списка право на управление проектами
		/// </summary>
		public UsersRightsPage ClickManageProjectsRadio()
		{
			Logger.Trace("Выбираем из списка право на управление проектами.");
			ManageProjectRadio.Click();
			return GetPage();
		}

		/// <summary>
		/// Выбрать из списка право на просмотр проектов
		/// </summary>
		public UsersRightsPage ClickViewProjectsRadio()
		{
			Logger.Trace("Выбираем из списка право на просмотр проектов.");
			ViewProjectRadio.Click();
			return GetPage();
		}

		/// <summary>
		/// Выбрать набор для всех проектов
		/// </summary>
		public UsersRightsPage ClickDefinedByConditionRadio()
		{
			Logger.Trace("Выбираем набор 'для всех проектов'.");
			if (Driver.ElementIsPresent(By.XPath(FOR_ANY_PROJECT_RADIO_XPATH)) && Driver.ElementIsEnabled(By.XPath(FOR_ANY_PROJECT_RADIO_XPATH)))
			{
				ForAnyProjectRadio.Click();
			}
			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку далее (при добавлении прав пользователя)
		/// </summary>
		public UsersRightsPage ClickNextBtn()
		{
			Logger.Trace("Нажимаем кнопку далее (при добавлении прав пользователя).");
			NextBtn.Click();
			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Добавить" (право)
		/// </summary>
		public UsersRightsPage ClickAddRightBtn()
		{
			Logger.Trace("Нажимаем кнопку 'Добавить' (право).");
			AddRightBtn.Click();
			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Сохранить"
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsPage ClickSaveBtn(string groupName)
		{
			Logger.Trace("Нажимаем кнопку 'Сохранить' ( настройки группы {0}).", groupName);
			SaveBtn = Driver.SetDynamicValue(How.XPath, SAVE_BTN_XPATH, groupName);
			SaveBtn.Click();
			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = GROUPS_RIGHTS_BTN_XPATH)]
		protected IWebElement GroupsBtn { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_GROUP_BTN_XPATH)]
		protected IWebElement CreateGroupBtn { get; set; }

		[FindsBy(How = How.XPath, Using = NEW_GROUP_NAME_INPUT_XPATH)]
		protected IWebElement NewGroupNameInput { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_NEW_GROUP_BTN_XPATH)]
		protected IWebElement SaveNewGroupBtn { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_PROJECTS_RIGHT_RADIO_XPATH)]
		protected IWebElement CreateProjectRadio { get; set; }

		[FindsBy(How = How.XPath, Using = VIEW_PROJECTS_RIGHT_RADIO_XPATH)]
		protected IWebElement ViewProjectRadio { get; set; }

		[FindsBy(How = How.XPath, Using = MANAGE_PROJECTS_RIGHT_RADIO_XPATH)]
		protected IWebElement ManageProjectRadio { get; set; }

		[FindsBy(How = How.XPath, Using = FOR_ANY_PROJECT_RADIO_XPATH)]
		protected IWebElement ForAnyProjectRadio { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BTN_XPATH)]
		protected IWebElement NextBtn { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_RIGHT_BTN_XPATH)]
		protected IWebElement AddRightBtn { get; set; }

		protected IWebElement SaveBtn { get; set; }

		protected IWebElement EditGroupBtn { get; set; }

		protected IWebElement Group { get; set; }

		protected IWebElement AddRightsBtn { get; set; }

		protected IWebElement AddGroupUsersInput { get; set; }

		protected IWebElement AddGroupUserBtn { get; set; }

		protected const string GROUPS_RIGHTS_BTN_XPATH = "//a[contains(@href,'/Groups/Index')]";
		protected const string GROUP_XPATH = "//td[contains(@data-bind, 'text: name')][string()='*#*']";
		protected const string CREATE_GROUP_BTN_XPATH = "//span[contains(@data-bind, 'click: addGroup')]//a[contains(string(),'Create Group')]";
		protected const string NEW_GROUP_NAME_INPUT_XPATH = "//input[contains(@class, 'add-group-popup')]";
		protected const string SAVE_NEW_GROUP_BTN_XPATH = "//div[contains(@class, 'g-popupbox__ft')]//a[contains(string(),'Create Group')]";
		protected const string EDIT_GROUP_BTN_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//span[contains(@data-bind, 'click: edit')]//a";
		protected const string ADD_RIGHTS_BTN_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//span[contains(@data-bind,'click: addAccessRight')]";
		protected const string GROUP_USER_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//ul[contains(@data-bind, 'foreach: users')]//li[contains(string(), '*##*')]";
		protected const string ADD_GROUP_USERS_INPUT_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//input[contains(@class, 'tblgrp_finduser')]";
		protected const string ADD_GROUP_USER_BTN_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//div[contains(@class, 'users-add-list')]//table//tr[contains(string(),'*##*')]//a[string() = 'Add']";
		protected const string CREATE_PROJECTS_RIGHT_RADIO_XPATH = "//li[contains(@class, 'add-access-right') and contains(string(), 'Create projects')]//input";
		protected const string VIEW_PROJECTS_RIGHT_RADIO_XPATH = "//li[contains(@class, 'add-access-right') and contains(string(), 'View projects')]//input";
		protected const string MANAGE_PROJECTS_RIGHT_RADIO_XPATH = "//li[contains(@class, 'add-access-right') and contains(string(), 'Manage projects')]//input";
		protected const string FOR_ANY_PROJECT_RADIO_XPATH = "//label[contains(string(), 'For any projects')]//input";
		protected const string NEXT_BTN_XPATH = "//div[contains(@class, 'add-access-right-popup')][2]//span[contains(@data-bind, 'click : moveToNextStep')]//a[string() = 'Next']";
		protected const string ADD_RIGHT_BTN_XPATH = "//div[contains(@class, 'add-access-right-popup')][2]//span[contains(@data-bind, 'visible : canFinishWizard, click : finishWizard')]//a[string() = 'Add']";
		protected const string VIEW_PROJECTS_RIGHT_TEXT_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//li[contains(string(), 'View all projects')]";
		protected const string MANAGE_PROJECTS_RIGHT_TEXT_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//li[contains(string(), 'Manage all projects')]";
		protected const string CREATE_PROJECTS_RIGHT_TEXT_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//li[contains(string(), 'Create any projects')]";
		protected const string SAVE_BTN_XPATH = "//tr[contains(string(),'*#*')]//following-sibling::tr[contains(@data-bind, 'if: isExpanded')]//span[contains(@data-bind,'click: save')]//a";
	}
}
