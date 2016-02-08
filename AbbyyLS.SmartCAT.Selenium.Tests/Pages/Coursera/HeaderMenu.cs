using System;

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
		/// Получить никнейм пользователя в меню.
		/// </summary>
		/// <returns>никнейм пользователя</returns>
		public string GetNickname()
		{
			CustomTestContext.WriteLine("Получить никнейм пользователя в меню.");

			return Nickname.Text;
		}

		/// <summary>
		/// Перейти на страницу курсов.
		/// </summary>
		public CoursesPage GoToCoursesPage()
		{
			CustomTestContext.WriteLine("Перейти на страницу курсов.");
			Courses.Click();

			return new CoursesPage(Driver).GetPage();
		}

		/// <summary>
		/// Перейти на страницу Leaderboard.
		/// </summary>
		public LeaderboardPage GoToLeaderboardPage()
		{
			CustomTestContext.WriteLine("Перейти на страницу Leaderboard.");
			LeaderboardMenu.Click();

			return new LeaderboardPage(Driver).GetPage();
		}

		/// <summary>
		/// Перейти на страницу профиля пользователя.
		/// </summary>
		public UserProfilePage GoToUserProfile()
		{
			CustomTestContext.WriteLine("Перейти на страницу профиля пользователя.");
			Profile.Click();

			return new UserProfilePage(Driver).GetPage();
		}

		/// <summary>
		/// Обновить страницу
		/// </summary>
		public T RefreshPage<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Обновить страницу.");
			Driver.Navigate().Refresh();

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.GetPage();
		}

		/// <summary>
		/// Перейти на главную страницу.
		/// </summary>
		public CourseraHomePage GoToHomePage()
		{
			CustomTestContext.WriteLine("Перейти на главную страницу.");
			HomePage.Click();

			return new CourseraHomePage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Sign Out.
		/// </summary>
		public CourseraHomePage ClickSignOut()
		{
			CustomTestContext.WriteLine("Нажать кнопку Sign Out.");
			SignOut.Click();

			return new CourseraHomePage(Driver).GetPage();
		}

		#endregion

		#region Составные методы страницы

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что фото пользователя отображается в меню.
		/// </summary>
		public bool IsPhotoUserDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что фото пользователя  отображается в меню.");
			
			return Photo.GetAttribute("src").Contains("/avatar/");
		}

		/// <summary>
		/// Проверить, что отображается имя пользователя.
		/// </summary>
		public bool IsUserNicknameDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отображается имя пользователя.");

			return Photo.GetAttribute("src").Contains("/avatar/");
		}

		/// <summary>
		/// Проверить, что диалога формы авторизации исчез
		/// </summary>
		public bool IsSignInDialogDisappeared()
		{
			return Driver.WaitUntilElementIsDisappeared(By.XPath(SIGN_IN_FORM));
		}

		/// <summary>
		/// Проверить, что открылось меню.
		/// </summary>
		private bool IsHeaderMenuOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(LEADERBOARD));
		}

		/// <summary>
		/// Проверить, что диалог реадактирования профиля закрылся.
		/// </summary>
		public bool IsEditProfileDialogDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что диалог реадактирования профиля закрылся.");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(CHANGE_PASSWORD_TAB));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = LEADERBOARD)]
		protected IWebElement LeaderboardMenu { get; set; }

		[FindsBy(How = How.XPath, Using = PROFILE)]
		protected IWebElement Profile { get; set; }

		[FindsBy(How = How.XPath, Using = HOME_PAGE)]
		protected IWebElement HomePage { get; set; }

		[FindsBy(How = How.XPath, Using = NICKNAME)]
		protected IWebElement Nickname { get; set; }

		[FindsBy(How = How.XPath, Using = COURSES)]
		protected IWebElement Courses { get; set; }

		[FindsBy(How = How.XPath, Using = PHOTO)]
		protected IWebElement Photo { get; set; }

		[FindsBy(How = How.XPath, Using = SIGN_OUT)]
		protected IWebElement SignOut { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string CHANGE_PASSWORD_TAB = "//label[contains(@data-bind, 'changePassword().hasPassword()')]";
		protected const string COURSES = ".//a[contains(@href,'/Courses')]";
		protected const string LEADERBOARD = ".//a[contains(@href,'/Leaderboard')]";
		protected const string PROFILE = ".//a[contains(@href,'/Profile')]";
		protected const string SIGN_IN_BUTTON = ".//input[@type ='submit']";
		protected const string HOME_PAGE = ".//span[@class ='up']";
		protected const string NICKNAME = "//a[@class='user-name']";
		protected const string PHOTO = ".//div[@id='main-menu']//span[contains(@class,'menu-user-link')]/img";
		protected const string SIGN_OUT = "//button[contains(@data-bind, 'logout')]";
		protected const string SIGN_IN_FORM = "//div[@id='login-form']";

		#endregion
	}
}
