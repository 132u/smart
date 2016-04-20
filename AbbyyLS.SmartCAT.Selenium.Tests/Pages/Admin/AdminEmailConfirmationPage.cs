using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminEmailConfirmationPage : IAbstractPage<AdminEmailConfirmationPage>
	{
		public WebDriver Driver { get; set; }

		public AdminEmailConfirmationPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public AdminEmailConfirmationPage LoadPage()
		{
			if (!IsAdminEmailConfirmationPageOpened())
			{
				throw new Exception("Произошла ошибка:\n не загрузилась страница подтверждения email");
			}

			return this;
		}

		/// <summary>
		/// Нажать на ссылку 'Go to sign-in page'
		/// </summary>
		public WorkspacePage ClickGoToSignInPageLink()
		{
			CustomTestContext.WriteLine("Нажать на ссылку 'Go to sign-in page'");
			GoToSignInPageLink.Click();

			return new WorkspacePage(Driver).LoadPage();
		}

		public bool IsAdminEmailConfirmationPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(GO_TO_SIGN_IN_PAGE));
		}

		[FindsBy(How = How.XPath, Using = GO_TO_SIGN_IN_PAGE)]
		private IWebElement GoToSignInPageLink { get; set; }

		protected const string GO_TO_SIGN_IN_PAGE = "//a[text()='Go to sign-in page']";
	}
}
