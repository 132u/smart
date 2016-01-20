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

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = JOIN_BUTTON)]
		protected IWebElement JoinButton { get; set; }

		[FindsBy(How = How.XPath, Using = SELECT_COURSE_BUTTON)]
		protected IWebElement SelectCourseButton { get; set; }

		[FindsBy(How = How.XPath, Using = WORKSPACE_BUTTON)]
		protected IWebElement WorkspaceButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string JOIN_BUTTON = ".//a[contains(@data-popup,'login-form')]";
		protected const string SELECT_COURSE_BUTTON = ".//a[contains(@href,'Courses') and @class='btn-green']";
		protected const string WORKSPACE_BUTTON = ".//a[contains(@href,'Workspace')]";
		protected const string LANGUAGE = "//div[@class='langSelect']";
		#endregion
	}
}
