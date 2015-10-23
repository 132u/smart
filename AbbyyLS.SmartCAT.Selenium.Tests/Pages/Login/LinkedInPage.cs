using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class LinkedInPage : BaseObject, IAbstractPage<LinkedInPage>
	{
		public WebDriver Driver { get; protected set; }

		public LinkedInPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public LinkedInPage GetPage()
		{
			var linkedInPage = new LinkedInPage(Driver);
			InitPage(linkedInPage, Driver);

			return linkedInPage;
		}

		public void LoadPage()
		{
			if (!IsLinkedInPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница LinkedInPage (вход в LinkedIn).");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести email
		/// </summary>
		/// <param name="email">email пользователя</param>
		public LinkedInPage SetEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести email пользователя на странице LinkedIn {0}.", email);

			Email.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Ввести password
		/// </summary>
		/// <param name="password">password пользователя</param>
		public LinkedInPage SetPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести пароль пользователя на странице LinkedIn {0}.", password);

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
		/// Проверить, открыта ли страница авторизации LinkedIn
		/// </summary>
		public bool IsLinkedInPageOpened()
		{
			CustomTestContext.WriteLine("Проверить, открыта ли страница авторизации LinkedIn.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(LINKED_IN_HEADER));
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

		protected const string PASSWORD = "//input[@id='session_password-oauth2SAuthorizeForm']";
		protected const string EMAIL = "//input[@id='session_key-oauth2SAuthorizeForm']";
		protected const string SUBMIT_BUTTON = "//input[@class='allow']";
		protected const string LINKED_IN_HEADER = "//div[@class='logo' and text()='LinkedIn']";

		#endregion
	}
}
