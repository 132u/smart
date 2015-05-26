using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	/// <summary>
	/// Страничка Lingvo.Pro Admin
	/// </summary>
	public class AdminLingvoProPage : BaseObject, IAbstractPage<AdminLingvoProPage>
	{
		public AdminLingvoProPage GetPage()
		{
			var adminLingvoProPage = new AdminLingvoProPage();
			InitPage(adminLingvoProPage);

			return adminLingvoProPage;
		}
		
		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ENTERPRISE_ACCOUNTS_REF)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось зайти в админку.\n" +
					" Не загружена страничка AdminLingvoProPage (Lingvo.Pro Admin).");
			}
		}

		/// <summary>
		/// Кликнуть по ссылке 'Корпоративные аккаунты'
		/// </summary>
		public AdminEnterpriseAccountsPage ClickEnterpriseAccountsReference()
		{
			Logger.Debug("Кликнуть по ссылке 'Корпоративные аккаунты'.");
			EnterpriseAccountsReference.Click();

			return new AdminEnterpriseAccountsPage().GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке 'Поиск писем'
		/// </summary>
		public AdminEmailsSearchPage ClickAdminLettersSearchReference()
		{
			Logger.Debug("Кликнуть по ссылке 'Поиск писем'.");
			LettersSearchReference.Click();

			return new AdminEmailsSearchPage().GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке 'Создать пользователя'
		/// </summary>
		public AdminCreateUserPage ClickCreateUserReference()
		{
			Logger.Debug("Кликнуть по ссылке 'Создать пользователя'.");
			CreateUserReference.Click();
			var adminCreateUserPage = new AdminCreateUserPage();

			return adminCreateUserPage.GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке Поиск пользователей в меню
		/// </summary>
		public AdminFindUserPage ClickSearchUserReference()
		{
			Logger.Trace("Клик по ссылке Поиск пользователей в меню");
			SearchUserReference.Click();

			return new AdminFindUserPage().GetPage();
		}

		[FindsBy(How = How.XPath, Using = ENTERPRISE_ACCOUNTS_REF)]
		protected IWebElement EnterpriseAccountsReference { get; set; }

		[FindsBy(How = How.XPath, Using = LETTERS_SEARCH_REF)]
		protected IWebElement LettersSearchReference { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_USER_REF)]
		protected IWebElement CreateUserReference { get; set; }

		[FindsBy(How = How.XPath, Using = SERACH_USER_LINK)]
		protected IWebElement SearchUserReference { get; set; }

		protected const string ENTERPRISE_ACCOUNTS_REF = "//a[@href='/EnterpriseAccounts']";
		protected const string LETTERS_SEARCH_REF = "//a[@href='/EMailTaskList']";
		protected const string CREATE_USER_REF = "//a[@href='/Users/Create']";
		protected const string SERACH_USER_LINK = "//a[@href='/Users']";
	}	
}