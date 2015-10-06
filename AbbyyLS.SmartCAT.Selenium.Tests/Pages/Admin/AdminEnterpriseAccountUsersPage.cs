using NUnit.Framework;
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
			if (!Driver.WaitUntilElementIsDisplay(By.Id(FIND_USER_INPUT_ID)))
			{
				Assert.Fail
					("Произошла ошибка:\n не загружена страничка AdminEnterpriseAccountUsersPage (Пользователи корпоративного аккаунта).");
			}
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
		/// Проверить, появился ли запрашиваемый пользователь в списке найденных
		/// </summary>
		/// <param name="userEmail">email пользователя</param>
		public AdminEnterpriseAccountUsersPage AssertUserFound(string userEmail)
		{
			Assert.IsTrue(Driver.WaitUntilElementIsDisplay
				(By.XPath(FOUND_USER_SURNAME_INPUT_XPATH.Replace("*#*", userEmail))),
				"Ошибка: не удалось найти пользователя." + userEmail);

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

		[FindsBy(Using = FIND_USER_INPUT_ID)]
		protected IWebElement FindUserInput { get; set; }

		[FindsBy(Using = FIND_USER_BTN_ID)]
		protected IWebElement FindUserButton { get; set; }

		[FindsBy(Using = ADD_USER_BTN_ID)]
		protected IWebElement AddUserButton { get; set; }

		protected IWebElement FoundUserSurnameInput { get; set; }

		protected IWebElement FoundUserNameInput { get; set; }

		protected const string ADDED_ACCOUNT_USERS_XPATH = "//table//tr[contains(string(), '*#*')]";
		protected const string FIND_USER_INPUT_ID = "searchText";
		protected const string FIND_USER_BTN_ID = "findUser";
		protected const string FOUND_USER_SURNAME_INPUT_XPATH = "//table//tbody[@id = 'usersTable']//tr[contains(string(), '*#*')]//input[@name = 'surnames']";
		protected const string FOUND_USER_NAME_INPUT_XPATH = "//table//tbody[@id = 'usersTable']//tr[contains(string(), '*#*')]//input[@name = 'names']";
		protected const string ADD_USER_BTN_ID = "addUsersBtn";
	}
}
