using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminLingvoProPage : IAbstractPage<AdminLingvoProPage>
	{
		public WebDriver Driver { get; protected set; }

		public AdminLingvoProPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public AdminLingvoProPage LoadPage()
		{
			if (!IsAdminLingvoProPageOpened())
			{
				throw new Exception("Произошла ошибка:\n не удалось зайти в админку.\n" +
					" Не загружена страничка AdminLingvoProPage (Lingvo.Pro Admin).");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Кликнуть по ссылке 'Корпоративные аккаунты'
		/// </summary>
		public AdminEnterpriseAccountsPage ClickEnterpriseAccountsLink()
		{
			CustomTestContext.WriteLine("Кликнуть по ссылке 'Корпоративные аккаунты'.");
			EnterpriseAccountsLink.Click();

			return new AdminEnterpriseAccountsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Кликнуть по ссылке 'Поиск писем'
		/// </summary>
		public AdminEmailsSearchPage ClickAdminLettersSearchReference()
		{
			CustomTestContext.WriteLine("Кликнуть по ссылке 'Поиск писем'.");
			LettersSearchReference.Click();

			return new AdminEmailsSearchPage(Driver).LoadPage();
		}

		/// <summary>
		/// Кликнуть по ссылке 'Создать пользователя'
		/// </summary>
		public AdminCreateUserPage ClickCreateUserReference()
		{
			CustomTestContext.WriteLine("Кликнуть по ссылке 'Создать пользователя'.");
			CreateUserReference.Click();
			var adminCreateUserPage = new AdminCreateUserPage(Driver);

			return adminCreateUserPage.LoadPage();
		}

		/// <summary>
		/// Кликнуть по ссылке Поиск пользователей в меню
		/// </summary>
		public AdminFindUserPage ClickSearchUserReference()
		{
			CustomTestContext.WriteLine("Клик по ссылке Поиск пользователей в меню");
			SearchUserReference.Click();

			return new AdminFindUserPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на ссылку 'Просмотреть пакеты словарей'
		/// </summary>
		public AdminDictionariesPackagesPage ClickDictionariesPackagesLink()
		{
			CustomTestContext.WriteLine("Нажать на ссылку 'Просмотреть пакеты словарей'.");
			DictionaryPackagesLink.Click();

			return new AdminDictionariesPackagesPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница Lingvo.Pro Admin
		/// </summary>
		public bool IsAdminLingvoProPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(ENTERPRISE_ACCOUNTS_LINK));
		}

		#endregion

		#region Объявление элементов страниц

		[FindsBy(How = How.XPath, Using = LETTERS_SEARCH_REF)]
		protected IWebElement LettersSearchReference { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_USER_REF)]
		protected IWebElement CreateUserReference { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_USER_LINK)]
		protected IWebElement SearchUserReference { get; set; }

		[FindsBy(How = How.XPath, Using = DICTIONARY_PACKAGES_LINK)]
		protected IWebElement DictionaryPackagesLink { get; set; }

		[FindsBy(How = How.XPath, Using = ENTERPRISE_ACCOUNTS_LINK)]
		protected IWebElement EnterpriseAccountsLink { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string LETTERS_SEARCH_REF = "//a[@href='/EMailTaskList']";
		protected const string CREATE_USER_REF = "//a[@href='/Users/Create']";
		protected const string SEARCH_USER_LINK = "//a[@href='/Users']";
		protected const string DICTIONARY_PACKAGES_LINK = "//a[contains(@href,'/DictionariesPackages')]";
		protected const string ENTERPRISE_ACCOUNTS_LINK = ".//a[@href='/EnterpriseAccounts']";

		#endregion
	}
}