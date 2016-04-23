using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class ProZConfirmLogInPage : IAbstractPage<ProZConfirmLogInPage>
	{
		public WebDriver Driver { get; protected set; }

		public ProZConfirmLogInPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public ProZConfirmLogInPage LoadPage()
		{
			if (!IsProZConfirmLogInPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница подтверждения" + 
					" перехода в SmartCat.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку AllowAccess
		/// </summary>
		public SelectAccountForm ClickAllowAccessButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку AllowAccess");
			AllowAccessButton.JavaScriptClick();

			return new SelectAccountForm(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		public bool IsProZConfirmLogInPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(PROZ_LOGO));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = PROZ_LOGO)]
		protected IWebElement ProzLogo { get; set; }

		[FindsBy(How = How.XPath, Using = ALLOW_ACCESS_BUTTON)]
		protected IWebElement AllowAccessButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string PROZ_LOGO = "//img[contains(@alt, 'ProZ.com global directory of translation services')]";
		protected const string ALLOW_ACCESS_BUTTON = "//button[contains(@value, 'yes')]";

		#endregion
	}
}
