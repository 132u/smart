using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	/// <summary>
	/// Вход в админку
	/// </summary>
	public class AdminSignInPage : BaseObject, IAbstractPage<AdminSignInPage>
	{
		public AdminSignInPage GetPage()
		{
			var adminSignInPage = new AdminSignInPage();
			InitPage(adminSignInPage);
			LoadPage();

			return adminSignInPage;
		}

		public void LoadPage()
		{
			Assert.IsTrue(Driver.WaitUntilElementIsPresent(By.XPath(LOGIN_FORM_XPATH)),
				"Произошла ошибка:\n не загрузилась страница AdminSignInPage (вход в админку).");
		}

		/// <summary>
		/// Вводим логин
		/// </summary>
		/// <param name="login">логин (адрес электронной почты)</param>
		public AdminSignInPage SetLogin(string login)
		{
			Logger.Debug("Вводим логин пользователя в админку: {0}.", login);
			Login.SetText(login);

			return GetPage();
		}

		/// <summary>
		/// Вводим пароль
		/// </summary>
		/// <param name="password">пароль</param>
		public AdminSignInPage SetPassword(string password)
		{
			Logger.Debug("Вводим пароль пользователя в админку: {0}.", password);
			Password.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Нажимаем сабмит
		/// </summary>
		public AdminLingvoProPage ClickSubmitBtn()
		{
			Logger.Debug("Нажимаем кнопку 'Submit'.");
			SubmitBtn.Click();
			var adminLingvoProPage = new AdminLingvoProPage();

			return adminLingvoProPage.GetPage();
		}

		[FindsBy(How = How.XPath, Using = LOGIN_XPATH)]
		protected IWebElement Login { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD_XPATH)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = SUBMIT_BTN_XPATH)]
		protected IWebElement SubmitBtn { get; set; }

		protected const string LOGIN_FORM_XPATH = "//form[contains(@action,'/Home/Login')]";
		protected const string LOGIN_XPATH = "//input[@name='email']";
		protected const string PASSWORD_XPATH = "//input[@name='password']";
		protected const string SUBMIT_BTN_XPATH = "//input[@type='submit']";
	}
}

