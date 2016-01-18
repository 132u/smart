using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	public class CourseraSignInDialog: BaseObject, IAbstractPage<CourseraSignInDialog>
	{
		public WebDriver Driver { get; protected set; }

		public CourseraSignInDialog(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public new CourseraSignInDialog GetPage()
		{
			InitPage(this, Driver);

			return this;
		}

		public void LoadPage()
		{
			if (!IsCourseraSignInDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог авторизации.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести логин.
		/// </summary>
		/// <param name="email">email</param>
		public CourseraSignInDialog FillEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести логин {0}.", email);
			Email.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Ввести пароль.
		/// </summary>
		/// <param name="password">пароль</param>
		public CourseraSignInDialog FillPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести пароль {0}.", password);
			Password.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку SigIn.
		/// </summary>
		public CourseraSignInDialog ClickSigInButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку SigIn.");
			SigInButton.Click();

			return GetPage();
		}

		#endregion
		
		#region Составные методы страницы

		/// <summary>
		/// Заполнить форму авторизации.
		/// </summary>
		/// <param name="email">логин</param>
		/// <param name="password">пароль</param>
		public CourseraHomePage LoginInCoursera(string email, string password)
		{
			CustomTestContext.WriteLine("Заполнить форму авторизации.");
			FillEmail(email);
			FillPassword(password);
			ClickSigInButton();
			
			return new CourseraHomePage(Driver).GetPage();
		}

		#endregion
		
		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что диалог авторизации открылся
		/// </summary>
		private bool IsCourseraSignInDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(EMAIL));
		}

		#endregion
		
		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = SIGN_IN_BUTTON)]
		protected IWebElement SigInButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string EMAIL = ".//input[@name='email']";
		protected const string PASSWORD = ".//input[@name='password']";
		protected const string SIGN_IN_BUTTON = ".//input[@type ='submit']";

		#endregion
	}
}
