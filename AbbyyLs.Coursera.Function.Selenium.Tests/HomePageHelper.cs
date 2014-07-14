using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.Coursera.Function.Selenium.Tests
{
	public class HomePageHelper : CommonHelper
	{
		public HomePageHelper(IWebDriver driver, WebDriverWait wait) :
            base (driver, wait)
        {
        }

		/// <summary>
		/// Получить имя пользователя
		/// </summary>
		/// <returns>Имя пользователя</returns>
		public string GetUserName()
		{
			return GetTextElement(By.XPath(USER_NAME_XPATH));
		}

		/// <summary>
		/// Возвращает есть ли событие
		/// </summary>
		/// <param name="targetText">Строка перевода</param>
		/// <param name="evString">Событие</param>
		/// <returns>Событие присутствует</returns>
		public bool GetIsEventPresentByTarget(string targetText, string evString = "")
		{
			string xPath = LAST_EVENTS_TARGET_XPATH + "/span[contains(@data-bind,'target')][contains(text(),'" +
				targetText + "')]/../..//td[4]//div[contains(@class,'" + evString + "')]";

			return GetIsElementExist(By.XPath(xPath));
		}

		/// <summary>
		/// Получить номер строки конретного события
		/// </summary>
		/// <param name="targetText">Текст в Target события</param>
		/// <param name="evString">Тип события</param>
		/// <returns>Номер строки с событием</returns>
		public int GetEventRowNumberByTarget(string targetText, string evString)
		{
			int rowNumber = 0;
			
			// Список событий
			IList<IWebElement> eventsList = GetElementList(By.XPath(LAST_EVENTS_TARGET_XPATH + "/span"));

			for (int i = 0; i < eventsList.Count; ++i)
			{
				if (eventsList[i].Text.Contains(targetText))
				{
					// Событие с тем же target
					if ((eventsList[i].FindElement(By.XPath("..")).FindElement(By.XPath("../td[4]//div[contains(@data-bind,'actionStyle')]"))).
						GetAttribute("class") == evString)
					{
						// и тип события совпадает
						rowNumber = (i + 1);
						break;
					}
				}
			}
			// Возвращаем номер строки с событием
			return rowNumber;
		}

		/// <summary>
		/// Получить автора события по номеру строки
		/// </summary>
		/// <param name="rowNumber">Номер строки</param>
		/// <returns>Имя автора</returns>
		public string GetEventAuthorByRowNumber(int rowNumber)
		{
			string xPath = LAST_EVENTS_XPATH + "//tr[" + (rowNumber + 1).ToString() + "]//td[4]//a[contains(@data-bind,'userName')]";
		
			return GetTextElement(By.XPath(xPath)).Trim();
		}

		/// <summary>
		/// Получить перевод в списке событий по номеру строки
		/// </summary>
		/// <param name="rowNumber">Номер строки с событием</param>
		/// <returns>Перевод</returns>
		public string GetEventTargetByRowNumber(int rowNumber)
		{
			string xPath = LAST_EVENTS_XPATH + "//tr[" + (rowNumber + 1).ToString() + "]//td[3]/span";
			
			return GetTextElement(By.XPath(xPath)).Trim();
		}

		/// <summary>
		/// Получить рейтинг в списке событий по номеру строки
		/// </summary>
		/// <param name="rowNumber">Номер строки с событием</param>
		/// <returns>Рейтинг</returns>
		public int GetEventRatingByRowNumber(int rowNumber)
		{
			string xPath = LAST_EVENTS_XPATH + "//tr[" + (rowNumber + 1).ToString() + "]//td[5]//span[contains(@data-bind,'rating')]";

			return int.Parse(GetTextElement(By.XPath(xPath)).Trim());
		}

		/// <summary>
		/// Получить activity по номеру строки
		/// </summary>
		/// <param name="rowNumber">Номер строки</param>
		/// <returns>Наименование activity</returns>
		public string GetEventActivityByRowNumber(int rowNumber)
		{
			string xPath = LAST_EVENTS_XPATH + "//tr[" + (rowNumber + 1).ToString() + "]//td[4]//div[contains(@data-bind,'actionStyle')]";

			return GetElementAttribute(By.XPath(xPath), "class");
		}

		/// <summary>
		/// Получить список переводов
		/// </summary>
		/// <returns>Список переводов</returns>
		public List<string> GetEventTargetList()
		{
			string xPath = LAST_EVENTS_XPATH + "//tr//td[3]//a[contains(@data-bind,'target')]";

			return GetTextListElement(By.XPath(xPath));
		}

		/// <summary>
		/// Проголосовать в списке событий
		/// </summary>
		/// <param name="rowNumber">номер строки с событием</param>
		/// <param name="voteUp">тип голоса (true: ЗА, false: Против)</param>
		/// <returns>можно ли проголосовать (возвращает false, если нельзя проголосовать - кнопка заблокирована)</returns>
		public bool GetIsCanVoteEventListByRowNumber(int rowNumber, bool voteUp)
		{
			string voteClass = voteUp ? "like" : "dislike";
			string xPath = LAST_EVENTS_XPATH + "//tr[" + (rowNumber + 1).ToString() + "]//td[5]";
			
			// Проверить, что можно проголосовать
			bool canVote = GetIsElementExist(By.XPath(xPath + "//div[@class='" + voteClass + "']"));
			if (canVote)
			{
				// Проголосовать
				ClickElement(By.XPath(xPath + "//div[@class='" + voteClass + "']"));
				// Дождаться, пока голос будет учтен
				WaitUntilDisplayElement(By.XPath(xPath + "//div[@class='" + voteClass + "d']"));
			}
			return canVote;
		}

		/// <summary>
		/// Проголосовать в списке событий за событие, за которое уже голосовали
		/// </summary>
		/// <param name="rowNumber">номер строки с событием</param>
		/// <param name="voteUp">тип голоса (true: За, false: против)</param>
		public void VoteVotedEventListByRowNumber(int rowNumber, bool voteUp)
		{
			// т.к. за событие уже голосовали, то class будет как уже проголосованный
			string voteClass = voteUp ? "liked" : "disliked";
			string xPath = LAST_EVENTS_XPATH + "//tr[" + (rowNumber + 1).ToString() + "]//td[5]//div[@class='" + voteClass + "']";

			// Проголосовать
			ClickElement(By.XPath(xPath));
		}

		/// <summary>
		/// Открыть страницу профиля пользователя
		/// </summary>
		public void OpenProfile()
		{
			ClickElement(By.ClassName(PROFILE_LINK_CLASSNAME));
		}

		/// <summary>
		/// Получить общее количество переведенных слов
		/// </summary>
		/// <returns>Общее количество переведенных слов</returns>
		public int GetTotalWords()
		{
			return int.Parse(GetTextElement(By.XPath(TOTAL_WORDS_XPATH)));
		}

		/// <summary>
		/// Получить общее количество переведенных страниц
		/// </summary>
		/// <returns>Общее количество переведенных страниц</returns>
		public int GetTotalPages()
		{
			return int.Parse(GetTextElement(By.XPath(TOTAL_PAGES_XPATH)));
		}



		protected const string USER_NAME_XPATH = ".//a[contains(@class,'user-link')]";

		protected const string TOTAL_XPATH = ".//div[@class='total-pages']";
		protected const string TOTAL_PAGES_XPATH = TOTAL_XPATH + "//span[contains(@data-bind,'totalPages')]";
		protected const string TOTAL_WORDS_XPATH = TOTAL_XPATH + "//span[contains(@data-bind,'totalWords')]";

		protected const string LAST_EVENTS_XPATH = ".//table[@class='last-events']";
		protected const string LAST_EVENTS_TARGET_XPATH = LAST_EVENTS_XPATH + "//tr//td[3]";

		protected const string PROFILE_LINK_CLASSNAME = "user-link";
	}
}
