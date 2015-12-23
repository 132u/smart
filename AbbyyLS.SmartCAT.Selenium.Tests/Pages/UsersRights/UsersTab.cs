﻿using System;
using System.Collections.Generic;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class UsersTab : UsersAndRightsBasePage, IAbstractPage<UsersTab>
	{
		public UsersTab(WebDriver driver) : base(driver)
		{
		}

		public new UsersTab GetPage()
		{
			var usersTab = new UsersTab(Driver);
			InitPage(usersTab, Driver);

			return usersTab;
		}

		public new void LoadPage()
		{
			if (!IsUsersTabOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось открыть диалог добавления права.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку сортировки по имени
		/// </summary>
		public UsersTab ClickSortByFirstName()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по имени.");
			SortByFirstName.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки фамилии
		/// </summary>
		public UsersTab ClickSortByLastName()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки фамилии");
			SortByLastName.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по ShortName
		/// </summary>
		public UsersTab ClickSortByShortName()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по ShortName");
			SortByShortName.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по адресу почты
		/// </summary>
		public UsersTab ClickSortByEmailAddress()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по адресу почты");
			SortByEmail.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по группе
		/// </summary>
		public UsersTab ClickSortByGroups()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по группе");
			SortByGroups.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате создания
		/// </summary>
		public UsersTab ClickSortByCreated()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по дате создания");
			SortByCreated.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по статусу
		/// </summary>
		public UsersTab ClickSortByStatus()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по статусу");
			SortByStatus.Click();

			return GetPage();
		}
		
		#endregion
		
		#region Составные методы страницы


		#endregion
		
		#region Методы, проверяющие состояние страницы
		
		/// <summary>
		/// Проверить, что пользователь есть в списке
		/// </summary>
		/// <param name="username">имя пользователя</param>
		public bool IsUserExistInList(string username)
		{
			CustomTestContext.WriteLine("Проверить, что пользователь есть в списке");

			return GetUserNameList().Contains(username);
		}

		/// <summary>
		/// Проверить, что открылась вкладка Пользователи
		/// </summary>
		public bool IsUsersTabOpened()
		{
			return UserSurnameList.Displayed;
		}

		#endregion

		#region Вспомогательные методы

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
		
		#endregion
		
		#region Объявление элементов страницы
		
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

		[FindsBy(How = How.XPath, Using = USER_SURNAME_LIST)]
		protected IWebElement UserSurnameList { get; set; }

		#endregion
		
		#region Описание XPath элементов

		protected const string USER_SURNAME_LIST = ".//table[contains(@class, 'js-users')]//tr[contains(@class, 'js-users-trwork')]//td[contains(@class, 'js-user-surname')]/p";
		protected const string USER_NAME_LIST = ".//table[contains(@class, 'js-users')]//tr[contains(@class, 'js-users-trwork')]//td[contains(@class, 'js-user-name')]/p";

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