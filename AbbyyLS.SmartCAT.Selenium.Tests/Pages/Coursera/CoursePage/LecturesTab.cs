using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera.CoursePage
{
	public class LecturesTab : CoursePage, IAbstractPage<LecturesTab>
	{
		public LecturesTab(WebDriver driver):base(driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public LecturesTab LoadPage()
		{
			if (!IsLecturesPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница лекций на странице курса.");
			}

			return this;
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

			return new EditorPage(Driver).LoadPage();
		}

		#endregion
		
		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница лекций.
		/// </summary>
		private bool IsLecturesPageOpened()
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
