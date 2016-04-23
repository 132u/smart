using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class ProZPage : IAbstractPage<ProZPage>
	{
		public WebDriver Driver { get; protected set; }

		public ProZPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public ProZPage LoadPage()
		{
			if (!IsProZPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница ProZPage (вход в ProZ).");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести логин пользователя на странице ProZ.
		/// </summary>
		/// <param name="login">логин</param>
		public ProZPage SetEmail(string login)
		{
			CustomTestContext.WriteLine("Ввести логин {0} пользователя на странице ProZ.", login);
			LoginField.SetText(login);

			return LoadPage();
		}

		/// <summary>
		/// Ввести пароль пользователя на странице ProZ.
		/// </summary>
		/// <param name="password">пароль пользователя</param>
		public ProZPage SetPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести пароль {0} пользователя на странице ProZ.", password);
			Password.SetText(password);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Login.
		/// </summary>
		public ProZConfirmLogInPage ClickSubmitButton()
		{
			CustomTestContext.WriteLine("Нажать 'Login' на странице ProZ.");
			SubmitButton.JavaScriptClick();

			return new ProZConfirmLogInPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Заполнить форму авторизации.
		/// </summary>
		/// <param name="login">login</param>
		/// <param name="password">пароль</param>
		public ProZConfirmLogInPage FillAuthorizationForm(string login, string password)
		{
			SetEmail(login);
			SetPassword(password);
			var selectAccountForm = ClickSubmitButton();

			return selectAccountForm;
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		public bool IsProZPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(PROZ_LOGO));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = LOGIN_FIELD)]
		protected IWebElement LoginField { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = PROZ_LOGO)]
		protected IWebElement ProzLogo { get; set; }

		[FindsBy(How = How.XPath, Using = SUBMIT_BUTTON)]
		protected IWebElement SubmitButton { get; set; }

		#endregion

		#region Описание Xpath элементов

		protected const string PROZ_LOGO = "//img[contains(@class, 'media-object')]";
		protected const string LOGIN_FIELD = "//div[contains(@class, 'form-group')]//input[contains(@name, 'username')]";
		protected const string PASSWORD = "//div[contains(@class, 'form-group')]//input[contains(@name, 'password')]";
		protected const string SUBMIT_BUTTON = "//button[contains(@type, 'submit')]";

		#endregion
	}
}