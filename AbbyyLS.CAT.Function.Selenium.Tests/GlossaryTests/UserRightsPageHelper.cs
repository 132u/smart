using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер для страницы прав пользователя
	/// </summary>
	public class UserRightsPageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public UserRightsPageHelper(IWebDriver driver, WebDriverWait wait)
			: base(driver, wait)
		{
		}

		/// <summary>
		/// Открыть группы
		/// </summary>
		public void OpenGroups()
		{
			// TODO попробовать через link text
			ClickElement(By.XPath(GROUP_LINK_XPATH));
		}

		/// <summary>
		/// Выбрать Администраторы
		/// </summary>
		public void SelectAdmins()
		{
			ClickElement(By.XPath(ADMIN_GROUP_XPATH));
		}

		/// <summary>
		/// Нажать Edit
		/// </summary>
		public void ClickEdit()
		{
			ClickElement(By.XPath(EDIT_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Add Rights
		/// </summary>
		public void ClickAddRights()
		{
			ClickElement(By.XPath(ADD_RIGHTS_BTN_XPATH));
		}

		/// <summary>
		/// Добавить возможность предлагать без указания глоссария
		/// </summary>
		public void SelectSuggestWithoutGlossary()
		{
			ClickElement(By.XPath(SUGGEST_WITHOUT_GLOSSARY_INPUT_XPATH));
		}

		/// <summary>
		/// Добавить возможность поиска по глоссарию
		/// </summary>
		public void SelectGlossarySearch()
		{
			ClickElement(By.XPath(GLOSSARY_SEARCH_INPUT_XPATH));
		}

		/// <summary>
		/// Кликнуть Next
		/// </summary>
		public void ClickNext()
		{
			ClickElement(By.XPath(NEXT_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Add
		/// </summary>
		public void ClickAdd()
		{
			ClickElement(By.XPath(ADD_BTN_XPATH));
		}

		/// <summary>
		/// Выбрать Все глоссарии
		/// </summary>
		public void SelectAllGlossaries()
		{
			ClickElement(By.XPath(ALL_GLOSSARIES_SELECT_XPATH));
		}

		/// <summary>
		/// Выбрать Управление глоссариями
		/// </summary>
		public void SelectManageGlossaries()
		{
			ClickElement(By.XPath(MANAGE_GLOSSARIES_INPUT_XPATH));
		}

		/// <summary>
		/// Кликнуть Сохранить
		/// </summary>
		public void ClickSave()
		{
			ClickElement(By.XPath(SAVE_BTN_XPATH));
		}

		/// <summary>
		/// Ожидание ожидание загрузки страницы прав пользователя
		/// </summary>
		/// <returns>Страница загрузилась</returns>
		public bool WaitUntilUsersRightsDisplay()
		{
			// Ожидаем пока загрузится страница
			return WaitUntilDisplayElement(By.XPath(USERS_RIGHTS_TABLE_XPATH));
		}

		/// <summary>
		/// Ожидание ожидание загрузки страницы прав групп пользователей
		/// </summary>
		/// <returns>Страница загрузилась</returns>
		public bool WaitUntilGroupsRightsDisplay()
		{
			// Ожидаем пока загрузится страница
			return WaitUntilDisplayElement(By.XPath(ADMIN_GROUP_XPATH));
		}

		/// <summary>
		/// Возвращает список имен пользователей
		/// </summary>
		/// <returns>Список имен пользователей</returns>
		public List<string> GetUserFullnameList()
		{
			var nameList = GetTextListElement(By.XPath(USERS_XPATH + USER_NAME_XPATH));
			var surnameList = GetTextListElement(By.XPath(USERS_XPATH + USER_SURNAME_XPATH));

			return nameList.Select((t, i) => (t + " " + surnameList[i]).Trim()).ToList();
		}

		/// <summary>
		/// Возвращает список имен групп пользователей
		/// </summary>
		/// <returns>Список имен групп пользователей</returns>
		public List<string> GetGroupNameList()
		{
			return GetTextListElement(By.XPath(GROUPS_XPATH + GROUP_NAME_XPATH));
		}

		/// <summary>
		/// Кликнуть создать группу
		/// </summary>
		public void ClickCreateGroup()
		{
			ClickElement(By.XPath(CREATE_GROUP_BTN_XPATH));
		}

		/// <summary>
		/// Ожидание загрузки формы создания группы
		/// </summary>
		/// <returns>Форма открылась</returns>
		public bool WaitUntilCreateFormDisplay()
		{
			// Ожидаем пока загрузится страница
			return WaitUntilDisplayElement(By.XPath(CREATE_GROUP_FORM_XPATH));
		}

		/// <summary>
		/// Ожидание закрытия формы создания группы
		/// </summary>
		/// <returns>Форма закрылась</returns>
		public bool WaitUntilCreateFormDisappear()
		{
			// Ожидаем пока загрузится страница
			return WaitUntilDisappearElement(By.XPath(CREATE_GROUP_FORM_XPATH), 15);
		}

		/// <summary>
		/// Вводит заданное имя в поле наименования группы
		/// </summary>
		/// <param name="groupName">Имя новой группы</param>
		public void AddGroupName(string groupName)
		{
			ClickAndSendTextElement(By.XPath(NAME_NEW_GROUP_XPATH), groupName);
		}

		/// <summary>
		/// Кликнуть сохранить группу
		/// </summary>
		public void ClickSaveGroup()
		{
			ClickElement(By.XPath(SAVE_NEW_GROUP_XPATH));
		}

		/// <summary>
		/// Возвращает отображается ли сообщение об ошибке имени группы
		/// </summary>
		/// <returns>Сообщение об ошибке отображается</returns>
		public bool ErrorMessageDisplay()
		{
			return GetIsElementDisplay(By.XPath(ERROR_NEW_GROUP_XPATH));
		}

		/// <summary>
		/// Кликнуть группу по имени
		/// </summary>
		public void ClickGroupByName(string groupName)
		{
			ClickElement(By.XPath(GROUPS_XPATH + GROUP_NAME_XPATH + 
				"[contains(text(), '" + groupName + "')]"));
		}

		/// <summary>
		/// Возвращает список пользователей в открытой группе
		/// </summary>
		/// <returns>Список пользователей</returns>
		public List<string> GetDisplayUsersInGroup()
		{
			var usersInGroup = new List<string>();

			var xPath = GROUPS_RIGHTS_TABLE_XPATH + GROUP_USERS_XPATH;

			var users = GetElementList(By.XPath(xPath));

			foreach (var user in users)
			{
				if (user.Displayed && user.Text != String.Empty)
				{
					usersInGroup.Add(user.Text);
				}
			}

			return usersInGroup;
		}

		/// <summary>
		/// Вводим имя нового пользователя группы
		/// </summary>
		/// <param name="userName">Имя нового пользователя</param>
		public void AddUserToGroup(string userName)
		{
			ClearAndAddText(By.XPath(USER_NAME_INPUT_XPATH), userName);
		}

		/// <summary>
		/// Проверяем есть ли Управление всеми глоссариями в списке прав группы
		/// </summary>
		public bool IsManageAllGlossariesRightIsPresent()
		{
			return GetIsElementExist(By.XPath(MANAGE_ALL_GLOSSARIES_TEXT_XPATH));
		}

		/// <summary>
		/// Метод позвращает true, если пользователь в группе. Иначе false.
		/// </summary>
		public bool CheckUserExistsInGroup(string userName)
		{
			// Получаем список элементов, содержащих имена пользователей группы  в виде IList<IWebElement>
			var usersInGroup = GetElementList(By.XPath(USERS_IN_GROUP_NAME_XPATH));

			//Возвращаем true, если среди пользователей есть искомый. Иначе false
			return usersInGroup.Any(u => u.GetAttribute("innerHTML") == userName);
		}

		/// <summary>
		/// Метод осуществляет клик по кнопке редактирования группы
		/// </summary>
		public void EditGroupClick()
		{
			ClickElement(By.XPath(BUTTON_EDIT_GROUP));
		}

		/// <summary>
		/// Метод осуществляет клик по кнопке сохранения группы
		/// </summary>
		public void SaveGroupClick()
		{
			ClickElement(By.XPath(BUTTON_SAVE_GROUP));
		}

		/// <summary>
		/// Метод возвращает текущеее кол-во групп
		/// </summary>
		/// <returns>кол-во групп</returns>
		public int GetGroupsCount()
		{
			return GetElementList(By.XPath(GROUP_ROWS_XPATH)).Count;
		}

		/// <summary>
		/// Кликнуть группу по номеру
		/// </summary>
		/// <param name="number">Номер(сверху. Отчёт от 0)</param>
		public void GetGroupsCount(int number)
		{
			ClickElement(By.XPath(GROUP_ROWS_XPATH + "[" + number + "]"));
		}

		/// <summary>
		/// Метод ожидает появления кнопки Edit в открытой группе
		/// </summary>
		public bool WaitUntilDisplayEdit()
		{
			return WaitUntilDisplayElement(By.XPath(BUTTON_EDIT_GROUP));
		}

		/// <summary>
		/// Метод возвращает список пользователей в группе
		/// </summary>
		/// <returns>список пользователей</returns>
		public IList<IWebElement> GetUserInGroupList()
		{
			return GetElementList(By.XPath(USERS_IN_GROUP_NAME_XPATH));
		}

		/// <summary>
		/// Кликнуть кнопку удаления пользователя из группы по номеру
		/// </summary>
		/// <param name="number">Номер(сверху. Отчёт от 0)</param>
		public void DeleteUserFromGroupClick(int number)
		{
			ClickElement(By.XPath(USERS_IN_GROUP_XPATH + "[" + number + "]" + USERS_IN_GROUP_BUTTON_DELETE_XPATH));
		}

		/// <summary>
		/// Кликнуть по кнопке добавления прав для группы
		/// </summary>
		public void AddAccessRigthButtonClick()
		{
			ClickElement(By.XPath(ADD_ACCESS_RIGHT_BUTTON));
		}

		/// <summary>
		/// Кликнуть по radio button с правом на создание проектов 
		/// </summary>
		public void RightCreateProjectClick()
		{
			ClickElement(By.XPath(RIGHT_CREATE_PROJECTS_XPATH));
		}

		/// <summary>
		/// Кликнуть кнопку Next, чтобы перейти к следующему шагу назначения прав
		/// </summary>
		public void AddRightNextClick()
		{
			ClickElement(By.XPath(ADD_RIGHT_NEXT_BUTTON_POPUP));
		}

		/// <summary>
		/// Кликнуть по radio button с дающий выбранные права для всех проектов
		/// </summary>
		public void ForAnyRpojectsClick()
		{
			ClickElement(By.XPath(FOR_ANY_PROJECTS_RADIO));
		}

		/// <summary>
		/// Кликнуть кнопку добавления прав (Кнопка Add  - завершает диалог)
		/// </summary>
		public void AddRightDialogClick()
		{
			ClickElement(By.XPath(ADD_RIGHT_BUTTON_POPUP));
		}

		/// <summary>
		/// Кликнуть по полю поиска пользователей
		/// </summary>
		public void FindUsersClick()
		{
			ClickElement(By.XPath(FIND_USERS_INPUT));
		}

		/// <summary>
		/// Ввести имя пользователя в поле поиска пользователей для добавления в группу
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public void FindUsersSendText(string userName)
		{
			ClickAndSendTextElement(By.XPath(FIND_USERS_INPUT), userName);
		}

		/// <summary>
		/// Во всплывающем списке пользователей выбираем нужного по имени
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public void AddUserInGroupClick(string userName)
		{
			ClickElement(By.XPath(ADD_USER_IN_GRORUP_BUTTON + userName + "']//following-sibling::*"));
		}

		protected const string PAGE_LINK_XPATH = "//a[contains(@href,'/Users/Index')]";
		protected const string GROUP_LINK_XPATH = "//a[contains(@href,'/Groups/Index')]";
		protected const string ADMIN_GROUP_XPATH = "//td[@data-bind='text: name' and text()='Administrators']";
		protected const string EDIT_BTN_XPATH = "//span[contains(@class,'js-editgroup-btn')]";
		protected const string ADD_RIGHTS_BTN_XPATH = "//span[contains(@class,'js-add-right-btn')]";
		protected const string SUGGEST_WITHOUT_GLOSSARY_INPUT_XPATH = "//li[@data-type='AddSuggestsWithoutGlossary']//input";
		protected const string MANAGE_GLOSSARIES_INPUT_XPATH = "//li[@data-type='GlossaryManagement']//input";
		protected const string GLOSSARY_SEARCH_INPUT_XPATH = "//li[@data-type='GlossarySearch']//input";
		protected const string NEXT_BTN_XPATH = "//span[contains(@class,'js-next')]";
		protected const string ADD_BTN_XPATH = "//span[contains(@class,'js-add')]//a[contains(text(),'Add')]";
		protected const string ALL_GLOSSARIES_SELECT_XPATH = "//div[contains(@class,'js-scope-section')][2]//input[contains(@name,'accessRightScopeType')]";
		protected const string SAVE_BTN_XPATH = "//span[contains(@class,'js-save-btn')]";

		protected const string CREATE_GROUP_BTN_XPATH = "//span[@data-bind='click: addGroup']//a[contains(text(),'Create Group')]";
		protected const string CREATE_GROUP_FORM_XPATH = "//div[contains(@class,'l-add-group-popup')]";
		protected const string NAME_NEW_GROUP_XPATH = CREATE_GROUP_FORM_XPATH + "//label/input";
		protected const string CANCEL_NEW_GROUP_XPATH = CREATE_GROUP_FORM_XPATH + "//a[contains(@class,'js-popup-close')]";
		protected const string SAVE_NEW_GROUP_XPATH = CREATE_GROUP_FORM_XPATH + "//a[contains(text(),'Create Group')]";
		protected const string ERROR_NEW_GROUP_XPATH = CREATE_GROUP_FORM_XPATH + "//div[@data-bind='visible: nameAlreadyExists' and @style='']";

		protected const string USERS_RIGHTS_TABLE_XPATH = ".//table[contains(@class, 'js-users')]";
		protected const string USERS_XPATH = USERS_RIGHTS_TABLE_XPATH + "//tr[contains(@class, 'js-users-trwork')]";
		protected const string USER_SURNAME_XPATH = "//td[contains(@class, 'js-user-surname')]/p";
		protected const string USER_NAME_XPATH = "//td[contains(@class, 'js-user-name')]/p";

		protected const string GROUPS_RIGHTS_TABLE_XPATH = "//tbody[@data-bind='foreach: filteredGroups']";
		protected const string GROUPS_XPATH = GROUPS_RIGHTS_TABLE_XPATH + "//tr[contains(@class, 'clickable')]";
		protected const string GROUP_NAME_XPATH = "//td[@data-bind='text: name']";
		protected const string GROUP_USERS_XPATH = "//ul[@data-bind='foreach: users']//span[@data-bind='text: name')]";
		protected const string USER_NAME_INPUT_XPATH = "//div[contains(@class,'l-corpr__tblgrp_finduserwrp')]//input";
		protected const string MANAGE_ALL_GLOSSARIES_TEXT_XPATH = "//ul[@data-bind='foreach: accessRights']//span[contains(string(), 'Manage all glossaries')]";

		protected const string GROUP_ROWS_XPATH = "//tbody[contains(@data-bind,'filteredGroups')]//tr[contains(@class,'clickable')]";
		protected const string BUTTON_EDIT_GROUP = "//span[contains(@data-bind,'click: edit')]";
		protected const string USERS_IN_GROUP_XPATH = "//ul[contains(@data-bind,'users')]//li";
		protected const string USERS_IN_GROUP_NAME_XPATH = USERS_IN_GROUP_XPATH + "//span[2]";
		protected const string USERS_IN_GROUP_BUTTON_DELETE_XPATH = "//span[1]";
		protected const string BUTTON_SAVE_GROUP = "//span[contains(@data-bind,'click: save')]";
		protected const string ADD_ACCESS_RIGHT_BUTTON = "//span[contains(@data-bind,'addAccessRight')]";
		protected const string RIGHT_CREATE_PROJECTS_XPATH = "//li[contains(@class,'add-access-right-popup')]/label/span[text()='Create projects']";
		protected const string ADD_RIGHT_BUTTON_POPUP = "//div[contains(@class,'add-access-right')][2]//span[contains(@data-bind,'finishWizard')]";
		protected const string ADD_RIGHT_NEXT_BUTTON_POPUP = "//div[contains(@class,'add-access-right')][2]//span[contains(@data-bind,'moveToNextStep')]";
		protected const string FOR_ANY_PROJECTS_RADIO = "//div[contains(@class,'js-add-access-right-popup')][2]//span[contains(@data-bind, 'unrestricted')]";
		protected const string FIND_USERS_INPUT = "//input[contains(@class,'finduser')]";
		protected const string ADD_USER_IN_GRORUP_BUTTON = "//table[contains(@data-bind,'foundUsers')]//td[text()='";
	}
}