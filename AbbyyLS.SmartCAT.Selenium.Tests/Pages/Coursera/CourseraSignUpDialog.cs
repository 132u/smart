using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	public class CourseraSignUpDialog: IAbstractPage<CourseraSignUpDialog>
	{
		public WebDriver Driver { get; protected set; }

		public CourseraSignUpDialog(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public CourseraSignUpDialog LoadPage()
		{
			if (!IsCourseraSignUpDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Не открылся диалог регистрации пользователя.");
			}

			return this;
		}
	
		#region Простые методы страницы

		/// <summary>
		/// Ввести имя.
		/// </summary>
		/// <param name="name">имя</param>
		public CourseraSignUpDialog FillName(string name)
		{
			CustomTestContext.WriteLine("Ввести имя {0}.", name);
			Name.SetText(name);

			return LoadPage();
		}

		/// <summary>
		/// Ввести фамилию.
		/// </summary>
		/// <param name="surname">фамилия</param>
		public CourseraSignUpDialog FillSurname(string surname)
		{
			CustomTestContext.WriteLine("Ввести фамилию {0}.", surname);
			Surname.SetText(surname);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку SignUp.
		/// </summary>
		public CompleteRegistrationDialog ClickSignUpButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку SignUp.");
			SignUpButton.Click();

			return new CompleteRegistrationDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel.
		/// </summary>
		public CourseraHomePage ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel.");
			var d=Driver.WaitUntilElementIsEnabled(By.XPath(CANCEL_BUTTON));
			CancelButton.Click();

			return new CourseraHomePage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Заполнить форму регистрации.
		/// </summary>
		/// <param name="name">имя</param>
		/// <param name="surname">фамилия</param>
		public CourseraSignUpDialog FillRegistrationForm(string name, string surname)
		{
			CustomTestContext.WriteLine("Заполнить форму регистрации.");
			FillName(name);
			FillSurname(surname);

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся диалог регистрации пользователя.
		/// </summary>
		private bool IsCourseraSignUpDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(NAME));
		}

		/// <summary>
		/// Проверить, что появилось сообщение о окончании регистраци.
		/// </summary>
		public bool IsThanksMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение о окончании регистраци.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(THANKS_MESSAGE));
		}

		#endregion
		
		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SIGN_UP_BUTTON)]
		protected IWebElement SignUpButton { get; set; }

		[FindsBy(How = How.XPath, Using = NAME)]
		protected IWebElement Name { get; set; }

		[FindsBy(How = How.XPath, Using = SURNAME)]
		protected IWebElement Surname { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string SIGN_UP_BUTTON = "//form[@id='login-form-register']//input[@class='btn-orange']";
		protected const string NAME = "//input[@name='name']";
		protected const string SURNAME = ".//input[@name ='surname']";
		protected const string CANCEL_BUTTON = "//div[contains(@id,'login')]//div[@class='cancel']";
		protected const string THANKS_MESSAGE = "//p[contains(text(),'Thanks for the registration')]";

		#endregion
	}
}
