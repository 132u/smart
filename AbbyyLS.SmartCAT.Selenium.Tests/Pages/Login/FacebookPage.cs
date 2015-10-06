using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class FacebookPage : BaseObject, IAbstractPage<FacebookPage>
	{
		public WebDriver Driver { get; protected set; }

		public FacebookPage(WebDriver driver)
		{
			Driver = driver;
		}

		public FacebookPage GetPage()
		{
			var facebookPage = new FacebookPage(Driver);
			InitPage(facebookPage, Driver);

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
			CustomTestContext.WriteLine("Ввести email пользователя на странице Facebook {0}.", email);

			Email.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Ввести password
		/// </summary>
		/// <param name="password">password пользователя</param>
		public FacebookPage SetPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести пароль пользователя на странице Facebook {0}.", password);

			Password.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Submit
		/// </summary>
		public SelectAccountForm ClickSubmitButton()
		{
			CustomTestContext.WriteLine("Нажать 'Sign In'.");

			SubmitButton.JavaScriptClick();

			return new SelectAccountForm(Driver).GetPage();
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
