using System;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.AssignmentPages
{
	public class DatePicker : TaskAssignmentPage, IAbstractPage<DatePicker>
	{
		public DatePicker(WebDriver driver)
			: base(driver)
		{
		}

		public new DatePicker GetPage()
		{
			var datePicker = new DatePicker(Driver);
			InitPage(datePicker, Driver);

			return datePicker;
		}

		public new void LoadPage()
		{
			if (!IsDatePickerOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся календарь.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Done
		/// </summary>
		public T ClickDoneButton<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Нажать кнопку Done.");
			DoneButton.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.GetPage();
		}

		/// <summary>
		/// Нажать на день
		/// </summary>
		/// <param name="day">день</param>
		public DatePicker ClickDay(int day)
		{
			CustomTestContext.WriteLine("Нажать на день {0}.", day);
			Day = Driver.SetDynamicValue(How.XPath, DAY, day.ToString());
			Day.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на стрелку для перехода к следующему месяцу.
		/// </summary>
		/// <param name="count"></param>
		public DatePicker ClickNextMonthArrow(int count = 1)
		{
			CustomTestContext.WriteLine("Нажать {0} раз на стрелку для перехода к следующему месяцу.", count);
			for (int i = 0; i < count; i++)
			{
				NextMonthArrow.Click();
			}

			return GetPage();
		}

		#endregion

		#region Составные методы страницы
		
		/// <summary>
		/// Установить дату
		/// </summary>
		/// <param name="day">день</param>
		/// <param name="nextMonth">следующий месяц</param>
		public T SetDate<T>(int day = 20, bool nextMonth = false) where T : class, IAbstractPage<T>
		{
			if (nextMonth)
			{
				ClickNextMonthArrow();
			}

			ClickDay(day);
			ClickDoneButton<T>();

			if(!Driver.WaitUntilElementIsDisappeared(By.XPath(DONE_BUTTON)))
			{
				throw new Exception("Произошла ошибка: Календарь не закрылся.");
			}

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что отркылся календарь
		/// </summary>
		public bool IsDatePickerOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DONE_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = DONE_BUTTON)]
		protected IWebElement DoneButton { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_MONTH_ARROW)]
		protected IWebElement NextMonthArrow { get; set; }

		protected IWebElement Day { get; set; }
		#endregion

		#region Описание XPath элементов

		protected const string DONE_BUTTON = "//button[contains(@class, 'datepicker-close')]";
		protected const string DAY = "//td[@data-handler='selectDay']//a[text()='*#*']";
		protected const string NEXT_MONTH_ARROW = "//div[contains(@class, 'header')]//a[@data-handler='next']";
		#endregion
	}
}
