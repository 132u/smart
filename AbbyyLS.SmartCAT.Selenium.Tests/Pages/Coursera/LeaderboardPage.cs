using System;
using System.Globalization;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	public class LeaderboardPage : HeaderMenu, IAbstractPage<LeaderboardPage>
	{
		public WebDriver Driver { get; protected set; }

		public LeaderboardPage(WebDriver driver): base(driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public LeaderboardPage GetPage()
		{
			var leaderboardPage = new LeaderboardPage(Driver);
			InitPage(leaderboardPage, Driver);

			return leaderboardPage;
		}

		public void LoadPage()
		{
			if (!IsLeaderboardPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница лидеров.");
			}
		}
		
		#region Простые методы страницы

		/// <summary>
		/// Нажать на имя пользователя.
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public UserProfilePage ClickUserName(string userName)
		{
			CustomTestContext.WriteLine("Нажать на имя пользователя {0}.", userName);
			UserName = Driver.SetDynamicValue(How.XPath, USER_NAME_IN_LIST, userName);
			UserName.Click();

			return new UserProfilePage(Driver).GetPage();
		}

		/// <summary>
		/// Раскрыть список курсов.
		/// </summary>
		public LeaderboardPage ExpandCoursesDropdown()
		{
			CustomTestContext.WriteLine("Раскрыть список курсов.");
			CoursesDropdown.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по курсу в дропдауне.
		/// </summary>
		public LeaderboardPage ClickCourseOption(string courseName)
		{
			CustomTestContext.WriteLine("Кликнуть по курсу '{0}' в дропдауне.", courseName);
			CourseOption = Driver.SetDynamicValue(How.XPath, COURSE_OPTION, courseName);
			CourseOption.Click();

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать курс.
		/// </summary>
		/// <param name="courseName">название курса</param>
		public LeaderboardPage SelectCourse(string courseName)
		{
			CustomTestContext.WriteLine("Выбрать курс '{0}'.", courseName);
			ExpandCoursesDropdown();
			ClickCourseOption(courseName);

			return GetPage();
		}

		#endregion
		
		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница лидеров.
		/// </summary>
		private bool IsLeaderboardPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.Id(COURSES_DROPDOWN));
		}
		
		/// <summary>
		/// Проверить, что фото пользователя в списке лидеров.
		/// </summary>
		public new bool IsPhotoUserDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что фото пользователя отображается в списке лидеров.");

			return UserPhoto.GetAttribute("src").Contains("/avatar/");

		}

		/// <summary>
		/// Проверить, что пользователь присутствует в списке лидеров.
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public bool IsUserNameDisplayed(string userName)
		{
			CustomTestContext.WriteLine("Проверить, что пользователь {0} присутствует в списке лидеров.", userName);
			UserName = Driver.SetDynamicValue(How.XPath, USER_NAME_IN_LIST, userName);

			return UserName.Displayed;
		}

		#endregion
		
		#region Вспомогательные методы

		/// <summary>
		/// Получить номер позиции пользователя в списке лидеров.
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public int GetUserLeaderboardPositionNumber(string userName)
		{
			CustomTestContext.WriteLine("Получить номер позиции пользователя {0} в списке лидеров.", userName);
			var position = Driver.SetDynamicValue(How.XPath, USER_POSITION_IN_LIST, userName).Text;
			int positionNubmer;

			if (!int.TryParse(position, out positionNubmer))
			{
				throw new Exception(string.Format("Произошла ошибка:\nНе удалось преобразование номера позиции {0} пользователя в списке лидеров в число.", position));
			}

			return positionNubmer;
		}

		/// <summary>
		/// Получить имя пользователя в таблице лидеров по номеру строки
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public string GetUserNameInList(int rowNumber = 1)
		{
			CustomTestContext.WriteLine("Получить имя пользователя в таблице лидеров строка №{0}.", rowNumber);
			UserRow = Driver.SetDynamicValue(How.XPath, USER_ROW, (rowNumber + 3).ToString());

			return UserRow.Text;
		}

		/// <summary>
		/// Получить рейтинг пользователя в списке лидеров.
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public double GetUserLeaderboardRating(string userName)
		{
			CustomTestContext.WriteLine("Получить рейтинг пользователя {0} в списке лидеров.", userName);
			var position = UserRating.Text;
			double positionNubmer;

			if (!double.TryParse(position, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out positionNubmer))
			{
				throw new Exception(string.Format("Произошла ошибка:\nНе удалось преобразование рейтинга {0} пользователя в списке лидеров в число.", position));
			}

			return positionNubmer;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = USER_RATING_IN_LIST)]
		protected IWebElement UserRating { get; set; }

		[FindsBy(How = How.Id, Using = COURSES_DROPDOWN)]
		protected IWebElement CoursesDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = USER_PHOTO)]
		protected IWebElement UserPhoto { get; set; }
		
		protected IWebElement CourseOption { get; set; }

		protected IWebElement UserName { get; set; }
		protected IWebElement UserRow { get; set; }
		#endregion

		#region Описание XPath элементов

		protected const string USER_NAME_IN_LIST = "//div[@class='rating']//tr[not(contains(@style,'display: none;'))]//td[3]//a[contains(text(),'*#*')]";
		protected const string USER_POSITION_IN_LIST = "//div[@class='rating']//tr[not(contains(@style,'display: none;'))]//td[3]//a[contains(text(),'*#*')]//..//..//td[1]";
		protected const string USER_RATING_IN_LIST = "//div[@class='rating']//tr[not(contains(@style,'display: none;'))][contains(@class,'active')]//td[contains(@data-bind,'rating')]";
		protected const string COURSES_DROPDOWN = "select_courses_rat";
		protected const string COURSE_OPTION = "//select[@id='select_courses_rat']/option[text() = '*#*']";
		protected const string USER_PHOTO = "//div[@class='rating']//tr[contains(@class,'active')]//td//img";
		protected const string USER_ROW = "//div[@class='rating']//tr[*#*]//td[3]//a[contains(@data-bind,'name')]";

		#endregion
	}
}
