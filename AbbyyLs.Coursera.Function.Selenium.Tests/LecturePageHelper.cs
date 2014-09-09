using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.Coursera.Function.Selenium.Tests
{
	public class LecturePageHelper : CommonHelper
	{
		public LecturePageHelper(IWebDriver driver, WebDriverWait wait) :
            base (driver, wait)
        {
        }

		/// <summary>
		/// Возвращает открылась ли страница лекций
		/// </summary>
		/// <returns>Старница лекций открыта</returns>
		public bool WaitUntilDisplayLecturesList()
		{
			if (!WaitUntilDisplayElement(By.XPath(LECTURES_XPATH)) ||
				GetTextElement(By.XPath(COURSE_NAME_XPATH)) == "Natural Language Processing")
			{
				RefreshPage();
				if (!WaitUntilDisplayElement(By.XPath(LECTURES_XPATH)) &&
				GetTextElement(By.XPath(COURSE_NAME_XPATH)) == "Natural Language Processing")
					return false;
				else
					return true;
			}
			else
				return true;
		}

		/// <summary>
		/// Переход в лекцию по номеру строки
		/// </summary>
		/// <param name="rowNumber">номер строки с лекцией</param>
		public void ClickLectureByRowNum(int rowNumber)
		{
			ClickElement(By.XPath(LECTURES_XPATH + "//tr[" + rowNumber + "]//a"));
		}

		/// <summary>
		/// Возвращает процент собственного прогресса по номеру лекции
		/// </summary>
		/// <param name="lectureNumber">Порядковый номер лекции</param>
		/// <returns>Процент собственного прогресса</returns>
		public int GetLecturePersonalProgressByNumber(int lectureNumber)
		{
			string xPath = LECTURES_XPATH + "//" + LECTURE_POSITION_XPATH + 
				"[contains(text(),'" + lectureNumber.ToString() + "')]/.." + 
				LECTURE_OWN_PROCENT_XPATH;

			string lectureProcent = GetTextElement(By.XPath(xPath)).
				Replace("%", "").
				Replace(".", ",");

			int lectureProgress = Int32.Parse(lectureProcent);

			return lectureProgress;
		}

		/// <summary>
		/// Возвращает процент общего прогресса по номеру лекции
		/// </summary>
		/// <param name="lectureNumber">Порядковый номер лекции</param>
		/// <returns>Процент общего прогресса</returns>
		public int GetLectureGeneralProgressByNumber(int lectureNumber)
		{
			string xPath = LECTURES_XPATH + "//" + LECTURE_POSITION_XPATH + 
				"[contains(text(),'" + lectureNumber.ToString() + "')]/.." + 
				LECTURE_GENERAL_PROCENT_XPATH;

			string lectureProcent = GetTextElement(By.XPath(xPath)).
				Replace("%", "").
				Replace(".", ",");

			int lectureProgress = Int32.Parse(lectureProcent);

			return lectureProgress;
		}

		/// <summary>
		/// Возвращает список названий лекций
		/// </summary>
		/// <returns>Список названий лекций</returns>
		public List<string> GetLecturesNameList()
		{
			List<string> lectureList = GetTextListElement(By.XPath(LECTURES_XPATH + LECTURE_NAME_XPATH));
			return lectureList;
		}

		/// <summary>
		/// Возвращает порядковый номер по имени лекции
		/// </summary>
		/// <param name="lectureName">Имя лекции</param>
		/// <returns>Порядковый номер лекции</returns>
		public int GetLectureRowNumberByName(string lectureName)
		{
			string xPath = LECTURES_XPATH + LECTURE_NAME_XPATH + 
				"[contains(text(),'" + lectureName.Trim() + "')]/../../" + 
				LECTURE_POSITION_XPATH;

			string lecturePosition = GetTextElement(By.XPath(xPath));

			int lectureRowNumber = Int32.Parse(lecturePosition);

			return lectureRowNumber;
		}



		protected const string COURSE_NAME_XPATH = ".//span[@id='course-title']";
		protected const string LECTURES_XPATH = ".//tbody[contains(@data-bind,'lectures')]";
		protected const string LECTURE_NAME_XPATH = "//a[contains(@data-bind,'name')]";
		protected const string LECTURE_OWN_PROCENT_XPATH = "//div[contains(@data-bind,'personalProgressView')]";
		protected const string LECTURE_GENERAL_PROCENT_XPATH = "//div[contains(@data-bind,'progressView')]";
		protected const string LECTURE_POSITION_XPATH = "td[contains(@data-bind,'position')]";

	}
}
