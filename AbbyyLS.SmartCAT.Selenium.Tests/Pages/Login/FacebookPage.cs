using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class FacebookPage : IAbstractPage<FacebookPage>
	{
		public WebDriver Driver { get; protected set; }

		public FacebookPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public FacebookPage LoadPage()
		{
			if (!IsFaceBookPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница FacebookPage (вход в Facebook).");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести email
		/// </summary>
		/// <param name="email">email пользователя</param>
		public FacebookPage SetEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести email пользователя на странице Facebook {0}.", email);

			Email.SetText(email);

			return LoadPage();
		}

		/// <summary>
		/// Ввести password
		/// </summary>
		/// <param name="password">password пользователя</param>
		public FacebookPage SetPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести пароль пользователя на странице Facebook {0}.", password);

			Password.SetText(password);

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
			SetPassword(password);
			var selectAccountForm = ClickSubmitButton();

			return selectAccountForm;
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница авторизации FaceBook
		/// </summary>
		public bool IsFaceBookPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SUBMIT_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = SUBMIT_BUTTON)]
		protected IWebElement SubmitButton { get; set; }

		#endregion

		#region Описание Xpath элементов

		protected const string EMAIL = "//input[@id='email']";
		protected const string SUBMIT_BUTTON = "//*[@name='login'][local-name()='button' or local-name()='input']";
		protected const string PASSWORD = "//input[@id='pass']";

		#endregion
	}
}
