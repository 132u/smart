using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class GooglePage : IAbstractPage<GooglePage>
	{
		public WebDriver Driver { get; protected set; }

		public GooglePage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public GooglePage LoadPage()
		{
			if (!IsGooglePageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница GooglePage (вход в Google).");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести email пользователя на странице Google.
		/// </summary>
		/// <param name="email">email пользователя</param>
		public GooglePage SetEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести email {0} пользователя на странице Google.", email);
			Email.SetText(email);

			return LoadPage();
		}

		/// <summary>
		/// Ввести пароль пользователя на странице Google.
		/// </summary>
		/// <param name="password">password пользователя</param>
		public GooglePage SetPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести пароль {0} пользователя на странице Google.", password);
			Driver.WaitUntilElementIsDisplay(By.XPath(PASSWORD));
			Password.SetText(password);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Next
		/// </summary>
		public GooglePage ClickNextButton()
		{
			CustomTestContext.WriteLine("Нажать 'Next'.");
			NextButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Submit
		/// </summary>
		public SelectAccountForm ClickSubmitButton()
		{
			CustomTestContext.WriteLine("Нажать 'Sign In'.");
			SubmitButton.JavaScriptClick();

			return new SelectAccountForm(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Заполнить форму авторизации
		/// </summary>
		/// <param name="email">email</param>
		/// <param name="password">пароль</param>
		public SelectAccountForm SubmitForm(string email, string password)
		{
			SetEmail(email);
			// Авторизация меняется не в первый раз (то с одной страницей, то с двумя).
			// Оставлю этот шаг на случай, если в будущем опять появится переход.
			ClickNextButton();
			SetPassword(password);
			var selectAccountForm = ClickSubmitButton();

			return selectAccountForm;
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница авторизации Google+
		/// </summary>
		public bool IsGooglePageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(GOOGLE_HEADER));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON)]
		protected IWebElement NextButton { get; set; }

		[FindsBy(How = How.XPath, Using = SUBMIT_BUTTON)]
		protected IWebElement SubmitButton { get; set; }

		#endregion

		#region Описание Xpath элементов

		protected const string PASSWORD = "//input[@id='Passwd']";
		protected const string EMAIL = "//input[@id='Email']";
		protected const string NEXT_BUTTON = "//input[@id='next']";
		protected const string SUBMIT_BUTTON = "//input[@id='signIn']";
		protected const string GOOGLE_HEADER = "//div[@aria-label='Google']";

		#endregion
	}
}
