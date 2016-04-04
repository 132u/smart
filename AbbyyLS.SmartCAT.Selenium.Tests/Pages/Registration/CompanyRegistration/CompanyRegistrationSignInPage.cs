using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration
{
	public class CompanyRegistrationSignInPage : IAbstractPage<CompanyRegistrationSignInPage>
	{
		public WebDriver Driver { get; protected set; }

		public CompanyRegistrationSignInPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public CompanyRegistrationSignInPage LoadPage()
		{
			if (!IsCompanyRegistrationSignInPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница входа для регистрации компаний с существующим аккаунтом.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести email
		/// </summary>
		/// <param name="email">email</param>
		public CompanyRegistrationSignInPage FillEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле Email.", email);
			Email.SetText(email);

			return LoadPage();
		}

		/// <summary>
		/// Ввести пароль
		/// </summary>
		///  <param name="password">пароль</param>
		public CompanyRegistrationSignInPage FillPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле пароля.", password);
			Password.SetText(password);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Sign In'
		/// </summary>
		public CompanyRegistrationSecondPage ClickSignInButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Sign In'.");
			SignInButton.JavaScriptClick();

			return new CompanyRegistrationSecondPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Заполнить форму авторизации
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		public CompanyRegistrationSignInPage FillSignInData(
			string email,
			string password)
		{
			FillEmail(email);
			FillPassword(password);

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница
		/// </summary>
		public bool IsCompanyRegistrationSignInPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(PASSWORD));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.Id, Using = SIGN_IN_BUTTON)]
		protected IWebElement SignInButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string EMAIL = "//form[@name='signinForm']//input[@id='email']";
		protected const string PASSWORD = "//form[@name='signinForm']//input[@id='password']";
		protected const string SIGN_IN_BUTTON = "btn-sign-in";
		protected const string CONFIRM_PASSWORD = "//form[@name='signupForm']//input[@id='confirm']";

		#endregion
	}
}
