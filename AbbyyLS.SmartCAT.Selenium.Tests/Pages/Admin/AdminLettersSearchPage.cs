using System;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	/// <summary>
	/// Поиск писем в админке
	/// </summary>
	public class AdminEmailsSearchPage : AdminLingvoProPage, IAbstractPage<AdminEmailsSearchPage>
	{
		public new AdminEmailsSearchPage GetPage()
		{
			var adminEmailsSearchPage = new AdminEmailsSearchPage();
			InitPage(adminEmailsSearchPage);

			return adminEmailsSearchPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.Id(SEARCH_EMAIL_INPUT_ID), timeout: 7))
			{
				Assert.Fail("Произошла ошибка:\n не загружена страничка AdminEmailsSearchPage (Поиск писем в админке).");
			}
		}

		/// <summary>
		/// Ввести email пользователя в поле для поиска
		/// </summary>
		/// <param name="email">емаил</param>
		public AdminEmailsSearchPage SetEmail(string email)
		{
			Logger.Debug("Ввести email пользователя {0} в поле для поиска.", email);
			SearchEmailInput.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Ввести ограничение по поиску писем
		/// </summary>
		/// <param name="limit">количество писем (последние письма)</param>
		public AdminEmailsSearchPage SetLimitCount(int limit)
		{
			Logger.Debug("Ввести ограничение по поиску писем: {0}.", limit);
			LimitCountInput.SetText(Convert.ToString(limit));

			return GetPage();
		}

		/// <summary>
		/// Кликнуть на кнопку "Найти"
		/// </summary>
		public AdminEmailsSearchPage ClickFindButton()
		{
			Logger.Debug("Кликнуть на кнопку 'Найти'.");
			FindButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, сработал ли поиск и появилась ли таблица с письмами
		/// </summary>
		public AdminEmailsSearchPage AssertFoundEmailesAppeared()
		{
			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(FOUND_EMAILS_TABLE), timeout: 7),
				"Произошла ошибка:\n не появилась таблица с найденными письмами.");

			return GetPage();
		}

		[FindsBy(Using = SEARCH_EMAIL_INPUT_ID)]
		protected IWebElement SearchEmailInput { get; set; }

		[FindsBy(Using = LIMIT_COUNT_INPUT_ID)]
		protected IWebElement LimitCountInput { get; set; }

		[FindsBy(How = How.XPath, Using = FIND_BTN_XPATH)]
		protected IWebElement FindButton { get; set; }

		protected const string SEARCH_EMAIL_INPUT_ID = "SearchEmail";
		protected const string LIMIT_COUNT_INPUT_ID = "LimitCount";
		protected const string FIND_BTN_XPATH = "//input[contains(@class, 'button initializeSearch')]";
		protected const string FOUND_EMAILS_TABLE = "//table[contains(@class, 'foundEmails')]";
	}
}
