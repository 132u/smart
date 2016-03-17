using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class CreateAccountPage : BaseObject, IAbstractPage<CreateAccountPage>
	{
		public WebDriver Driver { get; protected set; }

		public CreateAccountPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public CreateAccountPage GetPage()
		{
			var createAccountPage = new CreateAccountPage(Driver);
			InitPage(createAccountPage, Driver);

			return createAccountPage;
		}

		public void LoadPage()
		{
			Driver.WaitPageTotalLoad();
			if (!IsCreateAccountPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась форма создания аккаунта.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку 'Create Account'.
		/// </summary>
		public SelectProfileForm ClickCreateAccountButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Create Account'.");
			CreateAccountButton.Click();

			return new SelectProfileForm(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		public bool IsCreateAccountPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_ACCOUNT_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CREATE_ACCOUNT_BUTTON)]
		protected IWebElement CreateAccountButton { get; set; }

		#endregion
		
		#region Описание XPath элементов

		protected const string CREATE_ACCOUNT_BUTTON = "//button[contains(@data-bind, 'createAccount')]";

		#endregion
	}
}
