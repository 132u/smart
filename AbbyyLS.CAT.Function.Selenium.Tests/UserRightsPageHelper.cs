﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class UserRightsPageHelper : CommonHelper
    {
        public UserRightsPageHelper(IWebDriver driver, WebDriverWait wait) :
            base(driver, wait)
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
			if (!WaitUntilDisplayElement(By.XPath(USERS_RIGHTS_TABLE_XPATH)))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Ожидание ожидание загрузки страницы прав групп пользователей
		/// </summary>
		/// <returns>Страница загрузилась</returns>
		public bool WaitUntilGroupsRightsDisplay()
		{
			// Ожидаем пока загрузится страница
			if (!WaitUntilDisplayElement(By.XPath(ADMIN_GROUP_XPATH)))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Возвращает список имен пользователей
		/// </summary>
		/// <returns>Список имен пользователей</returns>
		public List<string> GetUserFullnameList()
		{
			List<string> fullNameList = new List<string>();
			List<string> nameList = GetTextListElement(By.XPath(USERS_XPATH + USER_NAME_XPATH));
			List<string> surnameList = GetTextListElement(By.XPath(USERS_XPATH + USER_SURNAME_XPATH));

			for (int i = 0; i < nameList.Count; i++)
			{
				fullNameList.Add((nameList[i] + " " + surnameList[i]).Trim());
			}

			return fullNameList;
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
			if (!WaitUntilDisplayElement(By.XPath(CREATE_GROUP_FORM_XPATH)))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Ожидание закрытия формы создания группы
		/// </summary>
		/// <returns>Форма закрылась</returns>
		public bool WaitUntilCreateFormDisappear()
		{
			// Ожидаем пока загрузится страница
			if (!WaitUntilDisappearElement(By.XPath(CREATE_GROUP_FORM_XPATH), 15))
			{
				return false;
			}
			return true;
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
			ClickElement(By.XPath(GROUPS_XPATH + GROUP_NAME_XPATH + "[contains(text(), '" + groupName + "')]"));
		}

		/// <summary>
		/// Возвращает список пользователей в открытой группе
		/// </summary>
		/// <returns>Список пользователей</returns>
		public List<string> GetDisplayUsersInGroup()
		{
			List<string> usersInGroup = new List<string>();

			string xPath = GROUPS_RIGHTS_TABLE_XPATH + GROUP_USERS_XPATH;

			IList<IWebElement> users = GetElementList(By.XPath(xPath));

			foreach (IWebElement user in users)
			{
				if (user.Displayed && user.Text != String.Empty)
					usersInGroup.Add(user.Text);
			}

			return usersInGroup;
		}



        protected const string PAGE_LINK_XPATH = "//a[contains(@href,'/Enterprise/Users')]";
        protected const string GROUP_LINK_XPATH = "//a[contains(@href,'/Enterprise/Groups')]";
        protected const string ADMIN_GROUP_XPATH = "//td[contains(@class,'js-group-name')][text()='Administrators']";
        protected const string EDIT_BTN_XPATH = "//span[contains(@class,'js-editgroup-btn')]";
        protected const string ADD_RIGHTS_BTN_XPATH = "//span[contains(@class,'js-add-right-btn')]";
        protected const string SUGGEST_WITHOUT_GLOSSARY_INPUT_XPATH = "//li[@data-type='AddSuggestsWithoutGlossary']//input";
        protected const string GLOSSARY_SEARCH_INPUT_XPATH = "//li[@data-type='GlossarySearch']//input";
        protected const string NEXT_BTN_XPATH = "//span[contains(@class,'js-next')]";
        protected const string ADD_BTN_XPATH = "//span[contains(@class,'js-add')]//a[contains(text(),'Add')]";
        protected const string ALL_GLOSSARIES_SELECT_XPATH = "//div[contains(@class,'js-scope-section')][2]//input[contains(@name,'accessRightScopeType')]";
        protected const string SAVE_BTN_XPATH = "//span[contains(@class,'js-save-btn')]";

		protected const string CREATE_GROUP_BTN_XPATH = "//span[contains(@class,'js-addgroup-btn')]//a[contains(text(),'Create group')]";
		protected const string CREATE_GROUP_FORM_XPATH = "//form[contains(@action,'/Enterprise/Groups/Add')]";
		protected const string NAME_NEW_GROUP_XPATH = CREATE_GROUP_FORM_XPATH + "//label/input";
		protected const string CANCEL_NEW_GROUP_XPATH = CREATE_GROUP_FORM_XPATH + "//a[contains(@class,'js-popup')]";
		protected const string SAVE_NEW_GROUP_XPATH = CREATE_GROUP_FORM_XPATH + "//input[contains(@value,'Create group')]";
		protected const string ERROR_NEW_GROUP_XPATH = CREATE_GROUP_FORM_XPATH + "//div[contains(@class,'js-error-message')]";

		protected const string USERS_RIGHTS_TABLE_XPATH = ".//table[contains(@class, 'js-users')]";
		protected const string USERS_XPATH = USERS_RIGHTS_TABLE_XPATH + "//tr[contains(@class, 'js-users-trwork')]";
		protected const string USER_SURNAME_XPATH = "//td[contains(@class, 'js-user-surname')]/p";
		protected const string USER_NAME_XPATH = "//td[contains(@class, 'js-user-name')]/p";

		protected const string GROUPS_RIGHTS_TABLE_XPATH = ".//table[contains(@class, 'js-groups')]";
		protected const string GROUPS_XPATH = GROUPS_RIGHTS_TABLE_XPATH + "//tr[contains(@class, 'js-group-row')]";
		protected const string GROUP_NAME_XPATH = "//td[contains(@class, 'js-group-name')]";
		protected const string GROUP_USERS_XPATH = "//ul[contains(@class, 'js-users-list')]//span[contains(@class, 'js-user-name')]";
    }
}