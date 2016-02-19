using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	/// <summary>
	/// Пользователи корпоративного аккаунта
	/// </summary>
	public class AdminEnterpriseAccountUsersPage : AdminLingvoProPage, IAbstractPage<AdminEnterpriseAccountUsersPage>
	{
		public AdminEnterpriseAccountUsersPage(WebDriver driver) : base(driver)
		{
		}

		public new AdminEnterpriseAccountUsersPage GetPage()
		{
			var adminEnterpriseAccountUsersPage = new AdminEnterpriseAccountUsersPage(Driver);
			InitPage(adminEnterpriseAccountUsersPage, Driver);

			return adminEnterpriseAccountUsersPage;
		}

		public new void LoadPage()
		{
			if (!IsAdminEnterpriseAccountUsersPageOpened())
			{
				throw new Exception
					("Произошла ошибка:\n не загружена страница со списком пользователей корпоративного аккаунта.");
			}
		}

		#region Простые методы

		/// <summary>
		/// Ввести email пользователя в строку для поиска
		/// </summary>
		/// <param name="userEmail">email пользователя</param>
		public AdminEnterpriseAccountUsersPage SetEmailToFindUserInput(string userEmail)
		{
			CustomTestContext.WriteLine("Ввести email пользователя {0} в строку для поиска.", userEmail);
			FindUserInput.SetText(userEmail);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Найти" (пользователя)
		/// </summary>
		public AdminEnterpriseAccountUsersPage ClickFindUserButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Найти'.(пользователя)");
			FindUserButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Ввести фамилию пользователя
		/// </summary>
		/// <param name="userEmail">email пользователя</param>
		/// <param name="surname">фамилия</param>
		public AdminEnterpriseAccountUsersPage SetUserSurname(string userEmail, string surname)
		{
			CustomTestContext.WriteLine("Ввести фамилию {0} нового пользователя.", surname);
			FoundUserSurnameInput = Driver.SetDynamicValue(How.XPath, FOUND_USER_SURNAME_INPUT_XPATH, userEmail);
			FoundUserSurnameInput.SetText(surname);

			return GetPage();
		}

		/// <summary>
		/// Ввести имя пользователя
		/// </summary>
		/// <param name="userEmail">email пользователя</param>
		/// <param name="name">имя</param>
		public AdminEnterpriseAccountUsersPage SetUserName(string userEmail, string name)
		{
			CustomTestContext.WriteLine("Ввести имя {0} нового пользователя.", name);
			FoundUserNameInput = Driver.SetDynamicValue(How.XPath, FOUND_USER_NAME_INPUT_XPATH, userEmail);
			FoundUserNameInput.SetText(name);

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Добавить" (администратора в аккаунт)
		/// </summary>
		public AdminEnterpriseAccountUsersPage ClickAddUserButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Добавить' (администратора в аккаунт).");
			AddUserButton.Click();

			return GetPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Добавить пользователя
		/// </summary>
		/// <param name="userEmail">email</param>
		/// <param name="userSurname">фамилия</param>
		/// <param name="userName">имя</param>
		public AdminEnterpriseAccountUsersPage AddUser(
			string userEmail,
			string userSurname,
			string userName)
		{
			SetUserSurname(userEmail, userSurname);
			SetUserName(userEmail, userName);
			ClickAddUserButton();

			return GetPage();
		}

		/// <summary>
		/// Найти пользователя
		/// </summary>
		/// <param name="login">login</param>
		public AdminEnterpriseAccountUsersPage FindUser(string login)
		{
			SetEmailToFindUserInput(login);
			ClickFindUserButton();

			return GetPage();
		}

		/// <summary>
		/// Добавить существующего пользователя в аккаунт, если он еще туда не добавлен
		/// </summary>
		/// <param name="userEmail">email</param>
		/// <param name="userSurname">фамилия</param>
		/// <param name="userName">имя</param>
		public AdminEnterpriseAccountUsersPage AddExistedUserToAccountIfNotAdded(
			string userEmail,
			string userSurname,
			string userName)
		{
			if (!IsUserAddedIntoAccount(userEmail))
			{
				FindUser(userEmail);

				if (!IsUserFound(userEmail))
				{
					throw new Exception(
						string.Format("Ошибка: не удалось найти пользователя {0}", userEmail));
				}

				AddUser(userEmail, userSurname, userName);
			}

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница со списком пользователей корпоративного аккаунта
		/// </summary>
		public bool IsAdminEnterpriseAccountUsersPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.Id(FIND_USER_INPUT_ID));
		}

		/// <summary>
		/// Проверить, добавлен ли пользователь в аккаунт
		/// </summary>
		/// <param name="userEmail">логин пользователя</param>
		public bool IsUserAddedIntoAccount(string userEmail)
		{
			CustomTestContext.WriteLine("Проверить, добавлен ли пользователь {0} в аккаунт.", userEmail);

			return Driver.ElementIsDisplayed(By.XPath(ADDED_ACCOUNT_USERS_XPATH.Replace("*#*", userEmail)));
		}

		/// <summary>
		/// Проверить, появился ли запрашиваемый пользователь в списке найденных
		/// </summary>
		/// <param name="userEmail">email пользователя</param>
		public bool IsUserFound(string userEmail)
		{
			CustomTestContext.WriteLine(
				"Проверить, появился ли запрашиваемый пользователь {0} в списке найденных", userEmail);

			return Driver.WaitUntilElementIsDisplay
				(By.XPath(FOUND_USER_SURNAME_INPUT_XPATH.Replace("*#*", userEmail)));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(Using = FIND_USER_INPUT_ID)]
		protected IWebElement FindUserInput { get; set; }

		[FindsBy(Using = FIND_USER_BTN_ID)]
		protected IWebElement FindUserButton { get; set; }

		[FindsBy(Using = ADD_USER_BTN_ID)]
		protected IWebElement AddUserButton { get; set; }

		protected IWebElement FoundUserSurnameInput { get; set; }

		protected IWebElement FoundUserNameInput { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string ADDED_ACCOUNT_USERS_XPATH = "//table//tr[contains(string(), '*#*')]";
		protected const string FIND_USER_INPUT_ID = "searchText";
		protected const string FIND_USER_BTN_ID = "findUser";
		protected const string FOUND_USER_SURNAME_INPUT_XPATH = "//table//tbody[@id = 'usersTable']//tr[contains(string(), '*#*')]//input[@name = 'surnames']";
		protected const string FOUND_USER_NAME_INPUT_XPATH = "//table//tbody[@id = 'usersTable']//tr[contains(string(), '*#*')]//input[@name = 'names']";
		protected const string ADD_USER_BTN_ID = "addUsersBtn";

		#endregion
	}
}
