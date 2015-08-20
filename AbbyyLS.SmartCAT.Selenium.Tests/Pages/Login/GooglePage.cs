using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class GooglePage : BaseObject, IAbstractPage<GooglePage>
	{
		public GooglePage GetPage()
		{
			var googlePage = new GooglePage();
			InitPage(googlePage);

			return googlePage;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(GOOGLE_HEADER)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница GooglePage (вход в Google).");
			}
		}

		/// <summary>
		/// Ввести email
		/// </summary>
		/// <param name="email">email пользователя</param>
		public GooglePage SetEmail(string email)
		{
			Logger.Debug("Ввести email пользователя на странице Google {0}.", email);

			Email.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Ввести password
		/// </summary>
		/// <param name="password">password пользователя</param>
		public GooglePage SetPassword(string password)
		{
			Logger.Debug("Ввести пароль пользователя на странице Google {0}.", password);
			Driver.WaitUntilElementIsDisplay(By.XPath(PASSWORD));
			Password.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Next
		/// </summary>
		public GooglePage ClickNextButton()
		{
			Logger.Debug("Нажать 'Next'.");

			NextButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Submit
		/// </summary>
		public SelectAccountForm ClickSubmitButton()
		{
			Logger.Debug("Нажать 'Sign In'.");

			SubmitButton.JavaScriptClick();

			return new SelectAccountForm().GetPage();
		}

		[FindsBy(How = How.XPath, Using = EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON)]
		protected IWebElement NextButton { get; set; }

		[FindsBy(How = How.XPath, Using = SUBMIT_BUTTON)]
		protected IWebElement SubmitButton { get; set; }

		protected const string PASSWORD = "//input[@id='Passwd']";
		protected const string EMAIL = "//input[@id='Email']";
		protected const string NEXT_BUTTON = "//input[@id='next']";
		protected const string SUBMIT_BUTTON = "//input[@id='signIn']";
		protected const string GOOGLE_HEADER = "//div[@aria-label='Google']";
	}
}
