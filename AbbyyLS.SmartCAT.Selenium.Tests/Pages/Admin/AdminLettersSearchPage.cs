﻿using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminLettersSearchPage : AdminLingvoProPage, IAbstractPage<AdminLettersSearchPage>
	{
		public AdminLettersSearchPage(WebDriver driver) : base(driver)
		{
		}

		public new AdminLettersSearchPage LoadPage()
		{
			if (!IsAdminLettersSearchPageOpened())
			{
				throw new Exception("Произошла ошибка:\n не загружена страница поиска писем в админке.");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Ввести email пользователя в поле для поиска
		/// </summary>
		/// <param name="email">емаил</param>
		public AdminLettersSearchPage SetEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести email пользователя {0} в поле для поиска.", email);
			SearchEmailInput.SetText(email);

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на кнопку "Найти"
		/// </summary>
		public AdminLettersSearchPage ClickFindButton()
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку 'Найти'.");
			FindButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на кнопку 'Подробнее'
		/// </summary>
		/// <param name="email">email</param>
		public AdminLettersSearchPage ClickOpenEmailConfirmationLetterButton(string email)
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку 'Подробнее'.");
			OpenEmailConfirmationLetterButton = Driver.SetDynamicValue(How.XPath, OPEN_EMAIL_CONFIRMATION_LETTER_BUTTON, email);
			OpenEmailConfirmationLetterButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на кнопку 'Подробнее'
		/// </summary>
		/// <param name="email">email</param>
		public AdminLettersSearchPage ClickOpenResendedEmailConfirmationLetterButton(string email)
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку 'Подробнее'.");
			OpenResendedEmailConfirmationLetterButton = Driver.SetDynamicValue(How.XPath, OPEN_RESENDED_EMAIL_CONFIRMATION_LETTER_BUTTON, email);
			OpenResendedEmailConfirmationLetterButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на кнопку 'Подробнее'
		/// </summary>
		/// <param name="email">email</param>
		public AdminLettersSearchPage ClickOpenLetterButton(string email)
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку 'Подробнее'.");
			OpenLastLetterButton = Driver.SetDynamicValue(How.XPath, OPEN_LAST_LETTER_BUTTON, email);
			OpenLastLetterButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Переключиться в окно письма
		/// </summary>
		public AdminLetterPage SwitchToLetterWindow()
		{
			CustomTestContext.WriteLine("Переключиться в окно письма.");
			Driver.SwitchToNewBrowserTab();

			return new AdminLetterPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Открыть письмо
		/// </summary>
		/// <param name="email">email</param>
		public AdminLetterPage OpenLetter(string email)
		{
			ClickOpenLetterButton(email);
			SwitchToLetterWindow();
			
			return new AdminLetterPage(Driver).LoadPage();
		}

		/// <summary>
		/// Открыть письмо подтверждения пароля
		/// </summary>
		/// <param name="email">email</param>
		public AdminLetterPage OpenEmailConfirmationLetter(string email)
		{
			ClickOpenEmailConfirmationLetterButton(email);
			SwitchToLetterWindow();

			return new AdminLetterPage(Driver).LoadPage();
		}

		/// <summary>
		/// Открыть письмо подтверждения пароля
		/// </summary>
		/// <param name="email">email</param>
		public AdminLetterPage OpenResendedEmailConfirmationLetter(string email)
		{
			ClickOpenResendedEmailConfirmationLetterButton(email);
			SwitchToLetterWindow();

			return new AdminLetterPage(Driver).LoadPage();
		}

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

		protected IWebElement OpenLastLetterButton { get; set; }

		protected IWebElement OpenEmailConfirmationLetterButton { get; set; }

		protected IWebElement OpenResendedEmailConfirmationLetterButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string SEARCH_EMAIL_INPUT_ID = "SearchEmail";
		protected const string LIMIT_COUNT_INPUT_ID = "LimitCount";
		protected const string FIND_BTN_XPATH = "//input[contains(@class, 'button initializeSearch')]";
		protected const string FOUND_EMAILS_TABLE = "//table[contains(@class, 'foundEmails')]";
		protected const string OPEN_EMAIL_CONFIRMATION_LETTER_BUTTON = "//td[text()='*#*']//following-sibling::td[text()='SmartCAT | Confirm your email to sign up for SmartCAT']//following-sibling::td[@class='openLetter']//a";
		protected const string OPEN_RESENDED_EMAIL_CONFIRMATION_LETTER_BUTTON = "//td[text()='*#*']//following-sibling::td[text()='Please confirm your SmartCAT registration']//following-sibling::td[@class='openLetter']//a";
		protected const string OPEN_LAST_LETTER_BUTTON = "//tr[1]//td[text()='*#*']//following-sibling::td[@class='openLetter']//a";

		#endregion
	}
}
