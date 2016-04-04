using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration.FreelanceRegistration
{
	class FreelanceRegistrationSignInPage : IAbstractPage<FreelanceRegistrationSignInPage>
	{
		public WebDriver Driver { get; protected set; }

		public FreelanceRegistrationSignInPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public FreelanceRegistrationSignInPage LoadPage()
		{
			if (!IsFreelanceRegistrationSignInPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница 'Sign In' регистрации фрилансеров.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести email
		/// </summary>
		/// <param name="email">email</param>
		public FreelanceRegistrationSignInPage FillEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести email {0}.", email);
			Email.SetText(email);

			return LoadPage();
		}

		/// <summary>
		/// Ввести пароль
		/// </summary>
		/// <param name="password">пароль</param>
		public FreelanceRegistrationSignInPage FillPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести пароль {0}.", password);
			Password.SetText(password);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Sign In' для продолжения регистрации фрилансера. 
		/// </summary>
		public WorkspacePage ClickSignInButtonWithInactivePersonalAccount()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Sign In' для продолжения регистрации фрилансера.");
			SignInButton.Click();

			return new WorkspacePage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Sign In' для перехода в Workspace.
		/// </summary>
		public WorkspacePage ClickSignInButtonWithActivePersonalAccount()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Sign In' для перехода в Workspace.");
			SignInButton.Click();

			return new WorkspacePage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Sign In', ожидая ошибку.
		/// </summary>
		public FreelanceRegistrationSignInPage ClickSignInButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Sign In', ожидая ошибку.");
			SignInButton.Click();

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Заполнить форму входа
		/// </summary>
		/// <param name="email">email</param>
		/// <param name="password">пароль</param>
		public FreelanceRegistrationSignInPage FillSignInForm(string email, string password)
		{
			CustomTestContext.WriteLine("Заполнить форму входа.");
			Email.SetText(email);
			Password.SetText(password);

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница 'Sign in' для фрилансеров
		/// </summary>
		public bool IsFreelanceRegistrationSignInPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(EMAIL));
		}

		/// <summary>
		/// Проверить, что появилось валидационное сообщение о неверном пароле.
		/// </summary>
		public bool IsWrongPasswordErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось валидационное сообщение о неверном пароле.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(WRONG_PASSWORD_ERROR));
		}

		/// <summary>
		/// Проверить, что появилось валидационное сообщение о том, что пользователь не существует.
		/// </summary>
		public bool IsUserNotExistErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось валидационное сообщение о том, что пользователь не существует.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(NOT_EXIST_USER_ERROR));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = SIGN_IN_BUTTON)]
		protected IWebElement SignInButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string EMAIL = "//form[@name='signinForm']//input[@id='email']";
		protected const string PASSWORD = "//form[@name='signinForm']//input[@id='password']";
		protected const string SIGN_IN_BUTTON = "//button[@id='btn-sign-in']";
		protected const string WRONG_PASSWORD_ERROR = "//div[@ng-message='wrong-password']";
		protected const string NOT_EXIST_USER_ERROR = "//span[contains(@translate, 'USER-NOT-FOUND-ERROR')]";//сообщение ,что юзера не существует 

		#endregion
	}
}
