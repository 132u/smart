using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class FacebookPage : BaseObject, IAbstractPage<FacebookPage>
	{
		public FacebookPage GetPage()
		{
			var facebookPage = new FacebookPage();
			InitPage(facebookPage);

			return facebookPage;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SUBMIT_BUTTON)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница FacebookPage (вход в Facebook).");
			}
		}

		/// <summary>
		/// Ввести email
		/// </summary>
		/// <param name="email">email пользователя</param>
		public FacebookPage SetEmail(string email)
		{
			Logger.Debug("Ввести email пользователя на странице Facebook {0}.", email);

			Email.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Ввести password
		/// </summary>
		/// <param name="password">password пользователя</param>
		public FacebookPage SetPassword(string password)
		{
			Logger.Debug("Ввести пароль пользователя на странице Facebook {0}.", password);

			Password.SetText(password);

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

		[FindsBy(How = How.XPath, Using = SUBMIT_BUTTON)]
		protected IWebElement SubmitButton { get; set; }

		protected const string EMAIL = "//input[@id='email']";
		protected const string SUBMIT_BUTTON = "//input[@id='u_0_2']";
		protected const string PASSWORD = "//input[@id='pass']";
	}
}
