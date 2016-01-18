using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	public class CoursePage : BaseObject, IAbstractPage<CoursePage>
	{
		public WebDriver Driver { get; protected set; }

		public CoursePage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public CoursePage GetPage()
		{
			var coursePage = new CoursePage(Driver);
			InitPage(coursePage, Driver);

			return coursePage;
		}

		public void LoadPage()
		{
			if (!IsCoursePageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница курса.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Открыть лекцию.
		/// </summary>
		/// <param name="lectureNumber">номер лекции</param>
		public EditorPage OpenLecture(int lectureNumber = 1)
		{
			CustomTestContext.WriteLine("Открыть лекцию №{0}.", lectureNumber);
			Lecture = Driver.SetDynamicValue(How.XPath, LECTURE, lectureNumber.ToString());
			Lecture.Click();

			return new EditorPage(Driver).GetPage();
		}

		#endregion

		#region Составные методы страницы


		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница курса.
		/// </summary>
		private bool IsCoursePageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(LECTURES_TABLE));
		}

		#endregion
		
		#region Объявление элементов страницы

		protected IWebElement Lecture { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string LECTURES_TABLE = ".//tbody[contains(@data-bind, 'lectures')]";
		protected const string LECTURE = ".//tbody[contains(@data-bind,'lectures')]//tr[*#*]//a";

		#endregion
	}
}