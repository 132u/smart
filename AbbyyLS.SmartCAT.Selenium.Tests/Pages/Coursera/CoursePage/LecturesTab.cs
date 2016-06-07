using System;
using System.Collections.Generic;

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

		/// <summary>
		/// Открыть лекцию.
		/// </summary>
		/// <param name="lectureNumber">название лекции</param>
		public EditorPage OpenLecture(string lecture)
		{
			CustomTestContext.WriteLine("Открыть лекцию '{0}'.", lecture);
			LectureName = Driver.SetDynamicValue(How.XPath, LECTURE_NAME, lecture);
			LectureName.ScrollAndClick();

			return new EditorPage(Driver).LoadPage();
		}

		/// <summary>
		/// Получить список названий лекций.
		/// </summary>
		public int GetLectureNamesCount()
		{
			CustomTestContext.WriteLine("Получить список названий лекций.");

			return Driver.GetElementsCount(By.XPath(LECTURE_NAME_LIST));
		}

		/// <summary>
		/// Получить номер первой лекции с пустым личным прогресс баром.
		/// </summary>
		public int GetLectureNumberWithEmptyPersonalProgress()
		{
			CustomTestContext.WriteLine("Получить номер первой лекции с пустым личным прогресс баром.");
			for (int i =1; i < GetLectureNamesCount(); i++)
			{
				if (GetPersonalProgressValueByLectureNumber(i).ToString() == "0")
				{
					return i;
				}
			}

			throw new Exception("Произошла ошибка:\n Нет подходящей лекции с пустым личным прогресс баром.");
		}

		/// <summary>
		/// Получить номер первой лекции с пустым общим прогресс баром.
		/// </summary>
		public int GetLectureNumberWithEmptyCommonProgress()
		{
			CustomTestContext.WriteLine("Получить номер первой лекции с пустым общим прогресс баром.");
			for (int i =1; i < GetLectureNamesCount(); i++)
			{
				if (GetCommonProgressValueByLectureNumber(i).ToString() == "0")
				{
					return i;
				}
			}

			throw new Exception("Произошла ошибка:\n Нет подходящей лекции с пустым общим прогресс баром.");
		}

		/// <summary>
		/// Получить значение личного прогресс бара для лекции.
		/// </summary>
		/// <param name="lectureNumber">номер лекции</param>
		public int GetPersonalProgressValueByLectureNumber(int lectureNumber)
		{
			CustomTestContext.WriteLine("Получить значение личного прогресс бара для лекции №{0}.", lectureNumber);
			PersonalProgressByLectureNumber = Driver.SetDynamicValue(How.XPath, PERSONAL_PROGRESS_BY_LECTURE_NUMBER, lectureNumber.ToString());
			var progress = PersonalProgressByLectureNumber.Text.Replace("%", "");
			int result;

			if (!int.TryParse(progress, out result))
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось преобразование значения {0} из личного прогресс бара.", progress));
			}

			return result;
		}

		/// <summary>
		/// Получить значение личного прогресс бара для лекции.
		/// </summary>
		/// <param name="lecture">название лекции</param>
		public int GetPersonalProgressValuebyLectureName(string lecture)
		{
			CustomTestContext.WriteLine("Получить значение личного прогресс бара для лекции '{0}'.", lecture);
			PersonalProgressByLectureName = Driver.SetDynamicValue(How.XPath, PERSONAL_PROGRESS_BY_LECTURE_NAME, lecture);
			var progress = PersonalProgressByLectureName.Text.Replace("%", "");
			int result;

			if (!int.TryParse(progress, out result))
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось преобразование значения {0} из личного прогресс бара.", progress));
			}

			return result;
		}

		/// <summary>
		/// Получить значение общего прогресс бара для лекции.
		/// </summary>
		/// <param name="lectureNumber">номер лекции</param>
		public int GetCommonProgressValueByLectureNumber(int lectureNumber)
		{
			CustomTestContext.WriteLine("Получить значение личного прогресс бара для лекции №{0}.", lectureNumber);
			CommonProgressByLectureNumber = Driver.SetDynamicValue(How.XPath, COMMON_PROGRESS_BY_LECTURE_NUMBER, lectureNumber.ToString());
			var progress = CommonProgressByLectureNumber.Text.Replace("%", "");
			int result;

			if (!int.TryParse(progress, out result))
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось преобразование значения {0} из общего прогресс бара.", progress));
			}

			return result;

		}

		/// <summary>
		/// Получить значение общего прогресс бара для лекции.
		/// </summary>
		/// <param name="lectureNumber">название лекции</param>
		public int GetCommonProgressValueByLectureName(string lecture)
		{
			CustomTestContext.WriteLine("Получить значение личного прогресс бара для лекции '{0}'.", lecture);
			CommonProgressByLectureName = Driver.SetDynamicValue(How.XPath, COMMON_PROGRESS, lecture);
			var progress = CommonProgressByLectureName.Text.Replace("%", "");
			int result;

			if (!int.TryParse(progress, out result))
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось преобразование значения {0} из общего прогресс бара.", progress));
			}

			return result;

		}
		/// <summary>
		/// Открыть первую лекцию с пустым личным погресс баром.
		/// </summary>
		public EditorPage ClickFirstEmptyPersonalProgressBar()
		{
			CustomTestContext.WriteLine("Открыть первую лекции с пустым личным погресс баром.");
			PersonalProgressList = Driver.GetElementList(By.XPath(PERSONAL_PROGRESS_LIST));
			PersonalProgressList[0].Click();

			return new EditorPage(Driver).LoadPage();
		}

		/// <summary>
		/// Открыть первую лекции с пустым общим погресс баром.
		/// </summary>
		public EditorPage ClickFirstEmptyCommonProgressBar()
		{
			CustomTestContext.WriteLine("Открыть первую лекции с пустым общим погресс баром.");
			CommonProgressList = Driver.GetElementList(By.XPath(COMMON_PROGRESS_LIST));
			CommonProgressList[0].Click();

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

		protected IList<IWebElement> PersonalProgressList { get; set; }
		protected IList<IWebElement> CommonProgressList { get; set; }
		protected IList<IWebElement> LectureNameList { get; set; }
		protected IWebElement Lecture { get; set; }
		protected IWebElement LectureName { get; set; }
		protected IWebElement PersonalProgressByLectureNumber { get; set; }
		protected IWebElement PersonalProgressByLectureName { get; set; }
		protected IWebElement CommonProgressByLectureNumber { get; set; }
		protected IWebElement CommonProgressByLectureName { get; set; }
		#endregion

		#region Описание XPath элементов

		protected const string LECTURES_TABLE = ".//tbody[contains(@data-bind, 'lectures')]";
		protected const string LECTURE = ".//tbody[contains(@data-bind,'lectures')]//tr[*#*]//a";
		protected const string LECTURE_NAME = ".//tbody[contains(@data-bind,'lectures')]//a[text()='*#*']";
		protected const string PERSONAL_PROGRESS_LIST = "//div[@id='progressBar']//following-sibling::div[@class='progBarMargin' and text()='0%']";
		protected const string COMMON_PROGRESS_LIST = "//div[@id='progressBar2']//following-sibling::div[@class='progBarMargin' and text()='0%']";
		protected const string LECTURE_NAME_LIST = "//a[contains(@data-bind,'name')]";
		protected const string PERSONAL_PROGRESS_BY_LECTURE_NUMBER = "//td[contains(@data-bind,'position')][text()='*#*']/ancestor::tr//div[contains(@data-bind,'personalProgressView')]";
		protected const string PERSONAL_PROGRESS_BY_LECTURE_NAME = "//a[text()='*#*']/ancestor::tr//td//div[contains(@data-bind,'personalProgressView')]";
		protected const string COMMON_PROGRESS_BY_LECTURE_NUMBER = "//td[contains(@data-bind,'position')][text()='*#*']/ancestor::tr//div[contains(@data-bind, 'text: progressView')]";
		protected const string COMMON_PROGRESS = "//a[text()='*#*']/ancestor::tr//td//div[contains(@data-bind, 'text: progressView')]";

		#endregion
	}
}
