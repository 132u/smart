using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminEmailsSearchPage : AdminLingvoProPage, IAbstractPage<AdminEmailsSearchPage>
	{
		public AdminEmailsSearchPage(WebDriver driver) : base(driver)
		{
		}

		public new AdminEmailsSearchPage GetPage()
		{
			var adminEmailsSearchPage = new AdminEmailsSearchPage(Driver);
			InitPage(adminEmailsSearchPage, Driver);

			return adminEmailsSearchPage;
		}

		public new void LoadPage()
		{
			if (!IsAdminLettersSearchPageOpened())
			{
				throw new Exception("Произошла ошибка:\n не загружена страница поиска писем в админке.");
			}
		}

		#region Простые методы

		/// <summary>
		/// Ввести email пользователя в поле для поиска
		/// </summary>
		/// <param name="email">емаил</param>
		public AdminEmailsSearchPage SetEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести email пользователя {0} в поле для поиска.", email);
			SearchEmailInput.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Ввести ограничение по поиску писем
		/// </summary>
		/// <param name="limit">количество писем (последние письма)</param>
		public AdminEmailsSearchPage SetLimitCount(int limit)
		{
			CustomTestContext.WriteLine("Ввести ограничение по поиску писем: {0}.", limit.ToString());
			LimitCountInput.SetText(Convert.ToString(limit));

			return GetPage();
		}

		/// <summary>
		/// Кликнуть на кнопку "Найти"
		/// </summary>
		public AdminEmailsSearchPage ClickFindButton()
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку 'Найти'.");
			FindButton.Click();

			return GetPage();
		}

		#endregion

		#region Составные методы

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница поиска писем в админке
		/// </summary>
		public bool IsAdminLettersSearchPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.Id(SEARCH_EMAIL_INPUT_ID));
		}

		/// <summary>
		/// Проверить, сработал ли поиск и появилась ли таблица с письмами
		/// </summary>
		public bool IsFoundEmailesTableAppeared()
		{
			CustomTestContext.WriteLine("Проверить, сработал ли поиск и появилась ли таблица с письмами");

			return Driver.WaitUntilElementIsDisplay(By.XPath(FOUND_EMAILS_TABLE));
		}

		#endregion

		#region Объявление элементов страниц

		[FindsBy(Using = SEARCH_EMAIL_INPUT_ID)]
		protected IWebElement SearchEmailInput { get; set; }

		[FindsBy(Using = LIMIT_COUNT_INPUT_ID)]
		protected IWebElement LimitCountInput { get; set; }

		[FindsBy(How = How.XPath, Using = FIND_BTN_XPATH)]
		protected IWebElement FindButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string SEARCH_EMAIL_INPUT_ID = "SearchEmail";
		protected const string LIMIT_COUNT_INPUT_ID = "LimitCount";
		protected const string FIND_BTN_XPATH = "//input[contains(@class, 'button initializeSearch')]";
		protected const string FOUND_EMAILS_TABLE = "//table[contains(@class, 'foundEmails')]";

		#endregion
	}
}
