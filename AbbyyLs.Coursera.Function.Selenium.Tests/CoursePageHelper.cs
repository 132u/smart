using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.Coursera.Function.Selenium.Tests
{
	public class CoursePageHelper : CommonHelper
	{
		public CoursePageHelper(IWebDriver driver, WebDriverWait wait) :
            base (driver, wait)
        {
        }

		/// <summary>
		/// Дождаться, пока будет получен список курсов
		/// </summary>
		/// <returns>Список курсов виден</returns>
		public bool WaitUntilCourseListDisplay()
		{
			if (!WaitUntilDisplayElement(By.XPath(CERTAIN_COURSE_XPATH + "[2]")))
			{
				RefreshPage();
				if (!WaitUntilDisplayElement(By.XPath(CERTAIN_COURSE_XPATH + "[2]")))
					return false;
				else
					return true;
			}
			else
				return true;
		}
		
		/// <summary>
		/// Переход к курсу по имени курса
		/// </summary>
		public void OpenCourseByName(string courseName)
		{
			ClickElement(By.XPath(COURSES_XPATH + COURSES_NAMES_XPATH + "[contains(text(),'" + courseName.Trim() + "')]/../../a"));
		}

		/// <summary>
		/// Возвращает процент перевода по имени курса
		/// </summary>
		/// <param name="courseName">Имя курса</param>
		/// <returns>Процент перевода</returns>
		public Decimal GetCourseProcentByName(string courseName)
		{
			string xPath = COURSES_XPATH + COURSES_NAMES_XPATH + "[contains(text(),'" + courseName.Trim() + "')]/../../a/../../td" + COURSES_PROCENT_XPATH;
			string courseProcent = GetTextElement(By.XPath(xPath)).
				Replace("%", "").
				Replace(".", ",");

			Decimal courseProgress = Decimal.Parse(courseProcent);

			return courseProgress;
		}
		
		/// <summary>
		/// Возвращает список названий курсов
		/// </summary>
		/// <returns>Список названий курсов</returns>
		public List<string> GetCoursesNameList()
		{
			List<string> courseList = GetTextListElement(By.XPath(COURSES_XPATH + COURSES_NAMES_XPATH));
			return courseList;
		}



		protected const string COURSES_XPATH = "//ul[contains(@class,'projects-list')]";
		protected const string CERTAIN_COURSE_XPATH = COURSES_XPATH + "//table";
		protected const string COURSES_NAMES_XPATH = "//div[contains(@class,'name')]";
		protected const string COURSES_PROCENT_XPATH = "//span[contains(@class,'percent')]";
	}
}
