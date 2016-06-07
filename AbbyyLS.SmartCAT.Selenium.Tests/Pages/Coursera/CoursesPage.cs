using System;
using System.Globalization;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	public class CoursesPage : HeaderMenu, IAbstractPage<CoursesPage>
	{
		public CoursesPage(WebDriver driver): base(driver)
		{
		}

		public new CoursesPage LoadPage()
		{
			if (!IsCoursesPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница курсов.");
			}

			return this;
		}
		
		#region Простые методы страницы

		/// <summary>
		/// Кликнуть по ссылке на курс.
		/// </summary>
		/// <param name="courseName">название курса</param>
		public CoursePage.CoursePage ClickCourse(string courseName)
		{
			CustomTestContext.WriteLine("Кликнуть по ссылке на курс {0}.", courseName);
			Course = Driver.SetDynamicValue(How.XPath, COURSE, courseName);
			Course.Click();

			return new CoursePage.CoursePage(Driver).LoadPage();
		}

		/// <summary>
		/// Получить процент прогресса курса.
		/// </summary>
		public double GetCourseProgress(string courseName)
		{
			CustomTestContext.WriteLine("Получить процент прогресса курса.");
			CoursePercent = Driver.SetDynamicValue(How.XPath, COURSE_PERCENT, courseName);
			var progress = CoursePercent.Text.Replace("%", "");
			double result;

			if (!double.TryParse(progress, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось преобразование значения {0} прогресс бара курса.", progress));
			}

			return result;
		}

		#endregion

		#region Методы, проверяющие состояние страницы
		
		/// <summary>
		/// Проверить, что открылась страница курсов
		/// </summary>
		private bool IsCoursesPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(COURSE_LIST));
		}

		#endregion

		#region Методы, ожидающие определенного состояния страницы

		/// <summary>
		/// Подождать, когда курс появиться в списке.
		/// </summary>
		/// <param name="courseName">название курса</param>
		public CoursesPage WaitCourseNameDisplayed(string courseName)
		{
			CustomTestContext.WriteLine("Подождать, когда курс {0} появится в списке.", courseName);
			var Course = Driver.SetDynamicValue(How.XPath, COURSE, courseName);
			int i = 0;
			while (i < 60)
			{
				if (Course.Displayed)
				{
					return LoadPage();
				}

				Thread.Sleep(1000);
				RefreshPage<CoursesPage>();
				i++;
			}

			throw new Exception("Курс не появился в списке.");
		}

		/// <summary>
		/// Подождать, когда обновится прогресс курса.
		/// </summary>
		public CoursesPage WaitCourseProgressChanged(string courseName, double countBefore)
		{
			CustomTestContext.WriteLine("Подождать, когда обновится прогресс '{0}' курса '{1}'.", countBefore, courseName);
			int i = 0;
			while (i < 60)
			{
				if (countBefore != GetCourseProgress(courseName))
				{
					return LoadPage();
				}

				Thread.Sleep(1000);
				RefreshPage<CoursesPage>();
				i++;
			}

			throw new Exception("Прогресс курса не обновился.");
		}
		#endregion

		#region Объявление элементов страницы
		protected IWebElement Course { get; set; }

		protected IWebElement CoursePercent { get; set; }

		#endregion

		#region Объявление элементов страницы

		protected const string COURSE = "//ul[contains(@class,'projects-list')]//span[contains(@class,'name')][contains(text(),'*#*')]/../../a";
		protected const string COURSE_LIST = "//ul[@class='projects-list']";
		protected const string COURSE_PERCENT = "//span[text()='*#*']/ancestor::td//preceding-sibling::td//span[@class='percent']";
		#endregion
	}
}
