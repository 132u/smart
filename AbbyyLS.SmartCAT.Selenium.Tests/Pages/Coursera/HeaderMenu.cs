using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	public class HeaderMenu : BaseObject, IAbstractPage<HeaderMenu>
	{
		public WebDriver Driver { get; protected set; }

		public HeaderMenu(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public HeaderMenu GetPage()
		{
			var header = new HeaderMenu(Driver);
			InitPage(header, Driver);

			return header;
		}
		
		public void LoadPage()
		{
			if (!IsHeaderMenuOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница лидеров.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на ссылку Leaderboard в меню.
		/// </summary>
		public LeaderboardPage ClickLeaderboardMenu()
		{
			CustomTestContext.WriteLine("Нажать на ссылку Leaderboard в меню.");
			LeaderboardMenu.Click();

			return new LeaderboardPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать на ссылку на профиль пользователя.
		/// </summary>
		public ProfilePage ClickProfile()
		{
			CustomTestContext.WriteLine("Нажать на ссылку на профиль пользователя.");
			Profile.Click();

			return new ProfilePage(Driver).GetPage();
		}

		#endregion

		#region Составные методы страницы

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что диалога формы авторизации исчез
		/// </summary>
		public bool IsSignInDialogDisappeared()
		{
			return Driver.WaitUntilElementIsDisappeared(By.XPath(SIGN_IN_BUTTON));
		}

		/// <summary>
		/// Проверить, что открылась страница лидеров.
		/// </summary>
		private bool IsHeaderMenuOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(LEADERBOARD));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = LEADERBOARD)]
		protected IWebElement LeaderboardMenu { get; set; }

		[FindsBy(How = How.XPath, Using = PROFILE)]
		protected IWebElement Profile { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string LEADERBOARD = ".//a[contains(@href,'/Leaderboard')]";
		protected const string PROFILE = ".//a[contains(@href,'/Profile')]";
		protected const string SIGN_IN_BUTTON = ".//input[@type ='submit']";

		#endregion
	}
}
