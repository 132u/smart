using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

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
			LoadPage();

			return adminLingvoProPage;
		}
		
		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.XPath(ENTERPRISE_ACCOUNTS_REF_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось зайти в админку.\n" +
					" Не загружена страничка AdminLingvoProPage (Lingvo.Pro Admin).");
			}
		}

		/// <summary>
		/// Кликнуть по ссылке "Корпоративные аккаунты"
		/// </summary>
		public AdminEnterpriseAccountsPage ClickEnterpriseAccountsRef()
		{
			Logger.Debug("Кликаем по ссылке 'Корпоративные аккаунты'.");
			EnterpriseAccountsRef.Click();
			var adminEnterpriseAccountsPage = new AdminEnterpriseAccountsPage();

			return adminEnterpriseAccountsPage.GetPage();
		}

		/// <summary>
		/// Кликнуть по ссылке "Поиск писем"
		/// </summary>
		public AdminEmailsSearchPage ClickAdminLettersSearchRef()
		{
			Logger.Debug("Кликаем по ссылке 'Поиск писем'.");
			LettersSearchRef.Click();
			var adminEmailsSearchPage = new AdminEmailsSearchPage();

			return adminEmailsSearchPage.GetPage();
		}

		[FindsBy(How = How.XPath, Using = ENTERPRISE_ACCOUNTS_REF_XPATH)]
		protected IWebElement EnterpriseAccountsRef { get; set; }

		[FindsBy(How = How.XPath, Using = LETTERS_SEARCH_REF_XPATH)]
		protected IWebElement LettersSearchRef { get; set; }

		protected const string ENTERPRISE_ACCOUNTS_REF_XPATH = "//a[@href='/EnterpriseAccounts']";
		protected const string LETTERS_SEARCH_REF_XPATH = "//a[@href='/EMailTaskList']";
	}	
}