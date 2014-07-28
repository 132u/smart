using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.Coursera.Function.Selenium.Tests
{
	public class LeaderboardPageHelper : CommonHelper
	{
		public LeaderboardPageHelper(IWebDriver driver, WebDriverWait wait) :
            base (driver, wait)
        {
        }

		/// <summary>
		/// Дождаться, пока изменится общее количество лидеров
		/// Необходимо для ожидания смены курса при выборе из выпадающего списка
		/// </summary>
		/// <param name="totalNum">Общее количество лидеров до выбора курса из списка</param>
		/// <returns>Общее количество лидеров изменилось</returns>
		public bool WaitUntilLeadersQuantityChanged(int totalNum)
		{
			return WaitUntilDisplayElement(By.XPath(LEADERS_QUANTITY + 
				"[not(contains(text()," + totalNum.ToString() + "))]"));
		}
		
		/// <summary>
		/// Вернуть, есть ли пользователь в списке лидерборда в активном списке
		/// (без учета, что пользователь может отображаться до или после списка из 10 человек - активный список из ссылок)
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <returns>есть пользователь</returns>
		public bool GetIsUserLeaderboardActiveList(string userName)
		{
			return GetIsElementExist(By.XPath(LEADERS_NAME_XPATH + "/a[contains(text(),'" + userName + "')]"));
		}

		/// <summary>
		/// Вернуть, есть ли пользователь в списке ниже лидерборда
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <returns>есть пользователь</returns>
		public bool GetIsUserLeaderboardDownList(string userName)
		{
			return GetIsElementExist(By.XPath(LEADERS_NAME_XPATH + "[contains(text(),'" + userName + "')]"));
		}

		/// <summary>
		/// Возвращает имя пользователя
		/// </summary>
		/// <returns>Имя пользователя</returns>
		public string GetUserName()
		{
			string userName = "";

			if (GetIsElementExist(By.XPath(LEADERS_XPATH + 
				"//tr[contains(@class,'active') and not(@disabled)]//td//a[contains(@data-bind,'name')]")))
			{
				userName = GetTextElement(By.XPath(LEADERS_XPATH +
				"//tr[contains(@class,'active') and not(@disabled)]//td//a[contains(@data-bind,'name')]"));
			}
			else if (GetIsElementExist(By.XPath(LEADERS_XPATH + 
				"//tr[contains(@class,'active') and @style='']//td[contains(@data-bind,'name')]")))
			{
				userName = GetTextElement(By.XPath(LEADERS_XPATH +
				"//tr[contains(@class,'active') and @style='']//td[contains(@data-bind,'name')]"));
			}
			
			return userName;
		}

		/// <summary>
		/// Возвращает, есть ли аватар у пользователя
		/// </summary>
		/// <returns>Есть аватар</returns>
		public bool GetIsAvatarPresentLeaderboard()
		{
			if (GetIsElementExist(By.XPath(LEADERS_XPATH +
				"//tr[contains(@class,'active') and not(@disabled)]//td//a[contains(@data-bind,'name')]")))
			{
				return GetElement(By.XPath(LEADERS_XPATH + 
					"//tr[contains(@class,'active') and not(@disabled)]//td//img")).
					GetAttribute("src").Contains("/avatar/");
			}
			else if (GetIsElementExist(By.XPath(LEADERS_XPATH +
				"//tr[contains(@class,'active') and @style='']//td[contains(@data-bind,'name')]")))
			{
				return GetElement(By.XPath(LEADERS_XPATH + 
					"//tr[contains(@class,'active') and @style='']//td//img")).
					GetAttribute("src").Contains("/avatar/");
			}

			return false;
		}

		/// <summary>
		/// Возвращает строку диапазона отображаемых лидеров
		/// </summary>
		/// <returns>Строка диапазона</returns>
		public string GetRangeLeaderList()
		{
			return GetTextElement(By.XPath(HEADER_RANGE_XPATH));
		}

		/// <summary>
		/// Возвращает общее количество пользователей
		/// </summary>
		/// <returns>Общее количество пользователей</returns>
		public int GetLeadersQuantity()
		{
			return int.Parse(GetTextElement(By.XPath(LEADERS_QUANTITY)));
		}
		
		/// <summary>
		/// Возвращает имя пользователя по позиции в активном листе
		/// </summary>
		/// <param name="rowNumber">Номер позиции в списке</param>
		/// <returns>Имя пользователя</returns>
		public string GetNameByRowNumber(int rowNumber)
		{
			// Позиция пользователя суммируется с невидимыми тремя строчками выше активного списка
			return GetTextElement(By.XPath(LEADERS_XPATH + "//tr[" + (rowNumber + 3).ToString() + "]//td[3]//a[contains(@data-bind,'name')]"));
		}

		/// <summary>
		/// Возвращает, присутствует ли пользователь по номеру строки
		/// </summary>
		/// <param name="rowNumber">Номер позиции в списке</param>
		/// <returns>Пользователь присутствует</returns>
		public bool GetIsNamePresentByRowNumber(int rowNumber)
		{
			// Позиция пользователя суммируется с невидимыми тремя строчками выше активного списка
			return GetIsElementExist(By.XPath(LEADERS_XPATH + "//tr[" + (rowNumber + 3).ToString() + "]//td[3]//a[contains(@data-bind,'name')]"));
		}

		/// <summary>
		/// Возвращает рейтинг пользователя
		/// </summary>
		/// <returns>Рейтинг</returns>
		public Decimal GetRaitingActiveUser()
		{
			return Decimal.Parse(GetTextElement(By.XPath(ACTIVE_USER_RATING_XPATH)).
				Trim().Replace(".", ","));
		}

		/// <summary>
		/// Кликнуть по пользователю
		/// </summary>
		/// <param name="rowNumber">Номер позиции в списке</param>
		public void ClickNameByRowNumber(int rowNumber)
		{
			// Позиция пользователя суммируется с невидимыми тремя строчками выше активного списка
			ClickElement(By.XPath(LEADERS_XPATH + "//tr[" + (rowNumber + 3).ToString() + "]//td[3]//a[contains(@data-bind,'name')]"));
		}
		
		/// <summary>
		/// Кликнуть по выпадающему списку курсов
		/// </summary>
		public void OpenCoursesList()
		{
			ClickElement(By.Id(COURSES_LIST_ID));
			WaitUntilDisplayElement(By.XPath(COURSES_IN_LIST_XPATH));
		}

		/// <summary>
		/// Кликнуть по курсу из выпадающего списка
		/// </summary>
		/// <param name="courseName">Имя курса</param>
		public void SelectCourseByName(string courseName)
		{
			IList<IWebElement> courseList = GetElementList(By.XPath(COURSES_IN_LIST_XPATH));
			foreach (IWebElement course in courseList)
			{
				if (course.Text == courseName)
				{
					course.Click();
					SendKeys.SendWait(@"{Enter}");
					break;
				}
			}
		}

		/// <summary>
		/// Возвращает список курсов
		/// </summary>
		/// <returns>Список курсов</returns>
		public List<string> GetCoursesList()
		{
			return GetTextListElement(By.XPath(COURSES_IN_LIST_XPATH));
		}

		/// <summary>
		/// Перейти на следующую страницу
		/// </summary>
		public void OpenNextPage()
		{
			string range = GetRangeLeaderList();

			ClickElement(By.XPath(HEADER_NEXT_BTN_XPATH));

			WaitUntilDisplayElement(By.XPath(HEADER_RANGE_XPATH + "[not(contains(text(),'" + range + "'))]"));
		}



		protected const string COURSES_LIST_ID = "select_courses_rat";
		protected const string COURSES_IN_LIST_XPATH = "//select[@id='select_courses_rat']/option";

		protected const string HEADER_XPATH = "//div[contains(@class,'tabs_rat_filtr')]";
		protected const string LEADERS_XPATH = "//div[@class='rating']";

		protected const string LEADERS_NAME_XPATH = LEADERS_XPATH + 
			"//tr[not(contains(@style,'display: none;'))]//td[3]";

		protected const string ACTIVE_USER_RATING_XPATH = LEADERS_XPATH +
			"//tr[not(contains(@style,'display: none;'))][contains(@class,'active')]//td[contains(@data-bind,'rating')]";
		
		protected const string LEADERS_QUANTITY = HEADER_XPATH + "//div[contains(@data-bind,'total')]";
		
		protected const string HEADER_RANGE_XPATH = HEADER_XPATH + "//div[contains(@data-bind,'currentPageRange')]";
		protected const string HEADER_NEXT_BTN_XPATH = HEADER_XPATH + "//a[contains(@data-bind,'nextPage')]";
	}
}
