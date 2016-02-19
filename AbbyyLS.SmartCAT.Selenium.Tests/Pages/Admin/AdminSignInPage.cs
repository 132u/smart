using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminSignInPage : BaseObject, IAbstractPage<AdminSignInPage>
	{
		public WebDriver Driver { get; private set; }

		public AdminSignInPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public AdminSignInPage GetPage()
		{
			var adminSignInPage = new AdminSignInPage(Driver);
			InitPage(adminSignInPage, Driver);

			return adminSignInPage;
		}

		public void LoadPage()
		{
			if (!IsAdminSignInPageOpened())
			{
				throw new Exception("Произошла ошибка:\n не загрузилась страница входа в админку.");
			}
		}

		#region Простые методы

		/// <summary>
		/// Ввести логин
		/// </summary>
		/// <param name="login">логин (адрес электронной почты)</param>
		public AdminSignInPage SetLogin(string login)
		{
			CustomTestContext.WriteLine("Ввести логин пользователя в админку: {0}.", login);
			Login.SetText(login);

			return GetPage();
		}

		/// <summary>
		/// Ввести пароль
		/// </summary>
		/// <param name="password">пароль</param>
		public AdminSignInPage SetPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести пароль пользователя в админку: {0}.", password);
			Password.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Нажать сабмит
		/// </summary>
		public AdminLingvoProPage ClickSubmitButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Submit'.");
			SubmitButton.Click();
			var adminLingvoProPage = new AdminLingvoProPage(Driver);

			return adminLingvoProPage.GetPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Логинимся в админку
		/// </summary>
		/// <param name="login">логин (email)</param>
		/// <param name="password">пароль</param>
		public AdminLingvoProPage SignIn(string login, string password)
		{
			SetLogin(login);
			SetPassword(password);
			var adminLingvoProPage = ClickSubmitButton();

			return adminLingvoProPage.GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница входа в админку
		/// </summary>
		public bool IsAdminSignInPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(LOGIN_FORM_XPATH));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = LOGIN_XPATH)]
		protected IWebElement Login { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD_XPATH)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = SUBMIT_BTN_XPATH)]
		protected IWebElement SubmitButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string LOGIN_FORM_XPATH = "//form[contains(@action,'/Home/Login')]";
		protected const string LOGIN_XPATH = "//input[@name='email']";
		protected const string PASSWORD_XPATH = "//input[@name='password']";
		protected const string SUBMIT_BTN_XPATH = "//input[@type='submit']";

		#endregion
	}
}

