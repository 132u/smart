using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminFindUserPage : AdminLingvoProPage, IAbstractPage<AdminFindUserPage>
	{
		public new AdminFindUserPage GetPage()
		{
			var adminFindUserPage = new AdminFindUserPage();
			InitPage(adminFindUserPage);

			return adminFindUserPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(INPUT_SEARCH)))
			{
				Assert.Fail("Произошла ошибка:\n не загружена страница поиска пользователя.");
			}
		}

		/// <summary>
		/// Ввести имя пользователя в поиск
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public AdminFindUserPage FillUserNameSearch(string userName)
		{
			Logger.Trace("Ввести имя пользователя '{0}' в поле поиска.", userName);
			InputSearch.SetText(userName);

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по кнопке 'Найти' рядом с полем поиска
		/// </summary>
		public AdminFindUserPage ClickFindButton()
		{
			Logger.Trace("Кликнуть по кнопке 'Найти' рядом с полем поиска");
			FindButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по email в таблице результатов поиска
		/// </summary>
		public AdminEditUserPage ClickEmailInSearchResultTable(string email)
		{
			Logger.Trace("Клик по email '{0}' в таблице результатов поиска", email);
			EmailInSearchResultTable = Driver.SetDynamicValue(How.XPath, EMAIL_IN_SEARCH_RES_TABLE, email);
			EmailInSearchResultTable.Click();

			return new AdminEditUserPage().GetPage();
		}

		[FindsBy(How = How.XPath, Using = INPUT_SEARCH)]
		protected IWebElement InputSearch { get; set; }

		[FindsBy(How = How.XPath, Using = EMAIL_IN_SEARCH_RES_TABLE)]
		protected IWebElement EmailInSearchResultTable { get; set; }

		[FindsBy(How = How.XPath, Using = FIND_BTN)]
		protected IWebElement FindButton { get; set; }

		protected const string INPUT_SEARCH = "//input[(@id = 'searchText')]";
		protected const string EMAIL_IN_SEARCH_RES_TABLE = "//a[text()='*#*']";
		protected const string FIND_BTN = "//form[@action='/Users']/input[2]";
	}
}
