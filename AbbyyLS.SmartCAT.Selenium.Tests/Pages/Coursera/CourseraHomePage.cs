using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	public class CourseraHomePage : HeaderMenu, IAbstractPage<CourseraHomePage>
	{
		public WebDriver Driver { get; protected set; }

		public CourseraHomePage(WebDriver driver) : base(driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public CourseraHomePage GetPage()
		{
			var homePage = new CourseraHomePage(Driver);
			InitPage(homePage, Driver);

			return homePage;
		}

		public void LoadPage()
		{
			if (!IsCourseraHomePageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась главная страница coursera.");
			}
		}

		#region Простые методы страницы


		/// <summary>
		/// Получить никнейм пользователя.
		/// </summary>
		/// <returns>никнейм пользователя</returns>
		public string GetNickname()
		{
			CustomTestContext.WriteLine("Получить никнейм пользователя.");

			return Nickname.Text;
		}

		/// <summary>
		/// Получить имя автора перевода.
		/// </summary>
		/// <param name="translation">перевод</param>
		public string GetAuthorInEventList(string translation)
		{
			CustomTestContext.WriteLine("Получить имя автора перевода '{0}' в списке событий.", translation);
			AuthorInEventList = Driver.SetDynamicValue(How.XPath, AUTHOR_IN_EVENT_LIST, translation);
			AuthorInEventList.Scroll();

			return AuthorInEventList.Text.Replace("...", "");
		}

		/// <summary>
		/// Нажать кнопку Workspace.
		/// </summary>
		public ProjectsPage ClickWorkspaceButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Workspace.");
			WorkspaceButton.Click();

			return new ProjectsPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Workspace без ожидания открытия страницы
		/// </summary>
		public void ClickWorkspaceButtonWithoutWaiting()
		{
			CustomTestContext.WriteLine("Нажать кнопку Workspace без ожидания открытия страницы");
			WorkspaceButton.Click();
		}

		/// <summary>
		/// Нажать кнопку Join.
		/// </summary>
		public CourseraSignInDialog ClickJoinButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Join.");
			JoinButton.Click();

			return new CourseraSignInDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку выбора курсов.
		/// </summary>
		public CoursesPage ClickSelectCourse()
		{
			CustomTestContext.WriteLine("Нажать кнопку выбора курсов.");
			SelectCourseButton.Click();

			return new CoursesPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать на имя пользователя.
		/// </summary>
		public UserProfilePage ClickProfile()
		{
			CustomTestContext.WriteLine("Нажать на имя пользователя.");
			Nickname.ScrollAndClick();

			return new UserProfilePage(Driver).GetPage();
		}

		#endregion

		#region Составные методы страницы
		
		#endregion
		
		#region Методы, проверяющие состояние страницы
		
		/// <summary>
		/// Проверить, что открылась главная страница курсеры
		/// </summary>
		public bool IsCourseraHomePageOpened()
		{
			return IsSignInDialogDisappeared() && Driver.WaitUntilElementIsDisplay(By.XPath(LANGUAGE));
		}

		/// <summary>
		/// Проверить, что перевод отображается в списке событий.
		/// </summary>
		/// <param name="targetTranslation">перевод</param>
		public bool IsTargetTranslationDisplayedInEventList(string targetTranslation)
		{
			CustomTestContext.WriteLine("Проверить, что перевод {0} отображается в списке событий.", targetTranslation);
			TargetTranslationInEventList = Driver.SetDynamicValue(How.XPath, TARGET_TRANSLATION_INEVENT_LIST, targetTranslation);
			TargetTranslationInEventList.Scroll();

			return TargetTranslationInEventList.Displayed;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = JOIN_BUTTON)]
		protected IWebElement JoinButton { get; set; }

		[FindsBy(How = How.XPath, Using = SELECT_COURSE_BUTTON)]
		protected IWebElement SelectCourseButton { get; set; }

		[FindsBy(How = How.XPath, Using = WORKSPACE_BUTTON)]
		protected IWebElement WorkspaceButton { get; set; }

		[FindsBy(How = How.XPath, Using = NICKNAME)]
		protected IWebElement Nickname { get; set; }
		protected IWebElement TargetTranslationInEventList { get; set; }

		protected IWebElement AuthorInEventList { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string JOIN_BUTTON = ".//a[contains(@data-popup,'login-form')]";
		protected const string SELECT_COURSE_BUTTON = ".//a[contains(@href,'Courses') and @class='btn-green']";
		protected const string WORKSPACE_BUTTON = ".//a[contains(@href,'Workspace')]";
		protected const string LANGUAGE = "//div[@class='langSelect']";
		protected const string NICKNAME = "//a[@class='user-link']";
		protected const string TARGET_TRANSLATION_INEVENT_LIST = ".//table[@class='last-events']//tr//td[4]/span[contains(@data-bind,'target')][contains(text(),'*#*')]";
		protected const string AUTHOR_IN_EVENT_LIST = ".//table[@class='last-events']//tr//td[4]/span[contains(@data-bind,'target')][contains(text(),'*#*')]/../..//td[@class='col-1']//a[contains(@data-bind, 'userName')]";

		#endregion
	}
}
