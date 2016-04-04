using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminFindUserPage : AdminLingvoProPage, IAbstractPage<AdminFindUserPage>
	{
		public AdminFindUserPage(WebDriver driver) : base(driver)
		{
		}

		public new AdminFindUserPage LoadPage()
		{
			if (!IsAdminFindUserPageOpened())
			{
				throw new Exception(
					"Произошла ошибка:\n не загружена страница поиска пользователя.");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Ввести имя пользователя в поиск
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public AdminFindUserPage FillUserNameSearch(string userName)
		{
			CustomTestContext.WriteLine("Ввести имя пользователя '{0}' в поле поиска.", userName);
			InputSearch.SetText(userName);

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по кнопке 'Найти' рядом с полем поиска
		/// </summary>
		public AdminFindUserPage ClickFindButton()
		{
			CustomTestContext.WriteLine("Кликнуть по кнопке 'Найти' рядом с полем поиска");
			FindButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по email в таблице результатов поиска
		/// </summary>
		public AdminEditUserPage ClickEmailInSearchResultTable(string email)
		{
			CustomTestContext.WriteLine("Клик по email '{0}' в таблице результатов поиска", email);
			EmailInSearchResultTable = Driver.SetDynamicValue(How.XPath, EMAIL_IN_SEARCH_RES_TABLE, email);
			EmailInSearchResultTable.Click();

			return new AdminEditUserPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Найти пользователя по email
		/// </summary>
		/// <param name="email"> email </param>
		public AdminEditUserPage FindUser(string email)
		{
			ClickSearchUserReference();
			FillUserNameSearch(email);
			ClickFindButton();
			var adminEditUserPage = ClickEmailInSearchResultTable(email);

			return adminEditUserPage.LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница поиска пользователя
		/// </summary>
		public bool IsAdminFindUserPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(INPUT_SEARCH));
		}

		#endregion

		#region Объявление элементов страниц

		[FindsBy(How = How.XPath, Using = INPUT_SEARCH)]
		protected IWebElement InputSearch { get; set; }

		[FindsBy(How = How.XPath, Using = EMAIL_IN_SEARCH_RES_TABLE)]
		protected IWebElement EmailInSearchResultTable { get; set; }

		[FindsBy(How = How.XPath, Using = FIND_BTN)]
		protected IWebElement FindButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string INPUT_SEARCH = "//input[(@id = 'searchText')]";
		protected const string EMAIL_IN_SEARCH_RES_TABLE = "//a[text()='*#*']";
		protected const string FIND_BTN = "//form[@action='/Users']/input[2]";

		#endregion
	}
}
