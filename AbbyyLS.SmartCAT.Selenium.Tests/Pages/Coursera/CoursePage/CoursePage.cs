using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera.CoursePage
{
	public class CoursePage : IAbstractPage<CoursePage>
	{
		public WebDriver Driver { get; protected set; }

		public CoursePage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public CoursePage LoadPage()
		{
			if (!IsCoursePageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница курса.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать по вкладке Лекции.
		/// </summary>
		public LecturesTab ClickLectureTab()
		{
			CustomTestContext.WriteLine("Нажать по вкладке Лекции.");
			LectureTab.Click();

			return new LecturesTab(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница курса.
		/// </summary>
		private bool IsCoursePageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(LECTURE_TAB), timeout: 60);
		}

		#endregion
		
		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = LECTURE_TAB)]
		protected IWebElement LectureTab { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string LECTURE_TAB = "//label[@for='tab_11']";

		#endregion
	}
}