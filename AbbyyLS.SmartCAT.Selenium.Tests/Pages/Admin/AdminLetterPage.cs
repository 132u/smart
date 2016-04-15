using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminLetterPage: IAbstractPage<AdminLetterPage>
	{
		public WebDriver Driver { get; protected set; }

		public AdminLetterPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public AdminLetterPage LoadPage()
		{
			if (!IsAdminLetterPageOpened())
			{
				throw new Exception("Произошла ошибка: не открылась страница письма.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Кликнуть на ссылку актвации пользователя
		/// </summary>
		public AccountInvitationPage ClickInvintationLink()
		{
			CustomTestContext.WriteLine("Кликнуть на ссылку актвации пользователя.");
			InvintationLink.Click();

			return new AccountInvitationPage(Driver).LoadPage();
		}
		
		#endregion
		
		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница письма
		/// </summary>
		public bool IsAdminLetterPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(INVINTATION_LINK));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = INVINTATION_LINK)]
		protected IWebElement InvintationLink { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string INVINTATION_LINK = "//a[contains(@href, '/Account/UseAccountInvitation')]";
		
		#endregion
	}
}
