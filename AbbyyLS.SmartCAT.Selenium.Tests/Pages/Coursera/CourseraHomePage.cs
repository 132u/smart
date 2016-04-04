using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	public class CourseraHomePage : HeaderMenu, IAbstractPage<CourseraHomePage>
	{
		public CourseraHomePage(WebDriver driver) : base(driver)
		{
		}

		public CourseraHomePage GetPage()
		{
			CustomTestContext.WriteLine("Переход на страницу курсеры.", ConfigurationManager.CourseraUrl);

			Driver.Navigate().GoToUrl(ConfigurationManager.CourseraUrl);

			return new CourseraHomePage(Driver).LoadPage();
		}

		public new CourseraHomePage LoadPage()
		{
			if (!IsCourseraHomePageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась главная страница coursera.");
			}

			return this;
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

			return new ProjectsPage(Driver).LoadPage();
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

			return new CourseraSignInDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку выбора курсов.
		/// </summary>
		public CoursesPage ClickSelectCourse()
		{
			CustomTestContext.WriteLine("Нажать кнопку выбора курсов.");
			SelectCourseButton.Click();

			return new CoursesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на имя пользователя.
		/// </summary>
		public UserProfilePage ClickProfile()
		{
			CustomTestContext.WriteLine("Нажать на имя пользователя.");
			Nickname.ScrollAndClick();

			return new UserProfilePage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку голосования За.
		/// </summary>
		public CourseraHomePage ClickVoteUpButton(string translation)
		{
			CustomTestContext.WriteLine("Нажать кнопку голосования За.");
			VoteUpButton = Driver.SetDynamicValue(How.XPath, VOTE_UP_BUTTON, translation);
			VoteUpButton.Click();
			Driver.WaitUntilElementIsDisplay(By.XPath(VOTE_UP_LIKED_BUTTON));

			return LoadPage();
		}
		/// <summary>
		/// Проскролить до кнопки голосования За.
		/// </summary>
		public CourseraHomePage ScrollVoteUpButton(string translation)
		{
			CustomTestContext.WriteLine("Проскролить до кнопки голосования За.");
			VoteUpButton = Driver.SetDynamicValue(How.XPath, VOTE_UP_BUTTON, translation);
			VoteUpButton.Scroll();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку голосования Против.
		/// </summary>
		public CourseraHomePage ClickVoteDownButton(string translation)
		{
			CustomTestContext.WriteLine("Нажать кнопку голосования Против.");
			ScrollVoteDownButton(translation);
			VoteDownButton.Click();
			Driver.WaitUntilElementIsDisplay(By.XPath(VOTE_DOWN_LIKED_BUTTON));

			return LoadPage();
		}

		/// <summary>
		/// Проскролить до кнопки голосования Против.
		/// </summary>
		public CourseraHomePage ScrollVoteDownButton(string translation)
		{
			CustomTestContext.WriteLine("Проскролить до кнопки голосования Против.");
			VoteDownButton = Driver.SetDynamicValue(How.XPath, VOTE_DOWN_BUTTON, translation);
			VoteDownButton.Scroll();

			return LoadPage();
		}

		/// <summary>
		/// Проскролить до количества голосов.
		/// </summary>
		public CourseraHomePage ScrollToVoteCount(string translation)
		{
			CustomTestContext.WriteLine("Проскролить до количества голосов.");
			VoteCount = Driver.SetDynamicValue(How.XPath, VOTE_COUNT, translation);
			VoteCount.Scroll();

			return LoadPage();
		}

		/// <summary>
		/// Получить количество голосов за перевод.
		/// </summary>
		public int GetVoteCount(string translation)
		{
			CustomTestContext.WriteLine("Получить количество голосов за перевод {0}.", translation);
			ScrollToVoteCount(translation);
			int result;

			if (!int.TryParse(VoteCount.Text, out result))
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось преобразование количества голосов {0} в число.", VoteCount.Text));
			}

			return result;
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
		protected IWebElement VoteUpButton { get; set; }
		protected IWebElement VoteDownButton { get; set; }
		protected IWebElement AuthorInEventList { get; set; }
		protected IWebElement VoteCount { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string JOIN_BUTTON = ".//a[contains(@data-popup,'login-form')]";
		protected const string SELECT_COURSE_BUTTON = ".//a[contains(@href,'Courses') and @class='btn-green']";
		protected const string WORKSPACE_BUTTON = ".//a[contains(@href,'Workspace')]";
		protected const string LANGUAGE = "//div[@class='langSelect']";
		protected const string NICKNAME = "//a[@class='user-link']";
		protected const string TARGET_TRANSLATION_INEVENT_LIST = ".//table[@class='last-events']//tr//td[4]/span[contains(@data-bind,'target')][contains(text(),'*#*')]";
		protected const string AUTHOR_IN_EVENT_LIST = ".//table[@class='last-events']//tr//td[4]/span[contains(@data-bind,'target')][contains(text(),'*#*')]/../..//td[@class='col-1']//a[contains(@data-bind, 'userName')]";
		protected const string VOTE_UP_BUTTON = ".//table[@class='last-events']//tr//td[@class='col-4']//span[contains(text(),'*#*')]//..//..//td[@class='col-5']//div[@class='like']";
		protected const string VOTE_UP_LIKED_BUTTON = ".//table[@class='last-events']//tr//td[@class='col-4']//span[contains(text(),'*#*')]//..//..//td[@class='col-5']//div[@class='liked']";
		protected const string VOTE_DOWN_LIKED_BUTTON = ".//table[@class='last-events']//tr//td[@class='col-4']//span[contains(text(),'*#*')]//..//..//td[@class='col-5']//div[@class='disliked']";
		protected const string VOTE_COUNT = ".//table[@class='last-events']//tr//td[@class='col-4']//span[contains(text(),'*#*')]//..//..//td[@class='col-5']//span[@data-bind='text: rating']";
		protected const string VOTE_DOWN_BUTTON = ".//table[@class='last-events']//tr//td[@class='col-4']//span[contains(text(),'*#*')]//..//..//td[@class='col-5']//div[@class='dislike']";

		#endregion
	}
}
