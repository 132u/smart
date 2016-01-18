﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	public class CoursesPage : HeaderMenu, IAbstractPage<CoursesPage>
	{
		public WebDriver Driver { get; protected set; }

		public CoursesPage(WebDriver driver): base(driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public CoursesPage GetPage()
		{
			var coursesPage = new CoursesPage(Driver);
			InitPage(coursesPage, Driver);

			return coursesPage;
		}

		public void LoadPage()
		{
			if (!IsCoursesPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница курсов.");
			}
		}
		
		#region Простые методы страницы

		/// <summary>
		/// Кликнуть по ссылке на курс.
		/// </summary>
		/// <param name="courseName">название курса</param>
		public CoursePage ClickCourse(string courseName)
		{
			CustomTestContext.WriteLine("Кликнуть по ссылке на курс {0}.", courseName);
			Course = Driver.SetDynamicValue(How.XPath, COURSE, courseName);
			Course.Click();

			return new CoursePage(Driver).GetPage();
		}

		#endregion

		#region Составные методы страницы
		
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

		#region Объявление элементов страницы
		protected IWebElement Course { get; set; }

		#endregion

		#region Объявление элементов страницы

		protected const string COURSE = "//ul[contains(@class,'projects-list')]//span[contains(@class,'name')][contains(text(),'*#*')]/../../a";
		protected const string COURSE_LIST = "//ul[@class='projects-list']";

		#endregion
	}
}
